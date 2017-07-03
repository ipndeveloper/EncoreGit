using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading;
using NetSteps.Encore.Core.IoC.Registry;

using NetSteps.Encore.Core.Reflection;
using NetSteps.Encore.Core.Stereotype;
using NetSteps.Encore.Core.Properties;

namespace NetSteps.Encore.Core.IoC.Containers
{
	internal partial class Container : Disposable, IContainer
	{
		static readonly MethodInfo CachedTryAutomaticRegisterType = typeof(Container).GetGenericMethod("TryAutomaticRegisterType", BindingFlags.NonPublic | BindingFlags.Instance, 0, 1);
		readonly IContainer _parent;
		readonly CreationContextOptions _options;
		int _disposers = 1;

		protected Container(bool isRoot)
		{
			IsRoot = isRoot;
			Key = Guid.NewGuid();
			Scope = new CleanupScope(true);
			Registry = new ContainerRegistry(this, null);
			IsTenant = false;
			TenantID = null;
			NetSteps.Encore.Core.IoC.Container.PushCurrent(this);
		}
		protected Container(IContainer parent, CreationContextOptions options)
		{
			_parent = parent;
			_options = options;
			Key = Guid.NewGuid();
			Scope = (options.HasFlag(CreationContextOptions.InheritScope)) ? parent.Scope : new CleanupScope(true);
			Registry = new ContainerRegistry(this, parent.Registry);
			IsTenant = parent.IsTenant;
			TenantID = parent.TenantID;
			NetSteps.Encore.Core.IoC.Container.PushCurrent(this);
		}
		protected Container(IContainer parent, CreationContextOptions options, bool isTenant, object tenantID)
		{
			_parent = parent;
			_options = options;
			Key = Guid.NewGuid();
			Scope = (options.HasFlag(CreationContextOptions.InheritScope)) ? parent.Scope : new CleanupScope(true);
			Registry = new ContainerRegistry(this, parent.Registry);
			IsTenant = isTenant;
			TenantID = tenantID;
			NetSteps.Encore.Core.IoC.Container.PushCurrent(this);
		}

		public Guid Key { get; private set; }
		public IContainerRegistry Registry { get; private set; }
		public bool IsRoot { get; private set; }
		public bool IsTenant { get; private set; }
		public object TenantID { get; private set; }

		public object NewUntyped(LifespanTracking tracking, Type targetType)
		{
			Contract.Assert(targetType != null);

			IResolver r;
			if (Registry.TryGetResolverForType(targetType, out r))
			{
				object instance;
				if (r.TargetType == targetType)
				{
					if (r.TryUntypedResolve(this, tracking, null, out instance))
						return instance;
				}
			}
			else if (targetType.IsClass)
			{
				if (LateDynamicTryAutomaticRegisterType(targetType))
					return NewUntyped(tracking, targetType);
			}
			throw new ContainerException(String.Concat("Cannot resolve type: ", targetType.GetReadableFullName()));		
		}

		public T New<T>(LifespanTracking tracking)
		{
			T instance;
			if (TryResolveWithoutRecursion(tracking, out instance))
			{
				return instance;
			}
			// Fallback #1: if it is a class, see if we can construct it...
			if (typeof(T).IsClass)
			{
				if (TryAutomaticRegisterType<T>()
					&& TryResolveWithoutRecursion<T>(tracking, out instance))
				{
					return instance;
				}
			}
			// Fallback #2: if it has a stereotype, see if we can register from stereotypical behavior...
			if (IsStereotype<T>())
			{
				if (TryAutoRegisterFromStereotype<T>()
					&& TryResolveWithoutRecursion<T>(tracking, out instance))
				{
					return instance;
				}
			}
			// Fallback #3: if it is a generic type, see if we can resolve the generic...			
			if (typeof(T).IsGenericType)
			{
				return ResolveGeneric<T>(tracking);
			}

			throw new ContainerException(String.Concat("Cannot resolve type: ", typeof(T).GetReadableFullName()));
		}

		bool TryAutoRegisterFromStereotype<T>()
		{
			lock (typeof(T).GetLockForType())
			{
				foreach (StereotypeAttribute attr in typeof(T).GetCustomAttributes(typeof(StereotypeAttribute), false))
				{
					if (attr.Behaviors == StereotypeBehaviors.AutoImplementedBehavior
						&& attr.RegisterStereotypeImplementation<T>(this))
					{
						return true;
					}
				}
			}
			return false;
		}

		bool IsStereotype<T>()
		{
			return typeof(T).IsDefined(typeof(StereotypeAttribute), true);
		}

		bool TryResolveWithoutRecursion<T>(LifespanTracking tracking, out T instance)
		{
			IResolver<T> r;
			if (Registry.TryGetResolverForType(out r))
			{
				return r.TryResolve(this, tracking, null, out instance);
			}
			instance = default(T);
			return false;
		}


