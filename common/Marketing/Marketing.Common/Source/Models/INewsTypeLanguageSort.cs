using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Marketing.Common.Models
{
	/// <summary>
	/// Common interface for NewsTypeLanguageSort.
	/// </summary>
	[ContractClass(typeof(Contracts.NewsTypeLanguageSortContracts))]
	public interface INewsTypeLanguageSort
	{
	    #region Primitive properties
	
		/// <summary>
		/// The NewsTypeID for this NewsTypeLanguageSort.
		/// </summary>
		short NewsTypeID { get; set; }
	
		/// <summary>
		/// The LanguageID for this NewsTypeLanguageSort.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The SortIndex for this NewsTypeLanguageSort.
		/// </summary>
		int SortIndex { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The NewsType for this NewsTypeLanguageSort.
		/// </summary>
	    INewsType NewsType { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(INewsTypeLanguageSort))]
		internal abstract class NewsTypeLanguageSortContracts : INewsTypeLanguageSort
		{
		    #region Primitive properties
		
			short INewsTypeLanguageSort.NewsTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INewsTypeLanguageSort.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int INewsTypeLanguageSort.SortIndex
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    INewsType INewsTypeLanguageSort.NewsType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
