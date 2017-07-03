using System;

namespace NetSteps.Common.Attributes
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Attribute to to translate Property names in UI.
    /// Created: 10/7/2010
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class TermNameAttribute : Attribute
    {
        public string TermName { get; set; }
        public string DefaultValue { get; set; }

        public TermNameAttribute(string termName)
        {
            TermName = termName;
        }
        public TermNameAttribute(string termName, string defaultValue)
        {
            TermName = termName;
            DefaultValue = defaultValue;
        }
    }
}
