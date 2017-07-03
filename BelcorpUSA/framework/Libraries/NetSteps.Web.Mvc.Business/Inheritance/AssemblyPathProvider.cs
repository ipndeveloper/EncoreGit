//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics.Contracts;
//using System.Reflection;
//using System.Text.RegularExpressions;
//using System.Web.Hosting;
//using NetSteps.Web.Caching;
//using NetSteps.Web.Mvc.Extensions;
//using NetSteps.Diagnostics.Utilities;
//using System.Linq;
//using System.Web.Caching;


//namespace NetSteps.Web.Mvc.Business.Inheritance
//{
//    public class AssemblyPathProvider : VirtualPathProvider
//    {

//        #region Fields

//        #endregion

//        #region Properties

//        protected IDictionary<string, AssemblyVirtualFile> Resources
//        {
//            get;
//            private set;
//        }

//        #endregion

//        #region Construction

//        public AssemblyPathProvider(IEnumerable<Assembly> assemblies)
//        {
//            Contract.Requires<ArgumentNullException>(assemblies != null);
//            Dictionary<string, AssemblyVirtualFile> resources = new Dictionary<string, AssemblyVirtualFile>(StringComparer.OrdinalIgnoreCase);
//            foreach (var assembly in assemblies)
//            {
//                foreach (var resourceName in assembly.GetManifestResourceNames())
//                {
//                    var segments = resourceName.Replace(assembly.GetName().Name, String.Empty).Split('.');
//                    var path = String.Concat(String.Join("/", segments.Take(segments.Length - 2)), '/', String.Join(".", segments.Skip(segments.Length - 2)));
//                    this.TraceInformation(String.Concat("Mapping ", resourceName, " to  virtual path ", path));
//                    if (resources.ContainsKey(path))
//                    {
//                        resources.Remove(path);
//                        this.TraceWarning(String.Concat(resourceName, " is overriding previous assemblies virtual path ", path));
//                    }
//                    resources.Add(path, new AssemblyVirtualFile(path, resourceName, assembly));
//                }
//            }
//            Resources = resources;
//        }

//        #endregion

//        #region Methods

//        private bool TryResolve(string virtualPath, out VirtualFile file)
//        {
//            bool result = false;
//            file = null;
//            virtualPath = virtualPath.TrimStart('~');
//            AssemblyVirtualFile fileAsVirtual = null;
//            result = Resources.TryGetValue(virtualPath, out fileAsVirtual);
//            this.TraceVerbose(String.Concat((result)
//                ? "Successfully resolved "
//                : "Failed to resolve ", virtualPath));
//            if (result) file = fileAsVirtual;
//            return result;
//        }

//        public override bool FileExists(string virtualPath)
//        {
//            Contract.Requires(!String.IsNullOrWhiteSpace(virtualPath));
//            VirtualFile file = null;
//            return Previous.FileExists(virtualPath) || TryResolve(virtualPath, out file);
//        }

//        public override VirtualFile GetFile(string virtualPath)
//        {
//            Contract.Requires(!String.IsNullOrWhiteSpace(virtualPath));
//            VirtualFile result = null;
//            if (!TryResolve(virtualPath, out result)) result = Previous.GetFile(virtualPath);
//            return result;
//        }

//        public override CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
//        {
//            Contract.Requires(!String.IsNullOrWhiteSpace(virtualPath));
//            VirtualFile file = null;
//            CacheDependency result = new NeverExpiringCacheDependency();
//            if (!TryResolve(virtualPath,out file)) result = Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
//            return result;
//        }

//        #endregion
//    }
//}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using NetSteps.Web.Caching;
using NetSteps.Web.Mvc.Extensions;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Web.Mvc.Business.Inheritance
{
	public class AssemblyPathProvider : VirtualPathProvider
	{
		private static readonly Regex __assemblyPathRegex = new Regex("^(~/|/)", RegexOptions.Compiled);
		private readonly IDictionary<string, byte[]> _resources;

		public AssemblyPathProvider(IEnumerable<Assembly> assemblies)
		{
			Contract.Requires<ArgumentNullException>(assemblies != null);

			_resources = GetResources(assemblies);
		}

		public override bool FileExists(string virtualPath)
		{
			using (var fileExistsTracer = this.TraceActivity(string.Format("AssemblyPathProvider::FileExists - checking if {0} exists", virtualPath)))
			{
				var retVal = (Previous != null && Previous.FileExists(virtualPath)) || ResourceExists(virtualPath);
				return retVal;
			}
		}

		public override VirtualFile GetFile(string virtualPath)
		{

			byte[] resourceData;
			if (_resources.TryGetValue(VirtualPathToResourcePath(virtualPath), out resourceData))
			{
				return new AssemblyVirtualFile(virtualPath, resourceData);
			}

			var retVal = Previous.GetFile(virtualPath);
			return retVal;

		}

		public override System.Web.Caching.CacheDependency GetCacheDependency(string virtualPath, IEnumerable virtualPathDependencies, DateTime utcStart)
		{
			if (ResourceExists(virtualPath))
			{
				return new NeverExpiringCacheDependency();
			}

			return Previous.GetCacheDependency(virtualPath, virtualPathDependencies, utcStart);
		}

		#region Helpers
		private IDictionary<string, byte[]> GetResources(IEnumerable<Assembly> assemblies)
		{
			Contract.Requires<ArgumentNullException>(assemblies != null);

			var resources = new Dictionary<string, byte[]>(StringComparer.OrdinalIgnoreCase);
			using (this.TraceActivity("Loading embedded resources"))
			{
				foreach (var assembly in assemblies)
				{
					var assemblyPrefix = assembly.GetName().Name + ".";
					foreach (var resourceName in assembly.GetManifestResourceNames())
					{
						var key = resourceName.Replace(assemblyPrefix, "").ToLower();
						if (!resources.ContainsKey(key))
						{
							this.TraceInformation(String.Format("Mapping embedded resource {0} to {1}", key, resourceName));
							resources[key] = assembly.GetResource(resourceName);
						}
					}
				}
			}
			return resources;
		}

		private static string VirtualPathToResourcePath(string virtualPath)
		{
			return __assemblyPathRegex
				.Replace(virtualPath, "")
				.Replace('/', '.')
				.ToLower();
		}

		private bool ResourceExists(string virtualPath)
		{
			return _resources.ContainsKey(VirtualPathToResourcePath(virtualPath));
		}
		#endregion
	}
}
