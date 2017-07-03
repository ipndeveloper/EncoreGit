using System.Web.Caching;

namespace NetSteps.Web.Caching
{
	public class NeverExpiringCacheDependency : CacheDependency
	{
		public NeverExpiringCacheDependency() : base() { }
	}
}
