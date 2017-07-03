using System;
using System.Text;

namespace NetSteps.Encore.Core.Buffers
{
	/// <summary>
	/// Helper for writing binary data to a buffer.
	/// </summary>
	[CLSCompliant(false)]
	public interface IBufferWriter
	{
		/// <summary>
		/// Gets the encoding used when writing string data.
		/// </summary>
		Encoding Encoding { get; }
		/// <summary>
		/// Fills a buffer.
		/// </summary>
		/// <param name="buffer">a buffer</param>
		/// <param name="offset">offset to begin</param>
		/// <param name="count">number of bytes to fill</param>
		/// <param name="value">fill value</param>
		void FillBytes(byte[] buffer, ref int offset, int count, byte value);
		/// <summary>
		/// Writes a boolean value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, bool value);
		/// <summary>
		/// Writes a byte value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, byte value);
		/// <summary>
		/// Writes a byte array to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, byte[] value);
		/// <summary>
		/// Writes from a byte array to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="source">the source array</param>
		/// <param name="sourceOffset">offset into source where copying begins</param>
		/// <param name="count">number of bytes to copy from source into buffer</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, byte[] source, int sourceOffset, int count);
		/// <summary>
		/// Writes a char value (two-byte) to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, char value);
		/// <summary>
		/// Writes an array of chars to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <param name="byteLengthPrefix">whether a byte length prefix should be written</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, char[] value, bool byteLengthPrefix);
		/// <summary>
		/// Writes an array of chars to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <param name="byteLengthPrefix">whether a byte length prefix should be written</param>
		/// <param name="coder">an encoding used to transform the string to bytes</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, char[] value, bool byteLengthPrefix, Encoding coder);
		/// <summary>
		/// Writes a decimal value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, decimal value);
		/// <summary>
		/// Writes a boolean value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, double value);
		/// <summary>
		/// Writes a Guid value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, Guid value);
		/// <summary>
		/// Writes an Int16 value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, short value);
		/// <summary>
		/// Writes an Int32 value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, int value);
		/// <summary>
		/// Writes an Int64 value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, long value);
		/// <summary>
		/// Writes a signed byte value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, sbyte value);
		/// <summary>
		/// Writes a float to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, float value);
		/// <summary>
		/// Writes a string to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the string</param>
		/// <param name="byteLengthPrefix">whether a byte length prefix should be written</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, string value, bool byteLengthPrefix);
		/// <summary>
		/// Writes a string to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the string</param>
		/// <param name="byteLengthPrefix">whether a byte length prefix should be written</param>
		/// <param name="coder">an encoding used to transform the string to bytes</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, string value, bool byteLengthPrefix, Encoding coder);
		/// <summary>
		/// Writes an UInt16 value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, ushort value);
		/// <summary>
		/// Writes an UInt32 value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, uint value);
		/// <summary>
		/// Writes an UInt64 value to the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="value">the value</param>
		/// <returns>number of bytes written</returns>
		int Write(byte[] buffer, ref int offset, ulong value);
		/// <summary>
		/// Writes an instance of type T to the buffer.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where writing begins</param>
		/// <param name="reflector">reflector for writing type T</param>
		/// <param name="value">the instance</param>
		/// <returns>number of bytes written</returns>
		int WriteReflectedObject<T>(byte[] buffer, ref int offset, IBufferReflector<T> reflector, T value);
	}

}
