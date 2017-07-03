using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Orders.Models.Shipping
{
    public class IndexModel
    {
        public IEnumerable<GridColumn> GridColumns { get; set; }

        public IEnumerable<SelectListItem> OrderTypes
        {
            get
            {
                return SmallCollectionCache.Instance.OrderTypes
                    .Where(x => x.Active && x.TermName != "ReturnOrder")
                    .Select(x => new SelectListItem
                    {
                        Text = x.GetTerm(),
                        Value = x.OrderTypeID.ToString()
                    })
                    .OrderBy(x => x.Text);
            }
        }
        public IEnumerable<SelectListItem> OrderStatuses
        {
            get
            {
                return SmallCollectionCache.Instance.OrderStatuses
                    .Where(x => x.Active)
                    .Select(x => new SelectListItem
                    {
                        Text = x.GetTerm(),
                        Value = x.OrderStatusID.ToString()
                    })
                    .OrderBy(x => x.Text);
            }
        }
        public IEnumerable<SelectListItem> OrderShipmentStatuses
        {
            get
            {
                return SmallCollectionCache.Instance.OrderShipmentStatuses
                    .Where(x => x.Active)
                    .Select(x => new SelectListItem
                    {
                        Text = x.GetTerm(),
                        Value = x.OrderShipmentStatusID.ToString()
                    })
                    .OrderBy(x => x.Text);
            }
        }

        public IndexModel()
        {
            GridColumns = new[]
            {
                new GridColumn { Name = "Tracking Number", TermName = "TrackingNumber" },
                new GridColumn { Name = "Date Shipped", TermName = "DateShipped" },
                new GridColumn { Name = "Order Number", TermName = "OrderNumber", OrderBy = "OrderNumber" },
                new GridColumn { Name = "Package", TermName = "Package" },
                new GridColumn { Name = "First Name", TermName = "FirstName" },
                new GridColumn { Name = "Last Name", TermName = "LastName" },
                new GridColumn { Name = "Type", TermName = "Type", OrderBy = "OrderType" },
                new GridColumn { Name = "Order Status", TermName = "OrderStatus", OrderBy = "OrderStatus" },
                new GridColumn { Name = "Shipment Status", TermName = "ShipmentStatus" },
                new GridColumn { Name = "Complete Date", TermName = "CompleteDate", OrderBy = "CompleteDateUTC" },
                new GridColumn { Name = "Consultant", TermName = "Consultant", OrderBy = "Consultant.FullName" }
            };
        }

        public class GridColumn
        {
            public string Name { get; set; }
            public string TermName { get; set; }
            public string OrderBy { get; set; }
        }
    }
}