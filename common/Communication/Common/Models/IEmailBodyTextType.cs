using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailBodyTextType.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailBodyTextTypeContracts))]
	public interface IEmailBodyTextType
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailBodyTextTypeID for this EmailBodyTextType.
		/// </summary>
		short EmailBodyTextTypeID { get; set; }
	
		/// <summary>
		/// The Name for this EmailBodyTextType.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The TermName for this EmailBodyTextType.
		/// </summary>
		string TermName { get; set; }
	
		/// <summary>
		/// The Description for this EmailBodyTextType.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this EmailBodyTextType.
		/// </summary>
		bool Active { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The EmailTemplates for this EmailBodyTextType.
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

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailBodyTextType))]
		internal abstract class EmailBodyTextTypeContracts : IEmailBodyTextType
		{
		    #region Primitive properties
		
			short IEmailBodyTextType.EmailBodyTextTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailBodyTextType.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailBodyTextType.TermName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailBodyTextType.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IEmailBodyTextType.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IEmailTemplate> IEmailBodyTextType.EmailTemplates
			{
				get { throw new NotImplementedException(); }
			}
		
			void IEmailBodyTextType.AddEmailTemplate(IEmailTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IEmailBodyTextType.RemoveEmailTemplate(IEmailTemplate item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
