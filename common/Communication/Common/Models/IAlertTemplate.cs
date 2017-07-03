using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for AlertTemplate.
	/// </summary>
	[ContractClass(typeof(Contracts.AlertTemplateContracts))]
	public interface IAlertTemplate
	{
	    #region Primitive properties
	
		/// <summary>
		/// The AlertTemplateID for this AlertTemplate.
		/// </summary>
		int AlertTemplateID { get; set; }
	
		/// <summary>
		/// The Name for this AlertTemplate.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this AlertTemplate.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The StoredProcedureName for this AlertTemplate.
		/// </summary>
		string StoredProcedureName { get; set; }
	
		/// <summary>
		/// The Active for this AlertTemplate.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The AlertPriorityID for this AlertTemplate.
		/// </summary>
		short AlertPriorityID { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The AlertTemplateTranslations for this AlertTemplate.
		/// </summary>
		IEnumerable<IAlertTemplateTranslation> AlertTemplateTranslations { get; }
	
		/// <summary>
		/// Adds an <see cref="IAlertTemplateTranslation"/> to the AlertTemplateTranslations collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertTemplateTranslation"/> to add.</param>
		void AddAlertTemplateTranslation(IAlertTemplateTranslation item);
	
		/// <summary>
		/// Removes an <see cref="IAlertTemplateTranslation"/> from the AlertTemplateTranslations collection.
		/// </summary>
		/// <param name="item">The <see cref="IAlertTemplateTranslation"/> to remove.</param>
		void RemoveAlertTemplateTranslation(IAlertTemplateTranslation item);
	
		/// <summary>
		/// The Tokens for this AlertTemplate.
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
		[ContractClassFor(typeof(IAlertTemplate))]
		internal abstract class AlertTemplateContracts : IAlertTemplate
		{
		    #region Primitive properties
		
			int IAlertTemplate.AlertTemplateID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAlertTemplate.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAlertTemplate.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IAlertTemplate.StoredProcedureName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IAlertTemplate.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			short IAlertTemplate.AlertPriorityID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IAlertTemplateTranslation> IAlertTemplate.AlertTemplateTranslations
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAlertTemplate.AddAlertTemplateTranslation(IAlertTemplateTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAlertTemplate.RemoveAlertTemplateTranslation(IAlertTemplateTranslation item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			IEnumerable<IToken> IAlertTemplate.Tokens
			{
				get { throw new NotImplementedException(); }
			}
		
			void IAlertTemplate.AddToken(IToken item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IAlertTemplate.RemoveToken(IToken item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
