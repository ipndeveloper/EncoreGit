using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core.Interfaces;
using WatiN.Core.Native.Windows;
using WatiN.Core.Native.InternetExplorer;

namespace TestMasterHelpProvider.DialogHandlers
{
    public class OKDialogHandler: IDialogHandler
	{

		/// <summary>
		/// OK Dialog Handler
		/// </summary>
        public OKDialogHandler()
		{
			
		}

		/// <summary>
		/// Gets whether we can handle dialogs.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="mainWindowHwnd"></param>
		/// <returns></returns>
		public bool CanHandleDialog(Window window, IntPtr mainWindowHwnd)
		{
            return GetOKButton(window) != null;

		}

        private WinButton GetOKButton(Window window)
        {
            var windowButton = new WindowsEnumerator().GetChildWindows(window.Hwnd, w => w.ClassName == "Button" && new WinButton(w.Hwnd).Title == "OK").FirstOrDefault();
            if (windowButton == null)
                return null;
            else
                return new WinButton(windowButton.Hwnd);
        }


		/// <summary>
		/// Handles a dialog based on the window title given in the constructor
		/// </summary>
		/// <param name="window"></param>
		/// <returns></returns>
		public bool HandleDialog(Window window)
		{
            var button = GetOKButton(window);
            if (button != null)
            {
                button.Click();
                return true;
            }
            else
            {
                return false;
            }

		}
    }
}
