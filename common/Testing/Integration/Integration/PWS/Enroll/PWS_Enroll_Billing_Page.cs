using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Billing_Page : PWS_Base_Page
    {
        private SelectList _paymentType, _expiryMonth, _expiryYear;
        private TextField _cardName, _cardNumber;
        private RadioButton _useMainAddress, _useShippingAddress, _useBillingAddress;
        private Address_Control _billingAddress;
        private Link _next;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _paymentType = _content.GetElement<SelectList>(new Param("PaymentMethod_PaymentTypeID"));
            _cardName = _content.GetElement<TextField>(new Param("PaymentMethod_NameOnCard"));
            _cardNumber = _content.GetElement<TextField>(new Param("PaymentMethod_CreditCardNumber"));
            _expiryMonth = _content.GetElement<SelectList>(new Param("PaymentMethod_CreditCardMonth"));
            _expiryYear = _content.GetElement<SelectList>(new Param("PaymentMethod_CreditCardYear"));
            _useMainAddress = _content.GetElement<RadioButton>(new Param("chkBillingAddressSourceMain"));
            _useShippingAddress = _content.GetElement<RadioButton>(new Param("chkBillingAddressSourceShipping"));
            _useBillingAddress = _content.GetElement<RadioButton>(new Param("chkBillingAddressSourceBilling"));
            _next = _content.GetElement<Link>(new Param("btnSubmit").Or(new Param("btnNext")));
        }

         public override bool IsPageRendered()
        {
            return _cardName.Exists;
        }

        public PWS_Enroll_Billing_Page UseMainAddress()
        {            
            _useMainAddress.CustomSelectRadioButton();
            return this;
        }

        public PWS_Enroll_Billing_Page UseShippingAddress()
        {
            _useShippingAddress.CustomSelectRadioButton();
            return this;
        }

        public PWS_Enroll_Billing_Page UseBillingAddress()
        {
            _useBillingAddress.CustomSelectRadioButton();
            return this;
        }

        public Address_Control Address
        {
            get
            {
                if (_billingAddress == null)
                    _billingAddress = _content.GetElement<Div>(new Param("BillingAddress")).As<Address_Control>();
                return _billingAddress;
            }
        }

        public PWS_Enroll_Billing_Page EnterCreditCard(BillingProfile profile)
        {
            _cardName.CustomSetTextQuicklyHelper(profile.Name);
            _paymentType.CustomSelectDropdownItem(profile.CardType.ToPattern());
            _cardNumber.CustomSetTextQuicklyHelper(profile.Account);
            _expiryMonth.CustomSelectDropdownItem(profile.Expiration.Month);
            _expiryYear.CustomSelectDropdownItem(profile.Expiration.Year.ToString());
            return this;
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        public bool ValidateCreditCard(BillingProfile profile)
        {
            bool valid = Compare.CustomCompare<string>(_paymentType.CustomGetSelectedItem(), CompareID.Equal, profile.CardType.ToPattern(), "Card Type");
            if (!Compare.CustomCompare<string>(_cardName.CustomGetText(), CompareID.Equal, profile.Name, "Card Name"))
                valid = false;
            if (!Compare.CustomCompare<string>(_cardNumber.CustomGetText(), CompareID.Equal, profile.Account, "Account Number"))
                valid = false;
            if (!Compare.CustomCompare<string>(_expiryMonth.SelectedOption.Value, CompareID.Equal, profile.Expiration.Month.ToString(), "Expiration Month"))
                valid = false;
            if (!Compare.CustomCompare<string>(_expiryYear.SelectedOption.Value, CompareID.Equal, profile.Expiration.Year.ToString(), "Expiration Year"))
                valid = false;
            return valid;
        }
    }
}
