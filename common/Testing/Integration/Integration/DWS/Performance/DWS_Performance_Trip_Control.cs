using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_Trip_Control : Control<Div>
    {
        Span _currentPoints;

        protected override void  InitializeContents()
        {
 	         base.InitializeContents();
            _currentPoints = Element.Span("incentiveCurPoints");
        }

        public float Progress
        {
            get { return float.Parse(_currentPoints.CustomGetText(), System.Globalization.NumberStyles.Currency); }
        }

    }
}
