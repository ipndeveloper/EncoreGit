using System;
using System.Data.SqlClient;

namespace NetSteps.Common.Base
{
	/// <summary>
	/// Author: John Egbert
	/// Description: Class to take a SqlDependency and custom action to perform when SqlDependency notifies of a change - JHE
	/// Created: 10-20-2010
	/// </summary>
	public class SqlDependencyChangeAction : IDisposable
	{
		#region Members
		private SqlDependency _sqlDependency;
		private Action _itemChanged;
		#endregion

		public SqlDependencyChangeAction(SqlDependency sqlDependency, Action itemChanged)
		{
			_itemChanged = itemChanged;
			_sqlDependency = sqlDependency;

			if (_sqlDependency != null)
			{
				_sqlDependency.OnChange -= SqlDependency_OnChange;
				_sqlDependency.OnChange += new System.Data.SqlClient.OnChangeEventHandler(SqlDependency_OnChange);
			}
		}

		protected void SqlDependency_OnChange(object sender, System.Data.SqlClient.SqlNotificationEventArgs e)
		{
			if (_itemChanged != null)
				_itemChanged();
            if (sender != null)
            {
                (sender as SqlDependency).OnChange -= SqlDependency_OnChange;
            }
		}

		#region IDisposable Members

		void IDisposable.Dispose()
		{
            if (_sqlDependency != null)
			    _sqlDependency.OnChange -= SqlDependency_OnChange;
			_sqlDependency = null;
			_itemChanged = null;
		}

		#endregion
	}
}
