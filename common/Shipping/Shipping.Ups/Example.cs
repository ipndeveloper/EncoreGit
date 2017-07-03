// -----------------------------------------------------------------------
// <copyright file="Example.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using NetSteps.Shipping.Ups.ShippingAPI;
namespace NetSteps.Shipping.Ups
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class Example
    {
        public void Test()
        {
            ShipService svc = new ShipService();
            ShipConfirmRequest request = new ShipConfirmRequest();

            var response = svc.ProcessShipConfirm(request);
        }

        public ShipmentResponse TestThatReturnsResponse()
        {
            ShipService svc = new ShipService();

            // set credentials
            svc.UPSSecurityValue = new UPSSecurity();
            svc.UPSSecurityValue.UsernameToken = new UPSSecurityUsernameToken();
            svc.UPSSecurityValue.UsernameToken.Password = "5Orange55";
            svc.UPSSecurityValue.UsernameToken.Username = "TylerGarlick";
            svc.UPSSecurityValue.ServiceAccessToken = new UPSSecurityServiceAccessToken();
            svc.UPSSecurityValue.ServiceAccessToken.AccessLicenseNumber = "2C9A674E8713D2B8";
            
            // initialize request
            ShipmentRequest request = new ShipmentRequest();
            request.Request = new RequestType();
            request.Request.RequestOption = new string[] {"validate"};      // will throw exception if set to "validate" and passed invalid addresses
            
            // set request shipment options
            request.Shipment = new ShipmentType();
            request.Shipment.ReturnService = new ReturnServiceType();
            request.Shipment.ReturnService.Code = "2";
            request.Shipment.Shipper = new ShipperType();
            request.Shipment.Shipper.Name = "Shipping Company";
            request.Shipment.Shipper.Phone = new ShipPhoneType();
            request.Shipment.Shipper.Phone.Number = "8015555555";
            request.Shipment.Shipper.ShipperNumber = "73966X";
            ShipAddressType address = new ShipAddressType();
            address.AddressLine = new string[] { "111 Test Street" };
            address.City = "Lehi";
            address.StateProvinceCode = "UT";
            address.PostalCode = "84101";
            address.CountryCode = "US";

            request.Shipment.Shipper.Address = address;
            request.Shipment.Shipper.Address.AddressLine = new string[] {"1250 E 200 S", "Suite 3C"};
            request.Shipment.Shipper.Address.City = "Lehi";
            request.Shipment.Shipper.Address.StateProvinceCode = "UT";
            request.Shipment.Shipper.Address.PostalCode = "84043";
            request.Shipment.Shipper.Address.CountryCode = "US";
            request.Shipment.ShipTo = new ShipToType();
            request.Shipment.ShipTo.Name = "Receiving Company";
            request.Shipment.ShipTo.Phone = new ShipPhoneType();
            request.Shipment.ShipTo.Phone.Number = "8015555556";
            request.Shipment.ShipTo.Address = new ShipToAddressType();
            request.Shipment.ShipTo.Address.AddressLine = new string[] { "335 5th Ave", "Apt 4" };
            request.Shipment.ShipTo.Address.City = "Salt Lake City";
            request.Shipment.ShipTo.Address.StateProvinceCode = "UT";
            request.Shipment.ShipTo.Address.PostalCode = "84103";
            request.Shipment.ShipTo.Address.CountryCode = "US";

            request.Shipment.ShipFrom = new ShipFromType();
            request.Shipment.ShipFrom.Name = "Test";
            request.Shipment.ShipFrom.Phone = new ShipPhoneType();
            request.Shipment.ShipFrom.Phone.Number = "8015555557";
            request.Shipment.ShipFrom.Address = address;

            PaymentInfoType paymentInformation = new PaymentInfoType();
            paymentInformation.ShipmentCharge = new ShipmentChargeType[] {new ShipmentChargeType()};
            paymentInformation.ShipmentCharge[0].Type = "01";
            paymentInformation.ShipmentCharge[0].BillShipper = new BillShipperType();
            paymentInformation.ShipmentCharge[0].BillShipper.AccountNumber = "73966X";
            request.Shipment.PaymentInformation = paymentInformation;

            request.Shipment.Service = new ServiceType();
            request.Shipment.Service.Code = "03";
            request.Shipment.Package = new PackageType[] {new PackageType()};
            request.Shipment.Package[0].Description = "A present";
            request.Shipment.Package[0].Packaging = new PackagingType();
            request.Shipment.Package[0].Packaging.Code = "02";
            request.Shipment.Package[0].PackageWeight = new PackageWeightType();
            request.Shipment.Package[0].PackageWeight.Weight = "5";
            request.Shipment.Package[0].PackageWeight.UnitOfMeasurement = new ShipUnitOfMeasurementType();
            request.Shipment.Package[0].PackageWeight.UnitOfMeasurement.Code = "LBS";

            

            return svc.ProcessShipment(request);
        }
    }
}
