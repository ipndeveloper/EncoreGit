using AddressValidator.Common;
using AddressValidator.Qas.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;
using com.qas.proweb;
using AddressValidator.Qas.Config;
using Moq;

namespace AddressValidator.Qas.Tests.IntegrationTests
{
    [TestClass]
    public class QasHelperTests
    {
        public QasAddressValidatorConfig LiveConfig
        {
            get
            {
                var mock = new Mock<QasAddressValidatorConfig>();
                mock.Setup(m => m.EndpointUrl).Returns("https://ws2.ondemand.qas.com/ProOnDemand/V3/ProOnDemandService.asmx?WSDL");
                mock.Setup(m => m.UserName).Returns("9002b944-384");
				mock.Setup(m => m.Password).Returns("Iwg#2012");

                return mock.Object;
            }
        } 

        //[Ignore]
        [TestMethod]
        public void QasVarifyAddress_PassValidAddress_ShouldComeBackAsVerifiedStatus()
        {
            // Arrange
            var target = new QasHelper(LiveConfig);

            var localAddress = Create.New<IValidationAddress>();
			localAddress.Address1 = "1250 E 200 S";
            localAddress.Address2 = "Suite 3C";
            localAddress.Address3 = "";
            localAddress.SubDivision = "Lehi";
            localAddress.MainDivision = "UT";
            localAddress.PostalCode = "84043";
            localAddress.Country = "US";


            // Act
            var result = target.QasVerifyAddress(localAddress); // pass valid address

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(SearchResult));
            Assert.IsTrue(result.VerifyLevel == SearchResult.VerificationLevels.Verified);
        }

		[TestMethod]
		public void QasVarifyAddress_PassInteractionRequiredAddress_ShouldComeBackAsInteractionRequiredStatus()
		{
			// Arrange
			var target = new QasHelper(LiveConfig);

			var localAddress = Create.New<IValidationAddress>();
			localAddress.Address1 = "19 Cook";
			localAddress.Address2 = "";
			localAddress.Address3 = "";
			localAddress.SubDivision = "Portland";
			localAddress.MainDivision = "OR";
			localAddress.PostalCode = "97212";
			localAddress.Country = "US";


			// Act
			var result = target.QasVerifyAddress(localAddress); // pass valid address

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(SearchResult));
			Assert.IsTrue(result.VerifyLevel == SearchResult.VerificationLevels.InteractionRequired);
		}
		
		[TestMethod]
		public void QasVarifyAddress_PassPremisesPartialAddress_ShouldComeBackAsPremisesPartialStatus()
		{
			// Arrange
			var target = new QasHelper(LiveConfig);

			var localAddress = Create.New<IValidationAddress>();
			localAddress.Address1 = "1250 E 200 S";
			localAddress.Address2 = "";
			localAddress.Address3 = "";
			localAddress.SubDivision = "Lehi";
			localAddress.MainDivision = "UT";
			localAddress.PostalCode = "84043";
			localAddress.Country = "US";


			// Act
			var result = target.QasVerifyAddress(localAddress);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(SearchResult));
			Assert.IsTrue(result.VerifyLevel == SearchResult.VerificationLevels.PremisesPartial);
		}

		[TestMethod]
		public void QasVarifyAddress_PassStreetPartialAddress_ShouldComeBackAsStreetPartialStatus()
		{
			// Arrange
			var target = new QasHelper(LiveConfig);

			var localAddress = Create.New<IValidationAddress>();
			localAddress.Address1 = "100 eliot rd";
			localAddress.Address2 = "";
			localAddress.Address3 = "";
			localAddress.SubDivision = "arlington";
			localAddress.MainDivision = "ma";
			localAddress.PostalCode = "02474";
			localAddress.Country = "US";


			// Act
			var result = target.QasVerifyAddress(localAddress);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(SearchResult));
			Assert.IsTrue(result.VerifyLevel == SearchResult.VerificationLevels.StreetPartial);
		}

		[TestMethod]
		public void QasVarifyAddress_PassMultipleAddress_ShouldComeBackAsMultipleStatus()
		{
			// Arrange
			var target = new QasHelper(LiveConfig);

			var localAddress = Create.New<IValidationAddress>();
			localAddress.Address1 = "4440 Image";
			localAddress.Address2 = "";
			localAddress.Address3 = "";
			localAddress.SubDivision = "Dallas";
			localAddress.MainDivision = "TX";
			localAddress.PostalCode = "";
			localAddress.Country = "US";


			// Act
			var result = target.QasVerifyAddress(localAddress);

			// Assert
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(SearchResult));
			Assert.IsTrue(result.VerifyLevel == SearchResult.VerificationLevels.Multiple);
		}

		//[Ignore]
        [TestMethod]
        public void QasVerifyAddress_PassInvalidAddress_ShouldComeBackAsNotVerified()
        {
            // Arrange
            var target = new QasHelper(LiveConfig);

            var invalidAddress = Create.New<IValidationAddress>();
            invalidAddress.Address1 = "abc";
            invalidAddress.SubDivision = "xyz";
            invalidAddress.PostalCode = "111111";
            invalidAddress.Country = "US";

            // Act
            var result = target.QasVerifyAddress(invalidAddress); // pass invalid address

            // Assert
            Assert.IsTrue(result.VerifyLevel == SearchResult.VerificationLevels.None);
        }
    }
}
