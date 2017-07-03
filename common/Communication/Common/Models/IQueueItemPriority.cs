using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for QueueItemPriority.
	/// </summary>
	[ContractClass(typeof(Contracts.QueueItemPriorityContracts))]
	public interface IQueueItemPriority
	{
	    #region Primitive properties
	
		/// <summary>
		/// The QueueItemPriorityID for this QueueItemPriority.
		/// </summary>
		short QueueItemPriorityID { get; set; }
	
		/// <summary>
		/// The Name for this QueueItemPriority.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this QueueItemPriority.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this QueueItemPriority.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this QueueItemPriority.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignActionQueueItems for this QueueItemPriority.
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
		/// The DomainEventQueueItems for this QueueItemPriority.
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
		[ContractClassFor(typeof(IQueueItemPriority))]
		internal abstract class QueueItemPriorityContracts : IQueueItemPriority
		{
		    #region Primitive properties
		
			short IQueueItemPriority.QueueItemPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IQueueItemPriority.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IQueueItemPriority.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IQueueItemPriority.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IQueueItemPriority.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignActionQueueItem> IQueueItemPriority.CampaignActionQueueItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IQueueItemPriority.AddCampaignActionQueueItem(ICampaignActionQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IQueueItemPriority.RemoveCampaignActionQueueItem(ICampaignActionQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDomainEventQueueItem> IQueueItemPriority.DomainEventQueueItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IQueueItemPriority.AddDomainEventQueueItem(IDomainEventQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IQueueItemPriority.RemoveDomainEventQueueItem(IDomainEventQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
