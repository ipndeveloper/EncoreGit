using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Products.Common.Models
{
	/// <summary>
	/// Common interface for ProductRelationsType.
	/// </summary>
	[ContractClass(typeof(Contracts.ProductRelationsTypeContracts))]
	public interface IProductRelationsType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The ProductRelationTypeID for this ProductRelationsType.
		/// </summary>
		int ProductRelationTypeID { get; set; }
	
		/// <summary>
		/// The Name for this ProductRelationsType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this ProductRelationsType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this ProductRelationsType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this ProductRelationsType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ProductPropertyTypeRelations for this ProductRelationsType.
		/// </summary>
		IEnumerable<IProductPropertyTypeRelation> ProductPropertyTypeRelations { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductPropertyTypeRelation"/> to the ProductPropertyTypeRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyTypeRelation"/> to add.</param>
		void AddProductPropertyTypeRelation(IProductPropertyTypeRelation item);
	
		/// <summary>
		/// Removes an <see cref="IProductPropertyTypeRelation"/> from the ProductPropertyTypeRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductPropertyTypeRelation"/> to remove.</param>
		void RemoveProductPropertyTypeRelation(IProductPropertyTypeRelation item);
	
		/// <summary>
		/// The ProductRelations for this ProductRelationsType.
		/// </summary>
		IEnumerable<IProductRelation> ProductRelations { get; }
	
		/// <summary>
		/// Adds an <see cref="IProductRelation"/> to the ProductRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductRelation"/> to add.</param>
		void AddProductRelation(IProductRelation item);
	
		/// <summary>
		/// Removes an <see cref="IProductRelation"/> from the ProductRelations collection.
		/// </summary>
		/// <param name="item">The <see cref="IProductRelation"/> to remove.</param>
		void RemoveProductRelation(IProductRelation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IProductRelationsType))]
		internal abstract class ProductRelationsTypeContracts : IProductRelationsType
		{
		    #region Primitive properties
		
			int IProductRelationsType.ProductRelationTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductRelationsType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductRelationsType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IProductRelationsType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IProductRelationsType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IProductPropertyTypeRelation> IProductRelationsType.ProductPropertyTypeRelations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductRelationsType.AddProductPropertyTypeRelation(IProductPropertyTypeRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductRelationsType.RemoveProductPropertyTypeRelation(IProductPropertyTypeRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IProductRelation> IProductRelationsType.ProductRelations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IProductRelationsType.AddProductRelation(IProductRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IProductRelationsType.RemoveProductRelation(IProductRelation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
