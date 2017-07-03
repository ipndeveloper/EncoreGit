using WatiN.Core;
using System;

namespace NetSteps.Testing.Integration.DWS.Performance
{    
    /// <summary>
    /// Title Progress
    /// </summary>
    public class DWS_Performance_Barometer_Control : DWS_Performance_TitleProgress_Control
    {
        private string _currentTitle;
        private string _payTitle;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
             _currentTitle = Element.Span("CurrentTitle").CustomGetText();
            _payTitle = Element.Span("PaidAsTitle").CustomGetText();
        }

        public string CurrentTitle
        {
            get { return _currentTitle; }
        }

        public string PayTitle
        {
            get { return _payTitle; }
        }

        public bool IsValid()
        {
            return Element.GetElement<Div>(new Param("ProgressIndicator", AttributeName.ID.ClassName)).Exists;
        }

        public bool IsValid(DWS_Performance_Barometer_Control barometer)
        {
            return
                (
                barometer.CurrentTitle == this.CurrentTitle &&
                barometer.PayTitle == this.PayTitle
                );
        }

    }
}
