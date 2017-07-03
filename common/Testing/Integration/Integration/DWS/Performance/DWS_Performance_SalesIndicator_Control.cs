using WatiN.Core;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_SalesIndicator_Control : Control<Div>
    {
        /// <summary>
        /// Sales Indicator
        /// </summary>
        public float Volume
        {
            get { return float.Parse(Element.Span("Volume").CustomGetText(), System.Globalization.NumberStyles.Currency); }
        }
    }
}
