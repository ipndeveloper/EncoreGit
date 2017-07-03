using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using HurricaneServer.Plugins;
using HurricaneServer.Plugins.Bounce;
using HurricaneServer.Plugins.Inbound;
using HurricaneServer.Plugins.Outbound;
using MailBee.ImapMail;
using MailBee.Mime;
using NetSteps.LogHelper;

namespace NetSteps.HurricanePlugin
{
    public class NetStepsPlugin : PluginBase, IInboundSMTPConnection, IOutboundProtocol, IOutboundSMTPConnection, IBounceProcess
    {
        public enum SendMailStatus
        {
            QueuedSuccess = 1,
            SentSuccess,
            Defered,
            Bounced,
            QueuedFail,
            ProcessFail
        }

        public Dictionary<int, string> sessionIPs = new Dictionary<int, string>();

        #region Unused Outbound Events
        public void OnEvent(IProtocolData protocolData, ref Dictionary<string, object> options)
        {
        }
        public DeferAction OnDefer(IOutboundMessageEnvelope messageEnvelope, OnFailureReasons reason, ref Dictionary<string, object> options)
        {
            Log.Info("OnDefer Reason:" + reason);
            try
            {
                var message = new MailMessage();
                message.LoadMessage(messageEnvelope.GetMessage());

                var sb = new StringBuilder();
                foreach (var kvp in options)
                    sb.Append(kvp.Key + "=" + kvp.Value + "  ");

                if (message.Headers.Exists("X-ATID"))
                    ProcessAlertHeader(message, String.Format("OnDefer Reason:" + reason), (int)SendMailStatus.Defered);
            }
            catch (Exception ex)
            {
                Log.Error("OnDefer Error:{0}", ex.Message);
            }

            return DeferAction.Defer;
        }

        public void OnFail(IOutboundMessageEnvelope messageEnvelope, OnFailureReasons reason, OnFailureType type, ref Dictionary<string, object> options)
        {
            Log.Info("OnFail Reason:" + reason);
            try
            {
                ProcessOnFailMessageHeaderTypes(messageEnvelope, ref options);
            }
            catch (Exception ex)
            {
                Log.Error("OnFail Error:" + ex.Message);
            }
        }

        public void OnSent(IOutboundMessageEnvelope messageEnvelope, ref Dictionary<string, object> options)
        {
            Log.Debug("OnSent Called");
            try
            {
                var message = new MailMessage();
                message.LoadMessage(messageEnvelope.GetMessage());

                var sb = new StringBuilder();
                foreach (var kvp in options)
                    sb.Append(kvp.Key + "=" + kvp.Value + "  ");

                if (message.Headers.Exists("X-ATID")) //Alert Trigger ID
                    ProcessAlertHeader(message, sb.ToString(), (int)SendMailStatus.SentSuccess);
            }
            catch (Exception ex)
            {
                Log.Error("OnSent Error:{0}", ex.Message);
            }
        }
        #endregion

        #region unused inbound events

        public InboundResponseAction OnAuth(int sessionId, AuthResult result, string account, string password, ref Dictionary<string, object> options, ref string response)
        {
            return InboundResponseAction.Success;
        }
        public InboundResponseAction OnData(int sessionId, ref Dictionary<string, object> options, ref string response)
        {
            return InboundResponseAction.Success;
        }
        public InboundResponseAction OnFrom(int sessionId, string from, ref Dictionary<string, object> options, ref string response)
        {
            return InboundResponseAction.Success;
        }
        public InboundResponseAction OnMessageComplete(int sessionId, ref Dictionary<string, object> options, ref string response)
        {
            return InboundResponseAction.Success;
        }
        public InboundResponseAction OnRset(int sessionId, ref Dictionary<string, object> options, ref string response)
        {
            return InboundResponseAction.Success;
        }
        #endregion

        private string _attachmentFolderPath = "";
        private List<string> _localIPs = new List<string>();
        private List<string> _connectionStrings = new List<string>();
        private Dictionary<string, string> _domainConnectionStrings = new Dictionary<string, string>();

