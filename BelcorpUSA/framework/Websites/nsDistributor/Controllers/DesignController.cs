using System;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;


namespace nsDistributor.Controllers
{
    public class DesignController : BaseController
    {
        public DesignController()
        {
            ViewBag.DesignCenter = CurrentSite.GetHtmlSectionByName("DesignCenter");
        }

        // GET: /Design/
        public virtual ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Recent()
        {
            var images = CoreContext.CurrentAccount.FileResources.Where(x => x.FileResourceTypeID ==
                  (int)Constants.FileResourceType.Image).OrderByDescending(x => x.DateCreatedUTC).Take(6);
            var imagePaths = images.Select(resource => string.Format("{0}?Stamp={1}", resource.FileResourcePath.RemoveFileUploadToken(), DateTime.Now)).ToList();
            ViewBag.ImagePaths = imagePaths;
            return PartialView("_RecentImages");
        }

        public virtual ActionResult Gallery()
        {
            return PartialView("UserGallery/Gallery", CoreContext.CurrentAccount.FileResources.ToList());
        }

        public virtual bool IsUserLoggedIn()
        {

            return BaseController.IsLoggedIn;
        }
    }
}
