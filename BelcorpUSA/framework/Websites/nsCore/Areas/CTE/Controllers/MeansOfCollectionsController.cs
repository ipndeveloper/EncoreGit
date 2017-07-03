using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NetSteps.Web.Mvc.Business.Controllers;

namespace nsCore.Areas.CTE.Controllers
{
    public class MeansOfCollectionsController : BaseController
    {
        //
        // GET: /CTE/MeansOfCollections/

        public ActionResult Index()
        {
            return View();
        }

    }
}
