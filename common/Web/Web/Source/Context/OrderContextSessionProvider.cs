using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using NetSteps.Data.Common.Context;
using NetSteps.Encore.Core.IoC;

namespace NetSteps.Web
{
	public static class OrderContextSessionProvider
	{
		/// <summary>
		/// The key name of the <see cref="IOrderContext"/> object stored in session.
		/// </summary>
		private const string _orderContextSessionKey = "_orderContext";

		/// <summary>
		/// Returns the <see cref="IOrderContext"/> object from session (creating it if necessary).
		/// </summary>
		public static IOrderContext Get(HttpSessionStateBase session)
		{
			Contract.Requires<ArgumentNullException>(session != null);

			var orderContext = session[_orderContextSessionKey] as IOrderContext;
			if (orderContext == null)
			{
				orderContext = Create.New<IOrderContext>();
				session[_orderContextSessionKey] = orderContext;
			}
			return orderContext;
		}

		/// <summary>
		/// Returns the <see cref="IOrderContext"/> object from session (creating it if necessary).
		/// </summary>
		public static IOrderContext Get(HttpSessionState session)
		{
			Contract.Requires<ArgumentNullException>(session != null);

			return Get(new HttpSessionStateWrapper(session));
		}

		/// <summary>
		/// Sets the <see cref="IOrderContext"/> object.
		/// </summary>
		public static void Set(HttpSessionStateBase session, IOrderContext context)
		{
			Contract.Requires<ArgumentNullException>(session != null);

			session[_orderContextSessionKey] = context;
		}

		/// <summary>
		/// Sets the <see cref="IOrderContext"/> object.
		/// </summary>
		public static void Set(HttpSessionState session, IOrderContext context)
		{
			Contract.Requires<ArgumentNullException>(session != null);

			Set(new HttpSessionStateWrapper(session), context);
		}

		public static void Clear(HttpSessionStateBase session)
		{
			Contract.Requires<ArgumentNullException>(session != null);

			session[_orderContextSessionKey] = null;
		}

		public static void Clear(HttpSessionState session)
		{
			Contract.Requires<ArgumentNullException>(session != null);

			Clear(new HttpSessionStateWrapper(session));
		}


	}
}
