using System;
using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_Payment_Page : DWS_Base_Page
    {
        private Link _apply, _submit;
        private DWS_Billing_Control _billing;
        private TextField _amount;
        private RadioButton _partyPayment, _customertPayment;
        private SelectList _selectCustomer;
        private Span _balance;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _apply = _content.GetElement<Link>(new Param("btnApplyPayment"));
            _billing = _content.GetElement<Div>(new Param("creditCardEntry")).As<DWS_Billing_Control>();
            _amount = _content.GetElement<TextField>(new Param("txtAmount"));
            _submit = _content.GetElement<Link>(new Param("btnNext"));
            _partyPayment = _content.GetElement<RadioButton>(new Param("rbPartyPayment"));
            _customertPayment = _content.GetElement<RadioButton>(new Param("rbCustomerPayment"));
            _selectCustomer = _content.GetElement<SelectList>(new Param("customer"));
            _balance = _content.GetElement<Span>(new Param("balanceDue"));
        }

        public override bool IsPageRendered()
        {
            return _balance.Exists;
        }

        public bool PartyPayment
        {
            get { return _partyPayment.Checked; }
            set { _partyPayment.CustomSelectRadioButton(value); }
        }

        public bool CustomerPayment
        {
            get { return _customertPayment.Checked; }
            set { _customertPayment.CustomSelectRadioButton(value); }
        }

        public decimal GetBalance()
        {
            return decimal.Parse(_balance.Text.Substring(1));
        }

        public int? SelectCustomer(RetailCustomer customer)
        {
            return _selectCustomer.CustomSelectDropdownItem(String.Format("{0} {1}", customer.FirstName, customer.LastName));
        }

        [Obsolete("Use 'EnterCreditPayment()'", true)]
        public void EnterCreditCard(BillingProfile profile, decimal amount, int? timeout = null)
        {
        }

        public void EnterCreditPayment(BillingProfile profile, decimal amount, int? timeout = null)
        {
            _billing.EnterDetails(profile);
            _amount.CustomSetTextQuicklyHelper(amount.ToString());
            _apply.CustomClick(timeout);
            _content.CustomWaitForSpinners();
        }

        [Obsolete("Use ClickSubmit<DWS_Orders_Party_Receipt_Page>()'", true)]
        public DWS_Orders_Party_Receipt_Page ClickSubmit()
        {
            _submit.CustomClick();
            return Util.GetPage<DWS_Orders_Party_Receipt_Page>();
        }

        public TPage ClickSubmit<TPage>() where TPage : DWS_Base_Page, new()
        {
            _submit.CustomClick();
            return Util.GetPage<TPage>();
        }

        public void EnterGuestPayment(RetailCustomer customer)
        {
            SelectCustomer(customer);
            EnterCreditPayment(customer.Billing.GetProfile(0), customer.ShoppingBag.Balance);
        }
    }
}
