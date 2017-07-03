using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Communication.Common;

namespace NetSteps.Communication.Services.Models
{
    public abstract class AccountAlertBase : IAccountAlert
    {
        public int AccountAlertId { get; set; }
        public int AccountId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? DismissedDateUtc { get; set; }
        public DateTime? ExpirationDateUtc { get; set; }
        public Guid ProviderKey { get; protected set; }
        public int AccountAlertDisplayKindId { get; set; }
    }
}
