using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Orders.Common.Models
{
	/// <summary>
	/// Common interface for AutoshipOrder.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoshipOrderContracts))]
	public interface IAutoshipOrder
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoshipOrderID for this AutoshipOrder.
		/// </summary>
		int AutoshipOrderID { get; set; }
	
		/// <summary>
		/// The TemplateOrderID for this AutoshipOrder.
		/// </summary>
		int TemplateOrderID { get; set; }
	
		/// <summary>
		/// The AccountID for this AutoshipOrder.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The AutoshipScheduleID for this AutoshipOrder.
		/// </summary>
		int AutoshipScheduleID { get; set; }
	
		/// <summary>
		/// The ConsecutiveOrders for this AutoshipOrder.
		/// </summary>
		int ConsecutiveOrders { get; set; }
	
		/// <summary>
		/// The DataVersion for this AutoshipOrder.
		/// </summary>
		byte[] DataVersion { get; set; }
	
		/// <summary>
		/// The Day for this AutoshipOrder.
		/// </summary>
		int Day { get; set; }
	
		/// <summary>
		/// The DateLastCreated for this AutoshipOrder.
		/// </summary>
		Nullable<System.DateTime> DateLastCreated { get; set; }
	
		/// <summary>
		/// The LastRunDate for this AutoshipOrder.
		/// </summary>
		Nullable<System.DateTime> LastRunDate { get; set; }
	
		/// <summary>
		/// The NextRunDate for this AutoshipOrder.
		/// </summary>
		Nullable<System.DateTime> NextRunDate { get; set; }
	
		/// <summary>
		/// The StartDate for this AutoshipOrder.
		/// </summary>
		Nullable<System.DateTime> StartDate { get; set; }
	
		/// <summary>
		/// The EndDate for this AutoshipOrder.
		/// </summary>
		Nullable<System.DateTime> EndDate { get; set; }
	
		/// <summary>
		/// The AutoshipReminderNextRunDate for this AutoshipOrder.
		/// </summary>
		Nullable<System.DateTime> AutoshipReminderNextRunDate { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Order for this AutoshipOrder.
		/// </summary>
	    IOrder Order { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoshipOrder))]
		internal abstract class AutoshipOrderContracts : IAutoshipOrder
		{
		    #region Primitive properties
		
			int IAutoshipOrder.AutoshipOrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipOrder.TemplateOrderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipOrder.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipOrder.AutoshipScheduleID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipOrder.ConsecutiveOrders
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte[] IAutoshipOrder.DataVersion
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoshipOrder.Day
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipOrder.DateLastCreated
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipOrder.LastRunDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipOrder.NextRunDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipOrder.StartDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipOrder.EndDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAutoshipOrder.AutoshipReminderNextRunDate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IOrder IAutoshipOrder.Order
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
