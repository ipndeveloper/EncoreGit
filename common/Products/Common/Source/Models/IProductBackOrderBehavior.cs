using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductBackOrderBehavior.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductBackOrderBehaviorContracts))]
	public interface IProductBackOrderBehavior
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductBackOrderBehaviorID for this ProductBackOrderBehavior.
		/// </summary>
		short ProductBackOrderBehaviorID { get; set; }
	
		/// <summary>
		/// The Name for this ProductBackOrderBehavior.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ProductBackOrderBehavior.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ProductBackOrderBehavior.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ProductBackOrderBehavior.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsDefault for this ProductBackOrderBehavior.
		/// </summary>
		bool IsDefault { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Products for this ProductBackOrderBehavior.
		/// </summary>
		IEnumerable<IProduct> Products { get; }
	
		/// <summary>
		/// Adds an <see cref="IProduct"/> to the Products collection.
		/// </summary>
		/// <param name="item">The <see cref="IProduct"/> to add.</param>
		void AddProduct(IProduct item);
	
		/// <summary>
		/// Removes an <see cref="IProduct"/> from the Products collection.
		/// </summary>
		/// <param name="item">The <see cref="IProduct"/> to remove.</param>
		void RemoveProduct(IProduct item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductBackOrderBehavior))]
		internal abstract class ProductBackOrderBehaviorContracts : IProductBackOrderBehavior
		{
		    #region Primitive properties
		
			short IProductBackOrderBehavior.ProductBackOrderBehaviorID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductBackOrderBehavior.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductBackOrderBehavior.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductBackOrderBehavior.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBackOrderBehavior.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductBackOrderBehavior.IsDefault
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IProduct> IProductBackOrderBehavior.Products
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductBackOrderBehavior.AddProduct(IProduct item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductBackOrderBehavior.RemoveProduct(IProduct item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
