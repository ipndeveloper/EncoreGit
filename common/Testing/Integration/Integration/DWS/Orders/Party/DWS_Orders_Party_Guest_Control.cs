using WatiN.Core;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Orders.Party
{
    public class DWS_Orders_Party_Guest_Control : Control<Div>
    {
        SearchSuggestionBox_Control _firstName;
        private TextField _lastName, _email;
        private CheckBox _directShip;
        private Div _shipAddress;
        private Address_Control _address;

        protected override void  InitializeContents()
        {
 	        base.InitializeContents();
            _firstName = Element.GetElement<TextField>(new Param("txtFirstName", AttributeName.ID.Id, RegexOptions.None)).As<SearchSuggestionBox_Control>();
            _lastName = Element.GetElement<TextField>(new Param("txtLastName", AttributeName.ID.Id, RegexOptions.None));
            _email = Element.GetElement<TextField>(new Param("txtEmail", AttributeName.ID.Id, RegexOptions.None));
            _directShip = Element.GetElement<CheckBox>(new Param("directShip", AttributeName.ID.ClassName));
            _shipAddress = Element.GetElement<Div>(new Param("DirectShipAddress", AttributeName.ID.ClassName));
            _address = _shipAddress.As<Address_Control>();
        }

        public Address_Control Address
        {
            get { return _address; }
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

        public bool DirectShip
        {
            get { return _directShip.CustomChecked(); }
            set
            {
                _directShip.CustomSetCheckBox(true);
            }
        }

        public void EnterGuest(RetailCustomer customer)
        {
            if (FirstName.Select(customer.FirstName, string.Format("{0} {1}", customer.FirstName, customer.LastName)))
            {
                do
                {
                    Thread.Sleep(1000);
                } while (string.IsNullOrEmpty(Email));
                if (Email != customer.Email)
                {
                    LastName = customer.LastName;
                    Email = customer.Email;
                }
            }
            else
            {
                _lastName.Focus();
                LastName = customer.LastName;
                Email = customer.Email;
            }
            if (customer.MainAddress != null)
            {
                DirectShip = true;
                _shipAddress.CustomRunScript(Util.strShow);
                Address.EnterAddress(customer.MainAddress);
            }
        }
    }
}
