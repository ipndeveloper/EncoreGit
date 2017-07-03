<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
 Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.DisbursementHolds.DisbursementHoldsIndexModel>" %>
<%@ Import Namespace="nsCore.Extensions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

	<div class="SectionHeader">
		<h2>
			Payment Disbursement Holds</h2>
		<a href="<%= ResolveUrl("~/Accounts/DisbursementHolds/Edit/") %>"><%: Html.Term("AddDisbursementHold", "Add Disbursement Hold") %></a>
	</div>
	<div>
		<% Html.PaginatedGrid("/Accounts/DisbursementHolds/Get")            
            .AddColumn(columnHeader: Html.Term("Reason", "Reason"),
                propertyName: "CheckHoldReason", isSortable: true)            
			.AddColumn(columnHeader: Html.Term("StartDate", "Start Date"),
            propertyName: "StartDate", isSortable: true, isDefaultSort: true, 
                defaultSortDirection: NetSteps.Common.Constants.SortDirection.Ascending)
			.AddColumn(columnHeader: Html.Term("HoldUntilDate", "Hold Until Date"),
				propertyName: "HoldUntil", isSortable: true)
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
            .ClickEntireRow()
            .Render();
		
		%>
	</div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">

<a href="<%= ResolveUrl("~/Accounts") %>">Accounts</a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> > Payment Disbursement Holds

</asp:Content>
