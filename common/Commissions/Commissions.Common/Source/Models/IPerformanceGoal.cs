using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Common.Models
{
	/// <summary>
	/// Common interface for PerformanceGoal.
	/// </summary>
	[ContractClass(typeof(Contracts.PerformanceGoalContracts))]
	public interface IPerformanceGoal
	{
	    #region Primitive properties
	
		/// <summary>
		/// The PerformanceGoalID for this PerformanceGoal.
		/// </summary>
		int PerformanceGoalID { get; set; }
	
		/// <summary>
		/// The TitleID for this PerformanceGoal.
		/// </summary>
		int TitleID { get; set; }
	
		/// <summary>
		/// The PVGoal for this PerformanceGoal.
		/// </summary>
		decimal PVGoal { get; set; }
	
		/// <summary>
		/// The GVGoal for this PerformanceGoal.
		/// </summary>
		decimal GVGoal { get; set; }
	
		/// <summary>
		/// The EffectiveDate for this PerformanceGoal.
		/// </summary>
		System.DateTime EffectiveDate { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Title for this PerformanceGoal.
		/// </summary>
	    ITitle Title { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IPerformanceGoal))]
		internal abstract class PerformanceGoalContracts : IPerformanceGoal
		{
		    #region Primitive properties
		
			int IPerformanceGoal.PerformanceGoalID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IPerformanceGoal.TitleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IPerformanceGoal.PVGoal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			decimal IPerformanceGoal.GVGoal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime IPerformanceGoal.EffectiveDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ITitle IPerformanceGoal.Title
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
