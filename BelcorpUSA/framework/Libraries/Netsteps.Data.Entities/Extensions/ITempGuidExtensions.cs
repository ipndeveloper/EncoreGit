using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Interfaces;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: ITempGuid Extensions
    /// Created: 03-01-2011
    /// </summary>
    public static class ITempGuidExtensions
    {
        public static T GetByGuid<T>(this IEnumerable<T> list, string guid) where T : ITempGuid
        {
            guid = guid.Replace("-", string.Empty);
            return list.FirstOrDefault(i => i.Guid.ToString().Replace("-", string.Empty) == guid);
        }
    }
}
