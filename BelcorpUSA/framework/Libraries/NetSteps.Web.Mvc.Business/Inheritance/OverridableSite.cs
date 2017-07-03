using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Web.Mvc.Business.Inheritance
{
	public static class OverridableSite
	{
		#region Fields

		private static readonly Type This = typeof(OverridableSite);
		public static Assembly[] OverrideAssemblies { get { return __overrideAssembliesFactory.Value; } }
		private static Lazy<Assembly[]> __overrideAssembliesFactory = new Lazy<Assembly[]>(InitializeOverrideAssemblies);

		#endregion

		#region Properties

		#endregion

		#region Construction

		#endregion

		#region Methods

		/// <summary>
		/// Registers the VirtualPathProvider and ControllerFactory for the application.
		/// </summary>
		public static void Register()
		{
			HostingEnvironment.RegisterVirtualPathProvider(new AssemblyPathProvider(OverrideAssemblies));

			ControllerBuilder.Current.SetControllerFactory(new OverridableControllerFactory(OverrideAssemblies));

			RouteTable.Routes.MapRoute("Resource", "Resource/{*path}", new { controller = "Resource", action = "Index", path = "" });
		}

		private static Assembly[] InitializeOverrideAssemblies()
		{
			var assemblyElements = new List<AssemblyElement>();
			foreach (AssemblyElement assemblyElement in OverridableSiteSection.Instance.Assemblies)
			{
				assemblyElements.Add(assemblyElement);
			}

			var assemblies = new List<Assembly>();
			foreach (var assemblyElement in assemblyElements.OrderBy(x => x.SortIndex))
			{
				var assembly = Assembly.Load(assemblyElement.Name);

				// This is lame, but it works - Lundy
				var orClass = assembly.GetType("NetSteps.Web.ApplicationOverrides");
				if (orClass != null)
				{
					var method = orClass.GetMethod("Application_Start");
					if (method != null)
					{
						method.Invoke(null, null);
					}
				}

				This.TraceInformation(String.Concat("Successfully loaded ", assembly.FullName, " at sort index ", assemblyElement.SortIndex));
				assemblies.Add(assembly);
			}

			return assemblies.ToArray();
		}

		#endregion
	}
}
