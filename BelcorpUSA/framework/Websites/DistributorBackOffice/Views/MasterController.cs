using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace DistributorBackOffice.Controllers
{
    public class MasterController : BaseController
    {
        public virtual ActionResult GenerateView(string path)
        {
            if (!path.StartsWith("Content/"))
            {
                Page currentPage = CurrentSite.GetPageByUrl("/" + path);

                if (currentPage != null && currentPage.PageID > 0)
                {
                    var nav = CurrentSite.Navigations.FirstOrDefault(n => n.PageID == currentPage.PageID);
                    if (nav != default(Navigation)) 
					{
                        ViewData["SelectedNavigationId"] = nav.ParentID.HasValue ? nav.ParentID.Value : nav.NavigationID;
					}
                    var layout = SmallCollectionCache.Instance.Layouts.GetById(currentPage.LayoutID);

                    // if the page is an external type and the external url is specified, run the url replace with token method - JWL
                    if (currentPage.PageTypeID == (short)NetSteps.Data.Entities.Constants.PageType.External && 
                        !string.IsNullOrEmpty(currentPage.ExternalUrl))
                    {
                        Account currentAccount = (Account)Session["CurrentAccount"];

                        currentPage.UpdateExternalUrlWithToken(currentAccount); 
                    }

                    return View(layout.ViewName, currentPage);
                }
                // If the page could not be found, do a final check for a custom 404 page
                if (path != "404")
                    return GenerateView("404");
            }
            return new HttpNotFoundResult();
        }
    }
}
