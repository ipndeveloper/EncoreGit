using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailTemplate.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailTemplateContracts))]
	public interface IEmailTemplate
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailTemplateID for this EmailTemplate.
		/// </summary>
		int EmailTemplateID { get; set; }
	
		/// <summary>
		/// The EmailTemplateTypeID for this EmailTemplate.
		/// </summary>
		short EmailTemplateTypeID { get; set; }
	
		/// <summary>
		/// The EmailBodyTextTypeID for this EmailTemplate.
		/// </summary>
		Nullable<short> EmailBodyTextTypeID { get; set; }
	
		/// <summary>
		/// The Name for this EmailTemplate.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Hits for this EmailTemplate.
		/// </summary>
		Nullable<int> Hits { get; set; }
	
		/// <summary>
		/// The Active for this EmailTemplate.
		/// </summary>
		Nullable<bool> Active { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The EmailBodyTextType for this EmailTemplate.
		/// </summary>
	    IEmailBodyTextType EmailBodyTextType { get; set; }
	
		/// <summary>
		/// The EmailTemplateType for this EmailTemplate.
		/// </summary>
	    IEmailTemplateType EmailTemplateType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The EmailCampaignActions for this EmailTemplate.
		/// </summary>
		IEnumerable<IEmailCampaignAction> EmailCampaignActions { get; }
	
		/// <summary>
		/// Adds an <see cref="IEmailCampaignAction"/> to the EmailCampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailCampaignAction"/> to add.</param>
		void AddEmailCampaignAction(IEmailCampaignAction item);
	
		/// <summary>
		/// Removes an <see cref="IEmailCampaignAction"/> from the EmailCampaignActions collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailCampaignAction"/> to remove.</param>
		void RemoveEmailCampaignAction(IEmailCampaignAction item);
	
		/// <summary>
		/// The EmailTemplateTranslations for this EmailTemplate.
		/// </summary>
		IEnumerable<IEmailTemplateTranslation> EmailTemplateTranslations { get; }
	
		/// <summary>
		/// Adds an <see cref="IEmailTemplateTranslation"/> to the EmailTemplateTranslations collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailTemplateTranslation"/> to add.</param>
		void AddEmailTemplateTranslation(IEmailTemplateTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IEmailTemplateTranslation"/> from the EmailTemplateTranslations collection.
		/// </summary>
		/// <param name="item">The <see cref="IEmailTemplateTranslation"/> to remove.</param>
		void RemoveEmailTemplateTranslation(IEmailTemplateTranslation item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailTemplate))]
		internal abstract class EmailTemplateContracts : IEmailTemplate
		{
		    #region Primitive properties
		
			int IEmailTemplate.EmailTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IEmailTemplate.EmailTemplateTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IEmailTemplate.EmailBodyTextTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplate.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEmailTemplate.Hits
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<bool> IEmailTemplate.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IEmailBodyTextType IEmailTemplate.EmailBodyTextType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IEmailTemplateType IEmailTemplate.EmailTemplateType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IEmailCampaignAction> IEmailTemplate.EmailCampaignActions
			{
				get { throw new NotImplementedException(); }
			}
		
			void IEmailTemplate.AddEmailCampaignAction(IEmailCampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IEmailTemplate.RemoveEmailCampaignAction(IEmailCampaignAction item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IEmailTemplateTranslation> IEmailTemplate.EmailTemplateTranslations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IEmailTemplate.AddEmailTemplateTranslation(IEmailTemplateTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IEmailTemplate.RemoveEmailTemplateTranslation(IEmailTemplateTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
