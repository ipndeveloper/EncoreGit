using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for AutoshipScheduleType.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoshipScheduleTypeContracts))]
	public interface IAutoshipScheduleType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoshipScheduleTypeID for this AutoshipScheduleType.
		/// </summary>
		int AutoshipScheduleTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AutoshipScheduleType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AutoshipScheduleType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this AutoshipScheduleType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this AutoshipScheduleType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AutoshipSchedules for this AutoshipScheduleType.
		/// </summary>
		IEnumerable<IAutoshipSchedule> AutoshipSchedules { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoshipSchedule"/> to the AutoshipSchedules collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipSchedule"/> to add.</param>
		void AddAutoshipSchedule(IAutoshipSchedule item);
	
		/// <summary>
		/// Removes an <see cref="IAutoshipSchedule"/> from the AutoshipSchedules collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipSchedule"/> to remove.</param>
		void RemoveAutoshipSchedule(IAutoshipSchedule item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoshipScheduleType))]
		internal abstract class AutoshipScheduleTypeContracts : IAutoshipScheduleType
		{
		    #region Primitive properties
		
			int IAutoshipScheduleType.AutoshipScheduleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoshipScheduleType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoshipScheduleType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoshipScheduleType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoshipScheduleType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAutoshipSchedule> IAutoshipScheduleType.AutoshipSchedules
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoshipScheduleType.AddAutoshipSchedule(IAutoshipSchedule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoshipScheduleType.RemoveAutoshipSchedule(IAutoshipSchedule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
