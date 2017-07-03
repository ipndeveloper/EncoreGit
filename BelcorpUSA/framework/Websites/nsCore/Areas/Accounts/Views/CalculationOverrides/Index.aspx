<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.CalculationOverrides.CalculactionOverridesIndexModel>" %>

<%@ Import Namespace="nsCore.Extensions" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">Accounts</a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> >
	<%: Html.Term("CommissionCalculationOverrides", "Commission Calculation Overrides")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			Commission Calculation Overrides</h2>
		<a href="<%= ResolveUrl("~/Accounts/CalculationOverrides/Edit/") %>">
			<%: Html.Term("AddOverride", "Add Override") %></a>
	</div>
	<div>
		<% 
			Html.PaginatedGrid("/Accounts/CalculationOverrides/Get")
				  .AddColumn(columnHeader: Html.Term("Period", "Period"), propertyName: "PeriodID",
					  isSortable: true,
					  isDefaultSort: true,
					  defaultSortDirection: NetSteps.Common.Constants.SortDirection.Ascending
					  )
				  .CanDelete("/Accounts/CalculationOverrides/Delete")
				  .AddColumn(columnHeader: Html.Term("Calculation", "Calculation"),
					  propertyName: "CalculationTypeID", isSortable: true)
				  .AddColumn(columnHeader: Html.Term("Type", "Type"),
					  propertyName: "OverrideTypeID", isSortable: true)
				  .AddColumn(columnHeader: Html.Term("Value", "Value"),
					  propertyName: "NewValue", isSortable: true)
				  .AddColumn(columnHeader: Html.Term("Reason", "Reason"),
					  propertyName: "OverrideReasonID", isSortable: true)
				  .AddColumn(columnHeader: Html.Term("User", "User"),
					  propertyName: "UserID", isSortable: true)
				  .AddColumn(columnHeader: Html.Term("CreatedDate", "Created Date"),
					  propertyName: "CreatedDate", isSortable: true)
				  .AddColumn(columnHeader: Html.Term("UpdatedDate", "Updated Date"),
					  propertyName: "UpdatedDate", isSortable: true)
				  .AddInputFilter(label: Html.Term("StartDate", "Start Date"),
					  startingValue: DateTime.Now.AddMonths(-6).StartOfMonth().ToShortDateString(),
					  parameterName: "startDate",
					  isDateTime: true
					  )
				  .AddInputFilter(label: Html.Term("EndDate", "End Date"),
					  startingValue: DateTime.Now.NextMonth().StartOfMonth().ToShortDateString(),
					  parameterName: "endDate",
					  isDateTime: true
					  )
				  .AddSelectFilter(label: Html.Term("Reason", "Reason"), parameterName: "overrideReasonID", values: new Dictionary<string, string>() { { "", Html.Term("SelectaReason", "Select a Reason...") } }.AddRange(Model.OverrideReasons))
				  .Render();
		%>
	</div>
</asp:Content>
