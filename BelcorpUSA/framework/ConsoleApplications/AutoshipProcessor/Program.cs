using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using NetSteps.Common.Configuration;
using NetSteps.Common.Events;
using NetSteps.Common.Extensions;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Processors;
using NetSteps.Encore.Core.Wireup;
using NetSteps.Diagnostics.Utilities;

namespace AutoshipProcessor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			try
			{
				new AutoshipProcessorServiceHost(args).RunService(args);
			}
			catch (Exception ex)
			{
				Tracer.Event(typeof(Program), new ExceptionTraceEvent(ex));
				throw;
			}
		}
	}
}