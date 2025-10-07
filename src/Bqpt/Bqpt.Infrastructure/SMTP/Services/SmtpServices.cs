////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;
using System.Net.Mail;
using System.Net.Mime;
using BC.Identity.Kernel;
using Bqpt.Common;

namespace Bqpt.Infrastructure
{
    public class SmtpServices : ISmtpServices
    {
        private readonly string _applicationName = AppConstants.ApplicationName;

        /// <summary>
        /// Main Helper Method to relay an email with attachments (if any) using BC Cisco Iron Port Appliance
        /// </summary>
        /// <param name="baseEmail"></param>
        /// <param name="useCustomTemplate"></param>
        /// <returns>
        /// Returns (sent) as string is all went good, or a notification of exception type that can
        /// be checked on ASG Logger Portal
        /// </returns>
        public string SendEmail(BaseEmailDto baseEmail, bool useCustomTemplate = false)
        {
            using (var smtpClient = new SmtpClient(AppConstants.RelayServerUrl)
            {
                Port = AppConstants.RelayServerPort,
                UseDefaultCredentials = true
            })
            {
                try
                {
                    var newEmail = new MailMessage { From = new MailAddress(baseEmail.From) };

                    foreach (var recipient in baseEmail.To)
                    {
                        newEmail.To.Add(recipient);
                    }

                    foreach (var recipient in baseEmail.Cc)
                    {
                        newEmail.CC.Add(recipient);
                    }

                    foreach (var recipient in baseEmail.Bcc)
                    {
                        newEmail.Bcc.Add(recipient);
                    }

                    newEmail.Priority = baseEmail.MailPriority;
                    newEmail.Subject = baseEmail.Subject;
                    newEmail.Body = useCustomTemplate
                                    ? GetCustomEmailHtmlTemplate(baseEmail.Body)
                                    : GetEmailHtmlTemplate(baseEmail.Body);

                    newEmail.IsBodyHtml = baseEmail.IsHtml;

                    if (baseEmail.HasAttachment)
                    {
                        newEmail.Attachments.Add(new Attachment(baseEmail.Attachment, baseEmail.AttachmentFileName, mediaType: MediaTypeNames.Application.Octet));
                    }

                    smtpClient.Send(newEmail);

                    return "sent";
                }
                catch (SmtpException smtpEx)
                {
                    var log = new KernelLoggerFactories(_applicationName);

                    log.Save(Castle.Core.Logging.LoggerLevel.Fatal, _applicationName, smtpEx.Message, smtpEx);

                    return $"SMTP Exception refer to ASG Logger for more information";
                }
                catch (Exception ex)
                {
                    var log = new KernelLoggerFactories(_applicationName);

                    log.Save(Castle.Core.Logging.LoggerLevel.Fatal, _applicationName, ex.Message, ex);

                    return $"Generic Exception refer to ASG Logger for more information";
                }
            }
        }

        /// <summary>
        /// Custom Implementation of HTML Template, to application specific requierements
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetCustomEmailHtmlTemplate(string content) => content;

        /// <summary>
        /// Standard HTML Template
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string GetEmailHtmlTemplate(string content) => $"<table align='center' style='margin: 0 auto 0 auto;width:800px;font-family: arial;'><tr><td><br /><br />{content}<br /><br /></td></tr><tr><td><hr /></td><tr><td><center style='font-family: arial; color: green; font-size: 10px;'>This is an application generated email. | Add <strong>NoReply Service Account</strong> to your Safe Sender List.</center></td></tr></table>";

        /// <summary>
        /// Culture Validation for Placeholders
        /// </summary>
        /// <param name="target"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private string Formatter(string target, params object[] args) => string.Format(CultureInfo.CurrentCulture, target, args);

        /// <summary>
        /// Given a Template from Db replace content in Placeholders
        /// </summary>
        /// <param name="template"></param>
        /// <param name="placeHolders"></param>
        public string PrepareHtmlTemplate(string template, params string[] placeHolders)
        {
            for (var i = 0; i < placeHolders.Length - 1; i += 2)
            {
                template = template.Replace(string.Format("<%{0}%>", Formatter(placeHolders[i])), placeHolders[i + 1]);
            }

            return template;
        }
    }
}