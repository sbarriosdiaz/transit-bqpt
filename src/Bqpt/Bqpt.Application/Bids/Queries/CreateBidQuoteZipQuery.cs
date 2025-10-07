using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Common;
using Bqpt.Domain;
using MediatR;
using Microsoft.Web.Mvc.Controls;

namespace Bqpt.Application
{
    public class CreateBidQuoteZipQuery : IRequest<List<string>>
    {
        public BidQuoteResponseDto BidQuoteResponse { get; }
        public List<string> Headers { get; }

        public CreateBidQuoteZipQuery(BidQuoteResponseDto bidQuoteResponse)
        {
            BidQuoteResponse = bidQuoteResponse;
           
        }
    }

    public class CreateBidQuoteZipQueryHandler : IRequestHandler<CreateBidQuoteZipQuery, List<string>>
    {
        private readonly string _exportPath;
        private readonly IMediator _mediator;

        public CreateBidQuoteZipQueryHandler(IMediator mediator)
        {
            _exportPath =ConfigurationManager.AppSettings["BidQuoteExportPath"];
            _mediator = mediator;
        }

        public async Task<List<string>> Handle(CreateBidQuoteZipQuery request, CancellationToken cancellationToken)
        {
            var generatedFiles = new List<string>();
            var recipient = ConfigurationManager.AppSettings["BUG::ContactEmail"];

            try
            {
                var vendorGroups = request.BidQuoteResponse.BidQuote.Parts
                    .Where(p => p.BidsSelectedCount > 0)
                    .SelectMany(p => p.VendorBids.Where(b => (bool)b.IsSelected),
                        (part, bid) => new { Part = part, Bid = bid })
                    .GroupBy(x => x.Bid.Vendor.AssetWorksVendorNumber)
                    .ToList();

                if (!vendorGroups.Any())
                    return generatedFiles;
               
                // Ensure export path exists (sanitized)
                var safeExportPath = _exportPath;

                // Ensure export directory exists
                Directory.CreateDirectory(_exportPath);


                Parallel.ForEach(vendorGroups, vendorGroup =>
                {
                    var xmlContent = GenerateVendorXml(request.BidQuoteResponse, vendorGroup.Key);
                    if (string.IsNullOrWhiteSpace(xmlContent)) return;

                    var rawVendorName = vendorGroup.First().Bid.Vendor.AssetWorksVendorName ?? "UnknownVendor";
                    var vendorName = SanitizeFileName(rawVendorName);
                    var xmlFileName = $"BidQuote_{request.BidQuoteResponse.BidQuote.AssetWorksBidQuoteId}_{vendorName}.xml";
                    var filePath = Path.Combine(safeExportPath, xmlFileName);

                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    using (var writer = new StreamWriter(fs, Encoding.UTF8))
                    {
                        writer.Write(xmlContent);
                    }
                });

                // Send ONE notification email with list of filenames
                if (generatedFiles.Any())
                {
                    var fileList = string.Join(Environment.NewLine, generatedFiles.Select(Path.GetFileName));
                    var message = $"XML File(s) to generate PO have been created successfully:{Environment.NewLine}{fileList}";

                    await _mediator.Send(new ContactSubmitCommand(
                        request.BidQuoteResponse.BidQuote.AssetWorksBidQuoteId,
                        message,
                        recipient,
                        null // no attachments, just list in email body
                    ));
                }

                return generatedFiles;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error generating BidQuote XML: {ex.Message}";

                try
                {
                    var fileList = generatedFiles.Any()
                        ? string.Join(Environment.NewLine, generatedFiles.Select(Path.GetFileName))
                        : "No files generated.";

                    var message = $"{errorMessage}{Environment.NewLine}Generated before failure:{Environment.NewLine}{fileList}";

                    await _mediator.Send(new ContactSubmitCommand(
                        request.BidQuoteResponse.BidQuote.AssetWorksBidQuoteId,
                        message,
                        recipient,
                        null
                    ));
                }
                catch
                {
                    // avoid recursive mediator failures
                }

                throw; // rethrow so controller can handle
            }
        }



        private static string SanitizeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(name.Length);
            foreach (var ch in name)
                sb.Append(invalid.Contains(ch) ? '_' : ch);
            return sb.ToString();
        }

