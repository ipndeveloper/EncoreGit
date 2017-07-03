using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DeviceNotification.
	/// </summary>
	[ContractClass(typeof(Contracts.DeviceNotificationContracts))]
	public interface IDeviceNotification
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DeviceNotificationID for this DeviceNotification.
		/// </summary>
		int DeviceNotificationID { get; set; }
	
		/// <summary>
		/// The AccountDeviceID for this DeviceNotification.
		/// </summary>
		int AccountDeviceID { get; set; }
	
		/// <summary>
		/// The QueueItemStatusID for this DeviceNotification.
		/// </summary>
		short QueueItemStatusID { get; set; }
	
		/// <summary>
		/// The Body for this DeviceNotification.
		/// </summary>
		string Body { get; set; }
	
		/// <summary>
		/// The ResultMessage for this DeviceNotification.
		/// </summary>
		string ResultMessage { get; set; }
	
		/// <summary>
		/// The DomainEventQueueItemID for this DeviceNotification.
		/// </summary>
		Nullable<int> DomainEventQueueItemID { get; set; }
	
		/// <summary>
		/// The AttemptCount for this DeviceNotification.
		/// </summary>
		byte AttemptCount { get; set; }
	
		/// <summary>
		/// The LastRunDateUTC for this DeviceNotification.
		/// </summary>
		Nullable<System.DateTime> LastRunDateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AccountDevice for this DeviceNotification.
		/// </summary>
	    IAccountDevice AccountDevice { get; set; }
	
		/// <summary>
		/// The DomainEventQueueItem for this DeviceNotification.
		/// </summary>
	    IDomainEventQueueItem DomainEventQueueItem { get; set; }
	
		/// <summary>
		/// The QueueItemStatus for this DeviceNotification.
		/// </summary>
	    IQueueItemStatus QueueItemStatus { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDeviceNotification))]
		internal abstract class DeviceNotificationContracts : IDeviceNotification
		{
		    #region Primitive properties
		
			int IDeviceNotification.DeviceNotificationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDeviceNotification.AccountDeviceID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IDeviceNotification.QueueItemStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDeviceNotification.Body
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDeviceNotification.ResultMessage
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IDeviceNotification.DomainEventQueueItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte IDeviceNotification.AttemptCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IDeviceNotification.LastRunDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccountDevice IDeviceNotification.AccountDevice
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IDomainEventQueueItem IDeviceNotification.DomainEventQueueItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IQueueItemStatus IDeviceNotification.QueueItemStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
