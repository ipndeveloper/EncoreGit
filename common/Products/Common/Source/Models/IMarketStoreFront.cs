using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for MarketStoreFront.
	/// </summary>
	[ContractClass(typeof(Contracts.MarketStoreFrontContracts))]
	public interface IMarketStoreFront
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MarketID for this MarketStoreFront.
		/// </summary>
		int MarketID { get; set; }
	
		/// <summary>
		/// The StoreFrontID for this MarketStoreFront.
		/// </summary>
		int StoreFrontID { get; set; }
	
		/// <summary>
		/// The SiteTypeID for this MarketStoreFront.
		/// </summary>
		short SiteTypeID { get; set; }
	
		/// <summary>
		/// The MarketStoreFrontID for this MarketStoreFront.
		/// </summary>
		int MarketStoreFrontID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMarketStoreFront))]
		internal abstract class MarketStoreFrontContracts : IMarketStoreFront
		{
		    #region Primitive properties
		
			int IMarketStoreFront.MarketID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMarketStoreFront.StoreFrontID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IMarketStoreFront.SiteTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMarketStoreFront.MarketStoreFrontID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
