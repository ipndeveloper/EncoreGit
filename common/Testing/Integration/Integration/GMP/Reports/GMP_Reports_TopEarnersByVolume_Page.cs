using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_TopEarnersByVolume_Page : GMP_Reports_Viewer_Page
    {
        private SelectList _volumeType;
        private TextField _count;
        private TableRow _parameters;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _volumeType = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            _count = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectPeriods(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl03_ddDropDownButton", "rptViewer_ctl04_ctl03_divDropDown", index);
            WaitForReport();
        }

        public void SelectVolumeType(int index)
        {
            _volumeType.CustomSelectDropdownItem(index);
            WaitForReport();
        }

        public void EnterCount(int count)
        {
            _count.CustomSetTextQuicklyHelper(count.ToString());
        }
    }
}
