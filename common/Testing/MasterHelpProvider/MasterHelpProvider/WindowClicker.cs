using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TestMasterHelpProvider
{
	public class WindowClicker
	{
		public const string IEDialogClassName = "32770";
		public const string FFDialogClassName = "MozillaDialogClass";

		/// <summary>
		/// Gets a window's window handle as an int.
		/// </summary>
		/// <param name="lpClassName">Dialog or Window class name</param>
		/// <param name="lpWindowName">Dialog or Window title</param>
		/// <returns></returns>
		[DllImport("User32.dll")]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
	}
}
