using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.Common
{
    [DTO]
    public interface IAccountAlertSearchParameters : IPaginationParameters, IOrderByParameters
    {
        IEnumerable<int> AccountAlertIds { get; set; }
        int? AccountId { get; set; }
        IEnumerable<Guid> ProviderKeys { get; set; }
        IEnumerable<int> AccountAlertDisplayKindIds { get; set; }
    }
}
