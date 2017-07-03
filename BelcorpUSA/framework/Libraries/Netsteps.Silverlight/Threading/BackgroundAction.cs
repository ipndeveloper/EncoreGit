using System;
using System.ComponentModel;

namespace NetSteps.Silverlight.Threading
{
    public class BackgroundAction
    {
        private BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private Action _action = null;

        public BackgroundAction(Action action)
        {
            _action = action;
            _backgroundWorker.DoWork += new DoWorkEventHandler((object sender, DoWorkEventArgs e) =>
            {
                _action();
            });
        }

        public void Start()
        {
            _backgroundWorker.RunWorkerAsync();
        }

        public static void DoWork(Action action)
        {
            var backgroundAction = new BackgroundAction(action);
            backgroundAction.Start();
        }
    }
}
