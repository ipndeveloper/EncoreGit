using WatiN.Core;
using System.Threading;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_1099Listing_Page : GMP_Reports_Viewer_Page
    {
        private TableRow _parameters;
        private SelectList _year;
        private RadioButton _executiveDecryption;
        private RadioButton _noExecutiveDecryption;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _parameters = Document.GetElement<TableRow>(new Param("ParametersRowrptViewer"));
            _year = Document.GetElement<SelectList>(new Param("rptViewer_ctl04_ctl03_ddValue"));
            _executiveDecryption = Document.GetElement<RadioButton>(new Param("rptViewer_ctl04_ctl07_rbTrue"));
            _noExecutiveDecryption = Document.GetElement<RadioButton>(new Param("rptViewer_ctl04_ctl07_rbFalse"));
        }

        public override bool IsPageRendered()
        {
            return _parameters.Exists;
        }

        public void SelectYear(int index)
        {
            Thread.Sleep(2000);
            _year.CustomSelectDropdownItem(index);
            this.WaitForReport();
        }

        public void SelectState(int index)
        {
            SelectCheckBox("rptViewer_ctl04_ctl11_ddDropDownButton", "rptViewer_ctl04_ctl11_divDropDown", index);
        }

        /// <summary>
        /// Not all versions of this report have this selector
        /// </summary>
        /// <param name="decryption"></param>
        public void SelectExecutiveDecryption(bool decryption, int? timeout = null)
        {
            if (decryption)
            {
                if (_executiveDecryption.Exists)
                {
                    _executiveDecryption.CustomClick(timeout);
                }
            }
            else
            {
                if (_noExecutiveDecryption.Exists)
                {
                    _noExecutiveDecryption.CustomClick(timeout);
                }
            }
        }
    }
}
