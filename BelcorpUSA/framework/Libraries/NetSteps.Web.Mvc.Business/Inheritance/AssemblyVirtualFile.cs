//using System.IO;
//using System.Web.Hosting;
//using System.Reflection;

//namespace NetSteps.Web.Mvc.Business.Inheritance
//{
//    public class AssemblyVirtualFile : VirtualFile
//    {
//        private readonly Assembly Assembly;
//        private readonly string ResourceName;

//        public AssemblyVirtualFile(string virtualPath, string resourceName, Assembly assembly)
//            : base(virtualPath)
//        {
//            ResourceName = resourceName;
//            Assembly = assembly;
//        }

//        public override Stream Open()
//        {
//            return Assembly.GetManifestResourceStream(ResourceName);
//        }
//    }
//}

using System.IO;
using System.Web.Hosting;

namespace NetSteps.Web.Mvc.Business.Inheritance
{
    public class AssemblyVirtualFile : VirtualFile
    {
		private readonly byte[] _data;

        public AssemblyVirtualFile(string virtualPath, byte[] data)
            : base(virtualPath)
        {
			_data = data;
        }

        public override Stream Open()
        {
			var stream = new MemoryStream(_data.Length);
			stream.Write(_data, 0, _data.Length);
			stream.Seek(0, SeekOrigin.Begin);
			return stream;
        }
    }
}