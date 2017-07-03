using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductType.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductTypeContracts))]
	public interface IProductType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductTypeID for this ProductType.
		/// </summary>
		int ProductTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ProductType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Active for this ProductType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The TermName for this ProductType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ProductType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Editable for this ProductType.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The DynamicKitGroupRules for this ProductType.
		/// </summary>
		IEnumerable<IDynamicKitGroupRule> DynamicKitGroupRules { get; }
	
		/// <summary>
		/// Adds an <see cref="IDynamicKitGroupRule"/> to the DynamicKitGroupRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKitGroupRule"/> to add.</param>
		void AddDynamicKitGroupRule(IDynamicKitGroupRule item);
	
		/// <summary>
		/// Removes an <see cref="IDynamicKitGroupRule"/> from the DynamicKitGroupRules collection.
		/// </summary>
		/// <param name="item">The <see cref="IDynamicKitGroupRule"/> to remove.</param>
		void RemoveDynamicKitGroupRule(IDynamicKitGroupRule item);
	
		/// <summary>
		/// The ProductBases for this ProductType.
		/// </summary>
		IEnumerable<IProductBase> ProductBases { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductBase"/> to the ProductBases collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBase"/> to add.</param>
		void AddProductBas(IProductBase item);
	
		/// <summary>
		/// Removes an <see cref="IProductBase"/> from the ProductBases collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductBase"/> to remove.</param>
		void RemoveProductBas(IProductBase item);
	
		/// <summary>
		/// The ProductPropertyTypes for this ProductType.
		/// </summary>
		IEnumerable<IProductPropertyType> ProductPropertyTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductPropertyType"/> to the ProductPropertyTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyType"/> to add.</param>
		void AddProductPropertyType(IProductPropertyType item);
	
		/// <summary>
		/// Removes an <see cref="IProductPropertyType"/> from the ProductPropertyTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyType"/> to remove.</param>
		void RemoveProductPropertyType(IProductPropertyType item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductType))]
		internal abstract class ProductTypeContracts : IProductType
		{
		    #region Primitive properties
		
			int IProductType.ProductTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IDynamicKitGroupRule> IProductType.DynamicKitGroupRules
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductType.AddDynamicKitGroupRule(IDynamicKitGroupRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductType.RemoveDynamicKitGroupRule(IDynamicKitGroupRule item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductBase> IProductType.ProductBases
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductType.AddProductBas(IProductBase item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductType.RemoveProductBas(IProductBase item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductPropertyType> IProductType.ProductPropertyTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductType.AddProductPropertyType(IProductPropertyType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductType.RemoveProductPropertyType(IProductPropertyType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
