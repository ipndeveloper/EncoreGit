using System;

namespace NetSteps.Common.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Basic Name/Value class
    /// Created: 06-26-2009
    /// </summary>
    [Serializable]
    public class NameValue<N, V>
    {
        public N Name { get; set; }
        public V Value { get; set; }
        public NameValue() { }
        public NameValue(N name, V value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", Name.ToString(), Value.ToString());
        }
    }
}
