using WatiN.Core;
using System;
using System.Text.RegularExpressions;
using System.Threading;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public abstract class GMP_Reports_Viewer_Page : NS_Page
    {
        protected override void InitializeContents()
        {
            base.InitializeContents();
        }

        /// <summary>
        /// Logs if report rendered.
        /// </summary>
        /// <returns></returns>
        public bool IsReportRendered()
        {
            bool rendered = Document.GetElement<Div>(new Param("VisibleReportContentrptViewer_ctl10")).Children().Count > 0;
            string report = this.GetType().ToString().Split('_')[2];
            if(rendered)
                Util.LogPass(string.Format("{0} rendered", report));
            else
                Util.LogFail(string.Format("{0} failed to render", report));
            return rendered;
        }

        public bool ViewReport(int? timeout = null, bool pageRequired = true)
        {
            if (timeout == null)
                timeout = Settings.WaitForCompleteTimeOut;
            Button btnViewReports = Document.Button("rptViewer_ctl04_ctl00");
            timeout = btnViewReports.CustomWaitForVisibility(true, timeout);
            timeout = btnViewReports.CustomClick(timeout);
            WaitForReport(timeout);
            return IsReportRendered();
        }

        public bool ToolbarExists()
        {
            return Document.Span("rptViewer_Toolbar").Exists;
        }

        public int? WaitForReport(int? timeout = null, int? delay = 1)
        {
            if (timeout == null)
                timeout = Settings.WaitForCompleteTimeOut;
            Element reportsWait;
            if (delay.HasValue)
                do
                {
                    reportsWait = Util.Browser.GetElement<Div>(new Param("rptViewer_AsyncWait_Wait"));
                    if (reportsWait.IsVisible())
                        break;
                    Thread.Sleep(1000);
                    timeout -= 1;
                } while (--delay > 0);
            do
            {
                reportsWait = Util.Browser.GetElement<Div>(new Param("rptViewer_AsyncWait_Wait"));
                if (!reportsWait.IsVisible())
                    break;
                Thread.Sleep(1000);
            } while (--timeout >= 0);
            if (timeout < 0)
                throw new TimeoutException("Report timeout");
            return timeout;
        }

        /// <summary>
        /// Selects a checkbox from the checkbox collecton "Control"
        /// The elements are identivied each time as viewing a report destroys the elements
        /// </summary>
        /// <param name="buttonID"></param>
        /// <param name="collectionID"></param>
        /// <param name="index"></param>
        protected void SelectCheckBox(string buttonID, string collectionID, int index, int? timeout = null)
        {
            Image button = Document.Image(buttonID);
            timeout = button.CustomClick(timeout);
            Thread.Sleep(1000);
            CheckBoxCollection collection = Document.GetElement<Div>(new Param(collectionID)).CheckBoxes;
            collection[0].CustomSetCheckBox(false);
            collection[index].CustomSetCheckBox(true);
            timeout = button.CustomClick(timeout);
            Util.Browser.CustomWaitForComplete(timeout);
        }
    }
}
