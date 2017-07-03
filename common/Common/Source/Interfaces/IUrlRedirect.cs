using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Common
{
    public interface IUrlRedirect
    {
        int UrlRedirectID { get; set; }
        short SiteTypeID { get; set; }
        string Url { get; set; }
        string TargetUrl { get; set; }
        bool IsPermanent { get; set; }
    }
}
