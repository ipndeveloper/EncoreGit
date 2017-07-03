using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Business.Mail
{
    public class MailAttachmentCollection : List<MailAttachment>
    {
        public MailAttachment GetByName(string name)
        {
            var matches = from sp in this
                          where sp.Name == name
                          select sp;

            if (matches == null || matches.Count() < 1)
                return null;

            int index = this.IndexOf((MailAttachment)matches.First());

            if (index < 0)
                return null;

            return this[index];
        }
    }
}
