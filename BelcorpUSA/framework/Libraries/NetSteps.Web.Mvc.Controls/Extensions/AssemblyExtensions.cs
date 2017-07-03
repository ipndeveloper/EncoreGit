using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NetSteps.Web.Mvc.Controls.Controllers.Enrollment;

namespace NetSteps.Web.Mvc.Controls
{
    public static class AssemblyExtensions
    {
        internal static IEnumerable<Type> GetEnrollmentTypes(this Assembly assembly)
        {
            return assembly
                .GetTypes()
                .Where(t => t.IsSubclassOf(typeof(EnrollmentStep)));
        }
    }
}
