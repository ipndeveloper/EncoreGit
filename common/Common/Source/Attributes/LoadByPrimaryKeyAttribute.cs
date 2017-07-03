using System;

namespace NetSteps.Common.Attributes
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Attribute to identify the property on a class that is the primary key.
    /// Created: 8/18/2010
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class LoadByPrimaryKeyAttribute : Attribute
    {
        public LoadByPrimaryKeyAttribute()
        {
        }
    }
}
