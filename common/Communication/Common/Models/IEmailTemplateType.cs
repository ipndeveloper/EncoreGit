using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailTemplateType.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailTemplateTypeContracts))]
	public interface IEmailTemplateType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailTemplateTypeID for this EmailTemplateType.
		/// </summary>
		short EmailTemplateTypeID { get; set; }
	
		/// <summary>
		/// The Name for this EmailTemplateType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this EmailTemplateType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this EmailTemplateType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this EmailTemplateType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The EmailTemplates for this EmailTemplateType.
		/// </summary>
		IEnumerable<IEmailTemplate> EmailTemplates { get; }
	
		/// <summary>
		/// Adds an <see cref="IEmailTemplate"/> to the EmailTemplates collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailTemplate"/> to add.</param>
		void AddEmailTemplate(IEmailTemplate item);
	
		/// <summary>
		/// Removes an <see cref="IEmailTemplate"/> from the EmailTemplates collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailTemplate"/> to remove.</param>
		void RemoveEmailTemplate(IEmailTemplate item);
	
		/// <summary>
		/// The Tokens for this EmailTemplateType.
		/// </summary>
		IEnumerable<IToken> Tokens { get; }
	
		/// <summary>
		/// Adds an <see cref="IToken"/> to the Tokens collection.
		/// </summary>
		/// <param name="item">The <see cref="IToken"/> to add.</param>
		void AddToken(IToken item);
	
		/// <summary>
		/// Removes an <see cref="IToken"/> from the Tokens collection.
		/// </summary>
		/// <param name="item">The <see cref="IToken"/> to remove.</param>
		void RemoveToken(IToken item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailTemplateType))]
		internal abstract class EmailTemplateTypeContracts : IEmailTemplateType
		{
		    #region Primitive properties
		
			short IEmailTemplateType.EmailTemplateTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IEmailTemplateType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IEmailTemplate> IEmailTemplateType.EmailTemplates
			{
				get { throw new NotImplementedException(); }
			}
		
			void IEmailTemplateType.AddEmailTemplate(IEmailTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IEmailTemplateType.RemoveEmailTemplate(IEmailTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IToken> IEmailTemplateType.Tokens
			{
				get { throw new NotImplementedException(); }
			}
		
			void IEmailTemplateType.AddToken(IToken item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IEmailTemplateType.RemoveToken(IToken item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
