using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Promotions.UI.Common.Interfaces
{
    [ContractClass(typeof(Contracts.PromotionContentServiceContracts))]
    public interface IPromotionContentService
    {
        IPromotionContent FirstOrDefault(int promotionID, int languageID);
        void Add(IPromotionContent model);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPromotionContentService))]
        internal abstract class PromotionContentServiceContracts : IPromotionContentService
        {
            IPromotionContent IPromotionContentService.FirstOrDefault(int promotionID, int languageID)
            {
                Contract.Requires<ArgumentOutOfRangeException>(promotionID > 0);
                Contract.Requires<ArgumentOutOfRangeException>(languageID > 0);
                throw new NotImplementedException();
            }

            void IPromotionContentService.Add(IPromotionContent model)
            {
                Contract.Requires<ArgumentNullException>(model != null);
            }
        }
    }
}
