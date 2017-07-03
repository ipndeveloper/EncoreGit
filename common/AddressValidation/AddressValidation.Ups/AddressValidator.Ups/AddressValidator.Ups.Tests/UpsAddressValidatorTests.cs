using System.Linq;
using AddressValidation.Common;
using AddressValidator.Common;
using AddressValidator.Ups.Config;
using AddressValidator.Ups.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.IoC;

namespace AddressValidator.Ups.Tests
{
    [TestClass]
    public class UpsAddressValidatorTests : AbstractTests
    {
        [TestMethod, ExpectedException(typeof(UpsAddressValidatorCredentialException))]
        public void UpsAddressValidator_SecurityCredentials_UserName_Required_But_Is_Empty()
        {
            var addressValidator = new UpsAddressValidator();
        }

        [TestMethod]
        public void UpsAddressValidator_ValidateAddress_Ambiguous_Result()
        {
            var addressValidator = SetupValidatorWithTestingConfig();

            var address = SetupValidAmbiguousAddress();
            var result = addressValidator.ValidateAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(AddressInfoResultState.Success, result.Status);
            Assert.IsTrue(result.Message.Contains("AmbiguousAddressIndicator"));
            Assert.IsTrue(result.ValidAddresses.Any());
        }

        [TestMethod]
        public void UpsAddressValidator_ValidateAddress_Valid_Result()
        {
            var addressValidator = SetupValidatorWithTestingConfig();

            var address = SetupValidAddress();
            var result = addressValidator.ValidateAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(AddressInfoResultState.Success, result.Status);
            Assert.IsTrue(result.Message.Contains("ValidAddressIndicator"));
            Assert.IsTrue(result.ValidAddresses.Count() == 1);
        }


        [TestMethod]
        public void UpsAddressValidator_ValidateAddressWithApt_Valid_Result()
        {
            var addressValidator = SetupValidatorWithTestingConfig();

            var address = SetupValidAddressWithApt();
            var result = addressValidator.ValidateAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(AddressInfoResultState.Success, result.Status);
            Assert.IsTrue(result.Message.Contains("ValidAddressIndicator"));
            Assert.IsTrue(result.ValidAddresses.Count() == 1);
        }


        [TestMethod]
        public void UpsAddressValidator_ValidateAddress_In_Utah_Valid_Result()
        {
            var addressValidator = SetupValidatorWithTestingConfig();

            var address = SetupNonCaliforniaAddress();
            var result = addressValidator.ValidateAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(AddressInfoResultState.Success, result.Status);
            Assert.IsTrue(result.Message.Contains("ValidAddressIndicator"));
            Assert.IsTrue(result.ValidAddresses.Count() == 1);
        }

        [TestMethod]
        public void UpsAddressValidator_ValidateAddress_NoCandidates_With_Missing_Address()
        {
            var addressValidator = SetupValidatorWithTestingConfig();

            var address = SetupValidAddress();
            address.Address1 = string.Empty;

            var result = addressValidator.ValidateAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(AddressInfoResultState.Success, result.Status);
            Assert.IsTrue(result.Message.Contains("NoCandidatesIndicator"));
            Assert.IsTrue(!result.ValidAddresses.Any());
        }

        [TestMethod]
        public void UpsAddressValidator_ValidateAddress_NoCandidates_With_Missing_Address_And_City()
        {
            var addressValidator = SetupValidatorWithTestingConfig();

            var address = SetupValidAddress();
            address.Address1 = string.Empty;
            address.SubDivision = string.Empty;

            var result = addressValidator.ValidateAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(AddressInfoResultState.Success, result.Status);
            Assert.IsTrue(result.Message.Contains("NoCandidatesIndicator"));
            Assert.IsTrue(!result.ValidAddresses.Any());
        }

        [TestMethod]
        public void UpsAddressValidator_ValidateAddress_NoCandidates_With_Missing_City_And_ZipCode()
        {
            var addressValidator = SetupValidatorWithTestingConfig();

            var address = SetupValidAddress();
            address.Address1 = string.Empty;
            address.SubDivision = string.Empty;

            var result = addressValidator.ValidateAddress(address);

            Assert.IsNotNull(result);
            Assert.AreEqual(AddressInfoResultState.Success, result.Status);
            Assert.IsTrue(result.Message.Contains("NoCandidatesIndicator"));
            Assert.IsTrue(!result.ValidAddresses.Any());
        }

        #region Helpers
        UpsAddressValidator SetupValidatorWithTestingConfig()
        {
            return new UpsAddressValidator(new UpsAddressValidatorConfiguration()
                                               {
                                                   UserName = "TylerGarlick",
                                                   Password = "5Orange55",
                                                   EndpointUrl = "https://onlinetools.ups.com/webservices/XAV",
                                                   AccessLicenseNumber = "2C9A674E8713D2B8"
                                               });
        }

        IValidationAddress SetupValidAddress()
        {
            return Create.Mutation(Create.New<IValidationAddress>(), t =>
            {
                t.Address1 = "1 Infinite Loop";
                t.Country = "US";
                t.MainDivision = "CA";
                t.SubDivision = "Cupertino";
                t.PostalCode = "95014";
            });
        }

        IValidationAddress SetupValidAddressWithApt()
        {
            return Create.Mutation(Create.New<IValidationAddress>(), t =>
            {
                t.Address1 = "1100 South 2000 East";
                t.Address2 = "# 123";
                t.Country = "US";
                t.MainDivision = "UT";
                t.SubDivision = "Clearfield";
                t.PostalCode = "84015";
            });
        }

        IValidationAddress SetupValidAmbiguousAddress()
        {
            return Create.Mutation(Create.New<IValidationAddress>(), t =>
            {
                t.Address1 = "AIRWAY ROAD SUITE 7";
                t.Country = "US";
                t.MainDivision = "CA";
                t.SubDivision = "San Diego";
                t.PostalCode = "92154";
            });
        }


        IValidationAddress SetupNonCaliforniaAddress()
        {
            return Create.Mutation(Create.New<IValidationAddress>(), t =>
            {
                t.Address1 = "1250 E 200 S, Suite 3C";
                t.Country = "US";
                t.MainDivision = "UT";
                t.SubDivision = "Lehi";
                t.PostalCode = "84043";
            });
        }
        #endregion
    }
}