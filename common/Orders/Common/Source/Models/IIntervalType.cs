using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for IntervalType.
	/// </summary>
	[ContractClass(typeof(Contracts.IntervalTypeContracts))]
	public interface IIntervalType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The IntervalTypeID for this IntervalType.
		/// </summary>
		int IntervalTypeID { get; set; }
	
		/// <summary>
		/// The Name for this IntervalType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this IntervalType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The TermName for this IntervalType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Interval for this IntervalType.
		/// </summary>
		byte Interval { get; set; }
	
		/// <summary>
		/// The IntervalDay for this IntervalType.
		/// </summary>
		Nullable<byte> IntervalDay { get; set; }
	
		/// <summary>
		/// The IsMonthly for this IntervalType.
		/// </summary>
		bool IsMonthly { get; set; }
	
		/// <summary>
		/// The IsWeekly for this IntervalType.
		/// </summary>
		bool IsWeekly { get; set; }
	
		/// <summary>
		/// The IsAnnual for this IntervalType.
		/// </summary>
		bool IsAnnual { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AutoshipSchedules for this IntervalType.
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
		[ContractClassFor(typeof(IIntervalType))]
		internal abstract class IntervalTypeContracts : IIntervalType
		{
		    #region Primitive properties
		
			int IIntervalType.IntervalTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIntervalType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIntervalType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IIntervalType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte IIntervalType.Interval
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<byte> IIntervalType.IntervalDay
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IIntervalType.IsMonthly
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IIntervalType.IsWeekly
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IIntervalType.IsAnnual
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAutoshipSchedule> IIntervalType.AutoshipSchedules
			{
				get { throw new NotImplementedException(); }
			}
		
			void IIntervalType.AddAutoshipSchedule(IAutoshipSchedule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IIntervalType.RemoveAutoshipSchedule(IAutoshipSchedule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
