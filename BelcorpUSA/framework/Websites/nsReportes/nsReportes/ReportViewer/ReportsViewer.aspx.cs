using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.Reporting.WebForms;
using System.Collections;
using System.Web.Mvc;
using System.Net;
using System.Configuration;

namespace nsRSReportes.ReportViewer
{
    public partial class ReportsViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                string ReportNameExcution = Request.QueryString["reporte"];
                ShowReport(ReportNameExcution);
            }
        }

        private void ShowReport(string ReportNameExcution)
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerURL"]))
                {
                    throw new Exception("Missing Report Url ReportServer from web.config file");
                }
                else if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerUser"]))
                {
                    throw new Exception("Missing user name from web.config file");
                }
                else if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerPassword"]))
                {
                    throw new Exception("Missing password from web.config file");
                }
                else if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["ReportServerDomain"]))
                {
                    throw new Exception("Missing domain from web.config file");

                }
                else
                {
                    string urlReportServer = ConfigurationManager.AppSettings["ReportServerURL"]; //+ "?ItemPath=%2fDetailAccountReceivable";
                    rptViewer.ServerReport.ReportServerUrl = new Uri(urlReportServer); //Set the ReportServer Url
                    rptViewer.ProcessingMode = ProcessingMode.Remote; // ProcessingMode will be Either Remote or Local
                    rptViewer.ServerReport.ReportPath = "/"+ReportNameExcution; //Passing the Report Path 
                    rptViewer.ShowParameterPrompts = true;
                    rptViewer.ShowRefreshButton = true;
                    rptViewer.ShowWaitControlCancelLink = true;
                    rptViewer.ShowBackButton = true;
                    rptViewer.ShowCredentialPrompts = true;   

                    //Creating an ArrayList for combine the Parameters which will be passed into SSRS Report
                    /*ArrayList reportParam = new ArrayList();
                    reportParam = ReportDefaultPatam();

                    ReportParameter[] param = new ReportParameter[reportParam.Count];
                    for (int k = 0; k < reportParam.Count; k++)
                    {
                        param[k] = (ReportParameter)reportParam[k];
                        param[k].Visible = false;
                    }*/

                    // pass crendentitilas
                    IReportServerCredentials irsc = new CustomReportCredentials(ConfigurationManager.AppSettings["ReportServerUser"], ConfigurationManager.AppSettings["ReportServerPassword"], ConfigurationManager.AppSettings["ReportServerDomain"]);
                    rptViewer.ServerReport.ReportServerCredentials = irsc;
                    //pass parmeters to report
                    //rptViewer.ServerReport.SetParameters(param); //Set Report Parameters
                    rptViewer.ServerReport.Refresh();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
          }

            private ReportParameter CreateReportParameter(string paramName, string pramValue)
            {
                ReportParameter aParam = new ReportParameter(paramName, pramValue);
                return aParam;
            }

            private ArrayList ReportDefaultPatam()
            {
                ArrayList arrLstDefaultParam = new ArrayList();
                arrLstDefaultParam.Add(CreateReportParameter("Header1", "Phase"));
                arrLstDefaultParam.Add(CreateReportParameter("PeriodIdLast", "201602"));
                return arrLstDefaultParam;
            }

            [Serializable]
            public class CustomReportCredentials : IReportServerCredentials
            {
                private string _UserName; 
                private string _PassWord; 
                private string _DomainName;

                public CustomReportCredentials(string UserName, string PassWord, string DomainName)
                {
                    _UserName = UserName;
                    _PassWord = PassWord;
                    _DomainName = DomainName;
                }

                public System.Security.Principal.WindowsIdentity ImpersonationUser
                {
                    get { return null; }
                }

                public ICredentials NetworkCredentials
                {
                    get { return new NetworkCredential(_UserName, _PassWord, _DomainName); }
                }

                public bool GetFormsCredentials(out Cookie authCookie, out string user,
                 out string password, out string authority)
                {
                    authCookie = null;
                    user = password = authority = null;
                    return false;
                }
            }

   }
}