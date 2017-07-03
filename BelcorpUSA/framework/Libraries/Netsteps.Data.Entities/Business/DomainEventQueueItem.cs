using System;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace NetSteps.Data.Entities
{
	public partial class DomainEventQueueItem
	{

		public static void AddEnrollmentCompletedEventToQueue(int accountID, int accountTypeID)
		{
			try
			{
				var domainEvent =
					SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.EnrollmentCompleted);
				if (domainEvent.Active && AccountType.CanSendEnrollmentCompletedEmail(accountTypeID))
				{
					var domainEventQueueItems =
						SetDomainEventQueueItemValues(Constants.DomainEventType.EnrollmentCompleted,
													  accountID: accountID);
					domainEventQueueItems.Save();
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void AddEnrollmentCompletedEventToQueue(int orderID, int accountTypeID, int accountID)
		{
			try
			{
				var domainEvent =
					SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.EnrollmentCompleted);
				if (domainEvent.Active && AccountType.CanSendEnrollmentCompletedEmail(accountTypeID))
				{
					var domainEventQueueItem =
						SetDomainEventQueueItemValues(Constants.DomainEventType.EnrollmentCompleted,
													  orderID: orderID,
													  accountID: accountID);
					domainEventQueueItem.Save();
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

        public static void EnlistarCorreoNotificacionSponsorNoValido(int AccountID)
        {
            try
            {
                var domainEvent =
                    SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.InvalidNotificationSponsor);
                if (domainEvent.Active )
                {
                    var domainEventQueueItem =
                        SetDomainEventQueueItemValues(Constants.DomainEventType.InvalidNotificationSponsor,
                        accountID: AccountID);
                    domainEventQueueItem.Save();
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
		/// <summary>
		/// This email is send to the Enroller, letting them know that a new distributor has enrolled.
        /// CVELEZ: Validates orderId HasValue
		/// </summary>
		public static void AddDistributorJoinsDownlineEventToQueue(int? orderID, int newAccountID)
		{
            int? flag = null;
            flag = orderID;
			try
			{
                if (flag.HasValue)
                {
                    var domainEvent =
                        SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.DistributorEnrolled);
                    if (domainEvent.Active)
                    {
                        var domainEventQueueItem =
                            SetDomainEventQueueItemValues(Constants.DomainEventType.DistributorEnrolled,
                                                          orderID: orderID,
                                                          accountID: newAccountID);
                        domainEventQueueItem.Save();
                    }
                }
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void AddProspectCreatedEventToQueue(int prospectID)
		{
			try
			{
				var domainEvent =
				   SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.ProspectCreated);
				if (domainEvent.Active)
				{
					var domainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.ProspectCreated,
																			 accountID: prospectID);
					domainEventQueueItem.Save();
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void AddSupportTicketInfoRequestedCompletedEventToQueue(int supportTicketID)
		{
			try
			{
				var domainEvent =
				   SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.SupportTicketInfoRequest);
				if (domainEvent.Active)
				{
					var domainEventQueueItem =
						SetDomainEventQueueItemValues(Constants.DomainEventType.SupportTicketInfoRequest,
													  supportTicketID: supportTicketID);
					domainEventQueueItem.Save();
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static void AddOrderCompletedEventToQueue(int orderID)
		{
			var domainEvent =
				  SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.OrderCompleted);
			if (domainEvent.Active)
			{
				var domainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.OrderCompleted,
																		 orderID: orderID);
				domainEventQueueItem.Save();
			}
		}

		public static void AddAutoshipCreditCardFailedEventToQueue(int orderID)
		{
			// 2011-11-26 Seems to be sending to the wrong person. - Lundy
			// var domainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.AutoshipCreditCardFailed, orderID: orderID);
			//domainEventQueueItem.Save();
		}


		public static void AddOrderShippedEventToQueue(int orderID)
		{
			var domainEvent =
				   SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.OrderShipped);
			if (domainEvent.Active)
			{
				var domainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.OrderShipped,
																		 orderID: orderID);
				domainEventQueueItem.Save();
			}
		}

		public static void AddAutoshipReminderEventToQueue(int orderID)
		{
			var domainEvent =
					SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.AutoshipReminder);
			if (domainEvent.Active)
			{
				var domainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.AutoshipReminder,
																		 orderID: orderID);
				domainEventQueueItem.Save();
			}
		}

		/// <summary>
		/// Add this Party Info to EventContext and DomanEventQueue table
		/// </summary>
		/// <param name="partyId">party Id</param>
		public static void AddPartyGuestOrderReminderEventToQueue(int partyId)
		{
			var domainEvent =
					SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.PartyGuestReminder);
			if (domainEvent.Active)
			{
				var domainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.PartyGuestReminder, null, null, null, null, partyId);

				domainEventQueueItem.Save();
			}
		}


		/// <summary>
		/// Sends emails to the person who canceled the autoship, as well as to the account sponsor if BCC is true.
		/// Both of the DomainEventTypes has to be active in order for the emails to be send.
		/// By Default: they are both off
		/// </summary>
		/// <param name="orderID">AutoshipOrder.OrderID</param>
		/// <param name="bccToSponsor">Determines whether to send BCC to the account sponsor.</param>
		public static void AddAutoshipCanceledEventToQueue(int orderID, bool bccToSponsor = false)
		{
			var domainEvent =
				SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.AutoshipCanceled);

			if (domainEvent.Active)
			{
				var domainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.AutoshipCanceled,
																		 orderID: orderID);
				domainEventQueueItem.Save();

				SendAutoshipCanceledBccToSponsorIfActive(orderID, bccToSponsor);
			}
		}

		public static void SendAutoshipCanceledBccToSponsorIfActive(int orderID, bool bccToSponsor)
		{
			if (bccToSponsor)
			{
				var bccDomainEvent =
					SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.AutoshipCanceledBcc);

				if (bccDomainEvent.Active)
				{
					var bccDomainEventQueueItem = SetDomainEventQueueItemValues(Constants.DomainEventType.AutoshipCanceledBcc,
																			orderID: orderID);
					bccDomainEventQueueItem.Save();
				}
			}
		}

		public static void AddBreakingNewsEventToQueue(int newsID)
		{
			try
			{
				var domainEvent =
				   SmallCollectionCache.Instance.DomainEventTypes.GetById((int)Constants.DomainEventType.BreakingNews);
				if (domainEvent.Active)
				{
					var domainEventQueueItems = SetDomainEventQueueItemValues(Constants.DomainEventType.BreakingNews,
																			  newsID: newsID);
					domainEventQueueItems.Save();
				}
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


		/// <summary>
		/// NOTE: Do not modify the code to SAVE the domainEventQueueItem in this method. Makes it hard to UNIT TEST!
		/// </summary>
		public static DomainEventQueueItem SetDomainEventQueueItemValues(Constants.DomainEventType eventType,
			int? orderID = null,
			int? accountID = null,
			int? newsID = null,
			int? supportTicketID = null,
			int? partyID = null,
			int? promotionID = null)
		{
			try
			{
				return SetDomainEventQueueItemValues((short)eventType, orderID, accountID, newsID, supportTicketID, partyID, promotionID);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// NOTE: Do not modify the code to SAVE the domainEventQueueItem in this method. Makes it hard to UNIT TEST!
		/// </summary>
		public static DomainEventQueueItem SetDomainEventQueueItemValues(short eventTypeID,
			int? orderID = null,
			int? accountID = null,
			int? newsID = null,
			int? supportTicketID = null,
			int? partyID = null,
			int? promotionID = null)
		{
			try
			{
				DomainEventQueueItem domainEventQueueItem = new DomainEventQueueItem();

				if (domainEventQueueItem.EventContext == null)
					domainEventQueueItem.EventContext = new EventContext();

				if (accountID.HasValue)
					domainEventQueueItem.EventContext.AccountID = accountID;

				if (orderID.HasValue)
					domainEventQueueItem.EventContext.OrderID = orderID;

				if (newsID.HasValue)
					domainEventQueueItem.EventContext.NewsID = newsID;

				if (supportTicketID.HasValue)
					domainEventQueueItem.EventContext.SupportTicketID = supportTicketID;

				if (partyID.HasValue)
					domainEventQueueItem.EventContext.PartyID = partyID;

				if (promotionID.HasValue)
					domainEventQueueItem.EventContext.PromotionID = promotionID;

				domainEventQueueItem.DomainEventTypeID = eventTypeID;
				domainEventQueueItem.QueueItemPriorityID = Constants.QueueItemPriority.Normal.ToShort();
				domainEventQueueItem.QueueItemStatusID = Constants.QueueItemStatus.Queued.ToShort();

				return domainEventQueueItem;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public static PaginatedList<int> QueueDomainEventItemIDs(int maxNumberToPoll)
		{
			try
			{
				PaginatedList<int> list = null;
				list = Repository.QueueDomainEventItemIDs(maxNumberToPoll);









				return list;
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}


	}
}
