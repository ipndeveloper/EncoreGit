using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailTemplateTranslation.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailTemplateTranslationContracts))]
	public interface IEmailTemplateTranslation
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailTemplateTranslationID for this EmailTemplateTranslation.
		/// </summary>
		int EmailTemplateTranslationID { get; set; }
	
		/// <summary>
		/// The EmailTemplateID for this EmailTemplateTranslation.
		/// </summary>
		int EmailTemplateID { get; set; }
	
		/// <summary>
		/// The LanguageID for this EmailTemplateTranslation.
		/// </summary>
		int LanguageID { get; set; }
	
		/// <summary>
		/// The Subject for this EmailTemplateTranslation.
		/// </summary>
		string Subject { get; set; }
	
		/// <summary>
		/// The Body for this EmailTemplateTranslation.
		/// </summary>
		string Body { get; set; }
	
		/// <summary>
		/// The FromAddress for this EmailTemplateTranslation.
		/// </summary>
		string FromAddress { get; set; }
	
		/// <summary>
		/// The AttachmentPath for this EmailTemplateTranslation.
		/// </summary>
		string AttachmentPath { get; set; }
	
		/// <summary>
		/// The Active for this EmailTemplateTranslation.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailTemplateTranslation))]
		internal abstract class EmailTemplateTranslationContracts : IEmailTemplateTranslation
		{
		    #region Primitive properties
		
			int IEmailTemplateTranslation.EmailTemplateTranslationID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IEmailTemplateTranslation.EmailTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IEmailTemplateTranslation.LanguageID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateTranslation.Subject
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateTranslation.Body
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateTranslation.FromAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateTranslation.AttachmentPath
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IEmailTemplateTranslation.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
