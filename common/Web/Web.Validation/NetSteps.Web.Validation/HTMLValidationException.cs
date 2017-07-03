using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Web.Validation
{
    public class HTMLValidationException : Exception
    {
        public HTMLValidationException()
            : base()
        { }

        public HTMLValidationException(string message)
            : base(message)
        { }

        public HTMLValidationException(string message, Exception innerException)
            : base(message, innerException)
        { }

        public HTMLValidationException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
