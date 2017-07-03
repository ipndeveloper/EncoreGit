using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace NetSteps.Commissions.Service.Operations
{
    class EqualsOperation : IOperation
    {
        private readonly string _column;
        private readonly object _obj;

        public EqualsOperation(string column, object obj)
        {
            _column = column;
            _obj = obj;
        }

        public string GetQueryClause(int position)
        {
            return string.Format("{0} = @{1}", _column, String.Concat(_column, position));
        }

		public SqlParameter GetParameter(int position)
        {
            return new SqlParameter(String.Concat("@", _column, position), _obj);
        }
    }
}
