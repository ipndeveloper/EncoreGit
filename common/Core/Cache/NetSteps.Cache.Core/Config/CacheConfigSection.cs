using System.Configuration;

namespace NetSteps.Core.Cache.Config
{
	/// <summary>
	/// Configuration section for cache settings.
	/// </summary>
	public class CacheConfigSection : ConfigurationSection
	{
		/// <summary>
		/// Configuration section name for cache settings
		/// </summary>
		public static readonly string SectionName = "netsteps.cache";

		/// <summary>
		/// Default value for max concurrent eviction workers if not set in config.
		/// </summary>
		public const int CDefaultMaxEvictionWorker = 4;

		/// <summary>
		/// Default value determining whether to listen for network evictions.
		/// </summary>
		public const bool CDefaultListenForNetworkEvictions = true;

		/// <summary>
		/// Property name for max concurrent eviction workers.
		/// </summary>
		public const string PropertyName_maxEvictionWorkers = "maxEvictionWorkers";

		/// <summary>
		/// Property name for listen-for network evictions.
		/// </summary>
		public const string PropertyName_listenForNetworkEvictions = "listenForNetworkEvictions";

		/// <summary>
		/// Property name for cache net interfaces.
		/// </summary>
		public const string PropertyName_cacheNetInterfaces = "cacheNetInterfaces";

		/// <summary>
		/// Property name for named caches.
		/// </summary>
		public const string PropertyName_namedCaches = "namedCaches";

		/// <summary>
		/// Indicates the maximum number of concurrent eviction workers the eviction manager
		/// should schedule.
		/// </summary>
		[ConfigurationProperty(PropertyName_maxEvictionWorkers, DefaultValue = CDefaultMaxEvictionWorker)]
		public int MaxEvictionWorkers
		{
			get { return (int)this[PropertyName_maxEvictionWorkers]; }
			set { this[PropertyName_maxEvictionWorkers] = value; }
		}

		/// <summary>
		/// Configurations for named caches.
		/// </summary>
		[ConfigurationProperty(PropertyName_namedCaches, IsDefaultCollection = true)]
		public NamedCacheConfigElementCollection NamedCaches
		{
			get { return (NamedCacheConfigElementCollection)this[PropertyName_namedCaches]; }
		}

		/// <summary>
		/// Gets the current configuration section.
		/// </summary>
		public static CacheConfigSection Current
		{
			get
			{
				CacheConfigSection config = ConfigurationManager.GetSection(
					CacheConfigSection.SectionName) as CacheConfigSection;
				return config ?? new CacheConfigSection();
			}
		}

		/// <summary>
		/// Gets either the named set of options or the default options if named options don't exist.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public MruCacheOptions NamedOrDefaultOptions<T>(string name)
		{
			var cfg = NamedCaches[name];
			if (cfg != null)
				return cfg.ToOptions<T>();
			else
				return new NamedCacheConfigElement().ToOptions<T>();
		}
	}
}