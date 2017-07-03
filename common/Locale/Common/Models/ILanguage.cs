using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Locale.Common.Models
{
	/// <summary>
	/// Common interface for Language.
	/// </summary>
	[ContractClass(typeof(Contracts.LanguageContracts))]
	public interface ILanguage
	{
	    #region Primitive properties
	
		/// <summary>
		/// The LanguageID for this Language.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The Name for this Language.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Language.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this Language.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The CultureInfo for this Language.
		/// </summary>
		string CultureInfo { get; set; }
	
		/// <summary>
		/// The Active for this Language.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The IsBase for this Language.
		/// </summary>
		Nullable<bool> IsBase { get; set; }
	
		/// <summary>
		/// The LanguageCode for this Language.
		/// </summary>
		string LanguageCode { get; set; }
	
		/// <summary>
		/// The LanguageCode3 for this Language.
		/// </summary>
		string LanguageCode3 { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ILanguage))]
		internal abstract class LanguageContracts : ILanguage
		{
		    #region Primitive properties
		
			int ILanguage.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILanguage.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILanguage.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILanguage.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILanguage.CultureInfo
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ILanguage.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> ILanguage.IsBase
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILanguage.LanguageCode
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ILanguage.LanguageCode3
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
