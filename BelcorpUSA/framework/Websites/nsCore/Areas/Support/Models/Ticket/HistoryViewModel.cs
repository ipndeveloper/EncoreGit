using nsCore.Models;

namespace nsCore.Areas.Support.Models.Ticket
{
    public class HistoryViewModel
    {
        public int SupportTicketID { get; set; }
        public string SupportTicketNumber { get; set; }
        public AuditHistoryGridModel AuditHistoryGridModel { get; set; }
    }
}