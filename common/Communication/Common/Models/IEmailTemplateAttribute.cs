using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailTemplateAttribute.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailTemplateAttributeContracts))]
	public interface IEmailTemplateAttribute
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailTemplateAttributeID for this EmailTemplateAttribute.
		/// </summary>
		int EmailTemplateAttributeID { get; set; }
	
		/// <summary>
		/// The Name for this EmailTemplateAttribute.
		/// </summary>
		string Name { get; set; }
	
		/// <summary>
		/// The Query for this EmailTemplateAttribute.
		/// </summary>
		string Query { get; set; }
	
		/// <summary>
		/// The Description for this EmailTemplateAttribute.
		/// </summary>
		string Description { get; set; }
	
		/// <summary>
		/// The Active for this EmailTemplateAttribute.
		/// </summary>
		bool Active { get; set; }
	
		/// <summary>
		/// The CommandType for this EmailTemplateAttribute.
		/// </summary>
		Nullable<int> CommandType { get; set; }
	
		/// <summary>
		/// The ConnectionID for this EmailTemplateAttribute.
		/// </summary>
		Nullable<int> ConnectionID { get; set; }
	
		/// <summary>
		/// The References for this EmailTemplateAttribute.
		/// </summary>
		string References { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailTemplateAttribute))]
		internal abstract class EmailTemplateAttributeContracts : IEmailTemplateAttribute
		{
		    #region Primitive properties
		
			int IEmailTemplateAttribute.EmailTemplateAttributeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateAttribute.Name
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateAttribute.Query
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateAttribute.Description
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IEmailTemplateAttribute.Active
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEmailTemplateAttribute.CommandType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEmailTemplateAttribute.ConnectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateAttribute.References
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
