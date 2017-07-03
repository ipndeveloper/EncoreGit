using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using NetSteps.Communication.Common;
using NetSteps.Common.Models;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Promotions.UI.Common.Interfaces
{
    [ContractClass(typeof(Contracts.PromotionInfoServiceContracts))]
    public interface IPromotionInfoService
    {
        IEnumerable<T> GetAvailablePromotionsForAccount<T>(Predicate<T> filter) where T : IPromotion;

        /// <summary>
        /// Gets display info for a list of promotions.
        /// </summary>
        /// <param name="promotionsValidFor"></param>
        /// <returns></returns>
        IEnumerable<IDisplayInfo> GetPromotionDisplayInfo(IEnumerable<IPromotion> promotionsValidFor, ILocalizationInfo localizationInfo);

        /// <summary>
        /// Will return the basic information for implementing model for displaying promotion Alerts for accounts.
        /// </summary>
        /// <param name="allAlerts"> </param>
        /// <returns></returns>
        IEnumerable<IAlertInfo> GetPromotionsAccountAlerts(IEnumerable<IPromotionAccountAlert> allAlerts, ILocalizationInfo localizationInfo);
    }

    namespace Contracts
    {
        [ContractClassFor(typeof(IPromotionInfoService))]
        internal abstract class PromotionInfoServiceContracts : IPromotionInfoService
        {
            IEnumerable<T> IPromotionInfoService.GetAvailablePromotionsForAccount<T>(Predicate<T> filter)
            {
                Contract.Requires<ArgumentNullException>(filter != null);
                throw new NotImplementedException();
            }

            IEnumerable<IDisplayInfo> IPromotionInfoService.GetPromotionDisplayInfo(IEnumerable<IPromotion> promotionsValidFor, ILocalizationInfo localizationInfo)
            {
                Contract.Requires<ArgumentNullException>(promotionsValidFor != null);
                Contract.Requires<ArgumentNullException>(localizationInfo != null);
                throw new NotImplementedException();
            }

            IEnumerable<IAlertInfo> IPromotionInfoService.GetPromotionsAccountAlerts(IEnumerable<IPromotionAccountAlert> allAlerts, ILocalizationInfo localizationInfo)
            {
                Contract.Requires<ArgumentNullException>(allAlerts != null);
                Contract.Requires<ArgumentNullException>(localizationInfo != null);
                throw new NotImplementedException();
            }
        }
    }
}
