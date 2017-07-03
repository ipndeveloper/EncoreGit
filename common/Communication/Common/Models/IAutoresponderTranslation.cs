using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AutoresponderTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.AutoresponderTranslationContracts))]
	public interface IAutoresponderTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AutoresponderTranslationID for this AutoresponderTranslation.
		/// </summary>
		int AutoresponderTranslationID { get; set; }
	
		/// <summary>
		/// The AutoresponderID for this AutoresponderTranslation.
		/// </summary>
		int AutoresponderID { get; set; }
	
		/// <summary>
		/// The LanguageID for this AutoresponderTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The DisplayText for this AutoresponderTranslation.
		/// </summary>
		string DisplayText { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAutoresponderTranslation))]
		internal abstract class AutoresponderTranslationContracts : IAutoresponderTranslation
		{
		    #region Primitive properties
		
			int IAutoresponderTranslation.AutoresponderTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoresponderTranslation.AutoresponderID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAutoresponderTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAutoresponderTranslation.DisplayText
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
