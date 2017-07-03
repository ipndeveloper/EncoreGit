using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using NetSteps.Common.Configuration;
using NetSteps.Common.DataFaker;
using NetSteps.Common.Events;
using NetSteps.Common.Extensions;
using NetSteps.Common.Reflection;
using NetSteps.Common.Threading;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;
using NetSteps.Data.Entities.Extensions;
using NetSteps.Data.Entities.Processors;

namespace AutoshipProcessor
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 8/13/2010
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (ApplicationContext.Instance.IsDebug)
                HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            ApplicationContext.Instance.ApplicationID = NetSteps.Data.Entities.EntitiesHelpers.GetApplicationIdFromConnectionString();
            ApplicationContext.Instance.IsWebApp = false;
            ApplicationContext.Instance.CurrentUser = CorporateUser.LoadFull(1); // Defaulting user to Admin for now - JHE
        }

        private void GenerateTestAutoshipTemplates()
        {
            try
            {
                var results = Account.Search(new NetSteps.Data.Entities.Business.AccountSearchParameters() { PageIndex = 2, PageSize = 200 });

                var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(NetSteps.Data.Entities.Constants.AutoshipSchedule.MonthlyAutoship.ToInt());
                int marketID = NetSteps.Data.Entities.Constants.Market.UnitedStates.ToInt();

                uxAutoshipProgressControl.ProcessTitle = "Generating Autoship Templates...";
                BackgroundAction action = new BackgroundAction(() =>
                {
                    int itemsProcessed = 0;
                    foreach (var item in results)
                    {
                        AutoshipOrder autoshipOrder = AutoshipOrder.LoadByAccountIDAndAutoshipScheduleID(item.AccountID, autoshipSchedule.AutoshipScheduleID);
                        if (autoshipOrder == null)
                        {
                            string currentItemDescription = string.Format("Creating Autoship Template for {0} {1} (AccountID: {2}) ", item.FirstName, item.LastName, item.AccountID);
                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                            {
                                var progressEventArgs = new ProgressEventArgs(results.Count, itemsProcessed, currentItemDescription);
                                uxAutoshipProgressControl.UpdateProgress(progressEventArgs);

                                this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
                                this.TaskbarItemInfo.ProgressValue = progressEventArgs.PercentComplete / 100;
                            }));

                            GenerateTestAutoshipTemplate(autoshipOrder, item.AccountID, autoshipSchedule.AutoshipScheduleID, marketID);

                            itemsProcessed++;
                        }
                    }

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                    {
                        this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    }));
                });
                action.Start();
            }
            catch (Exception ex)
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Error;
                    this.TaskbarItemInfo.Description = ex.Message;
                }));
            }
        }

        private void GenerateTestAutoshipTemplate(int accountID, int autoshipScheduleID, int marketID)
        {
            AutoshipOrder autoshipOrder = AutoshipOrder.LoadFullByAccountIDAndAutoshipScheduleID(accountID, autoshipScheduleID);
            GenerateTestAutoshipTemplate(autoshipOrder, accountID, autoshipScheduleID, marketID);
        }
        private void GenerateTestAutoshipTemplate(AutoshipOrder autoshipOrder, int accountID, int autoshipScheduleID, int marketID)
        {
            try
            {
                Account account = Account.LoadFull(accountID);
                EnsureAccountHasBasicInfo(account);

                bool newAutoship;
                if (autoshipOrder == null)
                    autoshipOrder = AutoshipOrder.LoadFullByAccountIDAndAutoshipScheduleID(accountID, autoshipScheduleID);
                AutoshipSchedule schedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(autoshipScheduleID);
                var autoshipScheduleDays = SmallCollectionCache.Instance.AutoshipScheduleDays.Where(a => a.AutoshipScheduleID == schedule.AutoshipScheduleID);

                DateTime lastCreated = DateTime.Now;
                if (autoshipOrder != null)
                {
                    newAutoship = false;
                    if (autoshipOrder.DateLastCreated.HasValue)
                        lastCreated = autoshipOrder.DateLastCreated.Value;
                    //order = autoshipOrder.Order;
                }
                else
                {
                    autoshipOrder = new AutoshipOrder();
                    autoshipOrder.StartTracking();
                    autoshipOrder.AccountID = account.AccountID;
                    autoshipOrder.AutoshipScheduleID = autoshipScheduleID;
                    autoshipOrder.StartDate = DateTime.Now;
                    autoshipOrder.NextRunDate = autoshipOrder.GetNextDueDate();
                    autoshipOrder.AutoshipScheduleDayID = autoshipScheduleDays.First(asd => asd.Day == autoshipOrder.NextRunDate.ToDateTime().Day).AutoshipScheduleDayID;

                    var order = new NetSteps.Data.Entities.Order(account);
                    order.OrderTypeID = AutoshipOrder.BusinessLogic.GetDefaultOrderTypeID(account.AccountTypeID, true);
                    order.OrderStatusID = NetSteps.Data.Entities.Constants.OrderStatus.Paid.ToShort();
                    order.SiteID = ConfigurationManager.GetAppSetting<int>(ConfigurationManager.VariableKey.NSCoreSiteID);
                    order.DateCreated = DateTime.Now;
                    order.CurrencyID = SmallCollectionCache.Instance.Markets.GetById(marketID).GetDefaultCurrencyID();
                    autoshipOrder.Order = order;

                    // Add Order Item
                    var availableProducts = new List<Product>();// Inventory.Instance.Products.Where(p => p.Prices.Count(pp => pp.ProductPriceTypeID == account.ProductPriceTypeID && pp.Price != 0) > 0).ToList();
                    var product = availableProducts.GetRandom();
                    autoshipOrder.Order.AddItem(product, 1);

                    for (int i = 0; i < NetSteps.Common.Random.Next(5, 5); i++)
                    {
                        product = availableProducts.GetRandom();
                        autoshipOrder.Order.AddItem(product, 1);
                    }

                    newAutoship = true;
                }

                IEnumerable<ShippingMethodWithRate> shippingMethods;
                if (newAutoship || autoshipOrder.Order.GetDefaultShipmentNoDefault() == null)
                {
                    Address defaultShippingAddress = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Constants.AddressType.Shipping);

                    if (defaultShippingAddress == null)
                    {
                        // TODO: Create new shipping address for the customer if one doesn't exist - JHE
                        Address newAddress = new Address();
                        Reflection.CopyPropertiesDynamic<IAddress, IAddress>(FakeObjects.GetFakeIAddress(), newAddress);
                        newAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Shipping.ToShort();
                        account.Addresses.Add(newAddress);
                        //account.Save();

                        OrderShipment shipment = autoshipOrder.Order.GetDefaultShipmentNoDefault();
                        if (shipment == null)
                        {
                            shipment = autoshipOrder.Order.GetDefaultShipment();
                            Reflection.CopyPropertiesDynamic<IAddress, IAddress>(newAddress, shipment);
                        }
                    }

                    shippingMethods = defaultShippingAddress != null ? GetNewShippingMethods(autoshipOrder.Order, defaultShippingAddress.AddressID) : new List<ShippingMethodWithRate>();
                }
                else
                {
                    //TODO: fix shipping methods
                    shippingMethods = Calculations.ShippingCalculator.GetShippingMethodsWithRates(autoshipOrder.Order.OrderCustomers[0], autoshipOrder.Order.GetDefaultShipment());
                    //shippingMethods = new List<ShippingMethod>();
                }

                autoshipOrder.Order.CalculateTotals();

                // Only add Address and Payment if there is not already one there from the Account Default for Testing - JHE
                OrderCustomer orderCustomer = autoshipOrder.Order.OrderCustomers[0];
                if (orderCustomer.OrderPayments.Count == 0 && account.AccountPaymentMethods.Count > 0)
                {
                    var response = autoshipOrder.Order.ApplyPayment(account.AccountPaymentMethods[0].AccountPaymentMethodID, autoshipOrder.Order.GrandTotal.ToDecimal());

                    //OrderPayment orderPayment = FakeObjects.GetFakeOrderPayment();
                    //orderPayment.CurrencyID = autoshipOrder.Order.CurrencyID;
                    //orderPayment.PaymentTypeID = NetSteps.Data.Entities.Constants.PaymentType.CreditCard.ToInt();
                    ////orderPayment.OrderID = autoshipOrder.Order.OrderID;
                    //orderCustomer.OrderPayments.Add(orderPayment);
                }
                else
                {
                    orderCustomer.OrderPayments[0].DecryptedAccountNumber = MiscFaker.GetCreditCard();
                    orderCustomer.OrderPayments[0].ExpirationDate = MiscFaker.GetExpirationDate();
                }

                autoshipOrder.Save();
                autoshipOrder.Order.SubmitOrder();
            }
            catch (Exception ex)
            {
                throw EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
            }
        }
        private IEnumerable<ShippingMethodWithRate> GetNewShippingMethods(NetSteps.Data.Entities.Order order, int addressId)
        {
            OrderShipment shipment = order.GetDefaultShipment();
            OrderCustomer customer = order.OrderCustomers[0];

            UpdateOrderShipmentAddress(order, shipment, addressId);

            order.CalculateTotals();

            var shippingMethods = Calculations.ShippingCalculator.GetShippingMethodsWithRates(customer, shipment).OrderBy(sm => sm.ShippingAmount).ToList();

            if (shipment.OrderShipmentID == 0 || !shippingMethods.Select(sm => sm.ShippingMethodID).Contains(shipment.ShippingMethodID.ToInt()))
            {
                // Set default shipping method
                if (shippingMethods.Count() > 0)
                {
                    var cheapestShippingMethod = shippingMethods.OrderBy(sm => sm.ShippingAmount).First();

                    shipment.ShippingMethodID = cheapestShippingMethod.ShippingMethodID;
                    //shipment.Name = cheapestShippingMethod.Name; // shipment.Name should be the name of the person the package is shipping to. - JHE
                }
                //order.Save();
            }

            return shippingMethods;
        }
        private void UpdateOrderShipmentAddress(NetSteps.Data.Entities.Order order, OrderShipment shipment, int addressId)
        {
            order.UpdateOrderShipmentAddress(shipment, addressId);
        }

        private void EnsureAccountHasBasicInfo(int accountID)
        {
            Account account = Account.LoadFull(accountID);
            EnsureAccountHasBasicInfo(account);
        }
        private void EnsureAccountHasBasicInfo(Account account)
        {
            Address defaultShippingAddress = account.Addresses.GetDefaultByTypeID(NetSteps.Data.Entities.Constants.AddressType.Shipping);

            // TODO: Create new shipping address for the customer if one doesn't exist - JHE
            if (defaultShippingAddress == null)
            {
                Address newAddress = new Address();
                Reflection.CopyPropertiesDynamic<IAddress, IAddress>(FakeObjects.GetFakeIAddress(), newAddress);
                newAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Shipping.ToShort();
                account.Addresses.Add(newAddress);
            }

            // TODO: Create new payment method for the customer if one doesn't exist - JHE
            if (account.AccountPaymentMethods.Count == 0)
            {
                AccountPaymentMethod orderPayment = FakeObjects.GetFakeAccountPaymentMethod();
                orderPayment.PaymentTypeID = NetSteps.Data.Entities.Constants.PaymentType.CreditCard.ToInt();
                orderPayment.IsDefault = true;
                account.AccountPaymentMethods.Add(orderPayment);
            }

            account.Save();
        }

        private void LoadAutoships()
        {
            uxBusyControl.Start();

            BackgroundAction action = new BackgroundAction(() =>
            {
                var autoshipSchedule = SmallCollectionCache.Instance.AutoshipSchedules.GetById(NetSteps.Data.Entities.Constants.AutoshipSchedule.MonthlyAutoship.ToInt());
                //var autoshipScheduleDays = SmallCollectionCache.Instance.AutoshipScheduleDays.Where(a => a.AutoshipScheduleID == autoshipSchedule.AutoshipScheduleID).ToList();
                //var autoshipScheduleDay = autoshipScheduleDays.FirstOrDefault(a => a.Day == DateTime.Today.Day);


                //AutoshipOrder.Repository.UpdateNextRunDates(autoshipSchedule.AutoshipScheduleID);
                var autoshipTemplates = AutoshipOrder.GetAutoshipTemplatesByNextDueDateByScheduleID(autoshipSchedule.AutoshipScheduleID, DateTime.Today.Date);

                //List<AutoshipProcessInfo> autoshipTemplates;
                ////if (autoshipScheduleDay != null)
                ////    autoshipTemplates = AutoshipOrder.GetAutoshipTemplatesByNextDueDateByScheduleID(autoshipScheduleDay.AutoshipScheduleDayID, DateTime.Today.Date);

                //// TODO: Uncomment the line above and remove this line below this comment; It is like this now just for testing - JHE
                //autoshipTemplates = AutoshipOrder.GetAutoshipTemplatesByNextDueDateByScheduleID(2, DateTime.Today.Date);

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxAutoshipOrders.ItemsSource = autoshipTemplates;
                    if (autoshipTemplates.Count > 0)
                        uxAutoshipOrders.SelectedIndex = 0;
                    uxBusyControl.Stop();
                }));
            });
            action.Start();
        }

        #region Event Handlers
        private void uxAutoshipOrders_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void uxGenerateTestTemplates_Click(object sender, RoutedEventArgs e)
        {
            GenerateTestAutoshipTemplates();
        }

        private void uxFindTemplatesToGenerate_Click(object sender, RoutedEventArgs e)
        {
            LoadAutoships();
        }

        private void uxStartAutoshipGeneration_Click(object sender, RoutedEventArgs e)
        {
            NetSteps.Data.Entities.Processors.AutoshipProcessor processor = ProcessorProviders.AutoshipProcessor as NetSteps.Data.Entities.Processors.AutoshipProcessor;
            processor.ProgressMessage += new EventHandler<ProgressMessageEventArgs>(processor_ProgressMessage);
            processor.ProgressUpdated += new NetSteps.Common.Events.ProgressEventHandler(processor_ProgressUpdated);
            processor.MaxDegreeOfParallelism = 6;

            uxAutoshipProgressControl.ProcessTitle = "Processing autoships...";
            BackgroundAction action = new BackgroundAction(() =>
            {
                processor.StartDate = DateTime.Now;
                processor.ProcessConsultantsMonthlyReplenishmentAutoships();
            });
            action.Start();
        }

        private void processor_ProgressUpdated(object sender, NetSteps.Common.Events.ProgressEventArgs e)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                uxAutoshipProgressControl.UpdateProgress(e);
            }));
        }

        private void processor_ProgressMessage(object sender, ProgressMessageEventArgs e)
        {
            if (e.ApplicationMessageType == NetSteps.Common.Constants.ApplicationMessageType.Standard)
                (sender as NetSteps.Data.Entities.Processors.AutoshipProcessor).WriteToConsole(ConsoleColor.White, e.Message);
            else if (e.ApplicationMessageType == NetSteps.Common.Constants.ApplicationMessageType.Error)
                (sender as NetSteps.Data.Entities.Processors.AutoshipProcessor).WriteToConsole(ConsoleColor.Red, e.Message);
            else if (e.ApplicationMessageType == NetSteps.Common.Constants.ApplicationMessageType.Successful)
                (sender as NetSteps.Data.Entities.Processors.AutoshipProcessor).WriteToConsole(ConsoleColor.Green, e.Message);
        }
        #endregion
    }
}
