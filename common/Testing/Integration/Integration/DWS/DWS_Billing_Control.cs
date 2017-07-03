using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS
{
    public class DWS_Billing_Control : Control<Div>
    {
        private TextField _name, _account, _cvv, _postalCode;
        private SelectList _month, _year;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _name = Element.GetElement<TextField>(new Param("txtNameOnCard"));
            _account = Element.GetElement<TextField>(new Param("txtCCN"));
            _cvv = Element.GetElement<TextField>(new Param("txtCVV"));
            _month = Element.GetElement<SelectList>(new Param("expMonth"));
            _year = Element.GetElement<SelectList>(new Param("expYear"));
            _postalCode = Element.GetElement<TextField>(new Param("billZipcode"));
        }

        public DWS_Billing_Control EnterDetails(BillingProfile profile)
        {
            _name.CustomSetTextQuicklyHelper(profile.Name);
            _account.CustomSetTextQuicklyHelper(profile.Account);
            _cvv.CustomSetTextQuicklyHelper(profile.CVV);
            _month.CustomSelectDropdownItem(profile.Expiration.Month.ToString());
            _year.CustomSelectDropdownItem(profile.Expiration.Year.ToString());
            _postalCode.CustomSetTextQuicklyHelper(profile.Address.PostalCode);
            return this;
        }
    }
}
