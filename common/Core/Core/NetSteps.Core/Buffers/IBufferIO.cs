using System;

namespace NetSteps.Encore.Core.Buffers
{
	/// <summary>
	/// Interface for objects that provide their own buffer IO.
	/// </summary>
	[CLSCompliant(false)]
	public interface IBufferIO
	{
		/// <summary>
		/// Writes to the buffer.
		/// </summary>
		/// <param name="writer">a buffer writer</param>
		/// <param name="target">the target buffer</param>
		/// <param name="offset">reference to an offset into the buffer where writing
		/// can begin; upon exit, must be incremented by the number of bytes consumed</param>
		/// <returns>total number of bytes written</returns>
		int WriteToBuffer(IBufferWriter writer, byte[] target, ref int offset);
		/// <summary>
		/// Reads from the buffer.
		/// </summary>
		/// <param name="reader">a buffer reader</param>
		/// <param name="source">the source buffer</param>
		/// <param name="offset">reference to an offset into the buffer where reading
		/// can begin; upon exit, must be incremented by the number of bytes consumed</param>
		/// <returns>the number of bytes consumed during the read</returns>
		int ReadFromBuffer(IBufferReader reader, byte[] source, ref int offset);
	}
}
