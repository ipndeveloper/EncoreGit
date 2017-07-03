using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Users.Common;
using NetSteps.Web.API.Users.Common;
using NetSteps.Web.API.Users.Common.Models;

namespace NetSteps.Web.API.Users.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private IUsersResult GetUserResult(int accountID)
        {
            var result = Create.New<IUsersResult>();
            result.AccountID = accountID;
            result.Username = "TestUser";
            result.Password = "Password";
            result.Success = true;

            return result;
        }


        [TestMethod]
        public void UserResults_Returns_Json_With_New_IUserResult()
        {
            // Arrange
            int accountID = 1000;
            string username = "TestUser";
            string password = "Password";

            var user = new Mock<IUsers>();
            var log = new Mock<ILogResolver>();
            var term = new Mock<ITermResolver>();

            var userResult = GetUserResult(accountID);

            user.Setup<IUsersResult>(x => x.AuthenticateUser(It.IsAny<string>(), It.IsAny<string>())).Returns(userResult);

            var controller = new UsersController(user.Object, log.Object, term.Object);

            // Act
			var credentials = new Credentials();
            credentials.Username = username;
            credentials.Password = password;
            var result = controller.AuthenticateUser(username, credentials);
                        
            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
        }
    }
}
