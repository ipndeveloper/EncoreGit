using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for AutoshipSchedule.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoshipScheduleContracts))]
	public interface IAutoshipSchedule
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoshipScheduleID for this AutoshipSchedule.
		/// </summary>
		int AutoshipScheduleID { get; set; }
	
		/// <summary>
		/// The OrderTypeID for this AutoshipSchedule.
		/// </summary>
		short OrderTypeID { get; set; }
	
		/// <summary>
		/// The Name for this AutoshipSchedule.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AutoshipSchedule.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The IntervalTypeID for this AutoshipSchedule.
		/// </summary>
		int IntervalTypeID { get; set; }
	
		/// <summary>
		/// The Active for this AutoshipSchedule.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The AutoshipScheduleTypeID for this AutoshipSchedule.
		/// </summary>
		int AutoshipScheduleTypeID { get; set; }
	
		/// <summary>
		/// The BaseSiteID for this AutoshipSchedule.
		/// </summary>
		Nullable<int> BaseSiteID { get; set; }
	
		/// <summary>
		/// The MinimumCommissionableTotal for this AutoshipSchedule.
		/// </summary>
		Nullable<decimal> MinimumCommissionableTotal { get; set; }
	
		/// <summary>
		/// The LastRunDate for this AutoshipSchedule.
		/// </summary>
		Nullable<System.DateTime> LastRunDate { get; set; }
	
		/// <summary>
		/// The NextRunDate for this AutoshipSchedule.
		/// </summary>
		Nullable<System.DateTime> NextRunDate { get; set; }
	
		/// <summary>
		/// The MaximumAttemptsPerInterval for this AutoshipSchedule.
		/// </summary>
		Nullable<int> MaximumAttemptsPerInterval { get; set; }
	
		/// <summary>
		/// The MaximumFailedIntervals for this AutoshipSchedule.
		/// </summary>
		Nullable<int> MaximumFailedIntervals { get; set; }
	
		/// <summary>
		/// The AutoshipReminderDayOffSet for this AutoshipSchedule.
		/// </summary>
		Nullable<int> AutoshipReminderDayOffSet { get; set; }
	
		/// <summary>
		/// The IsTemplateEditable for this AutoshipSchedule.
		/// </summary>
		bool IsTemplateEditable { get; set; }
	
		/// <summary>
		/// The IsEnrollable for this AutoshipSchedule.
		/// </summary>
		bool IsEnrollable { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AutoshipScheduleDays for this AutoshipSchedule.
		/// </summary>
		IEnumerable<IAutoshipScheduleDay> AutoshipScheduleDays { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoshipScheduleDay"/> to the AutoshipScheduleDays collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipScheduleDay"/> to add.</param>
		void AddAutoshipScheduleDay(IAutoshipScheduleDay item);
	
		/// <summary>
		/// Removes an <see cref="IAutoshipScheduleDay"/> from the AutoshipScheduleDays collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipScheduleDay"/> to remove.</param>
		void RemoveAutoshipScheduleDay(IAutoshipScheduleDay item);
	
		/// <summary>
		/// The AutoshipScheduleProducts for this AutoshipSchedule.
		/// </summary>
		IEnumerable<IAutoshipScheduleProduct> AutoshipScheduleProducts { get; }
	
		/// <summary>
		/// Adds an <see cref="IAutoshipScheduleProduct"/> to the AutoshipScheduleProducts collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipScheduleProduct"/> to add.</param>
		void AddAutoshipScheduleProduct(IAutoshipScheduleProduct item);
	
		/// <summary>
		/// Removes an <see cref="IAutoshipScheduleProduct"/> from the AutoshipScheduleProducts collection.
		/// </summary>
		/// <param name="item">The <see cref="IAutoshipScheduleProduct"/> to remove.</param>
		void RemoveAutoshipScheduleProduct(IAutoshipScheduleProduct item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoshipSchedule))]
		internal abstract class AutoshipScheduleContracts : IAutoshipSchedule
		{
		    #region Primitive properties
		
			int IAutoshipSchedule.AutoshipScheduleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAutoshipSchedule.OrderTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoshipSchedule.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoshipSchedule.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipSchedule.IntervalTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoshipSchedule.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipSchedule.AutoshipScheduleTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAutoshipSchedule.BaseSiteID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<decimal> IAutoshipSchedule.MinimumCommissionableTotal
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipSchedule.LastRunDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipSchedule.NextRunDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAutoshipSchedule.MaximumAttemptsPerInterval
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAutoshipSchedule.MaximumFailedIntervals
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IAutoshipSchedule.AutoshipReminderDayOffSet
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoshipSchedule.IsTemplateEditable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAutoshipSchedule.IsEnrollable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAutoshipScheduleDay> IAutoshipSchedule.AutoshipScheduleDays
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoshipSchedule.AddAutoshipScheduleDay(IAutoshipScheduleDay item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoshipSchedule.RemoveAutoshipScheduleDay(IAutoshipScheduleDay item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IAutoshipScheduleProduct> IAutoshipSchedule.AutoshipScheduleProducts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAutoshipSchedule.AddAutoshipScheduleProduct(IAutoshipScheduleProduct item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAutoshipSchedule.RemoveAutoshipScheduleProduct(IAutoshipScheduleProduct item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
