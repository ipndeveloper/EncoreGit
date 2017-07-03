using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Service
{
    public class RecordPropertyValidation : IRecordPropertyValidation
    {
        public RecordPropertyValidation(IRecordValidation validationRecord, object value)
        {
            ParentRecordValidation = validationRecord;
            OriginalValue = value;
        }

        public object OriginalValue { get; private set; }

        public IRecordValidation ParentRecordValidation { get; private set; }

        public PropertyValidationType ValidationInstruction { get; set; }

        public IRecordPropertyValidationResult Result { get; set; }
    }
}
