using System;
using System.Collections.Generic;
using NetSteps.Common.Base;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Mail;

namespace NetSteps.Data.Entities.Repositories
{
	public partial interface IMailMessageRepository : IBaseRepository<MailMessage, Int32>
	{
		void MarkAsRead(int mailMessageID);
		bool Send(MailMessage mailMessage, MailAccount mailAccount, int siteID);
		bool PurgeFolder(NetSteps.Data.Entities.Mail.Constants.MailFolderType mailFolder, MailAccount mailAccount);
		bool Move(MailMessage mailMessage, short destinationMailFolderTypeID);
		bool Move(int mailMessageID, short destinationMailFolderTypeID);
		bool SaveDraft(MailMessage mailMessage, MailAccount mailAccount);
		MailAttachment RetrieveMailAttachment(MailMessage mailMessage, string attachmentName, MailAccount mailAccount);
		List<MailMessage> LoadCollection(NetSteps.Data.Entities.Mail.Constants.MailFolderType mailFolder, MailAccount mailAccount);
		void MarkAsUnReadBatch(List<int> mailMessageIDs);
		void MarkAsReadBatch(List<int> mailMessageIDs);
		PaginatedList<MailMessageSearchData> Search(MailMessageSearchParameters searchParameters);
	}
}
