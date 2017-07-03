using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Encore.Core.IoC;
using MailConstants = NetSteps.Data.Entities.Mail.Constants;

namespace NetSteps.Data.Entities.Repositories
{
    [ContainerRegister(typeof(IMailMessageRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MailMessageRepository : BaseRepository<MailMessage, int, MailEntities>, IMailMessageRepository, IDefaultImplementation
    {
        #region Members
        //protected override Func<MailEntities, int, IQueryable<MailMessage>> selectFullQuery
        //{
        //    get
        //    {
        //        return CompiledQuery.Compile<MailEntities, int, IQueryable<MailMessage>>(
        //         (context, mailMessageID) => from u in context.MailMessages
        //                                     where u.MailMessageID == mailMessageID
        //                                     select u);
        //    }
        //}
        #endregion

        #region Methods
        public override MailMessage Load(int mailMessageID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                MailMessage message = new MailMessage();
                IDbCommand dbCommand = null;

                try
                {
                    dbCommand = DataAccess.SetCommand("usp_MailMessageSelect", connectionString: GetMailConnectionString());
                    DataAccess.AddInputParameter("MailMessageID", mailMessageID, dbCommand);
                    using (IDataReader rdr = DataAccess.ExecuteReader(dbCommand))
                    {
                        // First result set contains the mail message itself
                        if (rdr.Read())
                        {
                            do
                            {
                                //message.ExternalMessageID = ((int)msgRow["MailMessageID"]).ToString();
                                message.MailMessageID = rdr.GetInt32("MailMessageID");
                                message.Subject = rdr.GetString("Subject");
                                message.Body = rdr.GetString("Body");
                                message.HTMLBody = rdr.GetString("HTMLBody");
                                message.DateAddedUTC = rdr.GetDateTime("DateAddedUTC");
                                message.FromAddress = rdr.GetString("FromAddress");
                                message.ReplyToAddress = rdr.GetString("ReplyToAddress");
                                message.FromNickName = rdr.GetString("FromNickName");
                                message.BeenRead = rdr.GetBoolean("BeenRead");
                                message.MailMessagePriorityID = rdr.GetInt16("MailMessagePriorityID");
                                message.VisualTemplateID = rdr.GetNullableInt32("VisualTemplateID");
                                message.MailFolderTypeID = rdr.GetInt16("MailFolderTypeID");
                                message.FullFolderName = ((MailConstants.MailFolderType)rdr.GetInt16("MailFolderTypeID")).ToString();
                                message.AttachmentUniqueID = rdr.GetString("AttachmentUniqueID");
                                message.MailMessageTypeID = rdr.GetInt16("MailMessageTypeID");
                                message.CampaignActionID = rdr.GetNullableInt32("CampaignActionID");
                                message.EnableEventTracking = rdr.GetBoolean("EnableEventTracking");
                            } while (rdr.Read());
                        }
                        else
                        {
                            Exception ex = new Exception(string.Format("No MailMessage found for messageID: {0}", mailMessageID));
                            throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                        }

                        // Second result set contains all the individual mail addresses
                        rdr.NextResult();
                        while (rdr.Read())
                        {
                            MailMessageRecipient recipient = new MailMessageRecipient(rdr.GetString("NickName"), rdr.GetString("EmailAddress"));

                            recipient.MailMessageRecipientType = (MailConstants.MailMessageRecipientType)rdr.GetInt16("RecipientTypeID");
                            recipient.AccountID = rdr.GetNullableInt32("AccountID");

                            NetSteps.Data.Entities.Mail.Constants.EmailAddressType addressType = (NetSteps.Data.Entities.Mail.Constants.EmailAddressType)rdr.GetInt16("AddressTypeID");
                            if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.TO)
                                message.To.Add(recipient);
                            else if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.CC)
                                message.Cc.Add(recipient);
                            else if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.BCC)
                                message.Bcc.Add(recipient);
                        }

                        // Third result set contains all the attachments
                        rdr.NextResult();
                        while (rdr.Read())
                        {
                            MailAttachment attachment = new MailAttachment();
                            attachment.FileName = rdr.GetString("FileName");
                            attachment.Size = rdr.GetNullableInt32("Size");
                            message.Attachments.Add(attachment);
                        }
                    }

                    if (message.Attachments.Count > 0)
                        message.HasAttachments = true;

                    return message;
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }
            });
        }

        /// <summary>
        /// If the message is in the Trash folder the message will be permanently deleted.
        /// If it is not in the trash folder it will move it to the trash folder. - JHE
        /// </summary>
        /// <param name="mailMessage"></param>
        public override void Delete(MailMessage mailMessage)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                Delete(mailMessage.MailMessageID);
            });
        }

        /// <summary>
        /// If the message is in the Trash folder the message will be permanently deleted.
        /// If it is not in the trash folder it will move it to the trash folder. - JHE
        /// </summary>
        /// <param name="mailMessageID"></param>
        public override void Delete(int mailMessageID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                IDbCommand dbCommand = null;

                try
                {
                    dbCommand = DataAccess.SetCommand("usp_DeleteMailMessage", connectionString: GetMailConnectionString());
                    DataAccess.AddInputParameter("MailMessageID", mailMessageID, dbCommand);

                    DataAccess.ExecuteNonQuery(dbCommand);
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }
            });
        }

        public override void DeleteBatch(IEnumerable<int> mailMessageIDs)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                // TODO: Change this to be an actual Bulk delete later - JHE
                foreach (var primaryKey in mailMessageIDs)
                {
                    Delete(primaryKey);
                }
            });
        }

        public void MarkAsRead(int mailMessageID)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                IDbCommand dbCommand = null;

                try
                {
                    dbCommand = DataAccess.SetCommand("usp_MailMessageMarkRead", connectionString: GetMailConnectionString());                
                    DataAccess.AddInputParameter("MailMessageID", mailMessageID, dbCommand);
                    DataAccess.ExecuteNonQuery(dbCommand);
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }
            });
        }

        public void MarkAsUnReadBatch(List<int> mailMessageIDs)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (MailEntities context = new MailEntities())
                {
                    var query = from m in context.MailMessages
                                where mailMessageIDs.Contains(m.MailMessageID)
                                select m;

                    foreach (var mailMessage in query)
                        mailMessage.BeenRead = false;

                    context.SaveChanges();
                }
            });
        }

        public void MarkAsReadBatch(List<int> mailMessageIDs)
        {
            ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (MailEntities context = new MailEntities())
                {
                    var query = from m in context.MailMessages
                                where mailMessageIDs.Contains(m.MailMessageID)
                                select m;

                    foreach (var mailMessage in query)
                        mailMessage.BeenRead = true;

                    context.SaveChanges();
                }
            });
        }

        public bool Send(MailMessage mailMessage, MailAccount mailAccount, int siteID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                bool result = true;
                try
                {
                    SaveToDB(mailMessage, mailAccount, NetSteps.Data.Entities.Mail.Constants.MailFolderType.Outbox, siteID);
                }
                catch (Exception ex)
                {
                    EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                    result = false;
                }
                return result;
            });
        }

        /// <summary>
        /// Removes all the items from a folder. Deletes from child tables first
        /// </summary>
        /// <param name="mailFolder"></param>
        /// <param name="mailAccount"></param>
        /// <returns></returns>
        public bool PurgeFolder(NetSteps.Data.Entities.Mail.Constants.MailFolderType mailFolderType, MailAccount mailAccount)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                bool result = true;
                IDbCommand dbCommand = null;

                try
                {
                    dbCommand = DataAccess.SetCommand("usp_PurgeFolder", connectionString: GetMailConnectionString());
                    DataAccess.AddInputParameter("MailAccountID", mailAccount.MailAccountID, dbCommand);
                    DataAccess.AddInputParameter("MailFolderTypeID", mailFolderType.ToInt(), dbCommand);
                    DataAccess.ExecuteNonQuery(dbCommand);
                }
                catch (Exception ex)
                {
                    EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                    result = false;
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }

                return result;
            });
        }

        public bool Move(MailMessage mailMessage, short destinationMailFolderTypeID)
        {
            return Move(mailMessage.MailMessageID, destinationMailFolderTypeID);
        }

        public bool Move(int mailMessageID, short destinationMailFolderTypeID)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                bool result = true;
                IDbCommand dbCommand = null;

                try
                {
                    dbCommand = DataAccess.SetCommand("usp_MailMessageMoveFolder", connectionString: GetMailConnectionString());
                    DataAccess.AddInputParameter("MailMessageID", mailMessageID, dbCommand);
                    DataAccess.AddInputParameter("MailFolderTypeID", destinationMailFolderTypeID, dbCommand);
                    DataAccess.ExecuteNonQuery(dbCommand);
                }
                catch (Exception ex)
                {
                    EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                    result = false;
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }

                return result;
            });
        }

        public bool SaveDraft(MailMessage mailMessage, MailAccount mailAccount)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                bool result = true;
                try
                {
                    SaveToDB(mailMessage, mailAccount, NetSteps.Data.Entities.Mail.Constants.MailFolderType.Drafts, 0);
                }
                catch (Exception ex)
                {
                    EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                    result = false;
                }
                return result;
            });
        }

        public MailAttachment RetrieveMailAttachment(MailMessage mailMessage, string attachmentName, MailAccount mailAccount)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                foreach (MailAttachment Attach in mailMessage.Attachments)
                {
                    if (Attach.FileName == attachmentName)
                    {
                        string uniqueName = string.Format("{0}_{1}", mailMessage.AttachmentUniqueID, attachmentName);
                        string fullPath = Path.Combine(MailAttachment.UploadFinalFolder, uniqueName);

                        Attach.AttachmentData = File.ReadAllBytes(fullPath);
                        return Attach;
                    }
                }

                return null;
            });
        }

        public List<MailMessage> LoadCollection(short MailFolderTypeID, MailAccount mailAccount)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                IDbCommand dbCommand = null;
                List<MailMessage> messages = new List<MailMessage>();

                try
                {
                    dbCommand = DataAccess.SetCommand("usp_GetAllMessageHeadersByFolder", connectionString: GetMailConnectionString());
                    DataAccess.AddInputParameter("MailFolderTypeID", MailFolderTypeID, dbCommand);
                    DataAccess.AddInputParameter("MailAccountID", mailAccount.MailAccountID, dbCommand);
                    DataSet ds = DataAccess.GetDataSet(dbCommand);

                    foreach (DataRow headerRow in ds.Tables[0].Rows)
                    {
                        MailMessage message = new MailMessage();

                        // First data table contains the mail message itself
                        //message.ExternalMessageID = ((int)headerRow["MailMessageID"]).ToString();
                        message.MailMessageID = (int)headerRow["MailMessageID"];
                        message.Subject = (string)headerRow["Subject"];
                        message.DateAddedUTC = (DateTime)headerRow["DateAddedUTC"];
                        message.FromAddress = (string)headerRow["FromAddress"];
                        message.ReplyToAddress = (string)headerRow["ReplyToAddress"];
                        message.FromNickName = (string)headerRow["FromNickName"];
                        message.BeenRead = (bool)headerRow["BeenRead"];
                        message.MailMessagePriorityID = ((short)headerRow["MailMessagePriorityID"]);
                        message.VisualTemplateID = (int)headerRow["VisualTemplateID"];
                        message.FullFolderName = ((NetSteps.Data.Entities.Mail.Constants.MailFolderType)(short)headerRow["MailFolderTypeID"]).ToString();
                        message.AttachmentUniqueID = (string)headerRow["AttachmentUniqueID"];
                        message.HasAttachments = (bool)headerRow["HasAttachments"];

                        // Second data table contains all the individual mail addresses
                        foreach (DataRow address in ds.Tables[1].Rows)
                        {
                            if ((int)address["MailMessageID"] == message.MailMessageID)
                            {
                                MailMessageRecipient recipient = new MailMessageRecipient((string)address["NickName"], (string)address["EmailAddress"]);

                                recipient.MailMessageRecipientType = (NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType)(short)address["RecipientTypeID"];

                                NetSteps.Data.Entities.Mail.Constants.EmailAddressType addressType = (NetSteps.Data.Entities.Mail.Constants.EmailAddressType)(short)address["AddressTypeID"];
                                if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.TO)
                                    message.To.Add(recipient);
                                else if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.CC)
                                    message.Cc.Add(recipient);
                                else if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.BCC)
                                    message.Bcc.Add(recipient);
                            }
                        }

                        messages.Add(message);
                    }

                    return messages;
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }
            });
        }
        #endregion

        #region Private Methods
        private static string _mailConnectionString = string.Empty;
        internal static string GetMailConnectionString()
        {
            if (_mailConnectionString.IsNullOrEmpty())
            {
                _mailConnectionString = EntitiesHelpers.GetAdoConnectionString<MailEntities>();
            }
            return _mailConnectionString;
        }

        private void SaveToDB(MailMessage mailMessage, MailAccount mailAccount, MailConstants.MailFolderType folder, int siteID)
        {
            try
            {
                //bool hasGroupRecipient = false;

                DataTable accountIDTable = new DataTable("AccountIDList");
                accountIDTable.Columns.Add("AccountID", typeof(int));

                //REVIEW:this doesn't appear to be used anywhere...
                // Build list of account ids for the downline accounts (only if this is not a draft)
                if (folder != NetSteps.Data.Entities.Mail.Constants.MailFolderType.Drafts &&
                    mailMessage.FilterResults != null &&
                    mailMessage.FilterResults.Count > 0)
                {
                    foreach (string accountIDString in mailMessage.FilterResults)
                    {
                        int accountID;

                        if (int.TryParse(accountIDString, out accountID))
                            accountIDTable.Rows.Add(accountID);
                    }
                }

                //create a table with column names and types per the DB type
                DataTable addressTable = new DataTable("AddressList");
                addressTable.Columns.Add("EmailAddress", typeof(string));
                addressTable.Columns.Add("NickName", typeof(string));
                addressTable.Columns.Add("AddressTypeID", typeof(short));
                addressTable.Columns.Add("RecipientTypeID", typeof(short));
                addressTable.Columns.Add("AccountID", typeof(int));

                AddRecipients(MailConstants.EmailAddressType.TO, mailMessage.To, addressTable, accountIDTable);
                AddRecipients(MailConstants.EmailAddressType.CC, mailMessage.Cc, addressTable, accountIDTable);
                AddRecipients(MailConstants.EmailAddressType.BCC, mailMessage.Bcc, addressTable, accountIDTable);

                DataTable attachmentTable = new DataTable("AttachmentList");
                attachmentTable.Columns.Add("FileName", typeof(string));
                attachmentTable.Columns.Add("Size", typeof(Int32));
                if (mailMessage.Attachments != null)
                {
                    foreach (MailAttachment attachment in mailMessage.Attachments)
                    {
                        attachmentTable.Rows.Add(attachment.FileName, attachment.Size);
                    }
                }

                IDbCommand dbCommand = null;
                
                try
                {
                    // If the mail message already exists (MailMessageID != 0) then it will update the record, otherwise it will insert
                    dbCommand = DataAccess.SetCommand("usp_MailMessageInsertUpdate", connectionString: GetMailConnectionString());

                    DataAccess.AddOutputParameter("NewMailMessageID", dbCommand);
                    DataAccess.AddInputParameter("MailMessageID", mailMessage.MailMessageID, dbCommand);
                    DataAccess.AddInputParameter("Subject", mailMessage.Subject ?? string.Empty, dbCommand);
                    DataAccess.AddInputParameterCheckNull("Body", mailMessage.Body, dbCommand);
                    DataAccess.AddInputParameterCheckNull("HTMLBody", mailMessage.HTMLBody, dbCommand);
                    DataAccess.AddInputParameter("DateAddedUTC", DateTime.Now.LocalToUTC(), dbCommand);
                    DataAccess.AddInputParameter("FromAddress", mailMessage.FromAddress, dbCommand);
                    DataAccess.AddInputParameter("FromNickName", mailMessage.FromNickName ?? string.Empty, dbCommand);
                    DataAccess.AddInputParameter("MailAccountID", mailAccount.MailAccountID, dbCommand);

                    // "ReplyToAddress" is optional
                    if (!string.IsNullOrWhiteSpace(mailMessage.ReplyToAddress))
                    {
                        DataAccess.AddInputParameter("ReplyToAddress", mailMessage.ReplyToAddress, dbCommand);
                    }

                    DataAccess.AddInputParameter("IsOutbound", true, dbCommand);
                    DataAccess.AddInputParameter("MailMessageTypeID", mailMessage.MailMessageTypeID, dbCommand);
                    DataAccess.AddInputParameter("BeenRead", true, dbCommand); // all composed messages have "been read", this is only useful for incoming emails
                    DataAccess.AddInputParameter("MailMessagePriorityID", mailMessage.MailMessagePriorityID, dbCommand); // casting enum to int should give me the int value
                    DataAccess.AddInputParameter("VisualTemplateID", mailMessage.VisualTemplateID, dbCommand);
                    DataAccess.AddInputParameter("MailFolderTypeID", (short)folder, dbCommand); // casting enum to int should give me the int value 
                    if (!string.IsNullOrEmpty(mailMessage.AttachmentUniqueID))
                        DataAccess.AddInputParameter("AttachmentUniqueID", mailMessage.AttachmentUniqueID, dbCommand);
                    else
                        DataAccess.AddInputParameter("AttachmentUniqueID", Guid.Empty, dbCommand);
                    DataAccess.AddInputParameter("SiteID", siteID, dbCommand);

                    bool sendDownlineToExternalEmail = ConfigurationManager.GetAppSetting<bool>("SendDownlineToExternalEmail", false);
                    DataAccess.AddInputParameter("AddExternalEmailAddresses", sendDownlineToExternalEmail, dbCommand);
                    DataAccess.AddInputParameter("CampaignActionID", mailMessage.CampaignActionID, dbCommand);
                    DataAccess.AddInputParameter("EnableEventTracking", mailMessage.EnableEventTracking, dbCommand);

                    DataAccess.AddInputParameterStructured("Addresses", "dbo.TTOutboundAddressList", addressTable, dbCommand);
                    DataAccess.AddInputParameterStructured("Attachments", "dbo.TTAttachmentList", attachmentTable, dbCommand);
                    DataAccess.AddInputParameterStructured("AccountIDs", "dbo.TTAccountIDList", accountIDTable, dbCommand);

                    DataAccess.ExecuteNonQuery(dbCommand);
                    if (mailMessage.MailMessageID == 0)
                        mailMessage.MailMessageID = DataAccess.GetInt32ReturnValue("NewMailMessageID", dbCommand);
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
            }
        }

        private void AddRecipients(MailConstants.EmailAddressType emailAddressType, IEnumerable<MailMessageRecipient> recipients, DataTable addressTable, DataTable accountIDTable)
        {
            foreach (MailMessageRecipient r in recipients)
            {
                if (r.Internal.HasValue && r.Internal.Value && r.AccountID.HasValue)
                {
                    //add recipient by AccountID
                    //	in the sproc this will be used to lookup the email address
                    accountIDTable.Rows.Add(r.AccountID.Value);
                }
                else
                {
                    //add recipient by email address
                    addressTable.Rows.Add(r.Email.ToCleanString(), r.Name.ToCleanString(), (short)emailAddressType, (short)r.MailMessageRecipientType, r.AccountID);
                }
            }
        }

        public List<MailMessage> LoadCollection(NetSteps.Data.Entities.Mail.Constants.MailFolderType mailFolder, MailAccount mailAccount)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                List<MailMessage> coll = new List<MailMessage>();
                IDbCommand dbCommand = null;

                try
                {
                    dbCommand = DataAccess.SetCommand("usp_GetAllMessageHeadersByFolder", connectionString: GetMailConnectionString());
                    DataAccess.AddInputParameter("MailFolderTypeID", (short)mailFolder.ToInt(), dbCommand);
                    DataAccess.AddInputParameter("MailAccountID", mailAccount.MailAccountID, dbCommand);
                    DataSet ds = DataAccess.GetDataSet(dbCommand);

                    foreach (DataRow headerRow in ds.Tables[0].Rows)
                    {
                        MailMessage message = new MailMessage();

                        // First data table contains the mail message itself
                        message.MailMessageID = (int)headerRow["MailMessageID"];
                        message.Subject = (string)headerRow["Subject"];
                        message.DateAddedUTC = (DateTime)headerRow["DateAddedUTC"];
                        message.FromAddress = (string)headerRow["FromAddress"];
                        message.ReplyToAddress = (string)headerRow["ReplyToAddress"];
                        message.FromNickName = (string)headerRow["FromNickName"];
                        message.BeenRead = (bool)headerRow["BeenRead"];
                        message.MailMessagePriorityID = ((short)headerRow["MailMessagePriorityID"]);
                        message.VisualTemplateID = (int)headerRow["VisualTemplateID"];
                        message.FullFolderName = ((NetSteps.Data.Entities.Mail.Constants.MailFolderType)(short)headerRow["MailFolderTypeID"]).ToString();
                        message.AttachmentUniqueID = (string)headerRow["AttachmentUniqueID"];
                        message.HasAttachments = (bool)headerRow["HasAttachments"];

                        // Second data table contains all the individual mail addresses
                        foreach (DataRow address in ds.Tables[1].Rows)
                        {
                            if ((int)address["MailMessageID"] == message.MailMessageID)
                            {
                                MailMessageRecipient recipient = new MailMessageRecipient((string)address["NickName"], (string)address["EmailAddress"]);

                                recipient.MailMessageRecipientType = (NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType)(short)address["RecipientTypeID"];

                                NetSteps.Data.Entities.Mail.Constants.EmailAddressType addressType = (NetSteps.Data.Entities.Mail.Constants.EmailAddressType)(short)address["AddressTypeID"];
                                if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.TO)
                                    message.To.Add(recipient);
                                else if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.CC)
                                    message.Cc.Add(recipient);
                                else if (addressType == NetSteps.Data.Entities.Mail.Constants.EmailAddressType.BCC)
                                    message.Bcc.Add(recipient);
                            }
                        }

                        coll.Add(message);
                    }
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsDataException);
                }
                finally
                {
                    DataAccess.Close(dbCommand);
                }

                return coll;
            });
        }


        public PaginatedList<MailMessageSearchData> Search(MailMessageSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (MailEntities context = new MailEntities())
                {
                    PaginatedList<MailMessageSearchData> results = new PaginatedList<MailMessageSearchData>(searchParameters);

                    IQueryable<MailMessage> matchingItems = context.MailMessages;

                    if (searchParameters.MailAccountID.HasValue)
                        matchingItems = matchingItems.Where(a => a.MailAccountID == searchParameters.MailAccountID.Value);

                    if (searchParameters.MailFolderTypeID.HasValue)
                        matchingItems = matchingItems.Where(a => a.MailFolderTypeID == searchParameters.MailFolderTypeID.Value);

                    if (searchParameters.BeenRead.HasValue)
                        matchingItems = matchingItems.Where(a => a.BeenRead == searchParameters.BeenRead.Value);

                    if (!string.IsNullOrEmpty(searchParameters.Subject))
                        matchingItems = matchingItems.Where(a => a.Subject.Contains(searchParameters.Subject));

                    if (!string.IsNullOrEmpty(searchParameters.Body))
                        matchingItems = matchingItems.Where(a => a.Body.Contains(searchParameters.Body) || a.HTMLBody.Contains(searchParameters.Body));

                    if (searchParameters.StartDate.HasValue)
                    {
                        DateTime startDateUTC = searchParameters.StartDate.Value.StartOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where(a => a.DateAddedUTC >= startDateUTC);
                    }
                    if (searchParameters.EndDate.HasValue)
                    {
                        DateTime endDateUTC = searchParameters.EndDate.Value.EndOfDay().LocalToUTC();
                        matchingItems = matchingItems.Where(a => a.DateAddedUTC <= endDateUTC);
                    }

                    if (searchParameters.WhereClause != null)
                        matchingItems = matchingItems.Where(searchParameters.WhereClause);

                    if (!searchParameters.OrderBy.IsNullOrEmpty())
                    {
                        switch (searchParameters.OrderBy)
                        {
                            default:
                                matchingItems = matchingItems.ApplyOrderByFilter(searchParameters, context);
                                break;
                        }
                    }
                    else
                        matchingItems = matchingItems.OrderBy(a => a.DateAddedUTC);

                    // TotalCount must be set before applying Pagination - JHE
                    results.TotalCount = matchingItems.Count();

                    matchingItems = matchingItems.ApplyPagination(searchParameters);

                    var accountInfos = matchingItems.Select(a => new
                    {
                        a.MailMessageID,
                        a.MailAccountID,
                        a.MailFolderTypeID,
                        a.Subject,
                        a.Body,
                        a.HTMLBody,
                        a.DateAddedUTC,
                        a.BeenRead,
                        a.FromAddress,
                        a.ReplyToAddress,
                        a.FromNickName
                    });

                    foreach (var a in accountInfos.ToList())
                        results.Add(new MailMessageSearchData()
                        {
                            MailMessageID = a.MailMessageID,
                            MailAccountID = a.MailAccountID,
                            MailFolderTypeID = a.MailFolderTypeID,
                            MailFolderType = ((NetSteps.Data.Entities.Mail.Constants.EmailAddressType)a.MailFolderTypeID).PascalToSpaced(),
                            Subject = a.Subject,
                            Body = a.Body,
                            HTMLBody = a.HTMLBody,
                            DateAdded = a.DateAddedUTC.UTCToLocal(),
                            BeenRead = a.BeenRead,
                            From = (string.IsNullOrEmpty(a.FromNickName) ? a.FromAddress : a.FromNickName)
                        });

                    return results;
                }
            });
        }
        #endregion
    }
}
