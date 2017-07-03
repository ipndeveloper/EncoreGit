using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Promotions.Common.Model;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class PromotionTokenValueProvider : ITokenValueProvider
    {
        private Account _account;
        private IPromotion _promo;

        private const string PROMOTION_NAME = "PromotionName";
        private const string PROMOTION_START_DATE = "PromotionStartDate";
        private const string PROMOTION_END_DATE = "PromotionEndDate";
        private const string ACCOUNT_FIRST_NAME = "AccountFirstName";
        private const string ACCOUNT_LAST_NAME = "AccountLastName";
        private const string ACCOUNT_FULLNAME = "AccountFullName";

        public PromotionTokenValueProvider(Account account, IPromotion promo)
        {
            _promo = promo;
            _account = account;
        }

        private bool AccountIsValid()
        {
            return _account != null;
        }

        private string GetAccountFirstName()
        {
            return AccountIsValid() ? _account.FirstName : string.Empty;
        }

        private string GetAccountLastName()
        {
            return AccountIsValid() ? _account.LastName : string.Empty;
        }

        private string GetAccountFullName()
        {
            return AccountIsValid() ? _account.FullName : string.Empty;
        }
        private bool PromotionIsValid()
        {
            return _promo != null;
        }
        private string GetPromotionName()
        {
            return PromotionIsValid() ? _promo.Description: string.Empty;
        }

        private string GetPromotionStartDate()
        {
            return PromotionIsValid() && _promo.StartDate.HasValue ? _promo.StartDate.Value.ToShortDateString() : string.Empty;
        }

        private string GetPromotionEndDate()
        {
            return PromotionIsValid() && _promo.EndDate.HasValue? _promo.EndDate.Value.ToShortDateString() : string.Empty;
        }
        #region ITokenValueProvider Members

        public System.Collections.Generic.IEnumerable<string> GetKnownTokens()
        {
            return new List<string>
            {
                PROMOTION_NAME,
                PROMOTION_START_DATE,
                PROMOTION_END_DATE,
                ACCOUNT_FIRST_NAME,
                ACCOUNT_LAST_NAME,
                ACCOUNT_FULLNAME
            };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case ACCOUNT_FIRST_NAME:
                    return GetAccountFirstName();
                case ACCOUNT_LAST_NAME:
                    return GetAccountLastName();
                case ACCOUNT_FULLNAME:
                    return GetAccountFullName();
                case PROMOTION_NAME:
                    return GetPromotionName();
                case PROMOTION_START_DATE:
                    return GetPromotionStartDate();
                case PROMOTION_END_DATE:
                    return GetPromotionEndDate();
                default:
                    return string.Empty;
            }
        }
        #endregion
    }
}
