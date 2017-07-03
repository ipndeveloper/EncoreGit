using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HurricaneServer.Plugins.Inbound;
using MailBee.Mime;
using MailMessage = MailBee.Mime.MailMessage;
using System.IO;

namespace NetSteps.Hurricane.Plugin
{
    public class DataAccess
    {
        #region Public
        public List<string> GetClientDomains(string connectionString)
        {
            var domains = new List<string>();

            var sqlCommand = new SqlCommand
            {
                Connection = new SqlConnection(connectionString),
                CommandType = CommandType.StoredProcedure,
                CommandText = "usp_GetSiteDomains"
            };

            try
            {
                sqlCommand.Connection.Open();
                var reader = sqlCommand.ExecuteReader();

                while (reader.Read())
                {
                    string domain = ((string)reader["DomainName"]).ToLower();
                    domains.Add(domain);
                }
            }
            catch (Exception ex)
            {
                LogSqlException(sqlCommand, ex);
            }
            finally
            {
                sqlCommand.Connection.Close();
                sqlCommand.Dispose();
            }

            return domains;
        }

        public void SaveInboundEmail(IInboundMessageEnvelope envelope, string recipientEmailAddress, string attachmentFolderPath, string connectionString, string databaseVersion)
        {
            // Load the message using MailBee so we can work with it.
            MailMessage message = new MailMessage();
            message.LoadMessage(envelope.GetMessage());

            // Build address table
            DataTable addressTable = CreateAddressTable(message);            

            // Save attachments and build attachment table
            string uploadAttachmentID = Guid.NewGuid().ToString();
            DataTable attachmentTable = CreateAttachmentTable(message, attachmentFolderPath, uploadAttachmentID);

            SaveInboundEmailToDatabase(message, addressTable, attachmentTable, recipientEmailAddress, uploadAttachmentID, connectionString, databaseVersion);
        }

        public string GetContent(int siteId, string sectionName, string connectionString)
        {
            string content = string.Empty;

            var sqlCommand = new SqlCommand
            {
                Connection = new SqlConnection(connectionString),
                CommandType = CommandType.StoredProcedure,
                CommandText = "usp_htmlcontent_select_by_sectionname_and_siteid"
            };

            sqlCommand.Parameters.AddWithValue("@SectionName", sectionName);
            sqlCommand.Parameters.AddWithValue("@SiteID", siteId);

            try
            {
                sqlCommand.Connection.Open();
                var reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    content = (string)reader["Html"];
                }
            }
            catch (Exception ex)
            {
                LogSqlException(sqlCommand, ex);
            }
            finally
            {
                sqlCommand.Connection.Close();
                sqlCommand.Dispose();
            }

            return content;
        }

        public int? GetAccountTypeId(HurricaneMailClient client, string emailAddress)
        {
            int? accountTypeId = null;

            var sqlCommand = new SqlCommand
            {
                Connection = new SqlConnection(client.ConnectionString),
                CommandType = CommandType.StoredProcedure,
                CommandText = "usp_get_account_type_id_by_emailaddress"
            };

            sqlCommand.Parameters.AddWithValue("@EmailAddress", emailAddress);

            try
            {
                sqlCommand.Connection.Open();
                var reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    accountTypeId = (int)reader["AccountTypeID"];
                }
            }
            catch (Exception ex)
            {
                LogSqlException(sqlCommand, ex);
            }
            finally
            {
                sqlCommand.Connection.Close();
                sqlCommand.Dispose();
            }

