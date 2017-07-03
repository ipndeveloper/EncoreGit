using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignActionQueueItem.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignActionQueueItemContracts))]
	public interface ICampaignActionQueueItem
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignActionQueueItemID for this CampaignActionQueueItem.
		/// </summary>
		long CampaignActionQueueItemID { get; set; }
	
		/// <summary>
		/// The QueueItemStatusID for this CampaignActionQueueItem.
		/// </summary>
		short QueueItemStatusID { get; set; }
	
		/// <summary>
		/// The QueueItemPriorityID for this CampaignActionQueueItem.
		/// </summary>
		short QueueItemPriorityID { get; set; }
	
		/// <summary>
		/// The CampaignActionID for this CampaignActionQueueItem.
		/// </summary>
		int CampaignActionID { get; set; }
	
		/// <summary>
		/// The EventContextID for this CampaignActionQueueItem.
		/// </summary>
		int EventContextID { get; set; }
	
		/// <summary>
		/// The AttemptCount for this CampaignActionQueueItem.
		/// </summary>
		byte AttemptCount { get; set; }
	
		/// <summary>
		/// The LastRunDateUTC for this CampaignActionQueueItem.
		/// </summary>
		Nullable<System.DateTime> LastRunDateUTC { get; set; }
	
		/// <summary>
		/// The NextRunDateUTC for this CampaignActionQueueItem.
		/// </summary>
		Nullable<System.DateTime> NextRunDateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The CampaignAction for this CampaignActionQueueItem.
		/// </summary>
	    ICampaignAction CampaignAction { get; set; }
	
		/// <summary>
		/// The EventContext for this CampaignActionQueueItem.
		/// </summary>
	    IEventContext EventContext { get; set; }
	
		/// <summary>
		/// The QueueItemPriority for this CampaignActionQueueItem.
		/// </summary>
	    IQueueItemPriority QueueItemPriority { get; set; }
	
		/// <summary>
		/// The QueueItemStatus for this CampaignActionQueueItem.
		/// </summary>
	    IQueueItemStatus QueueItemStatus { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignActionQueueHistories for this CampaignActionQueueItem.
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

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignActionQueueItem))]
		internal abstract class CampaignActionQueueItemContracts : ICampaignActionQueueItem
		{
		    #region Primitive properties
		
			long ICampaignActionQueueItem.CampaignActionQueueItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICampaignActionQueueItem.QueueItemStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICampaignActionQueueItem.QueueItemPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignActionQueueItem.CampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignActionQueueItem.EventContextID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			byte ICampaignActionQueueItem.AttemptCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICampaignActionQueueItem.LastRunDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICampaignActionQueueItem.NextRunDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICampaignAction ICampaignActionQueueItem.CampaignAction
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IEventContext ICampaignActionQueueItem.EventContext
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IQueueItemPriority ICampaignActionQueueItem.QueueItemPriority
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IQueueItemStatus ICampaignActionQueueItem.QueueItemStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignActionQueueHistory> ICampaignActionQueueItem.CampaignActionQueueHistories
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaignActionQueueItem.AddCampaignActionQueueHistory(ICampaignActionQueueHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaignActionQueueItem.RemoveCampaignActionQueueHistory(ICampaignActionQueueHistory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
