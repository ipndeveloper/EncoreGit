using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Buffers;
using System.Diagnostics.Contracts;

namespace NetSteps.Core.Cache.CacheNet
{
	/// <summary>
	/// Describes header information about a CacheNet Message
	/// </summary>
	[CLSCompliant(false)]
	public sealed class CacheNetMessageHeader
	{
		#region Properties
		
		/// <summary>
		/// The Kind of the underlying Message
		/// </summary>
		public CacheNetMessageKind MessageKind { get; private set; }

		/// <summary>
		/// The Endianess that the Message follows when written or read from a buffer
		/// </summary>
		public CacheNetEndianness Endianness { get; private set; }

		/// <summary>
		/// The checksum value of the Message payload.
		/// </summary>
		public uint Checksum { get; private set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Constructs a new CacheNet message
		/// </summary>
		/// <param name="endianness">Indicates the endianess of the bytes when this message is written to a buffer</param>
		/// <param name="messageKind">The kind of the underlying message payload.</param>
		/// <param name="checksum">The checksum of the messge payload.</param>
		public CacheNetMessageHeader(CacheNetEndianness endianness, CacheNetMessageKind messageKind, uint checksum)
		{
			Endianness = endianness;
			MessageKind = messageKind;
			Checksum = checksum;
		}

		#endregion

		#region Methods
		
		/// <summary>
		/// Populates the CacheNetMessageHeader instance with data recieved on the buffer
		/// </summary>
		/// <param name="reader">The <see cref="IBufferReader"/> providing the correct endianess capabilties to read the message from the buffer</param>
		/// <param name="source">The buffer containing the message data</param>
		/// <param name="offset">The poition in the buffer to begin reading from.  This header type has a special purpose and expects this offset to be 8, the byte position following the Preamble check, which is generally performed by the Protocol.</param>
		/// <returns>The number of bytes read from the buffer</returns>
		public int ReadFromBuffer(IBufferReader reader, byte[] source, ref int offset)
		{
			Contract.Requires<ArgumentNullException>(reader != null);
			Contract.Requires<ArgumentNullException>(source != null);
			Contract.Requires<ArgumentOutOfRangeException>(source.Length > 0);
			Contract.Requires<ArgumentOutOfRangeException>(offset >= 0);
			Contract.Requires<ArgumentOutOfRangeException>(offset == 8, "CacheNetMessageHeader expects to be the first reader of the buffer following the Preamble Check.");

			int cursor = offset;

			MessageKind = (CacheNetMessageKind)reader.ReadInt32(source, ref cursor);

			var crc = CacheNetProtocol.Crc32.ComputeChecksum(source, cursor, source.Length - 4);
			int ofs = source.Length - 4;
			var checksum = reader.ReadUInt32(source, ref ofs);
			if (crc != checksum)
			{
				throw new InvalidOperationException("The buffer is corrupt; checksum does not match.");
			}

			Checksum = checksum;

			int result = cursor - offset;
			offset = cursor;
			return result;
		}

		/// <summary>
		/// Writes the message to the given buffer
		/// </summary>
		/// <param name="writer">The <see cref="IBufferWriter"/> providing the correct endianess capabilties to read the message from the buffer</param>
		/// <param name="target">The buffer to write to</param>
		/// <param name="offset">The position in the buffer to begin writting</param>
		/// <returns>The number of bytes written</returns>
		public int WriteToBuffer(IBufferWriter writer, byte[] target, ref int offset)
		{
			throw new NotImplementedException();
		} 

		#endregion
	}
}
