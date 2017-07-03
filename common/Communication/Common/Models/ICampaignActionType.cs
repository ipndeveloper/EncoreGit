using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignActionType.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignActionTypeContracts))]
	public interface ICampaignActionType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignActionTypeID for this CampaignActionType.
		/// </summary>
		short CampaignActionTypeID { get; set; }
	
		/// <summary>
		/// The Name for this CampaignActionType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this CampaignActionType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this CampaignActionType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this CampaignActionType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignActions for this CampaignActionType.
		/// </summary>
		IEnumerable<ICampaignAction> CampaignActions { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignAction"/> to the CampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignAction"/> to add.</param>
		void AddCampaignAction(ICampaignAction item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignAction"/> from the CampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignAction"/> to remove.</param>
		void RemoveCampaignAction(ICampaignAction item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignActionType))]
		internal abstract class CampaignActionTypeContracts : ICampaignActionType
		{
		    #region Primitive properties
		
			short ICampaignActionType.CampaignActionTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignActionType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignActionType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignActionType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaignActionType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignAction> ICampaignActionType.CampaignActions
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaignActionType.AddCampaignAction(ICampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaignActionType.RemoveCampaignAction(ICampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
