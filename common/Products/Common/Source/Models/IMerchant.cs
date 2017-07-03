using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using NetSteps.Addresses.Common.Models;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for Merchant.
	/// </summary>
	[ContractClass(typeof(Contracts.MerchantContracts))]
	public interface IMerchant
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MerchantID for this Merchant.
		/// </summary>
		int MerchantID { get; set; }
	
		/// <summary>
		/// The MerchantNumber for this Merchant.
		/// </summary>
		string MerchantNumber { get; set; }
	
		/// <summary>
		/// The BrandID for this Merchant.
		/// </summary>
		Nullable<int> BrandID { get; set; }
	
		/// <summary>
		/// The MerchantName for this Merchant.
		/// </summary>
		string MerchantName { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Brand for this Merchant.
		/// </summary>
	    IBrand Brand { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Addresses for this Merchant.
		/// </summary>
		IEnumerable<IAddress> Addresses { get; }
	
		/// <summary>
		/// Adds an <see cref="IAddress"/> to the Addresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddress"/> to add.</param>
		void AddAddress(IAddress item);
	
		/// <summary>
		/// Removes an <see cref="IAddress"/> from the Addresses collection.
		/// </summary>
		/// <param name="item">The <see cref="IAddress"/> to remove.</param>
		void RemoveAddress(IAddress item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMerchant))]
		internal abstract class MerchantContracts : IMerchant
		{
		    #region Primitive properties
		
			int IMerchant.MerchantID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMerchant.MerchantNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMerchant.BrandID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMerchant.MerchantName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IBrand IMerchant.Brand
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAddress> IMerchant.Addresses
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMerchant.AddAddress(IAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMerchant.RemoveAddress(IAddress item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
