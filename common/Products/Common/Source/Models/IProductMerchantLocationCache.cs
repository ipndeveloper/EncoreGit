using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductMerchantLocationCache.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductMerchantLocationCacheContracts))]
	public interface IProductMerchantLocationCache
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductMerchantLocationCacheID for this ProductMerchantLocationCache.
		/// </summary>
		int ProductMerchantLocationCacheID { get; set; }
	
		/// <summary>
		/// The ProductID for this ProductMerchantLocationCache.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The CategoryID for this ProductMerchantLocationCache.
		/// </summary>
		int CategoryID { get; set; }
	
		/// <summary>
		/// The MerchantName for this ProductMerchantLocationCache.
		/// </summary>
		string MerchantName { get; set; }
	
		/// <summary>
		/// The LongProductDescription for this ProductMerchantLocationCache.
		/// </summary>
		string LongProductDescription { get; set; }
	
		/// <summary>
		/// The LogoFileName for this ProductMerchantLocationCache.
		/// </summary>
		string LogoFileName { get; set; }
	
		/// <summary>
		/// The Award for this ProductMerchantLocationCache.
		/// </summary>
		string Award { get; set; }
	
		/// <summary>
		/// The MinimumPurchase for this ProductMerchantLocationCache.
		/// </summary>
		string MinimumPurchase { get; set; }
	
		/// <summary>
		/// The MaximumAwardPP for this ProductMerchantLocationCache.
		/// </summary>
		string MaximumAwardPP { get; set; }
	
		/// <summary>
		/// The AwardRating for this ProductMerchantLocationCache.
		/// </summary>
		string AwardRating { get; set; }
	
		/// <summary>
		/// The ExpressionType for this ProductMerchantLocationCache.
		/// </summary>
		string ExpressionType { get; set; }
	
		/// <summary>
		/// The Keywords for this ProductMerchantLocationCache.
		/// </summary>
		string Keywords { get; set; }
	
		/// <summary>
		/// The Latitude for this ProductMerchantLocationCache.
		/// </summary>
		Nullable<double> Latitude { get; set; }
	
		/// <summary>
		/// The Longitude for this ProductMerchantLocationCache.
		/// </summary>
		Nullable<double> Longitude { get; set; }
	
		/// <summary>
		/// The MerchantID for this ProductMerchantLocationCache.
		/// </summary>
		int MerchantID { get; set; }
	
		/// <summary>
		/// The AddressNumber for this ProductMerchantLocationCache.
		/// </summary>
		string AddressNumber { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductMerchantLocationCache))]
		internal abstract class ProductMerchantLocationCacheContracts : IProductMerchantLocationCache
		{
		    #region Primitive properties
		
			int IProductMerchantLocationCache.ProductMerchantLocationCacheID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductMerchantLocationCache.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductMerchantLocationCache.CategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.MerchantName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.LongProductDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.LogoFileName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.Award
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.MinimumPurchase
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.MaximumAwardPP
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.AwardRating
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.ExpressionType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.Keywords
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<double> IProductMerchantLocationCache.Latitude
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<double> IProductMerchantLocationCache.Longitude
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductMerchantLocationCache.MerchantID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductMerchantLocationCache.AddressNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
