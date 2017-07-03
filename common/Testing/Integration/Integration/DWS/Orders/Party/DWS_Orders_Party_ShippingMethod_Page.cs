using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_ShippingMethod_Page : DWS_Base_Page
    {
        private Link _continue;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _continue = Document.GetElement<Link>(new Param("btnNext2"));
            int? timeout = _continue.CustomWaitForEnabled();
            Thread.Sleep(1000);
            _continue.CustomWaitForEnabled(timeout:timeout);
        }

         public override bool IsPageRendered()
        {
            return _continue.Exists;
        }

         public DWS_Orders_Party_Payment_Page ClickContinue(int? timeout = null, bool pageRequired = true)
        {
            timeout = _continue.CustomClick(timeout);
            return Util.GetPage<DWS_Orders_Party_Payment_Page>(timeout, pageRequired);
        }
    }
}
