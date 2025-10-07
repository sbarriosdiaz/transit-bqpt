////////////////////////////////////////////////////////////////////////////////////////////////////////
// Broward County Application Services Group
// Application: BC Commerce | Lead Architect: Cesar L Diaz
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Web.Mvc;

namespace Bqpt.WebUI.Controllers
{
    public class ReportsController : Controller
    {
        public ActionResult BidReport(string id)
        {
            var reportFolder = System.Configuration.ConfigurationManager.AppSettings["ReportFolder"].ToString();
            var reportName = System.Configuration.ConfigurationManager.AppSettings["ReportName"].ToString();
            var reportDomain = System.Configuration.ConfigurationManager.AppSettings["ReportDomain"].ToString();
            var reportUsername = System.Configuration.ConfigurationManager.AppSettings["ReportUsername"].ToString();
            var reportPassword = System.Configuration.ConfigurationManager.AppSettings["ReportPassword"].ToString();

            var rs = new ReportExecution.ReportExecutionService();

            rs.Credentials = new System.Net.NetworkCredential(reportUsername, reportPassword, reportDomain);

            var format = "Excel";

            var devInfo = "";

            var parameters = new ReportExecution.ParameterValue[3];
            parameters[0] = new ReportExecution.ParameterValue();
            parameters[0].Name = "BidQuoteId";
            parameters[0].Value = id;

            var execInfo = new ReportExecution.ExecutionInfo();

            rs.ExecutionHeaderValue = new ReportExecution.ExecutionHeader();

            execInfo = rs.LoadReport($"/{reportFolder}/{reportName}", null);

            rs.SetExecutionParameters(parameters, "en-us");
            _ = rs.ExecutionHeaderValue.ExecutionID;

            var result = rs.Render(format,
                               devInfo,
                               out var extension,
                               out var mimeType,
                               out var encoding,
                               out var warnings,
                               out var streamIDs);

            execInfo = rs.GetExecutionInfo();

            var filename = $"{reportName}.{extension}";

            Response.Clear();
            Response.BufferOutput = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("Content-Length", result.Length.ToString());

            Response.AddHeader("Content-Disposition", $"attachment;filename={filename}");

            Response.AddHeader("Accept-Header", result.Length.ToString());
            Response.ContentType = $"application/{format}";

            Response.BinaryWrite(result);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
    }
}