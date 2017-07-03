using System.Runtime.InteropServices;
using System;
using WatiN.Core;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace NetSteps.Testing.Integration
{

    /// <summary>
    /// Class for capturing screenshot for Firefox browser.
    /// </summary>
    public class Screenshot
    {
        class NativeMethods
        {
            [DllImport("user32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

            [StructLayout(LayoutKind.Sequential)]
            public struct RECT
            {
                public int Left;
                public int Top;
                public int Right;
                public int Bottom;
            }
        }
        /// <summary> 
        /// Takes a screenshot of the browser. 
        /// </summary> 
        /// <param name="browser">The browser object.</param> 
        /// <param name="filename">The path to store the file.</param> 
        /// <returns></returns> 
        public static bool SaveScreenshot(Browser browser, string filename)
        {
            bool success = false;

            IntPtr hWnd = browser.hWnd;

            Util.Browser.BringToFront();

            NativeMethods.RECT rect = new NativeMethods.RECT();

            try
            {
                if (NativeMethods.GetWindowRect(hWnd, ref rect))
                {
                    Size size = new Size(rect.Right - rect.Left,
                                         rect.Bottom - rect.Top);

                    // Get information about the screen.
                    using (Graphics browserGraphics = Graphics.FromHwnd(hWnd))

                    // Apply that info to a bitmap.
                    using (Bitmap screenshot = new Bitmap(size.Width, size.Height,
                                                          browserGraphics))

                    // And create an Graphics to manipulate that bitmap. 
                    using (Graphics imageGraphics = Graphics.FromImage(screenshot))
                    {
                        int hWndX = rect.Left;
                        int hWndY = rect.Top;
                        imageGraphics.CopyFromScreen(hWndX, hWndY, 0, 0, size);
                        screenshot.Save(filename, ImageFormat.Bmp);
                        success = true;
                    }
                }
            }
            catch
            { }

            // otherwise, fails. 
            return success;
        }
    }
}
