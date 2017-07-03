using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.BatchProcess.TempTableWriters.Implementation.Model
{
    public class RecordProperty
    {
        public RecordProperty(bool isKey, string name, object value, Type propertyType)
        {
            IsKey = isKey;
            Name = name;
            Value = value;
            PropertyType = propertyType;
        }
        public bool IsKey { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }
        public Type PropertyType { get; private set; }
    }
}
