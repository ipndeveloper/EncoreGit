using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DomainEventQueueItem.
	/// </summary>
	[ContractClass(typeof(Contracts.DomainEventQueueItemContracts))]
	public interface IDomainEventQueueItem
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DomainEventQueueItemID for this DomainEventQueueItem.
		/// </summary>
		int DomainEventQueueItemID { get; set; }
	
		/// <summary>
		/// The DomainEventTypeID for this DomainEventQueueItem.
		/// </summary>
		short DomainEventTypeID { get; set; }
	
		/// <summary>
		/// The QueueItemStatusID for this DomainEventQueueItem.
		/// </summary>
		short QueueItemStatusID { get; set; }
	
		/// <summary>
		/// The QueueItemPriorityID for this DomainEventQueueItem.
		/// </summary>
		short QueueItemPriorityID { get; set; }
	
		/// <summary>
		/// The EventContextID for this DomainEventQueueItem.
		/// </summary>
		int EventContextID { get; set; }
	
		/// <summary>
		/// The AttemptCount for this DomainEventQueueItem.
		/// </summary>
		byte AttemptCount { get; set; }
	
		/// <summary>
		/// The LastRunDateUTC for this DomainEventQueueItem.
		/// </summary>
		Nullable<System.DateTime> LastRunDateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DomainEventType for this DomainEventQueueItem.
		/// </summary>
	    IDomainEventType DomainEventType { get; set; }
	
		/// <summary>
		/// The EventContext for this DomainEventQueueItem.
		/// </summary>
	    IEventContext EventContext { get; set; }
	
		/// <summary>
		/// The QueueItemPriority for this DomainEventQueueItem.
		/// </summary>
	    IQueueItemPriority QueueItemPriority { get; set; }
	
		/// <summary>
		/// The QueueItemStatus for this DomainEventQueueItem.
		/// </summary>
	    IQueueItemStatus QueueItemStatus { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DeviceNotifications for this DomainEventQueueItem.
		/// </summary>
		IEnumerable<IDeviceNotification> DeviceNotifications { get; }
	
		/// <summary>
		/// Adds an <see cref="IDeviceNotification"/> to the DeviceNotifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IDeviceNotification"/> to add.</param>
		void AddDeviceNotification(IDeviceNotification item);
	
		/// <summary>
		/// Removes an <see cref="IDeviceNotification"/> from the DeviceNotifications collection.
		/// </summary>
		/// <param name="item">The <see cref="IDeviceNotification"/> to remove.</param>
		void RemoveDeviceNotification(IDeviceNotification item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDomainEventQueueItem))]
		internal abstract class DomainEventQueueItemContracts : IDomainEventQueueItem
		{
		    #region Primitive properties
		
			int IDomainEventQueueItem.DomainEventQueueItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IDomainEventQueueItem.DomainEventTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IDomainEventQueueItem.QueueItemStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IDomainEventQueueItem.QueueItemPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDomainEventQueueItem.EventContextID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte IDomainEventQueueItem.AttemptCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IDomainEventQueueItem.LastRunDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDomainEventType IDomainEventQueueItem.DomainEventType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IEventContext IDomainEventQueueItem.EventContext
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IQueueItemPriority IDomainEventQueueItem.QueueItemPriority
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IQueueItemStatus IDomainEventQueueItem.QueueItemStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDeviceNotification> IDomainEventQueueItem.DeviceNotifications
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDomainEventQueueItem.AddDeviceNotification(IDeviceNotification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDomainEventQueueItem.RemoveDeviceNotification(IDeviceNotification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
