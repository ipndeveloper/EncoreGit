using System;

namespace NetSteps.Common.Exceptions
{
    [Serializable]
    public class ConcurrencyException : System.ApplicationException
    {
        public ConcurrencyException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ConcurrencyException(string message)
            : base(message)
        {
        }
    }
}
