namespace NetSteps.AzureMessageProcessor 
{
    using System;

    using FlitBit.Core;
    using FlitBit.Represent.Json;

    using Microsoft.ServiceBus.Messaging;

    using NetSteps.Infrastructure.Common.Email;

    /// <summary>
    /// The email processor.
    /// </summary>
    internal class EmailProcessor
    {
        /// <summary>
        /// The email provider.
        /// </summary>
        private readonly IEmailProvider emailProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailProcessor"/> class.
        /// </summary>
        /// <param name="emailProvider">
        /// The email provider.
        /// </param>
        public EmailProcessor(IEmailProvider emailProvider)
        {
            this.emailProvider = emailProvider;
        }

        /// <summary>
        /// The process.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool Process(BrokeredMessage message)
        {
            try
            {
                var factory = FactoryProvider.Current.GetFactory();
                var jsonRepresentation = factory.CreateInstance<IJsonRepresentation<IEmailMessage>>();
                var json = jsonRepresentation.RestoreItem(message.Properties["message"].ToString());
                return this.emailProvider.SendMail(json);
            }
            catch (Exception ex)
            {
                return true;
            }
        }
    }
}