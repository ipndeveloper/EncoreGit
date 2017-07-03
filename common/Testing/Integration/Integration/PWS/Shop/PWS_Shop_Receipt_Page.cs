using WatiN.Core;
namespace NetSteps.Testing.Integration.PWS.Shop
{
    public class PWS_Shop_Receipt_Page : PWS_Base_Page
    {
        private Link _print;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _print = _content.GetElement<Link>(new Param("FR Button btnPrintReceipt", AttributeName.ID.ClassName));
            _print.CustomWaitForExist();
        }

         public override bool IsPageRendered()
        {
            return _print.Exists;
        }
    }
}