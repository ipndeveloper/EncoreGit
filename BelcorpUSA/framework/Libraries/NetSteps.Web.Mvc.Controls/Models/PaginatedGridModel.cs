using System.Collections;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NetSteps.Web.Mvc.Controls.Models
{
    public class PaginatedGridModel
    {
        public HtmlHelper Helper { get; set; }
        public string Id { get; set; }
        public string GetUrl { get; set; }
        public bool CanDelete { get; set; }
        public string DeleteUrl { get; set; }
        public bool CanDeactivate { get; set; }
        public bool CanActivate { get; set; }
        public string ActiveChangeUrl { get; set; }
        public bool HidePagination { get; set; }
        public bool EntireRowClickable { get; set; }
        public bool ExcludeScriptSection { get; set; }
        public bool IsShippable { get; set; }
        private Table _gridTable = new Table();
        public Table GridTable { get { return _gridTable; } set { _gridTable = value; } }
        /// <summary>
        /// Index of the link to click when an entire row is clicked (only used if EntireRowClickable = true)
        /// </summary>
        public int RowClickLinkIndex { get; set; }

        private List<Column> _columns = new List<Column>();
        public List<Column> Columns
        {
            get
            {
                return _columns;
            }
        }

        private List<Filter> _filters = new List<Filter>();
        public List<Filter> Filters
        {
            get
            {
                return _filters;
            }
        }

        private List<Filter> _advancedFilters = new List<Filter>();
        public List<Filter> AdvancedFilters
        {
            get
            {
                return _advancedFilters;
            }
        }

        private List<Option> _options = new List<Option>();
        public List<Option> Options
        {
            get
            {
                return _options;
            }
        }

        private Dictionary<string, object> _extraData = new Dictionary<string, object>();
        public Dictionary<string, object> ExtraData
        {
            get
            {
                return _extraData;
            }
        }

        private List<KeyValuePair<int?, string>> _pageSizeOptions = new List<KeyValuePair<int?, string>>()
			{
				new KeyValuePair<int?, string> (15, "15"),
				new KeyValuePair<int?, string> (20, "20"),
				new KeyValuePair<int?, string> (50, "50"),
				new KeyValuePair<int?, string> (100, "100"),
			};
        public List<KeyValuePair<int?, string>> PageSizeOptions
        {
            get
            {
                return _pageSizeOptions;
            }
        }

        public class Table
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string CssClasses { get; set; }
        }

        public class Column
        {
            public string Header { get; set; }
            public string PropertyName { get; set; }
            public bool IsSortable { get; set; }
            public bool IsDefaultSort { get; set; }
            public int ColSpan { get; set; }
            /*CS.11JUL2016.Inicio.Ancho de la columna en el GRID:width:[WidthColumn]px*/
            public int? WidthColumn { get; set; }
            /*CS.11JUL2016.Fin.Ancho de la columna en el GRID:width:[WidthColumn]px*/
            private NetSteps.Common.Constants.SortDirection? _defaultSortDirection;
            public NetSteps.Common.Constants.SortDirection? DefaultSortDirection
            {
                get
                {
                    if (IsDefaultSort && !_defaultSortDirection.HasValue)
                        _defaultSortDirection = NetSteps.Common.Constants.SortDirection.Ascending;
                    return _defaultSortDirection;
                }
                set
                {
                    _defaultSortDirection = value;
                }
            }
        }

        public class Filter
        {
            public string Label { get; set; }
            public string ParameterName { get; set; }
            public object StartingValue { get; set; }
            public IDictionary Values { get; set; }
            public bool HasValues { get { return Values != null && Values.Count > 0; } }
            public bool Multiselect { get; set; }
            public bool IsDateTime { get; set; }
            public bool AddBreak { get; set; }
            public bool IsHidden { get; set; }
            public string Type { get; set; }
            public bool AutoPostBack { get; set; }
        }

        public class Option
        {
            public string Text { get; set; }
            public string Id { get; set; }
        }
    }

    public class PaginatedGridModel<T> : PaginatedGridModel
    {
    }
}
