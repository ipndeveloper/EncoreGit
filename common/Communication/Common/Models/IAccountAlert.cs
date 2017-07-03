using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Communication.Common
{
    public interface IAccountAlert
    {
        int AccountAlertId { get; set; }
        int AccountId { get; set; }
        DateTime CreatedDateUtc { get; set; }
        DateTime? ExpirationDateUtc { get; set; }
        DateTime? DismissedDateUtc { get; set; }
        Guid ProviderKey { get; }
        int AccountAlertDisplayKindId { get; set; }
    }
}
