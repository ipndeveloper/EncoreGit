using System;
using System.Runtime.Serialization;
using NetSteps.Common.Extensions;

namespace NetSteps.Data.Entities.Mail
{
	[Serializable]
	public class MailMessageRecipient
	{
		#region Properties
		public NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType MailMessageRecipientType { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public int? AccountID { get; set; }

		[DataMember]
		public bool? Internal { get; set; }

		/// <summary>
		/// Returns in the format of example: 'Bill Smith <billSmith@hotmail.com>' or 'billSmith@hotmail.com' (if Name is not set)
		/// </summary>
		public string AddressFormated
		{
			get
			{
				return GetAddressFormated(Name, Email, Internal ?? false);
			}
		}
		#endregion

		#region Constructors
		public MailMessageRecipient()
		{
		}

		public MailMessageRecipient(string email)
		{
			Email = email;
		}

		public MailMessageRecipient(string name, string email)
			: this(email)
		{
			Name = name;
		}

		public MailMessageRecipient(string name, string email, int accountID)
			: this(name, email)
		{
			AccountID = accountID;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Returns in the format of example: 'Bill Smith <billSmith@hotmail.com>' or 'billSmith@hotmail.com' (if Name is not set)
		/// </summary>
		public static string GetAddressFormated(string name, string email, bool isInternal = false)
		{
			if (!name.IsNullOrEmpty())
			{
				if (isInternal || string.IsNullOrWhiteSpace(email))
				{
					return name;
				}
				else
				{
					return string.Format("{0} <{1}>", name, email);
				}
			}
			else
			{
				return email;
			}
		}
		#endregion
	}
}
