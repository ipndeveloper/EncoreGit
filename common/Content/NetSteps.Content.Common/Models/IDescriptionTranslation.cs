using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for DescriptionTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.DescriptionTranslationContracts))]
	public interface IDescriptionTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The DescriptionTranslationID for this DescriptionTranslation.
		/// </summary>
		int DescriptionTranslationID { get; set; }
	
		/// <summary>
		/// The LanguageID for this DescriptionTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The Name for this DescriptionTranslation.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The ShortDescription for this DescriptionTranslation.
		/// </summary>
		string ShortDescription { get; set; }
	
		/// <summary>
		/// The LongDescription for this DescriptionTranslation.
		/// </summary>
		string LongDescription { get; set; }
	
		/// <summary>
		/// The ImagePath for this DescriptionTranslation.
		/// </summary>
		string ImagePath { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IDescriptionTranslation))]
		internal abstract class DescriptionTranslationContracts : IDescriptionTranslation
		{
		    #region Primitive properties
		
			int IDescriptionTranslation.DescriptionTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IDescriptionTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDescriptionTranslation.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDescriptionTranslation.ShortDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDescriptionTranslation.LongDescription
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IDescriptionTranslation.ImagePath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
