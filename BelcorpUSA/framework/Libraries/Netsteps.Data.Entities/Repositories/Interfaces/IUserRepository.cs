using System.Collections.Generic;
using NetSteps.Data.Entities.Business;

namespace NetSteps.Data.Entities.Repositories
{
    public partial interface IUserRepository
    {
        User Authenticate(string username, string password);
        bool IsUsernameAvailable(int userID, string username);
        UserSlimSearchData LoadSlim(int userID);
        List<UserSiteWidget> LoadUserSiteWigets(int userID);
        Dictionary<int, string> SlimSearch(string query, int? userTypeID, int? userStatusID);
        User GetByUsername(string username);
    }
}
