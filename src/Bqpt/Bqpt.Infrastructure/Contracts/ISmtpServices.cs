////////////// Broward County Application Services Group //////////////////////
///////////////////////////////////////////////////////////////////////////////
///// Ver:

///// Project: BQPT
///////////////////////////////////////////////////////////////////////////////

namespace Bqpt.Infrastructure
{
    public interface ISmtpServices
    {
        string PrepareHtmlTemplate(string template, params string[] placeHolders);

        string SendEmail(BaseEmailDto baseEmail, bool useCustomTemplate = false);
    }
}