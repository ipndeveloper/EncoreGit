using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductFileType.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductFileTypeContracts))]
	public interface IProductFileType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductFileTypeID for this ProductFileType.
		/// </summary>
		int ProductFileTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ProductFileType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ProductFileType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ProductFileType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ProductFileType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The Editable for this ProductFileType.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ProductFiles for this ProductFileType.
		/// </summary>
		IEnumerable<IProductFile> ProductFiles { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductFile"/> to the ProductFiles collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductFile"/> to add.</param>
		void AddProductFile(IProductFile item);
	
		/// <summary>
		/// Removes an <see cref="IProductFile"/> from the ProductFiles collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductFile"/> to remove.</param>
		void RemoveProductFile(IProductFile item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductFileType))]
		internal abstract class ProductFileTypeContracts : IProductFileType
		{
		    #region Primitive properties
		
			int IProductFileType.ProductFileTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductFileType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductFileType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductFileType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductFileType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductFileType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IProductFile> IProductFileType.ProductFiles
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductFileType.AddProductFile(IProductFile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductFileType.RemoveProductFile(IProductFile item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
