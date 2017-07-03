using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace NetSteps.SOD.Common.Exceptions
{
    public class SODException : Exception
    {
        public SODException()
            : base()
        {
        }

        public SODException(string message)
            : base(message)
        {
        }

        public SODException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SODException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class SODEmailExistsException : SODException
    {
        public SODEmailExistsException()
            : base()
        {
        }

        public SODEmailExistsException(string message)
            : base(message)
        {
        }

        public SODEmailExistsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SODEmailExistsException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class SODDistIDNotFoundException : SODException
    {
        public SODDistIDNotFoundException()
            : base()
        {
        }

        public SODDistIDNotFoundException(string message)
            : base(message)
        {
        }

        public SODDistIDNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SODDistIDNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    public class SODLoginFailedException : SODException
    {
        public SODLoginFailedException()
            : base()
        {
        }

        public SODLoginFailedException(string message)
            : base(message)
        {
        }

        public SODLoginFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public SODLoginFailedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
