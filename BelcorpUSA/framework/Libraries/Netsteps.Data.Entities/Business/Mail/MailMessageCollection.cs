using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Business.Mail
{
    public class MailMessageCollection : List<MailMessage>
    {
        ///// <summary>
        ///// Gets or sets the element at the specified index.
        ///// </summary>
        ///// <param name="index">The zero-based index of the element to get or set.</param>
        ///// <returns>The element at the specified index.</returns>
        //public MailMessage GetByExternalMessageID(string externalMessageID)
        //{
        //    var matches = from sp in this
        //                  where sp.ExternalMessageID == externalMessageID
        //                  select sp;

        //    if (matches == null || matches.Count() < 1)
        //        return null;

        //    int index = this.IndexOf((MailMessage)matches.First());

        //    if (index < 0)
        //        return null;

        //    return this[index];
        //}


        public static MailMessageCollection LoadCollection(MailFolder mailFolder, MailAccount mailAccount)
        {
            try
            {
                MailMessageCollection mailMessageCollection = DataAdapterFactory.Current.MailMessageAdapter.LoadCollection(mailFolder, mailAccount);
                return mailMessageCollection;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetstepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string LoadCollectionString(MailFolder mailFolder, MailAccount mailAccount)
        {
            try
            {
                string str = DataAdapterFactory.Current.MailMessageAdapter.LoadCollectionString(mailFolder, mailAccount);
                return str;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetstepsExceptionType.NetStepsBusinessException);
            }
        }

        public static MailMessageCollection LoadAssociated(MailAccount mailAccount, Contact contact)
        {
            try
            {
                MailMessageCollection mailMessageCollection = DataAdapterFactory.Current.MailMessageAdapter.LoadAssociated(mailAccount, contact);
                return mailMessageCollection;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetstepsExceptionType.NetStepsBusinessException);
            }
        }

        public string ToString(bool headersonly)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append(string.Format("Date,From,Subject"));
            if (!headersonly)
                builder.Append(string.Format(",Body"));

            builder.Append(Environment.NewLine);

            foreach (MailMessage mailMessage in this)
            {
                try
                {
                    string from = !string.IsNullOrEmpty(mailMessage.From) ? mailMessage.From : mailMessage.FromAddress;
                    builder.Append(string.Format("{0},{1},{2}", mailMessage.Date, from, mailMessage.Subject.Replace(",", "~~")));
                    //We should probably URLEncode the Body MN
                    if (!headersonly)
                        builder.Append(string.Format(",{0}", mailMessage.Body.Replace(",", "~~")));

                    builder.Append(Environment.NewLine);
                }
                catch (Exception ex)
                { }
            }

            return builder.ToString();
        }
    }
}
