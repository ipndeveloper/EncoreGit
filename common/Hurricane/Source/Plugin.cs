using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using HurricaneServer.Plugins;
using HurricaneServer.Plugins.Bounce;
using HurricaneServer.Plugins.Inbound;
using HurricaneServer.Plugins.OpenClickTracking;
using HurricaneServer.Plugins.Outbound;
using MailBee.Mime;
using MailMessage = MailBee.Mime.MailMessage;

namespace NetSteps.Hurricane.Plugin
{
    public class Plugin : PluginBase, IInboundSMTPConnection, IOutboundSMTPConnection, IBounceProcess, IOpenClickTracking
    {
        private DataAccess _db = new DataAccess();
        private List<string> _localIPs = new List<string>();
        private Dictionary<int, string> _sessionIPs = new Dictionary<int, string>();
        private HurricaneMailClientCollection _clients = ((CustomSettings)ConfigurationManager.GetSection("customSettings")).HurricaneMailClients;
        private Dictionary<string, HurricaneMailClient> _clientsByDomain = new Dictionary<string, HurricaneMailClient>();
        private Dictionary<string, HurricaneMailClient> _clientsByHurricaneAccountId = new Dictionary<string, HurricaneMailClient>();

        public Plugin() : base()
        {
            Log.Info("Starting Plugin");

            _localIPs.AddRange(ConfigurationManager.AppSettings["LocalIPList"].Split(','));
            Log.Info("LocalIPList: {0}", ConfigurationManager.AppSettings["LocalIPList"]);

            // Load client info and dictionaries
            foreach (HurricaneMailClient client in _clients)
            {
                // Check if database is available
                if (!string.IsNullOrEmpty(client.ConnectionString))
                {
                    // Get domain names from database
                    Log.Info("Getting domains for {0}", client.Name);
                    foreach (string domain in _db.GetClientDomains(client.ConnectionString))
                    {
                        Log.Info("Domain: {0}", domain);
                        _clientsByDomain[domain] = client;
                    }

                    // Get content, if specified in config
                    if (client.AutoForwardDisclaimerSiteId.HasValue)
                    {
                        if (!string.IsNullOrEmpty(client.AutoForwardDisclaimerSectionNameHtml))
                        {
                            Log.Info("Getting disclaimer HTML content for {0}", client.Name);
                            client.AutoForwardDisclaimerContentHtml = _db.GetContent(
                                client.AutoForwardDisclaimerSiteId.Value,
                                client.AutoForwardDisclaimerSectionNameHtml,
                                client.ConnectionString);
                        }
                        
                        if (!string.IsNullOrEmpty(client.AutoForwardDisclaimerSectionNameText))
                        {
                            Log.Info("Getting disclaimer text content for {0}", client.Name);
                            client.AutoForwardDisclaimerContentText = _db.GetContent(
                                client.AutoForwardDisclaimerSiteId.Value,
                                client.AutoForwardDisclaimerSectionNameText,
                                client.ConnectionString);
                        }
                    }
                }

                // Check if Hurricane AccountId exists
                if (!string.IsNullOrEmpty(client.HurricaneAccountId))
                {
                    _clientsByHurricaneAccountId[client.HurricaneAccountId] = client;
                }
            }

            Log.Info("Plugin Started");
        }

        #region IInboundSMTPConnection Events
        public InboundResponseAction OnConnect(int sessionId, string ip, ref Dictionary<string, object> options, ref string response)
        {
            // Records the remote IP which will be used in the OnRcptTo event to determine if we should relay messages.
            lock (_sessionIPs)
            {
                _sessionIPs[sessionId] = ip;
            }

            return InboundResponseAction.Success;
        }

        public void OnConnectionClosed(int sessionId, ref Dictionary<string, object> options)
        {
            // Remove from dictionary when the connection ends
            lock (_sessionIPs)
            {
                _sessionIPs.Remove(sessionId);
            }
        }

