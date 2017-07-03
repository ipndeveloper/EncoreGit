using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_Receipt_Page : DWS_Base_Page
    {
        private Link _print;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _print = _content.GetElement<Link>(new Param("Button Primary", AttributeName.ID.ClassName));
        }

        public override bool IsPageRendered()
        {
            return _print.Exists;
        }
    }
}
