using System;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Threading;
using NetSteps.Encore.Core.Reflection.Emit;
using NetSteps.Encore.Core.Reflection;

namespace NetSteps.Encore.Core.IoC.Constructors
{
	internal static class ConstructorAdapter
	{
		static readonly Lazy<EmittedModule> __module = new Lazy<EmittedModule>(() =>
		{ return RuntimeAssemblies.DynamicAssembly.DefineModule("Constructors", null); },
			LazyThreadSafetyMode.ExecutionAndPublication
			);
		internal static EmittedModule Module { get { return __module.Value; } }
	}
}
