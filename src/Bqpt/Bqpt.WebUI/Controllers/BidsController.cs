////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using BC;
using BC.Identity.Kernel;
using Bqpt.Application;
using Bqpt.Common;
using Bqpt.Domain;
using Bqpt.Infrastructure;
using MediatR;
using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace Bqpt.WebUI.Controllers
{
    public class BidsController : KernelControllerBase
    {
        private readonly IFilesConnectedServices _filesConnectedServices;

        public static class ActionName
        { public const string Dashboard = "Dashboard"; }

        public static class ControllerName
        { public const string Home = "Home"; }

        public BidsController(IMediator mediator, IFilesConnectedServices filesConnectedServices)
        {
            MediatR = mediator;
            _filesConnectedServices = filesConnectedServices;
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index() => View(await MediatR.Send(new BidQuoteAllDashboardQuery()));

        /// <summary>
        /// GET: AssetWork Bid Quote Parts
        /// </summary>
        /// <param name="assetWorksBidQuoteId"></param>
        /// <returns></returns>
        public async Task<ActionResult> AssetWork(string id) => View(await MediatR.Send(new AssetWorksBidQuoteQuery(id)));

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> GetVendorNoEmails(string id)
        {
            var response = await MediatR.Send(new AssetWorksBidQuoteQuery(id));

            return PartialView("_AssetWorkNoEmailPartial", response.VendorsWithNoEmail.Distinct());
        }

        /// <summary>
        /// POST: Create BidQuote
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult InitBidImport(string id) => PartialView("_BidQuoteImportPartial", new BidQuoteViewModel { AssetWorksBidQuoteId = id });

        /// <summary>
        /// POST:  Bid Quote Parts / post will process transaction/import and 308 redirect to import details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportBidQuote(BidQuoteViewModel vm)
        {
            if (!ModelState.IsValid) return RedirectToAction(ActionName.Dashboard, ControllerName.Home);

            var transaction = await MediatR.Send(new BidQuoteImportCommand(vm));

            return RedirectToAction(nameof(BidQuotes), new { id = transaction.Result });
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult InitBidDelete(string id) => PartialView("_BidQuoteDeletePartial", new BidQuoteViewModel { AssetWorksBidQuoteId = id });

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBidQuote(BidQuoteViewModel vm)
        {
            if (!ModelState.IsValid) return RedirectToAction(ActionName.Dashboard, ControllerName.Home);

            await MediatR.Send(new BidQuoteDeleteCommand(vm));

            return RedirectToAction("Dashboard", "Home");
        }

        public async Task<PartialViewResult> InitBidStart(string id)
        {
            var response = await MediatR.Send(new BidQuoteQuery(id));

            return PartialView("_BidQuoteStartPartial", new BidQuoteViewModel { AssetWorksBidQuoteId = response.BidQuote.AssetWorksBidQuoteId, BidScheduledEndTime = DateTime.Now.AddDays(7) });
        }

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> StartBidQuote(BidQuoteViewModel vm)
        {
            if (!ModelState.IsValid) return RedirectToAction(ActionName.Dashboard, ControllerName.Home);

            var transaction = await MediatR.Send(new BidQuoteStartCommand(vm));

            return !string.IsNullOrEmpty(transaction.Result)
                    ? RedirectToAction(nameof(BidQuotes), new { id = transaction.Result })
                    : RedirectToAction(ActionName.Dashboard, ControllerName.Home);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> InitBidUpdate(string id)
        {
            var response = await MediatR.Send(new BidQuoteQuery(id));

            return PartialView("_BidQuoteUpdatePartial", new BidQuoteViewModel
            {
                AssetWorksBidQuoteId = response.BidQuote.AssetWorksBidQuoteId,
                BidScheduledEndTime = response.BidQuote.BidScheduledEndTime,
                BidStartTime = response.BidQuote.BidStartTime,
                BidScheduledStartTime = response.BidQuote.BidScheduledStartTime
            });
        }

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateBidQuote(BidQuoteViewModel vm)
        {
            if (!ModelState.IsValid) return RedirectToAction(ActionName.Dashboard, ControllerName.Home);

            var transaction = await MediatR.Send(new BidQuoteUpdateCommand(vm));

            return !string.IsNullOrEmpty(transaction.Result)
                                ? RedirectToAction(nameof(BidQuotes), new { id = transaction.Result })
                                : RedirectToAction(ActionName.Dashboard, ControllerName.Home);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult InitBidEnd(string id) => PartialView("_BidQuoteEndPartial", new BidQuoteViewModel { AssetWorksBidQuoteId = id });

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> EndBidQuote(BidQuoteViewModel vm)
        {
            if (!ModelState.IsValid) RedirectToAction(ActionName.Dashboard, ControllerName.Home);

            var transaction = await MediatR.Send(new BidQuoteEndCommand(vm));

            return !string.IsNullOrEmpty(transaction.Result)
                                ? RedirectToAction(nameof(BidQuotes), new { id = transaction.Result })
                                : RedirectToAction(ActionName.Dashboard, ControllerName.Home);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult InitBidClose(string id) => PartialView("_BidQuoteClosePartial", new BidQuoteViewModel { AssetWorksBidQuoteId = id });

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> CloseBidQuote(BidQuoteViewModel vm)
        {
            if (!ModelState.IsValid) RedirectToAction(ActionName.Dashboard, ControllerName.Home);

            var transaction = await MediatR.Send(new BidQuoteCloseCommand(vm));

            return !string.IsNullOrEmpty(transaction.Result)
                                ? RedirectToAction(nameof(BidQuotes), new { id = transaction.Result })
                                : RedirectToAction(ActionName.Dashboard, ControllerName.Home);
        }

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SelectBidder(VendorPartBidSelectedViewModel vm)
        {
            if (!ModelState.IsValid) return JsonSuccess(AppConstants.ValidationWarning);

            var transaction = await MediatR.Send(new BidQuoteSelectBidderCommand(vm));

            return !transaction.Result.Equals("Error") ? JsonSuccess(transaction.Result) : JsonSuccess(transaction.Message);
        }

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdatePurchaseOrderQuantity(BidQuotePartViewModel vm)
        {
            if (!ModelState.IsValid) return JsonSuccess(AppConstants.JsonModelStateInvalid);

            var transaction = await MediatR.Send(new BidQuoteUpdatePOQuantityCommand(vm));

            return !string.IsNullOrEmpty(transaction.Result)
                                ? RedirectToAction(nameof(BidQuotes), new { id = transaction.Result })
                                : RedirectToAction(ActionName.Dashboard, ControllerName.Home);
        }

        /// <summary>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> BidQuotes(string id) => View(await MediatR.Send(new BidQuoteQuery(id)));

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> BidQuotesExportToExcel(string id)
        {
            var response = await MediatR.Send(new BidQuoteQuery(id));
            var vendors = response.SystemVendors.ToList();
            var bidQuote = response.BidQuote;
            var bidParts = bidQuote.Parts.ToList();

            var list = bidParts.Select(p => new
            {
                VendorCount = bidQuote.BidStartTime.HasValue ? p.VendorInPartsCount : vendors?.Count(v => v.CommodityCodes.Split(',').Contains(p.AssetWorksPartCommodityCode)),
                PartNo = p.AssetWorksPartNumber,
                Suffix = p.AssetWorksPartSuffix,
                CC = p.AssetWorksPartCommodityCode,
                Description = p.AssetWorksPartDescription,
                UnitPrice = string.Format("{0:C}", p.AssetWorksUnitPrice.ConvertToDecimal()),
                LineNumber = p.AssetWorksPartLineNumber,
                Requested = p.AssetWorksPartQuantityRequested,
                Committed = p.AssetWorksPartQuantityCommited,
                OnHand = p.AssetWorksPartQuantityOnHand,
                OnOrder = p.AssetWorksPartQuantityOnOrder,
            }).ToList();

            //Build Excel
            var grid = new GridView();
            grid.DataSource = list;
            grid.DataBind();

            for (var row = 0; row < grid.Rows.Count; row++)
            {
                grid.Rows[row].HorizontalAlign = HorizontalAlign.Left;

                for (var col = 0; col < grid.Rows[row].Cells.Count; col++)
                {
                    grid.Rows[row].Cells[col].Attributes.Add("style", "mso-number-format:\\@");
                    if ((row % 2) == 0)
                    {
                        grid.Rows[row].Cells[col].BackColor = System.Drawing.Color.LightGray;
                    }
                }
            }

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);

            Response.ClearContent();
            Response.Buffer = true;
            var FileName = "BidQuote_" + bidQuote.AssetWorksBidQuoteId + ".xls";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return null;
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> AssetWorksExportToExcel(string id)
        {
            var bidQuote = await MediatR.Send(new AssetWorksBidQuoteQuery(id));
            var bidParts = bidQuote.Parts.ToList();

            var list = bidParts.Select(x => new
            {
                LineNumber = x.AssetWorksPartLineNumber,
                PartNo = x.AssetWorksPartNumber,
                Suffix = x.AssetWorksPartSuffix,
                Description = x.AssetWorksPartDescription,
                UnitPrice = string.Format("{0:C}", x.AssetWorksUnitPrice.ConvertToDecimal()),
                Requested = x.AssetWorksPartQuantityRequested,
                Committed = x.AssetWorksPartQuantityCommited,
                OnHand = x.AssetWorksPartQuantityOnHand,
                OnOrder = x.AssetWorksPartQuantityOnOrder,
            }).ToList();

            //Build Excel
            var grid = new GridView();
            grid.DataSource = list;
            grid.DataBind();

            for (var row = 0; row < grid.Rows.Count; row++)
            {
                grid.Rows[row].HorizontalAlign = HorizontalAlign.Center;

                for (int col = 0; col < grid.Rows[row].Cells.Count; col++)
                {
                    grid.Rows[row].Cells[col].Attributes.Add("style", "mso-number-format:\\@");
                    if ((row % 2) == 0)
                    {
                        grid.Rows[row].Cells[col].BackColor = System.Drawing.Color.LightGray;
                    }
                }
            }

            var sw = new StringWriter();
            var htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);

            Response.ClearContent();
            Response.Buffer = true;
            var FileName = "AW_BidQuote_" + bidQuote.AssetWorksBidQuoteId + ".xls";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";

            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return null;
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <param name="filename"></param>
        /// <param name="fileextension"></param>
        /// <returns></returns>
        public async Task<ActionResult> DownloadFile(string id, string filename, string fileextension)
        {
            var response = await _filesConnectedServices.GetFile(id);

            if (response is null) return JsonSuccess(AppSettings.QUERY_ERROR);

            var cd = new ContentDisposition { FileName = $"{filename}{fileextension}", Inline = true };

            Response.AddHeader("Content-Disposition", cd.ToString());

            return File(response.Bytes, response.ContentType, response.FileName);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> InvoiceFileReader(string id)
        {
            var response = await _filesConnectedServices.GetFile(id);

            if (response is null) return RedirectToAction<ErrorsController>(a => a.NotFound());

            var metadata = await MediatR.Send(new InvoiceMetadataQuery(id));

            var cd = new ContentDisposition
            {
                FileName = metadata.DownloadReadyFile,
                Inline = false
            };

            Response.AddHeader("Content-Disposition", cd.ToString());

            return File(response.Bytes, response.ContentType, metadata.DownloadReadyFile);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <param name="referer"></param>
        /// <returns></returns>
        public ActionResult Viewer(string id, string referer)
        {
            ViewBag.Pdf = Url.Action(nameof(InvoiceFileReader), new { id });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SyncVendorInfo()
        {
            var transaction = await MediatR.Send(new SyncVendorInfo());

            return !transaction.Result.Equals("Error") ? JsonSuccess(transaction.Result) : JsonSuccess(transaction.Message);
        }
        [HttpGet]
        public async Task<ActionResult> GenerateXmlHeaders(string domainKey)
        {
            try
            {
                var result = await MediatR.Send(new BidQuoteQuery(domainKey));

                // Group selected bids by vendor
                //var generatedFiles = await MediatR.Send(new CreateBidQuoteZipQuery(result));
                /*var vendorGroups = result.BidQuote.Parts
                .Where(p => p.BidsSelectedCount > 0)
                .SelectMany(p => p.VendorBids.Where(b => b.IsSelected==true),
                            (part, bid) => new { Part = part, Bid = bid })
                .GroupBy(x => x.Bid.Vendor.AssetWorksVendorNumber)
                .ToList();

            if (!vendorGroups.Any())
            {
                TempData["Error"] = "No selected bids found to generate XML.";
                return RedirectToAction($"BidQuotes/{domainKey}", "Bids");
            }

            var extractPath = ConfigurationManager.AppSettings["BidQuoteExportPath"];
            Directory.CreateDirectory(extractPath);

            Parallel.ForEach(vendorGroups, vendorGroup =>
            {
                var xmlContent = GenerateVendorXml(result, vendorGroup.Key);
                if (string.IsNullOrWhiteSpace(xmlContent)) return;

                var rawVendorName = vendorGroup.First().Bid.Vendor.AssetWorksVendorName ?? "UnknownVendor";
                var vendorName = SanitizeFileName(rawVendorName);
                var xmlFileName = $"BidQuote_{result.BidQuote.AssetWorksBidQuoteId}_{vendorName}.xml";
                var filePath = Path.Combine(extractPath, xmlFileName);

                using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                using (var writer = new StreamWriter(fs, Encoding.UTF8))
                {
                    writer.Write(xmlContent);
                }
            });
*/
                /*if (generatedFiles != null)
                {*/
                    await MediatR.Send(new FlagUpdatePOCommand(domainKey));

                    // Set TempData success notification
                    TempData["Success"] = "PO files generated successfully!";

                    return RedirectToAction($"BidQuotes/{domainKey}", "Bids", result);
                /* }
               else 
                {
                    TempData["Success"] = "XML files no generated!";

                    return RedirectToAction($"BidQuotes/{domainKey}", "Bids", result);
                }*/
                
            }
            catch (Exception ex)
            {
                // Set TempData error notification
                TempData["Error"] = $"An error occurred: {ex.Message}";
                return RedirectToAction($"BidQuotes/{domainKey}", "Bids");
            }
        }


/*
        /// <summary>
        /// Replace invalid filename characters with underscores (safe vendor filenames).
        /// </summary>
        private static string SanitizeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var sb = new StringBuilder(name.Length);
            foreach (var ch in name)
                sb.Append(invalid.Contains(ch) ? '_' : ch);
            return sb.ToString();
        }

        private static string GenerateVendorXml( BidQuoteResponseDto query, string vendorId)
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
        public static class NetworkShareHelper
        {
            [DllImport("advapi32.dll", SetLastError = true)]
            private static extern bool LogonUser(
                string lpszUsername,
                string lpszDomain,
                string lpszPassword,
                int dwLogonType,
                int dwLogonProvider,
                out IntPtr phToken);

            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool CloseHandle(IntPtr hObject);

            private const int LOGON32_LOGON_NEW_CREDENTIALS = 9;
            private const int LOGON32_PROVIDER_DEFAULT = 0;

            public static void ImpersonateAndExecute(string domain, string username, string password, Action action)
            {
                IntPtr token;
                bool success = LogonUser(username, domain, password, LOGON32_LOGON_NEW_CREDENTIALS, LOGON32_PROVIDER_DEFAULT, out token);
                if (!success) throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());

                using (WindowsIdentity identity = new WindowsIdentity(token))
                using (WindowsImpersonationContext context = identity.Impersonate())
                {
                    action();
                }

                CloseHandle(token);
            }
        }*/

    }
}