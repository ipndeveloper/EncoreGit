using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;
using NetSteps.Foundation.Common;

namespace NetSteps.Communication.UI.Common
{
    [DTO]
    public interface IAccountAlertUISearchParameters : IPaginationParameters, IOrderByParameters
    {
        int? AccountId { get; set; }
    }
}
