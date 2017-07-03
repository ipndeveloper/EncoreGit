using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Communication.Common;

namespace NetSteps.Communication.Services
{
    public class PromotionAccountAlertProvider : IPromotionAccountAlertProvider
    {
        protected readonly IPromotionAccountAlertService _promotionAccountAlertService;

        public PromotionAccountAlertProvider(
            IPromotionAccountAlertService promotionAccountAlertService)
        {
            Contract.Requires<ArgumentNullException>(promotionAccountAlertService != null);

            _promotionAccountAlertService = promotionAccountAlertService;
        }

        public IList<IPromotionAccountAlert> GetBatch(IEnumerable<int> accountAlertIds)
        {
            return _promotionAccountAlertService.GetBatch(accountAlertIds);
        }

        IList<IAccountAlert> IAccountAlertProvider.GetBatch(IEnumerable<int> accountAlertIds)
        {
            return new List<IAccountAlert>(GetBatch(accountAlertIds));
        }
    }
}
