using NetSteps.Encore.Core.Configuration;

namespace NetSteps.Core.Cache.Config
{
	/// <summary>
	/// Contains collection of NamedCacheConfigElements
	/// </summary>
	public class NamedCacheConfigElementCollection : AbstractConfigurationElementCollection<NamedCacheConfigElement, string>
	{
		/// <summary>
		/// Gets an element's key; the Name property.
		/// </summary>
		/// <param name="element"></param>
		/// <returns></returns>
		protected override string PerformGetElementKey(NamedCacheConfigElement element)
		{
			return element.Name;
		}
	}
}
