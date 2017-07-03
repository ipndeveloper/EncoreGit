using System;

namespace DistributorBackOffice.Models.SupportTicketModels
{
    public class TicketHistoryModel
    {
        public DateTime DateChanged { get; set; }

        public string ColumnName { get; set; }

        public string OldValue { get; set; }

        public string NewValue { get; set; }
    }
}