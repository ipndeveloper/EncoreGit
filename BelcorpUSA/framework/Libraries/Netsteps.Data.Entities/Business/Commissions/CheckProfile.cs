using System;
using NetSteps.Common.Exceptions;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Commissions
{
    [Serializable]
    public partial class CheckProfile : BaseProfile
	{
		public CheckProfile(DisbursementProfile profile)
			: base(profile, NetSteps.Data.Entities.Constants.DisbursementProfileType.Check)
		{
		}

        //TODO: Verify with commissions that this mapping is as intended.
        /// <summary>
        /// Gets or sets the name on check.  This is being mapped to NameOnAccount, which may or may not be correct.
        /// </summary>
        /// <value>
        /// The name on check.
        /// </value>
		public string NameOnCheck
		{
            get { return _parentProfile.NameOnAccount; }
            set { _parentProfile.NameOnAccount = value; }
		}

		public int AddressID
		{
            get { return _parentProfile.AddressID.Value; }
            set { _parentProfile.AddressID = value; }
		}

        public Address GetAddress()
        {
            Address address = new Address();
            if (this.AddressID > 0)
            {
                try
                {
                    address = Address.Load(this.AddressID);
                }
                catch (NetStepsDataException ex)
                {
                    var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                }
            }
            return address;
        }
	}
}
