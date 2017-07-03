using System;

namespace NetSteps.Data.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreOnValidationAttribute : Attribute
    {
        public IgnoreOnValidationAttribute()
        {
        }
    }
}
