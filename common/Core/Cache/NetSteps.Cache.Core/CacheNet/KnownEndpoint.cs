using System;
using NetSteps.Encore.Core.Buffers;
using NetSteps.Encore.Core.Dto;

namespace NetSteps.Core.Cache.CacheNet
{
	/// <summary>
	/// DTO for tracking known endpoints.
	/// </summary>
	[DTO]
	public interface IKnownEndpoint
	{
		/// <summary>
		/// Gets and sets the endpoint's ip address.
		/// </summary>
		string IPAddress { get; set; }
		/// <summary>
		/// Gets and sets the endpoint's port.
		/// </summary>
		int Port { get; set; }
	}

	internal class KnownEndpoint : IBufferIO
	{
		public string IPAddress { get; private set; }
		public int Port { get; private set; }

		internal static KnownEndpoint Create(IBufferReader reader, byte[] source, ref int offset)
		{
			var result = new KnownEndpoint();
			result.ReadFromBuffer(reader, source, ref offset);
			return result;
		}

		public int WriteToBuffer(IBufferWriter writer, byte[] target, ref int offset)
		{
			var count = writer.Write(target, ref offset, IPAddress, true);
			count += writer.Write(target, ref offset, Port);
			return count;
		}

		public int ReadFromBuffer(IBufferReader reader, byte[] source, ref int offset)
		{
			var ofs = offset;
			IPAddress = reader.ReadStringWithByteCountPrefix(source, ref ofs);
			Port = reader.ReadInt32(source, ref ofs);
			return ofs - offset;
		}
	}
}
