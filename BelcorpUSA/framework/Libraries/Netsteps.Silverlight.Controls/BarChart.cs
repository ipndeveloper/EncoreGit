using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetSteps.Silverlight.Controls
{
    public class BarChart : Control
    {
        public BarChart()
            : base()
        {
            this.DefaultStyleKey = typeof(BarChart);

            this.SizeChanged += new SizeChangedEventHandler(BarChart_SizeChanged);
        }

        private void BarChart_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeBars();
        }

        private void resizeBars()
        {
            double availableWidth = (this.ActualWidth - TitleColumnWidth);
            if (availableWidth > 0)
            {
                RowOneWidth = availableWidth * RowOneWidthPercentage;
                RowTwoWidth = availableWidth * RowTwoWidthPercentage;
            }
        }

        public static readonly DependencyProperty RowOneWidthPercentageProperty = DependencyProperty.Register("RowOneWidthPercentage", typeof(double), typeof(BarChart), null);
        public double RowOneWidthPercentage
        {
            get
            {
                return (double)GetValue(RowOneWidthPercentageProperty);
            }
            set
            {
                if (value > 1)
                    value = value / 100;

                value = Math.Max(Math.Min(value, 1), 0);

                SetValue(RowOneWidthPercentageProperty, value);
            }
        }

        public static readonly DependencyProperty RowTwoWidthPercentageProperty = DependencyProperty.Register("RowTwoWidthPercentage", typeof(double), typeof(BarChart), null);
        public double RowTwoWidthPercentage
        {
            get
            {
                return (double)GetValue(RowTwoWidthPercentageProperty);
            }
            set
            {
                if (value > 1)
                    value = value / 100;

                value = Math.Max(Math.Min(value, 1), 0);

                SetValue(RowTwoWidthPercentageProperty, value);
            }
        }

        public static readonly DependencyProperty TitleColumnWidthProperty = DependencyProperty.Register("TitleColumnWidth", typeof(double), typeof(BarChart), null);
        public double TitleColumnWidth
        {
            get
            {
                return (double)GetValue(TitleColumnWidthProperty);
            }
            set
            {
                SetValue(TitleColumnWidthProperty, value);
            }
        }

        public static readonly DependencyProperty RowOneWidthProperty = DependencyProperty.Register("RowOneWidth", typeof(double), typeof(BarChart), null);
        public double RowOneWidth
        {
            get
            {
                return (double)GetValue(RowOneWidthProperty);
            }
            private set
            {
                SetValue(RowOneWidthProperty, value);
            }
        }

        public static readonly DependencyProperty RowTwoWidthProperty = DependencyProperty.Register("RowTwoWidth", typeof(double), typeof(BarChart), null);
        public double RowTwoWidth
        {
            get
            {
                return (double)GetValue(RowTwoWidthProperty);
            }
            private set
            {
                SetValue(RowTwoWidthProperty, value);
            }
        }

        public static readonly DependencyProperty TitleBackgroundBrushProperty = DependencyProperty.Register("TitleBackgroundBrush", typeof(Brush), typeof(BarChart), null);
        public Brush TitleBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(TitleBackgroundBrushProperty);
            }
            set
            {
                SetValue(TitleBackgroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty RowOneBackgroundBrushProperty = DependencyProperty.Register("RowOneBackgroundBrush", typeof(Brush), typeof(BarChart), null);
        public Brush RowOneBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(RowOneBackgroundBrushProperty);
            }
            set
            {
                SetValue(RowOneBackgroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty RowTwoBackgroundBrushProperty = DependencyProperty.Register("RowTwoBackgroundBrush", typeof(Brush), typeof(BarChart), null);
        public Brush RowTwoBackgroundBrush
        {
            get
            {
                return (Brush)GetValue(RowTwoBackgroundBrushProperty);
            }
            set
            {
                SetValue(RowTwoBackgroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty TitleForegroundBrushProperty = DependencyProperty.Register("TitleForegroundBrush", typeof(Brush), typeof(BarChart), null);
        public Brush TitleForegroundBrush
        {
            get
            {
                return (Brush)GetValue(TitleForegroundBrushProperty);
            }
            set
            {
                SetValue(TitleForegroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty RowOneForegroundBrushProperty = DependencyProperty.Register("RowOneForegroundBrush", typeof(Brush), typeof(BarChart), null);
        public Brush RowOneForegroundBrush
        {
            get
            {
                return (Brush)GetValue(RowOneForegroundBrushProperty);
            }
            set
            {
                SetValue(RowOneForegroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty RowTwoForegroundBrushProperty = DependencyProperty.Register("RowTwoForegroundBrush", typeof(Brush), typeof(BarChart), null);
        public Brush RowTwoForegroundBrush
        {
            get
            {
                return (Brush)GetValue(RowTwoForegroundBrushProperty);
            }
            set
            {
                SetValue(RowTwoForegroundBrushProperty, value);
            }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(string), typeof(BarChart), null);
        public string Title
        {
            get
            {
                return (string)GetValue(TitleProperty);
            }
            set
            {
                SetValue(TitleProperty, value);
            }
        }

        public static readonly DependencyProperty RowOneTextProperty = DependencyProperty.Register("RowOneText", typeof(string), typeof(BarChart), null);
        public string RowOneText
        {
            get
            {
                return (string)GetValue(RowOneTextProperty);
            }
            set
            {
                SetValue(RowOneTextProperty, value);
            }
        }

        public static readonly DependencyProperty RowTwoTextProperty = DependencyProperty.Register("RowTwoText", typeof(string), typeof(BarChart), null);
        public string RowTwoText
        {
            get
            {
                return (string)GetValue(RowTwoTextProperty);
            }
            set
            {
                SetValue(RowTwoTextProperty, value);
            }
        }
    }
}
