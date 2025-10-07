using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Web.Mvc;
using BC;
using BC.Identity.Kernel;
using Bqpt.Application;
using Bqpt.Common;
using Bqpt.Infrastructure;
using MediatR;

namespace Bqpt.ExternalUI.Controllers
{
    public class BidsController : KernelControllerBase
    {
        private readonly IFilesConnectedServices _filesConnectedServices;

        public BidsController(IMediator mediator, IFilesConnectedServices filesConnectedServices)
        {
            MediatR = mediator;
            _filesConnectedServices = filesConnectedServices;
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index() => View(await MediatR.Send(new VendorDashboardQuery()));

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ActionResult> VendorBid(string id) => View(await MediatR.Send(new VendorInPartsQuery(id)));

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> GetBidParts(string id) => PartialView("_BidPartsPartial", await MediatR.Send(new VendorInPartsQuery(id)));

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> InitAddBid(string id) => PartialView("_PartBidAddPartial", await MediatR.Send(new VendorInPartQuery(id)));

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> AddBid(VendorPartBidViewModel vm)
        {
            if (vm.VendorId == null)
            {
                ModelState.AddModelError(nameof(vm.VendorId), "Vendor Id is not valid");
            }
            else if (vm.BidQuotePartId == null)
            {
                ModelState.AddModelError(nameof(vm.BidQuotePartId), "Part Id is not valid");
            }
            else if (vm.QuantityAvailable == null)
            {
                ModelState.AddModelError(nameof(vm.QuantityAvailable), "Quantity is not valid");
            }
            else if (vm.ItemsPerUnit == null)
            {
                ModelState.AddModelError(nameof(vm.ItemsPerUnit), "Items Per Unit is not valid");
            }
            else if (vm.UnitPrice == null)
            {
                ModelState.AddModelError(nameof(vm.UnitPrice), "Unit Price is not valid");
            }
            else if (vm.EstimateDeliveryDate == null)
            {
                ModelState.AddModelError(nameof(vm.EstimateDeliveryDate), "Delivery Date is not valid");
            }

            var modelError = ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors, (modelState, error) => error.ErrorMessage).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return JsonSuccess("Error: " + modelError);
            }

            var transaction = await MediatR.Send(new VendorAddBidCommand(vm));

            return !transaction.Result.Equals("Error") ? JsonSuccess(transaction.Result) : JsonSuccess(transaction.Message);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> InitUpdateBid(string id) => PartialView("_PartBidUpdatePartial", await MediatR.Send(new VendorBidPartQuery(id)));

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> UpdateBid(VendorPartBidViewModel vm)
        {
            if (vm.QuantityAvailable == null)
            {
                ModelState.AddModelError(nameof(vm.QuantityAvailable), "Quantity is not valid");
            }
            else if (vm.ItemsPerUnit == null)
            {
                ModelState.AddModelError(nameof(vm.ItemsPerUnit), "Items Per Unit is not valid");
            }
            else if (vm.UnitPrice == null)
            {
                ModelState.AddModelError(nameof(vm.UnitPrice), "Unit Price is not valid");
            }
            else if (vm.EstimateDeliveryDate == null)
            {
                ModelState.AddModelError(nameof(vm.EstimateDeliveryDate), "Delivery Date is not valid");
            }

            var modelError = ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors, (modelState, error) => error.ErrorMessage).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return JsonSuccess("Error: " + modelError);
            }

            var transaction = await MediatR.Send(new VendorUpdateBidCommand(vm));
            return !transaction.Result.Equals("Error") ? JsonSuccess(transaction.Result) : JsonSuccess(transaction.Message);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> InitDeleteBid(string id) => PartialView("_PartBidDeletePartial", await MediatR.Send(new VendorBidPartQuery(id)));

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteBid(VendorPartBidViewModel vm)
        {
            if (vm.DomainKey == null)
            {
                ModelState.AddModelError(nameof(vm.QuantityAvailable), "Bid Part is not valid");
            }

            var modelError = ViewData.ModelState.Values.SelectMany(modelState => modelState.Errors, (modelState, error) => error.ErrorMessage).FirstOrDefault();

            if (!ModelState.IsValid)
            {
                return JsonSuccess("Error: " + modelError);
            }

            var transaction = await MediatR.Send(new VendorDeleteBidCommand(vm));

            return !transaction.Result.Equals("Error") ? JsonSuccess(transaction.Result) : JsonSuccess(transaction.Message);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<PartialViewResult> InitUploadVendorBidQuoteAttachment(string id) => PartialView("_VendorBidQuoteAttachmentManager", await MediatR.Send(new VendorBidQuoteAttachmentQuery(id)));

        /// <summary>
        /// POST:
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveInvoiceFile(VendorBidQuoteAttachmentViewModel vm)
        {
            if (!ModelState.IsValid) return JsonSuccess(AppSettings.TRANSACTION_ERROR);

            var transaction = await MediatR.Send(new VendorBidFileUploadCommand(vm));

            if (transaction is null) return JsonSuccess(AppConstants.JsonError);

            return PartialView("_VendorBidQuoteAttachmentsPartial", transaction.Result);
        }

        /// <summary>
        /// GET:
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<ActionResult> DeleteAttachment(VendorBidQuoteAttachmentDto payload) => PartialView("_VendorBidQuoteAttachmentsPartial", await MediatR.Send(new DeleteAttachmentCommand(payload)));

        public async Task<ActionResult> Files(string id)
        {
            var attachment = await MediatR.Send(new AttachmentQuery(id));

            var blobResponse = await _filesConnectedServices.GetFile(id);

            if (blobResponse is null) return JsonSuccess(AppSettings.QUERY_ERROR);

            var cd = new ContentDisposition { FileName = $"{attachment.AttachmentFriendlyName}{attachment.FileExtension}", Inline = true };

            Response.AddHeader("Content-Disposition", cd.ToString());

            return File(blobResponse.Bytes, blobResponse.ContentType, $"{attachment.AttachmentFriendlyName}{attachment.FileExtension}");
        }
    }
}