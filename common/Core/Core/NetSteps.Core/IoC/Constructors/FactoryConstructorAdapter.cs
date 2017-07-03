using System;

namespace NetSteps.Encore.Core.IoC.Constructors
{
	/// <summary>
	/// Factory constructor adapter.
	/// </summary>
	/// <typeparam name="T">target type T</typeparam>
	public sealed class FactoryConstructorAdapter<T> : ConstructorAdapter<T>
	{
		Func<IContainer, T> _factory;

		internal FactoryConstructorAdapter(Func<IContainer, T> factory)
		{
			if (factory == null) throw new ArgumentNullException("factory");
			_factory = factory;
		}

		/// <summary>
		/// Executes the constructor and returns the resulting instance.
		/// </summary>
		/// <param name="container">scoping container</param>
		/// <param name="name">the registered name or null</param>
		/// <param name="parameters">parameters intended for the new instance</param>
		/// <returns>a new instance</returns>
		public override T Execute(IContainer container, string name, params object[] parameters)
		{
			T instance = _factory(container);
			return instance;
		}
	}

	/// <summary>
	/// Factory constructor adapter.
	/// </summary>
	/// <typeparam name="T">target type T</typeparam>
	/// <typeparam name="C">concrete type C</typeparam>
	internal sealed class FactoryConstructorAdapter<T, C> : ConstructorAdapter<T>
		where C: T
	{
		Func<IContainer, C> _factory;

		internal FactoryConstructorAdapter(Func<IContainer, C> factory)
		{
			if (factory == null) throw new ArgumentNullException("factory");
			_factory = factory;
		}

		/// <summary>
		/// Executes the constructor and returns the resulting instance.
		/// </summary>
		/// <param name="container">scoping container</param>
		/// <param name="name">the registered name or null</param>
		/// <param name="parameters">parameters intended for the new instance</param>
		/// <returns>a new instance</returns>
		public override T Execute(IContainer container, string name, params object[] parameters)
		{
			T instance = _factory(container);
			return instance;
		}
	}
}
