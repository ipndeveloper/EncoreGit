using System;
using System.Configuration;
using System.Security.Principal;
using System.Web;
using System.Web.UI;
using Microsoft.Reporting.WebForms;
using NetSteps.Security;
using NetSteps.Web.Mvc.Helpers;

namespace DistributorBackOffice.WebForms
{
    public partial class ReportsDisplay : Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
        	if (IsPostBack)
        	{
        		return;
        	}

        	string reportPath = Request.QueryString["ReportID"];
        	if (string.IsNullOrEmpty(reportPath))
        	{
        		Response.Redirect("~/Areas/Reports/Views/Reports");
        	}

        	rptViewer.ProcessingMode = ProcessingMode.Remote;
        	rptViewer.ShowCredentialPrompts = true;
			rptViewer.KeepSessionAlive = false;

        	string configUser = Request.QueryString["configUser"] == null ? "ReportServerUser" : Request.QueryString["configUser"];
        	string configPass = Request.QueryString["configPass"] == null ? "ReportServerPassword" : Request.QueryString["configPass"];
        	string configDomain = Request.QueryString["configDomain"] == null ? "ReportServerDomain" : Request.QueryString["configDomain"];
        	string configServerURL = Request.QueryString["configServerURL"] == null ? "ReportServerURL" : Request.QueryString["configServerURL"];
        	string reportParams = Request.QueryString["params"] == null ? "" : Request.QueryString["params"];
        	bool parameterPrompts = Request.QueryString["paramPrompts"] == null ? true : Request.QueryString["paramPrompts"].ToLower() == "false" ? false : true;

        	rptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials() { configUser = configUser, configPass = configPass, configDomain = configDomain };
        	rptViewer.ServerReport.ReportPath = reportPath;
        	rptViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings[configServerURL]);

        	foreach (string p in reportParams.Split('~'))
        	{
        		var v = p.Split(':');
        		if (v.Length > 1)
        		{
        			string parameter = CoreContext.CurrentAccount.AccountID.ToString();
        			var rptParam = new ReportParameter(v[0], v[1]);

        			try
        			{
        				rptViewer.ServerReport.SetParameters(rptParam);
        			}
        			catch (Exception) { }
        		}
        	}

        	rptViewer.ShowParameterPrompts = parameterPrompts;

        	if (Request.QueryString["AccountNumber"] != null && !String.IsNullOrEmpty(Request.QueryString["AccountNumber"]))
        	{
        		var num = HttpUtility.UrlDecode(Encryption.DecryptTripleDES(Request.QueryString["AccountNumber"], Encryption.SingleSignOnSalt));
        		var param = new ReportParameter("AccountNumber", num);
        		rptViewer.ServerReport.SetParameters(param);
        	}

        	rptViewer.ServerReport.Refresh();
        }
    }

    public sealed class ReportServerCredentials : IReportServerCredentials
    {
        public string configUser = "ReportServerUser";
        public string configPass = "ReportServerPassword";
        public string configDomain = "ReportServerDomain";

        public WindowsIdentity ImpersonationUser
        {
            get
            {
                return null;
            }
        }

        public bool GetFormsCredentials(out System.Net.Cookie authCookie, out string userName, out string password, out string authority)
        {
            authCookie = null;
            userName = null;
            password = null;
            authority = null;

            return false;
        }

        public System.Net.ICredentials NetworkCredentials
        {
            get
            {
                var userName = ConfigurationManager.AppSettings[configUser];
                var password = ConfigurationManager.AppSettings[configPass];
                var domain = ConfigurationManager.AppSettings[configDomain];

                return new System.Net.NetworkCredential(userName, password, domain);
            }
        }
    }
}