using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Authorization.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Authorization.Service.Test
{
	[TestClass]
	public class AuthorizationServiceTest
	{
		[TestInitialize]
		public void Init()
		{
			WireupCoordinator.SelfConfigure();
		}

		[TestMethod]
		public void AuthorizationService_should_filter_function_set()
		{
			var functionSet = new string[] { "ToBeBlockedByConfig", "NotToBeBlocked" };
			var service = Create.New<IAuthorizationService>();
			var resultSet = service.FilterAuthorizationFunctions(functionSet);
			Assert.AreNotEqual(functionSet.Count(), resultSet.Count());
			foreach (var resultFunction in resultSet)
			{
				Assert.IsFalse(resultFunction.Equals("ToBeBlockedByConfig"));
			}
		}

		[TestMethod]
		public void AuthorizationService_should_filter_function_set_with_additional_filter()
		{
			var functionSet = new string[] { "ToBeBlockedByConfig", "NotToBeBlocked", "ToBeBlockedByFilter" };
			var service = Create.New<IAuthorizationService>();
			var resultSet = service.FilterAuthorizationFunctions(functionSet, (f) => { return !f.Equals("ToBeBlockedByFilter"); });
			Assert.AreNotEqual(functionSet.Count(), resultSet.Count());
			foreach (var resultFunction in resultSet)
			{
				Assert.IsFalse(resultFunction.Equals("ToBeBlockedByConfig"));
				Assert.IsFalse(resultFunction.Equals("ToBeBlockedByFilter"));
			}
		}

		[TestMethod]
		public void AuthorizationService_should_not_block_filter_function()
		{
			var functionSet = new string[] { "ToBeBlockedByConfig", "NotToBeBlocked", "AlsoNotToBeBlocked", "ToBeBlockedByFilter" };
			var service = Create.New<IAuthorizationService>();
			var resultSet = service.FilterAuthorizationFunctions(functionSet, (f) => { return !f.Equals("ToBeBlockedByFilter"); });
			Assert.AreNotEqual(functionSet.Count(), resultSet.Count());
			foreach (var resultFunction in resultSet)
			{
				Assert.IsFalse(resultFunction.Equals("ToBeBlockedByConfig"));
				Assert.IsFalse(resultFunction.Equals("ToBeBlockedByFilter"));
			}
			Assert.AreEqual(2, resultSet.Count());
		}
	}
}
