namespace NetSteps.Common.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Threading;
    using System.Web;

    /// <summary>
    /// Orders specific configuration information
    /// </summary>
    public class OrdersSection : ConfigurationSection
    {
        private static Lazy<OrdersSection> __singleton = new Lazy<OrdersSection>(LoadOrdersSection, LazyThreadSafetyMode.ExecutionAndPublication);

        public static OrdersSection Instance
        {
            get
            {
                return __singleton.Value;
            }
        }

        static OrdersSection LoadOrdersSection()
        {
            var ordersSection = LoadConfig<OrdersSection>(OrdersSection.SectionName);
            if (ordersSection == null)
            {
                ordersSection = new OrdersSection
                {
                    IsPartyOrderClient =
                        ConfigurationManager.GetAppSetting<bool>("IsPartyOrderClient", false),
                    PartyOrderMinimum =
                        ConfigurationManager.GetAppSetting<decimal>("PartyOrderMinimum", 0m)
                };
            }

            return ordersSection;
        }

        static T LoadConfig<T>(string name) where T : ConfigurationSection
        {
            ConfigurationSection section = (ApplicationContextCommon.Instance.IsWebApp && HttpContext.Current != null)
                    ? System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(@"~\").GetSection(name) as ConfigurationSection
                    : System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).GetSection(name) as ConfigurationSection;

            if (section == null)
            {
                return default(T);
            }

            return section as T;
        }

        public static readonly string SectionName = "netsteps.orders";

        const string prop_FundraisersEnabled = "FundraisersEnabled";
        const string prop_IsPartyOrderClient = "IsPartyOrderClient";
        const string prop_PartyOrderMinimum = "PartyOrderMinimum";
        const string prop_FundraiserOrderMinimum = "FundraiserOrderMinimum";

        [ConfigurationProperty(prop_FundraisersEnabled, IsRequired = false, DefaultValue = false)]
        public bool FundraisersEnabled
        {
            get { return (bool)this[prop_FundraisersEnabled]; }
            set { this[prop_FundraisersEnabled] = value; }
        }

        [ConfigurationProperty(prop_IsPartyOrderClient, IsRequired = true, DefaultValue = false)]
        public bool IsPartyOrderClient
        {
            get { return (bool)this[prop_IsPartyOrderClient]; }
            set { this[prop_IsPartyOrderClient] = value; }
        }

        [ConfigurationProperty(prop_PartyOrderMinimum, IsRequired = false, DefaultValue = "0")]
        public decimal PartyOrderMinimum
        {
            get { return (decimal)this[prop_PartyOrderMinimum]; }
            set { this[prop_PartyOrderMinimum] = value; }
        }

        [ConfigurationProperty(prop_FundraiserOrderMinimum, IsRequired = false, DefaultValue = "0")]
        public decimal FundraiserOrderMinimum
        {
            get { return (decimal)this[prop_FundraiserOrderMinimum]; }
            set { this[prop_FundraiserOrderMinimum] = value; }
        }

    }
}
