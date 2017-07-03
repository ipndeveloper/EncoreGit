using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using NetSteps.Encore.Core.IoC.Containers;
using NetSteps.Encore.Core.Wireup;

namespace NetSteps.Encore.Core.IoC
{
	/// <summary>
	/// Utility class for working with containers.
	/// </summary>
	public static class Container
	{
		static readonly string NetSteps_LogicalRoot_Container = "NetSteps_LogicalRoot_Container";

		[ThreadStatic]
		static Stack<IContainer> __current;

		/// <summary>
		/// Gets the container assigned to the current thread.
		/// </summary>
		public static IContainer Current
		{
			get
			{
				Contract.Ensures(Contract.Result<IContainer>() != null);

				var stack = __current;
				if (stack != null && stack.Count > 0)
				{
					return stack.Peek();
				}
				return Root;
			}
		}

		internal static void PushCurrent(IContainer c)
		{
			if (__current == null)
			{
				__current = new Stack<IContainer>();
			}
			__current.Push(c);
		}

		internal static void PopCurrentIfEquals(IContainer c)
		{
			var stack = __current;
			if (stack != null && stack.Count > 0)
			{
				if (stack.Peek() == c)
					stack.Pop();
			}
		}

		/// <summary>
		/// Identifies the current tenant's container as the logical root container.
		/// </summary>
		public static void IdentifyTenantAsLogicalRoot()
		{
			object tenantid;
			if (Root.TryResolveTenant(out tenantid))
			{
				CallContext.LogicalSetData(NetSteps_LogicalRoot_Container, tenantid);
			}
		}

		/// <summary>
		/// Gets the logical root container.
		/// </summary>
		public static IContainer LogicalRoot
		{
			get
			{
				var tenantid = CallContext.LogicalGetData(NetSteps_LogicalRoot_Container);
				if (tenantid != null)
				{
					return Root.ResolveTenantByID(tenantid);
				}
				return __root.Value;
			}
		}

		/// <summary>
		/// Gets the root container.
		/// </summary>
		public static IRootContainer Root
		{
			get
			{
				var root = __root.Value;
				if (!__initialized)
				{
					__initialized = true;
					WireupCoordinator.Instance.WireupDependencies(Assembly.GetExecutingAssembly());
					AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);
				}
				return root;
			}
		}

		static void CurrentDomain_ProcessExit(object sender, EventArgs e)
		{
			if (__root.IsValueCreated)
			{
				var root = __root.Value;
				// Restore the ambient scope from the root container;
				// without this the root cannot clean up its scope
				// because scopes are nested (per-thread) and must be
				// cleaned up in reverse order.
				CleanupScope.EnsureAmbient(root.Scope);
				root.Dispose();
			}
		}

		static bool __initialized;
		static readonly Lazy<IRootContainer> __root = new Lazy<IRootContainer>(() => new RootContainer(), LazyThreadSafetyMode.ExecutionAndPublication);
	}
}
