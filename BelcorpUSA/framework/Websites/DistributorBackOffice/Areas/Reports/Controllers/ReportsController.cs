using System.Web.Mvc;
using DistributorBackOffice.Controllers;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Services;

namespace DistributorBackOffice.Areas.Reports.Controllers
{
    public class ReportsController : BaseController
    {
        //
        // GET: /Reports/Reports/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Category view.
        /// </summary>
        /// <param name="id"><see cref="int">Category id.</see></param>
        /// <returns><see cref="ActionResult">Action Result</see></returns>
        public ActionResult Category(int? id)
        {
            ReportCategory category = ReportService.GetCategory(id);
            return View(category);
        }


        #region Old code for now
        // GET: /Reports/GetReports/

        //[OutputCache(CacheProfile = "PagedGridData")]
        //public virtual JsonResult GetReports(int page, int pageSize, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
        //{
        //    try
        //    {
        //        PaginatedList<ReportSearchData> reports = Report.SearchReports(new ReportSearchParameters()
        //        {
        //            PageIndex = page,
        //            PageSize = pageSize,
        //            OrderBy = orderBy,
        //            OrderByDirection = orderByDirection,
        //        });

        //        var builder = new StringBuilder();

        //        int count = 0;
        //        foreach (ReportSearchData reportSearchData in reports)
        //        {
        //            builder
        //                .Append(string.Format("<tr reportName=\"{0}\">", reportSearchData.ReportName))
        //                //.AppendLinkCell("~/Reports/Reports/ReportsDisplay", reportSearchData.ReportName.ToString())
        //                .AppendLinkCell("javascript:void(0);", reportSearchData.ReportName.ToString(), linkCssClass: "btnDisplayReportAction")
        //                .Append("</tr>");
        //            ++count;
        //        }

        //        return Json(new { totalPages = 1/*reports.TotalPages*/, page = builder.ToString(), JsonRequestBehavior = JsonRequestBehavior.AllowGet });
        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        return Json(new { result = false, message = exception.PublicMessage });
        //    }
        //}

        //public ActionResult DisplayReport()
        //{
        //    //This code is for iFrame code.
        //    // Session["iFrameURL"] = string.Format("DisplayReport.aspx?ReportName={0}", "SalesReport");
        //    return View();
        //}

        //public ActionResult ReportsDisplay()
        //{
        //    Session["iFrameURL"] = string.Format("DisplayReport.aspx?ReportName=\"SalesReport\"");
        //    return View();
        //}
#endregion
    }
}