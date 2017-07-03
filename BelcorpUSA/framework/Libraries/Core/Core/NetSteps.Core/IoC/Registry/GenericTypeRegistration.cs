

using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using NetSteps.Encore.Core.IoC.Constructors;

namespace NetSteps.Encore.Core.IoC.Registry
{
	internal class GenericTypeRegistration : TypeRegistration, IGenericTypeRegistration
	{
		readonly Type _target;
		
		public GenericTypeRegistration(IContainer container, Type generic, Type target)
			: base(container, generic)
		{
			_target = target;
		}

		public override Type TargetType { get { return _target; } }

		public override IResolver UntypedResolver
		{
			get { throw new NotImplementedException(); }
		}

		public IResolver<T> ResolverFor<T>()
		{
			Type t = typeof(T);
			Type ultimateTarget = _target.MakeGenericType(t.GetGenericArguments());

			var behavior = this.ScopeBehavior;
			var ctor = Activator.CreateInstance(typeof(ConstructorSet<,>).MakeGenericType(t, ultimateTarget), new object[] { null });

			if (behavior.HasFlag(ScopeBehavior.Singleton))
			{
				return (IResolver<T>)Activator.CreateInstance(typeof(SingletonResolver<,>).MakeGenericType(t, ultimateTarget), ctor);
			}
			if (behavior.HasFlag(ScopeBehavior.InstancePerScope))
			{
				return (IResolver<T>)Activator.CreateInstance(typeof(InstancePerScopeResolver<,>).MakeGenericType(t, ultimateTarget), ctor);
			}
			else
			{
				return (IResolver<T>)Activator.CreateInstance(typeof(Resolver<,>).MakeGenericType(t, ultimateTarget), ctor);
			}			
		}	
	}
}
