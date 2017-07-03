using System.Configuration;
using System.Web;
using System.Web.Configuration;

namespace NetSteps.Web.Security
{
    public class ConfigurationEncryption
    {
        /// <summary>
        /// Author: John Egbert
        /// Description: Will encrypt a SectionGroup in the webconfig. - JHE
        /// Created: 09-20-2011
        /// </summary>
        public static void ProtectSectionGroup(string sectionGroupName)
        {
            ProtectSectionGroup(sectionGroupName, "DataProtectionConfigurationProvider");
        }
        public static void ProtectSectionGroup(string sectionGroupName, string provider)
        {
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = HttpContext.Current.Request.PhysicalApplicationPath + "\\Web.config";
            System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            ConfigurationSectionGroup sectionGroup = configuration.GetSectionGroup(sectionGroupName) as ConfigurationSectionGroup;

            foreach (ConfigurationSection section in sectionGroup.Sections)
                ProtectSection(section.SectionInformation.SectionName, provider);
        }

        /// <summary>
        /// Will unencrypt a SectionGroup in the webconfig. - JHE
        /// </summary>
        /// <param name="sectionGroupName"></param>
        public static void UnProtectSectionGroup(string sectionGroupName)
        {
            ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
            configFile.ExeConfigFilename = HttpContext.Current.Request.PhysicalApplicationPath + "\\Web.config";
            System.Configuration.Configuration configuration = System.Configuration.ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);
            ConfigurationSectionGroup sectionGroup = configuration.GetSectionGroup(sectionGroupName) as ConfigurationSectionGroup;

            foreach (ConfigurationSection section in sectionGroup.Sections)
                UnProtectSection(section.SectionInformation.SectionName);
        }


        /// <summary>
        /// Example: ProtectSection("appSettings", "DataProtectionConfigurationProvider"); - JHE
        /// http://davidhayden.com/blog/dave/archive/2005/11/17/2572.aspx
        /// </summary>
        /// <param name="sectionName"></param>
        public static void ProtectSection(string sectionName)
        {
            ProtectSection(sectionName, "DataProtectionConfigurationProvider");
        }
        public static void ProtectSection(string sectionName, string provider)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            ConfigurationSection section = config.GetSection(sectionName);

            if (section != null && !section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection(provider);
                config.Save();
            }
        }

        /// <summary>
        /// Example: UnProtectSection("appSettings"); - JHE
        /// http://davidhayden.com/blog/dave/archive/2005/11/17/2572.aspx
        /// </summary>
        /// <param name="sectionName"></param>
        public static void UnProtectSection(string sectionName)
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            ConfigurationSection section = config.GetSection(sectionName);

            if (section != null && section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
                config.Save();
            }
        }

    }
}
