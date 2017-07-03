using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NetSteps.Common.Base;
using NetSteps.Common.Configuration;
using NetSteps.Common.Events;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities.Processors
{
	using System.Diagnostics.Contracts;

	using NetSteps.Data.Entities.Processors.Autoships;
	using NetSteps.Data.Common.Context;
	using NetSteps.Data.Common.Services;

	/// <summary>
	/// Author: John Egbert
	/// Created: 08/18/2010
	/// </summary>
	[ContainerRegister(typeof(IAutoshipProcessor), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.Singleton)]
	public class AutoshipProcessor : IAutoshipProcessor, IDefaultImplementation
	{
		#region Constructor / Repository Initialization
		protected IProcessorActivationRepository ActivationRepository;
		public AutoshipProcessor()
			: this(null)
		{

		}

		public AutoshipProcessor(IProcessorActivationRepository activationRepository)
		{
			ActivationRepository = activationRepository ?? Create.New<IProcessorActivationRepository>();
		}
		#endregion

		#region Events
		public event EventHandler<ProgressMessageEventArgs> ProgressMessage;
		protected virtual void OnProgressMessage(object sender, ProgressMessageEventArgs e)
		{
			if (ProgressMessage != null)
				ProgressMessage(this, e);
		}
		#endregion

		#region Main AutoshipProcessor Methods
		/// <summary>
		/// This is the main entry point.
		/// </summary>
		public virtual void ProcessAllAutoships(
			DateTime runDate,
			int threads,
			CancellationTokenSource cancellationTokenSource)
		{
			if (cancellationTokenSource == null)
			{
				throw new ArgumentNullException("cancellationTokenSource");
			}

			try
			{
				var allAutoshipSchedules = GetActiveAutoshipSchedules();

				foreach (var autoshipSchedule in allAutoshipSchedules)
				{
					if (!cancellationTokenSource.IsCancellationRequested)
					{
						ProcessAutoshipSchedule(autoshipSchedule, runDate, threads, cancellationTokenSource);
					}
				}
			}
			catch (Exception ex)
			{
				throw ex.Log(Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
		}

		protected virtual void ProcessAutoshipSchedule(
			AutoshipSchedule autoshipSchedule,
			DateTime runDate,
			int threads,
			CancellationTokenSource cancellationTokenSource)
		{
			if (autoshipSchedule == null)
			{
				throw new ArgumentNullException("autoshipSchedule");
			}
			if (cancellationTokenSource == null)
			{
				throw new ArgumentNullException("cancellationTokenSource");
			}

			AutoshipBatch autoshipBatch = null;

			try
			{
				// Check override date found in config file
				var processAutoshipsAfterDate = GetProcessAutoshipsAfterDate();
				if (processAutoshipsAfterDate.HasValue && runDate <= processAutoshipsAfterDate.Value)
				{
					OnProgressMessage(this, new ProgressMessageEventArgs(string.Format("Did not process '{0}' autoship type because it's off until after {1}", autoshipSchedule.Name.ToString(), processAutoshipsAfterDate.Value), NetSteps.Common.Constants.ApplicationMessageType.Warning));
					return;
				}

				// Get autoships
				var autoshipProcessInfos = AutoshipOrder.GetAutoshipTemplatesByNextDueDateByScheduleID(autoshipSchedule.AutoshipScheduleID, runDate);

				// Validate the collection of autoships (not each individual autoship)
				if (!autoshipProcessInfos.Any())
				{
					OnProgressMessage(this, new ProgressMessageEventArgs(
						string.Format("No {0} orders scheduled", autoshipSchedule.Name),
						Constants.ApplicationMessageType.Standard
					));
					return;
				}
				if (autoshipProcessInfos.GroupBy(m => m.AutoshipOrderID).Where(g => g.Count() > 1).Any())
					throw new NetStepsBusinessException("Duplicate Autoship Orders Loaded!");
				if (autoshipProcessInfos.GroupBy(m => m.TemplateOrderID).Where(g => g.Count() > 1).Any())
					throw new NetStepsBusinessException("Duplicate Template Orders Found!");

				// Create a batch
				autoshipBatch = new AutoshipBatch
				{
					UserID = ApplicationContext.Instance.CurrentUser.UserID,
					StartDateUTC = DateTime.UtcNow
				};
				autoshipBatch.Save();

				OnProgressMessage(this, new ProgressMessageEventArgs(
					string.Format("Beginning {0} Orders", autoshipSchedule.Name),
					Constants.ApplicationMessageType.Standard,
					new ProgressEventArgs(
						autoshipProcessInfos.Count,
						0,
						string.Empty
					)
				));

				// Run autoships
				int itemsProcessed = ProcessAutoships(autoshipProcessInfos, autoshipSchedule, autoshipBatch, runDate, threads, cancellationTokenSource);

				OnProgressMessage(this, new ProgressMessageEventArgs(
					string.Format("Completed {0} Orders", autoshipSchedule.Name),
					Constants.ApplicationMessageType.Standard,
					new ProgressEventArgs(
						autoshipProcessInfos.Count,
						itemsProcessed,
						string.Empty
					)
				));
			}
			catch (Exception ex)
			{
				throw ex.Log(Constants.NetStepsExceptionType.NetStepsApplicationException);
			}
			finally
			{
				// End the batch
				if (autoshipBatch != null)
				{
					autoshipBatch.EndDateUTC = DateTime.UtcNow;
					autoshipBatch.Save();
				}
			}
		}

		public virtual int ProcessAutoships(
			List<AutoshipProcessInfo> autoshipProcessInfos,
			AutoshipSchedule autoshipSchedule,
			AutoshipBatch autoshipBatch,
			DateTime runDate,
			int threads,
			CancellationTokenSource cancellationTokenSource)
		{
			if (autoshipProcessInfos == null)
			{
				throw new ArgumentNullException("autoshipProcessInfos");
			}
			if (autoshipSchedule == null)
			{
				throw new ArgumentNullException("autoshipSchedule");
			}
			if (autoshipBatch == null)
			{
				throw new ArgumentNullException("autoshipBatch");
			}
			if (cancellationTokenSource == null)
			{
				throw new ArgumentNullException("cancellationTokenSource");
			}

			int itemsProcessing = 0;
			int itemsProcessed = 0;
			DateTime progressStartDate = DateTime.Now;

			var parallelOptions = new ParallelOptions
			{
				CancellationToken = cancellationTokenSource.Token,
				MaxDegreeOfParallelism = threads
			};

			try
			{
				Parallel.ForEach(autoshipProcessInfos, parallelOptions, autoshipProcessInfo =>
				{
					ProgressEventArgs progressEventArgs = null;

					string currentItemDescription = string.Format("Processing {0} {1} (AccountID: {2}, TemplateOrderID: {3}) {4}. ",
						autoshipProcessInfo.FirstName,
						autoshipProcessInfo.LastName,
						autoshipProcessInfo.AccountID,
						autoshipProcessInfo.TemplateOrderID,
						autoshipSchedule.Name
					);

					try
					{
						var autoshipOrder = AutoshipOrder.LoadFull(autoshipProcessInfo.AutoshipOrderID);
						var result = ProcessAutoshipOrder(autoshipBatch.AutoshipBatchID, autoshipOrder, runDate);

						progressEventArgs = new ProgressEventArgs(progressStartDate, autoshipProcessInfos.Count, Interlocked.Increment(ref itemsProcessing), currentItemDescription);
						if (result.Success)
						{
							OnProgressMessage(this, new ProgressMessageEventArgs(
								string.Format("{0}\r\n  Success",
									currentItemDescription),
								NetSteps.Common.Constants.ApplicationMessageType.Successful,
								progressEventArgs
							));
						}
						else
						{
							OnProgressMessage(this, new ProgressMessageEventArgs(
								string.Format("{0}\r\n  Error Processing Autoship Order TemplateOrderID {1} for {2} {3} ({4}).  Message: {5}",
									currentItemDescription,
									autoshipProcessInfo.TemplateOrderID,
									autoshipProcessInfo.FirstName,
									autoshipProcessInfo.LastName,
									autoshipProcessInfo.AccountID,
									result.Message),
								NetSteps.Common.Constants.ApplicationMessageType.Error,
								progressEventArgs
							));
						}
					}
					catch (Exception ex)
					{
						string publicMessage = ex.Log().PublicMessage;
						OnProgressMessage(this, new ProgressMessageEventArgs(
							string.Format("{0}\r\n  Error Processing Autoship Order TemplateOrderID {1} for {2} {3} ({4}).  Message: {5}",
								currentItemDescription,
								autoshipProcessInfo.TemplateOrderID,
								autoshipProcessInfo.FirstName,
								autoshipProcessInfo.LastName,
								autoshipProcessInfo.AccountID,
								publicMessage),
							NetSteps.Common.Constants.ApplicationMessageType.Error,
							progressEventArgs
						));

						// If the exception was caused by a failure to successfully save the autoship order,
						// or prevented the autoship order from having save called on it, its NextRunDate will be out of date.
						// Try again to change the NextRunDate if it was not updated.
						try
						{
							var autoshipOrder = AutoshipOrder.LoadFull(autoshipProcessInfo.AutoshipOrderID);
							if (autoshipOrder.NextRunDate < DateTime.Now)
							{
								var newNextRunDate = autoshipOrder.CalculateNextRunDateOnFailure(runDate);
								autoshipOrder.NextRunDate = newNextRunDate;
								autoshipOrder.Save();
							}
						}
						catch (Exception ex2)
						{
							string publicMessage2 = ex2.Log().PublicMessage;
							OnProgressMessage(this, new ProgressMessageEventArgs(
								string.Format("{0}\r\n  Error attempting last chance save for Autoship Order TemplateOrderID {1} for {2} {3} ({4}).  Message: {5}",
									currentItemDescription,
									autoshipProcessInfo.TemplateOrderID,
									autoshipProcessInfo.FirstName,
									autoshipProcessInfo.LastName,
									autoshipProcessInfo.AccountID,
									publicMessage2),
								NetSteps.Common.Constants.ApplicationMessageType.Error,
								progressEventArgs
							));
						}
					}
					finally
					{
						Interlocked.Increment(ref itemsProcessed);
					}
				});
			}
			// OperationCanceledException means the process was cancelled and is ending gracefully.
			// Other exception types will be caught by the caller
			catch (OperationCanceledException)
			{
				OnProgressMessage(this, new ProgressMessageEventArgs(
					"Operation Cancelled",
					Constants.ApplicationMessageType.Standard
				));
			}

			return itemsProcessed;
		}

		public virtual BasicResponseItem<Order> ProcessAutoshipOrder(
			int autoshipBatchID,
			AutoshipOrder autoshipOrder,
			DateTime runDate)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			BasicResponseItem<Order> response = ValidateAutoshipForProcessing(autoshipOrder, runDate);

			// These will update the order, but they do not save the order.
			if (response.Success)
			{
				AutoshipOrderSucceeded(autoshipBatchID, autoshipOrder, response.Item, response.Message, runDate);
			}
			else
			{
				AutoshipOrderFailed(autoshipBatchID, autoshipOrder, response.Item, response.Message, runDate);
			}

			// Final save
			autoshipOrder.Save();

			return response;
		}
		#endregion

		#region Validation Methods
		protected virtual BasicResponseItem<Order> ValidateAutoshipForProcessing(AutoshipOrder autoshipOrder, DateTime runDate)
		{
			try
			{
				// Validate before setting LastRunDate
				// This may make changes to the order, but it does not save the order.
				var validateAutoshipResult = ValidateAutoship(autoshipOrder, runDate);

				// Set LastRunDate and save, regardless of whether or not validation succeeded
				autoshipOrder.LastRunDate = runDate;
				autoshipOrder.Save();

				if (validateAutoshipResult.Status == AutoshipStatus.Invalid)
				{
					// Validation failed
					return new BasicResponseItem<Order> { Success = false, Message = validateAutoshipResult.Message };
				}

				if (validateAutoshipResult.Status == AutoshipStatus.AlreadySubmitted)
				{
					return new BasicResponseItem<Order> { Success = true, Message = validateAutoshipResult.Message };
				}

				// Generate new order
				var childOrder = autoshipOrder.GenerateChildOrderFromTemplate();

				// Validate generated order
				var childResponse = ValidateGeneratedChild(autoshipOrder, childOrder);

				if (!childResponse.Success)
				{
					return new BasicResponseItem<Order> { Success = false, Message = childResponse.Message };
				}

				// Submit new order
				var orderContext = Create.New<IOrderContext>();
				orderContext.Order = childOrder;
				var orderService = Create.New<IOrderService>();
				var submitOrderResult = orderService.SubmitOrder(orderContext);

				return new BasicResponseItem<Order> { Item = childOrder, Success = submitOrderResult.Success, Message = submitOrderResult.Message };
			}
			catch (Exception ex)
			{
				return new BasicResponseItem<Order> { Success = false, Message = ex.Log(Constants.NetStepsExceptionType.NetStepsApplicationException, null, autoshipOrder.AccountID).PublicMessage };
			}
		}

		protected virtual BasicResponseItem<Order> ValidateGeneratedChild(AutoshipOrder template, Order child)
		{
			return new BasicResponseItem<Order>
			{
				Success = true,
				Item = child
			};
		}

		protected enum AutoshipStatus
		{
			AlreadySubmitted,
			Valid,
			Invalid
		}

		protected struct AutoshipValidation
		{
			public AutoshipValidation(AutoshipStatus status, string message)
				: this(status)
			{
				Message = message;
			}

			public AutoshipValidation(AutoshipStatus status)
				: this()
			{
				Status = status;
			}

			public AutoshipStatus Status;
			public string Message;
		}

		protected virtual AutoshipValidation ValidateAutoship(
			AutoshipOrder autoshipOrder,
			DateTime runDate)
		{
			if (autoshipOrder == null)
			{
				throw new ArgumentNullException("autoshipOrder");
			}

			try
			{
				// Validate account
				if (autoshipOrder.Account == null)
				{
					return new AutoshipValidation(AutoshipStatus.Invalid, "Autoship account is not loaded.");
				}

				// Validate order
				if (autoshipOrder.Order == null)
				{
					return new AutoshipValidation(AutoshipStatus.Invalid, "Autoship is missing an order template.");
				}

				// Validate order customer
				if (!autoshipOrder.Order.OrderCustomers.Any())
				{
					return new AutoshipValidation(AutoshipStatus.Invalid, "Order template must contain at least one customer.");
				}
				var orderCustomer = autoshipOrder.Order.OrderCustomers[0];

				// Validate order payment
				if (!orderCustomer.OrderPayments.Any())
				{
					return new AutoshipValidation(AutoshipStatus.Invalid, "Order template must contain at least one payment.");
				}

				// Validate NextRunDate
				if (autoshipOrder.NextRunDate == null)
				{
					return new AutoshipValidation(AutoshipStatus.Invalid, "Autoship NextRunDate is not set.");
				}
				if (autoshipOrder.NextRunDate.Value > runDate)
				{
					return new AutoshipValidation(AutoshipStatus.Invalid, string.Format("Autoship NextRunDate ({0:d}) is after the current run date ({1:d}).", autoshipOrder.NextRunDate, runDate));
				}

				// Validate LastRunDate
				if (autoshipOrder.LastRunDate != null
					&& autoshipOrder.LastRunDate.Value >= runDate)
				{
					return new AutoshipValidation(AutoshipStatus.AlreadySubmitted, String.Format("This Autoship has already been submitted.  LastRunDate: {0} RunDate: {1}", autoshipOrder.LastRunDate, runDate));
				}

				// Validate DateLastCreated
				var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
				var intervalType = SmallCollectionCache.Instance.IntervalTypes.GetById(autoshipSchedule.IntervalTypeID);
				var startOfInterval = intervalType.GetStartOfInterval(runDate);
				if (autoshipOrder.DateLastCreated != null
					&& autoshipOrder.DateLastCreated.Value >= startOfInterval)
				{
					return new AutoshipValidation(AutoshipStatus.AlreadySubmitted, String.Format("This Autoship has already been created.  DateLastCreated: {0} RunDate: {1}", autoshipOrder.DateLastCreated, runDate));
				}

				// Validate most recent successful child order
				var successfulChildOrderDates = Order.GetCompletedOrderDates(
					parentOrderID: autoshipOrder.TemplateOrderID,
					sortDirection: Constants.SortDirection.Descending,
					pageSize: 1
				);
				if (successfulChildOrderDates.Any())
				{
					var mostRecentSuccessfulChildOrderDate = successfulChildOrderDates
						.First()
						.UTCToLocal(ApplicationContext.Instance.CorporateTimeZoneInfo);
					if (mostRecentSuccessfulChildOrderDate >= startOfInterval)
					{
						return new AutoshipValidation(AutoshipStatus.AlreadySubmitted, String.Format("This Autoship has already successful child orders.  MostRecentSuccessfulChildOrderDate: {0} StartOfInterval: {1}", mostRecentSuccessfulChildOrderDate, startOfInterval));
					}
				}

				// Validate (and set) OrderType
				var orderTypeID = AutoshipOrder.BusinessLogic.GetDefaultOrderTypeID(autoshipOrder.Account.AccountTypeID, true);
				if (autoshipOrder.Order.OrderTypeID != orderTypeID)
				{
					autoshipOrder.Order.OrderTypeID = orderTypeID;
				}

				// Validate items for a fixed autoship
				if (autoshipOrder.HasFixedTemplateItems())
				{
					autoshipOrder.SyncFixedOrderItems(false);
				}

				return new AutoshipValidation(AutoshipStatus.Valid, String.Format("All Validation Successful"));
			}
			catch (Exception ex)
			{
				EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException, (autoshipOrder != null && autoshipOrder.Order != null) ? autoshipOrder.Order.OrderID.ToIntNullable() : null);
				return new AutoshipValidation(AutoshipStatus.Invalid, ex.Message);
			}
		}
		#endregion

		#region Processor Helper Methods
		protected virtual IEnumerable<AutoshipSchedule> GetActiveAutoshipSchedules()
		{
			return SmallCollectionCache.Instance.AutoshipSchedules
				.Where(x => x.Active);
		}

		protected virtual DateTime? GetProcessAutoshipsAfterDate()
		{
			string processAutoshipsAfterDateString = ConfigurationManager.AppSettings["ProcessAutoshipsAfterDate"];
			DateTime processAutoshipsAfterDate;
			if (!DateTime.TryParse(processAutoshipsAfterDateString, out processAutoshipsAfterDate))
			{
				return null;
			}
			return processAutoshipsAfterDate;
		}

		protected virtual void AutoshipOrderSucceeded(
			int autoshipBatchID,
			AutoshipOrder autoshipOrder,
			Order newOrder,
			string message,
			DateTime runDate)
		{
			// Create a log entry before updating NextRunDate, so that the # of attempts is correct.
			AutoshipOrder.LogAutoshipGenerationResults(
				autoshipBatchID,
				autoshipOrder.TemplateOrderID,
				newOrder != null ? (int?)newOrder.OrderID : null,
				true,
				message,
				runDate
			);

			autoshipOrder.DateLastCreated = runDate;
			autoshipOrder.NextRunDate = autoshipOrder.CalculateNextRunDateOnSuccess(runDate);
			autoshipOrder.ConsecutiveOrders = autoshipOrder.CalculateConsecutiveOrders();

			var autoshipOrderDTO = CreateDTOFromEntity(autoshipOrder);
			RenewAutoshipSiteIfInCancelledState(autoshipOrderDTO);
			PerformCustomSuccessLogic(autoshipOrder);
			if (newOrder != null)
			{
				OnAutoshipOrderProcessed(autoshipBatchID, autoshipOrder, newOrder, message, runDate, true);
			}
		}

		protected virtual void AutoshipOrderFailed(
			int autoshipBatchID,
			AutoshipOrder autoshipOrder,
			Order newOrder,
			string message,
			DateTime runDate)
		{
			if (newOrder != null)
			{
				newOrder.OrderStatusID = (short)Constants.OrderStatus.PendingError;
				newOrder.Save();
			}

			// Create a log entry before updating NextRunDate, so that the # of attempts is correct.
			AutoshipOrder.LogAutoshipGenerationResults(
				autoshipBatchID,
				autoshipOrder.TemplateOrderID,
				newOrder != null ? (int?)newOrder.OrderID : null,
				false,
				message,
				runDate
			);

			var newNextRunDate = autoshipOrder.CalculateNextRunDateOnFailure(runDate);
			autoshipOrder.NextRunDate = newNextRunDate;

			var autoshipOrderDTO = CreateDTOFromEntity(autoshipOrder);
			CancelAutoshipIfPastAllottedAttempts(autoshipOrderDTO, runDate, newNextRunDate);
			PerformCustomFailureLogic(autoshipOrder, runDate, newNextRunDate);

			// Avoid repeat reminders for autoship retries
			if (newNextRunDate == runDate.AddDays(1))
			{
				autoshipOrder.AutoshipReminderNextRunDate = newNextRunDate;
			}

			autoshipOrder.ConsecutiveOrders = autoshipOrder.CalculateConsecutiveOrders();
			OnAutoshipOrderProcessed(autoshipBatchID, autoshipOrder, newOrder, message, runDate, false);
		}

		protected virtual void PerformCustomFailureLogic(AutoshipOrder autoshipOrder, DateTime runDate, DateTime nextRunDate)
		{ }

		protected virtual void PerformCustomSuccessLogic(AutoshipOrder autoshipOrder)
		{
			int autoshipScheduleId = 0;
			int maxSuccessfulOrders = 0;
			if (!string.IsNullOrEmpty((ConfigurationManager.AppSettings["AutoshipPartialEnrollmentScheduleId"])))
				int.TryParse(ConfigurationManager.AppSettings["AutoshipPartialEnrollmentScheduleId"], out autoshipScheduleId);

			if (!string.IsNullOrEmpty((ConfigurationManager.AppSettings["MaxPartialEnrollmentSuccessfulOrders"])))
				int.TryParse(ConfigurationManager.AppSettings["MaxPartialEnrollmentSuccessfulOrders"], out maxSuccessfulOrders);

			if (autoshipScheduleId == 0 || maxSuccessfulOrders == 0)
				return; // do nothing

			// cancel the autoship order; we do not want to do any more processing on this autoship order
			var successfulOrders = Order.GetCompletedOrderDates(parentOrderID: autoshipOrder.TemplateOrderID).Count;
			if (autoshipOrder.AutoshipScheduleID == autoshipScheduleId &&
				successfulOrders >= maxSuccessfulOrders)
			{
				autoshipOrder.Order.OrderStatusID = (int)NetSteps.Data.Entities.Constants.OrderStatus.Cancelled;
				autoshipOrder.EndDate = DateTime.UtcNow;
				autoshipOrder.Save();
			}
		}

		protected virtual void OnAutoshipOrderProcessed(
			int autoshipBatchID,
			AutoshipOrder autoshipOrder,
			Order newOrder,
			string message,
			DateTime runDate,
			bool success)
		{

		}
		#endregion

		#region Account (De/Re)activation


		protected virtual void RenewAutoshipSiteIfInCancelledState(IAutoshipOrderDTO autoshipOrder)
		{
			Contract.Requires<ArgumentNullException>(autoshipOrder != null);
			if (autoshipOrder.AccountID == 0)
			{
				OnProgressMessage(this, new ProgressMessageEventArgs(string.Format("AccountID is 0. Skipping reactivation for autoship order {0}", autoshipOrder.AutoshipOrderID), Constants.ApplicationMessageType.Warning));
				return;
			}
			if (autoshipOrder.AutoshipOrderID == 0)
			{
				OnProgressMessage(this, new ProgressMessageEventArgs(string.Format("AutoshipOrderID is 0. Skipping reactivation for account {0}", autoshipOrder.AccountID), Constants.ApplicationMessageType.Warning));
				return;
			}

			IAutoshipScheduleDTO schedule = GetAndValidateAutoshipSchedule(autoshipOrder);

			if (schedule == null)
			{ return; }

			ISiteDTO autoshipSite = ActivationRepository.LoadSite(autoshipOrder.AccountID, autoshipOrder.AutoshipOrderID);
			if (autoshipSite == null || autoshipSite.SiteStatusID != (int)Constants.SiteStatus.DisabledForNonPayment)
			{ return; }

			ReactivateSite(autoshipSite.SiteID);
		}

		protected virtual void CancelAutoshipIfPastAllottedAttempts(IAutoshipOrderDTO autoshipOrder, DateTime runDate, DateTime newNextRunDate)
		{
			Contract.Requires<ArgumentNullException>(autoshipOrder != null);
			if (autoshipOrder.AccountID == 0)
			{
				OnProgressMessage(this, new ProgressMessageEventArgs(string.Format("AccountID is 0. Skipping reactivation for autoship order {0}", autoshipOrder.AutoshipOrderID), Constants.ApplicationMessageType.Warning));
				return;
			}
			if (autoshipOrder.AutoshipOrderID == 0)
			{
				OnProgressMessage(this, new ProgressMessageEventArgs(string.Format("AutoshipOrderID is 0. Skipping reactivation for account {0}", autoshipOrder.AccountID), Constants.ApplicationMessageType.Warning));
				return;
			}

			IAutoshipScheduleDTO schedule = GetAndValidateAutoshipSchedule(autoshipOrder);
			if (schedule == null)
			{ return; }

			IIntervalTypeDTO intervalType = ActivationRepository.LoadInterval(schedule.IntervalTypeID, runDate);
			if (intervalType == null)
			{
				OnProgressMessage(this, new ProgressMessageEventArgs(string.Format("There is no interval type for {0}", schedule.IntervalTypeID), Constants.ApplicationMessageType.Warning));
				return;
			}

			List<DateTime> successfulPaymentDates = Order.GetCompletedOrderDates(parentOrderID: autoshipOrder.TemplateOrderID, sortDirection: Constants.SortDirection.Descending, pageSize: 1);
			if (IsCurrentIntervalPaid(intervalType, runDate, successfulPaymentDates))
			{ return; }

			ISiteDTO autoshipSite = ActivationRepository.LoadSite(autoshipOrder.AccountID, autoshipOrder.AutoshipOrderID);
			if (autoshipSite == null || autoshipSite.SiteStatusID == (int)Constants.SiteStatus.DisabledForNonPayment)
			{ return; }

			if (!ShouldCancelSite(successfulPaymentDates, autoshipOrder, intervalType, newNextRunDate))
			{ return; }

			OnProgressMessage(this, new ProgressMessageEventArgs(string.Format("Disabling site for account {0}", autoshipOrder.AccountID), Constants.ApplicationMessageType.Warning));
			DeactivateWebsite(autoshipSite.SiteID);
		}

		public bool ShouldCancelSite(List<DateTime> successfulPaymentDates, IAutoshipOrderDTO autoshipOrder, IIntervalTypeDTO intervalType, DateTime newNextRunDate)
		{
			DateTime originDate = successfulPaymentDates.Count > 0 ? successfulPaymentDates[0] : autoshipOrder.StartDate ?? new DateTime();

			return IsNextRunDateOnNewInterval(intervalType, newNextRunDate)
			&& IsAutoshipPastAllottedIntervals(autoshipOrder, intervalType, newNextRunDate, originDate);
		}

		public IAutoshipScheduleDTO GetAndValidateAutoshipSchedule(IAutoshipOrderDTO autoshipOrder)
		{
			Contract.Requires<ArgumentNullException>(autoshipOrder != null);
			IAutoshipScheduleDTO schedule = ActivationRepository.GetActiveSchedule(autoshipOrder.AutoshipScheduleID);

			if (schedule == null)
			{ return null; }

			if (schedule.AutoshipScheduleTypeID != (int)Constants.AutoshipScheduleType.SiteSubscription)
			{ return null; }

			return schedule;
		}

		protected void DeactivateWebsite(int autoshipSiteID)
		{
			Contract.Requires<ArgumentException>(autoshipSiteID != 0);
			try
			{
				ActivationRepository.DeactivateSite(autoshipSiteID);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error occurred in ItWorks AutoshipProcessor.DeactivateWebsite; (Original Message: {0})", ex.Message), ex);
			}
		}

		protected void ReactivateSite(int autoshipSiteID)
		{
			Contract.Requires<ArgumentException>(autoshipSiteID != 0);
			try
			{
				ActivationRepository.ActivateSite(autoshipSiteID);
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("Error occurred in ItWorks AutoshipProcessor.ReactiveSite;\nThis site needs to be turned back on. SiteID: {1} (Original Message: {0})", ex.Message, autoshipSiteID), ex);
			}
		}


		public bool IsCurrentIntervalPaid(IIntervalTypeDTO intervalType, DateTime runDate, List<DateTime> successfulChildOrderDates = null)
		{
			Contract.Requires<ArgumentNullException>(intervalType != null);

			if (successfulChildOrderDates == null || !successfulChildOrderDates.Any())
			{
				return false;
			}

			//If there were any successful payments
			var mostRecentSuccessfulChildOrderDate = successfulChildOrderDates.OrderByDescending(da => da).First()
				.UTCToLocal(ApplicationContext.Instance.CorporateTimeZoneInfo);

			DateTime startOfInterval = intervalType.StartOfInterval;
			return mostRecentSuccessfulChildOrderDate >= startOfInterval;
		}

		public bool IsNextRunDateOnNewInterval(IIntervalTypeDTO intervalType, DateTime nextRunDate)
		{
			Contract.Requires<ArgumentNullException>(intervalType != null);
			return nextRunDate >= intervalType.StartOfNextInterval;
		}

		public bool IsAutoshipPastAllottedIntervals(IAutoshipOrderDTO autoshipOrder, IIntervalTypeDTO intervalType, DateTime nextRunDate, DateTime previousRunDate)
		{
			Contract.Requires<ArgumentNullException>(nextRunDate != null);
			Contract.Requires<ArgumentNullException>(previousRunDate != null);
			Contract.Requires<ArgumentNullException>(intervalType != null);

			int numberOfUnpaid = ActivationRepository.GetNumberOfIntervalsUnpaid(intervalType.IntervalTypeID, nextRunDate, previousRunDate);
			return numberOfUnpaid >= autoshipOrder.MaximumIntervals;
		}

		protected IAutoshipOrderDTO CreateDTOFromEntity(AutoshipOrder autoshipOrder)
		{
			var returnValue = Create.New<IAutoshipOrderDTO>();
			returnValue.AccountID = autoshipOrder.AccountID;
			returnValue.AutoshipOrderID = autoshipOrder.AutoshipOrderID;
			returnValue.AutoshipScheduleID = autoshipOrder.AutoshipScheduleID;
			returnValue.TemplateOrderID = autoshipOrder.TemplateOrderID;
			returnValue.StartDate = autoshipOrder.StartDate;

			if (autoshipOrder.AutoshipSchedule == null)
			{
				autoshipOrder.AutoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipOrder.AutoshipScheduleID);
			}

			returnValue.MaximumIntervals = autoshipOrder.AutoshipSchedule.MaximumFailedIntervals ?? 1;
			return returnValue;
		}
		#endregion
	}
}