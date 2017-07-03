<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" 
    Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.Title>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">
		<h2>
			<%= Html.Term("BrowseTitles", "Browse Titles") %>
        </h2>
        <a href="<%= ResolveUrl("~/Commissions/Configurations/EditTitle") %>"><%= Html.Term("CreateaNewTitle", "Create a New Title") %></a>
	</div>

    <%      Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.TitleSearchData>("~/Commissions/Configurations/Get")
            .AutoGenerateColumns()
            .Render(); %>  

</asp:Content>
