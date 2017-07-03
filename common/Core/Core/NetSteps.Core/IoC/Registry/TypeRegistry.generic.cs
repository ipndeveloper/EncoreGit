using System;
using System.Linq;
using NetSteps.Encore.Core.Reflection;
using System.Threading;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace NetSteps.Encore.Core.IoC.Registry
{
	class GenericTypeRegistry : ContainerOwned, IGenericTypeRegistry, ITypeRegistryManagement
	{
		readonly Lazy<ConcurrentDictionary<string, INamedTypeRegistration>> _named = new Lazy<ConcurrentDictionary<string, INamedTypeRegistration>>(LazyThreadSafetyMode.ExecutionAndPublication);
		readonly ConcurrentDictionary<Type, IResolver> _resolvers = new ConcurrentDictionary<Type, IResolver>();
		IGenericTypeRegistration _current;

		public GenericTypeRegistry(IContainer container, Type generic)
			: this(container, generic, null, null)
		{
		
		}
		GenericTypeRegistry(IContainer container, Type generic, IGenericTypeRegistration current)
			: this(container, generic, current, null)
		{ 
		}
		GenericTypeRegistry(IContainer container, Type generic, IGenericTypeRegistration current, IEnumerable<INamedTypeRegistration> named)
			: base(container)
		{
			this.RegisteredType = generic;
			_current = current;
			if (named != null)
			{
				foreach (var n in named)
				{
					_named.Value.TryAdd(n.Name, n);
				}
			}
		}

		public Type RegisteredType { get; private set; }
		public bool CanSpecializeRegistration { get; private set; }

        public bool HasNamedRegistration(string name)
        {
            return _named.IsValueCreated && _named.Value.ContainsKey(name);
        }

		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "Relies on CheckedSetRegistration and our own Dispose to dispose.")]
		public ITypeRegistration Register(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			// type must be a closed constructed type, based on the RegisteredType generic type definition.
			//if (type.ContainsGenericParameters)
			//  throw new ArgumentException(String.Concat("Invalid type; must be a closed constructed type (no unassigned generic parameters): ", type.GetReadableFullName()));
			if (!type.IsTypeofGenericTypeDefinition(RegisteredType))
				throw new ArgumentException(String.Concat("Invalid type; must be derived from: ", RegisteredType.GetReadableFullName()));
			
			IGenericTypeRegistration r;
			Thread.MemoryBarrier();
			var current = _current;
			Thread.MemoryBarrier();
			if (type.IsGenericTypeDefinition)
			{
				r = new GenericTypeRegistration(Container, RegisteredType, type);
			}
			else
			{
				r = new GenericTypeRegistration(Container, RegisteredType, type);
			}
			CheckedSetRegistration(r, current);
			return r;
		}

		ITypeRegistration Current
		{
			get
			{
				Thread.MemoryBarrier();
				var result = _current;
				Thread.MemoryBarrier();
				return result;				
			}
		}

		public IResolver UntypedResolver
		{
			get { throw new NotImplementedException(); }
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

		void CheckedSetRegistration(IGenericTypeRegistration reg, IGenericTypeRegistration current)
		{
			if (Interlocked.CompareExchange(ref _current, reg, current) != current)
			{
				Util.Dispose(ref current);
				throw new ContainerRegistryException("Victimized by concurrent registration.");
			}
		}

		public IResolver<T> ResolverFor<T>()
		{
			Type t = typeof(T);
			IResolver result;
			while (!_resolvers.TryGetValue(t, out result))
			{
				Thread.MemoryBarrier();
				var current = _current;
				Thread.MemoryBarrier();
				result = current.ResolverFor<T>();
				if (_resolvers.TryAdd(t, result))
					break;
				
				Util.Dispose(ref result);
			}
			return (IResolver<T>)result;
		}

		public ITypeRegistry MakeCopyForContainer(IContainer container)
		{
			Thread.MemoryBarrier();
			var current = _current;
			Thread.MemoryBarrier();
			
			if (_named.IsValueCreated)
			{
				return new GenericTypeRegistry(container, RegisteredType, current, _named.Value.Values);
			}
			else
			{
				return new GenericTypeRegistry(container, RegisteredType, current);
			}
		}
	}
}
