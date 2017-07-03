using System;
using System.Collections.Concurrent;

namespace NetSteps.Encore.Core.Parallel
{
	/// <summary>
	/// Indicates kinds of results when dumultiplexing.
	/// </summary>
	public enum DemuxResultKind
	{
		/// <summary>
		/// None.
		/// </summary>
		None = 0,
		/// <summary>
		/// The result was observed. This indicates the current thread observed a result
		/// originated by another thread.
		/// </summary>
		Observed = 1,
		/// <summary>
		/// The result was originated by the current thread.
		/// </summary>
		Originated = 3,
	}
	/// <summary>
	/// Demultiplexes an operation.
	/// </summary>
	/// <typeparam name="A"></typeparam>
	/// <typeparam name="R"></typeparam>
	public abstract class DemuxProducer<A, R>
	{
		/// <summary>
		/// Default timeout period.
		/// </summary>
		public static readonly TimeSpan DefaultDemuxTimeout = TimeSpan.FromSeconds(30);
		/// <summary>
		/// Default number of timeout retries.
		/// </summary>
		public static readonly int DefaultMaxRetries = 3;

		struct DemuxRecord
		{
			public DemuxRecord(DemuxResultKind kind, R value)
			{
				Kind = kind;
				Value = value;
			}
			public DemuxResultKind Kind;
			public R Value;
		}
		readonly TimeSpan _demuxTimeout;
		readonly int _maxRetries;
		readonly ConcurrentDictionary<A, Future<DemuxRecord>> _concurrentActiviy = new ConcurrentDictionary<A, Future<DemuxRecord>>();

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		public DemuxProducer()
			: this(DefaultDemuxTimeout, DefaultMaxRetries)
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="demuxTimeout">a timeout period</param>
		public DemuxProducer(TimeSpan demuxTimeout)
			: this(demuxTimeout, DefaultMaxRetries)
		{
		}

		/// <summary>
		/// Creates a new instance.
		/// </summary>
		/// <param name="demuxTimeout">a timeout period</param>
		/// <param name="maxRetries">max timeout retries</param>
		public DemuxProducer(TimeSpan demuxTimeout, int maxRetries)
		{
			_demuxTimeout = demuxTimeout;
			_maxRetries = maxRetries;
		}

		/// <summary>
		/// Tries to demux a completion result.
		/// </summary>
		/// <param name="args"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public DemuxResultKind TryConsume(A args, out R value)
		{
			var retries = 0;
			while (true)
			{
				try
				{
					return DemuxTryConsume(args, out value);
				}
				catch (TimeoutException)
				{
					retries++;
					if (retries == _maxRetries)
					{
						throw;
					}
				}
			}
		}

		DemuxResultKind DemuxTryConsume(A args, out R value)
		{
			var originator = false;
			Future<DemuxRecord> future = null, capture = null;
			try
			{
				future = _concurrentActiviy.GetOrAdd(args, a =>
				{
					capture = new Future<DemuxRecord>();
					return capture;
				});

				originator = Object.ReferenceEquals(capture, future);
				if (originator)
				{
					if (ProduceResult(args, out value))
					{
						capture.MarkCompleted(new DemuxRecord(DemuxResultKind.Observed, value));
						return DemuxResultKind.Originated;
					}
					else
					{
						capture.MarkCompleted(new DemuxRecord(DemuxResultKind.None, value));
					}
				}
			}
			catch (Exception ex)
			{
				if (originator)
				{
					capture.MarkFaulted(ex);
					throw;
				}
			}
			finally
			{
				if (originator)
				{
					Future<DemuxRecord> unused;
					_concurrentActiviy.TryRemove(args, out unused);
				}
			}
			var record = future.AwaitValue(_demuxTimeout);
			value = record.Value;
			return record.Kind;
		}

		/// <summary>
		/// Produces
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		protected abstract bool ProduceResult(A arg, out R value);
	}

}
