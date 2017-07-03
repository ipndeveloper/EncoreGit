using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Diagnostics.Utilities;

namespace WCFAPI.App_Code
{
	public class Init
	{
		public static void AppInitialize()
		{
			try
			{
				NetSteps.Encore.Core.Wireup.WireupCoordinator.SelfConfigure();
			}
			catch (System.Reflection.ReflectionTypeLoadException ex)
			{
				Type t = typeof(Init);
				Tracer.Event(t, new ExceptionTraceEvent(ex));
				foreach (var lex in ex.LoaderExceptions)
				{
					Tracer.Event(t, new ExceptionTraceEvent(lex));
				}
				throw ex.LoaderExceptions[0];
			}
		}
	}
}