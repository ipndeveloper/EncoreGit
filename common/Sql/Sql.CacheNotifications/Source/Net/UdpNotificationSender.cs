using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.NetworkInformation;

namespace NetSteps.Sql.CacheNotifications.Net
{
	public class UdpNotificationSender : INotificationSender
	{

		static readonly int MaxPacket = 65507;
		static readonly long Preamble = 0x43616368654e6574; // Big-endian, 'CacheNet' in hex.

		static readonly Crc32 __crc32 = new Crc32();

		/// <summary>
		/// Used to send notifications to Clients listening for object change events from within SQL server.
		/// </summary>
		/// <param name="recipients">A list of recipients to send the notification to. 
		///		<example>
		///			Format: host:port 
		///			127.0.0.1:65000
		///			localhost:500
		///			some.client.net:2500
		///		</example>
		/// </param>
		/// <param name="messageKind">The Kind of message being sent</param>
		/// <param name="identityKind">The Kind (Type) of identies being sent</param>
		/// <param name="name">A name used to reference to whom or what the identies belong</param>
		/// <param name="ids">A list of identities, as strings, for which this notification is about</param>
		public void Send(IEnumerable<string> recipients, IEnumerable<string> contextKeys, NotificationMessageKind messageKind, NotificationIdentityKind identityKind, IEnumerable<string> ids)
		{
			IEnumerable<byte[]> compiledMessages = CompileMessages(contextKeys, messageKind, identityKind, ids);
			List<EndPoint> endPoints = new List<EndPoint>();

			foreach (var rec in recipients)
			{
				var recParts = rec.Split(':');

				if (recParts.Length != 2) continue;

				var host = recParts[0];
				int port;
				if (!Int32.TryParse(recParts[1], out port) || port <= 0) continue;
				Int32.TryParse(recParts[1], out port);

				IPAddress ip;
				if (IPAddress.TryParse(host, out ip))
				{
					endPoints.Add(new IPEndPoint(ip, port));
				}
				else
				{
					foreach (var hostIp in Dns.GetHostAddresses(host))
					{
						if (hostIp.AddressFamily == AddressFamily.InterNetwork)
						{
							endPoints.Add(new IPEndPoint(hostIp, port));
							break;
						}
					}
				}
			}

			endPoints = endPoints.Distinct().ToList();


			using (Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
			{
				foreach (var ep in endPoints)
				{
					foreach (var msg in compiledMessages)
					{
						s.SendTo(msg, SocketFlags.None, ep);
					}
				}
			}
		}

		private IEnumerable<byte[]> CompileMessages(IEnumerable<string> contextKeys, NotificationMessageKind messageKind, NotificationIdentityKind identityKind, IEnumerable<string> ids)
		{
			List<byte[]> results = new List<byte[]>();

			byte[] messageHeader = CompileHeader(contextKeys, messageKind, identityKind);
			int headerAndCrc = messageHeader.Length + 4;
			List<byte[]> idSets = new List<byte[]>();
			List<string> idSource = new List<string>(ids);
			while (idSource.Any())
			{
				idSets.Add(PopulateIdSet(headerAndCrc, identityKind, ref idSource));
			}

			foreach (byte[] keys in idSets)
			{

				byte[] innerResults = new byte[headerAndCrc + keys.Length];

				Array.Copy(messageHeader, 0, innerResults, 0, messageHeader.Length);
				Array.Copy(keys, 0, innerResults, messageHeader.Length, keys.Length);

				var bitWriter = Buffers.BufferWriter.Create(Encoding.ASCII);
				int index = innerResults.Length - 4;
				uint crc = __crc32.ComputeChecksum(innerResults, 0, index);
				bitWriter.Write(innerResults, ref index, crc);
				results.Add(innerResults);
			}

			return results;
		}

		private byte[] CompileHeader(IEnumerable<string> contextKeys, NotificationMessageKind messageKind, NotificationIdentityKind identityKind)
		{
			int reserved = 16;//Preamble (8) + Message Kind (4) + Identity Kind (4)
			int availableBytes = MaxPacket - reserved - (MaxPacket / 4);//Reserve header space + 25% for identities...
	
			List<string> ckeys = contextKeys.ToList();
			byte[] ckeysBytes = WriteCollection_String(availableBytes, ref ckeys);
			if(ckeys.Any())
			{
				throw new InvalidDataException("To many context keys to build a meaningful message.");
			}
			int headerSize = reserved + ckeysBytes.Length;
			byte[] results = new byte[headerSize];

			int index = 0;
			var bitWriter = Buffers.BufferWriter.Create(Encoding.ASCII);

			bitWriter.Write(results, ref index, Preamble);
			
			Array.Copy(ckeysBytes, 0, results, index, ckeysBytes.Length);
			
			index += ckeysBytes.Length;

			bitWriter.Write(results, ref index, (int)messageKind);
			bitWriter.Write(results, ref index, (int)identityKind);

			return results;
		}

		private byte[] PopulateIdSet(int headerSize, NotificationIdentityKind identityKind, ref List<string> sourceIds)
		{
			int maxIdBytes = (MaxPacket - headerSize);

			byte[] results = null;

			if (sourceIds.Any())
			{
				switch (identityKind)
				{
					case NotificationIdentityKind.Int32:
						results = WriteCollection_FixedLength(maxIdBytes, 4, ref sourceIds);
						break;
					case NotificationIdentityKind.Int64:
						results = WriteCollection_FixedLength(maxIdBytes, 8, ref sourceIds);
						break;
					case NotificationIdentityKind.String:
						results = WriteCollection_String(maxIdBytes, ref sourceIds);
						break;
					case NotificationIdentityKind.Guid:
						results = WriteCollection_FixedLength(maxIdBytes, 16, ref sourceIds);
						break;
					default:
						break;
				} 
			}

			return results;
		}

		private byte[] WriteCollection_FixedLength(int maxIdBytes, int idByteSize, ref List<string> sourceIds)
		{
			int maxLessCount = maxIdBytes - 4; //Remove space for count
			int actualMaxIdBytes = (maxLessCount - (maxLessCount % idByteSize)); //remove remaining unusable space...
			int maxIdCount = Math.Min(sourceIds.Count, (actualMaxIdBytes / idByteSize));//Which is smaller, given Ids, or available space...
			actualMaxIdBytes = maxIdCount * idByteSize;
			byte[] results = new byte[actualMaxIdBytes + 4];//actual space + count

			int index = 0;
			var bitWriter = Buffers.BufferWriter.Create(Encoding.ASCII);
			bitWriter.Write(results, ref index, maxIdCount);

			int actualCount = maxIdCount;
			for (int i = 0; i < maxIdCount; i++)
			{
				string key = sourceIds[i];
				switch (idByteSize)
				{
					case 4:
						int key4;
						if (Int32.TryParse(key, out key4))
						{
							bitWriter.Write(results, ref index, key4);
						}
						else
						{
							actualCount--;
						}
						break;
					case 8:
						long key8;
						if (Int64.TryParse(key, out key8))
						{
							bitWriter.Write(results, ref index, key8);
						}
						else
						{
							actualCount--;
						}
						break;
					case 16:
						try
						{
							Guid key16 = new Guid(key);
							bitWriter.Write(results, ref index, key16);
						}
						catch (Exception e)
						{
							actualCount--;
						}
						break;
				}
			}

			if (actualCount != maxIdCount)
			{
				int idByteCount = (actualCount * idByteSize);
				byte[] actualResults = new byte[4 + idByteCount];
				int newIndex = 0;
				bitWriter.Write(actualResults, ref newIndex, actualCount);
				Array.Copy(results, 4, actualResults, 4, idByteCount);
				results = actualResults;
			}

			if (maxIdCount == sourceIds.Count)
			{
				sourceIds.Clear();
			}
			else
			{
				sourceIds.RemoveRange(0, maxIdCount);
			}

			return results;
		}

		private byte[] WriteCollection_String(int maxIdBytes, ref List<string> sourceIds)
		{
			int actualMaxIdBytes = maxIdBytes - 4;
			byte[] innerResults = new byte[actualMaxIdBytes];
			int index = 0;
			var bitWriter = Buffers.BufferWriter.Create(Encoding.ASCII);

			int consumedBytes = 0;
			int keyCount = 0;
			int listPosition = sourceIds.Count - 1;
			while (sourceIds.Any())
			{
				string key = sourceIds[listPosition];
				int keyBytes = key.Length + 4;
				if ((consumedBytes + keyBytes <= actualMaxIdBytes))
				{
					bitWriter.Write(innerResults, ref index, key, true);
					consumedBytes += keyBytes;
					keyCount++;
					sourceIds.RemoveAt(listPosition);
					listPosition--;
				}
				else
				{
					break;
				}
			}

			byte[] results = new byte[consumedBytes + 4];
			int outerIndex = 0;
			bitWriter.Write(results, ref outerIndex, keyCount);
			Array.Copy(innerResults, 0, results, outerIndex, consumedBytes);
			return results;
		}
	}
}
