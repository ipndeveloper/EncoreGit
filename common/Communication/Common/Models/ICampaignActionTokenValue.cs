using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignActionTokenValue.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignActionTokenValueContracts))]
	public interface ICampaignActionTokenValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignActionTokenValueID for this CampaignActionTokenValue.
		/// </summary>
		long CampaignActionTokenValueID { get; set; }
	
		/// <summary>
		/// The TokenID for this CampaignActionTokenValue.
		/// </summary>
		int TokenID { get; set; }
	
		/// <summary>
		/// The AccountID for this CampaignActionTokenValue.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The LanguageID for this CampaignActionTokenValue.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The PlaceholderValue for this CampaignActionTokenValue.
		/// </summary>
		string PlaceholderValue { get; set; }
	
		/// <summary>
		/// The CampaignActionID for this CampaignActionTokenValue.
		/// </summary>
		int CampaignActionID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignActionTokenValue))]
		internal abstract class CampaignActionTokenValueContracts : ICampaignActionTokenValue
		{
		    #region Primitive properties
		
			long ICampaignActionTokenValue.CampaignActionTokenValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignActionTokenValue.TokenID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICampaignActionTokenValue.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignActionTokenValue.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignActionTokenValue.PlaceholderValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignActionTokenValue.CampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
