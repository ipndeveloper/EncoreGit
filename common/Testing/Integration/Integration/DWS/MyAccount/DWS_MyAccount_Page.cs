using WatiN.Core;
using WatiN.Core.Extras;
using System.Text.RegularExpressions;
using NetSteps.Testing.Integration.GMP.Reports;

namespace NetSteps.Testing.Integration.DWS.MyAccount
{
    public class DWS_MyAccount_Page : DWS_MyAccount_Base_Page
    {
        private ControlCollection<DWS_MyAccount_Autoship_Control> _autoships;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _autoships = Document.Divs.Filter(Find.ByClass("clr mb10 autoship")).As<DWS_MyAccount_Autoship_Control>();

        }

         public override bool IsPageRendered()
        {
            return this.HeaderPageTitle.Contains("Account Overview");
        }

        public ControlCollection<DWS_MyAccount_Autoship_Control> AutoShips
        {
            get { return _autoships; }
        }

        public GMP_Reports_PersonalEarningsStatement_Page ClickMyEarningsReport()
        {
            _content.GetElement<Link>(new Param("ReportViewer", AttributeName.ID.Href, RegexOptions.None)).CustomClick();
            Util.AttachBrowser<GMP_Reports_PersonalEarningsStatement_Page>("ReportViewer");
            return Util.GetPage<GMP_Reports_PersonalEarningsStatement_Page>();
        }

        public DWS_MyAccount_ShippingProfile_Page ClickAddShippingProfile(int? timeout = null, bool pageRequired = true)
        {

            timeout = Document.GetElement<Link>(new Param("Account/BillingShippingProfiles/EditAddress$", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_MyAccount_ShippingProfile_Page>(timeout, pageRequired);
        }

        public DWS_MyAccount_BillingProfile_Page ClickAddBillingProfile(int? timeout = null, bool pageRequired = true)
        {

            timeout = Document.GetElement<Link>(new Param("Account/BillingShippingProfiles/EditPayment$", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_MyAccount_BillingProfile_Page>(timeout, pageRequired);
        }
    }
}