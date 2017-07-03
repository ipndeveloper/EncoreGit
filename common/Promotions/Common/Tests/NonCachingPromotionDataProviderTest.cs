using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Promotions.Common.CoreImplementations;
using NetSteps.Encore.Core.IoC;
using NetSteps.Promotions.Common.Model;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Promotions.Common.Repository;
using Moq;

namespace NetSteps.Promotions.Common.Tests
{
	/// <summary>
	/// Summary description for PromotionDataProviderTest
	/// </summary>
	[TestClass]
	public class NonCachingPromotionDataProviderTest
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
		}

		private void mockWireup(IContainer container)
		{
            var unitOfWorkMock = new Mock<IPromotionUnitOfWork>();
			container.ForType<IPromotionUnitOfWork>()
                .Register<IPromotionUnitOfWork>((c, p) => { return unitOfWorkMock.Object; })
				.ResolveAnInstancePerRequest()
				.End();

            var repositoryMock = new Mock<IPromotionRepository>();
			container.ForType<IPromotionRepository>()
                .Register<IPromotionRepository>((c, p) => { return repositoryMock.Object; })
				.ResolveAsSingleton()
				.End();
		}

		

	}
}
