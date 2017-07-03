using System;
using System.Linq;
using System.Web.Mvc;

using NetSteps.AccountLocatorService;
using NetSteps.AccountLocatorService.Common;
using NetSteps.Common.EldResolver;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

using nsDistributor.Areas.Enroll.Models.Sponsor;
using nsDistributor.Models.Shared;
using NetSteps.Data.Entities.Business.Logic;

namespace nsDistributor.Controllers
{
	public class SponsorController : BaseController
	{
		public virtual ActionResult Browse()
		{
			if (CurrentSite.IsBase)
			{
				var model = new BrowseModel();
				return View(model);
			}
			else
			{
				return RedirectToAction("Home", "Static");
			}
		}

		[HttpPost]
		public virtual ActionResult Browse(AccountLocatorModel model, int? PageIndex1)
		{

            // string accountNumber = AccountSponsorBusinessLogic.Instance.SeleccionAutomaticaSponsor(model.PostalCode);
			if (!ModelState.IsValid)
			{
				return JsonError(ModelState.Values.First(x => x.Errors.Any()).Errors.First().ErrorMessage);
			}

			try
			{
				var searchParameters = new AccountLocatorServiceSearchParameters
				{
					AccountTypeIDs = new[] { (short)Constants.AccountType.Distributor },
					PageIndex = model.PageIndex
				};
				model.ApplyTo(searchParameters);

				var accountLocatorService = Create.New<IAccountLocatorService>();
				var searchData = accountLocatorService.Search(searchParameters);

				model.LoadResources(searchData, x => Url.Action("SelectConsultant", "Locate") + "?" + "consultantId=" + x.AccountId + "&pwsUrl=" + Url.Encode(x.PwsUrl.EldEncode()));

				this.SetAccountLocatorLocationSearchUsedFlag(model);
			}
			catch (Exception ex)
			{
				return JsonError(ex.Log().PublicMessage);
			}

			return Json(new
			{
				results = model.SearchResults,
				showMoreButton = model.ShowMoreButton
			});
		}
	}
}