using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Common
{
    /// <summary>
    /// Service to validate a saved database record
    /// </summary>
    [ContractClass(typeof(RecordValidationServiceContract))]
    public interface IRecordValidationService
    {
        /// <summary>
        /// Validates the specified record and returns a record validation result.
        /// </summary>
        /// <param name="recordToValidate">The record to validate.</param>
        /// <returns></returns>
        IRecord Validate(IRecord recordToValidate);
    }

    [ContractClassFor(typeof(IRecordValidationService))]
    internal abstract class RecordValidationServiceContract : IRecordValidationService
    {

        public IRecord Validate(IRecord recordToValidate)
        {
            Contract.Requires<ArgumentNullException>(recordToValidate != null);
            Contract.Ensures(Contract.Result<IRecord>() != null);
            throw new NotImplementedException();
        }
    }
}