        public NetStepsPlugin()
            : base()
        {
            Log.Info("Starting Plugin");
            _localIPs = new List<string>();
            _domainConnectionStrings = new Dictionary<string, string>();
            _connectionStrings = new List<string>();

            Imap.LicenseKey = "MN200-F93135D731D031B23101B01D3CD9-F042";

            _localIPs.AddRange(ConfigurationManager.AppSettings["LocalIPList"].Split(','));
            Log.Info("LocalIPList: {0}", ConfigurationManager.AppSettings["LocalIPList"]);

            _attachmentFolderPath = ConfigurationManager.AppSettings["UploadFolder"];
            Log.Info("AttachmentFolderPath: {0}", _attachmentFolderPath);

            // load connection strings
            foreach (ConnectionStringSettings connection in ConfigurationManager.ConnectionStrings)
            {
                if (connection.Name.ToLower().StartsWith("mail"))
                {
                    _connectionStrings.Add(connection.ConnectionString);
                    Log.Info("Connection String: {0}:{1}", connection.Name, connection.ConnectionString);
                }
            }

            // loop through all the connection strings and gather a list of domains 
            foreach (string connectionString in _connectionStrings)
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                SqlCommand sqlCommand = new SqlCommand("usp_GetSiteDomains", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection
                };

                try
                {
                    sqlCommand.Connection.Open();
                    IDataReader reader = sqlCommand.ExecuteReader();

                    while (reader.Read())
                    {
                        string domainName = ((string)reader["DomainName"]).ToLower();
                        _domainConnectionStrings[domainName] = connectionString;
                        Log.Info("Domain {0}={1}", domainName, connectionString);
                    }
                }
                catch (Exception e)
                {
                    Log.Info("Exception reading siteDomains: connectionstring:{0}, Error: {1}", connectionString, e.Message);
                }
                finally
                {
                    sqlCommand.Connection.Close();
                    sqlCommand.Dispose();
                }
            }
            Log.Info("Plugin Started");
        }

        public InboundResponseAction OnConnect(int sessionId, string ip, ref Dictionary<string, object> options, ref string response)
        {
            // This records the remote IP which will be used in the OnRcptTo event to determine if this is a local or remote server
            // to determine if we should relay messages 
            lock (sessionIPs)
            {
                sessionIPs[sessionId] = ip;
            }
            return InboundResponseAction.Success;
        }

        public void OnConnectionClosed(int sessionId, ref Dictionary<string, object> options)
        {
            // remove from dictionary when the connection ends
            lock (sessionIPs)
            {
                sessionIPs.Remove(sessionId);
            }
        }

        public InboundResponseAction OnRcptTo(int sessionId, string recipient, ref Dictionary<string, object> options, ref string response)
        {
            // This event verifies the Recipients.
            Log.Debug("OnRcptTo Recipient: {0}", recipient);

            // If a local IP, then accept
            foreach (string localip in _localIPs)
            {
                string sessionIP = sessionIPs[sessionId];

                // if there is an exact match on the ip then return
                if (sessionIP == localip)
                {
                    Log.Debug("OnRcptTo LocalIP found {0} ({1}) - allowing relay", localip, sessionIP);
                    return InboundResponseAction.Success;
                }

                // if there is a wildcard then see if it matches
                if (localip.EndsWith("*") && sessionIP.StartsWith(localip.TrimEnd('*')))
                {
                    Log.Debug("OnRcptTo LocalIP found {0} ({1}) - allowing relay", localip, sessionIP);
                    return InboundResponseAction.Success;
                }
            }

            // if a remote IP verify it's an internal domain. If it is accept else reject
            EmailAddress emailaddress = new EmailAddress(recipient);
            string domainName = emailaddress.GetDomain().ToLower();
            Log.Debug("OnRcptTo Email Address: {0}, Domain: {1}", emailaddress, domainName);

            if (!_domainConnectionStrings.ContainsKey(domainName))
            {
                Log.Debug("OnRcptTo Not a local domain, rejecting relaying");
                return InboundResponseAction.Failure;
            }

            Log.Debug("OnRcptTo Local Domain Found");
            return InboundResponseAction.Success;
        }

