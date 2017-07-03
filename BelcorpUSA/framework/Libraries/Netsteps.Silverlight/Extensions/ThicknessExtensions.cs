using System.Windows;

namespace NetSteps.Silverlight.Extensions
{
    public static class ThicknessExtensions
    {
        public static Thickness Add(this Thickness thickness, Thickness thicknessToAdd)
        {
            return new Thickness(thickness.Left + thicknessToAdd.Left, thickness.Top + thicknessToAdd.Top, thickness.Right + thicknessToAdd.Right, thickness.Bottom + thicknessToAdd.Bottom);
        }

        public static Thickness FlipHorizontally(this Thickness thickness)
        {
            return new Thickness(thickness.Right, thickness.Top, thickness.Left, thickness.Bottom);
        }

        public static Thickness FlipVertically(this Thickness thickness)
        {
            return new Thickness(thickness.Left, thickness.Bottom, thickness.Right, thickness.Top);
        }
    }
}
