using NetSteps.SOD.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Xml.Linq;
using IoC = NetSteps.Encore.Core.IoC;
using System.Web;

namespace NetSteps.SOD.Common.Tests
{


    /// <summary>
    ///This is a test class for ExtensionsTest and is intended
    ///to contain all ExtensionsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ExtensionsTest
    {

        #region ToXElement Tests
        /// <summary>
        ///A test for ToXElement success case
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        public void ToXElementTest_SuccessCase()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            acct.Active = "true";
            acct.Address = "111 Country Rd";
            acct.City = "Exeter";
            acct.Country = "US";
            acct.DistID = "1008686";
            acct.Email = "jdskd837@test.com";
            acct.FirstName = "Jay";
            acct.ID = "100";
            acct.Language = "en-US";
            acct.LastName = "Gordon";
            acct.Password = "dakf32((8^%$37347djdh$#jhhglh*&%eyd8";
            acct.Phone = "801-555-8888";
            acct.Rank = "3";
            acct.State = "UT";
            acct.Type = "Distributor";
            acct.Website = "http://www.sell.com";
            acct.Zip = "84111";


            XElement expected = null;
            expected = new XElement("distributor",
                new XElement("firstname", Extensions_Accessor.SafeEncodeValue("Jay")),
                new XElement("lastname", Extensions_Accessor.SafeEncodeValue("Gordon")),
                new XElement("password", Extensions_Accessor.SafeEncodeValue(SuccessOnDemandApiUtils.PreparePasswordForApi("dakf32((8^%$37347djdh$#jhhglh*&%eyd8"))),
                new XElement("email", Extensions_Accessor.SafeEncodeValue("jdskd837@test.com")),
                new XElement("website", Extensions_Accessor.SafeEncodeValue("http://www.sell.com")),
                new XElement("phone", Extensions_Accessor.SafeEncodeValue("801-555-8888")),
                new XElement("address", Extensions_Accessor.SafeEncodeValue("111 Country Rd")),
                new XElement("city", Extensions_Accessor.SafeEncodeValue("Exeter")),
                new XElement("state", Extensions_Accessor.SafeEncodeValue("UT")),
                new XElement("zip", Extensions_Accessor.SafeEncodeValue("84111")),
                new XElement("country", Extensions_Accessor.SafeEncodeValue("US")),
                new XElement("language", Extensions_Accessor.SafeEncodeValue("en-US")),
                new XElement("active", Extensions_Accessor.SafeEncodeValue("true")),
                new XElement("type", Extensions_Accessor.SafeEncodeValue("Distributor")),
                new XElement("rank", Extensions_Accessor.SafeEncodeValue("3")),
                new XElement("ID", Extensions_Accessor.SafeEncodeValue("100"))
                );

            XElement actual;
            actual = Extensions_Accessor.ToXElement(acct);
            Assert.IsTrue(XNode.DeepEquals(expected, actual), "The expected XML does not match the actual XML");
        }

        /// <summary>
        ///A test for ToXElement failure case--If ToXElement is passed 
        ///a null acct value, should throw ArgumentNullException
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ToXElementTest_FailureCase_NullArgument()
        {
            IAccountInfo acct = null;
            XElement actual;

            actual = Extensions_Accessor.ToXElement(acct);
        }

        /// <summary>
        ///A test for ToXElement failure case--If ToXElement is passed 
        ///an acct with no firstname value, should throw ArgumentException
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void ToXElementTest_FailureCase_NoFirstNameValue()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            XElement actual;

