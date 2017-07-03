using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for AutoshipScheduleDay.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoshipScheduleDayContracts))]
	public interface IAutoshipScheduleDay
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoshipScheduleDayID for this AutoshipScheduleDay.
		/// </summary>
		int AutoshipScheduleDayID { get; set; }
	
		/// <summary>
		/// The AutoshipScheduleID for this AutoshipScheduleDay.
		/// </summary>
		int AutoshipScheduleID { get; set; }
	
		/// <summary>
		/// The Day for this AutoshipScheduleDay.
		/// </summary>
		byte Day { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AutoshipSchedule for this AutoshipScheduleDay.
		/// </summary>
	    IAutoshipSchedule AutoshipSchedule { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoshipScheduleDay))]
		internal abstract class AutoshipScheduleDayContracts : IAutoshipScheduleDay
		{
		    #region Primitive properties
		
			int IAutoshipScheduleDay.AutoshipScheduleDayID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipScheduleDay.AutoshipScheduleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte IAutoshipScheduleDay.Day
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAutoshipSchedule IAutoshipScheduleDay.AutoshipSchedule
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