		T ResolveGeneric<T>(LifespanTracking tracking)
		{
			var r = this.Registry;
			var t = typeof(T);

			var generic = t.GetGenericTypeDefinition();
			if (r.IsTypeRegistered(generic))
				return ResolveGenericAsRegistered<T>(generic, tracking);

			if (t.IsInterface)
			{
				foreach (var intf in t.GetInterfaces().Where(i => i.IsGenericType).Reverse())
				{
					generic = intf.GetGenericTypeDefinition();
					if (r.IsTypeRegistered(generic))
						return ResolveGenericAsRegistered<T>(generic, tracking);
				}
			}

			while (t != null && t.IsGenericType)
			{
				generic = t.GetGenericTypeDefinition();
				if (r.IsTypeRegistered(generic))
					return ResolveGenericAsRegistered<T>(generic, tracking);

				foreach (var intf in t.GetInterfaces().Where(i => i.IsGenericType).Reverse())
				{
					generic = intf.GetGenericTypeDefinition();
					if (r.IsTypeRegistered(generic))
						return ResolveGenericAsRegistered<T>(generic, tracking);
				}

				t = t.BaseType;
			}

			throw new ContainerException(String.Concat("Cannot resolve generic type: ", typeof(T).GetReadableFullName()));
		}

		T ResolveGenericAsRegistered<T>(Type generic, LifespanTracking tracking)
		{
			IResolver<T> r = Registry.ForGenericType(generic).ResolverFor<T>();
			T instance;
			if (r.TryResolve(this, tracking, null, out instance))
				return instance;

			throw new ContainerException(String.Concat("Cannot resolve generic type: ", typeof(T).GetReadableFullName()));
		}

		public T NewWithParams<T>(LifespanTracking tracking, params Param[] parameters)
		{
			IResolver<T> registration;
			if (Registry.TryGetResolverForType(out registration))
			{
				T instance;
				if (registration.TryResolve(this, tracking, null, out instance, parameters))
					return instance;
			}
			else
			{
				if (TryAutomaticRegisterType<T>())
					return NewWithParams<T>(tracking, parameters);
			}
			throw new ContainerException(String.Concat("Cannot resolve type: ", typeof(T).GetReadableFullName()));
		}

		public T NewNamed<T>(LifespanTracking tracking, string name)
		{
			Contract.Assert(name != null);
			Contract.Assert(name.Length > 0);

			IResolver<T> r;
			if (Registry.TryGetNamedResolverForType(name, out r))
			{
				T instance;
				if (r.TryResolve(this, tracking, null, out instance))
					return instance;
			}
			throw new ContainerException(String.Concat("Cannot resolve type ", typeof(T).GetReadableFullName(), " with name: ", name));
		}

		public T NewNamedWithParams<T>(LifespanTracking tracking, string name, params Param[] parameters)
		{
			Contract.Assert(name != null);
			Contract.Assert(name.Length > 0);

			IResolver<T> r;
			if (Registry.TryGetNamedResolverForType(name, out r))
			{
				T instance;
				if (r.TryResolve(this, tracking, null, out instance, parameters))
					return instance;
			}
			throw new ContainerException(String.Concat("Cannot resolve type ", typeof(T).GetReadableFullName(), " with name: ", name));
		}

		public T NewImplementationOf<T>(LifespanTracking tracking, Type subtype)
		{
			Contract.Assert(subtype != null);
			Contract.Assert(typeof(T).IsAssignableFrom(subtype), Resources.Chk_TypeMustBeAssignableToTypeofT);

			IResolver r;
			if (Registry.TryGetResolverForType(subtype, out r))
			{
				object instance;
				if (r.TargetType == subtype)
				{
					if (r.TryUntypedResolve(this, tracking, null, out instance))
						return (T)instance;
				}
			}
			else if (subtype.IsClass)
			{
				if (LateDynamicTryAutomaticRegisterType(subtype))
					return NewImplementationOf<T>(tracking, subtype);
			}
			throw new ContainerException(String.Concat("Cannot resolve type: ", typeof(T).GetReadableFullName()));
		}

		public IContainer MakeChildContainer(CreationContextOptions options)
		{
			return new Container(this, options);
		}
		protected IContainer MakeChildContainer(CreationContextOptions options, bool isTenant, object tenantID)
		{
			return new Container(this, options, isTenant, tenantID);
		}

		public ICleanupScope Scope { get; private set; }

		bool LateDynamicTryAutomaticRegisterType(Type type)
		{
			var dyn = CachedTryAutomaticRegisterType.MakeGenericMethod(type);
			return (bool)dyn.Invoke(this, null);
		}

		bool TryAutomaticRegisterType<T>()
		{
			if (typeof(T).IsAbstract) return false;

			Registry
				.ForType<T>()
				.DynamicRegister<T>()
				.End();
			return true;
		}

		public IContainer ShareContainer()
		{
			Interlocked.Increment(ref _disposers);
			return this;
		}

		protected override bool PerformDispose(bool disposing)
		{
			if (disposing && Interlocked.Decrement(ref _disposers) > 0)
			{
				return false;
			}
			else
			{
				NetSteps.Encore.Core.IoC.Container.PopCurrentIfEquals(this);
				Scope.Dispose();
				return true;
			}
		}
	}
}
