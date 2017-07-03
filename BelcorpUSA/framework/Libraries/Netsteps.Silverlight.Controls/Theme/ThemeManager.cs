using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using NetSteps.Silverlight.Base;
using NetSteps.Silverlight.Extensions;

namespace NetSteps.Silverlight.Controls
{
    public static class ThemeManager
    {
        private static object _lock = new object();
        private static Enums.Theme _theme = Enums.Theme.NetSteps;
        public static Enums.Theme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                lock (_lock)
                {
                    CurrentThemeResourceDictionary = null;
                    CurrentThemeResourceDictionaryKeys = new List<string>();

                    _theme = value;

                    loadCurrentThemeResourceDictionary();
                }
            }
        }

        #region Enums
        public static class Enums
        {
            public enum Theme
            {
                NetSteps,
                Zrii,
                RodanAndFields,
                Natura,
                Scentsy
            }
        }
        #endregion

        #region Members
        private static ThreadSafeDictionary<string, string> _themes = new ThreadSafeDictionary<string, string>();
        private static ThreadSafeList<Uri> _implicitThemes = new ThreadSafeList<Uri>();
        #endregion

        public static List<string> CurrentThemeResourceDictionaryKeys
        {
            get;
            private set;
        }

        public static ResourceDictionary CurrentThemeResourceDictionary
        {
            get;
            private set;
        }

        private static void loadCurrentThemeResourceDictionary()
        {
            List<string> keys;
            CurrentThemeResourceDictionary = Resource.Parse(CurrentThemeDictionaryUri, out keys);
            CurrentThemeResourceDictionaryKeys = keys;
        }

        public static Uri CurrentThemeDictionaryUri
        {
            get
            {
                if (_themes.Count == 0)
                    LoadImplicitThemes();

                return _implicitThemes[Theme.ToInt()];
            }
        }

        private static void LoadImplicitThemes()
        {
            _implicitThemes.Add(new Uri(@"/NetSteps.CRM.myHome;component/ResourceDictionaries/NetSteps.xaml", UriKind.Relative));
            _implicitThemes.Add(new Uri(@"/NetSteps.CRM.myHome;component/ResourceDictionaries/Zrii.xaml", UriKind.Relative));


            //Uri uri = new Uri("RodanAndFields.xaml", UriKind.Relative);
            //_implicitThemes.Add(uri);
            _implicitThemes.Add(new Uri(@"/NetSteps.CRM.myHome;component/ResourceDictionaries/RodanAndFields.xaml", UriKind.Relative));

            // TODO: Add ability to download these styles dynamically and apply them, later when the file sizes get larger. - JHE    

            _implicitThemes.Add(new Uri(@"/NetSteps.CRM.myHome;component/ResourceDictionaries/Natura.xaml", UriKind.Relative));

            _implicitThemes.Add(new Uri(@"/NetSteps.CRM.myHome;component/ResourceDictionaries/Scentsy.xaml", UriKind.Relative));
        }


        // Add this back for Silverlight 4 later; see - http://forums.silverlight.net/forums/p/163437/396400.aspx - JHE
        //public static void SetImplicitTheme(FrameworkElement frameworkElement)
        //{
        //    SetImplicitTheme(frameworkElement, Theme);
        //}
        //public static void SetImplicitTheme(FrameworkElement frameworkElement, Enums.Theme theme)
        //{
        //    if (_themes.Count == 0)
        //        LoadImplicitThemes();

        //    ImplicitStyleManager.SetResourceDictionaryUri(frameworkElement, _implicitThemes[theme.ToInt()]);
        //    ImplicitStyleManager.SetApplyMode(frameworkElement, ImplicitStylesApplyMode.Auto);   // For Performance; not using Auto - JHE
        //    ImplicitStyleManager.Apply(frameworkElement);
        //}

        public static void LoadExplicitThemes()
        {

            WebClient webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler((object sender, DownloadProgressChangedEventArgs e) =>
            {
                //ProgressTextBlock.Text = "Downloading "
                //  + e.ProgressPercentage + "%";
            });
            webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(webClient_OpenReadCompleted);
            webClient.OpenReadAsync(new Uri("../SilverlightThemes/NetSteps.xaml", UriKind.Relative));

        }

        private static void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            //if (e.Error != null)
            //{
            //    //ProgressTextBlock.Text = "Error: "
            //    //  + e.Error.Message;
            //    return;
            //}

            ////ProgressTextBlock.Text = "Loading";

            //_zipInfo = new StreamResourceInfo(e.Result, null);

            //// Read manifest from zip file
            //StreamResourceInfo manifestInfo = Application.GetResourceStream(_zipInfo, new Uri("content.xml", UriKind.Relative));

            //StreamReader reader = new StreamReader(manifestInfo.Stream);
            //XDocument document = XDocument.Load(reader);

            //var mediaFiles = from m in document.Descendants("mediafile")
            //                 select new MediaInfo
            //                 {
            //                     Type = (MediaType)Enum.Parse(typeof(MediaType),
            //                       m.Attribute("type").Value, true),

            //                     Name = m.Attribute("name").Value
            //                 };

            //_mediaInfos = new List<MediaInfo>();
            //_mediaInfos.AddRange(mediaFiles);

            //ProgressTextBlock.Visibility = Visibility.Collapsed;
            //PrevButton.IsEnabled = true;
            //NextButton.IsEnabled = true;
            //DisplayMedia(_mediaInfos[0]);

            //themes.Add(
        }


    }
}
