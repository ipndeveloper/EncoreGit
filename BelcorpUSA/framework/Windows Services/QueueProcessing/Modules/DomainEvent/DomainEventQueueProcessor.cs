using System;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Core.Cache;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;
using NetSteps.QueueProcessing.Modules.DomainEvent.Class;
using NetSteps.QueueProcessing.Modules.DomainEvent.DomainEventTaskHandlers;
using NetSteps.QueueProcessing.Modules.DomainEvent.Interfaces;
using NetSteps.QueueProcessing.Modules.ModuleBase;

namespace NetSteps.QueueProcessing.Modules.DomainEvent
{



	public class DomainEventQueueProcessor : CampaignActionBaseProcessor<int>, IDomainEventQueueProcessor, IDefaultImplementation
	{
		private readonly DomainEventCategoryTaskHandlerFactory _domainEventCategoryTaskHandlerFactory = new DomainEventCategoryTaskHandlerFactory();
		private readonly DomainEventTypeTaskHandlerFactory _domainEventTypeTaskHandlerFactory = new DomainEventTypeTaskHandlerFactory();

		public static readonly string CProcessorName = "DomainEventQueueProcessor";

		public DomainEventQueueProcessor()
		{
			Name = CProcessorName;

			RegisterBaseDomainEventCategoryTaskHandlers();
			RegisterBaseDomainEventTaskHandler();
		}

		class DomainEventCampaignResolver : DemuxCacheItemResolver<short, Campaign>
		{
			protected override bool DemultiplexedTryResolve(short domainEventTypeID, out Campaign value)
			{
				value = Campaign.LoadFullAllByDomainEventTypeID(domainEventTypeID)
					.FirstOrDefault();
				return value != null;
			}
		}
		ICache<short, Campaign> _domainEventCampaigns = new ActiveMruLocalMemoryCache<short, Campaign>("qp-domainEventCampaigns", new DomainEventCampaignResolver());


