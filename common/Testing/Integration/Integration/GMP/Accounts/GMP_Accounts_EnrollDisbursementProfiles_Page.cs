using WatiN.Core;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    /// <summary>
    /// Class related to Disbursement profile page.
    /// </summary>
    public class GMP_Accounts_EnrollDisbursementProfiles_Page : GMP_Accounts_Base_Page
    {
        private UnorderedList _disbursement;
        private ListItem _payByCheckheck, _payByEft;
        Address_Control _checkAddress;
        ElementCollection<Div> _efts;
        private CheckBox _useMainAddress;
        private Link _next, _skip;
        private Para _enroll;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _disbursement = _content.GetElement<UnorderedList>(new Param("PaymentMethods"));
            _disbursement.CustomWaitForExist();
            _payByCheckheck = _disbursement.GetElement<ListItem>(new Param("check", "rel"));
            _payByEft = _disbursement.GetElement<ListItem>(new Param("eft", "rel"));
            _checkAddress = _content.GetElement<Div>(new Param("check")).As<Address_Control>();
            _useMainAddress = _content.GetElement<CheckBox>(new Param("chkUseAddressOfRecord"));
            _efts = _content.GetElement<Div>(new Param("eft")).GetElements<Div>();
            _enroll = _content.GetElement<Para>(new Param("Enrollment SubmitPage", AttributeName.ID.ClassName));
            if (_enroll.GetElement<Link>(new Param("btnDPNext")).Exists)
                _next = _enroll.GetElement<Link>(new Param("btnDPNext"));
            else
                _next = _enroll.GetElement<Link>(new Param("btnNext"));
            if (_enroll.GetElement<Link>(new Param("btnDPSkip")).Exists)
                _skip = _enroll.GetElement<Link>(new Param("btnDPSkip"));
            else
                _skip = _enroll.GetElement<Link>(new Param("btnSkip"));

        }
         public override bool IsPageRendered()
        {
            return _disbursement.Exists;
        }

         public GMP_Accounts_EnrollDisbursementProfiles_Page ClickPayByCheck(int? timeout = null)
        {
            _payByCheckheck.CustomClick(timeout);
            return this;
        }

        public Address_Control CheckAddress
        {
            get { return _checkAddress; }
        }

        public GMP_Accounts_EnrollDisbursementProfiles_Page UseMainAddress(bool value)
        {
            _useMainAddress.CustomSetCheckBox(value);
            return this;
        }

        public GMP_Accounts_EnrollDisbursementProfiles_Page ClickPayByEft(int? timeout = null)
        {
            _payByEft.CustomClick(timeout);
            return this;
        }

        public EFT_Control GetEFT(int index)
        {
            return _efts[index].As<EFT_Control>();
        }


        public GMP_Accounts_EnrollPWS_Page ClickNext(int? timeout = null, bool pageRequired = true)
        {
            timeout = _next.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_EnrollPWS_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_EnrollPWS_Page ClickSkip(int? timeout = null, bool pageRequired = true)
        {
            timeout = _skip.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_EnrollPWS_Page>(timeout, pageRequired);
        }

        public void CommissionsCheck(int? timeout = null)
        {
            _payByCheckheck.CustomClick(timeout);
        }
    }
}