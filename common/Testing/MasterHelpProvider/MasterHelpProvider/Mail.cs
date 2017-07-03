using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Runtime.InteropServices;
using System.IO;

namespace TestMasterHelpProvider
{
	[Obsolete("Use EmailMessage instead.",  true)]
	public class Mail
	{

		Outlook._Application objOutlook;
		bool blnOutlookRunning = false;

		public Mail()
		{
			try
			{
				blnOutlookRunning = OutlookRunning();

				if (blnOutlookRunning == false)
				{
					objOutlook = new Outlook.ApplicationClass();
				}

			}
			catch (Exception ex)
			{
				throw new ApplicationException(ex.Message);
			}
		}

		private bool OutlookRunning()
		{
			try
			{
				objOutlook = Marshal.GetActiveObject("Outlook.Application") as Outlook.Application;
				return true;
			}
			catch(Exception ex)
			{
				return false;
			}

		}

		public void SendEmail(string strEmail)
		{
			try
			{
				Outlook.MailItem oMsg = (Outlook.MailItem)objOutlook.CreateItem(Microsoft.Office.Interop.Outlook.OlItemType.olMailItem);

				Outlook.Recipient oRecip;

				oRecip = (Outlook.Recipient)oMsg.Recipients.Add(strEmail);
				oRecip.Resolve();

				oMsg.Subject = "Automated Test " + DateTime.Now.ToString();
				oMsg.Body = "Automated Test";

				if (Directory.Exists("C:\\TestImage\\"))
				{
					//string[] fileEntries = Directory.GetFiles("C:\\TestImage\\");
					string[] fileEntries = Directory.GetFiles("C:\\TestImage\\", "*.doc", SearchOption.AllDirectories);

					foreach (string fileName in fileEntries)
					{

						oMsg.Attachments.Add(fileName, Outlook.OlAttachmentType.olByValue, null, null);

					}
				}

				oMsg.Send();

				System.Threading.Thread.Sleep(4000);

				Marshal.ReleaseComObject(oRecip);
				Marshal.ReleaseComObject(oMsg);
				oRecip = null;
				oMsg = null;

				if (blnOutlookRunning == false) //we opened it we will close it
				{
					objOutlook.Quit();
					Marshal.ReleaseComObject(objOutlook);
					objOutlook = null;
				}
				else
				{
					objOutlook = null;
				}
			}
			catch (Exception ex)
			{
				throw;
			}

		}

	}
}
