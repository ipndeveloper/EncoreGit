using System;
using System.Linq;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetSteps.Sql.CacheNotifications.Net;

namespace NetSteps.Sql.CacheNotifications.Tests
{
	[TestClass]
	public class UdpNotifiactionsTests
	{
		[TestMethod]
		public void Udp_Message_int32_Accuracy_Test()
		{
			Net.TestUdpNotificationListener Listener = new Net.TestUdpNotificationListener(64999);
			Listener.Listen();

			UdpNotificationSender sender = new UdpNotificationSender();
			string[] recipients = new string[] { "localhost:64999" };
			string[] contextKeys = new string[] { "Test-Accuracy" };
			string[] ids = new string[] { "123", "456", "789" };
			sender.Send(recipients, contextKeys, NotificationMessageKind.ExpirationById, NotificationIdentityKind.Int32, ids);

			Thread.Sleep(2000);

			Assert.IsTrue(Listener.ReceivedMessages.Count == 1);
			Net.TestRecievedMessage msg;
			if (Listener.ReceivedMessages.TryPop(out msg))
			{
				Assert.AreEqual("Test-Accuracy", msg.ContextKeys.First());
				Assert.AreEqual(NotificationIdentityKind.Int32, msg.IdentityKind);
				Assert.AreEqual(NotificationMessageKind.ExpirationById, msg.MessageKind);
				Assert.AreEqual(3, msg.Ids.Count);
				Assert.AreEqual(123, msg.Ids[0]);
				Assert.AreEqual(456, msg.Ids[1]); 
				Assert.AreEqual(789, msg.Ids[2]);
			}
			else
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void Udp_Message_int32_RoundTrip_Test()
		{
			Net.TestUdpNotificationListener Listener = new Net.TestUdpNotificationListener(65000);
			Listener.Listen();

			UdpNotificationSender sender = new UdpNotificationSender();
			string[] recipients = new string[] { "localhost:65000" };
			string[] contextKeys = new string[] { "Test-Int32" };
			string[] ids = GenerateIds();
			sender.Send(recipients, contextKeys, NotificationMessageKind.ExpirationById, NotificationIdentityKind.Int32, ids);

			Thread.Sleep(2000);

			Assert.IsTrue(Listener.ReceivedMessages.Count == 7);
			Net.TestRecievedMessage msg;
			if (Listener.ReceivedMessages.TryPop(out msg))
			{
				Assert.AreEqual("Test-Int32", msg.ContextKeys.First());
				Assert.AreEqual(NotificationIdentityKind.Int32, msg.IdentityKind);
				Assert.AreEqual(NotificationMessageKind.ExpirationById, msg.MessageKind);
			}
			else
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void Udp_Message_int64_RoundTrip_Test()
		{
			Net.TestUdpNotificationListener Listener = new Net.TestUdpNotificationListener(65001);
			Listener.Listen();

			UdpNotificationSender sender = new UdpNotificationSender();
			string[] recipients = new string[] { "localhost:65001" };
			string[] contextKeys = new string[] { "Test-Int64" };
			string[] ids = GenerateIds();
			sender.Send(recipients, contextKeys, NotificationMessageKind.ExpirationById, NotificationIdentityKind.Int64, ids);

			Thread.Sleep(2000);

			Assert.IsTrue(Listener.ReceivedMessages.Count == 13);
			Net.TestRecievedMessage msg;
			if (Listener.ReceivedMessages.TryPop(out msg))
			{
				Assert.AreEqual(msg.ContextKeys.First(), "Test-Int64");
				Assert.AreEqual(msg.IdentityKind, NotificationIdentityKind.Int64);
				Assert.AreEqual(msg.MessageKind, NotificationMessageKind.ExpirationById);
			}
			else
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void Udp_Message_string_RoundTrip_Test()
		{
			Net.TestUdpNotificationListener Listener = new Net.TestUdpNotificationListener(65002);
			Listener.Listen();

			UdpNotificationSender sender = new UdpNotificationSender();
			string[] recipients = new string[] { "localhost:65002" };
			string[] contextKeys = new string[] { "Test-String" };
			string[] ids = GenerateIds();
			sender.Send(recipients, contextKeys, NotificationMessageKind.ExpirationById, NotificationIdentityKind.String, ids);

			Thread.Sleep(2000);

			Assert.IsTrue(Listener.ReceivedMessages.Count == 21);
			Net.TestRecievedMessage msg;
			if (Listener.ReceivedMessages.TryPop(out msg))
			{
				Assert.AreEqual(msg.ContextKeys.First(), "Test-String");
				Assert.AreEqual(msg.IdentityKind, NotificationIdentityKind.String);
				Assert.AreEqual(msg.MessageKind, NotificationMessageKind.ExpirationById);
			}
			else
			{
				Assert.Fail();
			}
		}

		[TestMethod]
		public void Udp_Message_Guid_RoundTrip_Test()
		{
			Net.TestUdpNotificationListener Listener = new Net.TestUdpNotificationListener(65003);
			Listener.Listen();

			UdpNotificationSender sender = new UdpNotificationSender();
			string[] recipients = new string[] { "localhost:65003" };
			string[] contextKeys = new string[] { "Test-Guid" };
			string[] ids = GenerateGUIds();
			sender.Send(recipients, contextKeys, NotificationMessageKind.ExpirationById, NotificationIdentityKind.Guid, ids);

			Thread.Sleep(2000);

			Assert.IsTrue(Listener.ReceivedMessages.Count == 25);
			Net.TestRecievedMessage msg;
			if (Listener.ReceivedMessages.TryPop(out msg))
			{
				Assert.AreEqual(msg.ContextKeys.First(), "Test-Guid");
				Assert.AreEqual(msg.IdentityKind, NotificationIdentityKind.Guid);
				Assert.AreEqual(msg.MessageKind, NotificationMessageKind.ExpirationById);
			}
			else
			{
				Assert.Fail();
			}
		}

		private string[] GenerateGUIds()
		{
			string[] ids = new string[100000];
			for (int i = 0; i < 100000; i++)
			{
				ids[i] = Guid.NewGuid().ToString();
			}
			return ids;
		}

		private string[] GenerateIds()
		{
			Random r = new Random(Environment.TickCount);
			string[] ids = new string[100000];
			for (int i = 0; i < 100000; i++)
			{
				ids[i] = Convert.ToString(r.Next());
			}
			return ids;
		}
	}
}
