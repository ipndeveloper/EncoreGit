using System.Windows;
using System.Windows.Shapes;

namespace NetSteps.Silverlight.Extensions
{
    public static class LineExtensions
    {
        public static bool IsInVisualAppSpace(this Line line)
        {
            bool returnValue = false;
            // First get point - JHE
            Point point = UIElementExtensions.GetActualPosition(line, Application.Current.RootVisual);

            // TODO: Figure out the App size - JHE

            double width = (line.X1 > line.X2) ? line.X1 - line.X2 : line.X2 - line.X1;
            double height = (line.Y1 > line.Y2) ? line.Y1 - line.Y2 : line.Y2 - line.Y1;

            double endPointX = point.X + width;

            if (endPointX > 0 || endPointX > 0)
                returnValue = true;

            return returnValue;
        }

        public static bool IsVerticalLine(this Line line)
        {
            return line.X1 == line.X2 && line.Y1 != line.Y2;
        }

        public static bool IsHorizontalLine(this Line line)
        {
            return line.Y1 == line.Y2 && line.X1 != line.X2;
        }

        public static bool IsDiagonalLine(this Line line)
        {
            return (!line.IsVerticalLine() && !line.IsHorizontalLine());
        }

    }
}
