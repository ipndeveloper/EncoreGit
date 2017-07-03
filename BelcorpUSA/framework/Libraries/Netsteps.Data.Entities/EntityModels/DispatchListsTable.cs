using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetSteps.Data.Entities.EntityModels;

namespace NetSteps.Data.Entities.EntityModels
{
    public class DispatchListsTable
    {
        public Int32 DispatchListID { get; set; }
        public Int32 Editable { get; set; }
        public String Name { get; set; }
        public Int32? MarketID { get; set; }
        public List<DispatchsItemsListTable> Accounts { get; set; }
        public List<DispatchItemsListQuery> AccountsQuery { get; set; }
    }

    public class DispatchListsSearchData
    {
        public int DispatchListID { get; set; }
        public string Name { get; set; }
        public string MarketName { get; set; }
        public string Mercado { get; set; }
    }
}
