using System;
using System.Collections.Generic;
using System.Linq;
using NetSteps.Common.Base;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Extensions;

namespace nsCore.Areas.Orders.Models.Shipping
{
    public class GetPackagesModel : PackagesModel
    {
        public int PageIndex { get; set; }
        public int TotalPages { get; set; }

        public GetPackagesModel LoadResources(
            IPaginatedList<OrderShippingSearchData> orders,
            IEnumerable<string> errors,
            IFormatProvider formatProvider)
        {
            PageIndex = orders.PageIndex;
            TotalPages = orders.TotalPages;

            base.LoadResources(orders, errors, formatProvider);
                
            return this;
        }
    }

    public class PackagesModel
    {
        public IEnumerable<PackageModel> Packages { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public PackagesModel LoadResources(
            IList<OrderShippingSearchData> orders,
            IEnumerable<string> errors,
            IFormatProvider formatProvider)
        {
            Errors = errors;

            var packageList = new List<PackageModel>();
            foreach (var order in orders)
            {
                if (order.OrderShipments.Any())
                {
                    foreach (var orderShipment in order.OrderShipments)
                    {
                        if (orderShipment.OrderShipmentPackages == null || !orderShipment.OrderShipmentPackages.Any())
                        {
                            // No packages, add a single row
                            packageList.Add(new PackageModel().LoadResources(order, orderShipment, formatProvider));
                        }
                        else
                        {
                            // One or more packages, add one row per package
                            packageList.AddRange(
                                orderShipment.OrderShipmentPackages.Select(x => new PackageModel()
                                    .LoadResources(order, orderShipment, formatProvider, x.OrderShipmentPackageID)
                                )
                            );
                        }
                    }
                }
                else
                {
                    packageList.Add(new PackageModel().LoadResources(order, formatProvider));
                }
            }
            Packages = packageList;

            return this;
        }
    }

    public class PackageModel
    {
        public int? OrderShipmentPackageID { get; set; }
        public int OrderID { get; set; }
        public int? OrderShipmentID { get; set; }
        public string TrackingNumber { get; set; }
        public string OrderNumber { get; set; }
        public string PackageNumber { get; set; }
        public string OrderUrl { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OrderType { get; set; }
        public string OrderStatus { get; set; }
        public string OrderShipmentStatus { get; set; }
        public string CompleteDate { get; set; }
        public string DateShipped { get; set; }
        public string ConsultantFullName { get; set; }
        public string ConsultantUrl { get; set; }

        #region Infrastructure
        public PackageModel LoadResources(
            OrderShippingSearchData order,
            OrderShippingSearchData.OrderShipmentSearchData orderShipment,
            IFormatProvider formatProvider,
            int? orderShipmentPackageID = null)
        {
            OrderShipmentPackageID = orderShipmentPackageID;
            OrderID = order.OrderID;
            OrderShipmentID = orderShipment.OrderShipmentID;
            OrderNumber = order.OrderShipments.Count > 1
                ? Translation.GetTerm("OrderNumberWithShipmentNumberFormat", "{0} ({1} of {2})",
                    order.OrderNumber,
                    orderShipment.OrderShipmentIndex + 1,
                    order.OrderShipments.Count)
                : order.OrderNumber;
            OrderUrl = string.Format("/Orders/Details/Index/{0}", order.OrderNumber);
            FirstName = orderShipment.FirstName;
            LastName = orderShipment.LastName;
            OrderType = SmallCollectionCache.Instance.OrderTypes.GetById(order.OrderTypeID).GetTerm();
            OrderStatus = SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID).GetTerm();
            OrderShipmentStatus = SmallCollectionCache.Instance.OrderShipmentStatuses.GetById(orderShipment.OrderShipmentStatusID).GetTerm();
            CompleteDate = order.CompleteDateUTC.UTCToLocal().ToShortDateStringDisplay(formatProvider);
            ConsultantFullName = string.Format("{0} {1}", order.ConsultantFirstName, order.ConsultantLastName);
            ConsultantUrl = string.Format("/Accounts/Overview/Index/{0}", order.ConsultantAccountNumber);

            if (orderShipmentPackageID == null)
            {
                TrackingNumber = string.Empty;
                DateShipped = Translation.GetTerm("N/A");
                PackageNumber = "1 of 1";
            }
            else
            {
                var orderShipmentPackage = orderShipment.OrderShipmentPackages.First(x => x.OrderShipmentPackageID == orderShipmentPackageID.Value);
                TrackingNumber = orderShipmentPackage.TrackingNumber;
                DateShipped = orderShipmentPackage.DateShippedUTC.UTCToLocal().ToShortDateStringDisplay(formatProvider);
                if (orderShipment.OrderShipmentPackages.Count() == 1)
                {
                    PackageNumber = "1 of 1";
                }
                else
                {
                    PackageNumber = Translation.GetTerm("PackageNumberFormat", "{0} of {1}",
                        orderShipmentPackage.OrderShipmentPackageIndex + 1,
                        orderShipment.OrderShipmentPackages.Count());
                }
            }

            return this;
        }

        public PackageModel LoadResources(
            OrderShippingSearchData order,
            IFormatProvider formatProvider)
        {
            OrderID = order.OrderID;
            OrderNumber = order.OrderNumber;
            OrderUrl = string.Format("/Orders/Details/Index/{0}", order.OrderNumber);
            OrderType = SmallCollectionCache.Instance.OrderTypes.GetById(order.OrderTypeID).GetTerm();
            OrderStatus = SmallCollectionCache.Instance.OrderStatuses.GetById(order.OrderStatusID).GetTerm();
            CompleteDate = order.CompleteDateUTC.UTCToLocal().ToShortDateStringDisplay(formatProvider);
            ConsultantFullName = string.Format("{0} {1}", order.ConsultantFirstName, order.ConsultantLastName);
            ConsultantUrl = string.Format("/Accounts/Overview/Index/{0}", order.ConsultantAccountNumber);

            return this;
        }
        #endregion
    }
}