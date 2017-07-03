using System;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.Representation
{	
	/// <summary>
	/// Delegated JSON representation transform.
	/// </summary>
	/// <typeparam name="T">delegated target type T</typeparam>
	/// <typeparam name="C">target type C</typeparam>
	public class DelegatedJsonRepresentation<T, C> : DelegatedRepresentation<T, C, string>, IJsonRepresentation<T>
		where C : class, T
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="strict">Inidcates whether the JSON transorm should be strict</param>
		public DelegatedJsonRepresentation(bool strict)
			: base((strict) ? (IRepresentation<C, string>)new JsonTransformStrict<C>() : (IRepresentation<C, string>)new JsonTransformLoose<C>())
		{ 
		}
	}

	/// <summary>
	/// Delegated loose JSON representation transform.
	/// </summary>
	/// <typeparam name="T">delegated target type T</typeparam>
	/// <typeparam name="C">target type C</typeparam>
	public class DelegatedJsonRepresentationLoose<T, C> : DelegatedJsonRepresentation<T, C>
		where C : class, T
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DelegatedJsonRepresentationLoose()
			: base(false)
		{
		}
	}
	/// <summary>
	/// Delegated  strict JSON representation transform.
	/// </summary>
	/// <typeparam name="T">delegated target type T</typeparam>
	/// <typeparam name="C">target type C</typeparam>
	public class DelegatedJsonRepresentationStrict<T, C> : DelegatedJsonRepresentation<T, C>
		where C : class, T
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DelegatedJsonRepresentationStrict()
			: base(true)
		{
		}
	}
}
