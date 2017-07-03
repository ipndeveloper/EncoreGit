using System;
using System.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Diagnostics.Utilities;
using nsCore.Areas.Products.Models;
using System.Diagnostics.Contracts;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Services;
using nsCore.Controllers;
using System.Data;
using NetSteps.Common.Base;
using System.Data.SqlClient;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using nsCore.Areas.Reports.Models;
using System.Net;
using Microsoft.Reporting.WebForms;
using System.Collections;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using System.Web.Script.Serialization;

namespace nsCore.Areas.Reports.Controllers
{
    #region Modifications
    /*
        @01 BR-RM-003 Commissions Reports
        @02 BR-CC-017 Reporte Estado Mensual de Boletas de Pago
    */
    #endregion

    public class ReportsController : BaseController
    {
        //
        // GET: /Reports/Reports/
        protected virtual List<Product> AllFullProducts
        {
            get { return Session["AllFullProducts"] as List<Product>; }
            set { Session["AllFullProducts"] = value; }
        }

        protected virtual List<ProductSlimSearchData> AllProducts
        {
            get { return Session["AllSlimProducts"] as List<ProductSlimSearchData>; }
            set { Session["AllSlimProducts"] = value; }
        }
        protected virtual List<ProductBaseSearchData> AllBaseProducts
        {
            get { return Session["AllBaseProducts"] as List<ProductBaseSearchData>; }
            set { Session["AllBaseProducts"] = value; }
        }

        protected virtual List<WarehouseProduct> AllWarehouseProducts
        {
            get { return Session["AllWarehouseProducts"] as List<WarehouseProduct>; }
            set { Session["AllWarehouseProducts"] = value; }
        }


        protected virtual DataTable CurrentReportView
        {
            get { return Session["CurrentReportView"] as DataTable; }
            set { Session["CurrentReportView"] = value; }
        }

        public ActionResult Index()
        {
            CurrentReportView = new DataTable();
            return View();
        }


        /// <summary>
        /// Category view.
        /// </summary>
        /// <param name="id"><see cref="int">Category id.</see></param>
        /// <returns><see cref="ActionResult">Action Result</see></returns>
        public ActionResult Category(int? id)
        {
            ReportCategory category = ReportService.GetCategory(id);
            return View(category);
        }


        #region exportGenericGridv // Desarrollado por WV: 20160420

        public void exportGenericGridw(DataTable dt, string fileNameSave)
        {
            // Se cambia para realizar el proceso de Exportación Generico
            // WV: 20160420

            fileNameSave = string.Format("{0}_{1}.xlsx", fileNameSave, DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

            string attachment = "attachment; filename=" + fileNameSave;
            Response.BufferOutput = true;
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/vnd.ms-excel";
            string tab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                System.Web.HttpContext.Current.Response.Write(tab + dc.ColumnName);
                tab = "\t";
            }
            Response.Write("\n");
            int i;
            foreach (DataRow dr in dt.Rows)
            {
                tab = "";
                for (i = 0; i < dt.Columns.Count; i++)
                {
                    System.Web.HttpContext.Current.Response.Write(tab + dr[i].ToString());
                    tab = "\t";
                }
                Response.Write("\n");
            }
            Response.Flush();
            Response.SuppressContent = true;
        }


        #endregion


        #region PDF

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GenerarReportePDF(string orderNumber)
        {
            List<string> OrderList = new List<string>();

            OrderList.Add(orderNumber);

            return new NetSteps.Web.Mvc.ActionResults.PdfResult(string.Format("Order{0}.pdf", orderNumber), Pdf.GeneratePDFMemoryStream(OrderList));

        }

        #endregion

        #region Methods

        private SqlCommand GetCommand(string StoreProcedureName, string connectionStringName)
        {
            ////solo para pruebas
            //string connectionString = string.Empty;
            //if (connectionStringName.Equals("Commissions"))
            //    connectionString = "Data Source=BelcorpUSADatabase;Initial Catalog=BelcorpUSACommissions;Persist Security Info=True;Integrated Security=false;Application Name=GMP;MultipleActiveResultSets=True;uid=usr_encore;pwd=eNc0r3";
            //else
            //    connectionString = "Data Source=BelcorpUSADatabase;Initial Catalog=BelcorpUSACore;Persist Security Info=True;Integrated Security=false;Application Name=GMP;MultipleActiveResultSets=True;uid=usr_encore;pwd=eNc0r3";
            //SqlConnection con = new SqlConnection(connectionString);
            ////fin solo para pruebas

            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = StoreProcedureName;
            return cmd;
        }

        /// <summary>
        /// Get DailyOrders
        /// </summary>
        /// <param name="filters">filters[0] - Consultant,filters[1] - CompleteDate,filters[2] - CreatedDate,filters[3] - ShipmentDate,filters[4] - Status,filters[5] - OrderType</param>
        /// <returns></returns>
        private DataTable GetDailyOrders(string inSQL, string connectionStringName, List<object> filters)
        {
            DataSet ds = DataAccess.GetDataSet(inSQL, connectionStringName);
            string expression = string.Empty;
            string sortOrder = string.Empty;

            for (int i = 0; i < filters.Count; i++)
            {
                if (Convert.ToString(filters[i]) != "")
                {
                    switch (i)
                    {
                        //Consultant
                        case 0:
                            expression += string.Format("{0} LIKE '%{1}%' AND ", String.Concat("[", ds.Tables[0].Columns[1].ColumnName, "]"), Convert.ToString(filters[i]));
                            break;
                        //CompleteDate
                        case 1:
                            expression += string.Format("{0} >= '{1}' and {2} <= '{3}' AND ",
                                                        String.Concat("[", ds.Tables[0].Columns[7].ColumnName, "]"),
                                                        Convert.ToString(filters[i]),
                                                        String.Concat("[", ds.Tables[0].Columns[7].ColumnName, "]"),
                                                        Convert.ToString(Convert.ToDateTime(filters[i]).AddDays(1))
                                                        );
                            break;
                        //CreatedDate
                        case 2:
                            expression += string.Format("{0} >= '{1}' and {2} <= '{3}' AND ",
                                                        String.Concat("[", ds.Tables[0].Columns[8].ColumnName, "]"),
                                                        Convert.ToString(filters[i]),
                                                        String.Concat("[", ds.Tables[0].Columns[8].ColumnName, "]"),
                                                        Convert.ToString(Convert.ToDateTime(filters[i]).AddDays(1))
                                                        );
                            break;
                        //ShipmentDate
                        case 3:
                            expression += string.Format("{0} >= '{1}' and {2} <= '{3}' AND ",
                                                        String.Concat("[", ds.Tables[0].Columns[6].ColumnName, "]"),
                                                        Convert.ToString(filters[i]),
                                                        String.Concat("[", ds.Tables[0].Columns[6].ColumnName, "]"),
                                                        Convert.ToString(Convert.ToDateTime(filters[i]).AddDays(1))
                                                        );
                            break;
                        //Status
                        case 4:
                            expression += string.Format("{0} = '{1}' AND ", String.Concat("[", ds.Tables[0].Columns[10].ColumnName, "]"), Convert.ToString(filters[i]));
                            break;
                        //OrderType
                        case 5:
                            expression += string.Format("{0} = '{1}'", String.Concat("[", ds.Tables[0].Columns[9].ColumnName, "]"), Convert.ToString(filters[i]));
                            break;
                        default:
                            break;
                    }
                }
            }
            if (expression.EndsWith("AND ") && expression.Length > 0) expression += "1=1";
            DataView dv = new DataView(ds.Tables[0], expression, "", DataViewRowState.CurrentRows);
            this.CurrentReportView = dv.ToTable();
            return dv.ToTable("DailyOrdersReport");
        }

        #endregion

        #region DailyPaymentsReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult DailyPaymentsReportGate()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);

            ViewBag.Yesterday = yesterday.Month.ToString() + "/" + yesterday.Day.ToString() + "/" + yesterday.Year.ToString();

            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetDailyPaymentsReport(string status,
                                                           string orderNumber,
            //string completeDateIni,
                                                           DateTime? completeDateIni,
            //string completeDateFin,
                                                           DateTime? completeDateFin,
                                                           string orderTotalIni,
                                                           string orderTotalFin,
                                                           string totalPaidOrderIni,
                                                           string totalPaidOrderFin,
                                                           string totalBalanceIni,
                                                           string totalBalanceFin,
                                                           DateTime? paymentDateIni,
                                                           DateTime? paymentDateFin,
                                                           string paymentAmountIni,
                                                           string paymentAmountFin,
                                                           int page,
                                                           int pageSize,
                                                           string orderBy,
                                                           NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                completeDateIni = completeDateIni == null ? DateTime.Now.AddDays(-1) : completeDateIni;

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsDailyPayments", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                if (!string.IsNullOrEmpty(status)) FilterList.Add(new FilterSearchData() { IndexColumn = 4, Value = status, type = eFilterType.String, IsRank = false });
                if (!string.IsNullOrEmpty(orderNumber)) FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = orderNumber, type = eFilterType.String, IsRank = false });

