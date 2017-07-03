using System;
using System.Text;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Mail;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestMasterHelpProvider;
using TestMasterHelpProvider.Logging;
using TestMasterHelpProvider.Logging.Sql;
using WatiN.Core;

namespace NetSteps.Testing.Integration
{
    using NetSteps.Testing.Integration.PWS;
    using NetSteps.Testing.Integration.PWS.Enroll;

    [TestClass]
    public class BaseTestClass
    {
        #region Fields

        protected static string _urlPWS;
        protected static string _urlGMP;
        protected static string _urlDWS;
        protected static string _gmpUser;
        protected static string _gmpPassword;
        protected static string _dwsUser;
        protected static string _dwsPassword;
        protected static string _pwsUser;
        protected static string _pwsPassword;
        protected static string _urlCWS;
        private const string _errLogQuery = "SELECT * FROM dbo.ErrorLogs where dbo.ErrorLogs.ErrorLogID = ";
        private readonly string _imagePath = ConfigurationManager.AppSettings["ImagePath"];
        private TestContext _testContext;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the test context. This is used by the Unit Test framework and is different from WatiNContext.
        /// </summary>
        public TestContext TestContext
        {
            get
            {
                return this._testContext;
            }
            set
            {
                this._testContext = value;
            }
        }

        #endregion

        [TestInitialize]
        public void BaseTestInitiatilize()
        {
            this.KillAllBrowsers();

            Utils.LogFileName = string.Format("{0}_LogFile.txt", DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"));

            if (Util.LogFileName == null)
            {
                // Image path where image needs to be captured and log exception.
                if (!Directory.Exists(_imagePath))
                {
                    Directory.CreateDirectory(_imagePath);
                }

                string testResultsFolder = _imagePath + "TestRunReport_" + DateTime.Now.ToString("yyyyMMdd") + "\\";

                if (!Directory.Exists(testResultsFolder))
                {
                    Directory.CreateDirectory(testResultsFolder);
                }

                ConfigurationManager.AppSettings.Set("ImagePath", testResultsFolder);

                Util.LogFileName = ConfigurationManager.AppSettings["ImagePath"] + string.Format("{0}_LogFile.html", DateTime.Now.ToString("yyyy-MM-dd_HH.mm.ss"));
                Util.HTMLReporting(Util.LogFileName);
            }

            if (Utils.Logger.LogWriter is SqlLogWriter)
            {
                Test test = new Test(Client.Encore, this.GetType().FullName, this.TestContext.TestName);
                TestRun.Test = test;
            }

            Utils.Logger.WriteTestStartMessage(string.Format("Started test {0}", this.TestContext.TestName));
            //Util.StartTest(this.TestContext.TestName);
            Util.StartTest(this.TestContext.FullyQualifiedTestClassName, this.TestContext.TestName);
        }

        [TestCleanup]
        public void BaseTestCleanup()
        {
            string separater = "***************************************************************************";

            //Util.CloseBrowser();
            KillAllBrowsers();

            Util.ClearCookies();

            Utils.Logger.WriteTestEndMessage(string.Concat(string.Format("Ended test {0}{1}", this.TestContext.TestName, Environment.NewLine), Environment.NewLine, separater, Environment.NewLine));

            if (!Util.EndTest(this.TestContext.TestName, TestContext.CurrentTestOutcome) && (TestContext.CurrentTestOutcome == UnitTestOutcome.Passed))
            {
                 
                Assert.Fail("One or More Assertions failed");
            }
        }

        [ClassInitialize]
        public static void BaseClassInit(TestContext textContext)
        {

        }

        [ClassCleanup]
        public static void BaseClassCleanup()
        {
            Util.endOfLog();
            if (Utils.Logger.LogWriter is SqlLogWriter)
            {
                SqlLogWriter logWriter = (SqlLogWriter)Utils.Logger.LogWriter;
                logWriter.TestRun.MarkEndTime();

                TestRunResults testRunResults = new TestRunResults(logWriter.TestRun);

                logWriter.TestRun.EmailMessage.MessageBody = string.Format("{0}{1}{2}", logWriter.TestRun.EmailMessage.MessageBody, Environment.NewLine, testRunResults);
                logWriter.TestRun.EmailMessage.MessageSubject = string.Format("{0}, Test Run {1} [{2}]", testRunResults.ClientName, testRunResults.TestRunId, testRunResults.StartTime);
                logWriter.TestRun.EmailMessage.IsBodyHtml = true;
                logWriter.TestRun.EmailMessage.Send();
            }
        }

        /// <summary>
        /// Handle the exception.
        /// </summary>
        /// <param name="ex">Exception thrown from the test.</param>
        public void HandleException(Exception ex, bool fatal = true)
        {
            string getMethodNameAndLineNumber = string.Empty;

            // Log exception.                     
            TestContext.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "]: " + ex.Message);

            // Log stack trace if exists.
            if (!string.IsNullOrEmpty(ex.StackTrace))
            {
                TestContext.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "]: " + ex.StackTrace);
            }

