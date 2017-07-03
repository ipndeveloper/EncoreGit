using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Encore.Core.Net;
using NetSteps.Core.Cache.CacheNet;
using NetSteps.Core.Cache;
using System.Threading;

namespace NetSteps.Cache.Core.Tests.CacheNet
{
	[TestClass]
	public class CacheNetTests
	{
		//[TestMethod]
		//public void TestMethod1()
		//{
		//    var manager = new NetSteps.Core.Cache.CacheEvictionManager();
		//    manager.AddEvictionMonitor(new TestEvictionMonitor());
		//    var proto = new CacheNetProtocol(manager);
		//    UdpProtocolEndpoint<CacheNetMessage> endpoint = new UdpProtocolEndpoint<CacheNetMessage>(proto);
		//    endpoint.ParallelReceive(new IPEndPoint(IPAddress.Loopback, 65000));

		//    while (true)
		//    {
		//        Thread.SpinWait(1);
		//    }
		//}
	}

	public class TestEvictionMonitor : ICacheEvictionMonitor
	{
		public Guid RegistrationKey
		{
			get { return Guid.Empty; }
		}

		public IEnumerable<string> ContextKeys
		{
			get { return new List<string>(new string[] { "Test" }); }
		}

		public void OnEvictionNotification(string contextKey, object evictionInfo)
		{
			Assert.AreEqual("Test", contextKey);
		}

		public void Dispose() { }
	}
}
