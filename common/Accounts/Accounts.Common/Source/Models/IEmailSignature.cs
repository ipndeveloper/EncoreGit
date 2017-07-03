using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Accounts.Common.Models
{
	/// <summary>
	/// Common interface for EmailSignature.
	/// </summary>
	[ContractClass(typeof(Contracts.EmailSignatureContracts))]
	public interface IEmailSignature
	{
	    #region Primitive properties
	
		/// <summary>
		/// The EmailSignatureID for this EmailSignature.
		/// </summary>
		int EmailSignatureID { get; set; }
	
		/// <summary>
		/// The AccountID for this EmailSignature.
		/// </summary>
		int AccountID { get; set; }
	
		/// <summary>
		/// The Body for this EmailSignature.
		/// </summary>
		string Body { get; set; }
	
		/// <summary>
		/// The ImageUrl for this EmailSignature.
		/// </summary>
		string ImageUrl { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The Account for this EmailSignature.
		/// </summary>
	    IAccount Account { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IEmailSignature))]
		internal abstract class EmailSignatureContracts : IEmailSignature
		{
		    #region Primitive properties
		
			int IEmailSignature.EmailSignatureID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IEmailSignature.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailSignature.Body
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IEmailSignature.ImageUrl
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAccount IEmailSignature.Account
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
