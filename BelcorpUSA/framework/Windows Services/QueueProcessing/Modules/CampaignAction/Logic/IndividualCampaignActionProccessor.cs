// -----------------------------------------------------------------------
// <copyright file="IndividualCampaignActionProccessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;

namespace NetSteps.QueueProcessing.Modules.CampaignAction.Logic
{

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	[ContainerRegister(typeof(IIndividualCampaignActionProcessor), RegistrationBehaviors.Default)]
	public class IndividualCampaignActionProccessor : IIndividualCampaignActionProcessor
	{
		IQueueProcessorLogger _logger = Create.New<IQueueProcessorLogger>();

		public void Process(NetSteps.Data.Entities.CampaignAction action, Constants.CampaignType campaignType, IEnumerable<CampaignSubscriber> campaignSubscribers)
		{
			// Reload to make sure it's up-to-date
			var campaignAction = NetSteps.Data.Entities.CampaignAction.LoadFull(action.CampaignActionID);

			bool isComplete = campaignAction.IsCompleted;

			// TODO: Make the "long-running" timespan configurable
			bool isLongRunning = campaignAction.IsRunning && DateTime.Now > campaignAction.LastRunDate + TimeSpan.FromMinutes(20);

			try
			{
				if ((!isComplete && campaignAction.RunActionNow()) || isLongRunning)
				{
					campaignAction.LastRunDate = DateTime.Now;
					campaignAction.IsRunning = true;
					campaignAction.Save();

					switch (campaignType)
					{
						case Constants.CampaignType.Newsletters:

							//TODO:  Create Slim Search to get the validSubscribers.
							var alreadyQueuedAccounts = campaignAction.CampaignActionQueueItems.ToList(a => a.EventContext.AccountID);
							var validSubscribers = campaignSubscribers.Where(c => !alreadyQueuedAccounts.Contains(c.AccountID));

							//List<int> validSubscribers3 = CampaignAction.GetListOfPendingSubscribers(action.CampaignActionID);

							if (validSubscribers.Count() == 0)
							{
								isComplete = true;
							}
							else
							{
								foreach (var subscriber in validSubscribers)
								{
									CampaignActionQueueItem.AddCampaignActionQueueItemToQueue(campaignAction.CampaignActionID, null, subscriber.AccountID, null);
								}
							}

							break;
						default:
							break;
					}
				}
				campaignAction.IsCompleted = isComplete;
			}
			catch (Exception ex)
			{
				campaignAction.IsCompleted = false;
				_logger.Info("CampaignActionProcessor CampaignActionID: {0}, Exception: {1}", campaignAction.CampaignActionID, ex.InnerException);
			}
			finally
			{
				campaignAction.IsRunning = false;
				campaignAction.Save();
			}
		}

	}
}
