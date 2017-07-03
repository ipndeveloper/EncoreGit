using WatiN.Core;

namespace NetSteps.Testing.Integration.GMP.Reports
{
    public class GMP_Reports_TextBox_Control : Control<Div>
    {
        private TextField _txt;
        private CheckBox _ckb;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _txt = Element.GetElement<TextField>();
            _ckb = Element.CheckBox(Find.Any);
        }

        public string Value
        {
            set
            {
                if (value == null)
                    _ckb.CustomSetCheckBox(true);
                else
                {
                    _ckb.CustomSetCheckBox(false);
                    _txt.CustomSetTextQuicklyHelper(value);
                    _txt.CustomRunScript(Util.strKeyUp);
                }
            }
        }
    }
}
