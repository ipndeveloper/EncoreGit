using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Sql.CacheNotifications
{
	class CRC
	{
		internal const int CrcTableLength = 256;
	}

	/// <summary>
	/// Utility class for generating CRC16 checksums.
	/// </summary>
	public class Crc16
	{
		static readonly ushort[] __table = new ushort[CRC.CrcTableLength];

		/// <summary>
		/// Computes a checksum over an array of bytes.
		/// </summary>
		/// <param name="bytes">the bytes</param>
		/// <returns>the checksum</returns>
		[CLSCompliant(false)]
		public ushort ComputeChecksum(byte[] bytes)
		{
			if (bytes == null) throw new ArgumentNullException("bytes");
			ushort crc = 0;
			var len = bytes.Length;
			for (int i = 0; i < len; ++i)
			{
				byte index = (byte)(crc ^ bytes[i]);
				crc = (ushort)((crc >> 8) ^ __table[index]);
			}
			return crc;
		}

		static Crc16()
		{
			const ushort poly = 0xA001;
			ushort value;
			ushort temp;
			for (ushort i = 0; i < CRC.CrcTableLength; ++i)
			{
				value = 0;
				temp = i;
				for (byte j = 0; j < 8; ++j)
				{
					if (((value ^ temp) & 0x0001) != 0)
					{
						value = (ushort)((value >> 1) ^ poly);
					}
					else
					{
						value >>= 1;
					}
					temp >>= 1;
				}
				__table[i] = value;
			}
		}
	}

	/// <summary>
	/// Utility class for generating CRC16 checksums.
	/// </summary>
	public class Crc32
	{
		static readonly uint[] __table = new uint[CRC.CrcTableLength];

		/// <summary>
		/// Computes a checksum over an array of bytes.
		/// </summary>
		/// <param name="bytes">the bytes</param>
		/// <returns>the checksum</returns>
		[CLSCompliant(false)]
		public uint ComputeChecksum(byte[] bytes)
		{
			if (bytes == null) throw new ArgumentNullException("bytes");
			uint crc = 0xffffffff;
			for (int i = 0; i < bytes.Length; ++i)
			{
				byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
				crc = (uint)((crc >> 8) ^ __table[index]);
			}
			return ~crc;
		}

		/// <summary>
		/// Computes a checksum over an array of bytes beginning with the first and
		/// continuing to length.
		/// </summary>
		/// <param name="bytes"></param>
		/// <param name="first"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		[CLSCompliant(false)]
		public uint ComputeChecksum(byte[] bytes, int first, int length)
		{
			uint crc = 0xffffffff;
			for (int i = first; i < length; ++i)
			{
				byte index = (byte)(((crc) & 0xff) ^ bytes[i]);
				crc = (uint)((crc >> 8) ^ __table[index]);
			}
			return ~crc;
		}

		static Crc32()
		{
			uint poly = 0xedb88320;
			uint temp = 0;
			for (uint i = 0; i < CRC.CrcTableLength; ++i)
			{
				temp = i;
				for (int j = 8; j > 0; --j)
				{
					if ((temp & 1) == 1)
					{
						temp = (uint)((temp >> 1) ^ poly);
					}
					else
					{
						temp >>= 1;
					}
				}
				__table[i] = temp;
			}
		}
	}

	/// <summary>
	/// A few common initial CRC values
	/// </summary>
	public enum InitialCrcValue
	{
		/// <summary>
		/// All zero.
		/// </summary>
		Zeros = 0,
		/// <summary>
		/// Common initial value of 0x1D0F
		/// </summary>
		NonZero_x1D0F = 0x1d0f,
		/// <summary>
		/// Common initial value of 0xFFFF
		/// </summary>
		NonZero_xFFFF = 0xffff,
	}

	/// <summary>
	/// Utility class for generating CRC16CITT checksums.
	/// </summary>
	public class Crc16Ccitt
	{
		static readonly ushort[] __table = new ushort[CRC.CrcTableLength];

		readonly ushort _initialValue;

		/// <summary>
		/// Computes a checksum over an array of bytes.
		/// </summary>
		/// <param name="bytes">the bytes</param>
		/// <returns>the checksum</returns>
		[CLSCompliant(false)]
		public ushort ComputeChecksum(byte[] bytes)
		{
			if (bytes == null) throw new ArgumentNullException("bytes");
			ushort crc = this._initialValue;
			var len = bytes.Length;
			for (int i = 0; i < len; ++i)
			{
				crc = (ushort)((crc << 8) ^ __table[((crc >> 8) ^ (0xff & bytes[i]))]);
			}
			return crc;
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="initialValue">which initial value the checksum should use</param>
		public Crc16Ccitt(InitialCrcValue initialValue)
		{
			this._initialValue = (ushort)initialValue;
		}

		static Crc16Ccitt()
		{
			const ushort poly = 4129;
			ushort temp, a;
			for (int i = 0; i < CRC.CrcTableLength; ++i)
			{
				temp = 0;
				a = (ushort)(i << 8);
				for (int j = 0; j < 8; ++j)
				{
					if (((temp ^ a) & 0x8000) != 0)
					{
						temp = (ushort)((temp << 1) ^ poly);
					}
					else
					{
						temp <<= 1;
					}
					a <<= 1;
				}
				__table[i] = temp;
			}
		}
	}
}
