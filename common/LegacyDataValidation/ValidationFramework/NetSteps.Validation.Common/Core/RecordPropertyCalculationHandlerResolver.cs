using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.IoC;
using NetSteps.Validation.Common;

namespace NetSteps.Validation.Common.Core
{
    public class RecordPropertyCalculationHandlerResolver : IRecordPropertyCalculationHandlerResolver
    {
        private readonly IRecordPropertyCalculationHandler _defaultHandler;
        private Func<string, string, IRecordPropertyCalculationHandler> _factory;
        private Func<string, string, bool> _hasSpecificHandlerRegistered;

        public RecordPropertyCalculationHandlerResolver(Func<string, string, IRecordPropertyCalculationHandler> recordValidationFactory, Func<string, string, bool> hasSpecificHandlerRegistered)
        {
            _defaultHandler = new NonValidatingHandler();
            _factory = recordValidationFactory;
            _hasSpecificHandlerRegistered = hasSpecificHandlerRegistered;
        }

        public IRecordPropertyCalculationHandler GetHandler(string recordKind, string propertyName)
        {
            if (!_hasSpecificHandlerRegistered(recordKind, propertyName))
            {
                return _defaultHandler;
            }
            else
            {
                var handler = _factory(recordKind, propertyName);
                return handler;
            }
        }
    }
}
