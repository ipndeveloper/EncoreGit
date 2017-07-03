using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Core;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Common
{
    public class NonValidatingHandler : IRecordPropertyCalculationHandler
    {
        public virtual void Calculate(IRecordProperty propertyToCalculate)
        {
            propertyToCalculate.ExpectedValue = propertyToCalculate.OriginalValue;
            propertyToCalculate.SetResult(ValidationResultKind.IsFactual);
        }
    }
}