        public InboundResponseAction OnRcptTo(int sessionId, string recipient, ref Dictionary<string, object> options, ref string response)
        {
            Log.Debug("OnRcptTo Recipient: {0}", recipient);

            // Allow local IPs
            if (_sessionIPs.ContainsKey(sessionId))
            {
                string sessionIP = _sessionIPs[sessionId];

                foreach (string localIP in _localIPs)
                {
                    // Check for exact match or wildcard match
                    if (localIP == sessionIP
                        || (localIP.EndsWith("*") && sessionIP.StartsWith(localIP.TrimEnd('*'))))
                    {
                        Log.Debug("OnRcptTo LocalIP found {0} ({1}) - allowing relay", localIP, sessionIP);
                        return InboundResponseAction.Success;
                    }
                }
            }

            // Else allow local domains
            var emailaddress = new EmailAddress(recipient.ToLower());
            string domain = emailaddress.GetDomain();
            Log.Debug("OnRcptTo Email Address: {0}, Domain: {1}", emailaddress, domain);

            if (_clientsByDomain.ContainsKey(domain))
            {
                Log.Debug("OnRcptTo Local Domain Found");
                return InboundResponseAction.Success;
            }

            // Else fail
            Log.Debug("OnRcptTo Not a local IP or domain, rejecting relay");
            return InboundResponseAction.Failure;
        }

        public MessageResponseAction OnMessageRecieved(int sessionId, ref IInboundMessageEnvelope envelope, ref Dictionary<string, object> options)
        {
            Log.Debug("OnMessageReceived SystemMessageId: {0}, CustomMessageId: {1}, MailingId: {2}, MailFrom: {3}, Recipients: {4}",
                envelope.SystemMessageId,
                envelope.CustomMessageId,
                envelope.MailingId,
                envelope.MailFrom,
                envelope.Recipients);

            foreach (var option in options)
            {
                Log.Debug("OnMessageReceived Option Key: {0}, Value: {1}", option.Key, option.Value);
            }

            // Message will be parsed into this container later if any modifications are necessary
            MailMessage message = null;

            // Inbound processing
            var inboundMessageResponseAction = OnMessageReceived_ProcessInbound(ref envelope, ref options, ref message);

            // If inbound processing handled all recipients then we are done.
            if (inboundMessageResponseAction == MessageResponseAction.Ignore)
            {
                Log.Debug("No External Address Found - no relay performed");
                return MessageResponseAction.Ignore;
            }

            // Outbound processing
            OnMessageReceived_ProcessOutbound(ref envelope, ref options, ref message);

            // Save message if modified
            if (message != null)
            {
                try
                {
                    // MailBee has a "feature" when saving modified headers. It encodes the headers
                    // using the same charset as the body. This is a problem because ASP.NET often
                    // composes messages using UTF-8 for the headers and ASCII for the body. Then
                    // MailBee changes the headers to ASCII and we lose any international characters
                    // from the headers. The workaround is to always encode headers with UTF-8. - JGL
                    // See http://afterlogic.com/mailbee-net/docs/MailBee.Mime.MailMessage.Charset.html
                    message.EncodeAllHeaders(Encoding.UTF8, HeaderEncodingOptions.None);

                    var messageBytes = message.GetMessageRawData();
                    envelope.SetMessage(new MemoryStream(messageBytes), messageBytes.Length);
                }
                catch (Exception ex)
                {
                    Log.Error("OnMessageReceived Error Saving Message: {0}", ex.Message);
                }
            }
            
            return MessageResponseAction.Accept;
        }

