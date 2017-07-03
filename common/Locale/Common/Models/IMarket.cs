using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Locale.Common.Models
{
	/// <summary>
	/// Common interface for Market.
	/// </summary>
	[ContractClass(typeof(Contracts.MarketContracts))]
	public interface IMarket
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MarketID for this Market.
		/// </summary>
		int MarketID { get; set; }
	
		/// <summary>
		/// The Name for this Market.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Market.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this Market.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this Market.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The PickupPointsEnabled for this Market.
		/// </summary>
		bool PickupPointsEnabled { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMarket))]
		internal abstract class MarketContracts : IMarket
		{
		    #region Primitive properties
		
			int IMarket.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMarket.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMarket.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMarket.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMarket.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMarket.PickupPointsEnabled
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
