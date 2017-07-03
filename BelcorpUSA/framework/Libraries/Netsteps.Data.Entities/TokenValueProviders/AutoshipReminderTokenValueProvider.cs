using System;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class AutoshipReminderTokenValueProvider : ITokenValueProvider
    {
        private const string AUTOSHIP_ORDER_URL = "AutoshipOrderURL";
        private const string REP_NUMBER = "AccountNumber";
        private const string REP_FIRSTNAME = "RecipientFirstName";
        private const string REP_LASTNAME = "RecipientLastName";
        private const string AUTOSHIP_RUNDATE = "AutoshipRunDate";

        private readonly AutoshipOrder _autoshipOrder;
        private readonly string _autoshipOrderUrl;

        public AutoshipReminderTokenValueProvider(AutoshipOrder autoshipOrder, string autoshipOrderUrl)
        {
            _autoshipOrder = autoshipOrder;
            _autoshipOrderUrl = SetAutoshipOrderUrl(autoshipOrderUrl);
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>
                       {
                           AUTOSHIP_ORDER_URL,
                           REP_NUMBER,
                           REP_FIRSTNAME,
                           REP_LASTNAME,
                           AUTOSHIP_RUNDATE
                       };
        }


        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case AUTOSHIP_ORDER_URL:
                    return _autoshipOrderUrl;

                case REP_NUMBER:
                    return GetAccountNumber();

                case REP_FIRSTNAME:
                    return GetFirstName();

                case REP_LASTNAME:
                    return GetLastName();

                case AUTOSHIP_RUNDATE:
                    return GetNextRunDate();

                default:
                    return null;
            }
        }

        private string GetAccountNumber()
        {
            return IsValid() ? _autoshipOrder.Account.AccountNumber : string.Empty;
        }

        private string GetFirstName()
        {
            return IsValid() ? _autoshipOrder.Account.FirstName : string.Empty;
        }

        public string GetLastName()
        {
            return IsValid() ? _autoshipOrder.Account.LastName : string.Empty;
        }

        public string GetNextRunDate()
        {
            return IsValid() && _autoshipOrder.NextRunDate.HasValue
                       ? _autoshipOrder.NextRunDate.Value.ToShortDateString()
                       : string.Empty;
        }

        private bool IsValid()
        {
            return _autoshipOrder != null && _autoshipOrder.Account != null;
        }

        private string SetAutoshipOrderUrl(string url)
        {
            return !String.IsNullOrEmpty(url) ? url : string.Empty;
        }
    }
}
