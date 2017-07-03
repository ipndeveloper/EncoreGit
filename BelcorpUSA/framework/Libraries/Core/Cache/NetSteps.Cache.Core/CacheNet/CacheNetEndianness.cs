using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSteps.Core.Cache.CacheNet
{
	/// <summary>
	/// Indicates the Byte Order of values in a CacheNet packet
	/// </summary>
	public enum CacheNetEndianness
	{
		/// <summary>
		/// The Byte Order is Big Endian
		/// </summary>
		BigEndian = 0x00,
		
		/// <summary>
		/// The Byte Order is Little Endian
		/// </summary>
		LittleEndian = 0x01
	}
}
