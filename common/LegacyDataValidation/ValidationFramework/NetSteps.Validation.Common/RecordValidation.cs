using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Service
{
    public class RecordValidation : IRecordValidation
    {
        public RecordValidation(IRecord record)
        {
            Result = new RecordValidationResult();
            PropertyValidations = new Dictionary<string, IRecordPropertyValidation>();
            ChildRecordValidations = new List<IRecordValidation>();
        }

        public RecordValidation(IRecordValidation parentValidation, IRecord record) : this(record)
        {
            ParentRecord = parentValidation;
        }

        public RecordValidationResult Result { get; private set; }

        public IRecord Record { get; private set; }

        public IDictionary<string, IRecordPropertyValidation> PropertyValidations { get; private set; }

        public IList<IRecordValidation> ChildRecordValidations { get; private set; }

        public IRecordValidation ParentRecord { get; private set; }
    }
}
