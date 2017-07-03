using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Account_Control : Control<TableRow>
    {
        private string _account, _firstName, _lastName, _type, _status, _enrolled, _email, _sponsor, _location;
        private string _accountOverview;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            Element.CustomWaitForExist();
            Link lnk = Element.GetElement<TableCell>(new Param(0)).GetElement<Link>();
            _accountOverview = lnk.Url;
            _account = lnk.CustomGetText();
            _firstName = Element.GetElement<TableCell>(new Param(1)).CustomGetText();
            _lastName = Element.GetElement<TableCell>(new Param(2)).CustomGetText();
            _type = Element.GetElement<TableCell>(new Param(3)).CustomGetText();
            _status = Element.GetElement<TableCell>(new Param(4)).CustomGetText();
            _enrolled = Element.GetElement<TableCell>(new Param(5)).CustomGetText();
            _email = Element.GetElement<TableCell>(new Param(6)).CustomGetText();
            _sponsor = Element.GetElement<TableCell>(new Param(7)).CustomGetText();
            _location = Element.GetElement<TableCell>(new Param(8)).CustomGetText();
        }

        public string Account
        {
            get { return _account; }
        }

        public string FirstName
        {
            get { return _firstName; }
        }

        public string LastName
        {
            get { return _lastName; }
        }

        public string Type
        {
            get { return _type; }
        }

        public string Status
        {
            get { return _status; }
        }

        public string Enrolled
        {
            get { return _enrolled; }
        }

        public string Email
        {
            get { return _email; }
        }

        public string Sponsor
        {
            get { return _sponsor; }
        }

        public string Location
        {
            get { return _location; }
        }

        public GMP_Accounts_Overview_Page SelectAccount(int? timeout = null, bool pageRequired = true)
        {
            return Util.Browser.Navigate<GMP_Accounts_Overview_Page>(_accountOverview, timeout, pageRequired);
        }
    }
}
