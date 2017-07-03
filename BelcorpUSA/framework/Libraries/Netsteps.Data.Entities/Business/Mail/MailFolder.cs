using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.Data.Entities.Business.Mail
{
    [DataContract]
    public class MailFolder
    {
        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public string FullName { get; set; }

        public MailFolderNames NameAsEnum
        {
            get
            {
                return (MailFolderNames)Enum.Parse(typeof(MailFolderNames), FullName);
            }
        }

        public MailFolder()
        {
        }

        public MailFolder(string fullName)
        {
            FullName = fullName;
        }

        private MailMessageCollection _MailMessageCollection;
        public MailMessageCollection GetMailMessageCollection(MailAccount mailAccount)
        {
            if (_MailMessageCollection == null)
            {
                _MailMessageCollection = DataAdapterFactory.Current.MailMessageAdapter.LoadCollection(this, mailAccount);
            }
            return _MailMessageCollection;
        }

        public static bool Create(string folderName, MailAccount mailAccount)
        {
            IMailFolderAdapter adpt = DataAdapterFactory.Current.MailFolderAdapter;
            return adpt.Create(folderName, mailAccount);
        }

        public static bool Delete(string folderName, MailAccount mailAccount)
        {
            IMailFolderAdapter adpt = DataAdapterFactory.Current.MailFolderAdapter;
            return adpt.Delete(folderName, mailAccount);
        }

        //public static MailFolder GetInboxForAccount(int AccountID)
        //{
        //    MailAccount ma = MailAccount.LoadByAccountID(AccountID);
        //    if (ma != null)
        //    {
        //        string InboxString = Enum.GetName(typeof(MailFolder.MailFolderNames), MailFolder.MailFolderNames.Inbox);

        //        return ma.MailFolderCollection[InboxString];
        //    }
        //    return null;
        //}

        public static void CreateDefaultsForMailAccount(MailAccount mailAccount)
        {
            foreach (string folderName in Enum.GetNames(typeof(MailFolderNames)))
            {
                MailFolder.Create(folderName, mailAccount);
            }
        }
    }
}
