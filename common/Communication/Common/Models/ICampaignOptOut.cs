using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignOptOut.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignOptOutContracts))]
	public interface ICampaignOptOut
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignOptOutID for this CampaignOptOut.
		/// </summary>
		int CampaignOptOutID { get; set; }
	
		/// <summary>
		/// The DomainEventTypeID for this CampaignOptOut.
		/// </summary>
		Nullable<short> DomainEventTypeID { get; set; }
	
		/// <summary>
		/// The OptOutTypeID for this CampaignOptOut.
		/// </summary>
		Nullable<short> OptOutTypeID { get; set; }
	
		/// <summary>
		/// The CampaignID for this CampaignOptOut.
		/// </summary>
		Nullable<int> CampaignID { get; set; }
	
		/// <summary>
		/// The AccountID for this CampaignOptOut.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The EmailAddress for this CampaignOptOut.
		/// </summary>
		string EmailAddress { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignOptOut))]
		internal abstract class CampaignOptOutContracts : ICampaignOptOut
		{
		    #region Primitive properties
		
			int ICampaignOptOut.CampaignOptOutID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ICampaignOptOut.DomainEventTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ICampaignOptOut.OptOutTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICampaignOptOut.CampaignID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICampaignOptOut.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignOptOut.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
