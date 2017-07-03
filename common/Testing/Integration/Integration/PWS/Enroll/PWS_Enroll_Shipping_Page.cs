using WatiN.Core;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Shipping_Page : PWS_Base_Page
    {
        private RadioButton _useMainAddress, _useOtherAddress;
        private Address_Control _shippingAddress;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _useMainAddress = _content.GetElement<RadioButton>(new Param("chkIsSameShippingAddressTrue"));
            _useOtherAddress = _content.GetElement<RadioButton>(new Param("chkIsSameShippingAddressFalse"));
        }

         public override bool IsPageRendered()
        {
            return _useOtherAddress.Exists;
        }

        public bool UseMainAddress
        {
            get { return _useMainAddress.Checked; }
            set
            {
                if (value)
                    _useMainAddress.CustomSelectRadioButton();
                else
                    _useOtherAddress.CustomSelectRadioButton();
            }
        }

        public Address_Control Address
        {
            get
            {
                if (_shippingAddress == null)
                {
                    _shippingAddress = _content.GetElement<Div>(new Param("ShippingAddress")).As<Address_Control>();
                }
                return _shippingAddress;
            }
        }

        public TPage ClickNext<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            timeout = _content.GetElement<Link>(new Param("btnSubmit")).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }

    }
}
