using System;
using System.Linq;
using System.Web.Mvc;

using NetSteps.AccountLocatorService;
using NetSteps.AccountLocatorService.Common;
using NetSteps.Common.EldResolver;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

using nsDistributor.Models.Locate;
using nsDistributor.Models.Shared;

namespace nsDistributor.Controllers
{
	public class LocateController : BaseController
	{
		public virtual ActionResult Index()
		{
			var model = new IndexModel();

			return View(model);
		}

		[HttpPost]
		public virtual ActionResult Index(AccountLocatorModel model, int? pageIndex1)
		{
			if (!ModelState.IsValid)
			{
				return JsonError(ModelState.Values.First(x => x.Errors.Any()).Errors.First().ErrorMessage);
			}

			try
			{
				var searchParameters = new AccountLocatorServiceSearchParameters
				{
					AccountTypeIDs = new[] { (short)Constants.AccountType.Distributor },
					RequirePwsUrl = true,
					PageIndex = model.PageIndex
				};
				model.ApplyTo(searchParameters);

				var accountLocatorService = Create.New<IAccountLocatorService>();
				var searchData = accountLocatorService.Search(searchParameters);

				model.LoadResources(
					searchData,
					GetSelectUrlExpression()
				);

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

		public virtual ActionResult SelectConsultant(int consultantId, string pwsUrl)
		{
			string pwsUrlWithAccountLocatorFlag = pwsUrl;


			if (AccountLocatorLocationSearchUsed)
			{
				pwsUrlWithAccountLocatorFlag += pwsUrlWithAccountLocatorFlag.Contains("?")
				                                	? "&comingFromAccountLocator=true"
				                                	: "?comingFromAccountLocator=true";
			}

			return this.Redirect(pwsUrlWithAccountLocatorFlag);
		}

		protected virtual Func<IAccountLocatorServiceResult, string> GetSelectUrlExpression()
		{
			return x => Url.Action("SelectConsultant", "Locate") + 
				"?" + "consultantId=" + x.AccountId + 
				"&pwsUrl=" + Url.Encode(x.PwsUrl.EldEncode());
		}
	}
}
