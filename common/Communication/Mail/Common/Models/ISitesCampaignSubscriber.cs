using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for SitesCampaignSubscriber.
	/// </summary>
	[ContractClass(typeof(Contracts.SitesCampaignSubscriberContracts))]
	public interface ISitesCampaignSubscriber
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignSubscriberID for this SitesCampaignSubscriber.
		/// </summary>
		int CampaignSubscriberID { get; set; }
	
		/// <summary>
		/// The CampaignID for this SitesCampaignSubscriber.
		/// </summary>
		int CampaignID { get; set; }
	
		/// <summary>
		/// The AddedByAccountID for this SitesCampaignSubscriber.
		/// </summary>
		int AddedByAccountID { get; set; }
	
		/// <summary>
		/// The AccountID for this SitesCampaignSubscriber.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The DateAddedUTC for this SitesCampaignSubscriber.
		/// </summary>
		System.DateTime DateAddedUTC { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ISitesCampaignSubscriber))]
		internal abstract class SitesCampaignSubscriberContracts : ISitesCampaignSubscriber
		{
		    #region Primitive properties
		
			int ISitesCampaignSubscriber.CampaignSubscriberID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISitesCampaignSubscriber.CampaignID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISitesCampaignSubscriber.AddedByAccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ISitesCampaignSubscriber.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ISitesCampaignSubscriber.DateAddedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
