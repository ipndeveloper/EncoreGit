using System.Web.Mvc;
using NetSteps.Data.Entities.Business;
using NetSteps.Web.Mvc.ActionResults;

namespace NetSteps.Web.Mvc.Controls.Controllers
{
    public class SelfTestController : Controller
    {
        public ActionResult Index()
        {
            ServerSystemInformation serverInfo = new ServerSystemInformation();
            if(serverInfo.CpuUsage > 97)
                return Json(new { server_status = "unavailable"}, JsonRequestBehavior.AllowGet);
            if (serverInfo.CpuUsage > 70)
                return Json(new { server_status = "busy" }, JsonRequestBehavior.AllowGet);
            return Json(new { server_status = "available" }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Full()
        {
            if (Request.UserHostAddress != "74.95.55.30")
                return new JsonBasicResult();

            ServerSystemInformation serverInfo = new ServerSystemInformation();

            //Status Code
            var json = new
            {
                ip_addresses = serverInfo.IpAddresses,
                code_version = serverInfo.CodeVersion,
                db_response_time_ms = serverInfo.DatabaseResponseTime,
                system_info = new
                {
                    cpu_load = serverInfo.CpuUsage,
                    memory_usage = serverInfo.MemoryUsage,
                    available_phyiscal_memory = serverInfo.AvailablePhysicalMemory,
                    total_physical_memory = serverInfo.TotalPhysicalMemory,
                    operating_system = serverInfo.OperatingSystem,
                    drive_space = serverInfo.DriveSpace
                }
            };

            return Json(json, JsonRequestBehavior.AllowGet);

        }

    }
}
