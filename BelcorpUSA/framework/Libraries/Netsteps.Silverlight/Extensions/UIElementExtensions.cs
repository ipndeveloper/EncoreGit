using System.Windows;
using System.Windows.Media;

namespace NetSteps.Silverlight.Extensions
{
    public static class UIElementExtensions
    {
        public static Point GetActualPosition(this UIElement element)
        {
            return GetActualPosition(element, Application.Current.RootVisual);
        }

        /// <summary>
        /// http://www.silverlightshow.net/tips/How-to-get-the-relative-coordinates-of-an-element.aspx - JHE
        /// </summary>
        /// <param name="element"></param>
        /// <param name="parentControl"></param>
        /// <returns></returns>
        public static Point GetActualPosition(this UIElement element, UIElement parentControl)
        {
            try
            {
                GeneralTransform generalTransform = element.TransformToVisual(parentControl);
                Point offset = generalTransform.Transform(new Point(0, 0));
                return offset;
            }
            catch
            {
                return new Point(0, 0);
            }
        }

        public static bool IsInApplicationVisualSpace(this UIElement element)
        {
            // TODO: Try and find a way around having a Try/Catch for better performance - JHE
            try
            {
                bool returnValue = false;
                Point point = GetActualPosition(element, Application.Current.RootVisual);

                if (point.X == 0.0 && point.Y == 0.0)
                    returnValue = false;
                else if (point.X > 0.0 && point.Y > 0.0)
                    returnValue = true;

                return returnValue;
            }
            catch
            {
                return false;
            }
        }

        public static void Rotate(this UIElement element, double angle)
        {
            element.RenderTransform = new RotateTransform { Angle = angle };
        }
    }
}
