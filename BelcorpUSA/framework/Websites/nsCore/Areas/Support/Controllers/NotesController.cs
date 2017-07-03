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
using NetSteps.Web.Mvc.Helpers;
using nsCore.Controllers;

namespace nsCore.Areas.Support.Controllers
{
    public class NotesController : BaseController
    {
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Add(string parentEntityID, int? parentId, string subject, string noteText, bool? isNotInternal)
        {
            try
            {
                if (string.IsNullOrEmpty(subject) && string.IsNullOrEmpty(noteText))
                    return Json(new { result = false, message = Translation.GetTerm("NoteNotSavedNoSubjectOrTextCouldBeFoundForThisNote", "Note not saved.  No subject or text could be found for this note.") });

                Note newNote = new Note();
                newNote.StartEntityTracking();

                newNote.DateCreated = DateTime.Now;
                newNote.NoteTypeID = NetSteps.Data.Entities.Constants.NoteType.SupportTicketNotes.ToInt();
                newNote.UserID = ApplicationContext.Instance.CurrentUser.UserID;
                newNote.Subject = subject.ToCleanString();
                newNote.NoteText = noteText.ToCleanString();

                if (parentId.HasValue && parentId.Value > 0)
                    newNote.ParentID = parentId;

                if (isNotInternal.HasValue)
                    newNote.IsInternal = !isNotInternal.Value; // oposite value so if box is checked then we store 0 in the db field

                //make sure to save to the correct ticket if they have multiple tabs open, since the async post might be from a stale tab. 
                SupportTicket ticket = CoreContext.CurrentSupportTicket.SupportTicketID == parentEntityID.ToInt() ? CoreContext.CurrentSupportTicket : SupportTicket.Load(parentEntityID.ToInt());
                ticket.StartEntityTracking();
                ticket.Notes.Add(newNote);
                ticket.Save();
                ticket.BuildReadOnlyNotesTree();
                CoreContext.CurrentSupportTicket = ticket;

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
        [FunctionFilter("Support", "~/Accounts")]
        public virtual ActionResult Get(string id, DateTime startDate, DateTime endDate, string searchCriteria)
        {
            try
            {
                List<Note> notes = Note.LoadBySupportTicketID(Convert.ToInt32(id));
                List<Note> filteredList = Note.FilterNotes(startDate, endDate, searchCriteria, notes);

                if (filteredList.Count == 0)
                {
                    return Json(new { result = true, notes = string.Format("<div style=\"margin-left:10px;\">{0}</div>", string.IsNullOrEmpty(searchCriteria.Trim()) ? Translation.GetTerm("NoNotesPosted", "No notes posted.") : Translation.GetTerm("NoNotesFound.", "No notes found.")) });
                }

                // allow searching for child notes as well
                return Json(new
                {
                    result = true,
                    notes = filteredList.Any(n => !n.ParentID.HasValue) ? Build(filteredList.Where(n => !n.ParentID.HasValue), filteredList, false)
                                                   : Build(filteredList, filteredList, false)
                });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [FunctionFilter("Support", "~/Accounts")]
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
                            .Append("(").Append(childNotes.Count()).Append(" ").Append(Translation.GetTerm("Follow-up(s)", "Follow-up(s)")).Append(")")
                        .Append("</span>")
                        .Append("<span class=\"FR ExpandNote\"><a style=\"cursor: pointer\" onclick=\"createNewFollowup(").Append(n.NoteID).Append(");\" >").Append(Translation.GetTerm("Follow-up", "Follow-up")).Append("</a>");
                    if (childNotes.Count() > 0)
                    {
                        builder.Append(" | <a class=\"toggleChildNotes\" style=\"cursor: pointer\">").Append(Translation.GetTerm("Collapse")).Append("</a>");
                    }
                    builder.Append("</span>")
                        .Append("<span class='ClearAll'></span>")
                        .Append("<span class=\"NoteAuthor\">").Append(Translation.GetTerm("PostedOn", "Posted on")).Append(": ").Append(n.DateCreated.ToShortDateString()).Append("<br />")
                        .Append(Translation.GetTerm("PostedBy", "Posted by")).Append(": ").Append(n.UserInfo == null ? Translation.GetTerm("Unknown") : n.UserInfo.Username).Append("<br />")
                        .Append(Translation.GetTerm("PublishNoteToOwner", "Publish to owner (consultant)")).Append(": ").Append(n.IsInternal ? "No" : "Yes").Append("</span>").Append(n.NoteText);

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
                            .Append("<a style=\"cursor: pointer\" onclick=\"createNewFollowup(").Append(n.NoteID).Append(");\" >").Append(Translation.GetTerm("PostFollow-up", "Post Follow-up")).Append("</a>")
                        .Append("</span>")
                        .Append("<span class=\"ClearAll\"></span>")
                        .Append("<span class=\"NoteAuthor\">")
                            .Append(Translation.GetTerm("PostedOn", "Posted on")).Append(": ").Append(n.DateCreated.ToShortDateString()).Append("<br />")
                            .Append(Translation.GetTerm("PostedBy", "Posted by")).Append(": ").Append(n.UserInfo == null ? Translation.GetTerm("Unknown") : n.UserInfo.Username)
                            .Append("<br />")
                            .Append(Translation.GetTerm("PublishNoteToOwner", "Publish to owner (consultant)")).Append(": ").Append(n.IsInternal ? "No" : "Yes")
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
