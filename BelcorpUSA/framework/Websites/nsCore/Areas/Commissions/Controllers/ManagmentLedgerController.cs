

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
using Newtonsoft.Json;
using nsCore.Areas.Products.Controllers;

namespace nsCore.Areas.Commissions.Controllers
{
    // inicio 110052017
    // comentado por Ivan Pinedo => para solicionar el problema This request has been blocked because sensitive 
    //            information could be disclosed to third party web sites when this is used in a GET request. To allow GET requests, set JsonRequestBehavior to AllowGet.
    //public class ManagmentLedgerController : Controller
    // fin 11052017
    public class ManagmentLedgerController : BaseCommissionController
    {
        
        /// <summary>
        /// author           : mescobar
        /// company         : CSTI - Peru
        /// create        : 12/18/2015
        /// reason          : class controller of ManagmentLedger
        /// modified    : 
        /// reason     
        public ActionResult Index()
        {
            TempData["ManagementLedgerExport"] = null;
            return View();
        }
        //DateTime? startDate, DateTime? endDate, decimal entryamount
        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult Get(int? accountId, int? periodId, int? bonustypeId, DateTime? startDate, DateTime? endDate, decimal? entryamount, int? entryreasonId, int? entryoriginId, int? entrytypeId, 
                                        int page, int pageSize, string orderBy, string orderByDirection)
            {
            try
            {
                var tu =CoreContext.CurrentCultureInfo;
                var l =System.Globalization.CultureTypes.AllCultures;
                var ri = System.Globalization.RegionInfo.CurrentRegion;

                if (periodId == null)
                {
                    return Json(new { totalPages = 0, page = "<tr><td colspan=\"7\">There are no ManagmentLedger. Disbusement must be include for searching.</td></tr>", message = "DISBURSEMENT MUST BE INCLUDE FOR SEARCHING" });

                }

                if (accountId == null)
                {
                    accountId = 0;
                }

                if (entryamount == null)
                {
                    entryamount = 0;
                }

                if (orderByDirection == "Descending")
                {
                    orderByDirection = "desc";
                }
                else
                {
                    orderByDirection = "asc";
                }

                if (startDate.HasValue && startDate.Value.Year < 1900)
                    startDate = null;
                if (endDate.HasValue && endDate.Value.Year < 1900)
                    endDate = null;

                StringBuilder builder = new StringBuilder();
                int count = 0;

                PaginatedList<ManagmentLedgerSearchData> managamentsledger;

                if (TempData["ManagementLedgerExport"] == null)
                {
                    managamentsledger = new PaginatedList<ManagmentLedgerSearchData>();
                    TempData["ManagementLedgerExport"] = managamentsledger;
                }
                else
                {
                    managamentsledger = ManagmentLedger.Search(new ManagmentLedgerSearchParameters()
                    {
                        AccountID = accountId,
                        PeriodID = periodId,
                        BonusTypeID = bonustypeId,
                        StartDate = startDate,
                        EndDate = endDate,
                        EntryAmount = entryamount,
                        EntryReasonID = entryreasonId,
                        EntryOriginID = entryoriginId,
                        EntryTypeID = entrytypeId,
                        PageSize = pageSize,
                        PageIndex = page,
                        OrderBy = orderBy,
                        Order = orderByDirection
                    });
                }
                //TempData["ManagementLedgerExport"] = managamentsledger;
                //var td = TempData["ManagementLedgerExport"];
                TempData["ManagementLedgerExport"] = JsonConvert.SerializeObject(managamentsledger);

                foreach (var managamentledger in managamentsledger)
                {

                    builder.Append("<tr>");
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.AccountID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.ConsultantName.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.PeriodID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.BonusType.ToString()));
                    //builder.Append(String.Format("<td>{0}</td>", managamentledger.EntryAmount.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", Convert.ToDecimal(managamentledger.EntryAmount).ToString("N", CoreContext.CurrentCultureInfo)));
                    builder.Append(String.Format("<td>{0}</td>", string.IsNullOrEmpty(managamentledger.EndingBalance) ? "" : managamentledger.EndingBalance));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.EntryDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.EffectiveDate.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.UserID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.EntryNotes.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.EntryReason.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.EntryOrigin.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.EntryType.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.EntryDescription.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", managamentledger.DisbursementID.ToString()));
                    builder.Append(String.Format("<td>{0}</td>", Translation.GetTerm(managamentledger.DisbursementStatusTermName, managamentledger.DisbursementStatusTermName)));
                    builder.Append("</tr>");
                    ++count;
                }
              
                return Json(new { result = true, totalPages = managamentsledger.TotalPages, page = managamentsledger.TotalCount == 0 ? String.Format("<tr><td colspan=\"17\">{0}</td></tr>", Translation.GetTerm("ThereAreNoManagmentLedger", "There are not managment ledger")) : builder.ToString() });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        public virtual ActionResult Export(int? accountId, int? periodId, int? bonustypeId, DateTime? startDate, DateTime? endDate, decimal? entryamount, int? entryreasonId, int? entryoriginId, int? entrytypeId)
        {
            try
            {
                string fileNameSave = string.Format("{0}_{1}.xlsx", Translation.GetTerm("ManagementLedgerExport", "Management_Ledger_Export"), DateTime.Now.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo));

                PaginatedList<ManagmentLedgerSearchData> managamentsledger;

                if (TempData["ManagementLedgerExport"] == null)
                {
                    managamentsledger = new PaginatedList<ManagmentLedgerSearchData>();
                }
                else
                {
                    managamentsledger = ManagmentLedger.Search(new ManagmentLedgerSearchParameters()
                    {
                        AccountID = accountId,
                        PeriodID = periodId,
                        BonusTypeID = bonustypeId,
                        StartDate = startDate,
                        EndDate = endDate,
                        EntryAmount = entryamount,
                        EntryReasonID = entryreasonId,
                        EntryOriginID = entryoriginId,
                        EntryTypeID = entrytypeId,
                        PageSize = 0,
                        PageIndex = 0,
                        OrderBy = string.Empty,
                        Order = "asc"
                    },
                    true
                    );
                }

                var columns = new Dictionary<string, string>
				{
					{"AccountID", Translation.GetTerm("AccountID","AccountID")},
                    {"ConsultantName", Translation.GetTerm("ConsultantName","ConsultantName")},
                    {"PeriodID", Translation.GetTerm("PeriodID","PeriodID")},
                    {"BonusType", Translation.GetTerm("BonusType","BonusType")},
                    {"EntryAmount", Translation.GetTerm("EntryAmount","EntryAmount")},
                    {"EndingBalance", Translation.GetTerm("EndingBalance","EndingBalance")},
                    {"EntryDate", Translation.GetTerm("EntryDate","EntryDate")},
                    {"EffectiveDate", Translation.GetTerm("EffectiveDate","EffectiveDate")},
                    {"UserID", Translation.GetTerm("UserID","UserID")},
                    {"EntryNotes", Translation.GetTerm("EntryNotes","EntryNotes")},
                    {"EntryReason", Translation.GetTerm("EntryReason","EntryReason")},
                    {"EntryOrigin", Translation.GetTerm("EntryOrigin","EntryOrigin")},
                    {"EntryType", Translation.GetTerm("EntryType","EntryType")},
                    {"EntryDescription", Translation.GetTerm("EntryDescription","EntryDescription")}  
				};

                return new NetSteps.Web.Mvc.ActionResults.ExcelResult<ManagmentLedgerSearchData>(fileNameSave, managamentsledger, columns, null, columns.Keys.ToArray());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

    }
}
