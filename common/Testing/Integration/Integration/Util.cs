using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Mail;
using TestMasterHelpProvider;
using WatiN.Core;
using WatiN.Core.Extras;
using WatiN.Core.DialogHandlers;
using System.Threading;
using System.Text.RegularExpressions;
using SHDocVw;
using WatiN.Core.Constraints;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetSteps.Testing.Integration
{
    /// <summary>
    /// Class dealing with all helper methods.
    /// </summary>
    public class Util
    {
        #region Variables
        public static BrowserType browserType;
        public static string strKeyUp = "keyup";
        public static string strShow = "show";
        public static string strSlideDown = "slideDown";
        // Some pages use knockout.js to track field changes. If you fields have a 'data-bind=...' attribute then the field is set up to use knockout.js
        // Knockout.js triggers off the change event. No other events are needed.
        // References: http://stackoverflow.com/questions/3712825/unable-to-fire-jquery-change-event-on-selectlist-from-watin
        // References: http://www.sitepoint.com/beginners-guide-to-knockoutjs-part-1/
        // References: http://www.sitepoint.com/beginners-guide-to-knockoutjs-part-2/
        // References: http://learn.knockoutjs.com/#/?tutorial=intro
        // The "change" event can be run at any time, but experimentation shows the "change" event has to be run on each field that changes.
        // Use [fieldname].CustomRunScript(Util.strChange);
        public static string strChange = "change";
        public static string strAttributeSRC = "src";
        public static string attributeValue = "value";

        public static string connectionString = "EncoreQA";
        public static string defaultPWSUrl = "distributor.enqoreqa.com";
        private const string ErrLogQuery = "SELECT * FROM dbo.ErrorLogs where dbo.ErrorLogs.ErrorLogID = ";
        internal const string DwsPassword = "sunshine";
        private static int countBVT = 0;
        internal static int UtcTimeDiff = -6;
        private static StreamWriter writer = null;
        private static Random _random = new Random();

        private static int _logLevel = int.Parse(System.Configuration.ConfigurationManager.AppSettings["LogLevel"]);

        private static char[] _characters = new char[61]
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
                '0', '1', '2', '3', '4', '5', '6', '8', '9'
            };

        /// <summary>
        /// Browser instance for child browsers.
        /// </summary>
        public static Browser newBrowser;

        #endregion Variables

        #region Properties

        // Summary:
        //     Gets or sets the log file name for the current run.
        public static string LogFileName { get; set; }

        // Summary:
        //     Gets or sets the testFlag to fail a test case when anyone of its assertion failed.
        public static bool testFlag { get; set; }

        // Summary:
        //     Gets or sets the testBuild to send a mail when there is a major blocking issue.
        public static bool? testBuild { get; set; }

        public static readonly int CustomExistsTimeout = 30;

        #endregion Properties

        #region Methods

        #region WatiN

        public static int LogLevel()
        {
            return _logLevel;
        }

        public static string GetEmail()
        {
            return Util.GenerateRandomString() + "@abc.com";
        }

        public static int GetRandom(int min, int max)
        {
            return _random.Next(min, max);
        }

        public static string GetCreditCard(CreditCard.ID cardType = CreditCard.ID.Discover, bool valid = true)
        {
            string cc;
            switch (cardType)
            {
                case CreditCard.ID.MasterCard:
                    cc = string.Format("5228{0:D11}", _random.Next(0, int.MaxValue));
                    break;
                case CreditCard.ID.Visa:
                    cc = string.Format("4117{0:D11}", _random.Next(0, int.MaxValue));
                    break;
                case CreditCard.ID.AmericanExpress:
                    cc = string.Format("3745{0:D11}", _random.Next(0, int.MaxValue));
                    break;
                case CreditCard.ID.Maestro:
                    cc = string.Format("5641{0:D11}", _random.Next(0, int.MaxValue));
                    break;
                default: // Discover
                    cc = string.Format("6011{0:D11}", _random.Next(0, int.MaxValue));
                    break;

            }
            int sum = 0;
            for (int i = 0; i < 15; i++)
            {
                int digit = int.Parse(cc[i].ToString());
                if (i % 2 == 0)
                {
                    digit = 2 * digit;
                    if (digit > 9)
                    {
                        string num = digit.ToString();
                        digit = 0;
                        for (int j = 0; j < 2; j++)
                        {
                            digit += int.Parse(num[j].ToString());
                        }
                    }
                }
                sum += digit;
            }
            int checkdigit = (sum * 9) % 10;
            if (!valid)
                checkdigit++;
            return cc + checkdigit.ToString();
        }

        public static bool ValidateUrl(string url, HttpStatusCode responseCode = HttpStatusCode.OK)
        {
            bool result;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.01; Windows NT 5.0)";
            request.Method = "HEAD";
            request.ContentType = "text/html";
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.CookieContainer = new CookieContainer();
            request.CachePolicy = new RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException exception)
            {
                response = (HttpWebResponse)exception.Response;
            }
            finally
            {
                if (response == null)
                    result = false;
                else
                {
                    result = (response.StatusCode == responseCode);
                    response.Close();
                }
            }
            return result;
        }

        public static string GenerateRandomCharacterString(int length = 6, string prefix = null, int min = 0, int max = 60)
        {
            System.Text.StringBuilder stg = new System.Text.StringBuilder();
            if(!string.IsNullOrEmpty(prefix))
                stg.Append(prefix);
            for (int i = 0; i < length; i++)
                stg.Append(_characters[_random.Next(min, max)]);
            return stg.ToString();
        }

        /// <summary>
        /// Generate random string with Date time and GUID.
        /// </summary>
        /// <returns>Random string.</returns>
        public static string GenerateRandomString(int length = 4)
        {
            System.Threading.Thread.Sleep(100);
            return DateTime.Now.ToString("MMddyy") + System.Guid.NewGuid().ToString().Substring(0, length);
        }

        public static string GenerateRandomNumberString(int min, int max)
        {
            System.Threading.Thread.Sleep(100);
            return _random.Next(min, max).ToString();
        }

        /// <summary>
        /// Select enroller from suggested results.
        /// </summary>
        /// <param name="enrollerName">Enroller name.</param>
        [Obsolete("Use 'SearchSuggestinBox(string)'")]
        public static void CustomSelectEnrollerFromSuggestion(string enrollerName, int? timeout = null)
        {
            Util.Browser.GetElement<Para>(new Param("text", AttributeName.ID.ClassName).And(new Param(enrollerName, AttributeName.ID.InnerText))).CustomClick(timeout);
        }

        public static bool CertPageExists()
        {
            var diagnoseButton = Util.Browser.GetElement<Para>(new Param("diagnose"));
            bool exists = false;

            if (diagnoseButton.Exists)
                exists = true;

            return exists;
        }

        public static TPage ClickCertPageOverride<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            var overridelink = Util.Browser.GetElement<Para>(new Param("overridelink"));

            timeout = overridelink.CustomClick(timeout);

            return Util.GetPage<TPage>(timeout, pageRequired);
        }

        #region Browser

        /// <summary>
        /// Validate document loads.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// 
        /// <returns></returns>
        public static bool ValidateDocumentLoad(string url, int? timeout = null)
        {
            bool documentLoaded = false;

            if (!timeout.HasValue)
            {
                timeout = Settings.AttachToBrowserTimeOut;
            }

            while (timeout > 0)
            {
                ShellWindows shellWindows = new ShellWindowsClass();

                foreach (InternetExplorer ie in shellWindows)
                {
                    if (ie.LocationURL.Contains(url))
                    {
                        do
                        {
                            if (ie.Busy)
                            {
                                Thread.Sleep(1000);
                                timeout--;
                            }
                            else
                            {
                                documentLoaded = true;
                                break;
                            }
                        } while (timeout > 0);
                        ie.Quit();
                    }
                }
                if (!documentLoaded)
                {
                    if (--timeout < 0)
                        throw new TimeoutException("Document load timeout");
                    Thread.Sleep(1000);
                    timeout--;
                }
            }
            return documentLoaded;
        }

        /// <summary>
        /// Initialize browser.
        /// </summary>
        /// <param name="Url">Browse the URL.</param>
        public static TPage InitBrowser<TPage>(string Url, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            dynamic regKeys;
            string version = string.Empty;

            switch (ConfigurationManager.AppSettings["Browser"])
            {
                case "IE":
                    Browser = new IE();
                    regKeys = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Internet Explorer");
                    if (regKeys != null)
                    {
                        version = (string)regKeys.GetValue("Version");
                        version = version.Substring(0, version.IndexOf('.'));
                    }
                    break;

                case "Firefox":
                    Browser = new FireFox();
                    regKeys = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Mozilla\Mozilla Firefox");
                    if (regKeys != null)
                    {
                        version = (string)regKeys.GetValue("CurrentVersion");
                    }
                    break;
            }

            //Browser.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.Maximize);
            Browser.ShowWindow(WatiN.Core.Native.Windows.NativeMethods.WindowShowStyle.ShowNormal);
            Browser.BringToFront();
            Browser.GoTo(Url);
            if(!timeout.HasValue)
                timeout = timeout = Settings.WaitForCompleteTimeOut;
            Browser.CustomWaitForComplete(timeout);
            LogDoneMessage(string.Format("Launched '{0}' portal using '{1}:{2}' browser.", Url, browserType, version));
            return GetPage<TPage>(timeout, pageRequired);
        }

        /// <summary>
        /// Gets or sets the current context browser.
        /// </summary>
        public static Browser Browser
        {
            get
            {
                return WatiNContext.GetContext().Browser;
            }
            set
            {
                WatiNContext.GetContext().Browser = value;
            }
        }

        /// <summary>
        /// Close existing browser and dispose it.
        /// </summary>
        public static void CloseBrowser()
        {
            string url = Browser.Url;
            Browser.Close();
            if (!browserType.ToString().Contains("Firefox"))
            {
                Browser.Dispose();
            }
            System.Threading.Thread.Sleep(500);
            LogDoneMessage(string.Format("Close {0}", url));
        }

        /// <summary>
        /// Clears the Cookies.
        /// </summary>
        public static void ClearCookies()
        {
            switch (browserType)
            {
                case BrowserType.IE:

                    var dir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Cookies));
                    FileInfo[] filesindir = dir.GetFiles();
                    foreach (FileInfo f in filesindir)
                    {
                        if (f.Extension == ".txt")
                        {
                            f.Delete();
                        }
                    }
                    break;

                case BrowserType.Firefox:

                    var ffdir = new DirectoryInfo(string.Concat("C:\\Users\\", Environment.UserName, "\\AppData\\Local\\Mozilla\\Firefox\\Profiles"));
                    DirectoryInfo[] filesdirs = ffdir.GetDirectories("*.default");
                    filesdirs[0].Delete(true);

                    break;
            }
            System.Threading.Thread.Sleep(3000); // Allows cookies to clear
        }

        /// <summary>
        /// Close child browser instances.
        /// </summary>
        public static void CloseChildBrowsers()
        {
            if (newBrowser == null)
            {
                return;
            }

            Browser = newBrowser;

            string processName = string.Empty;

            switch (ConfigurationManager.AppSettings["Browser"])
            {
                case "IE":
                    processName = "iexplore";
                    break;
                case "Firefox":
                    processName = "firefox";
                    break;
                default:
                    processName = "iexplore";
                    break;
            }

            if (processName == "iexplore")
            {
                foreach (SHDocVw.InternetExplorer ieInst in new SHDocVw.ShellWindowsClass())
                {
                    string url = ieInst.LocationURL;
                    if (url.Contains("ReportViewer.aspx"))
                    {
                        ieInst.Quit();
                    }
                }
            }
            else if (processName == "firefox")
            {
                foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName(processName))
                {
                    try
                    {
                        if (proc.Handle != Browser.hWnd && proc.MainWindowTitle != string.Empty)
                        {
                            proc.CloseMainWindow();
                            proc.Dispose();
                            proc.Kill();
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                    catch
                    {
                    }
                }
            }

            System.Threading.Thread.Sleep(5000);
        }

        public static int? AttachBrowser(string url, int? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = Settings.AttachToBrowserTimeOut;
            newBrowser = Browser;
            var start = DateTime.Now;
            Browser = Browser.AttachTo(newBrowser.GetType(), Find.ByUrl(new Regex(url)), (int)timeout);
            var ts = DateTime.Now - start;
            timeout -= (int)ts.Seconds;
            LogDoneMessage(string.Format("Attach to {0}", Browser.Url));
            return timeout;
        }

        public static TPage AttachBrowser<TPage>(string url, int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            newBrowser = Browser;
            Browser = Browser.AttachTo(newBrowser.GetType(), Find.ByUrl(new Regex(url)));
            timeout = Browser.CustomWaitForComplete(timeout);
            LogDoneMessage(string.Format("Attach to {0}", Browser.Url));
            return GetPage<TPage>(timeout, pageRequired);
        }

        /// <summary>
        /// Attach to the new browser child
        /// Pass in the URL and ignore any queries in the string ('?' marks)
        /// </summary>
        /// <param name="url">string URL, boolean IgnoreQuery.</param>
        public static void AttachBrowser(string url, bool blIgnoreQuery, int? timeout = null)
        {
            if (!timeout.HasValue)
                timeout = Settings.AttachToBrowserTimeOut;
            newBrowser = Browser;
            DateTime start = DateTime.Now;
            Browser = Browser.AttachTo(newBrowser.GetType(), Find.ByUrl(url, blIgnoreQuery), (int) timeout);
            timeout = timeout - (DateTime.Now - start).Seconds;
            Util.Browser.CustomWaitForComplete(timeout);
        }


        #endregion Browser

        /// <summary>
        /// Generic page constructor
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static TPage GetPage<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new()
        {
            int wait = 0;
            if (!timeout.HasValue)
                timeout = Settings.WaitForCompleteTimeOut;
            TPage page = Page.CreatePage<TPage>(Util.Browser);
            while (!page.IsPageRendered())
            {
                if (--timeout < 0)
                    break;
                Thread.Sleep(1000);
                wait++;
            }
            if (timeout < 0)
            {
                string msg = string.Format("Unable to render page {0}", typeof(TPage));
                if (pageRequired)
                    throw new TimeoutException(msg);
            }
            else
            {
                if (Util.LogLevel() > 0)
                {
                    LogDoneMessage(string.Format("GetPage {0} seconds", wait));
                }
                LogPass(typeof(TPage).ToString());
            }
            return page;
        }

        #endregion WatiN

        #region Windows

        /// <summary>
        /// Handle the dialog box.
        /// </summary>
        /// <param name="element">Element to be clicked with no wait.</param>
        public static void HandleDailog(Element element, int? timeout = null)
        {
            // Create a handler.
            ReturnDialogHandlerIe9 myHandler = new ReturnDialogHandlerIe9();
            Util.Browser.AddDialogHandler(myHandler);

            // Click element with no wait.
            element.CustomClickNoWait();
            System.Threading.Thread.Sleep(2000);

            // Wait until pop-up appears and click ok and remove handler.
            myHandler.WaitUntilExists();
            myHandler.OKButton.Click();
            Util.Browser.RemoveDialogHandler(myHandler);
            Util.Browser.CustomWaitForComplete(timeout);
        }

        public static void HandleConfirmDialog(Element element, bool ok = true)
        {
            var handler = new ConfirmDialogHandler();
            using (new WatiN.Core.DialogHandlers.UseDialogOnce(Util.Browser.DialogWatcher, handler))
            {
                element.ClickNoWait();
                handler.WaitUntilExists();
                if(ok)
                    handler.OKButton.Click();
                else
                    handler.CancelButton.Click();
            }
        }

        /// <summary>
        /// Kill all processes.
        /// </summary>
        /// <param name="sName">Process name.</param>
        public static void KillAllProcess(string sName)
        {
            foreach (System.Diagnostics.Process proc in System.Diagnostics.Process.GetProcessesByName(sName))
            {
                if (proc.ProcessName.ToLowerInvariant().Contains(sName))
                {
                    try
                    {
                        proc.Kill();
                        //System.Threading.Thread.Sleep(1000);
                    }
                    catch
                    {
                    }
                }
            }

           // System.Threading.Thread.Sleep(5000);
        }


        #endregion Windows

        #region Html reports

        /// <summary>
        /// Start to write a new html file
        /// </summary>
        public static void HTMLReporting(string title)
        {
            writer = new StreamWriter(LogFileName, true);
            writer.WriteLine("<html xmlns=\"http://www.w3.org/1999/xhtml\" >");
            writer.WriteLine("<head>");
            writer.WriteLine(string.Format("<title>{0}</title>", title));
            writer.WriteLine("</head>");
            writer.WriteLine("<body>");
            writer.WriteLine("<H2 style=\"font-family:Verdana\">Test execution report</H2>");
            writer.WriteLine("<table style=\"font-family:Arial\" style=\"border:black\" border=\"true\">");
            writer.WriteLine("<tr >");
            writer.WriteLine("<td>TestCase</td>");
            writer.WriteLine("<td>Date</td>");
            writer.WriteLine("<td>Message</td>");
            writer.WriteLine("<td>Status</td>");
            writer.WriteLine("<td>Image Link</td>");
            writer.WriteLine("</tr>");
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// mark the starting of a test case in the html file
        /// </summary>
        /// <param name="testcase"></param>
        public static void StartTest(string className, string testcase)
        {
            testFlag = true;
            writer = new StreamWriter(LogFileName, true);
            writer.WriteLine("<tr style=\"color:Blue\">");
            writer.WriteLine("<td style=\"width:200\">" + className + " " + testcase + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + "-" + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + "-" + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + "-" + "</td>");
            writer.WriteLine("</tr>");
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Mark the ending of a test case in the html file
        /// </summary>
        /// <param name="testcase"></param>
        public static bool EndTest(string testcase, UnitTestOutcome outcome)
        {
            writer = new StreamWriter(LogFileName, true);
            if (testFlag == true && outcome == UnitTestOutcome.Passed)
                writer.WriteLine("<tr style=\"color:Blue\">");
            else
                writer.WriteLine("<tr style=\"color:CC00CC\">");
            if (testcase.Length > 50)
                writer.WriteLine("<td style=\"width:200\">" + "End Test Case: " + testcase.Insert(40, " ") + "</td>");
            else
                writer.WriteLine("<td style=\"width:200\">" + "End Test Case: " + testcase + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + "Test Result" + "</td>");
            if (testFlag == true && outcome == UnitTestOutcome.Passed)
                writer.WriteLine("<td style=\"width:200\">PASS</td>");
            else
                writer.WriteLine("<td style=\"width:200\">FAIL</td>");
            writer.WriteLine("<td style=\"width:200\">" + "-" + "</td>");
            writer.WriteLine("</tr>");
            writer.Flush();
            writer.Close();
            return testFlag;
        }

        /// <summary>
        /// Log a fail message in the html file
        /// </summary>
        /// <param name="message">Fail Message</param>
        /// <param name="imagepath">captured image path</param>
        public static void LogFail(string message, string imagepath = "", string status = "Fail")
        {
            writer = new StreamWriter(LogFileName, true);
            writer.WriteLine("<tr style=\"color:Red\">");
            writer.WriteLine("<td style=\"width:200\">" + "Validation" + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + message + "</td>");
            writer.WriteLine("<td style=\"width:200\">Fail</td>");
            writer.WriteLine("<td style=\"width:200\"><a href=\"" + imagepath + "\"/>" + imagepath + "</td>");
            writer.WriteLine("</tr>");
            writer.Flush();
            writer.Close();
            testFlag = false;
            testBuild = false;
        }

        /// <summary>
        /// Log a pass message in the html file
        /// </summary>
        /// <param name="message">Pass message</param>
        public static void LogPass(string message)
        {
            writer = new StreamWriter(LogFileName, true);
            writer.WriteLine("<tr style=\"color:Green\">");
            writer.WriteLine("<td style=\"width:200\">" + "Validation" + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + message + "</td>");
            writer.WriteLine("<td style=\"width:200\">PASS</td>");
            writer.WriteLine("<td style=\"width:200\">" + "-" + "</td>");
            writer.WriteLine("</tr>");
            writer.Flush();
            writer.Close();
        }

        public static void LogWarning(string message)
        {
            writer = new StreamWriter(LogFileName, true);
            writer.WriteLine("<tr style=\"color:brown\">");
            writer.WriteLine("<td style=\"width:200\">" + "Message" + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + message + "</td>");
            writer.WriteLine("<td style=\"width:200\">Warning</td>");
            writer.WriteLine("<td style=\"width:200\">" + "-" + "</td>");
            writer.WriteLine("</tr>");
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Log a Done message in the html file
        /// </summary>
        /// <param name="message">Done message</param>
        public static void LogDoneMessage(string message)
        {
            writer = new StreamWriter(LogFileName, true);
            writer.WriteLine("<tr style=\"color:orange\">");
            writer.WriteLine("<td style=\"width:200\">" + "User action" + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + DateTime.Now.ToString("yyyyMMddHHmmss") + "</td>");
            writer.WriteLine("<td style=\"width:200\">" + message + "</td>");
            writer.WriteLine("<td style=\"width:200\">Done</td>");
            writer.WriteLine("<td style=\"width:200\">" + "-" + "</td>");
            writer.WriteLine("</tr>");
            writer.Flush();
            writer.Close();
        }

        /// <summary>
        /// Completing the html log file
        /// </summary>
        public static void endOfLog()
        {
            writer = new StreamWriter(Util.LogFileName, true);
            writer.WriteLine("</table>");
            writer.WriteLine("</body>");
            writer.WriteLine("</html>");
            writer.Flush();
            writer.Close();
        }

        #endregion

        #region DataBase Calls.

        /// <summary>
        /// Get product availability.
        /// </summary>
        /// <param name="productsku">Product SKU.</param>
        /// <returns>Quantity on hand.</returns>
        public static int ProductAvailable(string productsku)
        {
            // Get the quantity on hand for a given product SKU.                  .
            DataSet ds = DataAccess.GetDataSet(string.Concat(@"SELECT QuantityOnHand FROM dbo.WarehouseProducts where dbo.WarehouseProducts.IsAvailable = 1 AND 
                    dbo.WarehouseProducts.ProductID = (SELECT ProductID from dbo.Products where dbo.Products.SKU='", string.Concat(productsku, "')")), connectionString);
            DataRowCollection dr = ds.Tables[0].Rows;

            return Convert.ToInt32(dr[0][0]);
        }

        /// <summary>
        /// Get Sponsor name of the default distributor QA site.
        /// </summary>
        /// <returns>Sponsor name.</returns>
        public static string GetSponsorNameFromDefaultDistributorPortal()
        {
            try
            {
                // Get the sponsor name of the default distributor QA site.                  .
                DataSet ds = DataAccess.GetDataSet(string.Format(@"Select FirstName + ' ' + Lastname from dbo.Accounts where dbo.Accounts.accountstatusid=1 and dbo.Accounts.AccountId in 
                    (Select AccountId from dbo.Sites where dbo.Sites.sitestatusid=1 and  dbo.Sites.SiteId in 
                    (Select siteid from dbo.SiteUrls where dbo.SiteUrls.Url like '%{0}%' and dbo.SiteUrls.isprimaryurl = 1))", defaultPWSUrl), connectionString);
                DataRowCollection dr = ds.Tables[0].Rows;

                Util.LogDoneMessage(string.Format("Sponsor of the site '{0}' is read from DB as '{1}' ", defaultPWSUrl, Convert.ToString(dr[0][0])));

                return Convert.ToString(dr[0][0]);
            }
            catch (Exception ex)
            {
                Util.LogDoneMessage(string.Format("Unable to get the sponsor name from 'realrawresults.itworksqa.net' site because of {0} ", ex.Message));
                string spon = Browser.GetElement<Div>(new Param("myName", AttributeName.ID.ClassName)).CustomGetText();
                Util.LogDoneMessage(string.Format("Sponsor of the site 'realrawresults.itworksqa.net' is read from the site as {0} ", spon));

                return spon;
            }
        }

        /// <summary>
        /// Get product price from products table.
        /// </summary>
        /// <param name="productSKU">Product SKU.</param>
        /// <param name="enrollmentType">Enrollment type.</param>
        /// <returns>Product price.</returns>
        public static double GetProductPrice(string productSKU, int enrollmentType)
        {
            // Get product id using product SKU.
            string productID = DataAccess.GetDataSet(string.Concat("SELECT ProductID from dbo.Products where dbo.Products.SKU = '", productSKU, "'"), connectionString).Tables[0].Rows[0][0].ToString();

            // Get the product price using product id and enrollment type.
            DataSet ds = DataAccess.GetDataSet(string.Concat("SELECT Price FROM dbo.ProductPrices where dbo.ProductPrices.ProductPriceTypeID = ", enrollmentType, " and dbo.ProductPrices.ProductID = ", productID), connectionString);
            DataRowCollection dr = ds.Tables[0].Rows;

            // Return product price.
            return Convert.ToDouble(dr[0][0]);
        }

        /// <summary>
        /// Get the error logs from ItWorks QA DB, if exists.
        /// </summary>
        public static void GetErrorLogs()
        {
            try
            {
                if (Util.Browser.GetElement<Div>(new Param("validation-summary-errors", AttributeName.ID.ClassName)).Exists)
                {
                    LogErrorMsg(Util.Browser.GetElement<Div>(new Param("validation-summary-errors", AttributeName.ID.ClassName)));
                }
                else if (Util.Browser.GetElement<Div>(new Param("message1")).Exists)
                {
                    LogErrorMsg(Util.Browser.GetElement<Div>(new Param("message1")));
                }
                else if (Util.Browser.Div(Find.ByStyle("display", "block").And(Find.ByStyle("color", "#ff0000"))).Exists)
                {
                    LogErrorMsg(Util.Browser.Div(Find.ByStyle("display", "block").And(Find.ByStyle("color", "#ff0000"))));
                }
                else if (Util.Browser.GetElement<Div>(new Param("Inner", AttributeName.ID.ClassName)).Exists)
                {
                    LogErrorMsg(Util.Browser.GetElement<Div>(new Param("Inner", AttributeName.ID.ClassName)), true);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Get error log.
        /// </summary>
        /// <param name="div">Error message from Div.</param>
        /// <param name="isGenericError">If true it is an generic error for all portals.</param>
        private static void LogErrorMsg(Div div, bool isGenericError = false)
        {
            // Log error from UI to log file.
            string errMsgUI = div.CustomGetText();
            LogDoneMessage(string.Concat("Error From UI: ", errMsgUI));
            string errId;

            if (!isGenericError)
            {
                errId = errMsgUI.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[0].Split(':')[1].Trim();
            }
            else
            {
                errId = errMsgUI.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries)[3].Split(' ')[13];
            }

            // Get the error details from ItWorks QA DB.
            DataSet ds = DataAccess.GetDataSet(string.Concat(ErrLogQuery, Convert.ToInt32(errId)), connectionString);
            DataColumnCollection dataRows = ds.Tables[0].Columns;
            DataRowCollection dr = ds.Tables[0].Rows;

            foreach (DataColumn c in dataRows)
            {
                LogDoneMessage(string.Concat(c.ColumnName, " : ", dr[0][c]));
            }
        }

        /// <summary>
        /// Get username and update password.
        /// </summary>
        /// <returns>User name.</returns>
        public static string GetUserNameAndUpdatePassword()
        {
            string userName;

            // Get the active distributor user id whose account id > 100.                 .
            DataSet ds = DataAccess.GetDataSet("select TOP 1 UserId from Accounts where Accountid > 100 and AccountStatusID = 1 and AccountTypeID = 1 and UserId is not NULL", connectionString);
            DataRowCollection dr = ds.Tables[0].Rows;
            int userID = Convert.ToInt16(dr[0][0]);

            // Get username of selected user. If it is null, add a random username else void.
            ds = DataAccess.GetDataSet(string.Format("select TOP 1 * from Users where UserID = {0}", userID), connectionString);
            DataColumnCollection dataRows = ds.Tables[0].Columns;
            dr = ds.Tables[0].Rows;

            if (string.IsNullOrEmpty((dr[0][4]).ToString()))
            {
                userName = Util.GenerateRandomString();
            }
            else
            {
                userName = (dr[0][4]).ToString();
            }

            // Updating password of the selected user.
            string updateQuery = string.Format(@"UPDATE [Users]
                                                 SET [Username] = '{0}'
                                                    ,[PasswordHash] = 'Vg3133h4EvCCh+rZZo3qqiEE8xCg/NHxE2Ga7vhQf0esTPOz1Lgo4R3iTvVfzc5GQ5Tar0p7r5gp0yejGupPaVknONwq258='                                                        
                                                    WHERE UserID = {1}", userName, userID);

            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings[connectionString].ConnectionString);
            SqlCommand sqlCommand = new SqlCommand(updateQuery, sqlConnection)
            {
                CommandType = CommandType.Text,
                Connection = sqlConnection
            };

            int success = DataAccess.ExecuteNonQuery(sqlCommand);
            if (success > 0)
            {
                Util.LogDoneMessage(string.Format("Successfully updated username as '{0}' and password as '{1}'.", userName, "sunshine"));
            }
            else
            {
                Util.LogDoneMessage("Unable to update username and password.");
            }

            return userName;
        }

        #endregion

        #region BVT Mail sender

        /// <summary>
        /// Send BVT test results as email when test has failures.
        /// </summary>
        public static void SendEmail(TestMasterHelpProvider.Logging.Sql.Client Client)
        {
            ++countBVT;

            if (countBVT == 3 && Util.testBuild != null)
            {
                Util.endOfLog();

                string mailServer = "smtp.mail.yahoo.com";
                string fromEmail = "nsbuildtest@yahoo.com";

                MailAddressCollection ToAddresses = new MailAddressCollection();
                MailAddressCollection CCAddresses = new MailAddressCollection();

                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailLogTo"]))
                {
                    string[] toAddresses = ConfigurationManager.AppSettings["EmailLogTo"].Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

                    if (toAddresses.Length > 0)
                    {
                        foreach (string nextAddress in toAddresses)
                        {
                            ToAddresses.Add(new MailAddress(nextAddress));
                        }
                    }
                }

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailLogCC"]))
                {
                    string[] ccAddresses = ConfigurationManager.AppSettings["EmailLogCC"].Split(new string[] { ",", ";" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ccAddresses.Length > 0)
                    {
                        foreach (string nextAddress in ccAddresses)
                        {
                            CCAddresses.Add(new MailAddress(nextAddress));
                        }
                    }
                }

                mail.To.Add(ToAddresses.ToString());
                mail.CC.Add(CCAddresses.ToString());

                using (StreamReader sr = new StreamReader(Util.LogFileName))
                {
                    mail.Subject = Client.ToString() + " QA Build Issues on " + DateTime.Now.ToShortDateString();
                    mail.IsBodyHtml = true;
                    mail.Body = sr.ReadToEnd();
                    mail.Priority = MailPriority.High;
                }

                SmtpClient client = new SmtpClient(mailServer);
                client.Port = 587;

                var userName = fromEmail;
                userName = fromEmail.Substring(0, fromEmail.IndexOf("@"));

                client.Credentials = new NetworkCredential(userName, "!P@ssword");
                client.Send(mail);
            }
        }
        #endregion


        public static int? CustomRunScript(string jQueryString, int? timeout = null)
        {
            if (Util.browserType.ToString().Contains("IE"))
            {
                Util.Browser.RunScript(jQueryString);
            }
            else if (Util.browserType.ToString().Contains("Firefox"))
            {
                Util.Browser.RunScript(string.Concat("window.", jQueryString));
            }
            return Util.Browser.CustomWaitForComplete(timeout);
        }

        #endregion Methods
    }
}
