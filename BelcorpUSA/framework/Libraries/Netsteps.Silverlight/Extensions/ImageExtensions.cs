using System;
using System.IO;
using System.Net;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace NetSteps.Silverlight.Extensions
{
    public static class ImageExtensions
    {
        public static void SetSource(this Image imageControl, string source)
        {
            if (source != null && (imageControl as Image) != null)
            {
                WebClient webClient = new WebClient();
                webClient.OpenReadCompleted += new OpenReadCompletedEventHandler(
                    (s, e) =>
                    {
                        if (e.Error == null && !e.Cancelled)
                        {
                            try
                            {
                                BitmapImage bmp = new BitmapImage();
                                bmp.SetSource(e.Result);
                                imageControl.Source = bmp;
                            }
                            catch (Exception ex)
                            {
                                //Exception handling
                            }
                        }
                        else
                        {
                            e.Error.ToString();   // something went wrong..
                        }
                    });
                webClient.OpenReadAsync(ApplicationContext.ResolveUrlWithXapAsBase(source));
            }
        }

        public static void SetSource(this Image imageControl, Stream source)
        {
            if (source != null && (imageControl as Image) != null)
            {
                try
                {
                    BitmapImage bmp = new BitmapImage();
                    bmp.SetSource(source);
                    imageControl.Source = bmp;
                }
                catch (Exception ex)
                {
                    //Exception handling
                }


            }
        }
    }
}
