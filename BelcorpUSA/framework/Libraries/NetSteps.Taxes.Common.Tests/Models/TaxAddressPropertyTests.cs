using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.Taxes.Common.Tests.Models
{
    [TestClass]
    public class TaxAddressPropertyTests
    {
        [TestMethod]
        public void Create_ConstructsTypeWithWithPropertiesMatchingArguments()
        {
            var permutations = new dynamic[] {
                new { Kind = AddressPropertyKind.City, Name = "Salt Lake City", Code = "SLC" },
                new { Kind = AddressPropertyKind.City, Name = "Salt Lake City", Code = default(String) },
                new { Kind = AddressPropertyKind.City, Name = default(String), Code = "SLC" },
                new { Kind = AddressPropertyKind.Country, Name = "United States of America", Code = "USA" },
                new { Kind = AddressPropertyKind.Country, Name = "United States of America", Code = default(String) },
                new { Kind = AddressPropertyKind.Country, Name = default(String), Code = "USA" },
                new { Kind = AddressPropertyKind.MainDivision, Name = "New Jersey", Code = "NJ" },
                new { Kind = AddressPropertyKind.MainDivision, Name = "New Jersey", Code = default(String) },
                new { Kind = AddressPropertyKind.MainDivision, Name = default(String), Code = "NJ" },
                new { Kind = AddressPropertyKind.SubDivision, Name = "Los Angeles County", Code = "LA" },
                new { Kind = AddressPropertyKind.SubDivision, Name = "Los Angeles County", Code = default(String) },
                new { Kind = AddressPropertyKind.SubDivision, Name = default(String), Code = "LA" },
                new { Kind = AddressPropertyKind.PostalCode, Name = "84103", Code = "84103" },
                new { Kind = AddressPropertyKind.PostalCode, Name = "84103", Code = default(String) },
                new { Kind = AddressPropertyKind.PostalCode, Name = default(String), Code = "84103" },                                
            };

            foreach (var p in permutations)
            {
                var prop = TaxAddressProperty.Create(p.Kind, p.Name, p.Code);
                Assert.AreEqual(p.Kind, prop.Kind);
                Assert.AreEqual(p.Name, prop.Name);
                Assert.AreEqual(p.Code, prop.Code);
            }            
        }

        [TestMethod]
        public void FromCity_CreatesCityProperty()
        {
            var tests = new dynamic[] {
                new { Name = "Salt Lake City" },
                new { Name = default(String) },
            };

            foreach (var test in tests)
            {
                var prop = TaxAddressProperty.FromCity(test.Name);
                Assert.AreEqual(AddressPropertyKind.City, prop.Kind);
                Assert.AreEqual(test.Name, prop.Name);
                Assert.AreEqual(null, prop.Code);
            }
        }

        [TestMethod]
        public void FromCounty_CreatesCountyProperty()
        {
            var tests = new dynamic[] {
                new { Name = "Salt Lake County", Code = "SL" },
                new { Name = default(String), Code = "SL" },
                new { Name = "Utah County", Code = default(String) },
            };

            foreach (var test in tests)
            {
                var first = TaxAddressProperty.FromCounty(test.Name);
                Assert.AreEqual(AddressPropertyKind.SubDivision, first.Kind);
                Assert.AreEqual(test.Name, first.Name);
                Assert.AreEqual(null, first.Code);


                var second = TaxAddressProperty.FromCounty(test.Name, test.Code);
                Assert.AreEqual(AddressPropertyKind.SubDivision, second.Kind);
                Assert.AreEqual(test.Name, second.Name);
                Assert.AreEqual(test.Code, second.Code);
            }
        }

        [TestMethod]
        public void FromCountry_CreatesCountryProperty()
        {
            var tests = new dynamic[] {
                new { Name = "Netherlands", Code = "NL" },
                new { Name = default(String), Code = "NL" },
                new { Name = "Netherlands", Code = default(String) },
            };

            foreach (var test in tests)
            {
                var first = TaxAddressProperty.FromCountry(test.Name);
                Assert.AreEqual(AddressPropertyKind.Country, first.Kind);
                Assert.AreEqual(test.Name, first.Name);
                Assert.AreEqual(null, first.Code);


                var second = TaxAddressProperty.FromCountry(test.Name, test.Code);
                Assert.AreEqual(AddressPropertyKind.Country, second.Kind);
                Assert.AreEqual(test.Name, second.Name);
                Assert.AreEqual(test.Code, second.Code);
            }
        }

        [TestMethod]
        public void FromState_CreatesStateProperty()
        {
            var tests = new dynamic[] {
                new { Name = "North Carolina", Code = "NC" },
                new { Name = default(String), Code = "NC" },
                new { Name = "South Carolina", Code = default(String) },
            };

            foreach (var test in tests)
            {
                var first = TaxAddressProperty.FromState(test.Name);
                Assert.AreEqual(AddressPropertyKind.MainDivision, first.Kind);
                Assert.AreEqual(test.Name, first.Name);
                Assert.AreEqual(null, first.Code);


                var second = TaxAddressProperty.FromState(test.Name, test.Code);
                Assert.AreEqual(AddressPropertyKind.MainDivision, second.Kind);
                Assert.AreEqual(test.Name, second.Name);
                Assert.AreEqual(test.Code, second.Code);
            }
        }

        [TestMethod]
        public void FromPostalCode_CreatesPostalCodeProperty()
        {
            var tests = new dynamic[] {
                new { Code = "84103" },
                new { Code = default(String) },
            };

            foreach (var test in tests)
            {
                var first = TaxAddressProperty.FromPostalCode(test.Code);
                Assert.AreEqual(AddressPropertyKind.PostalCode, first.Kind);
                Assert.AreEqual(test.Code, first.Name);
                Assert.AreEqual(test.Code, first.Code);
            }
        }


    }
}
