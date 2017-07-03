using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    [Obsolete("Use 'GMP_Accounts_AddressControl'")]
    public class GMP_Accounts_ShippingAddressControl : Control<Div>
    {
        private CheckBox _chkUseMainForShipping;
        private TextField _shippingAddressAttention;
        private TextField _shippingAddressAddress1;
        private TextField _shippingAddressAddress2;
        private TextField _shippingAddressAddress3;
        private TextField _shippingAddressZip;
        private TextField _shippingAddressZipPlusFour;
        private SelectList _shippingAddressCity;
        private SelectList _shippingAddressCounty;
        private SelectList _shippingAddressState;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _chkUseMainForShipping = Element.CheckBox("chkUseMainForShipping");
            _shippingAddressAttention = Element.GetElement<TextField>("shippingAddressAttention");
            _shippingAddressAddress1 = Element.GetElement<TextField>("shippingAddressAddress1");
            _shippingAddressAddress2 = Element.GetElement<TextField>("shippingAddressAddress2");
            _shippingAddressAddress3 = Element.GetElement<TextField>("shippingAddressAddress3");
            _shippingAddressZip = Element.GetElement<TextField>("shippingAddressZip");
            _shippingAddressZipPlusFour = Element.GetElement<TextField>("shippingAddressZipPlusFour");
            _shippingAddressCity = Element.SelectList("shippingAddressCity");
            _shippingAddressCounty = Element.SelectList("shippingAddressCounty");
            _shippingAddressState = Element.SelectList("shippingAddressState");
        }

        public void EnterShippingAddreess(string address1, string address2, string zip, string zip4 = null, string attention = null, int? city = null, int? county = null, int? state = null)
        {
            _chkUseMainForShipping.CustomSetCheckBox(true);
            _shippingAddressAddress1.CustomSetTextQuicklyHelper(address1);
            _shippingAddressAddress2.CustomSetTextQuicklyHelper(address2);
            _shippingAddressZip.CustomSetTextQuicklyHelper(zip);
            if (zip4 != null)
                _shippingAddressZipPlusFour.CustomSetTextQuicklyHelper(zip4);
            if (attention != null)
                _shippingAddressAttention.CustomSetTextQuicklyHelper(attention);
            if (city != null)
                _shippingAddressCity.CustomSelectDropdownItem((int)city);
            if (county != null)
                _shippingAddressCounty.CustomSelectDropdownItem((int)county);
            if (state != null)
                _shippingAddressState.CustomSelectDropdownItem((int)state);
        }

        public void EnterShippingAddreess()
        {
            _chkUseMainForShipping.CustomSetCheckBox(false);
        }
    }
}
