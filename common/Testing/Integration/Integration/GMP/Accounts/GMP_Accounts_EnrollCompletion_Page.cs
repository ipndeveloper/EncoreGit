using WatiN.Core;
using WatiN.Core.Extras;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_EnrollCompletion_Page : GMP_Accounts_Base_Page
    {
        private ElementCollection<HeaderLevel4> hfourMsgCongrats;
        private Link lnkAccountOverview;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            this.hfourMsgCongrats = Document.GetElement<Div>(new Param("StepBody", AttributeName.ID.ClassName)).ElementsOfType<HeaderLevel4>();
            this.lnkAccountOverview = Document.GetElement<Link>(new Param("/Accounts/Overview/Index/.*[0-9]", AttributeName.ID.Href, RegexOptions.None));
            lnkAccountOverview.CustomWaitForVisibility();
        }

         public override bool IsPageRendered()
        {
            return lnkAccountOverview.Exists;
        }

        /// <summary>
        /// Get message after complete enrollment.
        /// </summary>
        /// <returns>Message.</returns>
        [Obsolete("Not needed")]
        public string GetEnrollmentMessage()
        {
            System.Threading.Thread.Sleep(2000);

            if (this.hfourMsgCongrats.Count >= 1)
            {
                return this.hfourMsgCongrats[0].CustomGetText();
            }

            return string.Empty;
        }

        /// <summary>
        /// Click account overview link.
        /// </summary>
        public GMP_Accounts_Overview_Page ClickAccountOverviewLink(int? timeout = null, bool pageRequired = true)
        {
            timeout = this.lnkAccountOverview.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_Overview_Page>(timeout, pageRequired);
        }
    }
}
