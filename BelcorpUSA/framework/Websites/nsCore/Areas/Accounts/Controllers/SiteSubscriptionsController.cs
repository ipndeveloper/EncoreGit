using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Comparer;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Accounts.Controllers
{
    public class SiteSubscriptionsController : BaseAccountsController
    {
        [FunctionFilter("Accounts-Site Subscriptions", "~/Accounts/Overview")]
        public virtual ActionResult Index(string id, int baseSiteId)
        {
            try
            {
                if (ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.PayForSites))
                {
                    TempData["Error"] = Translation.GetTerm("MustEditSiteThroughSubscriptions", "You must edit this site's information through the subscriptions.");
                    return RedirectToAction("Index", "Overview", new { id = id });
                }

                if (String.IsNullOrWhiteSpace(id))
                {
                    return Redirect("~/Accounts");
                }
                this.AccountNum = id;

                Site distributorSite = Site.LoadByAccountID(CurrentAccount.AccountID).FirstOrDefault(s => s.BaseSiteID == baseSiteId);
                ViewData.Model = distributorSite ?? new Site();

                if (distributorSite == null)
                {
                    Address mainAddress = CurrentAccount.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Generated.ConstantsGenerated.AddressType.Main);

                    int marketID = mainAddress != null ? SmallCollectionCache.Instance.Countries.GetById(mainAddress.CountryID).MarketID : 0;

                    Site pwsBaseSite = Site.LoadBaseSiteForNewPWS(marketID);

                    ViewData["Domains"] = pwsBaseSite != null ? Site.LoadBaseSiteForNewPWS(marketID).GetDomains().ToArray() : new List<string>().ToArray();
                }
                else
                {
                    Site baseSite = Site.LoadSiteWithSiteURLs(distributorSite.IsBase ? distributorSite.SiteID : distributorSite.BaseSiteID.ToInt());

                    ViewData["Domains"] = baseSite != null ? baseSite.GetDomains().ToArray() : new List<string>().ToArray();
                }

                ViewData["BaseSiteId"] = baseSiteId;

                return View();
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        public virtual ActionResult CheckIfAvailableUrl(string url)
        {
            try
            {
                return Json(new { available = SiteUrl.IsAvailable(url) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Accounts-Site Subscriptions", "~/Accounts/Overview")]
        public virtual ActionResult SaveSiteSubscriptions(int baseSiteId, string siteName, string siteDescription, short? siteStatusId, int? siteDefaultLanguageId, List<SiteUrl> urls)
        {
            try
            {
                Site site = Site.LoadByAccountID(CoreContext.CurrentAccount.AccountID).FirstOrDefault();

                //Grab the current account's main or shipping address
                Account coreContextAccount = CoreContext.CurrentAccount;

                if (coreContextAccount.Addresses == null)
                    Account.LoadAddresses(coreContextAccount);

                Address tempAddress = coreContextAccount.Addresses.FirstOrDefault(ad => ad.IsDefault
                                                                                && (ad.AddressTypeID == (int)Constants.AddressType.Main
                                                                                    || ad.AddressTypeID == (int)Constants.AddressType.Shipping)
                                                                          );

                //Load the appropriate country to obtain a marketID. If there is no address, load the base site's MarketID (US)
                int marketID = 0;
                if (tempAddress != null)
                {
                    var country = tempAddress.GetCountryFromCache();
                    if (country != null)
                    {
                        marketID = country.MarketID;
                    }
                }
                if (marketID == 0)
                {
                    marketID = Site.Load(baseSiteId).MarketID;
                }

                if (site == null)
                {
                    site = new Site()
                    {
                        AccountID = CoreContext.CurrentAccount.AccountID,
                        AccountNumber = CoreContext.CurrentAccount.AccountNumber,
                        CreatedByUserID = NetSteps.Data.Entities.ApplicationContext.Instance.CurrentUserID,
                        AutoshipOrderID = null,
                        BaseSiteID = baseSiteId,
                        MarketID = marketID,
                        IsBase = false,
                        DateCreated = DateTime.Now,
                        DateSignedUp = DateTime.Now,
                        SiteTypeID = (int)Constants.SiteType.Replicated
                    };
                }

                site.Name = siteName;
                site.Description = siteDescription;
                site.DefaultLanguageID = siteDefaultLanguageId.ToInt();
                site.SiteStatusID = siteStatusId.Value;

                if (urls != null && urls.Count > 0)
                {
                    urls[0].IsPrimaryUrl = true;

                    site.SiteUrls.SyncTo(urls, new LambdaComparer<SiteUrl>((su1, su2) => su1.SiteUrlID == su2.SiteUrlID), (su1, su2) => su2.Url = su1.Url);
                }
                else
                    site.SiteUrls.RemoveAllAndMarkAsDeleted();

                site.Save();

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CoreContext.CurrentAccount != null) ? CoreContext.CurrentAccount.AccountID.ToIntNullable() : null);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
