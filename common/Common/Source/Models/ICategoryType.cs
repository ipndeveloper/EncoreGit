using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Common interface for CategoryType.
	/// </summary>
	[ContractClass(typeof(Contracts.CategoryTypeContracts))]
	public interface ICategoryType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CategoryTypeID for this CategoryType.
		/// </summary>
		short CategoryTypeID { get; set; }
	
		/// <summary>
		/// The Name for this CategoryType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this CategoryType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this CategoryType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this CategoryType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The Categories for this CategoryType.
		/// </summary>
		IEnumerable<ICategory> Categories { get; }
	
		/// <summary>
		/// Adds an <see cref="ICategory"/> to the Categories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to add.</param>
		void AddCategory(ICategory item);
	
		/// <summary>
		/// Removes an <see cref="ICategory"/> from the Categories collection.
		/// </summary>
		/// <param name="item">The <see cref="ICategory"/> to remove.</param>
		void RemoveCategory(ICategory item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICategoryType))]
		internal abstract class CategoryTypeContracts : ICategoryType
		{
		    #region Primitive properties
		
			short ICategoryType.CategoryTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICategoryType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICategoryType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICategoryType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ICategoryType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<ICategory> ICategoryType.Categories
			{
				get { throw new NotImplementedException(); }
			}
		
			void ICategoryType.AddCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void ICategoryType.RemoveCategory(ICategory item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