        private static string GenerateVendorXml(BidQuoteResponseDto query, string vendorId)
        {

            var headers = new List<string> { "2325", "8853:2", "8853:7", "8903:2", "[GROUPROW]" };

            var winnerParts = query.BidQuote.Parts
                .Where(part => part.BidsSelectedCount > 0)
                .SelectMany(part => part.VendorBids.Where(bid => (bool)bid.IsSelected), (part, bid) => new { Part = part, Bid = bid })
                .GroupBy(x => x.Bid.Vendor.AssetWorksVendorNumber)
                .ToList();

            var partGroup = winnerParts.FirstOrDefault(g => g.Key == vendorId);
            if (partGroup == null || !partGroup.Any())
                return null;

            var sb = new StringBuilder();

            // === Begin Workbook ===
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:o=\"urn:schemas-microsoft-com:office:office\"");
            sb.AppendLine(" xmlns:x=\"urn:schemas-microsoft-com:office:excel\"");
            sb.AppendLine(" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\"");
            sb.AppendLine(" xmlns:html=\"http://www.w3.org/TR/REC-html40\">");

            // === Styles ===
            sb.AppendLine(" <Styles>");
            sb.AppendLine("  <Style ss:ID=\"Default\" ss:Name=\"Normal\">");
            sb.AppendLine("   <Alignment ss:Vertical=\"Bottom\"/>");
            sb.AppendLine("   <Font ss:FontName=\"Calibri\" x:Family=\"Swiss\" ss:Size=\"11\" ss:Color=\"#000000\"/>");
            sb.AppendLine("  </Style>");
            sb.AppendLine(" </Styles>");

            // === Sheet1 ===
            sb.AppendLine(" <Worksheet ss:Name=\"Sheet1\">");
            sb.AppendLine("  <Table>");

            // Header row
            sb.AppendLine("   <Row>");
            foreach (var header in headers)
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{header}</Data></Cell>");
            sb.AppendLine("   </Row>");

            // Data row
            sb.AppendLine("   <Row>");
            foreach (var header in headers)
            {
                string value = string.Empty;

                if (header == "2325")
                    value = "[i]";
                else if (header == "8853:2")
                    value = partGroup.FirstOrDefault()?.Part?.AssetWorksPartLocationCode ?? "";
                else if (header == "8903:2")
                    value = query.BidQuote.AssetWorksBidQuoteId ?? "";
                else if (header == "8853:7")
                    value = partGroup.FirstOrDefault()?.Bid?.Vendor.AssetWorksVendorNumber ?? "";
                else if (header == "[GROUPROW]")
                    value = "[8874:1;PARTS;1-2:1-2]";

                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{value}</Data></Cell>");
            }
            sb.AppendLine("   </Row>");

            sb.AppendLine("  </Table>");
            sb.AppendLine(" </Worksheet>");

            // === PARTS Sheet ===
            sb.AppendLine(" <Worksheet ss:Name=\"PARTS\">");
            sb.AppendLine("  <Table>");

            var partHeaders = new List<string> { "2325", "[KEY]", "[KEY]", "8874:6", "8874:7", "8874:11", "8874:12" };

            // PARTS header row
            sb.AppendLine("   <Row>");
            foreach (var h in partHeaders)
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{h}</Data></Cell>");
            sb.AppendLine("   </Row>");

            // PARTS data rows
            foreach (var item in partGroup)
            {
                if (item.Part == null || item.Bid == null || item.Bid.Vendor == null)
                    continue;

                sb.AppendLine("   <Row>");
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">[i]</Data></Cell>");
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{item.Part.AssetWorksPartLocationCode}</Data></Cell>");
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{item.Bid.Vendor.AssetWorksVendorNumber}</Data></Cell>");
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{item.Part.AssetWorksPartNumber}</Data></Cell>");
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{item.Part.AssetWorksPartSuffix}</Data></Cell>");
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{item.Part.AssetWorksPartQuantityRequested}</Data></Cell>");
                sb.AppendLine($"    <Cell><Data ss:Type=\"String\">{item.Bid.UnitPrice}</Data></Cell>");
                sb.AppendLine("   </Row>");
            }

            sb.AppendLine("  </Table>");
            sb.AppendLine(" </Worksheet>");

            // === Close Workbook ===
            sb.AppendLine("</Workbook>");

            return sb.ToString();
        }
    }
}
