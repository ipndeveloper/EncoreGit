using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Marketing.Common.Models
{
	/// <summary>
	/// Common interface for NewsType.
	/// </summary>
	[ContractClass(typeof(Contracts.NewsTypeContracts))]
	public interface INewsType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NewsTypeID for this NewsType.
		/// </summary>
		short NewsTypeID { get; set; }
	
		/// <summary>
		/// The Name for this NewsType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this NewsType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this NewsType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this NewsType.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The ModifiedByUserID for this NewsType.
		/// </summary>
		Nullable<int> ModifiedByUserID { get; set; }
	
		/// <summary>
		/// The Editable for this NewsType.
		/// </summary>
		bool Editable { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The NewsTypeLanguageSorts for this NewsType.
		/// </summary>
		IEnumerable<INewsTypeLanguageSort> NewsTypeLanguageSorts { get; }
	
		/// <summary>
		/// Adds an <see cref="INewsTypeLanguageSort"/> to the NewsTypeLanguageSorts collection.
		/// </summary>
		/// <param name="item">The <see cref="INewsTypeLanguageSort"/> to add.</param>
		void AddNewsTypeLanguageSort(INewsTypeLanguageSort item);
	
		/// <summary>
		/// Removes an <see cref="INewsTypeLanguageSort"/> from the NewsTypeLanguageSorts collection.
		/// </summary>
		/// <param name="item">The <see cref="INewsTypeLanguageSort"/> to remove.</param>
		void RemoveNewsTypeLanguageSort(INewsTypeLanguageSort item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INewsType))]
		internal abstract class NewsTypeContracts : INewsType
		{
		    #region Primitive properties
		
			short INewsType.NewsTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INewsType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INewsType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string INewsType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INewsType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> INewsType.ModifiedByUserID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool INewsType.Editable
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<INewsTypeLanguageSort> INewsType.NewsTypeLanguageSorts
			{
				get { throw new NotImplementedException(); }
			}
		
			void INewsType.AddNewsTypeLanguageSort(INewsTypeLanguageSort item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void INewsType.RemoveNewsTypeLanguageSort(INewsTypeLanguageSort item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
