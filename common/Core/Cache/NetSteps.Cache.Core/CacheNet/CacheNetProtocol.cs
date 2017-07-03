using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Buffers;
using System.Diagnostics.Contracts;
using System.Net;
using NetSteps.Encore.Core.Net;
using NetSteps.Encore.Core;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Core.Cache.CacheNet
{
	/// <summary>
	/// The Protocol for handling CacheNetMessags
	/// </summary>
	[CLSCompliant(false)]
	public class CacheNetProtocol : INetworkProtocol<CacheNetMessage>
	{
		/// <summary>
		/// THe Preamble value for network messages communicating over this protocol
		/// </summary>
		public static readonly long Preamble = 0x43616368654e6574; // Big-endian, 'CacheNet' in hex.
		internal static readonly Crc32 Crc32 = new Crc32();

		IBufferWriter _littleWriter;
		IBufferReader _littleReader;
		IBufferWriter _bigWriter;
		IBufferReader _bigReader;
		IPEndPoint _endpoint;

		CacheEvictionManager _evictionManager;

		/// <summary>
		/// Creates a new CacheNetProtocol
		/// </summary>
		/// <param name="evictionManager">The evictionManager to use when recieving eviction notifications</param>
		public CacheNetProtocol(CacheEvictionManager evictionManager)
		{
			Contract.Requires<ArgumentNullException>(evictionManager != null);

			this._evictionManager = evictionManager;

			this._littleWriter = Create.NewNamed<IBufferWriter>("little-endian");
			this._littleReader = Create.NewNamed<IBufferReader>("little-endian");
			this._bigWriter = Create.NewNamed<IBufferWriter>("big-endian");
			this._bigReader = Create.NewNamed<IBufferReader>("big-endian");
		}

		/// <summary>
		/// Indicates if the protocol is duplex; always true
		/// </summary>
		public bool IsFullDuplex { get { return true; } }
		
		/// <summary>
		/// Indicates if this protocol is threadsafe; always true
		/// </summary>
		public bool IsThreadSafe { get { return true; } }


		/// <summary>
		/// initialize this protocol using the given endpoint
		/// </summary>
		/// <param name="endpoint"></param>
		public void Initialize(INetEndpoint endpoint)
		{
			Contract.Assert(endpoint != null);

			_endpoint = (IPEndPoint)endpoint.EndPoint;
		}

		/// <summary>
		/// Attempts to decode a datagram recived as a byte array into a CacheNetMessage
		/// </summary>
		/// <param name="endpoint">The endpoint the message was recieved from</param>
		/// <param name="buffer">The buffer containing the data</param>
		/// <param name="offset">The offset to begin consuming the buffer from</param>
		/// <param name="length">The amount of data to consume from the buffer</param>
		/// <returns>A DecodedMessageResult indicating the success of the decode and if applicable, the decoded message</returns>
		public DecodeMessageResult<CacheNetMessage> TryDecodeMessage(IPEndPoint endpoint, byte[] buffer, int offset, int length)
		{
			Contract.Assert(endpoint != null);

			byte[] internalBuffer = new byte[length];
			Array.Copy(buffer, offset, internalBuffer, 0, length);

			int cursor = offset;
			IBufferReader reader = null;

			CacheNetEndianness endianness = BitConverter.IsLittleEndian ? CacheNetEndianness.LittleEndian : CacheNetEndianness.BigEndian;
			long littlePreamble = _littleReader.ReadInt64(internalBuffer, ref cursor);
			if (littlePreamble == Preamble)
			{
				reader = _littleReader;
				endianness = CacheNetEndianness.LittleEndian;
			}
			else
			{
				cursor -= 8;
				long bigPreamble = _bigReader.ReadInt64(internalBuffer, ref cursor);
				if (bigPreamble == Preamble)
				{
					reader = _bigReader;
					endianness = CacheNetEndianness.BigEndian;
				}
			}

			if (reader == null)
			{
				return new DecodeMessageResult<CacheNetMessage>(DecodeMessageResultKind.Garbage, null);
			}

			CacheNetMessageHeader header = new CacheNetMessageHeader(endianness, CacheNetMessageKind.None, 0);
			header.ReadFromBuffer(reader, internalBuffer, ref cursor);

			CacheNetMessage m;
			switch (header.MessageKind)
			{
				case CacheNetMessageKind.ExpirationById:
					m = new ExpirationByIdMessage(header);
					break;
				default:
					throw new InvalidOperationException(String.Concat("Unrecognized message kind: ", header.MessageKind));
			}

			m.ReadFromBuffer(reader, internalBuffer, ref cursor);

			return new DecodeMessageResult<CacheNetMessage>(DecodeMessageResultKind.Success, m);
		}

		/// <summary>
		/// Encodes the given message into a transmittable byte array
		/// </summary>
		/// <param name="message">The message to encode</param>
		/// <returns>The encoded message</returns>
		public byte[] EncodeMessage(CacheNetMessage message)
		{
			CacheNetEndianness end = BitConverter.IsLittleEndian ? CacheNetEndianness.LittleEndian : CacheNetEndianness.BigEndian;
			return EncodeMessage(message, end);
		}

		/// <summary>
		/// Encodes the given message into a transmittable byte array
		/// </summary>
		/// <param name="message">The message to encode</param>
		/// <param name="endianness">The endianness with which to encode the given message</param>
		/// <returns>The encoded message</returns>
		public byte[] EncodeMessage(CacheNetMessage message, CacheNetEndianness endianness)
		{
			Contract.Assert(message != null);

			IBufferWriter writer = endianness == CacheNetEndianness.LittleEndian ? _littleWriter : _bigWriter;

			var buffer = new byte[100];
			var cursor = 0;
			var used = message.WriteToBuffer(writer, buffer, ref cursor);
			var result = new byte[used];
			Array.Copy(buffer, result, used);
			return result;
		}

		/// <summary>
		/// Performs the required action against a received message.
		/// </summary>
		/// <param name="endpoint">The endpoint on which the message was received.</param>
		/// <param name="timestamp">A timestamp indicating when the message was recieved</param>
		/// <param name="message">The message recieved</param>
		public void HandleEndpointMessage(IPEndPoint endpoint, DateTime timestamp, CacheNetMessage message)
		{
			Contract.Assert(endpoint != null);
			Contract.Assert(message != null);
			Contract.Assert(message.Header != null);

			switch (message.Header.MessageKind)
			{
				case CacheNetMessageKind.None:
					break;
				case CacheNetMessageKind.ExpirationById:
					_evictionManager.OnEvictionNotification(message);
					break;
				default:
					break;
			}
		}

		internal static IEnumerable<string> ReadContextKeys(IBufferReader reader, byte[] source, ref int offset)
		{
			var keys = new List<string>();
			var count = reader.ReadInt32(source, ref offset);
			if (count < 0)
			{
				throw new InvalidOperationException(String.Concat("Expected positive context key count but recieved ", count));
			}
			for (int i = 0; i < count; i++)
			{
				keys.Add(reader.ReadStringWithByteCountPrefix(source, ref offset));
			}
			return keys.ToReadOnly();
		}

		internal static IEnumerable<IKnownEndpoint> ReadKnownEndpoints(IBufferReader reader, byte[] source, ref int offset, ICopier<KnownEndpoint, IKnownEndpoint> copier)
		{
			var result = new List<IKnownEndpoint>();
			var count = reader.ReadInt32(source, ref offset);
			if (count < 0)
			{
				throw new InvalidOperationException(String.Concat("Expected positive endpoint count but recieved ", count));
			}
			for (int i = 0; i < count; i++)
			{
				result.Add(copier.Copy(KnownEndpoint.Create(reader, source, ref offset)));
			}
			return result.ToReadOnly();
		}

		internal static IEnumerable<int> ReadInt32Identities(IBufferReader reader, byte[] source, ref int offset)
		{
			var identities = new List<int>();
			var idCount = reader.ReadInt32(source, ref offset);
			if (idCount < 0)
			{
				throw new InvalidOperationException(String.Concat("Expected positive identity count but recieved ", idCount));
			}
			for (int i = 0; i < idCount; i++)
			{
				identities.Add(reader.ReadInt32(source, ref offset));
			}
			return identities.ToReadOnly();
		}

		internal static IEnumerable<long> ReadInt64Identities(IBufferReader reader, byte[] source, ref int offset)
		{
			var identities = new List<long>();
			var idCount = reader.ReadInt32(source, ref offset);
			if (idCount < 0)
			{
				throw new InvalidOperationException(String.Concat("Expected positive identity count but recieved ", idCount));
			}
			for (int i = 0; i < idCount; i++)
			{
				identities.Add(reader.ReadInt64(source, ref offset));
			}
			return identities.ToReadOnly();
		}

		internal static IEnumerable<string> ReadStringIdentities(IBufferReader reader, byte[] source, ref int offset)
		{
			var identities = new List<string>();
			var idCount = reader.ReadInt32(source, ref offset);
			if (idCount < 0)
			{
				throw new InvalidOperationException(String.Concat("Expected positive identity count but recieved ", idCount));
			}
			for (int i = 0; i < idCount; i++)
			{
				identities.Add(reader.ReadStringWithByteCountPrefix(source, ref offset));
			}
			return identities.ToReadOnly();
		}

		internal static IEnumerable<Guid> ReadGuidIdentities(IBufferReader reader, byte[] source, ref int offset)
		{
			var identities = new List<Guid>();
			var idCount = reader.ReadInt32(source, ref offset);
			if (idCount < 0)
			{
				throw new InvalidOperationException(String.Concat("Expected positive identity count but recieved ", idCount));
			}
			for (int i = 0; i < idCount; i++)
			{
				identities.Add(reader.ReadGuid(source, ref offset));
			}
			return identities.ToReadOnly();
		}

	}

}
