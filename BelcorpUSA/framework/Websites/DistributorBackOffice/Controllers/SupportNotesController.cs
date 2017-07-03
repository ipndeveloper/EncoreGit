using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;

namespace DistributorBackOffice.Controllers
{
    public class SupportNotesController : BaseController
    {
        [FunctionFilter("Support-Add Note", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual JsonResult AddNewNote(string subject, string noteText, int? parentID, string supportTicketNumber)
        {
            try
            {
                if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(noteText))
                    return Json(new { result = false, message = Translation.GetTerm("NoteNotSavedNoSubjectOrTextCouldBeFoundForThisNote", "Note not saved.") });

                Note newNote = new Note();
                newNote.StartEntityTracking();

                newNote.DateCreated = DateTime.Now;
                newNote.NoteTypeID = NetSteps.Data.Entities.Constants.NoteType.SupportTicketNotes.ToInt();
                newNote.UserID = CurrentAccount.UserID ?? null;
                newNote.Subject = subject.ToCleanString();
                newNote.NoteText = noteText.ToCleanString();
                newNote.IsInternal = false;

                // Check to see if this is a follow-up (child) note
                // If it is, assign this as a child note
                if (parentID.HasValue && parentID.Value > 0)
                    newNote.ParentID = parentID;

                SupportTicket ticket = SupportTicket.LoadBySupportTicketNumberFull(supportTicketNumber);
                ticket.StartEntityTracking();
                ticket.Notes.Add(newNote);
                ticket.Save();
                ticket.BuildReadOnlyNotesTree();

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

        [FunctionFilter("Support-Notes", "~/", NetSteps.Data.Entities.Constants.SiteType.BackOffice)]
        public virtual ActionResult Get(string id)
        {
            try
            {
                List<Note> notes = Note.LoadBySupportTicketID(Convert.ToInt32(id)).Where(x => x.IsInternal == false).ToList();

                if (notes.Count == 0)
                {
                    return Json(new { result = true, notes = string.Format("<div style=\"margin-left:10px;\">{0}</div>", Translation.GetTerm("NoNotesPosted", "No notes posted.")) });
                }
                return Json(new { result = true, notes = Build(notes.Where(n => !n.ParentID.HasValue), notes, false) });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }

        protected virtual string Build(IEnumerable<Note> notes, IEnumerable<Note> allNotes, bool isChild)
        {
            if (notes == null)
                return null;

            StringBuilder builder = new StringBuilder();
            notes.Each((n, i) =>
            {
                var childNotes = allNotes.Where(note => note.ParentID == n.NoteID);
                if (!isChild)
                {
                    builder.Append("<div class=\"AcctNote").Append(i % 2 == 0 ? "" : " Alt").Append("\">")
                        .Append("<span class=\"FL NoteTitle\">")
                            .Append("<b>").Append(n.Subject).Append(" (#").Append(n.NoteID).Append(")</b> ")
                        .Append("</span>")
                       .Append("<span class=\"FR ExpandNote\"><a style=\"cursor: pointer\" onclick=\"createNewFollowup(").Append(n.NoteID).Append(");\" >").Append("").Append("</a>");

                    if (childNotes.Count() > 0)
                    {
                        builder.Append("  ").Append("<a class=\"toggleChildNotes\" style=\"cursor: pointer\">").Append(Translation.GetTerm("Collapse")).Append("</a>");
                    }
                    builder.Append("</span>")
                        .Append("<span class='ClearAll'></span>")
                        .Append("<span class=\"NoteAuthor\">").Append(Translation.GetTerm("PostedOn", "Posted on")).Append(": ").Append(n.DateCreated.ToShortDateString()).Append("<br />")
                        .Append(Translation.GetTerm("PostedBy", "Posted by")).Append(": ").Append(n.UserInfo == null ? Translation.GetTerm("Unknown") : n.UserInfo.Username).Append("<br />")
                        .Append(n.NoteText);

                    if (childNotes.Count() > 0)
                    {
                        builder.Append("<div class=\"ChildNotes\">").Append(Build(childNotes, allNotes, true)).Append("</div>");
                    }
                    builder.Append("</div>");
                }
                else
                {
                    builder.Append("<div class=\"NoteReply\"><b>").Append(n.Subject).Append("</b> (").Append(childNotes.Count()).Append(" ").Append(Translation.GetTerm("Follow-up", "Follow-up")).Append(") ")
                        .Append("<span class=\"FR ExpandNote\">")
                        .Append("</span>")
                        .Append("<span class=\"ClearAll\"></span>")
                        .Append("<span class=\"NoteAuthor\">")
                            .Append(Translation.GetTerm("PostedOn", "Posted on")).Append(": ").Append(n.DateCreated.ToShortDateString()).Append("<br />")
                            .Append(Translation.GetTerm("PostedBy", "Posted by")).Append(": ").Append(n.UserInfo == null ? Translation.GetTerm("Unknown") : n.UserInfo.Username)
                            .Append("<br />")
                        .Append("</span>").Append(n.NoteText);

                    if (childNotes.Count() > 0)
                    {
                        builder.Append("<div class=\"ChildNotes\">").Append(Build(childNotes, allNotes, true)).Append("</div>");
                    }
                    builder.Append("</div>");
                }
            });
            return builder.ToString();
        }
    }
}
