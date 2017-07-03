using WatiN.Core;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace NetSteps.Testing.Integration.DWS.Performance
{
    /// <summary>
    /// Class related to controls and ops of DWS My progress page.
    /// </summary>
    public class DWS_Performance_Dashboard_Page : DWS_Performance_Base_Page
    {
        #region Controls

        private Span spanMyProgress;
        private Link lnkYourDownline;        
        private Link lnkToggleClose;
        private Link lnkEditModules;        
        private SelectList _period;
        private DWS_Performance_SalesIndicator_Control _salesIndicator;
        private DWS_Performance_MonthlyGoal_Control _monthlyGoal;
        private Div divAccountInfoCardWindow;
        private Span spinnerSpan;

        protected override void InitializeContents()
        {
            base.InitializeContents();

            this.spanMyProgress = Document.GetElement<Span>(new Param("My Progress", AttributeName.ID.InnerText));
            this.lnkYourDownline = Document.GetElement<Link>(new Param("/Performance/FlatDownline", AttributeName.ID.Href));
            this.lnkToggleClose = Document.GetElement<Link>(new Param("FR cancel close", AttributeName.ID.ClassName));
            this.lnkEditModules = Document.GetElement<Link>(new Param("editModules"));
            _period = Document.GetElement<SelectList>(new Param("statusSelectFilter"));
            _salesIndicator = CreateSalesIndicator();
            _monthlyGoal = Control.CreateControl<DWS_Performance_MonthlyGoal_Control>(Document.GetElement<Div>(new Param("kpiMonthlyGoal", AttributeName.ID.ClassName, RegexOptions.None)));
            this.divAccountInfoCardWindow = Document.GetElement<Div>(new Param("treeViewNodeWrapper"));
            spinnerSpan = Document.GetElement<Span>(new Param("loadingWrap", AttributeName.ID.ClassName));
        }

        #endregion

        #region Properties

        public int Periods
        {
            get { return _period.Options.Count; }
        }

        public DWS_Performance_SalesIndicator_Control SalesIndicator
        {
            get { return _salesIndicator; }
        }

        public DateTime SelectedPeriod
        {
            get { return DateTime.Parse(_period.CustomGetSelectedItem().ToString()); }
        }

        #endregion Properties

        #region Methods

        private DWS_Performance_SalesIndicator_Control CreateSalesIndicator(int? timeout = null, int? delay = 2)
        {
            Document.GetElement<Div>(new Param("kpiSV")).CustomWaitForSpinner(timeout, delay);
            return Control.CreateControl<DWS_Performance_SalesIndicator_Control>(Document.GetElement<Div>(new Param("kpiSV")));
        }

        public DWS_Performance_Trip_Control TripIncentive()
        {
            return Control.CreateControl<DWS_Performance_Trip_Control>(Document.GetElement<Div>(new Param("TripProgress", AttributeName.ID.ClassName, RegexOptions.None)));
        }

        [Obsolete("Use 'PerformanceOverview<TControl>(int?, int?)'", true)]
        public ListItem_Control PerformanceOverview(int? timeout = null, int? delay = 2)
        {
            Document.GetElement<Div>(new Param("kpiNameValue", AttributeName.ID.ClassName, RegexOptions.None)).CustomWaitForSpinner(timeout, delay);
            return Control.CreateControl<ListItem_Control>(Document.GetElement<Div>(new Param("kpiNameValue", AttributeName.ID.ClassName, RegexOptions.None)));
        }

        public TControl PerformanceOverview<TControl>(int? timeout = null, int? delay = 2) where TControl : Control, new()
        {
            _content.CustomWaitForSpinners();
            Div parent = _content.GetElement<Div>(new Param("nameValueBody"));
            return parent.As<TControl>();
        }

        /// <summary>
        /// Return TitleProgress control as specified
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T TitleProgress<T>() where T : DWS_Performance_TitleProgress_Control, new()
        {
            return _content.GetElement<Div>(new Param("TitleProgress", AttributeName.ID.ClassName, RegexOptions.None)).As<T>();
        }

        public void SelectPeriod(int index, int? timeout = null, int? delay = 2)
        {
            timeout = _period.CustomSelectDropdownItem(index, timeout);
            timeout = spinnerSpan.CustomWaitForSpinner(timeout, delay);
            Thread.Sleep(2000);
            Util.Browser.CustomWaitForComplete(timeout);
        }

        /// <summary>
        /// Is page rendered.
        /// </summary>
        /// <returns>True if page rendered, else false.</returns>
        public override bool IsPageRendered()
        {
            return Document.GetElement<SelectList>(new Param("statusSelectFilter")).Exists;
        }

        /// <summary>
        ///  Is Info Card Window shown.
        /// </summary>
        /// <returns>True if shown, else false.</returns>
        public bool IsInfoCardWindowShown()
        {
            return this.divAccountInfoCardWindow.Exists;
        }

        #endregion
    }
}