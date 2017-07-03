using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetSteps.Silverlight
{
    public class MemoryProfiler
    {
        #region private members
        private static object lockObject = new object();
        private static List<ObjectStruct> elementsList = new List<ObjectStruct>();
        private static int generationCounter = 0;
        #endregion

        #region innerClass
        private class ObjectStruct
        {
            public WeakReference Reference;             // weak reference to the monitored object
            public int Genereation;                     // the generation of the monitored object
            public string StackTrace;                   // stack trace string of when the object was created
            public bool DisposedSignaled;               // true if the Dispose() function of the object was called

            public ObjectStruct(WeakReference _Reference, int _Generation)
                : this(_Reference, _Generation, null)
            {

            }

            public ObjectStruct(WeakReference _Reference, int _Generation, string _StackTrace)
            {
                Reference = _Reference;
                Genereation = _Generation;
                StackTrace = _StackTrace;
                DisposedSignaled = false;
            }

            public override string ToString()
            {
                return Genereation.ToString() + ": " + Reference.Target.ToString() + ". Disposed signal = " + DisposedSignaled.ToString();
            }
        }
        #endregion

        #region public methods

        /// <summary>
        /// Call this method from within the constructor of the monitored object
        /// </summary>
        /// <param name="obj">The object to monitor</param>
        [Conditional("DEBUG")]
        public static void AddReference(object obj)
        {
            lock (lockObject)
            {
                // avoiding dups - can happen when adding an element and its base-class to the profiler
                if (elementsList.Any(p => p.Reference.IsAlive && p.Reference.Target == obj))
                    return;
                elementsList.Add(new ObjectStruct(new WeakReference(obj), generationCounter, new StackTrace().ToString()));
                //elementsList.Add(new ObjectStruct(new WeakReference(obj), generationCounter, null));
            }
        }

        /// <summary>
        /// Call this method whenever you want to check the current memory's state
        /// </summary>
        [Conditional("DEBUG")]
        public static void Check()
        {
            lock (lockObject)
            {
                long memUsage = GC.GetTotalMemory(true);
                for (int i = 0; i < elementsList.Count; i++)
                    if (!elementsList[i].Reference.IsAlive)
                        elementsList.RemoveAt(i--);
            }
            generationCounter++;
            //Debugger.Break();
        }

        /// <summary>
        /// Call this function from within the Dispose() method of the monitored object
        /// </summary>
        /// <param name="obj">The object to signal its disposal - should be in the profiler already!</param>
        [Conditional("DEBUG")]
        public static void SignalDisposed(object obj)
        {
            lock (lockObject)
            {
                var objectsToSignal = elementsList.Where(p => p.Reference.IsAlive && p.Reference.Target.Equals(obj));
                if (objectsToSignal.Any() && objectsToSignal.Count() == 1)
                {
                    var objectToSignal = objectsToSignal.First();
                    objectToSignal.DisposedSignaled = true;
                }
            }
        }

        #endregion
    }
}
