using System;
using System.Diagnostics;

namespace NetSteps.Common.Utility
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class to get the details of the current ExecutionContext in an application.
	/// Used to log usage statics to the database.
	/// Created: 03-11-2010
	/// Example Usage:
	///     ExceptionHandledDataAction.Run(new ExecutionContext(this), () =>
	///     {
	///         // Code to RUN goes here. 
	///     });
	/// </summary>
	public class ExecutionContext
	{
		#region Properties
		public short ApplicationID { get; set; }
		public int UserID { get; set; }
		public string AssemblyName { get; set; }
		public string MachineName { get; set; }
		public string Namespace { get; set; }
		public string ClassName { get; set; }
		public string MethodName { get; set; }
		#endregion

		public ExecutionContext(object executingObject)
		{
			// To increase performance when logging is turned off - JHE
            if (ApplicationContextCommon.Instance.EnableApplicationUsageLogging)
			{
                ApplicationID = ApplicationContextCommon.Instance.ApplicationID;
                UserID = ApplicationContextCommon.Instance.CurrentUserID;
				MachineName = Environment.MachineName;

				Type type = executingObject.GetType();
				AssemblyName = type.Assembly.FullName;
				Namespace = type.Namespace;
				ClassName = type.Name;

				// Get the caller's method name.
				var stack = new StackTrace(1, false);
                this.MethodName = stack.GetFrame(0).GetMethod().Name;
			}
		}
	}
}
