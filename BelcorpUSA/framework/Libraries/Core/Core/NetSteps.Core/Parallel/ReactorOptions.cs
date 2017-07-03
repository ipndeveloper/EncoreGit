using System;
using System.Diagnostics.Contracts;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Options for the Reactor class.
	/// </summary>
	public sealed class ReactorOptions
	{
        /// <summary>
        /// Default max DOP
        /// </summary>
        public static readonly int DefaultMaxDegreeOfParallelism = 
					Math.Min(1, Convert.ToInt32(Environment.ProcessorCount * 0.8));

        /// <summary>
        /// Default miximum parallel depth.
        /// </summary>
        public static readonly int DefaultMaxParallelDepth = 10000;
        /// <summary>
        /// Default dispatches per borrowed thread. Used when max parallel depth is exceeded.
        /// </summary>
        public static readonly int DefaultDispatchesPerBorrowedThread = 1;

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public ReactorOptions()
            : this(DefaultMaxDegreeOfParallelism)
		{
		}

		/// <summary>
		/// Creates a new instance with a max degree of parallelism.
		/// </summary>
		/// <param name="maxDegreeOfParallelism">a max degree of parallelism</param>
		public ReactorOptions(int maxDegreeOfParallelism)
			: this(maxDegreeOfParallelism, false, 0, DefaultMaxParallelDepth, DefaultDispatchesPerBorrowedThread)
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="maxDegreeOfParallelism">a max degree of parallelism</param>
		/// <param name="yieldBusyReactor">indicates whether to occasionally yield a busy reactor</param>
        /// <param name="yieldFrequency">indicates yield frequency when yielding a busy reactor</param>
        /// <param name="maxParallelDepth">maximum parallel depth</param>
        /// <param name="dispatchesPerSequential">dispatches per borowed thread</param>
		public ReactorOptions(int maxDegreeOfParallelism, bool yieldBusyReactor, int yieldFrequency,
            int maxParallelDepth, int dispatchesPerSequential)
		{
			Contract.Requires<ArgumentOutOfRangeException>(maxDegreeOfParallelism >= 1);
            Contract.Requires<ArgumentOutOfRangeException>(!yieldBusyReactor || yieldFrequency >= 1);
            Contract.Requires<ArgumentOutOfRangeException>(maxParallelDepth >= 1);
            Contract.Requires<ArgumentOutOfRangeException>(dispatchesPerSequential >= 1);

			MaxDegreeOfParallelism = maxDegreeOfParallelism;
			YieldBusyReactor = yieldBusyReactor;
			YieldFrequency = yieldFrequency;
            MaxParallelDepth = maxParallelDepth;
            DispatchesPerBorrowedThread = dispatchesPerSequential;
		}

		/// <summary>
		/// The reactor's max degree of parallelism. This option controls the maximum number of concurrent threads
		/// used to react to items pushed to the reactor.
		/// </summary>
		public int MaxDegreeOfParallelism { get; private set; }

		/// <summary>
		/// Whether the reactor yields busy reactor threads. This option can provide better parallelism when the
		/// entire thread pool is busy.
		/// </summary>
		public bool YieldBusyReactor { get; private set; }

		/// <summary>
		/// Indicates the frequency at which a reactor thread yields.
		/// </summary>
		/// <remarks>Generally speaking, when a reactor is configured to yield, each thread pool thread will
		/// react to at most YieldFrequency items before yielding the thread back to the pool.</remarks>
		public int YieldFrequency { get; private set; }

        /// <summary>
        /// Maximum parallel depth.
        /// </summary>
        /// <remarks>For busy reactors, borrows the callers thread when the maximum parallel depth
        /// is reached.</remarks>
        public int MaxParallelDepth { get; private set; }

        /// <summary>
        /// Sequential dispatches per borrowed thread.
        /// </summary>
        /// <remarks>For busy reactors, when borrowing the caller's thread, the number of items to
        /// process before returning control to the caller.</remarks>
        public int DispatchesPerBorrowedThread { get; private set; }
	}

}
