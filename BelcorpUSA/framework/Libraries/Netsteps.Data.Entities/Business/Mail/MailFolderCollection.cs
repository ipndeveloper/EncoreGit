using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities.Business.Mail
{
    public class MailFolderCollection : List<MailFolder>
    {
        public MailFolder GetByFullName(string fullName)
        {
            var matches = from sp in this
                          where sp.FullName.Trim().ToLower() == fullName.Trim().ToLower()
                          select sp;

            if (matches == null || matches.Count() < 1)
                return null;

            int index = this.IndexOf((MailFolder)matches.First());

            if (index < 0)
                return null;

            return this[index];
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        public MailFolder this[MailFolderNames MailFolderName]
        {
            get
            {
                string foldername = Enum.GetName(typeof(MailFolderNames), MailFolderName);
                return this[foldername];
            }
        }

        public static MailFolderCollection LoadCollection(MailAccount mailAccount)
        {
            try
            {
                MailFolderCollection coll = new MailFolderCollection();

                foreach (MailFolderNames name in Enum.GetValues(typeof(MailFolderNames)))
                    coll.Add(new MailFolder(name.ToString()));

                return coll;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetstepsExceptionType.NetStepsBusinessException);
            }
        }
    }
}
