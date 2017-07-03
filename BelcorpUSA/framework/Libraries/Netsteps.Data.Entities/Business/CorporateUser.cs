using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Security;

namespace NetSteps.Data.Entities
{
    public partial class CorporateUser : IUser
    {
        #region Properties
        #endregion

        #region Methods
        public bool IsUsernameAvailable(string username)
        {
            return User.IsUsernameAvailable(UserID, username);
        }
        public static bool IsUsernameAvailable(int userID, string username)
        {
            return User.IsUsernameAvailable(userID, username);
        }

        public static CorporateUser Authenticate(string username, string password)
        {
            try
            {
                var user = User.Authenticate(username, password);

                var corporateUser = Repository.LoadByUserIdFull(user.UserID);
                if (corporateUser != null)
                {
                    corporateUser.StartTracking();
                    corporateUser.IsLazyLoadingEnabled = true;
                }
                return corporateUser;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public bool HasFunction(string title)
        {
            return User.HasFunction(title);
        }

        public void GrantSiteAccess(int siteID)
        {
            try
            {
                Repository.GrantSiteAccess(this, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void RevokeSiteAccess(int siteID)
        {
            try
            {
                Repository.RevokeSiteAccess(this, siteID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public bool HasAccessToSite(int siteID)
        {
            if (this.HasAccessToAllSites)
                return true;
            else
                return this.Sites.Count(s => s.SiteID == siteID) > 0;
        }

        public BasicResponse NewPasswordIsValid(string newPassword, string newPasswordConfirmation)
        {
            try
            {
                return User.NewPasswordIsValid(newPassword, newPasswordConfirmation);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<CorporateUser> LoadAll()
        {
            try
            {
                var list = Repository.LoadAll();
                foreach (var item in list)
                {
                    item.StartTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<CorporateUser> LoadAllFull()
        {
            try
            {
                var list = Repository.LoadAllFull();
                foreach (var item in list)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<CorporateUser> LoadBatch(IEnumerable<int> primaryKeys)
        {
            try
            {
                var list = Repository.LoadBatch(primaryKeys.ToList());
                foreach (var item in list)
                {
                    item.StartEntityTracking();
                    item.IsLazyLoadingEnabled = true;
                }
                return list;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<CorporateUserSearchData> SearchUsers(CorporateUserSearchParameters searchParameters)
        {
            try
            {
                return Repository.Search(searchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Use to get an encrypted Single-Sign-On token for use in URLs. - JHE
        /// </summary>
        /// <param name="accountID"></param>
        /// <returns></returns>
        public static string GetSingleSignOnToken(int userID)
        {
            try
            {
            	return HttpUtility.UrlEncode(GetEncryptedEncoded(userID.ToString()));
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        /// <summary>
        /// Use to get an decrypted Single-Sign-On token and return the AccountID to Sign-In. - JHE
        /// </summary>
        /// <param name="singleSignOnToken"></param>
        /// <returns></returns>
        public static int GetIdFromSingleSignOnToken(string singleSignOnToken)
        {
            try
            {
                string encryptedToken = DecryptEncoded(singleSignOnToken);
                return encryptedToken.IsValidInt() ? encryptedToken.ToInt() : 0;
            }
            catch (Exception ex)
            {
                EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
                return 0;
            }
        }

		public static string GetEncryptedEncoded(string data)
		{
			return Encrypt(HttpUtility.UrlEncode(data));
		}

		public static string DecryptEncoded(string token)
		{
			return Decrypt(HttpUtility.UrlDecode(token));
		}

		public static string Decrypt(string token)
		{
			string decrypted = Encryption.DecryptTripleDES(token, Encryption.SingleSignOnSalt);
			return decrypted;
		}

		public static string Encrypt(string data)
		{
			string encrypted = Encryption.EncryptTripleDES(data, Encryption.SingleSignOnSalt);
			return encrypted;
		}

        #endregion

        #region IUser Members

        int IUser.UserID
        {
            get
            {
                return User.UserID;
            }
            set
            {
                User.UserID = value;
            }
        }

        short IUser.UserTypeID
        {
            get
            {
                return User.UserTypeID;
            }
            set
            {
                User.UserTypeID = value;
            }
        }

        short IUser.UserStatusID
        {
            get
            {
                return User.UserStatusID;
            }
            set
            {
                User.UserStatusID = value;
            }
        }

        string IUser.Username
        {
            get
            {
                return User.Username;
            }
            set
            {
                User.Username = value;
            }
        }

        string IUser.Password
        {
            set { User.Password = value; }
        }

        string IUser.PasswordHash
        {
            get
            {
                return User.PasswordHash;
            }
            set
            {
                User.PasswordHash = value;
            }
        }

        string IUser.FirstName
        {
            get
            {
                return this.FirstName;
            }
            set
            {
                this.FirstName = value;
            }
        }

        string IUser.LastName
        {
            get
            {
                return this.LastName;
            }
            set
            {
                this.LastName = value;
            }
        }

        string IUser.EmailAddress
        {
            get
            {
                return this.Email;
            }
            set
            {
                this.Email = value;
            }
        }

        int IUser.LanguageID
        {
            get
            {
                return this.User.DefaultLanguageID;
            }
            set
            {
                this.User.DefaultLanguageID = value;
            }
        }

        bool IUser.HasFunction(string function)
        {
            return User.HasFunction(function);
        }

        public bool HasFunction(string function, bool checkAnonymousRole = true, bool checkWorkstationUserRole = false, int? accountTypeID = null)
        {
            return User.HasFunction(function, checkAnonymousRole, checkWorkstationUserRole);
        }

        public IEnumerable<string> FunctionNames
        {
            get
            {
                IEnumerable<string> result = this.User.Functions.Select(x => x.Name).ToList();

                return result;
            }
        }

        #endregion
    }
}
