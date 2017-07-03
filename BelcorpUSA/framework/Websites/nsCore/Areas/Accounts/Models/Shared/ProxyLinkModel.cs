using NetSteps.Data.Entities.Business;

namespace nsCore.Areas.Accounts.Models.Shared
{
    public class ProxyLinkModel
    {
        public string LocalizedName { get; set; }
        public string Url { get; set; }

        public virtual ProxyLinkModel LoadResources(
            ProxyLinkData proxyLink)
        {
            LocalizedName = proxyLink.LocalizedName;
            Url = proxyLink.Url;

            return this;
        }
    }
}