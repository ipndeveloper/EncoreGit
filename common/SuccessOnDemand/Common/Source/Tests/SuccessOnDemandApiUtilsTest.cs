using NetSteps.SOD.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace NetSteps.SOD.Common.Tests
{
    
    
    /// <summary>
    ///This is a test class for SuccessOnDemandApiUtilsTest and is intended
    ///to contain all SuccessOnDemandApiUtilsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SuccessOnDemandApiUtilsTest
    {
        /// <summary>
        ///A test for PreparePasswordForApi--ensures a null password causes ArgumentNullException to be thrown
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException), "The password parameter was null but no ArgumentNullException was thrown.")]
        public void PreparePasswordForApiTest_PasswordCannotBeNull()
        {
            string password = null;
            string actual = SuccessOnDemandApiUtils.PreparePasswordForApi(password);
        }

        /// <summary>
        ///A test for PreparePasswordForApi--ensures a password shorter than 1 character causes ArgumentException to be thrown
        ///</summary>
        [TestMethod()]
        [ExpectedException(typeof(ArgumentException), "The password parameter's length was less than 1 but no ArgumentException was thrown.")]
        public void PreparePasswordForApiTest_PasswordLengthCannotBeLessThanOne()
        {
            string password = "";
            string actual = SuccessOnDemandApiUtils.PreparePasswordForApi(password);
        }

        /// <summary>
        ///A test for PreparePasswordForApi--ensures that the returned password consists of
        ///the first 20 alpha numeric characters in the string that was passed to the method.
        ///</summary>
        [TestMethod()]
        public void PreparePasswordForApiTest_SuccessCase()
        {
            string password = "hf@!~lkjsdahf7f%^t38&sfdeejhcxeyu^#gfdsaf&+_dhs$?dsfsd3423243";
            string expected = "hflkjsdahf7ft38sfdee";
            string actual = SuccessOnDemandApiUtils.PreparePasswordForApi(password);

            Assert.AreEqual<string>(expected, actual, "The expected password value was not equal to the actual password value.");
        }

        #region Unused Test Class Items
        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

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
