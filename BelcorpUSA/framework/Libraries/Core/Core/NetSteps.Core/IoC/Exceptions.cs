using System;
using System.Runtime.Serialization;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Base container exception.
	/// </summary>
	[Serializable]
	public class ContainerException: ApplicationException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public ContainerException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public ContainerException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public ContainerException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected ContainerException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}

	/// <summary>
	/// Indicates an exception during container registration.
	/// </summary>
	[Serializable]
	public class ContainerRegistryException: ContainerException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public ContainerRegistryException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public ContainerRegistryException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public ContainerRegistryException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected ContainerRegistryException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}


	/// <summary>
	/// Indicates a required parameter was not supplied while resolving a type.
	/// </summary>
	[Serializable]
	public class MissingParameterException : ContainerException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public MissingParameterException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public MissingParameterException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public MissingParameterException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected MissingParameterException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}

}
