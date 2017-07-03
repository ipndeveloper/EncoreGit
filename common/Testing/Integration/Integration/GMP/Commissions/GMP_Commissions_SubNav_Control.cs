using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Commissions
{
    public class GMP_Commissions_SubNav_Control : Control<UnorderedList>
    {
        private Link _admin;
        private Link _runner;
        private Link _publish;
        private Link _disbursements;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _admin = Element.GetElement<Link>(new Param("Admin", AttributeName.ID.InnerText));
            _runner = Element.GetElement<Link>(new Param("Runner", AttributeName.ID.InnerText));
            _publish = Element.GetElement<Link>(new Param("Publish", AttributeName.ID.InnerText));
            _disbursements = Element.GetElement<Link>(new Param("Disbursements", AttributeName.ID.InnerText));
        }

        public GMP_Commissions_Admin_Page ClickAdmin(int? timeout = null, bool pageRequired = true)
        {
            timeout = _admin.CustomClick(timeout);
            return Util.GetPage<GMP_Commissions_Admin_Page>(timeout, pageRequired);
        }

        public GMP_Commissions_Runner_Page ClickRunner(int? timeout = null, bool pageRequired = true)
        {
            timeout = _runner.CustomClick(timeout);
            return Util.GetPage<GMP_Commissions_Runner_Page>(timeout, pageRequired);
        }

        public GMP_Commissions_Publish_Page ClickPublish(int? timeout = null, bool pageRequired = true)
        {
            timeout = _publish.CustomClick(timeout);
            return Util.GetPage<GMP_Commissions_Publish_Page>(timeout, pageRequired);
        }

        public GMP_Commissions_Disbursements_Page ClickDisbursements(int? timeout = null, bool pageRequired = true)
        {
            timeout = _disbursements.CustomClick(timeout);
            return Util.GetPage<GMP_Commissions_Disbursements_Page>(timeout, pageRequired);
        }

    }
}
