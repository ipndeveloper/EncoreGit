using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Receipt_Page : PWS_Base_Page
    {
        private Link _print;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _print = _content.GetElement<Link>(new Param("FR Button btnPrintReceipt", AttributeName.ID.ClassName));
            _print.CustomWaitForExist();
        }

        public TPage ClickReturnHome<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("Button MinorButton", AttributeName.ID.ClassName)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

         public override bool IsPageRendered()
        {
            return _print.Exists;
        }
    }
}
