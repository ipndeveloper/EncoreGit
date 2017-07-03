﻿/// Silverlight Contrib
/// WheelMouseListener code contributed by Mike Snow
/// Blog: http://silverlight.net/blogs/msnow

using System;
using System.Windows.Browser;

namespace SilverlightContrib.Utilities
{
    /// <summary>
    /// Delegate for the MouseWheelScroll event.
    /// </summary>
    /// <param name="args">Event data for the MouseWheelScroll event.</param>
    public delegate void WheelMouseHandler(WheelMouseEventArgs args);

    /// <summary>
    /// Event data for the MouseWheelScroll event.
    /// </summary>
    public class WheelMouseEventArgs : EventArgs
    {
        private readonly double m_delta;

        public double Delta
        {
            get { return m_delta; }
        }

        public WheelMouseEventArgs(double delta)
        {
            m_delta = delta;
        }
    }

    /// <summary>
    /// Provides access to the browser-generated mouse wheel scrolling events.
    /// </summary>
    public class WheelMouseListener : IDisposable
    {
        public event WheelMouseHandler MouseWheelScroll;

        public WheelMouseListener()
        {
            HtmlPage.Window.AttachEvent("DOMMouseScroll", OnMouseWheel);
            HtmlPage.Window.AttachEvent("onmousewheel", OnMouseWheel);
            HtmlPage.Document.AttachEvent("onmousewheel", OnMouseWheel);
        }

        private void OnMouseWheel(object sender, HtmlEventArgs args)
        {
            double delta = 0;
            ScriptObject e = args.EventObject;

            if (e.GetProperty("detail") != null)
            {
                // Mozilla and Safari
                delta = ((double)e.GetProperty("detail"));
            }
            else if (e.GetProperty("wheelDelta") != null)
            {
                // IE and Opera
                delta = ((double)e.GetProperty("wheelDelta"));
            }

            delta = Math.Sign(delta);

            if (MouseWheelScroll != null)
                MouseWheelScroll(new WheelMouseEventArgs(delta));
        }

        /// <summary>
        /// Detaches from the browser-generated scroll events.
        /// </summary>
        public void Dispose()
        {
            HtmlPage.Window.DetachEvent("DOMMouseScroll", OnMouseWheel);
            HtmlPage.Window.DetachEvent("onmousewheel", OnMouseWheel);
            HtmlPage.Document.DetachEvent("onmousewheel", OnMouseWheel);
        }
    }
}
