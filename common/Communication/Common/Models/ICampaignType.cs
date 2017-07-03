using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignType.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignTypeContracts))]
	public interface ICampaignType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignTypeID for this CampaignType.
		/// </summary>
		short CampaignTypeID { get; set; }
	
		/// <summary>
		/// The Name for this CampaignType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this CampaignType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this CampaignType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this CampaignType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Campaigns for this CampaignType.
		/// </summary>
		IEnumerable<ICampaign> Campaigns { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaign"/> to the Campaigns collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaign"/> to add.</param>
		void AddCampaign(ICampaign item);
	
		/// <summary>
		/// Removes an <see cref="ICampaign"/> from the Campaigns collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaign"/> to remove.</param>
		void RemoveCampaign(ICampaign item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignType))]
		internal abstract class CampaignTypeContracts : ICampaignType
		{
		    #region Primitive properties
		
			short ICampaignType.CampaignTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaignType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaign> ICampaignType.Campaigns
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaignType.AddCampaign(ICampaign item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaignType.RemoveCampaign(ICampaign item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
