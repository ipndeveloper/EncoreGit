using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorBackOffice.Areas.Communication.Models.Email
{
	public class MailboxModel
	{
		public int? MailAccountID { get; set; }
		public string MailFolderName { get; set; }
		public int MessageCount { get; set; }
	}
}