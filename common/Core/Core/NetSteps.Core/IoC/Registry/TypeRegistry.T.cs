using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using NetSteps.Encore.Core.Reflection;
using System.Reflection;
using System.Diagnostics.Contracts;

using System.Diagnostics.CodeAnalysis;

namespace NetSteps.Encore.Core.IoC.Registry
{

	internal sealed class TypeRegistry<T> : ContainerOwned, ITypeRegistry<T>, ITypeRegistryManagement
	{
		readonly Lazy<ConcurrentDictionary<string, INamedTypeRegistration<T>>> _named = new Lazy<ConcurrentDictionary<string, INamedTypeRegistration<T>>>(LazyThreadSafetyMode.ExecutionAndPublication);
		ITypeRegistration<T> _current;
		Lazy<MethodInfo> _genericMake = new Lazy<MethodInfo>(() => typeof(TypeRegistry<T>).GetGenericMethod("MakeConcreteRegistrationFor", BindingFlags.NonPublic | BindingFlags.Instance, 1, 1), LazyThreadSafetyMode.PublicationOnly);
		Lazy<MethodInfo> _genericMakeNamed = new Lazy<MethodInfo>(() => typeof(TypeRegistry<T>).GetGenericMethod("MakeNamedConcreteRegistrationFor", BindingFlags.NonPublic | BindingFlags.Instance, 1, 1), LazyThreadSafetyMode.PublicationOnly);

		public TypeRegistry(IContainer container)
			: this(container, null, null)
		{
		}

		public TypeRegistry(IContainer container, ITypeRegistration<T> current, IEnumerable<INamedTypeRegistration<T>> named)
			: base(container)
		{
			_current = current;
			if (named != null)
			{
				foreach (var n in named)
				{
					_named.Value.TryAdd(n.Name, n);
				}
			}
		}

		public Type RegisteredType { get { return typeof(T); } }

		public bool CanSpecializeRegistration
		{
			get
			{
				var r = Current;
				return r == null
					|| (!r.ScopeBehavior.HasFlag(ScopeBehavior.SpecializationDisallowed));
			}
		}

		public bool HasNamedRegistration(string name)
		{
			return _named.IsValueCreated && _named.Value.ContainsKey(name);
		}

		public ITypeRegistration Register(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (!typeof(T).IsAssignableFrom(type)) throw new ArgumentNullException(String.Concat("Expected a type assignable to: ", RegisteredType.GetReadableFullName()), "type");

			var current = Current;
			CheckCanSpecializeRegistration(current);
			ITypeRegistration<T> reg = (ITypeRegistration<T>)_genericMake.Value.MakeGenericMethod(type).Invoke(this, new object[] { null });
			CheckedSetRegistration(reg, current);
			return reg;
		}

		public ITypeRegistration Register(Type type, params Param[] parameters)
		{
			if (type == null) throw new ArgumentNullException("typ");
			if (!typeof(T).IsAssignableFrom(type)) throw new ArgumentNullException(String.Concat("Expected a type assignable to: ", RegisteredType.GetReadableFullName()), "type");

			var current = Current;
			CheckCanSpecializeRegistration(current);
			ITypeRegistration<T> result = (ITypeRegistration<T>)_genericMake.Value.MakeGenericMethod(type).Invoke(this, new object[] { Container, parameters });
			CheckedSetRegistration(result, current);
			return result;
		}

		public ITypeRegistration Register<C>() where C : T
		{
			var current = Current;
			CheckCanSpecializeRegistration(current);
			var result = MakeConcreteRegistrationFor<C>(null);
			CheckedSetRegistration(result, current);
			return result;
		}

		public ITypeRegistration Register<C>(params Param[] parameters) where C : T
		{
			var current = Current;
			CheckCanSpecializeRegistration(current);
			var result = MakeConcreteRegistrationFor<C>(parameters);
			CheckedSetRegistration(result, current);
			return result;
		}

