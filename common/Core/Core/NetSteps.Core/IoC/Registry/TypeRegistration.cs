using System;
using NetSteps.Encore.Core.IoC.Constructors;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal abstract class TypeRegistration : ContainerRegistrationParticipant, ITypeRegistration
	{		
		protected TypeRegistration(IContainer container, Type registeredType)
			: base(container)
		{
			if (registeredType == null)
				throw new ArgumentNullException("RegisteredType");
			RegisteredType = registeredType;
		}

		public Type RegisteredType { get; private set; }
		public abstract Type TargetType { get; }
		public bool IsNamed { get; protected set; }
		public bool HasEnded { get; protected set; }
		public ScopeBehavior ScopeBehavior { get; protected set; }

		public ITypeRegistration ResolveAnInstancePerRequest()
		{
			if (ScopeBehavior.HasFlag(IoC.ScopeBehavior.SpecializationDisallowed))
				throw new InvalidOperationException(String.Concat("Cannot reconfigure registration: ", ScopeBehavior));

			ScopeBehavior = IoC.ScopeBehavior.InstancePerRequest;
			return this;
		}

		public ITypeRegistration ResolveAnInstancePerScope()
		{
			if (ScopeBehavior.HasFlag(IoC.ScopeBehavior.SpecializationDisallowed))
				throw new InvalidOperationException(String.Concat("Cannot reconfigure registration: ", ScopeBehavior));

			ScopeBehavior = IoC.ScopeBehavior.InstancePerScope;
			return this;
		}

		public ITypeRegistration ResolveAsSingleton()
		{
			if (ScopeBehavior.HasFlag(IoC.ScopeBehavior.SpecializationDisallowed))
				throw new InvalidOperationException(String.Concat("Cannot reconfigure registration: ", ScopeBehavior));

			ScopeBehavior = IoC.ScopeBehavior.Singleton;
			return this;
		}

		public ITypeRegistration DisallowSpecialization()
		{
			if (!ScopeBehavior.HasFlag(IoC.ScopeBehavior.SpecializationDisallowed))
			{
				ScopeBehavior = ScopeBehavior | IoC.ScopeBehavior.SpecializationDisallowed;
			}
			return this;
		}

		protected override void OnEnded()
		{
			HasEnded = true;
		}

		protected override bool PerformDispose(bool disposing)
		{
			return disposing;
		}

		public abstract IResolver UntypedResolver { get; }

	}	
}
