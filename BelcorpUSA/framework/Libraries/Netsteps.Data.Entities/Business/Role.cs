using System;
using System.Linq;
using NetSteps.Authorization.Common;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Data.Entities
{
    public partial class Role
    {
        #region Methods
        public static void CreateRole(string roleName)
        {
            try
            {
                roleName = roleName.ToCleanString();

                if (!string.IsNullOrEmpty(roleName))
                {
                    // Use cache first
                    var roles = SmallCollectionCache.Instance.Roles.ToList();
                    var existingRole = roles.FirstOrDefault(f => f.Name.ToUpper() == roleName.ToUpper());

                    // Check DB next before creating a new function - JHE
                    if (existingRole == null)
                    {
                        roles = Role.LoadAll();
                        existingRole = roles.FirstOrDefault(f => f.Name.ToUpper() == roleName.ToUpper());
                    }

                    if (existingRole == null)
                    {
                        Role newRole = new Role();
                        newRole.StartEntityTracking();
                        newRole.RoleTypeID = Constants.RoleType.DistributorRole.ToShort();
                        newRole.Name = roleName;
                        newRole.Active = true;
                        newRole.Editable = false;
                        newRole.Save();
                    }
                }
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }

        public bool HasFunction(string title)
        {
            try
            {
				// call to authorization service to specifically block functions
				var authenticationService = Create.New<IAuthorizationService>();
				if (authenticationService.FilterAuthorizationFunctions(new string[] { title }).Count() == 0)
				{ 
					// function has been blocked
					return false;
				}

                var function = Functions.GetByName(title);

                if (function == null)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion
    }
}
