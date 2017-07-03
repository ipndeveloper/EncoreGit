using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Security.Cryptography;
using System.Threading;
using NetSteps.Encore.Core.Buffers;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Utility class for generating random data.
	/// </summary>
	[CLSCompliant(false)]
	public sealed class DataGenerator
	{
		class Buffer
		{
			internal int Position;
			internal byte[] Bytes;
			internal int BytesRemaining { get { return Bytes.Length - Position; } }
		}

		static readonly int __allocationLength = 1024;
		static Buffer __buffer;
		IBufferReader _reader;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DataGenerator() : this(BufferReader.Create())
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="reader">A reader for interpreting the random bytes.</param>
		public DataGenerator(IBufferReader reader)
		{
			Contract.Requires<ArgumentNullException>(reader != null);
			_reader = reader;
		}

		static void FillBuffer(byte[] buffer)
		{
			int pos = 0;
			Thread.MemoryBarrier();
			var current = __buffer;
			Thread.MemoryBarrier();
			if (current == null)
			{
				current = AllocRandomBuffer();
				Interlocked.CompareExchange(ref __buffer, current, null);
			}

			while (pos < buffer.Length)
			{
				var rmn = buffer.Length - pos;
				lock (current)
				{
					var len = Math.Min(current.BytesRemaining, rmn);
					Array.Copy(current.Bytes, current.Position, buffer, pos, len);
					current.Position += len;
					pos += len;
					if (len == rmn) break;
				}

				var @new = AllocRandomBuffer();
				Interlocked.CompareExchange(ref __buffer, @new, current);
				current = @new;
			}
		}
		static Buffer AllocRandomBuffer()
		{
			var buffer = new Buffer { Position = 0, Bytes = new byte[__allocationLength] };
			using (var rng = new RNGCryptoServiceProvider())
			{
				rng.GetBytes(buffer.Bytes);
			}
			return buffer;
		}

		/// <summary>
		/// Gets a random boolean value.
		/// </summary>
		/// <returns>the value</returns>
		public bool GetBoolean()
		{
			return GetBytes(1)[0] % 2 == 0;
		}

		/// <summary>
		/// Gets a random byte value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public byte GetByte()
		{
			return GetBytes(1)[0];
		}

		/// <summary>
		/// Gets an array of random byte values.
		/// </summary>
		/// <param name="len">Number of bytes in the array.</param>
		/// <returns>the value</returns>
		public byte[] GetBytes(int len)
		{
			var buffer = new byte[len];
			FillBuffer(buffer);
			return buffer;
		}

		static readonly char[] CDigits = "0123456789".ToCharArray();

		/// <summary>
		/// Gets a string of random numeric values.
		/// </summary>
		/// <param name="len">number of characters</param>
		/// <returns>the value</returns>
		public string GetNumericString(int len)
		{
			var bytes = GetBytes(len);
			var numericChars = new char[len];
			for (int i = 0; i < len; i++)
			{
				numericChars[i] = CDigits[bytes[i] % 10];
			}
			return new String(numericChars);
		}

		/// <summary>
		/// Gets a random value of enum type E.
		/// </summary>
		/// <typeparam name="E">type E</typeparam>
		/// <returns>the value</returns>
		public E GetEnum<E>()
		{
			Contract.Requires(typeof(E).IsEnum, "typeof E must be an enum");
			var values = (E[])Enum.GetValues(typeof(E));
			if (values.Length > 0)
			{
				int i = Math.Abs(GetInt32());
				if (i > values.Length)
					i = i % values.Length;
				return values[i];
			}
			return default(E);
		}

		/// <summary>
		/// Gets a random char value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public char GetChar()
		{
			var bytes = GetBytes(sizeof(Char));
			var ofs = 0;
			return _reader.ReadChar(bytes, ref ofs);
		}

		/// <summary>
		/// Gets a random Int16 value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public short GetInt16()
		{
			var bytes = GetBytes(sizeof(short));
			var ofs = 0;
			return _reader.ReadInt16(bytes, ref ofs);
		}

		/// <summary>
		/// Gets a random Int32 value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public int GetInt32()
		{
			var bytes = GetBytes(sizeof(int));
			var ofs = 0;
			return _reader.ReadInt32(bytes, ref ofs);
		}

		/// <summary>
		/// Gets a random Int64 value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public long GetInt64()
		{
			var bytes = GetBytes(sizeof(long));
			var ofs = 0;
			return _reader.ReadInt64(bytes, ref ofs);
		}

		/// <summary>
		/// Gets a random decimal value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public decimal GetDecimal()
		{
			var bytes = GetBytes(14);
			bytes[13] %= 29;
			int ofs = 0;
			return new decimal(_reader.ReadInt32(bytes, ref ofs),
				_reader.ReadInt32(bytes, ref ofs),
				_reader.ReadInt32(bytes, ref ofs),
				bytes[12] % 2 == 0,
				bytes[13]
				);
		}

		/// <summary>
		/// Gets a random double floating point value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public double GetDouble()
		{
			var bytes = GetBytes(sizeof(double));
			var ofs = 0;
			return _reader.ReadDouble(bytes, ref ofs);
		}
		/// <summary>
		/// Gets a random single floating point value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public float GetSingle()
		{
			var bytes = GetBytes(sizeof(float));
			var ofs = 0;
			return _reader.ReadSingle(bytes, ref ofs);
		}
		/// <summary>
		/// Gets a random signed-byte value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		[CLSCompliant(false)]
		public sbyte GetSByte()
		{
			var bytes = GetBytes(sizeof(sbyte));
			var ofs = 0;
			return _reader.ReadSByte(bytes, ref ofs);
		}
		/// <summary>
		/// Gets a random UInt16 value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		[CLSCompliant(false)]
		public ushort GetUInt16()
		{
			var bytes = GetBytes(sizeof(ushort));
			var ofs = 0;
			return _reader.ReadUInt16(bytes, ref ofs);
		}
		/// <summary>
		/// Gets a random UInt32 value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		[CLSCompliant(false)]
		public uint GetUInt32()
		{
			var bytes = GetBytes(sizeof(uint));
			var ofs = 0;
			return _reader.ReadUInt32(bytes, ref ofs);
		}
		/// <summary>
		/// Gets a random UInt64 value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		[CLSCompliant(false)]
		public ulong GetUInt64()
		{
			var bytes = GetBytes(sizeof(ulong));
			var ofs = 0;
			return _reader.ReadUInt64(bytes, ref ofs);
		}
		/// <summary>
		/// Gets a random guid value.
		/// </summary>
		/// <returns>the value</returns>
		[SuppressMessage("Microsoft.Design", "CA1024")]
		public Guid GetGuid()
		{
			var bytes = GetBytes(16);
			var ofs = 0;
			return _reader.ReadGuid(bytes, ref ofs);
		}
		/// <summary>
		/// Gets an array of random char values.
		/// </summary>
		/// <param name="length">number of characters</param>
		/// <returns>the value</returns>
		public char[] GetCharacterArray(int length)
		{
			Contract.Requires(length >= 0);
			if (length == 0) return new char[0];

			char[] result = new char[length];
			var i = 0;
			while (i < result.Length)
			{
				// let the majority of characters be in the ascii range...
				char c = (i % 2 == 0 || i % 3 == 0)
					? Convert.ToChar(GetByte())
					: GetChar();

				// Sanitize the characters; can produce invalid unicode codepoints (won't round-trip).
				if (Char.IsWhiteSpace(c)
					|| Char.IsLetterOrDigit(c)
					|| Char.IsPunctuation(c)
					|| Char.IsSymbol(c))
				{
					result[i++] = c;
				}
			}
			return result;
		}
		/// <summary>
		/// Gets a random string value.
		/// </summary>
		/// <param name="length">length of the string</param>
		/// <returns>the value</returns>
		public string GetString(int length)
		{
			Contract.Requires(length >= 0);
			if (length == 0) return String.Empty;

			return new String(GetCharacterArray(length));
		}

		/// <summary>
		/// Gets an array of random items.
		/// </summary>
		/// <typeparam name="T">item type T</typeparam>
		/// <param name="length">length of the new array</param>
		/// <returns>the value</returns>
		public T[] GetArray<T>(int length)
		{
			return GetArray<T>(length, true);
		}

		/// <summary>
		/// Gets an array of random items.
		/// </summary>
		/// <typeparam name="T">item type T</typeparam>
		/// <param name="length">length of the new array</param>
		/// <param name="initializeEa">indicates whether each item is initialized with a random value</param>
		/// <returns>the value</returns>
		public T[] GetArray<T>(int length, bool initializeEa)
		{
			T[] r = new T[length];
			if (initializeEa)
			{
				var tc = Type.GetTypeCode(typeof(T));
				switch (tc)
				{
					case TypeCode.Boolean:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetBoolean();
						}
						break;
					case TypeCode.Byte:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetBytes(20);
						}
						break;
					case TypeCode.Char:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetChar();
						}
						break;
					case TypeCode.DateTime:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)new DateTime(GetInt64());
						}
						break;
					case TypeCode.Decimal:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetDecimal();
						}
						break;
					case TypeCode.Double:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetDouble();
						}
						break;
					case TypeCode.Int16:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetInt16();
						}
						break;
					case TypeCode.Int32:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetInt32();
						}
						break;
					case TypeCode.Int64:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetInt64();
						}
						break;
					case TypeCode.SByte:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetSByte();
						}
						break;
					case TypeCode.Single:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetSingle();
						}
						break;
					case TypeCode.String:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetString(40); // majic!
						}
						break;
					case TypeCode.UInt16:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetUInt16();
						}
						break;
					case TypeCode.UInt32:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetUInt32();
						}
						break;
					case TypeCode.UInt64:
						for (int i = 0; i < length; i++)
						{
							r[i] = (T)(object)GetUInt64();
						}
						break;
					default:
						throw new InvalidOperationException();
				}
			}
			return r;
		}
	}

}
