using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace NetSteps.Communication.Services.Entities
{
    [Table("AccountAlerts", Schema = "Communication")]
    public class AccountAlert
    {
        public int AccountAlertId { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? ExpirationDateUtc { get; set; }
        public DateTime? DismissedDateUtc { get; set; }
        public Guid ProviderKey { get; set; }
        public int AccountAlertDisplayKindId { get; set; }
    }
}
