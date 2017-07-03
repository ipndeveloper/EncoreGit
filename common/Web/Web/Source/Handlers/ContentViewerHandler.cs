using System;
using System.Web;
using NetSteps.Objects.Business;
using NetSteps.Web.Base;

namespace NetSteps.Web.Handlers
{
    // TODO: Add more options for loading different types of content later. - JHE
    public class ContentViewerHandler : BaseHttpHandler
    {
        #region
        private const string EMAIL_TEMPLATEID = "ETID";
        private const string HTMLCONTENTID = "HID";
        #endregion

        #region Members
        private string ErrorMsg = string.Empty;
        #endregion

        #region Methods
        #endregion

        #region BaseHttpHandler Overrides
        public override void SetResponseCachePolicy(HttpCachePolicy cache)
        {
            cache.SetCacheability(HttpCacheability.Public);
            cache.SetExpires(DateTime.Now.AddDays(14));
            cache.SetValidUntilExpires(true);
        }

        /// <summary>
        /// Main interface for reacting to the Thumbnailer request.
        /// </summary>
        /// <param name="context"></param>
        protected override void HandleRequest(HttpContext context)
        {
            int emailTemplateId = GetQueryStringVar<int>(EMAIL_TEMPLATEID);
            int htmlContentId = GetQueryStringVar<int>(HTMLCONTENTID);
            string content = string.Empty;

            if (emailTemplateId != 0)
            {
                EmailTemplate emailTemplate = EmailTemplate.Load(emailTemplateId);
                content = emailTemplate.Body;
            }
            else if (htmlContentId != 0)
            {
                HtmlContent htmlContent = HtmlContent.Find(htmlContentId);
                content = htmlContent.Html_ForDisplay;
            }

            if (content == string.Empty)
                WriteOutSimpleTextResponse("Content not specified on the QueryString!", context);
            else
                WriteOutFullTextResponse(content, context);
        }
        #endregion
    }
}
