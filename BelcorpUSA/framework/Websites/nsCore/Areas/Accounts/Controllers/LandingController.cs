using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities.Cache;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Accounts.Models.Landing;
using nsCore.Controllers;
using NetSteps.Common.Extensions;

namespace nsCore.Areas.Accounts.Controllers
{
    public class LandingController : BaseController
    {
        [FunctionFilter("Accounts", "~/Sites")]
        public virtual ActionResult Index()
        {
            CoreContext.CurrentOrder = null;
            CoreContext.CurrentAccount = null;

            var model = new IndexModel
            {
                States = (from country in SmallCollectionCache.Instance.Countries
                          join state in SmallCollectionCache.Instance.StateProvinces on country.CountryID equals state.CountryID
                          where country.Active
                          select new SelectListItem
                          {
                              Text = country.CountryCode3 + " - " + state.StateAbbreviation,
                              Value = state.StateProvinceID.ToString()
                          })
                         .OrderBy(x => x.Text)
            };

            int iCountry = System.Configuration.ConfigurationManager.AppSettings["EnvironmentCountry"].ToInt();
            var countryDate = SmallCollectionCache.Instance.Countries.GetById(iCountry);
            ViewData["countryID"] = countryDate.CountryCode3;

            return View(model);
        }
    }
}
