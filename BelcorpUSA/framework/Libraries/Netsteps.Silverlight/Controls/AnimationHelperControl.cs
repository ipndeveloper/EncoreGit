using System;
using System.Windows;
using System.Windows.Controls;

namespace NetSteps.Silverlight.Controls
{
    /// <summary>
    ///  From:
    ///  http://www.robertjantuit.nl/index.php/2008/09/13/storyboard-animation-for-the-scrollviewer-animationhelpercontrol/
    /// </summary>
    public class AnimationHelperControl : Control
    {
        public AnimationHelperControl()
        {

            Visibility = Visibility.Collapsed;

        }

        public double DoubleValue
        {

            get { return (double)GetValue(DoubleValueProperty); }

            set
            {

                SetValue(DoubleValueProperty, value);

                DoubleValueChanged(value);

            }

        }

        public static readonly DependencyProperty DoubleValueProperty =

            DependencyProperty.Register("DoubleValue", typeof(double), typeof(AnimationHelperControl), new PropertyMetadata((sender, e) =>
                {
                    if (e.OldValue != e.NewValue)
                    {
                        var ah = (AnimationHelperControl)sender;
                        ah.DoubleValue = (double)e.NewValue;
                    }
                }));

        public event EventHandler<DoubleEventArgs> DoubleValueChange;

        private void DoubleValueChanged(double newValue)
        {

            if (DoubleValueChange != null)

                DoubleValueChange(this, new DoubleEventArgs() { Value = newValue });

        }
    }

    public class DoubleEventArgs : EventArgs
    {
        private double _value;

        public double Value
        {

            get { return _value; }

            set { _value = value; }

        }
    }
}
