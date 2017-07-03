using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;

namespace NetSteps.Validation.BatchProcess.Common
{
    public interface IBatchValidationService
    {
        void ProcessBatch(IRecordRepository repository, IRecordQuery query, ICollection<IRecordValidationResultManager> logWriters, Func<IEnumerable<IDependentDataService>> dependentServiceCollector);
    }
}
