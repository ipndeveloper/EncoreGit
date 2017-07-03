using System.Configuration;
using System.Web.Configuration;
using NetSteps.Common.Extensions;

namespace NetSteps.Web.Mvc.Business.Inheritance
{
    public class OverridableSiteSection : ConfigurationSection
    {
        private static OverridableSiteSection _instance;
        private static object _syncRoot = new object();
        public static OverridableSiteSection Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = LoadSection();
                        }
                    }
                }
                return _instance;
            }
        }

        private OverridableSiteSection() { }

        private static OverridableSiteSection LoadSection()
        {
            System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration("~");

            return config.GetSection("OverridableSite") as OverridableSiteSection ?? new OverridableSiteSection();
        }

		[ConfigurationProperty("Assemblies")]
		public AssemblyElementCollection Assemblies
		{
			get { return this["Assemblies"] as AssemblyElementCollection; }
		}

		[ConfigurationProperty("ResourceOverrides")]
		public ResourceOverrideElementCollection ResourceOverrides
		{
			get { return this["ResourceOverrides"] as ResourceOverrideElementCollection; }
		}
    }

    public class AssemblyElement : ConfigurationElement
    {
        [ConfigurationProperty("Name", IsRequired = true)]
        public string Name
        {
            get
            {
                return this["Name"] as string;
            }
        }

        [ConfigurationProperty("SortIndex", IsRequired = true)]
        public int SortIndex
        {
            get
            {
                return this["SortIndex"].ToString().ToInt();
            }
        }
    }

    public class AssemblyElementCollection : ConfigurationElementCollection
    {
        public AssemblyElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as AssemblyElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new AssemblyElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AssemblyElement)element).Name;
        }
    }

    public class ResourceOverrideElement : ConfigurationElement
    {
        [ConfigurationProperty("VirtualPath", IsRequired = true)]
        public string VirtualPath
        {
            get
            {
                return this["VirtualPath"] as string;
            }
        }

        [ConfigurationProperty("UsePhysicalFile", IsRequired = false)]
        public bool UsePhysicalFile
        {
            get
            {
                return (bool)this["UsePhysicalFile"];
            }
        }

        [ConfigurationProperty("Assembly", IsRequired = false)]
        public string Assembly
        {
            get
            {
                return this["Assembly"] as string;
            }
        }

        [ConfigurationProperty("SortIndex", IsRequired = true)]
        public int SortIndex
        {
            get
            {
                return this["SortIndex"].ToString().ToInt();
            }
        }
    }

    public class ResourceOverrideElementCollection : ConfigurationElementCollection
    {
        public ResourceOverrideElement this[int index]
        {
            get
            {
                return base.BaseGet(index) as ResourceOverrideElement;
            }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                this.BaseAdd(index, value);
            }
        }

        public ResourceOverrideElement GetByKey(string virtualPath)
        {
            return BaseGet(virtualPath) as ResourceOverrideElement;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ResourceOverrideElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ResourceOverrideElement)element).VirtualPath;
        }
    }
}
