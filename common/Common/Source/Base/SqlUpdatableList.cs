using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace NetSteps.Common.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class returned from Data Layer with List of Entities and SqlDependency to notify of changes to those Entities - JHE
	/// Created: 08-18-2010
	/// </summary>
	[Serializable]
	public class SqlUpdatableList<T> : List<T>
	{
		public SqlDependency SqlDependency { get; set; }
	}
}
