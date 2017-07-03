using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Controls
{
    public partial class VideoPlayer : UserControl, IPopupChild, IStateChanged, IDisposable, IMainWindowModule, IVideoPlayback
    {
        #region Members
        private DispatcherTimer _statusTimer;

        private DoubleClickDispatcherTimer _doubleClickTimer = new DoubleClickDispatcherTimer();

        private string _text;
        private bool _playToStartPauseFrame;

        private bool _controlIsChangingValue = false;
        #endregion

        #region Properties
        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        public ImageSource _icon = "/Images/videoICO.png".ToImageSource();
        public ImageSource Icon
        {
            get
            {
                return _icon;
            }
            set
            {
                _icon = value;
            }
        }

        public string _imageSourceResourceKey = "";
        public string ImageSourceResourceKey
        {
            get
            {
                return _imageSourceResourceKey;
            }
            set
            {
                _imageSourceResourceKey = value;
            }
        }

        public bool _instanceIsPopup = false;
        public bool InstanceIsPopup
        {
            get
            {
                return _instanceIsPopup;
            }
            set
            {
                _instanceIsPopup = value;
            }
        }

        public Uri Source
        {
            get
            {
                return uxVideo.Source;
            }
            set
            {
                uxVideo.Source = value;
            }
        }

        public bool AutoPlay
        {
            get
            {
                return uxVideo.AutoPlay;
            }
            set
            {
                uxVideo.AutoPlay = value;

                if (!uxVideo.AutoPlay)
                    _playToStartPauseFrame = true;
            }
        }

        public MediaElementState CurrentState
        {
            get
            {
                return uxVideo.CurrentState;
            }
        }
        #endregion

        #region Initialize
        public VideoPlayer()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);
            uxVideo.CurrentStateChanged += new RoutedEventHandler(uxVideo_CurrentStateChanged);

            _statusTimer = new DispatcherTimer();
            _statusTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            _statusTimer.Tick += new EventHandler(_statusTimer_Tick);
            _statusTimer.Start();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Translation.TranslateRootControl(LayoutRoot);

            // Default the video to 3 second in - JHE
            if (_playToStartPauseFrame)
            {
                TimerHelper.SetTimeout(500, () =>
                {
                    this.Dispatcher.BeginInvoke(() =>
                    {
                        uxVideo.Position = TimeSpan.FromMilliseconds(3000);
                        uxVideo.Play();
                        TimerHelper.SetTimeout(50, () => { uxVideo.Pause(); });
                    });
                });
            }
        }
        #endregion

        #region Events
        public event StateEventHandler StateChanged;
        protected virtual void OnStateChanged(object sender, StateChangedEventArgs e)
        {
            if (StateChanged != null)
                StateChanged(this, e);
        }

        public event RoutedEventHandler CurrentStateChanged;
        protected virtual void OnCurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (CurrentStateChanged != null)
                CurrentStateChanged(this, e);
        }

        public event EventHandler CloseWindow;
        protected virtual void OnCloseWindow(object sender, EventArgs e)
        {
            if (CloseWindow != null)
                CloseWindow(this, e);
        }
        #endregion

        #region Methods
        public void Play()
        {
            uxVideo.Play();
        }

        public void Pause()
        {
            uxVideo.Pause();
        }

        public void Stop()
        {
            uxVideo.Stop();
        }
        #endregion

        #region Event Handlers
        private void LayoutRoot_MouseEnter(object sender, MouseEventArgs e)
        {
            ShowButtonsAnimation.Begin();

            btnPlay2.Visibility = Visibility.Visible;
        }

        private void LayoutRoot_MouseLeave(object sender, MouseEventArgs e)
        {
            HideButtonsAnimation.Begin();

            if (uxVideo.CurrentState == MediaElementState.Playing)
                btnPlay2.Visibility = Visibility.Collapsed;
            else
                btnPlay2.Visibility = Visibility.Visible;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (uxVideo.CurrentState == MediaElementState.Paused)
            {
                uxVideo.Play();
                PlaySymbol.Visibility = Visibility.Collapsed;
                PauseSymbol.Visibility = Visibility.Visible;

                PlaySymbol2.Visibility = Visibility.Collapsed;
                PauseSymbol2.Visibility = Visibility.Visible;
            }
            else if (uxVideo.CurrentState == MediaElementState.Playing)
            {
                uxVideo.Pause();
                PlaySymbol.Visibility = Visibility.Visible;
                PauseSymbol.Visibility = Visibility.Collapsed;

                PlaySymbol2.Visibility = Visibility.Visible;
                PauseSymbol2.Visibility = Visibility.Collapsed;
            }
            else
            {
                uxVideo.Stop();
                uxVideo.Play();
                PlaySymbol.Visibility = Visibility.Collapsed;
                PauseSymbol.Visibility = Visibility.Visible;

                PlaySymbol2.Visibility = Visibility.Collapsed;
                PauseSymbol2.Visibility = Visibility.Visible;
            }
        }

        private void uxVideo_CurrentStateChanged(object sender, RoutedEventArgs e)
        {
            if (uxVideo.CurrentState == MediaElementState.Buffering)
                uxBuffering.Visibility = Visibility.Visible;
            else
                uxBuffering.Visibility = Visibility.Collapsed;

            if (uxVideo.CurrentState == MediaElementState.Stopped)
            {
                uxBuffering.Visibility = Visibility.Collapsed;
                uxProgress.Value = 100;
                uxVideo.Stop();
            }
            else if (uxVideo.CurrentState == MediaElementState.Playing)
            {
                PlaySymbol.Visibility = Visibility.Collapsed;
                PauseSymbol.Visibility = Visibility.Visible;

                PlaySymbol2.Visibility = Visibility.Collapsed;
                PauseSymbol2.Visibility = Visibility.Visible;
            }
            else if (uxVideo.CurrentState == MediaElementState.Paused) //if the movie ends
            {
                PlaySymbol.Visibility = Visibility.Visible;
                PauseSymbol.Visibility = Visibility.Collapsed;

                PlaySymbol2.Visibility = Visibility.Visible;
                PauseSymbol2.Visibility = Visibility.Collapsed;
            }

            OnCurrentStateChanged(sender, e);
        }

        private void _statusTimer_Tick(object sender, EventArgs e)
        {
            if (uxVideo.CurrentState != MediaElementState.Opening)
            {
                // Not showing for now, since it will play with partial downloads and only buffing will stop the video - JHE
                //uxBuffering.Visibility = Visibility.Visible;
                //txtBuffering.Text = string.Format("Downloading {0}%", uxVideo.DownloadProgress.ToSilverlightPercentage());
            }
            else if (uxVideo.CurrentState != MediaElementState.Buffering)
            {
                uxBuffering.Visibility = Visibility.Visible;
                txtBuffering.Text = string.Format("Buffering {0}%", uxVideo.BufferingProgress.ToSilverlightPercentage());
            }

            if (uxVideo.DownloadProgress == 1 && (uxVideo.BufferingProgress == 1 || uxVideo.BufferingProgress == 0))
                uxBuffering.Visibility = Visibility.Collapsed;

            if (!(uxVideo.NaturalDuration.TimeSpan.TotalMilliseconds == 0 && uxVideo.Position.TotalMilliseconds == 0))
            {
                uxProgress.Value = (100 / uxVideo.NaturalDuration.TimeSpan.TotalMilliseconds * uxVideo.Position.TotalMilliseconds);

                _controlIsChangingValue = true;
                sldProgress.Maximum = uxVideo.NaturalDuration.TimeSpan.TotalMilliseconds;
                sldProgress.Value = uxVideo.Position.TotalMilliseconds;
                _controlIsChangingValue = false;
            }

            txtRemaining.Text = string.Format("{0}:{1} / {2}:{3}", uxVideo.Position.Minutes, uxVideo.Position.Seconds,
                uxVideo.NaturalDuration.TimeSpan.Minutes, uxVideo.NaturalDuration.TimeSpan.Seconds);
        }

        protected void doubleClickTimer_Tick(object sender, EventArgs e)
        {
            // stop the timer after an interval
            _doubleClickTimer.Stop();
        }

        private void LayoutRoot_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_doubleClickTimer.IsDoubleClick)
            {
                // TODO: Go to full screen on double click - JHE
                //OnStateChanged(this, new StateChangedEventArgs("ContactsMain", dgContacts.SelectedItem, "ContactView"));
                _doubleClickTimer.Stop();
            }
            else
            {
                _doubleClickTimer.Start();
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            uxVideo.Stop();
            PlaySymbol.Visibility = Visibility.Visible;
            PauseSymbol.Visibility = Visibility.Collapsed;

            PlaySymbol2.Visibility = Visibility.Visible;
            PauseSymbol2.Visibility = Visibility.Collapsed;
        }

        private void sldProgress_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            uxVideo.Position = TimeSpan.FromMilliseconds(sldProgress.Value);
        }

        private void sldProgress_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_controlIsChangingValue == false)
                uxVideo.Position = TimeSpan.FromMilliseconds(sldProgress.Value);
        }
        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            _statusTimer.Stop();
        }

        #endregion
    }
}
