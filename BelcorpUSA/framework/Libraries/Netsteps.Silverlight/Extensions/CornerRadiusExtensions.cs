using System.Windows;

namespace NetSteps.Silverlight.Extensions
{
    public static class CornerRadiusExtensions
    {
        public static CornerRadius FlipHorizontally(this CornerRadius cornerRadius)
        {
            return new CornerRadius(cornerRadius.TopRight, cornerRadius.TopLeft, cornerRadius.BottomLeft, cornerRadius.BottomRight);
        }

        public static CornerRadius FlipVertically(this CornerRadius cornerRadius)
        {
            return new CornerRadius(cornerRadius.BottomLeft, cornerRadius.BottomRight, cornerRadius.TopRight, cornerRadius.TopLeft);
        }
    }
}
