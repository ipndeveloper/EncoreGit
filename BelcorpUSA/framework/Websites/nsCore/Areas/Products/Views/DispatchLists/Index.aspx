<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Dispatch/Dispatch.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Dispatch") %>">
		<%= Html.Term("Dispatch") %></a> >
			<%= Html.Term("Dispatch") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2><%= Html.Term("DispatchLists")%></h2>
        <%= Html.Term("BrowseDispatchLists", "Browse DispatchLists")%> | <a href="<%= ResolveUrl("~/Products/DispatchLists/Create") %>"><%= Html.Term("CreateaNewDispatchLists", "Create a New DispatchLists")%></a>
	</div>
	<div>
		<%if (TempData["SavedDispatchLists"] != null)
	{ %>
		<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
			-moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0;
			border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold;
			margin-bottom: 10px; padding: 7px;">
			<div style="color: #385E0F; display: block;">
				<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("DispatchListsSaved", "DispatchLists saved successfully!") %></div>
		</div>
		<%} %>   
		<% Html.PaginatedGrid("~/Products/DispatchLists/Get") 
         .AddColumn(Html.Term("Name"), "Name", true, true, NetSteps.Common.Constants.SortDirection.Ascending)  
         .AddColumn(Html.Term("Mercado", "Mercado"), "Mercado", true) 
         .CanDelete("~/Products/Dispatch/DeleteDispatchLists") 
         .ClickEntireRow()
		 .Render(); %>
	</div>
</asp:Content>
