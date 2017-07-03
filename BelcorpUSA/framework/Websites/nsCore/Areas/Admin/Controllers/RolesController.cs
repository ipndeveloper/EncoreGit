using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Web.Mvc.Attributes;
using NetSteps.Web.Mvc.Helpers;

namespace nsCore.Areas.Admin.Controllers
{
    public class RolesController : Controller
    {
        [FunctionFilter("Admin-Create and Edit Role", "~/Admin/Users")]
        public virtual ActionResult Index()
        {
            SmallCollectionCache.Instance.Roles.ExpireCache();
            List<int> accountTypeRoleIDs = SmallCollectionCache.Instance.AccountTypes.SelectMany(at => at.Roles).Select(r => r.RoleID).ToList();
            return View(accountTypeRoleIDs);
        }

        [FunctionFilter("Admin-Create and Edit Role", "~/Admin/Users")]
        public virtual ActionResult Edit(int? id)
        {
            try
            {
                return View(id.HasValue ? Role.LoadFull(id.Value) : new Role() { Functions = new TrackableCollection<Function>(), Editable = true});
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                throw exception;
            }
        }

        [FunctionFilter("Admin-Create and Edit Role", "~/Admin/Users")]
        public virtual ActionResult Save(string name, List<int> functionIds, int roleId = 0)
        {
            try
            {
                Role role;
                if (roleId > 0)
                    role = Role.LoadFull(roleId);
                else
                {
                    role = new Role();
                    role.RoleTypeID = (int)Constants.RoleType.NscoreRole;
                    role.TermName = name;
                    role.Editable = true;
                }

                if (role.Name != name && !string.IsNullOrEmpty(name) && CoreContext.CurrentLanguageID == Language.English.LanguageID)
                {
                    role.Name = name;
                }

                var term = TermTranslation.LoadTermTranslationByTermNameAndLanguageID(role.TermName, CoreContext.CurrentLanguageID);
                if (term != null && term.Term != name)
                {
                    term.Term = name;
                    term.Save();
                }

                //role.Functions.SyncTo(functionIds ?? new List<int>(), f => f.FunctionID, id => SmallCollectionCache.Instance.Functions.GetById(id));

                /* Cache was causing object graph multiple entities error when saving - TK */
                role.Functions.SyncTo(functionIds ?? new List<int>(), f => f.FunctionID, id => Function.Load(id));

                role.Save();

                SmallCollectionCache.Instance.Roles.ExpireCache();
                // Reload Roles (CurrentUser) if CurrentUser is affected by the edit Role. - JHE

                if ((CoreContext.CurrentUser as CorporateUser).User.Roles.Any(r => r.RoleID == roleId))
                    CoreContext.CurrentUser = CorporateUser.LoadFull((CoreContext.CurrentUser as CorporateUser).CorporateUserID);

                return Json(new { result = true, roleId = role.RoleID });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }


        [FunctionFilter("Admin-Create and Edit Role", "~/Admin/Users")]
        public virtual ActionResult Delete(int id)
        {
            try
            {
                if (id == 0)
                    return Json(new { result = false, message = "Invalid RoleID." });

                Role role = Role.LoadFull(id);
                if (role != null && role.Editable)
                {
                    role.Delete();
                }

                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                return Json(new { result = false, message = exception.PublicMessage });
            }
        }
    }
}
