using System;
using System.Diagnostics.Contracts;
using NetSteps.Encore.Core.IoC;

using NetSteps.Encore.Core.Stereotype;
using NetSteps.Encore.Core.Properties;
using NetSteps.Encore.Core.Reflection;
using System.Reflection;

namespace NetSteps.Encore.Core
{
	/// <summary>
	/// Used by the framework too wireup copier implementations.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
	public class CopierAutoImplementAttribute : StereotypeAttribute
	{
		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public CopierAutoImplementAttribute()
			: base(StereotypeBehaviors.AutoImplementedBehavior)
		{
		}
		
		/// <summary>
		/// Creates and registers a copier implementation.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="container"></param>
		/// <returns></returns>	
		public override bool RegisterStereotypeImplementation<T>(IoC.IContainer container)
		{
			Contract.Assert(typeof(T).IsInterface, Resources.Err_StereotypeBehaviorGeneratedForInterfacesOnly);

			Type source = typeof(T).GetGenericArguments()[0];
			Type target = typeof(T).GetGenericArguments()[1];

			if (source.IsAnonymousType())
			{
				Type copierT = typeof(Copier<>).MakeGenericType(target);
				MethodInfo registerAnonymousSourceCopierForTarget = copierT.GetGenericMethod("RegisterAnonymousSourceCopierForTarget", BindingFlags.Static | BindingFlags.NonPublic, 0, 1).MakeGenericMethod(source);
				registerAnonymousSourceCopierForTarget.Invoke(null, Type.EmptyTypes);
			}
			else
			{
				Type concreteType = CopierTypeFactory.ConcreteType(source, target);

				Container.Root.ForType<T>()
					.Register(concreteType)
					.ResolveAnInstancePerRequest()
					.End();
			}
			return true;
		}
	}
}
