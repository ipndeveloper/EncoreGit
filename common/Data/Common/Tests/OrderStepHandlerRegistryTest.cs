using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Data.Common.Registries;
using NetSteps.Data.Common.Registries.Concrete;

namespace Data.Common.Tests
{
	[TestClass]
	public class OrderStepHandlerRegistryTest
	{
		[TestInitialize]
		public void init()
		{
			WireupCoordinator.SelfConfigure();
		}

		private void TestWireup(IContainer container)
		{

		}

		[TestMethod]
		public void OrderStepHandlerRegistry_should_be_registered()
		{
			var registry = Create.New<IOrderStepHandlerRegistry>();
			Assert.IsNotNull(registry);
			Assert.IsInstanceOfType(registry, typeof(OrderStepHandlerRegistry));
		}
	}
}
