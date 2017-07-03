
namespace DistributorBackOffice.Models.SupportTicketModels
{
    public class HistoryViewModel
    {
        public int SupportTicketID { get; set; }
        public string SupportTicketNumber { get; set; }
        public AuditHistoryGridModel AuditHistoryGridModel { get; set; }
    }
}