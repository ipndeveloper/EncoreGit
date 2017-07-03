using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestMasterHelpProvider.Logging.Sql
{
	public interface ISqlLoggable
	{
		/// <summary>
		/// Serializes the object to the database.
		/// </summary>
		void SerializeToDatabase();
	}
}
