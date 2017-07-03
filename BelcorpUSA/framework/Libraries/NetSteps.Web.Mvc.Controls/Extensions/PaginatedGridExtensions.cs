using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using NetSteps.Common;
using NetSteps.Common.Attributes;
using NetSteps.Common.Extensions;
using NetSteps.Common.Globalization;
using NetSteps.Web.Mvc.Controls.Models;

namespace NetSteps.Web.Mvc.Controls
{
    public static class PaginatedGridExtensions
    {
        #region Paginated Grid
        public static PaginatedGridModel<T> PaginatedGrid<T>(this HtmlHelper helper, string getUrl, string id = null)
        {
            return new PaginatedGridModel<T>()
            {
                Helper = helper,
                GetUrl = getUrl.ResolveUrl(),
                Id = id
            };
        }

        public static PaginatedGridModel PaginatedGrid(this HtmlHelper helper, string getUrl, string id = null)
        {
            return new PaginatedGridModel()
            {
                Helper = helper,
                GetUrl = getUrl.ResolveUrl(),
                Id = id
            };
        }

        public static PaginatedGridModel PaginatedGrid(this HtmlHelper helper, string getUrl, List<KeyValuePair<int?, string>> pageSizeOptions, string id = null)
        {
            PaginatedGridModel gridModel = new PaginatedGridModel
                                               {
                                                   Helper = helper,
                                                   GetUrl = getUrl.ResolveUrl(),
                                                   Id = id
                                               };
            gridModel.PageSizeOptions.AddRange(pageSizeOptions);

            return gridModel;
        }

        public static PaginatedGridModel ClickEntireRow(this PaginatedGridModel grid, int linkIndex = 0)
        {
            grid.EntireRowClickable = true;
            grid.RowClickLinkIndex = linkIndex;
            return grid;
        }

        public static PaginatedGridModel AddData(this PaginatedGridModel grid, string parameterName, object value)
        {
            grid.ExtraData.Add(parameterName, value);
            return grid;
        }

        public static PaginatedGridModel ClearPageSizeOptions(this PaginatedGridModel grid)
        {
            grid.PageSizeOptions.Clear();
            return grid;
        }

        public static PaginatedGridModel AddPageSizeOption(this PaginatedGridModel grid, int? pageSize, string text)
        {
            try
            {
                grid.PageSizeOptions.Add(new KeyValuePair<int?, string>(pageSize, text));
            }
            catch (ArgumentException)
            {
                //Swallow a duplicate key exception because we don't really care about that here
            }
            return grid;
        }

        public static PaginatedGridModel CanDelete(this PaginatedGridModel grid, string deleteUrl)
        {
            grid.CanDelete = true;
            grid.DeleteUrl = deleteUrl.ResolveUrl();
            return grid;
        }

        public static PaginatedGridModel CanChangeStatus(this PaginatedGridModel grid, bool canDeactivate, bool canActivate, string activeChangeUrl)
        {
            grid.CanDeactivate = canDeactivate;
            grid.CanActivate = canActivate;
            grid.ActiveChangeUrl = activeChangeUrl.ResolveUrl();
            return grid;
        }

        public static PaginatedGridModel CanBeShipped(this PaginatedGridModel grid, bool isShippable, string activeChangeUrl)
        {
            grid.IsShippable = true;
            grid.ActiveChangeUrl = activeChangeUrl.ResolveUrl();
            return grid;
        }

        public static PaginatedGridModel AddOption(this PaginatedGridModel grid, string id, string label)
        {
            grid.Options.Add(new PaginatedGridModel.Option()
            {
                Id = id,
                Text = label
            });
            return grid;
        }

        public static PaginatedGridModel ExcludeScriptSection(this PaginatedGridModel grid, bool excludeScriptSection = true)
        {
            grid.ExcludeScriptSection = excludeScriptSection;
            return grid;
        }

        public static PaginatedGridModel<T> AutoGenerateColumns<T>(this PaginatedGridModel<T> grid)
        {
            return AutoGenerateColumns(grid, typeof(T)) as PaginatedGridModel<T>;
        }

        public static PaginatedGridModel AutoGenerateAllColumns(this PaginatedGridModel grid, Type type)
        {
            return AutoGenerateColumns(grid, type);
        }

