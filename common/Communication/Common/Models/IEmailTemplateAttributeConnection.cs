using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailTemplateAttributeConnection.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailTemplateAttributeConnectionContracts))]
	public interface IEmailTemplateAttributeConnection
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailTemplateAttributeConnectionID for this EmailTemplateAttributeConnection.
		/// </summary>
		int EmailTemplateAttributeConnectionID { get; set; }
	
		/// <summary>
		/// The ConnectionName for this EmailTemplateAttributeConnection.
		/// </summary>
		string ConnectionName { get; set; }
	
		/// <summary>
		/// The ConnectionString for this EmailTemplateAttributeConnection.
		/// </summary>
		string ConnectionString { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailTemplateAttributeConnection))]
		internal abstract class EmailTemplateAttributeConnectionContracts : IEmailTemplateAttributeConnection
		{
		    #region Primitive properties
		
			int IEmailTemplateAttributeConnection.EmailTemplateAttributeConnectionID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateAttributeConnection.ConnectionName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateAttributeConnection.ConnectionString
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
