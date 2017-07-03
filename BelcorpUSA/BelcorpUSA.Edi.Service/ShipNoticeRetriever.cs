using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BelcorpUSA.Edi.Common.Orders;
using BelcorpUSA.Edi.Service.Configuration;
using System.Net;
using System.IO;
using NetSteps.Diagnostics.Utilities;

namespace BelcorpUSA.Edi.Service
{
	internal class ShipNoticeRetriever
	{
		public List<Edi856ShipNoticeList> GetShipNotices(EdiDropLocationConfigurationElement drop, string tz, string workingFolder)
		{
			var result = new List<Edi856ShipNoticeList>();
			if (Uri.IsWellFormedUriString(drop.Location, UriKind.Absolute))
			{
				Uri ftpLocation = new Uri(drop.Location, UriKind.Absolute);
				var fileList = GetFileList(ftpLocation, drop.Credentials);
				foreach (var file in fileList)
				{
					string workingFile = Path.Combine(workingFolder, file);
					Uri fileUri = new Uri(ftpLocation, file);
					if (file.StartsWith("856"))
					{
						if (DownloadFtpFile(drop.Credentials, workingFile, fileUri))
						{
							DeleteFtpFile(drop.Credentials, fileUri);
						}
					}
					else
					{
						DeleteFtpFile(drop.Credentials, fileUri);
					}
				}
				Directory.GetFiles(workingFolder, "*.edi").ToList().ForEach(workingFile =>
				{
					try
					{
						result.Add(Edi856ShipNoticeList.FromFile(workingFile, tz));
					}
					catch (Exception ex)
					{
						this.TraceException(ex);
						File.Move(workingFile, String.Concat(workingFile, ".error"));
					}
				});
			}
			else if (Directory.Exists(drop.Location))
			{
				foreach (var file in Directory.GetFiles(drop.Location, "856*"))
				{
					result.Add(Edi856ShipNoticeList.FromFile(file, tz));
				}
			}
			return result;
		}

		private bool DownloadFtpFile(EdiDropLocationConfigurationElement.CredentialsConfigurationElement creds, string workingFile, Uri fileUri)
		{
			FtpWebRequest fileRequest = FtpWebRequest.Create(fileUri) as FtpWebRequest;
			fileRequest.Credentials = new NetworkCredential(creds.UserName, creds.Password);
			fileRequest.Method = WebRequestMethods.Ftp.DownloadFile;
			FtpStatusCode responseCode;
			string responseDesc;
			try
			{
				using (var response = (FtpWebResponse)fileRequest.GetResponse())
				{
					responseCode = response.StatusCode;
					responseDesc = response.StatusDescription;
					using (var responseStream = response.GetResponseStream())
					using (var reader = new StreamReader(responseStream))
					using (var writer = new StreamWriter(workingFile))
					{
						writer.Write(reader.ReadToEnd());
					}
				}
				this.TraceInformation(String.Format("Downloaded File Data ({0} - {1}): {2}", responseCode, responseDesc, workingFile));
				return true;
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
			}
			return false;
		}

		private bool DeleteFtpFile(EdiDropLocationConfigurationElement.CredentialsConfigurationElement creds, Uri fileUri)
		{
			FtpWebRequest fileRequest = FtpWebRequest.Create(fileUri) as FtpWebRequest;
			fileRequest.Credentials = new NetworkCredential(creds.UserName, creds.Password);
			fileRequest.Method = WebRequestMethods.Ftp.DeleteFile;
			FtpStatusCode responseCode;
			string responseDesc;
			try
			{
				using (var response = (FtpWebResponse)fileRequest.GetResponse())
				{
					responseCode = response.StatusCode;
					responseDesc = response.StatusDescription;
				}
				this.TraceInformation(String.Format("Deleted File ({0} - {1}): {2}", responseCode, responseDesc, fileUri));
				return true;
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
			}
			return false;
		}

		private List<string> GetFileList(Uri ftpLocation, EdiDropLocationConfigurationElement.CredentialsConfigurationElement creds)
		{
			FtpWebRequest listrequest = FtpWebRequest.Create(ftpLocation) as FtpWebRequest;
			listrequest.Credentials = new NetworkCredential(creds.UserName, creds.Password);
			listrequest.Method = WebRequestMethods.Ftp.ListDirectory;
			FtpStatusCode responseCode;
			string responseDesc;
			string responseListing = null;
			try
			{
				using (var response = (FtpWebResponse)listrequest.GetResponse())
				{
					responseCode = response.StatusCode;
					responseDesc = response.StatusDescription;
					using (var responseStream = response.GetResponseStream())
					using (var reader = new StreamReader(responseStream))
					{
						responseListing = reader.ReadToEnd();
					}
					this.TraceInformation(String.Format("Recieved File List ({0} - {1}):\r\n{2}", responseCode, responseDesc, responseListing));
				}
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
			}
			if (!String.IsNullOrWhiteSpace(responseListing))
			{
				return new List<string>(responseListing.Split("\r\n".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
			}
			else
			{
				return new List<string>();
			}
		}
	}
}
