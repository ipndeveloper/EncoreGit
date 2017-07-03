using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading;
using NetSteps.Encore.Core.Properties;
using NetSteps.Encore.Core.Wireup.Meta;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace NetSteps.Encore.Core.Wireup
{
	internal sealed class DefaultWireupCoordinator : IWireupCoordinator
	{
		private static Regex[] __ignoredRegexNamespaces = new Regex[] { 
			new Regex("^mscorlib,.*", RegexOptions.Compiled), 
			new Regex("^System,.*", RegexOptions.Compiled), 
			new Regex(@"^System\..*", RegexOptions.Compiled), 
			new Regex(@"^Microsoft\..*", RegexOptions.Compiled),
			new Regex(@"^Newtonsoft\..*", RegexOptions.Compiled), 
			new Regex(@".*\.Contracts,.*", RegexOptions.Compiled) };

		public IEnumerable<AssemblyDependency> WireupDependencies(Assembly asm)
		{
			Contract.Assert(asm != null);

			return PerformWireupDependencies(asm);
		}

		IEnumerable<AssemblyDependency> PerformWireupDependencies(Assembly asm)
		{
			Contract.Assert(asm != null);

			var myDeps = new List<AssemblyDependency>();

			if (!__ignoredRegexNamespaces.Any(r => r.IsMatch(asm.FullName)))
			{
				lock (_lock)
				{
					// The stacks are used to avoid cycles among the dependency declarations.
					if (!IsAssemblyWired(asm) && !_assemblies.Contains(asm))
					{
						_assemblies.Push(asm);
						try
						{
							var deps = new List<WireupDependencyAttribute>();
							var tasks = new List<WireupTask>();
							var commands = new List<WireupAttribute>();

							AddDependencies(asm, null, myDeps, deps, asm.GetCustomAttributes(typeof(WireupDependencyAttribute), false));
							AddTasks(asm, null, myDeps, tasks, asm.GetCustomAttributes(typeof(WireupTaskAttribute), false));

							// Assemblies may have more than one module.
							foreach (var mod in asm.GetModules(false))
							{
								commands.AddRange(mod.GetCustomAttributes(typeof(WireupAttribute), false).Cast<WireupAttribute>());

								AddDependencies(asm, null, myDeps, deps, mod.GetCustomAttributes(typeof(WireupDependencyAttribute), false));
								AddTasks(asm, null, myDeps, tasks, mod.GetCustomAttributes(typeof(WireupTaskAttribute), false));

								foreach (var type in mod.GetTypes())
								{
									AddTasks(asm, type, myDeps, tasks, type.GetCustomAttributes(typeof(WireupTaskAttribute), false));
								}
							}

							// Phase: BeforeDependencies
							ProcessPhase(deps, tasks, asm, myDeps, WireupPhase.BeforeDependencies);

							// Phase: Dependencies
							ProcessPhase(deps, tasks, asm, myDeps, WireupPhase.Dependencies);

							// Phase: BeforeTasks
							ProcessPhase(deps, tasks, asm, myDeps, WireupPhase.BeforeTasks);

							// Phase: Tasks
							ProcessPhase(deps, tasks, asm, myDeps, WireupPhase.Tasks);

							// Phase: BeforeWireup
							ProcessPhase(deps, tasks, asm, myDeps, WireupPhase.BeforeWireup);

							// Execute the wireup commands declared for the assembly...
							commands.AddRange(asm.GetCustomAttributes(typeof(WireupAttribute), false).Cast<WireupAttribute>());
							foreach (WireupAttribute w in commands)
							{
								foreach (Type t in w.CommandType)
								{
									if (!_types.Contains(t))
										InvokeWireupCommand(t);
								}
							}

							// Phase: AfterWireup
							ProcessPhase(deps, tasks, asm, myDeps, WireupPhase.AfterWireup);

							if (!IsAssemblyWired(asm))
							{
								_asmTracking.TryAdd(asm.GetKeyForAssembly(), myDeps);
							}
						}
						finally
						{
							_assemblies.Pop();
						}
					}
				} 
			}
			return myDeps.ToReadOnly();
		}

		private void AddDependencies(Assembly asm, Type target, List<AssemblyDependency> deps, List<WireupDependencyAttribute> attrs, object[] dependencies)
		{
			foreach (var dep in dependencies.Cast<WireupDependencyAttribute>())
			{
				if (dep.Phase == WireupPhase.Immediate)
				{
					ProcessDependencyTarget(asm, dep.TargetType, deps);
				}
				else
				{
					attrs.Add(dep);
				}
			}
		}

		private void AddTasks(Assembly asm, Type target, List<AssemblyDependency> deps, List<WireupTask> attrs, object[] dependencies)
		{
			foreach (var task in dependencies.Cast<WireupTaskAttribute>())
			{
				if (task.Phase == WireupPhase.Immediate)
				{
					ProcessTask(asm, new WireupTask(target, task));
				}
				else
				{
					attrs.Add(new WireupTask(target, task));
				}
			}
		}

		private void ProcessPhase(List<WireupDependencyAttribute> deps, List<WireupTask> tasks, Assembly asm, List<AssemblyDependency> myDeps, WireupPhase wireupPhase)
		{
			foreach (var d in deps.Where(d => d.Phase == wireupPhase))
			{
				ProcessDependencyTarget(asm, d.TargetType, myDeps);
			}
			foreach (var t in tasks.Where(t => t.Task.Phase == wireupPhase))
			{
				ProcessTask(asm, t);
			}
		}

		private void ProcessDependencyTarget(Assembly asm, Type type, List<AssemblyDependency> myDeps)
		{
			if (!_types.Contains(type))
			{
				var asmName = asm.GetName();
				var dep = new AssemblyDependency(asmName.Name, String.Concat("v", asmName.Version.ToString()));
				if (!myDeps.Contains(dep))
				{
					myDeps.Add(dep);
					myDeps.AddRange(PerformWireupDependencies(type.Assembly));
				}
				InvokeWireupCommand(type);
			}
		}

		private void ProcessTask(Assembly asm, WireupTask task)
		{
			task.Task.ExecuteTask(this);
			foreach (var observer in _observers.Values)
			{
				observer.NotifyWireupTask(this, task.Task, task.Target);
			}
		}

		public void WireupDependency(Type type)
		{
			if (type == null) throw new ArgumentNullException("type");
			if (!typeof(IWireupCommand).IsAssignableFrom(type))
				throw new ArgumentException(Resources.Chk_TypeMustBeAssignableToIWireupCommand, "type");

			if (!_types.Contains(type))
				InvokeWireupCommand(type);
		}

		class CommandTracking
		{
			int _dependencyCount = 1;
			internal CommandTracking(string typeName)
			{
				this.TypeName = typeName;
			}
			public string TypeName { get; private set; }
			public int Increment()
			{
				return Interlocked.Increment(ref _dependencyCount);
			}
		}

		class WireupTask
		{
			internal WireupTask(Type target, WireupTaskAttribute attr)
			{
				this.Target = target;
				this.Task = attr;
			}

			public Type Target { get; private set; }
			public WireupTaskAttribute Task { get; private set; }
		}

		readonly Object _lock = new Object();
		readonly Stack<Assembly> _assemblies = new Stack<Assembly>();
		readonly Stack<Type> _types = new Stack<Type>();
		readonly ConcurrentDictionary<String, CommandTracking> _wired = new ConcurrentDictionary<string, CommandTracking>();
		readonly ConcurrentDictionary<String, IEnumerable<AssemblyDependency>> _asmTracking = new ConcurrentDictionary<String, IEnumerable<AssemblyDependency>>();

		void InvokeWireupCommand(Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Assume(_types != null);
			Contract.Assume(_asmTracking != null);

			if (!_types.Contains(type))
			{
				_types.Push(type);
				try
				{
					// Assemblies may have dependencies declared; wire them first.
					foreach (WireupDependencyAttribute d in type.GetCustomAttributes(typeof(WireupDependencyAttribute), false))
					{
						Type r = d.TargetType;
						WireupDependencies(r.Assembly);
					}
					var key = type.GetKeyForType();
					CommandTracking ours = null;
					var tracking = _wired.GetOrAdd(key, k =>
					{
						ours = new CommandTracking(k);
						return ours;
					});
					tracking.Increment();
					if (Object.ReferenceEquals(ours, tracking))
					{
						var cmd = (IWireupCommand)Activator.CreateInstance(type);
						cmd.Execute(this);
					}
				}
				finally
				{
					_types.Pop();
				}
			}
		}

		bool IsAssemblyWired(Assembly asm)
		{
			Contract.Requires<ArgumentNullException>(asm != null);
			Contract.Assume(_asmTracking != null);
			return _asmTracking.ContainsKey(asm.GetKeyForAssembly());
		}

		public IEnumerable<AssemblyDependency> ExposeDependenciesFor(Assembly assem)
		{
			IEnumerable<AssemblyDependency> deps;
			if (_asmTracking.TryGetValue(assem.GetKeyForAssembly(), out deps))
			{
				return deps.ToReadOnly();
			}
			return Enumerable.Empty<AssemblyDependency>();
		}

		ConcurrentDictionary<Guid, IWireupObserver> _observers = new ConcurrentDictionary<Guid, IWireupObserver>();

		public void RegisterObserver(IWireupObserver observer)
		{
			Contract.Requires<ArgumentNullException>(observer != null);

			_observers.TryAdd(observer.ObserverKey, observer);
		}

		public void UnregisterObserver(Guid key)
		{
			IWireupObserver observer;
			_observers.TryRemove(key, out observer);
		}

		public void NotifyAssemblyLoaded(Assembly assembly)
		{
			if (!__ignoredRegexNamespaces.Any(r => r.IsMatch(assembly.FullName)))
			{
				lock (_lock)
				{
					WireupDependencies(assembly);
				}
			}
			else
			{
				Trace.TraceInformation("Wireup Ignoring assembly '{0}'", assembly.FullName);
			}
		}
	}

}
