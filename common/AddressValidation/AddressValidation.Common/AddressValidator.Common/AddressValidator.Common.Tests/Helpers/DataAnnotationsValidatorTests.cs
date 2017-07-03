using System.Linq;
using AddressValidator.Common.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;

namespace AddressValidation.Common.Tests.Helpers
{
    [TestClass]
    public class DataAnnotationsValidatorTests
    {
        [TestMethod]
        public void DataAnnotationsValidator_Valid_With_Required_Attribute_Set_Properly()
        {
            var accessCredentials = Create.New<IWithRequiredAttributes>();
            accessCredentials.Name = "test";

            var results = accessCredentials.Validate();
            Assert.AreEqual(0, results.Count());
        }

        [TestMethod]
        public void DataAnnotationsValidator_InValid_With_Required_Attribute_Set_Improperly()
        {
            var accessCredentials = Create.New<IWithRequiredAttributes>();

            var results = accessCredentials.Validate();
            Assert.AreEqual(1, results.Count());
            Assert.IsTrue(results.First().ErrorMessage.Contains("Name"));
        }
    }
}