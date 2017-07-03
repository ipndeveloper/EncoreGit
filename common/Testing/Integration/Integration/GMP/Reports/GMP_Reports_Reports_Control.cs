using WatiN.Core;
using System;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public abstract class GMP_Reports_Reports_Control : Control<TableCell>
    {
        public TPage OpenReport<TPage>(string titleMatch, int? timeout = null) where TPage : GMP_Reports_Viewer_Page, new()
        {
            Link lnk = Element.GetElement<Link>(new Param(titleMatch, AttributeName.ID.Onclick, RegexOptions.None));
            timeout = lnk.CustomClick(timeout);
            return Util.AttachBrowser<TPage>("ReportViewer.aspx", timeout);
        }

    }
}
