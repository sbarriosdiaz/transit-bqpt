using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bqpt.Application;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using Bqpt.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace PO_Process
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            {
                // -------------------------
                // 1️⃣ Setup Dependency Injection
                // -------------------------
                var services = new ServiceCollection();

                // Register AppDbContext as scoped
                services.AddScoped<AppDbContext>();

                // Register your DbService
                services.AddScoped<IDbService, AppDbService>();

                // Register MediatR and scan the current assembly for handlers
                services.AddMediatR(typeof(Program).Assembly);

                // Register your SMTP service
                services.AddSingleton<SmtpServices>();

                var provider = services.BuildServiceProvider();

                var mediator = provider.GetRequiredService<IMediator>();
                var smtpServices = provider.GetRequiredService<SmtpServices>();

                try
                {
                    // -------------------------
                    // 2️⃣ Handle Bid Quotes
                    // -------------------------
                    using (var dbContext = provider.GetRequiredService<AppDbContext>())
                    {
                        var bidQuoteList = await dbContext.Set<BidQuote>()
                            .AsNoTracking()
                            .Where(x => x.DisplayOrder == 1)
                            .ToListAsync();                       

                        foreach (var bidQuote in bidQuoteList)
                        {
                            // Use mediator to send query
                            var result = await mediator.Send(new BidQuoteQuery(bidQuote.DomainKey.ToString()));
                            await mediator.Send(new CreateBidQuoteZipQuery(result));                            
                        }
                    }

                    // -------------------------
                    // 3️⃣ File Processing
                    // -------------------------
                    var pathFolder = ConfigurationManager.AppSettings["BidQuoteExportPath"];
                    var logs = ConfigurationManager.AppSettings["Logs"];
                    var error = ConfigurationManager.AppSettings["Error"];
                    var archived = ConfigurationManager.AppSettings["Archived"];
                    var faDataLoaderPath = ConfigurationManager.AppSettings["FADataLoaderPath"];

                    if (!File.Exists(faDataLoaderPath))
                    {
                        Console.WriteLine($"FADataLoader executable not found: {faDataLoaderPath}");
                        return;
                    }

                    if (!Directory.Exists(pathFolder))
                    {
                        Console.WriteLine("Input folder not found.");
                        return;
                    }

                    EnsureDirectoryExists(logs);
                    EnsureDirectoryExists(error);
                    EnsureDirectoryExists(archived);

                    var pOrders = Directory.GetFiles(pathFolder);
                    var processedXmlFiles = new List<string>();

                    foreach (var inputPath in pOrders)
                    {
                        var pOrder = Path.GetFileName(inputPath);

                        try
                        {
                            var arguments = $"-n \"1\" -i \"{inputPath}\" -l \"{logs}\" -ds \"http://localhost:9000\" -u \"Dataloader\" -p \"Dataloader123$\"";

                            var process = new Process
                            {
                                StartInfo = new ProcessStartInfo
                                {
                                    FileName = faDataLoaderPath,
                                    Arguments = arguments,
                                    WorkingDirectory = Path.GetDirectoryName(faDataLoaderPath),
                                    UseShellExecute = true,
                                    CreateNoWindow = false
                                }
                            };

                            process.Start();
                            await Task.Run(() => process.WaitForExit());

                            var logTime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            var logFileName = $"log_{Path.GetFileNameWithoutExtension(pOrder)}_{logTime}.txt";
                            var logDirectory = process.ExitCode == 0 ? logs : error;
                            var logFilePath = Path.Combine(logDirectory, logFileName);

                            File.WriteAllText(logFilePath,
                                $"Command: {faDataLoaderPath} {arguments}\n\nExit Code: {process.ExitCode}\n\nNote: Output was not captured because UseShellExecute=true.",
                                Encoding.UTF8);

                            using (var dbContext = provider.GetRequiredService<AppDbContext>())
                            {
                                var bidQuoteList = await dbContext.Set<BidQuote>()
                                    .AsNoTracking()
                                    .Where(x => x.DisplayOrder == 1)
                                    .ToListAsync();

                                

                                foreach (var bidQuote in bidQuoteList)
                                {
                                    // Use mediator to send query
                                    await mediator.Send(new FlagUpdatePOCommand(bidQuote.DomainKey));
                                }
                            }

                            Console.WriteLine($"Processed {pOrder}, log saved to {logFilePath}");
                        }
                        catch (Exception fileEx)
                        {
                            Console.WriteLine($"Error processing file {pOrder}: {fileEx.Message}");

                            try
                            {
                                var recipients = new List<MailAddress>
                            {
                                new MailAddress(ConfigurationManager.AppSettings["BUG::ContactEmail"])
                            };

                                var email = new BaseEmailDto(recipients, MailPriority.High)
                                {
                                    Subject = $"*** PO FILE FAILED: {pOrder} ***",
                                    IsHtml = true,
                                    Body = $@"
                                    <h2 style='color:red;'>PO File Processing Failed</h2>
                                    <p><b>File:</b> {pOrder}</p>
                                    <p><b>Time:</b> {DateTime.Now}</p>
                                    <p><b>Error Message:</b> {fileEx.Message}</p>
                                    <p><b>Stack Trace:</b><br/><pre>{fileEx.StackTrace}</pre></p>"
                                };

                                smtpServices.SendEmail(email, true);

                                Console.WriteLine($"Error email sent for file {pOrder}");
                            }
                            catch (Exception emailEx)
                            {
                                Console.WriteLine($"Failed to send error email for file {pOrder}: {emailEx.Message}");
                            }
                        }
                    }

                    // -------------------------
                    // 4️⃣ Archive Files
                    // -------------------------
                    var processedFiles = Directory.GetFiles(pathFolder);
                    var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                    foreach (var file in processedFiles)
                    {
                        var fileName = Path.GetFileName(file);
                        var destPath = Path.Combine(archived, fileName);

                        if (File.Exists(destPath))
                        {
                            var newFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{timestamp}{Path.GetExtension(fileName)}";
                            destPath = Path.Combine(archived, newFileName);
                        }

                        File.Move(file, destPath);

                        if (Path.GetExtension(fileName).Equals(".xml", StringComparison.OrdinalIgnoreCase))
                            processedXmlFiles.Add(fileName);
                    }

                    // -------------------------
                    // 5️⃣ Send Success Email Notification
                    // -------------------------
                    if (processedXmlFiles.Any())
                        SendEmailNotification(smtpServices, processedXmlFiles);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");

                    try
                    {
                        var recipients = new List<MailAddress>
                    {
                        new MailAddress(ConfigurationManager.AppSettings["BUG::ContactEmail"])
                    };

                        var email = new BaseEmailDto(recipients, MailPriority.High)
                        {
                            Subject = "*** PO PROCESS FAILED ***",
                            IsHtml = true,
                            Body = $@"
                            <h2 style='color:red;'>PO Process Failed</h2>
                            <p><b>Time:</b> {DateTime.Now}</p>
                            <p><b>Error Message:</b> {ex.Message}</p>
                            <p><b>Stack Trace:</b><br/><pre>{ex.StackTrace}</pre></p>"
                        };

                        smtpServices.SendEmail(email, true);

                        Console.WriteLine("Global error email sent successfully.");
                    }
                    catch (Exception emailEx)
                    {
                        Console.WriteLine($"Failed to send global error email: {emailEx.Message}");
                    }
                }
            }
        }

        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private static void SendEmailNotification(SmtpServices smtpServices, List<string> fileNames)
        {
            try
            {
                var recipients = new List<MailAddress> { new MailAddress(ConfigurationManager.AppSettings["BUG::ContactEmail"]) };
                var fileList = string.Join("<br/>", fileNames.Select(f => $"<b>{f}</b>"));
                var email = new BaseEmailDto(recipients, MailPriority.High)
                {
                    Subject = "*** Purchase Order Notification ***",
                    IsHtml = true,
                    Body = $"<h2>Dear Customer,</h2><br/>The following Purchase Orders are now open and started at {DateTime.Now}:<br/><br/>{fileList}<br/>"
                };

                smtpServices.SendEmail(email, true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send success email: {ex.Message}");
            }
        }

    }
}

    // -------------------------
    // 6️⃣ DbService Implementation
    // -------------------------
    public class AppDbService : IDbService
    {
        private readonly AppDbContext _context;

        public AppDbService(AppDbContext context)
        {
            _context = context;
        }

        public DbSet<T> Set<T>() where T : class => _context.Set<T>();

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
            => _context.SaveChangesAsync(cancellationToken);
    }

    // -------------------------
    // 7️⃣ BidQuote Query & Handler (Top-level)
    // -------------------------
    public class BidQuoteQuery : IRequest<BidQuoteResponseDto>
    {
        public BidQuoteQuery(string domainKey) => DomainKey = domainKey;
        public string DomainKey { get; }
    }

    public class BidQuoteQueryHandler : IRequestHandler<BidQuoteQuery, BidQuoteResponseDto>
    {
        private readonly IDbService _dbService;

        public BidQuoteQueryHandler(IDbService dbService)
        {
            _dbService = dbService;
        }

        public async Task<BidQuoteResponseDto> Handle(BidQuoteQuery request, CancellationToken cancellationToken)
        {
            var systemVendors = await _dbService.Set<Vendor>()
        .AsNoTracking()
        .Include(v => v.Contacts)
        .Where(x => x.IsActive)
         .Select(v => new VendorDto
         {
             DomainKey = v.DomainKey,
             Id = v.Id,
             AssetWorksVendorNumber = v.AssetWorksVendorNumber,
             AssetWorksVendorName = v.AssetWorksVendorName,
             AssetWorksVendorAccountingSystemNumber = v.AssetWorksVendorAccountingSystemNumber,
             AssetWorksVendorContactName = v.AssetWorksVendorContactName,
             AssetWorksVendorAddress1 = v.AssetWorksVendorAddress1,
             AssetWorksVendorAddress2 = v.AssetWorksVendorAddress2,
             AssetWorksVendorAddress3 = v.AssetWorksVendorAddress3,
             AssetWorksVendorAddress4 = v.AssetWorksVendorAddress4,
             AssetWorksVendorEmail = v.AssetWorksVendorEmail,
             AssetWorksVendorPhone = v.AssetWorksVendorPhone,
             AssetWorksVendorFax = v.AssetWorksVendorFax,
             CommodityCodes = v.CommodityCodes,
             Contacts = v.Contacts
            .OrderBy(c => c.AssetWorksContactName)
            .Where(c => c.IsActive && c.VendorId == v.Id)
            .Select(c => new ContactDto
            {
                AssetWorksVendorName = v.AssetWorksVendorName,
                AssetWorksVendorNumber = v.AssetWorksVendorNumber,
                AssetWorksContactName = c.AssetWorksContactName,
                AssetWorksContactEmail = c.AssetWorksContactEmail,
                AssetWorksContactPhone = c.AssetWorksContactPhone,
                AssetWorksContactAddress1 = c.AssetWorksContactAddress1,
                AssetWorksContactAddress2 = c.AssetWorksContactAddress2,
                AssetWorksContactAddress3 = c.AssetWorksContactAddress3,
                AssetWorksContactAddress4 = c.AssetWorksContactAddress4
            })
            .ToList(),
         }).ToListAsync(cancellationToken);

            var bidQuote = await _dbService.Set<BidQuote>()
                .AsNoTracking()
                .Include(x => x.Parts)
                .Include(q => q.Parts.Select(v => v.Bids))
                .Include(q => q.VendorBidQuoteAttachments.Select(a => a.Attachment))
                .Include(q => q.Parts.Select(v => v.Bids.Select(b => b.Vendor.Contacts)))
                .Include(q => q.Parts.Select(v => v.VendorInParts.Select(b => b.Vendor.Contacts)))
                .Where(x => x.DomainKey.Equals(request.DomainKey) && x.IsActive)
                .Select(bq => new BidQuoteDto
                {
                    Id = bq.Id,
                    AssetWorksBidQuoteId = bq.AssetWorksBidQuoteId,
                    DomainKey = bq.DomainKey,
                    Status = (bq.BidScheduledEndTime < DateTime.Now && bq.BidEndTime == null) ? (nameof(BidQuoteStatus.Expired)) : bq.Status,
                    BidStartTime = bq.BidStartTime,
                    BidEndTime = bq.BidEndTime,
                    BidScheduledStartTime = bq.BidScheduledStartTime,
                    BidScheduledEndTime = bq.BidScheduledEndTime,
                    BidImportedTime = bq.BidImportedTime,
                    BidClosedTime = bq.BidClosedTime,
                    DisplayOrder = bq.DisplayOrder,
                    AssetWorksDateInserted = bq.AssetWorksDateInserted,
                    AssetWorksDateApproval = bq.AssetWorksDateApproval,
                    AssetWorksDateRequired = bq.AssetWorksDateRequired,
                    AssetWorksDateRequested = bq.AssetWorksDateRequested,
                    Attachments = bq.VendorBidQuoteAttachments.Select(a => new VendorBidQuoteAttachmentDto
                    {
                        Id = a.Id,
                        DomainKey = a.DomainKey,
                        AttachmentId = a.Attachment.AttachmentId,
                        BlobUrl = a.Attachment.BlobUrl,
                        FileName = a.Attachment.FileName,
                        FileExtension = a.Attachment.FileExtension,
                        FileFriendlyName = a.Attachment.AttachmentFriendlyName,
                        Status = a.Attachment.Status
                    }),
                    Notes = bq.Notes.Select(n => new BidQuoteNoteDto
                    {
                        Id = n.Id,
                        Note = n.Note,
                    }).ToList(),
                    Parts = bq.Parts
                        .Where(p => p.IsActive)
                        .OrderBy(p => p.AssetWorksMinPartLineNumberInt)// Ensuring parts are ordered
                        .Select(p => new BidQuotePartDto
                        {
                            Id = p.Id,
                            DomainKey = p.DomainKey,
                            BidQuoteStatus = bq.Status,
                            AssetWorksPartNumber = p.AssetWorksPartNumber,
                            AssetWorksPartCommodityCode = p.AssetWorksPartCommodityCode,
                            AssetWorksPartCategory = p.AssetWorksPartCategory,
                            AssetWorksPartDescription = p.AssetWorksPartDescription,
                            AssetWorksPartManufacturerPartNumber = p.AssetWorksPartManufacturerPartNumber,
                            AssetWorksPartSuffix = p.AssetWorksPartSuffix,
                            AssetWorksPartLineNumber = p.AssetWorksPartLineNumber,
                            AssetWorksPartLocationCode = p.AssetWorksPartLocationCode,
                            AssetWorksPartDateInserted = p.AssetWorksPartDateInserted,
                            AssetWorksPartDateRequired = p.AssetWorksPartDateRequired,
                            AssetWorksPartQuantityOnHand = p.AssetWorksPartQuantityOnHand,
                            AssetWorksPartQuantityOnOrder = p.AssetWorksPartQuantityOnOrder,
                            AssetWorksPartQuantityCommited = p.AssetWorksPartQuantityCommited,
                            AssetWorksPartQuantityRequested = p.AssetWorksPartQuantityRequested,
                            AssetWorksUnitPrice = p.AssetWorksUnitPrice,
                            RequestedQuantity = p.RequestedQuantity,
                            PurchaseOrderQuantity = p.PurchaseOrderQuantity,
                            VendorInParts = p.VendorInParts
                                .Where(vp => vp.IsActive)
                                .OrderBy(vp => vp.Vendor.AssetWorksVendorName)
                                .Select(vp => new VendorInPartDto
                                {
                                    Id = vp.Id,
                                    DomainKey = vp.DomainKey,
                                    NotificationSent = vp.NotificationSent,
                                    VendorId = vp.VendorId,
                                    BidQuotePartId = p.Id,
                                    Vendor = new VendorDto
                                    {
                                        DomainKey = vp.Vendor.DomainKey,
                                        Id = vp.Vendor.Id,
                                        AssetWorksVendorNumber = vp.Vendor.AssetWorksVendorNumber,
                                        AssetWorksVendorName = vp.Vendor.AssetWorksVendorName,
                                        AssetWorksVendorAccountingSystemNumber = vp.Vendor.AssetWorksVendorAccountingSystemNumber,
                                        AssetWorksVendorContactName = vp.Vendor.AssetWorksVendorContactName,
                                        AssetWorksVendorEmail = vp.Vendor.AssetWorksVendorEmail,
                                        AssetWorksVendorPhone = vp.Vendor.AssetWorksVendorPhone
                                    }
                                }).ToList(),
                            VendorBids = p.Bids
                                .Where(vb => vb.IsActive)
                                .OrderBy(vb => vb.Vendor.AssetWorksVendorName)
                                .Select(vb => new VendorPartBidDto
                                {
                                    Id = vb.Id,
                                    DomainKey = vb.DomainKey,
                                    VendorId = vb.VendorId,
                                    BidQuoteStatus = bq.Status,
                                    BidQuotePartId = p.Id,
                                    QuantityAvailable = vb.QuantityAvailable ?? 0,
                                    ItemsPerUnit = vb.ItemsPerUnit ?? 0,
                                    UnitPrice = vb.UnitPrice ?? 0,
                                    EstimateDeliveryDate = vb.EstimateDeliveryDate,
                                    Core = vb.Core,
                                    Comment = vb.Comment,
                                    IsSelected = vb.IsSelected,
                                    Vendor = new VendorDto
                                    {
                                        DomainKey = vb.Vendor.DomainKey,
                                        Id = vb.Vendor.Id,
                                        AssetWorksVendorNumber = vb.Vendor.AssetWorksVendorNumber,
                                        AssetWorksVendorName = vb.Vendor.AssetWorksVendorName,
                                        AssetWorksVendorAccountingSystemNumber = vb.Vendor.AssetWorksVendorAccountingSystemNumber,
                                        AssetWorksVendorContactName = vb.Vendor.AssetWorksVendorContactName,
                                        AssetWorksVendorEmail = vb.Vendor.AssetWorksVendorEmail,
                                        AssetWorksVendorPhone = vb.Vendor.AssetWorksVendorPhone
                                    },
                                    Attachment = bq.VendorBidQuoteAttachments
                                        .Where(v => v.VendorId == vb.VendorId)
                                        .OrderByDescending(a => a.Id)
                                        .Select(a => new VendorBidQuoteAttachmentDto
                                        {
                                            Id = a.Id,
                                            DomainKey = a.DomainKey,
                                            AttachmentId = a.Attachment.AttachmentId,
                                            BlobUrl = a.Attachment.BlobUrl,
                                            FileName = a.Attachment.FileName,
                                            FileExtension = a.Attachment.FileExtension,
                                            FileFriendlyName = a.Attachment.AttachmentFriendlyName,
                                            Status = a.Attachment.Status
                                        }).FirstOrDefault()
                                }).ToList()
                        }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);

            return new BidQuoteResponseDto { BidQuote = bidQuote, SystemVendors = systemVendors };
        }
    }
    public class CreateBidQuoteZipQueryHandler : IRequestHandler<CreateBidQuoteZipQuery, List<string>>
    {
        private readonly string _exportPath;
        private readonly IMediator _mediator;

        public CreateBidQuoteZipQueryHandler(IMediator mediator)
        {
            _exportPath = ConfigurationManager.AppSettings["BidQuoteExportPath"];
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
    public class FlagUpdatePOCommand : IRequest<TransactionResult<string>>
    {
        public string DomainKey { get; set; }

        public FlagUpdatePOCommand(string domainKey)
        {
            DomainKey = domainKey;
        }

        public class Handler : IRequestHandler<FlagUpdatePOCommand, TransactionResult<string>>
        {
            private readonly IDbService _dbService;

            public Handler(IDbService dbService) => _dbService = dbService;

            public async Task<TransactionResult<string>> Handle(FlagUpdatePOCommand request, CancellationToken cancellationToken)
            {
                var bidQuote = await _dbService.Set<BidQuote>().FirstOrDefaultAsync(vb => vb.DomainKey.Equals(request.DomainKey), cancellationToken);
                if (bidQuote != null && bidQuote.DisplayOrder == 0)
                {
                    bidQuote.DisplayOrder = 1;
                }
                else
                {
                    bidQuote.DisplayOrder = 2;
                }

                await _dbService.SaveChangesAsync(cancellationToken);

                return new TransactionResult<string>(bidQuote.DomainKey, AppConstants.TransactionSuccess);
            }
        }
    }



