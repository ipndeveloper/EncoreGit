using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Authorization.Common.Configuration
{
    public class AuthorizationConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("disabledFunctions", IsDefaultCollection = true)]
        public FunctionCollection Functions
        {
            get
            {
                return this["disabledFunctions"] as FunctionCollection;
            }
        }
    }
}
