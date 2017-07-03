namespace NetSteps.AzureMessageProcessor  
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Net;
    using System.Threading;

    using FlitBit.Wireup;

    using Microsoft.ServiceBus;
    using Microsoft.ServiceBus.Messaging;
    using Microsoft.WindowsAzure;
    using Microsoft.WindowsAzure.ServiceRuntime;

    using NetSteps.Infrastructure.Common.Email;
    using NetSteps.Infrastructure.SendGridMail;

    /// <summary>
    /// The worker role.
    /// </summary>
    public class WorkerRole : RoleEntryPoint
    {
        /// <summary>
        /// The send grid user name.
        /// </summary>
        private string emailProviderUserName;

        /// <summary>
        /// The send grid password.
        /// </summary>
        private string emailProviderPassword;

        /// <summary>
        /// The queue name.
        /// </summary>
        private string emailQueueName;

        // QueueClient is thread-safe. Recommended that you cache 
        // rather than recreating it on every request

        /// <summary>
        /// The client.
        /// </summary>
        private QueueClient client;

        /// <summary>
        /// The is stopped.
        /// </summary>
        private bool isStopped;

        /// <summary>
        /// The email provider.
        /// </summary>
        private IEmailProvider emailProvider;

        /// <summary>
        /// The email processor.
        /// </summary>
        private EmailProcessor emailProcessor;

        /// <summary>
        /// The run.
        /// </summary>
        public override void Run()
        {
            while (!this.isStopped)
            {
                try
                {
                    // Receive the message
                    BrokeredMessage receivedMessage = this.client.Receive();

                    if (receivedMessage != null)
                    {
                        // Process the message
                        Trace.WriteLine("Processing", receivedMessage.SequenceNumber.ToString(CultureInfo.InvariantCulture));

                        if (!string.IsNullOrEmpty(receivedMessage.ContentType))
                        {
                            switch (receivedMessage.ContentType.ToUpperInvariant())
                            {
                                case "MAILMESSAGE":
                                    try
                                    {
                                        if (this.emailProcessor.Process(receivedMessage))
                                        {
                                            receivedMessage.Complete();
                                            Trace.WriteLine(string.Concat("Sent message at: ", DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), " UTC"));
                                        }
                                        else
                                        {
                                            receivedMessage.Abandon();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        receivedMessage.Abandon();
                                    }

                                    break;
                            }
                        }
                        else
                        {
                            receivedMessage.Complete();
                        }
                    }
                }
                catch (MessagingException e)
                {
                    if (!e.IsTransient)
                    {
                        Trace.WriteLine(e.Message);
                    }

                    Thread.Sleep(10000);
                }
                catch (OperationCanceledException e)
                {
                    if (!this.isStopped)
                    {
                        Trace.WriteLine(e.Message);
                    }
                }
                catch (Exception e)
                {
                    Trace.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// The on start.
        /// </summary>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections 
            ServicePointManager.DefaultConnectionLimit = 12;

            try
            {
                WireupCoordinator.SelfConfigure();

                this.emailQueueName = CloudConfigurationManager.GetSetting("EmailQueueName");
                this.emailProviderUserName = CloudConfigurationManager.GetSetting("SendGridUserName");
                this.emailProviderPassword = CloudConfigurationManager.GetSetting("SendGridPassword");

                string storageConnectionString = CloudConfigurationManager.GetSetting("Microsoft.BlobStorage.ConnectionString");
                var storageProvider = new Infrastructure.AzureBlobStorage.Provider(storageConnectionString, "nsemailattachments");

                this.emailProvider = new Provider(this.emailProviderUserName, this.emailProviderPassword, storageProvider);
                this.emailProcessor = new EmailProcessor(this.emailProvider);
                
                // Create the queue if it does not exist already
                string connectionString = CloudConfigurationManager.GetSetting("Microsoft.ServiceBus.ConnectionString");
                var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
                if (!namespaceManager.QueueExists(this.emailQueueName))
                {
                    namespaceManager.CreateQueue(this.emailQueueName);
                }

                // Initialize the connection to Service Bus Queue
                this.client = QueueClient.CreateFromConnectionString(connectionString, this.emailQueueName);
                this.isStopped = false;
                return base.OnStart();
            }
            catch (Exception e)
            {
                Trace.WriteLine(e.Message);
            }

            return false;
        }

        /// <summary>
        /// The on stop.
        /// </summary>
        public override void OnStop()
        {
            // Close the connection to Service Bus Queue
            this.isStopped = true;
            this.client.Close();
            base.OnStop();
        }
    }
}
