using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace NetSteps.Testing.Core
{
    public static partial class TestHelpers
    {
        /// <summary>
        /// Invokes a method using a given number of threads and waits for all threads to complete.
        /// </summary>
        /// <param name="method">The method to invoke.</param>
        /// <param name="numThreads">The number of threads to create and execute.</param>
        /// <param name="parameter">An optional object that contains data to be used by the method the thread executes.</param>
        public static void ExecuteThreads(
            ParameterizedThreadStart method,
            int numThreads,
            object parameter = null)
        {
            if (numThreads < 1)
            {
                throw new ArgumentException("numThreads must be greater than zero.");
            }
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            var threads = new Thread[numThreads];
            for (int i = 0; i < numThreads; i++)
            {
                threads[i] = new Thread(method);
            }
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].Start(parameter);
            }
            for (int i = 0; i < numThreads; i++)
            {
                threads[i].Join();
            }
        }
    }
}
