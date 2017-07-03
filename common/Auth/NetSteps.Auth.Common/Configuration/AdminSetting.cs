using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace NetSteps.Auth.Common.Configuration
{
    public class AdminSetting : ConfigurationElement
    {
        [ConfigurationProperty("name",IsRequired=true)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
        }

        [ConfigurationProperty("value", IsRequired = true)]
        public bool Value
        {
            get
            {
                return (bool)this["value"];
            }
        }
    }
}
