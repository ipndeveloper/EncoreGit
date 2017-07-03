
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Threading;
using NetSteps.Encore.Core.Parallel;
using NetSteps.Encore.Core.Properties;
using NetSteps.Encore.Core.Reflection;

namespace NetSteps.Encore.Core.Log
{
	internal sealed class LogSinkManager : ILogSinkManager, ILogSinkGhostWriter
	{
		struct DelegatedLogEventWriteRecord
		{
			public DelegatedLogEventWriteRecord(LogEventWriter writer, LogEvent evt)
			{
				Writer = writer;
				Event = evt;
			}
			internal LogEventWriter Writer;
			internal LogEvent Event;
		}
		class LogSinkReactor : Reactor<DelegatedLogEventWriteRecord>
		{
			public LogSinkReactor(Action<Reactor<DelegatedLogEventWriteRecord>, DelegatedLogEventWriteRecord> reactor, ReactorOptions options)
				: base(reactor, options)
			{
				Contract.Requires<ArgumentNullException>(reactor != null);
			}

			protected override bool AllowLogEvent(System.Diagnostics.SourceLevels levels)
			{
				// This filter eliminates the reactor from flooding the log with
				// background worker begin/end messages when the logging level is
				// configured to be verbose.
				return levels.HasFlag(SourceLevels.Warning);
			}
		}

		readonly ConcurrentDictionary<string, ILogSink> _logSinks = new ConcurrentDictionary<string, ILogSink>();
		readonly Reactor<DelegatedLogEventWriteRecord> _reactor;
		ILogSink _default;

		static LogSinkManager __singleton;
		public static LogSinkManager Singleton
		{
			get { return Util.NonBlockingLazyInitializeVolatile(ref __singleton, () => new LogSinkManager()); }
		}

		public ILogSink DefaultLogSink
		{
			get
			{
				return Util.NonBlockingLazyInitializeVolatile(ref _default, () =>
				{
					var config = LogConfigurationSection.Current;
					var writer = config.ResolvedDefaultLogWriter;
					return new LogSink(this
							, "default"
							, config.DefaultSourceLevel
							, config.DefaultStackTraceThreshold
							, writer
							, null);
				});
			}
		}

		internal LogSinkManager()
		{
			_reactor = new LogSinkReactor(Bg_WriteLogEvent, new ReactorOptions(
				 ReactorOptions.DefaultMaxDegreeOfParallelism,
				 false,
				 0,
				 LogConfigurationSection.Current.ParallelDispatchThreshold,
				 ReactorOptions.DefaultDispatchesPerBorrowedThread
			));
			_reactor.UncaughtException += new EventHandler<ReactorExceptionArgs>(_reactor_UncaughtException);

		}

		void Bg_WriteLogEvent(Reactor<DelegatedLogEventWriteRecord> reactor, DelegatedLogEventWriteRecord rec)
		{
			rec.Writer.WriteLogEvent(rec.Event);
		}

		void _reactor_UncaughtException(object sender, ReactorExceptionArgs e)
		{
			// Since we're already logging, eat the exception.
		}

		public ILogSink GetLogSinkForType(Type type)
		{
			Contract.Assert(type != null, Resources.Chk_CannotBeNull);

			string key = type.GetLogSourceName();
			ILogSink result = null;

			while (result == null)
			{
				if (_logSinks.TryGetValue(key, out result))
				{
					return result;
				}
				else
				{
					result = GetFallbackLogSink(key, type.GetReadableFullName());
				}
			}

			return result;
		}

		private ILogSink GetFallbackLogSink(string key, string namesp)
		{
			Contract.Requires<ArgumentNullException>(key != null);
			Contract.Requires<ArgumentNullException>(namesp != null);
			Contract.Requires<ArgumentException>(namesp.Length >= 0);

			var config = LogConfigurationSection.Current;

			lock (key.MakeReliableLockFromString())
			{
				ILogSink result = null;
				Stack<string> namespaces = new Stack<string>();
				try
				{
					while (result == null && SliceNamespace(ref namesp))
					{
						namespaces.Push(namesp);
						Monitor.Enter(namesp.MakeReliableLockFromString());
						if (_logSinks.TryGetValue(namesp, out result))
						{
							break;
						}
						else
						{ // attempt to load from config...
							var c = config.Namespaces[namesp];
							if (c != null)
							{
								var level = (c.IsSpecialized) ? c.SourceLevel : config.DefaultSourceLevel;
								var thresh = (c.IsSpecialized) ? c.StackTraceThreshold : config.DefaultStackTraceThreshold;
								if (!String.IsNullOrEmpty(c.WriterName))
								{
									// TODO: resolve log writer by name
								}
								else if (!String.IsNullOrEmpty(c.WriterTypeName))
								{
									var writer = (LogEventWriter)Activator.CreateInstance(c.ResolvedWriterType);
									writer.Initialize(namesp);
									result = new LogSink(this, namesp, level, thresh, writer, null);
								}
								else
								{
									var writer = (LogEventWriter)Activator.CreateInstance(config.ResolvedDefaultWriterType);
									writer.Initialize(namesp);
									result = new LogSink(this, namesp, level, thresh, writer, null);
								}

								break;
							}
						}
					}

					if (result == null)
					{
						var def = DefaultLogSink;
						result = new LogSink(this, namesp, def.Levels, def.StackTraceThreshold, LogEventWriter.NullWriter, def);
					}
					while (namespaces.Count > 0)
					{
						var k = namespaces.Pop();
						_logSinks.TryAdd(k, result);
						Monitor.Exit(k.MakeReliableLockFromString());
					}
				}
				finally
				{
					try
					{
						while (namespaces.Count > 0)
						{
							Monitor.Exit(namespaces.Pop().MakeReliableLockFromString());
						}
					}
					catch { }
				}

				_logSinks.TryAdd(key, result);
				return result;
			}
		}

		private bool SliceNamespace(ref string namesp)
		{
			Contract.Requires<ArgumentNullException>(namesp != null);
			var sliceAt = namesp.LastIndexOf('.');
			if (sliceAt >= 0)
			{
				// namespaces are interned to make the locking strategy work
				// across time/threads
				namesp = namesp.Substring(0, sliceAt).InternIt();
			}
			return sliceAt >= 0;
		}

		public void GhostWrite(LogEventWriter writer, LogEvent evt)
		{
			_reactor.Push(new DelegatedLogEventWriteRecord(writer, evt));
		}
	}
}