        private static PaginatedGridModel AutoGenerateColumns(PaginatedGridModel grid, Type type)
        {
            var properties = from p in type.GetProperties()
                             let o = p.IsDefined(typeof(DisplayAttribute), false) ? ((DisplayAttribute)p.GetCustomAttributes(typeof(DisplayAttribute), false).First()).GetOrder() ?? 0 : 0
                             orderby o
                             select p;

            foreach (PropertyInfo property in properties)
            {
                string header, propertyName;
                bool generateColumn = true, sortable = true, defaultSort = false;
                Constants.SortDirection? defaultSortDirection = null;
                if (property.IsDefined(typeof(DisplayAttribute), false))
                {
                    DisplayAttribute display = property.GetCustomAttributes(typeof(DisplayAttribute), false).First() as DisplayAttribute;
                    if (property.IsDefined(typeof(TermNameAttribute), false))
                    {
                        TermNameAttribute termName = property.GetCustomAttributes(typeof(TermNameAttribute), false).First() as TermNameAttribute;
                        header = string.IsNullOrEmpty(termName.TermName) ? property.Name.PascalToSpaced() : Translation.GetTerm(termName.TermName, termName.DefaultValue);
                    }
                    else
                        header = string.IsNullOrEmpty(display.Name) ? property.Name.PascalToSpaced() : Translation.GetTerm(display.Name, display.Name); 
                    if (display.GetAutoGenerateField().HasValue)
                        generateColumn = display.AutoGenerateField;
                }
                else
                {
                    if (property.IsDefined(typeof(TermNameAttribute), false))
                    {
                        TermNameAttribute termName = property.GetCustomAttributes(typeof(TermNameAttribute), false).First() as TermNameAttribute;
                        header = string.IsNullOrEmpty(termName.TermName) ? property.Name.PascalToSpaced() : Translation.GetTerm(termName.TermName, termName.DefaultValue);
                    }
                    else
                        header = Translation.GetTerm(property.Name.PascalToSpaced());
                }
                if (property.IsDefined(typeof(PropertyNameAttribute), false))
                {
                    PropertyNameAttribute propName = property.GetCustomAttributes(typeof(PropertyNameAttribute), false).First() as PropertyNameAttribute;
                    propertyName = propName.PropertyName;
                }
                else
                {
                    propertyName = property.Name;
                }
                if (property.IsDefined(typeof(SortableAttribute), false))
                    sortable = (property.GetCustomAttributes(typeof(SortableAttribute), false).First() as SortableAttribute).Sortable;
                if (property.IsDefined(typeof(DefaultSortAttribute), false))
                {
                    defaultSort = true;
                    defaultSortDirection = (property.GetCustomAttributes(typeof(DefaultSortAttribute), false).First() as DefaultSortAttribute).SortDirection;
                }
                if (generateColumn)
                {
                    string columnName = propertyName;
                    if (property.PropertyType.GetNonNullableType() == typeof(DateTime) && !propertyName.EndsWith("UTC"))
                        columnName = propertyName + "UTC";
                    grid.AddColumn(header, columnName, sortable, defaultSort, defaultSortDirection);
                }
            }
            return grid;
        }

        public static PaginatedGridModel AddColumn(this PaginatedGridModel grid, string columnHeader, string propertyName, bool isSortable = true, bool isDefaultSort = false, NetSteps.Common.Constants.SortDirection? defaultSortDirection = null, int colSpan = 1, int widthColumn = 0)
        {
            grid.Columns.Add(new PaginatedGridModel.Column()
            {
                Header = columnHeader,
                PropertyName = propertyName,
                IsSortable = isSortable,
                IsDefaultSort = isDefaultSort,
                DefaultSortDirection = defaultSortDirection,
                ColSpan = colSpan,
                WidthColumn = widthColumn
            });
            return grid;
        }

        public static PaginatedGridModel DefineTable(this PaginatedGridModel grid, string cssClasses = null)
        {
            grid.GridTable = new PaginatedGridModel.Table
            {
                CssClasses = cssClasses
            };
            return grid;
        }

