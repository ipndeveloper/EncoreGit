using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AlertCampaignAction.
	/// </summary>
	[ContractClass(typeof(Contracts.AlertCampaignActionContracts))]
	public interface IAlertCampaignAction
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AlertCampaignActionID for this AlertCampaignAction.
		/// </summary>
		int AlertCampaignActionID { get; set; }
	
		/// <summary>
		/// The CampaignActionID for this AlertCampaignAction.
		/// </summary>
		int CampaignActionID { get; set; }
	
		/// <summary>
		/// The AlertTemplateID for this AlertCampaignAction.
		/// </summary>
		int AlertTemplateID { get; set; }
	
		/// <summary>
		/// The DistributorEditableDateUTC for this AlertCampaignAction.
		/// </summary>
		Nullable<System.DateTime> DistributorEditableDateUTC { get; set; }
	
		/// <summary>
		/// The CanBeDismissed for this AlertCampaignAction.
		/// </summary>
		bool CanBeDismissed { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The CampaignAction for this AlertCampaignAction.
		/// </summary>
	    ICampaignAction CampaignAction { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAlertCampaignAction))]
		internal abstract class AlertCampaignActionContracts : IAlertCampaignAction
		{
		    #region Primitive properties
		
			int IAlertCampaignAction.AlertCampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAlertCampaignAction.CampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAlertCampaignAction.AlertTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> IAlertCampaignAction.DistributorEditableDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAlertCampaignAction.CanBeDismissed
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICampaignAction IAlertCampaignAction.CampaignAction
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
