using System;
using System.Diagnostics.Contracts;
using System.Text;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.Buffers
{
	/// <summary>
	/// Helper class that reads big-endian binary data from a buffer.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class BigEndianBufferReader : BufferReader
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public BigEndianBufferReader() : base(Encoding.Unicode)
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="enc">an encoding</param>
		public BigEndianBufferReader(Encoding enc)
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
				result = (ushort)(((uint)buffer[offset + 1] << 8) | (uint)buffer[offset]);
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
			Contract.Assert(offset <= buffer.Length - 4, Resources.Chk_OffsetWouldResultInBufferOverrun);
			
			uint result;
			unchecked
			{
				result = (((uint)buffer[offset + 3] << 24)
					| ((uint)buffer[offset + 2] << 16)
					| ((uint)buffer[offset + 1] << 8)
					| (uint)buffer[offset]);
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
			Contract.Assert(offset <= buffer.Length - 8, Resources.Chk_OffsetWouldResultInBufferOverrun);
			
			ulong result;
			unchecked
			{
				result = (((ulong)buffer[offset + 7] << 56)
					| ((ulong)buffer[offset + 6] << 48)
					| ((ulong)buffer[offset + 5] << 40)
					| ((ulong)buffer[offset + 4] << 32)
					| ((ulong)buffer[offset + 3] << 24)
					| ((ulong)buffer[offset + 2] << 16)
					| ((ulong)buffer[offset + 1] << 8)
					| (ulong)buffer[offset]);
			}
			offset += 8;
			return result;
		}
	}
	
}
