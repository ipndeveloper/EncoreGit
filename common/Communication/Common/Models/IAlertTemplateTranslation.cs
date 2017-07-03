using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AlertTemplateTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.AlertTemplateTranslationContracts))]
	public interface IAlertTemplateTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AlertTemplateTranslationID for this AlertTemplateTranslation.
		/// </summary>
		int AlertTemplateTranslationID { get; set; }
	
		/// <summary>
		/// The AlertTemplateID for this AlertTemplateTranslation.
		/// </summary>
		int AlertTemplateID { get; set; }
	
		/// <summary>
		/// The LanguageID for this AlertTemplateTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The Message for this AlertTemplateTranslation.
		/// </summary>
		string Message { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AlertTemplate for this AlertTemplateTranslation.
		/// </summary>
	    IAlertTemplate AlertTemplate { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IAlertTemplateTranslation))]
		internal abstract class AlertTemplateTranslationContracts : IAlertTemplateTranslation
		{
		    #region Primitive properties
		
			int IAlertTemplateTranslation.AlertTemplateTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAlertTemplateTranslation.AlertTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IAlertTemplateTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAlertTemplateTranslation.Message
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAlertTemplate IAlertTemplateTranslation.AlertTemplate
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
