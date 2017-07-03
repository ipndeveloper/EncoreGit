using System;
using System.Configuration;
//using AjaxControlToolkit;

namespace NetSteps.Web.Base
{
    public class ReplicatedMasterPage : BaseMasterPage
    {
        public void TransferToAdmin()
        {


            if (PageMode == ViewingMode.Edit)
            {
                Response.Redirect(
                    String.Format("{0}/Sites/SiteLanding.aspx", ConfigurationManager.AppSettings["AdminUrl"])
                );
            }
            else if (PageMode == ViewingMode.ConsultantEdit)
            {
                Response.Redirect(
                    String.Format("{0}/Landingpage.aspx", GetBackOfficeUrl())
                );
            }
            else if (PageMode == ViewingMode.Staging)
            {
                if (!String.IsNullOrEmpty(Request.QueryString["FromDashboard"]))
                {
                    Response.Redirect(
                        String.Format("{0}/Home.aspx", ConfigurationManager.AppSettings["AdminUrl"])
                    );
                }

                if (!String.IsNullOrEmpty(Request.QueryString["IsNew"]))
                {
                    Response.Redirect(
                        String.Format("{0}/ContentAdmin/EditContent.aspx?SectionId={1}", ConfigurationManager.AppSettings["AdminUrl"], ReviewSectionId)
                    );
                }

                if (!String.IsNullOrEmpty(Request.QueryString["VersionList"]))
                {
                    Response.Redirect(
                        String.Format("{0}/ContentAdmin/Versions.aspx?SectionId={1}&type={2}", ConfigurationManager.AppSettings["AdminUrl"], ReviewSectionId, Request.QueryString["VersionList"])
                    );
                }

                string originalQueryString = "";
                if (!String.IsNullOrEmpty(Request.QueryString["OriginalId"]))
                {
                    int originalId = 0;
                    if (int.TryParse(Request.QueryString["OriginalId"], out originalId))
                        originalQueryString = String.Format("&OriginalId={0}", originalId);
                }

                Response.Redirect(
                    String.Format("{0}/ContentAdmin/EditContent.aspx?SectionId={1}&ContentId={2}{3}", ConfigurationManager.AppSettings["AdminUrl"], ReviewSectionId, ReviewContentId, originalQueryString)
                );
            }
        }

        public string GetBackOfficeUrl()
        {
            string bourl = CurrentSite.BackOfficeUrl;

            if (string.IsNullOrEmpty(bourl))
            {
                bourl = ConfigurationManager.AppSettings["BackOfficeUrl"];
            }
            return bourl;
        }
    }
}
