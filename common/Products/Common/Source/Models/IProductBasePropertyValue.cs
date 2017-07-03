using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductBasePropertyValue.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductBasePropertyValueContracts))]
	public interface IProductBasePropertyValue
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductBasePropertyValueID for this ProductBasePropertyValue.
		/// </summary>
		int ProductBasePropertyValueID { get; set; }
	
		/// <summary>
		/// The ProductBaseID for this ProductBasePropertyValue.
		/// </summary>
		int ProductBaseID { get; set; }
	
		/// <summary>
		/// The ProductPropertyValueID for this ProductBasePropertyValue.
		/// </summary>
		int ProductPropertyValueID { get; set; }
	
		/// <summary>
		/// The Value for this ProductBasePropertyValue.
		/// </summary>
		string Value { get; set; }
	
		/// <summary>
		/// The FilePath for this ProductBasePropertyValue.
		/// </summary>
		string FilePath { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The ProductBas for this ProductBasePropertyValue.
		/// </summary>
	    IProductBase ProductBas { get; set; }
	
		/// <summary>
		/// The ProductPropertyValue for this ProductBasePropertyValue.
		/// </summary>
	    IProductPropertyValue ProductPropertyValue { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductBasePropertyValue))]
		internal abstract class ProductBasePropertyValueContracts : IProductBasePropertyValue
		{
		    #region Primitive properties
		
			int IProductBasePropertyValue.ProductBasePropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductBasePropertyValue.ProductBaseID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductBasePropertyValue.ProductPropertyValueID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductBasePropertyValue.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductBasePropertyValue.FilePath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IProductBase IProductBasePropertyValue.ProductBas
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IProductPropertyValue IProductBasePropertyValue.ProductPropertyValue
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
