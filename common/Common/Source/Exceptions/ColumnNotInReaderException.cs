using System;

namespace NetSteps.Common.Exceptions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Exception thrown by data layer when trying to access a column that is not
    /// returned by the database in the DataReader.
    /// Created: 01-03-2009
    /// </summary>
    [Serializable]
    public class ColumnNotInReaderException : System.Exception
    {
        public ColumnNotInReaderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ColumnNotInReaderException(string message)
            : base(message)
        {
        }
    }
}
