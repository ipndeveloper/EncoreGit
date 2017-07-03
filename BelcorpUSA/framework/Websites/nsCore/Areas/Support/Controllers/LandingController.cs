using System.ComponentModel;

using System.Reflection;
using NetSteps.Data.Entities.Extensions;

using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System;
using System.Web.Mvc;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Repositories;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;
using System.Collections.Generic;
using NetSteps.Data.Entities;
using System.Globalization;
using NetSteps.Common.Globalization;
using System.Data;
using System.Text;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Cache;

using NetSteps.Web.Mvc.Extensions;
namespace nsCore.Areas.Support.Controllers
{
    public class LandingController : BaseController
    {
        protected virtual DataTable CurrentReportView
        {
            get { return Session["CurrentReportViewSupport"] as DataTable; }
            set { Session["CurrentReportViewSupport"] = value; }
        }

        /// <summary>
        ///  Excel Export
        /// </summary>
        /// <returns>Excel File</returns>
        public virtual ActionResult Export(int? Id)
        {
            try
            {
                if (Session["ResultGrid"] != null)
                {

                    PaginatedList<SupportTicketSearchData> lstResultado = (PaginatedList<SupportTicketSearchData>)Session["ResultGrid"];

                    string fileNameSave = string.Format("{0}.xlsx", Translation.GetTerm("Support", "Support"));
                    DataTable table = new DataTable();
                    table.Columns.Add(Translation.GetTerm("SupportTicketNumber", "Support Ticket Numberr"));
                    table.Columns.Add(Translation.GetTerm("AssignedUsername", "Assigned Username"));
                    table.Columns.Add(Translation.GetTerm("SupportTicketPriority", "Priority"));
                    table.Columns.Add(Translation.GetTerm("Title", "Title"));
                    table.Columns.Add(Translation.GetTerm("FirstName", "First Name"));
                    table.Columns.Add(Translation.GetTerm("LastName", "Last Name"));
                    table.Columns.Add(Translation.GetTerm("SupportTicketStatus", "Status"));

                    table.Columns.Add(Translation.GetTerm("Created", "Created"));

                    table.Columns.Add(Translation.GetTerm("Changed", "Changed"));
                    table.Columns.Add(Translation.GetTerm("Category", "Category"));
                    table.Columns.Add(Translation.GetTerm("Site", "Site"));




                    DataRow drS;
                    foreach (var val in lstResultado)
                    {
                        drS = table.NewRow();



                        drS[Translation.GetTerm("SupportTicketNumber", "Support Ticket Number")] = val.SupportTicketNumber;
                        drS[Translation.GetTerm("AssignedUsername", "Assigned Username")] = val.AssignedUsername;
                        drS[Translation.GetTerm("SupportTicketPriority", "Priority")] = SmallCollectionCache.Instance.SupportTicketPriorities.GetById(val.SupportTicketPriorityID).GetTerm();
                        drS[Translation.GetTerm("Title", "Title")] = val.Title;
                        drS[Translation.GetTerm("FirstName", "First Name")] = val.FirstName;
                        drS[Translation.GetTerm("LastName", "Last Name")] = val.LastName;
                        drS[Translation.GetTerm("SupportTicketStatus", "Status")] = SmallCollectionCache.Instance.SupportTicketStatuses.GetById(val.SupportTicketStatusID).GetTerm();
                        drS[Translation.GetTerm("Category", "Category")] = val.SupportLevelMotive;
                        drS[Translation.GetTerm("Created", "Created")] = val.DateCreated;
                        drS[Translation.GetTerm("Changed", "Changed")] = val.DateLastModified;
                        drS[Translation.GetTerm("Site", "Site")] = val.IsSiteDWS == 1 ? "DWS" : "GMP";
                        table.Rows.Add(drS);
                    }

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<table border='" + "2px" + "'b>");
                    //write column headings
                    sb.Append("<tr>");
                    foreach (System.Data.DataColumn dc in table.Columns)
                    {
                        sb.Append("<td><b><font face=Arial size=2>" + dc.ColumnName + "</font></b></td>");
                    }
                    sb.Append("</tr>");

                    //write table data
                    foreach (System.Data.DataRow dr in table.Rows)
                    {
                        sb.Append("<tr>");
                        foreach (System.Data.DataColumn dc in table.Columns)
                        {
                            sb.Append("<td><font face=Arial size=" + "14px" + ">" + dr[dc].ToString() + "</font></td>");
                        }
                        sb.Append("</tr>");
                    }
                    sb.Append("</table>");


                    this.Response.AddHeader("content-disposition", "attachment; filename=" + fileNameSave + ".xls");
                    this.Response.ContentType = "application/vnd.ms-excel";
                    return this.Content(sb.ToString());

                }
                return null;

            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);
            }

        }

        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Index(DateTime? startDate, DateTime? endDate, string supportTicketNumber, int? assignedUserID, int? accountID)
        {
            bool allowManageMultiUsers = CoreContext.CurrentUser.HasFunction("Support-Manage All Tickets");

            if (!accountID.HasValue)
            {
                ViewData["AllowedToMangeOtherTickets"] = allowManageMultiUsers;
            }

            try
            {
                SupportTicketSearchParameters searchParams = new NetSteps.Data.Entities.Business.SupportTicketSearchParameters()
                {
                    AccountID = accountID,
                    AssignedUserID = assignedUserID,
                    SupportTicketNumber = supportTicketNumber,
                    StartDate = startDate,
                    EndDate = endDate,
                    PageIndex = 0,
                    PageSize = 20,
                    OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending
                };
                Dictionary<string, Dictionary<string, string>> dcAdicionales = null;
                var supportTickets = SupportTicket.Search(searchParams);
                Dictionary<string, string> dcSupporMotiveLevelJerarquia = SupportMotives.SearchAllSupportMotiveLevel(out dcAdicionales);
                ViewData["dcSupporMotiveLevelJerarquia"] = dcSupporMotiveLevelJerarquia;//.ToAJAXSearchResults();

                ViewData["dcAdicionales"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(dcAdicionales);
                ViewData["TotalCount"] = supportTickets.TotalCount;

                return View(searchParams);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }
    }
}

