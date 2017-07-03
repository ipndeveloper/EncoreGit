#region COPYRIGHT© 2009-2013 Phillip Clark. All rights reserved.
// For licensing information see License.txt (MIT style licensing).
// Copied from the FlitBit.Core Open Source library http://fbcore.codeplex.com
#endregion

using System;
using System.Text;

namespace NetSteps.Sql.CacheNotifications.Buffers
{
	/// <summary>
	/// Helper class for writing little-endian binary data to a buffer.
	/// </summary>
	public sealed class LittleEndianBufferWriter : BufferWriter
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public LittleEndianBufferWriter() : base(Encoding.Unicode)
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="enc">the encoding used to produce bytes for strings.</param>
		public LittleEndianBufferWriter(Encoding enc)
			: base(enc)
		{
		}

		/// <summary>
		/// Writes an UInt16 to a buffer.
		/// </summary>
		/// <param name="buffer">the target buffer</param>
		/// <param name="offset">an offset where writing begins</param>
		/// <param name="value">a value</param>
		/// <returns>the number of bytes written to the buffer</returns>
		[CLSCompliant(false)]
		public override int Write(byte[] buffer, ref int offset, ushort value)
		{
			//Contract.Assert(buffer != null);
			//Contract.Assert(offset >= 0);
			//Contract.Assert(offset <= buffer.Length - 2, Resources.Chk_OffsetWouldResultInBufferOverrun);
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (!(offset <= buffer.Length - 2))
			{
				throw new ArgumentOutOfRangeException("offset", "buffer and offset paramaters would result in a Buffer Overrun.");
			}

			int v = (int)value;
			unchecked
			{
				buffer[offset] = (byte)((v >> 8) & 0xFF);
				buffer[offset + 1] = (byte)(v & 0xFF);
			}
			offset += sizeof(UInt16);
			return sizeof(UInt16);
		}

		/// <summary>
		/// Writes an UInt32 to a buffer.
		/// </summary>
		/// <param name="buffer">the target buffer</param>
		/// <param name="offset">an offset where writing begins</param>
		/// <param name="value">a value</param>
		/// <returns>the number of bytes written to the buffer</returns>
		[CLSCompliant(false)]
		public override int Write(byte[] buffer, ref int offset, uint value)
		{
			//Contract.Assert(buffer != null);
			//Contract.Assert(offset >= 0);
			//Contract.Assert(offset <= buffer.Length - 4, Resources.Chk_OffsetWouldResultInBufferOverrun);
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (!(offset <= buffer.Length - 4))
			{
				throw new ArgumentOutOfRangeException("offset", "buffer and offset paramaters would result in a Buffer Overrun.");
			}

			unchecked
			{
				buffer[offset] = (byte)((value >> 24) & 0xFF);
				buffer[offset + 1] = (byte)((value >> 16) & 0xFF);
				buffer[offset + 2] = (byte)((value >> 8) & 0xFF);
				buffer[offset + 3] = (byte)(byte)(value & 0xFF);
			}
			offset += sizeof(UInt32);
			return sizeof(UInt32);
		}

		/// <summary>
		/// Writes an UInt64 to a buffer.
		/// </summary>
		/// <param name="buffer">the target buffer</param>
		/// <param name="offset">an offset where writing begins</param>
		/// <param name="value">a value</param>
		/// <returns>the number of bytes written to the buffer</returns>
		[CLSCompliant(false)]
		public override int Write(byte[] buffer, ref int offset, ulong value)
		{
			//Contract.Assert(buffer != null);
			//Contract.Assert(offset >= 0);
			//Contract.Assert(offset <= buffer.Length - 8, Resources.Chk_OffsetWouldResultInBufferOverrun);
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (offset < 0)
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (!(offset <= buffer.Length - 8))
			{
				throw new ArgumentOutOfRangeException("offset", "buffer and offset paramaters would result in a Buffer Overrun.");
			}

			unchecked
			{
				buffer[offset] = (byte)((value >> 56) & 0xFF);
				buffer[offset + 1] = (byte)((value >> 48) & 0xFF);
				buffer[offset + 2] = (byte)((value >> 40) & 0xFF);
				buffer[offset + 3] = (byte)((value >> 32) & 0xFF);
				buffer[offset + 4] = (byte)((value >> 24) & 0xFF);
				buffer[offset + 5] = (byte)((value >> 16) & 0xFF);
				buffer[offset + 6] = (byte)((value >> 8) & 0xFF);
				buffer[offset + 7] = (byte)(value & 0xFF);
			}
			offset += sizeof(UInt64);
			return sizeof(UInt64);
		}
	}
}
