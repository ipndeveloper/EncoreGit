using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Resources;
using System.Xml.Linq;

namespace NetSteps.Silverlight
{
    /// <summary>
    /// Downloader class to dynamically instantiate Silverlight control from other XAP files. - JHE
    /// 
    /// Example:
    /// Downloader downloader = new Downloader();
    /// downloader.XapDownloaded += new EventHandler<XapEventArgs>((object sender2, XapEventArgs e2) =>
    /// {
    ///     faceMonkey = e2.DownloadedContent;
    ///     AddControlToGrid();
    ///     uxLoadingAnimation.Visibility = Visibility.Collapsed;
    /// });
    /// downloader.LoadPackage("FaceMonkey.xap", "FaceMonkey.dll", "FaceMonkey.Page");
    /// uxLoadingAnimation.Visibility = Visibility.Visible;
    /// </summary>
    public class Downloader
    {
        #region Local members

        private static string LONG_ASSEMBLY_NAME = ", Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

        private static Dictionary<string, Stream> _DownloadedXaps;
        private static Dictionary<string, Stream> DownloadedXaps
        {
            get
            {
                if (_DownloadedXaps == null)
                    _DownloadedXaps = new Dictionary<string, Stream>();
                return _DownloadedXaps;
            }
        }


        #endregion

        public event EventHandler<XapEventArgs> XapDownloaded;

        public void DownloadXap(string xap)
        {
            try
            {
                Uri address = new Uri(xap, UriKind.RelativeOrAbsolute);
                WebClient client = new WebClient();
                client.OpenReadCompleted += new OpenReadCompletedEventHandler(
                (sender, e) =>
                {
                    try
                    {
                        if (e.Error != null)
                            return;

                        if (!DownloadedXaps.ContainsKey(xap))
                        {
                            Uri uri = new Uri("AppManifest.xaml", UriKind.Relative);
                            StreamResourceInfo resInfo = new StreamResourceInfo(e.Result, null);
                            StreamResourceInfo resInfo2 = Application.GetResourceStream(resInfo, uri);
                            Stream stream = resInfo2.Stream;
                            StreamReader reader = new StreamReader(stream);
                            string AppManifest = reader.ReadToEnd();

                            XElement deploymentRoot = XDocument.Parse(AppManifest).Root;
                            List<XElement> deploymentParts = (from assemblyParts in deploymentRoot.Elements().Elements() select assemblyParts).ToList();

                            foreach (XElement xEle in deploymentParts)
                            {
                                string source = xEle.Attribute("Source").Value;
                                AssemblyPart asmPart = new AssemblyPart();

                                StreamResourceInfo streamInfo = Application.GetResourceStream(new StreamResourceInfo(e.Result, "application/binary"), new Uri(source, UriKind.Relative));

                                asmPart.Load(streamInfo.Stream);
                            }

                            DownloadedXaps.Add(xap, e.Result);
                        }

                        XapEventArgs args = new XapEventArgs();
                        args.XapName = xap;
                        args.XapStream = DownloadedXaps[xap];
                        XapDownloaded(this, args);
                    }
                    catch (Exception ex)
                    {
                        //TODO: We need to add an error to the callback MN
                    }
                });

                client.OpenReadAsync(address);
            }
            catch (Exception ex)
            {
                //TODO: We need to add an error to the callback MN
            }
        }

        public static void LoadUserControl(XAttribute attrControl, Action<UserControl> callback)
        {
            var result = from cont in attrControl.Document.Descendants("Control")
                         where (attrControl.Value == cont.Attribute("Name").Value)
                         select cont;

            XElement xeControl = result.FirstOrDefault();

            LoadUserControl(xeControl, callback);
        }

        public static UserControl SynchLoadUserControl(XElement xeControl)
        {
            return (UserControl)SynchLoadObject(xeControl);
        }

        public static void LoadUserControl(XElement xeControl, Action<UserControl> callback)
        {
            LoadObject(xeControl,
                (obj) =>
                {
                    callback((UserControl)obj);
                });
        }

