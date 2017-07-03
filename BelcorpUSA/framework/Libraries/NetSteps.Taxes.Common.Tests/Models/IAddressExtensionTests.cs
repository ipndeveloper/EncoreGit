using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Addresses.Common.Models;
using NetSteps.Encore.Core.Dto;
using NetSteps.Encore.Core.IoC;
using NetSteps.Encore.Core.Xml;
using NetSteps.Taxes.Common.Models;

namespace NetSteps.Taxes.Common.Tests.Models
{
    [DTO]
    public interface ITestAddress : IAddress
    {
    }

    [TestClass]
    public class IAddressExtensionTests
    {
        [TestMethod]
        public void ToTaxAddress_CopiesTaxAddressPropertiesFromIAddressInstance()
        {
            // 100 addresses snagged from ItWorks
            var addresses = @"<rows>
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
<row Address1=""2894 Oxford Road"" City=""NEW OXFORD"" State=""PA"" PostalCode=""17350"" />
<row Address1=""3320 Dumont Lk Dr"" City=""ALLEGAN"" State=""MI"" PostalCode=""49010"" />
<row Address1=""2006 44th St"" Address2="""" City=""Grand Rapids"" County=""Kent"" State=""MI"" PostalCode=""49508"" />
<row Address1=""4335 Lake Michigan Drive"" City=""WALKER"" State=""MI"" PostalCode=""49534"" />
<row Address1=""227 Cypress Trail Dr"" City=""GRAND RAPIDS"" State=""MI"" PostalCode=""49503"" />
<row Address1=""28 Edgewoods"" Address2="""" City=""Grand Rapids"" County=""Kent"" State=""MI"" PostalCode=""49546"" />
<row Address1=""421 Broad Bend Circle"" City=""CHESAPEAKE"" State=""VA"" PostalCode=""23320"" />
<row Address1=""618 Leonard NE"" City=""GRAND RAPIDS"" State=""MI"" PostalCode=""49503"" />
<row Address1=""8902 Stone Harbour Loop"" City=""BRADENTON"" State=""FL"" PostalCode=""34212"" />
<row Address1=""905 Ely St"" City=""ALLEGAN"" State=""MI"" PostalCode=""49010"" />
<row Address1=""6520 anchor loop"" Address2=""303"" City=""Bradenton"" County=""Manatee"" State=""FL"" PostalCode=""34212"" />
<row Address1=""9819 Portside terrace"" City=""BRADENTON"" State=""FL"" PostalCode=""34212"" />
<row Address1=""3641 Miller st"" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""437 Tiburon Dr."" City=""MYRTLE BEACH"" State=""SC"" PostalCode=""29588"" />
<row Address1=""812 Hackberry Rd"" City=""MONROEVILLE"" State=""PA"" PostalCode=""15146"" />
<row Address1=""3641 miller st"" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""3641 Miller St."" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""704 Randolph St."" City="""" State=""NC"" PostalCode=""27360"" />
<row Address1=""3641 Miller St."" Address2="""" City=""Wheat Ridge"" County=""Jefferson"" State=""CO"" PostalCode=""80033"" />
<row Address1=""121 Ward Street"" City=""Salem"" State=""VA"" PostalCode=""24153"" />
<row Address1=""300  Long Branch Trail"" City=""LEXINGTON"" State=""NC"" PostalCode=""27295"" />
<row Address1=""300 Longbranch Tr."" City=""LEXINGTON"" State=""NC"" PostalCode=""27295"" />
<row Address1=""39668 20th Ave."" City=""BLOOMINGDALE"" State=""MI"" PostalCode=""49026"" />
<row Address1=""1205 D North Saginaw BLVD"" Address2=""258"" City=""Saginaw"" State=""TX"" PostalCode=""76179"" />
<row Address1=""1403 Northampton"" City=""KALAMAZOO"" State=""MI"" PostalCode=""49006"" />
<row Address1=""21572 E. 40th Pl. S."" City=""BROKEN ARROW"" State=""OK"" PostalCode=""74014"" />
<row Address1=""1510 Ebenezer Rd"" City="""" State=""SC"" PostalCode=""29732"" />
<row Address1=""935 Gainder Rd"" City=""PLAINWELL"" State=""MI"" PostalCode=""49080"" />
<row Address1=""2050 Beavercreek Rd."" Address2=""310"" City=""Oregon City"" County=""Clackamas"" State=""OR"" PostalCode=""97045"" />
<row Address1=""2140 W 4th St"" Address2="""" City=""Port Angeles"" County=""Clallam"" State=""WA"" PostalCode=""98363"" />
<row Address1=""8404 82nd St SW"" Address2=""Apt 102"" City=""LAKEWOOD"" State=""WA"" PostalCode=""98498"" />
<row Address1=""977 Olin Meadows"" City="""" State=""MI"" PostalCode=""49345"" />
<row Address1=""13710 Deer Trail"" City=""CONROE"" State=""TX"" PostalCode=""77302"" />
<row Address1=""2140 W 4th St"" Address2="""" City=""Port Angeles"" County=""Clallam"" State=""WA"" PostalCode=""98363"" />
<row Address1=""7687 Torrey Court"" Address2="""" City=""Arvada"" State=""CO"" PostalCode=""80007"" />
<row Address1=""13710 Deer Trail"" City=""CONROE"" State=""TX"" PostalCode=""77302"" />
<row Address1=""5115 Weeping Cherry Dr"" City=""BROWNS SUMMIT"" State=""NC"" PostalCode=""27214"" />
<row Address1=""3641 Miller Street"" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""19 Farmingdale Dr."" City=""HAMLIN"" State=""NY"" PostalCode=""14464"" />
<row Address1=""24 Mahan St"" City=""TENAFLY"" State=""NJ"" PostalCode=""07670"" />
<row Address1=""P.O. Box 1454"" City=""ALLEN"" State=""TX"" PostalCode=""75013"" />
<row Address1=""106 Forest Hill Rd"" City=""Lexington"" State=""NC"" PostalCode=""27295"" />
<row Address1=""636 Village Green Circle"" City=""MURFREESBORO"" State=""TN"" PostalCode=""37128"" />
<row Address1=""703 Grant Ave"" City=""Loveland"" State=""CO"" PostalCode=""80537"" />
<row Address1=""6424 Sugar Ridge Drive"" City=""ROANOKE"" State=""VA"" PostalCode=""24018"" />
<row Address1=""PO Box 1552"" City=""Willis"" State=""TX"" PostalCode=""77378"" />
<row Address1=""P. O. Box 1297"" City=""Nederland"" State=""CO"" PostalCode=""80466-1297"" />
<row Address1=""6602 Camino Del Rey"" City="""" State=""CO"" PostalCode=""80817"" />
<row Address1=""6720 Antigua Drive 43"" City=""FORT COLLINS"" State=""CO"" PostalCode=""80525"" />
<row Address1=""2150 W. 15th St. Apt.302A"" City=""Loveland"" State=""CO"" PostalCode=""80538"" />
<row Address1=""1318  S  200  E"" City="""" State=""UT"" PostalCode=""84115"" />
<row Address1=""225 Clover Lance"" City=""FORT COLLINS"" State=""CO"" PostalCode=""80521"" />
<row Address1=""!8344 Woodland Ridge Drive"" Address2=""Apt. #22"" City=""SPRING LAKE"" State=""MI"" PostalCode=""49456"" />
<row Address1=""P.O. 5744"" City=""Clearwater"" State=""FL"" PostalCode=""33758"" />
<row Address1=""PO Box 34"" City=""MOUNT EATON"" State=""OH"" PostalCode=""44659"" />
<row Address1=""2766 E 3300 S"" City=""SALT LAKE CITY"" State=""UT"" PostalCode=""84109"" />
<row Address1=""2323 Foothill Drive"" City="""" State=""UT"" PostalCode=""84109"" />
<row Address1=""885 Red Sage Lane"" City=""SALT LAKE CITY"" State=""UT"" PostalCode=""84107"" />
<row Address1=""3641 Miller st"" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""1804 N. Galloway Ave"" City=""MESQUITE"" State=""TX"" PostalCode=""75149"" />
<row Address1=""17059 Sierra Dr."" City=""BIG RAPIDS"" State=""MI"" PostalCode=""49307"" />
<row Address1=""2143 Laurel Lane"" City=""ALLISON PARK"" State=""PA"" PostalCode=""15101"" />
<row Address1=""3641 Miller St"" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""907 Port Daniel"" City=""LEANDER"" State=""TX"" PostalCode=""78641"" />
<row Address1=""3641 Miller St."" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""3944 Mcrae St"" City="""" State=""MI"" PostalCode=""49418"" />
<row Address1=""812 Hackberry Rd"" City=""MONROEVILLE"" State=""PA"" PostalCode=""15146"" />
<row Address1=""2313 Burkhart Rd"" City=""LEXINGTON"" State=""NC"" PostalCode=""27292"" />
<row Address1=""3641 Miller st"" City=""WHEAT RIDGE"" State=""CO"" PostalCode=""80033"" />
<row Address1=""204 37th Ave 178"" City="""" State=""FL"" PostalCode=""33714"" />
<row Address1=""151 Stanley"" City=""ELK GROVE VILLAGE"" State=""IL"" PostalCode=""60007"" />
<row Address1=""201 Strasbourg Dr"" City=""West Monroe"" State=""LA"" PostalCode=""71291"" />
<row Address1=""4785 Circleshore Dr"" Address2=""Apt 201"" City=""KENTWOOD"" State=""MI"" PostalCode=""49508"" />
<row Address1=""10249 Burman St"" City=""HOUSTON"" State=""TX"" PostalCode=""77029"" />
<row Address1=""91 Hall Lane"" City=""Bush Creek"" State=""TN"" PostalCode=""38547"" />
<row Address1=""7687 Torrey Court"" City=""ARVADA"" State=""CO"" PostalCode=""80007"" />
<row Address1=""W7005 Ctho"" City=""MEDFORD"" State=""WI"" PostalCode=""54451"" />
<row Address1=""437 Tiburon Dr."" City=""MYRTLE BEACH"" State=""SC"" PostalCode=""29588"" />
<row Address1=""1401 Old Hickory BLVD."" City=""BRENTWOOD"" State=""TN"" PostalCode=""37027"" />
<row Address1=""1401 Old Hickory Blvd"" City=""Brentwood"" State=""TN"" PostalCode=""37027"" />
<row Address1=""Po Box 1188"" City=""STANTON"" State=""TX"" PostalCode=""79782"" />
<row Address1=""1014 Fitroy Cir"" City="""" State=""TN"" PostalCode=""37174"" />
<row Address1=""4385 Havenridge Place"" City=""cumming"" State=""GA"" PostalCode=""30041"" />
<row Address1=""437 Tiburon Dr."" City=""Myrtle Beach"" State=""SC"" PostalCode=""29588"" />
<row Address1=""437 Tiburon Dr"" City=""Myrtle Beach"" State=""SC"" PostalCode=""29588"" />
<row Address1=""289 Laurel Ln"" City=""GURLEY"" State=""AL"" PostalCode=""35748"" />
</rows>".XmlToDynamic();

            foreach (var row in addresses.row)
            {
                var duck = Create.Mutation(Create.New<ITestAddress>(), a => {
                    a.Address1 = row.Address1;
                    a.City = row.City;
                    a.State = row.State;
                    a.PostalCode = row.PostalCode;
                }); ;
                var it = duck.ToTaxAddress();
                
                Assert.AreEqual(row.Address1, it.StreetAddress1);
                Assert.AreEqual(row.City, it.City.Name);
                Assert.AreEqual(row.State, it.MainDivision.Name);
                Assert.AreEqual(row.PostalCode, it.PostalCode.Code);
            }            
        }
    }
}
