using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    /// <summary>
    /// Class related to Section navigation under Performance tab.
    /// </summary>
    public class DWS_Performance_SectionNav_Control : Control<UnorderedList>
    {
        /// <summary>
        /// Click on Flat DownLine link.
        /// </summary>
        /// <returns>Flat DownLine page.</returns>
        public DWS_Performance_FlatDownLine_Page ClickFlatDownLine(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Performance/FlatDownline", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Performance_FlatDownLine_Page>(timeout, pageRequired);
        }

        /// <summary>
        /// Click on Graphical DownLine link.
        /// </summary>
        /// <returns>Graphical DownLine</returns>
        public DWS_Performance_GraphicalDownLine_Page ClickGraphicalLine(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Performance/GraphicalDownline", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Performance_GraphicalDownLine_Page>(timeout, pageRequired);
        }

        /// <summary>
        /// Click on Tree View link.
        /// </summary>
        /// <returns>Tree View</returns>
        public DWS_Performance_TreeView_Page ClickTreeView(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Performance/TreeView", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Performance_TreeView_Page>(timeout, pageRequired);
        }

        public DWS_Perforformance_Report_Page ClickStrartReport(int? timeout = null, bool pageRequired = true)
        {
            timeout = Element.GetElement<Link>(new Param("/Performance/NewFlatDownlineReport", AttributeName.ID.Href, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<DWS_Perforformance_Report_Page>(timeout, pageRequired);
        }
    }
}
