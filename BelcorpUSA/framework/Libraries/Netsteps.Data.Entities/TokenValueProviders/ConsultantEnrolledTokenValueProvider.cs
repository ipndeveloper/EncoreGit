using System;
using System.Collections.Generic;
using NetSteps.Common.Interfaces;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Extensions;

namespace NetSteps.Data.Entities.TokenValueProviders
{
	[ContainerRegister(typeof(IConsultantEnrolledTokenValueProvider), RegistrationBehaviors.IfNoneOther, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class ConsultantEnrolledTokenValueProvider : IConsultantEnrolledTokenValueProvider
	{
		private const string NEW_CONSULTANT_ACCOUNT_NUMBER = "NewConsultantAccountNumber";
		private const string NEW_CONSULTANT_FIRST_NANE = "NewConsultantFirstName";
		private const string NEW_CONSULTANT_LAST_NAME = "NewConsultantLastName";
		private const string NEW_DISTRIBUTOR_ENROLLMENT_DATE = "NewDistributorEnrollmentDate";
		private const string STARTER_KIT_ORDER_NUMBER = "OrderNumber";
		private const string NEW_CONSULTANT_SPONSOR_FIRSTNAME = "SponsorFirstName";
		private const string NEW_CONSULTANT_SPONSOR_LASTNAME = "SponsorLastName";
		private const string NEW_CONSULTANT_SPONSOR_EMAIL = "SponsorEmail";
        private const string NEW_CONSULTANT_SPONSOR_PHONE = "SponsorPhone";
		private const string NEW_CONSULTANT_USERNAME = "UserName";

		internal readonly Order _starterKitOrder;
		internal readonly Account _newAccount;
		internal readonly Account _sponsorAccount;
		internal readonly User _newAccountUser;

		public ConsultantEnrolledTokenValueProvider(Order starterKitOrder, Account newAccount, User newAccountUser)
		{
			_starterKitOrder = starterKitOrder;
			_newAccount = newAccount;
			_sponsorAccount = newAccount != null && newAccount.SponsorID.HasValue
									   ? Account.Load(newAccount.SponsorID.Value)
									   : null;
            List<AccountPhones> Aphones = new List<AccountPhones>();
            Aphones = Account.AccountPhonesList(_sponsorAccount.AccountID);
            foreach (AccountPhones ph in Aphones)
            {
                if (ph.PhoneTypeID == 1)
                {
                    _sponsorAccount.HomePhone = ph.PhoneNumber;
                    break;
                }
            }
            
       		_newAccountUser = newAccountUser;
		}

		public virtual IEnumerable<string> GetKnownTokens()
		{
			return new List<string>
			{
				NEW_CONSULTANT_ACCOUNT_NUMBER,
				NEW_CONSULTANT_FIRST_NANE,
				NEW_CONSULTANT_LAST_NAME,
				NEW_DISTRIBUTOR_ENROLLMENT_DATE,
				STARTER_KIT_ORDER_NUMBER,
				NEW_CONSULTANT_SPONSOR_FIRSTNAME,
				NEW_CONSULTANT_SPONSOR_LASTNAME,
				NEW_CONSULTANT_SPONSOR_EMAIL,
                NEW_CONSULTANT_SPONSOR_PHONE,
				NEW_CONSULTANT_USERNAME
			};
		}


		public virtual string GetTokenValue(string token)
		{
			switch (token)
			{
				case NEW_CONSULTANT_ACCOUNT_NUMBER:
					return _newAccount != null ? _newAccount.AccountNumber : String.Empty;

				case NEW_CONSULTANT_FIRST_NANE:
					return _newAccount != null ? _newAccount.FirstName : String.Empty;

				case NEW_CONSULTANT_LAST_NAME:
					return _newAccount != null ? _newAccount.LastName : String.Empty;

				case NEW_DISTRIBUTOR_ENROLLMENT_DATE:
					return _newAccount != null ? ((DateTime)_newAccount.EnrollmentDate).ToString("MM/dd/yyyy") : String.Empty;

				case STARTER_KIT_ORDER_NUMBER:
					return _starterKitOrder != null ? _starterKitOrder.OrderNumber : String.Empty;

				case NEW_CONSULTANT_SPONSOR_FIRSTNAME:
					return _sponsorAccount != null ? _sponsorAccount.FirstName : String.Empty;

				case NEW_CONSULTANT_SPONSOR_LASTNAME:
					return _sponsorAccount != null ? _sponsorAccount.LastName : String.Empty;

				case NEW_CONSULTANT_SPONSOR_EMAIL:
					return _sponsorAccount != null ? _sponsorAccount.EmailAddress : String.Empty;

                case NEW_CONSULTANT_SPONSOR_PHONE:
                    return _sponsorAccount != null ? _sponsorAccount.HomePhone : String.Empty;

				case NEW_CONSULTANT_USERNAME:
					return _newAccountUser != null ? _newAccountUser.Username : String.Empty;

				default:
					return String.Empty;
			}
		}
	}
}
