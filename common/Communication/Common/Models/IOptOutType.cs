using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for OptOutType.
	/// </summary>
	[ContractClass(typeof(Contracts.OptOutTypeContracts))]
	public interface IOptOutType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The OptOutTypeID for this OptOutType.
		/// </summary>
		short OptOutTypeID { get; set; }
	
		/// <summary>
		/// The Name for this OptOutType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this OptOutType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this OptOutType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this OptOutType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The CampaignOptOuts for this OptOutType.
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
		/// The OptOuts for this OptOutType.
		/// </summary>
		IEnumerable<IOptOut> OptOuts { get; }
	
		/// <summary>
		/// Adds an <see cref="IOptOut"/> to the OptOuts collection.
		/// </summary>
		/// <param name="item">The <see cref="IOptOut"/> to add.</param>
		void AddOptOut(IOptOut item);
	
		/// <summary>
		/// Removes an <see cref="IOptOut"/> from the OptOuts collection.
		/// </summary>
		/// <param name="item">The <see cref="IOptOut"/> to remove.</param>
		void RemoveOptOut(IOptOut item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IOptOutType))]
		internal abstract class OptOutTypeContracts : IOptOutType
		{
		    #region Primitive properties
		
			short IOptOutType.OptOutTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOptOutType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOptOutType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IOptOutType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IOptOutType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICampaignOptOut> IOptOutType.CampaignOptOuts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOptOutType.AddCampaignOptOut(ICampaignOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOptOutType.RemoveCampaignOptOut(ICampaignOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IOptOut> IOptOutType.OptOuts
			{
				get { throw new NotImplementedException(); }
			}
		
			void IOptOutType.AddOptOut(IOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IOptOutType.RemoveOptOut(IOptOut item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
