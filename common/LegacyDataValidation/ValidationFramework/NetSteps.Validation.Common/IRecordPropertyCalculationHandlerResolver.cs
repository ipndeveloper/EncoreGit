using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Common
{
    /// <summary>
    /// Registry for holding registered record handlers.
    /// </summary>
    [ContractClass(typeof(RecordValidationHandlerResolverContract))]
    public interface IRecordPropertyCalculationHandlerResolver
    {
        /// <summary>
        /// Gets the handler for the target record property.  If one is not found, returns the default handler (provides no calculation).
        /// </summary>
        /// <param name="recordKind">The record kind.</param>
        /// <param name="propertyName">The name of the target property.</param>
        /// <returns></returns>
        IRecordPropertyCalculationHandler GetHandler(string recordKind, string propertyName);
    }

    [ContractClassFor(typeof(IRecordPropertyCalculationHandlerResolver))]
    internal abstract class RecordValidationHandlerResolverContract : IRecordPropertyCalculationHandlerResolver
    {
        public IRecordPropertyCalculationHandler GetHandler(string recordKind, string propertyName)
        {
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(recordKind));
            Contract.Requires<ArgumentNullException>(!String.IsNullOrEmpty(propertyName));
            Contract.Ensures(Contract.Result<IRecordPropertyCalculationHandler>() != null);
            throw new NotImplementedException();
        }
    }
}
