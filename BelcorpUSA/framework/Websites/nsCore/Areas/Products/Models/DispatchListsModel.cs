using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities.EntityModels;

namespace nsCore.Areas.Products.Models
{
    public class DispatchListsModel
    {
        public Int32 DispatchListID { get; set; }
        public Int32 Editable { get; set; }
        public String Name { get; set; }
        public Int32? MarketID { get; set; }
        public List<DispatchsItemsListTable> Accounts { get; set; }
        public List<DispatchItemsListQuery> AccountsQuery { get; set; }
        
        public DispatchListsModel(DispatchListsTable entidad)
        {
            DispatchListID = entidad.DispatchListID;
            Name = entidad.Name;
            Accounts = entidad.Accounts;
            AccountsQuery = entidad.AccountsQuery;
        }
    }
}