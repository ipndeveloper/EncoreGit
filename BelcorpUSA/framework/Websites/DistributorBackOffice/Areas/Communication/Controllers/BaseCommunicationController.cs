using System.Linq;
using System.Web.Mvc;
using DistributorBackOffice.Controllers;
using NetSteps.Common.Configuration;
using NetSteps.Data.Entities;
using DistributorBackOffice.Infrastructure;

namespace DistributorBackOffice.Areas.Communication.Controllers
{
	public abstract class BaseCommunicationController : BaseController
	{
		protected override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			if (!(filterContext.Result is RedirectResult || filterContext.Result is RedirectToRouteResult))
			{
				ViewBag.External = ConfigurationManager.GetAppSetting<bool>(ConfigurationManager.VariableKey.AllowExternalEmail, false);
				if (ViewBag.External)
				{
					ViewBag.Groups = DistributionListCacheHelper.GetDistributionListByAccountID(CurrentAccount.AccountID);
				}
				ViewBag.Campaigns = Campaign.LoadAll();
			}
		}
	}
}
