using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Core.Cache.Config;

namespace NetSteps.Core.Cache.Tests
{
	[TestClass]
	public class CacheConfigSectionTests
	{
		[TestMethod]
		public void CanGetDefaultOptionsForUnconfiguredCache()
		{
			var config = CacheConfigSection.Current;
			var defa = config.NamedOrDefaultOptions<CacheConfigSectionTests>("shouldn't exist");
			Assert.IsNotNull(defa);
		}
	}
}
