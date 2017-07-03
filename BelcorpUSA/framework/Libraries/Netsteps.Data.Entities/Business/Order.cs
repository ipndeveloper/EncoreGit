#region Using
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using NetSteps.Commissions.Common;
using NetSteps.Common.Base;
using NetSteps.Common.Collections;
using NetSteps.Common.Exceptions;
using NetSteps.Common.Extensions;
using NetSteps.Common.Interfaces;
using NetSteps.Common.Reflection;
using NetSteps.Data.Common.Entities;
using NetSteps.Data.Common.Models;
using NetSteps.Data.Common.Services;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.HelperObjects.SearchData;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Business.Logic;
using NetSteps.Data.Entities.Business.Logic.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Generated;
using NetSteps.Data.Entities.Interfaces;
using NetSteps.Data.Entities.Services;
using NetSteps.Data.Entities.Tax;
using NetSteps.Encore.Core.IoC;
using NetSteps.Extensibility.Core;
using NetSteps.OrderAdjustments.Common.Model;
using IAddress = NetSteps.Addresses.Common.Models.IAddress;
using NetSteps.Data.Entities.Repositories;
using System.Threading.Tasks;
using NetSteps.Data.Entities.Business.HelperObjects;
using NetSteps.Data.Entities.Dto;
using NetSteps.Data.Entities.Business.HelperObjects.SearchParameters;
using System.Data;
using NetSteps.Data.Entities.EntityModels;
using System.Web;
using NetSteps.Data.Entities.Business.Common;

#endregion


namespace NetSteps.Data.Entities
{
    public partial class Order : IAuditedEntity, IKeyName<Order, string>, IOrder, NetSteps.Orders.Common.Models.IOrder, IDateLastModified
    {
        #region Members

        /// <summary>
        /// Related entities that can be included by the 'Load' methods (see <see cref="LoadRelationsExtensions"/>).
        /// WARNING: Changes to this list will affect various 'Load' methods, be careful.
        /// </summary>
        [Flags]
        public enum Relations
        {
            // These are bit flags so they must be numbered appropriately (eg. 0, 1, 2, 4, 8, 16)
            // Use bit-shifting for convenience (eg. 0, 1 << 0, 1 << 1, 1 << 2)
            None = 0,
            Notes = 1 << 0,
            OrderAdjustments = 1 << 1,
            OrderAdjustments_OrderAdjustmentOrderLineModifications = 1 << 2,
            OrderAdjustments_OrderAdjustmentOrderModifications = 1 << 3,
            OrderCustomers = 1 << 4,
            OrderCustomers_OrderItems = 1 << 5,
            OrderCustomers_OrderItems_GiftCards = 1 << 6,
            OrderCustomers_OrderItems_OrderItemMessages = 1 << 7,
            OrderCustomers_OrderItems_OrderItemPrices = 1 << 8,
            OrderCustomers_OrderItems_OrderItemProperties = 1 << 9,
            OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue = 1 << 10,
            OrderCustomers_OrderItems_OrderItemReturns = 1 << 11,
            OrderPayments = 1 << 12,
            OrderPayments_OrderPaymentResults = 1 << 13,
            OrderShipments = 1 << 14,
            OrderShipments_OrderShipmentPackages = 1 << 15,
            OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems = 1 << 16,
            AutoshipOrders = 1 << 17,

            /// <summary>
            /// The default relations used by the 'LoadFull' methods.
            /// </summary>
            LoadFull =
                Notes
                | OrderAdjustments
                | OrderAdjustments_OrderAdjustmentOrderLineModifications
                | OrderAdjustments_OrderAdjustmentOrderModifications
                | OrderCustomers
                | OrderCustomers_OrderItems
                | OrderCustomers_OrderItems_GiftCards
                | OrderCustomers_OrderItems_OrderItemMessages
                | OrderCustomers_OrderItems_OrderItemPrices
                | OrderCustomers_OrderItems_OrderItemProperties
                | OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue
                | OrderCustomers_OrderItems_OrderItemReturns
                | OrderPayments
                | OrderShipments
                | OrderShipments_OrderShipmentPackages
                | OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems
                | AutoshipOrders,

            /// <summary>
            /// The default relations used by the AutoshipOrder 'LoadFull' methods.
            /// </summary>
            LoadFullForAutoshipOrder =
                Notes
                | OrderAdjustments
                | OrderAdjustments_OrderAdjustmentOrderLineModifications
                | OrderAdjustments_OrderAdjustmentOrderModifications
                | OrderCustomers
                | OrderCustomers_OrderItems
                | OrderCustomers_OrderItems_OrderItemMessages
                | OrderCustomers_OrderItems_OrderItemPrices
                | OrderCustomers_OrderItems_OrderItemProperties
                | OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue
                | OrderPayments
                | OrderShipments,

            LoadChildOrdersForHostessRewards =
                OrderCustomers
                | OrderCustomers_OrderItems
                | OrderCustomers_OrderItems_OrderItemPrices,

            LoadPaymentDetails =
                OrderAdjustments
                | OrderAdjustments_OrderAdjustmentOrderLineModifications
                | OrderAdjustments_OrderAdjustmentOrderModifications
                | OrderCustomers
                | OrderPayments,

            LoadShipmentDetails =
                OrderCustomers
                | OrderCustomers_OrderItems
                | OrderPayments
                | OrderShipments
                | OrderShipments_OrderShipmentPackages
                | OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems,

            /// <summary>
            /// The default relations used by the Party 'LoadFull' methods.
            /// </summary>
            LoadFullForParty =
                Notes
                | OrderAdjustments
                | OrderAdjustments_OrderAdjustmentOrderLineModifications
                | OrderAdjustments_OrderAdjustmentOrderModifications
                | OrderCustomers
                | OrderCustomers_OrderItems
                | OrderCustomers_OrderItems_GiftCards
                | OrderCustomers_OrderItems_OrderItemMessages
                | OrderCustomers_OrderItems_OrderItemPrices
                | OrderCustomers_OrderItems_OrderItemProperties
                | OrderCustomers_OrderItems_OrderItemProperties_OrderItemPropertyValue
                | OrderCustomers_OrderItems_OrderItemReturns
                | OrderPayments
                | OrderPayments_OrderPaymentResults
                | OrderShipments
                | OrderShipments_OrderShipmentPackages
                | OrderShipments_OrderShipmentPackages_OrderShipmentPackageItems,

            LoadWithCustomersForParty =
                OrderCustomers,
        };

        private bool _calculationsDirty = true;
        private Dictionary<string, object> _orderModifiers;
        #endregion

        #region Properties

        /// <summary>
        /// This value is only stored in memory.  If an order has been loaded but not calculated it will be null.
        /// </summary>
        public decimal? PartyShippingTax { get; set; }

        /// <summary>
        /// Calls the virtual method in BusinessLogic that determines whether or not we are dividing the party shipping.
        /// </summary>
        public bool ShouldDividePartyShipping
        {
            get
            {
                return OrderService.ShouldDividePartyShipping();
            }
        }

        //public static PreOrder PreOrders = null; // { get; set; }
        //public static List<ShippingMethods> shippingMethods = null;
        //public static List<ProductRelations> productRelations = null;

        //public Dictionary<Tuple<int, int>, PreOrder> _PreOrderDictionary = null;
        //public PreOrder this[Tuple<int, int> index]
        //{
        //    get { return _PreOrderDictionary[index]; }
        //    set { _PreOrderDictionary[index] = value; }
        //}
        private bool _orderHasChanges;

        public bool OrderHasChanges
        {
            get
            {
                if (this.GetAllOrderItems().Any(q => q.HasChanges))
                    return true;
                else
                    return _orderHasChanges;
            }
            set
            {
                _orderHasChanges = value;
            }
        }

        private Constants.OrderPendingStates _orderPendingState;

        public Constants.OrderPendingStates OrderPendingState
        {
            get
            {
                if (this.IsCommissionable())
                    return Constants.OrderPendingStates.Completed;

                return _orderPendingState;
            }
            set
            {
                _orderPendingState = value;
            }
        }

        public Dictionary<string, object> OrderModifiers
        {
            get
            {
                if (_orderModifiers == null)
                {
                    _orderModifiers = new Dictionary<string, object>();
                }
                return _orderModifiers;
            }
        }

        public List<Order> OnlineChildOrders
        {
            get
            {
                return this.LoadChildOrdersForHostessRewards();
            }
        }

        internal bool CalculationsDirty
        {
            get { return _calculationsDirty; }
            set { _calculationsDirty = value; }
        }

        public string InvoiceNotes
        {
            get
            {
                return this.Notes.Count(n => n.NoteTypeID == (int)ConstantsGenerated.NoteType.OrderInvoiceNotes) > 0 ? this.Notes.First(n => n.NoteTypeID == (int)ConstantsGenerated.NoteType.OrderInvoiceNotes).NoteText : string.Empty;
            }
            set
            {
                if (this.Notes.Count(n => n.NoteTypeID == (int)ConstantsGenerated.NoteType.OrderInvoiceNotes) > 0)
                {
                    Note invoiceNote = this.Notes.First(n => n.NoteTypeID == (int)ConstantsGenerated.NoteType.OrderInvoiceNotes);
                    invoiceNote.NoteText = value;
                    //invoiceNote.DateModified = DateTime.Now.ApplicationNow();
                }
                else
                {
                    this.Notes.Add(new Note()
                    {
                        NoteTypeID = (int)ConstantsGenerated.NoteType.OrderInvoiceNotes,
                        NoteText = value,
                        DateCreated = DateTime.Now,
                        ModifiedByUserID = ApplicationContext.Instance.CurrentUser.UserID
                    });
                }
            }
        }

        public List<OrderPayment> AllOrderPayments
        {
            get
            {
                List<OrderPayment> result = new List<OrderPayment>();

                result.AddRange(this.OrderPayments);

                foreach (OrderCustomer customer in this.OrderCustomers)
                {
                    foreach (OrderPayment payment in customer.OrderPayments)
                    {
                        if (!result.Contains(payment))
                        {
                            result.Add(payment);
                        }
                    }
                }

                return result;
            }
        }

        private AccountSlimSearchData _consultantInfo;

        public AccountSlimSearchData ConsultantInfo
        {
            get
            {
                if ((_consultantInfo == null || _consultantInfo.AccountID != ConsultantID) && this.ConsultantID > 0)
                    _consultantInfo = Account.LoadSlim(this.ConsultantID);
                return _consultantInfo;
            }
        }

        public bool IsTemplate
        {
            get
            {
                return SmallCollectionCache.Instance.OrderTypes.GetById(OrderTypeID).IsTemplate;
            }
            set { } // For WCF Deserialization
        }

        public ITaxService TaxService { get { return Create.New<ITaxService>(); } }

        public IShippingCalculator ShippingCalculator { get { return Create.New<IShippingCalculator>(); } }

        public IOrderItemBusinessLogic OrderItemBusinessLogic { get { return Create.New<IOrderItemBusinessLogic>(); } }

        protected static IOrderService OrderService { get { return Create.New<IOrderService>(); } }

        #endregion

        #region Property Changed Methods

        partial void ShippingTotalOverrideChanged()
        {
            this.CalculationsDirty = true;
        }

        partial void TaxAmountTotalOverrideChanged()
        {
            this.CalculationsDirty = true;
        }

        #endregion

        #region Legacy Properties
        public decimal SubtotalRetail { get; set; }

        public decimal SubtotalAdjusted { get; set; }

        public decimal DeferredBalance { get; set; }

        public decimal SubtotalHostessDiscounted { get; set; }

        public decimal HostessOverage { get; set; }
        #endregion

        #region Constructors

        public static List<PaymentReturn> paymentReturn = null; // { get; set; }

