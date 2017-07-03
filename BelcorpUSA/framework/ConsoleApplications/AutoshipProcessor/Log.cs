using Microsoft.Practices.EnterpriseLibrary.Logging;
using NetSteps.Diagnostics.Utilities;
using System;

namespace AutoshipProcessor
{
	public static class Log
	{
		private static Type thisT = typeof(Log);
		public static void Debug(string msg)
		{
			Logger.Write(msg, "Debug");
			thisT.TraceVerbose(msg);
		}
		public static void Debug(string format, params object[] args)
		{
			Debug(string.Format(format, args));
		}
		public static void Info(string msg)
		{
			Logger.Write(msg, "Info");
			thisT.TraceInformation(msg);
		}
		public static void Info(string format, params object[] args)
		{
			Info(string.Format(format, args));
		}
		public static void Warning(string msg)
		{
			Logger.Write(msg, "Warning");
			thisT.TraceWarning(msg);
		}
		public static void Warning(string format, params object[] args)
		{
			Warning(string.Format(format, args));
		}
		public static void Error(string msg)
		{
			Logger.Write(msg, "Error");
			thisT.TraceError(msg);
		}
		public static void Error(string format, params object[] args)
		{
			Error(string.Format(format, args));
		}
	}
}
