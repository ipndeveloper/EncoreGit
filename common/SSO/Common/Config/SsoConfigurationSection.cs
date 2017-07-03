using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.SSO.Common.Config
{
    /// <summary>
    /// Configuration section for SSO
    /// </summary>
    public class SsoConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// Section name used in configuration file
        /// </summary>
        public static readonly string SectionName = "Sso";
        private const string PropertyName_KeyName = "KeyName";
        private const string Default_KeyName = "N3tst3ps_2008";

        private const string PropertyName_Salt = "Salt";
        private const string Default_Salt = "&netsteps_salt&";

        /// <summary>
        /// Name of the key used by SSO (should exist in the local box' key provider).
        /// </summary>
        [ConfigurationProperty(PropertyName_KeyName, IsRequired = true, DefaultValue = Default_KeyName)]
        public string KeyName
        {
            get
            {
                return (string)base[PropertyName_KeyName];
            }
            set
            {
                base[PropertyName_KeyName] = value;
            }
        }

        /// <summary>
        /// Salt, default being a preset GUID
        /// </summary>
        [ConfigurationProperty(PropertyName_Salt, IsRequired = true, DefaultValue = Default_Salt)]
        public string Salt
        {
            get
            {
                return (string)base[PropertyName_Salt];
            }
            set
            {
                base[PropertyName_Salt] = value;
            }
        }

        /// <summary>
        /// Gets the current SSO configuration or the default configuration.
        /// </summary>
        public static SsoConfigurationSection Current
        {
            get
            {
                var current = ConfigurationManager.GetSection(SectionName) as SsoConfigurationSection;
                return current ?? new SsoConfigurationSection();
            }
        }
    }
}
