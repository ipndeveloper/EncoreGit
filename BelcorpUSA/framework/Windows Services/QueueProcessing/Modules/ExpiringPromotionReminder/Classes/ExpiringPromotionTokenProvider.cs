// -----------------------------------------------------------------------
// <copyright file="ExpiringPromotionTokenProvider.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Classes
{
    using System.Collections.Generic;
    using System.Linq;
    using NetSteps.Data.Entities;
    using NetSteps.Encore.Core.IoC;
    using NetSteps.Promotions.Common.Model;
    using NetSteps.QueueProcessing.Modules.ExpiringPromotionReminder.Interfaces;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [ContainerRegister(typeof(IExpiringPromotionTokenProvider), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
    public class ExpiringPromotionTokenProvider : IExpiringPromotionTokenProvider
    {
        private const string ACCOUNT_FIRST_NANE = "AccountFirstName";
        private const string ACCOUNT_LAST_NAME = "AccountLastName";
        private const string ACCOUNT_FULL_NAME = "AccountFullName";
        private const string PROMOTION_NAME = "PromotionName";
        private const string PROMOTION_START_DATE = "PromotionStartDate";
        private const string PROMOTION_END_DATE = "PromotionEndDate";

        private readonly string[] _knownTokens = new string[] { ACCOUNT_FIRST_NANE, ACCOUNT_LAST_NAME, ACCOUNT_FULL_NAME, PROMOTION_NAME, PROMOTION_START_DATE, PROMOTION_END_DATE };

        Account Account { get; set; }
        IPromotion Promotion { get; set; }

        public ExpiringPromotionTokenProvider(Account account, IPromotion promotion)
        {
            this.Account = account;
            this.Promotion = promotion;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return _knownTokens.ToList();
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case ACCOUNT_FIRST_NANE:
                    return this.Account != null ? this.Account.FirstName : string.Empty;
                case ACCOUNT_LAST_NAME:
                    return this.Account != null ? this.Account.LastName : string.Empty;
                case ACCOUNT_FULL_NAME:
                    return this.Account != null ? this.Account.FullName : string.Empty;
                case PROMOTION_NAME:
                    return string.Empty;
                case PROMOTION_START_DATE:
                    return this.Promotion.StartDate.ToString();
                case PROMOTION_END_DATE:
                    return this.Promotion.EndDate.ToString();
                default:
                    return string.Empty;
            }
        }
    }
}
