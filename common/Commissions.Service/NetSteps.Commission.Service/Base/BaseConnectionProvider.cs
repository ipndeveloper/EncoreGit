using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Diagnostics.Utilities;

namespace NetSteps.Commissions.Service.Base
{
	public abstract class BaseConnectionProvider : IConnectionProvider
	{
		public abstract System.Data.SqlClient.SqlConnection GetConnection();

		public virtual bool CanConnect()
		{
			bool result = false;
			try
			{
				using (var conn = GetConnection())
				{
					conn.Open();
				}
				result = true;
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
			}
			return result;
		}
	}
}
