using System.Collections.Generic;
using System.Web.Mvc;
using NetSteps.Communication.UI.Common;
using NetSteps.Data.Entities;

namespace DistributorBackOffice.Models.Home
{
    public class IndexViewModel
    {
        public Site Site { get; set; }

        public ControllerContext ControllerContext { get; set; }

        public IEnumerable<SiteWidget> ActiveSiteWidgets { get; set; }

        public IEnumerable<Widget> UserWidgets { get; set; }

        public IList<IAccountAlertMessageModel> AlertMessages { get; set; }

        public IList<IAccountAlertModalModel> AlertModals { get; set; }


        public virtual IndexViewModel LoadResources(
            Site site,
            ControllerContext context,
            IEnumerable<SiteWidget> siteWidgets,
            IEnumerable<Widget> userWidgets,
            IList<IAccountAlertMessageModel> alertMessages,
            IList<IAccountAlertModalModel> alertModals)
        {
            this.Site = site;
            this.ControllerContext = context;
            this.ActiveSiteWidgets = siteWidgets;
            this.UserWidgets = userWidgets;
            this.AlertMessages = alertMessages;
            this.AlertModals = alertModals;

            return this;
        }

    }
}