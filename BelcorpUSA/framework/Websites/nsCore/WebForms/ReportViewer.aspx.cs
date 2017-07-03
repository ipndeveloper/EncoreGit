using System;
using System.Configuration;
using System.Security.Principal;
using Microsoft.Reporting.WebForms;
using NetSteps.Data.Entities;
using NetSteps.Encore.Core.IoC;
using NetSteps.Web.Mvc.Helpers;
using Page = System.Web.UI.Page;

namespace nsCore.WebForms
{
    public partial class ReportsDisplay : Page
    {
        private void Page_Load(object sender, EventArgs e)
        {
        	string token = Request.QueryString["Token"];
        	string reportPath = Request.QueryString["ReportID"];
        	
			if (string.IsNullOrEmpty(reportPath))
			{
				Response.Redirect("~/Areas/Reports/Views/Reports");
			}

        	string userIDString = CorporateUser.Decrypt(token);
        	int userID;
			if(!int.TryParse(userIDString, out userID))
			{
				Response.Redirect("~/Login");
			}

			if (userID <= 0)
            {
                Response.Redirect("~/Login");
            }

        	bool hasFunction = Convert.ToBoolean(ViewState["hasFunction"] ?? false);

			if(!hasFunction)
			{
				var currentUser = NetSteps.Data.Entities.User.LoadFull(userID);
				if (!currentUser.HasFunction("Reports"))
				{
					Response.Redirect("~/");
				}
				ViewState.Add("hasFunction", true);
			}


			int asyncPostBackTimeout;
            if (int.TryParse(ConfigurationManager.AppSettings["ReportServerAsyncPostBackTimeout"], out asyncPostBackTimeout))
                ScriptManager1.AsyncPostBackTimeout = asyncPostBackTimeout;

            if (!IsPostBack)
            {
                rptViewer.ProcessingMode = ProcessingMode.Remote;
                rptViewer.ShowCredentialPrompts = true;
				rptViewer.KeepSessionAlive = false;

                string configUser = Request.QueryString["configUser"] == null ? "ReportServerUser" : Request.QueryString["configUser"];
                string configPass = Request.QueryString["configPass"] == null ? "ReportServerPassword" : Request.QueryString["configPass"];
                string configDomain = Request.QueryString["configDomain"] == null ? "ReportServerDomain" : Request.QueryString["configDomain"];
                string configServerURL = Request.QueryString["configServerURL"] == null ? "ReportServerURL" : Request.QueryString["configServerURL"];
                string reportParams = Request.QueryString["params"] == null ? "" : Request.QueryString["params"];
                bool parameterPrompts = Request.QueryString["paramPrompts"] == null ? true : Request.QueryString["paramPrompts"].ToLower() == "false" ? false : true;
                string reportServerQuery = Request.QueryString["ReportServer"];
                //use report server if included in query, otherwise use config value.
                string reportServerUrl = string.IsNullOrEmpty(reportServerQuery) ? ConfigurationManager.AppSettings[configServerURL] : reportServerQuery;

                rptViewer.ServerReport.ReportServerCredentials = new ReportServerCredentials() { configUser = configUser, configPass = configPass, configDomain = configDomain };
                rptViewer.ServerReport.ReportPath = reportPath;
                rptViewer.ServerReport.ReportServerUrl = new Uri(reportServerUrl);

                foreach (string p in reportParams.Split('~'))
                {
                    var v = p.Split(':');
                    if (v.Length > 1)
                    {
                    	var rptParam = new ReportParameter(v[0], v[1]);

                        try
                        {
                            rptViewer.ServerReport.SetParameters(rptParam);
                        }
                        catch (Exception) { }
                    }
                }

                rptViewer.ShowParameterPrompts = parameterPrompts;

                // if this is order shipment report set report parameter
                if (Request.QueryString["OrderNumber"] != null && !String.IsNullOrEmpty(Request.QueryString["OrderNumber"]))
                {
                    var param = new ReportParameter("OrderNumber", Request.QueryString["OrderNumber"]);
                    rptViewer.ServerReport.SetParameters(param);
                }
                rptViewer.ServerReport.Refresh();
            }
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