using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NetSteps.Content.Common;
using NetSteps.Diagnostics.Logging.Common;
using NetSteps.Encore.Core.IoC;
using NetSteps.Modules.AvailabilityLookup.Common;
using NetSteps.Web.API.AvailabilityLookup.Common;

namespace NetSteps.Web.API.AvailabilityLookup.Tests
{
    [TestClass]
    public class AvailabilityLookupTests
    {

        private ILookupResult CreateLookupResult(int accountID)
        {
            var result = Create.New<ILookupResult>();
            result.AccountID = accountID;
            result.Success = true;

            return result;
        }


        [TestMethod]
        public void AvailabilityLookup()
        {
            // Arrange
            int accountID = 1000;

            var lookup = new Mock<IAvailabilityLookup>();
            var log = new Mock<ILogResolver>();
            var term = new Mock<ITermResolver>();

            var controller = new AvailabilityLookupController(lookup.Object, log.Object, term.Object);
            var lookupResult = CreateLookupResult(accountID);            

            lookup.Setup<ILookupResult>(x => x.Lookup(It.IsAny<string>())).Returns(lookupResult);

            // Act
            var result = controller.Lookup(null, "Test");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(NetSteps.Web.API.Base.Common.JsonResult));
        }
    }
}