        private static string CreateLongAssemblyName(string AssemblyName)
        {
            string result = AssemblyName;
            if (!result.Contains(','))
            {
                if (result.EndsWith(".dll"))
                {
                    result = result.Substring(0, result.IndexOf(".dll"));
                }

                result += LONG_ASSEMBLY_NAME;
            }

            return result;
        }

        private static void SetPropertiesForObject(XElement xeControl, object objControl)
        {
            var properties = from prop in xeControl.Descendants("Property")
                             select prop;

            foreach (XElement property in properties)
            {
                string propertyName = property.Attribute("Name").Value;
                XElement xmlValue = property.Descendants().First();

                PropertyInfo reflectedProperty = objControl.GetType().GetProperty(propertyName);

                object value = null;
                string type = xmlValue.Name.LocalName.Trim().ToLower();
                string stringValue = xmlValue.Value.ToString();
                switch (type)
                {
                    case "string": value = stringValue; break;
                    case "int": value = Convert.ToInt32(stringValue.Trim()); break;
                    case "bool": value = Convert.ToBoolean(stringValue.Trim()); break;
                    case "double": value = Convert.ToDouble(stringValue.Trim()); break;
                    case "visibility": value = Enum.Parse(typeof(Visibility), stringValue, true); break;
                    case "uri": value = new Uri(stringValue.Trim()); break;
                    default: value = XamlReader.Load(xmlValue.ToString()); break;
                }

                if (value != null && reflectedProperty != null)
                {
                    if (reflectedProperty.CanWrite)
                        reflectedProperty.SetValue(objControl, value, null);
                }
            }
        }

        public static object SynchLoadObject(XElement xeControl)
        {
            string XapPath = xeControl.Attribute("Path").Value;
            string AssemblyName = CreateLongAssemblyName(xeControl.Attribute("AssemblyName").Value);
            string ClassName = xeControl.Attribute("Class").Value;

            object objControl = null;
            try
            {
                AssemblyName an = new AssemblyName(AssemblyName);
                Assembly a = Assembly.Load(AssemblyName);
                objControl = a.CreateInstance(ClassName);
                SetPropertiesForObject(xeControl, objControl);
            }
            catch (Exception ex)
            {
                return null;
            }
            return (objControl);
        }

        public static void LoadObject(XElement xeControl, Action<object> callback)
        {
            if (xeControl != null)
            {
                string XapPath = xeControl.Attribute("Path").Value;
                string AssemblyName = CreateLongAssemblyName(xeControl.Attribute("AssemblyName").Value);
                string ClassName = xeControl.Attribute("Class").Value;

                if (DownloadedXaps.ContainsKey(XapPath))
                {
                    object objControl = null;
                    try
                    {
                        AssemblyName an = new AssemblyName(AssemblyName);
                        Assembly a = Assembly.Load(AssemblyName);
                        objControl = a.CreateInstance(ClassName);
                        SetPropertiesForObject(xeControl, objControl);
                    }
                    catch (Exception ex)
                    {
                        if (callback != null)
                            callback(null);
                    }
                    if (callback != null)
                        callback(objControl);

                }
                else
                {
                    Downloader dnldr = new Downloader();
                    dnldr.XapDownloaded += new EventHandler<XapEventArgs>(
                        (sender, e) =>
                        {
                            try
                            {
                                if (XapPath == e.XapName && e.XapStream != null && callback != null)
                                {
                                    AssemblyName an = new AssemblyName(AssemblyName);
                                    Assembly a = Assembly.Load(AssemblyName);
                                    object objControl = a.CreateInstance(ClassName);
                                    SetPropertiesForObject(xeControl, objControl);

                                    callback(objControl);
                                }
                            }
                            catch (Exception ex)
                            {
                                if (callback != null)
                                    callback(null);
                            }
                        });
                    dnldr.DownloadXap(XapPath);
                }
            }
            else
            {
                if (callback != null)
                    callback(null);
            }
        }

        public static FrameworkElement LoadUserControl(string name, System.Reflection.Assembly containingAssembly)
        {
            return Activator.CreateInstance(containingAssembly.GetType(name, false)) as FrameworkElement;
        }
    }

    public class XapEventArgs : EventArgs
    {
        public string XapName;
        public Stream XapStream;
    }
}
