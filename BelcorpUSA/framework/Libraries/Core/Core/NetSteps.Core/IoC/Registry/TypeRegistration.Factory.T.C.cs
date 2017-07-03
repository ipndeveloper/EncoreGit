using System;
using System.Threading;
using System.Diagnostics.Contracts;


namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class FactoryTypeRegistration<T, C> : TypeRegistration<T>
		where C : T
	{
		readonly Func<IContainer, Param[], C> _factory;
		readonly Lazy<IResolver<T>> _resolver;
		
		internal FactoryTypeRegistration(IContainer container, Func<IContainer, Param[], C> factory)
			: base(container)
		{
			Contract.Requires<ArgumentNullException>(factory != null);

			_factory = factory;
			_resolver = new Lazy<IResolver<T>>(ConfigureResolver, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		public override IResolver UntypedResolver { get { return _resolver.Value; } }
		public override IResolver<T> Resolver { get { return _resolver.Value; } }
		protected override IResolver<T> ConstructPerRequestResolver()
		{
			return new FactoryResolver<T, C>(_factory);			
		}
		protected override IResolver<T> ConstructPerScopeResolver()
		{
			return new FactoryInstancePerScopeResolver<T, C>(_factory);
		}
		protected override IResolver<T> ConstructSingletonResolver()
		{
			return new FactorySingletonResolver<T, C>(this.Container, _factory);
		}
	}

	internal sealed class NamedFactoryTypeRegistration<T, C> : FactoryTypeRegistration<T, C>, INamedTypeRegistration<T>
		where C : T
	{
		internal NamedFactoryTypeRegistration(IContainer container, string name, Func<IContainer, Param[], C> factory)
			: base(container, factory)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);

			base.IsNamed = true;
			this.Name = name;
		}

		public string Name { get; private set; }
	}
}
