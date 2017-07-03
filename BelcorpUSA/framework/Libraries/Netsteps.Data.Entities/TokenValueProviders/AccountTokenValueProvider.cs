using System.Collections.Generic;
using NetSteps.Commissions.Common;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using System;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    public class AccountTokenValueProvider : ITokenValueProvider
    {
        private const string ACCOUNT_NUMBER = "AccountNumber";
        private const string ACCOUNT_FIRST_NANE = "AccountFirstName";
        private const string ACCOUNT_LAST_NAME = "AccountLastName";
        private const string ACCOUNT_FULL_NAME = "AccountFullName";
        private const string ACCOUNT_PRODUCT_CREDIT_BALANCE = "AccountProductCreditBalance";

        private readonly Account _newAccount;

        public AccountTokenValueProvider(Account account)
        {
            this._newAccount = account;
        }

        public IEnumerable<string> GetKnownTokens()
        {
            return new List<string>
            {
                ACCOUNT_NUMBER,
                ACCOUNT_FIRST_NANE,
                ACCOUNT_LAST_NAME,
                ACCOUNT_FULL_NAME,
                ACCOUNT_PRODUCT_CREDIT_BALANCE
            };
        }

        public string GetTokenValue(string token)
        {
            switch (token)
            {
                case ACCOUNT_NUMBER:
                    return _newAccount != null ? _newAccount.AccountNumber : string.Empty;
                case ACCOUNT_FIRST_NANE:
                    return _newAccount != null ? _newAccount.FirstName : string.Empty;
                case ACCOUNT_LAST_NAME:
                    return _newAccount != null ? _newAccount.LastName : string.Empty;
                case ACCOUNT_FULL_NAME:
                    return _newAccount != null ? _newAccount.FullName : string.Empty;
                case ACCOUNT_PRODUCT_CREDIT_BALANCE:
                    {
                        if (_newAccount == null)
                        {
                            return string.Empty;
                        }

                        var service = Create.New<IProductCreditLedgerService>();
                        var productCreditBalance = service.GetCurrentBalanceLessPendingPayments(_newAccount.AccountID);
                        return productCreditBalance.ToMoneyString();
                    }

                default:
                    return null;
            }
        }
    }
}
