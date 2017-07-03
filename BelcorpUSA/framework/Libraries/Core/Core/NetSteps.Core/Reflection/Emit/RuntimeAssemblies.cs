﻿using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.IO;
using System.Reflection;
using System.Threading;

using System.Text;
using System.Diagnostics;

namespace NetSteps.Encore.Core.Reflection.Emit
{
	/// <summary>
	/// Utility class for emitting assemblies and tracking those assemblies so
	/// that type resolution works for the emitted types.
	/// </summary>
	public static class RuntimeAssemblies
	{		
		static readonly Lazy<EmittedAssembly> __dynamicAssembly = new Lazy<EmittedAssembly>(() =>
		{
			var now = DateTime.Now.ToString("O").Replace(':', '_');
			var current = Assembly.GetExecutingAssembly().GetName();
			var name = new AssemblyName(String.Concat(RuntimeAssembliesConfigSection.Current.DynamicAssemblyPrefix, now));
			name.Version = current.Version;
			name.CultureInfo = current.CultureInfo;

			var assem = new EmittedAssembly(name, name.Name);
			AppDomain.CurrentDomain.DomainUnload += new EventHandler(CurrentDomain_DomainUnload);
			return assem;
		}, LazyThreadSafetyMode.ExecutionAndPublication);

		static void CurrentDomain_DomainUnload(object sender, EventArgs e)
		{
			try
			{
#if DEBUG
                var saveAssembly = true;
#else
                var saveAssembly = RuntimeAssembliesConfigSection.Current.WriteAssembliesOnExit || WriteDynamicAssemblyOnExit;
#endif
				if (saveAssembly)
				{
					var assem = __dynamicAssembly.Value;
					assem.Compile();
					assem.Save();
				}
			}
			catch (Exception ex)
			{
                Trace.TraceWarning(String.Concat("Unexpected error while saving emitted assembly: ", ex.FormatForLogging(false)));
			}
		}

		/// <summary>
		/// Gets the dynamic assembly.
		/// </summary>
		public static EmittedAssembly DynamicAssembly { get { return __dynamicAssembly.Value; } }

        static Object __lock = new Object();
		static Dictionary<string, Assembly> __asm = new Dictionary<string, Assembly>();

		/// <summary>
		/// Indicates whether the dynamic assembly should be writen to disk upon exit.
		/// </summary>
		public static bool WriteDynamicAssemblyOnExit { get; set; }

		/// <summary>
		/// Prepares a type name.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="suffix"></param>
		/// <returns></returns>
		public static string PrepareTypeName(Type type, string suffix)
		{
			Contract.Requires<ArgumentNullException>(type != null);
			Contract.Requires<ArgumentNullException>(suffix != null);
			Contract.Requires<ArgumentNullException>(suffix.Length > 0);
			Contract.Ensures(Contract.Result<string>() != null);
						
			return String.Concat(type.GetEmittableFullName(), '$', suffix);
		}
				
		/// <summary>
		/// Generates an emittable full name for a type by name mangling.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static string GetEmittableFullName(this Type type)
		{
			Contract.Requires<ArgumentNullException>(type != null);

			string result;
			Type tt = (type.IsArray) ? type.GetElementType() : type;
			string simpleName = tt.Name;

			Contract.Assume(simpleName != null);
			Contract.Assert(simpleName.Length >= 0);

			var tick = simpleName.IndexOf('`');
			if (tick >= 0)
			{
				simpleName = simpleName.Substring(0, tick);
				var args = tt.GetGenericArguments();
				for (int i = 0; i < args.Length; i++)
				{
					if (i == 0)
						simpleName = String.Concat(simpleName, '<', args[i].GetEmittableFullName().Replace('.', '|'));
					else
						simpleName = String.Concat(simpleName, '`', args[i].GetEmittableFullName().Replace('.', '|'));
				}
				simpleName = String.Concat(simpleName, '>');
			}
			if (tt.IsNested)
			{
				result = String.Concat(tt.DeclaringType.GetReadableFullName(), "^", simpleName);
			}
			else
			{
				result = String.Concat(tt.Namespace, ".", simpleName);
			}
			return result;
		}

		[SuppressMessage("Microsoft.Performance", "CA1810")]
		static RuntimeAssemblies()
		{
			AppDomain.CurrentDomain.TypeResolve += new ResolveEventHandler(CurrentDomain_TypeResolve);
			AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
		}

		/// <summary>
		/// Gets an emitted assembly by name.
		/// </summary>
		/// <param name="name">the assembly's name</param>
		/// <returns>the assembly if it exists; otherwise null</returns>
		public static Assembly GetEmittedAssembly(string name)
		{
			Contract.Requires<ArgumentNullException>(name != null);

			Assembly asm;
			lock (__lock)
			{
				__asm.TryGetValue(name, out asm);
			}
			return asm;
		}

		/// <summary>
		/// Gets an emitted assembly by name; if it doesn't exist usess the emitter callback to 
		/// generate it.
		/// </summary>
		/// <param name="name">the assembly's name</param>
		/// <param name="emitter">a callback method that will emit the assembly</param>
		/// <returns>the assembly</returns>
		public static Assembly GetEmittedAssemblyWithEmitWhenNotFound(AssemblyName name, Action<EmittedAssembly> emitter)
		{
			Contract.Requires<ArgumentNullException>(name != null);
			Contract.Requires<ArgumentNullException>(emitter != null);

			Assembly asm;
            lock (__lock)
			{
				if (!__asm.TryGetValue(name.FullName, out asm))
				{
					EmittedAssembly emittedAsm = new EmittedAssembly(name, String.Empty);
					emitter(emittedAsm);
					asm = emittedAsm.Compile();
					__asm.Add(name.FullName, asm);
				}
			}
			return asm;
		}

		/// <summary>
		/// Creates an emitted assembly based on information taken from the target assembly.
		/// </summary>
		/// <param name="nameFormat">used to format the emitted assembly's name</param>
		/// <param name="target">the target assembly</param>
		/// <returns>an assembly name</returns>
		public static AssemblyName MakeEmittedAssemblyNameFromAssembly(string nameFormat, Assembly target)
		{
			Contract.Requires<ArgumentNullException>(nameFormat != null);
			Contract.Requires<ArgumentNullException>(target != null);

			AssemblyName tasmName = target.GetName();
			AssemblyName asmName = new AssemblyName(String.Format(nameFormat
					, tasmName.Name
					, tasmName.Version
					, Assembly.GetCallingAssembly().GetName().Version).Replace('.', '_')
					);
			asmName.Version = tasmName.Version;
			asmName.CultureInfo = tasmName.CultureInfo;
			return asmName;
		}

		static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
		{
			if (__dynamicAssembly.IsValueCreated)
			{
				var asm = __dynamicAssembly.Value;
				var reqName = new AssemblyName(args.Name);
				if (reqName.Name.StartsWith(RuntimeAssembliesConfigSection.Current.DynamicAssemblyPrefix)
					&& reqName.Version == asm.Name.Version)
				{
					return asm.Builder;
				}
			}
			return null;
		}
		static Assembly CurrentDomain_TypeResolve(object sender, ResolveEventArgs args)
		{
			return null;
		}    
	}
}