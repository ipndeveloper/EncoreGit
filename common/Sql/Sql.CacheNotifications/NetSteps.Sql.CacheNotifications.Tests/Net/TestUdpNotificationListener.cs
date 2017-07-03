using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NetSteps.Sql.CacheNotifications.Tests.Net
{
	public class TestUdpNotificationListener : IDisposable
	{
		#region Properties

		static readonly long Preamble = 0x43616368654e6574; // Big-endian, 'CacheNet' in hex.
		static readonly Crc32 __crc32 = new Crc32();
		static readonly int __minDataLength = 29; //This is the length of all headerfields and trailing crc

		private bool Listening;
		private int Port;

		private Socket Server;

		public ConcurrentStack<TestRecievedMessage> ReceivedMessages = new ConcurrentStack<TestRecievedMessage>();

		#endregion

		#region Constructors

		public TestUdpNotificationListener()
			: this(65000) { }

		public TestUdpNotificationListener(ushort port)
		{
			Port = port;
		}

		#endregion

		#region Methods

		public void Listen()
		{
			Server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			Server.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
			Server.ReceiveBufferSize = 1048576 * 2;
			IPEndPoint serverEndPoint = new IPEndPoint(IPAddress.Any, Port);
			Server.Bind(serverEndPoint);

			IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
			EndPoint cep = (EndPoint)clients;

			UdpState state = new UdpState();
			state.ClientEP = cep;
			Server.BeginReceiveFrom(state.Data, 0, state.Data.Length, SocketFlags.None, ref cep, new AsyncCallback(Recieve_Data), state);

			Listening = true;
		}

		public void StopListening()
		{
			if (Listening)
			{
				Server.Close();
				Listening = false;
			}
		}

		private void Recieve_Data(IAsyncResult ar)
		{
			byte[] data;
			UdpState recState = ar.AsyncState as UdpState;
			EndPoint cep = recState.ClientEP;
			int recieved = 0;
			try
			{
				if(Server != null && Listening)
					recieved = Server.EndReceiveFrom(ar, ref cep);
			}
			catch (Exception e) { }


			if (Server != null && Listening)
			{
				IPEndPoint clients = new IPEndPoint(IPAddress.Any, 0);
				EndPoint newcep = (EndPoint)clients;
				UdpState state = new UdpState();
				state.ClientEP = newcep;
				Server.BeginReceiveFrom(state.Data, 0, state.Data.Length, SocketFlags.None, ref newcep, new AsyncCallback(Recieve_Data), state);
			}

			if (recieved > 0)
			{
				data = new byte[recieved];
				Array.Copy(recState.Data, data, data.Length);

				if (data.Length >= __minDataLength)
				{
					var bitReader = FlitBit.Core.Buffers.BufferReader.Create(Encoding.ASCII);

					int crcIndex = data.Length - 4;
					uint datacrc = bitReader.ReadUInt32(data, ref crcIndex);
					uint computedcrc = __crc32.ComputeChecksum(data, 0, data.Length - 4);
					if (datacrc == computedcrc)
					{
						int ptr = 0;
						long pre = bitReader.ReadInt64(data, ref ptr);
						if (pre == Preamble)
						{
							TestRecievedMessage message = new TestRecievedMessage();
							message.DataLength = data.Length;
							int ctxKeyCount = bitReader.ReadInt32(data, ref ptr);

							for (int i = 0; i < ctxKeyCount; i++)
							{
								message.ContextKeys.Add(bitReader.ReadStringWithByteCountPrefix(data, ref ptr));
							}

							message.MessageKind = (NotificationMessageKind)bitReader.ReadInt32(data, ref ptr);
							message.IdentityKind = (NotificationIdentityKind)bitReader.ReadInt32(data, ref ptr);
							int idCount = bitReader.ReadInt32(data, ref ptr);
							for (int i = 0; i < idCount; i++)
							{
								switch (message.IdentityKind)
								{
									case NotificationIdentityKind.Int32:
										message.Ids.Add(bitReader.ReadInt32(data, ref ptr));
										break;
									case NotificationIdentityKind.Int64:
										message.Ids.Add(bitReader.ReadInt64(data, ref ptr));
										break;
									case NotificationIdentityKind.String:
										message.Ids.Add(bitReader.ReadStringWithByteCountPrefix(data, ref ptr));
										break;
									case NotificationIdentityKind.Guid:
										message.Ids.Add(bitReader.ReadGuid(data, ref ptr));
										break;
									default:
										break;
								}
							}

							ReceivedMessages.Push(message);
						}
					}
				}
			}
		}

		public void Dispose()
		{
			if (Listening)
				StopListening();
		}

		#endregion
	}

	internal class UdpState
	{
		internal EndPoint ClientEP;
		internal byte[] Data = new byte[65536];
	}
}
