using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.Properties;
using System.Text;

namespace NetSteps.Encore.Core.Buffers
{
	/// <summary>
	/// Implementation of IBufferreader that reads little endian data from a buffer.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class LittleEndianBufferReader : BufferReader
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public LittleEndianBufferReader() : base(Encoding.Unicode)
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="enc">an encoding</param>
		public LittleEndianBufferReader(Encoding enc)
			: base(enc)
		{
		}

		/// <summary>
		/// Reads an UInt16 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		public override ushort ReadUInt16(byte[] buffer, ref int offset)
		{
			Contract.Assert(buffer != null);
			Contract.Assert(offset >= 0);
			Contract.Assert(offset <= buffer.Length - 2, Resources.Chk_OffsetWouldResultInBufferOverrun);
			
			ushort result;
			unchecked
			{
				result = (ushort)(((uint)buffer[offset] << 8) | (uint)buffer[offset + 1]);
			}
			offset += 2;
			return result;
		}

		/// <summary>
		/// Reads an UInt32 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		public override uint ReadUInt32(byte[] buffer, ref int offset)
		{
			Contract.Assert(buffer != null);
			Contract.Assert(offset >= 0);
			Contract.Assert(offset <= buffer.Length - 2, Resources.Chk_OffsetWouldResultInBufferOverrun);
			
			uint result;
			unchecked
			{
				result = (((uint)buffer[offset] << 24)
					| ((uint)buffer[offset + 1] << 16)
					| ((uint)buffer[offset + 2] << 8)
					| (uint)buffer[offset + 3]);
			}
			offset += 4;
			return result;
		}

		/// <summary>
		/// Reads an UInt64 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		public override ulong ReadUInt64(byte[] buffer, ref int offset)
		{
			Contract.Assert(buffer != null);
			Contract.Assert(offset >= 0);
			Contract.Assert(offset <= buffer.Length - 2, Resources.Chk_OffsetWouldResultInBufferOverrun);
			
			ulong result;
			unchecked
			{
				result = (((ulong)buffer[offset] << 56)
					| ((ulong)buffer[offset + 1] << 48)
					| ((ulong)buffer[offset + 2] << 40)
					| ((ulong)buffer[offset + 3] << 32)
					| ((ulong)buffer[offset + 4] << 24)
					| ((ulong)buffer[offset + 5] << 16)
					| ((ulong)buffer[offset + 6] << 8)
					| (ulong)buffer[offset + 7]);
			}
			offset += 8;
			return result;
		}

	}

}
