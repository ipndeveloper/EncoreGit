using System;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Commissions
{
    [Serializable]
    public partial class EFTProfile : BaseProfile
	{
		public EFTProfile(DisbursementProfile profile)
			: base(profile, Constants.DisbursementProfileType.EFT)
		{
		}

		public DisbursementProfile DisbursementProfile
		{
			get { return _parentProfile; }
		}

        /// <summary>
        /// Gets or sets the name on account.
        /// </summary>
        /// <value>
        /// The name on account.
        /// </value>
		public string NameOnAccount
		{
            get { return _parentProfile.NameOnAccount; }
            set { _parentProfile.NameOnAccount = value; }
		}

		public string RoutingNumber
		{
            get { return _parentProfile.RoutingNumber; }
			set { _parentProfile.RoutingNumber = value; }
		}

		public string BankAccountNumber
		{
            get { return _parentProfile.BankAccountNumber; }
            set { _parentProfile.BankAccountNumber = value; }
		}

		public string BankName
		{
            get { return _parentProfile.BankName; }
            set { _parentProfile.BankName = value; }
		}

		public string BankPhone
		{
            get { return _parentProfile.BankPhone; }
            set { _parentProfile.BankPhone = value; }
		}

        /// <summary>
        /// Gets or sets the bank address identifier.  This assumes that the bank address id can never be null.....
        /// </summary>
        /// <value>
        /// The bank address identifier.
        /// </value>
		public int BankAddressID
		{
            get { return _parentProfile.AddressID.Value; }
            set { _parentProfile.AddressID = value; }
		}

		public Constants.BankAccountTypeEnum BankAccountType
		{
            get { return (Constants.BankAccountTypeEnum)_parentProfile.BankAccountTypeID.Value; }
            set { _parentProfile.BankAccountTypeID = (int)value; }
		}

		public bool EnrollmentFormReceived
		{
            get { return _parentProfile.EnrollmentFormReceived.Value; }
            set { _parentProfile.EnrollmentFormReceived = value; }
		}

        public Address GetAddress()
        {
            var address = new Address();
            if (this.BankAddressID > 0)
            {
                try
                {
                    address = Address.Load(this.BankAddressID);
                }
                catch (NetStepsDataException ex)
                {
                    EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                }
            }
            return address;
        }
	}
}
