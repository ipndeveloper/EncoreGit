using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ErrorLogViewer.Classes;
using NetSteps.Common;
using NetSteps.Common.Base;
using NetSteps.Common.DataAccess;
using NetSteps.Common.Extensions;
using NetSteps.Common.Threading;
using NetSteps.Data.Entities;
using NetSteps.Data.Entities.Business;
using NetSteps.Data.Entities.Cache;
using NetSteps.Wpf.Controls.Extensions;

namespace ErrorLogViewer
{
    /// <summary>
    /// Author: John Egbert
    /// Created: 7/22/2010
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Members
        private AppSettings _appSettings;
        private bool _isLoading = false;
        #endregion

        public MainWindow()
        {
            InitializeComponent();

            if (ApplicationContext.Instance.IsDebug)
                HibernatingRhinos.Profiler.Appender.EntityFramework.EntityFrameworkProfiler.Initialize();

            ApplicationContext.Instance.ApplicationID = NetSteps.Data.Entities.EntitiesHelpers.GetApplicationIdFromConnectionString();
            ApplicationContext.Instance.IsWebApp = false;
            ApplicationContext.Instance.CurrentUser = CorporateUser.LoadFull(1); // Defaulting user to Admin for now - JHE

            LoadConnectionString();

            uxStartDate.SelectedDate = DateTime.Now.AddDays(-1);
            uxEndDate.SelectedDate = DateTime.Now.EndOfDay();

            uxAppStatsStartDate.SelectedDate = DateTime.Now.AddDays(-7); // 6 Weeks
            uxAppStatsEndDate.SelectedDate = DateTime.Now.EndOfDay();
            uxBusyControl.Stop();

            //LoadErrors();
            //FillOverviewStats();
        }

