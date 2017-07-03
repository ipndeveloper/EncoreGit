using System;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace NetSteps.Silverlight
{
    public class HtmlControlHelper : IErrorMessage
    {
        #region Members
        private string _href;
        private Visibility _visibility;
        private UserControl _parent;

        private DispatcherTimer _sizeUpdateTimer;
        #endregion

        #region Properties
        private HtmlElement Div { get; set; }
        private HtmlElement IFrame { get; set; }
        private HtmlWindow Window { get; set; }
        public HtmlDocument Doc { get; set; }
        public bool IsLoaded { get; set; }

        public string Href
        {
            get
            {
                if (string.IsNullOrEmpty(_href))
                    return string.Empty;
                return _href;
            }
            set
            {
                if (value != Href)
                    Navigate(value);
                _href = value;
            }
        }

        public Visibility Visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                _visibility = value;
                IFrameVisibility(value);
            }
        }

        public UserControl Parent
        {
            get
            {
                return _parent;
            }
        }

        #endregion

        #region Initialize
        public HtmlControlHelper()
        {
            Div = HtmlPage.Document.GetElementById("HtmlContainer");
            IFrame = HtmlPage.Document.GetElementById("HtmlFrame");
            //Window = IFrame.GetProperty("contentWindow") as HtmlWindow;
            //IFrame.AttachEvent("onload", new EventHandler<HtmlEventArgs>(Content_PageLoaded));
        }

        public void SetControl(UserControl parent)
        {
            // TODO: Dispose of old Parent if not null

            _parent = parent;
            Parent.LayoutUpdated += new EventHandler(HTMLControl_LayoutUpdated);
            Parent.Loaded += new RoutedEventHandler((o, e) =>
            {
                SetSize();
            });

            _sizeUpdateTimer = new DispatcherTimer();
            _sizeUpdateTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            _sizeUpdateTimer.Tick += new EventHandler(_sizeUpdateTimer_Tick);
            _sizeUpdateTimer.Start();
            TimerHelper.SetTimeout(2000, () => { _sizeUpdateTimer.Stop(); });
        }

        #endregion

        #region Events
        public event EventHandler<HtmlEventArgs> HtmlPageLoaded;
        public void OnHtmlPageLoaded(HtmlEventArgs args)
        {
            if (HtmlPageLoaded != null)
                HtmlPageLoaded(this, args);
        }

        public event ErrorEventHandler ErrorOccured;
        protected virtual void OnErrorOccured(object sender, ErrorEventArgs e)
        {
            if (ErrorOccured != null)
                ErrorOccured(this, e);
        }
        #endregion

        #region Methods
        private void Navigate(string url)
        {
            Doc = null;
            //if (IsLoaded && !string.IsNullOrEmpty(url))
            if (!string.IsNullOrEmpty(url))
            {
                IsLoaded = false;
                HideIframe();
                Window.Navigate(ApplicationContext.ResolveUrlWithHtmlPageAsBase(url));
            }
        }

        public void SetSize()
        {
            try
            {
                if (this.Visibility == Visibility.Visible)
                {
                    UIElement root = Application.Current.RootVisual as UIElement;
                    GeneralTransform gt = Parent.TransformToVisual(root);
                    Point pos = gt.Transform(new Point(0, 0));
                    Div.SetStyleAttribute("left", pos.X.ToString() + "px");
                    Div.SetStyleAttribute("top", pos.Y.ToString() + "px");
                    Div.SetStyleAttribute("width", Parent.ActualWidth.ToString() + "px");
                    Div.SetStyleAttribute("height", Parent.ActualHeight.ToString() + "px");
                }
            }
            catch (Exception ex)
            {
                // Swallowing this for now. - JHE
                //OnErrorOccured(this, new ErrorEventArgs(ex));
            }
        }

        private void IFrameVisibility(Visibility value)
        {
            string display;
            if (value == Visibility.Visible)
                display = "inline";
            else
                display = "none";

            Div.SetStyleAttribute("display", display);
        }

        public void HideIframe()
        {
            IFrameVisibility(_visibility);
        }

        public void RestoreIframe()
        {
            IFrameVisibility(_visibility);
        }

        #endregion

        #region Event Handlers
        private void HTMLControl_LayoutUpdated(object sender, EventArgs e)
        {
            SetSize();
        }

        private void Content_PageLoaded(object sender, HtmlEventArgs e)
        {
            Doc = IFrame.GetProperty("document") as HtmlDocument;
            IsLoaded = true;
            OnHtmlPageLoaded(e);
            RestoreIframe();
        }

        private void _sizeUpdateTimer_Tick(object sender, EventArgs e)
        {
            if (this.Visibility == Visibility.Visible)
                SetSize();
        }
        #endregion
    }
}
