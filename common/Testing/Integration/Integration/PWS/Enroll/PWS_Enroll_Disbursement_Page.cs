using WatiN.Core;
using NetSteps.Testing.Integration.GMP.Accounts;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Disbursement_Page : PWS_Base_Page
    {
        private RadioButton _useCheck, _useEFT;
        private CheckBox _useMainAddress, _eSignnature, _hardRelease, _eftEnable1, _eftEnable2;
        private Address_Control _address;
        private Link _next;
        private ElementCollection<Div> _efts;
        private TextField _signature;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _useCheck = _content.GetElement<RadioButton>(new Param("rbPaymentCheck"));
            _useEFT = _content.GetElement<RadioButton>(new Param("rbPaymentEFT"));
            _useMainAddress = _content.GetElement<CheckBox>(new Param("chkUseAddressOfRecord"));
            _address = _content.GetElement<Div>(new Param("FormContainer", AttributeName.ID.ClassName)).As<Address_Control>();
            _next = _content.GetElement<Link>(new Param("btnNext"));
            _efts = _content.GetElements<Div>(new Param("FL mt10 mr10", AttributeName.ID.ClassName));
            _eSignnature = _content.GetElement<CheckBox>(new Param("chkAgreement"));
            _signature = _content.GetElement<TextField>(new Param("txtName"));
            _hardRelease = _content.GetElement<CheckBox>(new Param("chkHardRelease"));
            _eftEnable1 = _content.GetElement<CheckBox>(new Param("chkEnabledAccount1"));
            _eftEnable2 = _content.GetElement<CheckBox>(new Param("chkEnabledAccount2"));
        }

        public override bool IsPageRendered()
        {
            return (_signature.Exists || _hardRelease.Exists);
        }

        public bool CheckSignature
        {
            get { return _eSignnature.CustomChecked(); }
            set { _eSignnature.CustomSetCheckBox(value); }
        }

        public string ESignature
        {
            get { return _signature.CustomGetText(); }
            set { _signature.CustomSetTextQuicklyHelper(value); }
        }

        public bool UseCheckDisbursement
        {
            get { return _useCheck.Checked; }
            set
            {
                if (value)
                    _useCheck.CustomSelectRadioButton();
                else
                    _useEFT.CustomSelectRadioButton();
            }
        }

        public void EnableEFT(int eftcheckbox)
        {
            if (eftcheckbox == 1)
                _eftEnable1.Check();
            else
                _eftEnable2.Check();
        }

        public bool UseMainAddress
        {
            get { return _useMainAddress.CustomChecked(); }
            set { _useMainAddress.CustomSetCheckBox(value); }
        }

        public Address_Control CheckAddress
        {
            get { return _address; }
        }

        public EFT_Control GetEFT(int index)
        {
            _useEFT.CustomSelectRadioButton();
            return _efts[index].As<EFT_Control>();
        }

        public PWS_Enroll_PersonalWebsite_Page ClickNext(int? timeout = null, bool pageRequired = true)
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<PWS_Enroll_PersonalWebsite_Page>(timeout, pageRequired);
        }

    }
}
