using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Business.Mail
{
    public class MailMessageRecipientCollection : List<MailMessageRecipient>
    {
        public static char RECIPIENTS_SEPARATOR = ';';

        public MailMessageRecipient GetByEmail(string email)
        {
            var matches = from sp in this
                          where sp.Email == email
                          select sp;

            if (matches == null || matches.Count() < 1)
                return null;

            int index = this.IndexOf((MailMessageRecipient)matches.First());

            if (index < 0)
                return null;

            return this[index];
        }

        #region Assign string to MailMessageRecipientCollection

        public static implicit operator MailMessageRecipientCollection(string recipientsString)
        {
            MailMessageRecipientCollection recipients = new MailMessageRecipientCollection();
            string[] recipientArray = recipientsString.Split(new char[] { RECIPIENTS_SEPARATOR });

            foreach (string recipient in recipientArray)
                recipients.Add(new MailMessageRecipient(recipient));
            return recipients;
        }

        #endregion

        #region Assign MailMessageRecipientCollection to string

        public static implicit operator string(MailMessageRecipientCollection recipients)
        {
            return recipients.ToEmailList();
        }

        public string ToEmailList()
        {
            StringBuilder recipientsString = new StringBuilder();
            foreach (MailMessageRecipient recipient in this)
            {
                if (!string.IsNullOrEmpty(recipient.Email))
                {
                    recipientsString.Append(recipient.Email);
                    recipientsString.Append(RECIPIENTS_SEPARATOR);
                }
            }

            string result = recipientsString.ToString();

            if (result.Length > 0 && result[recipientsString.Length - 1].Equals(RECIPIENTS_SEPARATOR))
                result = result.Substring(0, result.Length - 2);

            return result;
        }

        #endregion
    }
}
