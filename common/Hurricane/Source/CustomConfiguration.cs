using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace NetSteps.Hurricane.Plugin
{
    public class CustomSettings : ConfigurationSection
    {
        [ConfigurationProperty("hurricaneMailClients")]
        [ConfigurationCollection(typeof(HurricaneMailClientCollection))]
        public HurricaneMailClientCollection HurricaneMailClients
        {
            get { return (HurricaneMailClientCollection)base["hurricaneMailClients"]; }
        }
    }

    public class HurricaneMailClientCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new HurricaneMailClient();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((HurricaneMailClient)element).Name;
        }
    }

    public class HurricaneMailClient : ConfigurationElement
    {
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get 
            {
                return this["name"] as string;
            }
        }

        [ConfigurationProperty("hurricaneAccountId")]
        public string HurricaneAccountId
        {
            get
            {
                return this["hurricaneAccountId"] as string;
            }
        }

        [ConfigurationProperty("databaseVersion")]
        public string DatabaseVersion
        {
            get
            {
                return this["databaseVersion"] as string;
            }
        }

        [ConfigurationProperty("connectionString")]
        public string ConnectionString
        {
            get
            {
                return this["connectionString"] as string;
            }
        }

        [ConfigurationProperty("enableLocalDelivery", DefaultValue = false)]
        public bool EnableLocalDelivery
        {
            get
            {
                return (bool?)this["enableLocalDelivery"] ?? false;
            }
        }

        [ConfigurationProperty("attachmentFolderPath")]
        public string AttachmentFolderPath
        {
            get
            {
                return this["attachmentFolderPath"] as string;
            }
        }

        [ConfigurationProperty("enableAutoForward", DefaultValue = false)]
        public bool EnableAutoForward
        {
            get
            {
                return (bool?)this["enableAutoForward"] ?? false;
            }
        }

        [ConfigurationProperty("autoForwardAccountTypeId")]
        public int? AutoForwardAccountTypeId
        {
            get
            {
                return this["autoForwardAccountTypeId"] as int?;
            }
        }

        [ConfigurationProperty("autoForwardDisclaimerSiteId")]
        public int? AutoForwardDisclaimerSiteId
        {
            get
            {
                return this["autoForwardDisclaimerSiteId"] as int?;
            }
        }

        [ConfigurationProperty("autoForwardDisclaimerSectionNameHtml")]
        public string AutoForwardDisclaimerSectionNameHtml
        {
            get
            {
                return this["autoForwardDisclaimerSectionNameHtml"] as string;
            }
        }

        [ConfigurationProperty("autoForwardDisclaimerSectionNameText")]
        public string AutoForwardDisclaimerSectionNameText
        {
            get
            {
                return this["autoForwardDisclaimerSectionNameText"] as string;
            }
        }

        [ConfigurationProperty("overrideMailFrom")]
        public string OverrideMailFrom
        {
            get
            {
                return this["overrideMailFrom"] as string;
            }
        }

        [ConfigurationProperty("overrideSender")]
        public string OverrideSender
        {
            get
            {
                return this["overrideSender"] as string;
            }
        }

        [ConfigurationProperty("enableRecipientStatusUpdates", DefaultValue = false)]
        public bool EnableRecipientStatusUpdates
        {
            get
            {
                return (bool?)this["enableRecipientStatusUpdates"] ?? false;
            }
        }

        [ConfigurationProperty("enableMessageTracking", DefaultValue = false)]
        public bool EnableMessageTracking
        {
            get
            {
                return (bool?)this["enableMessageTracking"] ?? false;
            }
        }

        public string AutoForwardDisclaimerContentHtml { get; set; }
        public string AutoForwardDisclaimerContentText { get; set; }
    }
}
