/*
Copyright (c) 2008 <a href="http://www.gutgames.com">James Craig</a>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.*/

using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace NetSteps.Web.Imaging
{
	/// <summary>
	/// Class for taking a screen shot of a web page
	/// </summary>
	public class WebPageThumbnail
	{
		#region Constructor
		/// <summary>
		/// Constructor
		/// </summary>
		public WebPageThumbnail()
		{
		}
		#endregion

		#region Private Variables
		private Bitmap Image;
		//private string FileName;
		private string Url;
		private int Width;
		private int Height;
		#endregion

		#region Public Functions
		/// <summary>
		/// Generates a screen shot of a web site
		/// </summary>
		/// <param name="url">Url to take the screen shot of</param>
		/// <param name="width">Width of the image (-1 for full size)</param>
		/// <param name="height">Height of the image (-1 for full size)</param>
		public Bitmap GenerateBitmap(string url, int width, int height)
		{
			this.Url = url;
			this.Width = width;
			this.Height = height;
			Thread tempThread = new Thread(new ThreadStart(CreateBrowser));
			tempThread.SetApartmentState(ApartmentState.STA);
			tempThread.Start();
			tempThread.Join();

			return Image;
		}
		#endregion

		#region Private Functions
		/// <summary>
		/// Creates the browser
		/// </summary>
		private void CreateBrowser()
		{
			WebBrowser browser = new WebBrowser();
			browser.ScrollBarsEnabled = false;
			browser.Navigate(Url);
			browser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(Browser_DocumentCompleted);
			while (browser.ReadyState != WebBrowserReadyState.Complete)
				Application.DoEvents();
			browser.Dispose();
		}
		/// <summary>
		/// Called when the browser is completed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Browser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			WebBrowser browser = (WebBrowser)sender;
			browser.ScriptErrorsSuppressed = true;
			browser.ScrollBarsEnabled = false;
			if (Width == -1)
			{
				browser.Width = browser.Document.Body.ScrollRectangle.Width;
			}
			else
			{
				browser.Width = Width;
			}
			if (Height == -1)
			{
				browser.Height = browser.Document.Body.ScrollRectangle.Height;
			}
			else
			{
				browser.Height = Height;
			}
			Image = new Bitmap(browser.Width, browser.Height);
			browser.BringToFront();
			browser.DrawToBitmap(Image, new Rectangle(0, 0, browser.Width, browser.Height));
		}
		#endregion
	}
}
