using WatiN.Core;
using WatiN.Core.Extras;
using TestMasterHelpProvider;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Testing.Integration.DWS;
using NetSteps.Testing.Integration.GMP.Orders;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace NetSteps.Testing.Integration.GMP.Accounts
{
    public class GMP_Accounts_Overview_Page : GMP_Accounts_OverviewBase_Page
    {
        #region Controls.

        private Div hOneLoggedInUser;
        private Div tableUserDetails;
        private Para paraDistributorStatus;
        private TextField txtEnrolledDate;
        private TextField txtRenewalDate;
        private Table orderDetails;
        private Div divAutoShip;
        private Div divSubscription;
        private Table tableRowDetails;
        private Div _accountProxies;
        private Div divOverview;
        private Table tableAccountDetails;

        private List<string> accountDetails = new List<string>() { "Type", "Status", "Sponsor", "Enrolled" };

        protected override void InitializeContents()
        {
            base.InitializeContents();
            // Overview
            this.hOneLoggedInUser = Document.GetElement<Div>(new Param("fullNameYellowWidget"));
            this.tableUserDetails = Document.GetElement<Div>(new Param("Content", AttributeName.ID.ClassName));
            this.paraDistributorStatus = Document.GetElement<Para>(new Param("DistributorStatus", AttributeName.ID.ClassName));
            this.txtEnrolledDate = Document.GetElement<TextField>(new Param("enrollmentDate"));
            this.txtRenewalDate = Document.GetElement<TextField>(new Param("nextRenewal"));
            this.orderDetails = Document.GetElement<Table>(new Param("paginatedGrid"));
            this.divAutoShip = Document.GetElement<Div>(new Param("clr", AttributeName.ID.ClassName));
            this.divSubscription = Document.GetElement<Div>(new Param("clr", AttributeName.ID.ClassName).And(new Param(1)));
            this.tableRowDetails = Document.GetElement<Table>(new Param("tableRowDetails"));
            this._accountProxies = Document.GetElement<Div>(new Param("DistributorProxies", AttributeName.ID.ClassName));
            this.divOverview = Document.GetElement<Div>(new Param("SectionHeader", AttributeName.ID.ClassName));
            this.tableAccountDetails = Document.GetElement<Table>(new Param("DetailsTag Section", AttributeName.ID.ClassName));
        }

        #endregion

        #region Properties

         public override bool IsPageRendered()
        {
            return Document.GetElement<Div>(new Param("OrderHistoryPreview", AttributeName.ID.ClassName)).Exists;
        }

        #endregion

        #region Methods

        public bool ProxyLinksExist()
        {
            Link lnk1 = _accountProxies.GetElement<Link>(new Param(0));
            Link lnk2 = _accountProxies.GetElement<Link>(new Param(1));
            return (lnk1.Exists && lnk2.Exists);
        }

        public bool ProxyLinkExist()
        {
            Link lnk = _accountProxies.GetElement<Link>(new Param(0));
            return lnk.Exists;
        }

        public GMP_Accounts_SubscriptionEdit_Page ClickEditPWS(int? timeout = null, bool pageRequired = true)
        {
            timeout = _content.GetElement<Link>(new Param("/Accounts/Subscriptions/Edit/", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_SubscriptionEdit_Page>(timeout, pageRequired);
        }

        public string PWS
        {
            get { return GetPWSLink(); }
        }

        public string GetPWSLink()
        {
            string[] separators = { "//", "Login" };
            Link lnk = _accountProxies.GetElement<Link>(new Param(1));
            return lnk.Url.Split(separators, 3, StringSplitOptions.None)[1];
        }
        
        /// <summary>
        /// Get account number from Url.
        /// </summary>
        /// <returns>Account number.</returns>
        public string GetAccountNumber()
        {
            string Url = Util.Browser.Url;
            string accountNumber = Url.Substring(Url.LastIndexOf('/'), (Url.Length - Url.LastIndexOf('/'))).Remove(0, 1);

            return accountNumber;
        }

        new public GMP_Accounts_PoliciesChangeHistory_Page ClickPoliciesChangeHistory(int? timeout = null, bool pageRequired = true)
        {
            timeout = Document.GetElement<Link>(new Param("/Accounts/Overview/PoliciesChangeHistory", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Accounts_PoliciesChangeHistory_Page>(timeout, pageRequired);
        }

        /// <summary>
        /// Get logged in user name
        /// </summary>
        /// <returns>Logged in user.</returns>
        public string GetLoggedInUserName()
        {
            //this.tableUserDetails.CustomWaitUntilExist();
            return this.tableUserDetails.ElementsOfType<HeaderLevel1>()[0].CustomGetText();
        }

        /// <summary>
        /// Get user details.
        /// </summary>
        /// <returns>User details.</returns>
        public Dictionary<string, string> GetUserDetails()
        {
            TableRowCollection rows = tableAccountDetails.TableBody(Find.Any).TableRows;
            
            Dictionary<string, string> details = new Dictionary<string, string>();
            int maxIndex = rows.Count - 1;
            for (int index = 0; index <= maxIndex; index++)
            {
                    for (int i = 0; i < accountDetails.Count; i++)
			        {
                        if (!rows[index].CustomGetText().Contains("Sub-Status"))
                        {
                            if (!accountDetails[i].Equals("Sponsor"))
                            {
                                if (rows[index].CustomGetText().Contains(accountDetails[i]))
                                {
                                    if (rows[index].CustomGetText().Contains("Enrolled"))
                                    {
                                        details.Add(accountDetails[i], txtEnrolledDate.Value);
                                        break;
                                    }
                                    else
                                    {
                                        details.Add(accountDetails[i], rows[index].CustomGetText());
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (rows[index].CustomGetText().Contains("Advisor") || rows[index].CustomGetText().Contains("Upline") || rows[index].CustomGetText().Contains("Placement") || rows[index].CustomGetText().Contains("Mentor"))
                                {
                                    details.Add(accountDetails[i], rows[index].CustomGetText());
                                    break;
                                }
                            }
                        }
                        else
                            break;
			        }
            }

            return details;
        }

        [Obsolete("Use 'ProxyLink<TPage>(int, int?, bool)' or 'ReleaseNavigateProxyLink<TPage>(int, int?, bool)'", true)]
        public TPage ClickProxyLink<TPage>(int i = 0, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            if (!timeout.HasValue)
                timeout = Settings.AttachToBrowserTimeOut;
            Link lnk =_accountProxies.GetElement<Link>(new Param(i));
            lnk.CustomWaitForExist(timeout: 5);
            string[] separators = { "//", "Login" };
            // Just keep the portal. Remove the protocol, Login, and query
            string url = lnk.Url.Split(separators, 3, StringSplitOptions.None)[1];
            timeout = lnk.CustomClick(timeout);
            return Util.AttachBrowser<TPage>(url, timeout, pageRequired);            
        }

        [Obsolete("Use 'ProxyLink<TPage>(int, int?, bool)'", true)]
        public TPage NavigateProxyLink<TPage>(int i = 0, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            Link lnk = _accountProxies.GetElement<Link>(new Param(i));
            lnk.CustomWaitForExist(timeout: 5);
            return Util.Browser.Navigate<TPage>(lnk.Url, timeout, pageRequired);
        }

        [Obsolete("Use 'ReleaseNavigateProxyLink<TPage>(int, int?, bool)'", true)]
        public TPage ReleaseNavigateProxyLink<TPage>(int i = 0, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            Link lnk = _accountProxies.GetElement<Link>(new Param(i));
            lnk.CustomWaitForExist(timeout: 5);
            return Util.Browser.Navigate<TPage>(lnk.Url.Replace("https", "http"), timeout, pageRequired);
            //if (Util.CertPageExists())
            //    proxy = Util.ClickCertPageOverride<TPage>();
            //return proxy;
        }

        public TPage ReleaseProxyLink<TPage>(int i = 0, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            Link lnk = _accountProxies.GetElement<Link>(new Param(i));
            lnk.CustomWaitForExist(timeout: 5);
            return Util.InitBrowser<TPage>(lnk.Url.Replace("https", "http"), timeout, pageRequired);
        }

        public TPage ProxyLink<TPage>(int i = 0, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            Link lnk = _accountProxies.GetElement<Link>(new Param(i));
            lnk.CustomWaitForExist(timeout: 5);
            return Util.InitBrowser<TPage>(lnk.Url, timeout, pageRequired);
        }

        /// <summary>
        /// Validate logged in user first and last names under DWS.
        /// </summary>        
        /// <param name="name">Name to be validated</param>
        /// <returns>True if exists, else false.</returns>
        public bool ValidateDWSLoggedInUserDetails(string name)
        {
            // Validate the logged in user.
            bool result;
            Span username = Util.Browser.Span(Find.ByClass("UserID"));

            if (username.Exists)
            {
                result = username.CustomGetText().Contains(name);
            }
            else
            {
                result = false;
                Util.LogFail("Unable validate logged in user.");
            }

            Util.Browser.Close();
            Util.Browser = Util.newBrowser;

            return result;
        }

        /// <summary>
        /// Click on Order ID.
        /// </summary>
        /// <param name="orderId">Order ID.</param>
        public GMP_Orders_Details_Page ClickOnOrderId(string orderId, int? timeout = null, bool pageRequired = true)
        {
            timeout = Util.Browser.GetElement<Link>(new Param(orderId, AttributeName.ID.InnerText, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<GMP_Orders_Details_Page>(timeout, pageRequired);
        }

        /// <summary>
        /// Get order details.
        /// </summary>
        /// <param name="row">Row index.</param>
        /// <returns>Order details under account overview page.</returns>
        public List<string> GetOrderDetails(int row = 1)
        {
            return this.orderDetails.GetTableRowsData(row);
        }

        /// <summary>
        /// Validate auto ship order product details under account overview page.
        /// </summary>
        /// <param name="prodId">Product Id.</param>
        /// <param name="prodQuantity">Quantity.</param>
        /// <returns>True if exists, else false.</returns>
        public bool ValidateAutoshipOrderProdDetails(string prodId, string prodQuantity)
        {
            bool result = true;
            result = Util.Browser.GetElement<Div>(new Param("clr ml10", AttributeName.ID.ClassName)).CustomGetText().Contains(string.Format("{0} x {1}", prodQuantity, prodId));

            return result;
        }
        
        /// <summary>
        /// Validate auto ship order due details/status under account overview page.
        /// </summary>
        /// <param name="dateValue">Due date.</param>
        /// <returns>True if exists, else false.</returns>
        public bool ValidateAutoshipOrderDueDetails(string dateValue)
        {
            bool result = true;

            Span dueDateValue = Util.Browser.Span(Find.ByClass("ml10 FL"));
            if (dateValue.Contains("/"))
            {
                result = dueDateValue.CustomGetText().Contains(string.Format("Due {0}", dateValue));
            }
            else
            {
                result = dueDateValue.CustomGetText().Contains(dateValue);
            }

            return result;
        }

        #endregion

        #region Validation Methods

        /// <summary>
        /// Validate Order details under account overview page.
        /// </summary>
        /// <param name="orderType">Order type.</param>
        /// <param name="status">Status.</param>
        /// <param name="shipStaus">Shipping status.</param>
        /// <param name="subTotal">Sub total.</param>
        /// <param name="row">Row index.</param>
        public bool ValidateOrderDetailsUnderAccountOverview(string orderType, string status, string shipStaus, string subTotal, int row = 1)
        {
            bool result = true;

            List<string> orderDetails = this.GetOrderDetails(row);

            if(!string.IsNullOrEmpty(orderDetails[1]))
            {
                string date = DateTime.Parse(orderDetails[1]).ToString("M/dd/yyyy");
                if (!Utils.AssertIsTrue((date.Contains(DateTime.UtcNow.AddHours(Util.UtcTimeDiff).ToString("M/dd/yyyy"))) ||
                    (date.Contains(DateTime.Now.ToString("M/dd/yyyy"))),
                string.Format("Date of order '{0}' is properly shown under account overview.", date))) result = false;
            }
            
            if (!Utils.AssertIsTrue(orderDetails[2].Contains(orderType),
                string.Format("Order type '{0}' is properly shown under account overview.", orderType))) result = false;
            if (!Utils.AssertIsTrue(orderDetails[3].Contains(status),
                string.Format("Order status '{0}' properly shown under account overview.", status))) result = false;
            if (!Utils.AssertIsTrue(orderDetails[4].Contains(shipStaus),
                string.Format("Order shipping status '{0}' properly shown under account overview.", shipStaus))) result = false;
            if (!Utils.AssertIsTrue(orderDetails[5].Contains(subTotal),
                string.Format("Order amount '{0}' is properly shown under account overview.", subTotal))) result = false;

            return result;
        }

         /// <summary>
        /// Validate Autoship Order and due details under account overview page.
        /// </summary>
        /// <param name="prodId">Product Id.</param>
        /// <param name="prodQty">Product Quantity.</param>
        /// <param name="dueDate">Due Date.</param>
        /// <returns>True if all validations are passed, else false.</returns>
        public bool ValidateAutoshipOrderDetailsUnderAccountOverview(string prodId, string prodQty, DateTime? dueDate)
        {
            bool result = true;
            string date = string.Empty;

            if (dueDate != null)
            {
                if (dueDate.Value.Day >= 28)
                {
                    date = string.Concat((dueDate.Value.Month + 1).ToString(), "/28", dueDate.Value.ToString("/yyyy"));
                }
                else
                {
                    date = string.Concat((dueDate.Value.Month + 1).ToString(), "/", dueDate.Value.Day.ToString(), dueDate.Value.ToString("/yyyy"));
                }
            }
            else
            {
                date = "Account Inactive";
            }

            if (!Utils.AssertIsTrue(this.ValidateAutoshipOrderProdDetails(prodId, prodQty),
                string.Format("Autoship Product details '{0} x {1}' is properly shown under account overview.", prodQty, prodId))) result = false;
            if (!Utils.AssertIsTrue(this.ValidateAutoshipOrderDueDetails(date),
                string.Format("Autoship Product Order due date/status '{0}' is properly shown under account overview.", date))) result = false;

            return result;
        }

        public bool ValidateAccountOverviewDetails(GMP_Accounts_Account_Control account)
        {
            bool valid = true;
            
            // Validate account over view details.
            if (!Compare.CustomCompare(this.GetLoggedInUserName(), CompareID.Contains, account.FirstName, "User first name"))
                valid = false;
            if (!Compare.CustomCompare(this.GetLoggedInUserName(), CompareID.Contains, account.LastName, "User last name"))
                valid = false;
            if (!Compare.CustomCompare(paraDistributorStatus.CustomGetText(), CompareID.Contains, ("#" + account.Account), "Account number"))
                valid = false;

            // Account overview under yellow widget.
            Dictionary<string, string> userDetails = this.GetUserDetails();

            if (!Compare.CustomCompare(userDetails["Type"], CompareID.Contains, account.Type, "Type"))
                valid = false;
            if (!Compare.CustomCompare(userDetails["Status"], CompareID.Contains, account.Status, "Status")) 
                valid = false;
            
            //this is temporay until they fix the bug on not having a sponsor on the accounts browse page
            //if (string.IsNullOrEmpty(account.Sponsor))
            //    sponsor = "NONE";
            //else
            //    sponsor = SponsorNameSplit(account.Sponsor);
            if (string.IsNullOrEmpty(userDetails["Sponsor"]))
                valid = false;
            if (string.IsNullOrEmpty(userDetails["Enrolled"]))
                valid = false;
            //if (!Compare.CustomCompare(userDetails["Sponsor"], CompareID.Contains, sponsor, "Sponsor")) 
            //    valid = false;
            //if (!Compare.CustomCompare(userDetails["Enrolled"], CompareID.Contains, account.Enrolled, "Date enrolled")) 
            //    valid = false;

            return valid;
        }

        public string SponsorNameSplit(string sponsorName)
        {
            char splitSponsor = '(';

            string[] sponsor = sponsorName.Split(splitSponsor);

            return sponsor[0].Trim();
        }

        #endregion        
    }
}