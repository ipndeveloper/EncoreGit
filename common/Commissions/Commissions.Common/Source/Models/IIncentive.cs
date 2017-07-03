using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for Incentive.
	/// </summary>
	[ContractClass(typeof(Contracts.IncentiveContracts))]
	public interface IIncentive
	{
	    #region Primitive properties
	
		/// <summary>
		/// The IncentiveID for this Incentive.
		/// </summary>
		int IncentiveID { get; set; }
	
		/// <summary>
		/// The IncentiveName for this Incentive.
		/// </summary>
		string IncentiveName { get; set; }
	
		/// <summary>
		/// The Description for this Incentive.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The EffectiveStartDate for this Incentive.
		/// </summary>
		System.DateTime EffectiveStartDate { get; set; }
	
		/// <summary>
		/// The EffectiveEndDate for this Incentive.
		/// </summary>
		System.DateTime EffectiveEndDate { get; set; }
	
		/// <summary>
		/// The CreateDate for this Incentive.
		/// </summary>
		System.DateTime CreateDate { get; set; }
	
		/// <summary>
		/// The QualifiedPoints for this Incentive.
		/// </summary>
		int QualifiedPoints { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IIncentive))]
		internal abstract class IncentiveContracts : IIncentive
		{
		    #region Primitive properties
		
			int IIncentive.IncentiveID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIncentive.IncentiveName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIncentive.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IIncentive.EffectiveStartDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IIncentive.EffectiveEndDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IIncentive.CreateDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IIncentive.QualifiedPoints
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