                //FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = completeDateIni.DateFormat(), type = eFilterType.DateTime, IsRank = true });
                //FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = completeDateFin.DateFormat(), type = eFilterType.DateTime, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = Convert.ToString(completeDateIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = Convert.ToString(completeDateFin), type = eFilterType.DateTime, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = orderTotalIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = orderTotalFin, type = eFilterType.Decimal, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 6, Value = totalPaidOrderIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 6, Value = totalPaidOrderFin, type = eFilterType.Decimal, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 7, Value = totalBalanceIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 7, Value = totalBalanceFin, type = eFilterType.Decimal, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 8, Value = Convert.ToString(paymentDateIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 8, Value = Convert.ToString(paymentDateFin), type = eFilterType.DateTime, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 10, Value = paymentAmountIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 10, Value = paymentAmountFin, type = eFilterType.Decimal, IsRank = true });

                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);
                //PaginatedList<DailyPaymentSearchData> DailyPayment;
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
        public virtual ActionResult DailyPaymentsExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("DailyPaymentsExport", "Daily Payments Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<DailyPaymentSearchData> data = new PaginatedList<DailyPaymentSearchData>();
                foreach (System.Data.DataRow row in table.Rows)
                {
                    DailyPaymentSearchData itemRow = new DailyPaymentSearchData();
                    object[] values = row.ItemArray;
                    itemRow.OrderNumber = Convert.ToString(values[0]);
                    itemRow.OrderShipmentID = Convert.ToString(values[1]);
                    itemRow.CompleteDate = Convert.ToString(values[2]);
                    itemRow.OrderType = Convert.ToString(values[3]);
                    itemRow.OrderStatus = Convert.ToString(values[4]);
                    itemRow.GrandTotal = Convert.ToString(values[5]);
                    itemRow.PaymentTotal = Convert.ToString(values[6]);
                    itemRow.Balance = Convert.ToString(values[7]);
                    itemRow.DateCreated = Convert.ToString(values[8]);
                    itemRow.PaymentType = Convert.ToString(values[9]);
                    itemRow.Amount = Convert.ToString(values[10]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"OrderNumber", Translation.GetTerm("OrderNumber", "OrderNumber")},
                    {"OrderShipmentID", Translation.GetTerm("OrderShipmentID")},
					{"CompleteDate", Translation.GetTerm("CompleteDate","CompleteDate")},
					{"OrderType", Translation.GetTerm("OrderType","OrderType")},
					{"OrderStatus", Translation.GetTerm("OrderStatus","OrderStatus")},
					{"GrandTotal", Translation.GetTerm("GrandTotal","GrandTotal")},
					{"PaymentTotal", Translation.GetTerm("PaymentTotal","PaymentTotal")},
					{"Balance", Translation.GetTerm("Balance", "Balance")},
                    {"DateCreated", Translation.GetTerm("DateCreated", "DateCreated")},
                    {"PaymentType", Translation.GetTerm("PaymentType", "PaymentType")},
                    {"Amount", Translation.GetTerm("Amount", "Amount")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<DailyPaymentSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region DailyOrdersReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult DailyOrdersReportGate()
        {
            DateTime yesterday = DateTime.Now.AddDays(-1);

            ViewBag.Yesterday = yesterday.Month.ToString() + "/" + yesterday.Day.ToString() + "/" + yesterday.Year.ToString();

            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetDailyOrdersReport(string accountNumber,
                                                         string name,
                                                         string customerNumber,
                                                         string customerName,
                                                         string orderNumber,
            //string completeDateUTCIni,
                                                         DateTime? completeDateUTCIni,
            //string completeDateUTCFin,
                                                         DateTime? completeDateUTCFin,
                                                         decimal? subtotalIni,
                                                         decimal? subtotalFin,
                                                         decimal? grandTotalIni,
                                                         decimal? grandTotalFin,
                                                         int page,
                                                         int pageSize,
                                                         string orderBy,
                                                         string orderByDirection)
        {
            try
            {
                //completeDateUTCIni = completeDateUTCIni == null ? DateTime.Now.AddDays(-1) : completeDateUTCIni;

                var orders = Order.SearchDialyOrders(new DailyOrderSearchParameters()
                {
                    GetAll = false,
                    LanguageID = CurrentLanguageID,
                    AccountName = name,
                    AccountNumber = accountNumber,
                    OrderNumber = orderNumber,
                    CompleteDateStart = completeDateUTCIni,
                    CompleteDateEnd = completeDateUTCFin,
                    SubTotalMin = subtotalIni,
                    SubTotalMax = subtotalFin,
                    GrandTotalMin = grandTotalIni,
                    GrandTotalMax = grandTotalFin,
                    PageSize = pageSize,
                    PageIndex = page,
                    OrderBy = orderBy
                });

                StringBuilder builder = new StringBuilder();
                foreach (var managamentledger in orders)
                {
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.OrderNumber));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.RepNumber));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.RepName));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.CustomerNumber));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.CustomerName));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.CustomerType));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.Sponsor));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.OrderShipmentID));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.DateShippedUTC));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.CompleteDateUTC));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.DateCreatedUTC));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.OrderType));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.OrderStatus));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.QV));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.CV));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.Price));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.State));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.SubTotal));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.ShippingTotal));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.HandlingTotal));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.TaxAmountTotal));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.GrandTotal));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.HasStarterKit));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.StarterKitPrice));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.City));
                    builder.Append("</tr>");
                }

                return Json(new { result = true, totalPages = orders.TotalPages, page = orders.TotalCount == 0 ? String.Format("<tr><td colspan=\"25\">{0}</td></tr>", Translation.GetTerm("NoOrders", "No orders were found")) : builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult DailyOrdersExport(string AccountName,
                                                      string AccountNumber,
                                                      string OrderNumber,
                                                      DateTime? CompleteDateStart,
                                                      DateTime? CompleteDateEnd,
                                                      decimal? SubTotalMin,
                                                      decimal? SubTotalMax,
                                                      decimal? GrandTotalMin,
                                                      decimal? GrandTotalMax)
        {
            try
            {

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("DailyOrdersExport", "DailyOrders Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

                var data = Order.SearchDialyOrders(new DailyOrderSearchParameters()
                {
                    GetAll = true,
                    LanguageID = CurrentLanguageID,
                    AccountName = AccountName,
                    AccountNumber = AccountNumber,
                    OrderNumber = OrderNumber,
                    CompleteDateStart = CompleteDateStart,
                    CompleteDateEnd = CompleteDateEnd,
                    SubTotalMin = SubTotalMin,
                    SubTotalMax = SubTotalMax,
                    GrandTotalMin = GrandTotalMin,
                    GrandTotalMax = GrandTotalMax
                });

                var columns = new Dictionary<string, string>
				{
					{"OrderNumber", Translation.GetTerm("OrderNumber", "Order Number")},
                    {"RepNumber", Translation.GetTerm("RepNumber", "Rep Number")},
					{"RepName", Translation.GetTerm("RepName","Rep Name")},
					{"CustomerNumber", Translation.GetTerm("CustomerNumber","Customer Number")},
					{"CustomerName", Translation.GetTerm("CustomerName","Customer Name")},
					{"CustomerType", Translation.GetTerm("CustomerType","Customer Type")},
					{"Sponsor", Translation.GetTerm("Sponsor","Sponsor")},
					{"OrderShipmentID", Translation.GetTerm("OrderShipmentID", "OrderShipment ID")},
                    {"DateShippedUTC", Translation.GetTerm("ShipmentDate", "Shipment Date")},
                    {"CompleteDateUTC", Translation.GetTerm("CompleteDate", "Complete Date")},
                    {"DateCreatedUTC", Translation.GetTerm("DateCreated", "Date Created")},
                    {"OrderType", Translation.GetTerm("OrderType", "Order Type")},
                    {"OrderStatus", Translation.GetTerm("OrderStatus", "Order Status")},
                    {"QV", Translation.GetTerm("QV", "QV")},
                    {"CV", Translation.GetTerm("CV", "CV")},
                    {"Price", Translation.GetTerm("Price", "Price")},
                    {"State", Translation.GetTerm("State", "State")},
                    {"SubTotal", Translation.GetTerm("SubTotal", "SubTotal")},
                    {"ShippingTotal", Translation.GetTerm("Shipping", "Shipping")},
                    {"HandlingTotal", Translation.GetTerm("Handling", "Handling")},
                    {"TaxAmountTotal", Translation.GetTerm("Tax", "Tax")},
                    {"GrandTotal", Translation.GetTerm("Total", "Total")},
                    {"HasStarterKit", Translation.GetTerm("HasStarterKit", "Has Starter Kit")},
                    {"StarterKitPrice", Translation.GetTerm("StarterKitPrice", "Starter Kit Price")},
                    {"City", Translation.GetTerm("City", "City")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<DailyOrderSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region EnrollmentsReport
        //Developed by Luis Peña V. - CSTI

        public ActionResult EnrollmentsReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetEnrollmentsReport(string month,
                                                         string state,
                                                         string newEnrollments,
                                                         int page,
                                                         int pageSize,
                                                         string orderBy,
                                                         NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsEnrollments", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                if (!string.IsNullOrEmpty(month)) FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = month, type = eFilterType.Int, IsRank = false });
                if (!string.IsNullOrEmpty(state)) FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = state, type = eFilterType.String, IsRank = false });
                if (!string.IsNullOrEmpty(newEnrollments)) FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = newEnrollments, type = eFilterType.Int, IsRank = false });

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
        public virtual ActionResult EnrollmentsExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("EnrollmentsExport", "Enrollments Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<EnrollmentsSearchData> data = new PaginatedList<EnrollmentsSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    EnrollmentsSearchData itemRow = new EnrollmentsSearchData();
                    object[] values = row.ItemArray;
                    itemRow.Month = Convert.ToString(values[0]);
                    itemRow.State = Convert.ToString(values[1]);
                    itemRow.NewEnrollments = Convert.ToString(values[2]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"Month", Translation.GetTerm("Month")},
					{"State", Translation.GetTerm("State","State")},
					{"NewEnrollments", Translation.GetTerm("NewEnrollments","NewEnrollments")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<EnrollmentsSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region OrderDetailReport
        //Developed by Luis Peña V. - CSTI

        public ActionResult OrderDetailReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetOrderDetailReport(string accountNumber,
                                                         string orderNumber,
                                                         string orderType,
                                                         string CUV,
                                                         string parentcode,
                                                         string sap,
                                                         string orderStatus,
                                                         DateTime? OrderDateIni,
                                                         DateTime? OrderDateFin,
                                                         int page,
                                                         int pageSize,
                                                         string orderBy,
                                                         NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsOrderDetails", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = orderNumber, type = eFilterType.String, IsRank = false });
                FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = accountNumber, type = eFilterType.String, IsRank = false });

                FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = orderType, type = eFilterType.String, IsRank = false });
                FilterList.Add(new FilterSearchData() { IndexColumn = 9, Value = CUV, type = eFilterType.String, IsRank = false });
                FilterList.Add(new FilterSearchData() { IndexColumn = 9, Value = parentcode, type = eFilterType.String, IsRank = false });
                FilterList.Add(new FilterSearchData() { IndexColumn = 10, Value = sap, type = eFilterType.String, IsRank = false });
                FilterList.Add(new FilterSearchData() { IndexColumn = 4, Value = orderStatus, type = eFilterType.String, IsRank = false });

                FilterList.Add(new FilterSearchData() { IndexColumn = 7, Value = Convert.ToString(OrderDateIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 7, Value = Convert.ToString(OrderDateFin), type = eFilterType.DateTime, IsRank = true });



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
        public virtual ActionResult OrderDetailExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("OrderDetailExport", "OrderDetail Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<OrderDetailSearchData> data = new PaginatedList<OrderDetailSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    OrderDetailSearchData itemRow = new OrderDetailSearchData();
                    object[] values = row.ItemArray;
                    itemRow.OrderNumber = Convert.ToString(values[0]);
                    itemRow.AccountNumber = Convert.ToString(values[1]);
                    itemRow.AccountType = Convert.ToString(values[2]);
                    itemRow.OrderType = Convert.ToString(values[3]);
                    itemRow.OrderStatus = Convert.ToString(values[4]);
                    itemRow.BAState = Convert.ToString(values[5]);
                    itemRow.ShipmentState = Convert.ToString(values[6]);
                    itemRow.OrderDate = Convert.ToString(values[7]);
                    itemRow.ParentCode = Convert.ToString(values[8]);
                    itemRow.CUV = Convert.ToString(values[9]);
                    itemRow.SAP = Convert.ToString(values[10]);
                    itemRow.BPCS = Convert.ToString(values[11]);
                    itemRow.ProductDescription = Convert.ToString(values[12]);
                    itemRow.Quantity = Convert.ToString(values[13]);
                    itemRow.Price = Convert.ToDecimal(values[14]);
                    itemRow.CV = Convert.ToString(values[15]);
                    itemRow.QV = Convert.ToString(values[16]);
                    itemRow.Net = Convert.ToString(values[17]);
                    itemRow.Subtotal = Convert.ToString(values[18]);
                    itemRow.TaxAmount = Convert.ToString(values[19]);
                    itemRow.GrandTotal = Convert.ToString(values[20]);

                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
                    {"OrderNumber", Translation.GetTerm("OrderNumber","OrderNumber")},
                    {"AccountNumber", Translation.GetTerm("AccountNumber30","AccountNumber")},
                    {"AccountType", Translation.GetTerm("AccountType","AccountType")},
                    {"OrderType", Translation.GetTerm("OrderType","OrderType")},
                    {"OrderStatus", Translation.GetTerm("OrderStatus","OrderStatus")},
                    {"BAState", Translation.GetTerm("BAState","BAState")},
                    {"ShipmentState", Translation.GetTerm("ShipmentState","ShipmentState")},
                    {"OrderDate", Translation.GetTerm("OrderDate","OrderDate")},
                    {"ParentCode", Translation.GetTerm("ParentCode","ParentCode")},
                    {"CUV", Translation.GetTerm("CUV","CUV")},
                    {"SAP", Translation.GetTerm("SAP","SAP")},
                    {"BPCS", Translation.GetTerm("BPCS","BPCS")},
                    {"ProductDescription", Translation.GetTerm("ProductDescription","ProductDescription")},
                    {"Quantity", Translation.GetTerm("Quantity","Quantity")},
                    {"Price", Translation.GetTerm("Price","Price")},
                    {"CV", Translation.GetTerm("CV","CV")},
                    {"QV", Translation.GetTerm("QV","QV")},
                    {"Net", Translation.GetTerm("Net","Net")},
                    {"Subtotal", Translation.GetTerm("Subtotal","Subtotal")},
                    {"TaxAmount", Translation.GetTerm("TaxAmount","TaxAmount")},
                    {"GrandTotal", Translation.GetTerm("GrandTotal","GrandTotal")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<OrderDetailSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region ProductCreditBalanceReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult ProductCreditBalanceReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetProductCreditBalanceReportBackup(string accountId,
                                                                  string credit_BalanceIni,
                                                                  string credit_BalanceFin,
                                                                  string startDate, string endDate,
                                                                  int page,
                                                                  int pageSize,
                                                                  string orderBy,
                                                                  NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore
                string bA_ID = accountId;
                string name = "";
                DataSet ds = DataAccess.GetDataSet(GetCommand("upsProductCreditBalance", "Commissions"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                if (!string.IsNullOrEmpty(bA_ID)) FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = bA_ID, type = eFilterType.Int, IsRank = false });
                if (!string.IsNullOrEmpty(name)) FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = name, type = eFilterType.String, IsRank = false });

                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = credit_BalanceIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = credit_BalanceFin, type = eFilterType.Decimal, IsRank = true });

                if (!string.IsNullOrEmpty(startDate)) FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = startDate, type = eFilterType.DateTime, IsRank = true });
                if (!string.IsNullOrEmpty(endDate)) FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = endDate, type = eFilterType.DateTime, IsRank = true });


                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);
                //PaginatedList<ProductCreditBalanceSearchData> ProductCreditBalanceList = dv.ToPaginatedList<ProductCreditBalanceSearchData>();
                //ProductCreditBalanceList.PageIndex = page;
                //ProductCreditBalanceList.PageSize = pageSize;
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
        public virtual ActionResult GetProductCreditBalanceReport(string accountId,
                                                                  string credit_BalanceIni,
                                                                  string credit_BalanceFin,
                                                                  string startDate, string endDate,
                                                                  string state,
                                                                  int page,
                                                                  int pageSize,
                                                                  string orderBy,
                                                                  NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                StringBuilder builder = new StringBuilder();
                int count = 0;

                var ListRenegotiationMethods = ProductRepository.BrowseProductCreditLedger(new ProductCreditLedgerParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    AccountID = accountId.ToInt(),
                    CreditBalanceMin = credit_BalanceIni,
                    CreditBalanceMax = credit_BalanceFin,
                    EntryDateFrom = startDate,
                    EntryDateTo = endDate,
                    State = state

                });



                foreach (var reneg in ListRenegotiationMethods)
                {



                    builder.Append("<tr>");
                    builder

                        .AppendCell(reneg.BA_ID)
                        .AppendCell(reneg.Name)
                        .AppendCell(reneg.EffectiveDate)
                        .AppendCell(reneg.EntryDescription)
                        .AppendCell(reneg.EntryReasonName)
                        .AppendCell(reneg.EntryOriginName)
                        .AppendCell(reneg.EntryTypeName)
                        .AppendCell(reneg.Credit_Balance, null, decimal.Parse(reneg.Credit_Balance) < 0 ? "color:red; text-align: right" : "text-align: right")
                        .AppendCell(reneg.EndingBalance, null, decimal.Parse(reneg.EndingBalance) < 0 ? "color:red; text-align: right" : "text-align: right")

                        .AppendCell(reneg.Ticket)
                        .AppendCell(reneg.Order)
                        .AppendCell(reneg.Soporte)

                       .Append("</tr>");

                    ++count;
                }

                return Json(new { result = true, totalPages = ListRenegotiationMethods.TotalPages, page = ListRenegotiationMethods.TotalCount == 0 ? "<tr><td colspan=\"7\">There are no rules</td></tr>" : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ProductCreditBalanceExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;
                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("ProductCreditBalanceExport", "ProductCreditBalance Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<ProductCreditBalanceSearchData> data = new PaginatedList<ProductCreditBalanceSearchData>();
                foreach (System.Data.DataRow row in table.Rows)
                {
                    ProductCreditBalanceSearchData itemRow = new ProductCreditBalanceSearchData();
                    object[] values = row.ItemArray;
                    itemRow.BA_ID = Convert.ToString(values[0]);
                    itemRow.Name = Convert.ToString(values[1]);
                    itemRow.Credit_Balance = Convert.ToString(values[2]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"BA_ID", Translation.GetTerm("BA Number")},
					{"Name", Translation.GetTerm("Name","Name")},
                    {"Credit_Balance", Translation.GetTerm("Credit_Balance", "Credit_Balance")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ProductCreditBalanceSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region TotalSalesReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult TotalSalesReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetTotalSalesReport(string state,
                                                        string month,
                                                        string subtotalIni,
                                                        string subtotalFin,
                                                        string grossIni,
                                                        string grossFin,
                                                        int page,
                                                        int pageSize,
                                                        string orderBy,
                                                        NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsNetSales", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();
                if (!string.IsNullOrEmpty(state)) FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = state, type = eFilterType.String, IsRank = false });
                if (!string.IsNullOrEmpty(month)) FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = month, type = eFilterType.Int, IsRank = false });

                FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = subtotalIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = subtotalFin, type = eFilterType.Decimal, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = grossIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = grossFin, type = eFilterType.Decimal, IsRank = true });

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
        public virtual ActionResult TotalSalesExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("TotalSalesExport", "TotalSales Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<TotalSalesSearchData> data = new PaginatedList<TotalSalesSearchData>();
                foreach (System.Data.DataRow row in table.Rows)
                {
                    TotalSalesSearchData itemRow = new TotalSalesSearchData();
                    object[] values = row.ItemArray;
                    itemRow.Month = Convert.ToInt32(values[0]);
                    itemRow.Subtotal = Convert.ToDecimal(values[1]);
                    itemRow.Gross = Convert.ToDecimal(values[2]);
                    itemRow.State = Convert.ToString(values[3]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"Month", Translation.GetTerm("Month")},
					{"Subtotal", Translation.GetTerm("Subtotal","Subtotal")},
					{"Gross", Translation.GetTerm("Gross","Gross")},
					{"State", Translation.GetTerm("State","State")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<TotalSalesSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region SalesSourceReport
        //Developed by Luis Peña V. - CSTI

        public ActionResult SalesSourceReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetSalesSourceReport(string month,
                                                         string productSalesIni,
                                                         string productSalesFin,
                                                         string sarterKitSalesIni,
                                                         string sarterKitSalesFin,
                                                         int page,
                                                         int pageSize,
                                                         string orderBy,
                                                         NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsSalesSource", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                if (!string.IsNullOrEmpty(month)) FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = month, type = eFilterType.Int, IsRank = false });

                //if (!string.IsNullOrEmpty(productsalesIni)) FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = productsalesIni, type = eFilterType.String, IsRank = false });
                //if (!string.IsNullOrEmpty(productsalesFin)) FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = productsalesFin, type = eFilterType.String, IsRank = false });

                FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = productSalesIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = productSalesFin, type = eFilterType.Decimal, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = sarterKitSalesIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = sarterKitSalesFin, type = eFilterType.Decimal, IsRank = true });

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
        public virtual ActionResult SalesSourceExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("SalesSourceExport", "SalesSource Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<SalesSourceSearchData> data = new PaginatedList<SalesSourceSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    SalesSourceSearchData itemRow = new SalesSourceSearchData();
                    object[] values = row.ItemArray;
                    itemRow.Month = Convert.ToString(values[0]);
                    itemRow.NoKit = Convert.ToString(values[1]);
                    itemRow.Kit = Convert.ToString(values[2]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"Month", Translation.GetTerm("Month")},
					{"NoKit", Translation.GetTerm("NoKit","Product Sales")},
					{"Kit", Translation.GetTerm("Kit","Starter Kit Sales")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<SalesSourceSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region DisbursmentProfilesReport
        //Developed by Luis Peña V. - CSTI

        public ActionResult DisbursmentProfilesReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetDisbursmentProfilesReport(string accountNumber,
                                                                 string name,
                                                                 string state,
                                                                 string postalCodeIni2,
                                                                 string postalCodeFin2,
                                                                 int page,
                                                                 int pageSize,
                                                                 string orderBy,
                                                                 NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsDisbursmentProfiles", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                if (!string.IsNullOrEmpty(accountNumber)) FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = accountNumber, type = eFilterType.String, IsRank = false });
                if (!string.IsNullOrEmpty(name)) FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = name, type = eFilterType.String, IsRank = false });
                if (!string.IsNullOrEmpty(state)) FilterList.Add(new FilterSearchData() { IndexColumn = 6, Value = state, type = eFilterType.String, IsRank = false });

                FilterList.Add(new FilterSearchData() { IndexColumn = 7, Value = postalCodeIni2, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 7, Value = postalCodeFin2, type = eFilterType.Decimal, IsRank = true });

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
        public virtual ActionResult DisbursmentProfilesExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("DisbursmentProfilesExport", "DisbursmentProfiles Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<DisbursmentProfilesSearchData> data = new PaginatedList<DisbursmentProfilesSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    DisbursmentProfilesSearchData itemRow = new DisbursmentProfilesSearchData();
                    object[] values = row.ItemArray;
                    itemRow.AccountNumber = Convert.ToString(values[0]);
                    itemRow.Name = Convert.ToString(values[1]);
                    itemRow.DisburmentType = Convert.ToString(values[2]);
                    itemRow.Address1 = Convert.ToString(values[3]);
                    itemRow.Address2 = Convert.ToString(values[4]);
                    itemRow.City = Convert.ToString(values[5]);
                    itemRow.State = Convert.ToString(values[6]);
                    itemRow.PostalCode = Convert.ToString(values[7]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"AccountNumber", Translation.GetTerm("AccountNumber30","AccountNumber")},
                    {"Name", Translation.GetTerm("Name","Name")},
					{"DisburmentType", Translation.GetTerm("DisburmentType","DisburmentType")},
					{"Address1", Translation.GetTerm("Address1","Address1")},
                    {"Address2", Translation.GetTerm("Address2","Address2")},
                    {"City", Translation.GetTerm("City","City")},
                    {"State", Translation.GetTerm("State","State")},
                    {"PostalCode", Translation.GetTerm("PostalCode","PostalCode")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<DisbursmentProfilesSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region VolumesReport
        //Developed by Luis Peña V. - CSTI

        public ActionResult VolumesReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetVolumesReport(string accountNumber,
                                                        string name,
                                                        string qvIni,
                                                        string qvFin,
                                                        string cvIni,
                                                        string cvFin,
                                                        int page,
                                                        int pageSize,
                                                        string orderBy,
                                                        NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsVolumes", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                if (!string.IsNullOrEmpty(accountNumber)) FilterList.Add(new FilterSearchData() { IndexColumn = 0, Value = accountNumber, type = eFilterType.String, IsRank = false });
                if (!string.IsNullOrEmpty(name)) FilterList.Add(new FilterSearchData() { IndexColumn = 1, Value = name, type = eFilterType.String, IsRank = false });

                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = qvIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 2, Value = qvFin, type = eFilterType.Decimal, IsRank = true });

                FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = cvIni, type = eFilterType.Decimal, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = cvFin, type = eFilterType.Decimal, IsRank = true });

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
        public virtual ActionResult VolumesExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("VolumesExport", "Volumes Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<VolumesSearchData> data = new PaginatedList<VolumesSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    VolumesSearchData itemRow = new VolumesSearchData();
                    object[] values = row.ItemArray;
                    itemRow.AccountNumber = Convert.ToString(values[0]);
                    itemRow.Name = Convert.ToString(values[1]);
                    itemRow.QV = Convert.ToDecimal(values[2]);
                    itemRow.CV = Convert.ToDecimal(values[3]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
                {
                    {"AccountNumber", Translation.GetTerm("AccountNumber1","AccountNumber")},
                    {"Name", Translation.GetTerm("Name","Name")},
                    {"QV", Translation.GetTerm("QV","QV")},
                    {"CV", Translation.GetTerm("CV","CV")},
                };

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<VolumesSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region WarehouseReport

        public ActionResult WareHouseReportGate()
        {
            AllProducts = Product.LoadAllSlim(new NetSteps.Common.Base.FilterPaginatedListParameters<Product>()
            {
                WhereClause = p => p.ProductBase.IsShippable && !(p.ProductBase.Products.Count > 1 && !p.IsVariantTemplate),
                OrderBy = "SKU",
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                PageIndex = 0,
                PageSize = null
            });
            AllBaseProducts = new ProductBaseRepository().Search(new ProductBaseSearchParameters()
            {
                PageIndex = 0,
                PageSize = null,
                OrderBy = "SKU"
            });

            AllWarehouseProducts = WarehouseProduct.LoadAll();
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetWarehouseReport(string query, string bpcs, string sapSku, int? type, bool? active, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var warehouseProducts = this.AllWarehouseProducts;

                StringBuilder builder = new StringBuilder("<tr><th></th><th></th><th></th><th></th><th></th><th></th><th></th>");
                Warehouse warehouse = SmallCollectionCache.Instance.Warehouses.Where(x => x.WarehouseID == 2).First();
                builder.Append("<th><div class=\"warehouse")
                .Append(warehouse.WarehouseID).Append("\"><span style=\"margin-left: 25px;\" class=\"SubGridHead\">")
                .Append(Translation.GetTerm("QuantityOnHand", "Quantity on Hand"))
                .Append("</span><span class=\"SubGridHead\">")
                .Append(Translation.GetTerm("Buffer"))
                .Append("</span><span class=\"SubGridHead\">")
                .Append(Translation.GetTerm("ReorderLevel", "Reorder Level"))
                .Append("</span><span class=\"SubGridHead\">")
                .Append(Translation.GetTerm("Allocated"))
                .Append("</span><span class=\"SubGridHead\">")
                .Append(Translation.GetTerm("Avalible_", "Avalible"))
                .Append("</span>");
                builder.Append("</th>");


                ProductRepository prepo = new ProductRepository();
                ProductBaseRepository pbase = new ProductBaseRepository();
                var repo = new ProductBaseRepository();
                var productBases = repo.Search(new ProductBaseSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    Query = query,
                    ProductTypeID = type,
                    Active = active,
                    BPCS = bpcs,
                    SAPSKU = sapSku
                });


                //
                foreach (ProductBaseSearchData productBase in productBases)
                {
                    var PSlim_ = AllProducts.FirstOrDefault(pslim => pslim.ProductBaseID == productBase.ProductBaseID);
                    if (PSlim_ != null && !PSlim_.IsVariantTemplate)
                    {

                        var offerType_ = ProductProperty.LoadByProductID(PSlim_.ProductID).Where(pp => pp.ProductPropertyTypeID == 11).FirstOrDefault();
                        String offertype = (offerType_ == null) ? "" : offerType_.PropertyValue;
                        builder.Append("<tr class=\"mainProduct\">")
                            .AppendCell(productBase.SKU)
                            .AppendCell(productBase.Name)
                            .AppendCell((productBase.Active) ? "Active" : "Inactive")
                            .AppendCell(offertype)
                            .AppendCell(productBase.SAPSKU)
                            .AppendCell(productBase.ProductType)
                            .AppendCell("")
                            ;

                        WarehouseProduct wp = warehouseProducts.FirstOrDefault(p => p.ProductID == PSlim_.ProductID && p.WarehouseID == warehouse.WarehouseID);
                        builder.Append(GetWarehouseInventoryCellsNoEdit(warehouse, wp, Product.Load(PSlim_.ProductID)));
                        builder.Append("</tr>");
                    }
                    else
                    {
                        var products = pbase.LoadFull(productBase.ProductBaseID).Products.OrderBy("SKU");
                        foreach (var product in products)
                        {
                            var offerType_ = product.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 11);
                            String offertype = (offerType_ == null) ? "" : offerType_.PropertyValue;
                            var SAP_ = product.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 3);
                            String SAP = (SAP_ == null) ? "" : SAP_.PropertyValue;
                            builder.Append("<tr class=\"mainProduct\">")
                                .AppendCell(product.SKU)
                                .AppendCell(product.Name)
                                .AppendCell((product.Active) ? "Active" : "Inactive")
                                .AppendCell(offertype)
                                .AppendCell(SAP)
                                .AppendCell(productBase.ProductType)
                                .AppendCell((product.IsVariantTemplate) ? "Variant Template" : "Variant")
                                ;


                            WarehouseProduct wp = warehouseProducts.FirstOrDefault(p => p.ProductID == product.ProductID && p.WarehouseID == warehouse.WarehouseID);
                            builder.Append(GetWarehouseInventoryCellsNoEdit(warehouse, wp, product));
                            builder.Append("</tr>");
                        }
                    }
                }
                return Json(new { totalPages = productBases.TotalPages, page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ExportExcel()
        {
            try
            {
                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("WareHouseExport", "WareHouse Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                var warehouseProducts = this.AllWarehouseProducts;
                Warehouse warehouse = SmallCollectionCache.Instance.Warehouses.Where(x => x.WarehouseID == 2).First();
                ProductRepository prepo = new ProductRepository();
                ProductBaseRepository pbase = new ProductBaseRepository();
                var repo = new ProductBaseRepository();
                var productBases = AllBaseProducts;
                PaginatedList<WarehouseSlimSearchData> data = new PaginatedList<WarehouseSlimSearchData>();

                //
                foreach (ProductBaseSearchData productBase in productBases)
                {
                    var PSlim_ = AllProducts.FirstOrDefault(pslim => pslim.ProductBaseID == productBase.ProductBaseID);
                    WarehouseSlimSearchData row;
                    if (PSlim_ != null && !PSlim_.IsVariantTemplate)
                    {
                        row = new WarehouseSlimSearchData();
                        var offerType_ = ProductProperty.LoadByProductID(PSlim_.ProductID).Where(pp => pp.ProductPropertyTypeID == 11).FirstOrDefault();
                        String offertype = (offerType_ == null) ? "" : offerType_.PropertyValue;
                        row.SKU = productBase.SKU;
                        row.Name = productBase.Name;
                        row.SAPCODE = productBase.SAPSKU;
                        row.ProductType = productBase.ProductType;
                        row.OfferType = offertype;
                        row.Active = (productBase.Active) ? "Active" : "Inactive";
                        row.Variant = "";
                        WarehouseProduct wp = warehouseProducts.FirstOrDefault(p => p.ProductID == PSlim_.ProductID && p.WarehouseID == warehouse.WarehouseID);

                        if (wp != null)
                        {
                            row.QtyonHand = "" + wp.QuantityOnHand;
                            row.Buffer = "" + wp.QuantityBuffer;
                            row.Allocated = "" + wp.QuantityAllocated;
                            row.Avalible = "" + (wp.QuantityOnHand - wp.QuantityAllocated);
                            row.ReorderLevel = "" + wp.ReorderLevel;
                        }
                        else
                        {
                            row.QtyonHand = "-";
                            row.Buffer = "-";
                            row.Allocated = "-";
                            row.Avalible = "-";
                            row.ReorderLevel = "-";
                        }

                        data.Add(row);
                    }
                    else
                    {
                        var products = pbase.LoadFull(productBase.ProductBaseID).Products.OrderBy("SKU");
                        foreach (var product in products)
                        {
                            row = new WarehouseSlimSearchData();
                            var offerType_ = product.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 11);
                            String offertype = (offerType_ == null) ? "" : offerType_.PropertyValue;
                            var SAP_ = product.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 3);
                            String SAP = (SAP_ == null) ? "" : SAP_.PropertyValue;
                            row.SKU = product.SKU;
                            row.Name = product.Name;
                            row.SAPCODE = SAP;
                            row.ProductType = productBase.ProductType;
                            row.OfferType = offertype;
                            row.Active = (product.Active) ? "Active" : "Inactive";
                            row.Variant = (product.IsVariantTemplate) ? "Variant Template" : "Variant";
                            WarehouseProduct wp = warehouseProducts.FirstOrDefault(p => p.ProductID == product.ProductID && p.WarehouseID == warehouse.WarehouseID);

                            if (wp != null)
                            {
                                row.QtyonHand = "" + wp.QuantityOnHand;
                                row.Buffer = "" + wp.QuantityBuffer;
                                row.Allocated = "" + wp.QuantityAllocated;
                                row.Avalible = "" + (wp.QuantityOnHand - wp.QuantityAllocated);
                                row.ReorderLevel = "" + wp.ReorderLevel;
                            }
                            else
                            {
                                row.QtyonHand = "-";
                                row.Buffer = "-";
                                row.Allocated = "-";
                                row.Avalible = "-";
                                row.ReorderLevel = "-";
                            }

                            data.Add(row);
                        }
                    }
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"SKU", Translation.GetTerm("CUV")},
					{"Name", Translation.GetTerm("Name","Name")},
					{"Active", Translation.GetTerm("Active","Active")},
					{"OfferType", Translation.GetTerm("OfferType","OfferType")},
					{"SAPCODE", Translation.GetTerm("SAPCODE", "SAP CODE")},
					{"ProductType", Translation.GetTerm("ProductType", "Product Type")},
					{"Variant", Translation.GetTerm("Variant", "Variant")},
					{"QtyonHand", Translation.GetTerm("QtyonHand", "Qty on Hand")},
					{"Buffer", Translation.GetTerm("Buffer", "Buffer")},
                    {"ReorderLevel", Translation.GetTerm("ReorderLevel", "Reorder Level")},
                    {"Allocated", Translation.GetTerm("Allocated", "Allocated")},
                    {"Avalible", Translation.GetTerm("Avalible", "Avalible")}
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<WarehouseSlimSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }
        private StringBuilder GetWarehouseInventoryCellsNoEdit(Warehouse warehouse, WarehouseProduct wp, Product product)
        {
            bool productExists = wp != default(WarehouseProduct) && wp.IsAvailable;
            StringBuilder builder = new StringBuilder();
            builder.Append("<td class=\"warehouseProduct\"><div class=\"warehouse").Append(warehouse.WarehouseID)
                .Append("\"><input type=\"hidden\" class=\"changed\" value=\"false\" />")
                .Append("<input type=\"hidden\" class=\"warehouseProductId\" value=\"").Append(wp == null ? 0 : wp.WarehouseProductID).Append("\" />")
                .Append("<input type=\"hidden\" class=\"warehouseId\" value=\"").Append(warehouse.WarehouseID).Append("\" />")
                .Append("<input type=\"hidden\" class=\"productId\" value=\"").Append(product.ProductID).Append("\" />")
                .Append("<input type=\"checkbox\"  disabled").Append(productExists ? " checked=\"checked\"" : "").Append(" class=\"IsAvailable warehouseEnabler\" />")
                .Append("<input type=\"text\" class=\"QuantityOnHand numeric\" value=\" ").Append(wp != null ? wp.QuantityOnHand : 0).Append("\"").Append(productExists ? "" : " style=\"color: #cfcfcf;\"").Append(" disabled=\"disabled\" ").Append(" />")
                .Append("<input type=\"text\" class=\"QuantityBuffer numeric\" value=\" ").Append(wp != null ? wp.QuantityBuffer : 0).Append("\"").Append(productExists ? "" : " style=\"color: #cfcfcf;\"").Append(" disabled=\"disabled\"").Append(" />")
                .Append("<input type=\"text\" class=\"ReorderLevel numeric\" value=\" ").Append(wp != null ? wp.ReorderLevel : 0).Append("\"").Append(productExists ? "" : " style=\"color: #cfcfcf;\"").Append(" disabled=\"disabled\"").Append(" />")
                .Append("<span class=\"QuantityAllocated\">").Append(wp == null ? 0 : wp.QuantityAllocated).Append("</span>")
                .Append("<span class=\"QuantityAvalible\">").Append(wp == null ? 0 : (wp.QuantityOnHand - wp.QuantityAllocated)).Append("</span>")
                .Append("<span ").Append("title=\"Not shipping\" style=\"display: ").Append(productExists ? "none" : "inline-block").Append(";\" class=\"icon-cancelled NoShip").Append(product.ProductBaseID).Append("\" ></span>")
                .Append("</div></td>");

            return builder;

        }

        #endregion

        #region ProductPricesReport
        public ActionResult ProductPricesReportGate()
        {

            AllProducts = Product.LoadAllSlim(new NetSteps.Common.Base.FilterPaginatedListParameters<Product>()
            {
                WhereClause = p => p.ProductBase.IsShippable && !(p.ProductBase.Products.Count > 1 && !p.IsVariantTemplate),
                OrderBy = "SKU",
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
                PageIndex = 0,
                PageSize = null
            });
            AllBaseProducts = new ProductBaseRepository().Search(new ProductBaseSearchParameters()
            {
                PageIndex = 0,
                PageSize = null,
                OrderBy = "SKU"
            });
            return View();
        }
        [OutputCache(CacheProfile = "DontCache")]
        //public virtual ActionResult Get(string query, int? type, bool? active, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        public virtual ActionResult ProductPricesReport(string query, string bpcs, string sapSku, int? type, bool? active, int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                var repo = new ProductBaseRepository();
                var productBases = repo.Search(new ProductBaseSearchParameters()
                {
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    OrderByDirection = orderByDirection,
                    Query = query,
                    ProductTypeID = type,
                    Active = active,
                    BPCS = bpcs,
                    SAPSKU = sapSku
                });


                StringBuilder builder = new StringBuilder();
                Currency currency = SmallCollectionCache.Instance.Currencies.GetById(1);

                var pbase = new ProductBaseRepository();
                var repop = new ProductRepository();

                foreach (var product in productBases)
                {
                    var PSlim_ = AllProducts.FirstOrDefault(slim => slim.ProductBaseID == product.ProductBaseID);
                    if (PSlim_ != null && !PSlim_.IsVariantTemplate)
                    {

                        var prices = repop.GetProductPrices(repop.GetProductIdBySKU(product.SKU), 1);
                        ProductPrice retail = prices.Where(pp => pp.ProductPriceTypeID == 1).FirstOrDefault();
                        ProductPrice QV = prices.Where(pp => pp.ProductPriceTypeID == 21).FirstOrDefault();
                        ProductPrice CV = prices.Where(pp => pp.ProductPriceTypeID == 18).FirstOrDefault();
                        ProductPrice Handling = prices.Where(pp => pp.ProductPriceTypeID == 11).FirstOrDefault();
                        builder.Append("<tr>")
                            .AppendCell(product.SKU)
                            .AppendCell(product.Name)
                            .AppendCell(product.ProductType)
                            .AppendCell(product.Categories)
                            .AppendCell(product.Catalogs)
                            .AppendCell(product.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                            .AppendCell(product.SAPSKU)
                            .AppendCell(product.BPCS)
                            .AppendCell(((retail == null) ? 0 : retail.Price).ToString(currency))
                            .AppendCell(((QV == null) ? 0 : QV.Price).ToString(currency))
                            .AppendCell(((CV == null) ? 0 : CV.Price).ToString(currency))
                            .AppendCell(((Handling == null) ? 0 : Handling.Price).ToString(currency))
                            .Append("</tr>");
                    }
                    else
                    {
                        var products = pbase.LoadFull(product.ProductBaseID).Products.OrderBy("SKU");
                        foreach (var paux in products)
                        {
                            var prices = repop.GetProductPrices(repop.GetProductIdBySKU(paux.SKU), 1);
                            ProductPrice retail = prices.Where(pp => pp.ProductPriceTypeID == 1).FirstOrDefault();
                            ProductPrice QV = prices.Where(pp => pp.ProductPriceTypeID == 21).FirstOrDefault();
                            ProductPrice CV = prices.Where(pp => pp.ProductPriceTypeID == 18).FirstOrDefault();
                            ProductPrice Handling = prices.Where(pp => pp.ProductPriceTypeID == 11).FirstOrDefault();
                            var SAP_ = paux.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 3);
                            String SAP = (SAP_ == null) ? "" : SAP_.PropertyValue;
                            var BPCS_ = paux.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 2);
                            String BPCS = (BPCS_ == null) ? "" : BPCS_.PropertyValue;
                            builder.Append("<tr>")
                                .AppendCell(paux.SKU)
                                .AppendCell(paux.Name)
                                .AppendCell(product.ProductType)
                                .AppendCell(product.Categories)
                                .AppendCell(product.Catalogs)
                                .AppendCell(paux.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"))
                                .AppendCell(SAP)
                                .AppendCell(BPCS)
                                .AppendCell(((retail == null) ? 0 : retail.Price).ToString(currency))
                                .AppendCell(((QV == null) ? 0 : QV.Price).ToString(currency))
                                .AppendCell(((CV == null) ? 0 : CV.Price).ToString(currency))
                                .AppendCell(((Handling == null) ? 0 : Handling.Price).ToString(currency))
                                .Append("</tr>");
                        }
                    }
                }
                return Json(new { totalPages = productBases.TotalPages, page = builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        public virtual ActionResult ProductPricesReportExportExcel()
        {

            try
            {
                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("ProductPriceExport", "Product Price Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));


                var repo = new ProductBaseRepository();
                var productBases = this.AllBaseProducts;
                PaginatedList<ProductPriceSearchData> data = new PaginatedList<ProductPriceSearchData>();


                StringBuilder builder = new StringBuilder();
                Currency currency = SmallCollectionCache.Instance.Currencies.GetById(1);
                ProductPriceSearchData row;

                var pbase = new ProductBaseRepository();
                var repop = new ProductRepository();

                foreach (var product in productBases)
                {
                    var PSlim_ = AllProducts.FirstOrDefault(slim => slim.ProductBaseID == product.ProductBaseID);
                    if (PSlim_ != null && !PSlim_.IsVariantTemplate)
                    {
                        row = new ProductPriceSearchData();
                        var prices = repop.GetProductPrices(repop.GetProductIdBySKU(product.SKU), 1);
                        ProductPrice retail = prices.Where(pp => pp.ProductPriceTypeID == 1).FirstOrDefault();
                        ProductPrice QV = prices.Where(pp => pp.ProductPriceTypeID == 21).FirstOrDefault();
                        ProductPrice CV = prices.Where(pp => pp.ProductPriceTypeID == 18).FirstOrDefault();
                        ProductPrice Handling = prices.Where(pp => pp.ProductPriceTypeID == 11).FirstOrDefault();
                        row.SKU = product.SKU;
                        row.Name = product.Name;
                        row.Type = product.ProductType;
                        row.Categories = product.Categories;
                        row.Catalogs = product.Catalogs;
                        row.Status = "" + ((product.Active) ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"));
                        row.SAPSKU = product.SAPSKU;
                        row.BPCS = product.BPCS;
                        row.Retail = (((retail == null) ? 0 : retail.Price).ToString(currency));
                        row.QV = (((QV == null) ? 0 : QV.Price).ToString(currency));
                        row.CV = (((CV == null) ? 0 : CV.Price).ToString(currency));
                        row.Handling = (((Handling == null) ? 0 : Handling.Price).ToString(currency));
                        data.Add(row);
                    }
                    else
                    {
                        var products = pbase.LoadFull(product.ProductBaseID).Products.OrderBy("SKU");
                        foreach (var paux in products)
                        {
                            row = new ProductPriceSearchData();
                            var prices = repop.GetProductPrices(repop.GetProductIdBySKU(paux.SKU), 1);
                            ProductPrice retail = prices.Where(pp => pp.ProductPriceTypeID == 1).FirstOrDefault();
                            ProductPrice QV = prices.Where(pp => pp.ProductPriceTypeID == 21).FirstOrDefault();
                            ProductPrice CV = prices.Where(pp => pp.ProductPriceTypeID == 18).FirstOrDefault();
                            ProductPrice Handling = prices.Where(pp => pp.ProductPriceTypeID == 11).FirstOrDefault();
                            var SAP_ = paux.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 3);
                            String SAP = (SAP_ == null) ? "" : SAP_.PropertyValue;
                            var BPCS_ = paux.Properties.FirstOrDefault(pr => pr.ProductPropertyTypeID == 2);
                            String BPCS = (BPCS_ == null) ? "" : BPCS_.PropertyValue;

                            row.SKU = (paux.SKU);
                            row.Name = (paux.Name);
                            row.Type = (product.ProductType);
                            row.Categories = (product.Categories);
                            row.Catalogs = (product.Catalogs);
                            row.Status = (paux.Active ? Translation.GetTerm("Active") : Translation.GetTerm("Inactive"));
                            row.SAPSKU = (SAP);
                            row.BPCS = (BPCS);
                            row.Retail = (((retail == null) ? 0 : retail.Price).ToString(currency));
                            row.QV = (((QV == null) ? 0 : QV.Price).ToString(currency));
                            row.CV = (((CV == null) ? 0 : CV.Price).ToString(currency));
                            row.Handling = (((Handling == null) ? 0 : Handling.Price).ToString(currency));
                            data.Add(row);
                        }
                    }
                }


                var columns = new Dictionary<string, string>
				{
					{"SKU", Translation.GetTerm("CUV")},
					{"Name", Translation.GetTerm("Name","Name")},
					{"Type", Translation.GetTerm("Type","Type")},
					{"Categories", Translation.GetTerm("categories","Categories")},
					{"Catalogs", Translation.GetTerm("Catalogs", "Catalogs")},
					{"Status", Translation.GetTerm("Status", "Status")},
					{"SAPSKU", Translation.GetTerm("SAPSKU", "SAPSKU")},
					{"BPCS", Translation.GetTerm("BPCS", "BPCS")},
                    {"Retail", Translation.GetTerm("Retail", "Retail")},
                    {"CV", Translation.GetTerm("CV", "CV")},
                    {"QV", Translation.GetTerm("QV", "QV")},
				    {"Handling", Translation.GetTerm("Handling", "Handling")}
				
                };

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ProductPriceSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }


        }
        #endregion

        #region InventoryMovementsReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult InventoryMovementsReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetInventoryMovementsReport(string cuv,
                                                                string sapCode,
                                                                DateTime? completeDateUTCIni,
                                                                DateTime? completeDateUTCFin,
                                                                int page,
                                                                int pageSize,
                                                                string orderBy,
                                                                NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                if (string.IsNullOrEmpty(cuv) && string.IsNullOrEmpty(sapCode))
                {
                    return Json(new { totalPages = 0, page = "", message = "CUV OR SAP CODE MUST BE INCLUDE FOR SEARCHING" });
                }

                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsInventoryMovements",
                                                   new Dictionary<string, object>() { {"@CUV", cuv == ""? null : cuv},
                                                                                      {"@SKU", sapCode == ""? null : sapCode}},
                                                                                      "Core"));

                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = Convert.ToString(completeDateUTCIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = Convert.ToString(completeDateUTCFin), type = eFilterType.DateTime, IsRank = true });

                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);

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
        public virtual ActionResult InventoryMovementsExport(string parameters)
        {
            try
            {
                #region Parameters

                string[] parameter = parameters.Split(Convert.ToChar("*"));
                string cuv = parameter[0];
                string sapCode = parameter[1];

                DateTime? completeDateUTCIni = null;
                //if (!string.IsNullOrEmpty(parameter[2])) completeDateUTCIni = Convert.ToDateTime(parameter[2].DateFormat()); //local
                if (!string.IsNullOrEmpty(parameter[2])) completeDateUTCIni = Convert.ToDateTime(parameter[2]);

                DateTime? completeDateUTCFin = null;
                //if (!string.IsNullOrEmpty(parameter[3])) completeDateUTCFin = Convert.ToDateTime(parameter[3].DateFormat()); //local
                if (!string.IsNullOrEmpty(parameter[3])) completeDateUTCFin = Convert.ToDateTime(parameter[3]);

                #endregion

                #region GetDataFromStore

                if (string.IsNullOrEmpty(cuv) && string.IsNullOrEmpty(sapCode))
                {
                    return Json(new { totalPages = 0, page = "", message = "CUV OR SAP CODE MUST BE INCLUDE FOR SEARCHING" });
                }
                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsInventoryMovements",
                                                   new Dictionary<string, object>() { {"@CUV", cuv == ""? null : cuv},
                                                                                      {"@SKU", sapCode == ""? null : sapCode}},
                                                                                      "Core"));

                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = Convert.ToString(completeDateUTCIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 3, Value = Convert.ToString(completeDateUTCFin), type = eFilterType.DateTime, IsRank = true });

                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, "CUV Code ASC", DataViewRowState.CurrentRows);

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("InventoryMovementsExport", "InventoryMovements Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<InventoryMovementSearchData> data = new PaginatedList<InventoryMovementSearchData>();

                foreach (System.Data.DataRow row in dv.ToTable("Table").Rows)
                {
                    InventoryMovementSearchData itemRow = new InventoryMovementSearchData();
                    object[] values = row.ItemArray;
                    itemRow.CUV = Convert.ToString(values[0]);
                    itemRow.SAPcode = Convert.ToString(values[1]);
                    itemRow.Description = Convert.ToString(values[2]);
                    itemRow.Date = Convert.ToString(values[3]);
                    itemRow.AllocatedBefore = Convert.ToString(values[4]);
                    itemRow.AllocatedAfter = Convert.ToString(values[5]);
                    itemRow.OnHandBefore = Convert.ToString(values[6]);
                    itemRow.OnHandAfter = Convert.ToString(values[7]);

                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"CUV", Translation.GetTerm("CUV")},
					{"SAPcode", Translation.GetTerm("SAPcode","SAPcode")},
					{"Description", Translation.GetTerm("Description","Description")},
					{"Date", Translation.GetTerm("Date","Date")},
					{"AllocatedBefore", Translation.GetTerm("AllocatedBefore", "AllocatedBefore")},
                    {"AllocatedAfter", Translation.GetTerm("AllocatedAfter", "AllocatedAfter")},
                    {"OnHandBefore", Translation.GetTerm("OnHandBefore", "OnHandBefore")},
                    {"OnHandAfter", Translation.GetTerm("OnHandAfter", "OnHandAfter")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<InventoryMovementSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region ItemsAllocatedByProductReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult ItemsAllocatedByProductReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetItemsAllocatedByProductReport(string cuv,
                                                                string sapCode,
                                                                DateTime? completeDateUTCIni,
                                                                DateTime? completeDateUTCFin,
                                                                int page,
                                                                int pageSize,
                                                                string orderBy,
                                                                NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                if (string.IsNullOrEmpty(cuv) && string.IsNullOrEmpty(sapCode))
                {
                    return Json(new { totalPages = 0, page = "", message = "CUV OR SAP CODE MUST BE INCLUDE FOR SEARCHING" });
                }

                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("ItemsAllocatedByProduct",
                                                   new Dictionary<string, object>() { {"@CUV", cuv == ""? null : cuv},
                                                                                      {"@SKU", sapCode == ""? null : sapCode}},
                                                                                      "Core"));

                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCFin), type = eFilterType.DateTime, IsRank = true });

                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);

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
        public virtual ActionResult ItemsAllocatedByProductExport(string parameters)
        {
            try
            {
                #region Parameters

                string[] parameter = parameters.Split(Convert.ToChar("*"));
                string cuv = parameter[0];
                string sapCode = parameter[1];

                DateTime? completeDateUTCIni = null;
                //if (!string.IsNullOrEmpty(parameter[2])) completeDateUTCIni = Convert.ToDateTime(parameter[2].DateFormat()); //local
                if (!string.IsNullOrEmpty(parameter[2])) completeDateUTCIni = Convert.ToDateTime(parameter[2]);

                DateTime? completeDateUTCFin = null;
                //if (!string.IsNullOrEmpty(parameter[3])) completeDateUTCFin = Convert.ToDateTime(parameter[3].DateFormat()); //local
                if (!string.IsNullOrEmpty(parameter[3])) completeDateUTCFin = Convert.ToDateTime(parameter[3]);

                #endregion

                #region GetDataFromStore

                if (string.IsNullOrEmpty(cuv) && string.IsNullOrEmpty(sapCode))
                {
                    return Json(new { totalPages = 0, page = "", message = "CUV OR SAP CODE MUST BE INCLUDE FOR SEARCHING" });
                }
                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("ItemsAllocatedByProduct",
                                                   new Dictionary<string, object>() { {"@CUV", cuv == ""? null : cuv},
                                                                                      {"@SKU", sapCode == ""? null : sapCode}},
                                                                                      "Core"));

                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCFin), type = eFilterType.DateTime, IsRank = true });

                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, "OrderNumber ASC", DataViewRowState.CurrentRows);

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("ItemsAllocatedByProductExport", "ItemsAllocatedByProduct Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<ItemsAllocatedByProductSearchData> data = new PaginatedList<ItemsAllocatedByProductSearchData>();

                foreach (System.Data.DataRow row in dv.ToTable("Table").Rows)
                {
                    ItemsAllocatedByProductSearchData itemRow = new ItemsAllocatedByProductSearchData();
                    object[] values = row.ItemArray;
                    itemRow.OrderNumber = Convert.ToString(values[0]);
                    itemRow.AccountNumber = Convert.ToString(values[1]);
                    itemRow.AccountType = Convert.ToString(values[2]);
                    itemRow.OrderType = Convert.ToString(values[3]);
                    itemRow.OrderStatus = Convert.ToString(values[4]);
                    itemRow.OrderDate = Convert.ToString(values[5]);
                    itemRow.ParentCode = Convert.ToString(values[6]);
                    itemRow.CUV = Convert.ToString(values[7]);
                    itemRow.SAPcode = Convert.ToString(values[8]);
                    itemRow.BPCSCode = Convert.ToString(values[9]);
                    itemRow.ProductDescription = Convert.ToString(values[10]);
                    itemRow.Quantity = Convert.ToString(values[11]);
                    itemRow.OrderShipment = Convert.ToString(values[12]);
                    itemRow.Price = Convert.ToString(values[13]);
                    itemRow.CV = Convert.ToString(values[14]);
                    itemRow.QV = Convert.ToString(values[15]);
                    itemRow.NetSale = Convert.ToString(values[16]);
                    itemRow.Subtotal = Convert.ToString(values[17]);
                    itemRow.TaxAmount = Convert.ToString(values[18]);
                    itemRow.GrandTotal = Convert.ToString(values[19]);

                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"OrderNumber", Translation.GetTerm("OrderNumber")},
					{"AccountNumber", Translation.GetTerm("AccountNumber","AccountNumber")},
					{"AccountType", Translation.GetTerm("AccountType","AccountType")},
					{"OrderType", Translation.GetTerm("OrderType","OrderType")},
					{"OrderStatus", Translation.GetTerm("OrderStatus", "OrderStatus")},
                    {"OrderDate", Translation.GetTerm("OrderDate", "OrderDate")},
                    {"ParentCode", Translation.GetTerm("ParentCode", "ParentCode")},
                    {"CUV", Translation.GetTerm("CUV", "CUV")},
                    {"SAPcode", Translation.GetTerm("SAPcode", "SAPcode")},
                    {"BPCSCode", Translation.GetTerm("BPCSCode", "BPCSCode")},
                    {"ProductDescription", Translation.GetTerm("ProductDescription", "ProductDescription")},
                    {"Quantity", Translation.GetTerm("Quantity", "Quantity")},
                    {"OrderShipment", Translation.GetTerm("OrderShipment", "OrderShipment")},
                    {"Price", Translation.GetTerm("Price", "Price")},
                    {"CV", Translation.GetTerm("CV", "CV")},
                    {"QV", Translation.GetTerm("QV", "QV")},
                    {"NetSale", Translation.GetTerm("NetSale", "NetSale")},
                    {"Subtotal", Translation.GetTerm("Subtotal", "Subtotal")},
                    {"TaxAmount", Translation.GetTerm("TaxAmount", "TaxAmount")},
                    {"GrandTotal", Translation.GetTerm("GrandTotal", "GrandTotal")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ItemsAllocatedByProductSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region SAPCodeRepeatedReport
        //Developed by Luis Peña V. - CSTI

        public ActionResult SAPCodeRepeatedReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetSAPCodeRepeatedReport(int page,
                                                             int pageSize,
                                                             string orderBy,
                                                             NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsSAPCodeRepeated", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

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
        public virtual ActionResult SAPCodeRepeatedExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("SAPCodeRepeatedExport", "SAP CodeRepeated Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<SAPCodeRepeatedSearchData> data = new PaginatedList<SAPCodeRepeatedSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    SAPCodeRepeatedSearchData itemRow = new SAPCodeRepeatedSearchData();
                    object[] values = row.ItemArray;
                    itemRow.SAPCode = Convert.ToString(values[0]);
                    itemRow.Quantity = Convert.ToString(values[1]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"SAPCode", Translation.GetTerm("SAPCode")},
					{"Quantity", Translation.GetTerm("Quantity","Quantity")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<SAPCodeRepeatedSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region ShipmentsByProductReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult ShipmentsByProductReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetShipmentsByProductReport(string cuv,
                                                                string sapCode,
                                                                DateTime? completeDateUTCIni,
                                                                DateTime? completeDateUTCFin,
                                                                int page,
                                                                int pageSize,
                                                                string orderBy,
                                                                NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                if (string.IsNullOrEmpty(cuv) && string.IsNullOrEmpty(sapCode))
                {
                    return Json(new { totalPages = 0, page = "", message = "CUV OR SAP CODE MUST BE INCLUDE FOR SEARCHING" });
                }

                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("ItemsSentByProd",
                                                   new Dictionary<string, object>() { {"@CUV", cuv == ""? null : cuv},
                                                                                      {"@SKU", sapCode == ""? null : sapCode}},
                                                                                      "Core"));

                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCFin), type = eFilterType.DateTime, IsRank = true });

                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);

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
        public virtual ActionResult ShipmentsByProductExport(string parameters)
        {
            try
            {
                #region Parameters

                string[] parameter = parameters.Split(Convert.ToChar("*"));
                string cuv = parameter[0];
                string sapCode = parameter[1];

                DateTime? completeDateUTCIni = null;
                //if (!string.IsNullOrEmpty(parameter[2])) completeDateUTCIni = Convert.ToDateTime(parameter[2].DateFormat()); //local
                if (!string.IsNullOrEmpty(parameter[2])) completeDateUTCIni = Convert.ToDateTime(parameter[2]);

                DateTime? completeDateUTCFin = null;
                //if (!string.IsNullOrEmpty(parameter[3])) completeDateUTCFin = Convert.ToDateTime(parameter[3].DateFormat()); //local
                if (!string.IsNullOrEmpty(parameter[3])) completeDateUTCFin = Convert.ToDateTime(parameter[3]);

                #endregion

                #region GetDataFromStore

                if (string.IsNullOrEmpty(cuv) && string.IsNullOrEmpty(sapCode))
                {
                    return Json(new { totalPages = 0, page = "", message = "CUV OR SAP CODE MUST BE INCLUDE FOR SEARCHING" });
                }
                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("ItemsSentByProd",
                                                   new Dictionary<string, object>() { {"@CUV", cuv == ""? null : cuv},
                                                                                      {"@SKU", sapCode == ""? null : sapCode}},
                                                                                      "Core"));

                List<FilterSearchData> FilterList = new List<FilterSearchData>();

                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCIni), type = eFilterType.DateTime, IsRank = true });
                FilterList.Add(new FilterSearchData() { IndexColumn = 5, Value = Convert.ToString(completeDateUTCFin), type = eFilterType.DateTime, IsRank = true });

                string expression = FilterList.GetFilterExpression(ds);
                DataView dv = new DataView(ds.Tables[0], expression, "OrderNumber ASC", DataViewRowState.CurrentRows);

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("ShipmentsByProductExport", "ShipmentsByProduct Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<ShipmentsByProductSearchData> data = new PaginatedList<ShipmentsByProductSearchData>();

                foreach (System.Data.DataRow row in dv.ToTable("Table").Rows)
                {
                    ShipmentsByProductSearchData itemRow = new ShipmentsByProductSearchData();
                    object[] values = row.ItemArray;
                    itemRow.OrderNumber = Convert.ToString(values[0]);
                    itemRow.AccountNumber = Convert.ToString(values[1]);
                    itemRow.AccountType = Convert.ToString(values[2]);
                    itemRow.OrderType = Convert.ToString(values[3]);
                    itemRow.OrderStatus = Convert.ToString(values[4]);
                    itemRow.OrderDate = Convert.ToString(values[5]);
                    itemRow.ParentCode = Convert.ToString(values[6]);
                    itemRow.CUV = Convert.ToString(values[7]);
                    itemRow.SAPcode = Convert.ToString(values[8]);
                    itemRow.BPCSCode = Convert.ToString(values[9]);
                    itemRow.ProductDescription = Convert.ToString(values[10]);
                    itemRow.Quantity = Convert.ToString(values[11]);
                    itemRow.OrderShipment = Convert.ToString(values[12]);
                    itemRow.Price = Convert.ToString(values[13]);
                    itemRow.CV = Convert.ToString(values[14]);
                    itemRow.QV = Convert.ToString(values[15]);
                    itemRow.NetSale = Convert.ToString(values[16]);
                    itemRow.Subtotal = Convert.ToString(values[17]);
                    itemRow.TaxAmount = Convert.ToString(values[18]);
                    itemRow.GrandTotal = Convert.ToString(values[19]);

                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"OrderNumber", Translation.GetTerm("OrderNumber")},
					{"AccountNumber", Translation.GetTerm("AccountNumber","AccountNumber")},
					{"AccountType", Translation.GetTerm("AccountType","AccountType")},
					{"OrderType", Translation.GetTerm("OrderType","OrderType")},
					{"OrderStatus", Translation.GetTerm("OrderStatus", "OrderStatus")},
                    {"OrderDate", Translation.GetTerm("OrderDate", "OrderDate")},
                    {"ParentCode", Translation.GetTerm("ParentCode", "ParentCode")},
                    {"CUV", Translation.GetTerm("CUV", "CUV")},
                    {"SAPcode", Translation.GetTerm("SAPcode", "SAPcode")},
                    {"BPCSCode", Translation.GetTerm("BPCSCode", "BPCSCode")},
                    {"ProductDescription", Translation.GetTerm("ProductDescription", "ProductDescription")},
                    {"Quantity", Translation.GetTerm("Quantity", "Quantity")},
                    {"OrderShipment", Translation.GetTerm("OrderShipment", "OrderShipment")},
                    {"Price", Translation.GetTerm("Price", "Price")},
                    {"CV", Translation.GetTerm("CV", "CV")},
                    {"QV", Translation.GetTerm("QV", "QV")},
                    {"NetSale", Translation.GetTerm("NetSale", "NetSale")},
                    {"Subtotal", Translation.GetTerm("Subtotal", "Subtotal")},
                    {"TaxAmount", Translation.GetTerm("TaxAmount", "TaxAmount")},
                    {"GrandTotal", Translation.GetTerm("GrandTotal", "GrandTotal")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ShipmentsByProductSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region PickingPendingsReport
        //Developed by Wesley Campos S. - CSTI

        public ActionResult PickingPendingsReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult GetPickingPendingsReport(int page,
                                                             int pageSize,
                                                             string orderBy,
                                                             NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsPickingPending",
                                                                         new Dictionary<string, object>() { },
                                                                         "Core"));
                DataView dv = new DataView(ds.Tables[0], "", string.Format("{0} {1}", orderBy, Convert.ToString(orderByDirection).StartsWith("A") ? "ASC" : "DESC"), DataViewRowState.CurrentRows);

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
        public virtual ActionResult PickingPendingsExport()
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(DataAccess.GetCommand("upsPickingPending",
                                                                          new Dictionary<string, object>() { },
                                                                          "Core"));
                DataView dv = new DataView(ds.Tables[0], "", "OrderNumber ASC", DataViewRowState.CurrentRows);

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("PickingPendingsExport", "PickingPendings Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<PickingPendingsSearchData> data = new PaginatedList<PickingPendingsSearchData>();

                foreach (System.Data.DataRow row in dv.ToTable("Table").Rows)
                {
                    PickingPendingsSearchData itemRow = new PickingPendingsSearchData();
                    object[] values = row.ItemArray;
                    itemRow.OrderNumber = Convert.ToString(values[0]);
                    itemRow.OrderType = Convert.ToString(values[1]);
                    itemRow.OrderStatus = Convert.ToString(values[2]);
                    itemRow.AccountNumber = Convert.ToString(values[3]);
                    itemRow.REPName = Convert.ToString(values[4]);
                    itemRow.OrderDate = Convert.ToString(values[5]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"OrderNumber", Translation.GetTerm("OrderNumber")},
                    {"OrderType", Translation.GetTerm("OrderType","OrderType")},
                    {"OrderStatus", Translation.GetTerm("OrderStatus", "OrderStatus")},
					{"AccountNumber", Translation.GetTerm("AccountNumber","AccountNumber")},
					{"REPName", Translation.GetTerm("REPName","REPName")},
                    {"OrderDate", Translation.GetTerm("OrderDate", "OrderDate")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<PickingPendingsSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion

        #region CancellPaidWithoutReturn
        //Developed by Luis Peña V. - CSTI

        public ActionResult CancellPaidWithoutReturnReportGate()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]

        public virtual ActionResult GetCancellPaidWithoutReturnReport(int page,
                                                                      int pageSize,
                                                                      string orderBy,
                                                                      NetSteps.Common.Constants.SortDirection orderByDirection)
        {
            try
            {
                #region GetDataFromStore

                DataSet ds = DataAccess.GetDataSet(GetCommand("upsCancellPaidWithoutReturn", "Core"));
                List<FilterSearchData> FilterList = new List<FilterSearchData>();

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
        public virtual ActionResult CancellPaidWithoutReturnExport()
        {
            try
            {
                #region GetDataFromStore

                DataTable table = CurrentReportView;

                #endregion

                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("CancellPaidWithoutReturnExport", "Cancell Paid Without Return Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
                PaginatedList<CancellPaidWithoutReturnSearchData> data = new PaginatedList<CancellPaidWithoutReturnSearchData>();

                foreach (System.Data.DataRow row in table.Rows)
                {
                    CancellPaidWithoutReturnSearchData itemRow = new CancellPaidWithoutReturnSearchData();
                    object[] values = row.ItemArray;
                    itemRow.OrderNumber = Convert.ToString(values[0]);
                    itemRow.OrderType = Convert.ToString(values[1]);
                    itemRow.OrderStatus = Convert.ToString(values[2]);
                    itemRow.AccountNumber = Convert.ToString(values[3]);
                    itemRow.Name = Convert.ToString(values[4]);
                    itemRow.OrderDate = Convert.ToString(values[5]);
                    data.Add(itemRow);
                }

                // TODO: Make these exported columns pull dynamically from user specified 'visible columns' later when functionality is available to users - JHE
                var columns = new Dictionary<string, string>
				{
					{"OrderNumber", Translation.GetTerm("OrderNumber")},
					{"OrderType", Translation.GetTerm("OrderType","OrderType")},
					{"OrderStatus", Translation.GetTerm("OrderStatus","OrderStatus")},
                    {"AccountNumber", Translation.GetTerm("AccountNumber")},
					{"Name", Translation.GetTerm("Name","Name")},
					{"OrderDate", Translation.GetTerm("OrderDate","OrderDate")},
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<CancellPaidWithoutReturnSearchData>(fileNameSave, data, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        #endregion


        #region Modifications @01

        #region TotalCommissions

        public ActionResult TotalCommissionsReportGate()
        {
            ViewBag.Periods = CommissionBusinessLogic.Instance.GetPeriods();
            ViewBag.CommTypes = CommissionBusinessLogic.Instance.GetCommissionTypes();

            return View();
        }

        public virtual ActionResult GetTotalComissionsReport(Constants.SortDirection orderByDirection, int page, int pageSize,
                                                                string orderBy, int? periodId, string commissionType, string accountNumber)
        {
            int accountId;
            bool canCast = Int32.TryParse(accountNumber, out accountId);
            if (accountNumber.Length > 0)
            {
                if (!canCast)
                    return Json(new { success = false, message = "Invalid Account Number", totalPages = 1, page = String.Empty });
            }

            var list = CommissionBusinessLogic.Instance.GetTotalCommissions(new CommissionSearchParameters
            {
                PageNumber = page,
                PageSize = pageSize,
                PeriodID = periodId.HasValue ? periodId.Value : 0,
                CommissionType = commissionType.Equals("0") ? null : commissionType,
                AccountID = accountId,
                OrderColumn = orderBy,
                SortDirection = orderByDirection
            });

            StringBuilder builder = new StringBuilder();
            int totalPages = 0;

            foreach (CommissionSearchData com in list)
            {
                totalPages = com.TotalPages;
                builder.Append("<tr>")
                       .AppendCell(com.PeriodID.ToString())
                       .AppendLinkCell(String.Format("~/Reports/Reports/SendData?PeriodID={0}&CommType={1}&AccountID={2}",
                                                            com.PeriodID, com.CommissionType, com.AccountNumber), com.AccountNumber.ToString())
                       .AppendCell(com.AccountName)
                       .AppendCell(com.CommissionType)
                       .AppendCell(com.CommissionName)
                       .AppendCell(com.CommissionAmount.ToString())
                       .Append("</tr>");
            }


            return Json(new { success = true, message = String.Empty, totalPages = totalPages, page = builder.ToString() });
        }

        #endregion

        #region DetailCommissions

        public ActionResult SendData(int? PeriodID, string CommType, int? AccountID)
        {
            TempData["periodId"] = PeriodID;
            TempData["comType"] = CommType;
            TempData["accountId"] = AccountID;

            return RedirectToAction("DetailCommissionsReportGate");
        }

        public ActionResult DetailCommissionsReportGate()
        {
            ViewBag.Periods = CommissionBusinessLogic.Instance.GetPeriods();
            ViewBag.CommTypes = CommissionBusinessLogic.Instance.GetCommissionTypes();
            ViewBag.period = TempData["periodId"];
            ViewBag.comType = TempData["comType"];
            ViewBag.account = TempData["accountId"];

            return View();
        }

        public virtual ActionResult GetDetailComissionsReport(Constants.SortDirection orderByDirection, int page, int pageSize,
                                                                string orderBy, int? periodId, string commissionType, string accountNumber)
        {
            int accountId;
            bool canCast = Int32.TryParse(accountNumber, out accountId);
            if (accountNumber.Length > 0)
            {
                if (!canCast)
                    return Json(new { success = false, message = "Invalid Account Number", totalPages = 1, page = String.Empty });
            }

            var list = CommissionBusinessLogic.Instance.GetDetailCommissions(new CommissionDetailSearchParameters
            {
                PageNumber = page,
                PageSize = pageSize,
                PeriodID = periodId.HasValue ? periodId.Value : 0,
                CommissionType = commissionType.Equals("0") ? null : commissionType,
                AccountID = accountId,
                OrderColumn = orderBy,
                SortDirection = orderByDirection
            });

            StringBuilder builder = new StringBuilder();
            int totalPages = 1;

            foreach (CommissionDetailSearchData det in list)
            {
                totalPages = det.TotalPages;
                builder.Append("<tr>")
                       .AppendCell(det.SponsorID.ToString())
                       .AppendCell(det.SponsorName)
                       .AppendCell(det.AccountNumber.ToString())
                       .AppendCell(det.AccountName)
                       .AppendCell(det.CommissionType)
                       .AppendCell(det.CommissionName)
                       .AppendCell(det.OrderNumber.ToString())
                       .AppendCell(det.CommissionableValue.ToString())
                       .AppendCell(det.Percentage.ToString())
                       .AppendCell(det.PayoutAmount.ToString())
                       .AppendCell(det.PeriodID.ToString())
                       .Append("</tr>");
            }

            return Json(new { success = true, message = String.Empty, totalPages = totalPages, page = builder.ToString() });
        }

        #endregion

        #endregion

        #region Modifications @02

        #region DebtsPerAge

        public ActionResult DebtsPerAge()
        {

            return View();
        }

        public ActionResult BrowseDebtsPerAge(int? accountId, string accountText, DateTime? startBirth, DateTime? endBirth,
                                                DateTime? startDue, DateTime? endDue, int? startOverdue, int? endOverdue,
                                                string orderNumber, string forfeit)
        {
            var model = new DebtsPerAgeSearchParameters()
            {
                AccountId = accountId,
                AccountText = accountText,
                StartBirthDate = startBirth,
                EndBirthDate = endBirth,
                StartDueDate = startDue,
                EndDueDate = endDue,
                DaysOverdueStart = startOverdue,
                DaysOverdueEnd = endOverdue,
                OrderNumber = String.IsNullOrEmpty(orderNumber) ? null : orderNumber,
                Forfeit = forfeit == "" ? null : (bool?)Boolean.Parse(forfeit)
            };

            return View(model);
        }

        public ActionResult GetTableDebtsPerAge(Constants.SortDirection orderByDirection, int page, int pageSize,
                                                string orderBy, int? accountId, DateTime? startBirth, DateTime? endBirth,
                                                DateTime? startDue, DateTime? endDue, int? startOverdue, int? endOverdue,
                                                string orderNumber, string forfeit)
        {
            List<DebtsPerAgeSearchData> list = OrderPaymentBizLogic.Instance.GetTableDebtsPerAge(new DebtsPerAgeSearchParameters()
            {
                AccountId = accountId,
                StartBirthDate = startBirth,
                EndBirthDate = endBirth,
                StartDueDate = startDue,
                EndDueDate = endDue,
                DaysOverdueStart = startOverdue,
                DaysOverdueEnd = endOverdue,
                OrderNumber = orderNumber,
                Forfeit = forfeit == "" ? null : (bool?)Boolean.Parse(forfeit),

                PageNumber = page,
                PageSize = pageSize,
                SortOrder = orderByDirection,
                OrderBy = orderBy
            });

            StringBuilder builder = new StringBuilder();
            int totalPages = 0;

            foreach (DebtsPerAgeSearchData debt in list)
            {
                totalPages = debt.TotalPages;
                builder.Append("<tr>")
                       .AppendCell(debt.AccountNumber.ToString())
                       .AppendCell(debt.FirstName)
                       .AppendCell(debt.LastName)
                       .AppendCell(debt.PaymentTicketNumber.HasValue ? debt.PaymentTicketNumber.Value.ToString() : String.Empty)
                       .AppendCell(debt.OrderNumber.ToString())
                       .AppendCell(debt.NfeNumber.HasValue ? debt.NfeNumber.Value.ToString() : String.Empty)
                       .AppendCell(debt.OrderDate.HasValue ? debt.OrderDate.Value.ToShortDateString() : String.Empty)
                       .AppendCell(debt.ExpirationDate.HasValue ? debt.ExpirationDate.Value.ToShortDateString() : String.Empty)
                       .AppendCell(debt.BalanceDate.HasValue ? debt.BalanceDate.ToShortDateString() : String.Empty)
                       .AppendCell(debt.OriginalBalance.ToString())
                       .AppendCell(debt.CurrentBalance.ToString())
                       .AppendCell(debt.OverdueDays.ToString())
                       .AppendCell(debt.Forfeit.ToString())
                       .AppendCell(debt.Period.ToString())
                       .AppendCell(debt.DateOfBirth.HasValue ? debt.DateOfBirth.ToShortDateString() : String.Empty)
                       .Append("</tr>");
            }

            ViewData["Campo"] = "Textote";

            return Json(new { success = true, message = String.Empty, totalPages = totalPages, page = builder.ToString() });
        }

        public ActionResult DebtsPerAgeExport(int? accountId, DateTime? startBirth, DateTime? endBirth,
                                                DateTime? startDue, DateTime? endDue, int? startOverdue, int? endOverdue,
                                                string orderNumber, string forfeit)
        {
            string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("DebtsPerAgeExport", "Debts Per Age Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
            var listToExport = OrderPaymentBizLogic.Instance.TableDebtsPerAgeExport(new DebtsPerAgeSearchParameters()
            {
                AccountId = accountId,
                StartBirthDate = startBirth,
                EndBirthDate = endBirth,
                StartDueDate = startDue,
                EndDueDate = endDue,
                DaysOverdueStart = startOverdue,
                DaysOverdueEnd = endOverdue,
                OrderNumber = String.IsNullOrEmpty(orderNumber) ? null : orderNumber,
                Forfeit = forfeit == "" ? null : (bool?)Boolean.Parse(forfeit)
            });

            var columns = new Dictionary<string, string>
			{
				{"AccountNumber", Translation.GetTerm("AccountNumber","Account Number")},
				{"FirstName", Translation.GetTerm("FirstName","First Name")},
				{"LastName", Translation.GetTerm("LastName","Last Name")},
				{"PaymentTicketNumber", Translation.GetTerm("PaymentTicketNumber", "Payment Ticket Number")},
				{"OrderNumber", Translation.GetTerm("OrderNumber", "Order Number")},
				{"NfeNumber", Translation.GetTerm("NfeNumber","NFE Number")},
				{"OrderDate", Translation.GetTerm("OrderDate","Order Date")},
                {"ExpirationDate", Translation.GetTerm("ExpirationDate", "Expiration Date")},
				{"BalanceDate", Translation.GetTerm("Balance Date", "Balance Date")},
				{"OriginalBalance", Translation.GetTerm("OriginalBalance","Original Balance")},
				{"CurrentBalance", Translation.GetTerm("CurrentBalance","Current Balance")},
				{"OverdueDays", Translation.GetTerm("OverdueDays", "Overdue Days")},
				{"Forfeit", Translation.GetTerm("Forfeit", "Forfeit")},
				{"Period", Translation.GetTerm("Period","Period")},
				{"DateOfBirth", Translation.GetTerm("RangeDateOfBirth","Date of Birth")}
			};

            return new NetSteps.Web.Mvc.ActionResults.ExcelResult<DebtsPerAgeSearchData>(fileNameSave, listToExport, columns, null, columns.Keys.ToArray());
        }

        #endregion

        #region TicketPaymentsPerMonth

        public ActionResult TicketPaymentsPerMonth()
        {
            ViewBag.statuses = OrderPaymentBizLogic.Instance.GetDropDownStatuses();

            return View();
        }

        public ActionResult BrowseTicketPaymentsPerMonth(int? ticketNumber, string ticketText, int? accountId, string accountText,
                                                        DateTime? startIssue, DateTime? endIssue, DateTime? startDue, DateTime? endDue,
                                                        string orderNumber, int? statusId, string month)
        {
            if (month.Length != 6)
                return RedirectToAction("TicketPaymentsPerMonth");


          




            //var f = Convert.ToDateTime(startIssue1);

            int selYear = Int32.Parse(month.Substring(0, 4));
            int selMonth = Int32.Parse(month.Substring(4));
            int lastMonthDay = DateTime.DaysInMonth(selYear, selMonth);

            ViewBag.statuses = OrderPaymentBizLogic.Instance.GetDropDownStatuses();
            var model = new TicketPaymentPerMonthSearchParameters()
            {
                TicketNumber = ticketNumber,
                TicketText = ticketText,
                AccountId = accountId,
                AccountText = accountText,

                StartIssueDate = startIssue,
                EndIssueDate = endIssue,
                StartDueDate = startDue,
                EndDueDate = endDue,
                //StartIssueDate =query["startIssue"] == "null" ? null:  (DateTime?)Convert.ToDateTime(query["startIssue"]),
                //EndIssueDate = query["endIssue"] == "null" ? null : (DateTime?)Convert.ToDateTime(query["endIssue"]),
                //StartDueDate = query["startDue"] == "null" ? null : (DateTime?)Convert.ToDateTime(query["startDue"]),
                //EndDueDate = query["endDue"] == "null" ? null : (DateTime?)Convert.ToDateTime(query["endDue"]),
                OrderNumber = String.IsNullOrEmpty(orderNumber) ? null : orderNumber,
                StatusId = statusId,
                Month = new DateTime(selYear, selMonth, lastMonthDay)
            };

            return View(model);
        }

        public ActionResult GetTableTicketPaymentsPerMonth(Constants.SortDirection orderByDirection, int page, int pageSize,
                                                            string orderBy, int? ticketNumber, int? accountId, DateTime? startIssue,
                                                            DateTime? endIssue, DateTime? startDue, DateTime? endDue,
                                                            string orderNumber, int? statusId, string month)
        {
            if (month.Length != 6)
                return Json(new
                {
                    success = false,
                    message = Translation.GetTerm("YearMonthRequired", "Month is Required: YYYYMM"),
                    totalPages = 0,
                    page = Translation.GetTerm("YearMonthRequired", "Month is Required: YYYYMM")
                });


            //var query = this.HttpContext.Request.QueryString;
                
            int selYear = Int32.Parse(month.Substring(0, 4));
            int selMonth = Int32.Parse(month.Substring(4));
            int lastMonthDay = DateTime.DaysInMonth(selYear, selMonth);

            List<TicketPaymentPerMonthSearchData> list = OrderPaymentBizLogic.Instance.GetTableTicketPaymentsPerMonth(
                new TicketPaymentPerMonthSearchParameters()
                {
                    TicketNumber = ticketNumber,
                    AccountId = accountId,

                    //StartIssueDate = System.Web.WebPages.StringExtensions.IsDateTime(query["startIssue"]) == false ? null : (DateTime?)Convert.ToDateTime(query["startIssue"]),
                    //EndIssueDate = System.Web.WebPages.StringExtensions.IsDateTime(query["endIssue"]) == false ? null : (DateTime?)Convert.ToDateTime(query["endIssue"]),
                    //StartDueDate = System.Web.WebPages.StringExtensions.IsDateTime(query["startDue"]) == false ? null : (DateTime?)Convert.ToDateTime(query["startDue"]),
                    //EndDueDate = System.Web.WebPages.StringExtensions.IsDateTime(query["endDue"]) == false ? null : (DateTime?)Convert.ToDateTime(query["endDue"]),


                    StartIssueDate = startIssue,
                    EndIssueDate = endIssue,
                    StartDueDate = startDue,
                    EndDueDate = endDue,
                    OrderNumber = String.IsNullOrEmpty(orderNumber) ? null : orderNumber,
                    StatusId = statusId,
                    Month = new DateTime(selYear, selMonth, lastMonthDay),

                    PageNumber = page,
                    PageSize = pageSize,
                    SortOrder = orderByDirection,
                    OrderBy = orderBy
                });

            StringBuilder builder = new StringBuilder();
            int totalPages = 0;

            foreach (TicketPaymentPerMonthSearchData debt in list)
            {
                totalPages = debt.TotalPages;
                builder.Append("<tr>")
                       .AppendCell(debt.PaymentTicketNumber > 0 ? debt.PaymentTicketNumber.ToString() : String.Empty)
                       .AppendCell(debt.OrderNumber.ToString())
                       .AppendCell(debt.NfeNumber.HasValue ? debt.NfeNumber.Value.ToString() : String.Empty)
                       .AppendCell(debt.OrderDate.HasValue ? debt.OrderDate.Value.ToShortDateString() : String.Empty)
                       .AppendCell(debt.ExpirationDate.HasValue ? debt.ExpirationDate.Value.ToShortDateString() : String.Empty)
                       .AppendCell(debt.BalanceDate.HasValue ? debt.BalanceDate.ToShortDateString() : String.Empty)
                       .AppendCell(debt.OriginalBalance.ToString())
                       .AppendCell(debt.CurrentBalance.ToString())
                       .AppendCell(debt.Status.ToString())
                       .AppendCell(debt.OriginalExpirationDate.HasValue ? debt.OriginalExpirationDate.Value.ToShortDateString() : String.Empty)
                       .AppendCell(debt.AccountNumber.ToString())
                       .AppendCell(debt.FirstName)
                       .AppendCell(debt.LastName)
                       .AppendCell(debt.PhoneNumber)
                       .Append("</tr>");
            }

            return Json(new { success = true, message = String.Empty, totalPages = totalPages, page = builder.ToString() });
        }
        public string GetCulturaFormat(string valor)
        {
            var fomatos = new List<System.Globalization.CultureInfo>
            {
                new System.Globalization.CultureInfo("pt-BR"),
                new System.Globalization.CultureInfo("en-US"),
                new System.Globalization.CultureInfo("es-US")

            };
            bool correcto = false;
            decimal numero = 0;
            DateTime fecha = DateTime.Now;
            //style = NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands |
            //NumberStyles.AllowParentheses; 

            foreach (var item in fomatos)
            {
              
                    if (DateTime.TryParse(valor, item, System.Globalization.DateTimeStyles.None, out fecha) == true)
                    {
                        correcto = true;
                        break;
                    }

                

            }
            if (correcto)
            {
               
              return fecha.ToString(CoreContext.CurrentCultureInfo);
            }
            else
                return valor;


        }
        public ActionResult TicketPaymentsPerMonthExport(int? ticketNumber, int? accountId, DateTime? startIssue, DateTime? endIssue,
                                                            DateTime? startDue, DateTime? endDue, string orderNumber, int? statusId,
                                                            string month)
        {
            if (month.Length != 6)
                return Json(new
                {
                    success = false,
                    message = Translation.GetTerm("YearMonthRequired", "Month is Required: YYYYMM"),
                    totalPages = 0,
                    page = String.Empty
                });

            int selYear = Int32.Parse(month.Substring(0, 4));
            int selMonth = Int32.Parse(month.Substring(4));
            int lastMonthDay = DateTime.DaysInMonth(selYear, selMonth);

            string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("TicketPaymentsPerMonthExport", "Ticket Payments Per Month Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));
            var listToExport = OrderPaymentBizLogic.Instance.TableTicketPaymentsPerMonthExport(new TicketPaymentPerMonthSearchParameters()
            {
                TicketNumber = ticketNumber,
                AccountId = accountId,
                StartIssueDate = startIssue,
                EndIssueDate = endIssue,
                StartDueDate = startDue,
                EndDueDate = endDue,
                OrderNumber = String.IsNullOrEmpty(orderNumber) ? null : orderNumber,
                StatusId = statusId,
                Month = new DateTime(selYear, selMonth, lastMonthDay)
            });

            var columns = new Dictionary<string, string>
			{
				{"PaymentTicketNumber", Translation.GetTerm("PaymentTicketNumber", "Payment Ticket Number")},
				{"OrderNumber", Translation.GetTerm("OrderNumber", "Order Number")},
				{"NfeNumber", Translation.GetTerm("NfeNumber","NFE Number")},
				{"OrderDate", Translation.GetTerm("OrderDate","Order Date")},
                {"ExpirationDate", Translation.GetTerm("ExpirationDate", "Expiration Date")},
				{"BalanceDate", Translation.GetTerm("BalanceDate", "Balance Date")},
				{"OriginalBalance", Translation.GetTerm("OriginalBalance","Original Balance")},
				{"CurrentBalance", Translation.GetTerm("CurrentBalance","Current Balance")},
                {"Status", Translation.GetTerm("Status", "Status")},
				{"OriginalExpirationDate", Translation.GetTerm("OriginalExpirationDate", "Original Expiration Date")},
				{"AccountNumber", Translation.GetTerm("AccountNumber","Account Number")},
				{"FirstName", Translation.GetTerm("FirstName","First Name")},
				{"LastName", Translation.GetTerm("LastName","Last Name")},
				{"PhoneNumber", Translation.GetTerm("PhoneNumber","Phone Number")}
			};

            return new NetSteps.Web.Mvc.ActionResults.ExcelResult<TicketPaymentPerMonthSearchData>(fileNameSave, listToExport, columns, null, columns.Keys.ToArray());
        }

        public ActionResult SearchTicketNumber(string query)
        {
            // OrderPaymentID, TicketNumber
            Dictionary<int, string> tickets = OrderPaymentBizLogic.Instance.GetTicketNumberLookUp(query);
            return Json(tickets.Select(t => new { id = t.Value, text = t.Value + " (#" + t.Key + ")" }));
        }

        #endregion

        #endregion

        #region ReportsFilterGate
        
        /// <summary>
        /// Metodo que genera los Reportes por Usuario
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportsFilterGate()
        {

            //var reports = from r in db.ReportsServiceList select r;

            var user = CoreContext.CurrentUser;
            // Importa la lista de los URLs con sus parametros
            var reports = DataAccess.ExecWithStoreProcedureLists<ReportsServiceProcModel>("Commissions", "Reports.uspGetReportsServiceList").ToList();
            // Importa las funciones activas para el ROL de Reports            
            var reportsRoles = DataAccess.ExecWithStoreProcedureListParam<ReportsServiceROLModel>("Commissions", "Reports.uspGetReportsServiceROLList",
                new SqlParameter("userID", SqlDbType.Int) { Value = user.UserID }).ToList();

            List<ReportsServiceModel> reporteReturn = new List<ReportsServiceModel>();

            List<string> lst = new List<string>();

            foreach (var itemsin in reports)
            {
                lst.Clear();
                if (itemsin.RolesReport != null || itemsin.RolesReport == "")
                {
                    string v_FillColumns = itemsin.RolesReport.ToString();
                    char delimiter = ',';
                    string[] fieldFill = v_FillColumns.Split(delimiter);
                    lst.AddRange(fieldFill);
                    foreach (var interno in lst)
                    {
                        if (reportsRoles.Exists(x => x.listRolesReport == Convert.ToInt32(interno)))
                        {
                            reporteReturn.Add(new ReportsServiceModel() { reportID = itemsin.reportID, reportName = itemsin.reportName, reportURL = itemsin.reportURL });
                            break;
                        }
                    }
                }
            }

            ViewData["reporteremoto"] = NetSteps.Common.Configuration.ConfigurationManager.AppSettings["ServerReportinService"];

            return View(reporteReturn);
        }

        public ActionResult ViewReports(string reporte)
        {
            string urlReporte = NetSteps.Common.Configuration.ConfigurationManager.AppSettings["ServerReportinService"] + "?reporte=" + reporte;
            return View(urlReporte);
        }

       
        #endregion

    }
}