		public override void CreateQueueItems(int maxNumberToEnqueue)
		{
			try
			{
				Logger.Info("DomainEventQueueProcessor - CreateQueueItems");

				var domainEventQueueItemIDs = DomainEventQueueItem.QueueDomainEventItemIDs(maxNumberToEnqueue);

				int itemCount = 0;
				foreach (var domainEventQueueItemID in domainEventQueueItemIDs)
				{
					this.Logger.Debug("Enqueueing item {0}", domainEventQueueItemID);
					EnqueueItem(domainEventQueueItemID);
					itemCount++;
				}

				Logger.Info("DomainEventQueueProcessor - Enqueued {0} Items", itemCount);
			}
			catch (Exception ex)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public override void ProcessQueueItem(int domainEventQueueItemID)
		{
			DomainEventQueueItem domainEventQueueItem = null;

			try
			{
				this.Logger.Info("loading domainEventQueueItem {0}", domainEventQueueItemID);
				domainEventQueueItem = DomainEventQueueItem.LoadFull(domainEventQueueItemID);

				this.Logger.Debug("testing domainEventQueueItem {0} for null", domainEventQueueItemID);
				if (domainEventQueueItem == null)
				{
					throw new Exception(string.Format("failed to load domainEventQueueItem {0}", domainEventQueueItemID));
				}
				else
				{
					this.Logger.Debug("domainEventQueueItem was not null");
				}

				this.Logger.Debug("testing domainEventQueueItem {0} for status", domainEventQueueItemID);
				if (domainEventQueueItem.QueueItemStatusID != Constants.QueueItemStatus.Running.ToShort())
				{
					this.Logger.Info("domainEventQueueItem {0} had status of {1}, not processing"
						, domainEventQueueItem.DomainEventQueueItemID, domainEventQueueItem.QueueItemStatusID);
					return;
				}
				else
				{
					this.Logger.Debug("domainEventQueueItem is in running state");
				}

				this.Logger.Debug("successfully loaded domainEventQueueItem {0}", domainEventQueueItemID);

				this.Logger.Info("Processing domainEventQueueItem {0} : attempt {1} : change tracking {2}"
					, domainEventQueueItem.DomainEventQueueItemID
					, domainEventQueueItem.AttemptCount
					, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
					);
				var domainEventTaskHandler = GetDomainEventTypeCategoryTaskHandler(domainEventQueueItem);

				if (domainEventTaskHandler != null)
				{
					this.Logger.Debug("running domainEventTaskHandler : change tracking {0}"
						, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
						);

					var runSucceeded = domainEventTaskHandler.Run(domainEventQueueItem);

					this.Logger.Debug("finished running domainEventTaskHandler : change tracking {0}"
						, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
						);

					domainEventQueueItem.StartEntityTracking();
					domainEventQueueItem.QueueItemStatusID = (runSucceeded) ? (short)Constants.QueueItemStatus.Completed
						: (short)Constants.QueueItemStatus.Failed;
				}
				else
				{
					string errorMessage = string.Format("DomainEventQueueProcessor.ProcessQueueItem has an invalid or unhandled DomainEventTypeCategory for DomainEventQueueItemID {0}.", domainEventQueueItem.DomainEventQueueItemID);
					this.Logger.Error(errorMessage);
					throw new Exception(errorMessage);
				}
			}
			catch (Exception ex)
			{
				this.Logger.Error("Error processing domainEventQueueItem {0}: {1}", domainEventQueueItemID, ex.ToString());

				if (domainEventQueueItem != null)
				{
					domainEventQueueItem.QueueItemStatusID = (short)Constants.QueueItemStatus.Failed;
				}
				throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
			finally
			{
				try
				{
					if (domainEventQueueItem != null)
					{
						this.Logger.Info("Saving domainEventQueueItem {0} with QueueItemStatusID {1}, change tracking is {2}"
							, domainEventQueueItem.DomainEventQueueItemID
							, domainEventQueueItem.QueueItemStatusID
							, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
							);
						domainEventQueueItem.Save();
					}
					else
					{
						this.Logger.Error("domainEventQueueItem {0} was null in finally", domainEventQueueItemID);
					}
					this.Logger.Info("Finished processing domainEventQueueItem {0}", domainEventQueueItemID);
				}
				catch (Exception excp)
				{
					this.Logger.Error("Error while saving domainEventQueueItem {0} : {1}", domainEventQueueItemID, excp.ToString());
					throw EntityExceptionHelper.GetAndLogNetStepsException(excp, Constants.NetStepsExceptionType.NetStepsBusinessException);
				}
			}
		}

		protected virtual IDomainEventTaskHandler GetDomainEventTaskHandlerForDomainEventQueueItem(DomainEventQueueItem domainEventQueueItem)
		{
			return _domainEventTypeTaskHandlerFactory.Get(domainEventQueueItem.DomainEventType.TermName)();
		}

		private void RegisterBaseDomainEventCategoryTaskHandlers()
		{
			_domainEventCategoryTaskHandlerFactory.Register(Constants.DomainEventTypeCategory.Campaign,
				(domainEventQueueItem) => new CampaignDomainEventTaskHandler(new CampaignDomainEventTaskHandlerHelper(this)));

			_domainEventCategoryTaskHandlerFactory.Register(Constants.DomainEventTypeCategory.DomainEventTask,
				(domainEventQueueItem) => GetDomainEventTaskHandlerForDomainEventQueueItem(domainEventQueueItem));
		}

		private void RegisterBaseDomainEventTaskHandler()
		{
			_domainEventTypeTaskHandlerFactory.Register(Constants.DomainEventType.BreakingNews,
				() => new BreakingNewsDomainEventTaskHandler()
			);
		}

		private DomainEventTypeCategory GetDomainEventTypeCategory(DomainEventQueueItem domainEventQueueItem)
		{
			this.Logger.Debug("trying to get GetDomainEventTypeCategory for domainEventQueueIten {0}", domainEventQueueItem);
			if (domainEventQueueItem.DomainEventType == null || domainEventQueueItem.DomainEventType.DomainEventTypeCategory == null)
			{
				if (domainEventQueueItem.DomainEventType == null)
				{
					throw new Exception("domainEventQueueItem.DomainEventType was null");
				}
				else
				{
					throw new Exception("domainEventQueueItem.DomainEventType.DomainEventTypeCategory was null");
				}
			}

			this.Logger.Debug("got DomainEventTypeCategory {0} for domainEventQueueItem {1}"
				, domainEventQueueItem.DomainEventType.Name
				, domainEventQueueItem.DomainEventQueueItemID);
			return domainEventQueueItem.DomainEventType.DomainEventTypeCategory;
		}

		private IDomainEventTaskHandler GetDomainEventTypeCategoryTaskHandler(DomainEventQueueItem domainEventQueueItem)
		{
			this.Logger.Debug("trying to GetDomainEventTypeCategoryTaskHandler for {0}", domainEventQueueItem.DomainEventQueueItemID);
			return _domainEventCategoryTaskHandlerFactory.Get(GetDomainEventTypeCategory(domainEventQueueItem).TermName)(domainEventQueueItem);
		}



		#region Campaign Helper Logic
		/// <summary>
		/// Temporary Helper class so we can move all the logic into a 
		/// campaign task helper without breaking every client's existing logic
		/// In the future everyone that needs to have a custom client campaign
		/// should override the CampaignDomainEventTaskHandler
		/// </summary>
		internal class CampaignDomainEventTaskHandlerHelper
		{
			private DomainEventQueueProcessor _domainEventProcessor;
			/// <summary>
			/// Initializes a new instance of the CampaignDomainEventTaskHelper class.
			/// </summary>
			public CampaignDomainEventTaskHandlerHelper(DomainEventQueueProcessor domainEventProcessor)
			{
				_domainEventProcessor = domainEventProcessor;
			}

			public bool ProcessCampaignActions(DomainEventQueueItem domainEventQueueItem, Campaign campaign)
			{
				return _domainEventProcessor.ProcessCampaignActions(domainEventQueueItem, campaign);
			}

			public bool ProcessCampaignAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
			{
				return _domainEventProcessor.ProcessCampaignAction(domainEventQueueItem, action);
			}

			public Campaign GetCampignByDomainEvent(DomainEventQueueItem domainEventQueueItem)
			{
				return _domainEventProcessor.GetCampignByDomainEvent(domainEventQueueItem);
			}

			public bool ProcessEmailAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
			{
				return _domainEventProcessor.ProcessEmailAction(domainEventQueueItem, action);
			}

			public bool ProcessAlertAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
			{
				return _domainEventProcessor.ProcessAlertAction(domainEventQueueItem, action);
			}

			public CampaignAccountAlertGenerator GetCampaignAlertSender(DomainEventQueueItem domainEventQueueItem, AlertCampaignAction alertCampaignAction)
			{
				return _domainEventProcessor.GetCampaignAlertSender(domainEventQueueItem, alertCampaignAction);
			}

			public ICampaignEmailSender GetCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
			{
				return _domainEventProcessor.GetCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
			}
		}

































		private const string ObsoleteMessage = "This method will be moved into the CampaignDomainEventTaskHandler. You should override the method in the CampaignDomainEventTaskHandler.";

		[Obsolete(ObsoleteMessage)]
		protected virtual bool ProcessCampaignActions(DomainEventQueueItem domainEventQueueItem, Campaign campaign)
		{
			bool processedSuccessfully = false;
			string error = string.Empty;

			var actions = campaign.CampaignActions.Where(a => a.Active).OrderBy(a => a.SortIndex).ToList();

			if (actions != null && actions.Count > 0)
			{
				foreach (var action in actions)
				{
					processedSuccessfully = ProcessCampaignAction(domainEventQueueItem, action);
				}
			}
			else
			{
				var domainEventType = SmallCollectionCache.Instance.DomainEventTypes.GetById(domainEventQueueItem.DomainEventTypeID);
				error = string.Format("DomianEventQueueProcessor.ProcessCampaignActions: No Campaign Actions Found for DomainEventType: {0}", domainEventType != null ? domainEventType.Name : domainEventQueueItem.DomainEventTypeID.ToString());
				EntityExceptionHelper.GetAndLogNetStepsException(error, Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);
			}

			return processedSuccessfully;
		}

		[Obsolete(ObsoleteMessage)]
		protected virtual bool ProcessCampaignAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
		{
			bool processedSuccessfully = true;
			bool runActionNow = action.RunActionNow();

			if (runActionNow)
			{
				switch ((Constants.CampaignActionType)action.CampaignActionTypeID)
				{
					case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignActionType.NotSet:
						break;
					case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignActionType.Email:
						processedSuccessfully = ProcessEmailAction(domainEventQueueItem, action);
						break;
					case NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignActionType.Alert:
						processedSuccessfully = ProcessAlertAction(domainEventQueueItem, action);
						break;
					default:
						break;
				}
			}

			return processedSuccessfully;
		}

		[Obsolete(ObsoleteMessage)]
		protected virtual Campaign GetCampignByDomainEvent(DomainEventQueueItem domainEventQueueItem)
		{
			Contract.Assert(domainEventQueueItem != null);

			return _domainEventCampaigns.Get(domainEventQueueItem.DomainEventTypeID);
		}

		private void LogCallBegin(DomainEventQueueItem domainEventQueueItem, string name)
		{
			this.Logger.Debug("{0} for DomainEventQueueItem {1} : change tracking {2}"
				, name
				, domainEventQueueItem.DomainEventQueueItemID
				, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
				);
		}

		private void LogCallEnd(DomainEventQueueItem domainEventQueueItem, string name)
		{
			this.Logger.Debug("finished {0} for DomainEventQueueItem {1} : change tracking {2}"
				, name
				, domainEventQueueItem.DomainEventQueueItemID
				, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
				);
		}

		// Determine and process different Email Templates differently - JHE
		[Obsolete(ObsoleteMessage)]
		protected virtual bool ProcessEmailAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
		{
			bool sentSuccessfully = false;
			string error = string.Empty;

			this.LogCallBegin(domainEventQueueItem, "ProcessEmailAction");
			if (action != null)
			{
				if (action.EmailCampaignActions != null)
				{
					var emailCampaignAction = action.EmailCampaignActions != null && action.EmailCampaignActions.Count > 0 ? action.EmailCampaignActions.FirstOrDefault() : null;

					if (emailCampaignAction != null)
					{
						this.LogCallBegin(domainEventQueueItem, "GetCampaignEmailSender");
						ICampaignEmailSender emailSender = GetCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
						this.LogCallEnd(domainEventQueueItem, "GetCampaignEmailSender");

						if (emailSender != null)
						{
							//as each party will have collection of Party lists,so for sending party guest reminder use sendEmails
							//Internally it will use the partylist collection and send out the emails
							if (Constants.DomainEventType.PartyGuestReminder == (Constants.DomainEventType)domainEventQueueItem.DomainEventTypeID)
							{
								this.LogCallBegin(domainEventQueueItem, "SendEmails");
								sentSuccessfully = emailSender.SendEmails();
								this.LogCallEnd(domainEventQueueItem, "SendEmails");
							}
							else
							{
								this.LogCallBegin(domainEventQueueItem, "SendEmail");
								sentSuccessfully = emailSender.SendEmail();
								this.LogCallEnd(domainEventQueueItem, "SendEmail");
							}
						}

						if (sentSuccessfully)
						{
							//this.LogCallBegin(domainEventQueueItem, "sentSuccessfully");
							//domainEventQueueItem.QueueItemStatusID = (short)Constants.QueueItemStatus.Completed;
						}
						else
						{
							error = string.Format("Email address not found for domainEventQueueItemID {0} : QueueProcessingService.CampaignHelpers.cs.ProcessEmailAction", domainEventQueueItem.DomainEventQueueItemID);
						}
					}
					else
					{
						error = string.Format("emailCampaignAction is Null : QueueProcessingService.CampaignHelpers.cs.ProcessEmailAction");
					}
				}
				else
				{
					error = string.Format("Action is Null : QueueProcessingService.CampaignHelpers.cs.ProcessEmailAction");
				}

				if (!string.IsNullOrEmpty(error))
				{
					this.Logger.Error(error);
					EntityExceptionHelper.GetAndLogNetStepsException(error, Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);
					domainEventQueueItem.QueueItemStatusID = (short)Constants.QueueItemStatus.Failed;
				}
			}

			this.Logger.Info("finished ProcessEmailAction for DomainEventQueueItem {0} with status {1} : change tracking {2}"
				, domainEventQueueItem.DomainEventQueueItemID
				, (sentSuccessfully) ? "SUCCESS" : "FAILED"
				, domainEventQueueItem.ChangeTracker.ChangeTrackingEnabled ? "ENABLED" : "DISABLED"
				);
			return sentSuccessfully;
		}

		[Obsolete(ObsoleteMessage)]
		protected virtual bool ProcessAlertAction(DomainEventQueueItem domainEventQueueItem, CampaignAction action)
		{
			bool createdSuccessfully = false;
			string error = string.Empty;

			this.Logger.Info("ProcessAlertAction for DomainEventQueueItem {0}", domainEventQueueItem.DomainEventQueueItemID);
			if (action != null)
			{
				if (action.AlertCampaignActions != null)
				{
					var alertCampaignAction = action.AlertCampaignActions != null && action.AlertCampaignActions.Count > 0 ? action.AlertCampaignActions.FirstOrDefault() : null;

					if (alertCampaignAction != null)
					{
						var alertProvider = GetCampaignAlertSender(domainEventQueueItem, alertCampaignAction);

						if (alertProvider != null)
							createdSuccessfully = alertProvider.CreateAlert();

						if (createdSuccessfully)
						{
							this.Logger.Info("DomainEventQueueItem {0} alert created successfully", domainEventQueueItem.DomainEventQueueItemID);
							domainEventQueueItem.QueueItemStatusID = (short)Constants.QueueItemStatus.Completed;
						}
						else
						{

							error = string.Format("Unable to create alert for domainEventQueueItemID {0} : QueueProcessingService.CampaignHelpers.cs.ProcessAlertAction", domainEventQueueItem.DomainEventQueueItemID);
						}
					}
					else
						error = string.Format("alertCampaignAction is Null : QueueProcessingService.CampaignHelpers.cs.ProcessAlertAction");
				}
				else
				{
					error = string.Format("Action is Null : QueueProcessingService.CampaignHelpers.cs.ProcessAlertAction");
				}

				if (!string.IsNullOrEmpty(error))
				{
					Logger.Info(error);
					EntityExceptionHelper.GetAndLogNetStepsException(error, Data.Entities.Constants.NetStepsExceptionType.NetStepsDataException);
					domainEventQueueItem.QueueItemStatusID = (short)Constants.QueueItemStatus.Failed;
				}
			}

			this.Logger.Info("finished ProcessAlertAction for DomainEventQueueItem {0} with status {1}"
				, domainEventQueueItem.DomainEventQueueItemID
				, (createdSuccessfully) ? "SUCCESS" : "FAILED");
			return createdSuccessfully;
		}

		[Obsolete(ObsoleteMessage)]
		protected virtual CampaignAccountAlertGenerator GetCampaignAlertSender(DomainEventQueueItem domainEventQueueItem, AlertCampaignAction alertCampaignAction)
		{
			return new GenericCampaignAccountAlertGenerator(domainEventQueueItem, alertCampaignAction);
		}

		[Obsolete(ObsoleteMessage)]
		protected virtual ICampaignEmailSender GetCampaignEmailSender(DomainEventQueueItem domainEventQueueItem, EmailCampaignAction emailCampaignAction)
		{
			ICampaignEmailSender emailSender = null;

			switch ((Constants.DomainEventType)domainEventQueueItem.DomainEventTypeID)
			{
				case Constants.DomainEventType.NotSet:
					break;

				case Constants.DomainEventType.OrderCompleted:
					emailSender = new OrderCompletedCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				// Welcome email send to the new Enrollee.
				case Constants.DomainEventType.EnrollmentCompleted:
					emailSender = new ConsultantEnrolledCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.AutoshipCreditCardFailed:
					emailSender = new AutoshipOrderCreditCardFailedCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.AccountCanceled:
					break;

				case Constants.DomainEventType.AutoshipCanceled:
					emailSender = new AutoshipCanceledCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.SupportTicketInfoRequest:    // Support Ticket Status - NEEDS INPUT
					emailSender = new SupportTicketInfoRequestCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.OrderShipped:
					emailSender = new OrderShippedCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				// Email send to the Enroller every time a new distributor enrolls
				case Constants.DomainEventType.DistributorEnrolled:
					emailSender = new DistributorJoinsDownlineCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.ProspectCreated:
					emailSender = new ProspectCreatedCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.AutoshipReminder:
					emailSender = new AutoshipReminderCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.PartyGuestReminder:
					emailSender = new PartyGuestReminderCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
					break;

				case Constants.DomainEventType.AutoshipCanceledBcc:
					emailSender = new AutoshipCanceledCampaignEmailSender(domainEventQueueItem, emailCampaignAction, true);
					break;

				case Constants.DomainEventType.ExpiringPromotionNotification:
					emailSender = Create.NewWithParams<IExpiringPromotionEmailSender>(
						LifespanTracking.External
						, Param.Value(domainEventQueueItem)
						, Param.Value(emailCampaignAction)
						);
					break;

                case Constants.DomainEventType.BirthdayAlert:
                    emailSender = new BirthdayAlertCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
                    break;

                case Constants.DomainEventType.PaymentTicket:
                    emailSender = new PaymentTicketCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
                    break;

                case Constants.DomainEventType.OrderStatus:
                    emailSender = new OrderStatusCampaignEmailSender(domainEventQueueItem, emailCampaignAction);
                    break;


				default:
					break;
			}

			return emailSender;
		}
		#endregion
	}
}
