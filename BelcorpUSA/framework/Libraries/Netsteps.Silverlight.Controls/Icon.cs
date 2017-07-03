using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace NetSteps.Silverlight.Controls
{
    [TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), TemplateVisualState(Name = "Disabled", GroupName = "CommonStates")]
    public class Icon : ContentControl
    {
        public Icon()
            : base()
        {
            this.DefaultStyleKey = typeof(Icon);
            this.Loaded += new RoutedEventHandler(Icon_Loaded);
            this.IsEnabledChanged += new DependencyPropertyChangedEventHandler(Icon_IsEnabledChanged);

            this.MouseLeftButtonDown += new MouseButtonEventHandler(Icon_MouseLeftButtonDown);
        }

        protected void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.RootVisual.LostMouseCapture -= new MouseEventHandler(RootVisual_LostMouseCapture);
            Application.Current.RootVisual.LostMouseCapture += new MouseEventHandler(RootVisual_LostMouseCapture);
        }

        void RootVisual_LostMouseCapture(object sender, MouseEventArgs e)
        {
            if (e.OriginalSource != this)
            {
                Application.Current.RootVisual.LostMouseCapture -= new MouseEventHandler(RootVisual_LostMouseCapture);
                this.IsMouseOver = false;
                UpdateVisualState(true);
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            UpdateVisualState(false);
        }

        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);

            this.IsMouseOver = true;
            this.UpdateVisualState(true);
        }

        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            this.IsMouseOver = false;
            this.UpdateVisualState(true);
        }

        protected void Icon_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateVisualState(false);
        }

        protected void Icon_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.IsMouseOver = false;
            this.UpdateVisualState(true);
        }

        public static readonly DependencyProperty IsMouseOverProperty = DependencyProperty.Register("IsMouseOver", typeof(bool), typeof(Icon), null);
        public bool IsMouseOver
        {
            get
            {
                return (bool)GetValue(IsMouseOverProperty);
            }
            internal set
            {
                SetValue(IsMouseOverProperty, value);
            }
        }

        public static readonly DependencyProperty NormalImageSourceProperty = DependencyProperty.Register("NormalImageSource", typeof(BitmapImage), typeof(Icon), null);
        public BitmapImage NormalImageSource
        {
            get
            {
                return (BitmapImage)GetValue(NormalImageSourceProperty);
            }

            set
            {
                SetValue(NormalImageSourceProperty, value);
            }
        }

        public static readonly DependencyProperty DisabledImageSourceProperty = DependencyProperty.Register("DisabledImageSource", typeof(BitmapImage), typeof(Icon), null);
        public BitmapImage DisabledImageSource
        {
            get
            {
                return (BitmapImage)GetValue(DisabledImageSourceProperty);
            }

            set
            {
                SetValue(DisabledImageSourceProperty, value);
            }
        }

        public static readonly DependencyProperty MouseOverImageSourceProperty = DependencyProperty.Register("MouseOverImageSource", typeof(BitmapImage), typeof(Icon), null);
        public BitmapImage MouseOverImageSource
        {
            get
            {
                return (BitmapImage)GetValue(MouseOverImageSourceProperty);
            }

            set
            {
                SetValue(MouseOverImageSourceProperty, value);
            }
        }

        internal void UpdateVisualState(bool useTransitions)
        {
            if (!IsEnabled && DisabledImageSource != null)
            {
                GoToState(useTransitions, "Disabled");
            }
            else if (IsMouseOver && MouseOverImageSource != null)
            {
                GoToState(useTransitions, "MouseOver");
            }
            else
            {
                GoToState(useTransitions, "Normal");
            }
        }

        protected bool GoToState(bool useTransitions, string stateName)
        {
            return VisualStateManager.GoToState(this, stateName, useTransitions);
        }
    }
}

