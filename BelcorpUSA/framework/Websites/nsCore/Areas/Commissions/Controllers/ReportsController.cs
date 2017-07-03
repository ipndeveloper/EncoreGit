using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Business;
using NetSteps.Common.Globalization;
using NetSteps.Common.Extensions;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using nsCore.Areas.Commissions.Models;

namespace nsCore.Areas.Commissions.Controllers
{
    public class ReportsController : BaseController
    {
        //
        // GET: /Commissions/Reports/

        protected virtual DataTable CurrentReportView
        {
            get { return Session["CurrentReportView"] as DataTable; }
            set { Session["CurrentReportView"] = value; }
        }

        protected virtual List<string> CurrentParams
        {
            get { return Session["currentParams"] as List<string>; }
            set { Session["currentParams"] = value; }
        }

        public ActionResult KpisPerPeriodGate()
        {
            CurrentParams = null;
            CurrentReportView = null;
            KpisPerPeriodModel KpisPerPeriodModel = new KpisPerPeriodModel();
            KpisPerPeriodModel.Periods = DataAccess.ExecQueryEntidadDictionary("Commissions", "spGetPeriodsForCommissions");
            return View(KpisPerPeriodModel);
        }

        public ActionResult BonusPerPeriodGate()
        {
            CurrentParams = null;
            CurrentReportView = null;
            BonusPerPeriodModel BonusPerPeriodModel = new BonusPerPeriodModel();
            BonusPerPeriodModel.Periods = DataAccess.ExecQueryEntidadDictionary("Commissions", "spGetPeriodsForCommissions");
            BonusPerPeriodModel.BonusTypes = DataAccess.ExecQueryEntidadDictionary("Commissions", "uspGetBonusTypes");
            return View(BonusPerPeriodModel);
        }

