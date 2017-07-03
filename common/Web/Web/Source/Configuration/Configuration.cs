using System.Configuration;

namespace NetSteps.Web
{
    public enum CompressionType
    {
        None = 0,
        GZip = 1
    }

    public class Configuration : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the type of the compression.
        /// </summary>
        /// <value>The type of the compression.</value>
        [ConfigurationProperty("viewstateCompression", IsRequired = false)]
        public CompressionType ViewstateCompression
        {
            get
            {
                { return (base["viewstateCompression"] == null) ? CompressionType.None : (CompressionType)base["viewstateCompression"]; }
            }
            set
            {
                { base["viewstateCompression"] = value; }
            }
        }


        /// <summary>
        /// Gets or sets the type of the compression.
        /// </summary>
        /// <value>The type of the compression.</value>
        [ConfigurationProperty("cacheHandlerImages", IsRequired = false)]
        public bool CacheHandlerImages
        {
            get
            {
                { return (base["cacheHandlerImages"] == null) ? true : (bool)base["cacheHandlerImages"]; }
            }
            set
            {
                { base["cacheHandlerImages"] = value; }
            }
        }

        [ConfigurationProperty("cacheTinyHandlerImages", IsRequired = false)]
        public bool CacheTinyHandlerImages
        {
            get
            {
                { return (base["cacheTinyHandlerImages"] == null) ? true : (bool)base["cacheTinyHandlerImages"]; }
            }
            set
            {
                { base["cacheTinyHandlerImages"] = value; }
            }
        }

        [ConfigurationProperty("cacheTextImages", IsRequired = false)]
        public bool CacheTextImages
        {
            get
            {
                { return (base["cacheTextImages"] == null) ? true : (bool)base["cacheTextImages"]; }
            }
            set
            {
                { base["cacheTextImages"] = value; }
            }
        }

        [ConfigurationProperty("reWriteUrls", IsRequired = false)]
        public bool ReWriteUrls
        {
            get
            {
                { return (base["reWriteUrls"] == null) ? true : (bool)base["reWriteUrls"]; }
            }
            set
            {
                { base["reWriteUrls"] = value; }
            }
        }

    }
}