            StackTrace st = new StackTrace(ex, true);
            StackFrame[] frames = st.GetFrames();

            foreach (StackFrame frame in frames)
            {
                if (frame.GetFileLineNumber() > 0)
                    getMethodNameAndLineNumber += string.Format("{0}(line#{1}); ", frame.GetMethod().Name, frame.GetFileLineNumber());
            }

            string message = string.Format(ex.Message + ": {0}", getMethodNameAndLineNumber);

            if (fatal)
            {
                Assert.Fail(message);
            }
            else
            {
                try
                {
                    TestContext.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "]: " + message);
                    string imagePath = string.Concat(ConfigurationManager.AppSettings["ImagePath"], TestContext.TestName + DateTime.Now.ToString("MMddyyyyHHmmss"), ".jpeg");

                    // Capture the screenshot and log the path in the TRX file.  
                    if (Util.browserType.ToString().Contains("IE"))
                    {
                        Util.Browser.CaptureWebPageToFile(imagePath);
                    }
                    else if (Util.browserType.ToString().Contains("Firefox"))
                    {
                        Screenshot.SaveScreenshot(Util.Browser, imagePath);
                    }

                    TestContext.WriteLine(string.Format("Screenshot captured at :{0}", imagePath));
                    Util.LogFail(message, imagePath);
                }

                catch
                {
                    Util.LogFail(message, "Unable to capture the image");
                    TestContext.WriteLine("Unable to capture the image");
                }
            }
            //this.AssertIsTrue(false, string.Format(ex.Message + ":- {0}", getMethodNameAndLineNumber));

