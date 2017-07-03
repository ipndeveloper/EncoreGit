using NetSteps.Validation.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.Common
{
    public interface IDependentDataService
    {
        string Name { get; }
        IRecordQuery QueryBase { get; set; }
        void Initialize();
    }
}
