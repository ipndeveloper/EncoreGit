using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for EmailTemplateToken.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailTemplateTokenContracts))]
	public interface IEmailTemplateToken
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailTemplateTokenID for this EmailTemplateToken.
		/// </summary>
		int EmailTemplateTokenID { get; set; }
	
		/// <summary>
		/// The AccountID for this EmailTemplateToken.
		/// </summary>
		Nullable<int> AccountID { get; set; }
	
		/// <summary>
		/// The Token for this EmailTemplateToken.
		/// </summary>
		string Token { get; set; }
	
		/// <summary>
		/// The Value for this EmailTemplateToken.
		/// </summary>
		string Value { get; set; }
	
		/// <summary>
		/// The PartyID for this EmailTemplateToken.
		/// </summary>
		Nullable<int> PartyID { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailTemplateToken))]
		internal abstract class EmailTemplateTokenContracts : IEmailTemplateToken
		{
		    #region Primitive properties
		
			int IEmailTemplateToken.EmailTemplateTokenID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEmailTemplateToken.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateToken.Token
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailTemplateToken.Value
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IEmailTemplateToken.PartyID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