        public ActionResult EarningsPerPeriodGate()
        {
            CurrentParams = null;
            CurrentReportView = null;
            EarningsPerPeriodModel EarningsPerPeriodModel = new EarningsPerPeriodModel();
            EarningsPerPeriodModel.Periods = DataAccess.ExecQueryEntidadDictionary("Commissions", "spGetPeriodsForCommissions");
            EarningsPerPeriodModel.BonusTypes = DataAccess.ExecQueryEntidadDictionary("Commissions", "uspGetBonusTypes");
            return View(EarningsPerPeriodModel);
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult KpisPerPeriod(int page,
                                                    int pageSize,
                                                    string orderBy,
                                                    NetSteps.Common.Constants.SortDirection orderByDirection,
                                                    string AccountID,
                                                    string SponsorID,
                                                    int? PeriodStart,
                                                    int? PeriodEnd)
        {
            try
            {
                #region GetDataFromStore

                if (PeriodStart == null || PeriodEnd == null)
                {
                    return Json(new { totalPages = 0, page = "", message = "Error" });
                }

                List<string> paramPeriods = new List<string>();
                paramPeriods.Add(AccountID);
                paramPeriods.Add(SponsorID);
                paramPeriods.Add(PeriodStart.ToString());
                paramPeriods.Add(PeriodEnd.ToString());

                CurrentParams = paramPeriods;

                int TotalPages = 0;
                int RowsCount = 0;

                int accountID = 0;
                int sponsorID = 0;

                int.TryParse(AccountID, out accountID);
                int.TryParse(SponsorID, out sponsorID);

                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsKPIsPerPeriod",
                                            new Dictionary<string, object>() { 
                                                                                {"@GetAll", false},
                                                                                {"@PeriodStart", PeriodStart},
                                                                                {"@PeriodEnd", PeriodEnd},
                                                                                {"@AccountID", accountID},
                                                                                {"@SponsorID", sponsorID},
                                                                                {"@PageSize", pageSize},
                                                                                {"@PageNumber", page},
                                                                                {"@Colum", orderBy},
                                                                                {"@Order", Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"}
                                                                            },"Commissions"));

                CurrentReportView = ds.Tables[0];
                int.TryParse(ds.Tables[1].Rows[0][0].ToString(), out RowsCount);
                TotalPages = ((RowsCount - 1) / pageSize) + 1;

                DataView dv = new DataView(ds.Tables[0]);
                CurrentReportView = dv.ToTable();
                #endregion

                var HTMLrows = dv.BuildGridTable(0, pageSize);

                return Json(new { result = true, totalPages = TotalPages, page = HTMLrows });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult KpisPerPeriodExport()
        {
            try
            {
                int PeriodStart = 0;
                int PeriodEnd = 0;
                int AccountID = 0;
                int SponsorID = 0;

                if (CurrentParams != null && CurrentParams.Count == 4)
                {
                    int.TryParse(CurrentParams[0], out AccountID);
                    int.TryParse(CurrentParams[1], out SponsorID);
                    int.TryParse(CurrentParams[2], out PeriodStart);
                    int.TryParse(CurrentParams[3], out PeriodEnd);
                }


                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsKPIsPerPeriod",
                                            new Dictionary<string, object>() { 
                                                                                {"@GetAll", true},
                                                                                {"@PeriodStart", PeriodStart},
                                                                                {"@PeriodEnd", PeriodEnd},
                                                                                {"@AccountID", AccountID},
                                                                                {"@SponsorID", SponsorID},
                                                                                {"@PageSize", 0},
                                                                                {"@PageNumber", 0},
                                                                                {"@Colum", string.Empty},
                                                                                {"@Order", "ASC"}
                                                                            }, "Commissions"));

                DataTable table = ds.Tables[0];

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("KPIsPerPeriodExport", "KPIs_Per_Period_Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<KPIsPerPeriodSearchData> data = new PaginatedList<KPIsPerPeriodSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    KPIsPerPeriodSearchData itemRow = new KPIsPerPeriodSearchData();
                    object[] values = row.ItemArray;
                    itemRow.PeriodID = Convert.ToString(values[0]);
                    itemRow.AccountID = Convert.ToString(values[1]);
                    itemRow.AccountName = Convert.ToString(values[2]);
                    itemRow.SponsorID = Convert.ToString(values[3]);
                    itemRow.SponsorName = Convert.ToString(values[4]);
                    itemRow.PaidAsCurrentMonth = Convert.ToString(values[5]);
                    itemRow.CareerTitle = Convert.ToString(values[6]);
                    itemRow.PQV = Convert.ToString(values[7]);
                    itemRow.PCV = Convert.ToString(values[8]);
                    itemRow.DQV = Convert.ToString(values[9]);
                    itemRow.CQL = Convert.ToString(values[10]);
                    itemRow.Title1Legs = Convert.ToString(values[11]);
                    itemRow.Title2Legs = Convert.ToString(values[12]);
                    itemRow.Title3Legs = Convert.ToString(values[13]);
                    itemRow.Title4Legs = Convert.ToString(values[14]);
                    itemRow.Title5Legs = Convert.ToString(values[15]);
                    itemRow.Title6Legs = Convert.ToString(values[16]);
                    itemRow.Title7Legs = Convert.ToString(values[17]);
                    itemRow.Title8Legs = Convert.ToString(values[18]);
                    itemRow.Title9Legs = Convert.ToString(values[19]);
                    itemRow.Title10Legs = Convert.ToString(values[20]);
                    itemRow.Title11Legs = Convert.ToString(values[21]);
                    itemRow.Title12Legs = Convert.ToString(values[22]);
                    itemRow.Title13Legs = Convert.ToString(values[23]);
                    itemRow.Title14Legs = Convert.ToString(values[24]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"PeriodID", Translation.GetTerm("CommissionReportPeriodID","Period")},
                    {"AccountID", Translation.GetTerm("CommissionReportAccountID","Account Number")},
                    {"AccountName", Translation.GetTerm("CommissionReportAccountName","Account Name")},
                    {"SponsorID", Translation.GetTerm("CommissionReportSponsorID","Sponsor ")},
                    {"SponsorName", Translation.GetTerm("CommissionReportSponsorName","Sponsor Name")},
                    {"PaidAsCurrentMonth", Translation.GetTerm("CommissionReportPaidAsCurrentMonth","Paid-As Title")},
                    {"CareerTitle", Translation.GetTerm("CommissionReportCareerTitle","Career Title")},
                    {"PQV", Translation.GetTerm("CommissionReportPQV","PQV")},
                    {"PCV", Translation.GetTerm("CommissionReportPCV","PCV")},
                    {"DQV", Translation.GetTerm("CommissionReportDQV","DQV")},
                    {"CQL", Translation.GetTerm("CommissionReportCQL","CQL")},
                    {"Title1Legs", Translation.GetTerm("CommissionReportTitle1Legs","C0")},
                    {"Title2Legs", Translation.GetTerm("CommissionReportTitle2Legs","C1")},
                    {"Title3Legs", Translation.GetTerm("CommissionReportTitle3Legs","C2")},
                    {"Title4Legs", Translation.GetTerm("CommissionReportTitle4Legs","C3")},
                    {"Title5Legs", Translation.GetTerm("CommissionReportTitle5Legs","M1")},
                    {"Title6Legs", Translation.GetTerm("CommissionReportTitle6Legs","M2")},
                    {"Title7Legs", Translation.GetTerm("CommissionReportTitle7Legs","M3")},
                    {"Title8Legs", Translation.GetTerm("CommissionReportTitle8Legs","L1")},
                    {"Title9Legs", Translation.GetTerm("CommissionReportTitle9Legs","L2")},
                    {"Title10Legs", Translation.GetTerm("CommissionReportTitle10Legs","L3")},
                    {"Title11Legs", Translation.GetTerm("CommissionReportTitle11Legs","L4")},
                    {"Title12Legs", Translation.GetTerm("CommissionReportTitle12Legs","L5")},
                    {"Title13Legs", Translation.GetTerm("CommissionReportTitle13Legs","L6")},
                    {"Title14Legs", Translation.GetTerm("CommissionReportTitle14Legs","L7")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<KPIsPerPeriodSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult BonusPerPeriod(int page,
                                                    int pageSize,
                                                    string orderBy,
                                                    NetSteps.Common.Constants.SortDirection orderByDirection,
                                                    int? BonusTypeID,
                                                    int? PeriodStart,
                                                    int? PeriodEnd)
        {
            try
            {
                #region GetDataFromStore

                if (PeriodStart == null || PeriodEnd == null)
                {
                    return Json(new { totalPages = 0, page = "", message = "Error" });
                }

                DataTable table = null;
                DataSet ds = null;

                List<string> paramPeriods = new List<string>();
                paramPeriods.Add(BonusTypeID.ToString());
                paramPeriods.Add(PeriodStart.ToString());
                paramPeriods.Add(PeriodEnd.ToString());

                if (CurrentParams != null && CurrentReportView != null)
                    if (paramPeriods.SequenceEqual((List<string>)CurrentParams)) table = CurrentReportView.Copy();

                CurrentParams = paramPeriods;

                if (table != null && table.Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(table);
                }
                else
                {
                    ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsBonusPerPeriod",
                                                    new Dictionary<string, object>() { {"@PeriodStart", PeriodStart},
                                                                                      {"@PeriodEnd", PeriodEnd},
                                                                                      {"@BonusTypeID", BonusTypeID ?? 0}  },
                                                                                       "Commissions"));
                    CurrentReportView = ds.Tables[0];
                }

                DataView dv = new DataView(ds.Tables[0], string.Empty, string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);
                CurrentReportView = dv.ToTable();
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
        public virtual ActionResult BonusPerPeriodExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("BonusPerPeriodExport", "Bonus_Per_Period_Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<BonusPerPeriodSearchData> data = new PaginatedList<BonusPerPeriodSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    BonusPerPeriodSearchData itemRow = new BonusPerPeriodSearchData();
                    object[] values = row.ItemArray;
                    itemRow.PeriodID = Convert.ToString(values[0]);
                    itemRow.BonusName = Convert.ToString(values[1]);
                    itemRow.Amount = Convert.ToString(values[2]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"PeriodID", Translation.GetTerm("CommissionReportPeriodID","Period")},
                    {"BonusName", Translation.GetTerm("CommissionReportBonusName","Bonus Name")},
                    {"Amount", Translation.GetTerm("CommissionReportAmount","Amount")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<BonusPerPeriodSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult EarningsPerPeriod(int page,
                                                    int pageSize,
                                                    string orderBy,
                                                    NetSteps.Common.Constants.SortDirection orderByDirection,
                                                    int? BonusTypeID,
                                                    string AccountID,
                                                    int? PeriodStart,
                                                    int? PeriodEnd)
        {
            try
            {
                #region GetDataFromStore

                if (PeriodStart == null || PeriodEnd == null)
                {
                    return Json(new { totalPages = 0, page = "", message = "Error" });
                }

                DataTable table = null;
                DataSet ds = null;

                List<string> paramPeriods = new List<string>();
                paramPeriods.Add(BonusTypeID.ToString());
                paramPeriods.Add(AccountID);
                paramPeriods.Add(PeriodStart.ToString());
                paramPeriods.Add(PeriodEnd.ToString());

                if (CurrentParams != null && CurrentReportView != null)
                    if (paramPeriods.SequenceEqual((List<string>)CurrentParams)) table = CurrentReportView.Copy();

                CurrentParams = paramPeriods;

                if (table != null && table.Rows.Count > 0)
                {
                    ds = new DataSet();
                    ds.Tables.Add(table);
                }
                else
                {
                    ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsEarningsPerPeriod",
                                                    new Dictionary<string, object>() { {"@PeriodStart", PeriodStart},
                                                                                      {"@PeriodEnd", PeriodEnd},
                                                                                      {"@BonusTypeID", BonusTypeID ?? 0}  },
                                                                                       "Commissions"));
                    CurrentReportView = ds.Tables[0];
                }

                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                if (!string.IsNullOrEmpty(AccountID)) FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = AccountID, type = eFilterType.Int, IsRank = false });
                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);
                CurrentReportView = dv.ToTable();
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
        public virtual ActionResult EarningsPerPeriodExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("EarningsPerPeriodExport", "Earnings_Per_Period_Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<EarningsPerPeriodSearchData> data = new PaginatedList<EarningsPerPeriodSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    EarningsPerPeriodSearchData itemRow = new EarningsPerPeriodSearchData();
                    object[] values = row.ItemArray;
                    itemRow.PeriodID = Convert.ToString(values[0]);
                    itemRow.BonusName = Convert.ToString(values[1]);
                    itemRow.AccountNumber = Convert.ToString(values[2]);
                    itemRow.AccountName = Convert.ToString(values[3]);
                    itemRow.Amount = Convert.ToString(values[4]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"PeriodID", Translation.GetTerm("CommissionReportPeriodID","Period")},
                    {"BonusName", Translation.GetTerm("CommissionReportBonusName","Bonus Name")},
                    {"AccountNumber", Translation.GetTerm("CommissionReportAccountID","Account Number")},
                    {"AccountName", Translation.GetTerm("CommissionReportAccountName","Account Name")},
                    {"Amount", Translation.GetTerm("CommissionReportAmount","Amount")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<EarningsPerPeriodSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        private SqlCommand GetCommand(string StoreProcedureName, string connectionStringName)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = StoreProcedureName;
            return cmd;
        }

    }
}
