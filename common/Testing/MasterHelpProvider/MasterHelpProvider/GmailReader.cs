using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;

namespace TestMasterHelpProvider
{
	public static class GmailReader
	{
		public static string[] emailFrom;
		public static string[] emailFromAddress;
		public static string[] emailMessages;
		public static Int16 tempCounter = 0;
		public static Int16 mailCount = 0;
		public static XmlDocument xmlDoc;
		
		public static void GetNewEmailCount()
		{
			System.Net.WebClient objClient = new System.Net.WebClient();
			XmlNodeList nodelist;
			XmlNode node;
			string strResponse;

			try
			{
				ServicePointManager.ServerCertificateValidationCallback +=
					delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
					{
						return true;
					};

				objClient.Credentials = new System.Net.NetworkCredential("NetStepsQA@gmail.com", "n3tst3ps");
				strResponse = Encoding.UTF8.GetString(objClient.DownloadData("https://mail.google.com/mail/feed/atom")); //https://USERNAME:PASSWORD@gmail.google.com/gmail/feed/atom
				string strReplace = "<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">";
				strResponse = strResponse.Replace(strReplace, "<feed>");

				xmlDoc = new XmlDocument();
					
				xmlDoc.LoadXml(strResponse);

				node = xmlDoc.SelectSingleNode("//feed/fullcount");

				mailCount = Convert.ToInt16(node.InnerText); // Get the number of unread emails

				if (mailCount > 0)
				{
					emailFrom = new string[mailCount - 1];
					emailMessages = new string[mailCount - 1];
					emailFromAddress = new string[mailCount - 1];

					nodelist = xmlDoc.SelectNodes("//feed/entry");

					foreach(XmlNode objNode in nodelist)
					{
						emailMessages[tempCounter] = objNode.ChildNodes.Item(0).InnerText;
						emailFrom[tempCounter] = objNode.ChildNodes.Item(6).ChildNodes[0].InnerText;
						emailFromAddress[tempCounter] = objNode.ChildNodes.Item(6).ChildNodes[1].InnerText;
						tempCounter += 1;
					}
					tempCounter = 0;
				}


			}
			catch (Exception ex)
			{

			}
		}
	}
}
