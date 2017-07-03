using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;

namespace NetSteps.Web.Mvc.Extensions
{
    public static class AssemblyExtensions
    {
		/// <summary>
		/// Returns an embedded resource from an assembly.
		/// </summary>
		public static byte[] GetResource(this Assembly assembly, string name)
		{
			Contract.Requires<ArgumentNullException>(assembly != null);
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentException>(name.Length > 0);

			var memoryStream = new MemoryStream();
			assembly.GetManifestResourceStream(name).CopyTo(memoryStream);
			return memoryStream.ToArray();
		}
    }
}
