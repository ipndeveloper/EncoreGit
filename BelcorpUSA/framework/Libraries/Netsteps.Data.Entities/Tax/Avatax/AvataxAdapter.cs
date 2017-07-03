using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Avalara.AvaTax.Adapter;
using Avalara.AvaTax.Adapter.TaxService;
using NetSteps.Addresses.Common.Models;
using NetSteps.Common.Configuration;
using NetSteps.Common.Extensions;
using NetSteps.Common.Reflection;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Services;
using NetSteps.Data.Entities.Tax.Avatax;
using NetSteps.Encore.Core.IoC;
using NetStepsException = NetSteps.Data.Entities;

namespace NetSteps.Data.Entities.AvataxAPI
{
	/// <summary>
	/// This class converts/maps netsteps data to the format required by the API.
	/// Acts as an intermediary between netsteps and API
	/// </summary>
	[ContainerRegister(typeof(IAvataxAdapter), RegistrationBehaviors.Default, ScopeBehavior = ScopeBehavior.InstancePerRequest)]
	public class AvataxAdapter : IDisposable, IAvataxAdapter
	{
		#region private members
		AvataxAPI avataxAPI = TaxAPIProvider.GetAvataxAPIInstance();
		private bool disposed = false;
		#endregion

		#region public properties

		public string PingResultCode(PingResult pingResult)
		{
			return pingResult != null ? (Util.ConvertResultCodeToString(pingResult.ResultCode)) : string.Empty;
		}
		/// <summary>
		/// 
		/// </summary>
		public string PingResultMessage(PingResult pingResult)
		{
			if (pingResult != null)
				if (pingResult.Messages.Count >= 1)
				{
					StringBuilder sb = new StringBuilder();
					foreach (Message msg in pingResult.Messages)
					{
						sb.AppendLine(msg.Summary);
					}

					return sb.ToString();
				}

			return string.Empty;
		}

		public bool IsGetTaxRequestSuccess(GetTaxResult getTaxResult)
		{
			return getTaxResult != null ? (getTaxResult.ResultCode == SeverityLevel.Success ? true : false) : false;
		}

		public string GetTaxResultCode(GetTaxResult getTaxResult)
		{
			return getTaxResult != null ? (Util.ConvertResultCodeToString(getTaxResult.ResultCode)) : string.Empty;
		}

		public string GetTaxResultMessage(GetTaxResult getTaxResult)
		{
			if (getTaxResult != null)
				if (getTaxResult.Messages.Count >= 1)
				{
					StringBuilder sb = new StringBuilder();
					foreach (Message msg in getTaxResult.Messages)
					{
						sb.AppendLine(msg.Summary);
					}

					return sb.ToString();
				}

			return string.Empty;
		}

		public bool IsPostTaxRequestSuccess(PostTaxResult postTaxResult)
		{
			return postTaxResult != null ? (postTaxResult.ResultCode == SeverityLevel.Success ? true : false) : false;
		}

		public string PostTaxResultCode(PostTaxResult postTaxResult)
		{
			return postTaxResult != null ? (Util.ConvertResultCodeToString(postTaxResult.ResultCode)) : string.Empty;
		}

		public string PostTaxResultMessage(PostTaxResult postTaxResult)
		{
			if (postTaxResult != null)
				if (postTaxResult.Messages.Count >= 1)
				{
					StringBuilder sb = new StringBuilder();
					foreach (Message msg in postTaxResult.Messages)
					{
						sb.AppendLine(msg.Summary);
					}

					return sb.ToString();
				}

			return string.Empty;
		}

		public bool IsCommitTaxRequestSuccess(CommitTaxResult commitTaxResult)
		{
			return commitTaxResult != null ? (commitTaxResult.ResultCode == SeverityLevel.Success ? true : false) : false;
		}

		public string CommitTaxResultCode(CommitTaxResult commitTaxResult)
		{
			return commitTaxResult != null ? (Util.ConvertResultCodeToString(commitTaxResult.ResultCode)) : string.Empty;
		}

		public string CommitTaxResultMessage(CommitTaxResult commitTaxResult)
		{
			if (commitTaxResult != null)
				if (commitTaxResult.Messages.Count >= 1)
				{
					StringBuilder sb = new StringBuilder();
					foreach (Message msg in commitTaxResult.Messages)
					{
						sb.AppendLine(msg.Summary);
					}

					return sb.ToString();
				}

			return string.Empty;
		}

		public bool IsAdjustTaxRequestSuccess(AdjustTaxResult adjustTaxResult)
		{
			return adjustTaxResult != null ? (adjustTaxResult.ResultCode == SeverityLevel.Success ? true : false) : false;
		}

		public string AdjustTaxResultCode(AdjustTaxResult adjustTaxResult)
		{
			return adjustTaxResult != null ? (Util.ConvertResultCodeToString(adjustTaxResult.ResultCode)) : string.Empty;
		}

		public string AdjustTaxResultMessage(AdjustTaxResult adjustTaxResult)
		{
			if (adjustTaxResult != null)
				if (adjustTaxResult.Messages.Count >= 1)
				{
					StringBuilder sb = new StringBuilder();
					foreach (Message msg in adjustTaxResult.Messages)
					{
						sb.AppendLine(msg.Summary);
					}

					return sb.ToString();
				}

			return string.Empty;
		}

		private Avalara.AvaTax.Adapter.AddressService.Address _originAddressDefault;
		/// <summary>
		/// Default Origin address saved in config file
		/// </summary>
		public Avalara.AvaTax.Adapter.AddressService.Address OriginAddressDefault
		{
			get { return _originAddressDefault; }
			set { _originAddressDefault = value; }
		}

		#endregion

		#region Constructors
		/// <summary>
		/// No parameter constructor
		/// </summary>
		public AvataxAdapter()
		{
			InitializeAPIObjects();
			InitializeDefaults();
		}
		/// <summary>
		/// parameterized constructor may be necessary if any other class needs to suppply the config class
		/// </summary>
		public AvataxAdapter(TaxSvc svc)
		{
			InitializeAPIObjects(svc);
			InitializeDefaults();
		}
		#endregion

