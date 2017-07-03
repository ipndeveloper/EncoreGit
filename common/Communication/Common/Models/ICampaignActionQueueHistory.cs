using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignActionQueueHistory.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignActionQueueHistoryContracts))]
	public interface ICampaignActionQueueHistory
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignActionQueueHistoryID for this CampaignActionQueueHistory.
		/// </summary>
		long CampaignActionQueueHistoryID { get; set; }
	
		/// <summary>
		/// The CampaignActionQueueItemID for this CampaignActionQueueHistory.
		/// </summary>
		long CampaignActionQueueItemID { get; set; }
	
		/// <summary>
		/// The QueueItemStatusID for this CampaignActionQueueHistory.
		/// </summary>
		short QueueItemStatusID { get; set; }
	
		/// <summary>
		/// The RunDateUTC for this CampaignActionQueueHistory.
		/// </summary>
		System.DateTime RunDateUTC { get; set; }
	
		/// <summary>
		/// The Result for this CampaignActionQueueHistory.
		/// </summary>
		string Result { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The CampaignActionQueueItem for this CampaignActionQueueHistory.
		/// </summary>
	    ICampaignActionQueueItem CampaignActionQueueItem { get; set; }
	
		/// <summary>
		/// The QueueItemStatus for this CampaignActionQueueHistory.
		/// </summary>
	    IQueueItemStatus QueueItemStatus { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignActionQueueHistory))]
		internal abstract class CampaignActionQueueHistoryContracts : ICampaignActionQueueHistory
		{
		    #region Primitive properties
		
			long ICampaignActionQueueHistory.CampaignActionQueueHistoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			long ICampaignActionQueueHistory.CampaignActionQueueItemID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICampaignActionQueueHistory.QueueItemStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ICampaignActionQueueHistory.RunDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignActionQueueHistory.Result
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICampaignActionQueueItem ICampaignActionQueueHistory.CampaignActionQueueItem
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IQueueItemStatus ICampaignActionQueueHistory.QueueItemStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
