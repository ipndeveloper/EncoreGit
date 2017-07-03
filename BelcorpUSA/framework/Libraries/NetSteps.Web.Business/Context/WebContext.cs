using System.Web;
using NetSteps.Data.Entities;

namespace NetSteps.Web.Business
{
    public static class WebContext
    {
        public static Site CurrentSite
        {
            get { return (Site)HttpContext.Current.Session["CurrentSite"]; }
            set { HttpContext.Current.Session["CurrentSite"] = value; }
        }
    }
}