using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for DomainEventType.
	/// </summary>
	[ContractClass(typeof(Contracts.DomainEventTypeContracts))]
	public interface IDomainEventType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DomainEventTypeID for this DomainEventType.
		/// </summary>
		short DomainEventTypeID { get; set; }
	
		/// <summary>
		/// The Name for this DomainEventType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this DomainEventType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this DomainEventType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this DomainEventType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The DomainEventTypeCategoryID for this DomainEventType.
		/// </summary>
		int DomainEventTypeCategoryID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The DomainEventTypeCategory for this DomainEventType.
		/// </summary>
	    IDomainEventTypeCategory DomainEventTypeCategory { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignOptOuts for this DomainEventType.
		/// </summary>
		IEnumerable<ICampaignOptOut> CampaignOptOuts { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignOptOut"/> to the CampaignOptOuts collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignOptOut"/> to add.</param>
		void AddCampaignOptOut(ICampaignOptOut item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignOptOut"/> from the CampaignOptOuts collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignOptOut"/> to remove.</param>
		void RemoveCampaignOptOut(ICampaignOptOut item);
	
		/// <summary>
		/// The Campaigns for this DomainEventType.
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
	
		/// <summary>
		/// The DomainEventQueueItems for this DomainEventType.
		/// </summary>
		IEnumerable<IDomainEventQueueItem> DomainEventQueueItems { get; }
	
		/// <summary>
		/// Adds an <see cref="IDomainEventQueueItem"/> to the DomainEventQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IDomainEventQueueItem"/> to add.</param>
		void AddDomainEventQueueItem(IDomainEventQueueItem item);
	
		/// <summary>
		/// Removes an <see cref="IDomainEventQueueItem"/> from the DomainEventQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="IDomainEventQueueItem"/> to remove.</param>
		void RemoveDomainEventQueueItem(IDomainEventQueueItem item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDomainEventType))]
		internal abstract class DomainEventTypeContracts : IDomainEventType
		{
		    #region Primitive properties
		
			short IDomainEventType.DomainEventTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDomainEventType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDomainEventType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDomainEventType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IDomainEventType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDomainEventType.DomainEventTypeCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IDomainEventTypeCategory IDomainEventType.DomainEventTypeCategory
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignOptOut> IDomainEventType.CampaignOptOuts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDomainEventType.AddCampaignOptOut(ICampaignOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDomainEventType.RemoveCampaignOptOut(ICampaignOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaign> IDomainEventType.Campaigns
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDomainEventType.AddCampaign(ICampaign item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDomainEventType.RemoveCampaign(ICampaign item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IDomainEventQueueItem> IDomainEventType.DomainEventQueueItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void IDomainEventType.AddDomainEventQueueItem(IDomainEventQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IDomainEventType.RemoveDomainEventQueueItem(IDomainEventQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
