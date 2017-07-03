using System;
using System.Windows;

namespace NetSteps.Silverlight.Extensions
{
    public static class BoolExtensions
    {
        public static bool ToBool(this bool? value)
        {
            return (value == null) ? false : Convert.ToBoolean(value);
        }

        public static Visibility ToVisibility(this bool value)
        {
            return (value == true) ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
