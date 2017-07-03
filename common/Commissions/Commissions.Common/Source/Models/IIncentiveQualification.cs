using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for IncentiveQualification.
	/// </summary>
	[ContractClass(typeof(Contracts.IncentiveQualificationContracts))]
	public interface IIncentiveQualification
	{
	    #region Primitive properties
	
		/// <summary>
		/// The IncentiveQualID for this IncentiveQualification.
		/// </summary>
		int IncentiveQualID { get; set; }
	
		/// <summary>
		/// The IncentiveID for this IncentiveQualification.
		/// </summary>
		int IncentiveID { get; set; }
	
		/// <summary>
		/// The CalcTypeID for this IncentiveQualification.
		/// </summary>
		int CalcTypeID { get; set; }
	
		/// <summary>
		/// The AwardUnit for this IncentiveQualification.
		/// </summary>
		int AwardUnit { get; set; }
	
		/// <summary>
		/// The RewardValue for this IncentiveQualification.
		/// </summary>
		int RewardValue { get; set; }
	
		/// <summary>
		/// The IncentiveLedgerEntryTypeID for this IncentiveQualification.
		/// </summary>
		Nullable<int> IncentiveLedgerEntryTypeID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The CalculationType for this IncentiveQualification.
		/// </summary>
	    ICalculationType CalculationType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IIncentiveQualification))]
		internal abstract class IncentiveQualificationContracts : IIncentiveQualification
		{
		    #region Primitive properties
		
			int IIncentiveQualification.IncentiveQualID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentiveQualification.IncentiveID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentiveQualification.CalcTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentiveQualification.AwardUnit
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentiveQualification.RewardValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IIncentiveQualification.IncentiveLedgerEntryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICalculationType IIncentiveQualification.CalculationType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
