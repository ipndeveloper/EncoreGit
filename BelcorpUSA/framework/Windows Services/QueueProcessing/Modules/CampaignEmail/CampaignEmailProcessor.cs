using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.CampaignEmail
{
    public class CampaignEmailQueueItem
    {
        //public DataRow row;
        //public EmailTemplateAttrCollection templateAttrs;
        //public Hashtable campaignEmailIds;
        //public List<EmailTemplate> templateList_ForThreading;
    }

    public class CampaignEmailProcessor : QueueProcessor<CampaignEmailQueueItem>
    {
        public static readonly string CProcessorName = "CampaignEmailProcessor";

        public CampaignEmailProcessor()
        {
            Name = CProcessorName;
        }

        public override void CreateQueueItems(int maxNumberToEnqueue)
        {
            /*
                        // Please note that each time this runs (every 60 seconds) it will create a new set of
                        // variables that will be queued and sent to each of the worker threads in the CampaignEmailQueueItem
                        // thus all the threads will be sharing the same templateAttrs, campaignEmailIds, templateList_ForThreading.
                        // Just to be clear, all items returned in this poll will use the same instances of the variables, however
                        // a new set will be created with the next poll.
                        // 
                        // This is probably not ideal, but it preserves how it worked in AutoEmailer when it used CampaignEmail_Legacy
                        // If we loaded templateAttrs up front and shared between polls then we'd have a caching issue when items were
                        // added or changed. Thus each new poll gets the latest templates.
 

                        Log.Info("CampaignEmailProcessor - CreateQueueItems");

                        EmailTemplateAttrCollection templateAttrs = null;
                        Hashtable campaignEmailIds = new Hashtable();
                        List<EmailTemplate> templateList_ForThreading = new List<EmailTemplate>();

                        int itemCount = 0;

                        // the readpast hint skips locked emails. These would be emails in the middle of being queued
                        // still wrapped in a transaction. This prevents us from emailing an item before all it's parts (such as
                        // addresses and attachments) have been added
                        string sql = "Exec usp_campaign_select_emails_to_send"; // "Select MailQueueID From MailQueue(READPAST) Where ErrorSending = 0";
                        DataSet dataset = NetSteps.Common.DataAccess.GetDataSet(sql);

                        // this loads all the templateattrs
                        if (dataset.Tables[0].Rows.Count > 0)
                            templateAttrs = EmailTemplateAttrCollection.GetAll();

                        foreach (DataRow row in dataset.Tables[0].Rows)
                        {
                            CampaignEmailQueueItem item = new CampaignEmailQueueItem();

                            item.row = row;
                            item.templateAttrs = templateAttrs;
                            item.campaignEmailIds = campaignEmailIds;
                            item.templateList_ForThreading = templateList_ForThreading;

                            EnqueueItem(item);

                            itemCount++;
                        }

                        Log.Info("CampaignEmailProcessor - Enqueued {0} Items", itemCount);
             */
        }

        public override void ProcessQueueItem(CampaignEmailQueueItem item)
        {
            /*
                        // Store different campaignemailids to increment sentcount
                        int campaignEmailId = 0;
                        int emailTemplateID = 0;
                        EmailTemplate template = null;
                        DateTime datesubscribed;
                        int distributionSubscriberId = 0;

                        int.TryParse(item.row["CampaignEmailID"].ToString(), out campaignEmailId);
                        int.TryParse(item.row["EmailTemplateID"].ToString(), out emailTemplateID);
                        int.TryParse(item.row["DistributionSubscriberId"].ToString(), out distributionSubscriberId);

                        // note that this is locked for all threads while it loads the EmailTemplate. This could slow down 
                        // the processing if multiple threads are constantly loading templates instead of reusing them, meaning if
                        // each thread winds up loading a template, all other threads will have to wait...possibly creating a bottleneck.
                        lock (item.templateList_ForThreading)
                        {
                            template = item.templateList_ForThreading.Find(obj => obj.Id == emailTemplateID);
                            if (template == null)
                            {
                                template = EmailTemplate.Load(emailTemplateID);
                                item.templateList_ForThreading.Add(template);
                            }
                        }

                        string subject = template.Subject;
                        string body = template.Body;
                        string fromAddress = template.FromAddress;

                        Dictionary<string, string> staticAttrs = new Dictionary<string, string>();
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.firstname.ToString(), item.row[OB.Constants.CampaignStaticAttributes.firstname.ToString()].ToString());
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.lastname.ToString(), item.row[OB.Constants.CampaignStaticAttributes.lastname.ToString()].ToString());
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.accountnumber.ToString(), item.row[OB.Constants.CampaignStaticAttributes.accountnumber.ToString()].ToString());
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.accountid.ToString(), item.row[OB.Constants.CampaignStaticAttributes.accountid.ToString()].ToString());
                        DateTime.TryParse(item.row[OB.Constants.CampaignStaticAttributes.datesubscribed.ToString()].ToString(), out datesubscribed);
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.datesubscribed.ToString(), datesubscribed.ToShortDateString());
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.distributionsubscriberid.ToString(), item.row[OB.Constants.CampaignStaticAttributes.distributionsubscriberid.ToString()].ToString());
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.campaignemailid.ToString(), item.row[OB.Constants.CampaignStaticAttributes.campaignemailid.ToString()].ToString());
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.campaignid.ToString(), item.row[OB.Constants.CampaignStaticAttributes.campaignid.ToString()].ToString());
                        staticAttrs.Add(OB.Constants.CampaignStaticAttributes.emailaddress.ToString(), item.row[OB.Constants.CampaignStaticAttributes.emailaddress.ToString()].ToString());


                        // replace the static substitute values first in both the subject and body
                        body = CampaignTextHelper.ReplaceStaticAttrsInString(body, staticAttrs);
                        subject = CampaignTextHelper.ReplaceStaticAttrsInString(subject, staticAttrs);
                        fromAddress = CampaignTextHelper.ReplaceStaticAttrsInString(fromAddress, staticAttrs);

                        // look for the email Parameter type template attr in the body and subject and make replacements                
                        body = CampaignTextHelper.ReplaceParameterTemplateAttr(distributionSubscriberId, item.templateAttrs, body);
                        subject = CampaignTextHelper.ReplaceParameterTemplateAttr(distributionSubscriberId, item.templateAttrs, subject);
                        fromAddress = CampaignTextHelper.ReplaceParameterTemplateAttr(distributionSubscriberId, item.templateAttrs, fromAddress);

                        // look for the email SQL type template attr in the body and subject and make replacements
                        body = CampaignTextHelper.ReplaceSQLTemplateAttr(staticAttrs, item.templateAttrs, body);
                        subject = CampaignTextHelper.ReplaceSQLTemplateAttr(staticAttrs, item.templateAttrs, subject);
                        fromAddress = CampaignTextHelper.ReplaceSQLTemplateAttr(staticAttrs, item.templateAttrs, fromAddress);

                        // look for the email C# type template attr in the body and subject and make replacements
                        body = CampaignTextHelper.ReplaceCSharpTemplateAttr(staticAttrs, item.templateAttrs, body);
                        subject = CampaignTextHelper.ReplaceCSharpTemplateAttr(staticAttrs, item.templateAttrs, subject);
                        fromAddress = CampaignTextHelper.ReplaceCSharpTemplateAttr(staticAttrs, item.templateAttrs, fromAddress);

                        // look for the "src=" element in the body and subject and fill the absolute path
                        body = CampaignTextHelper.FillAbsolutePathInSrc(body);
                        subject = CampaignTextHelper.FillAbsolutePathInSrc(subject);

                        // Put the info the queue object, but don't queue in DB, just send now
                        MailQueue_Legacy mail = new MailQueue_Legacy();
                        mail.FromAddress = template.FromAddress;
                        mail.Subject = subject;

                        //This will check to see if any HtmlConent exist for Section = 'CampaignOptOut' and add it to the body of the email if it does exits
                        int SiteId = CustomConfigurationHandler.Config.IDs.NSCoreSiteID;
                        if (SiteId > 0)
                        {
                            HtmlContent HtmlContent = Objects.Business.HtmlContent.FindBySectionNameAndSiteId("CampaignOptOut", SiteId);
                            if (HtmlContent != null && !string.IsNullOrEmpty(HtmlContent.Html))
                            {
                                if (HtmlContent.Html.Contains("<!--basepath-->"))
                                {
                                    HtmlContent.Html = HtmlContent.Html.Replace("<!--basepath-->", CustomConfigurationHandler.Config.Urls.WebsiteURL);
                                }
                                body += "<br>" + HtmlContent.Html;
                            }
                        }


                        mail.Body = body;
                        mail.AddAddress(item.row["EmailAddress"].ToString(), "", EmailAddressType_Legacy.TO);

                        //attachment code
                        try
                        {
                            if (!string.IsNullOrEmpty(template.AttachmentPath))
                            {
                                AttachmentCollection attachments = new AttachmentCollection(template.AttachmentPath);
                                foreach (Attachment attachment in attachments)
                                {
                                    string path = attachment.Path;
                                    if (File.Exists(path))
                                    {
                                        string aname = Path.GetFileName(path);
                                        byte[] adata = File.ReadAllBytes(path);
                                        mail.AddAttachment(aname, adata);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }

                        bool hasSendingError = false;
                        try
                        {
                            mail.ProcessEmail(OB.Constants.SMTPServer, OB.Constants.UseSMTPAuthentication, OB.Constants.SMTPUserName, OB.Constants.SMTPPassword);
                        }
                        catch (Exception e)
                        {
                            hasSendingError = true;

                            StringBuilder error = new StringBuilder();
                            error.AppendFormat("Error sending email:\r\n");
                            error.AppendFormat("    SMTPServer                  : {0}\r\n", OB.Constants.SMTPServer);
                            error.AppendFormat("    MailQueueID                 : {0}\r\n", mail.MailQueueID);
                            error.AppendFormat("    Subject                     : {0}\r\n", mail.Subject);
                            error.AppendFormat("    IsHtml                      : {0}\r\n", mail.IsHTML);
                            error.AppendFormat("    FromAddress                 : {0}\r\n", mail.FromAddress);
                            error.AppendFormat("    MailType                    : {0}\r\n", mail.MailType);
                            error.AppendFormat("    CampaignEmailLogID: {0}\r\n", mail.CampaignEmailLogID);

                            foreach (MailQueueAddress_Legacy address in mail.Addresses)
                                error.AppendFormat("    {0,-3} Address             : {1}\r\n", address.AddressType.ToString(), address.EmailAddress);

                            error.AppendFormat("    Error                       : {0}\r\n", e.Message);
                            Log.Error(error.ToString());
                        }

                        SqlCommand sqlCommand = DataAccess.SetCommand("CampaignEmailRecordQueue");
                        DataAccess.AddInputParameter("DistributionSubscriberID", distributionSubscriberId, sqlCommand);
                        DataAccess.AddInputParameter("CampaignEmailID", campaignEmailId, sqlCommand);
                        DataAccess.AddInputParameter("Failed", hasSendingError, sqlCommand);
                        DataAccess.ExecuteNonQuery(sqlCommand);


                        bool foundCampaignEmailId = false;
                        lock (item.campaignEmailIds)
                        {
                            foundCampaignEmailId = item.campaignEmailIds.ContainsKey(campaignEmailId);
                            if (!foundCampaignEmailId)
                                item.campaignEmailIds.Add(campaignEmailId, campaignEmailId);
                        }

                        if (!foundCampaignEmailId)
                        {
                            string sql = "exec CampaignEmail_Increment_SentCount " + campaignEmailId;
                            DataAccess.ExecuteNonQuery(sql);
                        }
             */
        }
    }
}
