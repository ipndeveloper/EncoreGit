using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BelcorpUSA.Edi.Common;
using BelcorpUSA.Edi.Service.Configuration;
using NetSteps.Data.Entities;
using BelcorpUSA.Edi.Common.Orders;
using BelcorpUSA.Edi.Service.Data;
using System.IO;
using NetSteps.Diagnostics.Utilities;
using NetSteps.Data.Entities.Cache;
using NetSteps.Encore.Core.IoC;
using NetSteps.Configuration;
using System.Net;
using System.Configuration;

namespace BelcorpUSA.Edi.Service
{
	/// <summary>
	/// Core processing logic for collecting and disseminating order data over X12 EDI documents for BelcorpUSA
	/// </summary>
	[ContainerRegister(typeof(IEdiService), RegistrationBehaviors.Default)]
	public class EdiService : IEdiService
	{
		#region Properties

		public EdiServiceConfigurationSection Configuration { get; private set; }

		#endregion

		#region Constructors

		public EdiService()
		{
			Configuration = ConfigurationUtility.GetSection<EdiServiceConfigurationSection>();
			Initialize();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Establishes file storage location for output
		/// </summary>
		protected void Initialize()
		{
			foreach (var config in Configuration.PurchaseOrdersDropLocations.Paid)
			{
				EnsureDirectory(config.Location);
			}
			foreach (var config in Configuration.PurchaseOrdersDropLocations.Canceled)
			{
				EnsureDirectory(config.Location);
			}
			foreach (var config in Configuration.PurchaseOrdersDropLocations.Returned)
			{
				EnsureDirectory(config.Location);
			}

			if (Configuration.Archival.Enabled)
			{
				EnsureDirectory(Configuration.Archival.Location);
			}

			EnsureDirectory(Configuration.WorkingFolder.Location);
		}

		private void EnsureDirectory(string dir)
		{
			if (!Uri.IsWellFormedUriString(dir, UriKind.Absolute))
			{
				if (!Directory.Exists(dir))
				{
					Directory.CreateDirectory(dir);
				}
			}
		}

		/// <summary>
		/// Processes x12 confirmation file.  Currently, BelcorpUSA does not intend to transmit these files...
		/// </summary>
		public void ProcessConfirmations()
		{

		}

		/// <summary>
		/// Entry point for processing x12 856 Ship Notice files.  856 files are delivered from the Third Party Logistics (3PL) system
		/// </summary>
		public void ProcessShipNotices()
		{
			ShipNoticeRetriever retriever = new ShipNoticeRetriever();
			ShipNoticeProcessor processor = new ShipNoticeProcessor();
			foreach (var shipLocation in Configuration.ShipNoticeDropLocations)
			{
				string tz = Configuration.EdiInterchange.PartnerInterchangeIds[shipLocation.PartnerName].PartnerTimeZone;
				var lists = retriever.GetShipNotices(shipLocation, tz, Configuration.WorkingFolder.Location);
				foreach (var list in lists)
				{
					foreach (var notice in list)
					{
						processor.ProcessShipNotice(notice);
					}
					ArchiveShipNoticeList(list);
				}
			}
		}

		/// <summary>
		/// Entry point for processing orders from the Encore system, resulting in x12 850 files.  This includes "Paid", "Cancelled" and "Return" order types..
		/// </summary>
		public void ProcessOrders()
		{
			try
			{
				using (var processPaidOrdersActivity = this.TraceActivity("Process Paid Orders"))
				{
					ProcessPaidOrders();
				}
/*
                using (var processCanceledOrdersActivity = this.TraceActivity("Process Canceled Orders"))
				{
                    ProcessCanceledOrders();
				}
 */
				using (var processReturnedOrdersActivity = this.TraceActivity("Process Returned Orders"))
				{
					ProcessReturnedOrders();
				}
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
			}
		}

		/// <summary>
		/// Internal entry point for "Paid" order processing.
		/// </summary>
		protected void ProcessPaidOrders()
		{
			var gracePeriod = Configuration.EdiInterchange.OrderCancellationGracePeriod;
			var interchangeTracking = EdiInterchangeTracker.GetNewInterchangeTracking("Paid");
			var priorTo = interchangeTracking.DateCreatedUtc.Subtract(gracePeriod);

			try
			{
				var paidOrderList = GetPaidOrders(priorTo);
				if (paidOrderList.Any())
				{
					paidOrderList.InterchangeDateUTC = interchangeTracking.DateCreatedUtc;
					paidOrderList.InterchangeControlNumber = interchangeTracking.EdiInterchangeTrackerId;

					interchangeTracking.AddShipmentTrackingToInterchangeTracking(paidOrderList);

                    GenerateUploadFilePDF(paidOrderList.Select((po) => po.OrderId).ToArray()); //Developed by Luis Peña V. - CSTI

					StoreOrdersFile(interchangeTracking, paidOrderList, Configuration.PurchaseOrdersDropLocations.Paid);
					MarkOrdersPrinted(paidOrderList.Select((po) => po.OrderId).ToArray());                   
                    
				}
				else
				{
					interchangeTracking.TerminateInterchangeTracking("No orders to process");
				}
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
				interchangeTracking.TerminateInterchangeTracking("Exception occured");
			}
		}

		private void MarkOrdersPrinted(int[] orderIds)
		{
			using (NetStepsEntities nse = new NetStepsEntities())
			{
				var orders = (from o in nse.Orders
							  where orderIds.Contains(o.OrderID)
							  select o).ToList();
				var status = (from s in nse.OrderStatuses
							  where s.Name == "Printed"
							  select s).First();
				orders.ForEach(o =>
				{
					o.StartTracking();
					o.OrderStatus = status;
					o.OrderStatusID = status.OrderStatusID;
				});

				nse.SaveChanges();
			}
		}


		/// <summary>
		/// Collects order data for Paid orders between the given dates.
		/// </summary>
		/// <param name="fromDateUtc">The date and time to start from, exclusive.</param>
		/// <param name="toDateUtc">The date and time to end on, inclusive.</param>
		/// <returns></returns>
		protected Edi850PurchaseOrderList GetPaidOrders(DateTime priorTo)
		{
			var result = new Edi850PurchaseOrderList();

			IEnumerable<Edi850PurchaseOrder> orders;
			using (NetStepsEntities nse = new NetStepsEntities())
			{
				orders =
					(from o in nse.Orders
					 join oc in nse.OrderCustomers on o.OrderID equals oc.OrderID
					 join oi in nse.OrderItems on oc.OrderCustomerID equals oi.OrderCustomerID
					 join oCustShip in nse.OrderShipments on new { OrderId = o.OrderID, OrderCustomerId = (int?)oc.OrderCustomerID } equals new { OrderId = oCustShip.OrderID, OrderCustomerId = oCustShip.OrderCustomerID } into os1
					 from custShip in os1.DefaultIfEmpty()
					 join orderShip1 in nse.OrderShipments on new { OrderId = o.OrderID, OrderCustomerId = (int?)null } equals new { OrderId = orderShip1.OrderID, OrderCustomerId = orderShip1.OrderCustomerID } into orderShip2
					 from orderShip in orderShip2.DefaultIfEmpty()
					 from shipping in
						 (from ship in nse.OrderShipments
						  where ship.OrderShipmentID == (custShip.OrderShipmentID > 0 ? custShip.OrderShipmentID : orderShip.OrderShipmentID)
						  select new
						  {
							  ShipmentId = ship.OrderShipmentID,
							  FirstName = ship.FirstName,
							  LastName = ship.LastName,
							  Address1 = ship.Address1,
							  Address2 = ship.Address2,
							  Address3 = ship.Address3,
							  City = ship.City,
							  State = ship.State,
							  PostalCode = ship.PostalCode,
							  CountryCode = ship.Country.CountryCode,
							  ShippingMethod = ship.ShippingMethod == null ? null : ship.ShippingMethod.ShortName,
							  WarehouseId = ship.StateProvince.ShippingRegion.WarehouseID
						  })
					 where
						o.CompleteDateUTC <= priorTo
						&& o.OrderStatus.Name == "Paid"
						&& o.OrderType.Name != "Return Order"
						&& oi.Product.ProductBase.IsShippable
						&& !oi.Product.ChildProductRelations.Any(cpr => cpr.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit)
					    && !(oi.Product.DynamicKits.Count>0)
                     select new
					 {
						 AccountId = o.ConsultantID,
						 OrderId = o.OrderID,
						 OrderDateUTC = o.CompleteDateUTC,
						 IsPromoItem = oi.OrderAdjustmentOrderLineModifications.Any(lm => lm.ModificationOperationID == (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem),
						 Sku = oi.SKU,
						 SAPCode = oi.Product.Properties.Where(pp => pp.ProductPropertyType.Name == "SAP Code").FirstOrDefault(),
						 ProdTypeId = oi.Product.ProductBase.ProductTypeID,
						 ParentProdTypeId = (oi.ParentOrderItemID.HasValue ? (int?)oi.ParentOrderItem.Product.ProductBase.ProductTypeID : (int?)null),
						 OrderTypeId = o.OrderTypeID,
						 Shipping = shipping,
						 Quantity = oi.Quantity
					 })
					 .GroupBy(a => new
					 {
						 AccountId = a.AccountId,
						 OrderId = a.OrderId,
						 OrderDateUTC = a.OrderDateUTC,
						 IsPromoItem = a.IsPromoItem,
						 Sku = a.Sku,
						 SAPCode = a.SAPCode,
						 ProdTypeId = a.ProdTypeId,
						 ParentProdTypeId = a.ParentProdTypeId,
						 OrderTypeId = a.OrderTypeId,
						 Shipping = a.Shipping
					 })
					 .Select(g => new
					 {
						 AccountId = g.Key.AccountId,
						 OrderId = g.Key.OrderId,
						 OrderDate = g.Key.OrderDateUTC,
						 Shipping = g.Key.Shipping,
						 IsPromoItem = g.Key.IsPromoItem,
						 Sku = g.Key.Sku,
						 SAPCode = g.Key.SAPCode,
						 ProdTypeId = g.Key.ProdTypeId,
						 ParentProdTypeId = g.Key.ParentProdTypeId,
						 OrderTypeId = g.Key.OrderTypeId,
						 Quantity = g.Sum(a => a.Quantity)
					 })
					 .ToArray() //Break into 'in sql' -> 'after sql'
					 .GroupBy(a => new
					 {
						 AccountId = a.AccountId,
						 OrderId = a.OrderId,
						 OrderDateUTC = a.OrderDate,
						 Shipping = a.Shipping
					 })
					 .Select(grp => new Edi850PurchaseOrder()
					 {
						 AccountId = grp.Key.AccountId,
						 OrderId = grp.Key.OrderId,
						 OrderDateUTC = grp.Key.OrderDateUTC.GetValueOrDefault(),
						 ShipmentId = grp.Key.Shipping.ShipmentId,
						 StatusCode = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].PaidStatusCode,
						 ShipFrom = new EdiName()
						 {
							 Name = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromName,
							 IdentificationQualifier = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromIdentificationQualifier,
							 IdentificationCode = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromIdentificationCode
						 },
						 ShipTo = new EdiGeographicLocation()
						 {
							 Name = String.Format("{0}, {1}", grp.Key.Shipping.LastName, grp.Key.Shipping.FirstName),
							 Address1 = grp.Key.Shipping.Address1,
							 Address2 = grp.Key.Shipping.Address2,
							 Address3 = grp.Key.Shipping.Address3,
							 City = grp.Key.Shipping.City,
							 StateProvinceCode = grp.Key.Shipping.State,
							 PostalCode = grp.Key.Shipping.PostalCode,
							 CountryCode = grp.Key.Shipping.CountryCode
						 },
						 ShipBy = grp.Key.Shipping.ShippingMethod,
						 Items = grp.Select(item =>
							 new Edi850PurchaseOrder.Edi850POItem()
							 {
								 Quantity = item.Quantity,
								 UnitOfMeasure = "EA",
								 ProductCodeQualifier = "VN",
								 ProductCode = item.SAPCode != null ? item.SAPCode.PropertyValue : String.Empty,
								 CatalogCodeQualifier = "CB",
								 CatalogCode = item.Sku,
								 CommercialMovementCode = GetCommercialMovementTypeCode(item.ParentProdTypeId ?? item.ProdTypeId, item.OrderTypeId, isPromoItem: item.IsPromoItem)
							 }).ToArray()
					 })
					 .ToArray();
			}

			result.AddRange(orders);

			return result;
		}

		protected void StoreOrdersFile(EdiInterchangeTracker interchangeTracking, Edi850PurchaseOrderList paidOrderList, IEnumerable<EdiDropLocationConfigurationElement> configs)
		{
			foreach (var config in configs)
			{
				var ediInfo = Configuration.EdiInterchange.PartnerInterchangeIds[config.PartnerName];
				paidOrderList.RecieverIdQualifier = "ZZ";
				paidOrderList.RecieverId = ediInfo.PartnerId;
				paidOrderList.SenderIdQualifier = "ZZ";
				paidOrderList.SenderId = ediInfo.OurId;
				paidOrderList.ClientCode = ediInfo.OurClientCode;
				string fileName = String.Concat("850.", config.PartnerName, ".", paidOrderList.InterchangeControlNumber.ToString().PadLeft(9, '0'), ".EDI");
				string fileBody = paidOrderList.ConvertOrderToX12850Document(Configuration.IsProduction, ediInfo.PartnerTimeZone, subelementSeperator: '>');

				if (config.Location.StartsWith("ftp"))
				{
					WriteOrderFileToFtp(paidOrderList, config, fileName, fileBody);
				}
				else
				{
					WriteOrderFileToLocalDisk(paidOrderList, config, fileName, fileBody);
				}

				interchangeTracking.AddPartnerTrackingToInterchangeTracking(config.PartnerName, fileName);

				ArchiveOrderFile(interchangeTracking, config, fileName, fileBody);
			}
		}

		protected void WriteOrderFileToFtp(Edi850PurchaseOrderList paidOrderList, EdiDropLocationConfigurationElement config, string fileName, string fileBody)
		{
			Uri uploadPath = new Uri(new Uri(config.Location, UriKind.Absolute), fileName);
			paidOrderList.FileName = uploadPath.AbsoluteUri.ToString();
			FtpWebRequest request = FtpWebRequest.Create(uploadPath) as FtpWebRequest;
			request.Credentials = new NetworkCredential(config.Credentials.UserName, config.Credentials.Password);
			request.Method = WebRequestMethods.Ftp.UploadFile;
			var fileBytes = Encoding.UTF8.GetBytes(fileBody);
			request.ContentLength = fileBytes.Length;
			using (var reqStream = request.GetRequestStream())
			{
				reqStream.Write(fileBytes, 0, fileBytes.Length);
			}
			var response = (FtpWebResponse)request.GetResponse();
			var responseCode = response.StatusCode;
			var responseDesc = response.StatusDescription;
			response.Close();
			if (responseCode != FtpStatusCode.ClosingData)
			{
				throw new Exception(responseDesc);
			}
			this.TraceInformation(String.Format("Uploaded {0}.  {1}: {2}", paidOrderList.FileName, responseCode, responseDesc));
		}

		protected void WriteOrderFileToLocalDisk(Edi850PurchaseOrderList paidOrderList, EdiDropLocationConfigurationElement config, string fileName, string fileBody)
		{
			string fileFullPath = Path.Combine(config.Location, fileName);
			paidOrderList.FileName = fileFullPath;
			File.WriteAllText(fileFullPath, fileBody, Encoding.UTF8);
		}

		protected void ArchiveOrderFile(EdiInterchangeTracker interchangeTracking, EdiDropLocationConfigurationElement config, string fileName, string fileBody)
		{
			if (Configuration.Archival.Enabled)
			{
				var archivePath = Path.Combine(Configuration.Archival.Location,
									config.PartnerName,
									"EDI850",
									interchangeTracking.OrderType,
									interchangeTracking.DateCreatedUtc.ToString("yyyy"),
									interchangeTracking.DateCreatedUtc.ToString("MM"),
									interchangeTracking.DateCreatedUtc.ToString("dd"));
				ArchiveFile(fileName, fileBody, archivePath);
			}
		}

		protected void ProcessCanceledOrders()
		{
			IEnumerable<EdiCanceledOrderTracker> canceledOrders;
			var ediCanceledOrdersList = GetCanceledOrders(out canceledOrders);
			if (ediCanceledOrdersList.Any())
			{
				var interchangeTracking = EdiInterchangeTracker.GetNewInterchangeTracking("Canceled");
				ediCanceledOrdersList.InterchangeDateUTC = interchangeTracking.DateCreatedUtc;
				ediCanceledOrdersList.InterchangeControlNumber = interchangeTracking.EdiInterchangeTrackerId;

				try
				{
					interchangeTracking.AddShipmentTrackingToInterchangeTracking(ediCanceledOrdersList);

					StoreOrdersFile(interchangeTracking, ediCanceledOrdersList, Configuration.PurchaseOrdersDropLocations.Canceled);

					using (var ctx = new EdiTrackingDbContext())
					{
						foreach (var item in canceledOrders)
						{
							ctx.EdiCanceledOrderTrackers.Attach(item);
							item.EdiInterchangeTrackerId = interchangeTracking.EdiInterchangeTrackerId;
						}
						ctx.SaveChanges();
					}

				}
				catch (Exception ex)
				{
					this.TraceException(ex);
					interchangeTracking.TerminateInterchangeTracking("Exception Occurred");
				}
			}
		}

		protected Edi850PurchaseOrderList GetCanceledOrders(out IEnumerable<EdiCanceledOrderTracker> canceledOrders)
		{
			var result = new Edi850PurchaseOrderList();
			using (EdiTrackingDbContext edictx = new EdiTrackingDbContext())
			{
				canceledOrders = edictx.Database.SqlQuery<EdiCanceledOrderTracker>("EXEC [BelcorpUSA].[USP_CollectCanceledOrdersForEdi]").ToArray();
			}
			if (canceledOrders.Any())
			{
				var canceledOrderIds = canceledOrders.Select(c => c.OrderId);
				IEnumerable<Edi850PurchaseOrder> orders;
				using (NetStepsEntities nse = new NetStepsEntities())
				{
					orders =
						(from o in nse.Orders
						 join oc in nse.OrderCustomers on o.OrderID equals oc.OrderID
						 join oi in nse.OrderItems on oc.OrderCustomerID equals oi.OrderCustomerID
						 join oCustShip in nse.OrderShipments on new { OrderId = o.OrderID, OrderCustomerId = (int?)oc.OrderCustomerID } equals new { OrderId = oCustShip.OrderID, OrderCustomerId = oCustShip.OrderCustomerID } into os1
						 from custShip in os1.DefaultIfEmpty()
						 join orderShip1 in nse.OrderShipments on new { OrderId = o.OrderID, OrderCustomerId = (int?)null } equals new { OrderId = orderShip1.OrderID, OrderCustomerId = orderShip1.OrderCustomerID } into orderShip2
						 from orderShip in orderShip2.DefaultIfEmpty()
						 from shipping in
							 (from ship in nse.OrderShipments
							  where ship.OrderShipmentID == (custShip.OrderShipmentID > 0 ? custShip.OrderShipmentID : orderShip.OrderShipmentID)
							  select new
							  {
								  ShipmentId = ship.OrderShipmentID,
								  FirstName = ship.FirstName,
								  LastName = ship.LastName,
								  Address1 = ship.Address1,
								  Address2 = ship.Address2,
								  Address3 = ship.Address3,
								  City = ship.City,
								  State = ship.State,
								  PostalCode = ship.PostalCode,
								  CountryCode = ship.Country.CountryCode,
								  WarehouseId = ship.StateProvince.ShippingRegion.WarehouseID
							  })
						 where canceledOrderIds.Contains(o.OrderID)
							&& oi.Product.ProductBase.IsShippable
							&& !oi.Product.ChildProductRelations.Any(cpr => cpr.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit)
						 select new
						 {
							 AccountId = o.ConsultantID,
							 OrderId = o.OrderID,
							 OrderDateUTC = o.CompleteDateUTC,
							 IsPromoItem = oi.OrderAdjustmentOrderLineModifications.Any(lm => lm.ModificationOperationID == (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem),
							 Sku = oi.SKU,
							 SAPCode = oi.Product.Properties.Where(pp => pp.ProductPropertyType.Name == "SAP Code").FirstOrDefault(),
							 ProdTypeId = oi.Product.ProductBase.ProductTypeID,
							 ParentProdTypeId = (oi.ParentOrderItemID.HasValue ? (int?)oi.ParentOrderItem.Product.ProductBase.ProductTypeID : (int?)null),
							 OrderTypeId = o.OrderTypeID,
							 Shipping = shipping,
							 Quantity = oi.Quantity
						 })
					 .GroupBy(a => new
					 {
						 AccountId = a.AccountId,
						 OrderId = a.OrderId,
						 OrderDateUTC = a.OrderDateUTC,
						 IsPromoItem = a.IsPromoItem,
						 Sku = a.Sku,
						 SAPCode = a.SAPCode,
						 ProdTypeId = a.ProdTypeId,
						 ParentProdTypeId = a.ParentProdTypeId,
						 OrderTypeId = a.OrderTypeId,
						 Shipping = a.Shipping
					 })
					 .Select(g => new
					 {
						 AccountId = g.Key.AccountId,
						 OrderId = g.Key.OrderId,
						 OrderDate = g.Key.OrderDateUTC,
						 Shipping = g.Key.Shipping,
						 IsPromoItem = g.Key.IsPromoItem,
						 Sku = g.Key.Sku,
						 SAPCode = g.Key.SAPCode,
						 ProdTypeId = g.Key.ProdTypeId,
						 ParentProdTypeId = g.Key.ParentProdTypeId,
						 OrderTypeId = g.Key.OrderTypeId,
						 Quantity = g.Sum(a => a.Quantity)
					 })
					 .ToArray() //Break into 'in sql' -> 'after sql'
					 .GroupBy(a => new
					 {
						 AccountId = a.AccountId,
						 OrderId = a.OrderId,
						 OrderDateUTC = a.OrderDate,
						 Shipping = a.Shipping
					 })
					 .Select(grp => new Edi850PurchaseOrder()
					 {
						 AccountId = grp.Key.AccountId,
						 OrderId = grp.Key.OrderId,
						 OrderDateUTC = grp.Key.OrderDateUTC.GetValueOrDefault(),
						 ShipmentId = grp.Key.Shipping.ShipmentId,
						 StatusCode = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].CanceledStatusCode,
						 ShipFrom = new EdiName()
						 {
							 Name = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromName,
							 IdentificationQualifier = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromIdentificationQualifier,
							 IdentificationCode = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromIdentificationCode
						 },
						 ShipTo = new EdiGeographicLocation()
						 {
							 Name = String.Format("{0}, {1}", grp.Key.Shipping.LastName, grp.Key.Shipping.FirstName),
							 Address1 = grp.Key.Shipping.Address1,
							 Address2 = grp.Key.Shipping.Address2,
							 Address3 = grp.Key.Shipping.Address3,
							 City = grp.Key.Shipping.City,
							 StateProvinceCode = grp.Key.Shipping.State,
							 PostalCode = grp.Key.Shipping.PostalCode,
							 CountryCode = grp.Key.Shipping.CountryCode
						 },
						 Items = grp.Select(item =>
							 new BelcorpUSA.Edi.Common.Orders.Edi850PurchaseOrder.Edi850POItem()
							 {
								 Quantity = item.Quantity,
								 UnitOfMeasure = "EA",
								 ProductCodeQualifier = "VN",
								 ProductCode = item.SAPCode != null ? item.SAPCode.PropertyValue : String.Empty,
								 CatalogCodeQualifier = "CB",
								 CatalogCode = item.Sku,
								 CommercialMovementCode = GetCommercialMovementTypeCode(item.ParentProdTypeId ?? item.ProdTypeId, item.OrderTypeId, isPromoItem: item.IsPromoItem)
							 }).ToArray()
					 })
					 .ToArray();
				}

				result.AddRange(orders);
			}

			return result;
		}

		protected void ProcessReturnedOrders()
		{
			var interchangeTracking = EdiInterchangeTracker.GetNewInterchangeTracking("Returned");
			var priorTo = interchangeTracking.DateCreatedUtc;
			try
			{
				IEnumerable<int> ordersToMarkPrinted;
				var orderList = GetReturnedOrders(priorTo, out ordersToMarkPrinted);
				if (orderList.Any())
				{
					orderList.InterchangeDateUTC = interchangeTracking.DateCreatedUtc;
					orderList.InterchangeControlNumber = interchangeTracking.EdiInterchangeTrackerId;

					interchangeTracking.AddShipmentTrackingToInterchangeTracking(orderList);

					StoreOrdersFile(interchangeTracking, orderList, Configuration.PurchaseOrdersDropLocations.Returned);
					MarkOrdersPrinted(ordersToMarkPrinted.ToArray());
				}
				else
				{
					interchangeTracking.TerminateInterchangeTracking("No orders to process");
				}
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
				interchangeTracking.TerminateInterchangeTracking("Exception occured");
			}
		}

		protected Edi850PurchaseOrderList GetReturnedOrders(DateTime priorToUTC, out IEnumerable<int> actualOrders)
		{
			var result = new Edi850PurchaseOrderList();
			actualOrders = new List<int>();
			IEnumerable<Edi850PurchaseOrder> orders;
			using (NetStepsEntities nse = new NetStepsEntities())
			{
				var set =
					(from o in nse.Orders
					 join parOrd in nse.Orders on o.ParentOrderID equals parOrd.OrderID
					 join oc in nse.OrderCustomers on o.OrderID equals oc.OrderID
					 join poc in nse.OrderCustomers on parOrd.OrderID equals poc.OrderID
					 join oi in nse.OrderItems on oc.OrderCustomerID equals oi.OrderCustomerID
					 join oCustShip in nse.OrderShipments on new { OrderId = parOrd.OrderID, OrderCustomerId = (int?)poc.OrderCustomerID } equals new { OrderId = oCustShip.OrderID, OrderCustomerId = oCustShip.OrderCustomerID } into os1
					 from custShip in os1.DefaultIfEmpty()
					 join orderShip1 in nse.OrderShipments on new { OrderId = o.OrderID, OrderCustomerId = (int?)null } equals new { OrderId = orderShip1.OrderID, OrderCustomerId = orderShip1.OrderCustomerID } into orderShip2
					 from orderShip in orderShip2.DefaultIfEmpty()
					 from shipping in
						 (from ship in nse.OrderShipments
						  where ship.OrderShipmentID == (custShip.OrderShipmentID > 0 ? custShip.OrderShipmentID : orderShip.OrderShipmentID)
						  select new
						  {
							  ShipmentId = ship.OrderShipmentID,
							  FirstName = ship.FirstName,
							  LastName = ship.LastName,
							  Address1 = ship.Address1,
							  Address2 = ship.Address2,
							  Address3 = ship.Address3,
							  City = ship.City,
							  State = ship.State,
							  PostalCode = ship.PostalCode,
							  CountryCode = ship.Country.CountryCode,
							  WarehouseId = ship.StateProvince.ShippingRegion.WarehouseID
						  })
					 where o.CompleteDateUTC <= priorToUTC
						&& o.OrderStatus.Name == "Paid"
						&& o.OrderType.Name == "Return Order"
                        && o.ParentOrder.OrderStatus.Name != "Cancelled Paid"
						&& oi.Product.ProductBase.IsShippable
						&& !oi.Product.ChildProductRelations.Any(cpr => cpr.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit)
					 select new
					 {
						 AccountId = o.ConsultantID,
						 OrderId = parOrd.OrderID,
						 ActOrderId = o.OrderID,
						 ParentOrderStatusId = parOrd.OrderStatusID,
						 OrderDateUTC = o.CompleteDateUTC,
						 IsPromoItem = oi.OrderAdjustmentOrderLineModifications.Any(lm => lm.ModificationOperationID == (int)NetSteps.OrderAdjustments.Common.Model.OrderAdjustmentOrderLineOperationKind.AddedItem),
						 Sku = oi.SKU,
						 SAPCode = oi.Product.Properties.Where(pp => pp.ProductPropertyType.Name == "SAP Code").FirstOrDefault(),
						 ProdTypeId = oi.Product.ProductBase.ProductTypeID,
						 ParentProdTypeId = (oi.ParentOrderItemID.HasValue ? (int?)oi.ParentOrderItem.Product.ProductBase.ProductTypeID : (int?)null),
						 OrderTypeId = o.OrderTypeID,
						 Shipping = shipping,
						 Quantity = oi.Quantity
					 })
					 .GroupBy(a => new
					 {
						 AccountId = a.AccountId,
						 OrderId = a.OrderId,
						 ActOrderId = a.ActOrderId,
						 ParentOrderStatusId = a.ParentOrderStatusId,
						 OrderDateUTC = a.OrderDateUTC,
						 IsPromoItem = a.IsPromoItem,
						 Sku = a.Sku,
						 SAPCode = a.SAPCode,
						 ProdTypeId = a.ProdTypeId,
						 ParentProdTypeId = a.ParentProdTypeId,
						 OrderTypeId = a.OrderTypeId,
						 Shipping = a.Shipping
					 })
					 .Select(g => new
					 {
						 AccountId = g.Key.AccountId,
						 OrderId = g.Key.OrderId,
						 ActOrderId = g.Key.ActOrderId,
						 ParentOrderStatusId = g.Key.ParentOrderStatusId,
						 OrderDate = g.Key.OrderDateUTC,
						 Shipping = g.Key.Shipping,
						 IsPromoItem = g.Key.IsPromoItem,
						 Sku = g.Key.Sku,
						 SAPCode = g.Key.SAPCode,
						 ProdTypeId = g.Key.ProdTypeId,
						 ParentProdTypeId = g.Key.ParentProdTypeId,
						 OrderTypeId = g.Key.OrderTypeId,
						 Quantity = g.Sum(a => a.Quantity)
					 })
					 .ToArray() //Break into 'in sql' -> 'after sql'
					 .GroupBy(a => new
					 {
						 AccountId = a.AccountId,
						 OrderId = a.OrderId,
						 ActOrderId = a.ActOrderId,
						 OrderDateUTC = a.OrderDate,
						 Shipping = a.Shipping
					 });

				((List<int>)actualOrders).AddRange(set.Select(g => g.Key.ActOrderId));

				orders = set.Select(grp => new Edi850PurchaseOrder()
				{
					AccountId = grp.Key.AccountId,
					OrderId = grp.Key.OrderId,
					OrderDateUTC = grp.Key.OrderDateUTC.GetValueOrDefault(),
					ShipmentId = grp.Key.Shipping.ShipmentId,
					StatusCode = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ReturnedStatusCode,
					ShipFrom = new EdiName()
					{
						Name = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromName,
						IdentificationQualifier = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromIdentificationQualifier,
						IdentificationCode = Configuration.EdiInterchange.PurchaseOrders[grp.Key.Shipping.WarehouseId.ToString()].ShipFromIdentificationCode
					},
					ShipTo = new EdiGeographicLocation()
					{
						Name = String.Format("{0}, {1}", grp.Key.Shipping.LastName, grp.Key.Shipping.FirstName),
						Address1 = grp.Key.Shipping.Address1,
						Address2 = grp.Key.Shipping.Address2,
						Address3 = grp.Key.Shipping.Address3,
						City = grp.Key.Shipping.City,
						StateProvinceCode = grp.Key.Shipping.State,
						PostalCode = grp.Key.Shipping.PostalCode,
						CountryCode = grp.Key.Shipping.CountryCode
					},
					Items = grp.Select(item =>
						new BelcorpUSA.Edi.Common.Orders.Edi850PurchaseOrder.Edi850POItem()
						{
							Quantity = item.Quantity,
							UnitOfMeasure = "EA",
							ProductCodeQualifier = "VN",
							ProductCode = item.SAPCode != null ? item.SAPCode.PropertyValue : String.Empty,
							CatalogCodeQualifier = "CB",
							CatalogCode = item.Sku,
							CommercialMovementCode = GetCommercialMovementTypeCode(item.ParentProdTypeId ?? item.ProdTypeId, item.OrderTypeId, item.ParentOrderStatusId, item.IsPromoItem)
						}).ToArray()
				})
				.ToArray();
			}

			result.AddRange(orders);

			return result;
		}

		protected string GetCommercialMovementTypeCode(int prodTypeId, short orderTypeId, int? parentOrderStatusId = null, bool isPromoItem = false)
		{
			string result = String.Empty;
			if (isPromoItem)
			{
				if (orderTypeId == (int)Constants.OrderType.ReturnOrder)
				{
					result = Configuration.EdiInterchange.PromotionItemReturnCMTCode ?? "S14";
				}
				else
				{
					result = Configuration.EdiInterchange.PromotionItemPaidCMTCode ?? "S13";
				}
			}
			else
			{
				using (EdiTrackingDbContext ctx = new EdiTrackingDbContext())
				{
					var code = (from cmt in ctx.CommercialMovementTypes
								join otptcmt in ctx.OrderTypeProductTypeCommercialMovementTypes on cmt.CommercialMovementTypeId equals otptcmt.CommercialMovementTypeId
								where otptcmt.OrderTypeId == orderTypeId
									&& otptcmt.ProductTypeId == prodTypeId
									&& ((!parentOrderStatusId.HasValue && !otptcmt.ParentOrderStatusId.HasValue) || (otptcmt.ParentOrderStatusId == parentOrderStatusId))
								select cmt.Code).FirstOrDefault();
					if (!String.IsNullOrWhiteSpace(code))
					{
						result = code;
					}
				}
			}
			return result;
		}

		protected void ArchiveShipNoticeList(Edi856ShipNoticeList notice)
		{
			var config = (from partner in Configuration.EdiInterchange.PartnerInterchangeIds
						  where partner.PartnerId == notice.SenderId
						  select partner).First();

			var archivePath = Path.Combine(Configuration.Archival.Location,
										config.PartnerName,
										"EDI856",
										notice.InterchangeDateUTC.ToString("yyyy"),
										notice.InterchangeDateUTC.ToString("MM"),
										notice.InterchangeDateUTC.ToString("dd"));
			if (File.Exists(notice.FileName))
			{
				try
				{
					if (!Directory.Exists(archivePath))
					{
						Directory.CreateDirectory(archivePath);
					}
					string fn = Path.GetFileNameWithoutExtension(notice.FileName);
					string ext = Path.GetExtension(notice.FileName);
					string newFileName = Path.Combine(archivePath, String.Concat(fn, ext));
					int fnCounter = 1;
					while (File.Exists(newFileName))
					{
						fnCounter++;
						newFileName = Path.Combine(archivePath, String.Concat(fn, " (", fnCounter, ")", ext));
					}
					File.Move(notice.FileName, newFileName);
				}
				catch (Exception ex)
				{
					this.TraceException(ex);
					throw;
				}
			}
		}

		protected void ArchiveFile(string fileName, string fileBody, string archivePath)
		{
			try
			{
				if (!Directory.Exists(archivePath))
				{
					Directory.CreateDirectory(archivePath);
				}

				File.WriteAllText(Path.Combine(archivePath, fileName), fileBody, Encoding.UTF8);
			}
			catch (Exception ex)
			{
				this.TraceException(ex);
			}
		}

        #region MetodosPDF

        private void GenerateUploadFilePDF(int[] orderIds)
        {
            MemoryStream ms = new MemoryStream();
            List<string> OrderList = new List<string>();

            using (NetStepsEntities nse = new NetStepsEntities())
            {
                var orders = (from o in nse.Orders
                              where orderIds.Contains(o.OrderID)
                              select o).ToList();

                foreach (var List in orders)
                {
                    OrderList.Add(List.OrderNumber);
                }
            }

            if (OrderList.Count > 0)
            {
                foreach (var List in OrderList)
                {
                    List<string> listOrderPaid = new List<string>();
                    listOrderPaid.Add(List);

                    ms = Pdf.GeneratePDFMemoryStream(listOrderPaid);

                    string fileName = string.Format("{0}_{1}.pdf", "OrdersPaid", List.ToString());

                    string uploadPath = ConfigurationManager.AppSettings["InvoiceDropLocation"].ToString() + fileName;

                    using (FileStream file = new FileStream(uploadPath, FileMode.Create, FileAccess.Write))
                    {
                        ms.WriteTo(file);
                    }
                }
            }
        }     

        #endregion

		#endregion
	}
}