		public ITypeRegistration RegisterWithName<C>(string name) where C : T
		{
			return RegisterWithName<C>(name, Param.EmptyParams);
		}


		public ITypeRegistration RegisterWithName<C>(string name, params Param[] parameters) where C : T
		{
			Contract.Assert(name != null);
			Contract.Assert(name.Length > 0);

			var result = MakeNamedConcreteRegistrationFor<C>(name, parameters);

			var registry = _named.Value;
			if (!registry.TryAdd(name, result))
			{
				INamedTypeRegistration<T> current;
				while (registry.TryGetValue(name, out current))
				{
					if (registry.TryUpdate(name, result, current))
					{
						break;
					}
				}
			}
			return result;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "Relies on CheckedSetRegistration and our own Dispose to dispose.")]
		public ITypeRegistration Register<C>(Func<IContainer, Param[], C> factory) where C : T
		{
			var current = Current;
			CheckCanSpecializeRegistration(current);

			var reg = new FactoryTypeRegistration<T, C>(Container, factory);
			CheckedSetRegistration(reg, current);
			return reg;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "Relies on CheckedSetRegistration and our own Dispose to dispose.")]
		public ITypeRegistration RegisterWithName<C>(string name, Func<IContainer, Param[], C> factory) where C : T
		{
			Contract.Assert(name != null);
			Contract.Assert(name.Length > 0);

			var result = new NamedFactoryTypeRegistration<T, C>(Container, name, factory);

			var registry = _named.Value;
			if (!registry.TryAdd(name, result))
			{
				INamedTypeRegistration<T> current;
				while (registry.TryGetValue(name, out current))
				{
					if (registry.TryUpdate(name, result, current))
					{
						break;
					}
				}
			}
			return result;
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "Relies on CheckedSetRegistration and our own Dispose to dispose.")]
		public ITypeRegistration LazyRegister(Func<Type, Type> factory)
		{
			Contract.Assert(factory != null);

			var current = Current;
			CheckCanSpecializeRegistration(current);

			var reg = new LazyTypeRegistration<T>(Container, factory, null);
			CheckedSetRegistration(reg, current);
			return reg;
		}

		public bool TryResolve(IContainer container, LifespanTracking tracking, out T instance)
		{
			Contract.Assert(container != null);

			var resolver = (IResolver<T>)UntypedResolver;
			if (resolver != null)
			{
				return resolver.TryResolve(container, tracking, null, out instance);
			}
			instance = default(T);
			return false;
		}

		public bool TryResolve(IContainer container, LifespanTracking tracking, out T instance, params Param[] parameters)
		{
			Contract.Assert(container != null);

			var resolver = (IResolver<T>)UntypedResolver;
			if (resolver != null)
			{
				return resolver.TryResolve(container, tracking, null, out instance, parameters);
			}
			instance = default(T);
			return false;
		}

		public bool TryResolveNamed(IContainer container, string name, LifespanTracking tracking, out T instance)
		{
			Contract.Assert(container != null);
			Contract.Assert(name != null);
			Contract.Assert(name.Length > 0);

			INamedTypeRegistration<T> current;
			if (_named.IsValueCreated
				&& _named.Value.TryGetValue(name, out current))
			{
				return current.Resolver.TryResolve(container, tracking, name, out instance);
			}
			instance = default(T);
			return false;
		}

		public bool TryResolveNamed(IContainer container, string name, LifespanTracking tracking, out T instance, Param[] parameters)
		{
			Contract.Assert(container != null);
			Contract.Assert(name != null);
			Contract.Assert(name.Length > 0);

			INamedTypeRegistration<T> current;
			if (_named.IsValueCreated
				&& _named.Value.TryGetValue(name, out current))
			{
				return current.Resolver.TryResolve(container, tracking, name, out instance);
			}
			instance = default(T);
			return false;
		}

		public ITypeRegistry MakeCopyForContainer(IContainer container)
		{
			Contract.Assert(container != null);

			if (_named.IsValueCreated)
			{
				return new TypeRegistry<T>(container, Current, _named.Value.Values);
			}
			else
			{
				return new TypeRegistry<T>(container, Current, null);
			}
		}

		public IResolver<T> Resolver
		{
			get
			{
				Thread.MemoryBarrier();
				var r = _current;
				Thread.MemoryBarrier();
				return (r != null) ? r.Resolver : null;
			}
		}

		public bool TryGetNamedResolver(string name, out IResolver<T> value)
		{
			Contract.Assert(name != null);
			Contract.Assert(name.Length > 0);

			INamedTypeRegistration<T> current;
			if (_named.IsValueCreated
				&& _named.Value.TryGetValue(name, out current))
			{
				value = current.Resolver;
				return true;
			}
			value = default(IResolver<T>);
			return false;
		}

		public IResolver UntypedResolver { get { return Resolver; } }

		ITypeRegistration<T> Current
		{
			get
			{
				Thread.MemoryBarrier();
				var r = _current;
				Thread.MemoryBarrier();
				return r;
			}
		}

		ITypeRegistration<T> MakeConcreteRegistrationFor<C>(Param[] parameters) where C : T
		{
			ITypeRegistration<T> result;
			if (typeof(C).IsInterface || typeof(C).IsAbstract)
			{
				// Abstract types resolved via the current container...
				result = new FactoryTypeRegistration<T, C>(Container, (c, p) => c.New<C>());
			}
			else if (typeof(C).IsClass)
			{
				result = (ITypeRegistration<T>)Activator.CreateInstance(typeof(TypeRegistration<,>).MakeGenericType(typeof(T), typeof(C)), Container, parameters);
			}
			else
			{
				throw new ArgumentException(String.Concat("Generic argument type C should be an interface or a class: typeof(", typeof(C).GetReadableFullName(), ") not supported."));
			}
			return result;
		}

		INamedTypeRegistration<T> MakeNamedConcreteRegistrationFor<C>(string name, Param[] parameters) where C : T
		{
			INamedTypeRegistration<T> result;
			if (typeof(C).IsInterface || typeof(C).IsAbstract)
			{
				// Abstract types resolved via the current container...
				result = new NamedFactoryTypeRegistration<T, C>(Container, name, (c, p) => c.New<C>());
			}
			else if (typeof(C).IsClass)
			{
				result = (INamedTypeRegistration<T>)Activator.CreateInstance(typeof(NamedTypeRegistration<,>).MakeGenericType(typeof(T), typeof(C)), Container, name, parameters);
			}
			else
			{
				throw new ArgumentException(String.Concat("Generic argument type C should be an interface or a class: typeof(", typeof(C).GetReadableFullName(), ") not supported."));
			}
			return result;
		}

		void CheckCanSpecializeRegistration(ITypeRegistration current)
		{
			if (current != null)
			{
				if (current.ScopeBehavior.HasFlag(ScopeBehavior.SpecializationDisallowed))
					throw new ContainerRegistryException(String.Concat("The type is registered in an outer scope and its registration cannot be specialized: ", RegisteredType.GetReadableFullName()));
			}
		}

		void CheckedSetRegistration(ITypeRegistration<T> reg, ITypeRegistration<T> current)
		{
			if (Interlocked.CompareExchange(ref _current, reg, current) != current)
			{
				Util.Dispose(ref current);
				throw new ContainerRegistryException("Victimized by concurrent registration.");
			}
		}
		protected override bool PerformDispose(bool disposing)
		{
			Thread.MemoryBarrier();
			var current = _current;
			Thread.MemoryBarrier();
			if (current != null)
			{
				current.Dispose();
				Thread.MemoryBarrier();
				_current = null;
				Thread.MemoryBarrier();
			}
			return disposing;
		}
	}
}
