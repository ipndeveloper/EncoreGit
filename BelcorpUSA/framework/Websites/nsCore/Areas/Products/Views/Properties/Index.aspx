<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
			<%= Html.Term("Properties") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Properties") %></h2>
		<a href="<%= ResolveUrl("~/Products/Properties/Edit") %>">
			<%= Html.Term("CreateaNewProperty", "Create a New Property") %></a>
	</div>
	<% Html.PaginatedGrid("~/Products/Properties/Get")
			   .AddColumn(Html.Term("PropertyName", "Property Name"), "Name", true, true, Constants.SortDirection.Ascending)
			   .AddColumn(Html.Term("Required"), "Required", true)
			   .AddColumn(Html.Term("Values"), "Values", false)
			   .CanDelete("~/Products/Properties/Delete")
			   .Render(); %>
</asp:Content>
