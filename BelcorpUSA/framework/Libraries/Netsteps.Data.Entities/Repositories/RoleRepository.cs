using System;
using System.Data.Objects;
using System.Linq;

namespace NetSteps.Data.Entities.Repositories
{
    public partial class RoleRepository
    {
        #region Members
        protected override Func<NetStepsEntities, int, IQueryable<Role>> loadFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, int, IQueryable<Role>>(
                 (context, roleID) => from r in context.Roles
                                               .Include("Functions")
                                      where r.RoleID == roleID
                                      select r);
            }
        }

        protected override Func<NetStepsEntities, IQueryable<Role>> loadAllFullQuery
        {
            get
            {
                return CompiledQuery.Compile<NetStepsEntities, IQueryable<Role>>(
                 (context) => from r in context.Roles
                                               .Include("Functions")
                              select r);
            }
        }
        #endregion
    }
}
