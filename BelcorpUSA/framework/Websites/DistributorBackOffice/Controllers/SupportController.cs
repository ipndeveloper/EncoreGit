using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Models.SupportTicketModels;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Utility;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;
//using NetSteps.Common;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Common.Configuration;
using System.IO;

using System.Web;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using nsCore.Areas.Support.Models.Ticket;


namespace DistributorBackOffice.Controllers
{
	public class SupportController : BaseController
	{
		[FunctionFilter("Support", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult Index(DateTime? startDate, DateTime? endDate, string supportTicketNumber, short? supportTicketStatusID, bool? showOpenTickets)
		{
			try
			{
				// Get the account id
				int accountID = CurrentAccount.AccountID;

				// Create search parameters for support tickets
				SupportTicketSearchParameters searchParams = new SupportTicketSearchParameters
				{
					AccountID = accountID,
					SupportTicketNumber = supportTicketNumber,
					StartDate = startDate,
					EndDate = endDate,
					SupportTicketStatusID = supportTicketStatusID,
					SupportTicketStatusOpen = showOpenTickets,
					PageIndex = 0,
					PageSize = 20,
					OrderByDirection = NetSteps.Common.Constants.SortDirection.Ascending,
					IsVisibleToOwner = true
				};

				// Search for current user's support tickets
				var currentUserSupportTickets = SupportTicket.Search(searchParams);
//SupportTicketStatuses

				// Based on supportTicketStatusID
				if (showOpenTickets == true)
				{
					ViewBag.Open = true;
				}
				else if (supportTicketStatusID > 0)
				{
					ViewBag.Selected = supportTicketStatusID;
				}

				// Pass this support ticket as the model
				return View(searchParams);
			}
			catch (Exception ex)
			{
				var exception =
					 EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		/// <summary>
		/// Allows the user to edit their ticket
		/// </summary>
		[FunctionFilter("Support-Edit Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult EditTicket(string id)
		{
			// Grab all of the support ticket categories and insert it in a ViewBag/ViewData
			var supportCategories = SmallCollectionCache.Instance.SupportTicketCategories
												 .OrderBy(x => x.SortIndex)
												 .Where(x => x.Active).ToList();

			ViewBag.Categories = supportCategories;

			SupportTicket ticket = new SupportTicket();
			if (!string.IsNullOrEmpty(id))
			{
				ticket = SupportTicket.LoadBySupportTicketNumberFull(id);
			}

			return View(ticket);
		}

		[HttpPost]
		[FunctionFilter("Support-Edit Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult EditTicket(string id, string title, string description,
																  short? supportTicketCategoryID)
		{
			SupportTicket ticket = new SupportTicket();

			try
			{
				if (!string.IsNullOrEmpty(id))
				{
					ticket = SupportTicket.LoadBySupportTicketNumberFull(id);
					ticket.Title = title;
					ticket.Description = description;
					ticket.SupportTicketCategoryID = supportTicketCategoryID;
					ticket.Save();
				}
				return Json(new { result = true, message = string.Empty });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}


		[FunctionFilter("Support-Create Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult CreateTicket(string id)
		{
            SupportTicketsBE objSupportTicketsBE = new SupportTicketsBE();
            if (!string.IsNullOrEmpty(id))
            {
                ViewData["Edicion"] = 0;
                objSupportTicketsBE = SupportTicket.ObtenerSupportTicketsBE(Convert.ToInt32(id));
                List<SupportTicketGestionBE> lstSupportTicketGestionBE = SupportTicket.ListarSupportTicketGestionBE(objSupportTicketsBE.SupportTicketID);
                CoreContext.CurrentSupportTicket = SupportTicket.LoadBySupportTicketNumberFull(objSupportTicketsBE.SupportTicketNumber);
                ViewData["lstSupportTicketGestionBE"] = lstSupportTicketGestionBE;
                ViewData["ParentIdentity"] = objSupportTicketsBE.SupportTicketID;
                ViewData["ShowPublishNoteToOwner"] = true;
            }
            else
            {
                ViewData["Edicion"] = 1;
            }

            ViewData["accountID"] = CurrentAccount.AccountID;
            List<System.Tuple<int, string, int, int>> lstSupportLevelParent = SupportTicket.GetLevelSupportLevelIsActive(0);
            ViewBag.lstSupportLevelParent = lstSupportLevelParent;

            List<SupportTicketsFilesBE> lstSupportTicketsFilesBE = SupportTicket.ObtenerSupportTicketsFilesporSupporMotive(objSupportTicketsBE.SupportTicketID) ?? new List<SupportTicketsFilesBE>();
            ViewData["StrlstSupportTicketsFilesBE"] = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(lstSupportTicketsFilesBE);
            ViewData["lstSupportTicketsFilesBE"] = lstSupportTicketsFilesBE;
            ViewData["SupportTicketID"] = objSupportTicketsBE.SupportTicketID;
            ViewData["SupportLevelID"] = objSupportTicketsBE.SupportLevelID;
            ViewData["SupportMotiveID"] = objSupportTicketsBE.SupportMotiveID;
            
            return View(  objSupportTicketsBE);
		}

        #region Gestion de tickets


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
            SupportTicketGestionBE objSupportTicketGestionBE
            )
        {
            supportTicketPriorityID = 1;//Urgent
            supportTicketStatusID = 1;//Entered;
            isVisibleToOwner = true;
            accountID = CurrentAccountId;

            objSupportTicketGestionBE = null;

            int UserID = CoreContext.CurrentUser.UserID == null ? 0 : CoreContext.CurrentUser.UserID;
           
            //objSupportTicketGestionBE.UserID = CoreContext.CurrentUser.UserID;

            string rutaDirectorio = GetBaseDir();

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
            objSupportTicketsBE.AssignedUserID = assignedUserID.HasValue ? assignedUserID.Value : 0;
            objSupportTicketsBE.IsVisibleToOwner = isVisibleToOwner;
            objSupportTicketsBE.SupportTicketPriorityID = Convert.ToInt16(supportTicketPriorityID.Value);
            objSupportTicketsBE.SupportTicketID = 0;// ticketToEdit.SupportTicketID;
            objSupportTicketsBE.IsSiteDWS = true;
            if (CoreContext.CurrentFilesSupportTickets != null)
            {
                lstSupportTicketsFilesBE = CoreContext.CurrentFilesSupportTickets;
                CoreContext.CurrentFilesSupportTickets = null;
            }
            // 
            // objSupportTicketGestionBE.SupportTicketStatusID = supportTicketStatusID;

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

                
                //ticketToEdit.Save();
                return Json(new { result = true, supportTicketNumber = Convert.ToString(10000 + Resultado), SupportTicketID = Resultado });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        } 


        [HttpPost]
        public ActionResult ListarSupportLevel(int input)
        {
            List<System.Tuple<int, string, int, int>> lstSupoprtLevel = SupportTicket.GetLevelSupportLevelIsActive(input);
            if (lstSupoprtLevel.Count == 0)
            {
                List<System.Tuple<int, string, int, int>> lstSupoprtLevel1 = SupportTicket.GetLevelSupportLevelMotiveIsActive(input,true);
                return Json(new { items = lstSupoprtLevel1, isLast = true });
            }
            else
            {
                return Json(new { items = lstSupoprtLevel, isLast = false });
            }

        }
        public virtual ActionResult GetDetaillSupporMotiveLevel(int SupportMotiveID, int SupportTicketID, int ModoEdicion)
        {
            try
            {
                SupportTicketDetaillModel objSupportTicketDetaillModel = GetSupportTicketDetaillModel(SupportMotiveID, SupportTicketID, ModoEdicion);

                return PartialView("SupportTicketDetaill", objSupportTicketDetaillModel);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static SupportTicketDetaillModel GetSupportTicketDetaillModel(int SupportMotiveID, int SupportTicketID, int ModoEdicion)
        {
            List<SupportMotivePropertyTypes> LstSupportMotivePropertyTypes = SupportTicket.ListarSupportMotivePropertyTypesPorMotivo(SupportMotiveID, SupportTicketID, true);
            List<SupportMotivePropertyValues> LstSupportMotivePropertyValues = SupportTicket.ListarSupportMotivePropertyValuesPorMotivo(SupportMotiveID);
            
            List<SupportTicketsFilesBE> lstSupportTicketsFilesBE = SupportTicket.ObtenerSupportTicketsFilesporSupporMotive(SupportTicketID);

            SupportTicketDetaillModel objSupportTicketDetaillModel = new SupportTicketDetaillModel
                (
                        LstSupportMotivePropertyTypes,
                        LstSupportMotivePropertyValues,                        
                        lstSupportTicketsFilesBE,
                        ModoEdicion: (ModoEdicion == 1)
                );
            return objSupportTicketDetaillModel;
        }


        #region Carga  de archivos 
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

         public string QuitarCaracteresInvalidos(String NombreArchivo)
        {
            string _NombreArchivo = NombreArchivo;
            string invalid = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars())+"_-";

            foreach (char c in invalid)
            {
                _NombreArchivo = _NombreArchivo.Replace(c.ToString(), "");
            }
            _NombreArchivo = Regex.Replace(_NombreArchivo, @"\s+", "-");
            return _NombreArchivo;
        }
        #endregion
        byte[] GetFile(string s)
        {
            System.IO.FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
        }
        #endregion

        
        //[HttpPost]
        //[FunctionFilter("Support-Create Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        //public virtual ActionResult CreateTicket(SupportTicket ticket)
        //{
        //    try
        //    {
        //        ticket.AccountID = CurrentAccount.AccountID;
        //        ticket.SupportTicketStatusID = Constants.SupportTicketStatus.Entered.ToShort();
        //        ticket.SupportTicketPriorityID = Constants.SupportTicketPriority.Medium.ToShort();
        //        ticket.IsVisibleToOwner = true;
        //        ticket.Save();
        //        return Json(new { result = true, message = string.Empty });

        //    }
        //    catch (Exception ex)
        //    {
        //        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
        //        return Json(new { result = false, message = exception.PublicMessage });
        //    }
        //}


        [HttpPost]
        [FunctionFilter("Support-Create Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult CreateTicket(SupportTicketSearchParameters ticket, List<SupportTicketsPropertySearchParameters> supportTicketsProperty,
            SupportTicketGestionSearchParameters supportTicketGestion, List<SupportTicketsFilesSearchParameters> supportTicketsFiles)
        {
            try
            {
                var dateCurrent = DateTime.Now;
                var userID = CurrentAccount.UserID;
                ticket.CreatedByUserID=int.Parse(userID.ToString());
                ticket.ModifiedByUserID=int.Parse(userID.ToString());
                ticket.DateCreatedUTC = dateCurrent;
                ticket.DateLastModifiedUTC = dateCurrent;     
                ticket.DateCloseUTC = dateCurrent;
                SupportTicketsBusinessLogic busines = new SupportTicketsBusinessLogic();
                var supportTicketID = busines.Insert(ticket).SupportTicketID;

                if (supportTicketsProperty != null)
                {
                    foreach (var item in supportTicketsProperty)
                    {
                        item.SupportTicketID = supportTicketID;
                        SupportTicketsPropertyBusinessLogic.Insert(item);
                    }
                }

                if (supportTicketsFiles != null)
                {
                    foreach (var item in supportTicketsFiles)
                    {
                        item.SupportTicketID = supportTicketID;
                        item.UserID = int.Parse(userID.ToString());
                        item.DateCreatedUTC = dateCurrent.Date;
                        SupportTicketsFilesBusinessLogic.Insert(item);
                    }
                }

                supportTicketGestion.SupportTicketID = supportTicketID;
                supportTicketGestion.DateCreatedUTC = dateCurrent;
                supportTicketGestion.UserID = int.Parse(userID.ToString());
                SupportTicketGestionBusinessLogic.Insert(supportTicketGestion);
                return Json(new { result = true, message = string.Empty });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

		[FunctionFilter("Support", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult ViewTicket(string id)
		{
			SupportTicket ticket = new SupportTicket();
			List<Note> notes = new List<Note>();

			if (!string.IsNullOrEmpty(id))
			{
				ticket = SupportTicket.LoadBySupportTicketNumberFull(id);
				ticket.SupportTicketCategory = SmallCollectionCache.Instance.
									 SupportTicketCategories.First(x => x.SupportTicketCategoryID == ticket.SupportTicketCategoryID);

				ticket.SupportTicketStatus = SmallCollectionCache.Instance.SupportTicketStatuses
									 .First(x => x.SupportTicketStatusID == ticket.SupportTicketStatusID);
			}

			return View(ticket);
		}

        public virtual ActionResult UploadFile()
        {
            string cleanedFileName = string.Empty;
            string fileName = string.Empty;

            bool nonHtml5Browser = Request.Files.Count > 0;

            if (nonHtml5Browser)
                fileName = Request.Files[0].FileName;
            else if (Request.ContentType.Equals("application/octet-stream", StringComparison.InvariantCultureIgnoreCase))
                fileName = Request.Params["qqfile"];
            else
                return Json(new { success = false, error = "No files uploaded." });

            //Save the file on the server
            cleanedFileName = System.Text.RegularExpressions.Regex.Replace(fileName, @"[^a-zA-Z0-9\s_\.]", "");
            string folder = string.Empty;
            switch (cleanedFileName.GetFileType())
            {
                case NetSteps.Common.Constants.FileType.Image:
                    folder = "Images";
                    break;
                case NetSteps.Common.Constants.FileType.Video:
                    folder = "Videos";
                    break;
                case NetSteps.Common.Constants.FileType.Flash:
                    folder = "Flash";
                    break;
                default:
                    folder = "Documents";
                    break;
            }
            
            string ruta = ConfigurationManager.AppSettings["SupportTicket"];
            
            string fullPath = ruta + Path.GetFileName(cleanedFileName);
            
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(Server.MapPath(fullPath), FileMode.Create, FileAccess.Write))
            {
                ms.WriteTo(file);
            }

            var json = new { success = true, folder = folder.ToLower(), uploaded = DateTime.Now.ToString(),
                             filePath = Path.GetFileName(cleanedFileName)
            };

            if (nonHtml5Browser)
                return Content(json.ToJSON(), "text/html");
            else
                return Json(json);
        }

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Support", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual ActionResult GetSupportTickets(int page, int pageSize, DateTime? startDate, DateTime? endDate, string supportTicketNumber, string title,
			 int? assignedUserID, int? accountID, int? supportTicketPriorityID, int? supportTicketCategoryID, int? supportTicketStatusID,
			 string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection, bool? showOpenTickets)
		{
			// Set the accountID to the current User's account
			if (accountID == null)
				accountID = CurrentAccount.AccountID;

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
					SupportTicketNumber = supportTicketNumber,
					StartDate = startDate,
					EndDate = endDate,
					SupportTicketStatusID = supportTicketStatusID,
					SupportTicketStatusOpen = showOpenTickets,
					Title = title,
					PageIndex = page,
					PageSize = pageSize,
					OrderByDirection = orderByDirection,
					IsVisibleToOwner = true,
                    IsSiteDWS=2
				});
				if (supportTickets.Count > 0)
				{
					int count = 0;
					foreach (var supportTicket in supportTickets)
					{
						builder.Append("<tr>")
                             .AppendLinkCell("~/Support/CreateTicket/" + supportTicket.SupportTicketID.ToString(), supportTicket.SupportTicketNumber)
							 .AppendCell("<span class=\"TitleCaption\">" + supportTicket.Title + "</span>", "")
							 .AppendCell(SmallCollectionCache.Instance.SupportTicketStatuses.GetById(supportTicket.SupportTicketStatusID).GetTerm())
                             .AppendCell(supportTicket.SupportLevelMotive)
							 .AppendCell(supportTicket.DateCreated.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
							 .Append("</tr>");
						++count;
					}
					return Json(new { result = true, totalPages = supportTickets.TotalPages, page = builder.ToString() }, JsonRequestBehavior.AllowGet);
				}
				else
					return Json(new { result = true, totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage }, JsonRequestBehavior.AllowGet);
			}
		}

		/// <summary>
		/// Returns the count for each support ticket type
		/// </summary>
		[FunctionFilter("Support", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual JsonResult TicketCount()
		{
			// Get the account id
			int accountID = CurrentAccount.AccountID;

			// Create search parameters for support tickets
			SupportTicketSearchParameters searchParams = new SupportTicketSearchParameters
			{
				AccountID = accountID,
				PageSize = null,
				IsVisibleToOwner = true
			};

			// Search for current user's support tickets
			var currentUserSupportTickets = SupportTicket.Search(searchParams);

			// Get counts for each support ticket status
			int? totalTickets = currentUserSupportTickets.TotalCount;
			int? openTickets = currentUserSupportTickets.Count(x => x.SupportTicketStatusID != (short)Constants.SupportTicketStatus.Resolved);
			int? resolvedTickets = currentUserSupportTickets.Count(x => x.SupportTicketStatusID == (short)Constants.SupportTicketStatus.Resolved);
			int? needsInputTickets = currentUserSupportTickets.Count(x => x.SupportTicketStatusID == (short)Constants.SupportTicketStatus.NeedsInput);

			return Json(new
			{
				result = true,
				totalTickets = totalTickets,
				openTickets = openTickets,
				resolvedTickets = resolvedTickets,
				needsInputTickets = needsInputTickets
			});

		}

		/// <summary>
		/// Changes the support ticket status to resolved.
		/// </summary>
		[FunctionFilter("Support-Edit Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
		public virtual JsonResult MarkAsResolved(string id)
		{
			try
			{
				SupportTicket ticket = new SupportTicket();

				if (!string.IsNullOrEmpty(id))
				{
					ticket = SupportTicket.LoadBySupportTicketNumber(id);
					ticket.SupportTicketStatusID = Constants.SupportTicketStatus.Resolved.ToShort();
					ticket.Save();
				}
				return Json(new { result = true, message = string.Empty });

			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[ChildActionOnly]
		public virtual ActionResult _TicketHistory(string id, string loadedEntitySessionVarKey = null)
		{
			try
			{
				SupportTicket supportTicket = SupportTicket.LoadBySupportTicketNumber(id);

				var model = new HistoryViewModel();
				model.LoadResources(supportTicket);

				AuditHistoryGridModel gridModel = model.AuditHistoryGridModel;

				PaginatedList<AuditLogRow> auditLogRows = null;
				if (!loadedEntitySessionVarKey.IsNullOrEmpty() && Session[loadedEntitySessionVarKey] != null)
				{
					auditLogRows = AuditHelper.GetAuditLog(Session[loadedEntitySessionVarKey], gridModel.EntityName, gridModel.EntityId, new AuditLogSearchParameters()
					{
						PageSize = null
					});
				}
				else
				{
					auditLogRows = AuditHelper.GetAuditLog(gridModel.EntityName, gridModel.EntityId, new AuditLogSearchParameters()
					{
						PageSize = null
					});
				}

				List<string> propertyNames = new List<string>();

				List<TicketHistoryModel> ticketHistoryModel = new List<TicketHistoryModel>();

				propertyNames.Add(GetPropertyName(() => (new SupportTicket()).SupportTicketStatusID));
				propertyNames.Add(GetPropertyName(() => (new SupportTicket()).Description));
				propertyNames.Add(GetPropertyName(() => (new SupportTicket()).SupportTicketCategoryID));
				propertyNames.Add(GetPropertyName(() => (new SupportTicket()).Title));

				foreach (AuditLogRow auditLogRow in auditLogRows.Where(x => propertyNames.Contains(x.ColumnName)))
				{

					string oldValue = string.Empty;
					string newValue = string.Empty;
					string columnName = string.Empty;

					if (propertyNames.Find(x => x == (GetPropertyName(() => (new SupportTicket()).SupportTicketStatusID))) == auditLogRow.ColumnName)
					{
						columnName = Translation.GetTerm("SupportTicketStatus", "Support Ticket Status");
						oldValue = SmallCollectionCache.Instance.SupportTicketStatuses.GetById(auditLogRow.OldValue.ToShort()).GetTerm();
						newValue = SmallCollectionCache.Instance.SupportTicketStatuses.GetById(auditLogRow.NewValue.ToShort()).GetTerm();
					}
					else if (propertyNames.Find(x => x == (GetPropertyName(() => (new SupportTicket()).SupportTicketCategoryID))) == auditLogRow.ColumnName)
					{
						columnName = Translation.GetTerm("SupportTicketCategory", "Support Ticket Category");
						oldValue = SmallCollectionCache.Instance.SupportTicketCategories.GetById(auditLogRow.OldValue.ToShort()).GetTerm();
						newValue = SmallCollectionCache.Instance.SupportTicketCategories.GetById(auditLogRow.NewValue.ToShort()).GetTerm();
					}
					else
					{
						columnName = Translation.GetTerm(auditLogRow.ColumnName);
						oldValue = auditLogRow.OldValue;
						newValue = auditLogRow.NewValue;
					}

					ticketHistoryModel.Add(new TicketHistoryModel
					{
						ColumnName = columnName,
						DateChanged = auditLogRow.DateChanged,
						OldValue = oldValue,
						NewValue = newValue
					});

				}

				return PartialView(ticketHistoryModel);
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				throw exception;
			}
		}

		public virtual string GetPropertyName<T>(Expression<Func<T>> expression)
		{
			MemberExpression body = (MemberExpression)expression.Body;
			return body.Member.Name;
		}

        [OutputCache(CacheProfile = "AutoCompleteData")]
        [FunctionFilter("Support-Create Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SearchAccount(string query)
        {
            try
            {
                return Json(NetSteps.Data.Entities.Account.SlimSearch(query).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        [FunctionFilter("Support-Create Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SearchMotive(string query)
        {
            try {
                //SupportMotives busines = new SupportMotives();
                List<SupportMotiveSearchData> motives = new List<SupportMotiveSearchData>();
                motives = SupportMotives.SearchByMotive(query);
                //motives.ToDictionary(x=>x.)
                Dictionary<int, string> listaFinal = new Dictionary<int, string>();
                foreach (var item in motives.ToList())
                {
                    listaFinal.Add(item.SupportMotiveID, item.Name);
                }
              
                return Json(listaFinal.ToAJAXSearchResults());
            }
            catch(Exception ex) {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        [OutputCache(CacheProfile = "AutoCompleteData")]
        [FunctionFilter("Support-Create Ticket", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult SearchUser(string query)
        {
            try
            {
                return Json(NetSteps.Data.Entities.User.SlimSearch(query).ToAJAXSearchResults());
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }        

        public virtual ActionResult GetConfigMotive(int MotiveID)
        {
            try
            {
               string stringBuilder = BuildPropertyItems(MotiveID);

               return Json(new { result = true, select = stringBuilder.ToString() });

            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual string BuildPropertyItems(int MotiveID)
        {
            var builder = new StringBuilder();
            SupportTicketsBusinessLogic busines = new SupportTicketsBusinessLogic();
            if (MotiveID > 0)
            {
              
                var ListFormDinamic = AccountReferencesBusinessLogic.GetDinamicFormMotive(MotiveID);
                builder.Append("<div id=\"propertyContainer").Append("\" style=\"margin-left:15px;\">");
                builder.Append("<input id=\"hdnCanProperties\" type=\"hidden\" value=\"").Append(ListFormDinamic.Count()).Append("\" />");
                int i = 0;
                foreach (var item in ListFormDinamic) {
                
                
                    if (item.DataType == "List")
                    {
                        busines = new SupportTicketsBusinessLogic();
                        int supportMotivePropertyTypeID = int.Parse(item.SupportMotivePropertyTypeID.ToString());
                        var list = busines.GetValuesByPropertyID(supportMotivePropertyTypeID);

                        builder.Append("<div class=\"FRow\">");
                        builder.Append("<div class=\"FLabel\">");
                        builder.Append("<label>").Append(item.NameEtiqueta).Append("</label>");
                        builder.Append("<input type=\"hidden\" id=\"hdnvalor").Append(i).Append("\" value=\"").Append(item.SupportMotivePropertyTypeID).Append("\" />");
                        builder.Append("<input type=\"hidden\" id=\"hdnList_Text").Append(i).Append("\" value=\"List\" />");
                        builder.Append("</div>");
                        builder.Append("<div class=\"FInput\">");
                        builder.Append("<select id=\"valor").Append(i).Append("\"  >");
                        builder.Append("<option value=\"0\" selected=\"selected\">Select Value</option>");  
                        foreach (var it in list)
                        {
                            builder.Append("<option value=" + it.ID + ">" + it.Value + "</option>");
                        }

                        builder.Append("</select>");
                        //builder.Append("<a  href=\"#\" onclick=\"GoURL_Param();return false\" id=\"btnSaveNote\" class=\"Button BigBlue FR\">");
                        builder.Append("<a  href=\"#\" onclick=\"GoURL_Param('").Append(item.Link).Append("', ").Append(i).Append(" );return false\" id=\"btnSaveNote\" class=\"Button BigBlue FR\">");
                        builder.Append("<span>").Append(item.NameButon).Append("</span>").Append("</a>");

                        builder.Append("</div>");
                        builder.Append("<br />");

                        builder.Append("</div>");

                    }
                    else {


                        builder.Append("<div class=\"FRow\">");
                        builder.Append("<div class=\"FLabel\">");
                        builder.Append("<label>").Append(item.NameEtiqueta).Append("</label>");
                        builder.Append("<input type=\"hidden\" id=\"hdnvalor").Append(i).Append("\" value=\"").Append(item.SupportMotivePropertyTypeID).Append("\" />");
                        builder.Append("<input type=\"hidden\" id=\"hdnList_Text").Append(i).Append("\" value=\"Texto\" />");
                        builder.Append("</div>");
                        builder.Append("<div class=\"FInput\">");
                        builder.Append("<input  type=\"text\"  id=\"valor").Append(i).Append("\" />");

                        builder.Append("<a  href=\"#\" onclick=\"GoURL_Param('").Append(item.Link).Append("',").Append(i).Append(" );return false\" id=\"btnSaveNote\" class=\"Button BigBlue FR\">");
                        builder.Append("<span>").Append(item.NameButon).Append("</span>").Append("</a>");

                        builder.Append("</div>");
                        builder.Append("<br />");

                        builder.Append("</div>");

                    }
                    i++;
                    }
                builder.Append("</div>");
            }
            return builder.ToString();
        }

        public virtual ActionResult GetItemsLevels_Motives(string ID)
        {
            try
            {
               
                List<SupportLevelSearchData> lstitemChild = SupportLevels.GetItemsLevelMotives(ID,"0");
                List<SupportLevelSearchData> lstitemMotives = SupportLevels.GetItemsLevelMotives(ID, "1");

                bool existDataChild=false;
                if (lstitemChild != null) existDataChild = true;
                bool existDataMotives = false;
                if (lstitemMotives != null) existDataMotives = true;

                return Json(new { isLast = existDataChild, select = lstitemChild, isMotive = existDataMotives, selecMotives = lstitemMotives });

               
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual string BuildTreeLevel_Motive_Items(int Level_MotiveID)
        {
            var builder = new StringBuilder();
            SupportTicketsBusinessLogic busines = new SupportTicketsBusinessLogic();
            if (Level_MotiveID > 0)
            {

                var ListFormDinamic = AccountReferencesBusinessLogic.GetDinamicFormMotive(Level_MotiveID);
                builder.Append("<div id=\"propertyContainer").Append("\" style=\"margin-left:15px;\">");
                builder.Append("<input id=\"hdnCanProperties\" type=\"hidden\" value=\"").Append(ListFormDinamic.Count()).Append("\" />");
                int i = 0;
                foreach (var item in ListFormDinamic)
                {


                    if (item.DataType == "List")
                    {
                        busines = new SupportTicketsBusinessLogic();
                        int supportMotivePropertyTypeID = int.Parse(item.SupportMotivePropertyTypeID.ToString());
                        var list = busines.GetValuesByPropertyID(supportMotivePropertyTypeID);

                        builder.Append("<div class=\"FRow\">");
                        builder.Append("<div class=\"FLabel\">");
                        builder.Append("<label>").Append(item.NameEtiqueta).Append("</label>");
                        builder.Append("<input type=\"hidden\" id=\"hdnvalor").Append(i).Append("\" value=\"").Append(item.SupportMotivePropertyTypeID).Append("\" />");
                        builder.Append("<input type=\"hidden\" id=\"hdnList_Text").Append(i).Append("\" value=\"List\" />");
                        builder.Append("</div>");
                        builder.Append("<div class=\"FInput\">");
                        builder.Append("<select id=\"valor").Append(i).Append("\"  >");
                        builder.Append("<option value=\"0\" selected=\"selected\">Select Value</option>");
                        foreach (var it in list)
                        {
                            builder.Append("<option value=" + it.ID + ">" + it.Value + "</option>");
                        }

                        builder.Append("</select>");
                        //builder.Append("<a  href=\"#\" onclick=\"GoURL_Param();return false\" id=\"btnSaveNote\" class=\"Button BigBlue FR\">");
                        builder.Append("<a  href=\"#\" onclick=\"GoURL_Param('").Append(item.Link).Append("', ").Append(i).Append(" );return false\" id=\"btnSaveNote\" class=\"Button BigBlue FR\">");
                        builder.Append("<span>").Append(item.NameButon).Append("</span>").Append("</a>");

                        builder.Append("</div>");
                        builder.Append("<br />");

                        builder.Append("</div>");

                    }
                    else
                    {


                        builder.Append("<div class=\"FRow\">");
                        builder.Append("<div class=\"FLabel\">");
                        builder.Append("<label>").Append(item.NameEtiqueta).Append("</label>");
                        builder.Append("<input type=\"hidden\" id=\"hdnvalor").Append(i).Append("\" value=\"").Append(item.SupportMotivePropertyTypeID).Append("\" />");
                        builder.Append("<input type=\"hidden\" id=\"hdnList_Text").Append(i).Append("\" value=\"Texto\" />");
                        builder.Append("</div>");
                        builder.Append("<div class=\"FInput\">");
                        builder.Append("<input  type=\"text\"  id=\"valor").Append(i).Append("\" />");

                        builder.Append("<a  href=\"#\" onclick=\"GoURL_Param('").Append(item.Link).Append("',").Append(i).Append(" );return false\" id=\"btnSaveNote\" class=\"Button BigBlue FR\">");
                        builder.Append("<span>").Append(item.NameButon).Append("</span>").Append("</a>");

                        builder.Append("</div>");
                        builder.Append("<br />");

                        builder.Append("</div>");

                    }
                    i++;
                }
                builder.Append("</div>");
            }
            return builder.ToString();
        }
	}
}
