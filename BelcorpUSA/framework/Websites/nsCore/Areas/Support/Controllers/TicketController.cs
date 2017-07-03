using System;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;

using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
using nsCore.Areas.Support.Infrastructure;
using nsCore.Areas.Support.Models.Ticket;
using nsCore.Controllers;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using NetSteps.Web.Mvc.Controls.Models;
using NetSteps.Web.Mvc.Controls;
using NetSteps.Data.Entities.Business.Logic;
using System.Collections.Generic;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.EntityModels;
using NetSteps.Data.Entities.Extensions;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using System.Globalization;

namespace nsCore.Areas.Support.Controllers
{
    public class TicketController : BaseController
    {
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Edit(string id)
        {
            SupportTicket ticket = new SupportTicket();
            ViewData["IsSiteDWS"] = 0;
            if (!string.IsNullOrEmpty(id))
            {
                SupportTicketsBE objSupportTicketsBE = new SupportTicketsBE();
                ticket = SupportTicket.LoadBySupportTicketNumberFull(id);

                objSupportTicketsBE = SupportTicket.ObtenerSupportTicketsBE(ticket.SupportTicketID);

                ViewData["SupportTicketID"] = objSupportTicketsBE.SupportTicketID;
                ViewData["SupportMotiveID"] = objSupportTicketsBE.SupportMotiveID;
                ViewData["SupportLevelID"] = objSupportTicketsBE.SupportLevelID;
                List<SupportTicketsFilesBE> lstSupportTicketsFilesBE = SupportTicket.ObtenerSupportTicketsFilesporSupporMotive(ticket.SupportTicketID) ?? new List<SupportTicketsFilesBE>();
                ViewData["lstSupportTicketsFilesBE"] = lstSupportTicketsFilesBE;
                ViewData["StrlstSupportTicketsFilesBE"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lstSupportTicketsFilesBE);

                List<SupportTicketGestionBE> lstSupportTicketGestionBE = SupportTicket.ListarSupportTicketGestionBE(objSupportTicketsBE.SupportTicketID);

                ViewData["lstSupportTicketGestionBE"] = lstSupportTicketGestionBE;
                ViewData["IsSiteDWS"] = (Convert.ToBoolean(objSupportTicketsBE.IsSiteDWS) ? 1 : 0);

                // si el regsitro no tienes usuario asignado  al registro se bloquea para el usuario que ingreso
                if (objSupportTicketsBE.BlockUserID == 0)
                {
                    byte resultado = SupportTicket.BloquearSUpportTickets(objSupportTicketsBE.SupportTicketID, CoreContext.CurrentUser.UserID);
                }
                ViewData["Edicion"] = ((objSupportTicketsBE.BlockUserID != 0 && objSupportTicketsBE.BlockUserID != CoreContext.CurrentUser.UserID) || (objSupportTicketsBE.NotEdit)) ? 0 : 1;

                ViewData["BlockUserName"] = objSupportTicketsBE.BlockUserName;

            }
            else
            {
                ViewData["Edicion"] = 1;
                ViewData["SupportTicketID"] = 0;
                ViewData["SupportMotiveID"] = 0;
                ViewData["SupportLevelID"] = 0;

                ViewData["lstSupportTicketsFilesBE"] = new List<SupportTicketsFilesBE>();
                ViewData["StrlstSupportTicketsFilesBE"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(new List<SupportTicketsFilesBE>());

            }

            CoreContext.CurrentSupportTicket = ticket;
            List<System.Tuple<int, string, int, int>> lstSupportLevelParent = SupportTicket.GetLevelSupportLevelIsActive(0);
            // Used by Notes partial view (TODO: NotesViewModel) - JGL
            ViewBag.lstSupportLevelParent = lstSupportLevelParent;

            ViewData["ParentIdentity"] = ticket.SupportTicketID;



            ViewData["ShowPublishNoteToOwner"] = true;

            return View();
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        [FunctionFilter("Support", "~/Accounts")]

        public virtual ActionResult SearchTh(int accountID, string textx)
        {
            ViewData["AccountId"] = accountID;
            ViewData["Texto"] = textx;

            DataTable ValToSearch = ResultTicketSearchExtensions.SearchForAccount(accountID, textx);

          
            ViewData["DataSe"] = ValToSearch;
            int count = 0;
            ViewBag.Val = ValToSearch;


            //StringBuilder builder = new StringBuilder();

            //if (ValToSearch.Count > 0)
            //{
            //    foreach (var ValToSearcha in ValToSearch)
            //    {
            //        builder.Append("<tr>")
            //            .AppendCell(ValToSearcha.ID.ToString())
            //            .AppendCell(ValToSearcha.int1.ToString())
            //            .AppendCell(ValToSearcha.int2.ToString())
            //             .AppendCell(ValToSearcha.v1.ToString())
            //              .AppendCell(ValToSearcha.v2.ToString())
            //               .AppendCell(ValToSearcha.v3.ToString())
            //            .AppendCell(ValToSearcha.dt1.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
            //            .Append("</tr>");
            //        ++count;
            //    }

            //    return Json(new { result = true, totalPages = ValToSearch.Count, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"10\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" }, JsonRequestBehavior.AllowGet);

            return View(ValToSearch);

        }

        public virtual ActionResult SearchTh2(int accountID, string textx)
        {
            ViewData["AccountId"] = accountID;
            ViewData["Texto"] = textx;

            DataTable ValToSearch = ResultTicketSearchExtensions.SearchForAccount(accountID, textx);

            ViewData["DataSe"] = ValToSearch;
            int count = 0;
            ViewBag.Val = ValToSearch;
            //StringBuilder builder = new StringBuilder();

            //if (ValToSearch.Count > 0)
            //{
            //    foreach (var ValToSearcha in ValToSearch)
            //    {
            //        builder.Append("<tr>")
            //            .AppendCell(ValToSearcha.ID.ToString())
            //            .AppendCell(ValToSearcha.int1.ToString())
            //            .AppendCell(ValToSearcha.int2.ToString())
            //             .AppendCell(ValToSearcha.v1.ToString())
            //              .AppendCell(ValToSearcha.v2.ToString())
            //               .AppendCell(ValToSearcha.v3.ToString())
            //            .AppendCell(ValToSearcha.dt1.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
            //            .Append("</tr>");
            //        ++count;
            //    }

            //    return Json(new { result = true, totalPages = ValToSearch.Count, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
            //}
            //else
            //    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"10\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" }, JsonRequestBehavior.AllowGet);

            return View(DataTableToJsonWithJavaScriptSerializer(ValToSearch));

        }


        public string DataTableToJsonWithJavaScriptSerializer(DataTable table)
        {
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in table.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in table.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            return jsSerializer.Serialize(parentRow);
        }

        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult CreateTicketForAccount(int? accountID)
        {
            SupportTicket ticket = new SupportTicket();

            if (accountID.HasValue)
            {
                ticket.AccountID = (int)accountID;
                ticket.Account = Account.Load((int)accountID);
            }

            CoreContext.CurrentSupportTicket = ticket;

            // Used by Notes partial view (TODO: NotesViewModel) - JGL
            ViewData["ParentIdentity"] = ticket.SupportTicketID;
            ViewData["ShowPublishNoteToOwner"] = true;
            ViewData["IsSiteDWS"] = 0;

            ViewData["Edicion"] = 1;
            ViewData["SupportTicketID"] = 0;
            ViewData["SupportMotiveID"] = 0;
            ViewData["SupportLevelID"] = 0;
            ViewData["lstSupportTicketsFilesBE"] = new List<SupportTicketsFilesBE>();
            ViewData["StrlstSupportTicketsFilesBE"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(new List<SupportTicketsFilesBE>());
            List<System.Tuple<int, string, int, int>> lstSupportLevelParent = SupportTicket.GetLevelSupportLevelIsActive(0);
            // Used by Notes partial view (TODO: NotesViewModel) - JGL
            ViewBag.lstSupportLevelParent = lstSupportLevelParent;
            return View("Edit");

        }

        [OutputCache(CacheProfile = "PagedGridData")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult History(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id))
                    return RedirectToAction("Index", "Landing");

                var supportTicket = SupportTicket.LoadBySupportTicketNumber(id);

                var model = new HistoryViewModel();
                model.LoadResources(supportTicket);

                // Used by Notes partial view (TODO: NotesViewModel) - JGL
                ViewData["ParentIdentity"] = supportTicket.SupportTicketID;
                ViewData["ShowPublishNoteToOwner"] = true;

                return View(model);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult RequestNewTicket()
        {
            SupportTicket requestedTicket = null;

            try
            {
                bool requestTicketResult = false;
                string requestTicketNumber = string.Empty;

                requestedTicket = SupportTicket.RequestNewTicket(CoreContext.CurrentUser.UserID);

                if (requestedTicket != null)
                {
                    requestTicketResult = true;
                    requestTicketNumber = requestedTicket.SupportTicketNumber;
                }

                return Json(new { result = requestTicketResult, supportTicketNumber = requestTicketNumber });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [FunctionFilter("Support", "~/Accounts")]
        [HttpPost]
        public virtual ActionResult Save(
            int accountID, string title,
            string description, int? supportTicketCategoryID,
            int? supportTicketPriorityID,
            int supportTicketStatusID, int? assignedUserID,
            bool isVisibleToOwner, int? oldTicketStatusID,
            List<SupportTicketsPropertyBE> lstSupportTicketsPropertyBE,
            List<SupportTicketsFilesBE> lstSupportTicketsFilesBE,
            int? SupportLevelID,
            int? SupportMotiveID,
            List<int> ListaEliminarSupportTicketsFiles,
            SupportTicketGestionBE objSupportTicketGestionBE,
            string notifyMails
            )
        {
            int UserID = CoreContext.CurrentUser.UserID == null ? 0 : CoreContext.CurrentUser.UserID;

            objSupportTicketGestionBE.UserID = CoreContext.CurrentUser.UserID;
            objSupportTicketGestionBE.SupportTicketStatusID = supportTicketStatusID;
            objSupportTicketGestionBE.isInternal = isVisibleToOwner;
            objSupportTicketGestionBE.Descripction += notifyMails == "" ? "" : "\r\n Notificado a: " + notifyMails;

            string rutaDirectorio = GetBaseDir();

            SupportTicket ticketToEdit = null;

            if (CoreContext.CurrentSupportTicket.SupportTicketID > 0)
                ticketToEdit = SupportTicket.LoadBySupportTicketNumber(CoreContext.CurrentSupportTicket.SupportTicketNumber);
            else
                ticketToEdit = CoreContext.CurrentSupportTicket;


            SupportTicketsBE objSupportTicketsBE = new SupportTicketsBE();
            objSupportTicketsBE.RutaDirectorio = rutaDirectorio;

            objSupportTicketsBE.SupportLevelID = SupportLevelID.HasValue ? SupportLevelID.Value : 0;
            objSupportTicketsBE.SupportMotiveID = SupportMotiveID.HasValue ? SupportMotiveID.Value : 0;

            objSupportTicketsBE.CreatedByUserID = UserID;
            objSupportTicketsBE.ModifiedByUserID = UserID;
            objSupportTicketsBE.AccountID = accountID;
            objSupportTicketsBE.Title = title;
            objSupportTicketsBE.Description = description;
            objSupportTicketsBE.SupportTicketCategoryID = Convert.ToInt16(supportTicketCategoryID.HasValue ? supportTicketCategoryID.Value : 0);
            objSupportTicketsBE.SupportTicketStatusID = Convert.ToInt16(supportTicketStatusID);
            objSupportTicketsBE.AssignedUserID = assignedUserID.HasValue ? assignedUserID.Value : UserID;
            objSupportTicketsBE.IsVisibleToOwner = isVisibleToOwner;
            objSupportTicketsBE.SupportTicketPriorityID = Convert.ToInt16(supportTicketPriorityID.Value);
            objSupportTicketsBE.SupportTicketID = ticketToEdit.SupportTicketID;
            objSupportTicketsBE.IsSiteDWS = false;

            if (CoreContext.CurrentFilesSupportTickets != null)
            {
                lstSupportTicketsFilesBE = CoreContext.CurrentFilesSupportTickets;
                CoreContext.CurrentFilesSupportTickets = null;
            }

            int Resultado = SupportTicket.InsertarSuportTickets(
                                      objSupportTicketsBE,
                                      lstSupportTicketsPropertyBE ?? new List<SupportTicketsPropertyBE>(),
                                      lstSupportTicketsFilesBE ?? new List<SupportTicketsFilesBE>(),
                                      ListaEliminarSupportTicketsFiles ?? new List<int>(),
                                      objSupportTicketGestionBE
                                      );
            //SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket;
            // Only do a load (not load full, to avoid updating the account) - JHE


            try
            {
                // When Ticket status changed to 'Needs Input' send a notification email
                if (oldTicketStatusID != supportTicketStatusID && supportTicketStatusID == Constants.SupportTicketStatus.NeedsInput.ToInt())
                {
                    // TODO: Disable auto-responder for v.1
                    //DomainEventQueueItem.AddSupportTicketInfoRequestedCompletedEventToQueue(ticketToEdit.SupportTicketID);
                }

                ticketToEdit.Title = title;
                ticketToEdit.AccountID = accountID;
                ticketToEdit.Description = description;
                ticketToEdit.SupportTicketCategoryID = supportTicketCategoryID.ToShortNullable();
                ticketToEdit.SupportTicketPriorityID = supportTicketPriorityID.ToShort();
                ticketToEdit.SupportTicketStatusID = supportTicketStatusID.ToShort();
                ticketToEdit.AssignedUserID = assignedUserID;
                ticketToEdit.IsVisibleToOwner = isVisibleToOwner;

                if (ticketToEdit.SupportTicketNumber == null)
                {
                    ticketToEdit.SupportTicketNumber = Convert.ToString(10000 + Resultado);
                    objSupportTicketsBE.SupportTicketID = Resultado;
                }                
                
                //ticketToEdit.Save();

                //Enviar notificación del ticket por mail
                ExternalMail.SendMailSupportTicket(ticketToEdit, objSupportTicketsBE, notifyMails, CurrentLanguageID);

                return Json(new { result = true, supportTicketNumber = ticketToEdit.SupportTicketNumber, SupportTicketID = Resultado });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult GetSupportTickets(int page, int pageSize, DateTime? startDate, DateTime? endDate, string supportTicketNumber, string title,
            int? assignedUserID, int? accountID, int? supportTicketPriorityID, int? supportTicketCategoryID, int? supportTicketStatusID,
            string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, string UserSearch, string ConsultantSearch,
            int? SupportLevelID, int? SupportMotiveID, byte IsSiteDWS = 2
            )
        {

            bool allowManageMultiUsers = CoreContext.CurrentUser.HasFunction("Support-Manage All Tickets");

            if (!allowManageMultiUsers)
            {
                assignedUserID = CoreContext.CurrentUser.UserID;
            }

            if ((!assignedUserID.HasValue || assignedUserID.Value <= 0) && !UserSearch.IsNullOrWhiteSpace())
            {
                var result = this.GetFirstUserIdFromUserSearch(UserSearch);

                assignedUserID = result > 0 ? result : assignedUserID;
            }

            if ((!accountID.HasValue || accountID.Value <= 0) && !ConsultantSearch.IsNullOrWhiteSpace())
            {
                var result = this.GetFirstAccountIdFromAccountSearch(ConsultantSearch);

                accountID = result > 0 ? result : accountID;
            }

            try
            {
                if (startDate.HasValue && startDate.Value.Year < 1900)
                    startDate = null;
                if (endDate.HasValue && endDate.Value.Year < 1900)
                    endDate = null;
                StringBuilder builder = new StringBuilder();
                var supportTickets = SupportTicket.Search(new NetSteps.Data.Entities.Business.SupportTicketSearchParameters()
                {
                    AccountID = accountID,
                    AssignedUserID = assignedUserID,
                    SupportTicketNumber = supportTicketNumber,
                    Title = title,
                    SupportTicketPriorityID = supportTicketPriorityID,
                    SupportTicketCategoryID = supportTicketCategoryID,
                    SupportTicketStatusID = supportTicketStatusID,
                    StartDate = startDate,
                    EndDate = endDate,
                    PageIndex = page,
                    PageSize = pageSize,
                    OrderBy = orderBy,
                    SupportLevelID = SupportLevelID,
                    SupportMotiveID = SupportMotiveID,
                    OrderByDirection = orderByDirection,
                    IsSiteDWS = IsSiteDWS
                });

                Session["ResultGrid"] = supportTickets;
                ;

                if (supportTickets.Count > 0)
                {
                    int count = 0;
                    foreach (var supportTicket in supportTickets)
                    {
                        builder.Append("<tr>")
                            .AppendLinkCell("~/Support/Ticket/Edit/" + supportTicket.SupportTicketNumber, supportTicket.SupportTicketNumber)
                            .AppendCell(supportTicket.AssignedUsername)
                            .AppendCell(SmallCollectionCache.Instance.SupportTicketPriorities.GetById(supportTicket.SupportTicketPriorityID).GetTerm())
                            .AppendCell(supportTicket.Title)
                            .AppendLinkCell("~/Accounts/Overview/Index/" + supportTicket.AccountNumber, supportTicket.FirstName)
                            .AppendLinkCell("~/Accounts/Overview/Index/" + supportTicket.AccountNumber, supportTicket.LastName)
                            .AppendCell(SmallCollectionCache.Instance.SupportTicketStatuses.GetById(supportTicket.SupportTicketStatusID).GetTerm())
                            .AppendCell(supportTicket.SupportLevelMotive)
                            .AppendCell(supportTicket.DateCreated.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .AppendCell(supportTicket.DateLastModified.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
                            .AppendCell(supportTicket.IsSiteDWS == 1 ? "DWS" : "GMP")
                            .Append("</tr>");
                        ++count;
                    }
                    return Json(new { result = true, totalPages = supportTickets.TotalPages, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"10\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage }, JsonRequestBehavior.AllowGet);
            }
        }

        [OutputCache(CacheProfile = "PagedGridData")]
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult GetSupportTicketsDetails(int page, int pageSize, DateTime? startDate, string orderBy, string orderByDirection, DateTime? endDate
               , int? typeConsult, int? SupportTicketPriorityID, int? SupportTicketCategoryID, int? SupportTicketStatusID
               , string SupportTicketNumber, string Title, string OrderNumber, string InvoiceNumber, int? AssignedUserID, int? accountID, int? CreatebyUserID, int? UserTypeID, int? AccountTypeID,
               int? IsConfirmID, int? CampaignID, int? SupportLevelID, int? SupportMotiveID, byte IsSiteDWS = 2

           )
        {

            //bool allowManageMultiUsers = CoreContext.CurrentUser.HasFunction("Support-Manage All Tickets");
            //if (!allowManageMultiUsers)
            //{
            //    AssignedUserID = CoreContext.CurrentUser.UserID;
            //}
            //if (CampaignID == 0)
            //{
            //    CampaignID = null;
            //} 
            if (orderByDirection == "Descending")
            {
                orderByDirection = "desc";
            }
            else
            {
                orderByDirection = "asc";
            }

            var ConsultParameter = new SupportTicketSearchDetailsParameter()
            {
                TypeConsult = typeConsult ?? 0,
                PriorityID = SupportTicketPriorityID,
                CategoryID = SupportTicketCategoryID,
                StatusID = SupportTicketStatusID,
                SupportTicket = SupportTicketNumber,
                Title = Title,
                AssignedUserID = AssignedUserID,
                ConsultSearchID = accountID,
                CreateByUserID = CreatebyUserID,
                TypeUserID = UserTypeID,
                TypeConsultID = AccountTypeID,
                OrderNumber = OrderNumber,
                InvoiceNumber = InvoiceNumber,
                IsConfirmID = IsConfirmID,
                StartDate = startDate,
                EndDate = endDate,
                CampaignID = CampaignID,
                PageSize = pageSize,
                PageIndex = page,
                OrderBy = orderBy,
                Order = orderByDirection,
                SupportLevelID = SupportLevelID.HasValue ? SupportLevelID.Value : 0,
                SupportMotiveID = SupportMotiveID.HasValue ? SupportMotiveID.Value : 0,
                IsSiteDWS = IsSiteDWS
            };
            var ticketConsult = ConsultSupportTicketLogic.Instance.GetSupportTicket(ConsultParameter);

            int rowTotal = 0;
            StringBuilder builder = new StringBuilder();
            int count = 0;

            Session["ConsultParameter"] = ConsultParameter;
            Session["ResultGrid"] = ticketConsult;

            foreach (var ticketConsultResult in ticketConsult)
            {
                builder.Append("<tr>");
                builder
                    .AppendLinkCell("~/Support/Ticket/Edit/" + ticketConsultResult.SupportTicketNumber, ticketConsultResult.SupportTicketNumber)
                    .AppendCell(ticketConsultResult.AssignedUsername)
                    .AppendCell(ticketConsultResult.PriorityName)
                    //.AppendCell(ticketConsultResult.Title)
                    .AppendCell(ticketConsultResult.AccountNumber.ToString())
                    .AppendCell(ticketConsultResult.OrderNumber)
                    .AppendCell(ticketConsultResult.State)
                    .AppendCell(ticketConsultResult.City)
                    .AppendCell(ticketConsultResult.InvoiceNumber)
                    .AppendLinkCell("~/Accounts/Overview/Index/" + ticketConsultResult.AccountNumber, ticketConsultResult.FirstName)
                    .AppendLinkCell("~/Accounts/Overview/Index/" + ticketConsultResult.AccountNumber, ticketConsultResult.LastName)
                    .AppendCell(ticketConsultResult.StatusName)
                    .AppendCell(ticketConsultResult.CategoryName)
                    .AppendCell(ticketConsultResult.CreateUserName)
                    .AppendCell(FormatDatetimeByLenguage(ticketConsultResult.DateCreated,CoreContext.CurrentCultureInfo))
                    .AppendCell(FormatDatetimeByLenguage(ticketConsultResult.DateLastModified,CoreContext.CurrentCultureInfo))
                    .AppendCell(FormatDatetimeByLenguage(ticketConsultResult.Close,CoreContext.CurrentCultureInfo))
                    ;
                builder.Append("</tr>");
                ++count;
                rowTotal = Convert.ToInt32(ticketConsultResult.RowTotal);
            }
            return Json(new { result = true, totalPages = rowTotal, page = rowTotal == 0 ? "<tr><td colspan=\"10\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" : builder.ToString() });
        }

        //private string FormatDatetimeByLenguage(string fecha, CultureInfo culture)
        //{
        //    if (string.IsNullOrEmpty(fecha))
        //        return "N/A";
        //    else
        //    {
               
        //        DateTime Temp;
        //        if (DateTime.TryParse(fecha, out Temp) == false)
        //        {

        //            if (culture.Name == "en-US")
        //                return string.Format("{0:MM/dd/yyyy HH:mm:ss}", Temp);
        //            else
        //                return string.Format("{0:dd/MM/yyyy HH:mm:ss}", Temp);
        //        }
        //        else
        //            return fecha;

        //    }
        //}


        private string FormatDatetimeByLenguage(string fecha, CultureInfo culture)
        {
            if (string.IsNullOrEmpty(fecha))
                return "N/A";
            else
            {
                DateTime? fec = null;
                DateTime Temp;

                var split = fecha.Split('/');
                if (split.Length >= 3)
                {

                    if (culture.Name == "en-US")

                        return string.Format("{0}/{1}/{2}", split[1], split[0], split[2]);

                    else
                        return string.Format("{0}/{1}/{2}", split[0], split[1], split[2]);

                }

                else
                    return fecha;

            }
        }

        private int GetFirstUserIdFromUserSearch(string query)
        {
            var result = NetSteps.Data.Entities.User.SlimSearchGetFirstResult(query);

            return result.Key;
        }

        private int GetFirstAccountIdFromAccountSearch(string query)
        {
            var result = AccountCache.GetTopAccountSearchByTextResult(query);

            return result.Key;
        }


        #region Gestion de tickets
        [HttpPost]
        public ActionResult ListarSupportLevel(int input)
        {
            List<System.Tuple<int, string, int, int>> lstSupoprtLevel = SupportTicket.GetLevelSupportLevelIsActive(input);

            if (lstSupoprtLevel.Count == 0)
            {
                List<System.Tuple<int, string, int, int>> lstSupoprtLevel1 = SupportTicket.GetLevelSupportLevelMotiveIsActive(input);

                return Json(new { items = lstSupoprtLevel1, isLast = true });
            }
            else
            {
                return Json(new { items = lstSupoprtLevel, isLast = false });
            }

        }
        public virtual ActionResult GetDetaillSupporMotiveLevel(int SupportMotiveID, int SupportTicketID, int ModoEdicion, int IsSiteDWS)
        {
            try
            {
                SupportTicketDetaillModel objSupportTicketDetaillModel = GetSupportTicketDetaillModel(SupportMotiveID, SupportTicketID, ModoEdicion, (IsSiteDWS == 1));

                return PartialView("SupportTicketDetaill", objSupportTicketDetaillModel);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public virtual ActionResult InformacionCuenta(int AccountID)
        {
            try
            {
                AccountInformacion objAccountInformacion = Account.ListarAccountsInformacionAdicional(AccountID);

                return PartialView("AccountInformacionAdicional", objAccountInformacion);

            }
            catch (Exception)
            {

                throw;
            }
        }
        public static SupportTicketDetaillModel GetSupportTicketDetaillModel(int SupportMotiveID, int SupportTicketID, int ModoEdicion, Boolean IsSiteDWS
                 )
        {
            List<SupportMotivePropertyTypes> LstSupportMotivePropertyTypes = SupportTicket.ListarSupportMotivePropertyTypesPorMotivo(SupportMotiveID, SupportTicketID, false);
            List<SupportMotivePropertyValues> LstSupportMotivePropertyValues = SupportTicket.ListarSupportMotivePropertyValuesPorMotivo(SupportMotiveID);
            List<SupportMotiveTask> LstSupportMotiveTask = SupportTicket.ListarSupportMotiveTaskPorMotivo(SupportMotiveID);
            List<SupportTicketsFilesBE> lstSupportTicketsFilesBE = SupportTicket.ObtenerSupportTicketsFilesporSupporMotive(SupportTicketID);

            SupportTicketDetaillModel objSupportTicketDetaillModel = new SupportTicketDetaillModel
                (
                        LstSupportMotivePropertyTypes,
                        LstSupportMotivePropertyValues,
                        LstSupportMotiveTask,
                        lstSupportTicketsFilesBE,
                        ModoEdicion: (ModoEdicion == 1),
                        IsSiteDWS: IsSiteDWS

                );
            return objSupportTicketDetaillModel;
        }
        [HttpPost]
        public ActionResult GuardarArchivos()
        {

            List<SupportTicketsFilesBE> lstSupportTicketsFilesBE = new List<SupportTicketsFilesBE>();
            SupportTicketsFilesBE objSupportTicketsFilesBE = null;

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                objSupportTicketsFilesBE = new SupportTicketsFilesBE();
                objSupportTicketsFilesBE.FilePath = QuitarCaracteresInvalidos(Path.GetFileName(file.FileName));
                objSupportTicketsFilesBE.Content = ConvertirMatrizByte(file);
                lstSupportTicketsFilesBE.Add
                    (
                         objSupportTicketsFilesBE
                    );
            }

            CoreContext.CurrentFilesSupportTickets = lstSupportTicketsFilesBE;
            return Json(new { ok = "ok" });
        }
        public byte[] ConvertirMatrizByte(HttpPostedFileBase file)
        {
            byte[] FileUpload = null;
            BinaryReader rdr = new BinaryReader(file.InputStream);
            FileUpload = rdr.ReadBytes((int)file.ContentLength);
            return FileUpload;
        }
        public ActionResult Download(int? id)
        {
            Dictionary<int, string> dcSupportTicketFile = SupportTicket.GetFileName(id.HasValue ? id.Value : 0);

            if (dcSupportTicketFile != null)
            {
                int _id = id.HasValue ? id.Value : 0;
                // string extension =
                string fullName = Path.Combine(GetBaseDir(), _id.ToString() + "_" + dcSupportTicketFile[_id]);

                byte[] fileBytes = GetFile(fullName);
                return File(
                    fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fullName.Replace("-", " "));
            }
            return File(
                    new byte[1], System.Net.Mime.MediaTypeNames.Application.Octet, "notFound.txt");

        }
        string GetBaseDir()
        {
            string pathFile = WebConfigurationManager.AppSettings["SupportTicketDirectory"].ToString();
            int index = pathFile.IndexOf("~");
            if (index >= 0)
            {
                return HttpContext.Server.MapPath(pathFile);
            }
            else
            {
                return pathFile;
            }

        }

        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }

        public string QuitarCaracteresInvalidos(String NombreArchivo)
        {
            string _NombreArchivo = NombreArchivo;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars()) + "_-";

            foreach (char c in invalid)
            {
                _NombreArchivo = _NombreArchivo.Replace(c.ToString(), "");
            }
            _NombreArchivo = Regex.Replace(_NombreArchivo, @"\s+", "-");
            return _NombreArchivo;
        }
        #endregion


    }
}
