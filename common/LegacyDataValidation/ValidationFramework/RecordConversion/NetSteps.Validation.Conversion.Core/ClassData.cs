using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Conversion.Core
{
    internal class ClassData
    {
        public ClassData()
        {
            PropertyAccessors = new Dictionary<string, PropertyData>();
            CollectionAccessors = new Dictionary<string, PropertyData>();
        }

        public IDictionary<string, PropertyData> PropertyAccessors { get; private set; }

        public IDictionary<string, PropertyData> CollectionAccessors { get; private set; }

        public string TypeName { get; set; }
    }
}
