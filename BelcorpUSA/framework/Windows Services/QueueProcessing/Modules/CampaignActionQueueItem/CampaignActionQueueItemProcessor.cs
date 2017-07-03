using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.TokenReplacement;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.TokenValueProviders;

namespace NetSteps.QueueProcessing.Modules.CampaignActionQueueItem
{
    using NetSteps.QueueProcessing.Modules.ModuleBase;

    public class CampaignActionQueueItemProcessor : QueueProcessor<NetSteps.Data.Entities.CampaignActionQueueItem>
    {
        class CampaignActionTokenValueResolver : DemuxCacheItemResolver<int, List<CampaignActionTokenValue>>
        {
            protected override bool DemultiplexedTryResolve(int campaignActionID, out List<CampaignActionTokenValue> value)
            {
                value = CampaignActionTokenValue.LoadAllByCampaignActionID(campaignActionID);
                return value != null;
            }
        }
        ICache<int, List<CampaignActionTokenValue>> _actionCache = new ActiveMruLocalMemoryCache<int, List<CampaignActionTokenValue>>("qp-CampaignActionTokenValues",
             new MruCacheOptions { CacheItemLifespan = TimeSpan.FromMinutes(10) }, new CampaignActionTokenValueResolver());

        private readonly MailAccount _accountToSendFrom;

        public static readonly string CProcessorName = "CampaignActionQueueItemProcessor";

        public CampaignActionQueueItemProcessor()
        {
            Name = CProcessorName;

            _accountToSendFrom = MailAccount.GetCorporateMailAccount();
        }

