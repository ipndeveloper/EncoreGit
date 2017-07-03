using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Encore.Core.Buffers;

namespace NetSteps.Core.Cache.CacheNet
{
	/// <summary>
	/// Base class for CacheNet messages.
	/// </summary>
	[CLSCompliant(false)]	
	public abstract class CacheNetMessage : IBufferIO
	{
		/// <summary>
		/// Gets the message's header.
		/// </summary>
		public CacheNetMessageHeader Header { get; private set; }
		/// <summary>
		/// Gets the message's context keys.
		/// </summary>
		public IEnumerable<string> ContextKeys { get; private set; }


		/// <summary>
		/// Creates a new instance, with header information.
		/// </summary>
		/// <param name="header">the header</param>
		protected CacheNetMessage(CacheNetMessageHeader header)
		{
			Header = header;
		}

		/// <summary>
		/// Writes the instance into the target buffer.
		/// </summary>
		/// <param name="writer">a writer</param>
		/// <param name="target">the target buffer</param>
		/// <param name="offset">offset where writing begins</param>
		/// <returns>number of bytes written</returns>
		public int WriteToBuffer(IBufferWriter writer, byte[] target, ref int offset)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Reads the instance from the source buffer.
		/// </summary>
		/// <param name="reader">a reader</param>
		/// <param name="source">a source buffeer</param>
		/// <param name="offset">offset where reading begins</param>
		/// <returns>number of bytes read</returns>
		public int ReadFromBuffer(IBufferReader reader, byte[] source, ref int offset)
		{
			var ofs = offset;
			
			this.ContextKeys = CacheNetProtocol.ReadContextKeys(reader, source, ref ofs);
			
			PerformReadFromBuffer(reader, source, ref ofs);

			var result = offset - ofs;
			offset = ofs;
			return result;
		}

		/// <summary>
		/// Used by subclasses to read their own data from a source buffer.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="source"></param>
		/// <param name="offset"></param>
		protected abstract void PerformReadFromBuffer(IBufferReader reader, byte[] source, ref int offset);
		/// <summary>
		/// Used by subclasses to write their own data to a target buffer.
		/// </summary>
		/// <param name="reader"></param>
		/// <param name="target"></param>
		/// <param name="offset"></param>
		protected abstract void PerformWriteToBuffer(IBufferReader reader, byte[] target, ref int offset);
	}
}
