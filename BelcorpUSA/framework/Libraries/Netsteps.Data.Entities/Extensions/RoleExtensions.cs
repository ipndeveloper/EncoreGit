using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Role Extensions
    /// Created: 06-25-2010
    /// </summary>
    public static class RoleExtensions
    {
        public static bool ContainsRole(this IEnumerable<Role> roles, int roleID)
        {
            return roles.Count(f => f.RoleID == roleID) > 0;
        }

        public static Role GetByName(this IEnumerable<Role> roles, string name)
        {
            return roles.FirstOrDefault(f => f.Name == name);
        }
    }
}
