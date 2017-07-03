using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Page : GMP_Accounts_Base_Page
    {
        private Link _advancedGo;
        private SearchSuggestionBox_Control _searchBox;
        private SelectList _accountStatus;
        private SelectList _accountType;
        private SelectList _accountTitle;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _advancedGo = Document.GetElement<Link>(new Param("btnAdvancedGo"));
            _searchBox = Control.CreateControl<SearchSuggestionBox_Control>(Document.GetElement<TextField>(new Param("txtSearch")));
            _accountStatus = Document.GetElement<SelectList>(new Param("sStatus"));
            _accountType = Document.GetElement<SelectList>(new Param("sAccountType"));
            _accountTitle = Document.GetElement<SelectList>(new Param("txtTitle"));
        }

        /// <summary>
        /// Search user Account.
        /// </summary>
        /// <param name="AccountName">Account name.</param>
        public GMP_Accounts_Overview_Page SearchAccount(string search, string select = null, int? timeout = null, bool pageRequired = true)
        {
            _searchBox.Select(search, select);
            return Util.GetPage<GMP_Accounts_Overview_Page>(timeout, pageRequired);
        }

         public override bool IsPageRendered()
        {
            return _advancedGo.Exists;
        }
    }
}
