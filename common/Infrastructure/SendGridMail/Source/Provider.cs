namespace NetSteps.Infrastructure.SendGridMail
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;

    using NetSteps.Infrastructure.Common.Email;
    using NetSteps.Infrastructure.Common.Storage;

    using global::SendGridMail;
    using global::SendGridMail.Transport;

    // http://sendgrid.com/docs/API_Reference/Web_API/mail.html
    // https://sendgrid.com/api/mail.send.json?api_user=youremail@domain.com&api_key=secureSecret&to=destination@example.com&toname=Destination&subject=Examplev1_(deprecated)Subject&text=testingtextbody&from=info@domain.com
    
    /// <summary>
    /// The send grid provider.
    /// </summary>
    public class Provider : IEmailProvider
    {
        /// <summary>
        /// The user name.
        /// </summary>
        private readonly string userName;

        /// <summary>
        /// The password.
        /// </summary>
        private readonly string password;

        /// <summary>
        /// The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Provider"/> class.
        /// </summary>
        /// <param name="userName">
        /// The user name.
        /// </param>
        /// <param name="password">
        /// The password.
        /// </param>
        /// <param name="storageProvider">
        /// The storage Provider.
        /// </param>
        public Provider(string userName, string password, IStorageProvider storageProvider)
        {
            this.userName = userName;
            this.password = password;
            this.storageProvider = storageProvider;
        }

        /// <summary>
        /// The send mail.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool SendMail(IEmailMessage message)
        {
            var myMessage = SendGrid.GetInstance();
            myMessage.Subject = message.Subject;
            myMessage.Text = message.Body;
            myMessage.Html = message.HtmlBody;
            myMessage.From = message.Sender.ToMailAddress();

            if (message.ReplyTo != null)
            {
                myMessage.ReplyTo = new[] { message.ReplyTo.ToMailAddress() };
            }

            if (message.Recipients != null && message.Recipients.Any())
            {
                myMessage.AddTo(this.GetFormattedAddresseeList(message.Recipients));
            }

            if (message.CarbonCopyTo != null && message.CarbonCopyTo.Any())
            {
                myMessage.AddCc(this.GetFormattedAddresseeList(message.CarbonCopyTo));
            }

            if (message.BlindCopyTo != null && message.BlindCopyTo.Any())
            {
                myMessage.AddBcc(this.GetFormattedAddresseeList(message.BlindCopyTo));
            }

            myMessage.Headers.Add("Priority", message.Priority.ToString());

            foreach (var emailAttachment in message.Attachments)
            {
                var uri = new Uri(emailAttachment.FullPath);
                var bytes = this.storageProvider.Retrieve(uri);

                using (var stream = new MemoryStream(bytes))
                {
                    myMessage.AddAttachment(stream, emailAttachment.Name);    
                }
            }

            var credentials = new NetworkCredential(this.userName, this.password);
            var transportWeb = Web.GetInstance(credentials);

            try
            {
                transportWeb.Deliver(myMessage);

                foreach (var emailAttachment in message.Attachments)
                {
                    var uri = new Uri(emailAttachment.FullPath);
                    this.storageProvider.Remove(uri);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The get formatted addressee list.
        /// </summary>
        /// <param name="addresses">
        /// The addresses.
        /// </param>
        /// <returns>
        /// The formatted email addresses.
        /// </returns>
        private IEnumerable<string> GetFormattedAddresseeList(List<MailAddress> addresses)
        {
            return addresses.Select(mailAddress => string.Format("{0} <{1}>", mailAddress.DisplayName, mailAddress.Address)).ToList();
        }
    }
}
