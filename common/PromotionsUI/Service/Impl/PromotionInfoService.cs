using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using NetSteps.Communication.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Common.Models;
using NetSteps.Promotions.Common;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.Qualifications;
using NetSteps.Promotions.UI.Common.Interfaces;

namespace NetSteps.Promotions.UI.Service.Impl
{
    public class PromotionInfoService : IPromotionInfoService
    {
        #region Constants
        //public static class ConstantsAlert
        //{
        //    public const string AlertTitleText = "AlertTitleText";
        //    public const string AlertMessageText = "AlertMessageText";
        //}

        //public static class ConstantsPromotion
        //{
        //    public const string PromotionDescription = "PromotionDescription";
        //    public const string PromotionExpired = "PromotionExpired";
        //    public const string PromotionTitle = "PromotionTitle";
        //    public const string PromotionAction = "PromotionAction";
        //}
        #endregion

        #region Fields
        private readonly IPromotionService _promotionService;
        private readonly IPromotionContentService _promotionContentService;
        #endregion

        #region Constructor
        public PromotionInfoService(
            IPromotionService promotionService,
            IPromotionContentService promotionContentService)
        {
            Contract.Requires<ArgumentNullException>(promotionService != null);
            Contract.Requires<ArgumentNullException>(promotionContentService != null);

            _promotionService = promotionService;
            _promotionContentService = promotionContentService;
        }
        #endregion

        #region Methods
        public virtual IEnumerable<T> GetAvailablePromotionsForAccount<T>(Predicate<T> filter)
            where T : IPromotion
        {
            return _promotionService.GetPromotions(filter);
        }

        public virtual IEnumerable<IDisplayInfo> GetPromotionDisplayInfo(IEnumerable<IPromotion> promotionsValidFor, ILocalizationInfo localizationInfo)
        {
            foreach (var promotion in promotionsValidFor)
            {
                var display = Create.New<IDisplayInfo>();
                SetDisplayValues(promotion, display);
                display.PromotionId = promotion.PromotionID.ToString();
                display.FormatProvider = CultureInfo.GetCultureInfo(localizationInfo.CultureName);

                var promotionContent = _promotionContentService.FirstOrDefault(
                    promotion.PromotionID,
                    localizationInfo.LanguageId
                );

                SetDisplayValuesFromProviderPromo(display, promotionContent);

                yield return display;
            }
        }

        public virtual IEnumerable<IAlertInfo> GetPromotionsAccountAlerts(IEnumerable<IPromotionAccountAlert> allAlerts, ILocalizationInfo localizationInfo)
        {
            foreach (var alert in allAlerts)
            {
                var promotionContent = _promotionContentService.FirstOrDefault(
                    alert.PromotionId,
                    localizationInfo.LanguageId
                );

                var info = Create.New<IAlertInfo>();
                info.AccountAlertId = alert.AccountAlertId;
                info.PromotionId = alert.PromotionId.ToString();
                info.PartialName = "_PromotionAccountAlertModal";
                info.FormatProvider = CultureInfo.GetCultureInfo(localizationInfo.CultureName);
                SetDisplayValuesFromProviderAlert(info, promotionContent);
                SetDisplayValues(_promotionService.GetPromotion(alert.PromotionId), info);
                SetDisplayValuesFromProviderPromo(info, promotionContent);
                yield return info;
            }
        }

        public virtual void SetDisplayValues(IPromotion promotion, IDisplayInfo info)
        {
            if (promotion == null) return;
            info.Description = promotion.Description;
            info.ExpiredDate = promotion.EndDate;
            info.CouponCode = FetchCode(promotion);
        }

        public virtual void SetDisplayValuesFromProviderPromo(IDisplayInfo info, IPromotionContent promotionContent)
        {
            if (promotionContent == null) return;
            info.Title = promotionContent.Title;
            info.Description = promotionContent.Description;
            info.ActionText = promotionContent.ActionText;
            info.ImagePaths = new[] { promotionContent.ImagePath };
        }

        public virtual void SetDisplayValuesFromProviderAlert(IAlertInfo info, IPromotionContent promotionContent)
        {
            if (promotionContent == null) return;
            info.PartialTitle = promotionContent.AlertTitle;
        }

        public virtual string FetchCode(IPromotion promotion)
        {
            var qualifications = promotion.PromotionQualifications;

            if (qualifications != null && qualifications.Any())
            {
                foreach (var qualification in qualifications)
                {
                    var q = qualification.Value as IPromotionCodeQualificationExtension;
                    if (q != null)
                    {
                        return q.PromotionCode;
                    }
                }
            }
            return string.Empty;
        }
        #endregion
    }
}