            // Get the error logs from DB and log them to TRX file.
            //Util.GetErrorLogs();
        }

        /// <summary>
        /// Log the error message in the TRX file.
        /// </summary>
        /// <param name="message">Error message to be logged.</param>
        public void LogErrMsgInTrx(string message)
        {
            try
            {
                TestContext.WriteLine("[" + DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "]: " + message);
                string imagePath = string.Concat(ConfigurationManager.AppSettings["ImagePath"], TestContext.TestName + DateTime.Now.ToString("MMddyyyyHHmmss"), ".jpeg");

                // Capture the screenshot and log the path in the TRX file.  
                if (Util.browserType.ToString().Contains("IE"))
                {
                    Util.Browser.CaptureWebPageToFile(imagePath);
                }
                else if (Util.browserType.ToString().Contains("Firefox"))
                {
                    Screenshot.SaveScreenshot(Util.Browser, imagePath);
                }

                TestContext.WriteLine(string.Format("Screenshot captured at :{0}", imagePath));
                Util.LogFail(message, imagePath);
            }

            catch
            {
                Util.LogFail(message, "Unable to capture the image");
                TestContext.WriteLine("Unable to capture the image");
            }
        }

        private static string GetMessage(string msg, int[] defects)
        {
            StringBuilder def = new StringBuilder(msg);
            if (defects != null)
            {
                bool first = true;
                foreach (int defect in defects)
                {
                    if (first)
                    {
                        if (msg != null)
                            def.Append(", ");
                        def.Append("Defects: ");
                        first = false;
                    }
                    else
                        def.Append(", ");
                    def.Append(defect.ToString());
                }
            }
            return def.ToString();
        }

        public bool AssertIsTrue(bool condition, string logMessage, bool fatal = true, params int[] defects)
        {
            bool result = false;
            try
            {
                Assert.IsTrue(condition, GetMessage(logMessage, defects));
                Util.LogPass(logMessage);
                result = true;
            }
            catch (AssertFailedException ex)
            {
                HandleException(ex, fatal);
            }
            return result;
        }

        public bool AssertIsFalse(bool condition, string logMessage, bool fatal = true, params int[] defects)
        {
            bool result = false;
            try
            {
                Assert.IsFalse(condition, GetMessage(logMessage, defects));
                Util.LogPass(logMessage);
                result = true;
            }
            catch (AssertFailedException ex)
            {
                HandleException(ex, fatal);
            }
            return result;
        }

        /// <summary>
        /// Kill all the existing browsers.
        /// </summary>
        private void KillAllBrowsers()
        {
            switch (ConfigurationManager.AppSettings["Browser"])
            {
                case "IE":
                    Util.KillAllProcess("iexplore");
                    Util.browserType = BrowserType.IE;
                    break;
                case "Firefox":
                    Util.KillAllProcess("firefox");
                    Util.browserType = BrowserType.Firefox;
                    break;
            }
        }

        //protected void CheckEnvironment(string url)
        //{
        //    if (!(url.Contains("qa") || url.Contains("stage") || url.Contains("staging") || url.Contains("promotions") || url.Contains("netstepscorp") || url.Contains(".dev.")))
        //        throw new ArgumentException("Invalid environment for Configuration");
        //}

        protected PWS_Home_Page PWSEnrollRetailCustomer(PWS_Base_Page page, RetailCustomer customer, bool logout = false)
        {
            var home = page.GlobalHeader().ClickLogin().ClickSignUp().EnterNewUser(customer).ClickSignUp<PWS_Home_Page>();
            if (logout)
                home = page.GlobalHeader().UserMenu.ClickLogout<PWS_Home_Page>();
            return home;
        }

        protected PWS_Base_Page PWSEnrollRetailCustomer<TPage>(PWS_Base_Page page, RetailCustomer customer, bool logout = false) where TPage : PWS_Base_Page, new()
        {
            page = page.GlobalHeader().ClickLogin<PWS_Enroll_LoginSignup_Page>().EnterNewUser(customer).ClickSignUp<TPage>();
            if (logout)
                page = page.GlobalHeader().UserMenu.ClickLogout<PWS_Home_Page>();
            return page;
        }

        protected PWS_Base_Page PWSTestLogin(PWS_Base_Page page, RetailCustomer customer, bool logout = false)
        {
            var home = page.GlobalHeader().ClickLogin().ClickSignUp().EnterExistingUser(customer).ClickLogin<PWS_Home_Page>();
            AssertIsTrue(page.GlobalHeader().ValidateLogin(customer), "Log in from page");

            home = home.GlobalHeader().UserMenu.ClickLogout<PWS_Home_Page>().GlobalHeader().ClickLogin().EnterExistingUser(customer).ClickLogin<PWS_Home_Page>();
            AssertIsTrue(page.GlobalHeader().ValidateLogin(customer), "Log in from header");

            if (logout)
                home = page.GlobalHeader().UserMenu.ClickLogout<PWS_Home_Page>();
            return home;
        }

        protected PWS_Base_Page PWSTestLogin<TPage>(PWS_Base_Page page, RetailCustomer customer, bool logout = false) where TPage : PWS_Base_Page, new()
        {
            page = page.GlobalHeader().ClickLogin<PWS_Enroll_LoginSignup_Page>().EnterExistingUser(customer).ClickLogin<TPage>();
            AssertIsTrue(page.GlobalHeader().ValidateLogin(customer), "Log in from page");

            if (logout)
                page = page.GlobalHeader().UserMenu.ClickLogout<PWS_Home_Page>();
            return page;
        }
    }
}
