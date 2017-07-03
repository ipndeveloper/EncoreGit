using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using nsCore.Controllers;
using NetSteps.Web.Mvc.Helpers;
using System.Data;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Common.Base;
using NetSteps.Common.Globalization;
using NetSteps.Common.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.MLMQueries.Controllers
{
    public class MLMQueriesController : BaseController
    {
        //
        // GET: /MLMQueries/MLMQueries/

        protected virtual DataTable CurrentMLMReportView
        {
            get { return Session["CurrentMLMReportView"] as DataTable; }
            set { Session["CurrentMLMReportView"] = value; }
        }

        public ActionResult Index()
        {
            return View();
        }

        #region MLMQueries
        //Developed by Jordan Cruz T. - CGI

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult MLMQueriesReport(int page,
                                                     int pageSize,
                                                     string careerTitle,
                                                     string paidAsTitle,
                                                     int periods,
                                                     int indicators,
                                                     string accountNumber,
                                                     string accountID,
                                                     string orderBy,
                                                     NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("usp_getAccountKPIS",
                                                                         new Dictionary<string, object>() { {"@AccountNumber", accountNumber == ""? null : accountNumber},
                                                                         {"@AccountID", accountID == "" ? null : accountID},
                                                                         {"@Period", periods},
                                                                         {"@CTitle", careerTitle},
                                                                         {"@PTitle", paidAsTitle},
                                                                         {"@Indicator", indicators}},
                                                                         "Commissions"));
                DataView dv = new DataView(ds.Tables[0], "", string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);
                CurrentMLMReportView = dv.ToTable();
                #endregion

                return Json(new { totalPages = dv.TotalPages(pageSize), page = dv.BuildGridTable(page, pageSize) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult MLMQueriesExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentMLMReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("MLMAccountKPIsExport", "AccountKPIs Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<AccountKPIsSearchData> data = new PaginatedList<AccountKPIsSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    AccountKPIsSearchData itemRow = new AccountKPIsSearchData();
                    object[] values = row.ItemArray;
                    itemRow.AccountNumber = Convert.ToString(values[0]);
                    itemRow.Name = Convert.ToString(values[1]);
                    itemRow.Period = Convert.ToInt32(values[2]);
                    itemRow.CareerTitle = Convert.ToString(values[3]);
                    itemRow.PaidAsTitle = Convert.ToString(values[4]);
                    itemRow.Indicator = Convert.ToString(values[5]);
                    itemRow.Amount = Convert.ToDouble(values[6]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"AccountNumber", Translation.GetTerm("MLMColAccountNumber")},
                    {"Name", Translation.GetTerm("MLMColName","Name")},
                    {"Period", Translation.GetTerm("MLMColPeriod", "Period")},
					{"CareerTitle", Translation.GetTerm("MLMColCareerTitle","CareerTitle")},
					{"PaidAsTitle", Translation.GetTerm("MLMColPaidAsTitle","PaidAsTitle")},
                    {"Indicator", Translation.GetTerm("MLMColIndicator", "Indicator")},
                    {"Amount", Translation.GetTerm("MLMColAmount", "Amount")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<AccountKPIsSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion
    }
}
