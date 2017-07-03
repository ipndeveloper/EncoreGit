using System;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Sites.Models;

namespace nsCore.Areas.Sites.Controllers
{
	public class EditController : BaseSitesController
	{

		/// <summary>
		/// Checks if the url is available
		/// </summary>
		/// <param name="url">The url to check for</param>
		/// <returns></returns>
		public virtual ActionResult CheckIfUrlAvailable(string url)
		{
			try
			{
				var available = Content(SiteUrl.IsAvailable(url).ToString());
				return bool.Parse(available.Content)
					? Json(new { result = true, available = true, message = "URL available" })
					: Json(new { result = false, message = "URL is not available" });
			}
			catch (Exception e)
			{
				return
					Json(
						new { result = false, available = false, message = String.Format("An error while checking the url: {0}", e.Message) });
			}
		}
		private void LoadDomainObjects(int? siteID, out Site site, out Site baseSite)
		{
			site = siteID.HasValue
					   ? Site.LoadFullWithoutContent(siteID.Value)
					   : (CurrentSite == null ? null : Site.LoadFullWithoutContent(CurrentSite.SiteID));

			if (site == null)
			{
				baseSite = Site.LoadBaseSiteForNewPWS(CoreContext.CurrentMarketId);
				if (site == null)
				{
					site = new Site()
					{
						IsBase = false,
						SiteTypeID = (int)Constants.SiteType.Replicated,
						DateSignedUp = DateTime.Now,
						BaseSiteID = baseSite.SiteID
					};
				}
			}
			else
			{
				if (site.IsBase)
				{
					baseSite = site;
				}
				else if (site.BaseSiteID.HasValue)
				{
					baseSite = Site.LoadFullWithoutContent(site.BaseSiteID.Value);
				}
				else
				{
					baseSite = default(Site);
				}
			}

			CurrentSite = site;
		}

		/// <summary>
		/// Show a site to be able to edit it
		/// </summary>
		/// <param name="id">The id of the site to edit</param>
		/// <returns></returns>
		[FunctionFilter("Sites-Create and Edit Base Site", "~/Sites/Overview")]
		[HttpGet]
		public virtual ActionResult Index(int? id)
		{
			try
			{
				Site site, baseSite;
				LoadDomainObjects(id, out site, out baseSite);
				EditModel model = new EditModel(site, baseSite);
				return View(model);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		/// <summary>
		/// Save a site
		/// </summary>
		[FunctionFilter("Sites-Create and Edit Base Site", "~/Sites/Overview")]
		[HttpPost]
		[ActionName("Index")]
		public virtual ActionResult SaveSite(EditModel model)
		{
			try
			{
				Site site, baseSite;
				LoadDomainObjects(model.SiteID, out site, out baseSite);
				model.ImportReadOnly(site, baseSite);
				model.Validate(ModelState);

				if (ModelState.IsValid)
				{
					site.StartEntityTracking();
					// The Site object points to languages both by its default language in the Language field
					// and in the SiteLanguages relationship in the Languages collection.
					// If we already have the language loaded in the default laguage,
					// use it to avoid loading the same language twice.
					site.Languages.SyncTo(model.SiteLanguages,
						x => x.LanguageID,
						id => site.Language != null && site.Language.LanguageID == id ? site.Language : Language.Load(id));

					model.Export(site);
					site.Save();
					model.SiteID = site.SiteID;

					(CoreContext.CurrentUser as CorporateUser).GrantSiteAccess(site.SiteID);
					if (CurrentSite == null || CurrentSite.SiteID == site.SiteID)
					{
						CurrentSite = site;
					}

					model.SuccessMessage = "Site saved successfully!";
				}

				return View("Index", model);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[HttpPost]
		public virtual ActionResult AddAnotherURL(EditModel model)
		{
			try
			{
				Site site, baseSite;
				LoadDomainObjects(model.SiteID, out site, out baseSite);
				model.ImportReadOnly(site, baseSite);

				model.SiteUrls.Add(new EditModel.EditSiteUrlModel(new SiteUrl() { Url = string.Empty }, baseSite));
				return PartialView("EditSiteURLs", model.SiteUrls);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		[HttpPost]
		public virtual ActionResult RemoveURL(EditModel model, int? indexForRemoval)
		{
			try
			{
				Site site, baseSite;
				LoadDomainObjects(model.SiteID, out site, out baseSite);
				model.ImportReadOnly(site, baseSite);

				if (indexForRemoval.HasValue && model.SiteUrls.Count > indexForRemoval)
				{
					model.SiteUrls.RemoveAt(indexForRemoval.Value);
				}

				return PartialView("EditSiteURLs", model.SiteUrls);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}
	}
}
