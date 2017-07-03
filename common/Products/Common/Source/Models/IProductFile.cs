using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductFile.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductFileContracts))]
	public interface IProductFile
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductFileID for this ProductFile.
		/// </summary>
		int ProductFileID { get; set; }
	
		/// <summary>
		/// The ProductFileTypeID for this ProductFile.
		/// </summary>
		int ProductFileTypeID { get; set; }
	
		/// <summary>
		/// The ProductID for this ProductFile.
		/// </summary>
		int ProductID { get; set; }
	
		/// <summary>
		/// The FilePath for this ProductFile.
		/// </summary>
		string FilePath { get; set; }
	
		/// <summary>
		/// The SortIndex for this ProductFile.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this ProductFile.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductFile))]
		internal abstract class ProductFileContracts : IProductFile
		{
		    #region Primitive properties
		
			int IProductFile.ProductFileID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductFile.ProductFileTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductFile.ProductID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductFile.FilePath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IProductFile.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IProductFile.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
