using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignEmail.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignEmailContracts))]
	public interface ICampaignEmail
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignEmailID for this CampaignEmail.
		/// </summary>
		int CampaignEmailID { get; set; }
	
		/// <summary>
		/// The CampaignID for this CampaignEmail.
		/// </summary>
		int CampaignID { get; set; }
	
		/// <summary>
		/// The EmailName for this CampaignEmail.
		/// </summary>
		string EmailName { get; set; }
	
		/// <summary>
		/// The DateToBeSentUTC for this CampaignEmail.
		/// </summary>
		Nullable<System.DateTime> DateToBeSentUTC { get; set; }
	
		/// <summary>
		/// The WaitTimeInDays for this CampaignEmail.
		/// </summary>
		Nullable<short> WaitTimeInDays { get; set; }
	
		/// <summary>
		/// The Active for this CampaignEmail.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The SentCount for this CampaignEmail.
		/// </summary>
		Nullable<int> SentCount { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Campaign for this CampaignEmail.
		/// </summary>
	    ICampaign Campaign { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignEmail))]
		internal abstract class CampaignEmailContracts : ICampaignEmail
		{
		    #region Primitive properties
		
			int ICampaignEmail.CampaignEmailID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignEmail.CampaignID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignEmail.EmailName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICampaignEmail.DateToBeSentUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ICampaignEmail.WaitTimeInDays
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaignEmail.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICampaignEmail.SentCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICampaign ICampaignEmail.Campaign
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
