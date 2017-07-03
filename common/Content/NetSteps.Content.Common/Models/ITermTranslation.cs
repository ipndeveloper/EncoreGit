using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Content.Common.Models
{
	/// <summary>
	/// Common interface for TermTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.TermTranslationContracts))]
	public interface ITermTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The TermTranslationID for this TermTranslation.
		/// </summary>
		int TermTranslationID { get; set; }
	
		/// <summary>
		/// The LanguageID for this TermTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The TermName for this TermTranslation.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Term for this TermTranslation.
		/// </summary>
		string Term { get; set; }
	
		/// <summary>
		/// The LastUpdatedUTC for this TermTranslation.
		/// </summary>
		Nullable<System.DateTime> LastUpdatedUTC { get; set; }
	
		/// <summary>
		/// The Active for this TermTranslation.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(ITermTranslation))]
		internal abstract class TermTranslationContracts : ITermTranslation
		{
		    #region Primitive properties
		
			int ITermTranslation.TermTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int ITermTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITermTranslation.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string ITermTranslation.Term
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<System.DateTime> ITermTranslation.LastUpdatedUTC
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool ITermTranslation.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
