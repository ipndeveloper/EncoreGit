using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class AutoshipCanceledTokenValueProvider : ITokenValueProvider
    {
        private const string REP_FIRSTNAME = "RecipientFirstName";
        private const string REP_LASTNAME = "RecipientLastName";
        private const string REP_NUMBER = "AccountNumber";
        private const string AUTOSHIP_RUNDATE = "AutoshipRunDate";
        private const string AUTOSHIP_ORDER_URL = "AutoshipOrderURL";
        private const string SPONSOR_FIRSTNAME = "SponsorFirstName";
        private const string SPONSOR_LASTNAME = "SponsorLastName";
        private const string SPONSOR_EMAIL = "SponsorEmail";
        private const string SPONSOR_PHONE = "SponsorPhone";
        private const string SPONSOR_PWS_URL = "SponsorPwsUrl";


        private readonly AutoshipOrder _autoshipOrder;
        private readonly string _autoshipOrderUrl;
        private readonly Account _sponsor;

        public AutoshipCanceledTokenValueProvider(AutoshipOrder autoshipOrder, string autoshipOrderUrl)
        {
            _autoshipOrder = autoshipOrder;
            _autoshipOrderUrl = SetAutoshipOrderUrl(autoshipOrderUrl);
            _sponsor = SetAccountSponsor(autoshipOrder);
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>
                       {
                           AUTOSHIP_ORDER_URL,
                           REP_NUMBER,
                           REP_FIRSTNAME,
                           REP_LASTNAME,
                           AUTOSHIP_RUNDATE,
                           SPONSOR_FIRSTNAME,
                           SPONSOR_LASTNAME,
                           SPONSOR_EMAIL,
                           SPONSOR_PHONE,
                           SPONSOR_PWS_URL
                       };
        }


        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case AUTOSHIP_ORDER_URL:
                    return _autoshipOrderUrl;

                case REP_NUMBER:
                    return GetAccountNumber(_autoshipOrder.Account);

                case REP_FIRSTNAME:
                    return GetFirstName(_autoshipOrder.Account);

                case REP_LASTNAME:
                    return GetLastName(_autoshipOrder.Account);

                case AUTOSHIP_RUNDATE:
                    return GetNextRunDate();

                case SPONSOR_FIRSTNAME:
                    return GetFirstName(_sponsor);

                case SPONSOR_LASTNAME:
                    return GetLastName(_sponsor);

                case SPONSOR_EMAIL:
                    return _sponsor.EmailAddress;

                case SPONSOR_PHONE:
                    return _sponsor.MainPhone;

                case SPONSOR_PWS_URL:
                    return GetPwsUrl(_sponsor);

                default:
                    return null;
            }
        }

        public Account SetAccountSponsor(AutoshipOrder autoshipOrder)
        {
            Account account;
            if (autoshipOrder.Account.Sponsor.IsNotNull())
                account = autoshipOrder.Account.Sponsor;

            else if (autoshipOrder.Account.SponsorID.HasValue)
                account = Account.Load(autoshipOrder.Account.SponsorID.Value);

            else
                account = new Account();

            return account;
        }

        private string GetPwsUrl(Account account)
        {
            var link = Site.LoadByAccountID(account.AccountID)
                .Where(s => s.SiteUrls.Count > 0)
                .SelectMany(s => s.SiteUrls.Where(u => u.IsPrimaryUrl).Select(u => u.Url))
                .FirstOrDefault();

            return link ?? string.Empty;
        }

        private string GetAccountNumber(Account account)
        {
            return IsValid() ? account.AccountNumber : string.Empty;
        }

        private string GetFirstName(Account account)
        {
            return IsValid() ? account.FirstName : string.Empty;
        }

        public string GetLastName(Account account)
        {
            return IsValid() ? account.LastName : string.Empty;
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
