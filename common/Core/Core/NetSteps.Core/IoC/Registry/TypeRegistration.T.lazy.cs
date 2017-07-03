
using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using NetSteps.Encore.Core.IoC.Constructors;


namespace NetSteps.Encore.Core.IoC.Registry
{
	internal sealed class LazyTypeRegistration<T> : TypeRegistration<T>
	{
		readonly Param[] _parameters;
		readonly Object _lock = new Object();
		readonly Func<Type, Type> _factory;
		readonly Lazy<IResolver<T>> _resolver;
		Type _concreteType;
				
		internal LazyTypeRegistration(IContainer container, Func<Type,Type> factory, Param[] parameters)		
			: base(container)
		{
			Contract.Requires<ArgumentNullException>(factory != null);

			_factory = factory;
			_resolver = new Lazy<IResolver<T>>(ConfigureResolver, LazyThreadSafetyMode.ExecutionAndPublication);
			_parameters = parameters;
		}
		
		public override Type TargetType
		{
			get
			{
				return Util.LazyInitializeWithLock<Type>(ref _concreteType, _lock, ResolveType);
			}
		}
		public override IResolver UntypedResolver { get { return _resolver.Value; } }
		public override IResolver<T> Resolver { get { return _resolver.Value; } }

		protected override IResolver<T> ConfigureResolver()
		{	
			var m = typeof(LazyTypeRegistration<T>)
				.GetMethod("StrongConfigureResolver", BindingFlags.Instance | BindingFlags.NonPublic)
				.MakeGenericMethod(TargetType);
			return (IResolver<T>)m.Invoke(this, null);
		}

		Type ResolveType()
		{
			return _factory(typeof(T));
		}

		IResolver<T> StrongConfigureResolver<C>()
			where C: class, T
		{			
			var behavior = this.ScopeBehavior;			
			ConstructorSet<T, C> ctors = new ConstructorSet<T,C>(_parameters);

			if (behavior.HasFlag(ScopeBehavior.Singleton))
				return new SingletonResolver<T, C>(Container, ctors);
			if (behavior.HasFlag(ScopeBehavior.InstancePerScope))
				return new InstancePerScopeResolver<T, C>(ctors);
			else return new Resolver<T, C>(ctors);		
		}

		protected override IResolver<T> ConstructPerRequestResolver()
		{
			throw new NotImplementedException();
		}

		protected override IResolver<T> ConstructPerScopeResolver()
		{
			throw new NotImplementedException();
		}

		protected override IResolver<T> ConstructSingletonResolver()
		{
			throw new NotImplementedException();
		}
	}
}
