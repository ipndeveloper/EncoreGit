<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
 Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.AccountTitleOverrides.AccountTitleOverrideIndexModel>"  %>

<%@ Import Namespace="nsCore.Extensions" %>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">Accounts</a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> > <%: Html.Term("TitleOverrides", "Title Overrides")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			Commission Title Overrides</h2>
		<a href="<%= ResolveUrl("~/Accounts/AccountTitleOverrides/Edit") %>"><%: Html.Term("AddTitleOverride", "Add Title Override") %></a>
	</div>
	<div>
		<% Html.PaginatedGrid("~/Accounts/AccountTitleOverrides/Get")
			.AddColumn(columnHeader: Html.Term("Period", "Period"), 
                propertyName: "PeriodID", 
                isSortable: true, 
                isDefaultSort: true, 
                defaultSortDirection: NetSteps.Common.Constants.SortDirection.Ascending
                //, isDateTime: true
                )
            .CanDelete("/Accounts/AccountTitleOverrides/Delete")
			.AddColumn(columnHeader: Html.Term("Title", "Title"),
				propertyName: "TitleID", isSortable: true)
            .AddColumn(columnHeader: Html.Term("Override", "Override"),
                propertyName: "OverrideTitle")
			.AddColumn(columnHeader: Html.Term("Type", "Type"),
				propertyName: "TitleTypeID", isSortable: true)
			.AddColumn(columnHeader: Html.Term("Reason", "Reason"),
				propertyName: "OverrideReasonID", isSortable: true)
			.AddColumn(columnHeader: Html.Term("User", "User"),
				propertyName: "UserID", isSortable: true)
			.AddColumn(columnHeader: Html.Term("CreatedDate", "Created Date"),
				propertyName: "CreatedDate", isSortable: true)
			.AddColumn(columnHeader: Html.Term("UpdatedDate", "Updated Date"),
				propertyName: "UpdatedDate", isSortable: true)
            .AddInputFilter(label: Html.Term("StartDate", "Start Date"),
                startingValue: DateTime.Now.AddMonths(-6).StartOfMonth().ToShortDateString()
                ,parameterName: "StartDate"
                ,isDateTime: true
                )
            .AddInputFilter(label: Html.Term("EndDate", "End Date"),
                startingValue: DateTime.Now.NextMonth().StartOfMonth().ToShortDateString()
                ,parameterName: "EndDate"
                ,isDateTime: true
                )
            .AddSelectFilter(label: Html.Term("Reason", "Reason"), parameterName: "overrideReasonID", values: new Dictionary<string, string>() { { "", Html.Term("SelectaReason", "Select a Reason...") } }.AddRange(Model.DisplayReasons))
            .Render();
		
		%>
	</div>
</asp:Content>
