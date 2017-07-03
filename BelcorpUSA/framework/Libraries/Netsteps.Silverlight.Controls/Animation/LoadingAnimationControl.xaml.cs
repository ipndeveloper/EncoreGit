using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Controls
{
    public partial class LoadingAnimationControl : UserControl
    {
        private int _displayDelay = 1;
        private int _busyProcessCount = 0;

        #region Initialize
        public LoadingAnimationControl()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);
            StopAnimation();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var bmc = new BitmapCache();
            AnimationImage.CacheMode = bmc;

            //Translation.TranslateRootControl(LayoutRoot);
        }
        #endregion

        #region Methods
        public void StartAnimation(string displayText)
        {
            this.Dispatcher.BeginInvoke(() =>
            {
                txtDisplay.Text = displayText;
            });
            _busyProcessCount = _busyProcessCount + 1;
            TimerHelper.SetTimeout(_displayDelay, () => { UpdateAnimationVisibility(); });
        }

        public void StartAnimation()
        {
            string Term = Translation.GetTerm("Controls_Loading", "Loading...");
            StartAnimation(Term);
        }

        public void StopAnimation()
        {
            if (_busyProcessCount > 0)
                _busyProcessCount = _busyProcessCount - 1;
            UpdateAnimationVisibility();
        }

        public void StopAnimation(bool resetProcessCount)
        {
            if (resetProcessCount)
                _busyProcessCount = 0;
            StopAnimation();
        }

        public void UpdateAnimationVisibility()
        {
            Visibility visibility = (_busyProcessCount > 0) ? Visibility.Visible : Visibility.Collapsed;
            this.Dispatcher.BeginInvoke(() =>
            {
                //if (visibility == Visibility.Visible)
                //    StartWaitingAnimation.Begin();
                //else
                //    StartWaitingAnimation.Stop();

                LayoutRoot.Visibility = visibility;
            });
        }
        #endregion
    }
}