        public MessageResponseAction OnMessageRecieved(int sessionId, ref IInboundMessageEnvelope envelope, ref Dictionary<string, object> options)
        {
            Log.Debug("OnMessageRecieved From: {0}, Recipients: {1}", envelope.MailFrom, envelope.Recipients);
            bool foundExternalAddress = false;
            string externalRecipientList = "";

            // load the message using MailBee so we can work with it.
            MailBee.Mime.MailMessage message = new MailBee.Mime.MailMessage();
            message.LoadMessage(envelope.GetMessage());

            // The Recipients are different from the To,CC,BCC lines - it's the list of people on this server we're suppose to deliver to
            // I tried to use the message.GetAllRecipients() but it had the wrong list...the envelope had the right one.
            string[] allReceipients = envelope.Recipients.Split(',');
            foreach (string aRecipient in allReceipients)
            {
                EmailAddress recipient = new EmailAddress(aRecipient.Replace("<", "").Replace(">", ""));

                Log.Debug("OnMessageRecieved looping through recipients: {0}", recipient.Email);

                string domainName = recipient.GetDomain().ToLower();
                Log.Debug("OnMessageRecieved Recipient: {0}, Domain: {1}", recipient.Email, domainName);

                // if this is a local address then save it to the db
                if (_domainConnectionStrings.ContainsKey(domainName))
                {
                    try
                    {
                        string connectionString = _domainConnectionStrings[domainName];
                        Log.Debug("OnMessageRecieved Local Domain Found. Domain: {0}, ConnectionString: {1}", domainName, connectionString);

                        // save attachments to the attachment folder

                        //create a table with column names and types per the DB type
                        DataTable addressTable = new DataTable("AddressList");
                        addressTable.Columns.Add("EmailAddress", typeof(string));
                        addressTable.Columns.Add("NickName", typeof(string));
                        addressTable.Columns.Add("AddressTypeID", typeof(short));
                        addressTable.Columns.Add("RecipientTypeID", typeof(short));

                        foreach (EmailAddress address in message.To)
                            addressTable.Rows.Add(address.Email, address.DisplayName, NetSteps.Data.Entities.Mail.Constants.EmailAddressType.TO, (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual);
                        foreach (EmailAddress address in message.Cc)
                            addressTable.Rows.Add(address.Email, address.DisplayName, NetSteps.Data.Entities.Mail.Constants.EmailAddressType.CC, (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual);
                        foreach (EmailAddress address in message.Bcc)
                            addressTable.Rows.Add(address.Email, address.DisplayName, NetSteps.Data.Entities.Mail.Constants.EmailAddressType.BCC, (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientType.Individual);

                        string uploadAttachmentID = Guid.NewGuid().ToString();
                        DataTable attachmentTable = new DataTable("AttachmentList");
                        attachmentTable.Columns.Add("FileName", typeof(string));
                        attachmentTable.Columns.Add("Size", typeof(Int32));

                        Log.Debug("OnMessageRecieved {0} Attachment(s).", message.Attachments.Count);
                        foreach (MailBee.Mime.Attachment attachment in message.Attachments)
                        {
                            string uniqueName = string.Format("{0}_{1}", uploadAttachmentID, attachment.Filename);
                            string fullPath = Path.Combine(_attachmentFolderPath, uniqueName);

                            Log.Debug("OnMessageRecieved Saving Attachment: {0}", fullPath);
                            attachment.Save(fullPath, true);
                            attachmentTable.Rows.Add(attachment.Filename, attachment.Size);
                        }

                        SqlConnection sqlConnection = new SqlConnection(connectionString);
                        SqlCommand sqlCommand = new SqlCommand("usp_MailMessageDeliverFromSMTPServer", sqlConnection)
                        {
                            CommandType = CommandType.StoredProcedure,
                            Connection = sqlConnection
                        };

                        sqlCommand.Parameters.AddWithValue("@RecipientEmailAddress", recipient.Email);
                        sqlCommand.Parameters.AddWithValue("@Subject", message.Subject);
                        sqlCommand.Parameters.AddWithValue("@Body", message.BodyPlainText);
                        sqlCommand.Parameters.AddWithValue("@HTMLBody", message.BodyHtmlText);
                        sqlCommand.Parameters.AddWithValue("@DateAddedMTN", message.Date); // date of original message
                        sqlCommand.Parameters.AddWithValue("@FromAddress", message.From.Email);
                        sqlCommand.Parameters.AddWithValue("@FromNickName", message.From.DisplayName);
                        sqlCommand.Parameters.AddWithValue("@IsOutbound", false);
                        sqlCommand.Parameters.AddWithValue("@MailMessageTypeID", (short)NetSteps.Data.Entities.Mail.Constants.EmailMessageType.AdHoc); // casting enum to int should give me the int value
                        sqlCommand.Parameters.AddWithValue("@BeenRead", false);
                        sqlCommand.Parameters.AddWithValue("@MailMessagePriorityID", (short)message.Priority); // casting enum to int should give me the int value
                        sqlCommand.Parameters.AddWithValue("@MailFolderTypeID", (short)NetSteps.Data.Entities.Mail.Constants.MailFolderType.Inbox); // casting enum to int should give me the int value 
                        sqlCommand.Parameters.AddWithValue("@AttachmentUniqueID", uploadAttachmentID);
                        sqlCommand.Parameters.AddWithValue("@SiteID", 0);

                        SqlParameter pAddresses = sqlCommand.Parameters.AddWithValue("@Addresses", addressTable);
                        pAddresses.SqlDbType = SqlDbType.Structured;
                        pAddresses.TypeName = "dbo.TTAddressList";

                        SqlParameter pAttachments = sqlCommand.Parameters.AddWithValue("@Attachments", attachmentTable);
                        pAttachments.SqlDbType = SqlDbType.Structured;
                        pAttachments.TypeName = "dbo.TTAttachmentList";

                        try
                        {
                            sqlCommand.Connection.Open();
                            sqlCommand.ExecuteNonQuery();

                            Log.Debug("OnMessageRecieved Successfully saved to db. Recipient: {0}", recipient.Email);
                        }
                        finally
                        {
                            sqlCommand.Connection.Close();
                            sqlCommand.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Debug("Exception saving email locally. Error: {0}", e.Message);
                    }
                }




                //Process Newsletter Always
                if (message.Headers.Exists("X-NLID"))
                {
                    Log.Debug("OnMessageRecieved ****Newsletter***");
                    //TODO: add more logging like the Alerts
                    return MessageResponseAction.Accept;
                }
                //Process Alert Always
                if (message.Headers.Exists("X-ATID"))
                {
                    try
                    {
                        Log.Debug("OnMessageRecieved ****Alert***");
                        ProcessAlertHeader(message, "", (int)SendMailStatus.QueuedSuccess);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("OnMessageRecieved Alert Error:{0}", ex.Message);
                    }
                    return MessageResponseAction.Accept;
                }


                // if this is an external address then add it to a modified recipients list that only contains external names.
                // Hurricane server will relay to those recipients, avoiding the internal addresses that we already saved to the db.
                else
                {
                    if (externalRecipientList != "")
                        externalRecipientList += ",";

                    externalRecipientList += string.Format("<{0}>", recipient.Email);

                    foundExternalAddress = true;
                }
            }

            // if we found an external address then we need to relay the message. First, change the recipient
            // list to only have the external addresses so it doesn't try to relay for internal addresses.
            if (foundExternalAddress)
            {
                Log.Debug("External Address Found, updating recipient list for relay: {0}", externalRecipientList);
                envelope.Recipients = externalRecipientList;
                return MessageResponseAction.Accept;
            }

            Log.Debug("No External Address Found - no relay performed");
            return MessageResponseAction.Ignore;
        }











        // ------ START OF NEW CODE - JHE

        public void OnBounce(IBounceMessageEnvelope message, ref Dictionary<string, object> options)
        {
            Log.Debug("OnBounce Called");
            try
            {
                ProcessOnBounceMessageHeaderTypes(message, ref options);
            }
            catch (Exception ex)
            {
                Log.Error("OnBounce DB Logging Error:" + ex.Message + Environment.NewLine + "InnerException" + ex.InnerException);
            }
        }


        /*      Hurricane Server can automatically tag outbound mail so that any bounces can be tracked, 
        * analyzed and reported. Hurricane Server does this by automatically creating the return path 
        * for each message it sends and embedding the tracking information you specify. You must setup 
        * a bounce domain for Hurricane Server to use in the return path and configure your DNS to 
        * point the MX records for the bounce domain back to your Hurricane Server machine. When Hurricane 
        * Server receives the bounced messages it can extract the tracking information which was embedded 
        * in the return path and use this information to figure out which account and mailing id the messaged belongs to.
        * 
        *      For more information, use the web management console to edit an account, select the 
        * bounce configuration tab and press the help button in the upper right corner.
        */

        private void ProcessOnFailMessageHeaderTypes(IOutboundMessageEnvelope messageEnvelope, ref Dictionary<string, object> options)
        {
            var message = new MailMessage();
            message.LoadMessage(messageEnvelope.GetMessage());

            var sb = new StringBuilder();
            foreach (var kvp in options)
                sb.Append(kvp.Key + "=" + kvp.Value + "  ");

            if (message.Headers.Exists("X-NLID"))
                ProcessNewsletterHeader(message, sb.ToString(), (int)SendMailStatus.ProcessFail);
            else if (message.Headers.Exists("X-ATID"))
                ProcessAlertHeader(message, sb.ToString(), (int)SendMailStatus.ProcessFail);
        }

        private void ProcessOnBounceMessageHeaderTypes(IBounceMessageEnvelope messageEnvelope, ref Dictionary<string, object> options)
        {
            var sb = new StringBuilder();
            foreach (var kvp in options)
                sb.Append(kvp.Key + "=" + kvp.Value + "  ");

            var message = new MailMessage();
            message.LoadMessage(messageEnvelope.Message);

            if (message.Headers.Exists("X-NLID"))
                ProcessNewsletterHeader(message, sb.ToString(), (int)SendMailStatus.Bounced);
            else if (message.Headers.Exists("X-ATID"))
                ProcessAlertHeader(message, sb.ToString(), (int)SendMailStatus.Bounced);
        }


        private void ProcessNewsletterHeader(MailMessage message, string exceptionString, int sendMailStatusID)
        {
            var newsletterId = 0;
            var contactId = 0;
            var sender = string.Empty;

            foreach (Header h in message.Headers)
            {
                if (h.Name == "X-NLID")
                    newsletterId = Convert.ToInt32(h.Value);
                if (h.Name == "X-AID")
                    contactId = Convert.ToInt32(h.Value);
                if (h.Name == "Sender-ID")
                    sender = h.Value.Replace("<", "").Replace(">", "");
            }
            var senderAddress = new EmailAddress(sender);
            var domainName = senderAddress.GetDomain().ToLower();
            var msg = String.Format("Newsletter Event:{0} NewsletterID={1}|AccountID={2}|Domain={3}", (SendMailStatus)sendMailStatusID, newsletterId, contactId, domainName);
            Log.Info(msg);

            switch ((SendMailStatus)sendMailStatusID)
            {
                case SendMailStatus.Bounced:
                    NewsletterBounceFailTracking(0, 0, message.To.AsString, msg, "", domainName, 2);
                    break;
                case SendMailStatus.ProcessFail:
                    NewsletterBounceFailTracking(newsletterId, contactId, "", msg, exceptionString, domainName, 1);
                    break;
            }
        }

        private void ProcessAlertHeader(MailMessage message, string exceptionString, int sendMailStatusID)
        {
            var alertTriggerId = 0;
            var consultantId = 0;
            var sender = string.Empty;

            foreach (Header h in message.Headers)
            {
                if (h.Name == "X-ATID")
                    alertTriggerId = Convert.ToInt32(h.Value);
                if (h.Name == "X-AID")
                    consultantId = Convert.ToInt32(h.Value);
                if (h.Name == "Sender-ID")
                    sender = h.Value.Replace("<", "").Replace(">", "");
            }
            var senderAddress = new EmailAddress(sender);
            var domainName = senderAddress.GetDomain().ToLower();
            var msg = String.Format("Alert {0}: AlertTriggerID={1}|AccountID={2}|Domain={3}", (SendMailStatus)sendMailStatusID, alertTriggerId, consultantId, domainName);
            Log.Info(msg);

            if ((SendMailStatus)sendMailStatusID == SendMailStatus.QueuedFail || (SendMailStatus)sendMailStatusID == SendMailStatus.ProcessFail ||
                (SendMailStatus)sendMailStatusID == SendMailStatus.Bounced)
                exceptionString += Environment.NewLine + msg;

            AlertSendStatus(alertTriggerId, sendMailStatusID, exceptionString, domainName);
        }


        private void NewsletterBounceFailTracking(int newsletterId, int contactId, string bounceEmailAddress, string errorMessage, string innerException, string domainName, int errorType)
        {
            SqlCommand sqlCommand = null;

            try
            {
                var connectionString = _domainConnectionStrings[domainName];

                var sqlConnection = new SqlConnection(connectionString);
                sqlCommand = new SqlCommand("usp_NewsletterSendError", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection
                };
                sqlCommand.Parameters.AddWithValue("@newsletterID", newsletterId);
                sqlCommand.Parameters.AddWithValue("@contact_AccountID", contactId);
                sqlCommand.Parameters.AddWithValue("@bounced_Email", bounceEmailAddress);
                sqlCommand.Parameters.AddWithValue("@errorMsg", errorMessage);
                sqlCommand.Parameters.AddWithValue("@innerException", innerException);
                sqlCommand.Parameters.AddWithValue("@errorTypeID", errorType);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();

                Log.Debug(String.Format("Fail or bounce saved to db. NewsletterID:{0}  Recipient AccountID:{1} OR Bounced Address:{2}", newsletterId, contactId, bounceEmailAddress));
            }
            catch (Exception ex)
            {
                Log.Error("Failed to upload Newsletter fail or bounce.  Error:" + ex.Message);
            }
            finally
            {
                if (sqlCommand != null)
                {
                    sqlCommand.Connection.Close();
                    sqlCommand.Dispose();
                }
            }
        }

        private void AlertSendStatus(int alertTriggerId, int sendMailStatusId, string message, string domainName)
        {
            SqlCommand sqlCommand = null;

            try
            {
                var connectionString = _domainConnectionStrings[domainName];

                var sqlConnection = new SqlConnection(connectionString);
                sqlCommand = new SqlCommand("usp_AlertSendStatus_Insert", sqlConnection)
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = sqlConnection
                };
                sqlCommand.Parameters.AddWithValue("@alertTriggerID", alertTriggerId);
                sqlCommand.Parameters.AddWithValue("@sendMailStatusID", sendMailStatusId);
                sqlCommand.Parameters.AddWithValue("@message", message);
                sqlCommand.Connection.Open();
                sqlCommand.ExecuteNonQuery();

                Log.Debug(String.Format("AlertSendStatus insert for AlertTriggerID:{0}", alertTriggerId));
            }
            catch (Exception ex)
            {
                Log.Error("Failed to insert to AlertSendStatus.{0}      Error:{1}{0}      AlertTriggerID:{2} Message:{3}", Environment.NewLine, ex.Message, alertTriggerId, message);
            }
            finally
            {
                if (sqlCommand != null)
                {
                    sqlCommand.Connection.Close();
                    sqlCommand.Dispose();
                }
            }
        }




    }
}