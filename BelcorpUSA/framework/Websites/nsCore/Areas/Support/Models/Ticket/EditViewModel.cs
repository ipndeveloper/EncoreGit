using System.Collections.Generic;
using NetSteps.Data.Entities;

namespace nsCore.Areas.Support.Models.Ticket
{
    // TODO: Implement the view model when time permits...
    public class EditViewModel
    {
        public SupportTicket SupportTicket { get; set; }

        public int? OldTicketStatus { get; set; }

        public List<SupportTicketCategory> Categories { get; set; }

        public List<SupportTicketStatus> Statuses { get; set; }

        public List<SupportTicketPriority> Priorities { get; set; }

        public NotesData NotesData { get; set; }
    }

    public class NotesData
    {
        public int ParentIdentity { get; set; }

        public bool ShowPublishNoteToOwner { get; set; }
    }
}