using System.Collections.Generic;
using System.Linq;

namespace NetSteps.Data.Entities.Extensions
{
    /// <summary>
    /// Author: John Egbert
    /// Description: Function Extensions
    /// Created: 05-04-2010
    /// </summary>
    public static class FunctionExtensions
    {
        public static bool ContainsFunction(this IEnumerable<Function> functions, int functionID)
        {
            return functions.Count(f => f.FunctionID == functionID) > 0;
        }

        public static Function GetByName(this IEnumerable<Function> functions, string title)
        {
            return functions.FirstOrDefault(f => f.Name == title);
        }
    }
}
