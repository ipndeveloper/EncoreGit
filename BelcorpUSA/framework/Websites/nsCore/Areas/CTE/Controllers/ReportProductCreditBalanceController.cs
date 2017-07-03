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
using System.Globalization;

namespace nsCore.Areas.CTE.Controllers
{
   

    public class ReportProductCreditBalanceController : BaseController
    {
       
        //Developed by Luis Peña V. - CSTI

        


        

        public ActionResult ProductCreditBalanceReportGate()
        {
            return View();
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
                    CreditBalanceMax= credit_BalanceFin,
                    EntryDateFrom= startDate,
                    EntryDateTo= endDate,
                    State = state

                });

                

                foreach (var reneg in ListRenegotiationMethods)
                {

                    builder.Append("<tr>");
                    builder
                        
                        .AppendCell(reneg.BA_ID)
                        .AppendCell(reneg.Name)
                        .AppendCell(FormatDatetimeByLenguage(reneg.EffectiveDate,CoreContext.CurrentCultureInfo))                        
                        .AppendCell(reneg.EntryDescription)
                        .AppendCell(reneg.EntryReasonName)
                        .AppendCell(reneg.EntryOriginName)
                        .AppendCell(reneg.EntryTypeName)

                        // AGREGADO POR IPN
                        .AppendCell(Convert.ToDecimal(reneg.Credit_Balance).ToString("N"), null, decimal.Parse(reneg.Credit_Balance) < 0 ? "color:red; text-align: right" : "text-align: right")
                        .AppendCell(Convert.ToDecimal(reneg.EndingBalance).ToString("N"), null, decimal.Parse(reneg.EndingBalance) < 0 ? "color:red; text-align: right" : "text-align: right")

                        // INICIO 10042017 COMENTADO POR IPN PARA GENERALIZAR EL FORMATO DECIMAL 
                        //.AppendCell(reneg.Credit_Balance.Replace(".", ",").ToDecimal().ToString("N"), null, decimal.Parse(reneg.Credit_Balance) < 0 ? "color:red; text-align: right" : "text-align: right")
                        //.AppendCell(reneg.EndingBalance.Replace(".",",").ToDecimal().ToString("N"), null, decimal.Parse(reneg.EndingBalance) < 0 ? "color:red; text-align: right" : "text-align: right")

                        //  FIN 10042017

                        //.AppendCell(eb, null, decimal.Parse(reneg.EndingBalance) < 0 ? "color:red; text-align: right" : "text-align: right")
                                             
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
       
        public static string FormatDatetimeByLenguage(string fecha, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(fecha))
                return "N/A";
            else
            {

            }
            {
                DateTime? fec = null;
                DateTime Temp;

                var split = fecha.Split('/');
                if (split.Length >= 3)
                {

                    if (culture.Name == "en-US")

                        return string.Format("{0}/{1}/{2}", split[1], split[0], split[2]);

                    else
                        return string.Format("{0}/{1}/{2}", split[0], split[1], split[2]);

                }

                else
                    return fecha;

            }
        }

    }       
}