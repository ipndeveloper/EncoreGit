using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using NetSteps.Web.Extensions;
using System.Reflection;
using NetSteps.Common.Base;
using System.Globalization;

namespace NetSteps.Data.Entities.Business
{
    /// <summary>
    /// //Developed by Wesley Campos S. - CSTI
    /// </summary>
    public class FilterSearchData
    {
        public int IndexColumn { get; set; }
        public string Value { get; set; }
        public eFilterType type { get; set; }
        public bool IsRank { get; set; }
    }

    /// <summary>
    /// //Developed by Wesley Campos S. - CSTI
    /// </summary>
    public enum eFilterType
    {
        DateTime,
        Decimal,
        String,
        Int
    }

    /// <summary>
    /// //Developed by Wesley Campos S. - CSTI
    /// </summary>
    public static class FilterSearchDataMethods
    {
        public static string GetFilterExpression(this List<FilterSearchData> filters, DataSet ds)
        {
            string expression = string.Empty;
            foreach (FilterSearchData filter in filters)
            {
                if (!string.IsNullOrEmpty(filter.Value) && !filter.IsRank)
                {
                    switch (filter.type)
                    {
                        case eFilterType.String:
                            expression += string.Format("{0} LIKE '%{1}%' AND ", String.Concat("[", ds.Tables[0].Columns[filter.IndexColumn].ColumnName, "]"), filter.Value);
                            break;
                        case eFilterType.Int:
                            expression += string.Format("{0} = {1} AND ", String.Concat("[", ds.Tables[0].Columns[filter.IndexColumn].ColumnName, "]"), filter.Value);
                            break;
                        default:
                            break;
                    }
                }
            }

            List<FilterSearchData> Rankfilters = filters.FindAll(x => x.IsRank);
            for (int i = 0; i < Rankfilters.Count; i += 2)
            {
                switch (Rankfilters[i].type)
                {
                    case eFilterType.DateTime:
                        expression += GetDateFilter(String.Concat("[", ds.Tables[0].Columns[Rankfilters[i].IndexColumn].ColumnName, "]"), Rankfilters[i].Value, Rankfilters[i + 1].Value);
                        break;
                    case eFilterType.Decimal:
                        expression += GetDecimalFilter(String.Concat("[", ds.Tables[0].Columns[Rankfilters[i].IndexColumn].ColumnName, "]"), Rankfilters[i].Value, Rankfilters[i + 1].Value);
                        break;
                    default:
                        break;
                }
            }

            if (expression.EndsWith("AND ") && expression.Length > 0) expression += "1=1";
            return expression;
        }

        private static string GetDateFilter(string ColumnName,string IniDate,string FinDate)
        {
            string Result = string.Empty;
            if(!string.IsNullOrEmpty(IniDate) && !string.IsNullOrEmpty(FinDate))
                Result = string.Format("( {0} >= '{1}' and {2} < '{3}' ) AND ",
                                        ColumnName,
                                        IniDate,
                                        ColumnName,
                                        Convert.ToString(Convert.ToDateTime(FinDate).AddDays(1)));
            else if (!string.IsNullOrEmpty(IniDate) && string.IsNullOrEmpty(FinDate))
                Result = string.Format("( {0} >= '{1}' ) AND ",
                                        ColumnName,
                                        IniDate);
            else if (string.IsNullOrEmpty(IniDate) && !string.IsNullOrEmpty(FinDate))
                Result = string.Format("( {0} < '{1}' ) AND ",
                                        ColumnName,
                                        Convert.ToString(Convert.ToDateTime(FinDate).AddDays(1)));
            return Result;
        }

        private static string GetDecimalFilter(string ColumnName, string MinValue, string MaxValue)
        {
            string Result = string.Empty;

            if (!string.IsNullOrEmpty(MinValue) && !string.IsNullOrEmpty(MaxValue))
                Result = string.Format("( {0} >= '{1}' and {2} <= '{3}' ) AND ",ColumnName,MinValue,ColumnName,MaxValue);

            else if (!string.IsNullOrEmpty(MinValue) && string.IsNullOrEmpty(MaxValue))
                Result = string.Format("( {0} >= '{1}' ) AND ",ColumnName,MinValue);

            else if (string.IsNullOrEmpty(MinValue) && !string.IsNullOrEmpty(MaxValue))
                Result = string.Format("( {0} <= '{1}' ) AND ",ColumnName,MaxValue);
            return Result;
        }

        public static bool IsDecimal(this object value)
        {
            decimal number;
            if (value == null) value = string.Empty;
            if (Decimal.TryParse(value.ToString(), out number))
            {
                if (value.ToString().Contains(".")) return true;
                else return false;
            }
            else
                return false;
        }

        public static string BuildGridTable(this DataView dv,int Page,int PageSize)
        {
            StringBuilder builder = new StringBuilder();
            int Start;
            int Index = 1;
            Start = Page * PageSize + 1;
            PageSize = PageSize * (1 + Page);
            foreach (System.Data.DataRow row in dv.ToTable("Report").Rows)
            {
                if (Index >= Start && Index <= PageSize)
                {
                    builder.Append("<tr class=\"mainProduct\">");
                    object[] values = row.ItemArray;
                    //string rowString = string.Empty;
                    foreach (var item in values)
                    {
                        
                        if (item.IsDecimal()) builder.AppendCell(Convert.ToDecimal(item).ToString("C", new CultureInfo("en-US")));
                        //if (item.IsDecimal()) builder.AppendCell(String.Format("${0:0.00}", Convert.ToDecimal(item)));
                        else builder.AppendCell(Convert.ToString(item));
                    }
                    builder.Append("</tr>");
                }
                Index++;
            }
            return builder.ToString();
        }

        public static int TotalPages(this DataView dv, int pageSize)
        {
            int RowCount = dv.ToTable("TotalSalesReport").Rows.Count;
            int TotalPages = 0;
            if (RowCount % pageSize == 0) TotalPages = (RowCount / pageSize);
            else TotalPages = (RowCount / pageSize) + 1;
            return TotalPages;
        }

        public static string BuildGridTable<T>(this PaginatedList<T> ObjectList)
        {
            string Resul = string.Empty;
            StringBuilder builder = new StringBuilder();
            foreach (var item in ObjectList)
            {
                builder.Append("<tr class=\"mainProduct\">");
                foreach (PropertyInfo property in item.GetType().GetProperties())
                {
                    builder.AppendCell(Convert.ToString(item.GetType().GetProperty(property.Name).GetValue(item, null)));
                }
                builder.Append("</tr>");
            }
            return builder.ToString();
        }

        public static string DateFormat(this string date)
        {
            //12/13/2014
            string Result = string.Empty;
            if (string.IsNullOrEmpty(date)) return Result;
            else
            {
                string[] IniDate = date.Split(Convert.ToChar("/"));
                return string.Format("{0}/{1}/{2}", IniDate[1], IniDate[0], IniDate[2]);
            }
        }

        public static PaginatedList<T> ToPaginatedList<T>(this DataView dv) where T : new()
        {
            PaginatedList<T> Resul = new PaginatedList<T>();
            foreach (System.Data.DataRow row in dv.ToTable("Report").Rows)
            {
                var obj = new T();
                object[] Values = row.ItemArray;
                int index = 0;
                foreach (PropertyInfo property in obj.GetType().GetProperties())
                {
                    obj.GetType().GetProperty(property.Name).SetValue(obj, Convert.ToString(Values[index]), null);
                    index++;
                }
                Resul.Add(obj);
            }
            return Resul;
        }
    }

}
