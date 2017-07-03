using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for QueueItemStatus.
	/// </summary>
	[ContractClass(typeof(Contracts.QueueItemStatusContracts))]
	public interface IQueueItemStatus
	{
	    #region Primitive properties
	
		/// <summary>
		/// The QueueItemStatusID for this QueueItemStatus.
		/// </summary>
		short QueueItemStatusID { get; set; }
	
		/// <summary>
		/// The Name for this QueueItemStatus.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this QueueItemStatus.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this QueueItemStatus.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this QueueItemStatus.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignActionQueueHistories for this QueueItemStatus.
		/// </summary>
		IEnumerable<ICampaignActionQueueHistory> CampaignActionQueueHistories { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignActionQueueHistory"/> to the CampaignActionQueueHistories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionQueueHistory"/> to add.</param>
		void AddCampaignActionQueueHistory(ICampaignActionQueueHistory item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignActionQueueHistory"/> from the CampaignActionQueueHistories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionQueueHistory"/> to remove.</param>
		void RemoveCampaignActionQueueHistory(ICampaignActionQueueHistory item);
	
		/// <summary>
		/// The CampaignActionQueueItems for this QueueItemStatus.
		/// </summary>
		IEnumerable<ICampaignActionQueueItem> CampaignActionQueueItems { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignActionQueueItem"/> to the CampaignActionQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionQueueItem"/> to add.</param>
		void AddCampaignActionQueueItem(ICampaignActionQueueItem item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignActionQueueItem"/> from the CampaignActionQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionQueueItem"/> to remove.</param>
		void RemoveCampaignActionQueueItem(ICampaignActionQueueItem item);
	
		/// <summary>
		/// The DeviceNotifications for this QueueItemStatus.
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
	
		/// <summary>
		/// The DomainEventQueueItems for this QueueItemStatus.
		/// </summary>
		IEnumerable<IDomainEventQueueItem> DomainEventQueueItems { get; }
	
		/// <summary>
		/// Adds an <see cref="IDomainEventQueueItem"/> to the DomainEventQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IDomainEventQueueItem"/> to add.</param>
		void AddDomainEventQueueItem(IDomainEventQueueItem item);
	
		/// <summary>
		/// Removes an <see cref="IDomainEventQueueItem"/> from the DomainEventQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IDomainEventQueueItem"/> to remove.</param>
		void RemoveDomainEventQueueItem(IDomainEventQueueItem item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IQueueItemStatus))]
		internal abstract class QueueItemStatusContracts : IQueueItemStatus
		{
		    #region Primitive properties
		
			short IQueueItemStatus.QueueItemStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IQueueItemStatus.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IQueueItemStatus.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IQueueItemStatus.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IQueueItemStatus.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignActionQueueHistory> IQueueItemStatus.CampaignActionQueueHistories
			{
				get { throw new NotImplementedException(); }
			}
		
			void IQueueItemStatus.AddCampaignActionQueueHistory(ICampaignActionQueueHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IQueueItemStatus.RemoveCampaignActionQueueHistory(ICampaignActionQueueHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaignActionQueueItem> IQueueItemStatus.CampaignActionQueueItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IQueueItemStatus.AddCampaignActionQueueItem(ICampaignActionQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IQueueItemStatus.RemoveCampaignActionQueueItem(ICampaignActionQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDeviceNotification> IQueueItemStatus.DeviceNotifications
			{
				get { throw new NotImplementedException(); }
			}
		
			void IQueueItemStatus.AddDeviceNotification(IDeviceNotification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IQueueItemStatus.RemoveDeviceNotification(IDeviceNotification item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDomainEventQueueItem> IQueueItemStatus.DomainEventQueueItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IQueueItemStatus.AddDomainEventQueueItem(IDomainEventQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IQueueItemStatus.RemoveDomainEventQueueItem(IDomainEventQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
