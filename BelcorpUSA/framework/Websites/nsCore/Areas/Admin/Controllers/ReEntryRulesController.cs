using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Generated;
using Newtonsoft.Json.Linq;
using nsCore.Models;
using nsCore.Areas.Admin.Models.ReEntryRules;
using NetSteps.Data.Entities.Exceptions;


namespace nsCore.Areas.Admin.Controllers
{
    public class ReEntryRulesController : Controller
    {    
        public ActionResult Index()
        {
            ReEntryRulesModel model = new ReEntryRulesModel();
            model.listReEntryRules = ReEntryRulesBusinessLogic.GetReEntryRules();
            model.cantReEntryRules = ReEntryRulesBusinessLogic.GetReEntryRules().Count();
            return View(model);
        }


        [HttpPost]
        public ActionResult Save(List<ReEntryRulesParameters> listReEntryRules)
        {
            try
            {
                foreach (ReEntryRulesParameters reEntryRule in listReEntryRules)
                {
                    ReEntryRulesBusinessLogic busines = new ReEntryRulesBusinessLogic();
                    if (reEntryRule.ReEntryRuleID == 0)
                    {
                        busines = new ReEntryRulesBusinessLogic();
                        var res = busines.Insert(reEntryRule);
                        reEntryRule.ReEntryRuleID = res.ID;
                    }
                    else
                    {
                        busines = new ReEntryRulesBusinessLogic();
                        busines.Update(reEntryRule);
                    }
                }
                //return ActionResult("Index");
                return Json(new
                {
                    result = true,
                    reEntryRules = listReEntryRules.Select(apt => new
                    {
                        ReEntryRuleID = apt.ReEntryRuleID,
                        ReEntryRuleValueID = apt.ReEntryRuleValueID
                    })
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
      
    }
}
