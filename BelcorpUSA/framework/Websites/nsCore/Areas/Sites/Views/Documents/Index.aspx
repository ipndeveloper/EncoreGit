<%@  Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage<IList<NetSteps.Data.Entities.Archive>>" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>"><%= Html.Term("Sites") %></a> >
	<%= Html.Term("DocumentLibrary", "Document Library") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Document Library", "Document Library") %></h2>
		<%= Html.Term("DocumentLibrary", "Document Library") %>
		| <a href="<%= Page.ResolveUrl("~/Sites/Documents/Edit") %>"><%= Html.Term("AddDocument", "Add Document") %></a> |
		<a href="<%= ResolveUrl("~/Sites/Documents/Categories") %>"><%= Html.Term("ManageDocumentCategories", "Manage Document Categories") %></a>
	</div>
	<%if (TempData["SavedResource"] != null)
   { %>
	<div id="ReturnSuccessMessage">
		<div id="SuccessMessage">
			<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("DocumentSaved", "Document saved successfully!")%></div>
	</div>
	<%} %>
	<% Html.PaginatedGrid<ArchiveSearchData>("~/Sites/Documents/Get")
		.AutoGenerateColumns()
		.CanChangeStatus(true, true, "~/Sites/Documents/ChangeStatus")
		.AddSelectFilter(Html.Term("Status"), "active", new Dictionary<string, string>() { { "", Html.Term("SelectStatus", "Select a Status...") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
		.AddInputFilter(Html.Term("Name"), "name")
		.Render(); %>
</asp:Content>
