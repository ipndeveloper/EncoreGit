using System;
using System.Threading;
using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_SetUp_Page : DWS_Orders_Base_Page
    {
        private SearchSuggestionBox_Control _firstName;
        private TextField _lastName, _email, _partyName;
        private Phone_Control _phone;
        private Address_Control _address;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _firstName = _content.GetElement<TextField>(new Param("txtFirstName")).As<SearchSuggestionBox_Control>();
            _lastName = _content.GetElement<TextField>(new Param("txtLastName"));
            _email = _content.GetElement<TextField>(new Param("txtEmail"));
            _phone = _content.GetElement<Div>(new Param("phone")).As<Phone_Control>();
            _address = _content.GetElement<Div>(new Param("FormContainer", AttributeName.ID.ClassName)).As<Address_Control>();
            _partyName = _content.GetElement<TextField>(new Param("txtPartyName"));
        }

        public override bool IsPageRendered()
        {
            return _partyName.Exists;
        }

        public SearchSuggestionBox_Control FirstName
        {
            get { return _firstName; }
        }

        public string LastName
        {
            get { return _lastName.CustomGetText(); }
            set { _lastName.CustomSetTextQuicklyHelper(value); }
        }

        public string Email
        {
            get { return _email.CustomGetText(); }
            set { _email.CustomSetTextQuicklyHelper(value); }
        }

        public Address_Control Address
        {
            get { return _address; }
        }

        public void EnterPhone(Phone phone)
        {
            _phone.Country = phone.Country;
            _phone.PhoneNumber = phone.PhoneNumber;
        }

        public void EnterHost(RetailCustomer host)
        {
            if (FirstName.Select(host.FirstName, String.Format("{0} {1}", host.FirstName, host.LastName)))
            {
                do
                {
                    Thread.Sleep(1000);
                } while (string.IsNullOrEmpty(Email));

                if (Email != host.Email)
                {
                    Email = host.Email;
                    EnterPhone(host.Phones.GetPhone(0));
                    Address.ConfigureAddressControl(true, false, true, true).EnterAddress(host.MainAddress);
                }
            }
            else
            {
                _lastName.Focus();
                LastName = host.LastName;
                Email = host.Email;
                EnterPhone(host.Phones.GetPhone(0));
                Address.ConfigureAddressControl(true, false, true, true).EnterAddress(host.MainAddress);
                Thread.Sleep(5000); //I've not been able to time this
            }
            Util.Browser.CustomWaitForSpinners();
        }

        public string PartyName
        {
            get { return _partyName.CustomGetText(); }
            set { _partyName.CustomSetTextQuicklyHelper(value); }
        }

        public bool UseEvites
        {
            set { _content.GetElement<CheckBox>(new Param("chkUseEvites")).CustomSetCheckBox(value); }
        }

        public DWS_Orders_Party_Cart_Page ClickContinue(int? timeout = null, bool pageRequired = true)
        {
            timeout = Util.Browser.CustomWaitForComplete(timeout);
            timeout = Document.GetElement<Link>(new Param("btnNext")).CustomClick(timeout);
            timeout = Document.CustomWaitForSpinners(timeout);
            return Util.GetPage<DWS_Orders_Party_Cart_Page>(timeout, pageRequired);
        }
    }
}