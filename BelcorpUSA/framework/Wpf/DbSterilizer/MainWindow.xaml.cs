using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using NetSteps.Common;
using NetSteps.Common.DataFaker;
using NetSteps.Common.Events;
using NetSteps.Common.Extensions;
using NetSteps.Common.Reflection;
using NetSteps.Common.Threading;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business.Interfaces;
using NetSteps.Data.Entities.Cache;
using NetSteps.Data.Entities.Exceptions;

namespace DbSterilizer
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 9/7/2010
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        protected readonly object _lock = new object();

        protected CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        protected ParallelOptions parallelOptions = new ParallelOptions();
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            if (ApplicationContext.Instance.IsDebug)
                HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            ApplicationContext.Instance.ApplicationID = NetSteps.Data.Entities.EntitiesHelpers.GetApplicationIdFromConnectionString();
            ApplicationContext.Instance.IsWebApp = false;
            ApplicationContext.Instance.CurrentUser = CorporateUser.LoadFull(1); // Defaulting user to Admin for now - JHE

            uxItemProgressControl.CancelProcess += new EventHandler(uxItemProgressControl_CancelProcess);
        }

        #region Methods
        private void CleanAccounts()
        {
            try
            {
                var allAccounts = Account.Search(new NetSteps.Data.Entities.Business.AccountSearchParameters() { PageSize = 1000000 });

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxItemProgressControl.ProcessTitle = "Cleaning Account Data...";
                }));

                int total = allAccounts.Count();
                int itemsProcessed = 0;
                DateTime startDate = DateTime.Now;

                cancellationTokenSource = new CancellationTokenSource();
                parallelOptions.CancellationToken = cancellationTokenSource.Token;
                Parallel.ForEach(allAccounts, parallelOptions, account =>
                {
                    string currentItemDescription = string.Format("{0} {1} ({2})", account.FirstName, account.LastName, account.AccountID);
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, currentItemDescription);
                    try
                    {
                        UpdateProgress(progressEventArgs);

                        CleanAccount(account.AccountID);

                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException ex)
                    {
                        progressEventArgs = new ProgressEventArgs(null, 0, 0, "Progress Canceled");
                        UpdateProgress(progressEventArgs);
                    }
                    catch (Exception ex)
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    }
                    finally
                    {
                        lock (_lock)
                        {
                            itemsProcessed++;
                        }
                    }
                });

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, "Completed");
                    UpdateProgress(progressEventArgs);
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    uxCleanAccount.Content = uxCleanAccount.Content + " (Complete)";
                }));
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
        private void CleanAccount(int accountID)
        {
            // TODO: Fix!; Loading multiple references to same object on Account.Address and AccountPaymentMethods.Address - JHE
            Account account = Account.Load(accountID);

            if (account.NickName == "(Generated)")
                return;

            account = Account.LoadFull(accountID);

            var name = NameFaker.NameObject();
            var mainAddress = FakeObjects.GetFakeIAddress(name);

            account.FirstName = name.FirstName;
            account.LastName = name.LastName;
            account.Birthday = NetSteps.Common.Random.GetBoolean() ? DateTimeFaker.BirthDay() : (DateTime?)null;
            account.GenderID = NetSteps.Common.Random.GetBoolean() ? (name.IsMale) ? NetSteps.Data.Entities.Constants.Gender.Male.ToShort() : NetSteps.Data.Entities.Constants.Gender.Female.ToShort() : (short?)null;

            account.IsTaxExempt = NetSteps.Common.Random.GetBoolean();
            if (account.IsTaxExempt.ToBool())
                account.DecryptedTaxNumber = MiscFaker.SocialSecurityNumber();

            account.IsEntity = NetSteps.Common.Random.GetBoolean();
            if (account.IsEntity)
                account.EntityName = MiscFaker.GetBusinessName();
            account.CoApplicant = null;

            account.EmailAddress = InternetFaker.Email(name);
            account.ReceivedApplication = NetSteps.Common.Random.GetBoolean();
            account.EnrollmentDate = NetSteps.Common.Random.GetDateTime(DateTime.Now.AddYears(-10), DateTime.Now);
            account.NickName = "(Generated)";

            if (account.User != null)
            {
                account.User.Username = InternetFaker.UserName(name);
                account.User.UserTypeID = NetSteps.Data.Entities.Constants.UserType.Distributor.ToShort();
                //account.User.PasswordOLD = InternetFaker.PasswordWeak();
                //account.User.Password = account.User.PasswordOLD;
                account.User.PasswordQuestion = null;
                account.User.PasswordAnswer = null;
                account.User.LoginMessage = null;
            }

            foreach (var accountPhone in account.AccountPhones)
                accountPhone.PhoneNumber = PhoneFaker.Phone();

            foreach (var accountPaymentMethod in account.AccountPaymentMethods)
            {
                var fakeAccountPaymentMethod = FakeObjects.GetFakeAccountPaymentMethod(name);

                int paymentTypeID = accountPaymentMethod.PaymentTypeID;
                Reflection.CopyPropertiesDynamic<IPayment, IPayment>(fakeAccountPaymentMethod, accountPaymentMethod);

                if (accountPaymentMethod.BillingAddress == null)
                {
                    accountPaymentMethod.BillingAddress = new Address();
                    accountPaymentMethod.BillingAddress.StartEntityTracking();

                    Reflection.CopyPropertiesDynamic<IAddress, IAddress>(fakeAccountPaymentMethod, accountPaymentMethod);
                    (accountPaymentMethod as IAddress).AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                    accountPaymentMethod.BillingAddress.AddressTypeID = NetSteps.Data.Entities.Constants.AddressType.Billing.ToShort();
                }
                else
                    Reflection.CopyPropertiesDynamic<IAddress, IAddress>(mainAddress, accountPaymentMethod);

                accountPaymentMethod.PaymentTypeID = paymentTypeID;
                accountPaymentMethod.NameOnCard = name.FirstAndLastName;
            }

            foreach (var address in account.Addresses)
            {
                bool isDefault = address.IsDefault;
                short addressTypeID = address.AddressTypeID;
                if (address.AddressTypeID == NetSteps.Data.Entities.Constants.AddressType.Main.ToShort())
                    Reflection.CopyPropertiesDynamic<IAddress, IAddress>(mainAddress, address);
                else
                    Reflection.CopyPropertiesDynamic<IAddress, IAddress>(FakeObjects.GetFakeIAddress(name), address);

                string addressTypeName = SmallCollectionCache.Instance.AddressTypes.GetById(address.AddressTypeID).Name;
                address.AddressTypeID = addressTypeID;
                address.ProfileName = isDefault ? "(Default " + addressTypeName + ")" : addressTypeName;
            }

            account.Save();
        }

        private void CleanOrders()
        {
            try
            {
                var allOrders = Order.Search(new NetSteps.Data.Entities.Business.OrderSearchParameters() { PageSize = 1000000 });
                //var allOrders = Order.Search(new NetSteps.Data.Entities.Business.OrderSearchParameters() { OrderNumber = "407" });

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxItemProgressControl.ProcessTitle = "Cleaning Orders Data...";
                }));

                int total = allOrders.Count();
                int itemsProcessed = 0;
                DateTime startDate = DateTime.Now;

                cancellationTokenSource = new CancellationTokenSource();
                parallelOptions.CancellationToken = cancellationTokenSource.Token;
                Parallel.ForEach(allOrders, parallelOptions, item =>
                {
                    string currentItemDescription = string.Format("{0} {1} ({2})", "", "", item.OrderID);
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, currentItemDescription);
                    try
                    {
                        var order = Order.LoadFull(item.OrderID);

                        UpdateProgress(progressEventArgs);

                        CleanOrder(order);

                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException ex)
                    {
                        progressEventArgs = new ProgressEventArgs(null, 0, 0, "Progress Canceled");
                        UpdateProgress(progressEventArgs);
                    }
                    catch (Exception ex)
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    }
                    finally
                    {
                        lock (_lock)
                        {
                            itemsProcessed++;
                        }
                    }
                });

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, "Completed");
                    UpdateProgress(progressEventArgs);
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    uxCleanOrders.Content = uxCleanOrders.Content + " (Complete)";
                }));
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
        private void CleanOrder(Order order)
        {
            var name = NameFaker.NameObject();
            var address = FakeObjects.GetFakeIAddress(name);

            if (order.OrderCustomers != null)
            {
                foreach (var orderCustomer in order.OrderCustomers)
                {
                    if (orderCustomer.OrderPayments != null)
                    {
                        foreach (var orderPayment in orderCustomer.OrderPayments)
                        {
                            orderPayment.NameOnCard = name.FirstAndLastName;
                            orderPayment.DecryptedAccountNumber = MiscFaker.GetCreditCard();
                            orderPayment.ExpirationDate = MiscFaker.GetExpirationDate();
                            Reflection.CopyPropertiesDynamic<IAddress, IAddress>(address, orderPayment);

                            foreach (var orderPaymentResult in orderPayment.OrderPaymentResults)
                            {
                                orderPaymentResult.DecryptedAccountNumber = orderPayment.DecryptedAccountNumber;
                                orderPaymentResult.ExpirationDate = orderPayment.ExpirationDate;
                            }
                        }
                    }
                }
            }

            if (order.OrderShipments != null)
            {
                foreach (var orderShipment in order.OrderShipments)
                {
                    Reflection.CopyPropertiesDynamic<IAddress, IAddress>(address, orderShipment);
                }
            }

            order.Save();
        }

        private void CleanCorporateUsers()
        {
            try
            {
                var allUsers = CorporateUser.LoadAll();

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxItemProgressControl.ProcessTitle = "Cleaning CorporateUsers Data...";
                }));

                int total = allUsers.Count();
                int itemsProcessed = 0;
                DateTime startDate = DateTime.Now;

                cancellationTokenSource = new CancellationTokenSource();
                parallelOptions.CancellationToken = cancellationTokenSource.Token;
                Parallel.ForEach(allUsers, parallelOptions, item =>
                {
                    string currentItemDescription = string.Format("{0} {1} ({2})", item.FirstName, item.LastName, item.CorporateUserID);
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, currentItemDescription);
                    try
                    {
                        var corporateUser = CorporateUser.LoadFull(item.CorporateUserID);

                        UpdateProgress(progressEventArgs);
                        CleanCorporateUser(corporateUser);

                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException ex)
                    {
                        progressEventArgs = new ProgressEventArgs(null, 0, 0, "Progress Canceled");
                        UpdateProgress(progressEventArgs);
                    }
                    catch (Exception ex)
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    }
                    finally
                    {
                        lock (_lock)
                        {
                            itemsProcessed++;
                        }
                    }
                });

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, "Completed");
                    UpdateProgress(progressEventArgs);
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    uxCleanCorporateUsers.Content = uxCleanCorporateUsers.Content + " (Complete)";
                }));
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
        private void CleanCorporateUser(CorporateUser corporateUser)
        {
            if (corporateUser.FirstName == "Admin")
                return;

            var name = NameFaker.NameObject();

            corporateUser.FirstName = name.FirstName;
            corporateUser.LastName = name.LastName;
            corporateUser.Email = InternetFaker.Email(name);
            corporateUser.PhoneNumber = PhoneFaker.Phone();

            if (corporateUser.User != null)
            {
                corporateUser.User.Username = InternetFaker.UserName(name);
                //corporateUser.User.PasswordOLD = InternetFaker.PasswordWeak();
                //corporateUser.User.Password = corporateUser.User.PasswordOLD;
                corporateUser.User.PasswordQuestion = null;
                corporateUser.User.PasswordAnswer = null;
                corporateUser.User.LoginMessage = null;
            }

            corporateUser.Save();
        }


        private void CleanOutOldTaxCache()
        {
            try
            {
                DateTime startDate = DateTime.Now;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxItemProgressControl.ProcessTitle = string.Format("Clean Out Old TaxCache Data (count: {0}) ...", TaxCache.GetCount());

                    var progressEventArgs = new ProgressEventArgs(startDate, 1, 0, "Cleaning.... (this is a long running process)");
                    UpdateProgress(progressEventArgs);
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
                }));

                TaxCache.CleanOutOldTaxData();

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var progressEventArgs = new ProgressEventArgs(startDate, 1, 1, "Completed");
                    UpdateProgress(progressEventArgs);
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                }));
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

        // TODO: Clean Products, Catalogs - JHE

        private void GenerateAccounts(int total)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxItemProgressControl.ProcessTitle = "Generating Account Data...";
                }));

                int itemsProcessed = 0;
                DateTime startDate = DateTime.Now;

                cancellationTokenSource = new CancellationTokenSource();
                parallelOptions.CancellationToken = cancellationTokenSource.Token;
                Parallel.For(0, total, parallelOptions, i =>
                {
                    string currentItemDescription = string.Empty;
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, currentItemDescription);
                    try
                    {
                        UpdateProgress(progressEventArgs);

                        var account = FakeObjects.GetFakeAccount();
                        account.Save();

                        currentItemDescription = string.Format("{0} {1} ({2})", account.FirstName, account.LastName, account.AccountID);
                        progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, currentItemDescription);

                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException ex)
                    {
                        progressEventArgs = new ProgressEventArgs(null, 0, 0, "Progress Canceled");
                        UpdateProgress(progressEventArgs);
                    }
                    catch (Exception ex)
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    }
                    finally
                    {
                        lock (_lock)
                        {
                            itemsProcessed++;
                        }
                    }
                });

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, "Completed");
                    UpdateProgress(progressEventArgs);
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    uxGenerateAccounts.Content = uxGenerateAccounts.Content + " (Complete)";
                }));
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

        private void GenerateOrders(int total)
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxItemProgressControl.ProcessTitle = "Generating Order Data...";
                }));

                int itemsProcessed = 0;
                DateTime startDate = DateTime.Now;

                cancellationTokenSource = new CancellationTokenSource();
                parallelOptions.CancellationToken = cancellationTokenSource.Token;
                Parallel.For(0, total, parallelOptions, i =>
                {
                    string currentItemDescription = string.Empty;
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, currentItemDescription);
                    try
                    {
                        UpdateProgress(progressEventArgs);

                        var order = FakeObjects.GetFakeOrder();
                        order.Save();

                        OrderCustomer orderCustomer = order.OrderCustomers[0];
                        currentItemDescription = string.Format("{0} {1} ({2})", orderCustomer.AccountInfo.FirstName, orderCustomer.AccountInfo.LastName, orderCustomer.AccountID);
                        progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, currentItemDescription);

                        parallelOptions.CancellationToken.ThrowIfCancellationRequested();
                    }
                    catch (OperationCanceledException ex)
                    {
                        progressEventArgs = new ProgressEventArgs(null, 0, 0, "Progress Canceled");
                        UpdateProgress(progressEventArgs);
                    }
                    catch (Exception ex)
                    {
                        var exception = EntityExceptionHelper.GetAndLogNetStepsException(ex, NetSteps.Data.Entities.Constants.NetStepsExceptionType.NetStepsApplicationException);
                    }
                    finally
                    {
                        lock (_lock)
                        {
                            itemsProcessed++;
                        }
                    }
                });

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var progressEventArgs = new ProgressEventArgs(startDate, total, itemsProcessed, "Completed");
                    UpdateProgress(progressEventArgs);
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    uxGenerateOrders.Content = uxGenerateOrders.Content + " (Complete)";
                }));
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

        private void GenerateAdminUser()
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxItemProgressControl.ProcessTitle = "Generating Order Data...";
                }));

                //CorporateUser corporateUser = CorporateUser.Load(

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                    uxGenerateAdminUser.Content = uxGenerateAdminUser.Content + " (Complete)";
                }));
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
        #endregion

        #region Helper Methods
        private void UpdateProgress(ProgressEventArgs progressEventArgs)
        {
            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
            {
                uxItemProgressControl.UpdateProgress(progressEventArgs);

                this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Normal;
                this.TaskbarItemInfo.ProgressValue = progressEventArgs.PercentComplete / 100;
            }));
        }
        #endregion

        #region Event Handlers
        private void uxBegin_Click(object sender, RoutedEventArgs e)
        {
            bool cleanCorporateUsers = uxCleanCorporateUsers.IsChecked.ToBool();
            bool cleanAccount = uxCleanAccount.IsChecked.ToBool();
            bool cleanOrders = uxCleanOrders.IsChecked.ToBool();
            bool cleanOldTaxCache = uxOldTaxCache.IsChecked.ToBool();

            uxCleanCorporateUsers.Content = uxCleanCorporateUsers.Content.ToString().Replace("(Complete)", string.Empty).Trim();
            uxCleanAccount.Content = uxCleanAccount.Content.ToString().Replace("(Complete)", string.Empty).Trim();
            uxCleanOrders.Content = uxCleanOrders.Content.ToString().Replace("(Complete)", string.Empty).Trim();

            parallelOptions.MaxDegreeOfParallelism = uxThreads.Value.ToInt();
            BackgroundAction action = new BackgroundAction(() =>
            {
                if (cleanCorporateUsers)
                    CleanCorporateUsers();
                if (cleanAccount)
                    CleanAccounts();
                if (cleanOrders)
                    CleanOrders();
                if (cleanOldTaxCache)
                    CleanOutOldTaxCache();
            });
            action.Start();
        }

        private void uxItemProgressControl_CancelProcess(object sender, EventArgs e)
        {
            cancellationTokenSource.Cancel();
        }

        private void uxThreads_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            uxTreadsLabel.Text = string.Format("Threads ({0})", uxThreads.Value.ToInt());
            parallelOptions.MaxDegreeOfParallelism = uxThreads.Value.ToInt();
        }

        private void uxStartGenerate_Click(object sender, RoutedEventArgs e)
        {
            bool generateAdminUser = uxGenerateAdminUser.IsChecked.ToBool();
            bool generateAccounts = uxGenerateAccounts.IsChecked.ToBool();
            bool generateOrders = uxGenerateOrders.IsChecked.ToBool();
            int accountTotal = uxAccountTotal.Text.ToInt();
            int ordersTotal = uxOrdersTotal.Text.ToInt();

            uxGenerateAdminUser.Content = uxGenerateAdminUser.Content.ToString().Replace("(Complete)", string.Empty).Trim();
            uxGenerateAccounts.Content = uxGenerateAccounts.Content.ToString().Replace("(Complete)", string.Empty).Trim();
            uxGenerateOrders.Content = uxGenerateOrders.Content.ToString().Replace("(Complete)", string.Empty).Trim();

            parallelOptions.MaxDegreeOfParallelism = uxThreads.Value.ToInt();
            BackgroundAction action = new BackgroundAction(() =>
            {
                if (generateAdminUser)
                    GenerateAdminUser();
                if (generateAccounts)
                    GenerateAccounts(accountTotal);
                if (generateOrders)
                    GenerateOrders(ordersTotal);
            });
            action.Start();

        }
        #endregion
    }
}
