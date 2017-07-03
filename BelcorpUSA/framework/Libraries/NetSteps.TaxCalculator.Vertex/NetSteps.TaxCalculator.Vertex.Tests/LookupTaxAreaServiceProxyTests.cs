using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Addresses.Common.Models;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Xml;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.TaxCalculator.Vertex.Tests
{
	[DTO]
	public interface ITestAddress : IAddress
	{
	}

    [TestClass]
    public class LookupTaxAreaServiceProxyTests
    {
		// Disabled due to invalid credentials. This test is for development use.
		[Ignore]
		[TestMethod]
        public void LookupTaxAreaForWellKnownAddresses()
        {
            // Some addresses snagged from ItWorks
            var addresses = @"
<rows>
    <row Address1=""1938 Cliffview Court"" City=""MURFREESBORO"" State=""TN"" PostalCode=""37128"" />
    <row Address1=""1204 S. 11th"" City=""Blackwell"" State=""OK"" PostalCode=""74631"" />
    <row Address1=""5849 Georgia Ave"" City=""NEW PORT RICHEY"" State=""FL"" PostalCode=""34653"" />
    <row Address1=""5325 E SR 64"" Address2="""" City=""Bradenton"" County=""Manatee"" State=""FL"" PostalCode=""34208"" />
    <row Address1=""2006  44th St.  SE"" Address2="""" City=""Grand Rapids"" County=""Kent"" State=""MI"" PostalCode=""49508"" />
    <row Address1=""2006 44th Street"" Address2=""Suite 2"" City=""Grand Rapids"" County=""Kent"" State=""MI"" PostalCode=""49508"" />
    <row Address1=""234234"" City=""ALLEGAN"" State=""MI"" PostalCode=""49010"" />
    <row Address1=""8617 River Preserve Drive"" City=""BRADENTON"" State=""FL"" PostalCode=""34212"" />
    <row Address1=""2006 44th"" City=""GRAND RAPIDS"" State=""MI"" PostalCode=""49546"" />
    <row Address1=""725 S.Adams St.L19"" City=""BIRMINGHAM"" State=""MI"" PostalCode=""48009"" />
    <row Address1=""404 NW Dorset Ct."" City=""PORT SAINT LUCIE"" State=""FL"" PostalCode=""34983"" />
    <row Address1=""264 Buckthorn Road"" City=""NEW CASTLE"" State=""CO"" PostalCode=""81647"" />
    <row Address1=""2140 W 4th St"" City=""PORT ANGELES"" State=""WA"" PostalCode=""98363"" />
    <row Address1=""8617 River Preserve Drive"" City=""BRADENTON"" State=""FL"" PostalCode=""34212"" />
</rows>".XmlToDynamic();

            foreach (var row in addresses.row)
            {
                var duck = Create.Mutation(Create.New<ITestAddress>(), a =>
                {
                    a.Address1 = row.Address1;
                    a.City = row.City;
                    a.State = row.State;
                    a.PostalCode = row.PostalCode;
                });
                var it = duck.ToTaxAddress();

                var taxArea = LookupTaxAreaServiceProxy.Lookup(duck.ToTaxAddress());
                Assert.IsNotNull(taxArea);
            }
        }
    }
}
