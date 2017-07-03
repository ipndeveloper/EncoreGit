using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NetSteps.Sql.CacheNotifications.Tests.Net;
using System.Threading;
using System.Threading.Tasks;

namespace NetSteps.Sql.CacheNotifications.Tests.ListenerConsole
{
	class Program
	{
		static TestUdpNotificationListener Listener;

		static void Main(string[] args)
		{
			Listener = new TestUdpNotificationListener();
			Listener.Listen();

			int i = 0;
			while (true)
			{
				TestRecievedMessage m;
				if (Listener.ReceivedMessages.TryPop(out m))
				{
					i++;
					Console.WriteLine(" ====== Message " + i.ToString() + " Recieved ======");
					Console.WriteLine("DataLength: " + m.DataLength);
					Console.WriteLine("ContextKeys: " + String.Join(", ", m.ContextKeys));
					Console.WriteLine("MessageKind: " + m.MessageKind.ToString());
					Console.WriteLine("IdentityKind: " + m.IdentityKind.ToString());
					if (m.Ids.Any())
					{
						Console.WriteLine("Ids (" + m.Ids.Count + ") : " + m.Ids.First().ToString() + " - " + m.Ids.Last().ToString());
					}
					else
					{
						Console.WriteLine("Ids (0) : ");
					}
					Console.WriteLine();
				}
				else
				{
					Thread.Sleep(200);
				}
			}
		}
	}
}
