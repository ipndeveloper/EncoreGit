using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Validation.Common;
using NetSteps.Validation.Common.Model;

namespace NetSteps.Validation.Conversion.Core.Model
{
    public class RecordProperty : IRecordProperty
    {
        public RecordProperty(string PropertyName, object originalValue, IRecord parentRecord, RecordPropertyRole role, Type propertyType)
        {
            ValidationInstruction = PropertyValidationType.TreatedAsFact;
            OriginalValue = originalValue;
            ParentRecord = parentRecord;
            Name = PropertyName;
            PropertyRole = role;
            PropertyType = propertyType;
        }

        public PropertyValidationType ValidationInstruction { get; set; }

        public object OriginalValue { get; set; }

        public IRecord ParentRecord { get; private set; }

        public ValidationResultKind ResultKind { get; private set; }

        public object ExpectedValue { get; set; }

        public string Name { get; set; }

        public RecordPropertyRole PropertyRole { get; private set; }

        public Type PropertyType { get; private set; }

        public void SetResult(ValidationResultKind result)
        {
            ResultKind = ResultKind.EscalateTo(result);
        }
    }
}
