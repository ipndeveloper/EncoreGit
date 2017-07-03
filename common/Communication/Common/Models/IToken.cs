using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for Token.
	/// </summary>
	[ContractClass(typeof(Contracts.TokenContracts))]
	public interface IToken
	{
	    #region Primitive properties
	
		/// <summary>
		/// The TokenID for this Token.
		/// </summary>
		int TokenID { get; set; }
	
		/// <summary>
		/// The Name for this Token.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this Token.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Placeholder for this Token.
		/// </summary>
		string Placeholder { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AlertTemplates for this Token.
		/// </summary>
		IEnumerable<IAlertTemplate> AlertTemplates { get; }
	
		/// <summary>
		/// Adds an <see cref="IAlertTemplate"/> to the AlertTemplates collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertTemplate"/> to add.</param>
		void AddAlertTemplate(IAlertTemplate item);
	
		/// <summary>
		/// Removes an <see cref="IAlertTemplate"/> from the AlertTemplates collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertTemplate"/> to remove.</param>
		void RemoveAlertTemplate(IAlertTemplate item);
	
		/// <summary>
		/// The CampaignActionTokenValues for this Token.
		/// </summary>
		IEnumerable<ICampaignActionTokenValue> CampaignActionTokenValues { get; }
	
		/// <summary>
		/// Adds an <see cref="ICampaignActionTokenValue"/> to the CampaignActionTokenValues collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionTokenValue"/> to add.</param>
		void AddCampaignActionTokenValue(ICampaignActionTokenValue item);
	
		/// <summary>
		/// Removes an <see cref="ICampaignActionTokenValue"/> from the CampaignActionTokenValues collection.
		/// </summary>
		/// <param name="item">The <see cref="ICampaignActionTokenValue"/> to remove.</param>
		void RemoveCampaignActionTokenValue(ICampaignActionTokenValue item);
	
		/// <summary>
		/// The EmailTemplateTypes for this Token.
		/// </summary>
		IEnumerable<IEmailTemplateType> EmailTemplateTypes { get; }
	
		/// <summary>
		/// Adds an <see cref="IEmailTemplateType"/> to the EmailTemplateTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailTemplateType"/> to add.</param>
		void AddEmailTemplateType(IEmailTemplateType item);
	
		/// <summary>
		/// Removes an <see cref="IEmailTemplateType"/> from the EmailTemplateTypes collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailTemplateType"/> to remove.</param>
		void RemoveEmailTemplateType(IEmailTemplateType item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IToken))]
		internal abstract class TokenContracts : IToken
		{
		    #region Primitive properties
		
			int IToken.TokenID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IToken.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IToken.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IToken.Placeholder
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAlertTemplate> IToken.AlertTemplates
			{
				get { throw new NotImplementedException(); }
			}
		
			void IToken.AddAlertTemplate(IAlertTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IToken.RemoveAlertTemplate(IAlertTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<ICampaignActionTokenValue> IToken.CampaignActionTokenValues
			{
				get { throw new NotImplementedException(); }
			}
		
			void IToken.AddCampaignActionTokenValue(ICampaignActionTokenValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IToken.RemoveCampaignActionTokenValue(ICampaignActionTokenValue item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IEmailTemplateType> IToken.EmailTemplateTypes
			{
				get { throw new NotImplementedException(); }
			}
		
			void IToken.AddEmailTemplateType(IEmailTemplateType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IToken.RemoveEmailTemplateType(IEmailTemplateType item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
