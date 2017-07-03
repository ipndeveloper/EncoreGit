using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Promotions.Plugins.Common.PromotionKinds.Base;
using nsCore.Areas.Products.Models.Promotions.Interfaces;

namespace nsCore.Areas.Products.Models.Promotions.Converters.Base
{
    public class StandardQualificationPromotionConversion<TPromotion, TModel>
        where TPromotion : IStandardQualificationPromotion
        where TModel : IStandardQualificationPromotionModel
    {

        #region ToModel

        public virtual TModel Convert(TPromotion promotion)
        {
            var model = Create.New<TModel>();
            model.PromotionID = promotion.PromotionID;
            SetName(model, promotion.Description);
            SetCouponCode(model, promotion.PromotionCode);
            //INI - GR_Encore-07
            SetContinuity(model, promotion.Continuity);
            SetNewBAs(model, promotion.AccountConsistencyStatusIDs);
            SetActivityStatusIDs(model, promotion.ActivityStatusIDs);
            //FIN - GR_Encore-07
            SetAccountTypeIDs(model, promotion.AccountTypeIDs);
            SetDates(model, promotion.StartDate, promotion.EndDate);
            SetOneTimeUse(model, promotion.OneTimeUse);
            SetOrderTypeIDs(model, promotion.OrderTypes);
            SetPaidAsTitles(model, promotion.AccountTitles.Where(title => title.TitleTypeID == (int)Constants.TitleType.PaidAS));
            SetRecognizedTitles(model, promotion.AccountTitles.Where(title => title.TitleTypeID == (int)Constants.TitleType.Recognition));
            SetAccountIDs(model, promotion.AccountIDs);
            return model;
        }

        #region Helpers



        internal void SetName(TModel model, string promotionDescription)
        {
            model.Name = promotionDescription;
        }

        internal void SetCouponCode(TModel model, string promotionCode)
        {
            model.CouponCode = promotionCode;
            model.HasCouponCode = !String.IsNullOrEmpty(promotionCode);
        }

        //INI- GR_Encore-07
        internal void SetContinuity(TModel model, bool continuity)
        {
            model.HasContinuity = continuity;
        }

        internal void SetNewBAs(TModel model, IEnumerable<short> newBAIDs)
        {
            model.NewBAStatusIDs = newBAIDs.Select(x => (int)x).ToList();
            model.HasBAStatusIDs = newBAIDs.Count() > 0;
        }

        internal void SetActivityStatusIDs(TModel model, IEnumerable<short> activityStatusIDs)
        {
            model.ActivityStatusIDs = activityStatusIDs.ToList();
            model.HasActivityStatusIDs = activityStatusIDs.Count() > 0;
        }
        //FIN - GR_Encore-07

        internal void SetAccountTypeIDs(TModel model, IEnumerable<short> accountTypeIDs)
        {
            model.AccountTypeIDs = accountTypeIDs.Select(x => (int)x).ToList();
            model.HasAccountTypes = model.HasAccountTypes || accountTypeIDs.Count() > 0;
        }

        internal void SetDates(TModel model, DateTime? startDate, DateTime? endDate)
        {
            model.StartDate = startDate;
            model.EndDate = endDate;
            model.HasDates = startDate.HasValue || endDate.HasValue;
        }

        internal void SetOneTimeUse(TModel model, bool oneTimeUse)
        {
            model.OneTimeUse = oneTimeUse;
        }

        internal void SetOrderTypeIDs(TModel model, IEnumerable<int> orderTypeIDs)
        {
            model.OrderTypeIDs = orderTypeIDs.ToList();
            model.HasOrderTypeIDs = orderTypeIDs.Count() > 0;
        }

        internal void SetPaidAsTitles(TModel model, IEnumerable<NetSteps.Promotions.Plugins.Common.Qualifications.IAccountTitleOption> paidAsTitles)
        {
            model.PaidAsTitleIDs = paidAsTitles.Select(option => option.TitleID).ToList();
            model.HasAccountTypes = model.HasAccountTypes || paidAsTitles.Count() > 0;
        }

        internal void SetRecognizedTitles(TModel model, IEnumerable<NetSteps.Promotions.Plugins.Common.Qualifications.IAccountTitleOption> recognizedTitles)
        {
            model.RecognizedTitleIDs = recognizedTitles.Select(option => option.TitleID).ToList();
            model.HasAccountTypes = model.HasAccountTypes || recognizedTitles.Count() > 0;
        }

        private void SetAccountIDs(TModel model, IEnumerable<int> accountIDs)
        {
            foreach (var newType in accountIDs)
                model.AccountIDs.Add(newType);
            model.HasAccountIDs = model.AccountIDs.Count > 0;
        }

        #endregion

        #endregion

        #region ToIPromotion

        public virtual TPromotion Convert(TModel model)
        {
            var promotion = Create.New<TPromotion>();
            promotion.PromotionStatusTypeID = (int)PromotionStatus.Enabled;
            promotion.PromotionID = model.PromotionID;
            SetDescription(promotion, model.Name);
            SetPromotionCode(promotion, model.HasCouponCode, model.CouponCode);
            //INI - GR_Encore-07
            SetContinuity(promotion, model.HasContinuity);
            SetNewBAs(promotion, model.HasBAStatusIDs, model.NewBAStatusIDs);
            SetActivityStatusIDs(promotion, model.HasActivityStatusIDs, model.ActivityStatusIDs);
            //FIN - GR_Encore-07
            SetAccountTypeIDs(promotion, model.HasAccountTypes, model.AccountTypeIDs);
            SetDates(promotion, model.HasDates, model.StartDate, model.EndDate);
            SetOneTimeUse(promotion, model.OneTimeUse);
            SetOrderTypeIDs(promotion, model.HasOrderTypeIDs, model.OrderTypeIDs);
            SetAccountTitles(promotion, model.HasAccountTypes, model.PaidAsTitleIDs, (int)Constants.TitleType.PaidAS);
            SetAccountTitles(promotion, model.HasAccountTypes, model.RecognizedTitleIDs, (int)Constants.TitleType.Recognition);
            SetAccountIDs(promotion, model.HasAccountIDs, model.AccountIDs);
            return promotion;
        }



        #region Helpers

        public void SetDescription(TPromotion promotion, string name)
        {
            promotion.Description = name;
        }

        public void SetPromotionCode(TPromotion promotion, bool hasPromotionCode, string promotionCode)
        {

            if (hasPromotionCode && !String.IsNullOrEmpty(promotionCode))
            {
                promotion.PromotionCode = promotionCode;
            }
            else
            {
                promotion.PromotionCode = null;
            }
        }

        //INI - GR_Encore-07
        public void SetContinuity(TPromotion promotion, bool continuity)
        {
            promotion.Continuity = continuity;
        }

        public void SetNewBAs(TPromotion promotion, bool hasNewBAsIDs, IList<int> newBAIDs)
        {
            if (hasNewBAsIDs)
            {
                var added = newBAIDs.Where(newtype => !(promotion.AccountConsistencyStatusIDs.Contains((short)newtype)));
                var removed = promotion.AccountConsistencyStatusIDs.Where(oldtype => newBAIDs.Contains((int)oldtype)).ToList();
                foreach (var newType in added)
                {
                    promotion.AddAccountConsistencyStatusID((short)newType);
                }
                foreach (var oldType in removed)
                {
                    promotion.DeleteAccountConsistencyStatusID(oldType);
                }
            }
            else
            {
                foreach (var existing in promotion.AccountConsistencyStatusIDs)
                {
                    promotion.DeleteAccountConsistencyStatusID(existing);
                }
            }
        }

        public void SetActivityStatusIDs(TPromotion promotion, bool hasActivityStatusIDs, IList<short> activityStatusIDs)
        {
            if (hasActivityStatusIDs)
            {
                var added = activityStatusIDs.Where(newtype => !(promotion.ActivityStatusIDs.Contains(newtype)));
                var removed = promotion.ActivityStatusIDs.Where(oldtype => activityStatusIDs.Contains(oldtype)).ToList();
                foreach (var newType in added)
                {
                    promotion.AddActivityStatusID(newType);
                }
                foreach (var oldType in removed)
                {
                    promotion.DeleteActivityStatusID(oldType);
                }
            }
            else
            {
                foreach (var existing in promotion.ActivityStatusIDs)
                {
                    promotion.DeleteActivityStatusID(existing);
                }
            }
        }
        //FIN - GR_Encore-07

        public void SetAccountTypeIDs(TPromotion promotion, bool hasAccountTypeIDs, IList<int> accountTypeIDs)
        {
            if (hasAccountTypeIDs)
            {
                var added = accountTypeIDs.Where(newtype => !(promotion.AccountTypeIDs.Contains((short)newtype)));
                var removed = promotion.AccountTypeIDs.Where(oldtype => accountTypeIDs.Contains((int)oldtype)).ToList();
                foreach (var newType in added)
                {
                    promotion.AddAccountTypeID((short)newType);
                }
                foreach (var oldType in removed)
                {
                    promotion.DeleteAccountTypeID(oldType);
                }
            }
            else
            {
                foreach (var existing in promotion.AccountTypeIDs)
                {
                    promotion.DeleteAccountTypeID(existing);
                }
            }
        }

        public void SetDates(TPromotion promotion, bool hasDates, DateTime? startDate, DateTime? endDate)
        {
            if (hasDates)
            {
                promotion.StartDate = startDate;
                promotion.EndDate = endDate;
            }
            else
            {
                promotion.StartDate = null;
                promotion.EndDate = null;
            }
        }

        public void SetOneTimeUse(TPromotion promotion, bool oneTimeUse)
        {
            promotion.OneTimeUse = oneTimeUse;
        }

        public void SetOrderTypeIDs(TPromotion promotion, bool hasOrderTypeIDs, IList<int> orderTypeIDs)
        {
            if (hasOrderTypeIDs)
            {
                var added = orderTypeIDs.Where(newtype => !(promotion.OrderTypes.Contains(newtype)));
                var removed = promotion.OrderTypes.Where(oldtype => orderTypeIDs.Contains(oldtype)).ToList();
                foreach (var newType in added)
                {
                    promotion.AddOrderTypeID(newType);
                }
                foreach (var oldType in removed)
                {
                    promotion.DeleteOrderTypeID(oldType);
                }
            }
            else
            {
                foreach (var existing in promotion.OrderTypes)
                {
                    promotion.DeleteOrderTypeID(existing);
                }
            }
        }

        private void SetAccountIDs(TPromotion promotion, bool hasAccountIDs, IList<int> accountIDs)
        {
            if (hasAccountIDs)
            {
                var added = accountIDs.Where(newtype => !(promotion.AccountIDs.Contains(newtype)));
                var removed = promotion.AccountIDs.Where(oldtype => accountIDs.Contains(oldtype)).ToList();
                foreach (var newType in added)
                {
                    promotion.AddAccountID(newType);
                }
                foreach (var oldType in removed)
                {
                    promotion.DeleteAccountID(oldType);
                }
            }
            else
            {
                foreach (var existing in promotion.AccountIDs)
                {
                    promotion.DeleteAccountID(existing);
                }
            }
        }


        public void SetAccountTitles(TPromotion promotion, bool hasAccountTitles, IList<int> accountTitleIds, int titleTypeID)
        {
            if (!hasAccountTitles)
            {
                foreach (var existing in promotion.AccountTitles)
                {
                    promotion.DeleteAccountTitle(existing.TitleID, existing.TitleTypeID);
                }
            }
            else
            {
                var current = promotion.AccountTitles.Where(title => title.TitleTypeID == titleTypeID).Select(title => title.TitleID);
                var added = accountTitleIds.Where(newtitle => !(current.Contains(newtitle)));
                var removed = current.Where(oldtitle => accountTitleIds.Contains(oldtitle)).ToList();
                foreach (var newtitle in added)
                {
                    promotion.AddAccountTitle(newtitle, titleTypeID);
                }
                foreach (var oldtitle in removed)
                {
                    promotion.DeleteAccountTitle(oldtitle, titleTypeID);
                }
            }
        }

        #endregion

        #endregion
    }
}