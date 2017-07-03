using System;
using System.ServiceModel;

namespace NetSteps.Silverlight.WCFExtensions
{
    public static class WCFHelper
    {
        /// <summary>
        /// Returns a url that will work both in DEV, TEST & LIVE. - JHE
        /// Example: CRMWCFClient("BasicHttpBinding_CRMWCF", WCFHelper.GetEndServicePointPath("../CRMWCF.svc"));
        /// </summary>
        /// <returns></returns>
        public static string GetEndServicePointPath(this string relativePath)
        {
            Uri address = new Uri(ApplicationContext.ApplicationHostSource, relativePath);
            return address.AbsoluteUri;
        }

        public static EndpointAddress ToEndpointAddress(this string relativePath)
        {
            EndpointAddress ea = new EndpointAddress(relativePath.GetEndServicePointPath());
            return ea;
        }

    }
}
