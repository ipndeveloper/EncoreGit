using System;

namespace NetSteps.Common.Events
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Arg class to send Application progress message updates to listener.
	/// Created: 08-13-2010
	/// </summary>
	public class ProgressMessageEventArgs : EventArgs
	{
		public string Message { get; set; }
		public Constants.ApplicationMessageType ApplicationMessageType { get; set; }
        public ProgressEventArgs Progress { get; set; }

		public ProgressMessageEventArgs(string message, Constants.ApplicationMessageType applicationMessageType, ProgressEventArgs progress = null)
		{
			Message = message;
			ApplicationMessageType = applicationMessageType;
		    Progress = progress;
		}
	}
}
