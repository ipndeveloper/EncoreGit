using System;

namespace NetSteps.Silverlight
{
    public delegate void TitleChangeEventHandler(object sender, TitleChangeEventArgs e);

    public class TitleChangeEventArgs : EventArgs
    {
        public string Title { get; protected set; }

        public TitleChangeEventArgs(string title)
        {
            Title = title;
        }
    }
}
