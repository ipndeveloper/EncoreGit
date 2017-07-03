using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Reporting.WebForms;
using System.Net;

namespace NetSteps.Web.Reporting.WebForms
{
	public class NSReportServerConnection : IReportServerConnection2
	{
		public IEnumerable<System.Net.Cookie> Cookies
		{
			get { return null; }
		}

		public IEnumerable<string> Headers
		{
			get { return null; }
		}

		public Uri ReportServerUrl
		{
			get
			{
				string url = ConfigurationManager.AppSettings["ReportServerUrl"];

				if (string.IsNullOrEmpty(url))
					throw new InvalidOperationException("Please specify the report server URL in the project's Web.config file");

				return new Uri(url);
			}
		}

		public int Timeout
		{
			get
			{
				int ito;
				string to = ConfigurationManager.AppSettings["ReportServerTimeout"];
				if (!String.IsNullOrWhiteSpace(to))
				{
					if (!int.TryParse(to, out ito))
						ito = 60000;
				}
				else
				{
					ito = 60000;
				}
				return ito;
			}
		}

		public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
		{
			authCookie = null;
			userName = null;
			password = null;
			authority = null;

			// Not using form credentials
			return false;
		}

		public System.Security.Principal.WindowsIdentity ImpersonationUser
		{
			get { return null; }
		}

		public System.Net.ICredentials NetworkCredentials
		{
			get
			{ 
				// Read the user information from the web.config file. By reading the information on demand instead of 
				// storing it, the credentials will not be stored in session, reducing the vulnerable surface area to the
				// web.config file, which can be secured with an ACL.

				string userName = ConfigurationManager.AppSettings["ReportServerUser"];
				if (string.IsNullOrEmpty(userName))
					throw new InvalidOperationException("Please specify the user name in the project's Web.config file.");

				string password = ConfigurationManager.AppSettings["ReportServerPassword"];
				if (string.IsNullOrEmpty(password))
					throw new InvalidOperationException("Please specify the password in the project's Web.config file");

				string domain = ConfigurationManager.AppSettings["ReportServerDomain"];

				return new NetworkCredential(userName, password, domain);
			}
		}
	}
}
