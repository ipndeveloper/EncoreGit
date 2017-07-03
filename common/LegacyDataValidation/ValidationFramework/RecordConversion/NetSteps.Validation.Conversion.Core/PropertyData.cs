using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Validation.Conversion.Core
{
    internal class PropertyData
    {
        internal Func<object, object> PropertyValue { get; set; }
        internal Type PropertyType { get; set; }
    }
}
