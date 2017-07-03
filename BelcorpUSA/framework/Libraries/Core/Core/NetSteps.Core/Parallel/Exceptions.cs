using System;
using System.Runtime.Serialization;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Base exception thrown by the parallel framework.
	/// </summary>
	[Serializable]
	public class ParallelException : ApplicationException
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public ParallelException() : base() { }
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="message">an error message</param>
		public ParallelException(string message) : base(message) { }
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="message">an error message</param>
		/// <param name="cause">an inner exception that caused this exception</param>
		public ParallelException(string message, Exception cause) : base(message, cause) { }
		/// <summary>
		/// Used by serialization to create an instance.
		/// </summary>
		/// <param name="si"></param>
		/// <param name="sc"></param>
		protected ParallelException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
	}

	/// <summary>
	/// Exception indicating a task timed out before completion.
	/// </summary>
	[Serializable]
	public class ParallelTimeoutException : ParallelException
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public ParallelTimeoutException() : base() { }
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="message">an error message</param>
		public ParallelTimeoutException(string message) : base(message) { }
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="message">an error message</param>
		/// <param name="cause">an inner exception that caused this exception</param>
		public ParallelTimeoutException(string message, Exception cause) : base(message, cause) { }

		/// <summary>
		/// Used by serialization to create an instance.
		/// </summary>
		/// <param name="si"></param>
		/// <param name="sc"></param>
		protected ParallelTimeoutException(SerializationInfo si, StreamingContext sc) : base(si, sc) { }
	}

}
