using System;
using System.Data.SqlClient;

namespace NetSteps.Common.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class returned from Data Layer with Entities and SqlDependency to notify of changes to that Entity - JHE
	/// Created: 10-05-2010
	/// </summary>
	[Serializable]
	public class SqlUpdatableItem<T>
	{
		public SqlDependency SqlDependency { get; set; }
		public T Item { get; set; }
	}
}
