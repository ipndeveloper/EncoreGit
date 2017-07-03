using System;
using System.Runtime.Serialization;

namespace NetSteps.Models.Core
{
	/// <summary>
	/// Base model exception.
	/// </summary>
	[Serializable]
	public class ModelException : ApplicationException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public ModelException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public ModelException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public ModelException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected ModelException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}

	/// <summary>
	/// Base model exception.
	/// </summary>
	[Serializable]
	public class UnresolvableModelException : ModelException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public UnresolvableModelException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public UnresolvableModelException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public UnresolvableModelException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected UnresolvableModelException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}

	/// <summary>
	/// Exception indicating a reference or dereference was attempted against
	/// an undefined referent.
	/// </summary>
	[Serializable]
	public class UndefinedReferentException : ModelException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public UndefinedReferentException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public UndefinedReferentException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public UndefinedReferentException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected UndefinedReferentException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}

	/// <summary>
	/// Exception an attempt was made to modify an immutable instance.
	/// </summary>
	[Serializable]
	public class ImmutableModelException : ModelException
	{
		/// <summary>
		/// Default constructor; creates a new instance.
		/// </summary>
		public ImmutableModelException()
			: base()
		{
		}

		/// <summary>
		/// Creates a new instance using the error message given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		public ImmutableModelException(string errorMessage)
			: base(errorMessage)
		{
		}

		/// <summary>
		/// Creates a new instance using the error message and cuase given.
		/// </summary>
		/// <param name="errorMessage">An error message describing the exception.</param>
		/// <param name="cause">An inner exception that caused this exception</param>
		public ImmutableModelException(string errorMessage, Exception cause)
			: base(errorMessage, cause)
		{
		}

		/// <summary>
		/// Used during serialization.
		/// </summary>
		/// <param name="si">SerializationInfo</param>
		/// <param name="sc">StreamingContext</param>
		protected ImmutableModelException(SerializationInfo si, StreamingContext sc)
			: base(si, sc)
		{
		}
	}
		
}
