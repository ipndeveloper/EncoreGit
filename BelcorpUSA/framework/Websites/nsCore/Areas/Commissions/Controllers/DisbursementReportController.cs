

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using NetSteps.Common.Globalization;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Web.Mvc.Business.Controllers;

namespace nsCore.Areas.Commissions.Controllers
{
    public class DisbursementReportController : BaseController
    {

        /// <summary>
        /// author           : mescobar
        /// commpany         : CSTI - Peru
        /// create           : 12/18/2015
        /// reason          : class controller of Report Controller 
        /// modified    : 
        /// reason    
        public ActionResult Index()
        {
            return View();
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int? accountId, int? periodID, int? disbursementstatusID, 
                                        int page, int pageSize,string orderBy, string orderByDirection)
        {
            try
            {

                if (periodID == null)
                {
                    return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">There are no DisbusementReport. Disbursement must be include for searching.</td></tr>", message = "DISBURSEMENT MUST BE INCLUDE FOR SEARCHING" });

                }
                if (accountId == null)
                {
                    accountId = 0;
                }


                if (orderByDirection == "Descending")
                {
                    orderByDirection = "desc";
                }
                else
                {
                    orderByDirection = "asc";
                }

                StringBuilder builder = new StringBuilder();
                int count = 0;
                

                var disbursementreports = DisbursementReport.Search(new DisbursementReportSearchParameters()
                {   
                    AccountID = accountId,
                    PeriodID = periodID,
                    DisbursementStatusID = disbursementstatusID,
                    PageSize = pageSize,
                    PageIndex = page,
                    OrderBy = orderBy,
                    Order = orderByDirection
                });
                foreach (var disbursementreport in disbursementreports)
                {
                   
                    builder.Append("<tr>");
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.PeriodID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.AccountID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.Name.ToString()));

                    builder.Append(String.Format("<td>{0}</td>", Convert.ToDecimal(disbursementreport.Amount).ToString("N", CoreContext.CurrentCultureInfo)));
                    //builder.Append(String.Format("<td>{0}</td>", disbursementreport.Amount.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.DisbursementProfileID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.DisbursementTypeID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.DisbursementID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.DisbursementStatusID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.DisbursementStatus.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.DisbursementDateUTC.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)));
                    builder.Append(String.Format("<td>{0}</td>", disbursementreport.DateModifiedUTC.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)));
                    //builder.Append(String.Format("<td>{0}</td>", disbursementreport.EntryNotes.ToString()));
                    //builder.Append(String.Format("<td>{0}</td>", disbursementreport.EntryReason.ToString()));
                    //builder.Append(String.Format("<td>{0}</td>", disbursementreport.EntryOrigin.ToString()));
                    //builder.Append(String.Format("<td>{0}</td>", disbursementreport.EntryType.ToString()));
                    //builder.Append(String.Format("<td>{0}</td>", disbursementreport.EntryDescription.ToString()));
                    
                    builder.Append("</tr>");
                    ++count;
                }
                return Json(new { result = true, totalPages = disbursementreports.TotalPages, page = disbursementreports.TotalCount == 0 ? String.Format("<tr><td colspan=\"8\">{0}</td></tr>", Translation.GetTerm("ThereAreNoDisbursement", "There are not disbursement")) : builder.ToString() }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult Export(int? accountId, int? periodID, int? disbursementstatusID)
        {
            try
            {
                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("DisbursementReportExport", "Disbursement_Report_Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

                var disbursementreports = DisbursementReport.Search(new DisbursementReportSearchParameters()
                {
                    AccountID = accountId,
                    PeriodID = periodID,
                    DisbursementStatusID = disbursementstatusID,
                    PageSize = 0,
                    PageIndex = 0,
                    OrderBy = string.Empty,
                    Order = "asc"
                },
                true
                );

                var columns = new Dictionary<string, string>
				{
                    {"PeriodID", Translation.GetTerm("PeriodID","PeriodID")},
					{"AccountID", Translation.GetTerm("AccountID","AccountID")},
                    {"Name", Translation.GetTerm("Name","Name")},
                    {"Amount", Translation.GetTerm("Amount","Amount")},
                    {"DisbursementProfileID", Translation.GetTerm("DisbursementProfileID","DisbursementProfileID")},
                    {"DisbursementTypeID", Translation.GetTerm("DisbursementTypeID","DisbursementTypeID")},
                    {"DisbursementID", Translation.GetTerm("DisbursementID","DisbursementID")},
                    {"DisbursementStatusID", Translation.GetTerm("DisbursementStatusID","DisbursementStatusID")},
                    {"DisbursementStatus", Translation.GetTerm("DisbursementStatus","DisbursementStatus")},
                    {"DisbursementDateUTC", Translation.GetTerm("DisbursementDateUTC","DisbursementDateUTC")},
                    {"DateModifiedUTC", Translation.GetTerm("DateModifiedUTC","DateModifiedUTC")},
                    
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<DisbursementReportSearchData>(fileNameSave, disbursementreports, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }


        
            
    }
}