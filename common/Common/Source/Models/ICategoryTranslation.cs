using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Common.Models
{
	/// <summary>
	/// Common interface for CategoryTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.CategoryTranslationContracts))]
	public interface ICategoryTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The CategoryTranslationID for this CategoryTranslation.
		/// </summary>
		int CategoryTranslationID { get; set; }
	
		/// <summary>
		/// The CategoryID for this CategoryTranslation.
		/// </summary>
		int CategoryID { get; set; }
	
		/// <summary>
		/// The LanguageID for this CategoryTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The Name for this CategoryTranslation.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The HtmlContentID for this CategoryTranslation.
		/// </summary>
		Nullable<int> HtmlContentID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ICategoryTranslation))]
		internal abstract class CategoryTranslationContracts : ICategoryTranslation
		{
		    #region Primitive properties
		
			int ICategoryTranslation.CategoryTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICategoryTranslation.CategoryID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ICategoryTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ICategoryTranslation.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> ICategoryTranslation.HtmlContentID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
