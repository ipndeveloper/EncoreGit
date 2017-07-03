using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NetSteps.Communication.Common;
using NetSteps.Communication.UI.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Common.Models;
using NetSteps.Promotions.UI.Common.Interfaces;

namespace NetSteps.Promotions.UI.Service.Impl
{
    public class PromotionAccountAlertUIProvider : IPromotionAccountAlertUIProvider
    {
        protected readonly IPromotionAccountAlertService _promotionAccountAlertService;
        protected readonly IPromotionInfoService _promotionInfoService;

        public PromotionAccountAlertUIProvider(
            IPromotionAccountAlertService promotionAccountAlertService,
            IPromotionInfoService promotionInfoService)
        {
            Contract.Requires<ArgumentNullException>(promotionAccountAlertService != null);
            Contract.Requires<ArgumentNullException>(promotionInfoService != null);

            _promotionAccountAlertService = promotionAccountAlertService;
            _promotionInfoService = promotionInfoService;
        }

        public IEnumerable<IAccountAlertMessageModel> GetMessages(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo)
        {
            return
                _promotionInfoService.GetPromotionsAccountAlerts(
                    _promotionAccountAlertService.GetBatch(accountAlertIds),
                    localizationInfo
                )
                .Select(x =>
                {
                    var model = Create.New<IAccountAlertMessageModel>();
                    model.AccountAlertId = x.AccountAlertId;
                    model.Message = x.Title;
                    model.IsDismissable = true;
                    return model;
                });
        }

        public IEnumerable<IAccountAlertModalModel> GetModals(IEnumerable<int> accountAlertIds, ILocalizationInfo localizationInfo)
        {
            return _promotionInfoService.GetPromotionsAccountAlerts(
                _promotionAccountAlertService.GetBatch(accountAlertIds),
                localizationInfo
            );
        }

        public void Dismiss(int accountAlertId, int accountId)
        {
            _promotionAccountAlertService.Dismiss(accountAlertId, accountId);
        }
    }
}
