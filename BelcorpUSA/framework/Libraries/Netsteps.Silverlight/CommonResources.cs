using System.Windows.Media;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight
{
    public static class CommonResources
    {
        #region Gradient Brushes
        public static class GradientBrushes
        {
            public static LinearGradientBrush WhiteToWhite
            {
                get
                {
                    return "#FFFFFFFF".ToColor().ToLinearGradientBrush("#FFFFFFFF".ToColor());
                }
            }

            public static LinearGradientBrush GreyToWhite
            {
                get
                {
                    return "#FFeaeaea".ToColor().ToLinearGradientBrush("#FFFFFFFF".ToColor());
                }
            }

            public static LinearGradientBrush DarkGreyToWhite
            {
                get
                {
                    return "#FFC9C9C9".ToColor().ToLinearGradientBrush("#FFE4E4E4".ToColor());
                }
            }

            public static LinearGradientBrush DarkGreenToGreen
            {
                get
                {
                    return "#FF71bb49".ToColor().ToLinearGradientBrush();
                }
            }

            public static LinearGradientBrush DarkBlueToBlue
            {
                get
                {
                    return "#FF4987bb".ToColor().ToLinearGradientBrush();
                }
            }

            public static LinearGradientBrush DarkRedToRed
            {
                get
                {
                    return "#FFe22c00".ToColor().ToLinearGradientBrush();
                }
            }

            public static LinearGradientBrush DarkOrangeToOrange
            {
                get
                {
                    return "#FFce9400".ToColor().ToLinearGradientBrush();
                }
            }

            public static LinearGradientBrush DarkPurpleToPurple
            {
                get
                {
                    return "#FF573870".ToColor().ToLinearGradientBrush();
                }
            }
        }
        #endregion

        #region Solid Brushes
        public static class SolidBrushes
        {
            public static SolidColorBrush White
            {
                get
                {
                    return new SolidColorBrush("#FFFFFF".ToColor());
                }
            }

            public static SolidColorBrush Black
            {
                get
                {
                    return new SolidColorBrush("#000000".ToColor());
                }
            }

            public static SolidColorBrush DarkGrey
            {
                get
                {
                    return new SolidColorBrush("#a8a8a8".ToColor());
                }
            }

            public static SolidColorBrush Transparent
            {
                get
                {
                    return new SolidColorBrush("#00000000".ToColor());
                }
            }

            public static SolidColorBrush Orange
            {
                get
                {
                    return new SolidColorBrush("#FFffa800".ToColor());
                }
            }

            public static SolidColorBrush Yellow
            {
                get
                {
                    return new SolidColorBrush("#FFfcff00".ToColor());
                }
            }

            public static SolidColorBrush Red
            {
                get
                {
                    return new SolidColorBrush("#FFff0000".ToColor());
                }
            }

            public static SolidColorBrush Blue
            {
                get
                {
                    return new SolidColorBrush("#FF2a00ff".ToColor());
                }
            }

            public static SolidColorBrush Green
            {
                get
                {
                    return new SolidColorBrush("#FF0cff00".ToColor());
                }
            }

            public static SolidColorBrush Purple
            {
                get
                {
                    return new SolidColorBrush("#FF9c00ff".ToColor());
                }
            }
        }
        #endregion
    }
}
