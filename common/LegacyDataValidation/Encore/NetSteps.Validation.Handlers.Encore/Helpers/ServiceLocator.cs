using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Validation.Handlers.Helpers
{
    /// <summary>
    /// Introduces a dependency on the IOC.  Not sure if there's another elegant way to do this without this dependency.
    /// </summary>
    public static class ServiceLocator
    {
        public static T FindService<T>()
        {
            return Create.New<T>();
        }
    }
}
