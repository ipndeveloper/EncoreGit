using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace NetSteps.Communication.Mail.Common.Models
{
	/// <summary>
	/// Common interface for MailMessageGroupAddress.
	/// </summary>
	[ContractClass(typeof(Contracts.MailMessageGroupAddressContracts))]
	public interface IMailMessageGroupAddress
	{
	    #region Primitive properties
	
		/// <summary>
		/// The MailMessageGroupAddressID for this MailMessageGroupAddress.
		/// </summary>
		int MailMessageGroupAddressID { get; set; }
	
		/// <summary>
		/// The MailMessageGroupID for this MailMessageGroupAddress.
		/// </summary>
		int MailMessageGroupID { get; set; }
	
		/// <summary>
		/// The EmailAddress for this MailMessageGroupAddress.
		/// </summary>
		string EmailAddress { get; set; }
	
		/// <summary>
		/// The NickName for this MailMessageGroupAddress.
		/// </summary>
		string NickName { get; set; }
	
		/// <summary>
		/// The AddressTypeID for this MailMessageGroupAddress.
		/// </summary>
		Nullable<short> AddressTypeID { get; set; }
	
		/// <summary>
		/// The RecipientTypeID for this MailMessageGroupAddress.
		/// </summary>
		Nullable<short> RecipientTypeID { get; set; }
	
		/// <summary>
		/// The RecipientStatusID for this MailMessageGroupAddress.
		/// </summary>
		Nullable<short> RecipientStatusID { get; set; }
	
		/// <summary>
		/// The AccountID for this MailMessageGroupAddress.
		/// </summary>
		Nullable<int> AccountID { get; set; }

	    #endregion
	
	    #region Single navigation properties
	
		/// <summary>
		/// The AddressType for this MailMessageGroupAddress.
		/// </summary>
	    IAddressType AddressType { get; set; }
	
		/// <summary>
		/// The MailMessageGroup for this MailMessageGroupAddress.
		/// </summary>
	    IMailMessageGroup MailMessageGroup { get; set; }
	
		/// <summary>
		/// The RecipientStatus for this MailMessageGroupAddress.
		/// </summary>
	    IRecipientStatus RecipientStatus { get; set; }
	
		/// <summary>
		/// The RecipientType for this MailMessageGroupAddress.
		/// </summary>
	    IRecipientType RecipientType { get; set; }

	    #endregion
	
	    #region Collection navigation properties
	
		/// <summary>
		/// The MailMessageRecipientEvents for this MailMessageGroupAddress.
		/// </summary>
		IEnumerable<IMailMessageRecipientEvent> MailMessageRecipientEvents { get; }
	
		/// <summary>
		/// Adds an <see cref="IMailMessageRecipientEvent"/> to the MailMessageRecipientEvents collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageRecipientEvent"/> to add.</param>
		void AddMailMessageRecipientEvent(IMailMessageRecipientEvent item);
	
		/// <summary>
		/// Removes an <see cref="IMailMessageRecipientEvent"/> from the MailMessageRecipientEvents collection.
		/// </summary>
		/// <param name="item">The <see cref="IMailMessageRecipientEvent"/> to remove.</param>
		void RemoveMailMessageRecipientEvent(IMailMessageRecipientEvent item);

	    #endregion
	}
	
	namespace Contracts
	{
		[ContractClassFor(typeof(IMailMessageGroupAddress))]
		internal abstract class MailMessageGroupAddressContracts : IMailMessageGroupAddress
		{
		    #region Primitive properties
		
			int IMailMessageGroupAddress.MailMessageGroupAddressID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			int IMailMessageGroupAddress.MailMessageGroupID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageGroupAddress.EmailAddress
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			string IMailMessageGroupAddress.NickName
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IMailMessageGroupAddress.AddressTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IMailMessageGroupAddress.RecipientTypeID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<short> IMailMessageGroupAddress.RecipientStatusID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
			Nullable<int> IMailMessageGroupAddress.AccountID
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Single navigation properties
		
		    IAddressType IMailMessageGroupAddress.AddressType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IMailMessageGroup IMailMessageGroupAddress.MailMessageGroup
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IRecipientStatus IMailMessageGroupAddress.RecipientStatus
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}
		
		    IRecipientType IMailMessageGroupAddress.RecipientType
			{
				get { throw new NotImplementedException(); }
				set { throw new NotImplementedException(); }
			}

		    #endregion
		
		    #region Collection navigation properties
		
			IEnumerable<IMailMessageRecipientEvent> IMailMessageGroupAddress.MailMessageRecipientEvents
			{
				get { throw new NotImplementedException(); }
			}
		
			void IMailMessageGroupAddress.AddMailMessageRecipientEvent(IMailMessageRecipientEvent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}
		
			void IMailMessageGroupAddress.RemoveMailMessageRecipientEvent(IMailMessageRecipientEvent item)
			{
				Contract.Requires<ArgumentNullException>(item != null);
				throw new NotImplementedException();
			}

		    #endregion
		}
	}
}
