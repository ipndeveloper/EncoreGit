namespace NetSteps.Communication.AzureEmail
{
    using System;
    using System.Net;

    using FlitBit.Core;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;

    using NetSteps.Communication.Common.Services.Email;
    using NetSteps.Infrastructure.Common.Email;
    using NetSteps.Infrastructure.Common.Storage;

    /// <summary>
    /// The email service.
    /// </summary>
    public class Service : IEmailService
    {
        /// <summary>
        /// The client.
        /// </summary>
        private readonly QueueClient client;

        /// <summary>
        /// The storage provider.
        /// </summary>
        private readonly IStorageProvider storageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="Service"/> class.
        /// </summary>
        /// <param name="serviceBusConnectionString">
        /// The service bus connection string.
        /// </param>
        /// <param name="queueName">
        /// The queue name.
        /// </param>
        /// <param name="storageProvider">
        /// The storage Provider.
        /// </param>
        public Service(string serviceBusConnectionString, string queueName, IStorageProvider storageProvider)
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            // Create the queue if it does not exist already
            var namespaceManager = NamespaceManager.CreateFromConnectionString(serviceBusConnectionString);
            if (!namespaceManager.QueueExists(queueName))
            {
                namespaceManager.CreateQueue(queueName);
            }

            // Initialize the connection to Service Bus Queue
            this.client = QueueClient.CreateFromConnectionString(serviceBusConnectionString, queueName);

            this.storageProvider = storageProvider;
        }

        /// <summary>
        /// The send email.
        /// </summary>
        /// <param name="emailMessage">
        /// The message.
        /// </param>
        public void SendEmail(IEmailMessage emailMessage)
        {
            var message = new BrokeredMessage { ContentType = "MailMessage" };

            // Save attachments to blob storage.
            foreach (var emailAttachment in emailMessage.Attachments)
            {
                emailAttachment.FullPath = this.SaveEmailAttachment(emailAttachment.Bytes, emailAttachment.Name);
                emailAttachment.Bytes = null;
            }

            message.Properties["message"] = emailMessage.ToJson();
            this.client.Send(message);
        }

        /// <summary>
        /// The save email attachment to blob storage so email processor can easily pick it up.
        /// </summary>
        /// <param name="file">
        /// The file.
        /// </param>
        /// <param name="fileName">
        /// The file name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string SaveEmailAttachment(byte[] file, string fileName)
        {
            var key = Guid.NewGuid().ToString();
            return this.storageProvider.Persist(file, key, fileName).ToString();
        }
    }
}
