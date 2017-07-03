using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Text;
using System.Web.Configuration;
using System.Web.Mvc;
using DistributorBackOffice.Helpers.Performance;
using DistributorBackOffice.Models.Performance;
using Fasterflect;
using NetSteps.Accounts.Downline.UI.Common;
using NetSteps.Accounts.Downline.UI.Common.InfoCard;
using NetSteps.Common.Attributes;
using NetSteps.Common.Configuration;
using NetSteps.Common.Dynamic;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Common.Serialization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Encore.Core.IoC;
using NetSteps.Foundation.Common;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.ActionResults;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;
using Newtonsoft.Json;
using Account = NetSteps.Data.Entities.Account;
using NetSteps.Commissions.Common;
using NetSteps.Commissions.Common.Models;
using DistributorBackOffice.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Collections;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DistributorBackOffice.Controllers
{
    public class PerformanceController : AccountReportController<DownlineReportParameters>
    {
        private ICommissionsService _commissionsService = Create.New<ICommissionsService>();

        public static class Functions
        {
            public const string Performance = "Performance";
            public const string PerformanceTreeView = "Performance-Tree View";
            public const string PerformanceDownlineManagement = "Performance-Downline Management";
            public const string PerformanceFlatDownline = "Performance-Flat Downline";
            public const string PerformanceExportExcel = "Performance-Export Excel";
            public const string PerformanceEmailDownline = "Performance-Email Downline";
            public const string PerformanceEmailAllDownline = "Performance-Email All Downline";
            public const string PerformanceOverview = "Performance-Performance Overview";
            public const string PerformanceSponsorSearch = "Performance-Sponsor Search";
            public const string PerformanceGraphicalDownline = "Performance-Graphical Downline";
        }

        #region Members
        private static object _lock = new object();
        protected static Dictionary<string, string> _columnHeaders = null;

        private readonly Lazy<IDownlineUIService> _downlineUIServiceFactory = new Lazy<IDownlineUIService>(Create.New<IDownlineUIService>);
        public virtual IDownlineUIService DownlineUIService { get { return _downlineUIServiceFactory.Value; } }

        /// <summary>
        /// The number of tree levels to include in the initial page load.
        /// Note: The jstree plugin seems to work best when it only receives one level at a time.
        /// </summary>
        public virtual int InitialTreeLevels { get { return 1; } }

        /// <summary>
        /// The number of tree levels to include in AJAX drill-downs.
        /// Note: The jstree plugin seems to work best when it only receives one level at a time.
        /// </summary>
        public virtual int DrillDownTreeLevels { get { return 1; } }
        #endregion

        #region Properties
        public override Constants.AccountReportType AccountReportType
        {
            get
            {
                return NetSteps.Data.Entities.Generated.ConstantsGenerated.AccountReportType.DownlineReport;
            }
        }

        public GenealogyHelpers GenealogyHelper
        {
            get
            {
                return new GenealogyHelpers();
            }
        }

        public int StartLevel { get; set; }

        public int MaxLevel { get; set; }

        public bool ShowPCs { get; set; }

        public bool ShowVolumeWidget { get; set; }

        public bool ShowTitleProgressWidget { get; set; }

        public bool ShowPerformanceOverviewWidget { get; set; }


        protected Dictionary<string, Func<Account, string, string>> _reportCustomColumnFunctions;
        public Dictionary<string, Func<Account, string, string>> ReportCustomColumnFunctions
        {
            get
            {
                if (_reportCustomColumnFunctions == null)
                    InitializeReportCustomColumnFunctions();
                return _reportCustomColumnFunctions;
            }
        }

        protected virtual void InitializeReportCustomColumnFunctions()
        {
            _reportCustomColumnFunctions = new Dictionary<string, Func<Account, string, string>>();
        }
        #endregion

        [OutputCache(CacheProfile = "DontCache")]
        [AnyFunctionFilter("~/", Constants.SiteType.BackOffice,
            Functions.Performance,
            Functions.PerformanceTreeView,
            Functions.PerformanceDownlineManagement,
            Functions.PerformanceFlatDownline,
            Functions.PerformanceExportExcel,
            Functions.PerformanceEmailDownline,
            Functions.PerformanceEmailAllDownline,
            Functions.PerformanceOverview,
            Functions.PerformanceSponsorSearch,
            Functions.PerformanceGraphicalDownline)]
        public virtual ActionResult Index(int? accountReportID = null)
        {
            var currentUser = ApplicationContext.Instance.CurrentAccount.User;
            if (!currentUser.HasFunction(Functions.PerformanceOverview))
            {
                if (currentUser.HasFunction(Functions.PerformanceTreeView)) return RedirectToAction("TreeView");
                else if (currentUser.HasFunction(Functions.PerformanceDownlineManagement)) return RedirectToAction("DownlineManagement");
                else if (currentUser.HasFunction(Functions.PerformanceFlatDownline)) return RedirectToAction("FlatDownline");
                else if (currentUser.HasFunction(Functions.PerformanceGraphicalDownline)) return RedirectToAction("GraphicalDownline");
                /*	
                 * Pass over the following and redirect to home because
                 * we cannot suport redirecting to them from here:
                 *	 Functions.PerformanceExportExcel
                 *	 Functions.PerformanceEmailDownline
                 *	 Functions.PerformanceEmailAllDownline
                 *	 Functions.PerformanceSponsorSearch
                 */
                else return Redirect("~/");
            }
            var model = new PerformanceOverviewViewModel();
            model.ShowPerformanceVolumeWidget = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.ShowPerformanceVolumeWidget, true);
            model.ShowPerformanceTitleProgressionWidget = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.ShowPerformanceTitleProgressionWidget, true);
            model.UseAdvancedPerformanceTitleProgressionWidget = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseAdvancedPerformanceTitleProgressionWidget, false);
            //model.ShowPerformanceEarningsWidget = ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.ShowPerformanceEarningsWidget, true);
            //model.ShowPerformanceQuickStartWidget = ConfigurationManager.GetAppSetting(ConfigurationManager.VariableKey.ShowPerformanceQuickStartWidget, true);
            ViewData["SelectedReport"] = accountReportID.HasValue ? accountReportID.Value.ToString() : "NA";

            try
            {
                // Reset report if 0 is passed in. - JHE
                if (accountReportID == 0)
                {
                    CurrentReportParameters = GetDefaultReportParameters(CurrentReportParameters.PeriodID);
                    return RedirectToAction("FlatDownline");
                }

                if (accountReportID != null && accountReportID > 0)
                {
                    // TODO: Check that the report belongs to the current account if the not a 'base' report. - JHE
                    var accountReport = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID.ToInt());
                    if (accountReport != null)
                    {
                        CurrentReportParameters = BinarySerializationHelper.Deserialize<DownlineReportParameters>(accountReport.Data);
                        CurrentReportParameters.AccountReportID = accountReportID;

                        ViewData["CurrentAccountReport"] = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);
                    }
                    //Corporate Reports - SCC
                    var corporateReport = CorporateAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID.ToInt());
                    if (corporateReport != null)
                    {
                        CurrentReportParameters = BinarySerializationHelper.Deserialize<DownlineReportParameters>(corporateReport.Data);
                        CurrentReportParameters.AccountReportID = accountReportID;
                        CurrentReportParameters.PeriodID = _commissionsService.GetCurrentPeriod().PeriodId;

                        ViewData["CorporateAccountReport"] = CorporateAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);
                    }
                }

                ReportResults results = GetReportResults(CurrentReportParameters);
                SetColumnNameLookups(results.Downline);
                SetDefaultVisibleColumns(results.Downline, null);

                ViewData["ColumnHeaders"] = _columnHeaders;
                ViewData["CurrentAccountReports"] = CurrentAccountReports.Where(r => r.AccountReportTypeID == Constants.AccountReportType.DownlineReport.ToShort()).ToList();
                ViewData["CurrentReportParameters"] = CurrentReportParameters;
                ViewData["CorporateAccountReports"] = CorporateAccountReports.Where(r => r.AccountReportTypeID == Constants.AccountReportType.DownlineReport.ToShort()).ToList();
                ViewData["LastUpdated"] = results.Downline.LastUpdated;

                // Get title names and codes for use in the KPI charts
                var titles = Title.ListTitles(new NetSteps.Common.Base.FilterDateRangePaginatedListParameters<NetSteps.Data.Entities.Business.HelperObjects.SearchData.TitleSearchData>()
                {
                    PageIndex = 0,
                    PageSize = 10000,
                    OrderBy = string.Empty,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
                });

                ViewBag.TitleDataJson = MvcHtmlString.Create(
                       JsonConvert.SerializeObject(
                    //_commissionsService.GetTitles()
                              titles
                    //.Where(t => t.Active)
                              .OrderBy(t => t.SortOrder)
                              .Select(t => new
                              {
                                  key = t.TitleID,
                                  value = new
                                  {
                                      code = t.TitleCode,
                                      name = Translation.GetTerm(t.TermName, t.ClientName)
                                  }
                              })
                              .ToArray()
                       )
                );

                ViewData["contextCore"] = string.Format("Core Culture: {0} decimal: {1}", CoreContext.CurrentCultureInfo.Name, CoreContext.CurrentCultureInfo.NumberFormat.NumberDecimalSeparator);
                ViewData["contextCurrent"] = string.Format("Current System Curlture: {0} decimal: {1}", System.Globalization.CultureInfo.CurrentCulture.Name, System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                GetCustomWidgetData(CurrentAccount.AccountID);
                model.ReportResults = results;
                return View(model);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        protected virtual void GetCustomWidgetData(int accountId)
        {

        }

        [OutputCache(Duration = 60, VaryByParam = "query")]
        [FunctionFilter(Functions.Performance, "~/", Constants.SiteType.BackOffice)]
        public virtual ActionResult SearchDownline(string query)
        {
            try
            {
                var accountId = CurrentAccount.AccountID;

                var model = DownlineUIService.SearchDownline(accountId, query);

                return Json(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult GetQuickStartWidgetsData()
        {
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public virtual ActionResult GetEarningsWidgetsData()
        {
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        public virtual int GetPaidAsOrCareerTitle(IEnumerable<TitleProgressionAdvancedWidgetRow> advancedRowData, string titleType)
        {
            var titleRow = advancedRowData.FirstOrDefault(d => d.TypeCode.Equals(titleType, StringComparison.InvariantCultureIgnoreCase));
            var title = 0;
            if (titleRow != null)
            {
                int.TryParse(titleRow.Data.ToString(), out title);
            }
            return title;
        }

        #region Tree View
        [FunctionFilter(Functions.PerformanceTreeView, "~/Performance", Constants.SiteType.BackOffice)]
        public virtual ActionResult TreeView()
        {
            try
            {
                SetMasterPageViewData();

                var accountID = CurrentAccount.AccountID;
                var downline = this.GetDownline(CurrentReportParameters.PeriodID, accountID);

                StringBuilder childrenString = new StringBuilder();
                string rootNodeHtml = string.Empty;

                if (IsKeyPresentInDictionary(downline.LookupNode, accountID))
                {
                    var parentNode = downline.LookupNode[accountID];

                    var children = CustomFilterByAccountStatus(parentNode.Children);

                    foreach (var childNode in children)
                        childrenString.Append(GetNodeHtml(downline, childNode.AccountID, string.Empty));

                    string csval = childrenString.ToString();
                    rootNodeHtml = GetNodeHtml(downline, accountID, (!String.IsNullOrWhiteSpace(csval) ? string.Format("<ul>{0}</ul>", csval) : String.Empty), true);
                }

                var accountInfoCardModel = Create.New<IDownlineInfoCardModel>();

                #region SetProperties
                //Developed by WCS - CSTI

                if (!SetProperties(accountID, downline)) { } //return Json(new { result = true, message = Translation.GetTerm("NoPerformanceDataAvailable", "User has no performance data available") });

                #endregion

                DownlineUIService.LoadDownlineInfoCardModelOptions(
                    accountInfoCardModel.OptionsBag,
                    Url.Action("GetDownlineInfoCardData"),
                    Url.Content("~/Communication/Email/Compose?emailAction=BlankEmail&toAddress=")
                );

                DownlineUIService.LoadDownlineInfoCardModelOptions(
                    accountInfoCardModel.OptionsBag,
                    Url.Action("GetDownlineInfoCardData"),
                    Url.Content("~/Communication/Email/Compose?emailAction=BlankEmail&toAddress=")
                );

                DownlineUIService.LoadDownlineInfoCardModelData(
                    accountInfoCardModel.DataBag,
                    rootAccountId: accountID,
                    accountId: accountID
                );

                var model = Create.New<ITreeViewModel>();
                model.DownlineInfoCard = accountInfoCardModel;

                ViewData["RootNodeHtml"] = rootNodeHtml;
                ViewBag.View = "tv"; //Developed by WCS - CSTI
                GetPaidAsTitle(); //Developed by WCS - CSTI
                return View(model);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        //Developed by WCS - CSTI
        private void GetPaidAsTitle()
        {
            var q = _commissionsService.GetTitles().Where(t => t.Active).OrderBy(t => t.SortOrder)
                          .Select(t => new
                          {
                              key = t.TitleId,
                              value = new
                              {
                                  code = t.TitleCode,
                                  name = Translation.GetTerm(t.TermName, t.ClientName)
                              }
                          }).OrderBy(t => t.key);

            StringBuilder sb = new StringBuilder();
            foreach (var item in q.Select((x, i) => new { Value = x, Index = i }))
            {
                sb.AppendFormat("<li style=\"font-size:10px\"" + "class=\"" + "keyBlock acct{0}" + "\">{1}" + "</li>", item.Index + 1, item.Value.value.name);
            }
            ViewBag.HtmlOutput = sb.ToString();
        }

        [OutputCache(Duration = 60, VaryByParam = "nodeId")]
        [FunctionFilter(Functions.PerformanceTreeView, "~/Performance", Constants.SiteType.BackOffice)]
        public virtual ActionResult GetTreeNodes(string nodeId)
        {
            try
            {
                int accountID = -1;//CoreContext.CurrentAccount.AccountID;

                if (!nodeId.IsNullOrEmpty())
                    accountID = nodeId.ToInt();

                var downline = this.GetDownline(CurrentReportParameters.PeriodID, accountID);

                StringBuilder childrenString = new StringBuilder();
                if (IsKeyPresentInDictionary(downline.LookupNode, accountID))
                {
                    var parentNode = downline.LookupNode[accountID];

                    var children = CustomFilterByAccountStatus(parentNode.Children);

                    foreach (var childNode in children)
                        childrenString.Append(GetNodeHtml(downline, childNode.AccountID, string.Empty));
                }

                return Content(childrenString.ToString());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        [OutputCache(Duration = 60, VaryByParam = "accountId")]
        [AnyFunctionFilter("~/Performance", Constants.SiteType.BackOffice, Functions.PerformanceTreeView, Functions.PerformanceGraphicalDownline)]
        public virtual ActionResult GetDownlineInfoCardData(int? accountId)
        {
            var rootAccountId = CurrentAccount.AccountID;
            var downline = this.GetDownline(CurrentReportParameters.PeriodID, rootAccountId);

            #region SetProperties

            //Developed by WCS - CSTI
            if (!SetProperties(accountId, downline)) { }//return Json(new { result = true, message = Translation.GetTerm("NoPerformanceDataAvailable", "User has no performance data available") });

            #endregion


            IDownlineInfoCardModel model = Create.New<IDownlineInfoCardModel>();
            DownlineUIService.LoadDownlineInfoCardModelData(
                model.DataBag,
                rootAccountId: rootAccountId,
                accountId: accountId ?? rootAccountId
            );

            return Json(model.TempData);
        }

        private bool SetProperties(int? accountId, Downline downline)
        {
            var curentPeriod = CurrentReportParameters.PeriodID;//_commissionsService.GetCurrentPeriod();
            if (curentPeriod == null) return false;
            else
            {
                ReportResults results = new ReportResults();
                results.Downline = downline;//this.GetDownline(curentPeriod.PeriodId);
                if (!results.Downline.Lookup.ContainsKey(accountId ?? 0))
                    return false;
                var currentNodeDetails = results.Downline.Lookup[Convert.ToInt32(accountId)];
                string pv = string.Empty;
                string gv = string.Empty;
                string dv = string.Empty;
                foreach (PropertyInfo property in currentNodeDetails.GetType().GetProperties())
                {
                    switch (property.Name)
                    {
                        case "PQV":
                            pv = Convert.ToString(currentNodeDetails.GetType().GetProperty(property.Name).GetValue(currentNodeDetails, null));
                            break;
                        case "GQV":
                            gv = Convert.ToString(currentNodeDetails.GetType().GetProperty(property.Name).GetValue(currentNodeDetails, null));
                            break;
                        case "DQV":
                            dv = Convert.ToString(currentNodeDetails.GetType().GetProperty(property.Name).GetValue(currentNodeDetails, null));
                            break;
                    }
                }
                DownlineUIService.pv = string.IsNullOrEmpty(pv) ? "0.00" : pv;
                DownlineUIService.gv = string.IsNullOrEmpty(gv) ? "0.00" : gv;
                DownlineUIService.dv = string.IsNullOrEmpty(dv) ? "0.00" : dv;
                DownlineUIService.AccountId = Convert.ToInt32(accountId);
                if (downline.Lookup.ContainsKey(Convert.ToInt32(accountId)))
                {
                    var accountNode = downline.Lookup[Convert.ToInt32(accountId)];
                    int paidTitle = 0;
                    int currentTitle = 0;

                    int.TryParse(accountNode.PaidAsTitle, out paidTitle);
                    int.TryParse(accountNode.CurrentTitle, out currentTitle);

                    DownlineUIService.paidAsTitleID = paidTitle;
                    DownlineUIService.currentTitleID = currentTitle;
                }
                return true;
            }

        }

        [OutputCache(Duration = 60, VaryByParam = "nodeId")]
        [FunctionFilter(Functions.PerformanceTreeView, "~/Performance", Constants.SiteType.BackOffice)]
        public virtual ActionResult GetTreeNodeDetails(string nodeId)
        {
            try
            {
                int currentAccountID = CurrentAccount.AccountID;

                if (!nodeId.IsNullOrEmpty())
                    currentAccountID = nodeId.ToInt();

                var downline = this.GetDownline(CurrentReportParameters.PeriodID);

                // Ensure that the dictionary contains the key before continuing...
                if (downline.Lookup.ContainsKey(currentAccountID))
                {
                    var parentNodeDetails = downline.Lookup[currentAccountID];

                    SetDefaultVisibleColumns(downline, LoadTreeViewCustomColumns());
                    SetColumnNameLookups(downline);

                    bool hasFirstAndLastName = false;
                    string name = string.Empty;
                    if (downline.DynamicTypeGetters.ContainsKey("FirstName") && downline.DynamicTypeGetters.ContainsKey("LastName"))
                    {
                        name = string.Format("{0} {1}", downline.DynamicTypeGetters["FirstName"](parentNodeDetails), downline.DynamicTypeGetters["LastName"](parentNodeDetails));
                        hasFirstAndLastName = true;
                    }

                    StringBuilder builder = BuildInfoCardHtml(downline, hasFirstAndLastName, parentNodeDetails);

                    string emailAddress = CachedData.GetAccountEmailAddress(currentAccountID);

                    return Json(new { result = true, fullName = name, details = builder.ToString(), id = currentAccountID, email = emailAddress });
                }

                // To avoid dictionary invalid key exceptions
                return Json(new { result = false, message = Translation.GetTerm("AccountNotFound", "The current account was not found!") });
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public virtual ReportCustomColumns LoadTreeViewCustomColumns()
        {
            return null;
        }

        public virtual ReportCustomColumns LoadFlatDownlineCustomColumns()
        {
            return null;
        }

        public virtual ReportCustomColumns LoadFlatDownlineCustomColumns(AccountReport report)
        {
            return null;
        }

        public virtual StringBuilder BuildInfoCardHtml(Downline downline, bool hasFirstAndLastName, dynamic parentNodeDetails)
        {
            var builder = new StringBuilder();

            if (!downline.DynamicTypeGetters.ContainsKey("AccountID"))
            {
                return builder;
            }

            int accountID = downline.DynamicTypeGetters["AccountID"](parentNodeDetails);

            Account account = Account.LoadInfoCard(accountID);

            builder.Append("<ul class=\"lr dash flat info\">");
            foreach (string visibleColumn in CurrentReportParameters.VisibleColumns)
            {
                if (ReportCustomColumnFunctions.ContainsKey(visibleColumn + "InfoCard"))
                {
                    builder.Append(ReportCustomColumnFunctions[visibleColumn + "InfoCard"].Invoke(account, visibleColumn));
                }
                else
                {
                    //Overrides column based on name. If an override is performed we will not need to add the html again, so continue
                    if (OverrideDefaultConfigColumns(visibleColumn, builder, account, downline, parentNodeDetails))
                        continue;

                    string stringValue = FormatData(downline, accountID, visibleColumn, downline.DynamicTypeGetters[visibleColumn](parentNodeDetails), GenealogyHelper);

                    string html = string.Format("<li><label class=\"bold\">{0}</label><div>{1}</div><br /></li>",
                        Translation.GetTerm(visibleColumn, visibleColumn.PascalToSpaced()),
                        stringValue);

                    builder.Append(html);
                }
            }

            builder.Append("</ul>");
            return builder;
        }

        /// <summary>
        /// Used in overridden method due to the problems with accessing DynamicTypeGetters from an inheriting class
        /// </summary>
        /// <param name="downline"></param>
        /// <param name="accountID"></param>
        /// <param name="columnName"></param>
        /// <param name="parentNodeDetails"></param>
        /// <returns></returns>
        public string GetStringValue(Downline downline, int accountID, string columnName, dynamic parentNodeDetails)
        {
            return FormatData(downline, accountID, columnName, downline.DynamicTypeGetters[columnName](parentNodeDetails), GenealogyHelper);
        }

        public virtual bool OverrideDefaultConfigColumns(string columnName, StringBuilder builder, Account account, Downline downline, dynamic parentNodeDetails)
        {
            switch (columnName)
            {
                case "FirstName":
                    return true;
                case "LastName":
                    return true;
            }

            return false;
        }

        [OutputCache(Duration = 60, VaryByParam = "search_string")]
        [FunctionFilter(Functions.PerformanceTreeView, "~/Performance", Constants.SiteType.BackOffice)]
        public virtual ActionResult SearchTreeNodes(string search_string)
        {
            try
            {
                int targetAccountId;
                if (int.TryParse(search_string, out targetAccountId))
                {
                    int rootAccountId = CurrentAccount.AccountID;
                    var model = DownlineUIService.GetTreePath(rootAccountId, targetAccountId);
                    return Json(model);
                }

                return Json(new string[] { });
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }
        #endregion

        
        #region Flat Downline
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceFlatDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult NewFlatDownlineReport()
        {
            return RedirectToAction("FlatDownline", new { accountReportID = 0 });
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceFlatDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult FlatDownline(int? accountReportID = null, int? periodID = null)
        {
            if (!accountReportID.HasValue || accountReportID.Value == 0)
            {
                CurrentReportParameters = GetDefaultReportParameters(periodID ?? CurrentReportParameters.PeriodID);
            }

            ViewData["SelectedReport"] = accountReportID.HasValue ? accountReportID.Value.ToString() : "NA";
            ViewData["PeriodID"] = CurrentReportParameters.PeriodID;

            try
            {
                if (accountReportID.HasValue && accountReportID > 0)
                {
                    // TODO: Check that the report belongs to the current account if the not a 'base' report. - JHE
                    var accountReport = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID.ToInt());
                    if (accountReport != null)
                    {
                        CurrentReportParameters = BinarySerializationHelper.Deserialize<DownlineReportParameters>(accountReport.Data);
                        CurrentReportParameters.AccountReportID = accountReportID;

                        ViewData["CurrentAccountReport"] = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);
                    }
                    //Corporate Reports - SCC
                    var corporateReport = CorporateAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID.ToInt());
                    if (corporateReport != null)
                    {
                        var curPeriod = CurrentReportParameters.PeriodID;
                        if (curPeriod <= 0)
                        {
                            var per = _commissionsService.GetCurrentPeriod();
                            if (per != null)
                            {
                                curPeriod = per.PeriodId;
                            }
                        }
                        CurrentReportParameters = BinarySerializationHelper.Deserialize<DownlineReportParameters>(corporateReport.Data);
                        CurrentReportParameters.AccountReportID = accountReportID;
                        CurrentReportParameters.PeriodID = curPeriod;

                        // The sponsorID defaults to a specific account, which ultimately filters out valid results.
                        // Setting sponsorID to the current account to avoid this

                        if (corporateReport.Name.Contains("(NEED ID)"))
                            CurrentReportParameters.SponsorID = CurrentAccount.AccountID;

                        ViewData["CorporateAccountReport"] = CorporateAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);
                    }
                }

                ReportResults results = GetReportResults(CurrentReportParameters);
                SetColumnNameLookups(results.Downline, reset: true);

                if (accountReportID == null || accountReportID == 0)
                {
                    SetDefaultVisibleColumns(results.Downline, LoadFlatDownlineCustomColumns());
                }
                else
                {
                    SetDefaultVisibleColumns(results.Downline, LoadFlatDownlineCustomColumns(AccountReport.Load(accountReportID ?? 0)));
                }

                SetMasterPageViewData();

                ViewData["ColumnHeaders"] = _columnHeaders;
                ViewData["LastUpdated"] = results.Downline.LastUpdated;

                return View(results);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }


        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Reports", "~/", ConstantsGenerated.SiteType.BackOffice)]
        public virtual ActionResult SaveReportData(GetFlatViewModel model)
        //public virtual ActionResult SaveReport(string reportName, GetFlatViewModel model)
        {
            AssignCurrentReportParameterValuesFromModel(model);
            return base.SaveReport(model.ReportName);
        }


        /// <add key="FlatDownlineColumns" value="Level;ID,Name;State;NewStatus;StatusBA;Term;PV;LifeTimeRankStr;CareerTitleStr;PQV;GQV;DQVT;DQV;FirstGenM3;JoinDate;C0;C1;C2;C3;M1;M2;M3;L1;L2;L3;L4;L5;L6;L7"/>
        public static Dictionary<string, string> DownlineManagementColumns = new Dictionary<string, string>(){  
                {"Level","Level"},
                {"ID","ID"},
                {"Name", "Name"},
                {"Phone", "Phone"},
                {"Email", "E-mail"},
                {"State", "State"},
                {"StatusBA", "StatusBA"},
                {"NewStatus", "NewStatus"},
                {"PCV", "PV"},
                {"LifeTimeRankStr", "LifeTimeRankStr"},
                {"Term", "Term"},
                {"CareerTitleStr", "CareerTitleStr"},
                {"PQV", "PQV"},
                {"GQV", "GQV"},
                {"DQVT", "DQVT"},
                {"DQV", "DQV"},
                {"C0", "C0"},
                {"C1", "C1"},
                {"C2", "C2"},
                {"C3", "C3"},
                {"M1", "M1"},
                {"M2", "M2"},
                {"M3", "M3"},
                {"L1", "L1"},
                {"L2", "L2"},
                {"L3", "L3"},
                {"L4", "L4"},
                {"L5", "L5"},
                {"L7", "L7"},
                {"L6", "L6"},
        };

        /// <summary>
        /// Columnas que se muestran por defecto en la vista
        /// </summary>
        /// <add key="FlatDownlineColumns" value="Level;ID,Name;State;NewStatus;StatusBA;Term;PV;LifeTimeRankStr;CareerTitleStr;PQV;GQV;DQVT;DQV;FirstGenM3;JoinDate;C0;C1;C2;C3;M1;M2;M3;L1;L2;L3;L4;L5;L6;L7"/>
        public static Dictionary<string, string> FlatColumns = new Dictionary<string, string>(){  
                {"Level","Level"},
                {"ID","ID"},
                {"Name", "Name"},
                {"Phone", "Phone"},
                {"Email", "E-mail"},
                {"State", "State"},
                {"StatusBA", "StatusBA"},
                {"NewStatus", "NewStatus"},
                {"PCV", "PV"},
                {"LifeTimeRankStr", "LifeTimeRankStr"},
                {"Term", "Term"},
                {"CareerTitleStr", "CareerTitleStr"},
                {"PQV", "PQV"},
                {"GQV", "GQV"},
                {"DQVT", "DQVT"},
                {"DCV", "DCV"},
                {"C0", "C0"},
                {"C1", "C1"},
                {"C2", "C2"},
                {"C3", "C3"},
                {"M1", "M1"},
                {"M2", "M2"},
                {"M3", "M3"},
                {"L1", "L1"},
                {"L2", "L2"},
                {"L3", "L3"},
                {"L4", "L4"},
                {"L5", "L5"},
                {"L7", "L7"},
                {"L6", "L6"},
        };

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceFlatDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetFlat(Models.Performance.GetFlatViewModel model)
        {
            try
            {

                StringBuilder builder = new StringBuilder();

                AssignCurrentReportParameterValuesFromModel(model);


                var results = GetReportResults(CurrentReportParameters);
                SetColumnNameLookups(results.Downline);
                //Set the visible columns based upon if we're loading an account report or not
                if (CurrentReportParameters.AccountReportID == null || CurrentReportParameters.AccountReportID == 0)
                    SetDefaultVisibleColumns(results.Downline, LoadFlatDownlineCustomColumns());
                else
                    SetDefaultVisibleColumns(results.Downline, LoadFlatDownlineCustomColumns(AccountReport.Load(CurrentReportParameters.AccountReportID ?? 0)));

                var list = results.ResultsDetails as IEnumerable<dynamic>;

                Dictionary<string, int> columnIndexes = new Dictionary<string, int>();
                int counter = 0;

                //Creates a dictionary of column indexes based upon the order of the headers. This is used to ensure that the data in the table matches up with the headers
                foreach (KeyValuePair<string, string> header in _columnHeaders)
                    columnIndexes.Add(header.Key, counter++);

                List<Account> tempAccounts = null;
                //List<Account> tempAccounts = Account.LoadBatchFull(list.Select(l => (int)l.AccountID).ToList());
                int accountCounter = 0;

                //Iterate through each account
                foreach (var child in list)
                {
                    int accountID = results.Downline.DynamicTypeGetters["AccountID"](child);
                    builder.Append(string.Format("<tr data-id=\"{0}\">", accountID));

                    //Append both the checkbox and the Select columns
                    if (model.IncludeCheckbox)
                        builder.AppendCheckBoxCell(accountID.ToString(), accountID.ToString(), accountID.ToString(), "downlineAccount");

                    builder.AppendLinkCell("javascript:void(0);", "Select", linkCssClass: "groupByCurrentAccount");


                    //Create an array that will hold the html generated by this method.
                    //The html will be put into the array based upon the columnIndex dictionary populated earlier
                    string[] htmlSegments = new string[columnIndexes.Count];
                    foreach (var visibleColumn in CurrentReportParameters.VisibleColumns.Where(m => FlatColumns.ContainsKey(m)))
                    //foreach (var visibleColumn in CurrentReportParameters.VisibleColumns)
                    {
                        if (ReportCustomColumnFunctions.ContainsKey(visibleColumn + "Flat"))
                        {
                            if (tempAccounts == null)
                            {
                                tempAccounts = Account.LoadInfoCardBatch(list.Select(l => (int)l.AccountID).ToList()).ToList();
                            }

                            htmlSegments[columnIndexes[visibleColumn]] =
                                ReportCustomColumnFunctions[visibleColumn + "Flat"].Invoke(tempAccounts[accountCounter], visibleColumn).
                                    ToCellString();
                        }
                        else if (results.Downline.DynamicTypeGetters.ContainsKey(visibleColumn) && visibleColumn != "State")
                        {
                            string stringValue = FormatData(
                                results.Downline,
                                accountID,
                                visibleColumn,
                                results.Downline.DynamicTypeGetters[visibleColumn](child),
                                GenealogyHelper);

                            if (columnIndexes.ContainsKey(visibleColumn))
                            {
                                htmlSegments[columnIndexes[visibleColumn]] = stringValue.ToCellString();
                            }
                        }
                    }
                    
                    //Add the array of html to the strinbBuilder
                    for (int i = 0; i < htmlSegments.Length; i++)
                        //builder.Append(htmlSegments[i]);
                        builder.Append(string.IsNullOrEmpty(htmlSegments[i]) ? htmlSegments[i] :  GetCurrencyByCultureInfo(htmlSegments[i]));
                        // inicio 30052017 => comentando por hundred para generalizar los formatos currency
                        //builder.Append((i == 7 || i == 10 || i == 11 || i == 12 || i == 13) ? htmlSegments[i].Replace("$", "") : htmlSegments[i]);
                        // fin 20052017
                    builder.Append("</tr>");
                    

                    accountCounter++;
                }

                return Json(new { result = true, totalPages = Math.Ceiling(results.TotalCount / (double)model.PageSize), page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public string GetValueOfTd(string value)
        {
             string td_pattern = @"<td\b[^>]*?>(?<V>[\s\S]*?)</\s*td>";
             var matches = Regex.Match(value, td_pattern, RegexOptions.Singleline);
            //foreach (Match match in matches)
            return matches.Groups["V"].Value;
        }

        public string GetCurrencyByCultureInfo(string valor)
        {

            var template = "<td>{0}</td>";
            valor = GetValueOfTd(valor);
            NumberStyles NumberStyle = NumberStyles.AllowCurrencySymbol;
            var cultures = new List<CultureInfo>
            {
                new CultureInfo("en-US"),
                new CultureInfo("es-US"),
                new CultureInfo("pt-BR")
            };

            

            if (!cultures.Any(y=>y.Name == CoreContext.CurrentCultureInfo.Name))
                 cultures.Add(CoreContext.CurrentCultureInfo);

           

            foreach (var cultura in cultures)
            {
                var symbol =cultura.NumberFormat.CurrencySymbol.ToString();
                if (valor.IndexOf(symbol) > -1)
                {
                    valor.Replace(symbol, CoreContext.CurrentCultureInfo.NumberFormat.CurrencySymbol.ToString());
                    break;
                }
            }

              
            

              return string.Format(template, valor);
        }
        protected virtual void AssignCurrentReportParameterValuesFromModel(GetFlatViewModel model)
        {
            var currentReportParameters = CurrentReportParameters;
            currentReportParameters.PageIndex = model.Page;
            currentReportParameters.PageSize = model.PageSize;
            currentReportParameters.OrderBy = model.OrderBy;
            currentReportParameters.OrderByDirection = model.OrderByDirection;
            currentReportParameters.StartDate = model.StartDate;
            currentReportParameters.EndDate = model.EndDate;
            currentReportParameters.BirthMonth = model.BirthMonth;
            currentReportParameters.PVFrom = model.PvFrom;
            currentReportParameters.PVTo = model.PvTo;
            currentReportParameters.GVFrom = model.GvFrom;
            currentReportParameters.GVTo = model.GvTo;
            currentReportParameters.ShowMyTeam = model.ShowMyTeam;
            currentReportParameters.SponsorID = model.SponsorId;
            currentReportParameters.Titles = model.Titles;
            currentReportParameters.AccountTypes = model.AccountTypes;
            currentReportParameters.AccountStatuses = model.AccountStatuses;
            currentReportParameters.States = model.States;
            currentReportParameters.SearchValue = model.SearchValue;
            currentReportParameters.MonthsInactive = model.MonthsInActive;
            currentReportParameters.CurrentTopOfTreeAccountID = model.CurrentTopOfTreeAccountId;
            currentReportParameters.GroupBySponsorTree = model.GroupBySponsorTree;
            if (model.ShowMyTeam)
                currentReportParameters.SponsorID = CurrentAccount.AccountID;

            if (model.PeriodId != null)
                currentReportParameters.PeriodID = model.PeriodId.ToInt();

            //Agregando Filtros 
            currentReportParameters.LevelFrom = model.LevelFrom;
            currentReportParameters.LevelTo = model.LevelTo;
            currentReportParameters.GenFrom = model.GenFrom;
            currentReportParameters.GenTo = model.GenTo;
            currentReportParameters.DVFrom = model.DVFrom;
            currentReportParameters.DVTo = model.DVTo;

            CurrentReportParameters = currentReportParameters;
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceExportExcel, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult ExportExcel(int? periodId = null)
        {
            
            try
            {
                var searchParameters = CurrentReportParameters;
                string fileNameSave = string.Format("{0} {1}.xlsx", Translation.GetTerm("DownlineExport", "Downline Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

                if (periodId != null)
                {
                    searchParameters.PeriodID = periodId.ToInt();
                    fileNameSave = string.Format("{0} {1}.xlsx", Translation.GetTerm("DownlineExport", "Downline Export"), periodId);
                }

                searchParameters.PageIndex = 0;
                searchParameters.PageSize = null;

                var results = GetReportResults(searchParameters);
                SetColumnNameLookups(results.Downline);

                // TODO: Move this (overridenValueGetters) to Business Logic (IoC) - JHE
                Dictionary<string, MemberGetter> overridenValueGetters = new Dictionary<string, MemberGetter>();
                overridenValueGetters.Add("PaidAsTitle", v => ((v as dynamic).PaidAsTitle as int? != null) ? _commissionsService.GetTitle(((v as dynamic).PaidAsTitle as int?).ToInt()).TermName : Translation.GetTerm("N/A"));
                overridenValueGetters.Add("CurrentTitle", v => ((v as dynamic).CurrentTitle as int? != null) ? _commissionsService.GetTitle(((v as dynamic).CurrentTitle as int?).ToInt()).TermName : Translation.GetTerm("N/A"));
                int currentAccountNodeLevel = results.Downline.LookupNode[CurrentAccount.AccountID].Level;
                overridenValueGetters.Add("Level", v => ((v as dynamic).Level - currentAccountNodeLevel));
                return new ExcelResult<dynamic>(fileNameSave, results.ResultsDetails as IEnumerable<dynamic>, _columnHeaders, overridenValueGetters, CurrentReportParameters.VisibleColumns.ToArray());
                
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.Performance, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SaveVisibleColumns(List<string> visibleColumns)
        {
            try
            {
                if (visibleColumns != null && visibleColumns.Count > 0)
                {
                    List<string> newColumns = new List<string>();
                    foreach (string column in visibleColumns)
                        newColumns.Add(column);

                    CurrentReportParameters.VisibleColumns = newColumns;
                    return Json(new { result = true });
                }
                else
                    return Json(new { result = false, message = Translation.GetTerm("AtLeast1ColumnMustBeVisible", "At least 1 column must be visible.") });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceEmailDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult DownlineEmail(List<int> accountIDs)
        {
            JsonResult result;

            try
            {
                if (accountIDs != null && accountIDs.Count > 0)
                {
                    TempData["EmailDownline"] = accountIDs;

                    result = Json(new { result = true });
                }
                else
                {
                    result = Json(new { result = false, message = Translation.GetTerm("No_Customers_Selected", "No customers selected.") });
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                result = Json(new { result = false, message = exception.PublicMessage });
            }

            return result;
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceEmailAllDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public ActionResult DownlineEmailAll()
        {
            JsonResult result;

            var defaultReport = GetDefaultReportParameters(CurrentReportParameters.PeriodID);
            try
            {
                //get current page size and index
                defaultReport.PageIndex = CurrentReportParameters.PageIndex;
                defaultReport.PageSize = CurrentReportParameters.PageSize;
                //remove page size and index
                CurrentReportParameters.PageIndex = 0;
                CurrentReportParameters.PageSize = null;

                ReportResults results = GetReportResults(CurrentReportParameters);
                var list = results.ResultsDetails as IEnumerable<dynamic>;
                List<int> accountIDs = list.Select(r => (int)r.AccountID).ToList();

                if (accountIDs != null && accountIDs.Count > 0)
                {
                    TempData["EmailDownline"] = accountIDs;

                    result = Json(new { result = true });
                }
                else
                {
                    result = Json(new { result = false, message = Translation.GetTerm("No_Customers_Selected", "No customers selected.") });
                }

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                result = Json(new { result = false, message = exception.PublicMessage });
            }
            finally
            {
                //set back current page size and index
                CurrentReportParameters.PageIndex = defaultReport.PageIndex;
                CurrentReportParameters.PageSize = defaultReport.PageSize;
            }

            return result;
        }
        #endregion

        protected virtual StringBuilder GetPerformanceOverviewWidgetDataFromProc(int periodId)
        {
            StringBuilder builder = new StringBuilder();
            var results = _commissionsService.GetDistributorPerformanceOverviewData(CurrentAccount.AccountID, periodId);
            string title = String.Empty;
            string value = String.Empty;
            byte bloquearPrimerSegundoRegistro = 0;
            foreach (var r in results)
            {

                // el resto LegDetails
                title = Translation.GetTerm(r.TermName, r.TermName.PascalToSpaced());

                if (r.KPITypeCode.ToLower().Contains("leg"))
                {
                    value = FormatData(r.DataType, r.KPIValue, r.TermName).Replace("N/A", "0");
                    value = value.ToString(); 
                }
                else
                {
                    //value = FormatData(r.DataType, r.KPIValue, r.TermName).Replace("N/A", "0.00");

                    //value = "$ " + r.KPIValue; //EL QV NO DEBE TENER SIGNO $

                    //value = (r.TermName.Equals("PQV") || r.TermName.Equals("GQV") || r.TermName.Equals("DQV")) ? r.KPIValue.Replace(".00","") : "$ " + r.KPIValue;
                    // inicio 30052017 => agregado por hundred
                    value = (r.TermName.Equals("PQV") || r.TermName.Equals("GQV") || r.TermName.Equals("DQV")) ? r.KPIValue.Replace(".00", "") :  GetCulturaFormat(r.KPIValue);
                    // fin 30052017 => agregado por hundred
                }

                if (bloquearPrimerSegundoRegistro < 2)
                    builder.Append(string.Format("<div class=\"overviewNameValue FL UI-lightBg brdr1\" id=\"{2}\"><span class=\"FL\">{0}:</span><span class=\"FR bold overviewVal\">{1}</span><span class=\"clr\"></span></div>", title, value, r.KPITypeCode));
                else if (bloquearPrimerSegundoRegistro == 2)
                    builder.Append(string.Format("<div class=\"overviewNameValue FL UI-lightBg brdr1\" id=\"{2}\" style=\"cursor: pointer;\" onClick=\"return ShowPopUp('{2}', '{0}');\"><span class=\"FL\">*  {0}:</span><span class=\"FR bold overviewVal\">{1}</span><span class=\"clr\"></span></div>", title, value, r.KPITypeCode));
                else
                    builder.Append(string.Format("<div class=\"overviewNameValue FL UI-lightBg brdr1\" id=\"{2}\" style=\"cursor: pointer;\" onClick=\"return ShowPopUp('{2}', '{0}');\"><span class=\"FL\">{0}:</span><span class=\"FR bold overviewVal\">{1}</span><span class=\"clr\"></span></div>", title, value, r.KPITypeCode));
                bloquearPrimerSegundoRegistro++;
            }
            builder.Append("<span class=\"clr\" style='color:Red'><b>" + Translation.GetTerm("relatorioMSG", "* Indicador válido até junho de 2016") + "</b></span>");
            builder.Append("<span class=\"clr\"></span>");
            return builder;
        }
        public string GetCulturaFormat(string valor)
        {
            bool correcto = false;
            decimal numero = 0;

            var formatos = new List<System.Globalization.CultureInfo>
            {
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-US")

            };

            if (!formatos.Any(x => x.Name == CoreContext.CurrentCultureInfo.Name))
                formatos.Add(CoreContext.CurrentCultureInfo);



            if (string.IsNullOrEmpty(valor))
                return numero.ToString("C", CoreContext.CurrentCultureInfo);


            foreach (var item in formatos)
            {

                if (decimal.TryParse(valor, System.Globalization.NumberStyles.AllowDecimalPoint, item, out numero) == true)
                {
                    correcto = true;
                    break;
                }



            }
            if (correcto)
            {

                return numero.ToString("C", CoreContext.CurrentCultureInfo);

            }
            else
                return valor;


        }

        protected virtual StringBuilder GetPerformanceOverviewWidgetDataFromDownlineCache(IList<string> columns, int periodId)
        {
            StringBuilder builder = new StringBuilder();

            var downline = this.GetDownline(periodId);

            if (!downline.Lookup.ContainsKey(CurrentAccount.AccountID))
                return builder;
            //return Json(new { result = false, message = "Account data not found in downline cache." });

            var currentNodeData = downline.Lookup[CurrentAccount.AccountID];

            SetColumnNameLookups(downline);

            foreach (var columnName in columns)
            {
                if (_columnHeaders.ContainsKey(columnName))
                {
                    string title = string.Empty;
                    string value = string.Empty;

                    switch (columnName)
                    {
                        case "SponsorID":
                            var sponsorNodeData = downline.Lookup[downline.DynamicTypeGetters["SponsorID"](currentNodeData)];

                            title = Translation.GetTerm("Sponsor", "Sponsor");
                            value = string.Format("<a href=\"{0}{1}\">{2}</a>",
                                "~/Contacts/View/".ResolveUrl(),
                                sponsorNodeData.AccountID,
                                Account.ToFullName(sponsorNodeData.FirstName, string.Empty, sponsorNodeData.LastName, CoreContext.CurrentCultureInfo.Name));
                            break;
                        case "FlatDownlineCount":
                            title = Translation.GetTerm(_columnHeaders[columnName], _columnHeaders[columnName].PascalToSpaced());
                            value = string.Format("<a id=\"flatDownlineCountLink\" href=\"{0}\">{1}</a>", ("~/Performance/FlatDownline?periodID=" + periodId).ResolveUrl(), downline.DynamicTypeGetters[columnName](currentNodeData));
                            break;
                        case "UplineDirectorID":
                            var uplineDirectorID = downline.DynamicTypeGetters[columnName](currentNodeData);
                            if (uplineDirectorID != null)
                            {
                                var directorAccountData = Account.LoadByAccountNumber(uplineDirectorID.ToString());
                                if (directorAccountData != null)
                                {
                                    title = Translation.GetTerm("UplineDirector", "Upline Director");
                                    value = string.Format("<a href=\"{0}{1}\">{2}</a>",
                                                            "~/Contacts/View/".ResolveUrl(),
                                                            directorAccountData.AccountID,
                                                            Account.ToFullName(directorAccountData.FirstName,
                                                                                string.Empty,
                                                                                directorAccountData.LastName,
                                                                                CoreContext.CurrentCultureInfo.Name));
                                }
                            }
                            break;
                        default:
                            title = Translation.GetTerm(_columnHeaders[columnName], _columnHeaders[columnName].PascalToSpaced());
                            value = FormatData(downline, CurrentAccount.AccountID, columnName, downline.DynamicTypeGetters[columnName](currentNodeData), GenealogyHelper);
                            break;
                    }

                    builder.Append(string.Format("<div class=\"FL UI-lightBg brdr1 overviewNameValue\" id=\"{2}\"><span class=\"FL\">{0}:</span><span class=\"FR bold overviewVal\">{1}</span><span class=\"clr\"></span></div>", title, value, columnName));
                }
            }

            builder.Append("<span class=\"clr\"></span>");
            return builder;
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceOverview, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetPerformanceOverview(int? periodId, bool showPerformanceWidgets = false)
        {
            ShowVolumeWidget = true;
            ShowTitleProgressWidget = true;
            ShowPerformanceOverviewWidget = true;
            var showGraphicalPerformance = ShouldShowGraphicalPerformanceWidgets(showPerformanceWidgets);
            if (!showGraphicalPerformance)
            {
                ShowVolumeWidget = false;
                ShowTitleProgressWidget = false;
            }

            if (!periodId.HasValue)
            {
                //We dont want to show a message anymore. SOK 
                var curPeriod = _commissionsService.GetCurrentPeriod();
                if (curPeriod != null)
                {
                    periodId = curPeriod.PeriodId;
                }
                else
                {
                    periodId = 0;
                }
                //return Json(new { result = true, message = "User has no performance data available" });
            }

            try
            {
                StringBuilder builder = new StringBuilder();

                bool useDownlineCacheForKPIs = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseDownlineCacheForKPIs, true);

                if (useDownlineCacheForKPIs)
                {
                    string columnsString = ConfigurationManager.GetAppSetting<string>("CommissionKpiColumns");
                    IList<string> columns = null;
                    if (!String.IsNullOrWhiteSpace(columnsString))
                    {
                        columns = columnsString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList().TrimWhitespace();
                    }

                    builder = GetPerformanceOverviewWidgetDataFromDownlineCache(columns, periodId.Value);
                }
                else
                {
                    builder = GetPerformanceOverviewWidgetDataFromProc(periodId.Value);
                }

                return Json(new
                {
                    result  =true,
                    data = builder.ToString(),
                    model = showGraphicalPerformance ? new PerformanceLandingViewModel(_commissionsService.GetDistributorPerformanceData(CurrentAccount.AccountID, periodId.Value)) : null,
                    showVolumeWidget = ShowVolumeWidget,
                    showTitleProgressWidget = ShowTitleProgressWidget,
                    showPerformanceOverviewWidget = ShowPerformanceOverviewWidget
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceOverview, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetNameValueWidgetData(int? periodId)
        {
            if (!periodId.HasValue)
            {
                //We dont want to show a message anymore. SOK 
                return Json(new { result = true, message = "User has no performance data available" });
            }

            try
            {
                StringBuilder builder = new StringBuilder();

                bool useDownlineCacheForKPIs = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.UseDownlineCacheForKPIs, true);
                if (useDownlineCacheForKPIs)
                {
                    string columnsString = ConfigurationManager.GetAppSetting<string>("CommissionKpiColumns");
                    IList<string> columns = null;
                    if (!String.IsNullOrWhiteSpace(columnsString))
                    {
                        columns = columnsString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList().TrimWhitespace();
                    }

                    builder = GetPerformanceOverviewWidgetDataFromDownlineCache(columns, periodId.Value);
                    if (builder.ToString().IsNullOrEmpty())
                        return Json(new { result = false, message = "Account data not found in downline cache." });
                }
                else
                {
                    builder = GetPerformanceOverviewWidgetDataFromProc(periodId.Value);
                }

                return Json(new
                {
                    result = true,
                    data = builder.ToString(),
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceOverview, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetVolumeWidgetData(int? periodId)
        {
            if (!periodId.HasValue)
            {
                return Json(new { result = true, message = "User has no performance data available" });
            }

            var modelPeriodId = periodId ?? _commissionsService.GetCurrentPeriod().PeriodId;
            var results = _commissionsService.GetDistributorPerformanceData(CurrentAccount.AccountID, modelPeriodId);
            var model = new PerformanceLandingViewModel(results);

            return Json(new
            {
                result = true,
                model = model,
            });
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceOverview, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetTitleProgressionWidgetData(int? periodId)
        {
            if (!periodId.HasValue)
            {
                //We dont want to show a message anymore. SOK 
                return Json(new { result = true, message = Translation.GetTerm("NoPerformanceDataAvailable", "User has no performance data available") });
            }

            try
            {
                //Need to get brian to fix stored proc
                var downline = DownlineCache.GetDownline(periodId.Value, CurrentAccount.AccountID);
                if (!downline.Lookup.ContainsKey(CurrentAccount.AccountID))
                {
                    return Json(new { result = true, message = Translation.GetTerm("NoPerformanceDataAvailable", "User has no performance data available") });
                }

                TitleProgressionWidgetViewModel model = null;
                if (downline.Lookup.ContainsKey(CurrentAccountId))
                {
                    var accountNode = downline.Lookup[CurrentAccount.AccountID];
                    int paidAsTitleID;
                    int.TryParse(accountNode.PaidAsCurrentMonth, out paidAsTitleID);//accountNode.PaidAsTitle;
                    int currentTitleID;
                    int.TryParse(accountNode.CareerTitle, out currentTitleID); //accountNode.CurrentTitle;
                    var currentTitle = _commissionsService.GetTitle(currentTitleID);
                    var paidAsTitle = _commissionsService.GetTitle(paidAsTitleID);
                    model = new TitleProgressionWidgetViewModel()
                    {
                        PaidAsLevel = paidAsTitleID,
                        PaidAsLevelName = paidAsTitle != null ? Translation.GetTerm(paidAsTitle.TermName) : String.Empty,
                        CurrentLevel = currentTitleID,
                        CurrentLevelName = currentTitle != null ? Translation.GetTerm(currentTitle.TermName) : String.Empty
                    };
                }
                else
                {
                    model = new TitleProgressionWidgetViewModel();
                }

                return Json(new
                {
                    result = true,
                    model = model,
                });
            }
            catch (Exception excp)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(excp, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceOverview, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetPerformanceLandingData(int periodId)
        {
            var results = _commissionsService.GetDistributorPerformanceData(CurrentAccount.AccountID, periodId);
            var model = new PerformanceLandingViewModel(results);
            return Json(new { result = true, data = model });
        }

        /// <summary>
        /// Obtiene el detalle del volumen de Rerport KPIS
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="code">KPI Code</param>
        /// <returns></returns>
        public virtual ActionResult GetVolumenDetail(int periodId, string code)
        {
            string htmlResult = string.Empty;

            switch (code)
            {
                case "PQV":
                case "PCV":
                case "GQV":
                case "GCV":
                case "DQV":
                case "DCV":
                    htmlResult = GetAccountKpisDetails(periodId, code).ToString();
                    break;
                default:
                    htmlResult = GetAccountKpisDetailsLegs(periodId, code).ToString();
                    break;
            }

            return Json(new { result = true, data = htmlResult });
        }

        /// <summary>
        /// Obtiene el Detalle de kpis
        /// </summary>
        /// <param name="periodId"></param>
        /// <param name="kpiCode"></param>
        /// <returns></returns>
        private StringBuilder GetAccountKpisDetails(int periodId, string kpiCode)
        {
            int accountID = CurrentAccountId;

            StringBuilder builder = new StringBuilder();

            var data = _commissionsService.GetReportAccountKPIDetails(periodId, accountID, kpiCode);

            //builder.Append("<div id='dvData'>");
            //builder.Append("<table class=\"tableModal\">");
            builder.Append("<table id='ResultList'>");
            builder.Append(string.Format(@"<tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th></tr>",
                Translation.GetTerm("Level", "Level"),
                Translation.GetTerm("CustomeID", "Customer ID"),
                Translation.GetTerm("CustomerName", "Customer Name"),
                Translation.GetTerm("QualifiedVolumen", "Qualified Volumen")
                ));

            foreach (var item in data)
            {
                builder.Append(string.Format("<tr><td>{0}</td>", item.Level));
                builder.Append(string.Format("<td>{0}</td>", item.DownlineID));
                builder.Append(string.Format("<td>{0}</td>", item.DownlineName));
                if (kpiCode.Equals("DQV") || kpiCode.Equals("GQV"))
                    builder.Append(string.Format("<td style='text-align:right'>{0}</td></tr>", Decimal.ToInt32(Math.Round(item.QV.ToDecimal())).ToString()));
                else
                    builder.Append(string.Format("<td style='text-align:right'>{0}</td></tr>", item.QV.ToMoneyString()));
            }
            builder.Append(string.Format("<tr><td>{0}</td>", "<input type='hidden' id='Codigo' name='Codigo' value='" + kpiCode + "'>"));
            builder.Append(string.Format("<td>{0}</td>", ""));
            builder.Append(string.Format("<td><b>{0}</b></td>", "Total:"));

            if (kpiCode.Equals("DQV") || kpiCode.Equals("GQV"))
                builder.Append(string.Format("<td style='text-align:right'><b>{0}</b></td></tr>", Decimal.ToInt32(Math.Round(data.Sum(suma => suma.QV).ToDecimal())).ToString()));
            else
                builder.Append(string.Format("<td style='text-align:right'><b>{0}</b></td></tr>", data.Sum(suma => suma.QV).ToMoneyString()));

            builder.Append("</table>");
            builder.Append("</div>");

            return builder;
        }


        private StringBuilder GetAccountKpisDetailsLegs(int periodId, string kpiCode)
        {
            int accountId = CurrentAccountId;
            StringBuilder builder = new StringBuilder();

            var data = _commissionsService.GetReportAccountKPIDetailsLegs(periodId, accountId, kpiCode);

            builder.Append("<table class=\"tableModal\">");
            builder.Append(string.Format(@"<tr><th>{0}</th><th>{1}</th><th>{2}</th><th>{3}</th><th>{4}</th><th>{5}</th><th>{6}</th><th>{7}</th></tr>",
                Translation.GetTerm("DownlineID", "Cod. Cons"),
                Translation.GetTerm("DownlineName", "Name"),
                Translation.GetTerm("Level", "Level"),
                Translation.GetTerm("Generation", "Generation"),
                Translation.GetTerm("CareerTitle", "Career Title"),
                Translation.GetTerm("PaidAsTitle", "Paid As Title"),
                Translation.GetTerm("PV", "PV"),
                Translation.GetTerm("DV", "DV")
                ));

            foreach (var item in data)
            {
                builder.Append(string.Format("<tr><td>{0}</td>", item.DownlineID));
                builder.Append(string.Format("<td>{0}</td>", item.DownlineName));
                builder.Append(string.Format("<td>{0}</td>", item.Level));
                builder.Append(string.Format("<td>{0}</td>", item.Generation));
                builder.Append(string.Format("<td>{0}</td>", item.CareerTitle));
                builder.Append(string.Format("<td>{0}</td>", item.DownlinePaidAsTitle));
                builder.Append(string.Format("<td style='text-align:right'>{0}</td>", item.PQV.ToMoneyString()));
                builder.Append(string.Format("<td style='text-align:right'>{0}</td></tr>", item.DQV.ToMoneyString()));
            }
            builder.Append(string.Format("<tr><td>{0}</td>", ""));
            builder.Append(string.Format("<td>{0}</td>", ""));
            builder.Append(string.Format("<td>{0}</td>", ""));
            builder.Append(string.Format("<td>{0}</td>", ""));
            builder.Append(string.Format("<td>{0}</td>", ""));
            builder.Append(string.Format("<td>{0}</td>", "Total"));
            builder.Append(string.Format("<td style='text-align:right'><b>{0}</b></td>", data.Sum(suma => suma.PQV).ToMoneyString()));
            builder.Append(string.Format("<td style='text-align:right'><b>{0}</b></td></tr>", data.Sum(suma => suma.DQV).ToMoneyString()));
            builder.Append("</table>");

            return builder;
        }


        public virtual ActionResult ExportToExcel(int periodId, string code)
        {
            try
            {

                //PaginatedList<InventoryMovementSearchData> data = new PaginatedList<InventoryMovementSearchData>();
                int accountId = CurrentAccountId;
                IEnumerable<IReportAccountKPIDetail> dataDetail;
                switch (code)
                {
                    case "PQV":
                    case "PCV":
                    case "GQV":
                    case "GCV":
                    case "DQV":
                    case "DCV":
                        dataDetail = _commissionsService.GetReportAccountKPIDetails(periodId, accountId, code);
                        break;
                    default:
                        dataDetail = _commissionsService.GetReportAccountKPIDetailsLegs(periodId, accountId, code);
                        break;
                }



                //foreach (var item in dataDetail)
                //{
                //    ReportAccountKPIDetailSearchData entidad = new ReportAccountKPIDetailSearchData();
                //    entidad.AccountKPIDetailID = (int)item.AccountKPIDetailID;
                //    entidad.DownlineID = (int)item.DownlineID;
                //    entidad.DownlineName = item.DownlineName;
                //    entidad.Level = (int)item.Level;
                //    entidad.Generation = (int)item.Generation;
                //    entidad.CareerTitle = item.CareerTitle;
                //    entidad.DownlinePaidAsTitle = item.DownlinePaidAsTitle;
                //    entidad.PQV = (decimal)item.PQV;
                //    entidad.DQV = (decimal)item.DQV;
                //    //data.Add(entidad);
                //}

                var grid = new GridView();
                grid.DataSource = dataDetail;
                grid.DataBind();

                Response.ClearContent();
                Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");

                Response.ContentType = "application/excel";

                StringWriter sw = new StringWriter();

                HtmlTextWriter htw = new HtmlTextWriter(sw);

                grid.RenderControl(htw);

                Response.Write(sw.ToString());

                Response.End();

                return View("Index");
                /*
                 
                string nombreArchivo = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                  Response.ClearContent();
                 //Response.Buffer = true;
                 Response.AddHeader("content-disposition", "attachment;filename=" + nombreArchivo);
                 //Response.ContentType = "application/ms-excel";
                 Response.ContentType = "application/excel";
                 //Response.Charset = "UTF-8"; 
                 //Response.ContentType = "application/vnd.ms-excel";
                 //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";  
                 StringWriter sw = new StringWriter();
                 HtmlTextWriter htw = new HtmlTextWriter(sw);

                 grid.RenderControl(htw);

                 Response.Write(sw.ToString());
                 //Response.Output.Write(sw.ToString());
                 //Response.Flush();
                 Response.End();
                 */
                ////////////////////Response.ClearContent();
                //////////////////////Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
                ////////////////////Response.AddHeader("content-disposition", "attachment; filename=" + nombreArchivo);
                ////////////////////Response.ContentType = "application/excel";

                ////////////////////StringWriter sw = new StringWriter();

                ////////////////////HtmlTextWriter htw = new HtmlTextWriter(sw);

                ////////////////////grid.RenderControl(htw);

                ////////////////////Response.Write(sw.ToString());

                ////////////////////Response.End();

                //string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("AccountKPI"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

                //var columns = new Dictionary<string, string>
                //{
                //    {"AccountKPIDetailID", Translation.GetTerm("AccountKPIDetailID")} ,
                //    {"CareerTitle", Translation.GetTerm("CareerTitle")} ,
                //    {"DownlineID", Translation.GetTerm("DownlineID")} ,
                //    {"DownlineName", Translation.GetTerm("Period")} ,
                //    {"DownlinePaidAsTitle", Translation.GetTerm("ExperiedDate")} ,
                //    {"DQV", Translation.GetTerm("DQV")} ,
                //    {"Generation", Translation.GetTerm("Generation")} ,
                //    {"Level", Translation.GetTerm("Level")} ,
                //    {"PQV", Translation.GetTerm("PQV")} ,
                //    {"QV", Translation.GetTerm("QV")}  
                //};

                ////return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ReportAccountKPIDetailSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());

                //return Json(new { success = true, message = Translation.GetTerm("SuccessfullyProsecuted") });
                //return RedirectToAction("Index"); 
                return View("Index");
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        #region Helper Methods

        protected virtual Downline GetDownline(int periodID)
        {
            return DownlineCache.GetDownline(periodID);
        }

        protected virtual Downline GetDownline(int periodID, int sponsorID)
        {
            return DownlineCache.GetDownline(periodID, sponsorID);
        }

        protected virtual Account GetCurrentAccount()
        {
            return CurrentAccount;
        }

        protected virtual bool ShouldShowGraphicalPerformanceWidgets(bool showPerformanceWidgets)
        {
            return showPerformanceWidgets;
        }
        /// <summary>
        /// Ensures that the specified dictionary contains the given key
        /// </summary>
        private bool IsKeyPresentInDictionary<T, V>(Dictionary<T, V> dict, T key)
        {
            return dict.ContainsKey(key);
        }

        private Node CreateNode(dynamic nodeDetails, string fullName, string type, string toolTip)
        {
            string IsCommissionQualified = nodeDetails == null ? "false" : nodeDetails.IsCommissionQualified.ToString();

            short paidAsTitleID = 0;

            short.TryParse(nodeDetails.PaidAsTitle, out paidAsTitleID);

            Node root = new Node
            {
                id = Convert.ToInt32(nodeDetails.AccountID),
                name = fullName,
                data = new NodeData
                {
                    type = type,
                    tooltip = toolTip,
                    hex = GetHexForNodeType(type, bool.Parse(IsCommissionQualified), nodeDetails.AutoshipProcessDate, paidAsTitleID)
                }
            };
            return root;
        }

        private string GetHexForNodeType(string type, bool? isCommissionQualified,
            DateTime? autoshipProcessDate, short paidAsTitleID)
        {
            string hex = "#000";//Just default the hex to black incase it doesn't get set to anything..
            if (type == "CommissionQualified")
            {
                string status = Account.GetStatusForCommissionQualified(isCommissionQualified, autoshipProcessDate);
                switch (status)
                {
                    case "Qualified":
                        hex = "#50B948";
                        break;
                    case "Pending":
                        hex = "#FDD48D";
                        break;
                    case "UnQualified":
                        hex = "#F72222";
                        break;
                }
            }
            else
            {
                /*R2594 - CGI(DT-JCT;JICM) Colores dependiendo del PaidAsTitle*/
                //string paidTitle = Translation.GetTerm(_commissionsService.GetTitle(accountTypeID).TermName).Replace(" ", "");

                var paidAsTitle = _commissionsService.GetTitle(paidAsTitleID);

                string paidAsTitleCode = paidAsTitle != null ? paidAsTitle.TitleCode : string.Empty;

                switch (paidAsTitleCode)
                {
                    case "T13":
                        hex = "#F998EF";
                        break;
                    case "T12":
                        hex = "#76B4E2";
                        break;
                    case "T11":
                        hex = "#E5E5E5";
                        break;
                    case "T10":
                        hex = "#8080B9";
                        break;
                    case "T9":
                        hex = "#CAB595";
                        break;
                    case "T8":
                        hex = "#8C8D53";
                        break;
                    case "T7":
                        hex = "#F2F408";
                        break;
                    case "T6":
                        hex = "#195CB0";
                        break;
                    case "T5":
                        hex = "#950BA9";
                        break;
                    case "T4":
                        hex = "#2F2928";
                        break;
                    case "T3":
                        hex = "#22B14C";
                        break;
                    case "T2":
                        hex = "#ED1C24";
                        break;
                    case "T1":
                        hex = "#FF7F27";
                        break;
                }

                //switch (paidAsTitle)
                //{
                //    case "PresidentialBeautyExecutive":
                //        hex = "#F998EF";
                //        break;
                //    case "SeniorExecutiveBeautyVicePresident":
                //        hex = "#76B4E2";
                //        break;
                //    case "ExecutiveBeautyVicePresident":
                //        hex = "#E5E5E5";
                //        break;
                //    case "SeniorBeautyVicePresident":
                //        hex = "#8080B9";
                //        break;
                //    case "BeautyVicePresident":
                //        hex = "#CAB595";
                //        break;
                //    case "SeniorBeautyDirector":
                //        hex = "#8C8D53";
                //        break;
                //    case "BeautyDirector":
                //        hex = "#F2F408";
                //        break;
                //    case "BeautyManager":
                //        hex = "#195CB0";
                //        break;
                //    case "BeautyAdvisor4":
                //        hex = "#950BA9";
                //        break;
                //    case "BeautyAdvisor3":
                //        hex = "#2F2928";
                //        break;
                //    case "BeautyAdvisor2":
                //        hex = "#22B14C";
                //        break;
                //    case "BeautyAdvisor1":
                //        hex = "#ED1C24";
                //        break;
                //    case "BeautyAdvisor":
                //        hex = "#FF7F27";
                //        break;
                //}

            }
            return hex;
        }

        protected override DownlineReportParameters GetDefaultReportParameters()
        {
            return this.GetDefaultReportParameters(null);
        }

        protected DownlineReportParameters GetDefaultReportParameters(int? periodId = null)
        {
            var downlineReportParameters = DefaultDownlineReportParameters();
            int period = periodId.GetValueOrDefault();

            if (period == 0)
            {
                var p = _commissionsService.GetCurrentPeriod();
                if (p != null)
                {
                    period = p.PeriodId;
                }
                else
                {
                    var now = DateTime.UtcNow;
                    downlineReportParameters.PeriodID = (now.Year * 100) + now.Month;
                }
            }

            downlineReportParameters.PeriodID = period;

            return downlineReportParameters;
        }

        protected virtual DownlineReportParameters DefaultDownlineReportParameters()
        {
            return new DownlineReportParameters()
            {
                PageIndex = 0,
                PageSize = 15,
                OrderBy = "AccountNumber",
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
            };
        }

        public virtual bool GroupByDownlineTree(DownlineReportParameters searchParameters)
        {
            return searchParameters.CurrentTopOfTreeAccountID.HasValue || searchParameters.GroupBySponsorTree.ToBool();
        }

        public virtual ReportResults GetReportResults(DownlineReportParameters searchParameters)
        {
            return GetReportResults(searchParameters, true);
        }

        public virtual ReportResults GetReportResults(DownlineReportParameters searchParameters, bool orderBy)
        {
            try
            {
                int rootAccountID = CurrentAccount.AccountID;
                ReportResults results = new ReportResults();

                results.Downline = this.GetDownline(searchParameters.PeriodID, rootAccountID);

                //// results.Downline = this.GetDownline(201409);
                // //************* Developed by WCS - Incidente con Flat Downline ***********************
                // if (searchParameters.CurrentTopOfTreeAccountID.HasValue && Convert.ToInt32(searchParameters.CurrentTopOfTreeAccountID).Equals(rootAccountID)) searchParameters.CurrentTopOfTreeAccountID = null;
                // //************************************************************************************

                // if (searchParameters.CurrentTopOfTreeAccountID.HasValue) 
                //     rootAccountID = searchParameters.CurrentTopOfTreeAccountID.Value;

                if (searchParameters.CurrentTopOfTreeAccountID.HasValue)
                    rootAccountID = searchParameters.CurrentTopOfTreeAccountID.Value;


                results.CurrentNode = results.Downline.GetTree(rootAccountID);

                List<dynamic> newList = new List<dynamic>();

                if (GroupByDownlineTree(searchParameters))
                {
                    int maxLevel = MaxLevel > 0 ? MaxLevel : 100; //Cambio para agrupamiento de niveles (Solución USA - Modificado Juan Morales - CSTI)
                    newList = results.OrderBySponsor(rootAccountID, searchParameters.PeriodID, maxLevel).ToList();
                }
                else
                {
                    //newList = results.CurrentNodeFlatChildrenDetails.ToList();
                    newList.AddRange(results.Downline.Lookup.Values);
                }

                //if (results.Downline.Lookup.ContainsKey(rootAccountID) && !GroupByDownlineTree(searchParameters))
                //{
                //    var currentNodeDetails = results.Downline.Lookup[rootAccountID];
                //    newList.Add(currentNodeDetails);
                //}

                if (newList.Count == 0)
                {
                    results.ResultsDetails = newList;
                    return results;
                }

                IEnumerable<dynamic> tempList = newList;

                if (searchParameters.Titles != null && searchParameters.Titles.Count > 0)
                {
                    tempList = tempList.Where(a => !string.IsNullOrEmpty(a.CurrentTitle) && searchParameters.Titles.Contains(int.Parse(a.CurrentTitle)));
                    //tempList = tempList.Where(a => ((int?)a.CurrentTitle).HasValue && searchParameters.Titles.Contains(((int?)a.CurrentTitle).Value));
                }
                if (searchParameters.AccountTypes != null && searchParameters.AccountTypes.Count > 0)
                {
                    //tempList = tempList.Where(a => !string.IsNullOrEmpty(a.AccountTypeID) && searchParameters.AccountTypes.Contains(int.Parse(a.AccountTypeID)));
                    tempList = tempList.Where(a => ((int?)a.AccountTypeID).HasValue && searchParameters.AccountTypes.Contains(((int?)a.AccountTypeID).Value));
                }

                if (searchParameters.AccountStatuses != null && searchParameters.AccountStatuses.Count > 0)
                {
                    tempList = tempList.Where(a => !string.IsNullOrEmpty(a.AccountStatusID) && searchParameters.AccountStatuses.Contains(int.Parse(a.AccountStatusID)));
                    //tempList = tempList.Where(a => ((int?)a.AccountStatusID).HasValue && searchParameters.AccountStatuses.Contains(((int?)a.AccountStatusID).Value));
                }

                if (searchParameters.States != null && searchParameters.States.Count > 0)
                {
                    var states = searchParameters.States.Select(s => SmallCollectionCache.Instance.StateProvinces.GetById(s).StateAbbreviation);
                    tempList = tempList.Where(a => !string.IsNullOrEmpty(a.State) && states.Any(s => a.State.Contains(s)));
                }

                if (searchParameters.BirthMonth.HasValue)
                {
                    tempList = tempList.Where(a => a.BirthdayUTC.HasValue ? a.BirthdayUTC.Value.Month == searchParameters.BirthMonth.Value : false);
                }

                var matchingItems = tempList.ToGenericList().AsQueryable();
                if (matchingItems.Any())
                {
                    
                    //Filtros Nuevos Level, Generation, DV
                    if (searchParameters.LevelFrom.HasValue && matchingItems.Any())
                    {
                        matchingItems = matchingItems.Where("Level >= @0", searchParameters.LevelFrom.Value);
                    }
                    if (searchParameters.LevelTo.HasValue && matchingItems.Any())
                    {
                        matchingItems =
                            matchingItems.Where(
                                string.Format("Level <= @0{0}", searchParameters.LevelFrom.HasValue ? "" : " || !Level.HasValue"),
                                searchParameters.LevelTo.Value);
                    }

                    if (searchParameters.GenFrom.HasValue && matchingItems.Any())
                    {
                        matchingItems = matchingItems.Where("Generation >= @0", searchParameters.GenFrom.Value);
                    }
                    if (searchParameters.GenTo.HasValue && matchingItems.Any())
                    {
                        matchingItems =
                            matchingItems.Where(
                                string.Format("Generation <= @0{0}", searchParameters.GenFrom.HasValue ? "" : " || !Generation.HasValue"),
                                searchParameters.GenTo.Value);
                    }

                    if (searchParameters.DVFrom.HasValue && matchingItems.Any())
                    {
                        matchingItems = matchingItems.Where("DQV >= @0", searchParameters.DVFrom.Value);
                    }
                    if (searchParameters.DVTo.HasValue && matchingItems.Any())
                    {
                        matchingItems =
                            matchingItems.Where(
                                string.Format("DQV <= @0{0}", searchParameters.DVFrom.HasValue ? "" : " || !DQV.HasValue"),
                                searchParameters.DVTo.Value);
                    }
                    
                    
                    if (searchParameters.StartDate.HasValue && matchingItems.Any())
                    {
                        DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where("JoinDate >= @0", startDateUTC);
                    }
                    if (searchParameters.EndDate.HasValue && matchingItems.Any())
                    {
                        DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where("JoinDate <= @0", endDateUTC);
                    }

                    if (searchParameters.PVFrom.HasValue && matchingItems.Any())
                    {
                        matchingItems = matchingItems.Where("PV >= @0", searchParameters.PVFrom.Value);
                    }
                    if (searchParameters.PVTo.HasValue && matchingItems.Any())
                    {
                        matchingItems =
                            matchingItems.Where(
                                string.Format("PV <= @0{0}", searchParameters.PVFrom.HasValue ? "" : " || !PV.HasValue"),
                                searchParameters.PVTo.Value);
                    }

                    if (searchParameters.GVFrom.HasValue && matchingItems.Any())
                    {
                        matchingItems = matchingItems.Where("GQV >= @0", searchParameters.GVFrom.Value);
                    }
                    if (searchParameters.GVTo.HasValue && matchingItems.Any())
                    {
                        matchingItems =
                            matchingItems.Where(
                                string.Format("GQV <= @0{0}", searchParameters.GVFrom.HasValue ? "" : " || !GQV.HasValue"),
                                searchParameters.GVTo.Value);
                    }

                    //Search by sponsor to get all people sponsored - SCC
                    if (searchParameters.SponsorID.HasValue && searchParameters.SponsorID.Value > 0 && matchingItems.Any())
                    {
                        matchingItems = matchingItems.Where("SponsorID == @0", searchParameters.SponsorID);
                    }

                    //Add months inactive - SCC
                    if (searchParameters.MonthsInactive.HasValue && searchParameters.MonthsInactive.Value > 0 && matchingItems.Any())
                    {
                        matchingItems = matchingItems.Where("LastOrderCommissionDateUTC <= @0",
                                                            DateTime.Today.AddMonths(
                                                                -searchParameters.MonthsInactive.Value).LocalToUTC());
                    }

                    // Search all string columns for SearchValue - JHE
                    if (!searchParameters.SearchValue.IsNullOrEmpty() && matchingItems.Any())
                    {
                        searchParameters.SearchValue = searchParameters.SearchValue.ToCleanString();
                        var searchWords = searchParameters.SearchValue.Split(' ').ToList();

                        foreach (var searchWord in searchWords)
                        {
                            string stringPropertyQuery =
                                results.Downline.DynamicType.GetPropertiesByOrderIfDeclared().Where(
                                    p => p.PropertyType == typeof(string)).Select(
                                        p =>
                                        string.Format("{0} != null && {0}.ToLower().Contains(\"{1}\")", p.Name,
                                                      searchWord.ToLower())).Join(" || ");

                            matchingItems = matchingItems.Where(string.Format("({0})", stringPropertyQuery));
                        }
                    }

                    if (!searchParameters.AccountNumber.IsNullOrEmpty() && matchingItems.Any())
                    {
                        searchParameters.AccountNumber = searchParameters.AccountNumber.ToCleanString();
                        matchingItems = matchingItems.Where("AccountId != null && AccountId == @0",
                                                            searchParameters.AccountNumber);
                    }

                    matchingItems = CustomFilterByPeriodEndDate(matchingItems, searchParameters.PeriodID);

                    if (!searchParameters.CurrentTopOfTreeAccountID.HasValue && !searchParameters.GroupBySponsorTree.ToBool())
                    {
                        if (!searchParameters.OrderBy.IsNullOrEmpty() && matchingItems.Any() && orderBy)
                            matchingItems = searchParameters.OrderByDirection ==
                                            NetSteps.Common.Constants.SortDirection.Ascending
                                                ? DynamicQueryable.OrderBy(matchingItems, searchParameters.OrderBy)
                                                : DynamicQueryable.OrderBy(matchingItems, searchParameters.OrderBy + " DESC");
                    }

                    

                }

                results.TotalCount = matchingItems.Count();

                if (searchParameters.PageSize != null)
                    results.ResultsDetails = matchingItems.Skip(searchParameters.PageIndex * searchParameters.PageSize.ToInt()).Take(searchParameters.PageSize.ToInt());
                else
                    results.ResultsDetails = matchingItems.Take(results.TotalCount);

                return results;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        protected virtual IQueryable CustomFilterByPeriodEndDate(IQueryable query, int periodID)
        {
            return query;
        }

        protected virtual void SetDefaultVisibleColumns(Downline downline, ReportCustomColumns customColumns)
        {
            // Default to showing all columns - JHE
            var param = CurrentReportParameters;
            var columns = GetFlatDownLineColumnsFromWebConfig();

            var returnedColumns = downline.DynamicType.GetProperties().Select(p => p.Name);

            if (customColumns == null)
            {
                if (param.VisibleColumns == null || param.VisibleColumns.Count == 0)
                    param.VisibleColumns = returnedColumns.Any() ? columns.Where(x => returnedColumns.Contains(x)).ToList() : columns;
                //returnedColumns.Any() ? columns.Where(x => returnedColumns.Contains(x)).ToList() : columns;
                else
                {
                    param.VisibleColumns = param.VisibleColumns.Where(p => columns.Contains(p)).Select(p => p).ToList();
                }
            }
            else
            {
                if (customColumns.ContainsAllColumns)
                {
                    columns.Clear();
                    _columnHeaders.Clear();
                }

                //Sort the collection by the provided index, then iterate through the collection
                foreach (AdditionalRow s in customColumns.AdditionalRows.OrderBy(ac => ac.Index))
                {
                    if (!columns.Contains(s.Name))
                    {
                        if (s.Index < columns.Count)
                        {
                            columns.Insert(s.Index, s.Name);
                        }
                        else
                        {
                            columns.Add(s.Name);
                        }
                    }
                    if (!_columnHeaders.ContainsKey(s.Name))
                        _columnHeaders.Add(s.Name, s.NameFriendly);
                }

                param.VisibleColumns = columns.Where(x => returnedColumns.Contains(x) || customColumns.AdditionalRows.Any(a => a.Name == x)).ToList();
            }

            CurrentReportParameters = param;
        }

        private List<string> GetFlatDownLineColumnsFromWebConfig()
        {
            return ConfigurationManager.GetAppSetting<string>(ConfigurationManager.VariableKey.FlatDownlineColumns).Split(';', ',').TrimWhitespace().ToList();
        }

        private List<string> GetDownlineManagementColumnsFromWebConfig()
        {
            string dmc = ConfigurationManager.AppSettings["DownlineManagementColumns"];
            //return ConfigurationManager.GetAppSetting<string>(ConfigurationManager.AppSettings["DownlineManagementColumns"]).Split(';', ',').TrimWhitespace().ToList();
            return dmc.Split(';',',').TrimWhitespace().ToList();
        }

        private IEnumerable<dynamic> PropertyOrderedFromConfig(Downline downline)
        {
            var columns = GetFlatDownLineColumnsFromWebConfig();

            var configPropValues = new Dictionary<int, string>();

            for (int i = 0; i < columns.Count; i++)
            {
                configPropValues.Add(i, columns[i]);
            }

            var orderedProperty = from c in configPropValues
                                  join p in downline.DynamicType.GetProperties() on c.Value.ToLower() equals p.Name.ToLower() into props
                                  from pr in props.DefaultIfEmpty()
                                  orderby c.Key
                                  select new { ColumnName = c.Value, PInfo = pr };
            return orderedProperty;
        }

        private IEnumerable<dynamic> PropertyDownlineManagementFromConfig(Downline downline)
        {
            var columns = GetDownlineManagementColumnsFromWebConfig();

            var configPropValues = new Dictionary<int, string>();

            for (int i = 0; i < columns.Count; i++)
            {
                configPropValues.Add(i, columns[i]);
            }

            var orderedProperty = from c in configPropValues
                                  join p in downline.DynamicType.GetProperties() on c.Value.ToLower() equals p.Name.ToLower() into props
                                  from pr in props.DefaultIfEmpty()
                                  orderby c.Key
                                  select new { ColumnName = c.Value, PInfo = pr };
            return orderedProperty;
        }

        protected virtual void SetColumnNameLookups(Downline downline, bool reset = false)
        {
            if (_columnHeaders != null && _columnHeaders.Count > 0 && !reset)
            {
                return;
            }

            lock (_lock)
            {
                _columnHeaders = new Dictionary<string, string>();

                foreach (dynamic d in PropertyOrderedFromConfig(downline))
                {
                    string columnName = d.ColumnName;
                    PropertyInfo property = d.PInfo;
                    string header, propertyName;
                    if (property != null)
                    {
                        if (property.IsDefined(typeof(DisplayAttribute), false))
                        {
                            DisplayAttribute display = property.GetCustomAttributes(typeof(DisplayAttribute), false).First() as DisplayAttribute;
                            if (property.IsDefined(typeof(TermNameAttribute), false))
                            {
                                TermNameAttribute termName = property.GetCustomAttributes(typeof(TermNameAttribute), false).First() as TermNameAttribute;
                                header = string.IsNullOrEmpty(termName.TermName) ? property.Name.PascalToSpaced() : Translation.GetTerm(termName.TermName, termName.DefaultValue);
                            }
                            else
                                header = string.IsNullOrEmpty(display.Name) ? property.Name.PascalToSpaced() : display.Name;
                        }
                        else
                        {
                            if (property.IsDefined(typeof(TermNameAttribute), false))
                            {
                                TermNameAttribute termName = property.GetCustomAttributes(typeof(TermNameAttribute), false).First() as TermNameAttribute;
                                header = string.IsNullOrEmpty(termName.TermName) ? property.Name.PascalToSpaced() : Translation.GetTerm(termName.TermName, termName.DefaultValue);
                            }
                            else
                                header = property.Name.PascalToSpaced();
                        }
                        if (property.IsDefined(typeof(PropertyNameAttribute), false))
                        {
                            PropertyNameAttribute propName = property.GetCustomAttributes(typeof(PropertyNameAttribute), false).First() as PropertyNameAttribute;
                            propertyName = propName.PropertyName;
                        }
                        else
                            propertyName = property.Name;
                    }
                    else
                    {
                        header = columnName;
                        propertyName = columnName;
                    }

                    _columnHeaders.Add(propertyName, header);
                }
            }
        }
        
        protected virtual void SetColumnNameDownlineManagementLookups(Downline downline, bool reset = false)
        {
            if (_columnHeaders != null && _columnHeaders.Count > 0 && !reset)
            {
                return;
            }

            lock (_lock)
            {
                _columnHeaders = new Dictionary<string, string>();

                foreach (dynamic d in PropertyDownlineManagementFromConfig(downline))
                {
                    string columnName = d.ColumnName;
                    PropertyInfo property = d.PInfo;
                    string header, propertyName;
                    if (property != null)
                    {
                        if (property.IsDefined(typeof(DisplayAttribute), false))
                        {
                            DisplayAttribute display = property.GetCustomAttributes(typeof(DisplayAttribute), false).First() as DisplayAttribute;
                            if (property.IsDefined(typeof(TermNameAttribute), false))
                            {
                                TermNameAttribute termName = property.GetCustomAttributes(typeof(TermNameAttribute), false).First() as TermNameAttribute;
                                header = string.IsNullOrEmpty(termName.TermName) ? property.Name.PascalToSpaced() : Translation.GetTerm(termName.TermName, termName.DefaultValue);
                            }
                            else
                                header = string.IsNullOrEmpty(display.Name) ? property.Name.PascalToSpaced() : display.Name;
                        }
                        else
                        {
                            if (property.IsDefined(typeof(TermNameAttribute), false))
                            {
                                TermNameAttribute termName = property.GetCustomAttributes(typeof(TermNameAttribute), false).First() as TermNameAttribute;
                                header = string.IsNullOrEmpty(termName.TermName) ? property.Name.PascalToSpaced() : Translation.GetTerm(termName.TermName, termName.DefaultValue);
                            }
                            else
                                header = property.Name.PascalToSpaced();
                        }
                        if (property.IsDefined(typeof(PropertyNameAttribute), false))
                        {
                            PropertyNameAttribute propName = property.GetCustomAttributes(typeof(PropertyNameAttribute), false).First() as PropertyNameAttribute;
                            propertyName = propName.PropertyName;
                        }
                        else
                            propertyName = property.Name;
                    }
                    else
                    {
                        header = columnName;
                        propertyName = columnName;
                    }

                    _columnHeaders.Add(propertyName, header);
                }
            }
        }

        protected virtual string GetNodeHtml(Downline downline, int accountID, string childNodesHtml, bool defaultToOpened = false)
        {
            string result;
            if (downline.LookupNode.ContainsKey(accountID) && downline.Lookup.ContainsKey(accountID))
            {
                var parentNode = downline.LookupNode[accountID];
                var parentNodeDetails = downline.Lookup[accountID];

                string cssClass = (parentNode.HasChildren.ToBool()) ? "jstree-open" : "jstree-leaf";
                if (defaultToOpened)
                    cssClass = "jstree-open";

                bool parsedValueIsCommissionQualified = false;
                bool.TryParse(parentNodeDetails.IsCommissionQualified, out parsedValueIsCommissionQualified);

                string status = Account.GetStatusForCommissionQualified(parsedValueIsCommissionQualified, parentNodeDetails.AutoshipProcessDate);
                //string status = Account.GetStatusForCommissionQualified(parentNodeDetails.IsCommissionQualified, parentNodeDetails.AutoshipProcessDate);

                #region PaidAsTitle
                //Developed by WCS - CSTI

                int paidAsTitleID = 0;
                if (downline.Lookup.ContainsKey(Convert.ToInt32(accountID)))
                {
                    var accountNode = downline.Lookup[Convert.ToInt32(accountID)];
                    int.TryParse(accountNode.PaidAsTitle, out paidAsTitleID);
                }
               
                var paidAsTitle = _commissionsService.GetTitle(paidAsTitleID);

                string paidAsTitleClass = paidAsTitle != null ? "acct" + paidAsTitle.TitleCode.Replace("T", "") : string.Empty;

                #endregion
                result = string.Format("<li id=\"{3}\" class=\"{2}\" title=\"{6}\"><a href=\"#\" data-CustomerType=\"{4}\" data-CommissionQualified=\"{5}\">{0}</a>{1}</li>",
                    GetNodeTitle(downline, accountID), childNodesHtml, cssClass,
                    accountID, paidAsTitleClass, status, GetNodeHoverTitle(downline, accountID));
            }
            else
            {
                result = string.Format("<li id=\"{0}\" class=\"jstree-leaf\" title=\"{0} - Missing Data\">Account {0} - Missing Data</li>", accountID);
            }

            return result;
        }

        protected virtual string GetNodeTitle(Downline downline, int accountID)
        {
            var parentNodeDetails = downline.Lookup[accountID];
            string fullName = string.Format("{0} {1}", parentNodeDetails.FirstName, parentNodeDetails.LastName);

            return string.Format("{0} - {1}",  parentNodeDetails.AccountNumber, fullName);
        }

        protected virtual string GetNodeHoverTitle(Downline downline, int accountID)
        {
            var parentNodeDetails = downline.Lookup[accountID];
            string fullName = string.Format("{0} {1}", parentNodeDetails.FirstName, parentNodeDetails.LastName);

            string currentTitle;
            int currentTitleID = 0;
            /*R2594 - CGI(DT-JCT;JICM) - Cambiando de CurrentTitle a PaidAsTitle*/
            //int? currentTitleID = parentNodeDetails.CurrentTitle;

            //if (currentTitleID.HasValue)
            if (parentNodeDetails.CurrentTitle != null)
            {
                int.TryParse(parentNodeDetails.CurrentTitle, out currentTitleID);
                currentTitle = Translation.GetTerm(_commissionsService.GetTitle(currentTitleID).TermName);
                //currentTitle = Translation.GetTerm(_commissionsService.GetTitle(currentTitleID.Value).TermName);
            }
            else
            {
                currentTitle = Translation.GetTerm("N/A");
            }

            return string.Format("{0} - {1}", fullName, currentTitle);
        }

        protected virtual void SetMasterPageViewData()
        {
            ViewData["CurrentAccountReports"] = CurrentAccountReports.Where(r => r.AccountReportTypeID == Constants.AccountReportType.DownlineReport.ToShort()).ToList();
            ViewData["CorporateAccountReports"] = CorporateAccountReports.Where(r => r.AccountReportTypeID == Constants.AccountReportType.DownlineReport.ToShort()).ToList();
            ViewData["CurrentReportParameters"] = CurrentReportParameters;
        }

        [NonAction]
        public virtual string FormatData(Downline downline, int accountID, string columnName, object data, GenealogyHelpers genealogyHelper)
        {
            try
            {
                string result;

                if (data is string)
                {
                    if (columnName.Equals("StatusBA") || columnName.Equals("NewStatus"))
                        result = Translation.GetTerm(5,(string)data, (string)data);
                    else
                        result = genealogyHelper.StringColumnValues(columnName, data);
                }
                else if (data is DateTime)
                {
                    result = genealogyHelper.DateTimeColumnValues(columnName, data);
                }
                else if (data is int)
                {
                    result = genealogyHelper.IntegerColumnValues(downline, accountID, columnName, data);
                    if (columnName.Equals("Level"))
                    {
                        result = genealogyHelper.IntegerColumnValues(downline, accountID, columnName, data);
                        if (Int32.Parse(result) > 0)
                            result = (Int32.Parse(result) - 1).ToString();
                    }
                    else
                        result = genealogyHelper.IntegerColumnValues(downline, accountID, columnName, data);
                }
                else if (data is double)
                {
                    result = genealogyHelper.DoubleColumnValues(data);
                }
                else if (data is decimal)
                {
                    result = genealogyHelper.DecimalColumnValues(data, columnName);
                }
                else if (data is short)
                {
                    result = genealogyHelper.ShortColumnValues(data, columnName);
                }
                else
                    result = (data == null) ? genealogyHelper.DisplayNA() : data.ToString();

                return result;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        protected virtual string FormatData(string dataType, string data, string columnName)
        {
            try
            {
                if (String.IsNullOrWhiteSpace(data))
                    return Translation.GetTerm("N/A");
                if (dataType == "System.String")
                    return Translation.GetTerm(data);
                else if (dataType == "System.Boolean")
                    return data == "True" ? Translation.GetTerm("Yes") : Translation.GetTerm("No");
                else if (dataType == "System.DateTime")
                {
                    DateTime parsedValue = DateTime.Parse(data);
                    return parsedValue.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo);
                }
                else if (dataType == "System.Int32")
                {
                    int parsedValue = Int32.Parse(data);
                    return parsedValue.ToString();
                }
                else if (dataType == "System.Decimal")
                {
                    if (columnName.Contains("Percentage"))
                    {
                        decimal parsedValue = Decimal.Parse(data);
                        return parsedValue.ToPercentageString(CoreContext.CurrentCultureInfo);
                    }
                    else if (columnName.Contains("Volume"))
                    {
                        //decimal parsedValue = Decimal.Parse(data, CoreContext.CurrentCultureInfo);
                        //return parsedValue.ToMoneyString(CoreContext.CurrentCultureInfo);
                        var value = string.IsNullOrEmpty(data) ? (0m).ToString("F2", CoreContext.CurrentCultureInfo) : data;
                        return string.Format("{0}{1}", CoreContext.CurrentCultureInfo.NumberFormat.CurrencySymbol, value);
                    }
                    else
                    {
                        decimal parsedValue = Decimal.Parse(data);
                        return Convert.ToDouble(parsedValue).ToString(System.Globalization.CultureInfo.InvariantCulture);//.TruncateDoubleInsertCommas();
                    }
                }
                else
                    return (data == null) ? Translation.GetTerm("N/A") : data;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
        #endregion

        #region Graphical Downline Viewer

        [FunctionFilter(Functions.PerformanceGraphicalDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GraphicalDownline(int? accountReportID = null)
        {
            try
            {
                ViewData["ColumnHeaders"] = _columnHeaders;
                ViewData["CurrentAccountReports"] = CurrentAccountReports.Where(r => r.AccountReportTypeID == (short)Constants.AccountReportType.DownlineReport).ToList();
                ViewData["CorporateAccountReports"] = CorporateAccountReports.Where(r => r.AccountReportTypeID == (short)Constants.AccountReportType.DownlineReport).ToList();
                ViewData["CurrentReportParameters"] = CurrentReportParameters;

                int accountId = CurrentAccount.AccountID;
                var downline = this.GetDownline(CurrentReportParameters.PeriodID, accountId);

                var accountInfoCardModel = Create.New<IDownlineInfoCardModel>();

                #region SetProperties

                //Developed by WCS - CSTI
                if (!SetProperties(accountId, downline)) { }//return Json(new { result = true, message = Translation.GetTerm("NoPerformanceDataAvailable", "User has no performance data available") });

                #endregion

                DownlineUIService.LoadDownlineInfoCardModelOptions(
                    accountInfoCardModel.OptionsBag,
                    Url.Action("GetDownlineInfoCardData"),
                    Url.Content("~/Communication/Email/Compose?emailAction=BlankEmail&toAddress=")
                );
                DownlineUIService.LoadDownlineInfoCardModelData(
                    accountInfoCardModel.DataBag,
                    rootAccountId: accountId,
                    accountId: accountId
                );

                dynamic model = new DynamicDictionary();
                model.DownlineInfoCard = accountInfoCardModel;
                ViewBag.View = "gv"; //Developed by WCS - CSTI
                GetPaidAsTitle(); /*R2594-CGI(DT-JCT;JICM) Llenando viewbag utilizado en la vista para reenderizar paleta de colores paidastitle*/
                return View(model);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        [FunctionFilter(Functions.PerformanceGraphicalDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult GetDownlineNodes(int rootNodeId, int maxLevels, bool showPCs, bool showConsultants, string type)
        {
            Node root = GetDownline(rootNodeId, maxLevels, showPCs, showConsultants, type);

            if (root != null)
            {
                string uplineBreadCrumb = BuildBreadCrumbOfSponsors(rootNodeId,
                (!showPCs && !showConsultants) ? Constants.TreeType.ECWithRollArounds : Constants.TreeType.Placement);

                return Json(new { downline = root, upline = uplineBreadCrumb });
            }

            return Json(new { result = false, message = Translation.GetTerm("NoDataAvailableForThisUser", "No data available for this user!") });
        }

        protected virtual string BuildBreadCrumbOfSponsors(int downlineId, Constants.TreeType treeType)
        {
            StringBuilder breadCrumb = new StringBuilder();

            var downline = this.GetDownline(CurrentReportParameters.PeriodID, CurrentAccount.AccountID);
            var parentNode = downline.LookupNode[downlineId];

            var downlineNode = parentNode;

            while (downlineNode != null)
            {
                breadCrumb.Insert(0, "<a onclick=\"javascript:rootNodeId = " + downlineNode.AccountID.ToString() + ";showPage();return false;\" href=\"#\" class=\"breadCrumbLink\">" + GetNodeTitle(downline, downlineNode.AccountID) + "</a> > ");

                if (downlineNode.AccountID == CurrentAccount.AccountID)
                {
                    downlineNode = null;
                }
                else
                {
                    downlineNode = downlineNode.Parent;
                }
            }

            return breadCrumb.ToString();
        }

        protected virtual Node GetDownline(int accountID, int maxLevels, bool includePCs, bool includeNonExecutiveConsultants, string type)
        {
            Downline downline = this.GetDownline(CurrentReportParameters.PeriodID, accountID);

            // Ensure that the key exists before accessing it to avoid exceptionzzz...
            if (IsKeyPresentInDictionary(downline.Lookup, accountID) && IsKeyPresentInDictionary(downline.LookupNode, accountID))
            {
                var parentNodeDetails = downline.Lookup[accountID];
                var parentNode = downline.LookupNode[accountID];

                string fullName = GetNodeTitle(downline, accountID);
                string toolTip = GetNodeHoverTitle(downline, accountID);

                List<DownlineNode> childrenNodes = parentNode.Children.ToList();//.Where(n => (n.Level - parentNode.Level) <= 1).ToList();

                Node root = CreateNode(parentNodeDetails, fullName, type, toolTip);

                StartLevel = parentNode.Level;
                MaxLevel = maxLevels;
                ShowPCs = includePCs;

                AppendChildren(root, downline, childrenNodes, 1, type);

                return root;
            }
            return null;
        }

        protected virtual IEnumerable<DownlineNode> CustomFilterByAccountStatus(IEnumerable<DownlineNode> nodes)
        {
            return nodes;
        }

        protected virtual IEnumerable<dynamic> CustomFilterByAccountStatus(IEnumerable<dynamic> matchingItems)
        {
            return matchingItems;
        }

        public virtual void AppendChildren(Node parent, Downline downline, List<DownlineNode> childNodes, int level, string type)
        {
            if (childNodes.Count > 0)
            {
                childNodes = CustomFilterByAccountStatus(childNodes).ToList();

                parent.children = new List<Node>();
                foreach (DownlineNode child in childNodes)
                {
                    var childNodeDetails = downline.Lookup[child.AccountID];
                    string fullName = GetNodeTitle(downline, child.AccountID);
                    string toolTip = GetNodeHoverTitle(downline, child.AccountID);

                    bool include = true;

                    Node c = CreateNode(childNodeDetails, fullName, type, toolTip);

                    switch ((ConstantsGenerated.AccountType)childNodeDetails.AccountTypeID)
                    {
                        case ConstantsGenerated.AccountType.PreferredCustomer:
                            if (!ShowPCs)
                            {
                                include = false;
                            }
                            break;
                        default:
                            break;
                    }

                    if (include)
                    {
                        parent.children.Add(c);

                        if (level < MaxLevel)
                        {
                            AppendChildren(c, downline, child.Children.ToList(), level + 1, type);
                        }
                    }
                }
            }
        }

        [FunctionFilter(Functions.Performance, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult AutoComplete(string query)
        {
            IEnumerable<dynamic> matchingItems = DownlineCache.SearchDownline(CurrentAccount.AccountID, query);

            matchingItems = CustomFilterByAccountStatus(matchingItems);

            var downlineContacts = matchingItems.Select(a => new
            {
                id = (int)a.AccountID,
                text = (string)(a.FirstName + " " + a.LastName) + " (#" + a.AccountNumber + ")"
            });

            return Json(downlineContacts, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [FunctionFilter(Functions.PerformanceSponsorSearch, "~/Performance", Constants.SiteType.BackOffice)]
        public virtual ActionResult SponsorSearch(string query)
        {
            try
            {
                return Json(AccountCache.GetAccountSearchByTextResults(query).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ExpireCache()
        {
            try
            {
                DownlineCache.ExpireCache();
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual string GetDynamicTypeGetterString(Downline downline, string propertyName, int currentAccountID)
        {
            var d1 = downline.DynamicTypeGetters[propertyName];
            var dictionary = downline.Lookup[currentAccountID];
            return d1(dictionary).ToString();
        }

        public virtual bool DynamicTypeGetterContainsString(string stringInQuestion, Downline downline)
        {
            return downline.DynamicTypeGetters.ContainsKey(stringInQuestion);
        }

        protected Dictionary<string, Func<Account, string, string>> _htmlCardFunctions;
        public Dictionary<string, Func<Account, string, string>> HtmlCardFunctions
        {
            get
            {
                if (_htmlCardFunctions == null)
                    InitializeHtmlCardFunctions();
                return _htmlCardFunctions;
            }
        }

        protected virtual void InitializeHtmlCardFunctions()
        {
            _htmlCardFunctions = new Dictionary<string, Func<Account, string, string>>();
        }

        #region Downline Management
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceFlatDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult NewCallDownlineReport()
        {
            return RedirectToAction("DownlineManagement", new { accountReportID = 0 });
        }

        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter(Functions.PerformanceFlatDownline, "~/Performance", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult DownlineManagement(int? accountReportID = null, int? periodID = null)
        {
            if (!accountReportID.HasValue || accountReportID.Value == 0)
            {
                CurrentReportParameters = GetDefaultReportParameters(periodID ?? CurrentReportParameters.PeriodID);
            }

            ViewData["SelectedReport"] = accountReportID.HasValue ? accountReportID.Value.ToString() : "NA";
            ViewData["PeriodID"] = CurrentReportParameters.PeriodID;

            try
            {
                if (accountReportID.HasValue && accountReportID > 0)
                {
                    // TODO: Check that the report belongs to the current account if the not a 'base' report. - JHE
                    var accountReport = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID.ToInt());
                    if (accountReport != null)
                    {
                        CurrentReportParameters = BinarySerializationHelper.Deserialize<DownlineReportParameters>(accountReport.Data);
                        CurrentReportParameters.AccountReportID = accountReportID;

                        ViewData["CurrentAccountReport"] = CurrentAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);
                    }
                    //Corporate Reports - SCC
                    var corporateReport = CorporateAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID.ToInt());
                    if (corporateReport != null)
                    {
                        var curPeriod = CurrentReportParameters.PeriodID;
                        if (curPeriod <= 0)
                        {
                            var per = _commissionsService.GetCurrentPeriod();
                            if (per != null)
                            {
                                curPeriod = per.PeriodId;
                            }
                        }
                        CurrentReportParameters = BinarySerializationHelper.Deserialize<DownlineReportParameters>(corporateReport.Data);
                        CurrentReportParameters.AccountReportID = accountReportID;
                        CurrentReportParameters.PeriodID = curPeriod;

                        // The sponsorID defaults to a specific account, which ultimately filters out valid results.
                        // Setting sponsorID to the current account to avoid this

                        if (corporateReport.Name.Contains("(NEED ID)"))
                            CurrentReportParameters.SponsorID = CurrentAccount.AccountID;

                        ViewData["CorporateAccountReport"] = CorporateAccountReports.FirstOrDefault(r => r.AccountReportID == accountReportID);
                    }
                }

                ReportResults results = GetReportResults(CurrentReportParameters);
                SetColumnNameDownlineManagementLookups(results.Downline, reset: true);

                if (accountReportID == null || accountReportID == 0)
                {
                    SetDefaultVisibleColumns(results.Downline, LoadFlatDownlineCustomColumns());
                }
                else
                {
                    SetDefaultVisibleColumns(results.Downline, LoadFlatDownlineCustomColumns(AccountReport.Load(accountReportID ?? 0)));
                }

                SetMasterPageViewData();

                ViewData["ColumnHeaders"] = _columnHeaders;
                ViewData["LastUpdated"] = results.Downline.LastUpdated;

                return View(results);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }
        #endregion
    }

    public class NodeData
    {
        public string type;
        public string hex;
        public string tooltip;
    }

    public class Node
    {
        public int id;
        public string name;
        public List<Node> children;
        public NodeData data;
    }

    public class AdditionalRow
    {
        public string Name { get; set; }
        public string NameFriendly { get; set; }
        public int Index { get; set; }
        public Func<Account, string, string> Func { get; set; }

        public AdditionalRow(string name, string nameFriendly)
            : this(name, nameFriendly, null)
        {
        }

        public AdditionalRow(string name, string nameFriendly, int? index)
        {
            if (name != null)
                Name = name;
            if (nameFriendly != null)
                NameFriendly = nameFriendly;
            if (index.HasValue)
                Index = index.Value;
        }
    }

    public class ReportCustomColumns
    {
        public ReportCustomColumns()
        {
            AdditionalRows = new List<AdditionalRow>();
        }

        public List<AdditionalRow> AdditionalRows { get; set; }
        public bool ContainsAllColumns { get; set; }
    }
    public class PerformanceExtensions
    {
        public static string GetPercentage(decimal? value)
        {
            if (value.HasValue)
                return NetSteps.Common.Extensions.DecimalExtensions.ToPercentageString(value.Value);

            return string.Empty;
        }

        public static string GetPercentageOnly(decimal? value)
        {
            if (value.HasValue)
                return string.Format("{0:0.#} %", value);

            return string.Empty;
        }
    }
}
