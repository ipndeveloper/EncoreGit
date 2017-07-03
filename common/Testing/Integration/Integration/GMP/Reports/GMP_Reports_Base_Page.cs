using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public abstract class GMP_Reports_Base_Page : GMP_Base_Page
    {
        protected TableCell _coreContent;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _coreContent = Document.GetElement<TableCell>(new Param("CoreContent", AttributeName.ID.ClassName));
        }

        #region Methods

        /// <summary>
        /// Global navigation control.
        /// </summary>
        public GMP_Reports_SectionNav_Control SectionNav()
        {
            //get { return _secNav.As<GMP_Reports_SectionNav_Control>(); }
            return _secNav.As<GMP_Reports_SectionNav_Control>();
        }

        protected TPage GoToIt<TPage>(Link lnk, string url, int? timeout = null) where TPage : GMP_Reports_Base_Page, new()
        {
            lnk.CustomClick(timeout);
            return Util.AttachBrowser<TPage>("ReportViewer.aspx");
        }

        protected new string Title
        {
            get { return _coreContent.ElementWithTag("h3", Find.Any).CustomGetText(); }
        }

        #endregion
    }
}
