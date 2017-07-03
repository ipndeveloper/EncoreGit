using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;
using NetSteps.Silverlight.Base;

namespace NetSteps.Silverlight
{
    public static partial class Resource
    {
        public static ResourceDictionary Parse(Uri uri, out List<String> keys)
        {
            Debug.Assert(uri != null, "uri cannot be null.");



            // Dynamic Font Embedding test - JHE
            //Uri address = new Uri(Application.Current.Host.Source, string.Format("../ClientBin/{0}.xap", xapName));
            //WebClient webClient = new WebClient();
            //webClient.OpenReadCompleted += new OpenReadCompletedEventHandler((object sender, OpenReadCompletedEventArgs e) =>
            //{
            //    if (e.Error != null)
            //        throw e.Error;

            //    if ((e.Error == null) && (e.Cancelled == false))
            //    {
            //        StreamResourceInfo zipResourceInfo = new StreamResourceInfo(e.Result, @"application/zip");
            //        StreamResourceInfo fontInfo = Application.GetResourceStream(zipResourceInfo, new Uri(fontFileName, UriKind.Relative));
            //        //txtWelcomeMessage.FontSource = new FontSource(fontInfo.Stream);
            //        //txtWelcomeMessage.FontFamily = new FontFamily("Trade Gothic LT Std");
            //        //txtWelcomeMessage.FontSize = 28;
            //        return fontInfo;
            //    }
            //});
            //webClient.OpenReadAsync(address);


            keys = null;

            Stream resource = Application.GetResourceStream(uri).Stream;
            using (StreamReader reader = new StreamReader(resource))
            {
                string xaml = reader.ReadToEnd();

                //  ResourceDictionary is not enumerable and doesn't implement the Keys collection yet.
                //  Hopefully this will not be the case in future releases.
                XDocument xml = XDocument.Parse(xaml);

                string keyAttribute = "{http://schemas.microsoft.com/winfx/2006/xaml}Key";
                string nameAttribute = "{http://schemas.microsoft.com/winfx/2006/xaml}Name";

                if (xml.Root != null)
                {
                    keys = (from element in xml.Root.Elements()
                            where (element.Attribute(keyAttribute) != null) || (element.Attribute(nameAttribute) != null)
                            select (element.Attribute(keyAttribute) ?? element.Attribute(nameAttribute)).Value).ToList();
                }
            }

            StreamResourceInfo info = Application.GetResourceStream(uri);
            if (info == null)
            {
                throw new Exception("Resource Not Found");
            }
            else
            {
                try
                {
                    return ResourceParser.Parse(info.Stream, false);
                }
                catch
                {
                    throw new Exception("Unable To Load Resources");
                }
            }
        }

        public static Dictionary<string, string> GetResourcesDictionaryXaml(Uri uri)
        {
            Debug.Assert(uri != null, "uri cannot be null.");
            Dictionary<string, string> resourcesDictionaryXaml = new Dictionary<string, string>();

            Stream resource = Application.GetResourceStream(uri).Stream;
            using (StreamReader reader = new StreamReader(resource))
            {
                string xaml = reader.ReadToEnd();

                //  ResourceDictionary is not enumerable and doesn't implement the Keys collection yet.
                //  Hopefully this will not be the case in future releases.
                XDocument xml = XDocument.Parse(xaml);

                string keyAttribute = "{http://schemas.microsoft.com/winfx/2006/xaml}Key";
                string nameAttribute = "{http://schemas.microsoft.com/winfx/2006/xaml}Name";

                if (xml.Root != null)
                {
                    var elements = from element in xml.Root.Elements()
                                   where (element.Attribute(keyAttribute) != null) || (element.Attribute(nameAttribute) != null)
                                   select new NameValue<string, string>()
                                   {
                                       Name = (element.Attribute(keyAttribute) ?? element.Attribute(nameAttribute)).Value,
                                       Value = element.ToString()
                                   };

                    foreach (var element in elements.ToList())
                        resourcesDictionaryXaml.Add(element.Name, element.Value);
                }
            }

            return resourcesDictionaryXaml;
        }
    }
}