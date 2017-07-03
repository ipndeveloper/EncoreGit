using System;
using System.Text.RegularExpressions;
using System.Threading;
using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Shop
{
    /// <summary>
    /// Class related to Controls and methods of PWS Billing page.
    /// </summary>
    public class PWS_Shop_Billing_Page : PWS_Shop_Base_Page
    {
        private Billing_Control _billing;
        private Link _continue;
        private Link _apply;
        private Span _balance;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _continue = _content.GetElement<Link>(new Param("btnNext"));
            _billing = _content.GetElement<Div>(new Param("creditCardEntry")).As<Billing_Control>();
            _apply = _content.GetElement<Link>(new Param("btnApplyPayment"));
            _balance = _content.GetElement<Span>(new Param("balanceDue"));
        }

        public override bool IsPageRendered()
        {
            return _billing.Element.Exists;
        }

        public Billing_Control Billing
        {
            get { return _billing; }
        }

        [Obsolete("Use 'Billing'", true)]
        public PWS_Shop_Billing_Page EnterBilling(BillingProfile profile)
        {
            return this;
        }

        public PWS_Shop_Billing_Page ClickApplyPayment(int? timeout = null)
        {
            timeout = _apply.CustomClick(timeout);
            Util.Browser.CustomWaitForSpinners(timeout);
            return this;
        }

        public PWS_Shop_ConfirmOrder_Page ClickContinue(int? timeout = null, bool pageRequired = true)
        {
            timeout = _content.GetElement<Span>(new Param("balanceDue")).CustomWaitForAttributeValue(AttributeName.ID.InnerText, "0.00", RegexOptions.None, timeout);
            timeout = _continue.CustomClick(timeout);
            return Util.GetPage<PWS_Shop_ConfirmOrder_Page>(timeout, pageRequired);
        }

        public decimal Balance()
        {
            return Decimal.Parse(_balance.CustomGetText().Substring(1));
        }
    }
}