using System;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Utility;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Mail;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Repositories
{
    [ContainerRegister(typeof(IMailMessageGroupAddressRepository), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
    public partial class MailMessageGroupAddressRepository : BaseRepository<MailMessageGroupAddress, int, MailEntities>, IMailMessageGroupAddressRepository, IDefaultImplementation
    {
        #region Members
        #region Helper status IDs for clarity when building queries
        // These are the statuses for a "sent" MailMessageGroup.
        private static short[] _groupSentStatuses =
        {
            (short)NetSteps.Data.Entities.Mail.Constants.MessageGroupStatusType.SMTPSent,
            (short)NetSteps.Data.Entities.Mail.Constants.MessageGroupStatusType.InboundReceived
        };

        // These are the statuses for a "sent" recipient. We include "Unknown" as a sent status for recipients
        // just in case the mail server failed to update the recipient status to "Delivered". This is safe as long
        // as we know the MailMessageGroup parent is marked as "sent".
        private static short[] _recipientSentStatuses =
        {
            (short)NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus.Unknown,
            (short)NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus.Delivered
        };

        // These are the statuses for a "failed" recipient.
        private static short[] _recipientFailedStatuses =
        {
            (short)NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus.DeliveryError,
            (short)NetSteps.Data.Entities.Mail.Constants.EmailRecipientStatus.InvalidAddress
        };

        // This is the type for a "bounced" event.
        private static short _eventBounceType = (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientEventType.MessageBounced;

        // This is the type for an "opened" event.
        private static short _eventOpenType = (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientEventType.MessageOpened;

        // This is the type for a "clicked" event.
        private static short _eventClickType = (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientEventType.LinkClicked;

        // These are the types for "action" events (opens and clicks).
        private static short[] _eventActionTypes =
        {
            (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientEventType.MessageOpened,
            (short)NetSteps.Data.Entities.Mail.Constants.MailMessageRecipientEventType.LinkClicked
        };
        #endregion

        #region Helper expressions for clarity when building queries
        private Expression<Func<MailMessageGroupAddress, bool>> _recipientGroupSentCondition = r =>
            _groupSentStatuses.Contains(r.MailMessageGroup.MessageGroupStatusID.Value);

        private Expression<Func<MailMessageGroupStatusAudit, bool>> _groupAuditSentCondition = a =>
            _groupSentStatuses.Contains(a.MessageGroupStatusID.Value);

        private Expression<Func<MailMessageGroupAddress, bool>> _recipientSentCondition = r =>
            _recipientSentStatuses.Contains(r.RecipientStatusID.Value);

        private Expression<Func<MailMessageGroupAddress, bool>> _recipientFailedCondition = r =>
            _recipientFailedStatuses.Contains(r.RecipientStatusID.Value)
            || r.MailMessageRecipientEvents.Any(e => e.MailMessageRecipientEventTypeID == _eventBounceType);

        // This is the condition for an "opened" recipient. We include "clicked" events as a
        // safety check because if we got a click event we know they opened the message.
        private Expression<Func<MailMessageGroupAddress, bool>> _recipientOpenedCondition = r =>
            r.MailMessageRecipientEvents.Any(e =>
                _eventActionTypes.Contains(e.MailMessageRecipientEventTypeID));

        private Expression<Func<MailMessageGroupAddress, bool>> _recipientClickedCondition = r =>
            r.MailMessageRecipientEvents.Any(e =>
                e.MailMessageRecipientEventTypeID == _eventClickType);

        private Expression<Func<MailMessageRecipientEvent, bool>> _eventIsOpenCondition = e =>
            e.MailMessageRecipientEventTypeID == _eventOpenType;

        private Expression<Func<MailMessageRecipientEvent, bool>> _eventIsClickCondition = e =>
            e.MailMessageRecipientEventTypeID == _eventClickType;

        private Expression<Func<MailMessageRecipientEvent, bool>> _eventIsActionCondition = e =>
            _eventActionTypes.Contains(e.MailMessageRecipientEventTypeID);
        #endregion
        #endregion

        #region Methods
        public MailMessageGroupAddressSearchTotals GetSearchTotals(MailMessageGroupAddressSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (MailEntities context = new MailEntities())
                {
                    var totals = new MailMessageGroupAddressSearchTotals();

                    // "Totals" search type is implicit
                    searchParameters.SearchType = Constants.MailMessageGroupAddressSearchType.Totals;

                    var query = ApplyBaseSearchQuery(context.MailMessageGroupAddresses, searchParameters, context);

                    totals.SentCount = query.Count(_recipientSentCondition);
                    totals.FailedCount = query.Count(_recipientFailedCondition);
                    totals.OpenedCount = query.Count(_recipientOpenedCondition);
                    totals.ClickedCount = query.Count(_recipientClickedCondition);

                    return totals;
                }
            });
        }

        public PaginatedList<MailMessageGroupAddressSearchData> Search(MailMessageGroupAddressSearchParameters searchParameters)
        {
            return ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
            {
                using (MailEntities context = new MailEntities())
                {
                    var results = new PaginatedList<MailMessageGroupAddressSearchData>(searchParameters);

                    var baseQuery = ApplyBaseSearchQuery(context.MailMessageGroupAddresses, searchParameters, context);

                    var selectQuery = from x in baseQuery
                                      join sa in context.SitesAccounts on x.AccountID equals sa.AccountID into Accounts
                                      from a in Accounts.DefaultIfEmpty() // outer join
                                      select new MailMessageGroupAddressSearchData
                                      {
                                          MailMessageGroupAddressID = x.MailMessageGroupAddressID,
                                          MailMessageGroupID = x.MailMessageGroupID,
                                          MailMessageID = x.MailMessageGroup.MailMessageID,
                                          CampaignActionID = x.MailMessageGroup.MailMessage.CampaignActionID,
                                          SenderMailAccountID = x.MailMessageGroup.MailMessage.MailAccountID,
                                          AccountID = x.AccountID,
                                          EmailAddress = x.EmailAddress,
                                          FirstName = a.FirstName,
                                          LastName = a.LastName,
                                          DateSentUTC = x.MailMessageGroup.MailMessageGroupStatusAudits
                                                            .AsQueryable()
                                                            .Where(_groupAuditSentCondition)
                                                            .OrderBy(audit => audit.DateAddedUTC)
                                                            .FirstOrDefault()
                                                            .DateAddedUTC,
                                          TotalActions = x.MailMessageRecipientEvents
                                                             .AsQueryable()
                                                             .Count(_eventIsActionCondition),
                                          LastActionDateUTC = x.MailMessageRecipientEvents
                                                                  .AsQueryable()
                                                                  .Where(_eventIsActionCondition)
                                                                  .OrderByDescending(e => e.DateCreatedUTC)
                                                                  .FirstOrDefault()
                                                                  .DateCreatedUTC,
                                          LastClickUrl = x.MailMessageRecipientEvents
                                                             .AsQueryable()
                                                             .Where(_eventIsClickCondition)
                                                             .OrderByDescending(e => e.DateCreatedUTC)
                                                             .FirstOrDefault()
                                                             .Url
                                      };

                    // Sort
                    if (!string.IsNullOrWhiteSpace(searchParameters.OrderByString))
                        selectQuery = DynamicQueryable.OrderBy(selectQuery, searchParameters.OrderByString);
                    else
                        selectQuery = selectQuery.OrderBy(x => x.MailMessageGroupAddressID);

                    // Count
                    results.TotalCount = selectQuery.Count();

                    // Page
                    selectQuery = selectQuery.ApplyPagination(searchParameters);

#if DEBUG
                    var q = selectQuery.ToTraceString();
#endif

                    results.AddRange(selectQuery);

                    return results;
                }
            });
        }
        #endregion

        #region Private Methods
        private IQueryable<MailMessageGroupAddress> ApplyBaseSearchQuery(IQueryable<MailMessageGroupAddress> query, MailMessageGroupAddressSearchParameters searchParameters, MailEntities context)
        {
            switch (searchParameters.SearchType)
            {
                case Constants.MailMessageGroupAddressSearchType.Sent:
                    query = query.Where(_recipientGroupSentCondition).Where(_recipientSentCondition);
                    break;
                case Constants.MailMessageGroupAddressSearchType.Failed:
                    query = query.Where(_recipientGroupSentCondition).Where(_recipientFailedCondition);
                    break;
                case Constants.MailMessageGroupAddressSearchType.Opened:
                    query = query.Where(_recipientGroupSentCondition).Where(_recipientOpenedCondition);
                    break;
                case Constants.MailMessageGroupAddressSearchType.Clicked:
                    query = query.Where(_recipientGroupSentCondition).Where(_recipientClickedCondition);
                    break;
                case Constants.MailMessageGroupAddressSearchType.Totals:
                    query = query.Where(_recipientGroupSentCondition);
                    break;
                case Constants.MailMessageGroupAddressSearchType.NotSet:
                default:
                    // No filter
                    break;
            }

            if (searchParameters.MailMessageID.HasValue)
                query = query.Where(x => x.MailMessageGroup.MailMessageID == searchParameters.MailMessageID.Value);

            if (searchParameters.CampaignActionID.HasValue)
                query = query.Where(x => x.MailMessageGroup.MailMessage.CampaignActionID == searchParameters.CampaignActionID.Value);

            if (searchParameters.CampaignSubscriptionAddedByAccountID.HasValue)
            {
                query = from x in query
                        join cs in context.SitesCampaignSubscribers on x.AccountID equals cs.AccountID
                        where cs.AddedByAccountID == searchParameters.CampaignSubscriptionAddedByAccountID.Value
                        select x;
            }

            if (searchParameters.SenderMailAccountID.HasValue)
                query = query.Where(x => x.MailMessageGroup.MailMessage.MailAccountID == searchParameters.SenderMailAccountID.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.SenderEmailAddress))
                query = query.Where(x => x.MailMessageGroup.MailMessage.FromAddress.Contains(searchParameters.SenderEmailAddress));

            if (searchParameters.RecipientAccountID.HasValue)
                query = query.Where(x => x.AccountID == searchParameters.RecipientAccountID.Value);

            if (!string.IsNullOrWhiteSpace(searchParameters.RecipientEmailAddress))
                query = query.Where(x => x.EmailAddress.Contains(searchParameters.RecipientEmailAddress));

            // Start/End dates have different meanings depending on the search type.
            if (searchParameters.StartDate.HasValue)
            {
                var startDateUTC = searchParameters.StartDate.Value.Date.LocalToUTC();
                switch (searchParameters.SearchType)
                {
                    case Constants.MailMessageGroupAddressSearchType.Opened:
                        // Filter by "open" & "click" event dates.
                        query = query.Where(x =>
                            x.MailMessageRecipientEvents
                                .Any(e =>
                                    // Remember to count "clicks" as "opens" when determining recipient open status.
                                    _eventActionTypes.Contains(e.MailMessageRecipientEventTypeID)
                                    && e.DateCreatedUTC >= startDateUTC));
                        break;
                    
                    case Constants.MailMessageGroupAddressSearchType.Clicked:
                        // Filter by "click" event dates.
                        query = query.Where(x =>
                            x.MailMessageRecipientEvents
                                .Any(e =>
                                    e.MailMessageRecipientEventTypeID == _eventClickType
                                    && e.DateCreatedUTC >= startDateUTC));
                        break;

                    case Constants.MailMessageGroupAddressSearchType.Sent:
                    case Constants.MailMessageGroupAddressSearchType.Failed:
                    case Constants.MailMessageGroupAddressSearchType.Totals:
                        // Filter by sent date.
                        query = query.Where(x =>
                            x.MailMessageGroup.MailMessageGroupStatusAudits
                                .AsQueryable()
                                .Any(a =>
                                    a.MessageGroupStatusID != null
                                    && _groupSentStatuses.Contains(a.MessageGroupStatusID.Value)
                                    && a.DateAddedUTC >= startDateUTC));
                        break;

                    case Constants.MailMessageGroupAddressSearchType.NotSet:
                    default:
                        // Filter by email creation date.
                        query = query.Where(x => x.MailMessageGroup.MailMessage.DateAddedUTC >= startDateUTC);
                        break;
                }
            }
            if (searchParameters.EndDate.HasValue)
            {
                var endDateUTC = searchParameters.EndDate.Value.Date.LocalToUTC();
                switch (searchParameters.SearchType)
                {
                    case Constants.MailMessageGroupAddressSearchType.Opened:
                        // Filter by "open" & "click" event dates.
                        query = query.Where(x =>
                            x.MailMessageRecipientEvents
                                .Any(e =>
                                    // Remember to count "clicks" as "opens" when determining recipient open status.
                                    _eventActionTypes.Contains(e.MailMessageRecipientEventTypeID)
                                    && e.DateCreatedUTC <= endDateUTC));
                        break;

                    case Constants.MailMessageGroupAddressSearchType.Clicked:
                        // Filter by "click" event dates.
                        query = query.Where(x =>
                            x.MailMessageRecipientEvents
                                .Any(e =>
                                    e.MailMessageRecipientEventTypeID == _eventClickType
                                    && e.DateCreatedUTC <= endDateUTC));
                        break;

                    case Constants.MailMessageGroupAddressSearchType.Sent:
                    case Constants.MailMessageGroupAddressSearchType.Failed:
                    case Constants.MailMessageGroupAddressSearchType.Totals:
                        // Filter by sent date.
                        query = query.Where(x =>
                            x.MailMessageGroup.MailMessageGroupStatusAudits
                                .AsQueryable()
                                .Any(a =>
                                    a.MessageGroupStatusID != null
                                    && _groupSentStatuses.Contains(a.MessageGroupStatusID.Value)
                                    && a.DateAddedUTC <= endDateUTC));
                        break;

                    case Constants.MailMessageGroupAddressSearchType.NotSet:
                    default:
                        // Filter by email creation date.
                        query = query.Where(x => x.MailMessageGroup.MailMessage.DateAddedUTC <= endDateUTC);
                        break;
                }
            }

            if (searchParameters.WhereClause != null)
                query = query.Where(searchParameters.WhereClause);

            return query;
        }
        #endregion
    }
}
