using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NetSteps.Data.Entities.EntityModels;
//using System.Web.Mvc.HtmlH;

namespace nsCore.Areas.Products.Models
{
    public class DispatchModel
    {
        public Int32 DispatchID { get; set; }
        public Int32 DispatchTypeID { get; set; }
        public Int32 DispatchStatusType { get; set; }
        public String Description { get; set; }
        public Int32? PeriodStart { get; set; }
        public Int32? PeriodEnd { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public Int32 OnlyTime { get; set; }
        public Int32? ListScope { get; set; }
        public Int32 Status { get; set; }
        public String Termname { get; set; }
        public Int32 SortIndex { get; set; }
        public bool Editable { get; set; }
        public List<DispatchItemsTable> Products { get; set; }
        public List<DispatchItemsQuery> ProductsQuery { get; set; }

        public DispatchModel(DispatchTable entidad)
        {
            DispatchID = entidad.DispatchID;
            DispatchTypeID = entidad.DispatchTypeID;
            DispatchStatusType = entidad.DispatchStatusType;
            Description = entidad.Description;
            PeriodStart = entidad.PeriodStart;
            PeriodEnd = entidad.PeriodEnd;
            DateStart = entidad.DateStart;
            DateEnd = entidad.DateEnd;
            OnlyTime = entidad.OnlyTime;
            ListScope = entidad.ListScope;
            Status = entidad.Status;
            Termname = entidad.Termname;
            SortIndex = entidad.SortIndex;
            Editable = entidad.Editable;
            Products = entidad.Products;
            ProductsQuery = entidad.ProductsQuery;
        }
    }
 
}