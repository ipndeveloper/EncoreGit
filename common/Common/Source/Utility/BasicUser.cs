
namespace NetSteps.Common.Utility
{
    public class BasicUser
    {
        public string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
            }
        }

        public string _password;
        public string Password
        {
            get { return _password; }
            set { _password = value; }
        }
    }
}
