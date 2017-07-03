using System;
using System.Runtime.Serialization;

namespace NetSteps.Encore.Core.Collections
{
	/// <summary>
	/// Base registration exception.
	/// </summary>
	[Serializable]
	public class RegistrationException : ApplicationException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public RegistrationException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public RegistrationException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public RegistrationException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected RegistrationException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}
}
