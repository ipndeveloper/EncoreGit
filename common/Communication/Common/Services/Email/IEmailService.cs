namespace NetSteps.Communication.Common.Services.Email
{
    using NetSteps.Infrastructure.Common.Email;

    /// <summary>
    /// The EmailService interface.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// The send email.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        void SendEmail(IEmailMessage message);
    }
}
