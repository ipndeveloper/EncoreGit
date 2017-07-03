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

namespace FrameworkAg
{
    public partial class LoginView : UserControl
    {
        #region Members
        public ChildWindow Window { get; set; }
        #endregion

        #region Properties
        public new object DataContext
        {
            get
            {
                return LayoutRoot.DataContext;
            }
            set
            {
                if (value is LoginModel)
                    HookUpEvents(value as LoginModel);
                LayoutRoot.DataContext = value;
            }
        }
        #endregion

        #region Initialize
        public LoginView()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserControl_Loaded);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion

        #region Events
        #endregion

        #region Methods
        public void HookUpEvents(LoginModel loginModel)
        {
            loginModel.LoginSuccessful -= new EventHandler(loginModel_LoginSuccessful);
            loginModel.LoginSuccessful += new EventHandler(loginModel_LoginSuccessful);
            loginModel.LoginFailed -= new EventHandler(loginModel_LoginFailed);
            loginModel.LoginFailed += new EventHandler(loginModel_LoginFailed);
        }

        void loginModel_LoginFailed(object sender, EventArgs e)
        {
            uxErrorMessage.Text = "Login Failed";
        }

        void loginModel_LoginSuccessful(object sender, EventArgs e)
        {
            uxErrorMessage.Text = string.Empty;
            if (Window != null)
                Window.Close();
        }
        #endregion

        #region Event Handlers
        #endregion
    }
}
