using System;
using System.Diagnostics;

namespace NetSteps.Common
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class to test performance of operation by measuring how long it takes to perform a set of actions
	/// and output the basic stats of the operation.
	/// Created: 02-24-2010
	/// </summary>
	public class OperationTimer : IDisposable
	{
		private Int64 _startTime;
		private string _text;
		private Int64 _collectionCount;
		private Action<string> _completedAction;
		private int _cycles = 0;

		public OperationTimer(string text)
		{
			PrepareForOperation();

			_text = text;
			_collectionCount = GC.CollectionCount(0);


			// This should be the last statement in this method to keep timing as accurate as possible.
			_startTime = Stopwatch.GetTimestamp();
		}
		public OperationTimer(string text, int cycles, Action<string> completedAction)
		{
			PrepareForOperation();

			_text = text;
			_completedAction = completedAction;
			_cycles = cycles;
			_collectionCount = GC.CollectionCount(0);


			// This should be the last statement in this method to keep timing as accurate as possible.
			_startTime = Stopwatch.GetTimestamp();
		}

		public void Dispose()
		{
			var timeSpan = (Stopwatch.GetTimestamp() - _startTime) / (double)Stopwatch.Frequency;
			string completedMessage = string.Format("{2} count: {4}, {0,6:###.00} seconds (GCs={1,3}) avg({3,6:###.00}/sec)", (Stopwatch.GetTimestamp() - _startTime) / (double)Stopwatch.Frequency,
				GC.CollectionCount(0) - _collectionCount, _text, timeSpan / _cycles, _cycles);

			if (_completedAction != null)
				_completedAction(completedMessage);
		}

		public static void PrepareForOperation()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}

	/// <summary>
	/// Author: Daniel Stafford
	/// Description: Simplified version of the OperationTimer class that automatically prints out the results to the debug window
	/// Created: 08-20-2010
	/// </summary>
	public class OperationDebugTimer : IDisposable
	{
		private Stopwatch _watch;
		private string _methodName;

		public OperationDebugTimer()
		{
			StackTrace st = new StackTrace();
			var callingMethod = st.GetFrame(1).GetMethod();
			_methodName = callingMethod.DeclaringType.Name + "." + callingMethod.Name;
			_watch = Stopwatch.StartNew();
		}

		public OperationDebugTimer(string methodName)
		{
			_methodName = methodName;
			_watch = Stopwatch.StartNew();
		}

		public void Dispose()
		{
			_watch.Stop();

			System.Diagnostics.Debug.WriteLine(string.Format("Method '{0}' took {1} millisecond(s), or {2} ticks", _methodName, _watch.ElapsedMilliseconds, _watch.ElapsedTicks));
		}
	}
}
