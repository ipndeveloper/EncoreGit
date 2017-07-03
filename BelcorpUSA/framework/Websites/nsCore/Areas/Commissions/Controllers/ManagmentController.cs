using System;
using System.Web.Mvc;
using NetSteps.Common.Configuration;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Encore.Core.IoC;

using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Data.Entities.Business;
using NetSteps.Common.Globalization;

namespace nsCore.Areas.Commissions.Controllers
{
    public class ManagmentController : BaseCommissionController
    {
        //
        // GET: /Commissions/Managment/
        public virtual ActionResult Index()
        {
            int ClosedPeriodValue;
            ClosedPeriodValue = DisbursementManagement.GetLatestClosedPeriodByPlan();
            ViewBag.ClosedPeriodValue =  ClosedPeriodValue;
            return View();
            
        }

        [OutputCache(CacheProfile = "DontCache")]
        public virtual ActionResult ProcessManagment(int periodID, int OptionDisbursement)
        {
            try
            {
                string mensaje = string.Empty;
                if (OptionDisbursement == 1)
                {
                    mensaje = DisbursementManagement.ExistsDisbursementsByPeriod(periodID);
                    if (mensaje.Trim().Length > 0)
                    {
                        mensaje = Translation.TermTranslation.GetTerm(mensaje, "Disbursements exist for this period %1").ToString().Replace("%1", periodID.ToString()).ToString();
                        return Json(new { message = mensaje, Success = 0 });
                    }
                    else
                    {
                    
                        DisbursementManagement.MoveBonusValuesToLedgers(periodID);
                        mensaje = Translation.TermTranslation.GetTerm("TheProcessEndedSuccessfully", "The process ended successfully").ToString();
                        return Json(new { message = mensaje, Success = 1 });
                    }
                }
                else
                    if (OptionDisbursement == 2)
                    {
                        mensaje = DisbursementManagement.ExistsDibsCreateRecordsByPeriod(periodID);
                        if (mensaje.Trim().Length > 0)
                        {
                            mensaje = Translation.TermTranslation.GetTerm(mensaje, "Start with the approval process of Commissions and Bonuses").ToString().Replace("%1", periodID.ToString()).ToString();
                            return Json(new { message = mensaje, Success = 0 });
                        }
                        else
                        {
                            DisbursementManagement.DisbCreateRecords(periodID);
                            mensaje = Translation.TermTranslation.GetTerm("TheProcessEndedSuccessfully", "The process ended successfully").ToString();
                            return Json(new { message = mensaje, Success = 1 });
                        }
                    }
                    else
                        if (OptionDisbursement == 3)
                        {
                            mensaje = DisbursementManagement.ExistsSendToBankByPeriod(periodID);
                            if (mensaje.Trim().Length > 0)
                            {
                                mensaje = Translation.TermTranslation.GetTerm(mensaje, "Payment approval process for the period %1").ToString().Replace("%1", periodID.ToString()).ToString();
                                return Json(new { message = mensaje, Success = 0 });
                            }
                            else
                            {
                                DisbursementManagement.SendToBank(periodID);
                                mensaje = Translation.TermTranslation.GetTerm("TheProcessEndedSuccessfully", "The process ended successfully").ToString();
                                return Json(new { message = mensaje, Success = 1 });
                            }
                        }
                var data = new { message = 1 };//Payoneer.SubmitPayments(payoneerDisbursements) };
                return Json(data);

             }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


    }
}
