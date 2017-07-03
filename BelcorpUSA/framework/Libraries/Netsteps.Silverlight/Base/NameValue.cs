
namespace NetSteps.Silverlight.Base
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Basic Name/Value class
    /// Created: 06-26-2009
    /// </summary>
    public class NameValue<N, V>
    {
        public N Name { get; set; }
        public V Value { get; set; }    public NameValue() { }
        public NameValue(N name, V value)
        {
            Name = name;
            Value = value;
        }
    }
}
