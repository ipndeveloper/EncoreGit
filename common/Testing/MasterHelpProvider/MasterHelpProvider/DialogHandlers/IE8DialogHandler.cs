using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WatiN.Core.Interfaces;
using WatiN.Core.Native.Windows;

namespace TestMasterHelpProvider.DialogHandlers
{
	public class IE8DialogHandler : IDialogHandler
	{
		private string _title;

		/// <summary>
		/// IE8 Dialog Handler
		/// </summary>
		/// <param name="title">Title of the dialog window</param>
		public IE8DialogHandler(string title)
		{
			_title = title;
		}

		/// <summary>
		/// Gets whether we can handle dialogs.
		/// </summary>
		/// <param name="window"></param>
		/// <param name="mainWindowHwnd"></param>
		/// <returns></returns>
		public bool CanHandleDialog(Window window, IntPtr mainWindowHwnd)
		{
			return true;
		}

		/// <summary>
		/// Handles a dialog based on the window title given in the constructor
		/// </summary>
		/// <param name="window"></param>
		/// <returns></returns>
		public bool HandleDialog(Window window)
		{
			bool result = false;

			IntPtr dialogPointer = WindowClicker.FindWindow(null, _title);

			if (dialogPointer != null)
			{
				Window dialog = new Window(dialogPointer);
				dialog.ForceClose();

				result = true;
			}

			return result;
		}
	}
}
