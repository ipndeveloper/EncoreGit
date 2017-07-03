<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Models.AuditHistoryGridModel>" %>

<% Html.PaginatedGrid("~/Security/GetAuditHistory")
    .AddData("entityName", Model.EntityName)
    .AddData("entityId", Model.EntityId)
    .AddInputFilter(Html.Term("ColumnName", "Column Name"), "columnName", "")
    .AddInputFilter(Html.Term("TableName", "Table Name"), "tableName", "")
    .AddInputFilter(Html.Term("ID"), "pk", "")
    .AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate", new DateTime(1900, 1, 1).ToShortDateString(), true)
    .AddInputFilter(Html.Term("To", "To"), "endDate", DateTime.Now.ToShortDateString(), true, true)
    .AddData("loadedEntitySessionVarKey", Model.LoadedEntitySessionVarKey)
    
    .AddColumn(Html.Term("DateChanged", "Date Changed"), "DateChanged", true, true, NetSteps.Common.Constants.SortDirection.Descending)
    .AddColumn(Html.Term("ID"), "PK", true)
    .AddColumn(Html.Term("Table"), "TableName", true)
    .AddColumn(Html.Term("Column"), "ColumnName", true)
    .AddColumn(Html.Term("OldValue", "Old Value"), "OldValue", true)
    .AddColumn(Html.Term("NewValue", "New Value"), "NewValue", false)
    .AddColumn(Html.Term("Username", "Username"), "Username", true)
    .AddColumn(Html.Term("Application", "Application"), "ApplicationID", true)
    .Render(); %>