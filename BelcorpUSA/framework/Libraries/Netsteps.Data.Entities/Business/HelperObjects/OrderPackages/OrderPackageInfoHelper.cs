using System.Collections.Generic;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Encore.Core.IoC;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using System.Linq;

namespace NetSteps.Data.Entities.Business.HelperObjects.OrderPackages
{
    public interface IOrderPackageInfoHelper
    {
        IEnumerable<OrderPackageInfoModel> GetOrderPackageInfoList(Order order);
        IEnumerable<OrderPackageInfoModel> GetOrderPackageInfoList(int orderID);
    }
    
    [ContainerRegister(typeof(IOrderPackageInfoHelper), RegistrationBehaviors.Default)]
    public class OrderPackageInfoHelper : IOrderPackageInfoHelper
    {
        public IEnumerable<OrderPackageInfoModel> GetOrderPackageInfoList(Order order)
        {
            return ConsolidateShipmentDetails(order);
        }

        public IEnumerable<OrderPackageInfoModel> GetOrderPackageInfoList(int orderID)
        {
            Order order = Order.LoadWithShipmentDetails(orderID);
            return ConsolidateShipmentDetails(order);
        }

        protected IEnumerable<OrderPackageInfoModel> ConsolidateShipmentDetails(Order order)
        {
            var packageInfoList = new List<OrderPackageInfoModel>();

            //if (order.OrderShipments.IsNotNull())
            //{
            //    foreach (var shipment in order.OrderShipments)
            //    {
            //        packageInfoList.AddRange(PackageInfoModels(shipment));
            //    }
            //}

            foreach (var shipment in order.OrderShipments)
            {
                List<ShippingCalculatorSearchData.GetShipping> objGetShipping = ShippingCalculatorExtensions.GetShippingResult(shipment.PostalCode);

                foreach (var item in objGetShipping.Where(x => x.ShippingMethodID == shipment.ShippingMethodID && x.OrderTypeID == shipment.Order.OrderTypeID).ToList())
                {
                    OrderPackageInfoModel objE = new OrderPackageInfoModel();
                    objE.ShipMethodName = item.DaysForDelivery + " " + shipment.ShippingMethodName + " " + item.Name;
                    objE.OrderCustomerID = shipment.OrderCustomerID;
                    packageInfoList.Add(objE);
                }

            }

            return packageInfoList;
        }

        protected IEnumerable<OrderPackageInfoModel> PackageInfoModels(OrderShipment shipment)
        {
            var packageInfoModels = new List<OrderPackageInfoModel>();

            if (shipment.OrderShipmentPackages.Count == 0)
            {
                packageInfoModels.Add(new OrderPackageInfoModel
                                          {
                                              ShipMethodName = shipment.ShippingMethodName
                                          });
            }
            else
            {
                AddAllPackagesToTheList(shipment, packageInfoModels);
            }

            return packageInfoModels;
        }

        protected void AddAllPackagesToTheList(OrderShipment shipment, IList<OrderPackageInfoModel> packageInfoModels)
        {
            foreach (var package in shipment.OrderShipmentPackages)
            {
                var packageItem = GetOrderPackageInformation(package);
                packageInfoModels.Add(packageItem);
            }
        }

        protected OrderPackageInfoModel GetOrderPackageInformation(OrderShipmentPackage package)
        {
            var model = new OrderPackageInfoModel
                            {
                                ShipMethodName = package.ShippingMethodName,
                                ShipDate = package.DateShipped,
                                TrackingNumber = package.TrackingNumber,
                                TrackingUrl = package.TrackingUrl
                            };

            if (package.ShippingMethodID.HasValue)
                model.BaseTrackUrl = OrderShipment.GetBaseTrackUrl(package.ShippingMethodID.Value, package.TrackingNumber);
            return model;
        }
    }
}