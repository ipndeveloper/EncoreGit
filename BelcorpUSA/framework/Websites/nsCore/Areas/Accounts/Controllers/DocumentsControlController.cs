using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Shared;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common;
//using nsCore.Areas.Accounts.Models.DocumentsControl;
using NetSteps.Data.Entities.Business.Logic;
using nsCore.Areas.Accounts.Models.DocumentsControl;
using Newtonsoft.Json;

namespace nsCore.Areas.Accounts.Controllers
{
    public class DocumentsControlController : BaseAccountsController
    {

        CreditRequirementsBusinessLogic busines = new CreditRequirementsBusinessLogic();
        RequirementStatusesBusinesLogic status = new RequirementStatusesBusinesLogic();
        [FunctionFilter("Accounts-Commission Ledger", "~/Accounts/DocumentsControl")]
        public virtual ActionResult Index()
        {
            try
            {
                var account = CoreContext.CurrentAccount;

                busines = new CreditRequirementsBusinessLogic();
                status = new RequirementStatusesBusinesLogic();
                var creditReq = busines.GetCreditRequirementsByAccount(account.AccountID);
                DocumentsControlModel model = new DocumentsControlModel();
                model.listCreditRequirement = creditReq;

                IEnumerable <dynamic> AllStatuses = status.GetAllStatus();
                dynamic defaultItem = AllStatuses.Where(x => x.TermName == "ToValidate").FirstOrDefault();

                List<dynamic> RequirementStatusesList = AllStatuses.Where(x => x.TermName != "ToValidate").ToList();
                RequirementStatusesList.Insert(0, defaultItem);
                
                model.lisRequirementStatuses = RequirementStatusesList;
                model.cantRequirement = creditReq.Count();

       
                
                ViewBag.Requirements = JsonConvert.SerializeObject(model.listCreditRequirement);
                return View(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }


        [OutputCache(CacheProfile = "PagedGridData")]
        [FunctionFilter("Accounts-Commission Ledger", "~/Accounts/DocumentsControl")]
        public virtual ActionResult Save(List<CreditRequirementSearchData> creditRequirements)
        {
            try
            {
                foreach (CreditRequirementSearchData creditRequirement in creditRequirements)
                {
                    if (creditRequirement.CreditRequirementID == 0)
                    {

                        creditRequirement.AccountID = CoreContext.CurrentAccount.AccountID;
                        creditRequirement.LastModifiedDate = DateTime.Today;
                        creditRequirement.CreationDate = DateTime.Today;
                        creditRequirement.UserCreatedID = CoreContext.CurrentUser.UserID;
                        creditRequirement.LastUserModifiedID = CoreContext.CurrentUser.UserID;
                        creditRequirement.Observations = creditRequirement.Observations == null ? string.Empty : creditRequirement.Observations;

                        busines = new CreditRequirementsBusinessLogic();
                        var res = busines.Insert(creditRequirement);
                        creditRequirement.CreditRequirementID = res.ID;
                    }
                    else
                    {
                        if (creditRequirement.IsModified)
                        {
                            busines = new CreditRequirementsBusinessLogic();
                            var creditReq = busines.GetCreditRequirementsByCreditRequirementId(creditRequirement.CreditRequirementID);

                            if (creditReq.RequirementTypeID == creditRequirement.RequirementTypeID)
                            {
                                creditReq.Observations = creditRequirement.Observations;
                                creditReq.RequirementStatusID = creditRequirement.RequirementStatusID;
                                creditReq.LastUserModifiedID = CoreContext.CurrentUser.UserID;
                                creditReq.LastModifiedDate = DateTime.Today;

                                busines = new CreditRequirementsBusinessLogic();
                                busines.Update(creditReq);

                                creditRequirement.LastUserModifiedName = CoreContext.CurrentUser.Username;
                                creditRequirement.LastModifiedDate = DateTime.Today;
                            }
                        }
                    }
                }
                //return ActionResult("Index");
                return Json(new
                {
                    result = true,
                    creditRequirements = creditRequirements.Select(apt => new
                    {
                        requirementTypeID = apt.RequirementTypeID,
                        creditRequirementID = apt.CreditRequirementID,
                        lastUserModifiedName = apt.LastUserModifiedName,
                        lastModifiedDate = apt.LastModifiedDate.ToShortDateString()

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
