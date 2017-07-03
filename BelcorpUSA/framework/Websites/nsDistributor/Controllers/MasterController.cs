using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace nsDistributor.Controllers
{
    public class MasterController : BaseController
    {
        public virtual ActionResult GenerateView(string path)
        {
            if (path!=null && !path.StartsWith("Content/"))
            {
                //if the path is just to the index/home page (path will be "/")
                if (string.IsNullOrEmpty(path))
                {
                    return new StaticController().Home();
                }

                Page currentPage = CurrentSite.GetPageByUrl("/" + path);

                if (currentPage != null && currentPage.PageID > 0)
                {
                    var site = CurrentSite.IsBase ? CurrentSite : SiteCache.GetSiteByID(CurrentSite.BaseSiteID.Value);
                    var nav = site.Navigations.FirstOrDefault(n => n.PageID == currentPage.PageID);
                    if (nav != default(Navigation))
                    {
                        ViewBag.SelectedNavigationId = nav.ParentID.HasValue ? nav.ParentID.Value : nav.NavigationID;
                    }
                    var layout = SmallCollectionCache.Instance.Layouts.GetById(currentPage.LayoutID);
                    return View(layout.ViewName, currentPage);
                }
            }
            // Check for custom 404 error page in the site
            // The recursion will check for a 404 and result in 404.cshtml
            // if it does not exist.

            // Check for custom 404 error page in the site
            // The recursion will check for a 404 and result in 404.cshtml
            // if it does not exist.
            if (path != "404")
            {
                return GenerateView("404");
            }

            return View("~/Views/Error/404.cshtml");
        }

        public virtual ActionResult RedirectToPws(string pwsurl)
        {
            return Redirect(pwsurl);
        }
    }
}
