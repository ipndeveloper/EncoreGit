using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.BatchProcess.Common
{
    public interface IRecordValidationResultManager
    {
        void AddValidation(IRecord validation);
        void NotifyValidationComplete();
        void OnFinished();
    }
}
