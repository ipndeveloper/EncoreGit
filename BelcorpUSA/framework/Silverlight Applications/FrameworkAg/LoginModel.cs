using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace FrameworkAg
{
    public class LoginModel
    {
        private string _userName;
        [Required(ErrorMessage = "Please enter your Username.")]
        public string UserName
        {
            get { return _userName; }
            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "UserName" });
                _userName = value;

                (AuthenticateUser as MyDelegateCommand<bool>).NotifyCanExecuteChanged();
            }
        }

        private string _password;
        [Required(ErrorMessage = "Please enter your Password.")]
        public string Password
        {
            get { return _password; }
            set
            {
                Validator.ValidateProperty(value, new ValidationContext(this, null, null) { MemberName = "Password" });
                _password = value;

                (AuthenticateUser as MyDelegateCommand<bool>).NotifyCanExecuteChanged();
            }
        }

        #region Events
        public event EventHandler LoginSuccessful;
        protected virtual void OnLoginSuccessful(object sender, EventArgs e)
        {
            if (LoginSuccessful != null)
                LoginSuccessful(this, e);
        }

        public event EventHandler LoginFailed;
        protected virtual void OnLoginFailed(object sender, EventArgs e)
        {
            if (LoginFailed != null)
                LoginFailed(this, e);
        }
        #endregion

        ICommand _authenticateUser;
        public ICommand AuthenticateUser
        {
            get
            {
                if (_authenticateUser == null)
                {
                    _authenticateUser = new MyDelegateCommand<bool>(
                      p =>
                      {
                          // TODO: Wire up real authentication later. - JHE
                          if (UserName == "test" && Password == "test")
                              OnLoginSuccessful(this, null);
                          else
                              OnLoginFailed(this, null);
                      },
                      p =>
                      {
                          //return (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrWhiteSpace(Password));
                          return true;
                      });
                }
                return (_authenticateUser);
            }
        }
    }
}