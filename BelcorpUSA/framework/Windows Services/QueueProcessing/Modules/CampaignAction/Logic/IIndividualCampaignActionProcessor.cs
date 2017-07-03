// -----------------------------------------------------------------------
// <copyright file="IIndividualCampaignActionProcessor.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace NetSteps.QueueProcessing.Modules.CampaignAction.Logic
{
	using System.Collections.Generic;
	using NetSteps.Data.Entities;

	/// <summary>
	/// TODO: Update summary.
	/// </summary>
	public interface IIndividualCampaignActionProcessor
	{
		void Process(NetSteps.Data.Entities.CampaignAction action, Constants.CampaignType campaignType, IEnumerable<CampaignSubscriber> campaignSubscribers);
	}
}
