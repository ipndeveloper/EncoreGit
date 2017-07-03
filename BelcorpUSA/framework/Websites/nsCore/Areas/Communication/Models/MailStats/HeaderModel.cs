using System;
using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Communication.Models.MailStats
{
    public class HeaderModel
    {
        public DateTime? MailingDate { get; set; }
        public MailMessageGroupAddressSearchTotals Totals { get; set; }
    }
}