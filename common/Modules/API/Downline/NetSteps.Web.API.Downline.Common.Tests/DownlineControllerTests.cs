using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.Downline.Common;
using NetSteps.Web.API.Base.Common;
using NetSteps.Web.API.Downline.Common;
using NetSteps.Web.API.Downline.Common.Models;

namespace NetSteps.Web.API.Downline.Common.Tests
{
    [TestClass]
    public class DownlineControllerTests
    {
        private List<IDownlineAccount> GetDownlineAccounts()
        {
            List<IDownlineAccount> results = new List<IDownlineAccount>();

            for (int i = 0; i < 3; i++)
            {
                IDownlineAccount account = Create.New<IDownlineAccount>();
                account.AccountID = (i + 1) * 1000;
                account.FirstName = "Test";
                account.LastName = "Name";

                results.Add(account);
            }

            return results;
        }


        private IDownlineSearchResult GetDownlineSearchResult()
        {
            var result = Create.New<IDownlineSearchResult>();
            result.Success = true;
            result.DownlineAccounts = GetDownlineAccounts();

            return result;
        }

        [TestMethod]
        public void DownlineController_Search_By_Sponsor_Account_Returns_Json()
        {
            // Arrange
            int sponsorID = 1;
            int accountID = 1000;

			var model = Create.New<SearchDownlineModel>();
			model.AccountID = accountID;
			model.SponsorID = sponsorID;

            var downline = new Mock<IDownlineSearch>();
            var term = new Mock<ITermResolver>();
            var log = new Mock<ILogResolver>();

            var searchResult = GetDownlineSearchResult();
            downline.Setup<IDownlineSearchResult>(x => x.Search(It.IsAny<ISearchDownlineModel>())).Returns(searchResult);

            var controller = new DownlineController(downline.Object, log.Object, term.Object);

            // Act
            var result = controller.Search(sponsorID, model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
        }

        [TestMethod]
        public void DownlineController_Search_By_Sponsor_Query_Returns_Json()
        {
            // Arrange
            int sponsorID = 1;
            string query = "SELECT TOP 1 * FROM Accounts WHERE AccountID = 1000";
			var model = Create.New<SearchDownlineModel>();
			model.SponsorID = sponsorID;
			model.Query = query;

            var downline = new Mock<IDownlineSearch>();
            var term = new Mock<ITermResolver>();
            var log = new Mock<ILogResolver>();

            var searchResult = GetDownlineSearchResult();
			downline.Setup<IDownlineSearchResult>(x => x.Search(It.IsAny<ISearchDownlineModel>())).Returns(searchResult);

            var controller = new DownlineController(downline.Object, log.Object, term.Object);

            // Act
            var result = controller.Search(sponsorID, model);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
        }

    }
}
