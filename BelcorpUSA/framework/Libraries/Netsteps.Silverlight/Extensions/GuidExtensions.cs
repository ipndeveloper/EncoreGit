using System;

namespace NetSteps.Silverlight.Extensions
{
    public static class GuidExtensions
    {
        public static bool IsNullOrEmpty(this Guid value)
        {
            Guid emptyGuid = new Guid();
            if (value == null || emptyGuid.ToString() == value.ToString())
                return true;
            else
                return false;
        }
    }
}