        #region Methods
        private void LoadErrors()
        {
            _isLoading = true;
            uxBusyControl.Start();

            bool useLiveDb = uxUseLiveDatabase.IsChecked.ToBool();
            string customConnectionString = uxConnectionstring.Text;

            // metadata=res://*/EntityModels.Main.NetStepsDB.csdl|res://*/EntityModels.Main.NetStepsDB.ssdl|res://*/EntityModels.Main.NetStepsDB.msl;provider=System.Data.SqlClient;provider connection string=&quot;Data Source=10.130.0.50\qa_itworks;Initial Catalog=ItWorksCore;User ID=siteuser;Password=q3T0urn3y;MultipleActiveResultSets=True;Application Name=nsDistributor;Max Pool Size=200&quot;

            ErrorLogSearchParameters param = new ErrorLogSearchParameters()
            {
                StartDate = uxStartDate.SelectedDate,
                EndDate = uxEndDate.SelectedDate.EndOfDay(),
                Message = uxSearchData.Text.Trim(),
                PageSize = null,
                OrderBy = "LogDateUTC",
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Descending
            };

            string machineName = string.Empty;
            if (uxMachineName.SelectedItem is ComboBoxItem)
                machineName = (uxMachineName.SelectedItem as ComboBoxItem).Content.ToString();
            if (machineName != "All Machines")
                param.MachineName = machineName;
            else
                param.MachineName = null;

            string applicationName = string.Empty;
            if (uxApplication.SelectedItem is ComboBoxItem)
                applicationName = (uxApplication.SelectedItem as ComboBoxItem).Content.ToString();
            if (!string.IsNullOrEmpty(applicationName) && applicationName != "All Applications")
                param.ApplicationID = SmallCollectionCache.Instance.Applications.FirstOrDefault(a => a.Name == applicationName).ApplicationID;
            else
                param.ApplicationID = null;

            this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            BackgroundAction action = new BackgroundAction(() =>
            {
                PaginatedList<ErrorLog> errors = null;
                if (!customConnectionString.IsNullOrEmpty())
                    errors = ErrorLog.Search(param, customConnectionString);
                else if (useLiveDb)
                    errors = ErrorLog.Search(param, "name=NetStepsEntitiesLive");
                else
                    errors = ErrorLog.Search(param);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxRecentErrors.ItemsSource = errors;
                    if (errors.Count > 0)
                        uxRecentErrors.SelectedIndex = 0;
                    uxBusyControl.Stop();

                    if (string.IsNullOrEmpty(machineName) || machineName == "All Machines")
                        FillMachineNames();

                    if (string.IsNullOrEmpty(applicationName) || applicationName == "All Applications")
                        FilApplications();

                    FillGraph();

                    _isLoading = false;

                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                }));
            });
            action.Start();
        }

        private void FillMachineNames()
        {
            var errors = uxRecentErrors.ItemsSource as PaginatedList<ErrorLog>;
            var machineNames = errors.Select(e => e.MachineName.ToCleanString().ToUpper()).Distinct().Sort();

            uxMachineName.Items.Clear();
            uxMachineName.Items.Add(new ComboBoxItem() { Content = "All Machines" });
            foreach (var item in machineNames)
                uxMachineName.Items.Add(new ComboBoxItem() { Content = item });
            uxMachineName.SelectedIndex = 0;
        }

        private void FilApplications()
        {
            var errors = uxRecentErrors.ItemsSource as PaginatedList<ErrorLog>;

            List<string> applicationNames = new List<string>();
            foreach (var applicationID in errors.Select(e => e.ApplicationID).Distinct().Where(a => a > 0))
                applicationNames.Add(SmallCollectionCache.Instance.Applications.GetById(applicationID.ToShort()).Name);

            uxApplication.Items.Clear();
            uxApplication.Items.Add(new ComboBoxItem() { Content = "All Applications" });
            foreach (var item in applicationNames)
                uxApplication.Items.Add(new ComboBoxItem() { Content = item });
            uxApplication.SelectedIndex = 0;
        }

        private void FillGraph()
        {
            var errors = uxRecentErrors.ItemsSource as PaginatedList<ErrorLog>;

            uxGraphDaily.FillWithDailyStatsWithInDayRange(errors, s => s.LogDate, uxStartDate.SelectedDate.ToDateTime(), uxEndDate.SelectedDate.ToDateTime(), "Errors for selected day range.");
            uxGraphDailySmall.FillWithDailyStatsWithInDayRange(errors, s => s.LogDate, uxStartDate.SelectedDate.ToDateTime(), uxEndDate.SelectedDate.ToDateTime(), "");
        }

        private void FillOverviewStats(ApplicationUsageLogSearchParameters param)
        {
            var dailyErrors = GetDailyStats("ErrorLogs", "LogDateUTC", "ErrorLogID");
            var dailyOrders = GetDailyStats("Orders", "CommissionDateUTC", "OrderID");
            var dailyEnrollments = GetDailyStats("Accounts", "EnrollmentDateUTC", "AccountID");

            uxApplicationUsageStats.Title = "Overview stats for selected day range.";
            uxApplicationUsageStats.Series.Clear();
            uxApplicationUsageStats.AddLineSeriesDataToChart(dailyErrors, param.StartDate.ToDateTime(), param.EndDate.ToDateTime(), "Errors");
            uxApplicationUsageStats.AddLineSeriesDataToChart(dailyOrders, param.StartDate.ToDateTime(), param.EndDate.ToDateTime(), "Orders");
            uxApplicationUsageStats.AddLineSeriesDataToChart(dailyEnrollments, param.StartDate.ToDateTime(), param.EndDate.ToDateTime(), "Enrollments");
        }

        private void FillPieStats(ApplicationUsageLogSearchParameters param)
        {
            BackgroundAction action = new BackgroundAction(() =>
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var applicationPoolNameErrors = GetGroupedStats("ErrorLogs", "ApplicationPoolName", "LogDateUTC", param.StartDate.ToDateTime(), param.EndDate.ToDateTime());

                    uxAppPoolStats.Series.Clear();
                    uxAppPoolStats.AddPieSeriesDataToChart(applicationPoolNameErrors, param.StartDate.ToDateTime(), param.EndDate.ToDateTime(), "Application Pool");
                }));
            });
            action.Start();

            BackgroundAction action2 = new BackgroundAction(() =>
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var machineNameErrors = GetGroupedStats("ErrorLogs", "MachineName", "LogDateUTC", param.StartDate.ToDateTime(), param.EndDate.ToDateTime());

                    uxMachineNameStats.Series.Clear();
                    uxMachineNameStats.AddPieSeriesDataToChart(machineNameErrors, param.StartDate.ToDateTime(), param.EndDate.ToDateTime(), "Machine Name");
                }));
            });
            action2.Start();

            BackgroundAction action3 = new BackgroundAction(() =>
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    var applicationErrors = GetGroupedStats("ErrorLogs", "ApplicationId", "LogDateUTC", param.StartDate.ToDateTime(), param.EndDate.ToDateTime());

                    uxApplicationStats.Series.Clear();
                    uxApplicationStats.AddPieSeriesDataToChart(applicationErrors, param.StartDate.ToDateTime(), param.EndDate.ToDateTime(), "Application");
                }));
            });
            action3.Start();
        }

        // Helper method to get rows stats on a table - JHE
        private List<KeyValuePair<DateTime, int>> GetDailyStats(string tableName, string groupByDateColumnName, string primaryKeyColumnName)
        {
            var connectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();
            if (!uxConnectionstring.Text.IsNullOrEmpty())
                connectionString = uxConnectionstring.Text.GetEntityConnection().GetStandardConnectionString();

            string query = @"SELECT DATEADD(dd,0, DATEDIFF(dd,0,o.{ColumnName})) AS DATE, COUNT(o.{PrimaryKeyColumnName}) AS CountPerDay
                            FROM {TableName} o WITH (NOLOCK)
                            GROUP BY DATEADD(dd,0, DATEDIFF(dd,0,o.{ColumnName}))
                            ORDER BY DATE ASC";

            query = query.Replace("{TableName}", tableName);
            query = query.Replace("{ColumnName}", groupByDateColumnName);
            query = query.Replace("{PrimaryKeyColumnName}", primaryKeyColumnName);

            var ds = DataAccess.GetDataSet(query, string.Empty, connectionString);

            List<KeyValuePair<DateTime, int>> list = new List<KeyValuePair<DateTime, int>>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                DateTime date = DateTime.MinValue;
                if (row["DATE"].ToString().ToDateTimeNullable().HasValue)
                    date = row["DATE"].ToString().ToDateTimeNullable().Value;
                list.Add(new KeyValuePair<DateTime, int>(date, row["CountPerDay"].ToString().ToInt()));
            }
            return list;
        }

        private List<KeyValuePair<string, int>> GetGroupedStats(string tableName, string groupByDateColumnName, string dateColumnName, DateTime startDate, DateTime endDate)
        {
            var connectionString = EntitiesHelpers.GetAdoConnectionString<NetStepsEntities>();
            if (!uxConnectionstring.Text.IsNullOrEmpty())
                connectionString = uxConnectionstring.Text.GetEntityConnection().GetStandardConnectionString();

            string query = @"SELECT ISNULL(CONVERT(VARCHAR(50), {ColumnName}), 'Unknown') AS 'Name', COUNT(o.ErrorLogID) AS Total
                            FROM {TableName} o WITH (NOLOCK)
                            WHERE o.{DateColumnName} >= '{StartDate}' AND o.{DateColumnName} <= '{EndDate}'
                            GROUP BY {ColumnName}";

            query = query.Replace("{TableName}", tableName);
            query = query.Replace("{DateColumnName}", dateColumnName);
            query = query.Replace("{ColumnName}", groupByDateColumnName);
            query = query.Replace("{StartDate}", startDate.ToString("yyyy-MM-dd"));
            query = query.Replace("{EndDate}", endDate.ToString("yyyy-MM-dd"));
            //query = query.Replace("{PrimaryKeyColumnName}", primaryKeyColumnName);

            var ds = DataAccess.GetDataSet(query, string.Empty, connectionString, 120);

            List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string name = string.Empty;
                if (!row["Name"].ToString().IsNullOrEmpty())
                    name = row["Name"].ToString();
                list.Add(new KeyValuePair<string, int>(name, row["Total"].ToString().ToInt()));
            }
            return list;
        }


        private void LoadConnectionString()
        {
            try
            {
                string filePath = string.Format("{0}\\{1}", System.Windows.Forms.Application.CommonAppDataPath, "ErrorLogViewerSettings.dat");

                _appSettings = IO.FileDeSerialize<AppSettings>(filePath);

                BindSettings();
            }
            catch
            {
                _appSettings = new AppSettings();
            }
        }
        private void SaveConnectionString()
        {
            string filePath = string.Format("{0}\\{1}", System.Windows.Forms.Application.CommonAppDataPath, "ErrorLogViewerSettings.dat");
            IO.FileSerialize(_appSettings, filePath);
        }
        private void BindSettings()
        {
            uxConnectionstring.Text = _appSettings.ConnectionString;
        }
        #endregion

        #region Event Handlers
        private void uxRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadErrors();
        }

        private void uxRecentErrors_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var errorLog = (sender as ListBox).SelectedItem as ErrorLog;
            uxError.DataContext = errorLog;
        }

        private void uxMachineName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoading)
                LoadErrors();
        }

        private void uxApplication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_isLoading)
                LoadErrors();
        }

        private void uxRefreshApplicationUsageStats_Click(object sender, RoutedEventArgs e)
        {
            _isLoading = true;
            uxBusyControl.Start();

            bool useLiveDb = uxUseLiveDatabase.IsChecked.ToBool();

            ApplicationUsageLogSearchParameters param = new ApplicationUsageLogSearchParameters()
            {
                StartDate = uxAppStatsStartDate.SelectedDate,
                EndDate = uxAppStatsEndDate.SelectedDate.EndOfDay(),
                PageSize = null,
                OrderBy = "UsageDateUTC",
                OrderByDirection = NetSteps.Common.Constants.SortDirection.Descending
            };

            this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.Indeterminate;
            BackgroundAction action = new BackgroundAction(() =>
            {
                PaginatedList<ApplicationUsageLog> logs = null;
                if (useLiveDb)
                    logs = ApplicationUsageLog.Search(param, "name=NetStepsEntitiesLive");
                else
                    logs = ApplicationUsageLog.Search(param);
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    uxBusyControl.Stop();

                    //if (string.IsNullOrEmpty(machineName) || machineName == "All Machines")
                    //    FillMachineNames();

                    //if (string.IsNullOrEmpty(applicationName) || applicationName == "All Applications")
                    //    FilApplications();

                    FillPieStats(param);

                    _isLoading = false;

                    this.TaskbarItemInfo.ProgressState = System.Windows.Shell.TaskbarItemProgressState.None;
                }));
            });
            action.Start();

            BackgroundAction action2 = new BackgroundAction(() =>
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(() =>
                {
                    FillOverviewStats(param);
                }));
            });
            action2.Start();
        }

        private void uxUpdateConnectionString_Click(object sender, RoutedEventArgs e)
        {
            LoadErrors();
        }
        #endregion

        private void uxConnectionstring_TextChanged(object sender, TextChangedEventArgs e)
        {
            _appSettings.ConnectionString = uxConnectionstring.Text;
            SaveConnectionString();
        }
    }
}
