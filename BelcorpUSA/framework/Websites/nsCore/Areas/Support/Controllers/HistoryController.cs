using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nsCore.Areas.Support.Controllers
{
    public class HistoryController : Controller
    {
        //
        // GET: /Support/Ticket/History/

        public ActionResult Index()
        {
            return View();
        }

    }
}
