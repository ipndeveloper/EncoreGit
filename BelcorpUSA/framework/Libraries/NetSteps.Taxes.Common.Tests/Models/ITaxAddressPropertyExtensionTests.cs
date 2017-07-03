using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.Taxes.Common.Tests.Models
{
    [TestClass]
    public class ITaxAddressPropertyExtensionTests
    {
        [TestMethod]
        public void CodeOrNameOrDefault_WhenCodeAndNameAreNull_ReturnsNull()
        {
            var prop = TaxAddressProperty.Create(AddressPropertyKind.City, null, null);
            Assert.IsNull(prop.CodeOrNameOrDefault());
        }

        [TestMethod]
        public void CodeOrNameOrDefault_WhenCodeIsSetAndNameIsNull_ReturnsCode()
        {
            var test = new
            {
                Name = (String)null,
                Code = "AL"
            };
            var prop = TaxAddressProperty.Create(AddressPropertyKind.MainDivision, test.Name, test.Code);
            Assert.AreEqual(test.Code, prop.Code);
            Assert.AreEqual(test.Name, prop.Name);
			Assert.AreEqual(test.Code, prop.NameOrCodeOrDefault());
			Assert.AreEqual(test.Code, prop.CodeOrNameOrDefault());
        }

        [TestMethod]
        public void CodeOrNameOrDefault_WhenCodeIsNullAndNameIsSet_ReturnsName()
        {
            var test = new
            {
                Name = "Alabama",
                Code = (String)null
            };
            var prop = TaxAddressProperty.Create(AddressPropertyKind.MainDivision, test.Name, test.Code);
            Assert.AreEqual(test.Code, prop.Code);
            Assert.AreEqual(test.Name, prop.Name);
			Assert.AreEqual(test.Name, prop.NameOrCodeOrDefault());
			Assert.AreEqual(test.Name, prop.CodeOrNameOrDefault());
        }

        [TestMethod]
        public void CodeOrNameOrDefault_WhenCodeIsSetAndNameIsSet_ReturnsCode()
        {
            var test = new
            {
                Name = "Salt Lake County",
                Code = "SLC"
            };
            var prop = TaxAddressProperty.Create(AddressPropertyKind.SubDivision, test.Name, test.Code);
            Assert.AreEqual(test.Code, prop.Code);
            Assert.AreEqual(test.Name, prop.Name);
            Assert.AreEqual(test.Code, prop.CodeOrNameOrDefault());
        }

        [TestMethod]
        public void NameOrCodeOrDefault_WhenCodeAndNameAreNull_ReturnsNull()
        {
            var prop = TaxAddressProperty.Create(AddressPropertyKind.City, null, null);
            Assert.IsNull(prop.NameOrCodeOrDefault());
        }

        [TestMethod]
        public void NameOrCodeOrDefault_WhenCodeIsSetAndNameIsNull_ReturnsCode()
        {
            var test = new
            {
                Name = (String)null,
                Code = "AL"
            };
            var prop = TaxAddressProperty.Create(AddressPropertyKind.MainDivision, test.Name, test.Code);
            Assert.AreEqual(test.Code, prop.Code);
            Assert.AreEqual(test.Name, prop.Name);
            Assert.AreEqual(test.Code, prop.NameOrCodeOrDefault());
        }

        [TestMethod]
        public void NameOrCodeOrDefault_WhenCodeIsNullAndNameIsSet_ReturnsName()
        {
            var test = new
            {
                Name = "Alabama",
                Code = (String)null
            };
            var prop = TaxAddressProperty.Create(AddressPropertyKind.MainDivision, test.Name, test.Code);
            Assert.AreEqual(test.Code, prop.Code);
            Assert.AreEqual(test.Name, prop.Name);
            Assert.AreEqual(test.Name, prop.NameOrCodeOrDefault());
        }

        [TestMethod]
        public void NameOrCodeOrDefault_WhenCodeIsSetAndNameIsSet_ReturnsCode()
        {
            var test = new
            {
                Name = "Salt Lake County",
                Code = "SLC"
            };
            var prop = TaxAddressProperty.Create(AddressPropertyKind.SubDivision, test.Name, test.Code);
            Assert.AreEqual(test.Code, prop.Code);
            Assert.AreEqual(test.Name, prop.Name);
            Assert.AreEqual(test.Name, prop.NameOrCodeOrDefault());
        }

    }
}