            return accountTypeId;
        }

        public List<string> GetAutoForwardEmailAddresses(HurricaneMailClient client, string emailAddress)
        {
            var autoForwardEmailAddresses = new List<string>();

            var sqlCommand = new SqlCommand
            {
                Connection = new SqlConnection(client.ConnectionString),
                CommandType = CommandType.StoredProcedure,
                CommandText = "usp_secondaryemailaddresses_mailaccountid"
            };

            sqlCommand.Parameters.AddWithValue("@InternalEmailAddress", emailAddress);

            try
            {
                sqlCommand.Connection.Open();
                var reader = sqlCommand.ExecuteReader();

                if (reader.Read())
                {
                    string autoForwardEmailAddress = (string)reader["EmailAddress"];
                    autoForwardEmailAddresses.Add(autoForwardEmailAddress);
                }
            }
            catch (Exception ex)
            {
                LogSqlException(sqlCommand, ex);
            }
            finally
            {
                sqlCommand.Connection.Close();
                sqlCommand.Dispose();
            }

            return autoForwardEmailAddresses;
        }

        public void UpdateMailMessageRecipientStatus(
            HurricaneMailClient client,
            int mailMessageGroupID,
            string recipientEmailAddress,
            EmailRecipientStatus emailRecipientStatus)
        {
            var sqlCommand = new SqlCommand
            {
                Connection = new SqlConnection(client.ConnectionString),
                CommandType = CommandType.StoredProcedure,
                CommandText = "usp_UpdateRecipientStatusByEmailAddress"
            };

            sqlCommand.Parameters.AddWithValue("@MailMessageGroupID", mailMessageGroupID);
            sqlCommand.Parameters.AddWithValue("@RecipientEmailAddress", recipientEmailAddress);
            sqlCommand.Parameters.AddWithValue("@RecipientStatusID", (short)emailRecipientStatus);

            try
            {
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                Log.Debug("MailMessageRecipientStatus successfully saved to db. Client: {0}, MailMessageGroupID: {1}, RecipientEmailAddress: {2}, RecipientStatus: {3}",
                    client.Name, mailMessageGroupID, recipientEmailAddress, emailRecipientStatus);
            }
            catch (Exception ex)
            {
                LogSqlException(sqlCommand, ex);
            }
            finally
            {
                sqlCommand.Connection.Close();
                sqlCommand.Dispose();
            }
        }

        public void SaveMailMessageRecipientEvent(
            HurricaneMailClient client,
            int mailMessageGroupID,
            string recipientEmailAddress,
            MailMessageRecipientEventType mailMessageRecipientEventType,
            string text,
            string url)
        {
            var sqlCommand = new SqlCommand
            {
                Connection = new SqlConnection(client.ConnectionString),
                CommandType = CommandType.StoredProcedure,
                CommandText = "usp_MailMessageRecipientEvent_InsertByEmailAddress"
            };

            sqlCommand.Parameters.AddWithValue("@MailMessageGroupID", mailMessageGroupID);
            sqlCommand.Parameters.AddWithValue("@RecipientEmailAddress", recipientEmailAddress);
	        sqlCommand.Parameters.AddWithValue("@MailMessageRecipientEventTypeID", (short)mailMessageRecipientEventType);
	        sqlCommand.Parameters.AddWithValue("@DateCreatedUTC", DateTime.UtcNow);
            sqlCommand.Parameters.AddWithValue("@Text", text);
            sqlCommand.Parameters.AddWithValue("@Url", url);

            try
            {
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                Log.Debug("MailMessageRecipientEvent successfully saved to db. Client: {0}, MailMessageGroupID: {1}, RecipientEmailAddress: {2}, Type: {3}",
                    client.Name, mailMessageGroupID, recipientEmailAddress, mailMessageRecipientEventType);
            }
            catch (Exception ex)
            {
                LogSqlException(sqlCommand, ex);
            }
            finally
            {
                sqlCommand.Connection.Close();
                sqlCommand.Dispose();
            }
        }
        #endregion

        #region Private
        private DataTable CreateAddressTable(MailMessage message)
        {
            // Create a table with column names and types per the DB type
            DataTable addressTable = new DataTable("AddressList");
            addressTable.Columns.Add("EmailAddress", typeof(string));
            addressTable.Columns.Add("NickName", typeof(string));
            addressTable.Columns.Add("AddressTypeID", typeof(short));
            addressTable.Columns.Add("RecipientTypeID", typeof(short));

            // Populate table
            foreach (EmailAddress address in message.To)
                addressTable.Rows.Add(address.Email, address.DisplayName, (short)EmailAddressType.TO, (short)MailMessageRecipientType.Individual);
            foreach (EmailAddress address in message.Cc)
                addressTable.Rows.Add(address.Email, address.DisplayName, (short)EmailAddressType.CC, (short)MailMessageRecipientType.Individual);
            foreach (EmailAddress address in message.Bcc)
                addressTable.Rows.Add(address.Email, address.DisplayName, (short)EmailAddressType.BCC, (short)MailMessageRecipientType.Individual);

            return addressTable;
        }

        private DataTable CreateAttachmentTable(MailMessage message, string attachmentFolderPath, string uploadAttachmentID)
        {
            // Create a table with column names and types per the DB type
            DataTable attachmentTable = new DataTable("AttachmentList");
            attachmentTable.Columns.Add("FileName", typeof(string));
            attachmentTable.Columns.Add("Size", typeof(Int32));

            if (message.Attachments.Count > 0)
            {
                // Save attachments and populate table
                Log.Debug("Processing {0} Attachment(s).", message.Attachments.Count);
                foreach (Attachment attachment in message.Attachments)
                {
                    string uniqueName = string.Format("{0}_{1}", uploadAttachmentID, attachment.Filename);
                    string fullPath = Path.Combine(attachmentFolderPath, uniqueName);

                    Log.Debug("OnMessageRecieved Saving Attachment: {0}", fullPath);
                    attachment.Save(fullPath, true);
                    attachmentTable.Rows.Add(attachment.Filename, attachment.Size);
                }
            }

            return attachmentTable;
        }

        private void SaveInboundEmailToDatabase(
            MailMessage message,
            DataTable addressTable,
            DataTable attachmentTable,
            string recipientEmailAddress,
            string uploadAttachmentID,
            string connectionString,
            string databaseVersion)
        {
            var sqlCommand = new SqlCommand
            {
                Connection = new SqlConnection(connectionString),
                CommandType = CommandType.StoredProcedure,
                CommandText = "usp_MailMessageDeliverFromSMTPServer"
            };

            sqlCommand.Parameters.AddWithValue("@RecipientEmailAddress", recipientEmailAddress);
            sqlCommand.Parameters.AddWithValue("@Subject", message.Subject);
            sqlCommand.Parameters.AddWithValue("@Body", message.BodyPlainText);
            sqlCommand.Parameters.AddWithValue("@HTMLBody", message.BodyHtmlText);
            sqlCommand.Parameters.AddWithValue("@FromAddress", message.From.Email);
            sqlCommand.Parameters.AddWithValue("@FromNickName", message.From.DisplayName);
            sqlCommand.Parameters.AddWithValue("@IsOutbound", false);
            sqlCommand.Parameters.AddWithValue("@MailMessageTypeID", (short)EmailMessageType.AdHoc);
            sqlCommand.Parameters.AddWithValue("@BeenRead", false);
            sqlCommand.Parameters.AddWithValue("@MailMessagePriorityID", (short)message.Priority);
            sqlCommand.Parameters.AddWithValue("@MailFolderTypeID", (short)MailFolderType.Inbox);
            sqlCommand.Parameters.AddWithValue("@AttachmentUniqueID", uploadAttachmentID);
            sqlCommand.Parameters.AddWithValue("@SiteID", 0);

            SqlParameter pAddresses = sqlCommand.Parameters.AddWithValue("@Addresses", addressTable);
            pAddresses.SqlDbType = SqlDbType.Structured;
            pAddresses.TypeName = "dbo.TTAddressList";

            SqlParameter pAttachments = sqlCommand.Parameters.AddWithValue("@Attachments", attachmentTable);
            pAttachments.SqlDbType = SqlDbType.Structured;
            pAttachments.TypeName = "dbo.TTAttachmentList";

            // Legacy dbs use local time
            if(!string.IsNullOrEmpty(databaseVersion) && databaseVersion.StartsWith("4"))
                sqlCommand.Parameters.AddWithValue("@DateAddedMTN", DateTime.Now);
            else
                sqlCommand.Parameters.AddWithValue("@DateAddedUTC", DateTime.UtcNow);

            try
            {
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();
                Log.Debug("Successfully saved to db. Recipient: {0}", recipientEmailAddress);
            }
            catch (Exception ex)
            {
                LogSqlException(sqlCommand, ex);

                // We want to know if this fails.
                throw ex;
            }
            finally
            {
                sqlCommand.Connection.Close();
                sqlCommand.Dispose();
            }
        }

        private void LogSqlException(SqlCommand sqlCommand, Exception ex)
        {
            Log.Error("Exception in SQL action: Database: {0}, Command: {1}, Error: {2}", sqlCommand.Connection.Database, sqlCommand.CommandText, ex.Message);
        }
        #endregion
    }
}
