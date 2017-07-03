using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO;
using NetSteps.Silverlight;

namespace FrameworkAg
{
    public partial class MainPage : UserControl
    {
        #region Members
        #endregion

        #region Properties
        #endregion

        #region Initialize
        public MainPage()
        {
            InitializeComponent();
            AppFactory.Dispatcher = this.Dispatcher;
            Loaded += new RoutedEventHandler(UserControl_Loaded);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //loadingAnimationControl1.StartAnimation("test");
            //busyIndicator1.IsBusy = true;

            //PopupLogin();
            //PopupEmail();
            PopupFileBrowser();
        }
        #endregion

        #region Events
        #endregion

        #region Methods
        private void PopupLogin()
        {
            // TODO: Find re-sizeable behavior for ChildWindow - JHE
            ChildWindow window = new ChildWindow();
            window.HasCloseButton = false;
            LoginView view = new LoginView()
            {
                DataContext = new LoginModel(),
                Window = window
            };
            window.Content = view;
            window.Show();

            window.Closed += new EventHandler(window_Closed);
        }

        private void PopupEmail()
        {
            ChildWindow window = new ChildWindow();
            EmailCompose view = new EmailCompose()
            {
                DataContext = new Email()
            };
            window.Content = view;
            window.Show();
        }

        private void PopupFileBrowser()
        {
            ChildWindow window = new ChildWindow();
            FileBrowser view = new FileBrowser();
            window.Content = view;
            window.Show();
        }

        void window_Closed(object sender, EventArgs e)
        {
            PopupEmail();
        }
        #endregion

        #region Event Handlers
        
        #endregion
    }
}
