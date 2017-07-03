using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Common.Models
{
	/// <summary>
	/// Common interface for MailDomain.
	/// </summary>
	[ContractClass(typeof(Contracts.MailDomainContracts))]
	public interface IMailDomain
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailDomainID for this MailDomain.
		/// </summary>
		int MailDomainID { get; set; }
	
		/// <summary>
		/// The DomainName for this MailDomain.
		/// </summary>
		string DomainName { get; set; }
	
		/// <summary>
		/// The ServiceUri for this MailDomain.
		/// </summary>
		string ServiceUri { get; set; }
	
		/// <summary>
		/// The UserName for this MailDomain.
		/// </summary>
		string UserName { get; set; }
	
		/// <summary>
		/// The Password for this MailDomain.
		/// </summary>
		string Password { get; set; }
	
		/// <summary>
		/// The Server for this MailDomain.
		/// </summary>
		string Server { get; set; }
	
		/// <summary>
		/// The Port for this MailDomain.
		/// </summary>
		int Port { get; set; }
	
		/// <summary>
		/// The ServerUri for this MailDomain.
		/// </summary>
		string ServerUri { get; set; }
	
		/// <summary>
		/// The IsDefaultForInternalMailAccounts for this MailDomain.
		/// </summary>
		bool IsDefaultForInternalMailAccounts { get; set; }

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailDomain))]
		internal abstract class MailDomainContracts : IMailDomain
		{
		    #region Primitive properties
		
			int IMailDomain.MailDomainID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailDomain.DomainName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailDomain.ServiceUri
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailDomain.UserName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailDomain.Password
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailDomain.Server
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailDomain.Port
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailDomain.ServerUri
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			bool IMailDomain.IsDefaultForInternalMailAccounts
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		}
	}
}
