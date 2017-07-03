namespace NetSteps.Common.Interfaces
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 02-05-2010
    /// </summary>
    public interface IUser
    {
        int UserID { get; set; }
        short UserTypeID { get; set; }
        short UserStatusID { get; set; }
        string Username { get; set; }
        string Password { set; }
        string PasswordHash { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string EmailAddress { get; set; }
        int LanguageID { get; set; }

        bool HasFunction(string function);
        bool HasFunction(string function, bool checkAnonymousRole = true, bool checkWorkstationUserRole = false, int? accountTypeID = null);
    }
}
