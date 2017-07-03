using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.Reflection;
using System.Reflection;
using NetSteps.Encore.Core.Wireup.Meta;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Indicates registration behavior.
	/// </summary>
	[Flags]
	public enum RegistrationBehaviors
	{
		/// <summary>
		/// None; empty.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates the target is the default implementation.
		/// </summary>
		Default = 1,
		/// <summary>
		/// Indicates the target is a named implementation.
		/// </summary>
		Named = 2,
		/// <summary>
		/// Indicates the target is intended to override
		/// the default implementation.
		/// </summary>
		OverrideDefault = 4,
		/// <summary>
		/// Indicates the target is intended to override
		/// a named implementation.
		/// </summary>
		OverrideNamed = 8,
		/// <summary>
		/// Indicates the target is to be the registered
		/// instance if no other registrations exist.
		/// </summary>
		IfNoneOther = 16,
		/// <summary>
		/// Indicates the target is either the default instance
		/// or if a default exists; target overrides the default.
		/// </summary>
		DefaultOrOverrideDefault = Default | OverrideDefault
	}

	/// <summary>
	/// Indicates that the target should be registered with the
	/// IoC container upon assembly wireup.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=true)]
	public class ContainerRegisterAttribute : WireupTaskAttribute
	{
		internal static MethodInfo ApplyRegistrationForMethod = typeof(ContainerRegisterAttribute).GetGenericMethod("ApplyRegistrationFor", BindingFlags.NonPublic | BindingFlags.Instance, 1, 2);

		/// <summary>
		/// Creates a new instance; used the implemented type can be inferred from a single
		/// superclass or interface implementation.
		/// </summary>
		public ContainerRegisterAttribute() : base(Wireup.WireupPhase.AfterWireup)
		{
			Behaviors = RegistrationBehaviors.Default;
		}
		
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="registerFor">indicates the type for which the target will become
		/// a registered implementation.</param>
		/// <param name="behaviors">registration behaviors</param>
		public ContainerRegisterAttribute(Type registerFor, RegistrationBehaviors behaviors)
            : this(Wireup.WireupPhase.AfterWireup, registerFor, behaviors)
		{
		}

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="phase">the wireup phase in which the registration should be completed</param>
        /// <param name="registerFor">indicates the type for which the target will become
        /// a registered implementation.</param>
        /// <param name="behaviors">registration behaviors</param>
        public ContainerRegisterAttribute(WireupPhase phase, Type registerFor, RegistrationBehaviors behaviors)
            : base(phase)
        {
            RegistratedForType = registerFor;
            Behaviors = behaviors;
        }

        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="registerFor">indicates the type for which the target will become
        /// a registered implementation.</param>
        /// <param name="behaviors">registration behaviors</param>
        /// <param name="name">the registered implementation's name</param>
        public ContainerRegisterAttribute(Type registerFor, RegistrationBehaviors behaviors, string name)
            : this(WireupPhase.AfterWireup, registerFor, behaviors, name)
        {
        }

		/// <summary>
		/// Creates a new instance.
		/// </summary>
        /// <param name="phase">the wireup phase in which the registration should be completed</param>
        /// <param name="registerFor">indicates the type for which the target will become
		/// a registered implementation.</param>
		/// <param name="behaviors">registration behaviors</param>
		/// <param name="name">the registered implementation's name</param>
		public ContainerRegisterAttribute(WireupPhase phase, Type registerFor, RegistrationBehaviors behaviors, string name)
            : base(phase)
		{
			RegistratedForType = registerFor;
			Behaviors = behaviors;
			Name = name;
		}

		/// <summary>
		/// Gets the type for which the target type is registered.
		/// </summary>
		public Type RegistratedForType { get; private set; }
		/// <summary>
		/// Gets the registration behaviors.
		/// </summary>
		public RegistrationBehaviors Behaviors { get; private set; }
		/// <summary>
		/// Gets the named implementation's name; otherwise null.
		/// </summary>
		public string Name { get; private set; }
		/// <summary>
		/// Indicates scope behavior for instances of the implementation type.
		/// </summary>
		public ScopeBehavior ScopeBehavior { get; set; }

		internal void ApplyRegistrationFor<T, C>(IContainer container)
			where C: T
		{
			Contract.Requires<ArgumentNullException>(container != null);

			if (Behaviors.HasFlag(RegistrationBehaviors.IfNoneOther)
				&& container.Registry.IsTypeRegistered(typeof(T)))
				return;

			var reg = container.Registry.ForType<T>();

			if (Behaviors.HasFlag(RegistrationBehaviors.Named)) {
				Contract.Assert(!String.IsNullOrEmpty(Name), 
					String.Concat("ContainerRegister must be given a name if it includes the 'Named' behavior: ",
					typeof(C).GetReadableFullName()));

				SetScopeBehavior(reg.RegisterWithName<C>(Name));
			}
			else 
			{
				SetScopeBehavior(reg.Register<C>());
			}
		}

		private void SetScopeBehavior(ITypeRegistration reg)
		{
			var scope = ScopeBehavior;
			if (scope.HasFlag(ScopeBehavior.Singleton))
			{
				reg.ResolveAsSingleton();
			}
			else if (scope.HasFlag(ScopeBehavior.InstancePerScope))
			{
				reg.ResolveAnInstancePerScope();
			}
			else
			{
				reg.ResolveAnInstancePerRequest();
			}
			if (scope.HasFlag(ScopeBehavior.SpecializationDisallowed))
			{
				reg.DisallowSpecialization();
			}
		}

		/// <summary>
		/// Performs the wireup task.
		/// </summary>
		/// <param name="coordinator"></param>
		protected override void PerformTask(Wireup.IWireupCoordinator coordinator)
		{
			// Purposely does nothing; the IoC's root container is wired up as an
			// observer and will perform any required work during notification.
		}
	}
}
