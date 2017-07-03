using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for CampaignAction.
	/// </summary>
	[ContractClass(typeof(Contracts.CampaignActionContracts))]
	public interface ICampaignAction
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CampaignActionID for this CampaignAction.
		/// </summary>
		int CampaignActionID { get; set; }
	
		/// <summary>
		/// The CampaignActionTypeID for this CampaignAction.
		/// </summary>
		short CampaignActionTypeID { get; set; }
	
		/// <summary>
		/// The CampaignID for this CampaignAction.
		/// </summary>
		int CampaignID { get; set; }
	
		/// <summary>
		/// The Name for this CampaignAction.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The IntervalTimeUnitTypeID for this CampaignAction.
		/// </summary>
		Nullable<short> IntervalTimeUnitTypeID { get; set; }
	
		/// <summary>
		/// The Interval for this CampaignAction.
		/// </summary>
		Nullable<int> Interval { get; set; }
	
		/// <summary>
		/// The RunImmediately for this CampaignAction.
		/// </summary>
		bool RunImmediately { get; set; }
	
		/// <summary>
		/// The NextRunDateUTC for this CampaignAction.
		/// </summary>
		Nullable<System.DateTime> NextRunDateUTC { get; set; }
	
		/// <summary>
		/// The SortIndex for this CampaignAction.
		/// </summary>
		short SortIndex { get; set; }
	
		/// <summary>
		/// The Active for this CampaignAction.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsRunning for this CampaignAction.
		/// </summary>
		bool IsRunning { get; set; }
	
		/// <summary>
		/// The IsCompleted for this CampaignAction.
		/// </summary>
		bool IsCompleted { get; set; }
	
		/// <summary>
		/// The LastRunDateUTC for this CampaignAction.
		/// </summary>
		Nullable<System.DateTime> LastRunDateUTC { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Campaign for this CampaignAction.
		/// </summary>
	    ICampaign Campaign { get; set; }
	
		/// <summary>
		/// The CampaignActionType for this CampaignAction.
		/// </summary>
	    ICampaignActionType CampaignActionType { get; set; }
	
		/// <summary>
		/// The TimeUnitType for this CampaignAction.
		/// </summary>
	    ITimeUnitType TimeUnitType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AlertCampaignActions for this CampaignAction.
		/// </summary>
		IEnumerable<IAlertCampaignAction> AlertCampaignActions { get; }
	
		/// <summary>
		/// Adds an <see cref="IAlertCampaignAction"/> to the AlertCampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertCampaignAction"/> to add.</param>
		void AddAlertCampaignAction(IAlertCampaignAction item);
	
		/// <summary>
		/// Removes an <see cref="IAlertCampaignAction"/> from the AlertCampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertCampaignAction"/> to remove.</param>
		void RemoveAlertCampaignAction(IAlertCampaignAction item);
	
		/// <summary>
		/// The CampaignActionQueueItems for this CampaignAction.
		/// </summary>
		IEnumerable<ICampaignActionQueueItem> CampaignActionQueueItems { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignActionQueueItem"/> to the CampaignActionQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionQueueItem"/> to add.</param>
		void AddCampaignActionQueueItem(ICampaignActionQueueItem item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignActionQueueItem"/> from the CampaignActionQueueItems collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionQueueItem"/> to remove.</param>
		void RemoveCampaignActionQueueItem(ICampaignActionQueueItem item);
	
		/// <summary>
		/// The CampaignActionTokenValues for this CampaignAction.
		/// </summary>
		IEnumerable<ICampaignActionTokenValue> CampaignActionTokenValues { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignActionTokenValue"/> to the CampaignActionTokenValues collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionTokenValue"/> to add.</param>
		void AddCampaignActionTokenValue(ICampaignActionTokenValue item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignActionTokenValue"/> from the CampaignActionTokenValues collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionTokenValue"/> to remove.</param>
		void RemoveCampaignActionTokenValue(ICampaignActionTokenValue item);
	
		/// <summary>
		/// The EmailCampaignActions for this CampaignAction.
		/// </summary>
		IEnumerable<IEmailCampaignAction> EmailCampaignActions { get; }
	
		/// <summary>
		/// Adds an <see cref="IEmailCampaignAction"/> to the EmailCampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailCampaignAction"/> to add.</param>
		void AddEmailCampaignAction(IEmailCampaignAction item);
	
		/// <summary>
		/// Removes an <see cref="IEmailCampaignAction"/> from the EmailCampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailCampaignAction"/> to remove.</param>
		void RemoveEmailCampaignAction(IEmailCampaignAction item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICampaignAction))]
		internal abstract class CampaignActionContracts : ICampaignAction
		{
		    #region Primitive properties
		
			int ICampaignAction.CampaignActionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICampaignAction.CampaignActionTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICampaignAction.CampaignID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICampaignAction.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> ICampaignAction.IntervalTimeUnitTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICampaignAction.Interval
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaignAction.RunImmediately
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICampaignAction.NextRunDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICampaignAction.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaignAction.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaignAction.IsRunning
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICampaignAction.IsCompleted
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ICampaignAction.LastRunDateUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    ICampaign ICampaignAction.Campaign
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ICampaignActionType ICampaignAction.CampaignActionType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    ITimeUnitType ICampaignAction.TimeUnitType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAlertCampaignAction> ICampaignAction.AlertCampaignActions
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaignAction.AddAlertCampaignAction(IAlertCampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaignAction.RemoveAlertCampaignAction(IAlertCampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaignActionQueueItem> ICampaignAction.CampaignActionQueueItems
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaignAction.AddCampaignActionQueueItem(ICampaignActionQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaignAction.RemoveCampaignActionQueueItem(ICampaignActionQueueItem item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaignActionTokenValue> ICampaignAction.CampaignActionTokenValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaignAction.AddCampaignActionTokenValue(ICampaignActionTokenValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaignAction.RemoveCampaignActionTokenValue(ICampaignActionTokenValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IEmailCampaignAction> ICampaignAction.EmailCampaignActions
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICampaignAction.AddEmailCampaignAction(IEmailCampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICampaignAction.RemoveEmailCampaignAction(IEmailCampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
