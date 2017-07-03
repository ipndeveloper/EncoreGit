using WatiN.Core;
using System.Threading;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.PWS.Enroll
{
    public class PWS_Enroll_Agreements_Page : PWS_Base_Page
    {
        private ElementCollection<CheckBox> _agreements;

        protected override void InitializeContents()
        {
            base.InitializeContents();
            _content.GetElement<CheckBox>().CustomWaitForExist();
            Thread.Sleep(2000);
            _agreements = _content.GetElements<CheckBox>();
        }

        public override bool IsPageRendered()
        {
            if (_agreements.Count > 0)
                return true;
            else
                return false;
        }

        public int AgreementCount
        {
            get { return _agreements.Count; }
        }

        public PWS_Enroll_Agreements_Page CheckAgreement(int index)
        {
            _agreements[index].CustomSetCheckBox(true);
            return this;
        }

        public PWS_Enroll_Agreements_Page CheckAllAgreements()
        {            
            foreach (CheckBox agreement in _agreements)
                agreement.CustomSetCheckBox(true);
            return this;
        }

        public TPage ClickContinue<TPage>(int? timeout = null, bool pageRequired = true) where TPage : NS_Page, new() 
        {
            timeout = _content.GetElement<Link>(new Param("Button", AttributeName.ID.ClassName, RegexOptions.None)).CustomClick(timeout);
            return Util.GetPage<TPage>(timeout, pageRequired);
        }
    }
}
