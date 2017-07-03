using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using nsCore.Areas.Support.Models.Ticket;
using nsCore.Models;

namespace nsCore.Areas.Support.Infrastructure
{
    /// <summary>
    /// Extension methods for ticket view models.
    /// </summary>
    public static class TicketExtensions
    {
        #region HistoryViewModel
        public static void LoadResources(this HistoryViewModel model, SupportTicket supportTicket)
        {
            model.SupportTicketID = supportTicket.SupportTicketID;
            model.SupportTicketNumber = supportTicket.SupportTicketNumber;
            model.AuditHistoryGridModel = new AuditHistoryGridModel
            {
                EntityName = "SupportTicket",
                EntityId = supportTicket.SupportTicketID
            };
        }
        #endregion


        // TODO: Implement this view model when time permits.
        #region EditViewModel
        public static void LoadResources(this EditViewModel model, SupportTicket supportTicket, bool publishNoteToOwner = true)
        {
            model.SupportTicket = supportTicket;
            model.Statuses = SmallCollectionCache.Instance.SupportTicketStatuses.OrderBy(p => p.SortIndex).Where(a => a.Active).ToList();
            model.Categories = SmallCollectionCache.Instance.SupportTicketCategories.OrderBy(s => s.SortIndex).Where(a => a.Active).ToList();
            model.Priorities = SmallCollectionCache.Instance.SupportTicketPriorities.OrderBy(p => p.SortIndex).Where(a => a.Active).ToList();
            model.NotesData = new NotesData
            {
                ParentIdentity = supportTicket.SupportTicketID,
                ShowPublishNoteToOwner = publishNoteToOwner
            };
        }
        #endregion
    }
}