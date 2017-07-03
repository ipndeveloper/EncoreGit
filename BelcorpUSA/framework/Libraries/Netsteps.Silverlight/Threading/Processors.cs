using System.Threading;

namespace NetSteps.Silverlight.Threading
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 12/17/2009
    /// </summary>
    public class Processors
    {
        // http://www.codeproject.com/KB/silverlight/multicore.aspx - JHE
        public static bool HasMoreThanOneCPU()
        {
            bool moreThanOne = false;
            bool toContinue = true;
            ManualResetEvent m = new ManualResetEvent(false);
            Thread t = new Thread(new ThreadStart(delegate
            {
                m.Set();
                while (toContinue)
                    moreThanOne = true;
            }));
            t.IsBackground = false;
            t.Start();
            m.WaitOne();
            Thread.Sleep(0);
            moreThanOne = false;
            Thread.SpinWait(100000);
            toContinue = false;
            return moreThanOne;
        }

    }
}
