using System.Windows.Media;

namespace NetSteps.Silverlight.Extensions
{
    public static class LinearGradientBrushExtensions
    {
        // Not tested yet - JHE
        public static LinearGradientBrush Reverse(this LinearGradientBrush linearGradientBrush)
        {
            GradientStopCollection gradientStopCollection = new GradientStopCollection();
            for (int i = linearGradientBrush.GradientStops.Count - 1; i >= 0; i--)
                gradientStopCollection.Add(linearGradientBrush.GradientStops[i]);

            linearGradientBrush.GradientStops = gradientStopCollection;
            return linearGradientBrush;
        }
    }
}
