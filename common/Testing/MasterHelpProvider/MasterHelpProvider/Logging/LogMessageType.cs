using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Logging
{
	/// <summary>
	/// Use this enum to define what message to write to the log.
	/// </summary>
	public enum LogMessageType
	{
		Debug = 1,
		Exception,
		Pass,
		Fail,
        Info,
		TestStart,
		TestEnd
	}
}
