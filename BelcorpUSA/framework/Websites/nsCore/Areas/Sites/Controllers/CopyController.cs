using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Sites.Controllers
{
	public class CopyController : BaseSitesController
	{
		[FunctionFilter("Sites-Copy", "~/Accounts")]
		public ActionResult Index()
		{
			return View();
		}

		/// <summary>
		/// Save a site
		/// </summary>
		/// <returns></returns>
		public virtual ActionResult Save(int? siteId, string siteName, string description, int statusId, string subdomain, int marketId, int defaultLanguageId)
		{
			if (!CoreContext.CurrentUser.HasFunction("Sites-Copy"))
				return Json(new { result = false, message = Translation.GetTerm("UserDoesNotHavePermissionsToCopyASite", "The user does not have permissions to copy a site.") });

			try
			{
				if (siteId.HasValue && siteId > 0)
				{
					string url = "http://" + subdomain;
					if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
					{
						return Json(new { result = false, message = "URL is invalid or not wellformed." });
					}

					Site site = Site.CopyBaseSite(siteId.ToInt(), marketId, siteName, description, defaultLanguageId, new List<string>() { "http://" + subdomain });
				}

				return Json(new { result = true });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}
	}
}
