using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for AccountPriceType.
	/// </summary>
	[ContractClass(typeof(Contracts.AccountPriceTypeContracts))]
	public interface IAccountPriceType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AccountPriceTypeID for this AccountPriceType.
		/// </summary>
		int AccountPriceTypeID { get; set; }
	
		/// <summary>
		/// The ProductPriceTypeID for this AccountPriceType.
		/// </summary>
		int ProductPriceTypeID { get; set; }
	
		/// <summary>
		/// The AccountTypeID for this AccountPriceType.
		/// </summary>
		short AccountTypeID { get; set; }
	
		/// <summary>
		/// The PriceRelationshipTypeID for this AccountPriceType.
		/// </summary>
		int PriceRelationshipTypeID { get; set; }
	
		/// <summary>
		/// The StoreFrontID for this AccountPriceType.
		/// </summary>
		int StoreFrontID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The PriceRelationshipType for this AccountPriceType.
		/// </summary>
	    IPriceRelationshipType PriceRelationshipType { get; set; }
	
		/// <summary>
		/// The ProductPriceType for this AccountPriceType.
		/// </summary>
	    IProductPriceType ProductPriceType { get; set; }
	
		/// <summary>
		/// The StoreFront for this AccountPriceType.
		/// </summary>
	    IStoreFront StoreFront { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAccountPriceType))]
		internal abstract class AccountPriceTypeContracts : IAccountPriceType
		{
		    #region Primitive properties
		
			int IAccountPriceType.AccountPriceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPriceType.ProductPriceTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAccountPriceType.AccountTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPriceType.PriceRelationshipTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAccountPriceType.StoreFrontID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IPriceRelationshipType IAccountPriceType.PriceRelationshipType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductPriceType IAccountPriceType.ProductPriceType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IStoreFront IAccountPriceType.StoreFront
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
