using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Communication.Common
{
    public interface IPromotionAccountAlert : IAccountAlert
    {
        int PromotionId { get; set; }
    }
}
