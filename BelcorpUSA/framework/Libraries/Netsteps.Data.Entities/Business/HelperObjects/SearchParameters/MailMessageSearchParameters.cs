using NetSteps.Common.Base;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Business
{
	public class MailMessageSearchParameters : FilterDateRangePaginatedListParameters<MailMessage>
	{
		public int? MailAccountID { get; set; }

		public short? MailFolderTypeID { get; set; }

		public string Subject { get; set; }

		public string Body { get; set; }

		public bool? BeenRead { get; set; }
	}
}
