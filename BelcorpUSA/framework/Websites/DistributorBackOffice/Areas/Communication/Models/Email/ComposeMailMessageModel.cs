using System.Collections.Generic;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Mail;

namespace DistributorBackOffice.Areas.Communication.Models.Email
{
    public class ComposeMailMessageModel
    {
        /// <summary>
        /// Initializes a new instance of the ComposeModel class.
        /// </summary>
        public ComposeMailMessageModel(MailMessage message, List<Archive> removedArchives)
        {
            Message = message;
            RemovedArchives = removedArchives;
        }
        public MailMessage Message { get; set; }
        public List<Archive> RemovedArchives { get; set; }
    }
}