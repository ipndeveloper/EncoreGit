
namespace NetSteps.Silverlight
{
    public class Security
    {
        public static bool IsUserAuthenticated
        {
            get
            {
                return Variables.IsolatedStorage.Get<bool>("IsUserAuthenticated");
            }
            set
            {
                Variables.IsolatedStorage.Set<bool>("IsUserAuthenticated", value);
            }
        }
    }
}
