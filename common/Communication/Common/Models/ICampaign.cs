using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for Campaign.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignContracts))]
	public interface ICampaign
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignID for this Campaign.
		/// </summary>
		int CampaignID { get; set; }
	
		/// <summary>
		/// The CampaignTypeID for this Campaign.
		/// </summary>
		short CampaignTypeID { get; set; }
	
		/// <summary>
		/// The DomainEventTypeID for this Campaign.
		/// </summary>
		Nullable<short> DomainEventTypeID { get; set; }
	
		/// <summary>
		/// The Name for this Campaign.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Description for this Campaign.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The DateCreatedUTC for this Campaign.
		/// </summary>
		System.DateTime DateCreatedUTC { get; set; }
	
		/// <summary>
		/// The CreatedByUserID for this Campaign.
		/// </summary>
		Nullable<int> CreatedByUserID { get; set; }
	
		/// <summary>
		/// The StartDateUTC for this Campaign.
		/// </summary>
		Nullable<System.DateTime> StartDateUTC { get; set; }
	
		/// <summary>
		/// The EndDateUTC for this Campaign.
		/// </summary>
		Nullable<System.DateTime> EndDateUTC { get; set; }
	
		/// <summary>
		/// The Active for this Campaign.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsCorporate for this Campaign.
		/// </summary>
		bool IsCorporate { get; set; }
	
		/// <summary>
		/// The MarketID for this Campaign.
		/// </summary>
		int MarketID { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignActions for this Campaign.
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
	
		/// <summary>
		/// The CampaignEmails for this Campaign.
		/// </summary>
		IEnumerable<ICampaignEmail> CampaignEmails { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignEmail"/> to the CampaignEmails collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignEmail"/> to add.</param>
		void AddCampaignEmail(ICampaignEmail item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignEmail"/> from the CampaignEmails collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignEmail"/> to remove.</param>
		void RemoveCampaignEmail(ICampaignEmail item);
	
		/// <summary>
		/// The CampaignOptOuts for this Campaign.
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
		/// The CampaignSubscribers for this Campaign.
		/// </summary>
		IEnumerable<ICampaignSubscriber> CampaignSubscribers { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignSubscriber"/> to the CampaignSubscribers collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignSubscriber"/> to add.</param>
		void AddCampaignSubscriber(ICampaignSubscriber item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignSubscriber"/> from the CampaignSubscribers collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignSubscriber"/> to remove.</param>
		void RemoveCampaignSubscriber(ICampaignSubscriber item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaign))]
		internal abstract class CampaignContracts : ICampaign
		{
		    #region Primitive properties
		
			int ICampaign.CampaignID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICampaign.CampaignTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ICampaign.DomainEventTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaign.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaign.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			System.DateTime ICampaign.DateCreatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICampaign.CreatedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICampaign.StartDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICampaign.EndDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaign.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaign.IsCorporate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaign.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignAction> ICampaign.CampaignActions
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaign.AddCampaignAction(ICampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaign.RemoveCampaignAction(ICampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaignEmail> ICampaign.CampaignEmails
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaign.AddCampaignEmail(ICampaignEmail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaign.RemoveCampaignEmail(ICampaignEmail item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaignOptOut> ICampaign.CampaignOptOuts
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaign.AddCampaignOptOut(ICampaignOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaign.RemoveCampaignOptOut(ICampaignOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaignSubscriber> ICampaign.CampaignSubscribers
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaign.AddCampaignSubscriber(ICampaignSubscriber item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaign.RemoveCampaignSubscriber(ICampaignSubscriber item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
