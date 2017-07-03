using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

using DistributorBackOffice.Areas.Account.Models.Edit;

using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business;

namespace DistributorBackOffice.Areas.Account.Controllers
{
	public class EditAdditionalController : BaseAccountsController
	{
		[HttpGet]
		[FunctionFilter("Accounts", "~/", Constants.SiteType.BackOffice)]
		public virtual ActionResult Index()
		{
			Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
			var viewModel = new AccountModel();
			viewModel.Account = CurrentAccount;
            viewModel = GetCreditRequirementsByAccount(viewModel);
			return View(viewModel);
		}

        private AccountModel GetCreditRequirementsByAccount(AccountModel viewModel)
        {
           AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();        
           var result = busines.GetCreditRequirementsByAccount(CurrentAccount.AccountID);
           viewModel.listAdditonal = result;
            return viewModel;
        }

     
        [OutputCache(CacheProfile = "DontCache")]
        [FunctionFilter("Accounts-Create and Edit Account", "~/Account", Constants.SiteType.BackOffice)]
        public virtual ActionResult AdditionalInformation(List<AccountPropertiesParameters> AccountProperties, 
            int ReferenceID, string ReferenceName, string PhoneNumberMain, int RelationShip
            ,List<AccountSocialNetworksParameters> AccountSocialNetworks
            )
        {
            AccountPropertiesBusinessLogic busines = new AccountPropertiesBusinessLogic();
            AccountSocialNetworksBusinessLogic businesSocial = new AccountSocialNetworksBusinessLogic();
            try
            {
                AccountReferencesBusinessLogic referenceBusines = new AccountReferencesBusinessLogic();
                AccountPropertiesParameters referenceDat = new AccountPropertiesParameters();
                if (ReferenceID == 0)
                {
                   
                    referenceDat.AccountID = CurrentAccount.AccountID;
                    referenceDat.ReferenceName = ReferenceName;
                    referenceDat.PhoneNumberMain = Int64.Parse(PhoneNumberMain);
                    referenceDat.RelationShip = RelationShip;
                    var  res02=  referenceBusines.Insert(referenceDat);
                    ReferenceID = (int)((dynamic)((System.Dynamic.ExpandoObject)(res02))).ID;
                }
                else
                {
                    referenceDat.AccountReferencID = ReferenceID;
                    referenceDat.AccountID = CurrentAccount.AccountID;
                    referenceDat.ReferenceName = ReferenceName;
                    referenceDat.PhoneNumberMain = Int64.Parse(PhoneNumberMain); 
                    referenceDat.RelationShip = RelationShip;
                    referenceBusines.Update(referenceDat);
                }


                foreach (AccountPropertiesParameters creditRequirement in AccountProperties)
                {
                    creditRequirement.AccountID = CurrentAccount.AccountID;
                    creditRequirement.Active = true;
                    if (creditRequirement.AccountPropertyID == 0)
                    {       
                        busines = new AccountPropertiesBusinessLogic();
                        var res = busines.Insert(creditRequirement);
                        creditRequirement.AccountPropertyID = res.ID;
                    }
                    else
                    {
                        busines = new AccountPropertiesBusinessLogic();
                        busines.Update(creditRequirement);               
                    }
                }

                foreach (AccountSocialNetworksParameters accountSocialNetwork in AccountSocialNetworks)
                {
                    accountSocialNetwork.AccountID = CurrentAccount.AccountID;

                    if (accountSocialNetwork.AccountSocialNetworkID == 0)
                    {
                        businesSocial = new AccountSocialNetworksBusinessLogic();
                        var res = businesSocial.Insert(accountSocialNetwork);
                        accountSocialNetwork.AccountSocialNetworkID = res.ID;
                    }
                    else
                    {
                        businesSocial = new AccountSocialNetworksBusinessLogic();
                        businesSocial.Update(accountSocialNetwork);               
                    }
                }

                return Json(new
                {
                    result = true,
                    referenceID=ReferenceID,
                    creditRequirements = AccountProperties.Select(apt => new
                    {
                        accountPropertyID = apt.AccountPropertyID
                        
                    }),
                    accountSocialNetworks = AccountSocialNetworks.Select(apt => new
                    {
                        accountSocialNetworkID = apt.AccountSocialNetworkID

                    }),
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
