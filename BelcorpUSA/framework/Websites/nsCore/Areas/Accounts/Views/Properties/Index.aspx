<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">
		<%= Html.Term("Accounts") %></a> >
			<%= Html.Term("Properties") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Properties") %></h2>
		<a href="<%= ResolveUrl("~/Accounts/Properties/Edit") %>">
			<%= Html.Term("CreateaNewProperty", "Create a New Property") %></a>
	</div>
	<% Html.PaginatedGrid("~/Accounts/Properties/Get")
			   .AddColumn(Html.Term("PropertyName", "Property Name"), "Name", true, true, Constants.SortDirection.Ascending)
			   .AddColumn(Html.Term("Required"), "Required", true)
               .AddColumn(Html.Term("Active", "Active"), "Active", false)
			   .AddColumn(Html.Term("Values"), "Values", false)
               .AddColumn(Html.Term("MinimumLength", "Minimum Length"), "Minimum Length", false)
               .AddColumn(Html.Term("MaximumLength", "Maximum Length"), "Maximum Length", false)
			   .CanDelete("~/Accounts/Properties/Delete")
			   .Render(); %>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>