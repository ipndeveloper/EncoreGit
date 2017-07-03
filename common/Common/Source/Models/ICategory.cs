using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Common interface for Category.
	/// </summary>
	[ContractClass(typeof(Contracts.CategoryContracts))]
	public interface ICategory
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CategoryID for this Category.
		/// </summary>
		int CategoryID { get; set; }
	
		/// <summary>
		/// The ParentCategoryID for this Category.
		/// </summary>
		Nullable<int> ParentCategoryID { get; set; }
	
		/// <summary>
		/// The SortIndex for this Category.
		/// </summary>
		int SortIndex { get; set; }
	
		/// <summary>
		/// The SlotCount for this Category.
		/// </summary>
		Nullable<int> SlotCount { get; set; }
	
		/// <summary>
		/// The CategoryTypeID for this Category.
		/// </summary>
		short CategoryTypeID { get; set; }
	
		/// <summary>
		/// The CategoryNumber for this Category.
		/// </summary>
		string CategoryNumber { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The ChildCategories for this Category.
		/// </summary>
		IEnumerable<ICategory> ChildCategories { get; }
	
		/// <summary>
		/// Adds an <see cref="ICategory"/> to the ChildCategories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to add.</param>
		void AddChildCategory(ICategory item);
	
		/// <summary>
		/// Removes an <see cref="ICategory"/> from the ChildCategories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to remove.</param>
		void RemoveChildCategory(ICategory item);
	
		/// <summary>
		/// The Translations for this Category.
		/// </summary>
		IEnumerable<ICategoryTranslation> Translations { get; }
	
		/// <summary>
		/// Adds an <see cref="ICategoryTranslation"/> to the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategoryTranslation"/> to add.</param>
		void AddTranslation(ICategoryTranslation item);
	
		/// <summary>
		/// Removes an <see cref="ICategoryTranslation"/> from the Translations collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategoryTranslation"/> to remove.</param>
		void RemoveTranslation(ICategoryTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICategory))]
		internal abstract class CategoryContracts : ICategory
		{
		    #region Primitive properties
		
			int ICategory.CategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICategory.ParentCategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICategory.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICategory.SlotCount
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short ICategory.CategoryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICategory.CategoryNumber
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICategory> ICategory.ChildCategories
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICategory.AddChildCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICategory.RemoveChildCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICategoryTranslation> ICategory.Translations
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICategory.AddTranslation(ICategoryTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICategory.RemoveTranslation(ICategoryTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
