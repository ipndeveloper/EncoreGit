using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using NetSteps.SOD.Common;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.SOD.Wrapper.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class WrapperTests
    {
        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        [TestInitialize]
        public void Init()
        {
            WireupCoordinator.SelfConfigure();
        }

        [TestMethod]
        public void CanCreateUpdateAndLoginSOD()
        {
            var test = new
            {
                FirstName = "Joe",
                LastName = "Tester",
                Email = Guid.NewGuid().ToString("N") + "@test.netsteps.com",
                Password = "12345"
            };

            using (var create = Create.SharedOrNewContainer())
            {

                // Ensure we can create a distributor account...
                var api = create.New<ISuccessOnDemandApi>();
                var acct = create.New<IAccountInfo>();
                acct.FirstName = test.FirstName;
                acct.LastName = test.LastName;
                acct.Email = test.Email;
                acct.Password = test.Password;
                var created = api.CreateAccount(acct);

                Assert.IsTrue(String.IsNullOrWhiteSpace(created.Error));
                Assert.IsNotNull(created.DistID);

                // Record the DistID so we can update..
                acct.DistID = created.DistID;

                // modify the pw...
                acct.Password = "67890";

                var updated = api.UpdateAccount(acct);

                Assert.IsTrue(String.IsNullOrWhiteSpace(updated.Error));

                // Ensure we can log in as the distributor...
                var login = create.Mutation(create.New<ILoginInfo>(), l =>
                {
                    l.Email = acct.Email;
                    l.Password = acct.Password;
                });
                var loginResponse = api.Login(login);
                Assert.IsTrue(loginResponse);
            }
        }
    }
}
