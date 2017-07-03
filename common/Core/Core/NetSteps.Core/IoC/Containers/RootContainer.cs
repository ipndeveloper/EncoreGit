using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Threading;
using NetSteps.Encore.Core.IoC.Registry;

namespace NetSteps.Encore.Core.IoC.Containers
{
	internal sealed class RootContainer : Container, IRootContainer
	{
		public static readonly Guid ContainerWireupObserverKey = new Guid("FDD60331-54F3-4BA2-AD19-CBDDE73CC023");

		Lazy<ConcurrentDictionary<object, IContainer>> _tenantContainers = new Lazy<ConcurrentDictionary<object, IContainer>>(LazyThreadSafetyMode.PublicationOnly);
		TypeRegistry<ITenantResolver> _multitenant;

		internal RootContainer() : base(true)
		{
			// IContainer should always resolve to the container itself.
			
			this.ForType<IContainer>().Register((c, p) => c).DisallowSpecialization();
		}

		public bool SupportsMultipleTenants { get { return _multitenant != null; } }

		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification="Relies on CleanupScope to dispose the registry.")]
		public ITypeRegistration RegisterMultiTenant<TTenantResolver>() where TTenantResolver : class, ITenantResolver, new()
		{
			if (_multitenant != null)
				throw new ContainerException("Each root container may only register one tenant resolver.");

			_multitenant = Scope.Add(new TypeRegistry<ITenantResolver>(this));
			return _multitenant.Register<TTenantResolver>();			
		}

		[SuppressMessage("Microsoft.Reliability", "CA2000", Justification = "Relies on CleanupScope to dispose the registry.")]		
		public ITypeRegistration RegisterMultiTenant<TTenantResolver>(Func<IContainer, Param[], TTenantResolver> factory) where TTenantResolver: class, ITenantResolver
		{
			if (_multitenant != null)
				throw new ContainerException("Each root container may only register one tenant resolver.");

			_multitenant = Scope.Add(new TypeRegistry<ITenantResolver>(this));
			return _multitenant.Register<TTenantResolver>(factory);
		}

		public IContainer RegisterTenant(object tenantID)
		{
			Contract.Ensures(Contract.Result<IContainer>() != null);
			Contract.Assert(tenantID != null, "tenantID cannot be null");

			var tenants = _tenantContainers.Value;
			IContainer tenant;
			while (true)
			{
				if (tenants.TryGetValue(tenantID, out tenant))
				{
					break;
				}
				else 
				{
					var temp = MakeChildContainer(CreationContextOptions.EnableCaching, true, tenantID);
					if (tenants.TryAdd(tenantID, temp))
					{
						tenant = temp;
						break;
					}
					else
					{
						temp.Dispose();
					}
				}
			}
			return tenant;
		}

		public IContainer ResolveCurrentTenant()
		{
			if (_multitenant == null)
				throw new ContainerException("Missing tenant resolver.");

			object id;
			if (TryResolveTenant(out id))
			{
				return RegisterTenant(id).MakeChildContainer();
			}			
			throw new ContainerException("No such tenant.");
		}

		public IContainer ResolveTenantByID(object tenant)
		{
			var tenants = _tenantContainers.Value;
			
			IContainer tenantContainer;
			if (!tenants.TryGetValue(tenant, out tenantContainer))
				throw new ContainerException("No such tenant.");

			return tenantContainer.MakeChildContainer();
		}

		public bool TryResolveTenant(out object tenant)
		{
			ITenantResolver resolver;
			if (_multitenant != null
				&& _multitenant.TryResolve(this, LifespanTracking.Automatic, out resolver))
			{
				return resolver.TryResolveTenant(out tenant);
			}
			tenant = null;
			return false;
		}

		public Guid ObserverKey { get { return ContainerWireupObserverKey; } }

		public void NotifyWireupTask(Wireup.IWireupCoordinator coordinator, Wireup.Meta.WireupTaskAttribute task, Type target)
		{
			var cra = task as ContainerRegisterAttribute;
			if (cra != null && target != null)
			{				
				var method = ContainerRegisterAttribute.ApplyRegistrationForMethod.MakeGenericMethod(cra.RegistratedForType, target);
				method.Invoke(cra, new object[] { this });
			}
		}
	}
}
