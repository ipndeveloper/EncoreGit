using System;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC.Constructors;
using System.Threading;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class TypeRegistration<T, C> : TypeRegistration<T>
		where C : class, T
	{
		readonly ConstructorSet<T, C> _constructors;
		readonly Lazy<IResolver<T>> _resolver;

		public TypeRegistration(IContainer container)
			: this(container, null)
		{
		}
		public TypeRegistration(IContainer container, Param[] parameters)
			: base(container)
		{
			_constructors = new ConstructorSet<T, C>(parameters);
			_resolver = new Lazy<IResolver<T>>(ConfigureResolver, LazyThreadSafetyMode.ExecutionAndPublication);
		}

		public override IResolver UntypedResolver { get { return _resolver.Value; } }
		public override IResolver<T> Resolver { get { return _resolver.Value; } }
		
		protected override IResolver<T> ConstructPerRequestResolver()
		{
			return new Resolver<T, C>(_constructors);			
		}

		protected override IResolver<T> ConstructPerScopeResolver()
		{
			return new InstancePerScopeResolver<T, C>(_constructors);
		}

		protected override IResolver<T> ConstructSingletonResolver()
		{
			return new SingletonResolver<T, C>(Container, _constructors);
		}
	}
}
