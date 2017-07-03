using System.Windows;
using System.Windows.Controls;

namespace NetSteps.Silverlight.Extensions
{
    public static class ButtonExtensions
    {
        public static bool IsVisible(this Button value)
        {
            return (value.Visibility == Visibility.Visible);
        }
    }
}
