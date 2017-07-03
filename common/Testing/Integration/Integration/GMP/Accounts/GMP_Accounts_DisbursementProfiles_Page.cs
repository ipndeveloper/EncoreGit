using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_DisbursementProfiles_Page : GMP_Accounts_Section_Page
    {
        private Link _save;
        private RadioButton _payByCheck, _payByEft;
        private CheckBox _useMainAddress;
        private Address_Control _checkAddress;
        private ElementCollection<Div> _efts;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _save = Document.GetElement<Link>(new Param("btnSave"));
            _payByCheck = _content.GetElement<RadioButton>(new Param("rbPaymentCheck"));
            _payByEft = _content.GetElement<RadioButton>(new Param("rbPaymentEFT"));
            _useMainAddress = _content.GetElement<CheckBox>(new Param("chkUseAddressOfRecord"));
            _checkAddress = _content.GetElement<Div>(new Param("check")).As<Address_Control>();
            _efts = _content.GetElement<Div>(new Param("eft")).GetElements<Div>(new Param("FL", AttributeName.ID.ClassName, RegexOptions.None));
        }

         public override bool IsPageRendered()
        {
            return _save.Exists;
        }

        public bool PayByCheck
        {
            get { return _payByCheck.Checked; }
            set { _payByCheck.CustomSelectRadioButton(value); }
        }

        public bool PayByEFT
        {
            get { return _payByEft.Checked; }
            set { _payByEft.CustomSelectRadioButton(value); }
        }

        public bool UseMainAddress
        {
            get { return _useMainAddress.CustomChecked(); }
            set { _useMainAddress.CustomSetCheckBox(value); }
        }

        public Address_Control CheckAddress
        {
            get { return _checkAddress; }
        }

        public EFT_Control GetEft(int index)
        {
            return _efts[index].As<EFT_Control>();
        }
    }
}