        private MessageResponseAction OnMessageReceived_ProcessInbound(ref IInboundMessageEnvelope envelope, ref Dictionary<string, object> options, ref MailMessage message)
        {
            // Client will be set later if any local recipients are found
            HurricaneMailClient client = null;

            // Parse envelope recipients
            var recipients = envelope.Recipients
                                .Split(',')
                                .Select(r => new EmailAddress(r.Replace("<", "").Replace(">", "").ToLower()))
                                .ToList();

            // Containers for recipients that will be removed/added from the envelope
            var deliveredRecipients = new List<EmailAddress>();
            var autoForwardRecipients = new List<EmailAddress>();

            foreach (var recipient in recipients)
            {
                string domain = recipient.GetDomain();
                Log.Debug("OnMessageReceived Recipient: {0}", recipient.Email);

                if (_clientsByDomain.ContainsKey(domain)) // Local domain
                {
                    client = _clientsByDomain[domain];

                    if (client.EnableLocalDelivery) // Local delivery
                    {
                        Log.Debug("OnMessageReceived Local Delivery Client: {0}, Recipient: {1}", client.Name, recipient.Email);

                        try
                        {
                            _db.SaveInboundEmail(envelope, recipient.Email, client.AttachmentFolderPath, client.ConnectionString, client.DatabaseVersion);

                            // Local delivery was successful. We can remove this recipient.
                            deliveredRecipients.Add(recipient);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Exception saving email locally. Client: {0}, Recipient: {1}, Error: {2}", client.Name, recipient.Email, ex.Message);

                            // Note: Local delivery failed. We will leave the recipient on the envelope and
                            //       Hurricane will generate a routing loop NDR. This seems like the
                            //       best option to notify the sender that delivery failed. If we
                            //       remove the recipient from the envelope, no one will ever know
                            //       that delivery failed.
                        }
                    }

                    if (client.EnableAutoForward)
                    {
                        try
                        {
                            // Filter by AccountTypeId if it was specified
                            if (client.AutoForwardAccountTypeId == null
                                || client.AutoForwardAccountTypeId == _db.GetAccountTypeId(client, recipient.Email))
                            {
                                Log.Debug("OnMessageReceived Auto Forward Client: {0}, Recipient: {1}", client.Name, recipient.Email);

                                // Get auto forward email address(es)
                                var recipientAFEmailAddresses = _db.GetAutoForwardEmailAddresses(client, recipient.Email);

                                // Parse auto forward recipients
                                var recipientAFRecipients = recipientAFEmailAddresses
                                                                .Select(e => new EmailAddress(e.ToLower()));

                                // Ignore local domains (there shouldn't be any)
                                recipientAFRecipients = recipientAFRecipients
                                                            .Where(e => !_clientsByDomain.ContainsKey(e.GetDomain()));

                                // Add auto forward recipients to outer collection
                                autoForwardRecipients.AddRange(recipientAFRecipients);

                                Log.Debug("OnMessageReceived Auto Forward Client: {0}, Added: {1}", client.Name, string.Join(",", recipientAFRecipients.Select(e => e.Email).ToArray()));
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Error("OnMessageReceived Auto Forward Error Client: {0}, Recipient: {1}, Error: {2}", client.Name, recipient.Email, ex.Message);
                        }
                    }
                }
            }

            // If the recipients haven't changed, we are done.
            if (deliveredRecipients.Count == 0 && autoForwardRecipients.Count == 0)
            {
                return MessageResponseAction.Accept;
            }

            // Remove delivered recipients
            if (deliveredRecipients.Count > 0)
            {
                recipients = recipients.Except(deliveredRecipients).ToList();
            }

            // Add auto forward recipients
            if (autoForwardRecipients.Count > 0)
            {
                recipients.AddRange(autoForwardRecipients);

                // Append any auto forward disclaimers
                if (client != null
                    && (!string.IsNullOrEmpty(client.AutoForwardDisclaimerContentHtml) || !string.IsNullOrEmpty(client.AutoForwardDisclaimerContentText)))
                {
                    Log.Debug("OnMessageReceived Auto Forward Disclaimer Client: {0}", client.Name);

                    try
                    {
                        // Lazy-load message
                        if (message == null)
                        {
                            message = new MailMessage();
                            message.LoadMessage(envelope.GetMessage());
                        }

                        message.BodyHtmlText += "<br /><br />" + client.AutoForwardDisclaimerContentHtml;
                        message.BodyPlainText += "\n\n" + client.AutoForwardDisclaimerContentText;
                    }
                    catch (Exception ex)
                    {
                        Log.Error("OnMessageReceived Auto Forward Disclaimer Error Client: {0}, Error: {1}", client.Name, ex.Message);
                    }
                }
            }

            // If there are no recipients left, we are done (and we can ignore the message).
            if (recipients.Count == 0)
            {
                Log.Debug("No External Address Found - no relay performed");
                return MessageResponseAction.Ignore;
            }

            // The recipient list has changed, so we need to update the envelope recipients
            string externalRecipientString = string.Join(",", recipients.Select(r => string.Format("<{0}>", r.Email)).ToArray());
            Log.Debug("Updating recipient list for relay: {0}", externalRecipientString);
            envelope.Recipients = externalRecipientString;

            return MessageResponseAction.Accept;
        }

        private void OnMessageReceived_ProcessOutbound(ref IInboundMessageEnvelope envelope, ref Dictionary<string, object> options, ref MailMessage message)
        {
            // Outbound processing requires a matching Hurricane AccountId in app.config
            // Hurricane embeds the AccountId in the options dictionary (see SDK)
            if (options.ContainsKey("AccountId")
                && _clientsByHurricaneAccountId.ContainsKey(options["AccountId"].ToString()))
            {
                var client = _clientsByHurricaneAccountId[options["AccountId"].ToString()];

                // Override MailFrom (this came from Natura)
                if (!string.IsNullOrEmpty(client.OverrideMailFrom))
                {
                    Log.Debug("OnMessageReceived Override Client: {0}, Mail From: {1}", client.Name, client.OverrideMailFrom);

                    envelope.MailFrom = client.OverrideMailFrom;
                }

                // Override Sender (this came from Natura)
                if (!string.IsNullOrEmpty(client.OverrideSender))
                {
                    Log.Debug("OnMessageReceived Override Client: {0}, Sender: {1}", client.Name, client.OverrideSender);

                    try
                    {
                        // Lazy-load message
                        if (message == null)
                        {
                            message = new MailMessage();
                            message.LoadMessage(envelope.GetMessage());
                        }

                        message.Headers.Add("Sender", client.OverrideSender, true);
                    }
                    catch (Exception ex)
                    {
                        Log.Error("OnMessageReceived Override Error Client: {0}, Sender: {1}, Error: {2}", client.Name, client.OverrideSender, ex.Message);
                    }
                }
            }
        }
        #endregion

        #region IOutboundSMTPConnection Events
        public DeferAction OnDefer(IOutboundMessageEnvelope messageEnvelope, OnFailureReasons reason, ref Dictionary<string, object> options)
        {
            Log.Debug("OnDefer: SystemMessageId: {0}, AccountId: {1}, CustomMessageId: {2}, MailingId: {3}, Reason: {4}, Sender: {5}, Recipient: {6}, Domain: {7}, StatusMessage: {8}",
                messageEnvelope.SystemMessageId,
                messageEnvelope.AccountId,
                messageEnvelope.CustomMessageId,
                messageEnvelope.MailingId,
                reason,
                messageEnvelope.Sender,
                messageEnvelope.Recipient,
                messageEnvelope.Domain,
                messageEnvelope.StatusMessage);

            foreach (var option in options)
            {
                Log.Debug("OnDefer Option: Key: {0}, Value: {1}", option.Key, option.Value);
            }

            return DeferAction.Defer;
        }

        public void OnFail(IOutboundMessageEnvelope messageEnvelope, OnFailureReasons reason, OnFailureType type, ref Dictionary<string, object> options)
        {
            Log.Debug("OnFail: SystemMessageId: {0}, AccountId: {1}, CustomMessageId: {2}, MailingId: {3}, Reason: {4}, Type: {5}, Sender: {6}, Recipient: {7}, Domain: {8}, StatusMessage: {9}",
                messageEnvelope.SystemMessageId,
                messageEnvelope.AccountId,
                messageEnvelope.CustomMessageId,
                messageEnvelope.MailingId,
                reason,
                type,
                messageEnvelope.Sender,
                messageEnvelope.Recipient,
                messageEnvelope.Domain,
                messageEnvelope.StatusMessage);

            foreach (var option in options)
            {
                Log.Debug("OnFail Option: Key: {0}, Value: {1}", option.Key, option.Value);
            }

            ProcessRecipientStatusUpdate(messageEnvelope, EmailRecipientStatus.DeliveryError);
        }

        public void OnSent(IOutboundMessageEnvelope messageEnvelope, ref Dictionary<string, object> options)
        {
            Log.Debug("OnSent: SystemMessageId: {0}, AccountId: {1}, CustomMessageId: {2}, MailingId: {3}, Sender: {4}, Recipient: {5}, Domain: {6}, StatusMessage: {7}",
                messageEnvelope.SystemMessageId,
                messageEnvelope.AccountId,
                messageEnvelope.CustomMessageId,
                messageEnvelope.MailingId,
                messageEnvelope.Sender,
                messageEnvelope.Recipient,
                messageEnvelope.Domain,
                messageEnvelope.StatusMessage);

            foreach (var option in options)
            {
                Log.Debug("OnSent Option: Key: {0}, Value: {1}", option.Key, option.Value);
            }

            ProcessRecipientStatusUpdate(messageEnvelope, EmailRecipientStatus.Delivered);
        }
        #endregion

        #region IBounceProcess Events
        public void OnBounce(IBounceMessageEnvelope message, ref Dictionary<string, object> options)
        {
            Log.Debug("OnBounce: SystemMessageId: {0}, AccountId: {1}, CustomMessageId: {2}, MailingId: {3}, BounceType: {4}, BounceCategory: {5}, BounceAddress: {6}, From: {7}, To: {8}",
                message.SystemMessageId,
                message.AccountId,
                message.CustomMessageId,
                message.MailingId,
                message.BounceType,
                message.BounceCategory,
                message.BounceAddress,
                message.From,
                message.To);

            foreach (var option in options)
            {
                Log.Debug("OnBounce Option: Key: {0}, Value: {1}", option.Key, option.Value);
            }

            // Build event text
            string text = string.Format("{0}, {1}", message.BounceCategory, message.BounceType);
            if(options.ContainsKey("BounceStatus"))
                text += ", " + options["BounceStatus"];
            if(options.ContainsKey("DiagnosticCode"))
                text += ", " + options["DiagnosticCode"];

            ProcessRecipientEvent(message.AccountId, message.CustomMessageId, message.BounceAddress, MailMessageRecipientEventType.MessageBounced, text, null);
        }
        #endregion

        #region IOpenClickTracking Events
        public void OnOpenClickTrackingHit(OpenClickTrackingHitType hitType, IOpenClickTrackingHitData hitData, ref Dictionary<string, object> options)
        {
            Log.Debug("OnOpenClickTrackingHit HitType: {0}, SystemMessageId: {1}, AccountId: {2}, CustomMessageId: {3}, MailingId: {4}, URL: {5}",
                hitType,
                hitData.SystemMessageId,
                hitData.AccountId,
                hitData.CustomMessageId,
                hitData.MailingId,
                hitData.URL);

            foreach (var option in options)
            {
                Log.Debug("OnOpenClickTrackingHit Option: Key: {0}, Value: {1}", option.Key, option.Value);
            }

            // Validate EmailAddress in options
            if (!options.ContainsKey("EmailAddress"))
            {
                return;
            }

            if (hitType == OpenClickTrackingHitType.Open)
            {
                ProcessRecipientEvent(hitData.AccountId, hitData.CustomMessageId, options["EmailAddress"] as string, MailMessageRecipientEventType.MessageOpened, null, null);
            }
            else if (hitType == OpenClickTrackingHitType.Click)
            {
                ProcessRecipientEvent(hitData.AccountId, hitData.CustomMessageId, options["EmailAddress"] as string, MailMessageRecipientEventType.LinkClicked, null, hitData.URL);
            }
        }
        #endregion

        #region Private
        private void ProcessRecipientStatusUpdate(IOutboundMessageEnvelope messageEnvelope, EmailRecipientStatus emailRecipientStatus)
        {
            // Validate AccountId is a client
            if (!_clientsByHurricaneAccountId.ContainsKey(messageEnvelope.AccountId.ToString()))
            {
                return;
            }

            var client = _clientsByHurricaneAccountId[messageEnvelope.AccountId.ToString()];

            // Validate recipient status updates are enabled
            if (!client.EnableRecipientStatusUpdates)
            {
                return;
            }

            // Validate CustomMessageId (MailMessageGroupID)
            int mailMessageGroupID;
            if (!int.TryParse(messageEnvelope.CustomMessageId, out mailMessageGroupID))
            {
                return;
            }

            // Validate Recipient
            if (string.IsNullOrEmpty(messageEnvelope.Recipient))
            {
                return;
            }

            Log.Debug("Updating Recipient Status: Client: {0}, MailMessageGroupID: {1}, RecipientEmailAddress: {2}, RecipientStatus: {3}",
                client.Name, mailMessageGroupID, messageEnvelope.Recipient, emailRecipientStatus);

            _db.UpdateMailMessageRecipientStatus(client, mailMessageGroupID, messageEnvelope.Recipient, emailRecipientStatus);
        }

        private void ProcessRecipientEvent(int accountId, string customMessageId, string recipientEmailAddress, MailMessageRecipientEventType mailMessageRecipientEventType, string text, string url)
        {
            // Validate AccountId is a client
            if (!_clientsByHurricaneAccountId.ContainsKey(accountId.ToString()))
            {
                return;
            }

            var client = _clientsByHurricaneAccountId[accountId.ToString()];

            // Validate tracking is enabled
            if (!client.EnableMessageTracking)
            {
                return;
            }

            // Validate CustomMessageId (MailMessageGroupID)
            int mailMessageGroupID;
            if (!int.TryParse(customMessageId, out mailMessageGroupID))
            {
                return;
            }

            // Validate RecipientEmailAddress
            if (string.IsNullOrEmpty(recipientEmailAddress))
            {
                return;
            }

            Log.Debug("Saving Recipient Event: Client: {0}, MailMessageGroupID: {1}, RecipientEmailAddress: {2}, Type: {3}",
                client.Name, mailMessageGroupID, recipientEmailAddress, mailMessageRecipientEventType);
            
            _db.SaveMailMessageRecipientEvent(client, mailMessageGroupID, recipientEmailAddress, mailMessageRecipientEventType, text, url);
        }
        #endregion

        #region Unused IInboundSMTPConnection Events
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
    }
}
