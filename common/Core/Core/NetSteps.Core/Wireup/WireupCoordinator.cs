using System;
using System.Linq;
using System.Configuration;
using System.Reflection;
using System.Threading;
using NetSteps.Encore.Core.Configuration;

namespace NetSteps.Encore.Core.Wireup
{
	/// <summary>
	/// Utility class for coordinating wireup.
	/// </summary>
	public static class WireupCoordinator
	{
		static bool __initialized;
        static bool __reentryDetected;
        static object __sync = new Object();
		static readonly Lazy<IWireupCoordinator> _coordinator = new Lazy<IWireupCoordinator>(PerformBootstrapCurrentProcess, LazyThreadSafetyMode.ExecutionAndPublication);

		/// <summary>
		/// Accesses the singleton IWireupCoordinator instance.
		/// </summary>
		public static IWireupCoordinator Instance
		{
			get
			{
				var coord = _coordinator.Value;
				if (!__initialized)
				{
                    lock (__sync)
                    {
                        if (!__initialized)
                        {
                            if (!__reentryDetected)
                            {
                                try
                                {
                                    __reentryDetected = true;

                                    // in case there is no config; make sure this assembly is whole...
                                    coord.WireupDependencies(Assembly.GetExecutingAssembly());
                                    var config = WireupConfigurationSection.Instance;
                                    foreach (WireupConfigurationElement e in config.Assemblies.OrderBy(e => e.Ordinal))
                                    {
                                        e.PerformWireup(coord);
                                    }
                                    var domain = AppDomain.CurrentDomain;
                                    if (config.WireupAllRunningAssemblies)
                                    {
                                        foreach (var asm in domain.GetAssemblies())
                                        {
                                            coord.NotifyAssemblyLoaded(asm);
                                        }
                                    }
                                    if (config.HookAssemblyLoad)
                                    {
                                        domain.AssemblyLoad += new AssemblyLoadEventHandler((sender, e) => {
                                            coord.NotifyAssemblyLoaded(e.LoadedAssembly);
                                        });
                                    }
                                
                                    __initialized = true;
                                }
                                finally
                                {
                                    __reentryDetected = false;
                                }                                
                            }
                        }
                    }
				}
				return coord;
			}
		}

		/// <summary>
		/// Causes the wireup coordinator to self-configure.
		/// </summary>
		/// <returns>the coordinator after it self-configures</returns>
		public static IWireupCoordinator SelfConfigure()
		{
			var coordinator = WireupCoordinator.Instance;
			coordinator.WireupDependencies(Assembly.GetCallingAssembly());
			return coordinator;
		}

		static IWireupCoordinator PerformBootstrapCurrentProcess()
		{			
			return WireupConfigurationSection.Instance.Coordinator;
		}
	}
}
