

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using NetSteps.Encore.Core.Reflection;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal sealed class ContainerRegistry : ContainerOwned,  IContainerRegistry
	{
		ConcurrentDictionary<Type, ITypeRegistry> _registrations = new ConcurrentDictionary<Type, ITypeRegistry>();

		internal ContainerRegistry(IContainer container)
			: base(container)
		{
		}

		internal ContainerRegistry(IContainer container, IContainerRegistry baseRegistry)
			: base(container)
		{
			BaseRegistry = baseRegistry;
		}

		IContainerRegistry BaseRegistry { get; set; }

		public ITypeRegistry<T> ForType<T>()
		{
			return AddOrGetTypeRegistry(typeof(T), c => new TypeRegistry<T>(c));
		}

		public IGenericTypeRegistry ForGenericType(Type generic)
		{
			if (generic == null) throw new ArgumentNullException("generic");
			if (!generic.IsGenericTypeDefinition) throw new ArgumentException(String.Concat("Expected a generic type definition, received: ", generic.GetReadableFullName()), "generic");
			
			return AddOrGetTypeRegistry(generic, c => new GenericTypeRegistry(c, generic));
		}

		public bool IsTypeRegistered<T>()
		{
			return _registrations.ContainsKey(typeof(T))
				|| (BaseRegistry != null && BaseRegistry.IsTypeRegistered(typeof(T)));
		}

		public bool IsTypeRegistered(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			return _registrations.ContainsKey(type)
				|| (BaseRegistry != null && BaseRegistry.IsTypeRegistered(type));			
		}

        public bool IsTypeRegisteredWithName<T>(string name)
        {
            return IsTypeRegisteredWithName(typeof(T), name);
        }

        public bool IsTypeRegisteredWithName(Type type, string name)
        {
            ITypeRegistry reg;
            if (_registrations.TryGetValue(type, out reg))
            {
                return reg.HasNamedRegistration(name);
            }
            return (BaseRegistry != null && BaseRegistry.IsTypeRegisteredWithName(type, name));
        }

		public bool TryGetResolverForType(Type type, out IResolver value)
		{
			if (type == null) throw new ArgumentNullException("type");

			ITypeRegistry temp;
			bool gotItHere = _registrations.TryGetValue(type, out temp);
			if (gotItHere)
			{
				value = temp.UntypedResolver;
				return true;
			}
			else if (BaseRegistry != null)
			{
				return BaseRegistry.TryGetResolverForType(type, out value);
			}
			value = null;
			return false;
		}

		public bool TryGetResolverForType<T>(out IResolver<T> value)
		{
			ITypeRegistry temp;
			bool gotItHere = _registrations.TryGetValue(typeof(T), out temp);
			if (gotItHere)
			{
				value = ((ITypeRegistry<T>)temp).Resolver;
				return true;
			}
			else if (BaseRegistry != null)
			{
				return BaseRegistry.TryGetResolverForType(out value);
			}
			value = null;
			return false;
		}

		public bool TryGetNamedResolverForType<T>(string name, out IResolver<T> value)
		{
			ITypeRegistry r;
			bool gotItHere = _registrations.TryGetValue(typeof(T), out r);
			if (gotItHere)
			{
				return ((ITypeRegistry<T>)r).TryGetNamedResolver(name, out value);
			}
			else if (BaseRegistry != null)
			{
				return BaseRegistry.TryGetNamedResolverForType(name, out value);
			}
			value = null;
			return false;
		}
				
		protected override bool PerformDispose(bool disposing)
		{
			if (disposing)
			{
				foreach (var reg in _registrations.Values)
				{
					reg.Dispose();
				}
				return true;
			}
			return false;
		}

		TTypeRegistry AddOrGetTypeRegistry<TTypeRegistry>(Type type, Func<IContainer, TTypeRegistry> factory)
			where TTypeRegistry: ITypeRegistry
		{
			ITypeRegistry r = null;
			do
			{
				ITypeRegistry value;
				if (_registrations.TryGetValue(type, out value))
				{
					r = value;
				}
				else
				{
					var underlying = BaseRegistry;
					ITypeRegistryManagement mgt;
					if (underlying != null && underlying.TryGetTypeRegistryManagement(type, out mgt))
					{
						r = mgt.MakeCopyForContainer(Container);
					}
					else
					{
						r = factory(Container);
					}
					if (!_registrations.TryAdd(type, r))
					{
						Util.Dispose(ref r);
					}
				}
			}
			while (r == null);

			return (TTypeRegistry)r;
		}

		public bool TryGetTypeRegistryManagement(Type type, out ITypeRegistryManagement value)
		{
			ITypeRegistry temp;
			bool gotItHere = _registrations.TryGetValue(type, out temp);
			if (gotItHere)
			{
				value = temp as ITypeRegistryManagement;
				return value != null;
			}
			else if (BaseRegistry != null)
			{
				return BaseRegistry.TryGetTypeRegistryManagement(type, out value);
			}
			value = null;
			return false;
		}
	}
}
