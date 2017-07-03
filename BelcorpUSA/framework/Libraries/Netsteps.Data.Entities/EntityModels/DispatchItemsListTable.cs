using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.EntityModels;
using System.Data.SqlClient;
using System.Data;
using NetSteps.Data.Entities.Exceptions;


namespace NetSteps.Data.Entities.EntityModels
{
        public class DispatchsItemsListTable
        {
            public int DispatchListID { get; set; }
            public int AccountID { get; set; }
            public string Name { get; set; }
            public string AccountNumber { get; set; }
        }

        public class DispatchsItemsParameters
        {
            public int DispatchListID { get; set; }
            public string AccountNumber { get; set; }
            public string Name { get; set; }
        }

        public class DispatchItemsListQuery
        {
            public int DispatchListID { get; set; }
            public int AccountID { get; set; }
            public string Name { get; set; }
         }
}
