using System;

namespace NetSteps.Encore.Core.IoC
{	
	/// <summary>
	/// Type registry for type T
	/// </summary>
	/// <typeparam name="T">type T</typeparam>
	public interface ITypeRegistry<T> : ITypeRegistry
	{
		/// <summary>
		/// Registers implementation type C for use when resolving instances of type T.
		/// </summary>
		/// <typeparam name="C">sublcass type C</typeparam>
		/// <returns>the registration for chaining calls</returns>
		ITypeRegistration Register<C>() where C : T;
		
		/// <summary>
		/// Registers implementation type C for use when resolving instances of type T.
		/// </summary>
		/// <typeparam name="C">sublcass type C</typeparam>
		/// <param name="parameters">one or more Params to be used when resolving instances of type T</param>
		/// <returns>the registration for chaining calls</returns>		
		ITypeRegistration Register<C>(params Param[] parameters) where C : T;

		/// <summary>
		/// Registers a factory for use when resolving instances of type T.
		/// </summary>
		/// <typeparam name="C">implementation type C</typeparam>
		/// <param name="factory">a factory providing instances of type C</param>
		/// <returns>the registration for chaining calls</returns>		
		ITypeRegistration Register<C>(Func<IContainer, Param[], C> factory) where C : T;
		
		/// <summary>
		/// Registers implementation type C as a named registration for use when resolving instances of type T by name.
		/// </summary>
		/// <typeparam name="C">implementation type C</typeparam>
		/// <param name="name">the name</param>
		/// <returns>the registration for chaining calls</returns>
		ITypeRegistration RegisterWithName<C>(string name) where C : T;

		/// <summary>
		/// Registers implementation type C as a named registration for use when resolving instances of type T by name.
		/// </summary>
		/// <typeparam name="C">implementation type C</typeparam>
		/// <param name="name">the name</param>
		/// <returns>the registration for chaining calls</returns>
		/// <param name="parameters">one or more Params to be used when resolving instances of type T</param>
		/// <returns>the registration for chaining calls</returns>
		ITypeRegistration RegisterWithName<C>(string name, params Param[] parameters) where C : T;

		/// <summary>
		/// Registers a named factory registration for use when resolving instances of type T by name.
		/// </summary>
		/// <typeparam name="C">implementation type C</typeparam>
		/// <param name="name">the name</param>
		/// <param name="factory">a factory providing instances of type C</param>
		/// <returns>the registration for chaining calls</returns>		
		ITypeRegistration RegisterWithName<C>(string name, Func<IContainer, Param[], C> factory) where C : T;
		
		/// <summary>
		/// Registers a function that will provide an implementation of type T upon demand.
		/// </summary>
		/// <param name="producer">a callback function that will produce the 
		/// implementation type upon demand</param>
		/// <returns>the resulting type registration</returns>
		ITypeRegistration LazyRegister(Func<Type, Type> producer);

		/// <summary>
		/// Gets the type resolver for type T. Intended for framework use; you should never need this.
		/// </summary>
		IResolver<T> Resolver { get; }
        
		/// <summary>
		/// Gets the named type resolver for type T. Intended for framework use; you should never need this.
		/// </summary>
		/// <param name="name">the type's name</param>
		/// <param name="value">variable that will hold the resolver upon success</param>
		/// <returns>true if successful; otherwise false.</returns>
		bool TryGetNamedResolver(string name, out IResolver<T> value);
	}
	
}
