using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class CampaignSubscriberTokenValueProvider : ITokenValueProvider
    {
        private Account _campaignSubscriber;
        private Account _subscribedByAccount;

        private const string RECIPIENT_FIRST_NAME = "RecipientFirstName";
        private const string RECIPIENT_LAST_NAME = "RecipientLastName";
        private const string RECIPIENT_FULL_NAME = "RecipientFullName";
        private const string SPONSOR_FIRST_NAME = "SponsorFirstName";
        private const string SPONSOR_LAST_NAME = "SponsorLastName";
        private const string SPONSOR_FULL_NAME = "SponsorFullName";
        private const string SPONSOR_PHONE = "SponsorPhone";
        private const string SPONSOR_EMAIL = "SponsorEmail";

        public CampaignSubscriberTokenValueProvider(Account campaignSubscriber, Account subscribedByAccount)
        {
            _campaignSubscriber = campaignSubscriber;
            _subscribedByAccount = subscribedByAccount;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>()
            {
                RECIPIENT_FIRST_NAME,
                RECIPIENT_LAST_NAME,
                RECIPIENT_FULL_NAME,
                SPONSOR_FIRST_NAME,
                SPONSOR_LAST_NAME, 
                SPONSOR_FULL_NAME,
                SPONSOR_PHONE,
                SPONSOR_EMAIL
            };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case RECIPIENT_FIRST_NAME:
                    return _campaignSubscriber.FirstName;
                case RECIPIENT_LAST_NAME:
                    return _campaignSubscriber.LastName;
                case RECIPIENT_FULL_NAME:
                    return _campaignSubscriber.FullName;
                case SPONSOR_FIRST_NAME:
                    return _subscribedByAccount.FirstName;
                case SPONSOR_LAST_NAME:
                    return _subscribedByAccount.LastName;
                case SPONSOR_FULL_NAME:
                    return _subscribedByAccount.FullName;
                case SPONSOR_PHONE:
                    return _subscribedByAccount.HomePhone;
                case SPONSOR_EMAIL:
                    return _subscribedByAccount.EmailAddress;
                default:
                    return "";
            }
        }
    }
}
