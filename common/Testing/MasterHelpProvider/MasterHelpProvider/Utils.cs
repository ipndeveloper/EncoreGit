using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using SHDocVw;
using TestMasterHelpProvider.Logging;
using TestMasterHelpProvider.Logging.Sql;
using WatiN.Core;
using WatiN.Core.DialogHandlers;
using Config = System.Configuration;
using Thread = System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Forms;
using System.Drawing;
namespace TestMasterHelpProvider
{
	public static class Utils
	{
		#region Fields

		public const string UseSqlLoggingAppSettingName = "UseSqlLogging";
		public const string TestAutomationServerMachineName = "NETSTEPSDEV12";
		public const string DefaultLogFileLocation = @"C:\TestFiles";

		private static string __logFileName = "LogFile.txt";
		private static bool __runTestsInTestMode = false;
		private static Logger __logger;

		#endregion

		#region Enumerations

		public enum Environment
		{
			Staging,
			Test,
			Live,
			Dev,
			QA,
		}

		public enum BrowserType
		{
			InternetExplorer,
			FireFox,
			Chrome,
			Unknown
		}

		#endregion

		#region Properties

		/// <summary>
		/// Gets whether or not to run automated tests in test mode. Running 
		/// tests in test mode causes Util-based assertions to break the 
		/// test.
		/// </summary>
		public static bool RunTestsInTestMode
		{
			get
			{
				string runTestsInTestMode = Config.ConfigurationManager.AppSettings[TestMasterHelpProviderConstants.RunTestsInTestModeSettingName];

				if (!String.IsNullOrEmpty(runTestsInTestMode))
				{
					Boolean.TryParse(runTestsInTestMode, out Utils.__runTestsInTestMode);
				}

				return Utils.__runTestsInTestMode;
			}
		}

		/// <summary>
		/// Gets or sets the log file name for the current run.
		/// </summary>
		public static string LogFileName
		{
			get { return Utils.__logFileName; }
			set { Utils.__logFileName = value; }
		}

		/// <summary>
		/// Gets the logger and using the config and machine name determines how to 
		/// log. If an SqlLogWriter is determined to be the best logging solution, 
		/// then a new Logger.Sql.TestRun is created.
		/// </summary>
		public static Logger Logger
		{
			get
			{
				if (Utils.__logger == null)
				{
					if (System.Environment.MachineName.Equals(Utils.TestAutomationServerMachineName))
					{
						SqlLogWriter newLogWriter = new SqlLogWriter();

						TestRun newTestRun = new TestRun();
						newLogWriter.TestRun = newTestRun;
						newLogWriter.TestRun.SerializeToDatabase();

						Utils.__logger = Logger.GetInstance(newLogWriter);
					}
					else
					{
						if (!String.IsNullOrEmpty(Config.ConfigurationManager.AppSettings[Utils.UseSqlLoggingAppSettingName]) || !bool.Parse(Config.ConfigurationManager.AppSettings[Utils.UseSqlLoggingAppSettingName]))
						{
							if (!Directory.Exists(Utils.DefaultLogFileLocation))
							{
								Directory.CreateDirectory(Utils.DefaultLogFileLocation);
							}

							PlainLogWriter newLogWriter = new PlainLogWriter(Path.Combine(Utils.DefaultLogFileLocation, Utils.__logFileName));

							Utils.__logger = Logger.GetInstance(newLogWriter);
						}
						else if (bool.Parse(Config.ConfigurationManager.AppSettings[Utils.UseSqlLoggingAppSettingName]))
						{
							SqlLogWriter newLogWriter = new SqlLogWriter();

							TestRun newTestRun = new TestRun();
							newLogWriter.TestRun = newTestRun;
							newLogWriter.TestRun.SerializeToDatabase();

							Utils.__logger = Logger.GetInstance(newLogWriter);
						}
					}
				}

				return Utils.__logger;
			}
		}

		#endregion

		#region Methods

