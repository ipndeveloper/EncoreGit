
using System;
using System.Runtime.Serialization;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: A helper class to return a Success response along with a message for and error
    /// to offer an alternative to method calls that return a bool and take out/ref string parameters. 
    /// Created: 08-05-2010
    /// </summary>
    [Serializable]
    [DataContract]
    public class BasicResponseItem<T> : BasicResponse
    {
        [DataMember]
        public T Item { get; set; }
    }
}
