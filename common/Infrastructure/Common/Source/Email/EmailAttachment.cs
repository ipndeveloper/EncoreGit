namespace NetSteps.Infrastructure.Common.Email
{
    /// <summary>
    /// The email attachment.
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// Gets or sets the full path.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        public byte[] Bytes { get; set; }
    }
}