        public override void CreateQueueItems(int maxNumberToPoll)
        {
            //Get Newsletters that are past due and add them to. 
            try
            {
                Logger.Info("CampaignActionQueueItemProcessor - CreateQueueItems");

                var campaignActionQueueItems = NetSteps.Data.Entities.CampaignActionQueueItem.QueueCampaignActionItems(maxNumberToPoll);

                int itemCount = 0;
                foreach (var campaignActionQueueItem in campaignActionQueueItems)
                {
                    EnqueueItem(campaignActionQueueItem);
                    itemCount++;
                }

                Logger.Info("CampaignActionQueueItemProcessor - Enqueued {0} Items", itemCount);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public override void ProcessQueueItem(NetSteps.Data.Entities.CampaignActionQueueItem campaignActionQueueItem)
        {
            try
            {
                //TODO:  Refactor SOK
                NetSteps.Data.Entities.CampaignAction campaignAction = NetSteps.Data.Entities.CampaignAction.LoadFull(campaignActionQueueItem.CampaignActionID);
                Campaign campaign = campaignAction != null ? Campaign.LoadFull(campaignAction.CampaignID) : null;

                if (campaign != null)
                {
                    ProcessCampaignAction(campaignActionQueueItem, campaignAction);
                }
                campaignActionQueueItem.QueueItemStatusID = (short)Constants.QueueItemStatus.Completed;
            }
            catch (Exception ex)
            {
                campaignActionQueueItem.QueueItemStatusID = (short)Constants.QueueItemStatus.Failed;
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            finally
            {
                campaignActionQueueItem.Save();
            }
        }

        private void ProcessCampaignAction(NetSteps.Data.Entities.CampaignActionQueueItem domainEventQueueItem, NetSteps.Data.Entities.CampaignAction action)
        {
            switch ((Constants.CampaignType)action.Campaign.CampaignTypeID)
            {
                case Constants.CampaignType.NotSet:
                    break;
                case Constants.CampaignType.MassEmails:
                    break;
                case Constants.CampaignType.Campaigns:
                    break;
                case Constants.CampaignType.EventBasedEmails:
                    break;
                case Constants.CampaignType.MassAlerts:
                    break;
                case Constants.CampaignType.Newsletters:
                    ProcessNewsletterCampaignActions(domainEventQueueItem, action);
                    break;
                default:
                    break;
            }
        }

        private void ProcessNewsletterCampaignActions(NetSteps.Data.Entities.CampaignActionQueueItem campaignActionQueueItem, NetSteps.Data.Entities.CampaignAction action)
        {
            switch ((Constants.CampaignActionType)action.CampaignActionTypeID)
            {
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignActionType.NotSet:
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignActionType.Email:
                    ProcessEmailCampaignAction(campaignActionQueueItem, action);
                    break;
                case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignActionType.Alert:
                    //CampaignHelpers.ProcessAlertAction(domainEventQueueItem, action);
                    break;
                default:
                    break;
            }
        }

        private void ProcessEmailCampaignAction(NetSteps.Data.Entities.CampaignActionQueueItem campaignActionQueueItem, NetSteps.Data.Entities.CampaignAction action)
        {
            var emailCampaignAction = action.EmailCampaignActions.FirstOrDefault();

            if (emailCampaignAction != null)
            {
                var campaignSubscriber = action.Campaign.CampaignSubscribers.Where(a => a.AccountID == campaignActionQueueItem.EventContext.AccountID).FirstOrDefault();
                var newsletterRecipient = Account.Load(EventContext.Load(campaignActionQueueItem.EventContextID).AccountID.ToInt());
                var subscribedByAccount = Account.Load(campaignSubscriber.AddedByAccountID);

                var emailTemplate = EmailTemplate.LoadFull(emailCampaignAction.EmailTemplateID);
                var emailTemplateTranslation = emailTemplate.EmailTemplateTranslations.GetByLanguageID(newsletterRecipient.DefaultLanguageID)
                        ?? emailTemplate.EmailTemplateTranslations.GetByLanguageID((int)Constants.Language.English);
                if (emailTemplateTranslation != null)
                {
                    var campaignActionValues = GetAllTokenValuesByCampaignActionID(action.CampaignActionID);

                    // Distributor-specified content and images
                    var distributorContentTokenValueProvider = CampaignActionTokenValue.CreateCompositeDistributorTokenValueProvider(campaignActionValues, campaignActionQueueItem.CampaignActionID, newsletterRecipient.DefaultLanguageID, campaignSubscriber.AddedByAccountID);
                    // Values that come from the subscriber or the person who signed them up, like SubscriberFirstName, etc.
                    var campaignSubscriberTokenValueProvider = new CampaignSubscriberTokenValueProvider(newsletterRecipient, subscribedByAccount);

                    var compositeTokenValueProvider = new CompositeTokenValueProvider(new ITokenValueProvider[] { distributorContentTokenValueProvider, campaignSubscriberTokenValueProvider });

                    var mailMessage = emailTemplateTranslation.GetTokenReplacedMailMessage(compositeTokenValueProvider);

                    // 6-30-2011: Code to insert Opt-Out link into the email. Currently for English only
                    if (newsletterRecipient.DefaultLanguageID == Constants.Language.English.ToShort())
                        mailMessage.AppendOptOutFooter(newsletterRecipient.EmailAddress, newsletterRecipient.DefaultLanguageID);


                    mailMessage.To.Add(new NetSteps.Data.Entities.Mail.MailMessageRecipient(newsletterRecipient.FullName, newsletterRecipient.EmailAddress, campaignSubscriber.AccountID));
                    mailMessage.CampaignActionID = campaignActionQueueItem.CampaignActionID;
                    mailMessage.EnableEventTracking = true;
                    mailMessage.Send(_accountToSendFrom, GetSiteIDToSendFrom());

                    campaignActionQueueItem.QueueItemStatusID = (int)Constants.QueueItemStatus.Completed;
                }
                else
                {
                    // Couldn't find a translation for the subscriber's language or an English one, don't bother trying again
                    campaignActionQueueItem.QueueItemStatusID = (int)Constants.QueueItemStatus.Failed;
                }

                campaignActionQueueItem.Save();
            }
        }

        private int GetSiteIDToSendFrom()
        {
            return 420;
        }

        private List<CampaignActionTokenValue> GetAllTokenValuesByCampaignActionID(int campaignActionID)
        {
            return _actionCache.Get(campaignActionID);
        }

    }
}
