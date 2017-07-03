using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Common.Base;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Business.Controllers;
using NetSteps.Common.Globalization;
using NetSteps.Web.Mvc.Helpers;
using NetSteps.Data.Entities.Cache;

namespace nsCore.Areas.Commissions.Controllers
{
    public class SponsorController : Controller
    {
        //
        // GET: /Commissions/Sponsor/

        public ActionResult Index()
        {

           

            int EnvironmentCountry = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"]);
            ViewData["Sponsorsxxx"] = SponsorshipDataAcces.Search(EnvironmentCountry);
            Country country = SmallCollectionCache.Instance.Countries.First(c => c.CountryID == EnvironmentCountry); 

            //int MarketID = CoreContext.CurrentLanguageID;

            List<SponsorDataAccountStatus> SponsorList = SponsorshipDataAcces.SearchAccounts(country.MarketID);

            if (SponsorList == null)
                SponsorList = new List<SponsorDataAccountStatus>();

            ViewData["SponsorStatusAccounts"] = SponsorList;

            ViewData["spGetRulePerStatus"] = SponsorshipDataAcces.spGetRulePerStatus();
            ViewData["spGetRulesPerDocuments"] = SponsorshipDataAcces.spGetRulesPerDocuments();

            ViewData["spGetTitlesPaids"] = SponsorshipDataAcces.spGetTitlesPaids();
            ViewData["spGetTitlesRecognizeds"] = SponsorshipDataAcces.spGetTitlesRecognizeds();
            return View();
                      
        }
          //Insertar AccountStatuses

        [HttpPost]
        public virtual ActionResult Save(List<SponsorDataAccountStatus> AccountStatus,
            List<SponsorDataTitleType> Titles,//List<SponsorDataTitleType> TitlesRecognized,
            List<SponsorshipSearchData> RulesPerDocuments)
        {
            try
    		{
                if (AccountStatus != null)
                {
                    if (AccountStatus.Count > 0)
                    {
                        SponsorshipDataAcces.spDeleteRulePerStatus();
                        foreach (var accStatus in AccountStatus)
                        {
                            var obj = new SponsorDataAccountStatus()
                            {
                                AccountStatusID = Convert.ToInt32(accStatus.AccountStatusID)
                            };
                            SponsorshipDataAcces.Insert(obj);
                        }
                    }
                }
                else {
                    SponsorshipDataAcces.spDeleteRulePerStatus();
                }
                if (RulesPerDocuments != null)
                {
                    SponsorshipDataAcces.spDeleteRulesPerDocuments();
                    foreach (var RulesDoc in RulesPerDocuments)
                    {
                        var RulesDocuments = new SponsorshipSearchData()
                        {
                            RequirementTypeID = RulesDoc.RequirementTypeID,
                            LogicalOperator = "AND",
                            Order = 0
                        };
                        SponsorshipDataAcces.InsertDoc(RulesDocuments);
                    }
                }
                else {
                    SponsorshipDataAcces.spDeleteRulesPerDocuments();
                }
                if (Titles != null)
                {
                    SponsorshipDataAcces.spDeleteRulePerTitle();
                    foreach (var title in Titles)
                    {
                        var TitlePaidRec = new SponsorDataTitleType()
                        {
                            TitleID = Convert.ToInt32(title.TitleID),
                            TitleTypeID = Convert.ToInt32(title.TitleTypeID)
                        };
                        SponsorshipDataAcces.InsertTitles(TitlePaidRec);
                    }
                }
                else {
                    SponsorshipDataAcces.spDeleteRulePerTitle();
                }
                //if (TitlesRecognized != null)
                //{
                //    SponsorshipDataAcces.spDeleteRulePerTitle();
                //    foreach (var TitlesRecognizeds in TitlesRecognized)
                //    {
                //        var TitleRecognized = new SponsorDataTitleType()
                //        {
                //            TitleID = Convert.ToInt32(TitlesRecognizeds.TitleID),
                //            TitleTypeID = Convert.ToInt32(2)
                //        };
                //        SponsorshipDataAcces.InsertTitles(TitleRecognized);
                //    }
                //}
                //else {
                //    SponsorshipDataAcces.spDeleteRulePerTitle();
                //}
            

                return Json(new { result = true });
                }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

    }
}

 
