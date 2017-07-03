namespace NetSteps.Infrastructure.Common.Email 
{
    /// <summary>
    /// The EmailProvider interface.
    /// </summary>
    public interface IEmailProvider
    {
        /// <summary>
        /// The send mail.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        bool SendMail(IEmailMessage message);
    }
}
