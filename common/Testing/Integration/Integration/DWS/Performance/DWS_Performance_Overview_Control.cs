using System;
using WatiN.Core;
using System.Text.RegularExpressions;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    public class DWS_Performance_Overview_Control : Control<Div>
    {
        public string GetValue(int index)
        {
            return Element.GetElement<Span>(new Param("FR", AttributeName.ID.ClassName, RegexOptions.None).And(new Param(index))).CustomGetText();
        }
    }
}
