<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/FInterestRulesManagement.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%=  Html.Term("BrowseRules", "Browse Rules")%>
        </h2>
        
        <%= Html.Term("BrowseRules", "Browse Rules")%> | <a href="<%= ResolveUrl("~/CTE/InterestRules/GenerateRule") %>"><%= Html.Term("GenerateRules", "Generate Rules")%></a>
	</div>
    <% Html.PaginatedGrid<CTERulesSearchData>("~/CTE/InterestRules/GetRules/")
            .AutoGenerateColumns()
            .HideClientSpecificColumns_()            
            .ClickEntireRow()
		    .Render(); 
        %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>


