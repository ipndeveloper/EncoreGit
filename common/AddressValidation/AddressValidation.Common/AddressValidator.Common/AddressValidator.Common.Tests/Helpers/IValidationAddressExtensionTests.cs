using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Common.Data;
using NetSteps.Data.Common.Entities;

namespace AddressValidation.Common.Tests.Helpers
{
    class Address : IAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }

    [TestClass]
    public class IValidationAddressExtensionTests
    {
        [TestMethod]
        public void ToValidationAddress_Values_Translated_From_IAddress_To_IValidationAddress_Correctly()
        {
                var address = new Address()
                                  {
                                      Address1 = "Address1",
                                      Address2 = "Address2",
                                      Address3 = "Address3",
                                      City = "City",
                                      State = "State",
                                      PostalCode = "PostalCode",
                                      Country = "US"
                                  };
                var validationAddress = address.ToValidationAddress();

                Assert.IsNotNull(validationAddress);
                Assert.AreEqual(address.Address1, validationAddress.Address1);
                Assert.AreEqual(address.Address2, validationAddress.Address2);
                Assert.AreEqual(address.Address3, validationAddress.Address3);
                Assert.AreEqual(address.City, validationAddress.SubDivision);
                Assert.AreEqual(address.State, validationAddress.MainDivision);
                Assert.AreEqual(address.PostalCode, validationAddress.PostalCode);
        }
    }
}