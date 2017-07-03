using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.TokenValueProviders
{
    /// <summary>
    /// Token value provider to be used for Email Template Type "Consultant Joins Downline".
    /// </summary>
	[ContainerRegister(typeof(DistributorJoinsDownlineTokenValueProvider), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.Default)]
    public class DistributorJoinsDownlineTokenValueProvider : ITokenValueProvider
    {
        /// <summary>
        /// Tokens to support "Enrollment Completed" Email Template Type since these are still in use for some clients for distributor enrollment.
        /// </summary>
        private const string NEW_CONSULTANT_ACCOUNT_NUMBER = "NewConsultantAccountNumber";
        private const string NEW_CONSULTANT_FIRST_NANE = "NewConsultantFirstName";
        private const string NEW_CONSULTANT_LAST_NAME = "NewConsultantLastName";
        private const string NEW_CONSULTANT_PHONE = "NewConsultantPhone";

        /// <summary>
        /// Tokens for Email Template Type "Consultant Joins Downline". 
        /// </summary>
        private const string NEW_DISTRIBUTOR_ACCOUNT_NUMBER = "NewDistributorAccountNumber";
        private const string NEW_DISTRIBUTOR_AUTOSHIP_ORDER_NUMBER = "NewDistributorAutoshipOrderNumber";
        private const string NEW_DISTRIBUTOR_FIRST_NAME = "NewDistributorFirstName";
        private const string NEW_DISTRIBUTOR_LAST_NAME = "NewDistributorLastName";
        private const string NEW_DISTRIBUTOR_FULL_NAME = "NewDistributorFullName";
        private const string NEW_DISTIRBUTOR_INITIAL_ORDER_NUMBER = "NewDistirbutorInitialOrderNumber";

        private const string SPONSOR_FULL_NAME = "SponsorFullName";
        //private const string SPONSOR_PHONE = "SponsorPhone";
        private const string STARTER_KIT_ORDER_NUMBER = "OrderNumber";


        protected readonly Order _starterKitOrder;
        protected readonly Account _newAccount;
        protected readonly Account _sponsor;

        public DistributorJoinsDownlineTokenValueProvider(Order starterKitOrder, Account newAccount)
        {
            this._starterKitOrder = starterKitOrder;
            this._newAccount = newAccount;
            this._sponsor  = newAccount != null && newAccount.SponsorID.HasValue
                                       ? Account.Load(newAccount.SponsorID.Value)
                                       : null;
            List<AccountPhones> Aphones = new List<AccountPhones>();
            Aphones = Account.AccountPhonesList(_newAccount.AccountID);
            foreach (AccountPhones ph in Aphones)
            {
                if (ph.PhoneTypeID == 1)
                {
                    _newAccount.HomePhone = ph.PhoneNumber;
                    break;
                }
            }

        }

        /// <summary>
        /// Gets the list of tokens which are part of the email template.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetKnownTokens()
        {
            return new List<string>
            {
                SPONSOR_FULL_NAME,
                NEW_CONSULTANT_ACCOUNT_NUMBER,
                NEW_CONSULTANT_FIRST_NANE,
                NEW_CONSULTANT_LAST_NAME,
                NEW_DISTRIBUTOR_ACCOUNT_NUMBER,
                NEW_DISTRIBUTOR_AUTOSHIP_ORDER_NUMBER,
                NEW_DISTIRBUTOR_INITIAL_ORDER_NUMBER,
                NEW_DISTRIBUTOR_FIRST_NAME,
                NEW_DISTRIBUTOR_FULL_NAME,
                NEW_DISTRIBUTOR_LAST_NAME,
                NEW_CONSULTANT_PHONE,
				STARTER_KIT_ORDER_NUMBER
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributorJoinsDownlineTokenValueProvider"/> class.
        /// </summary>
        /// <param name="starterKitOrder">The starter kit order.</param>
        /// <param name="newAccount">The new distributor account.</param>
        /// <param name="sponsor">The sponsor for the distributor.</param>
        public DistributorJoinsDownlineTokenValueProvider(Order starterKitOrder, Account newAccount, Account sponsor)
        {
            this._starterKitOrder = starterKitOrder;
            this._newAccount = newAccount;
            //this._sponsor = sponsor;
            this._sponsor = newAccount != null && newAccount.SponsorID.HasValue
                                      ? Account.Load(newAccount.SponsorID.Value)
                                      : null;

            List<AccountPhones> Aphones = new List<AccountPhones>();
            Aphones = Account.AccountPhonesList(_newAccount.AccountID);
            foreach (AccountPhones ph in Aphones)
            {
                if (ph.PhoneTypeID == 1)
                {
                    _newAccount.HomePhone = ph.PhoneNumber;
                    break;
                }
            }
        }

        /// <summary>
        /// Gets the value for the given token.
        /// </summary>
        /// <param name="token">The token whose value needs to be retrieved.</param>
        /// <returns></returns>
        public virtual string GetTokenValue(string token)
        {
            switch (token)
            {
                case SPONSOR_FULL_NAME:
                    return (_newAccount != null && _newAccount.SponsorInfo != null) ? _newAccount.SponsorInfo.FullName : string.Empty;

                case NEW_CONSULTANT_ACCOUNT_NUMBER:
                    return _newAccount != null ? _newAccount.AccountNumber : string.Empty;

                case NEW_CONSULTANT_FIRST_NANE:
                    return _newAccount != null ? _newAccount.FirstName : string.Empty;

                case NEW_CONSULTANT_LAST_NAME:
                    return _newAccount != null ? _newAccount.LastName : string.Empty;

                case NEW_DISTRIBUTOR_ACCOUNT_NUMBER:
                    return _newAccount != null ? _newAccount.AccountNumber : string.Empty;

                case NEW_DISTRIBUTOR_AUTOSHIP_ORDER_NUMBER:
                    return (_newAccount != null && _newAccount.AutoshipOrders.Any()) ? _newAccount.AutoshipOrders.FirstOrDefault().AutoshipOrderID.ToString() : string.Empty;

                case NEW_DISTRIBUTOR_FIRST_NAME:
                    return _newAccount != null ? _newAccount.FirstName : string.Empty;

                case NEW_DISTRIBUTOR_LAST_NAME:
                    return _newAccount != null ? _newAccount.LastName : string.Empty;

                case NEW_DISTRIBUTOR_FULL_NAME:
                    return _newAccount != null ? _newAccount.FullName : string.Empty;

                case NEW_DISTIRBUTOR_INITIAL_ORDER_NUMBER:
                    return _starterKitOrder != null ? _starterKitOrder.OrderNumber : string.Empty;

                case STARTER_KIT_ORDER_NUMBER:
                    return _starterKitOrder != null ? _starterKitOrder.OrderNumber : string.Empty;

                case NEW_CONSULTANT_PHONE:
                    return _sponsor != null ? _newAccount.HomePhone : string.Empty;

                default:
                    return null;
            }
        }
    }
}
