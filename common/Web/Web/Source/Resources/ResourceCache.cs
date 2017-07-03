using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace NetSteps.WebControls
{
    /// <summary>
    /// For caching assembly resources. - JHE
    /// </summary>
    public static class ResourceCache
    {
        private static bool _cacheResources = false; // False for testing - JHE
        private static object _lock = new object();
        private static string resourcePath = "NetSteps.WebControls.Resources.{0}";

        private static string _pngFixScript = string.Empty;
        public static string PngFixScript
        {
            get
            {
                if (!_cacheResources)
                    _pngFixScript = null;

                if (string.IsNullOrEmpty(_pngFixScript))
                {
                    lock (_lock)
                    {
                        if (string.IsNullOrEmpty(_pngFixScript))
                        {
                            try
                            {
                                using (StreamReader textStreamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(string.Format(resourcePath, "PngFix.js"))))
                                {
                                    while (!textStreamReader.EndOfStream)
                                        _pngFixScript = textStreamReader.ReadToEnd();
                                }
                            }
                            catch
                            {
                                throw new Exception("Error loading resource: PngFix.js");
                            }
                        }
                    }
                }
                return _pngFixScript;
            }
        }
    }
}
