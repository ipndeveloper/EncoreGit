using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Common
{
    [ContractClass(typeof(RecordValidationHandlerContract))]
    public interface IRecordPropertyCalculationHandler
    {
        /// <summary>
        /// Calculates the specified property to calculate.
        /// </summary>
        /// <param name="propertyToCalculate">The property to calculate.</param>
        void Calculate(IRecordProperty propertyToCalculate);
    }

    [ContractClassFor(typeof(IRecordPropertyCalculationHandler))]
    internal abstract class RecordValidationHandlerContract : IRecordPropertyCalculationHandler
    {

        public void Calculate(IRecordProperty propertyToCalculate)
        {
            throw new NotImplementedException();
        }
    }
}
