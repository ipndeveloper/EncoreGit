using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailCampaignAction.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailCampaignActionContracts))]
	public interface IEmailCampaignAction
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailCampaignActionID for this EmailCampaignAction.
		/// </summary>
		int EmailCampaignActionID { get; set; }
	
		/// <summary>
		/// The EmailTemplateID for this EmailCampaignAction.
		/// </summary>
		int EmailTemplateID { get; set; }
	
		/// <summary>
		/// The CampaignActionID for this EmailCampaignAction.
		/// </summary>
		int CampaignActionID { get; set; }
	
		/// <summary>
		/// The DistributorEditableDateUTC for this EmailCampaignAction.
		/// </summary>
		Nullable<System.DateTime> DistributorEditableDateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The CampaignAction for this EmailCampaignAction.
		/// </summary>
	    ICampaignAction CampaignAction { get; set; }
	
		/// <summary>
		/// The EmailTemplate for this EmailCampaignAction.
		/// </summary>
	    IEmailTemplate EmailTemplate { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailCampaignAction))]
		internal abstract class EmailCampaignActionContracts : IEmailCampaignAction
		{
		    #region Primitive properties
		
			int IEmailCampaignAction.EmailCampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IEmailCampaignAction.EmailTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IEmailCampaignAction.CampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IEmailCampaignAction.DistributorEditableDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICampaignAction IEmailCampaignAction.CampaignAction
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IEmailTemplate IEmailCampaignAction.EmailTemplate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
