using System;

namespace NetSteps.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RequiresFunctionAttribute : Attribute
    {
        private string _function;
        public string Function
        {
            get { return _function; }
            private set { _function = value; }
        }

        public RequiresFunctionAttribute(string function)
        {
            this.Function = function;
        }
    }
}
