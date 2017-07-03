using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Mail;
using System.Configuration;

namespace TestMasterHelpProvider
{
	public class EmailMessage
	{
		#region Fields

		public const string EmailLogToAppSettingName = "EmailLogTo";

		private const string DefaultHost = "mail.gopronetworks.com"; 
		private const bool DefaultCredentials = true;

		private MailMessage _mailMessage;
		private SmtpClient _smtpClient;

		#endregion

		#region Properties

		/// <summary>
		/// Gets or sets the mail message for the email client.
		/// </summary>
		public MailAddress MessageFromAddress
		{
			get { return _mailMessage.From; }
			set { _mailMessage.From = value; }
		}

		/// <summary>
		/// Gets the mail address collection for TO addresses.
		/// </summary>
		public MailAddressCollection ToAddresses
		{
			get { return _mailMessage.To; }
		}

		/// <summary>
		/// Gets the mail address collection for CC addresses.
		/// </summary>
		public MailAddressCollection MessageCcAddresses
		{
			get { return _mailMessage.CC; }
		}

		/// <summary>
		/// Gets or sets the subject of the email.
		/// </summary>
		public string MessageSubject
		{
			get { return _mailMessage.Subject; }
			set { _mailMessage.Subject = value; }
		}

		/// <summary>
		/// Gets or sets the body of the email.
		/// </summary>
		public string MessageBody
		{
			get { return _mailMessage.Body; }
			set { _mailMessage.Body = value; }
		}

		/// <summary>
		/// Gets or sets the encoding of the email body.
		/// </summary>
		public Encoding MessageBodyEncoding
		{
			get { return _mailMessage.BodyEncoding; }
			set { _mailMessage.BodyEncoding = value; }
		}

		/// <summary>
		/// Gets or sets whether the message body of the email is HTML.
		/// </summary>
		public bool IsBodyHtml
		{
			get { return _mailMessage.IsBodyHtml; }
			set { _mailMessage.IsBodyHtml = value; }
		}

		/// <summary>
		/// Gets or sets the credentials for the email client. This property will be set by the configuration first.
		/// </summary>
		public ICredentialsByHost SmtpCredentials
		{
			get { return _smtpClient.Credentials; }
			set { _smtpClient.Credentials = value; }
		}

		/// <summary>
		/// Gets or sets the SMTP host. This property will be set by the configuration first.
		/// </summary>
		public string SmtpHost
		{
			get { return _smtpClient.Host; }
			set { _smtpClient.Host = value; }
		}

		/// <summary>
		/// Gets or sets the port of the SMTP host. This property will be set by the configuration first.
		/// </summary>
		public int SmtpPort
		{
			get { return _smtpClient.Port; }
			set { _smtpClient.Port = value; }
		}

		/// <summary>
		/// Gets or sets whether the default credentials are sent with the request. This property will be set by the configuration first.
		/// </summary>
		public bool UseDefaultCredentials
		{
			get { return _smtpClient.UseDefaultCredentials; }
			set { _smtpClient.UseDefaultCredentials = value; }
		}

		#endregion

		#region Constructor

		/// <summary>
		/// Creates an instance of EmailMessage using the credentials and configuration in the App.config.
		/// See http://weblogs.asp.net/scottgu/archive/2005/12/10/432854.aspx.
		/// </summary>
		public EmailMessage()
		{
			_mailMessage = new MailMessage();
			_smtpClient = new SmtpClient();

			ConfigureSmtpClient();
			AddConfiguredToAddresses();
		}

		/// <summary>
		/// Creates an instance of EmailMessage using the credentials and configuration in the App.config.
		/// See http://weblogs.asp.net/scottgu/archive/2005/12/10/432854.aspx.
		/// </summary>
		/// <param name="message"></param>
		public EmailMessage(MailMessage message)
		{
			_mailMessage = message;
			_smtpClient = new SmtpClient();

			ConfigureSmtpClient();
			AddConfiguredToAddresses();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Sends the MailMessage object defined by EmailMessage.
		/// </summary>
		public void Send()
		{
			_smtpClient.Send(_mailMessage);
		}

		/// <summary>
		/// Adds an attachment to the email.
		/// </summary>
		/// <param name="attachment"></param>
		public void AddAttachment(Attachment attachment)
		{
			_mailMessage.Attachments.Add(attachment);
		}

		/// <summary>
		/// Attempts to remove the given attachment returning success or failure.
		/// </summary>
		/// <param name="attachment"></param>
		/// <returns></returns>
		public bool RemoveAttachment(Attachment attachment)
		{
			return _mailMessage.Attachments.Remove(attachment);
		}

		/// <summary>
		/// Removes all attachments from the message.
		/// </summary>
		public void ClearAttachments()
		{
			_mailMessage.Attachments.Clear();
		}

		/// <summary>
		/// Adds any email addresses in the AppSettings configuration to the MailMessage. Configuration must be under 
		/// AppSettings and named "EmailLogTo". Emails addresses may be comma- or semi-colon-separated.
		/// </summary>
		private void AddConfiguredToAddresses()
		{
			if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[EmailMessage.EmailLogToAppSettingName]))
			{
				string[] toAddresses = ConfigurationManager.AppSettings[EmailMessage.EmailLogToAppSettingName].Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

				if (toAddresses.Length > 0)
				{
					foreach (string nextAddress in toAddresses)
					{
						this.ToAddresses.Add(new MailAddress(nextAddress));
					}
				}
			}
		}

		/// <summary>
		/// Initializes the SMTP client.
		/// </summary>
		private void ConfigureSmtpClient()
		{
			System.Net.Configuration.MailSettingsSectionGroup mailSettingsSectionGroup = ConfigurationManager.GetSection("system.net/mailSettings") as System.Net.Configuration.MailSettingsSectionGroup;

			if (mailSettingsSectionGroup != null)
			{
				_smtpClient.Host = mailSettingsSectionGroup.Smtp.Network.Host;
				_smtpClient.UseDefaultCredentials = mailSettingsSectionGroup.Smtp.Network.DefaultCredentials;
			}
			else
			{
				_smtpClient.Host = EmailMessage.DefaultHost;
				_smtpClient.UseDefaultCredentials = EmailMessage.DefaultCredentials;
			}
		}

		#endregion
	}
}
