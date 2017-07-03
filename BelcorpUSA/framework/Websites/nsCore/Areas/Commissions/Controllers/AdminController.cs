using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Commissions.Common.Models;
using NetSteps.Encore.Core.IoC;
using NetSteps.Commissions.Common;

namespace nsCore.Areas.Commissions.Controllers
{
    public class AdminController : BaseCommissionController
    {
        private readonly ICommissionsService _commissionsService = Create.New<ICommissionsService>();

        [FunctionFilter("Commissions", "~/Accounts")]
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual JsonResult OpenPeriods()
        {

            return this.Json(new { periods = _commissionsService.GetOpenPeriods(DisbursementFrequencyKind.Monthly) });//SmallCollectionCache.Instance.Periods.GetOpenPeriods()};
        }
    }
}
