using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for TimeUnitType.
	/// </summary>
	[ContractClass(typeof(Contracts.TimeUnitTypeContracts))]
	public interface ITimeUnitType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The TimeUnitTypeID for this TimeUnitType.
		/// </summary>
		short TimeUnitTypeID { get; set; }
	
		/// <summary>
		/// The Name for this TimeUnitType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this TimeUnitType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this TimeUnitType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this TimeUnitType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignActions for this TimeUnitType.
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
		[ContractClassFor(typeof(ITimeUnitType))]
		internal abstract class TimeUnitTypeContracts : ITimeUnitType
		{
		    #region Primitive properties
		
			short ITimeUnitType.TimeUnitTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITimeUnitType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITimeUnitType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITimeUnitType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ITimeUnitType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignAction> ITimeUnitType.CampaignActions
			{
				get { throw new NotImplementedException(); }
			}
		
			void ITimeUnitType.AddCampaignAction(ICampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ITimeUnitType.RemoveCampaignAction(ICampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
