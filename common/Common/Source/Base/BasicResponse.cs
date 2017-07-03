using System;
using System.Runtime.Serialization;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A helper class to return a Success response along with a message for and error
    /// to offer an alternative to method calls that return a bool and take out/ref string parameters. 
    /// Created: 04-14-2010
    /// </summary>
    [Serializable]
    public class BasicResponse
    {
        public bool Success { get; set; }

        private string _message = string.Empty;
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }
    }
}
