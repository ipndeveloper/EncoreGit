using NetSteps.Data.Entities;

namespace DistributorBackOffice.Models.SupportTicketModels
{
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
    }
}