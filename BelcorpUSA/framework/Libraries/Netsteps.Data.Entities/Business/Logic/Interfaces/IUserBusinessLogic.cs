using System.Collections.Generic;
using NetSteps.Common.Base;

namespace NetSteps.Data.Entities.Business.Logic.Interfaces
{
    public partial interface IUserBusinessLogic
    {
        BasicResponse NewPasswordIsValid(string newPassword, string newPasswordConfirmation);
        void LogFailedLogin(User entity);
        void LogSucessfulLogin(User entity);
        string GeneratePassword();
        bool IsUsernameAvailable(Repositories.IUserRepository repository, int userID, string username);
        User Authenticate(Repositories.IUserRepository repository, string username, string password);
        UserSlimSearchData LoadSlim(Repositories.IUserRepository repository, int userID);
        List<UserSiteWidget> LoadUserSiteWigets(Repositories.IUserRepository repository, int userID);
        Dictionary<int, string> SlimSearch(Repositories.IUserRepository repository, string query, int? userTypeID = null, int? userStatusID = null);
        User GetByUserName(Repositories.IUserRepository repository, string username);
        bool SendResetPasswordEmail(string username, int siteId);
    }
}
