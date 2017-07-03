namespace NetSteps.Infrastructure.Common.Email
{
    using System.Collections.Generic;

    using FlitBit.Dto;

    /// <summary>
    /// The EmailMessage interface.
    /// </summary>
    [DTO]
    public interface IEmailMessage
    {
        /// <summary>
        /// Gets or sets the recipients.
        /// </summary>
        List<MailAddress> Recipients { get; set; }

        /// <summary>
        /// Gets or sets the carbon copy to.
        /// </summary>
        List<MailAddress> CarbonCopyTo { get; set; }

        /// <summary>
        /// Gets or sets the blind copy to.
        /// </summary>
        List<MailAddress> BlindCopyTo { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body.
        /// </summary>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets the html body.
        /// </summary>
        string HtmlBody { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        MailAddress Sender { get; set; }

        /// <summary>
        /// Gets or sets the reply to.
        /// </summary>
        MailAddress ReplyTo { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        System.Net.Mail.MailPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the attachments.
        /// </summary>
        List<EmailAttachment> Attachments { get; set; }
    }
}
