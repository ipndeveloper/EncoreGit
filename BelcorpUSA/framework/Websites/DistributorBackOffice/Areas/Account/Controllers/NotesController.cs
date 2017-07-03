using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using DistributorBackOffice.Areas.Account.Models.Notes;
using DistributorBackOffice.Controllers;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Extensions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.Areas.Account.Controllers
{
	public class NotesController : BaseController
	{
		[HttpPost]
		[FunctionFilter("Accounts-Add Notes", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult Add(int accountID, int? parentId, string subject, string noteText)
		{
			try
			{
				if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(noteText))
					return Json(new { result = false, message = Translation.GetTerm("NoteNotSavedNoSubjectOrTextCouldBeFoundForThisNote", "Note not saved.  No subject or text could be found for this note.") });

				Note newNote = new Note();
				newNote.StartEntityTracking();

				newNote.DateCreated = DateTime.Now;
				newNote.NoteTypeID = NetSteps.Data.Entities.Constants.NoteType.CRMNotes.ToInt();
				newNote.UserID = CurrentAccount.UserID;
				newNote.Subject = subject.ToCleanString();
				newNote.NoteText = noteText.ToCleanString();

				if (parentId.HasValue && parentId.Value > 0)
					newNote.ParentID = parentId;

				NetSteps.Data.Entities.Account account = NetSteps.Data.Entities.Account.LoadFull(accountID);
				account.StartEntityTracking();
				account.Notes.Add(newNote);
				account.Save();
				account.BuildReadOnlyNotesTree();

				if (newNote.NoteID > 0)
					return Json(new { result = true });
				else
					return Json(new { result = false, message = Translation.GetTerm("NoteNotSaved", "Note not saved") });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Accounts-Notes", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult Get(string id, DateTime startDate, DateTime endDate, string searchCriteria)
		{
			try
			{
				List<Note> notes = Note.LoadByAccountNumber(id);
				List<Note> filteredList = Note.FilterNotes(startDate, endDate, searchCriteria, notes);
				filteredList.RemoveWhere(n => n.ParentID != null);

				if (filteredList.Count == 0)
				{
					return Json(new { result = true, notes = string.Format("<div style=\"margin-left:10px;\">{0}</div>", string.IsNullOrEmpty(searchCriteria.Trim()) ? Translation.GetTerm("NoNotesPosted", "No notes posted.") : Translation.GetTerm("NoNotesFound.", "No notes found.")) });
				}
				return Json(new { result = true, notes = Build(filteredList, new List<Note>(), false) });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[OutputCache(CacheProfile = "PagedGridData")]
		[FunctionFilter("Accounts-Notes", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetForGrid(int page, int pageSize, int? accountID, string searchText, DateTime? startDate, DateTime? endDate, string orderBy, NetSteps.Common.Constants.SortDirection orderByDirection)
		{
			try
			{
				if (startDate.HasValue && startDate.Value.Year < 1900)
					startDate = null;
				if (endDate.HasValue && endDate.Value.Year < 1900)
					endDate = null;

				var notes = Note.Search(new NetSteps.Data.Entities.Business.NoteSearchParameters()
				{
					PageIndex = page,
					PageSize = pageSize,
					NoteTypeID = NetSteps.Data.Entities.Constants.NoteType.CRMNotes.ToInt(),
					StartDate = startDate.StartOfDay(),
					EndDate = endDate.EndOfDay(),
					AccountID = accountID,
					UserID = CurrentAccount.UserID,
					SearchText = searchText.ToCleanStringNullable(),
					OrderBy = orderBy,
					OrderByDirection = orderByDirection
				});
				if (notes.Count > 0)
				{
					StringBuilder builder = new StringBuilder();
					foreach (NoteSearchData note in notes)
					{
						builder.Append(@"<tr class=""notes"" data-id=" + @note.NoteID + ">")
							//.AppendLinkCell("~/Orders/Details/Index/" + order.OrderNumber, order.OrderNumber)
							.AppendCell(note.DateCreated.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo))
							.AppendCell(note.Subject)
							.AppendCell(note.NoteText.Truncate(50, true))
							.Append("</tr>");
					}
					return Json(new { totalPages = notes.TotalPages, page = builder.ToString() });
				}

				return Json(new { totalPages = 0, page = "<tr><td colspan=\"9\">" + Translation.GetTerm("ThereWereNoRecordsFoundThatMeetThatCriteriaPleaseTryAgain", "There were no records found that meet that criteria. Please try again.") + "</td></tr>" });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		[FunctionFilter("Accounts-Notes", "~/Account", Constants.SiteType.BackOffice)]
		public virtual ActionResult GetSpecifiedNote(int noteID = 0, bool disabled = false, int accountID = 0)
		{
			try
			{
				Note note = noteID != 0 ? Note.Load(noteID) : new Note();


				return PartialView("Note", new AccountNoteViewModel { Note = note, isDisabled = disabled, AccountID = accountID });
			}
			catch (Exception ex)
			{
				var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, accountID: (CurrentAccount != null) ? CurrentAccount.AccountID.ToIntNullable() : null);
				return Json(new { result = false, message = exception.PublicMessage });
			}
		}

		public virtual string Build(IEnumerable<Note> notes, IList<Note> processedNotes, bool isChild)
		{
			if (notes == null)
				return null;

			StringBuilder builder = new StringBuilder();
			notes.Each((n, i) =>
			{
				if (!processedNotes.Contains(n))
				{
					if (!isChild)
					{
						builder.Append("<div class=\"AcctNote").Append(i % 2 == 0 ? "" : " Alt").Append("\">")
							.Append("<span class=\"FL NoteTitle\">")
								.Append("<b>").Append(n.Subject).Append(" (#").Append(n.NoteID).Append(")</b> ")
								.Append("(").Append(n.FollowupNotes.CountSafe()).Append(" " + Translation.GetTerm("Follow-up(s)", "Follow-up(s)") + ")")
							.Append("</span>")
							.Append("<span class=\"FR ExpandNote\"><a style=\"cursor: pointer\" onclick=\"createNewFollowup(").Append(n.NoteID).Append(");\" >" + Translation.GetTerm("Follow-up", "Follow-up") + "</a>");
						if (n.FollowupNotes.CountSafe() > 0)
						{
							builder.Append(" | <a class=\"toggleChildNotes\" style=\"cursor: pointer\">" + Translation.GetTerm("Collapse") + "</a>");
						}
						builder.Append("</span>")
							.Append("<span class='ClearAll'></span>")
							.Append("<span class=\"NoteAuthor\">" + Translation.GetTerm("PostedOn", "Posted on") + ": ").Append(n.DateCreated.ToShortDateString()).Append("<br />")
							.Append("" + Translation.GetTerm("PostedBy", "Posted by") + ": ").Append(n.UserInfo == null ? Translation.GetTerm("Unknown") : n.UserInfo.Username).Append("</span>").Append(n.NoteText);

						processedNotes.Add(n);
						if (n.FollowupNotes.CountSafe() > 0)
						{
							builder.Append("<div class=\"ChildNotes\">").Append(Build(n.FollowupNotes, processedNotes, true)).Append("</div>");
						}
						builder.Append("</div>");
					}
					else
					{
						builder.Append("<div class=\"NoteReply\"><b>").Append(n.Subject).Append("</b> (").Append(n.FollowupNotes.CountSafe()).Append(" " + Translation.GetTerm("Follow-up", "Follow-up") + ") ")
							.Append("<span class=\"FR ExpandNote\">")
								.Append("<a style=\"cursor: pointer\" onclick=\"createNewFollowup(").Append(n.NoteID).Append(");\" >" + Translation.GetTerm("PostFollow-up", "Post Follow-up") + "</a>")
							.Append("</span>")
							.Append("<span class=\"ClearAll\"></span>")
							.Append("<span class=\"NoteAuthor\">")
								.Append("" + Translation.GetTerm("PostedOn", "Posted on") + ":").Append(n.DateCreated.ToShortDateString()).Append("<br />")
								.Append("" + Translation.GetTerm("PostedBy", "Posted by") + ": ").Append(n.UserInfo == null ? Translation.GetTerm("Unknown") : n.UserInfo.Username)
							.Append("</span>").Append(n.NoteText);

						processedNotes.Add(n);
						if (n.FollowupNotes.CountSafe() > 0)
						{
							builder.Append("<div class=\"ChildNotes\">").Append(Build(n.FollowupNotes, processedNotes, true)).Append("</div>");
						}
						builder.Append("</div>");
					}
				}
			});
			return builder.ToString();
		}
	}
}