        /// <summary>
        /// The account passed into this constructor is the order customer for the order. - JHE
        /// </summary>
        /// <param name="account"></param>
        public Order(Account account, int? orderTypeID = null)
        {

            if (!this.ChangeTracker.ChangeTrackingEnabled)
            {
                this.StartEntityTracking();
            }

            DefaultValues();

            if (orderTypeID.HasValue)
            {
                this.OrderTypeID = orderTypeID.ToShort();
            }

            //PreOrders = new PreOrder();
            //productRelations = new List<ProductRelations>();
            //shippingMethods = new List<ShippingMethods>();
            //_PreOrderDictionary = new Dictionary<Tuple<int, int>, PreOrder>();

            this.SetConsultantID(account);

            AddNewCustomer(account);

            paymentReturn = new List<PaymentReturn>();
        }
        #endregion

        #region Methods
        public static Order LoadByOrderNumber(string orderNumber)
        {
            try
            {
                return BusinessLogic.LoadByOrderNumber(Repository, orderNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static IEnumerable<Order> LoadOrderWithShipmentAndPaymentDetails(IEnumerable<string> orderNumbers)
        {
            try
            {
                return BusinessLogic.LoadOrderWithShipmentAndPaymentDetails(Repository, orderNumbers);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Order LoadOrderWithPaymentDetails(int orderID)
        {
            try
            {
                return BusinessLogic.LoadOrderWithPaymentDetails(Repository, orderID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static Order LoadByOrderNumberFull(string orderNumber)
        {
            try
            {
                //PreOrders = new PreOrder();
                //productRelations = new List<ProductRelations>(); 
                //shippingMethods = new List<ShippingMethods>();
                return BusinessLogic.LoadByOrderNumberFull(Repository, orderNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static PaginatedList<OrderSearchData> Search(OrderSearchParameters orderSearchParameters)
        {
            try
            {
                return BusinessLogic.Search(Repository, orderSearchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static List<OrderTackingsSearchData> SearchOrderTrackingByOrderNumber(string OrderNumber, int page, int pageSize, string column, string order)
        {
            try
            {
                List<OrderTackingsSearchData> result = new List<OrderTackingsSearchData>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderNumber }, 
                                        { "@PageSize", pageSize }, { "@PageNumber", page }, { "@Colum", column }, { "@Order", order } };
                SqlDataReader reader = DataAccess.GetDataReader("spSearchOrderTrackingByOrderNumber", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new OrderTackingsSearchData()
                        {
                            OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]),
                            Name = Convert.ToString(reader["Name"]),
                            InitialTackingDateUTC = Convert.ToDateTime(reader["InitialTackingDateUTC"]),
                            FinalTackingDateUTC = Convert.ToDateTime(reader["FinalTackingDateUTC"]),
                            Comment = Convert.ToString(reader["Comment"]),
                            RowTotal = Convert.ToInt32(reader["RowTotal"]),
                            Etapa = Convert.ToInt32(reader["Etapa"]),
                            ImagenStatus = Convert.ToString(reader["ImagenStatus"])
                        });
                    }
                }
                return result;


            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static DateTime GetExpectedDeliveryDateByOrderNumber(string OrderNumber)
        {
            try
            {
                DateTime? DeliveryDateUTC = null;
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderNumber } };
                SqlDataReader reader = DataAccess.GetDataReader("spGetExpectedDeliveryDateByOrderNumber", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        DeliveryDateUTC = Convert.ToDateTime(reader["DeliveryDateUTC"]);
                    }
                }


                return DeliveryDateUTC.Value;
            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static bool ValidateOrderRulesPreCondition(int? accountId, out string message)
        {
            try
            {
                return BusinessLogic.ValidateOrderRulesPreCondition(Repository, accountId, out message);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int Count(OrderSearchParameters searchParameters)
        {
            try
            {
                return BusinessLogic.Count(Repository, searchParameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static IList<Order> LoadChildOrders(int parentOrderID, params int[] childOrderTypeIDs)
        {
            try
            {
                return BusinessLogic.LoadChildOrders(Repository, parentOrderID, childOrderTypeIDs).AsConcrete<IOrder, Order>();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public IList<Order> LoadChildOnlineOrders()
        {
            return LoadChildOrdersFull(this.OrderID, (int)Constants.OrderType.OnlineOrder, (int)Constants.OrderType.PortalOrder);
        }

        public List<Order> LoadChildOrdersForHostessRewards()
        {
            return Repository.LoadChildOrdersForHostessRewards(this.OrderID, (int)Constants.OrderType.OnlineOrder, (int)Constants.OrderType.PortalOrder);
        }

        public static IList<Order> LoadChildOrdersFull(int parentOrderID, params int[] childOrderTypeIDs)
        {
            try
            {
                return BusinessLogic.LoadChildOrdersFull(Repository, parentOrderID, childOrderTypeIDs).AsConcrete<IOrder, Order>();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public OrderItem AddItem(int productID, int quantity)
        {
            var inventory = Create.New<InventoryBaseRepository>();

            var product = inventory.GetProduct(productID);
            return AddItem(this.OrderCustomers[0], product, quantity);
        }

        public OrderItem AddItem(IProduct product, int quantity)
        {
            return AddItem(this.OrderCustomers[0], product, quantity);
        }

        public OrderItem AddItem(IOrderCustomer orderCustomer, IProduct product, int quantity, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            return AddItem(orderCustomer, product, quantity, Constants.OrderItemType.Retail, -1, -1, false, hostRewardRuleId: null, parentGuid: parentGuid, dynamicKitGroupId: dynamicKitGroupId);
        }

        public OrderItem AddItem(IOrderCustomer orderCustomer, IProduct product, int quantity, Constants.OrderItemType orderItemType, decimal itemPriceOverride, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            return AddItem(orderCustomer, product, quantity, orderItemType, itemPriceOverride, -1, false, hostRewardRuleId: null, parentGuid: parentGuid, dynamicKitGroupId: dynamicKitGroupId);
        }

        public OrderItem AddItem(IOrderCustomer orderCustomer, IProduct product, int quantity, Constants.OrderItemType orderItemType,
            decimal itemPriceOverride, decimal itemCVOverride, bool wholesaleOverride, int? hostRewardRuleId = null, string parentGuid = null, int? dynamicKitGroupId = null)
        {
            return (OrderItem)OrderService.AddItem(this, orderCustomer, product, quantity, (short)orderItemType, hostRewardRuleId, parentGuid, dynamicKitGroupId);
        }

        public bool RemoveItem(int orderItemId)
        {
            // this makes this method exclusive to the first customer.  
            return RemoveItem(this.OrderCustomers[0], orderItemId);
        }

        public bool RemoveItem(IOrderItem orderItem)
        {
            if (((OrderItem)orderItem).OrderCustomer == null)
                return true;
            else
                return RemoveItem(((OrderItem)orderItem).OrderCustomer, orderItem as OrderItem);
        }

        public bool RemoveItem(OrderCustomer orderCustomer, int orderItemId)
        {
            return RemoveItem(orderCustomer, orderCustomer.OrderItems.GetOrderItem(orderItemId));
        }

        public bool RemoveItem(OrderCustomer orderCustomer, string Guid)
        {
            return RemoveItem(orderCustomer, orderCustomer.OrderItems.GetByGuid(Guid));
        }

        public bool RemoveItem(OrderCustomer orderCustomer, OrderItem orderItem)
        {
            OrderService.RemoveOrderItem(orderCustomer, orderItem);
            return true;
        }

        /// <summary>
        /// I don't think it is good practice to remove all items and them add them back in. Don't use this method for that and
        /// re-factor places in code that do, to do more of a sync instead. - JHE
        /// </summary>
        /// <param name="orderCustomer"></param>
        /// <returns></returns>
        public bool RemoveAllItems(OrderCustomer orderCustomer)
        {
            bool returnValue = true;
            foreach (var item in orderCustomer.OrderItems.ToList())
            {
                if (!RemoveItem(orderCustomer, item))
                    returnValue = false;
            }

            return returnValue;
        }

        public void UpdateItem(IOrderCustomer orderCustomer, IOrderItem orderItem, int quantity, decimal? itemPriceOverride = null, decimal? itemCVOverride = null, bool wholesaleOverride = false)
        {
            OrderService.UpdateItem(this, orderCustomer, orderItem, quantity, itemPriceOverride, itemCVOverride, wholesaleOverride);
        }

        public OrderCustomer AddNewCustomer(int accountID)
        {
            var orderCustomer = new OrderCustomer
            {
                OrderCustomerTypeID = (int)Constants.OrderCustomerType.Normal,
                AccountID = accountID
            };

            //orderCustomer.IsTaxExempt = orderCustomer.AccountInfo.IsTaxExempt;
            OrderCustomers.Add(orderCustomer);

            return orderCustomer;
        }

        public OrderCustomer AddNewCustomer(Account account)
        {
            if (account == null)
            {
                throw new ArgumentNullException("account");
            }

            var orderCustomer = AddNewCustomer(account.AccountID);

            if (account.AccountID == 0)
            {
                orderCustomer.SetDefaultAccountTypeID(account.AccountTypeID);
            }

            return orderCustomer;
        }

        public void UpdateCustomer(Account account)
        {
            try
            {
                ((OrderService)OrderService).UpdateCustomer(this, account);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public bool CanBeDynamicKitUpSold(OrderCustomer customer, Product product, int numberOfProductsAway)
        {
            return OrderService.CanBeDynamicKitUpSold(this, customer, product, numberOfProductsAway);
        }

        public bool CanBeAddedToDynamicKitGroup(int productId, DynamicKitGroup group)
        {
            return ((OrderService)OrderService).CanBeAddedToDynamicKitGroup(productId, group);
        }

        public IEnumerable<Product> GetPotentialDynamicKitUpSaleProducts(OrderCustomer customer, IList<IProduct> sortedDynamicKitProducts)
        {
            return OrderService.GetPotentialDynamicKitUpSaleProducts(this, customer, sortedDynamicKitProducts).Select(kp => (Product)kp);
        }

        public string ConvertToDynamicKit(OrderCustomer customer, Product kitProduct)
        {
            return OrderService.ConvertToDynamicKit(this, customer, kitProduct);
        }
        /// <summary>
        /// Defaults a new order with Billing and Shipping address from the account specified in the 
        /// parameter: orderCustomer
        /// </summary>
        /// <param name="orderCustomer"></param>
        public void SetDefaultsWithAccountValues(OrderCustomer orderCustomer)
        {
            try
            {
                var account = Account.LoadFull(orderCustomer.AccountID);

                #region Shipping Address
                Address accountDefaultShippingAddress = account.Addresses.FirstOrDefault((address) => address.AddressTypeID == Constants.AddressType.Shipping.ToShort() && address.IsDefault);
                if (accountDefaultShippingAddress == null)
                    accountDefaultShippingAddress = account.Addresses.FirstOrDefault((address) => address.AddressTypeID == Constants.AddressType.Shipping.ToShort());
                if (accountDefaultShippingAddress == null)
                    accountDefaultShippingAddress = account.Addresses.FirstOrDefault((address) => address.AddressTypeID == Constants.AddressType.Main.ToShort());

                OrderShipment orderShipment = GetDefaultShipmentNoDefault();

                if (orderShipment != null && accountDefaultShippingAddress != null)
                {
                    if (orderShipment.OrderID == 0)
                        orderShipment.OrderID = OrderID;
                    Address.CopyPropertiesTo(accountDefaultShippingAddress, orderShipment);
                }
                else if (orderShipment == null && accountDefaultShippingAddress != null)
                {
                    orderShipment = new OrderShipment();
                    orderShipment.OrderID = OrderID;
                    orderShipment.OrderCustomerID = orderCustomer.OrderCustomerID;
                    orderShipment.OrderShipmentStatusID = Constants.OrderShipmentStatus.Pending.ToShort();
                    Address.CopyPropertiesTo(accountDefaultShippingAddress, orderShipment);
                    this.OrderShipments.Add(orderShipment);
                }
                #endregion

                #region Default Shipping Method
                if (orderShipment != null)
                {
                    if (this.CurrencyID == 0)
                    {
                        OrderService.SetCurrencyID(this);
                    }

                    // Default to the least expensive shipping method
                    var lowestPriceRate = ShippingCalculator.GetLeastExpensiveShippingMethod(orderCustomer, orderShipment);
                    orderShipment.ShippingMethodID = lowestPriceRate.ShippingMethodID;
                }
                #endregion

                #region Default Payment
                OrderService.SetCurrencyID(this);
                AccountPaymentMethod defaultAccountPaymentMethod = account.AccountPaymentMethods.FirstOrDefault((accountPaymentMethod) => accountPaymentMethod.IsDefault);
                if (defaultAccountPaymentMethod == null)
                    defaultAccountPaymentMethod = account.AccountPaymentMethods.FirstOrDefault();
                if (defaultAccountPaymentMethod != null)
                {
                    OrderPayment orderPayment = new OrderPayment();
                    orderPayment.StartTracking();
                    orderPayment.OrderID = OrderID;
                    orderPayment.OrderCustomerID = orderCustomer.OrderCustomerID;
                    orderPayment.PaymentTypeID = defaultAccountPaymentMethod.PaymentTypeID;
                    orderPayment.CurrencyID = this.CurrencyID;

                    orderPayment.OrderPaymentStatusID = Constants.OrderPaymentStatus.Pending.ToShort();

                    Reflection.CopyPropertiesDynamic<IPayment, IPayment>(defaultAccountPaymentMethod, orderPayment);
                    Address.CopyPropertiesTo(defaultAccountPaymentMethod, orderPayment);

                    if (defaultAccountPaymentMethod.BillingAddressID == null)
                        orderPayment.BillingPostalCode = string.Empty;

                    orderPayment.Amount = this.GrandTotal.ToDecimal();
                    orderCustomer.OrderPayments.Add(orderPayment);
                }
                #endregion
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        /// <summary>
        /// This login can be overridden through IOrderBusinessLogic.GetDefaultShipment() - JHE
        /// </summary>
        /// <returns></returns>
        public OrderShipment GetDefaultShipment()
        {
            try
            {
                return ((OrderService)OrderService).GetDefaultShipment(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public OrderShipment GetDefaultShipmentNoDefault()
        {
            try
            {
                return ((OrderService)OrderService).GetDefaultShipmentNoDefault(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public decimal GetEnrollmentCreditBalance()
        {
            decimal enrollmentCreditBalance = 0;

            if (this.OrderCustomers.Any() && this.OrderCustomers[0].AccountID > 0)
            {
                var service = Create.New<IProductCreditLedgerService>();
                var accountId = this.OrderCustomers[0].AccountID;
                var ledgerEntryKind = service.GetEntryKind("EC");
                enrollmentCreditBalance = service.GetCurrentBalance(accountId, ledgerEntryKind);
            }

            return enrollmentCreditBalance;
        }

        public decimal GetEnrollmentCredit_OrderBalance(decimal? enrollmentCreditBalance = null)
        {
            decimal orderBalance = 0;

            if (!enrollmentCreditBalance.HasValue)
                enrollmentCreditBalance = GetEnrollmentCreditBalance();

            if (enrollmentCreditBalance.Value > this.GrandTotal.Value)
            {
                orderBalance = this.GrandTotal.Value;
            }
            else
            {
                orderBalance = this.GrandTotal.Value - enrollmentCreditBalance.Value;
            }

            return orderBalance;
        }

        public decimal GetEnrollmentCredit_OrderPaymentTotal()
        {
            return this.OrderPayments.Where(op => op.PaymentTypeID == (int)Constants.PaymentType.EnrollmentCredit && op.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToInt()).Sum(op => op.Amount);
        }

        public decimal GetNonEnrollmentCredit_OrderPaymentTotal()
        {
            return this.OrderPayments.Where(op => op.PaymentTypeID != (int)Constants.PaymentType.EnrollmentCredit && op.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToInt()).Sum(op => op.Amount);
        }

        [Obsolete("DO NOT USE!!! CALL THE ORDER SERVICE!!!")]
        public BasicResponse SubmitOrder()
        {
            return null;
        }

        public static Product GetRestockingFeeProduct()
        {
            return (Product)OrderService.GetRestockingFeeProduct();
        }

        public void DeletePayment(int orderPaymentId)
        {
            try
            {
                OrderPayment payment = OrderPayments.FirstOrDefault(p => p.OrderPaymentID == orderPaymentId);
                if (payment != null)
                    DeletePayment(payment);
                else
                    throw EntityExceptionHelper.GetAndLogNetStepsException(new Exception(string.Format("OrderPaymentID ({0}) not found on order({1}).", orderPaymentId, this.OrderID)), Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
            }
        }

        internal void DeletePayment(OrderPayment orderPayment)
        {
            try
            {
                orderPayment.Delete();
                this.OrderPayments.Remove(orderPayment);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID, orderPayment.OrderCustomer.AccountID);
            }
        }

        public BasicResponse Refund(OrderPayment orderPayment, decimal amount)
        {
            IPaymentGateway gateway = PaymentGateways.Payments.GetPaymentGateway(this, orderPayment);
            if (gateway != null)
                return gateway.Refund(orderPayment, amount);
            else
                return new BasicResponse()
                {
                    Message = "Gateway not found for PaymentTypeID: " + orderPayment.PaymentTypeID,
                    Success = false
                };
        }

        public bool IsReturnOrder()
        {
            return IsReturnOrder(OrderTypeID);
        }

        public static bool IsReturnOrder(short orderTypeID)
        {
            return orderTypeID == (short)Constants.OrderType.ReturnOrder;
        }

        public bool IsReturnable()
        {
            return OrderService.IsReturnable(this);
        }

        public bool IsOrderFullyReturned(Order order)
        {
            try
            {
                return OrderService.IsOrderFullyReturned(order);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, (order != null) ? order.OrderID.ToIntNullable() : null);
            }
        }

        public static bool IsOrderFullyReturned(Order order, List<Order> returnOrders)
        {
            try
            {
                return OrderService.IsOrderFullyReturned(order, returnOrders.Select(o => (IOrder)o).ToList());
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, (order != null) ? order.OrderID.ToIntNullable() : null);
            }
        }

        public bool IsAutoshipOrder()
        {
            return OrderService.IsAutoshipOrder(OrderTypeID);
        }

        public static bool IsAutoshipOrder(short orderTypeID)
        {
            return OrderService.IsAutoshipOrder(orderTypeID);
        }

        public static bool IsDynamicKitValid(OrderItem orderItem)
        {
            return OrderService.IsDynamicKitValid(orderItem);
        }

        public static bool IsDynamicKitGroupValid(OrderItem kitOrderItem, DynamicKitGroup dynamicKitGroup)
        {
            return ((OrderService)OrderService).IsDynamicKitGroupValid(kitOrderItem, dynamicKitGroup);
        }

        public static BasicResponse CanBeAddedToDynamicKit(OrderCustomer orderCustomer, Product product, int quantity, string parentGuid, int? dynamicKitGroupId)
        {
            return OrderService.CanBeAddedToDynamicKit(orderCustomer, product, quantity, parentGuid, dynamicKitGroupId);
        }

        public static bool IsStaticKitValid(OrderItem orderItem)
        {
            return OrderService.IsStaticKitValid(orderItem);
        }

        [Obsolete("Use NetSteps.Data.Entities.Business.Logic.IsCancelPosible()")]
        public bool IsCancellable()
        {
            return OrderService.IsCancellable(this);
        }

        public bool IsEditable()
        {
            return OrderService.IsEditable(this);
        }

        public bool HasConsultantOnOrder()
        {
            return OrderService.HasConsultantOnOrder(this);
        }

        public bool CanChangeToPaidStatus()
        {
            return OrderService.CanChangeToPaidStatus(this);
        }

        public bool IsPaidInFull()
        {
            return OrderService.IsPaidInFull(this);
        }

        /// <param name="customer">Will use the first customer on the order if left null</param>
        /// <param name="user">User is only used to validate the role-function for GMP, otherwise, we use the Distributor account type to validate the role-function</param>
        //public BasicResponseItem<OrderPayment> ApplyPaymentToCustomer(int PaymentTypeID, decimal amount,string NamePayment, int IdPayment, OrderCustomer customer = null, NetSteps.Common.Interfaces.IUser user = null)

        public BasicResponseItem<OrderPayment> ApplyPaymentToCustomer(int PaymentTypeID, decimal amount, string NamePayment, IPayment paymentMethod, OrderCustomer customer = null, NetSteps.Common.Interfaces.IUser user = null)
        {
            try
            {
                return ((OrderService)OrderService).ApplyPaymentToCustomer(GetRepository(), this, customer, paymentMethod, amount, user);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public BasicResponseItem<OrderPayment> ApplyPaymentToCustomers(int PaymentTypeID, decimal amount, IPayment paymentMethod, OrderCustomer customer = null, NetSteps.Common.Interfaces.IUser user = null)
        {
            try
            {
                return ((OrderService)OrderService).ApplyPaymentToCustomer(GetRepository(), this, customer, paymentMethod, amount, user);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }
        public BasicResponseItem<OrderPayment> ApplyPaymentToCustomerPreviosBalance(int PaymentTypeID, decimal amount, IPayment paymentMethod, OrderCustomer customer = null, NetSteps.Common.Interfaces.IUser user = null)
        {
            try
            {
                return ((OrderService)OrderService).ApplyPaymentToCustomerPreviosBalance(GetRepository(), this, customer, paymentMethod, amount, user);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }
        /// <summary>
        /// Applies the payment to the order instead of the order customer (used for party orders) - DES
        /// </summary>
        public BasicResponseItem<OrderPayment> ApplyPaymentToOrder(IPayment paymentMethod, decimal amount)
        {
            try
            {
                return ((OrderService)OrderService).ApplyPaymentToOrder(GetRepository(), this, paymentMethod, amount);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void RemovePayment(OrderPayment orderPayment, bool forceCancel = false)
        {
            try
            {
                //if (orderPayment.OrderPaymentID != 0)
                //    orderPayment.Delete();	// Don't delete here. Let the Save() method handle the update. - JHE

                bool hasExistingOrderPaymentResults = false;
                hasExistingOrderPaymentResults = OrderPayment.HasOrderPaymentResults(orderPayment.OrderPaymentID);

                if (hasExistingOrderPaymentResults)
                {
                    if (orderPayment.OrderPaymentStatusID != (short)Constants.OrderPaymentStatus.Completed || forceCancel)
                    {
                        orderPayment.OrderPaymentStatusID = (short)Constants.OrderPaymentStatus.Cancelled;
                    }
                }
                else
                {
                    if (orderPayment.ChangeTracker.State != ObjectState.Added)
                    {
                        orderPayment.MarkAsDeleted();
                    }
                    this.OrderPayments.Remove(orderPayment);
                }
                CalculationsDirty = true;
            }
            catch (Exception ex)
            {
                var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException,
                    orderID: OrderID == 0 ? null : OrderID.ToIntNullable());
            }
        }

        public static short GetStandardOrderTypeID(OrderCustomer orderCustomer)
        {
            return OrderService.GetStandardOrderTypeID(orderCustomer);
        }

        public void PerformOverrides(List<OrderItemOverride> items, decimal taxAmount, decimal shippingAmount)
        {
            try
            {
                ((OrderService)OrderService).PerformOverrides(this, items, taxAmount, shippingAmount);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void UpdateOrderShipmentAddress(OrderShipment shipment, int addressId)
        {
            try
            {
                ((OrderService)OrderService).UpdateOrderShipmentAddress(this, shipment, addressId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex,
                    Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void UpdateOrderShipmentAddress(OrderShipment shipment, IAddress address)
        {
            try
            {
                ((OrderService)OrderService).UpdateOrderShipmentAddress(this, shipment, address);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void AddOrUpdateOrderItem(int orderCustomerId, IEnumerable<OrderItemUpdateInfo> productUpdates, bool overrideQuantity, string parentGuid = null, int? dynamicKitGroupId = null, bool removePendingPayments = true)
        {
            try
            {
                OrderService.AddOrUpdateOrderItem(this, OrderCustomer.Load(orderCustomerId), productUpdates, overrideQuantity, parentGuid, dynamicKitGroupId);
            }
            catch (Exception ex)
            {
                if (!(ex is NetStepsBusinessException))
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
                else
                    throw ex;
            }
        }

        public void AddOrUpdateOrderItem(OrderCustomer customer, IEnumerable<OrderItemUpdateInfo> productUpdates, bool overrideQuantity, string parentGuid = null, int? dynamicKitGroupId = null, bool removePendingPayments = true)
        {
            try
            {
                OrderService.AddOrUpdateOrderItem(this, customer, productUpdates, overrideQuantity, parentGuid, dynamicKitGroupId);
            }
            catch (Exception ex)
            {
                if (!(ex is NetStepsBusinessException))
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
                else
                    throw ex;
            }
        }

        public void CancelOverrides()
        {
            try
            {
                OrderService.CancelOverrides(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void UndoReplacementOrderPrices()
        {
            try
            {
                OrderService.UndoReplacementOrderPrices(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void SetReplacementOrderPrices(List<OrderItemOverride> items, decimal taxAmount, decimal shippingAmount)
        {
            try
            {
                ((OrderService)OrderService).SetReplacementOrderPrices(this, items, taxAmount, shippingAmount);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public IEnumerable<ShippingMethodWithRate> UpdateOrderShipmentAddressAndDefaultShipping(int addressId)
        {
            try
            {
                return ((OrderService)OrderService).UpdateOrderShipmentAddressAndDefaultShipping(this, addressId);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public IEnumerable<ShippingMethodWithRate> UpdateOrderShipmentAddressAndDefaultShipping(IAddress address)
        {
            try
            {
                return ((OrderService)OrderService).UpdateOrderShipmentAddressAndDefaultShipping(this, address);
            }
            catch (PartyOrderMinimumAmountException ex)
            {
                throw new PartyOrderMinimumAmountException(ex);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void ValidateOrderShipmentShippingMethod(OrderShipment shipment, IEnumerable<ShippingMethodWithRate> shippingMethods)
        {
            try
            {
                ((OrderService)OrderService).ValidateOrderShipmentShippingMethod(shipment, shippingMethods);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public void ValidateOrderShipmentShippingMethod(OrderShipment shipment, OrderCustomer customer)
        {
            try
            {
                ((OrderService)OrderService).ValidateOrderShipmentShippingMethod(shipment, customer);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public ShippingMethodWithRate SelectDefaultShippingMethod(IEnumerable<ShippingMethodWithRate> shippingMethods)
        {
            try
            {
                return ((OrderService)OrderService).SelectDefaultShippingMethod(shippingMethods);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public BasicResponse ValidateOrderItemsByStoreFront(Order order)
        {
            try
            {
                return ((OrderService)OrderService).ValidateOrderItemsByStoreFront(order);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public IEnumerable<ShippingMethodWithRate> GetShippingMethods()
        {
            try
            {
                return ((OrderService)OrderService).GetShippingMethods(this);
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                throw new ProductShippingExcludedShippingException(ex.ProductsThatHaveExcludedShipping);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public IEnumerable<ShippingMethodWithRate> GetShippingMethods(OrderShipment orderShipment)
        {
            try
            {
                return ((OrderService)OrderService).GetShippingMethods(this, orderShipment);
            }
            catch (ProductShippingExcludedShippingException ex)
            {
                throw new ProductShippingExcludedShippingException(ex.ProductsThatHaveExcludedShipping);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public int? GetDefaultShippingMethodID()
        {
            OrderShipment shipment = this.GetDefaultShipment();
            return shipment.ShippingMethodID;
        }

        public decimal GetDefaultShippingTotal()
        {
            var rates = ShippingCalculator.GetShippingMethodsWithRates(this);
            var defaultRate = rates.SingleOrDefault(x => x.ShippingMethodID == GetDefaultShippingMethodID().Value);
            if (defaultRate == null)
                return -1;
            else
                return defaultRate.ShippingAmount;
        }

        public int PreorderID { get; set; }
        public int WareHouseID { get; set; }

        public void SetShippingMethod(int? shippingMethodId)
        {
            OrderShipment shipment = this.GetDefaultShipment();
            shipment.ShippingMethodID = shippingMethodId;

            this.CalculationsDirty = true;
        }

        public void RemoveAllOrderItems()
        {
            foreach (OrderCustomer orderCustomer in this.OrderCustomers)
            {
                RemoveAllItems(orderCustomer);
            }

            this.CalculationsDirty = true;
        }

        public void AddCustomOrderItemsToAllCustomers(List<OrderItem> orderItems)
        {
            foreach (OrderCustomer orderCustomer in this.OrderCustomers)
            {
                orderCustomer.OrderItems.AddRange(orderItems);
            }
            this.CalculationsDirty = true;
        }

        public void BuildReadOnlyNotesTree()
        {
            try
            {
                BusinessLogic.BuildReadOnlyNotesTree(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static string GetPopupMessageForOrderDetail(Order order)
        {
            try
            {
                return BusinessLogic.GetPopupMessageForOrderDetail(Repository, order);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, (order != null) ? order.OrderID : order.OrderID.ToIntNullable(), null);
            }
        }

        public static PaginatedList<AuditLogRow> GetAuditLog(Order fullyLoadedOrder, AuditLogSearchParameters param)
        {
            try
            {
                return BusinessLogic.GetAuditLog(Repository, fullyLoadedOrder, param);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, (fullyLoadedOrder != null) ? fullyLoadedOrder.OrderID : fullyLoadedOrder.OrderID.ToIntNullable(), null);
            }
        }

        public static Order LoadWithShipmentDetails(int orderID, bool enableChangeTracking = false)
        {
            return BusinessLogic.LoadWithShipmentDetails(Repository, orderID, enableChangeTracking);
        }

        public static List<Order> LoadBatchWithShipmentDetails(IEnumerable<int> orderIDs, bool enableChangeTracking = false)
        {
            return BusinessLogic.LoadBatchWithShipmentDetails(Repository, orderIDs, enableChangeTracking);
        }

        public static List<DateTime> GetCompletedOrderDates(
            int? orderTypeID = null,
            int? parentOrderID = null,
            int? orderCustomerAccountID = null,
            Constants.SortDirection sortDirection = Constants.SortDirection.Ascending,
            int? pageSize = null)
        {
            return ((OrderService)OrderService).GetCompletedOrderDates(
                Repository,
                orderTypeID: orderTypeID,
                parentOrderID: parentOrderID,
                orderCustomerAccountID: orderCustomerAccountID,
                sortDirection: sortDirection,
                pageSize: pageSize
            );
        }

        public int GetShippingMarketID()
        {
            int result = 0;
            int countryID = 0;
            Country country = null;

            if (this.OrderShipments != null && this.OrderShipments.Count > 0)
            {
                countryID = this.OrderShipments[0].CountryID;
                country = SmallCollectionCache.Instance.Countries.GetById(countryID);

                if (country != null)
                {
                    result = country.MarketID;
                }
            }

            return result;
        }

        public static IAddress GetDefaultShippingAddress(int orderId, int orderCustomerId = 0)
        {
            return Repository.GetDefaultShippingAddress(orderId, orderCustomerId);
        }

        #endregion

        #region Basic Crud
        public override void Save()
        {
            try
            {
                if (this.CurrencyID == 0)
                    OrderService.SetCurrencyID(this);

                
                if (this.OrderNumber.IsNullOrEmpty())
                {
                    OrderService.GenerateAndSetNewOrderNumber(this);
                }

                // For Testing to check the number of objects in Order object graph - JHE
                //List<IObjectWithChangeTracker> allTrackerItems = new List<IObjectWithChangeTracker>();
                //IObjectWithChangeTrackerExtensions.GetAllChangeTrackerItems(this, allTrackerItems, true, true);
                //var dups = this.FindDuplicateEntitiesInObjectGraph();

                // Set ExpirationStatusID
                
                Order.UpdatePreOrder(this.OrderID, Convert.ToInt32(HttpContext.Current.Session["PreOrder"]));
                base.Save();

            
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }


        /// <summary>
        /// Developer: JZV
        /// Date: 27/08/2015
        /// </summary>
        /// <param name="OrderID"></param>
        /// <param name="OrderStatusID"></param>
        public void UpdatePersonalIndicator(int OrderID, short OrderStatusID)
        {
            try
            {
                OrderRepository.UpdatePersonalIndicator(OrderID, OrderStatusID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        /// <summary>
        /// Only calls base.Save. It's used to get the OrderID for the OrderNumber without having to use a temp GUID.
        /// </summary>
        internal void BaseSave()
        {
            base.Save();
        }

        public override void Delete()
        {
            try
            {
                foreach (var orderCustomer in this.OrderCustomers.ToList())
                {
                    foreach (var orderItem in orderCustomer.OrderItems.ToList())
                    {
                        if (orderItem.ChangeTracker.State != ObjectState.Added)
                            orderItem.MarkAsDeleted();
                    }
                    foreach (var orderPayment in orderCustomer.OrderPayments.ToList())
                    {
                        orderPayment.OrderID = 0;
                        if (orderPayment.ChangeTracker.State != ObjectState.Added)
                            orderPayment.MarkAsDeleted();
                    }
                    foreach (var orderShipment in orderCustomer.OrderShipments.ToList())
                    {
                        if (orderShipment.ChangeTracker.State != ObjectState.Added)
                            orderShipment.MarkAsDeleted();

                    }
                    foreach (var orderShipment in this.OrderShipments.ToList())
                    {
                        if (orderShipment.ChangeTracker.State != ObjectState.Added)
                            orderShipment.MarkAsDeleted();
                    }

                    if (orderCustomer.ChangeTracker.State != ObjectState.Added)
                        orderCustomer.MarkAsDeleted();
                }

                if (this.ChangeTracker.State != ObjectState.Added)
                    this.MarkAsDeleted();
                this.Save();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
            }
        }

        public static bool ExistsByOrderNumber(string orderNumber)
        {
            try
            {
                return BusinessLogic.ExistsByOrderNumber(Repository, orderNumber);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }
        #endregion

        #region Calculation Methods

        public bool IsCommissionable()
        {
            bool isCommissionable = false;

            if (OrderStatus != null)
            {
                isCommissionable = OrderStatus.IsCommissionable;
            }
            else
            {
                var status = SmallCollectionCache.Instance.OrderStatuses.FirstOrDefault(q => q.OrderStatusID == OrderStatusID);
                isCommissionable = status == null ? true : status.IsCommissionable;
            }

            if (OrderTypeID > 0)
            {
                var firstOrDefault = SmallCollectionCache.Instance.OrderTypes.FirstOrDefault(t => t.OrderTypeID == OrderTypeID);
                // Template orders are always pending and never commissionable
                if (firstOrDefault != null && firstOrDefault.IsTemplate)
                    isCommissionable = false;
            }

            return isCommissionable;
        }

        public void CalculatePartyShipping()
        {
            try
            {
                ShippingCalculator.CalculatePartyShipping(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public void FinalizeTax()
        {
            foreach (OrderCustomer orderCustomer in OrderCustomers)
            {
                try
                {
                    TaxService.FinalizeTax(orderCustomer);
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
                }
            }
        }

        public void CancelTax()
        {
            foreach (OrderCustomer orderCustomer in OrderCustomers)
            {
                try
                {
                    TaxService.CancelTax(orderCustomer);
                }
                catch (Exception ex)
                {
                    throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable());
                }
            }
        }
        #endregion

        public virtual Func<Order, string> GetIdColumnFunc
        {
            get
            {
                return i => i.OrderNumber;
            }
        }

        public virtual Action<Order, string> SetIdColumnFunc
        {
            get
            {
                return (Order i, string id) => i.OrderNumber = id;
            }
        }

        #region IKeyName<Order,string> Members

        string IKeyName<Order, string>.ID
        {
            get
            {
                return this.OrderNumber;
            }
            set
            {
                this.OrderNumber = value;
            }
        }

        string IKeyName<Order, string>.Title
        {
            get
            {
                if (BusinessLogic.GetTitleColumnFunc != null)
                    return BusinessLogic.GetTitleColumnFunc((Order)this);
                else
                    return string.Empty;
            }
            set
            {
                if (BusinessLogic.SetTitleColumnFunc != null)
                    BusinessLogic.SetTitleColumnFunc((Order)this, value);
            }
        }

        Func<Order, string> IKeyName<Order, string>.GetIdColumnFunc
        {
            get
            {
                return i => i.OrderNumber;
            }
        }

        Action<Order, string> IKeyName<Order, string>.SetIdColumnFunc
        {
            get
            {
                return (Order i, string id) => i.OrderNumber = id;
            }
        }

        Func<Order, string> IKeyName<Order, string>.GetTitleColumnFunc
        {
            get
            {
                return BusinessLogic.GetTitleColumnFunc;
            }
        }

        Action<Order, string> IKeyName<Order, string>.SetTitleColumnFunc
        {
            get
            {
                return BusinessLogic.SetTitleColumnFunc;
            }
        }

        #endregion

        public void SetHostess(OrderCustomer hostess)
        {
            try
            {
                OrderService.SetHostess(this, hostess);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
            }
        }

        public OrderCustomer GetHostess()
        {
            try
            {
                return (OrderCustomer)OrderService.GetHostess(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
            }
        }

        public Nullable<decimal> GetRemainingHostessRewards()
        {
            try
            {
                return OrderService.GetRemainingHostessRewardsCredit(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
            }
        }

        public void SetConsultantID(Account account, Account siteOwner = null)
        {
            try
            {
                ((OrderService)OrderService).SetConsultantID(this, account, siteOwner);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
            }
        }

        public void CalculatePartyTax()
        {
            try
            {
                TaxService.CalculatePartyTax(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        protected static object _lock = new object();

        public static short CalculateOrderShippedStatus(Order order)
        {
            return OrderService.CalculateOrderShippedStatus(order);
        }

        public void UpdateInventoryLevels(bool? returnProducts = null, bool? originalOrderCancelled = null)
        {
            OrderService.UpdateInventoryLevels(this, returnProducts, originalOrderCancelled);
        }

        public IEnumerable<HostessRewardRule> GetApplicableHostessRewardRules(int? marketID = null)
        {
            try
            {
                return ((OrderService)OrderService).GetApplicableHostessRewardRules(this, marketID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException, this.OrderID.ToIntNullable(), null);
            }
        }

        public void ChangeToPaidStatus()
        {
            try
            {
                ((OrderService)OrderService).ChangeToPaidStatus(this);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public IList<OrderItem> GetHostessRewardOrderItems()
        {
            try
            {
                return OrderService.GetHostessRewardOrderItems(this).AsConcrete<IOrderItem, OrderItem>();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public BasicResponse ValidateHostessRewards()
        {
            OrderCustomer hostess = GetHostess();
            var response = hostess.ValidateHostessRewards();
            return response;
        }

        public void NegateValuesForReturn()
        {
            TaxAmountTotalOverride *= -1;
            TaxAmountOrderItems *= -1;
            TaxAmountShipping *= -1;
            TaxableTotal *= -1;
            TaxAmountTotal *= -1;
            Subtotal *= -1;
            CommissionableTotal *= -1;
            GrandTotal *= -1;
            ShippingTotalOverride *= -1;
            ShippingTotal *= -1;
            PaymentTotal *= -1;
            OrderCustomers.Each(oc =>
            {
                oc.OrderItems.Each(i =>
                {
                    i.Quantity *= -1;
                    i.AdjustedPrice *= -1;
                });
                oc.OrderPayments.Each(op => op.Amount *= -1);
                oc.Total *= -1;
                oc.Subtotal *= -1;
                oc.CommissionableTotal *= -1;
                oc.ShippingTotal *= -1;
                oc.TaxableTotal *= -1;
                oc.TaxAmountTotal *= -1;
                oc.TaxAmountOrderItems *= -1;
                oc.TaxAmount *= -1;
                oc.TaxAmountCity *= -1;
                oc.TaxAmountState *= -1;
                oc.TaxAmountCounty *= -1;
                oc.TaxAmountDistrict *= -1;
                oc.TaxAmountCountry *= -1;
                oc.TaxAmountShipping *= -1;
            });
        }

        public void ParentUnselected(int OrderItemID)
        {
            OrderCustomers.Each(oc =>
            {
                oc.OrderItems.Where(x => x.OrderItemID == OrderItemID).Each(i =>
                {
                    i.Quantity *= -1;
                    i.AdjustedPrice *= 0;
                });
                oc.OrderPayments.Each(op => op.Amount *= 0);
                oc.Total *= 0;
                oc.Subtotal *= 0;
                oc.CommissionableTotal *= 0;
                oc.ShippingTotal *= 0;
                oc.TaxableTotal *= 0;
                oc.TaxAmountTotal *= 0;
                oc.TaxAmountOrderItems *= 0;
                oc.TaxAmount *= -1;
                oc.TaxAmountCity *= 0;
                oc.TaxAmountState *= 0;
                oc.TaxAmountCounty *= 0;
                oc.TaxAmountDistrict *= 0;
                oc.TaxAmountCountry *= 0;
                oc.TaxAmountShipping *= 0;
            });
        }

        public void OnOrderSuccessfullyCompleted()
        {
            OrderService.OnOrderSuccessfullyCompleted(this);
        }

        public void AddOrderAdjustment(IOrderAdjustment adjustment)
        {
            Contract.Assert(adjustment is OrderAdjustment);

            OrderAdjustments.Add((OrderAdjustment)adjustment);
        }

        /// <summary>
        /// Gets the order adjustment IDs for account ID.  This should probably be refactored into the order domain.
        /// </summary>
        /// <param name="accountID">The account ID.</param>
        /// <returns></returns>
        public IEnumerable<int> GetExistingOrderAdjustmentIDsForAccount(int accountID)
        {
            var accts = new NetStepsEntities().GetOrderAdjustmentIDsForAccountID(accountID);
            return accts.Where(x => x.HasValue).Select(x => x.Value);
        }

        /// <summary>
        /// Returns all of the OrderItems attached to every OrderCustomer
        /// </summary>
        /// <returns>List of every OrderItem from every OrderCustomer</returns>
        public IEnumerable<OrderItem> GetAllOrderItems()
        {
            var allOrderItems = new List<OrderItem>();
            var orderCustomers = this.GetOrderCustomers();

            foreach (var orderCustomer in orderCustomers)
            {
                IEnumerable<OrderItem> orderItems = orderCustomer.OrderItems;

                if (orderItems == null || !orderItems.Any())
                {
                    orderItems = orderCustomer.LoadOrderItems();
                }

                if (orderItems != null)
                {
                    allOrderItems.AddRange(orderItems);
                }
            }

            return allOrderItems;
        }

        public IList<OrderCustomer> GetOrderCustomers()
        {
            var repository = this.GetRepository();
            IEnumerable<OrderCustomer> orderCustomers = this.OrderCustomers;

            if (orderCustomers == null || !orderCustomers.Any())
            {
                orderCustomers = repository.LoadOrderCustomers(this.OrderID);
            }

            return new List<OrderCustomer>(orderCustomers);
        }

        IList<IAutoshipOrder> IOrder.AutoshipOrders
        {
            get { return this.AutoshipOrders.Cast<IAutoshipOrder>().ToList(); }
        }

        bool IOrder.CalculationsDirty
        {
            get
            {
                return this.CalculationsDirty;
            }
            set
            {
                this.CalculationsDirty = value;
            }
        }

        IList<IOrderCustomer> IOrder.OrderCustomers
        {
            get
            {
                return OrderCustomers.Cast<IOrderCustomer>().ToList();
            }
        }

        IList<IOrderAdjustment> IOrder.OrderAdjustments
        {
            get
            {
                return OrderAdjustments.Cast<IOrderAdjustment>().ToList();
            }
        }

        IOrderItem IOrder.AddItem(int ProductID, int Quantity)
        {
            return this.AddItem(ProductID, Quantity);
        }

        void IOrder.Save()
        {
            this.Save();
        }

        IOrderItem IOrder.AddItem(IOrderCustomer customer, int productId, int quantity, string parentGuid, int? dynamicKitGroupId, short? orderItemParentTypeID, bool disableDuplicateChecking)
        {
            Contract.Assert(customer is OrderCustomer);

            InventoryBaseRepository repository = Create.New<InventoryBaseRepository>();
            Product product = repository.GetProduct(productId);

            return this.AddItem(customer as OrderCustomer, product, quantity, parentGuid, dynamicKitGroupId);
        }

        public bool ClearAdjustments()
        {
            var providerRegistry = Create.New<IDataObjectExtensionProviderRegistry>();
            _orderAdjustments.ToList().ForEach(x =>
            {
                var provider = providerRegistry.RetrieveExtensionProvider(x.ExtensionProviderKey);
                provider.DeleteDataObjectExtension(x);
                x.OrderAdjustmentOrderLineModifications.ToList().ForEach(mod =>
                {
                    if (mod.ModificationOperationID == (int)OrderAdjustmentOrderLineOperationKind.AddedItem && mod.OrderItem.OrderCustomer != null)
                    {
                        RemoveItem(mod.OrderItem);
                    }
                    mod.MarkAsDeleted();
                });
                x.OrderAdjustmentOrderModifications.ToList().ForEach(mod =>
                {
                    mod.MarkAsDeleted();
                });
                x.MarkAsDeleted();
            });
            foreach (var orderCustomer in OrderCustomers)
            {
                orderCustomer.OrderAdjustmentOrderModifications.Clear();
                foreach (var orderItem in orderCustomer.OrderItems)
                {
                    orderItem.OrderAdjustmentOrderLineModifications.Clear();
                }
            }
            _orderAdjustments.Clear();
            return true;
        }

        public static IList<int> GetValidOrderStatusIdsForOrderAdjustment()
        {
            return OrderService.GetValidOrderStatusIdsForOrderAdjustment();
        }

        ICollection<IOrder> IOrder.ChildOrders
        {
            get
            {
                return this.ChildOrders.ToArray();
            }
        }

        [Obsolete("Do not call this!!!  Use the OrderService!!!")]
        public void CalculateTotals()
        {

        }

        #region CSTI - FHP (PreOrder)

        public static int getPreOrder(int accountID, int siteID, int? OrderID)
        {
            PreOrder objE = new PreOrder();
            if (OrderID > 0)
            {
                ////Modelo que se carga al inicializar la PreOrder
                //objE.AccountID = accountID;
                //objE.SiteID = siteID;
                //objE.WareHouseID = WarehouseExtensions.WareHouseByAddresID(accountID);
                //objE.OrderType = "Order";
                ////objE.getShippingDate = Order.GetShippingMethods(PreOrders.WareHouseID, 1);
                //objE.PreOrderId = 
                // objE.OrderID = OrderID.Value;  
                return Order.GetPreOrderByOrderID(OrderID.Value);

            }
            else
            {
                ////Modelo que se carga al inicializar la PreOrder
                //objE.AccountID = accountID;
                //objE.SiteID = siteID;
                //objE.WareHouseID = WarehouseExtensions.WareHouseByAddresID(accountID);
                //objE.OrderType = "PreOrder";
                ////objE.getShippingDate = Order.GetShippingMethods(PreOrders.WareHouseID, 1);
                //objE.PreOrderId = 
                return PreOrderBusinessLogic.Instance.CreatePreOrder(accountID, siteID);
            }
        }

        public static decimal GetCredit()
        {
            //Proceso pendiente no se invocara por ahora según el Req: BR-PD-002
            decimal TotalCredit = 0;
            return TotalCredit;
        }

        public static int UPDPaymentConfigurations(int OrderPaymentID, int OrderID, int PaymentConfigurationID, int? Cuota, OrderPaymentsParameters p, string GeneralParameterVal)
        {
            return PaymentsMethodsExtensions.UPDPaymentConfigurations(OrderPaymentID, OrderID, PaymentConfigurationID, Cuota, p, GeneralParameterVal);
        }
        public static List<PaymentsTable> GetPaymentsTable(PaymentsTable param)
        {
            return PaymentsMethodsExtensions.GetPaymentsTable(param);
        }
        public static int UPDAccountCredit(int AccountID, decimal ValorComparacion)
        {
            return PaymentsMethodsExtensions.UPDAccountCredit(AccountID, ValorComparacion);
        }

        public static bool UPDOrderPayments(int OrderID)
        {
            return PaymentsMethodsExtensions.UPDOrderPayments(OrderID);
        }

        public static Dictionary<string, string> SelectPaymentMethod(int AccountID, int OrderTypeID)
        {
            return GetPaymentMethods.GetPaymentMethod(AccountID, OrderTypeID);
        }

        public static int GetApplyPayment(ApplyPaymentSearchData param)
        {
            return PaymentsMethodsExtensions.GetApplyPayment(param);
        }

        public static int GetApplyPaymentType(int PaymentConfigurationID)
        {
            return PaymentsMethodsExtensions.GetApplyPaymentType(PaymentConfigurationID);
        }

        public static decimal GetMinimunAmount()
        {
            //Pendiente por definir segun el requerimiento BR-PD-002
            decimal TotalCredit = 400;
            return TotalCredit;
        }

        public static decimal GetProductCredit(int accountID)
        {
            decimal TotalCredit = 500;
            return TotalCredit;
        }

        //public static int IsActiveProductMaterial(int ProductID)
        public static List<int> TakeActiveProductMaterial(IEnumerable<int> Products)
        {
            //return PreOrderExtension.IsActiveProductMaterial(ProductID);
            return PreOrderExtension.TakeActiveProductMaterial(Products);
        }

        public static int WareHouseByAccountAddress(int accountID, int addressID)
        {
            return WareHouseMaterialBussinessLogic.Instance.WareHouseByAccountAddress(accountID, addressID);
        }

        public static DateTime GetShippingMethods(int WareHouse, int PostalCode)
        {
            return DateTime.Now;
        }

        public static int UPDOrderbyID(int OrderID, int TypeSave, int WarehouseID, int SelectedPeriod, int AccountTypeID) //usada para cuenta tipo testing (GMP)
        {
            return PaymentsMethodsExtensions.UPDOrderbyID(OrderID, TypeSave, WarehouseID, SelectedPeriod, AccountTypeID);
        }

        public static int UPDOrderbyID(int OrderID, int TypeSave, int WarehouseID)
        {
            return PaymentsMethodsExtensions.UPDOrderbyID(OrderID, TypeSave, WarehouseID);
        }

        public static int UPDOrderShipments(int OrderShipmentID, string PostalCode, int WareHouseID, int OrderID, string DateEstimated)
        {
            return PaymentsMethodsExtensions.UPDOrderShipments(OrderShipmentID, PostalCode, WareHouseID, OrderID, DateEstimated);
        }

        public static int UPDOrderItemProduct(int OrderItemID, int OrderId)
        {
            return PaymentsMethodsExtensions.UPDOrderItemProduct(OrderItemID, OrderId);
        }

        public static int UPDOrderItemClaimsProduct(int OrderItemID, int OrderId)
        {
            return PaymentsMethodsExtensions.UPDOrderItemClaimsProduct(OrderItemID, OrderId);
        }

        public static decimal GetProductCreditByAccount(int accountID)
        {
            return PreOrderExtension.GetProductCreditByAccount(accountID);
        }

        public static decimal GetProductCreditByAccountDet(int OrderNumber)
        {
            return PreOrderExtension.GetProductCreditByAccountDet(OrderNumber);
        }

        public static int UPDOrderItemProductReturn(int OriginalOrderID, int ReturnOrderID, List<ReturnOrderItemDto> OrderItemList, int ModifiedByUser, bool EsOrdenRetornoCompleta)
        {
            return OrderExtensions.UPDOrderItemProductReturn(OriginalOrderID, ReturnOrderID, OrderItemList, ModifiedByUser, EsOrdenRetornoCompleta);
        }

        public static void SaveAllocation(int materialID, int quantity, int wareHouseId, int productId, int QuantityProduct, int preOrderId, int AccountTypeID,bool isClaims)
        {
            if (!AccountTypeID.Equals(ConstantsGenerated.AccountType.Testing.GetHashCode()))
                WareHouseMaterialBussinessLogic.Instance.UpdWareHouseMaterials(materialID, quantity, wareHouseId);
            WareHouseMaterialBussinessLogic.Instance.InsWareHouseMaterialAllocationPreOrder(materialID, wareHouseId, quantity, productId, preOrderId, QuantityProduct, isClaims);
        }

        public static List<ApplyPaymentSearchData.OrderShipment> GetOrderShipment(int OrderCustomerID)
        {
            try
            {
                return PaymentsMethodsExtensions.GetOrderShipment(OrderCustomerID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MensajeSearchData> GenerateAllocation(int productID,
                                                                 int quantity,
                                                                 int orderID,
                                                                 int wareHouseID,
              EntitiesEnums.MaintenanceMode numManteminiento,
                                                                 int preOrderId,
                                                                 int AccountTypeID,
              bool isClaims)
        {

            List<MensajeSearchData> objMesage = new List<MensajeSearchData>();

            //Inicio SubProceso - Agregar producto a la orden
            if (numManteminiento == EntitiesEnums.MaintenanceMode.Add)
            {
                objMesage = AddLineOrder(productID, quantity, wareHouseID, preOrderId, AccountTypeID, isClaims);
            }
            else if (numManteminiento == EntitiesEnums.MaintenanceMode.Update)
            {
                //Inicio SubProceso – Modificar cantidad a una línea
                UpdateLineOrder(productID, quantity, wareHouseID, preOrderId, AccountTypeID);
                objMesage = AddLineOrder(productID, quantity, wareHouseID, preOrderId, AccountTypeID, isClaims);
            }
            else if (numManteminiento == EntitiesEnums.MaintenanceMode.Delete)
            {
                //Inicio SubProceso – Remover una línea de la orden
                RemoveLineOrder(productID, wareHouseID, preOrderId, AccountTypeID, isClaims);

                /*CS.24MAY2016.Inicio*/
                ProductPriceTypeOrder entidad = new ProductPriceTypeOrder();
                entidad.OrderID = orderID;
                entidad.PreOrderID = preOrderId;
                entidad.ProductID = productID;
                ManProductPriceTypeOrder.Delete(entidad);
                /*CS.24MAY2016.Fin*/
            }
            return objMesage;
        }


        /*
         * Proceso que toma la lista de los Dispatch del SP getDispatchxAccountxProducts y los valida
         * para determinar si tienen o no Stock para ser enviados en el pedido de la consultora.
         * wv:20160523
         */
        public static List<DispatchProducts> getDispatchProducts(int accountId, int orderId, int orderTypeID, int wareHouseId, int PreOrder, int ProductPriceTypeID, int accounType, bool isDispatch, bool insert, bool existListDispatch)
        {
            Dictionary<int, bool> result = new Dictionary<int, bool>();
            result = NetSteps.Data.Entities.Periods.GetPeriodByDate(DateTime.Now);
            var productsDispatch = OrderExtensions.GetDispatchProducts(accountId, result.ElementAt(0).Key);
            List<DispatchProducts> objProductsDispatch = new List<DispatchProducts>();
            if ((productsDispatch.Count > 0))
            {
                //Si encontramos ProductsDispatch
                //List<DispatchProducts> objProductsDispatch = new List<DispatchProducts>();

                foreach (var item in productsDispatch)
                {
                    List<MensajeSearchData> GA = GenerateAllocation(item.ProductId, item.Quantity, orderId,
                       wareHouseId, EntitiesEnums.MaintenanceMode.Add, PreOrder, accounType, false);
                    if (!GA[0].Estado)
                    {
                        continue;
                    }
                    else
                    {
                        //Si hay stock disponible
                        DispatchProducts objPD = new DispatchProducts();
                        objPD.Name = item.Name;
                        objPD.ProductId = item.ProductId;
                        objPD.Quantity = item.Quantity;
                        objPD.SKU = item.SKU;
                        objPD.ParentOrderItemID = item.ParentOrderItemID;
                        objPD.OrderItemID = item.OrderItemID;
                        objPD.OrderDispatchID = item.OrderDispatchID;
                        objPD.NameType = item.NameType;
                        objProductsDispatch.Add(objPD);
                    }
                }
                return objProductsDispatch;
            }
            else
            {
                //Si no encontramos ProductsDispatch
                return objProductsDispatch;
            }
        }
        // ---------------------------------------------------------------------------------------------------------------------------------------------




        public static List<ProductClains> GetClains(int accountId, int orderId, int orderTypeID, int wareHouseId, int PreOrder, int ProductPriceTypeID, int accounType, bool isClaims, bool insert)
        {
            //Found ClainsItems
            var orderItemsClains = OrderExtensions.GetProductClains(accountId);

            if (orderItemsClains.Count > 0)
            {
                List<ProductClains> objProductClains = new List<ProductClains>();
                foreach (var item in orderItemsClains)
                { 
                    //Valid disponibility
                    if (item.ParentOrderItemID == null)
                    {
                        var GA = GenerateAllocation(item.ProductId, item.Quantity, orderId, wareHouseId, EntitiesEnums.MaintenanceMode.Add,
                            PreOrder, accounType, isClaims);
                        if (GA.Count == 0)
                        {
                            ProductClains objE = new ProductClains();
                            objE.Name = item.Name;
                            objE.ProductId = item.ProductId;
                            objE.Quantity = item.Quantity;
                            objE.SKU = item.SKU;
                            objE.ParentOrderItemID = item.ParentOrderItemID;
                            objE.OrderItemID = item.OrderItemID;
                            objE.OrderClaimID = item.OrderClaimID;
                            objProductClains.Add(objE);
                        }
                        else
                        {
                            foreach (var objGA in GA)
                            {
                                //No Stock
                                if (!objGA.Estado)
                                {
                                    //Return Money
                                    break;
                                }
                                else
                                {
                                    ProductClains objE = new ProductClains();
                                    objE.Name = item.Name;
                                    objE.ProductId = item.ProductId;
                                    objE.Quantity = item.Quantity;
                                    objE.SKU = item.SKU;
                                    objE.ParentOrderItemID = item.ParentOrderItemID;
                                    objE.OrderItemID = item.OrderItemID;
                                    objE.OrderClaimID = item.OrderClaimID;
                                    objProductClains.Add(objE);
                                }
                            }
                        }

                    }
                    else
                    {
                        ProductClains objE = new ProductClains();
                        objE.Name = item.Name;
                        objE.ProductId = item.ProductId;
                        objE.Quantity = item.Quantity;
                        objE.SKU = item.SKU;
                        objE.ParentOrderItemID = item.ParentOrderItemID;
                        objE.OrderItemID = item.OrderItemID;
                        objE.OrderClaimID = item.OrderClaimID;
                        objProductClains.Add(objE);
                    }
                }
                return objProductClains;
            }
            else
            {
                return null;
            }
        }
        public Boolean CheckStock(int productId, int wareHouseID) 
        {
            List<InventoryCheck> InventoryCheck = PreOrderBusinessLogic.Instance.InventoryCheckResult(productId, wareHouseID);
            if (InventoryCheck.Count > 0)
            {
                foreach (var item in InventoryCheck)
                {
                    if (item.Available <= 0)
                    {
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
            return true;
        }


        public static List<MensajeSearchData> AddLineOrder(int productId,
                                                           int quantity,
                                                           int wareHouseID,
                                                           int preOrderID,
         int accountTypeID,
                                                           bool isClaims)
        {
            //Paso 2  = Validar disponibilidad de producto
            List<MensajeSearchData> objMesage = new List<MensajeSearchData>();
            var objAddLineValidStock = PreOrderExtension.GetAddLineValidStock(wareHouseID, quantity, productId, preOrderID, accountTypeID);
            if (objAddLineValidStock.Any(x => x.materialID == 0))
            {
                objMesage.Add(new MensajeSearchData() { Message = "Produto Indisponível Em Estoque", Estado = false });
            }
            else
            {
                objMesage.Add(new MensajeSearchData() { Message = "", Estado = true });
            }

            return objMesage;
        }

        public static void UpdateLineOrder(int ProductID,
                                           int Quantity,
                                           int wareHouseID,
                                           int preOrderId,
                                           int AccountTypeID)
        {

            RemoveLineOrder(ProductID, wareHouseID, preOrderId, AccountTypeID, false);
        }

        public static void RemoveLineOrder(int productID,
                                           int wareHouseID,
                                           int preOrderId,
                                           int AccountTypeID,
                                           bool isClaims)
        {
            List<CheckKitInventory> materialAllocations = PreOrderExtension.GetWareHouseMaterialAllocations(preOrderId,
                productID);
            int rowsAfec = 0;
            foreach (var item in materialAllocations)
            {
                if (!AccountTypeID.Equals(ConstantsGenerated.AccountType.Testing.GetHashCode()))
                {
                    rowsAfec = rowsAfec +
                               PreOrderExtension.UpdWarehouseMaterial(item.MaterialID, item.Available, wareHouseID);
                }
            }

            if (rowsAfec > 0)
            {
                PreOrderExtension.DelWareHouseMaterialsAllocationsXPreOrder(productID, preOrderId);
            }

            //Invocar al proceso IncludePromotionProducts
            IncludePromotionProducts(productID, wareHouseID, preOrderId, AccountTypeID, isClaims);
        }

        public static decimal GetCreditLedgerByAccountID(int AccountID)
        {
            return PreOrderBusinessLogic.Instance.GetCreditLedgerByAccountID(AccountID);
        }

        public static List<MensajeSearchData> RecorrerIncludeReplacement(int productID,
                                                                         int materialID,
                                                                         int quantity,
                                                                         int wareHouseID,
                                                                         int orderID,
                                                                         string orderType,
                                                                         int selectedUnits,
                                                                         int Priority,
                                                                         int preOrderId,
                                                                         int AccountTypeID,
                                                                         bool isClaims)
        {
            List<MensajeSearchData> objMessage = new List<MensajeSearchData>();
            List<Replacement> Replacement = PreOrderBusinessLogic.Instance.ReplacementResult(Priority, productID, materialID);
            if (objMessage.Count == null)
            {
                objMessage.Add(new MensajeSearchData() { Message = "Produto Indisponível Em Estoque", Estado = false });
                return objMessage;
            }
            int QuantityTemp = quantity;
            foreach (var replacement in Replacement)
            {
                //Paso 4 = Si el resultado de la consulta es vacio finaliza el proceso.                 
                if (replacement.MaterialID == null)
                {
                    objMessage.Add(new MensajeSearchData() { Message = "Produto Indisponível Em Estoque", Estado = false });
                }
                //Paso 5 = Caso contrario vereficar disponiblidad de inventario
                else
                {
                    //Paso 6 = Asignar a la variable InventoryCheck
                    List<IncludeInventoryCheck> InventoryCheck = PreOrderBusinessLogic.Instance.IncludeInventoryCheckResult(replacement.MaterialID, wareHouseID);
                    //Paso 7 = Validar 
                    foreach (var inventoryCheck in InventoryCheck)
                    {
                        //Paso 7 = Validar  si Cumple 
                        if (QuantityTemp > inventoryCheck.Available)
                        {
                            //Paso 8 = inventoryCheck.Available > 0
                            if (inventoryCheck.Available > 0)
                            {
                                //ProductRelations objE = new Business.ProductRelations();
                                //objE.MaterialId = inventoryCheck.MaterialID;
                                //objE.ProductID = productID;
                                //objE.Quantity = inventoryCheck.Available;
                                //productRelations.Add(objE);
                                selectedUnits = selectedUnits + inventoryCheck.Available;

                                SaveAllocation(inventoryCheck.MaterialID, inventoryCheck.Available, wareHouseID, productID, quantity, preOrderId, AccountTypeID, isClaims);
                                materialID = replacement.MaterialID;
                                //Falta Stock
                                objMessage.Add(new MensajeSearchData() { Message = "Existe apenas " + selectedUnits + " unidades para este producto em estoque", Estado = true, NewQuantity = selectedUnits });
                            }
                            else if (inventoryCheck.Available <= 0 || selectedUnits == 0)
                            {
                                //productRelations.RemoveWhere(x => x.MaterialId == inventoryCheck.MaterialID);
                                objMessage.Add(new MensajeSearchData() { Message = "Produto Indisponível Em Estoque", Estado = false });
                                break;
                            }
                            else if (inventoryCheck.Available <= 0 && selectedUnits > 0)
                            {
                                //foreach (var item in productRelations)
                                //{
                                //    RemoveLineOrder(item.ProductID, item.Quantity, orderID, orderType, wareHouseID,preOrderId);
                                //    break;
                                //}
                                //productRelations.RemoveWhere(x => x.MaterialId == inventoryCheck.MaterialID);
                                objMessage.Add(new MensajeSearchData() { Message = "Existe apenas " + selectedUnits + " unidades para este producto em estoque", Estado = true });
                            }
                        }
                        else
                        {
                            //Paso 9
                            //ProductRelations objE = new Business.ProductRelations();
                            //objE.ProductIDRelation = productID;
                            //objE.ProductID = replacement.ProductID;
                            //objE.MaterialId = inventoryCheck.MaterialID;
                            //objE.Quantity = quantity;
                            //productRelations.Add(objE);
                            SaveAllocation(inventoryCheck.MaterialID, quantity, wareHouseID, replacement.ProductID, quantity, preOrderId, AccountTypeID, isClaims);
                            objMessage.Add(new MensajeSearchData() { Message = "", Estado = true });
                        }
                    }

                }
            }
            return objMessage;
        }

        public static List<MensajeSearchData> IncludeReplacement(int productID,
                                                                 int materialID,
                                                                 int quantity,
                                                                 int wareHouseID,
                                                                 int orderID,
                                                                 string orderType,
                                                                 int selectedUnits,
                                                                 int preOrderID,
                                                                 int AccountTypeID,
                                                                 bool isClaims)
        {
            List<MensajeSearchData> objMessage = new List<MensajeSearchData>();
            //Paso 1 -- Inicializar la variable K = 0 
            //Paso 2 --Ejecutar un contador 
            int QuantityTemp = quantity;
            /////Variable del Paso9  
            //Paso 3 --Asignar el resultado a la variable Replacement
            int Priority = PreOrderExtension.GetIncludeReplacementPriority(productID, materialID);

            for (int i = 1; i <= Priority; i++)
            {
                if (objMessage.Count > 0)
                {
                    int Newquantity = objMessage[i].NewQuantity;
                    objMessage = RecorrerIncludeReplacement(productID, materialID, quantity, wareHouseID, orderID, orderType, Newquantity, i, preOrderID, AccountTypeID, isClaims);
                }
                else
                {
                    objMessage = RecorrerIncludeReplacement(productID, materialID, quantity, wareHouseID, orderID, orderType, selectedUnits, i, preOrderID, AccountTypeID, isClaims);
                }
                if (objMessage[0].Estado)
                {
                    break;
                }
            }
            if (Priority == 0)
            {
                objMessage.Add(new MensajeSearchData() { Message = "", Estado = false });
            }
            if (!objMessage[0].Estado)
            {
                //foreach (var item in productRelations)
                //{
                //    RemoveLineOrder(item.ProductID, item.Quantity, orderID, orderType, wareHouseID, preOrderID);
                //    break;
                //}
            }
            return objMessage;
        }

        public static void IncludePromotionProducts(int PromotionProductID,
                                                    int wareHouseID,
                                                    int preOrderId,
                                                    int AccountTypeID,
                                                    bool isClaims)
        {
            //Validar que productos se encuentran en promoción.
            List<ProductPromotion> promotionByProductID = PreOrderExtension.GetPromotionByProductID(PromotionProductID);
            foreach (var item in promotionByProductID)
            {
                AddLineOrder(item.PromotionProductID, item.Quantity, wareHouseID, preOrderId, AccountTypeID, isClaims);
            }
        }

        public static void ClearAllocation(int MaterialID, int Quantity, int wareHouseID, int preOrderId)
        {
            //Paso 1 = Actualizar el registro en WareHouseMaterials
            PreOrderExtension.UpdWarehouseMaterial(MaterialID, Quantity, wareHouseID);
            //PreOrderExtension.DelWareHouseMaterialsAllocationsXPreOrder(MaterialID,preOrderId,wareHouseID);
            //if (preOrder.OrderType == "PreOrder")
            //{
            //    PreOrderExtension.DelWareHouseMaterialsAllocationsXPreOrder(MaterialID, preOrder);
            //}
            //else if (preOrder.OrderType == "Order")
            //{
            //    PreOrderExtension.DelWareHouseMaterialsAllocationXOrder(preOrder.OrderID, MaterialID, preOrder.WareHouseID);
            //}
        }

        public static int CheckKitReplacement(int ProductID, int MaterialID, int Quantity, int SelectedUnits, int preOrderId, int wareHouseId)
        {
            //Variables por definir  
            //Paso 1 = Inicializar k 
            int k = 0;
            int Priority = PreOrderExtension.GetIncludeReplacementPriority(ProductID, MaterialID);
            do
            {

                //Paso 2 = Declarar QuantityTemp
                int QuantityTemp = Quantity;
                //Paso 3 = Generar el contador
                k = k + 1;
                //Paso 4 = Asignar el resultado de la consulta a la variable Replacement
                List<CheckKitReplacement> Replacements = PreOrderBusinessLogic.Instance.CheckKitReplacementResult(k, ProductID, MaterialID);
                foreach (var replacement in Replacements)
                {
                    //Paso 5 = Si el valor es null finalizar el proceso
                    if (replacement.MaterialID == null)
                    {
                        return 0;
                    }
                    //Paso 6 = Vereficar disponibilidad en Inventario 
                    else
                    {
                        //Paso 7 = Asignar la consulta a la variable 
                        List<CheckKitInventory> InventoryCheck = PreOrderBusinessLogic.Instance.CheckKitInventoryResult(replacement.MaterialID, wareHouseId);
                        foreach (var inventoryCheck in InventoryCheck)
                        {
                            //Paso 8 = Validar cantidades
                            if (QuantityTemp > inventoryCheck.Available)
                            {
                                //Paso 9 = Validar que sea mayor a 0
                                if (inventoryCheck.Available > 0)
                                {
                                    SelectedUnits = SelectedUnits + inventoryCheck.Available;
                                    MaterialID = replacement.MaterialID;
                                    QuantityTemp = inventoryCheck.Available;
                                    //Volver al paso 3 ???
                                    k = k + 1;
                                    break;

                                }
                                else if (inventoryCheck.Available <= 0 && SelectedUnits == 0)
                                {
                                    return 0;
                                }
                                else
                                {
                                    if (inventoryCheck.Available <= 0 && SelectedUnits > 0)
                                    {
                                        return SelectedUnits;
                                    }
                                }
                            }
                            else
                            {
                                //ProductRelations objE = new Business.ProductRelations();
                                //objE.MaterialId = inventoryCheck.MaterialID;
                                //objE.ProductID = ProductID;
                                //objE.Quantity = Quantity;
                                //objE.isKit = 1;
                                //objE.FinalAvailable = null;
                                //objE.MaterialRelarionID = MaterialID;
                                //productRelations.Add(objE); 
                                //Paso 10 
                                SelectedUnits = Quantity + SelectedUnits;
                                return SelectedUnits;
                            }
                        }
                    }
                }

            } while (Priority >= k);
            return 0 + SelectedUnits;
            //Paso 11 = Finalizar el proceso

        }

        public void IncludeKitReplacement(int productID,
                                          int materialID,
                                          int quantity,
                                          int SelectedUnits,
                                          string orderType,
                                          int orderID,
                                          int wareHouseID,
                                          int preOrderID,
                                          int AccountTypeID,
                                          bool isClaims)
        {

            //Paso 4 = Asignar el resultado a la variable Replacement 
            int k = 0;
            //Paso 2 = QuantityTemp = Quantity
            int QuantityTemp = quantity;
            //Paso 3 = Generar el contador
            k = k + 1;

            List<Replacement> Replacement = PreOrderBusinessLogic.Instance.ReplacementResult(k, productID, materialID);
            foreach (var replacement in Replacement)
            {

                //Paso 5 = Si el resultado de la consulta es vacio finaliza
                if (replacement.MaterialID == null)
                {
                    return;
                }
                else
                {
                    //Paso 6 = vereficar disponibilidad en inventario del replacement
                    //Paso 7 = Asignar el resultado a la variable inventoryCheck
                    List<IncludeInventoryCheck> InventoryCheck = PreOrderBusinessLogic.Instance.IncludeInventoryCheckResult(replacement.MaterialID, wareHouseID);

                    foreach (var inventoryCheck in InventoryCheck)
                    {
                        //Paso 8 = Validar que quantity sea mayor que inventoryCheck.Available
                        if (quantity > inventoryCheck.Available)
                        {
                            //Paso 9 = Invoca al proceso SaveAllocation
                            SaveAllocation(inventoryCheck.MaterialID, inventoryCheck.Available, wareHouseID, productID, quantity, preOrderID, AccountTypeID, isClaims);
                            SelectedUnits = SelectedUnits + inventoryCheck.Available;
                            materialID = replacement.MaterialID;
                            //Vuelve al paso 3
                            k = k + 1;
                            List<Replacement> ReplacementForInventory = PreOrderBusinessLogic.Instance.ReplacementResult(k, productID, materialID);
                            foreach (var replacementForInventory in ReplacementForInventory)
                            {
                                //Paso 5 = Si el resultado de la consulta es vacio finaliza
                                if (replacementForInventory.MaterialID == null)
                                {
                                    return;
                                }
                                else
                                {
                                    //Paso 6 = vereficar disponibilidad en inventario del replacement
                                    //Paso 7 = Asignar el resultado a la variable inventoryCheck
                                    List<IncludeInventoryCheck> InventoryCheckForInventory = PreOrderBusinessLogic.Instance.IncludeInventoryCheckResult(replacement.MaterialID, wareHouseID);

                                    foreach (var inventoryCheckForInventory in InventoryCheckForInventory)
                                    {
                                        //Paso 8 = Validar que quantity sea mayor que inventoryCheck.Available
                                        if (quantity > inventoryCheckForInventory.Available)
                                        {
                                            SaveAllocation(materialID, inventoryCheckForInventory.Available, wareHouseID, productID, quantity, preOrderID, AccountTypeID, isClaims);
                                            SelectedUnits = SelectedUnits + inventoryCheckForInventory.Available;
                                            materialID = replacementForInventory.MaterialID;
                                        }
                                    }
                                }
                                if (inventoryCheck.Available <= 0 && SelectedUnits > 0)
                                {
                                    return;
                                }

                            }
                        }
                        else
                        {
                            SaveAllocation(materialID, QuantityTemp, wareHouseID, productID, quantity, preOrderID, AccountTypeID, isClaims);
                            //return 
                        }
                    }

                }
            }

            return;
        }

        public static List<OrdersStatus> GetStatusOrder(int? accountID)
        {
            return PreOrderBusinessLogic.Instance.GetStatusOrder(accountID);
        }

        public static void UpdatePreOrder(int orderId, int preOrderID)
        {
            PreOrderBusinessLogic.Instance.UpdatePreOrder(preOrderID, orderId);
        }

        public void AsignarReservasAOrden(int orderId, int preOrderID)
        {
            PreOrderBusinessLogic.Instance.AsignarReservasAOrden(orderId, preOrderID);
        }

        public List<BackOrderedProductData> GetBackOrderedProductData(int accountID)
        {
            try
            {
                return OrderExtensions.GetBackOrderedProductData(accountID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MaterialName> GetMaterialWithMaterialID(int WareHouseID, int PreOrderID)
        {
            try
            {
                return OrderExtensions.GetMaterialWithMaterialID(WareHouseID, PreOrderID);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static List<MaterialName> GetProductDetails(int OrderID)
        {
            try
            {
                return OrderExtensions.GetProductDetails(OrderID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MaterialName> GetWareHouseMaterialsByOrderId(int OrderID, int ProductID)
        {
            try
            {
                return OrderExtensions.GetWareHouseMaterialsByOrderId(OrderID, ProductID);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public static List<WareHouseMaterialAllocations> GetWareHouseMaterialAllocations(int PreOrderID)
        {
            try
            {
                return OrderExtensions.GetWareHouseMaterialAllocations(PreOrderID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int DelWareHouseMaterialAllocations(int PreOrderId)
        {
            try
            {
                return OrderExtensions.DelWareHouseMaterialAllocations(PreOrderId);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public static List<WareHouseMaterialAllocations> GetWareHouseMaterialAllocationsbyProduct(int PreOrderID)
        {
            try
            {
                return OrderExtensions.GetWareHouseMaterialAllocationsbyProduct(PreOrderID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MaterialWareHouseMaterial> GetMaterialWareHouseMaterial(int OrderId)
        {
            try
            {
                return OrderExtensions.GetMaterialWareHouseMaterial(OrderId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MaterialWareHouseMaterial> GetMaterialWareHouseMaterialPWS(int OrderId)
        {
            try
            {
                return OrderExtensions.GetMaterialWareHouseMaterialPWS(OrderId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetOrderPending(int AccountID, int SiteID)
        {
            try
            {
                return OrderExtensions.GetOrderPending(AccountID, SiteID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<PreOrderCondition> GetPreOrderConditions(int AccountID, int LanguageID)
        {
            try
            {
                return OrderExtensions.GetPreOrderConditions(AccountID, LanguageID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int GetPreOrderPending(string OrderNumber, int SiteID)
        {
            try
            {
                return OrderExtensions.GetPreOrderPending(OrderNumber, SiteID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<MaterialOrderItem> GetMaterialOrderItem(int OrderCustomerID)
        {
            try
            {
                return OrderExtensions.GetMaterialOrderItem(OrderCustomerID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<OrderItemsStock> uspGetOrderItemsStock(int OrderCustomerID)
        {
            try
            {
                return OrderExtensions.uspGetOrderItemsStock(OrderCustomerID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int GetPreOders(int AccountID, int SiteID)
        {
            try
            {
                return OrderExtensions.GetPreOders(AccountID, SiteID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int GetShippingMethodID(int ShippingOrderTypeID)
        {
            try
            {
                return OrderExtensions.GetShippingMethodID(ShippingOrderTypeID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int GetProOrderUpdate(int AccountID, int SiteID)
        {
            try
            {
                return OrderExtensions.GetProOrderUpdate(AccountID, SiteID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int GetPreOrderByOrderID(int OrderID)
        {
            try
            {
                return OrderExtensions.GetPreOrderByOrderID(OrderID);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static int GetPreOderEdit(int OrderId)
        {
            try
            {
                return OrderExtensions.GetPreOderEdit(OrderId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public static void UpdOrderItemClaims(int accountID, int orderID)
        //{
        //    var getBackOrderProductData = OrderExtensions.GetBackOrderedProductData(accountID);
        //    foreach (var item in getBackOrderProductData)
        //    {
        //        OrderExtensions.UpdOrderItemClaims(orderID, item.OrderItemClaimID.Value);
        //    }
        //}

        public static void UpdWareHouseMaterialAllocations(int OrderID, int preOrderID)
        {
            OrderExtensions.UpdWareHouseMaterialAllocations(OrderID, preOrderID);
        }

        //public static int GetOrderId (string orderNumber)
        //{
        //    List<PreOrder> preOrder = PreOrderExtension.GetPreOrderByOrderNumber(orderNumber);
        //    int orderId = 0;
        //    foreach (var item in preOrder)
        //    {
        //        orderId = item.OrderID; 
        //    }

        //    return o

        //}
        #endregion

        /// <summary>
        /// Id Support ticket
        /// </summary>
     

        #region External Properties
        [System.ComponentModel.Browsable(false)]
        public string AccountNumber { get; set; }

        [System.ComponentModel.Browsable(false)]
        public string CustomerFirstName { get; set; }

        [System.ComponentModel.Browsable(false)]
        public string CustomerLastName { get; set; }

        /// <summary>
        /// Obtiene o establece Total
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public decimal TmpRetailTotal { get; set; }

        /// <summary>
        /// Obtiene o establece ordercustomerAccount
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public int TmpOrderCustomer { get; set; }

        /// <summary>
        /// Obtiene o establece 
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public int TmpAccountID { get; set; }

        /// <summary>
        /// Original Discount Temp
        /// </summary>
        [System.ComponentModel.Browsable(false)]
        public decimal TmpOrderDiscount { get; set; }
        #endregion

        public static int updateOrderStatus(string OrderNumber, int newOrderStatus)
        {
            try
            {
                int OrderStatusID = newOrderStatus;
                return OrderRepository.spUpdateStatusByOrderNumber(OrderNumber, OrderStatusID);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }

        }
        public static List<OrderTackingsSearchData> SearchOrderTrackingByOrderNumberDWS(string OrderNumber, int page, int pageSize, string column, string order)
        {
            try
            {

                List<OrderTackingsSearchData> result = new List<OrderTackingsSearchData>();
                Dictionary<string, object> parameters = new Dictionary<string, object>() { { "@OrderNumber", OrderNumber }, 
                                        { "@PageSize", pageSize }, { "@PageNumber", page }, { "@Colum", column }, { "@Order", order } };
                SqlDataReader reader = DataAccess.GetDataReader("spSearchOrderTrackingByOrderNumberDWS", parameters, "Core");

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        result.Add(new OrderTackingsSearchData()
                        {
                            OrderStatusID = Convert.ToInt32(reader["OrderStatusID"]),
                            Name = Convert.ToString(reader["Name"]),
                            InitialTackingDateUTC = Convert.ToDateTime(reader["InitialTackingDateUTC"]),
                            FinalTackingDateUTC = Convert.ToDateTime(reader["FinalTackingDateUTC"]),
                            Comment = Convert.ToString(reader["Comment"]),
                            RowTotal = Convert.ToInt32(reader["RowTotal"]),
                            Etapa = Convert.ToInt32(reader["Etapa"]),
                            ImagenStatus = Convert.ToString(reader["ImagenStatus"])
                        });
                    }
                }
                return result;


            }
            catch (Exception ex)
            {

                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsBusinessException);
            }
        }

        public static int InsertarOrderTrackings(
                                int OrderCustomerID,
                                Int16 OrderStatuses,
                                DateTime? InitialTackingDateUTC,
                                DateTime? FinalTackingDateUTC,
                                int UserID,
                                string Description
                    )
        {
            return new OrderRepository().InsertarOrderTrackings(
                        OrderCustomerID,
                        OrderStatuses,
                        InitialTackingDateUTC,
                        FinalTackingDateUTC,
                        UserID,
                        Description
                );
        }


        public static List<int> GetOrderCustomerIdsByStatus(int statusId, int minutesCount)
        {
            try
            {
                return OrderExtensions.GetOrderCustomerIdsByStatus(statusId, minutesCount);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static List<decimal> GetInvoiceNumbersByOrderID(int orderId)
        {
            try
            {
                return OrderExtensions.GetInvoiceNumbersByOrderID(orderId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static DataTable GetOrderInvoiceDetail(string invoiceNumber)
        {
            try
            {
                return OrderExtensions.GetOrderInvoiceDetail(invoiceNumber);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static PaginatedList<DailyOrderSearchData> SearchDialyOrders(DailyOrderSearchParameters parameters)
        {
            try
            {
                return OrderExtensions.SearchDialyOrders(parameters);
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, Constants.NetStepsExceptionType.NetStepsApplicationException);

            }
        }

        //INI - GR4172
        public string GetOrderAndPeriods(int orderId)
        {
            try
            {
                var periods = new List<Tuple<int, string>>();
                List<int> orderlist = new List<int>();
                orderlist.Add(orderId);
                periods = new OrderRepository().GetOrderAndPeriods(orderlist);
                return periods.FirstOrDefault().Item2;
            }
            catch (Exception)
            {
                return "";
            }
        }
        //FIN - GR4172
    }
}