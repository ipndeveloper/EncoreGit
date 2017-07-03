using System;

namespace NetSteps.Data.Entities.Commissions
{
    [Serializable]
    public partial class ProPayProfile : BaseProfile
	{
		public ProPayProfile(DisbursementProfile profile)
			: base(profile, NetSteps.Data.Entities.Constants.DisbursementProfileType.ProPay)
		{
		}

        /// <summary>
        /// Gets or sets the propay account number.  This replaces "Get/Set Attribute Value".  Note that the entity value is a string but
        /// this property has been handled as an integer.  Temporarily I am converting the string value to an int, this may cause problems.
        /// </summary>
        /// <value>
        /// The propay account number.
        /// </value>
        public int PropayAccountNumber
        {
            get { return Convert.ToInt32(_parentProfile.PropayAccountNumber); }
            set { _parentProfile.PropayAccountNumber = value.ToString(); }
        }
	}
}
