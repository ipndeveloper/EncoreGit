namespace NetSteps.Infrastructure.Common.Email
{
    /// <summary>
    /// The extensions.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// The to mail address.
        /// </summary>
        /// <param name="dto">
        /// The DTO.
        /// </param>
        /// <returns>
        /// The <see cref="MailAddress"/>.
        /// </returns>
        public static System.Net.Mail.MailAddress ToMailAddress(this MailAddress dto)
        {
            return new System.Net.Mail.MailAddress(dto.Address, dto.DisplayName);
        }
    }
}
