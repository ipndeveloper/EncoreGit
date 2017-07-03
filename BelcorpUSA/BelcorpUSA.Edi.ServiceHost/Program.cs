using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using NetSteps.Diagnostics.Utilities;

namespace BelcorpUSA.Edi.ServiceHost
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		static void Main(string[] args = null)
		{
			try
			{
				new BelcorpUSAEdiServiceHost().RunService(args);
			}
			catch (Exception ex)
			{
				Tracer.Event(typeof(Program), new ExceptionTraceEvent(ex));
				throw;
			}
		}
	}
}
