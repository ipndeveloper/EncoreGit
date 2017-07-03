using WatiN.Core;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_SubNav_Control : Control<UnorderedList>
    {
        private Link _browseAccounts;
        private SearchSuggestionBox_Control _searchBox;
        private Link _enrollNewAccount;
        private GMP_Accounts_EnrollSubNav_Control _enrollAccount;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _browseAccounts = Element.GetElement<Link>((new Param("/Accounts/Browse", AttributeName.ID.Href, RegexOptions.None)));
            _searchBox = Control.CreateControl<SearchSuggestionBox_Control>(Util.Browser.GetElement<TextField>(new Param("txtSearch")));
            _enrollNewAccount = Element.GetElement<Link>((new Param("/Enrollment", AttributeName.ID.Href, RegexOptions.None)));
            _enrollAccount = Element.GetElement<Div>(new Param("DropDown", AttributeName.ID.ClassName)).As<GMP_Accounts_EnrollSubNav_Control>();
        }

        /// <summary>
        /// Click browse accounts link.
        /// </summary>
        /// <param name="noWait">If true click browse accounts link with no wait, else void.</param>
        public GMP_Accounts_Browse_Page ClickBrowseAccounts(int? timeout = null, int? delay = 2, bool pageRequired = true)
        {
            timeout = _browseAccounts.CustomClick(timeout);
            /*
             * Page appears to load and then update without user intervention
             * A second call to Wait for Complete is needed
             */
            timeout = Util.Browser.CustomWaitForComplete(timeout);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_EnrollType_Page ClickEnrollNewAccount(int? timeout = null, bool pageRequired = true)
        {
            timeout = _enrollNewAccount.CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_EnrollType_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_EnrollSubNav_Control EnrollNewAccount
        {
            get { return _enrollAccount; }
        }

        /// <summary>
        /// Search user Account.
        /// </summary>
        /// <param name="AccountName">Account name.</param>
        public GMP_Accounts_Overview_Page SearchAccount(string account, string match = null, bool exactMatch = false, int? timeout = null, bool pageRequired = true)
        {
            Thread.Sleep(2000);
            _searchBox.Select(account, match, exactMatch, timeout);
            return Util.GetPage<GMP_Accounts_Overview_Page>(timeout, pageRequired);
        }
    }
}
