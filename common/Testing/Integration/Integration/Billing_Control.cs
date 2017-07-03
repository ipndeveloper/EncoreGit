using WatiN.Core;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace NetSteps.Testing.Integration
{
    public class Billing_Control : Control<Div>
    {
        bool  _hasCardType, _hasCVV, _hasAddress;
        private TextField _name, _account, _cvv, _zip;
        private SelectList _paymentType, _month, _year;
        private Address_Control _address;

        protected override void  InitializeContents()
        {
            Element.CustomWaitForExist();
            _hasCardType = false;
            _hasCVV = false;
            _hasAddress = false;
            _paymentType = Element.GetElement<SelectList>(new Param("PaymentMethod_PaymentTypeID"));
            _name = Element.GetElement<TextField>(new Param("nameOnCard", AttributeName.ID.Id, RegexOptions.IgnoreCase));
            _account = Element.GetElement<TextField>(new Param("Number", AttributeName.ID.Id, RegexOptions.None).Or(new Param("txtCCN")));
            _month = Element.GetElement<SelectList>(new Param("Month", AttributeName.ID.Id, RegexOptions.None));
            _year = Element.GetElement<SelectList>(new Param("Year", AttributeName.ID.Id, RegexOptions.None));
            _address = Element.As<Address_Control>();
            _paymentType = Element.GetElement<SelectList>(new Param("PaymentMethod_PaymentTypeID"));
            _cvv = Element.GetElement<TextField>(new Param("txtCVV"));
            _zip = Element.GetElement<TextField>(new Param("billZipcode"));
        }

        public Address_Control Address
        {
            get { return _address; }
        }

        public Billing_Control ConfigureBillingControl(bool hasCardType = false, bool hasCVV = false, bool hasAddress = false, bool countrySelector = false, bool profileLine = false, bool attentionLine = false, bool thirdAddressLine = false)
        {
            _hasCardType = hasCardType;
            _hasCVV = hasCVV;
            _hasAddress = hasAddress;
            if (_hasAddress)
                Address.ConfigureAddressControl(countrySelector, profileLine, attentionLine, thirdAddressLine);
            return this;
        }

        public void EnterBilling(BillingProfile profile)
        {
            _name.CustomSetTextQuicklyHelper(profile.Name);
            _account.CustomSetTextQuicklyHelper(profile.Account);
            _month.CustomSelectDropdownItem(profile.Expiration.Month.ToString());
            _year.CustomSelectDropdownItem(profile.Expiration.Year.ToString());
            if (_hasCardType && profile.CardType != CreditCard.ID.None)
                _paymentType.CustomSelectDropdownItem(profile.CardType.ToPattern());
            if (_hasAddress && profile.Address.Address1 != null)
            {
                _address.EnterAddress(profile.Address);
            }
            else if (_hasCVV)
            {
                _cvv.CustomSetTextQuicklyHelper(profile.CVV);
                _zip.CustomSetTextQuicklyHelper(profile.Address.PostalCode);
            }

        }
    }
}
