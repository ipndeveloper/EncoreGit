using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_CriticalInventoryLevels_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private SelectList _organization;
        private TextField _bufferPercentMin;
        private TextField _onhandMin;
        private TextField _allocatedMin;
        private TextField _availableMin;
        private TextField _bufferMin;
        private GMP_Reports_TextBox_Control _bufferPercentMax;
        private GMP_Reports_TextBox_Control _onhandMax;
        private GMP_Reports_TextBox_Control _allocatedMax;
        private GMP_Reports_TextBox_Control _avaliableMax;
        private GMP_Reports_TextBox_Control _bufferMax;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _organization = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl05_ddValue"));
            _bufferPercentMin = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl07_txtValue"));
            _onhandMin = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl11_txtValue"));
            _allocatedMin = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl15_txtValue"));
            _availableMin = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl19_txtValue"));
            _bufferMin = Document.GetElement<TextField>(new Param("rptViewer_ctl04_ctl23_txtValue"));
            _bufferPercentMax = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl09")));
            _onhandMax = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl13")));
            _allocatedMax = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl17")));
            _avaliableMax = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl21")));
            _bufferMax = Control.CreateControl<GMP_Reports_TextBox_Control>(Document.GetElement<Div>(new Param("rptViewer_ctl04_ctl25")));

        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectWarehouse(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl03_ddDropDownButton", "rptViewer_ctl04_ctl03_divDropDown", index);
        }

        public void SelectOrganization(int index)
        {
            _organization.CustomSelectDropdownItem(index, runScript:false);
            WaitForReport();
        }

        public void EnterBuffertPercentMinimum(int count)
        {
            _bufferPercentMin.CustomSetTextQuicklyHelper(count.ToString());
        }

        public void EnterOnhandMinimum(int count)
        {
            _onhandMin.CustomSetTextQuicklyHelper(count.ToString());
        }

        public void EnterAllocatedMinimum(int count)
        {
            _allocatedMin.CustomSetTextQuicklyHelper(count.ToString());
        }

        public void EnterAvailableMinimum(int count)
        {
            _availableMin.CustomSetTextQuicklyHelper(count.ToString());
        }

        public void EnterBufferMinimum(int count)
        {
            _bufferMin.CustomSetTextQuicklyHelper(count.ToString());
        }

        public void EnterBuffertPercentMaximum(int? count)
        {
           
            _bufferPercentMax.Value = (count == null) ? null : count.ToString();
        }

        public void EnterOnhandMaximum(int? count)
        {
            _onhandMax.Value = (count == null) ? null : count.ToString();
        }

        public void EnterAllocatedMaximum(int? count)
        {
            _allocatedMax.Value = (count == null) ? null : count.ToString();
        }

        public void EnterAvailableMaximum(int? count)
        {
            _avaliableMax.Value = (count == null) ? null : count.ToString();
        }

        public void EnterBufferMaximum(int? count)
        {
            _bufferMax.Value = (count == null) ? null : count.ToString();
        }
    }
}