		#region public methods
		/// <summary>
		/// 
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="detailLevel">Summary = 0, Document = 1, Line = 2, Tax = 3, Diagnostic = 4</param>
		/// <param name="pingBeforeRequest"></param>
		/// <param name="taxRequestValues"></param>
		public GetTaxResult GetQuote(OrderCustomer orderCustomer, int detailLevel, bool pingBeforeRequest, Dictionary<string, string> taxRequestValues, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU, AvataxCalculationInfo avataxCalculationInfo)
		{
			List<OrderItem> orderItems = orderCustomer.ParentOrderItems;
			//use the direct ship location if the customer has a direct shipment, otherwise use the orders default ship location. 
			OrderShipment shipment = orderCustomer.OrderShipments.Any(os => os.IsDirectShipment) ? orderCustomer.OrderShipments.First(os => os.IsDirectShipment) : orderCustomer.Order.GetDefaultShipmentNoDefault();

			// Set default detailLevel if value is out of range
			if (detailLevel < 0 || detailLevel > 4)
				detailLevel = (int)DetailLevel.Tax;

			IAddress originAddress = SetOriginShippingAddress(orderCustomer);
			var isValidationSuccessful = ValidateAddress(shipment);
			if (!isValidationSuccessful)
			{
				return null;
			}

			InitializeAPIObjects();
			InitializeDefaults();
			Dictionary<string, string> _itemCodeIndexes = new Dictionary<string, string>();
			DataTable _data = new DataTable("Lines");
			GetTaxRequest getTaxRequest = SetUpTaxRequestData(orderCustomer, shipment, orderCustomer.OrderID.ToString(), DocumentType.SalesOrder, (DetailLevel)detailLevel, false, taxRequestValues, lineItemColumnValuesBySKU, _itemCodeIndexes, _data, originAddress);
			GetTaxResult getTaxResult = pingBeforeRequest ? SendGetTaxRequest(getTaxRequest) : SendGetTaxRequestNoPing(getTaxRequest);
			SetItemTaxes(orderItems, getTaxResult, _itemCodeIndexes, avataxCalculationInfo);
			return getTaxResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="detailLevel">Summary = 0, Document = 1, Line = 2, Tax = 3, Diagnostic = 4</param>
		/// <param name="commit"></param>
		/// <param name="pingBeforeRequest"></param>
		/// <param name="taxRequestValues"></param>
		public GetTaxResult SaveInvoice(OrderCustomer orderCustomer, int detailLevel, bool commit, bool pingBeforeRequest, Dictionary<string, string> taxRequestValues, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU)
		{
			List<OrderItem> orderItems = orderCustomer.ParentOrderItems;
			OrderShipment shipment = orderCustomer.Order.GetDefaultShipmentNoDefault();

			// Set default detailLevel if value is out of range
			if (detailLevel < 0 || detailLevel > 4)
				detailLevel = (int)DetailLevel.Tax;

			IAddress originAddress = SetOriginShippingAddress(orderCustomer);
			var isValidationSuccessful = ValidateAddress(shipment);

			if (isValidationSuccessful)
			{
				InitializeAPIObjects();
				InitializeDefaults();
				Dictionary<string, string> _itemCodeIndexes = new Dictionary<string, string>();
				DataTable _data = new DataTable("Lines");
				AvataxCalculationInfo avataxCalculationInfo = new AvataxCalculationInfo();

				// For PartyOrders there will be single Order tied to multiple customers. So generate unique orderID. 
				string orderID = (orderCustomer.Order.OrderTypeID == ConstantsGenerated.OrderType.PartyOrder.ToShort()) ? (orderCustomer.OrderID + "-" + orderCustomer.AccountID).ToString() :
					 orderCustomer.OrderID.ToString();

				GetTaxRequest getTaxRequest = SetUpTaxRequestData(orderCustomer, shipment, orderID, DocumentType.SalesInvoice, (DetailLevel)detailLevel, true, taxRequestValues, lineItemColumnValuesBySKU, _itemCodeIndexes, _data, originAddress);
				GetTaxResult getTaxResult = pingBeforeRequest ? SendGetTaxRequest(getTaxRequest) : SendGetTaxRequestNoPing(getTaxRequest);
				SetItemTaxes(orderItems, getTaxResult, _itemCodeIndexes, avataxCalculationInfo);
				return getTaxResult;
			}
			else
				return null;
		}

		/// <summary>
		/// This method sets the Origin address to Warehouse address.
		/// </summary>
		private IAddress SetOriginShippingAddress(OrderCustomer orderCustomer)
		{
			Address defaultShippingAddress = null;
			OrderShipment orderShipment = orderCustomer.Order.OrderShipments.FirstOrDefault(x => x.OrderCustomer == orderCustomer) ?? orderCustomer.Order.GetDefaultShipment();
			if (orderShipment == null)
			{
				defaultShippingAddress = orderCustomer.Order.Consultant.Addresses.FirstOrDefault(x => x.AddressTypeID == (short)NetSteps.Data.Entities.Constants.AddressType.Shipping);
				if (defaultShippingAddress != null)
				{
					orderShipment = new OrderShipment();
					Reflection.CopyPropertiesDynamic<IAddress, IAddress>(defaultShippingAddress, orderShipment);
				}
			}
			else
			{
				defaultShippingAddress = CopyAddressFromOrderShipment(orderShipment);
			}
			Warehouse warehouse = Warehouse.FindNearestByAddress(defaultShippingAddress);
			IAddress originAddress = SmallCollectionCache.Instance.Warehouses.GetById(warehouse.WarehouseID).Address;
			return originAddress;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="shipment"></param>
		/// <returns></returns>
		private Address CopyAddressFromOrderShipment(OrderShipment shipment)
		{
			//Do this to prevent translated values from being sent for State name
			var state = SmallCollectionCache.Instance.StateProvinces.GetById(shipment.StateProvinceID.Value);
			Address address = new Address();
			address.Address1 = shipment.Address1;
			address.Address2 = shipment.Address2;
			address.Address3 = shipment.Address3;
			address.City = shipment.City;
			address.County = shipment.County;
			address.State = state.Name;
			address.PostalCode = shipment.PostalCode;
			address.Country = shipment.Country;
			address.StateProvinceID = shipment.StateProvinceID;
			return address;

		}

		/// <summary>
		/// Setup GetTaxRequest with data to be sent to Avatax
		/// </summary>
		/// <param name="order"></param>
		/// <param name="orderCustomer"></param>
		/// <param name="documentType"></param>
		/// <param name="detailLevel"></param>
		/// <param name="commit"></param>
		public GetTaxRequest SetUpTaxRequestData(OrderCustomer orderCustomer, OrderShipment shipment, string orderId, DocumentType documentType, DetailLevel detailLevel, bool commit, Dictionary<string, string> taxRequestValues, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU, Dictionary<string, string> _itemCodeIndexes, DataTable _data, IAddress _originAddress)
		{
			try
			{
				Dictionary<string, string> taxRequestValuesNoErrors = SetDefaultTaxRequestColumnValues(taxRequestValues);

				GetTaxRequest _getTaxRequest = new GetTaxRequest();
				// Ship From
				_getTaxRequest.OriginAddress = !ValidateAddress(_originAddress) ? OriginAddressDefault : GetShippingAddressAvataxFormat(_originAddress);     //GetShipFromAddress(shipment);   //The default address on Avatax

				// Ship To            
				_getTaxRequest.DestinationAddress = GetShippingAddressAvataxFormat(shipment);

				SetupLineData(orderCustomer, lineItemColumnValuesBySKU, _itemCodeIndexes, _data);
				Line line;
				foreach (DataRow row in _data.Rows)
				{
					line = new Line();
					LoadLineItem(row, line);
					_getTaxRequest.Lines.Add(line);
				}
				line = null;

				_getTaxRequest.CompanyCode = avataxAPI.CompanyCode;                     //"netsteps";
				_getTaxRequest.DocCode = orderId;                            //"DOC0001"
				_getTaxRequest.DocDate = Util.GetDateFromString(taxRequestValuesNoErrors[Constants.TaxRequestColumns.DocDate.ToString()]);
				_getTaxRequest.DocType = documentType;
				_getTaxRequest.Discount = Util.GetDecimalFromString(taxRequestValuesNoErrors[Constants.TaxRequestColumns.Discount.ToString()]);
				_getTaxRequest.ExemptionNo = taxRequestValuesNoErrors[Constants.TaxRequestColumns.ExemptionNo.ToString()];
				_getTaxRequest.DetailLevel = detailLevel;
				_getTaxRequest.CustomerCode = orderCustomer.AccountID.ToString();
				_getTaxRequest.CustomerUsageType = taxRequestValuesNoErrors[Constants.TaxRequestColumns.CustomerUsageType.ToString()];
				_getTaxRequest.SalespersonCode = taxRequestValuesNoErrors[Constants.TaxRequestColumns.SalespersonCode.ToString()];
				_getTaxRequest.PurchaseOrderNo = taxRequestValuesNoErrors[Constants.TaxRequestColumns.PurchaseOrderNo.ToString()];
				_getTaxRequest.LocationCode = taxRequestValuesNoErrors[Constants.TaxRequestColumns.LocationCode.ToString()];            //: to clarify
				_getTaxRequest.Commit = commit;

				//tax override if any
				_getTaxRequest.TaxOverride.TaxOverrideType = (Avalara.AvaTax.Adapter.TaxService.TaxOverrideType)(int.Parse(taxRequestValuesNoErrors[Constants.TaxRequestColumns.TaxOverrideType.ToString()]));
				_getTaxRequest.TaxOverride.TaxAmount = Util.GetDecimalFromString(taxRequestValuesNoErrors[Constants.TaxRequestColumns.TaxAmount.ToString()]);
				_getTaxRequest.TaxOverride.TaxDate = Util.GetDateFromString(taxRequestValuesNoErrors[Constants.TaxRequestColumns.TaxDate.ToString()]);
				_getTaxRequest.TaxOverride.Reason = taxRequestValuesNoErrors[Constants.TaxRequestColumns.Reason.ToString()];
				_getTaxRequest.CurrencyCode = taxRequestValuesNoErrors[Constants.TaxRequestColumns.CurrencyCode.ToString()];

				return _getTaxRequest;
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public GetTaxResult SendGetTaxRequest(GetTaxRequest getTaxRequest)
		{
			try
			{
				PingResult pingResult = Ping();
				if (PingSuccess(pingResult))
				{
					GetTaxResult getTaxResult = avataxAPI.GetTax(getTaxRequest);
					return getTaxResult;
				}
				else
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(PingResultCode(pingResult) + " : " + PingResultMessage(pingResult));
				}
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public GetTaxResult SendGetTaxRequestNoPing(GetTaxRequest getTaxRequest)
		{
			try
			{
				GetTaxResult getTaxResult = avataxAPI.GetTax(getTaxRequest);
				return getTaxResult;
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public bool PingSuccess(PingResult pingResult)
		{
			try
			{
				return pingResult.ResultCode >= SeverityLevel.Error ? false : true;
				//TODO: log / return errormsg, Resultcode
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		public PingResult Ping()
		{
			try
			{
				PingResult pingResult = avataxAPI.PingAvatax();
				return pingResult;
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Avalara.AvaTax.Adapter.AddressService.Address GetOriginAddressDefault()
		{
			if (OriginAddressDefault == null)
			{
				OriginAddressDefault = new Avalara.AvaTax.Adapter.AddressService.Address();
				OriginAddressDefault.Line1 = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ORIGINADDRESSLINE1);
				OriginAddressDefault.Line2 = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ORIGINADDRESSLINE2);
				OriginAddressDefault.Line3 = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ORIGINADDRESSLINE3);
				OriginAddressDefault.City = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ORIGINADDRESSCITY);
				OriginAddressDefault.Region = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ORIGINADDRESSREGION);
				OriginAddressDefault.PostalCode = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ORIGINADDRESSPOSTALCODE);
				OriginAddressDefault.Country = Util.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_ORIGINADDRESSCOUNTRY);
			}

			return OriginAddressDefault;
		}

		/// <summary>
		/// PostTax [to succeed, the document must be in a state of saved.]
		/// </summary>
		/// <param name="docCode">orderId</param>
		/// <param name="totalAmount">total taxable amount</param>
		/// <param name="totalTax">total tax</param>
		/// <param name="newDocCode">to change the docCode</param>
		/// <param name="commit"></param>
		public PostTaxResult PostTax(string docCode, decimal totalAmount, decimal totalTax, string newDocCode, bool commit)
		{
			//avataxAPI = TaxAPIProvider.GetAvataxAPIInstance();
			PostTaxRequest postTaxRequest = new PostTaxRequest();

			postTaxRequest.CompanyCode = avataxAPI.CompanyCode;
			postTaxRequest.DocType = DocumentType.SalesInvoice;
			postTaxRequest.DocCode = docCode;

			postTaxRequest.DocDate = DateTime.Today;
			postTaxRequest.TotalAmount = totalAmount;
			postTaxRequest.TotalTax = totalTax;
			//_postTaxRequest.NewDocCode = newDocCode;
			postTaxRequest.Commit = commit;

			PostTaxResult postTaxResult = SendPostTaxRequest(postTaxRequest);
			return postTaxResult;
		}

		/// <summary>
		/// send postTax request
		/// </summary>
		/// <param name="postTaxRequest"></param>
		public PostTaxResult SendPostTaxRequest(PostTaxRequest postTaxRequest)
		{
			try
			{
				PingResult pingResult = Ping();
				if (PingSuccess(pingResult))
				{
					PostTaxResult postTaxResult = avataxAPI.PostTax(postTaxRequest);
					return postTaxResult;
				}
				else
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(PingResultCode(pingResult) + " : " + PingResultMessage(pingResult));
				}
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Commiting tax document [to succeed, the document must be in a state of posted]
		/// </summary>
		/// <param name="docCode"></param>
		/// <param name="newDocCode"></param>
		public CommitTaxResult CommitTax(string docCode, string newDocCode)
		{
			CommitTaxRequest commitTaxRequest = new CommitTaxRequest();

			commitTaxRequest.CompanyCode = avataxAPI.CompanyCode;
			commitTaxRequest.DocType = DocumentType.SalesInvoice;
			commitTaxRequest.DocCode = docCode;
			commitTaxRequest.NewDocCode = newDocCode;

			CommitTaxResult commitTaxResult = SendCommitTaxRequest(commitTaxRequest);
			return commitTaxResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="commitTaxRequest"></param>
		public CommitTaxResult SendCommitTaxRequest(CommitTaxRequest commitTaxRequest)
		{
			try
			{
				PingResult pingResult = Ping();
				if (PingSuccess(pingResult))
				{
					CommitTaxResult commitTaxResult = avataxAPI.CommitTax(commitTaxRequest);
					return commitTaxResult;
				}
				else
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(PingResultCode(pingResult) + " : " + PingResultMessage(pingResult));
				}
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="docCode"></param>
		/// <param name="cancelCode"></param>
		public CancelTaxResult CancelTax(string docCode, int cancelCode, int documentType)
		{
			CancelTaxRequest cancelTaxRequest = new CancelTaxRequest();

			cancelTaxRequest.CancelCode = (CancelCode)cancelCode;
			cancelTaxRequest.CompanyCode = avataxAPI.CompanyCode;
			cancelTaxRequest.DocType = (DocumentType)documentType;
			cancelTaxRequest.DocCode = docCode;

			CancelTaxResult cancelTaxResult = SendCancelTaxRequest(cancelTaxRequest);
			return cancelTaxResult;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancelTaxRequest"></param>
		public CancelTaxResult SendCancelTaxRequest(CancelTaxRequest cancelTaxRequest)
		{
			try
			{
				PingResult pingResult = Ping();
				if (PingSuccess(pingResult))
				{
					CancelTaxResult cancelTaxResult = avataxAPI.CancelTax(cancelTaxRequest);
					return cancelTaxResult;
				}
				else
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(PingResultCode(pingResult) + " : " + PingResultMessage(pingResult));
				}
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		#region Return Order
		/// <summary>
		/// Handling Return Invoices
		/// Assumptions:
		/// The document in question has already been committed and tax remitted to the tax jurisdictions.
		/// There may be multiple lines in the document.
		/// A complete or partial refund is the expected outcome.
		/// </summary>
		public GetTaxResult ReturnInvoice(Order order, Order parentOrder, OrderCustomer orderCustomer, Dictionary<string, string> taxRequestValues, bool commit)
		{
			InitializeAPIObjects();
			Dictionary<string, string> _itemCodeIndexes = new Dictionary<string, string>();
			DataTable _data = new DataTable("Lines");

			Dictionary<string, string> taxRequestValuesNoErrors = SetDefaultTaxRequestColumnValues(taxRequestValues);

			GetTaxRequest getTaxRequest = new GetTaxRequest();
			getTaxRequest.CompanyCode = avataxAPI.CompanyCode;
			getTaxRequest.CustomerCode = orderCustomer.AccountID.ToString();

			//Call a GetTax with a duplicate of the invoice you want to process returns on
			//re-use the original document's invoice number with a '.1' added to it
			//_getTaxRequest.DocCode = orderCustomer.Order.ParentOrderID.ToInt().ToString() + ".1";

			// Party orders may have returns for multiple customers, so ensure the doccode is unique for the customer. 
			getTaxRequest.DocCode = orderCustomer.OrderID.ToString() + "-" + orderCustomer.OrderCustomerID.ToString();

			//Set the DocDate to the tax reporting month that you want the return to appear in (typically the current month).
			getTaxRequest.DocDate = DateTime.UtcNow;

			getTaxRequest.DocType = DocumentType.ReturnInvoice;                //GetTax call for return order

			getTaxRequest.TaxOverride.TaxOverrideType = TaxOverrideType.TaxDate;
			getTaxRequest.TaxOverride.TaxDate = parentOrder != null ? parentOrder.DateCreated : orderCustomer.Order.DateCreated;
			getTaxRequest.TaxOverride.Reason = "Return Items";

			getTaxRequest.CustomerUsageType = taxRequestValuesNoErrors[Constants.TaxRequestColumns.CustomerUsageType.ToString()];

			//_getTaxRequest.Discount = Util.GetDecimalFromString(taxRequestValuesNoErrors[Constants.TaxRequestColumns.Discount.ToString()]);
			//_getTaxRequest.SalespersonCode = taxRequestValuesNoErrors[Constants.TaxRequestColumns.SalespersonCode.ToString()];
			//_getTaxRequest.PurchaseOrderNo = taxRequestValuesNoErrors[Constants.TaxRequestColumns.PurchaseOrderNo.ToString()];
			//_getTaxRequest.LocationCode = taxRequestValuesNoErrors[Constants.TaxRequestColumns.LocationCode.ToString()];

			//Set GetTaxRequest.ReferenceCode = to the original Invoice for tracking purposes.
			getTaxRequest.ReferenceCode = orderCustomer.Order.ParentOrderID.ToInt().ToString();
			getTaxRequest.Commit = commit;

			OrderShipment shipment = orderCustomer.Order.GetDefaultShipmentNoDefault();
			IAddress originAddress = SetOriginShippingAddress(orderCustomer);

			// Ship From
			getTaxRequest.OriginAddress = !ValidateAddress(originAddress) ? OriginAddressDefault : GetShippingAddressAvataxFormat(originAddress);     //GetShipFromAddress(shipment);   //The default address on Avatax
			// Ship To            
			getTaxRequest.DestinationAddress = GetShippingAddressAvataxFormat(shipment);

			//Setup lineitems for return Order
			SetupLineDataReturnOrder(order, parentOrder, orderCustomer, new Dictionary<string, Dictionary<string, string>>(), _itemCodeIndexes, _data);

			Line line;
			foreach (DataRow row in _data.Rows)
			{
				line = new Line();
				LoadLineItem(row, line);
				getTaxRequest.Lines.Add(line);
			}
			line = null;

			GetTaxResult getTaxResult = SendGetTaxRequest(getTaxRequest);
            return getTaxResult;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="returnOrderItems"></param>
		/// <param name="lineItemColumnValuesBySKU"></param>
		private void SetupLineDataReturnOrder(Order order, Order parentOrder, OrderCustomer orderCustomer, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			// 2012-03-09, JWL, Added DataTable.Clear
			if (_data != null)
			{
				_data.Clear();
			}

			int counter = 0;
			Dictionary<string, string> lineItemColumnValues = new Dictionary<string, string>();
			SetupLineColumns(_data);
			try
			{
				List<OrderItemReturn> returnOrderItems = order.OrderCustomers.SelectMany(x => x.OrderItems.SelectMany(o => o.OrderItemReturns)).ToList();

				foreach (OrderItemReturn returnOrderItem in returnOrderItems)
				{
					if (lineItemColumnValuesBySKU.ContainsKey(returnOrderItem.OrderItem.SKU))
						lineItemColumnValues = lineItemColumnValuesBySKU[returnOrderItem.OrderItem.SKU];

					AddLineItemToRowReturnOrder(returnOrderItem, counter, lineItemColumnValues, _itemCodeIndexes, _data);
					counter++;
				}

				//Get restockingFee for return order if any
				var restockingFeeItem =
					order.OrderCustomers.SelectMany(oc => oc.OrderItems)
						.FirstOrDefault(oi => oi.SKU.EqualsIgnoreCase(Constants.RESTOCKINGFEESKU));
				decimal restockingFee = restockingFeeItem != null ? restockingFeeItem.GetAdjustedPrice() * -1 : 0.0m;

				AddRestockingLineItemToRowReturnOrder(counter, 1, restockingFee, _itemCodeIndexes, _data);
				counter++;

				// Set shipping line item - return order
				// Consider the shipping total for the current order.
				AddShippingLineItemToRowReturnOrder(counter, returnOrderItems != null ? returnOrderItems.Count : 0, order.ShippingTotalOverride.ToDecimal(), _itemCodeIndexes, _data);

				// Increment the counter to avoid duplicate line number.
				counter++;

				// Set handling line item - return order
				AddHandlingLineItemToRowReturnOrder(counter, returnOrderItems != null ? returnOrderItems.Count : 0, order.HandlingTotal.ToDecimal(), _itemCodeIndexes, _data);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Add restocking fee to return/cancel order
		/// </summary>
		/// <param name="counter"></param>
		/// <param name="quantity"></param>
		/// <param name="restockingFees"></param>
		private void AddRestockingLineItemToRowReturnOrder(int counter, int quantity, decimal restockingFees, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			if (_itemCodeIndexes.ContainsKey(Constants.AVATAX_RESTOCKINGITEMCODE))
			{
				_itemCodeIndexes[Constants.AVATAX_RESTOCKINGITEMCODE] = counter.ToString();
			}
			else
			{
				_itemCodeIndexes.Add(Constants.AVATAX_RESTOCKINGITEMCODE, counter.ToString());
			}

			DataRow row = _data.NewRow();
			try
			{
				row[Constants.LineColumns.No.ToString()] = counter;
				row[Constants.LineColumns.ItemCode.ToString()] = Constants.AVATAX_RESTOCKINGITEMCODE;
				row[Constants.LineColumns.Qty.ToString()] = quantity;
				row[Constants.LineColumns.Amount.ToString()] = restockingFees;

				_data.Rows.Add(row);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Add product / orderItem to row
		/// </summary>
		/// <param name="orderItem"></param>
		/// <param name="counter"></param>
		private void AddLineItemToRowReturnOrder(OrderItemReturn returnOrderItem, int counter, Dictionary<string, string> lineItemColumnValues, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			var product = Product.Load((int)returnOrderItem.OrderItem.ProductID);

			// Save the counter and corresponding SKU in a dictionary for later reference
			// TaxLine returned by GetTaxResult does not contain ItemCode sent, hence items to be retrieved by Item No sent
			if (_itemCodeIndexes.ContainsKey(returnOrderItem.OrderItem.Guid.ToString()))
			{
				_itemCodeIndexes[returnOrderItem.OrderItem.Guid.ToString()] = counter.ToString();
			}
			else
			{
				_itemCodeIndexes.Add(returnOrderItem.OrderItem.Guid.ToString(), counter.ToString());
			}

			// set default lineItemColumn Values if not supplied
			Dictionary<string, string> lineItemColumnValuesNoErrors = SetDefaultLineItemColumnValues(lineItemColumnValues);

			DataRow row = _data.NewRow();
			try
			{
				row[Constants.LineColumns.No.ToString()] = counter;
				row[Constants.LineColumns.ItemCode.ToString()] = returnOrderItem.OrderItem.SKU;
				row[Constants.LineColumns.Qty.ToString()] = (Decimal)returnOrderItem.Quantity;

				//Set the Amt property to a negative dollar amount
				row[Constants.LineColumns.Amount.ToString()] = product.DoesChargeTax()
					? returnOrderItem.OrderItem.Taxes.TaxableTotal*returnOrderItem.Quantity*-1
					: 0m;

				row[Constants.LineColumns.Discounted.ToString()] = returnOrderItem.OrderItem.Discount != null ? true : false;
				row[Constants.LineColumns.Discount.ToString()] = returnOrderItem.OrderItem.Discount != null ? returnOrderItem.OrderItem.Discount.Value : 0.0M;

				row[Constants.LineColumns.ExemptionNo.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.ExemptionNo.ToString()];
				row[Constants.LineColumns.Reference1.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.Reference1.ToString()];
				row[Constants.LineColumns.Reference2.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.Reference2.ToString()];
				row[Constants.LineColumns.RevAcct.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.RevAcct.ToString()];
				row[Constants.LineColumns.TaxCode.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.TaxCode.ToString()];    //Let the default mapping on Avalara apply; //TODO:include TaxCode it as part of OrderItem
				row[Constants.LineColumns.CustomerUsageType.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.CustomerUsageType.ToString()];

				row[Constants.LineColumns.Description.ToString()] = returnOrderItem.OrderItem.ProductName;

				row[Constants.LineColumns.IsTaxOverriden.ToString()] = Util.GetBooleanFromString(lineItemColumnValuesNoErrors[Constants.LineColumns.IsTaxOverriden.ToString()]);   //convert to bool
				row[Constants.LineColumns.TaxOverride.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.TaxOverride.ToString()];
				row[Constants.LineColumns.TaxOverrideType.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.TaxOverrideType.ToString()];
				row[Constants.LineColumns.TaxAmount.ToString()] = Util.GetDecimalFromString(lineItemColumnValuesNoErrors[Constants.LineColumns.TaxAmount.ToString()]);  //convert to decimal
				row[Constants.LineColumns.TaxDate.ToString()] = Util.GetDateFromString(lineItemColumnValuesNoErrors[Constants.LineColumns.TaxDate.ToString()]);         //DateTime.Today.ToShortDateString();
				row[Constants.LineColumns.Reason.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.Reason.ToString()];


				_data.Rows.Add(row);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="counter"></param>
		/// <param name="orderCustomer"></param>
		private void AddShippingLineItemToRowReturnOrder(int counter, int quantity, decimal shippingTotal, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			if (_itemCodeIndexes.ContainsKey(Constants.AVATAX_SHIPPINGITEMCODE))
			{
				_itemCodeIndexes[Constants.AVATAX_SHIPPINGITEMCODE] = counter.ToString();
			}
			else
			{
				_itemCodeIndexes.Add(Constants.AVATAX_SHIPPINGITEMCODE, counter.ToString());
			}

			DataRow row = _data.NewRow();
			try
			{
				row[Constants.LineColumns.No.ToString()] = counter;
				row[Constants.LineColumns.ItemCode.ToString()] = Constants.AVATAX_SHIPPINGITEMCODE;
				row[Constants.LineColumns.Qty.ToString()] = quantity;
				row[Constants.LineColumns.Amount.ToString()] = shippingTotal;
				row[Constants.LineColumns.TaxCode.ToString()] = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_SHIPPINGTAXCODE);

				_data.Rows.Add(row);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="counter"></param>
		/// <param name="orderCustomer"></param>
		private void AddHandlingLineItemToRowReturnOrder(int counter, int quantity, decimal handlingTotal, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			if (_itemCodeIndexes.ContainsKey(Constants.AVATAX_HANDLINGITEMCODE))
			{
				_itemCodeIndexes[Constants.AVATAX_HANDLINGITEMCODE] = counter.ToString();
			}
			else
			{
				_itemCodeIndexes.Add(Constants.AVATAX_HANDLINGITEMCODE, counter.ToString());
			}

			DataRow row = _data.NewRow();
			try
			{
				row[Constants.LineColumns.No.ToString()] = counter;
				row[Constants.LineColumns.ItemCode.ToString()] = Constants.AVATAX_HANDLINGITEMCODE;
				row[Constants.LineColumns.Qty.ToString()] = quantity;
				row[Constants.LineColumns.Amount.ToString()] = quantity * handlingTotal * (-1);
				row[Constants.LineColumns.TaxCode.ToString()] = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_HANDLINGTAXCODE);

				_data.Rows.Add(row);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cancelTaxRequest"></param>
		public AdjustTaxResult SendAdjustTaxRequest(AdjustTaxRequest adjustTaxRequest)
		{
			try
			{
				PingResult pingResult = Ping();
				if (PingSuccess(pingResult))
				{
					AdjustTaxResult adjustTaxResult = avataxAPI.AdjustTax(adjustTaxRequest);
					return adjustTaxResult;
				}
				else
				{
					throw EntityExceptionHelper.GetAndLogNetStepsException(PingResultCode(pingResult) + " : " + PingResultMessage(pingResult));
				}
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}
		#endregion

		/// <summary>         
		///
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			// This object will be cleaned up by the Dispose method.
			// Therefore, you should call GC.SupressFinalize to
			// take this object off the finalization queue
			// and prevent finalization code for this object
			// from executing a second time.
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// http://msdn.microsoft.com/en-us/library/system.idisposable.aspx
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			// Check to see if Dispose has already been called.
			if (!this.disposed)
			{
				disposed = true;
			}
		}
		#endregion

		#region private methods
		/// <summary>
		/// 
		/// </summary>
		private void InitializeAPIObjects()
		{
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitializeAPIObjects(TaxSvc svc)
		{
			avataxAPI.SetTaxSvc(svc);
		}

		/// <summary>
		/// 
		/// </summary>
		private void InitializeDefaults()
		{
			this.OriginAddressDefault = GetOriginAddressDefault();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="taxRequestValues"></param>
		/// <returns></returns>
		private Dictionary<string, string> SetDefaultTaxRequestColumnValues(Dictionary<string, string> taxRequestValues)
		{
			Dictionary<string, string> returnValue = new Dictionary<string, string>();
			try
			{
				returnValue.Add(Constants.TaxRequestColumns.DocDate.ToString(), DateTime.Today.ToShortDateString());
				returnValue.Add(Constants.TaxRequestColumns.Discount.ToString(), "0.0");
				returnValue.Add(Constants.TaxRequestColumns.ExemptionNo.ToString(), "");
				returnValue.Add(Constants.TaxRequestColumns.CustomerUsageType.ToString(), "");
				returnValue.Add(Constants.TaxRequestColumns.SalespersonCode.ToString(), "");
				returnValue.Add(Constants.TaxRequestColumns.PurchaseOrderNo.ToString(), "");
				returnValue.Add(Constants.TaxRequestColumns.LocationCode.ToString(), "");
				returnValue.Add(Constants.TaxRequestColumns.TaxOverrideType.ToString(), ((int)TaxOverrideType.None).ToString());
				returnValue.Add(Constants.TaxRequestColumns.TaxAmount.ToString(), "0.0");
				returnValue.Add(Constants.TaxRequestColumns.TaxDate.ToString(), DateTime.Today.ToShortDateString());
				returnValue.Add(Constants.TaxRequestColumns.Reason.ToString(), "");
				returnValue.Add(Constants.TaxRequestColumns.CurrencyCode.ToString(), "");

				returnValue[Constants.TaxRequestColumns.DocDate.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.DocDate.ToString()) ? returnValue[Constants.TaxRequestColumns.DocDate.ToString()] : taxRequestValues[Constants.TaxRequestColumns.Discount.ToString()];
				returnValue[Constants.TaxRequestColumns.Discount.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.Discount.ToString()) ? returnValue[Constants.TaxRequestColumns.Discount.ToString()] : taxRequestValues[Constants.TaxRequestColumns.Discount.ToString()];
				returnValue[Constants.TaxRequestColumns.ExemptionNo.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.ExemptionNo.ToString()) ? returnValue[Constants.TaxRequestColumns.ExemptionNo.ToString()] : taxRequestValues[Constants.TaxRequestColumns.ExemptionNo.ToString()];
				returnValue[Constants.TaxRequestColumns.CustomerUsageType.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.CustomerUsageType.ToString()) ? returnValue[Constants.TaxRequestColumns.CustomerUsageType.ToString()] : taxRequestValues[Constants.TaxRequestColumns.CustomerUsageType.ToString()];
				returnValue[Constants.TaxRequestColumns.SalespersonCode.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.SalespersonCode.ToString()) ? returnValue[Constants.TaxRequestColumns.SalespersonCode.ToString()] : taxRequestValues[Constants.TaxRequestColumns.SalespersonCode.ToString()];
				returnValue[Constants.TaxRequestColumns.PurchaseOrderNo.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.PurchaseOrderNo.ToString()) ? returnValue[Constants.TaxRequestColumns.PurchaseOrderNo.ToString()] : taxRequestValues[Constants.TaxRequestColumns.PurchaseOrderNo.ToString()];
				returnValue[Constants.TaxRequestColumns.LocationCode.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.LocationCode.ToString()) ? returnValue[Constants.TaxRequestColumns.LocationCode.ToString()] : taxRequestValues[Constants.TaxRequestColumns.LocationCode.ToString()];
				returnValue[Constants.TaxRequestColumns.TaxOverrideType.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.TaxOverrideType.ToString()) ? returnValue[Constants.TaxRequestColumns.TaxOverrideType.ToString()] : taxRequestValues[Constants.TaxRequestColumns.TaxOverrideType.ToString()];
				returnValue[Constants.TaxRequestColumns.TaxAmount.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.TaxAmount.ToString()) ? returnValue[Constants.TaxRequestColumns.TaxAmount.ToString()] : taxRequestValues[Constants.TaxRequestColumns.TaxAmount.ToString()];
				returnValue[Constants.TaxRequestColumns.TaxDate.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.TaxDate.ToString()) ? returnValue[Constants.TaxRequestColumns.TaxDate.ToString()] : taxRequestValues[Constants.TaxRequestColumns.TaxDate.ToString()];
				returnValue[Constants.TaxRequestColumns.Reason.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.Reason.ToString()) ? returnValue[Constants.TaxRequestColumns.Reason.ToString()] : taxRequestValues[Constants.TaxRequestColumns.Reason.ToString()];
				returnValue[Constants.TaxRequestColumns.CurrencyCode.ToString()] = !taxRequestValues.ContainsKey(Constants.TaxRequestColumns.CurrencyCode.ToString()) ? returnValue[Constants.TaxRequestColumns.CurrencyCode.ToString()] : taxRequestValues[Constants.TaxRequestColumns.CurrencyCode.ToString()];
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
			return returnValue;
		}

		/// <summary>
		/// Add products to Lineitem collection to be sent to Avatax
		/// </summary>
		/// <param name="orderCustomer"></param>
		private void SetupLineData(OrderCustomer orderCustomer, Dictionary<string, Dictionary<string, string>> lineItemColumnValuesBySKU, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			// 2012-03-09, JWL, Added DataTable.Clear
			_data.Clear();

			int counter = 0;
			Dictionary<string, string> lineItemColumnValues = new Dictionary<string, string>();
			SetupLineColumns(_data);
			try
			{
				foreach (OrderItem orderItem in orderCustomer.ParentOrderItems)
				{
					if (lineItemColumnValuesBySKU.ContainsKey(orderItem.SKU))
						lineItemColumnValues = lineItemColumnValuesBySKU[orderItem.SKU];

					AddLineItemToRow(orderItem, counter, lineItemColumnValues, _itemCodeIndexes, _data);
					counter++;

					// Child items are being included as part of bundle/kit - as per Existing logic in BaseTaxService
				}

				//Add a line item to fetch shipping tax rate                
				AddShippingLineItemToRow(counter, orderCustomer, _itemCodeIndexes, _data);
				counter++;

				//Add a line item for handling charges               
				AddHandlingLineItemToRow(counter, orderCustomer, _itemCodeIndexes, _data);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Add product / orderItem to row
		/// </summary>
		/// <param name="orderItem"></param>
		/// <param name="counter"></param>
		private void AddLineItemToRow(OrderItem orderItem, int counter, Dictionary<string, string> lineItemColumnValues, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			var invRep = Create.New<InventoryBaseRepository>();
			var product = invRep.GetProduct(orderItem.ProductID.GetValueOrDefault());

			// Save the counter and corresponding SKU in a dictionary for later reference
			// TaxLine returned by GetTaxResult does not contain ItemCode sent, hence items to be retrieved by Item No sent
			if (_itemCodeIndexes.ContainsKey(orderItem.Guid.ToString()))
			{
				_itemCodeIndexes[orderItem.Guid.ToString()] = counter.ToString();
			}
			else
			{
				_itemCodeIndexes.Add(orderItem.Guid.ToString(), counter.ToString());
			}

			// set default lineItemColumn Values if not supplied
			Dictionary<string, string> lineItemColumnValuesNoErrors = SetDefaultLineItemColumnValues(lineItemColumnValues);

			DataRow row = _data.NewRow();
			try
			{
				row[Constants.LineColumns.No.ToString()] = counter;
				row[Constants.LineColumns.ItemCode.ToString()] = orderItem.SKU;
				row[Constants.LineColumns.Qty.ToString()] = (Decimal)orderItem.Quantity;
				row[Constants.LineColumns.Amount.ToString()] = orderItem.Taxes.TaxableTotal;
				row[Constants.LineColumns.ExemptionNo.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.ExemptionNo.ToString()];
				row[Constants.LineColumns.Reference1.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.Reference1.ToString()];
				row[Constants.LineColumns.Reference2.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.Reference2.ToString()];
				row[Constants.LineColumns.RevAcct.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.RevAcct.ToString()];
				row[Constants.LineColumns.TaxCode.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.TaxCode.ToString()];    //Let the default mapping on Avalara apply; //TODO:include TaxCode it as part of OrderItem
				row[Constants.LineColumns.CustomerUsageType.ToString()] = lineItemColumnValuesNoErrors[Constants.LineColumns.CustomerUsageType.ToString()];
				row[Constants.LineColumns.Description.ToString()] = orderItem.ProductName;

				// non-taxable
				var isTaxOverriden =
					 Util.GetBooleanFromString(
						  lineItemColumnValuesNoErrors[Constants.LineColumns.IsTaxOverriden.ToString()]);
				var taxOverride = lineItemColumnValuesNoErrors[Constants.LineColumns.TaxOverride.ToString()];
				var taxOverrideType = lineItemColumnValuesNoErrors[Constants.LineColumns.TaxOverrideType.ToString()];
				var taxAmount = Util.GetDecimalFromString(lineItemColumnValuesNoErrors[Constants.LineColumns.TaxAmount.ToString()]);
				var taxDate = Util.GetDateFromString(lineItemColumnValuesNoErrors[Constants.LineColumns.TaxDate.ToString()]);         //DateTime.Today.ToShortDateString()
				var taxReason = lineItemColumnValuesNoErrors[Constants.LineColumns.Reason.ToString()];
				var taxCode = lineItemColumnValuesNoErrors[Constants.LineColumns.TaxCode.ToString()];

				if (!product.DoesChargeTax())
				{
					isTaxOverriden = true;
					taxOverrideType = TaxOverrideType.Exemption.ToString();
					taxReason = "No Tax";
					taxAmount = 0;
					taxCode = "NT";
				}

				row[Constants.LineColumns.IsTaxOverriden.ToString()] = isTaxOverriden;   //convert to bool
				row[Constants.LineColumns.TaxOverride.ToString()] = taxOverride;
				row[Constants.LineColumns.TaxOverrideType.ToString()] = taxOverrideType;
				row[Constants.LineColumns.TaxAmount.ToString()] = taxAmount;
				row[Constants.LineColumns.TaxDate.ToString()] = taxDate;
				row[Constants.LineColumns.Reason.ToString()] = taxReason;
				row[Constants.LineColumns.TaxCode.ToString()] = taxCode;

				_data.Rows.Add(row);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Adds shipping line item to get shipping tax rate
		/// </summary>
		private void AddShippingLineItemToRow(int counter, OrderCustomer orderCustomer, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			if (_itemCodeIndexes.ContainsKey(Constants.AVATAX_SHIPPINGITEMCODE))
			{
				_itemCodeIndexes[Constants.AVATAX_SHIPPINGITEMCODE] = counter.ToString();
			}
			else
			{
				_itemCodeIndexes.Add(Constants.AVATAX_SHIPPINGITEMCODE, counter.ToString());
			}

			DataRow row = _data.NewRow();
			try
			{
				row[Constants.LineColumns.No.ToString()] = counter;
				row[Constants.LineColumns.ItemCode.ToString()] = Constants.AVATAX_SHIPPINGITEMCODE;
				row[Constants.LineColumns.Qty.ToString()] = orderCustomer.ParentOrderItems.Count;

				// Shipping total for Party
				if (orderCustomer.Order.OrderTypeID == ConstantsGenerated.OrderType.PartyOrder.ToShort() && !orderCustomer.OrderShipments.Any(os => os.IsDirectShipment) && !orderCustomer.Order.ShouldDividePartyShipping)
				{
					row[Constants.LineColumns.Amount.ToString()] = orderCustomer.Order.PartyShipmentTotal.ToDecimal();
				}
				else
				{
					row[Constants.LineColumns.Amount.ToString()] = orderCustomer.ShippingTotal.ToDecimal();
				}

				row[Constants.LineColumns.TaxCode.ToString()] = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_SHIPPINGTAXCODE);

				_data.Rows.Add(row);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="counter"></param>
		/// <param name="orderCustomer"></param>
		private void AddHandlingLineItemToRow(int counter, OrderCustomer orderCustomer, Dictionary<string, string> _itemCodeIndexes, DataTable _data)
		{
			if (_itemCodeIndexes.ContainsKey(Constants.AVATAX_HANDLINGITEMCODE))
			{
				_itemCodeIndexes[Constants.AVATAX_HANDLINGITEMCODE] = counter.ToString();
			}
			else
			{
				_itemCodeIndexes.Add(Constants.AVATAX_HANDLINGITEMCODE, counter.ToString());
			}

			DataRow row = _data.NewRow();
			try
			{
				row[Constants.LineColumns.No.ToString()] = counter;
				row[Constants.LineColumns.ItemCode.ToString()] = Constants.AVATAX_HANDLINGITEMCODE;
				row[Constants.LineColumns.Qty.ToString()] = orderCustomer.ParentOrderItems.Count;
				row[Constants.LineColumns.Amount.ToString()] = orderCustomer.HandlingTotal.ToDecimal();

				row[Constants.LineColumns.TaxCode.ToString()] = ConfigurationManager.GetConfigValues(Constants.AVATAX_CONFIGSECTION, Constants.AVATAX_HANDLINGTAXCODE);

				_data.Rows.Add(row);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Fill line with data from row
		/// </summary>
		/// <param name="row"></param>
		/// <param name="line"></param>
		private void LoadLineItem(DataRow row, Line line)
		{
			try
			{
				line.No = row[Constants.LineColumns.No.ToString()].ToString();
				line.ItemCode = row[Constants.LineColumns.ItemCode.ToString()].ToString();
				line.Qty = (!Convert.IsDBNull(row[Constants.LineColumns.Qty.ToString()])) ? Convert.ToDouble(row[Constants.LineColumns.Qty.ToString()]) : 0;
				line.Amount = (!Convert.IsDBNull(row[Constants.LineColumns.Amount.ToString()])) ? Convert.ToDecimal(row[Constants.LineColumns.Amount.ToString()]) : 0;
				line.Discounted = (!Convert.IsDBNull(row[Constants.LineColumns.Discounted.ToString()])) ? Convert.ToBoolean(row[Constants.LineColumns.Discounted.ToString()]) : false;

				//line.Discount = Convert.ToDecimal(row[Constants.LineColumns.Discount.ToString()]);

				line.ExemptionNo = row[Constants.LineColumns.ExemptionNo.ToString()].ToString();
				line.Ref1 = row[Constants.LineColumns.Reference1.ToString()].ToString();
				line.Ref2 = row[Constants.LineColumns.Reference2.ToString()].ToString();
				line.RevAcct = row[Constants.LineColumns.RevAcct.ToString()].ToString();
				line.TaxCode = row[Constants.LineColumns.TaxCode.ToString()].ToString();
				line.CustomerUsageType = row[Constants.LineColumns.CustomerUsageType.ToString()].ToString();
				line.Description = row[Constants.LineColumns.Description.ToString()].ToString();

				line.TaxOverride.TaxOverrideType = Util.GetTaxOverrideType(row[Constants.LineColumns.TaxOverrideType.ToString()].ToString());
				line.TaxOverride.TaxAmount = Util.GetDecimalFromString(row[Constants.LineColumns.TaxAmount.ToString()].ToString());
				line.TaxOverride.TaxDate = (!Convert.IsDBNull(row[Constants.LineColumns.TaxDate.ToString()])) ? DateTime.Parse(row[Constants.LineColumns.TaxDate.ToString()].ToString()) : DateTime.Now;
				line.TaxOverride.Reason = row[Constants.LineColumns.Reason.ToString()].ToString();
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		/// <summary>
		/// Sets up the column names and headers for table: _data
		/// </summary>
		private void SetupLineColumns(DataTable _data)
		{
			if (_data.Columns.Count > 0)
			{
				_data.Columns.Clear();
			}

			DataColumn col;

			col = new DataColumn(Constants.LineColumns.No.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.No.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.ItemCode.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.ItemCode.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.Qty.ToString(), typeof(Decimal));
			col.Caption = Constants.LineColumns.Qty.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.Amount.ToString(), typeof(Decimal));
			col.Caption = Constants.LineColumns.Amount.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.Discounted.ToString(), typeof(bool));
			col.Caption = Constants.LineColumns.Discounted.ToString();
			_data.Columns.Add(col);
			col.DefaultValue = false;

			col = new DataColumn(Constants.LineColumns.Discount.ToString(), typeof(Decimal));
			col.Caption = Constants.LineColumns.Discount.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.ExemptionNo.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.ExemptionNo.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.Reference1.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.Reference1.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.Reference2.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.Reference2.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.RevAcct.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.RevAcct.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.TaxCode.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.TaxCode.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.CustomerUsageType.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.CustomerUsageType.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.Description.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.Description.ToString();
			_data.Columns.Add(col);

			//Added for 5.0
			col = new DataColumn(Constants.LineColumns.IsTaxOverriden.ToString(), typeof(bool));
			col.Caption = Constants.LineColumns.IsTaxOverriden.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.TaxOverride.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.TaxOverride.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.TaxOverrideType.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.TaxOverrideType.ToString();
			col.DefaultValue = "None";
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.Reason.ToString(), typeof(string));
			col.Caption = Constants.LineColumns.Reason.ToString();
			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.TaxAmount.ToString(), typeof(Decimal));
			col.Caption = Constants.LineColumns.TaxAmount.ToString();

			_data.Columns.Add(col);

			col = new DataColumn(Constants.LineColumns.TaxDate.ToString(), typeof(string));//DateTime
			col.Caption = Constants.LineColumns.TaxDate.ToString();
			_data.Columns.Add(col);

		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="NetStepsaddress"></param>
		/// <returns></returns>
		private Avalara.AvaTax.Adapter.AddressService.Address GetShippingAddressAvataxFormat(IAddress NetStepsaddress)
		{
			//Error handling: return default origin address if OriginAddress property not set by the caller
			if (NetStepsaddress == null)
				return GetOriginAddressDefault();

			Avalara.AvaTax.Adapter.AddressService.Address returnaddress = new Avalara.AvaTax.Adapter.AddressService.Address();
			returnaddress.Line1 = NetStepsaddress.Address1;
			returnaddress.Line2 = NetStepsaddress.Address2;
			returnaddress.Line3 = NetStepsaddress.Address3;
			returnaddress.City = NetStepsaddress.City;

			//Do this to prevent translated values from being sent for State name
			var state = SmallCollectionCache.Instance.StateProvinces.GetById(NetStepsaddress.StateProvinceID.Value);
			returnaddress.Region = state.Name;               //mismatch

			returnaddress.PostalCode = NetStepsaddress.PostalCode;

			//Do this to prevent translated values from being sent for Country name
			var country = SmallCollectionCache.Instance.Countries.GetById(NetStepsaddress.CountryID);
			returnaddress.Country = country.Name;

			return returnaddress;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="lineItemColumnValues"></param>
		/// <returns></returns>
		private Dictionary<string, string> SetDefaultLineItemColumnValues(Dictionary<string, string> lineItemColumnValues)
		{
			Dictionary<string, string> returnValue = new Dictionary<string, string>();
			try
			{
				returnValue.Add(Constants.LineColumns.ExemptionNo.ToString(), "");
				returnValue.Add(Constants.LineColumns.Reference1.ToString(), "");
				returnValue.Add(Constants.LineColumns.Reference2.ToString(), "");
				returnValue.Add(Constants.LineColumns.RevAcct.ToString(), "");
				returnValue.Add(Constants.LineColumns.TaxCode.ToString(), "");
				returnValue.Add(Constants.LineColumns.CustomerUsageType.ToString(), "");
				returnValue.Add(Constants.LineColumns.IsTaxOverriden.ToString(), "false");
				returnValue.Add(Constants.LineColumns.TaxOverride.ToString(), "0.0");
				returnValue.Add(Constants.LineColumns.TaxOverrideType.ToString(), ((int)TaxOverrideType.None).ToString());
				returnValue.Add(Constants.LineColumns.TaxAmount.ToString(), "0.0");
				returnValue.Add(Constants.LineColumns.TaxDate.ToString(), DateTime.Today.ToShortDateString());
				returnValue.Add(Constants.LineColumns.Reason.ToString(), "");

				returnValue[Constants.LineColumns.ExemptionNo.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.ExemptionNo.ToString()) ? returnValue[Constants.LineColumns.ExemptionNo.ToString()] : lineItemColumnValues[Constants.LineColumns.ExemptionNo.ToString()];
				returnValue[Constants.LineColumns.Reference1.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.Reference1.ToString()) ? returnValue[Constants.LineColumns.Reference1.ToString()] : lineItemColumnValues[Constants.LineColumns.Reference1.ToString()];
				returnValue[Constants.LineColumns.Reference2.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.Reference2.ToString()) ? returnValue[Constants.LineColumns.Reference2.ToString()] : lineItemColumnValues[Constants.LineColumns.Reference2.ToString()];
				returnValue[Constants.LineColumns.RevAcct.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.RevAcct.ToString()) ? returnValue[Constants.LineColumns.RevAcct.ToString()] : lineItemColumnValues[Constants.LineColumns.RevAcct.ToString()];
				returnValue[Constants.LineColumns.TaxCode.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.TaxCode.ToString()) ? returnValue[Constants.LineColumns.TaxCode.ToString()] : lineItemColumnValues[Constants.LineColumns.TaxCode.ToString()];
				returnValue[Constants.LineColumns.CustomerUsageType.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.CustomerUsageType.ToString()) ? returnValue[Constants.LineColumns.CustomerUsageType.ToString()] : lineItemColumnValues[Constants.LineColumns.CustomerUsageType.ToString()];
				returnValue[Constants.LineColumns.IsTaxOverriden.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.IsTaxOverriden.ToString()) ? returnValue[Constants.LineColumns.IsTaxOverriden.ToString()] : lineItemColumnValues[Constants.LineColumns.IsTaxOverriden.ToString()];
				returnValue[Constants.LineColumns.TaxOverride.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.TaxOverride.ToString()) ? returnValue[Constants.LineColumns.TaxOverride.ToString()] : lineItemColumnValues[Constants.LineColumns.TaxOverride.ToString()];
				returnValue[Constants.LineColumns.TaxOverrideType.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.TaxOverrideType.ToString()) ? returnValue[Constants.LineColumns.TaxOverrideType.ToString()] : lineItemColumnValues[Constants.LineColumns.TaxOverrideType.ToString()];
				returnValue[Constants.LineColumns.TaxAmount.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.TaxAmount.ToString()) ? returnValue[Constants.LineColumns.TaxAmount.ToString()] : lineItemColumnValues[Constants.LineColumns.TaxAmount.ToString()];
				returnValue[Constants.LineColumns.TaxDate.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.TaxDate.ToString()) ? returnValue[Constants.LineColumns.TaxDate.ToString()] : lineItemColumnValues[Constants.LineColumns.TaxDate.ToString()];
				returnValue[Constants.LineColumns.Reason.ToString()] = !lineItemColumnValues.ContainsKey(Constants.LineColumns.Reason.ToString()) ? returnValue[Constants.LineColumns.Reason.ToString()] : lineItemColumnValues[Constants.LineColumns.Reason.ToString()];
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
			return returnValue;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		private bool ValidateAddress(OrderShipment orderShipment)
		{
			if (orderShipment == null)
			{
				return false;
			}

			bool IsCityAvailable = !string.IsNullOrEmpty(orderShipment.City);
			bool IsStateAvailable = !string.IsNullOrEmpty(orderShipment.State);
			bool IsPostalCodeAvailable = !string.IsNullOrEmpty(orderShipment.PostalCode);

			return (IsCityAvailable && IsStateAvailable && IsPostalCodeAvailable);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="orderShipment"></param>
		private bool ValidateAddress(IAddress address)
		{
			if (address == null)
			{
				return false;
			}

			bool IsCityAvailable = !string.IsNullOrEmpty(address.City);
			bool IsStateAvailable = !string.IsNullOrEmpty(address.State);
			bool IsPostalCodeAvailable = !string.IsNullOrEmpty(address.PostalCode);

			return (IsCityAvailable && IsStateAvailable && IsPostalCodeAvailable);
		}
		#endregion

		#region map API data to netsteps data

		private void SetItemTaxes(List<OrderItem> orderItems, GetTaxResult getTaxResult, Dictionary<string, string> _itemCodeIndexes, AvataxCalculationInfo avataxCalculationInfo)
		{
			if (getTaxResult == null)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException("TaxResult is null", NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
			try
			{
				foreach (OrderItem orderItem in orderItems)
				{
					CallSetTaxToOrderItem(orderItem, getTaxResult, _itemCodeIndexes);
				}

				SetShippingTax(getTaxResult, _itemCodeIndexes, avataxCalculationInfo);
				SetHandlingTax(getTaxResult, _itemCodeIndexes, avataxCalculationInfo);
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		private void CallSetTaxToOrderItem(OrderItem orderItem, GetTaxResult getTaxResult, Dictionary<string, string> _itemCodeIndexes)
		{
			int itemIndex = Util.GetIntFromString(_itemCodeIndexes[orderItem.Guid.ToString()]);
			if (getTaxResult.TaxLines.Count > 0)
			{
				TaxLine taxLine = getTaxResult.TaxLines[itemIndex];

				if (taxLine != null)
				{
					SetTaxToOrderItem(orderItem, taxLine);
				}
			}
		}

		private void SetTaxToOrderItem(OrderItem orderItem, TaxLine taxLine)
		{
			try
			{
				orderItem.Taxes.TaxPercent = (decimal)taxLine.Rate;     //tax rate saved to tax percent            

				//TaxableTotal is given from Encore; Avatax returns the tax calculated on it as well as the rates
				//Avatax takes into account the taxability criteria such as:
				//Exempt code(customerUsageType), From-To address, nexus, non-taxability, tax-override etc.

				orderItem.Taxes.TaxableTotal = taxLine.Taxable;
				orderItem.Taxes.TaxAmountTotal = taxLine.TaxCalculated;

				foreach (TaxDetail taxDetail in taxLine.TaxDetails)
				{
					switch (taxDetail.JurisType)
					{
						case JurisdictionType.State:
							orderItem.Taxes.TaxPercentState = (decimal)taxDetail.Rate;
							break;
						case JurisdictionType.City:
							orderItem.Taxes.TaxPercentCity = (decimal)taxDetail.Rate;
							break;
						case JurisdictionType.County:
							orderItem.Taxes.TaxPercentCounty = (decimal)taxDetail.Rate;
							break;
						case JurisdictionType.Special:
							orderItem.Taxes.TaxPercentDistrict = (decimal)taxDetail.Rate;     //to clarify
							break;
					}
				}
			}
			catch (Exception e)
			{
				throw EntityExceptionHelper.GetAndLogNetStepsException(e, NetStepsException.Constants.NetStepsExceptionType.NetStepsBusinessException);
			}
		}

		private void SetShippingTax(GetTaxResult getTaxResult, Dictionary<string, string> _itemCodeIndexes, AvataxCalculationInfo avataxCalculationInfo)
		{
			int itemIndex = Util.GetIntFromString(_itemCodeIndexes[Constants.AVATAX_SHIPPINGITEMCODE]);
			if (getTaxResult.TaxLines.Count > 0)
			{
				TaxLine taxLine = getTaxResult.TaxLines[itemIndex];

				if (taxLine != null)
				{
					// Use the Shipping tax.
					avataxCalculationInfo.ShippingTaxable = taxLine.Taxable;
					avataxCalculationInfo.ShippingTax = taxLine.Tax;
					avataxCalculationInfo.ShippingTaxRate = taxLine.Rate;
				}
			}
		}

		private void SetHandlingTax(GetTaxResult getTaxResult, Dictionary<string, string> _itemCodeIndexes, AvataxCalculationInfo avataxCalculationInfo)
		{
			int itemIndex = Util.GetIntFromString(_itemCodeIndexes[Constants.AVATAX_HANDLINGITEMCODE]);
			if (getTaxResult.TaxLines.Count > 0)
			{
				TaxLine taxLine = getTaxResult.TaxLines[itemIndex];

				if (taxLine != null)
				{
					// Use the Shipping tax.
					avataxCalculationInfo.HandlingTax = taxLine.Tax;
					avataxCalculationInfo.HandlingTaxRate = taxLine.Rate;
					avataxCalculationInfo.HandlingTaxable = taxLine.Taxable;
				}
			}
		}
		#endregion
	}
}