        public static PaginatedGridModel AddInputFilter(this PaginatedGridModel grid, string label, string parameterName, object startingValue = null, bool isDateTime = false, bool addBreak = false, bool isHidden = false, string type = "")
        {
            grid.Filters.Add(new PaginatedGridModel.Filter()
            {
                Label = label,
                ParameterName = parameterName,
                StartingValue = startingValue,
                IsDateTime = isDateTime,
                AddBreak = addBreak,
                IsHidden = isHidden,
                Type = type
            });
            return grid;
        }

        public static PaginatedGridModel AddSelectFilter(this PaginatedGridModel grid, string label, string parameterName, IDictionary values, object startingValue = null, bool addBreak = false, bool multiselect = false, bool autoPostBack = true)
        {
            grid.Filters.Add(new PaginatedGridModel.Filter()
            {
                Label = label,
                ParameterName = parameterName,
                StartingValue = startingValue,
                Values = values,
                AddBreak = addBreak,
                Multiselect = multiselect,
                AutoPostBack = autoPostBack
            });
            return grid;
        }

        public static PaginatedGridModel AddAdvancedInputFilter(this PaginatedGridModel grid, string label, string parameterName, object startingValue = null, bool isDateTime = false, bool addBreak = false)
        {
            grid.AdvancedFilters.Add(new PaginatedGridModel.Filter()
            {
                Label = label,
                ParameterName = parameterName,
                StartingValue = startingValue,
                IsDateTime = isDateTime,
                AddBreak = addBreak
            });
            return grid;
        }

        public static PaginatedGridModel AddAdvancedSelectFilter(this PaginatedGridModel grid, string label, string parameterName, IDictionary values, object startingValue = null, bool addBreak = false, bool multiselect = false, bool autoPostBack = true)
        {
            grid.AdvancedFilters.Add(new PaginatedGridModel.Filter()
            {
                Label = label,
                ParameterName = parameterName,
                StartingValue = startingValue,
                Values = values,
                Multiselect = multiselect,
                AddBreak = addBreak,
                AutoPostBack = autoPostBack
            });
            return grid;
        }

        public static PaginatedGridModel AddAdvancedCheckboxFilter(this PaginatedGridModel grid, string label, string parameterName, bool startingValue = false, bool addBreak = false)
        {
            grid.AdvancedFilters.Add(new PaginatedGridModel.Filter()
            {
                Label = label,
                ParameterName = parameterName,
                StartingValue = startingValue,
                AddBreak = addBreak,
                Type = "checkbox"
            });
            return grid;
        }

        public static void Render(this PaginatedGridModel grid)
        {
            grid.Helper.RenderPartial("PaginatedGrid", grid);
        }


        public static bool IsSelectedEntry(this PaginatedGridModel grid, PaginatedGridModel.Filter filter, DictionaryEntry entry)
        {
            var startingValue = filter.StartingValue;
            if (startingValue is IEnumerable<object>)
            {
                return (startingValue as IEnumerable<object>).Contains(entry.Key);
            }
            else if (startingValue is IEnumerable<int>)
            {
                return (startingValue as IEnumerable<int>).Contains(Convert.ToInt32(entry.Key));
            }
            else
            {
                return entry.Key.Equals(startingValue);
            }
        }

        public static PaginatedGridModel SetDefaultSort(this PaginatedGridModel grid, string propertyName, NetSteps.Common.Constants.SortDirection sortDirection)
        {
            var defaultColumn = grid.Columns.FirstOrDefault(c => c.IsDefaultSort);
            var newDefaultColumn = grid.Columns.FirstOrDefault(c => c.PropertyName == propertyName);
            if (newDefaultColumn != null)
            {
                if (defaultColumn != null)
                    defaultColumn.IsDefaultSort = false;
                newDefaultColumn.IsDefaultSort = true;
                newDefaultColumn.DefaultSortDirection = sortDirection;
            }
            return grid;
        }

        public static PaginatedGridModel HidePagination(this PaginatedGridModel grid)
        {
            grid.HidePagination = true;

            return grid;
        }

        public static PaginatedGridModel HideClientSpecificColumns(this PaginatedGridModel grid)
        {
            grid.Columns.RemoveWhere(c => c.PropertyName == "CoApplicant");
            return grid;
        }

        public static PaginatedGridModel HideClientSpecificColumns_(this PaginatedGridModel grid)
        {
            grid.Columns.RemoveWhere(c => c.Header == "CoApplicant");
            return grid;
        }
        #endregion
    }
}
