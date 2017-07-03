using System;
using System.Text;

namespace NetSteps.Encore.Core.Buffers
{
	/// <summary>
	/// Helper for reading binary data from a buffer.
	/// </summary>
	[CLSCompliant(false)]
	public interface IBufferReader
	{
		/// <summary>
		/// Gets the encoding used when reading string data.
		/// </summary>
		Encoding Encoding { get; }
		/// <summary>
		/// Reads a boolean from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		bool ReadBoolean(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads a byte from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		byte ReadByte(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an array of bytes from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <param name="count">the number of bytes in the array</param>
		/// <returns>a value</returns>
		byte[] ReadBytes(byte[] buffer, ref int offset, int count);
		/// <summary>
		/// Reads a char from the buffer (two-byte).
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		char ReadChar(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an array of characters from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <param name="arrayLength">the number of characters in the array</param>
		/// <returns>a value</returns>
		char[] ReadCharArray(byte[] buffer, ref int offset, int arrayLength);
		/// <summary>
		/// Reads a decimal from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		decimal ReadDecimal(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads a double from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		double ReadDouble(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an encoded string from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <param name="byteCount">the number of bytes used by the string</param>
		/// <param name="coder">an encoding used to interpret the bytes</param>
		/// <returns>a value</returns>
		string ReadEncodedString(byte[] buffer, ref int offset, int byteCount, Encoding coder);
		/// <summary>
		/// Reads a Guid from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		Guid ReadGuid(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an Int16 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		short ReadInt16(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an Int32 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		int ReadInt32(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an UInt64 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		long ReadInt64(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads a signed byte from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		sbyte ReadSByte(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads a Single from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		float ReadSingle(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads a length-prefixed string from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		string ReadStringWithByteCountPrefix(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads a length-prefixed string from the buffer using the encoding given.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <param name="coder">an encoding used to interpret the bytes read</param>
		/// <returns>a value</returns>
		string ReadStringWithByteCountPrefix(byte[] buffer, ref int offset, Encoding coder);
		/// <summary>
		/// Reads an UInt16 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		ushort ReadUInt16(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an UInt32 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		uint ReadUInt32(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an UInt64 from the buffer.
		/// </summary>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <returns>a value</returns>
		ulong ReadUInt64(byte[] buffer, ref int offset);
		/// <summary>
		/// Reads an instance of type T from the buffer.
		/// </summary>
		/// <typeparam name="T">type T</typeparam>
		/// <param name="buffer">the buffer</param>
		/// <param name="offset">offest into buffer where reading begins</param>
		/// <param name="reflector">reflector for reading type T</param>
		/// <returns>the instance of type T read from the buffer</returns>
		T ReadReflectedObject<T>(byte[] buffer, ref int offset, IBufferReflector<T> reflector);	
	}
}
