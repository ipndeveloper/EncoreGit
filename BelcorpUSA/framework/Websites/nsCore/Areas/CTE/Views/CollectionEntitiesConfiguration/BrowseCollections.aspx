<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/CollectionEntitiesConfigurationManagement.Master" 
Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.Business.CollectionEntitySearchData>>" %>

<%--//@01 20150817 BR-CC-002 G&S LIB: Se crea la pantalla--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%=  Html.Term("BrowseMeansOfCollection", "Browse Means of Collection")%>
        </h2>
        
       <%= Html.Term("BrowseMeansOfCollection", "Browse Means of Collection")%> | <a href="<%= ResolveUrl("~/CTE/CollectionEntitiesConfiguration/CreateCollectionEntities") %>"><%= Html.Term("CreateCollections", "Create Collection Entities")%></a>
	</div>
    <% Html.PaginatedGrid<CollectionEntitySearchData>("~/CTE/CollectionEntitiesConfiguration/GetCollections")
            .AutoGenerateColumns()

            .AddInputFilter(Html.Term("CollectionEntityName", "Collection Entity Name Or ID"), "CollectionEntityName")
            .AddInputFilter(Html.Term("Status", "Status"), "Status")


            .AddSelectFilter(Html.Term("Location"), "CompanyID", new Dictionary<string, string>() { { "", Html.Term("SelectaLocation", "Select a Location...") } }.AddRange(NetSteps.Data.Entities.Repositories.CollectionEntitiesRepository.BrowseCompanies().ToDictionary(os => os.CompanyID.ToString(), os => os.CompanyName)))
            .AddSelectFilter(Html.Term("PaymentTypes"), "PaymentTypeID", new Dictionary<string, string>() { { "", Html.Term("SelectPaymentType", "Select a Payment Type...") } }.AddRange(SmallCollectionCache.Instance.PaymentTypes.ToDictionary(os => os.PaymentTypeID.ToString(), os => os.GetTerm())))
                                                                                                           
            //.HideClientSpecificColumns_()            
            .ClickEntireRow()
		    .Render(); 
    %>
</asp:Content>


