using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using Moq;
using NetSteps.Modules.Users.Common;

namespace NetSteps.Modules.Users.Tests
{
	[TestClass]
	public class UsersTests
	{
		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		private IUsersSearch CreateTestUsersSite(int accountID, string password)
		{
			var res = Create.Mutation(Create.New<IUsersSearch>(), ia =>
			{
				ia.AccountID = 1;
				ia.Password = password;
			});
			return res;
		}

		[TestMethod]
		public void Users_Successful_AccountID_ReturnsNewIUsersResultWithPassword()
		{
			var mock = new Mock<IUsersRepositoryAdapter>();
			mock.Setup<IUsersSearch>(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns((int s1, string s2) => CreateTestUsersSite(s1, s2));
			var auth = new DefaultUsers(mock.Object);
			var result = auth.AuthenticateUser("TestAccount", "existingPassword");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IUsersResult));
			Assert.IsTrue(result.Success);
		}

		[TestMethod]
		public void Users_UnSuccessful_AccountID_ReturnsNewIUsersResult()
		{
			var mock = new Mock<IUsersRepositoryAdapter>();
            mock.Setup<IUsersSearch>(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>()));
			var auth = new DefaultUsers(mock.Object);
            var result = auth.AuthenticateUser("TestAccount", "existingPassword");

			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(IUsersResult));
			Assert.IsFalse(result.Success);
		}
	}
}
