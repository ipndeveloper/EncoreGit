using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.QueueProcessing.Common;
using NetSteps.QueueProcessing.Modules.CampaignAction;
using NetSteps.QueueProcessing.Modules.CampaignAction.Logic;
namespace Tests
{
	[TestClass]
	public class CampaignActionProcessorTests
	{
		[TestMethod]
		public void ShouldProcessAllActiveCampaigns()
		{
			var times = new List<TimeSpan>();
			for (int iloop = 0; iloop < 10; ++iloop)
			{
				using (var container = Create.NewContainer())
				{
					container.Registry.ForType<IIndividualCampaignProcessor>()
						.Register<MockIndividualCampaignProcessor>()
						.ResolveAsSingleton()
						.End();

					container.Registry.ForType<IQueueProcessorLogger>()
						.Register<MockLogger>()
						.ResolveAsSingleton()
						.End();

					CampaignActionProcessor processor = new CampaignActionProcessor(Create.New<IQueueProcessorLogger>());

					var startTime = DateTime.Now;
					processor.CreateQueueItems(50000);

					var endTime = DateTime.Now;
					var elapsed = endTime - startTime;
					times.Add(elapsed);
				}
			}

			var average = times.Average(x => x.TotalSeconds);
			int a = 1;
		}

		[TestMethod]
		public void IndividualCampaignProcessor()
		{
			var times = new List<TimeSpan>();
			using (var container = Create.NewContainer())
			{
				container.Registry.ForType<IIndividualCampaignProcessor>()
					.Register<IndividualCampaignProcessor>()
					.ResolveAsSingleton()
					.End();

				container.Registry.ForType<IIndividualCampaignActionProcessor>()
					.Register<MockIndividualCampaignActionProcessor>()
					.ResolveAsSingleton()
					.End();

				container.Registry.ForType<IQueueProcessorLogger>()
					.Register<MockLogger>()
					.ResolveAsSingleton()
					.End();

				for (int iloop = 0; iloop < 10; ++iloop)
				{
					var startTime = DateTime.Now;

					var campaigns = Campaign.LoadAll();

					foreach (var campaign in campaigns.Where(c => c.Active))
					{
						var individualCampaignProcessor = Create.New<IIndividualCampaignProcessor>();
						individualCampaignProcessor.Process(campaign);
					}

					var endTime = DateTime.Now;
					var elapsed = endTime - startTime;
					times.Add(elapsed);
				}
			}

			var average = times.Average(x => x.TotalSeconds);
			int a = 1;
		}

		public class MockIndividualCampaignProcessor : IIndividualCampaignProcessor
		{
			int _count = 0;

			public void Process(NetSteps.Data.Entities.Campaign campaign)
			{
				++_count;
			}
		}

		public class MockIndividualCampaignActionProcessor : IIndividualCampaignActionProcessor
		{
			int _count = 0;

			public void Process(CampaignAction action, NetSteps.Data.Entities.Generated.ConstantsGenerated.CampaignType campaignType, IEnumerable<CampaignSubscriber> campaignSubscribers)
			{
				++_count;
			}
		}

		public class MockLogger : IQueueProcessorLogger
		{
			public void Debug(string format, params object[] args)
			{
			}

			public void Debug(string msg)
			{
			}

			public void Error(string format, params object[] args)
			{
			}

			public void Error(string msg)
			{
			}

			public void Info(string format, params object[] args)
			{
			}

			public void Info(string msg)
			{
			}
		}


	}
}