		/// <summary>
		/// Gets the browser type.
		/// </summary>
		/// <returns></returns>
		public static Utils.BrowserType GetBrowserType()
		{
			System.Type type = WatiNContext.GetContext().Browser.GetType();

			if (type.Name.Equals("FireFox", StringComparison.InvariantCultureIgnoreCase))
			{
				return BrowserType.FireFox;
			}
			else if (type.Name.Equals("IE", StringComparison.InvariantCultureIgnoreCase))
			{
				return BrowserType.InternetExplorer;
			}
			else
			{
				return BrowserType.Unknown;
			}

		}

		#region Speed Up typing

		public static void TypeTextQuickly(this TextField textfield, string text)
		{
			textfield.SetAttributeValue("value", text);
		}

		#endregion

		#region Assertions

		public static string GetDefectsString(int[] defects = null)
		{
			StringBuilder allDefects = new StringBuilder(); ;
			if (defects != null)
			{
				bool first = true;
				foreach (int defect in defects)
				{
					if (first)
						allDefects.Append(" Defects: ");
					else
						allDefects.Append(", ");
					allDefects.Append(defect.ToString());
					first = false;
				}
			}
			return allDefects.ToString();
		}

		/// <summary>
		/// Asserts whether an expression is true without breaking out of the test.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public static bool AssertIsTrue(bool expression, string logMessage = null, int[] defects = null)
		{
			bool result = false;

			try
			{
				Assert.IsTrue(expression);

				Utils.Logger.WritePassMessage(String.Format("AssertIsTrue: Pass{0}", string.IsNullOrEmpty(logMessage) ? string.Empty : String.Format(" Message: {0}", logMessage.Replace("'", "''"))));

				result = true;
			}
			catch (Exception)
			{
				Utils.Logger.WriteFailMessage(String.Format("AssertIsTrue: Fail{0}{1}", String.IsNullOrEmpty(logMessage) ? String.Empty : String.Format(" Message: {0}", logMessage.Replace("'", "''")), GetDefectsString(defects)));

				if (Utils.__runTestsInTestMode)
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Asserts whether an expression is false without breaking out of the test.
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public static bool AssertIsFalse(bool expression, string logMessage = null, int[] defects = null)
		{
			bool result = false;

			try
			{
				Assert.IsFalse(expression);

				Utils.Logger.WritePassMessage(String.Format("AssertIsFalse: Pass{0}", String.IsNullOrEmpty(logMessage) ? String.Empty : String.Format(" Message: {0}", logMessage.Replace("'", "''"))));

				result = true;
			}
			catch (Exception)
			{
				Utils.Logger.WriteFailMessage(String.Format("AssertIsFalse: Fail{0}{1}", String.IsNullOrEmpty(logMessage) ? String.Empty : String.Format(" Message: {0}", logMessage.Replace("'", "''")), GetDefectsString(defects)));

				if (Utils.__runTestsInTestMode)
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Asserts that the page contains certain text without breaking out of the test.
		/// </summary>
		/// <param name="expectedString"></param>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public static bool AssertPageContainsText(string expectedString, string logMessage = null, int[] defects = null)
		{
			bool result = false;

			try
			{
				Assert.IsTrue(WatiNContext.GetContext().Browser.ContainsText(expectedString));

				Utils.Logger.WritePassMessage(String.Format("AssertPageContainsText: Pass{0}", String.IsNullOrEmpty(logMessage) ? string.Format(" String: {0}", expectedString.Replace("'", "''")) : String.Format(" Message: {0}", logMessage.Replace("'", "''"))));

				result = true;
			}
			catch (Exception)
			{
				Utils.Logger.WriteFailMessage(String.Format("AssertPageContainsText: Fail{0}(1)}", String.IsNullOrEmpty(logMessage) ? string.Format(" String: {0}", expectedString.Replace("'", "''")) : String.Format(" Message: {0}", logMessage.Replace("'", "''")), GetDefectsString(defects)));

				if (Utils.__runTestsInTestMode)
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Asserts whether the page does not contain certain text without breaking out of the test.
		/// </summary>
		/// <param name="expectedString"></param>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public static bool AssertPageDoesNotContainText(string expectedString, string logMessage = null, int[] defects = null)
		{
			bool result = false;

			try
			{
				Assert.IsFalse(WatiNContext.GetContext().Browser.ContainsText(expectedString));

				Utils.Logger.WritePassMessage(String.Format("AssertDoesNotPageContainText: Pass{0}", String.IsNullOrEmpty(logMessage) ? string.Format(" String: {0}", expectedString.Replace("'", "''")) : String.Format(" Message: {0}", logMessage.Replace("'", "''"))));

				result = true;
			}
			catch (Exception)
			{
				Utils.Logger.WriteFailMessage(String.Format("AssertDoesNotPageContainText: Fail{0}{1}", String.IsNullOrEmpty(logMessage) ? string.Format(" String: {0}", expectedString.Replace("'", "''")) : String.Format(" Message: {0}", logMessage.Replace("'", "''")), GetDefectsString(defects)));

				if (Utils.__runTestsInTestMode)
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Asserts that an image exists.
		/// </summary>
		/// <param name="expectedImage"></param>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public static bool AssertImageExists(string expectedImage, string logMessage = null, int[] defects = null)
		{
			bool result;
			try
			{
				Assert.IsTrue(WatiNContext.GetContext().Browser.ContainsText(expectedImage));

				Utils.Logger.WritePassMessage(String.Format("AssertImageExists: Pass{0}", String.IsNullOrEmpty(logMessage) ? String.Format(" Image: {0}", expectedImage) : String.Format(" Message: {0}", logMessage.Replace("'", "''"))));

				result = true;
			}
			catch (Exception)
			{
				Utils.Logger.WriteFailMessage(String.Format("AssertImageExists: Fail{0}{1}", String.IsNullOrEmpty(logMessage) ? String.Format(" Image: {0}", expectedImage) : String.Format(" Message {0}", logMessage.Replace("'", "''")), GetDefectsString(defects)));

				if (Utils.__runTestsInTestMode)
				{
					throw;
				}
				result = false;
			}

			return result;
		}

		/// <summary>
		/// Asserts whether an element exists. This should be used when it is desirable to log the validation of elements.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public static bool AssertElementExists(Element element, string logMessage = null, int[] defects = null)
		{
			bool result = false;

			try
			{
				Assert.IsTrue(element.Exists);

				Utils.Logger.WritePassMessage(String.Format("AssertElementExists: Pass{0}", !String.IsNullOrEmpty(logMessage) ? String.Empty : String.Format(" Message: {0}", logMessage.Replace("'", "''"))));

				result = true;
			}
			catch (Exception)
			{
				Utils.Logger.WriteFailMessage(String.Format("AssertElementExists: Fail{0}{1}", String.IsNullOrEmpty(logMessage) ? String.Empty : String.Format(" Message: {0}", logMessage), GetDefectsString(defects)));

				if (Utils.__runTestsInTestMode)
				{
					throw;
				}
			}

			return result;
		}

		/// <summary>
		/// Asserts whether an element does not exist. This should be used when it is desirable to log the validation of elements.
		/// </summary>
		/// <param name="element"></param>
		/// <param name="logMessage"></param>
		/// <returns></returns>
		public static bool AssertElementDoesNotExist(Element element, string logMessage = null, int[] defects = null)
		{
			bool result = false;

			try
			{
				Assert.IsFalse(element.Exists);

				Utils.Logger.WritePassMessage(String.Format("AssertElementDoesNotExists: Pass{0}", String.IsNullOrEmpty(logMessage) ? String.Empty : String.Format(" Message: {0}", logMessage)));

				result = true;
			}
			catch (Exception)
			{
				Utils.Logger.WriteFailMessage(String.Format("AssertElementDoesNotExists: Fail{0}(1)", String.IsNullOrEmpty(logMessage) ? String.Empty : String.Format(" Message: {0}", logMessage), GetDefectsString(defects)));

				if (Utils.__runTestsInTestMode)
				{
					throw;
				}
			}

			return result;
		}

		#endregion

		#region Dialog Closers

		public static void CloseJSDialog()
		{
			ConfirmDialogHandler confirmDialog = new ConfirmDialogHandler();
			Wait(10000);
			WatiNContext.GetContext().Browser.AddDialogHandler(confirmDialog);
			Wait(10000);
			confirmDialog.OKButton.Click();
		}

		#endregion

		#region Waiters

		public static void Wait(int milliseconds)
		{
			Thread.Thread.Sleep(milliseconds);
		}

		public static void WaitDefault()
		{
			Wait(TestMasterHelpProviderConstants.DefaultWaitTime);
		}

		#endregion

		public static void ClickBackBrowserButton()
		{
			WatiNContext.GetContext().Browser.Back();
		}

		#region Test Folder Stuff

		/// <summary>
		/// Creates the TestFiles directory in the root of C:\.
		/// </summary>
		public static void CreateDir()
		{
			DirectoryInfo dir = new DirectoryInfo(@"C:\TestFiles");
			dir.Create();
		}

		/// <summary>
		/// Clears the test files folder.
		/// </summary>
		public static void ClearTestFolder()
		{
			try
			{
				if (Directory.Exists("C:\\TestFiles\\"))
				{
					string[] fileEntries = Directory.GetFiles("C:\\TestFiles\\");

					foreach (string fileName in fileEntries)
					{

						File.Delete(fileName);
					}
				}
			}
			catch (Exception)
			{
				throw;
			}
		}

		/// <summary>
		/// Deletes a directory given by path.
		/// </summary>
		/// <param name="Path"></param>
		/// <returns></returns>
		public static bool DeleteDirectory(string Path)
		{
			if (Directory.Exists(Path))
			{
				try
				{
					ClearAttributes(Path);
					Directory.Delete(Path, true);
				}
				catch (IOException)
				{
					return false;
				}
			}
			return true;
		}

		/// <summary>
		/// Clears the attributes of a given directory.
		/// </summary>
		/// <param name="currentDir"></param>
		public static void ClearAttributes(string currentDir)
		{
			if (Directory.Exists(currentDir))
			{
				string[] subDirs = Directory.GetDirectories(currentDir);
				foreach (string dir in subDirs)
					ClearAttributes(dir);
				string[] files = files = Directory.GetFiles(currentDir);
				foreach (string file in files)
					File.SetAttributes(file, FileAttributes.Normal);
			}
		}

		/// <summary>
		/// Clears test results.
		/// </summary>
		/// <param name="strProjectPath"></param>
		public static void ClearTestResults(string strProjectPath)
		{
			try
			{
				strProjectPath += "\\TestResults\\";

				if (Directory.Exists(strProjectPath))
				{
					try
					{
						DirectoryInfo objDir = new DirectoryInfo(strProjectPath);
						objDir.Delete(true);
					}
					catch (Exception ex)
					{
						//swallow all exceptions
						//if the folder cant be deleted we dont care its probably in use
					}
				}
			}
			catch (Exception ex)
			{
				throw;
			}

		}

		/// <summary>
		/// Determines whether a particular file is already locked by another program.
		/// </summary>
		/// <param name="path"></param>
		/// <returns></returns>
		static bool FileInUse(string path)
		{
			try
			{
				//Just opening the file as open/create
				using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
				{
					//If required we can check for read/write by using fs.CanRead or fs.CanWrite
				}
				return false;
			}
			catch (IOException ex)
			{
				//check if message is for a File IO
				string __message = ex.Message.ToString();
				if (__message.Contains("The process cannot access the file"))
					return true;
				else
					throw;
			}
		}

		#endregion

		/// <summary>
		/// Generates a random file name.
		/// </summary>
		/// <returns></returns>
		[Obsolete("Use Fakers instead.", false)]
		public static string GetRandomName()
		{
			String strName = Path.GetRandomFileName();
			strName = strName.Replace("-", String.Empty);
			strName = strName.Replace("_", String.Empty);
			strName = strName.Replace(".", String.Empty);

			return strName;

		}

		/// <summary>
		/// Loads HTML into a WinForm.
		/// </summary>
		/// <param name="strPath"></param>
		public static void LoadHTML(string strPath)
		{
			System.Windows.Forms.WebBrowser objIE = new System.Windows.Forms.WebBrowser();
			objIE.Navigate(strPath);

			HtmlDocument objDoc = objIE.Document;
		}

		/// <summary>
		/// Gets an HttpWebResponse for a URL.
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string GetPage(string url)
		{
			WebResponse response = null;
			Stream stream = null;
			StreamReader reader = null;

			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				response = request.GetResponse();
				stream = response.GetResponseStream();

				if (!response.ContentType.ToLower().StartsWith("text/"))
					return null;

				string buffer = String.Empty, line;

				reader = new StreamReader(stream);

				while ((line = reader.ReadLine()) != null)
				{
					buffer += line + "\r\n";
				}

				return buffer;
			}
			catch (WebException e)
			{
				System.Console.WriteLine("Can't download:" + e);
				return null;
			}
			catch (IOException e)
			{
				System.Console.WriteLine("Can't download:" + e);
				return null;
			}
			finally
			{
				if (reader != null)
					reader.Close();

				if (stream != null)
					stream.Close();

				if (response != null)
					response.Close();
			}
		}

		/// <summary>
		/// Reads the text of a file.
		/// </summary>
		/// <param name="strPath"></param>
		/// <returns></returns>
		public static string ReadFile(string strPath)
		{
			TextReader tr = new StreamReader(strPath);
			tr.ReadToEnd();
			return tr.ToString();
		}

		/// <summary>
		/// Waits for the browser to finish loading a page.
		/// </summary>
		public static void WaitForPageToLoad()
		{
			WatiNContext.GetContext().Browser.WaitForComplete();
		}

		#region " Mouse Click "

		/// <summary>
		/// Performs a native mouse click.
		/// </summary>
		/// <param name="dwFlags"></param>
		/// <param name="dx"></param>
		/// <param name="dy"></param>
		/// <param name="cButtons"></param>
		/// <param name="dwExtraInfo"></param>
		[DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
		public static extern void mouse_event(long dwFlags, long dx, long dy, long cButtons, long dwExtraInfo);

		private const int MOUSEEVENTF_LEFTDOWN = 0x02;
		private const int MOUSEEVENTF_LEFTUP = 0x04;
		private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
		private const int MOUSEEVENTF_RIGHTUP = 0x10;

		public static void DoMouseClick(int x, int y)
		{
			Cursor.Position = new Point((int)x, (int)y);

			mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, Cursor.Position.X, Cursor.Position.Y, 0, 0);
		}

		#endregion

		/// <summary>
		/// Strips bad (html-wise) characters from a string.
		/// </summary>
		/// <param name="strValue"></param>
		/// <returns></returns>
		public static string StripIllegalCharacters(string strValue)
		{
			char[] badChars = new char[] { ' ', '!', '@', '#', ',', '\'', '$', '%', '^', '&', '*', '(', ')', '_', '+', '¢', '°', '"', '=', '?', '\\', '~', ':', ';', 
			'[', ']', '{', '}', '|', '<', '>', '.', '/', '`', '_', '…' };

			for (int i = 0; i < badChars.Length; i++)
			{
				strValue = strValue.Replace(badChars[i].ToString(), String.Empty);
			}

			return strValue;

		}

		/// <summary>
		/// Closes a PDF [window] in Internet Explorer.
		/// </summary>
		/// <param name="partialPDFUrl"></param>
		public static void closePDFInIE(string partialPDFUrl)
		{
			ShellWindows shellWindows = new ShellWindows();

			string filename;

			foreach (InternetExplorer ie in shellWindows)
			{
				filename = Path.GetFileNameWithoutExtension(ie.FullName).ToLower();

				if (filename.Equals("iexplore") && ie.LocationURL.Contains(partialPDFUrl))
				{
					int i = 0;
					bool hasLoaded = false;
					do
					{
						if (!ie.Busy)
						{
							hasLoaded = true;
						}
						else
						{
							i++;

							Thread.Thread.Sleep(1000);
						}
					} while (!hasLoaded || i == 25); //only wait 25 seconds, then move on.
					ie.Quit();
				}
			}
		}

		////public static void Wait(int milliseconds)
		//{
		//    System.Threading.Thread.Sleep(milliseconds);
		//}

		#endregion
	}
}
