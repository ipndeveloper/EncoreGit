using WatiN.Core;
using System;
using System.Text.RegularExpressions;
using System.Threading;

using System.Collections.Generic;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Browse_Page : GMP_Accounts_Base_Page
    {
        private Pagination_Control _pagination;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _pagination = Document.GetElement<Div>(new Param("paginatedGridPagination")).As<Pagination_Control>();
        }

        #region Properties

         public override bool IsPageRendered()
        {
            return Document.GetElement<TextField>(new Param("sponsorInputFilter")).Exists; //Title.Contains("Browse Accounts");
        }

        public Pagination_Control Pagination
        {
            get { return _pagination; }
        }

        #endregion Properties

        #region Methods

        public GMP_Accounts_Browse_Page SelectStatus(AccountStatus.ID accountStatusID, int? timeout = null, bool pageRequired = true)
        {
            timeout = _content.SelectList("statusSelectFilter").CustomSelectDropdownItem(accountStatusID.ToPattern(), timeout);
            Thread.Sleep(2000);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Browse_Page SelectType(string customerType, int? timeout = null, bool pageRequired = true)
        {
            timeout = _content.SelectList("typeSelectFilter").CustomSelectDropdownItem(customerType, timeout);
            Thread.Sleep(2000);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Browse_Page SelectCountry(Country.ID country, int? timeout = null, bool pageRequired = true)
        {
            timeout = _content.SelectList("countrySelectFilter").CustomSelectDropdownItem(country.ToPattern(), timeout);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Browse_Page EnterStartDate(string startDate, int? timeout = null, bool pageRequired = true)
        {
            _content.GetElement<TextField>(new Param("startDateInputFilter")).CustomSetTextQuicklyHelper(startDate);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Browse_Page EnterEndDate(string endDate, int? timeout = null, bool pageRequired = true)
        {
            _content.GetElement<TextField>(new Param("endDateInputFilter")).CustomSetTextQuicklyHelper(endDate);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Browse_Page EnterEmail(string email, int? timeout = null, bool pageRequired = true)
        {
            _content.GetElement<TextField>(new Param("emailInputFilter")).CustomSetTextQuicklyHelper(email);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Browse_Page SelectAccount(string account, int? timeout = null, bool pageRequired = true)
        {
            _content.GetElement<TextField>(new Param("accountNumberOrNameInputFilter")).CustomSetTextQuicklyHelper(account);
            return Util.GetPage<GMP_Accounts_Browse_Page>(timeout, pageRequired);
        }

        public GMP_Accounts_Browse_Page ClickApplyFilter(int? timeout = null, int? delay = 2, bool pageRequired = true)
        {
            timeout = _content.GetElement<Link>(new Param("Button ModSearch filterButton", AttributeName.ID.ClassName)).CustomClick(timeout);
            return this;
        }

        private TableRowCollection GetTable(int? timeout = null, int? delay = 2)
        {
            Table tbl = Document.GetElement<Table>(new Param("paginatedGrid"));
            tbl.CustomWaitForSpinner(timeout, delay);
            Thread.Sleep(1000);
            return tbl.TableBody(Find.Any).TableRows;
        }

        public List<GMP_Accounts_Account_Control> GetAccounts(int minIndex = 1, int? maxIndex = null, int? timeout = null)
        {
            TableRowCollection rows = GetTable();
            List<GMP_Accounts_Account_Control> accounts = new List<GMP_Accounts_Account_Control>();
            if (!maxIndex.HasValue)
                maxIndex = rows.Count - 1;
            for (int index = minIndex; index <= maxIndex; index++)
            {
                accounts.Add(rows[index].As<GMP_Accounts_Account_Control>());
            }
            return accounts;
        }

        public GMP_Accounts_Account_Control GetAccount(int? index = null, int minIndex = 1, int? maxIndex = null)
        {
            TableRowCollection accounts = GetTable();
            if (!maxIndex.HasValue)
                maxIndex = accounts.Count - 1;
            if (!index.HasValue)
            {
                index = Util.GetRandom(minIndex, (int)maxIndex);
            }
            GMP_Accounts_Account_Control account = accounts[(int)index].As<GMP_Accounts_Account_Control>();
            return account;
        }

        #endregion Methods
    }
}
