using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignSubscriber.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignSubscriberContracts))]
	public interface ICampaignSubscriber
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignSubscriberID for this CampaignSubscriber.
		/// </summary>
		int CampaignSubscriberID { get; set; }
	
		/// <summary>
		/// The CampaignID for this CampaignSubscriber.
		/// </summary>
		int CampaignID { get; set; }
	
		/// <summary>
		/// The AddedByAccountID for this CampaignSubscriber.
		/// </summary>
		int AddedByAccountID { get; set; }
	
		/// <summary>
		/// The AccountID for this CampaignSubscriber.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The DateAddedUTC for this CampaignSubscriber.
		/// </summary>
		System.DateTime DateAddedUTC { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignSubscriber))]
		internal abstract class CampaignSubscriberContracts : ICampaignSubscriber
		{
		    #region Primitive properties
		
			int ICampaignSubscriber.CampaignSubscriberID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignSubscriber.CampaignID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignSubscriber.AddedByAccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignSubscriber.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ICampaignSubscriber.DateAddedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