            actual = Extensions_Accessor.ToXElement(acct);
        }

        /// <summary>
        ///A test for ToXElement failure case--If ToXElement is passed 
        ///an acct with no lastname value, should throw ArgumentException
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void ToXElementTest_FailureCase_NoLastNameValue()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            acct.FirstName = "John";
            XElement actual;

            actual = Extensions_Accessor.ToXElement(acct);
        }

        /// <summary>
        ///A test for ToXElement failure case--If ToXElement is passed 
        ///an acct with no password value, should throw ArgumentException
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void ToXElementTest_FailureCase_NoPasswordValue()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            acct.FirstName = "John";
            acct.LastName = "Johnson";
            XElement actual;

            actual = Extensions_Accessor.ToXElement(acct);
        }

        /// <summary>
        ///A test for ToXElement failure case--If ToXElement is passed 
        ///an acct with no email value, should throw ArgumentException
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        [ExpectedException(typeof(ArgumentException))]
        public void ToXElementTest_FailureCase_NoEmailValue()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            acct.FirstName = "John";
            acct.LastName = "Johnson";
            acct.Password = "asdfdsafkj;asdkfjasd;kfja;sfdsfsadjf;lkadsjf;sadjf;sk";
            XElement actual;

            actual = Extensions_Accessor.ToXElement(acct);
        }
        #endregion

        #region SafeEncodeValue Tests
        /// <summary>
        ///A test for SafeEncodeValue to ensure values containing no 
        ///special characters do NOT get enclosed with CDATA
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        public void SafeEncodeValueTest_RegularValuesDoNotGetEnclosedWithCDATA()
        {
            string value = "John Mary";
            XText expected = new XText("John Mary");
            XText actual;

            actual = Extensions_Accessor.SafeEncodeValue(value);

            Assert.IsTrue(expected.ToString() == actual.ToString(), "The given value was not encoded correctly.");
        }

        /// <summary>
        ///A test for SafeEncodeValue to ensure values
        ///containing ampersands get enclosed with CDATA
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        public void SafeEncodeValueTest_AmpersandGetsEnclosedWithCDATA()
        {
            string value = "John & Mary";
            XText expected = new XCData("John & Mary");
            XText actual;

            actual = Extensions_Accessor.SafeEncodeValue(value);
            
            Assert.IsTrue(expected.ToString() == actual.ToString(), "The given value contained an ampersand but was not properly enclosed by CDATA tags.");
        }

        /// <summary>
        ///A test for SafeEncodeValue to ensure values
        ///containing less-than signs get enclosed with CDATA
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        public void SafeEncodeValueTest_LessThanSignGetsEnclosedWithCDATA()
        {
            string value = "John < Mary";
            XText expected = new XCData("John < Mary");
            XText actual;

            actual = Extensions_Accessor.SafeEncodeValue(value);

            Assert.IsTrue(expected.ToString() == actual.ToString(), "The given value contained a less-than sign but was not properly enclosed by CDATA tags.");
        }

        /// <summary>
        ///A test for SafeEncodeValue to ensure values
        ///containing greater-than signs get enclosed with CDATA
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        public void SafeEncodeValueTest_GreaterThanSignGetsEnclosedWithCDATA()
        {
            string value = "John > Mary";
            XText expected = new XCData("John > Mary");
            XText actual;

            actual = Extensions_Accessor.SafeEncodeValue(value);

            Assert.IsTrue(expected.ToString() == actual.ToString(), "The given value contained a greater-than sign but was not properly enclosed by CDATA tags.");
        }

        /// <summary>
        ///A test for SafeEncodeValue to ensure values
        ///containing the percentage signs get enclosed with CDATA
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        public void SafeEncodeValueTest_PercentSignGetsEnclosedWithCDATA()
        {
            string value = "John % Mary";
            XText expected = new XCData("John % Mary");
            XText actual;

            actual = Extensions_Accessor.SafeEncodeValue(value);

            Assert.IsTrue(expected.ToString() == actual.ToString(), "The given value contained a percentage sign but was not properly enclosed by CDATA tags.");
        }

        /// <summary>
        ///A test for SafeEncodeValue to ensure null values are returned as XText objects instantiated
        ///with empty strings and avoids throwing a NullArgumentException from the XText constructor
        ///</summary>
        [TestMethod()]
        [DeploymentItem("NetSteps.SOD.Common.dll")]
        public void SafeEncodeValueTest_NullValuesGetReturnedAsEmptyStringXTextObjects()
        {
            string value = null;
            XText expected = new XText(String.Empty);
            XText actual = Extensions_Accessor.SafeEncodeValue(value);

            Assert.IsTrue(expected.ToString() == actual.ToString(), "The null value should have returned an XText object initialized as an empty string, but did not.");
        }
        #endregion

        #region EncodeAsPostBody Tests
        /// <summary>
        ///A test for EncodeAsPostBody--if passed a null value should throw an ArgumentNullException
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncodeAsPostBodyTest_NullValueThrowsException()
        {
            IAccountInfo acct = null;
            string actual = Extensions.EncodeAsPostBody(acct);
        }

        /// <summary>
        ///A test for EncodeAsPostBody--if passed a null value should throw an ArgumentNullException (overloaded version)
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncodeAsPostBodyTest_NullValueThrowsExceptionOnOverloadedVersion()
        {
            IAccountInfo acct = null;
            string actual = Extensions.EncodeAsPostBody(acct, true);
        }

        /// <summary>
        ///A test for EncodeAsPostBody--Post body to be used to create account
        ///</summary>
        [TestMethod()]
        public void EncodeAsPostBodyTest_ForCreate()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            acct.Email = "jdskd837@test.com";
            acct.FirstName = "Jay";
            acct.LastName = "Gordon";
            acct.Password = "dakf32((8^%$37347djdh$#jhhglh*&%eyd8";

            bool forCreate = true;
            string expected = String.Concat("xmldata=", Extensions_Accessor.ToXElement(acct).ToString());

            string actual = Extensions.EncodeAsPostBody(acct, forCreate);

            Assert.AreEqual<string>(expected, actual, "The IAccountInfo was not properly encoded as post body XML data.");
        }

        /// <summary>
        ///A test for EncodeAsPostBody--Post body to be used to update account, includes DistID
        ///</summary>
        [TestMethod()]
        public void EncodeAsPostBodyTest_ForUpdateIncludesDistID()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            acct.DistID = "10101010";
            acct.Email = "jdskd837@test.com";
            acct.FirstName = "Jay";
            acct.LastName = "Gordon";
            acct.Password = "dakf32((8^%$37347djdh$#jhhglh*&%eyd8";

            bool forCreate = false;
            var xml = Extensions_Accessor.ToXElement(acct);
            xml.AddFirst(new XElement("DistID", acct.DistID));
            string expected = String.Concat("xmldata=", xml.ToString());

            string actual = Extensions.EncodeAsPostBody(acct, forCreate);

            Assert.AreEqual<string>(expected, actual, "The IAccountInfo was not properly encoded as post body XML data.");
        }

        /// <summary>
        ///A test for EncodeAsPostBody--Post body to be used to update account, doesn't include DistID.
        ///When encoding for update (i.e. forCreate==false), a DistID must be included. In this test it's not,
        ///so we should get an ArgumentException.
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EncodeAsPostBodyTest_ForUpdateDoesNotIncludeDistID()
        {
            IAccountInfo acct = IoC.Create.New<IAccountInfo>();
            acct.Email = "jdskd837@test.com";
            acct.FirstName = "Jay";
            acct.LastName = "Gordon";
            acct.Password = "dakf32((8^%$37347djdh$#jhhglh*&%eyd8";

            bool forCreate = false;
            var xml = Extensions_Accessor.ToXElement(acct);
            xml.AddFirst(new XElement("DistID", acct.DistID));
            string expected = String.Concat("xmldata=", xml.ToString());

            string actual = Extensions.EncodeAsPostBody(acct, forCreate);
        }
        #endregion

        #region EncodeAsFormBody Tests
        /// <summary>
        ///A test for EncodeAsFormBody--should throw ArgumentNullException if called on a null value
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void EncodeAsFormBodyTest_NullValueShouldThrowException()
        {
            ILoginInfo login = null;
            string actual = Extensions.EncodeAsFormBody(login);
        }

        /// <summary>
        ///A test for EncodeAsFormBody--should throw ArgumentException if the login info doesn't include an email address
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EncodeAsFormBodyTest_NoEmailAddressShouldThrowException()
        {
            ILoginInfo login = IoC.Create.New<ILoginInfo>();
            login.Password = "dkfjadsklfhsdfhkdjsfghkdjsgfksdsdfkghsd";
            string actual = Extensions.EncodeAsFormBody(login);
        }

        /// <summary>
        ///A test for EncodeAsFormBody--should throw ArgumentException if the login info doesn't include a password
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void EncodeAsFormBodyTest_NoPasswordShouldThrowException()
        {
            ILoginInfo login = IoC.Create.New<ILoginInfo>();
            login.Email = "test@test.com";
            string actual = Extensions.EncodeAsFormBody(login);
        }

        /// <summary>
        ///A test for EncodeAsFormBody
        ///</summary>
        [TestMethod()]
        public void EncodeAsFormBodyTest_SuccessCase()
        {
            ILoginInfo login = IoC.Create.New<ILoginInfo>();
            login.Email = "test@test.com";
            login.Password = "dsafdsjhgdjkdjkasddsskafg";

            string expected = String.Concat("email_address=", HttpUtility.UrlEncode(login.Email)
                , "&password=", HttpUtility.UrlEncode(SuccessOnDemandApiUtils.PreparePasswordForApi(login.Password)));

            string actual = Extensions.EncodeAsFormBody(login);
            Assert.AreEqual<string>(expected, actual, "The ILoginInfo was not properly encoded into form body data.");
        }
        #endregion

        #region Unused Test Class Items
        //private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        //public TestContext TestContext
        //{
        //    get
        //    {
        //        return testContextInstance;
        //    }
        //    set
        //    {
        //        testContextInstance = value;
        //    }
        //}
        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion
        #endregion

    }
}
