using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Operations
{
	class LessThanOperation : IOperation
	{
		private readonly string _column;
		private readonly object _obj;
		private readonly bool _isEqualTo;

		public LessThanOperation(string column, object obj, bool isEqualTo)
		{
			_column = column;
			_obj = obj;
			_isEqualTo = isEqualTo;
		}

		public string GetQueryClause(int position)
		{
			var operation = _isEqualTo ? "<=" : "<";
			return string.Format("{0} {1} @{2}", _column, operation, String.Concat(_column, position));
		}

		public SqlParameter GetParameter(int position)
		{
			return new SqlParameter(String.Concat("@", _column, position), _obj);
		}

	}
}
