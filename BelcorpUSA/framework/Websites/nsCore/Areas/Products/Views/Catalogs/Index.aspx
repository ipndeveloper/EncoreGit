<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/Catalogs.Master"
	Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Catalog>>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > 
			<%= Html.Term("CatalogManagement", "Catalog Management") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Catalogs") %></h2>
		<%= Html.Term("BrowseCatalogs", "Browse Catalogs") %> | <a href="<%= ResolveUrl("~/Products/Catalogs/Edit") %>"><%= Html.Term("CreateaNewCatalog", "Create a New Catalog") %></a>
	</div>
	<% Html.PaginatedGrid<CatalogSearchData>("~/Products/Catalogs/Get")
		.AutoGenerateColumns()
		.CanChangeStatus(true, true, "~/Products/Catalogs/ChangeStatus")
        .ClickEntireRow()
		.Render(); %>
</asp:Content>
