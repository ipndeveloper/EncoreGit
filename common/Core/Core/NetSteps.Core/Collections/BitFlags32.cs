using System;
using System.Runtime.InteropServices;
using System.Text;

namespace NetSteps.Encore.Core.Collections
{
	/// <summary>
	/// Utility class for working with bit/flags.
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	public struct BitFlags32 : IEquatable<BitFlags32>
	{
		static readonly int CHashCodeSeed = typeof(BitFlags32).GetKeyForType().GetHashCode();
		internal const int CFlagCount = 32;
		static readonly string[] CNibbleBitPatterns = new string[] { "0000", "0001", "0010", "0011", "0100", "0101", "0110", "0111", "1000", "1001", "1010", "1011", "1100", "1101", "1110", "1111", };

		/// <summary>
		/// Empty instance; all bits turned off.
		/// </summary>
		public static readonly BitFlags32 Empty = new BitFlags32(0);

		int _flags;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="flags">flag values</param>
		public BitFlags32(int flags)
		{
			_flags = flags;
		}

		/// <summary>
		/// Indicates whether the bit flags are empty (none set to true).
		/// </summary>
		public bool IsEmpty { get { return _flags == 0; } }

		/// <summary>
		/// Number of flags currently set to true.
		/// </summary>
		public int TrueFlagCount
		{
			get { return _flags.CountBitsInFlag(); }
		}

		/// <summary>
		/// Gets and sets bit values to ON (true) or OFF (false).
		/// </summary>
		public bool this[int bit]
		{
			get
			{
				if (bit < 0 || bit >= CFlagCount)
					throw new ArgumentOutOfRangeException("bit", "bit must be between 0 and 31 inclusive");

				return (_flags & (1 << bit)) != 0;
			}
		}

		/// <summary>
		/// Turns the bit on at the position indicated.
		/// </summary>
		/// <param name="bit">bit position</param>
		/// <returns>flags with the indicated bit turned on</returns>
		public BitFlags32 On(int bit)
		{
			if (bit < 0 || bit >= CFlagCount)
				throw new ArgumentOutOfRangeException("bit", "bit must be between 0 and 31 inclusive");

			return IncludeFlags(1 << bit);
		}

		/// <summary>
		/// Turns the bit off at the position indicated.
		/// </summary>
		/// <param name="bit">bit position</param>
		/// <returns>flags with the indicated bit turned off</returns>
		public BitFlags32 Off(int bit)
		{
			if (bit < 0 || bit >= CFlagCount)
				throw new ArgumentOutOfRangeException("bit", "bit must be between 0 and 31 inclusive");

			return ExcludeFlags(1 << bit);
		}

		/// <summary>
		/// Includes all of the flags given. (turns on bits corresponding to the bits given)
		/// </summary>
		/// <param name="flags">flags to turn on</param>
		/// <returns>the flags for chaining</returns>
		public BitFlags32 IncludeFlags(int flags)
		{
			return new BitFlags32(_flags | flags);
		}

		/// <summary>
		/// Excludes all of the flags given. (turns off bits corresponding to the bits given)
		/// </summary>
		/// <param name="flags">flags to turn off</param>
		/// <returns>the flags for chaining</returns>
		public BitFlags32 ExcludeFlags(int flags)
		{
			return new BitFlags32(_flags &= ~(flags));
		}

		/// <summary>
		/// Determines if the flags are equal to another.
		/// </summary>
		/// <param name="other">the other</param>
		/// <returns>true if equal; otherwise false</returns>
		public bool Equals(BitFlags32 other)
		{
			return _flags == other._flags;
		}

		/// <summary>
		/// Determines if the flags are equal to another object.
		/// </summary>
		/// <param name="obj">the other object</param>
		/// <returns>true if equal; otherwise false</returns>
		public override bool Equals(object obj)
		{
			return obj is BitFlags32
				&& Equals((BitFlags32)obj);
		}

		/// <summary>
		/// Gets a hashcode for the instance.
		/// </summary>
		/// <returns>a hashcode</returns>
		public override int GetHashCode()
		{
			int prime = Constants.RandomPrime; // a random prime

			int result = CHashCodeSeed * prime;
			result ^= prime * _flags;
			return result;
		}

		/// <summary>
		/// Converts the bit vector into a bit string.
		/// </summary>
		/// <returns>bits string</returns>
		public override string ToString()
		{
			var bits = new StringBuilder(32);
			bits.Append(CNibbleBitPatterns[(_flags & 0xF0000000) >> 28]);
			bits.Append(CNibbleBitPatterns[(_flags & 0x0F000000) >> 24]);
			bits.Append(CNibbleBitPatterns[(_flags & 0x00F00000) >> 20]);
			bits.Append(CNibbleBitPatterns[(_flags & 0x000F0000) >> 16]);
			bits.Append(CNibbleBitPatterns[(_flags & 0x0000F000) >> 12]);
			bits.Append(CNibbleBitPatterns[(_flags & 0x00000F00) >> 8]);
			bits.Append(CNibbleBitPatterns[(_flags & 0x000000F0) >> 4]);
			bits.Append(CNibbleBitPatterns[_flags & 0x0000000F]);
			return bits.ToString();
		}

		/// <summary>
		/// Equality operator.
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are equal; otherwise false</returns>
		public static bool operator ==(BitFlags32 lhs, BitFlags32 rhs)
		{
			return lhs._flags == rhs._flags;
		}

		/// <summary>
		/// Inequality operator.
		/// </summary>
		/// <param name="lhs">left hand comparand</param>
		/// <param name="rhs">right hand comparand</param>
		/// <returns>true if the comparands are NOT equal; otherwise false</returns>
		public static bool operator !=(BitFlags32 lhs, BitFlags32 rhs)
		{
			return lhs._flags != rhs._flags;
		}

		/// <summary>
		/// Implicit conversion operator from BitFlags to Int32.
		/// </summary>
		/// <param name="flags">value to convert</param>
		/// <returns>an Int32 representation of the flags</returns>
		public static implicit operator int(BitFlags32 flags)
		{
			return flags._flags;
		}

		/// <summary>
		/// Implicit converstion operator from Int32 to BitFlags32
		/// </summary>
		/// <param name="flags">value to convert</param>
		/// <returns>a BitFlags32</returns>
		public static implicit operator BitFlags32(int flags)
		{
			return new BitFlags32(flags);
		}
	}
}
