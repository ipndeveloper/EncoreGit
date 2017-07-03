<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/PaymentMethodsConfigurationsManagement.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.HelperObjects.SearchParameters.PaymentMethodsSearchParameters>" %>

<%--//@01 20150717 BR-CC-003 G&S LIB: Se crea la pantalla--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%=  Html.Term("BrowsePaymentRulesConfiguration", "Browse Payment Rules Configuration")%>
        </h2>
        
        <%= Html.Term("BrowsePaymentRulesConfiguration", "Browse Payment Rules Configuration") %> | <a href="<%= ResolveUrl("~/CTE/PaymentsMethodsConfiguration/PaymentsRules") %>"><%= Html.Term("PaymentRulesandConfiguration", "Payment Rules and Configurations")%></a>
	</div>
    <% Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.PaymentMethodsSearchData>("~/CTE/PaymentsMethodsConfiguration/GetPayments")
            .AutoGenerateColumns()

            .AddInputFilter(Html.Term("CollectionEntityName", "Collection Entity Name Or ID"), "CollectionEntityName")
            .AddInputFilter(Html.Term("Days For Payment", "Days For Payment"), "DaysForPayment")
            .AddInputFilter(Html.Term("Tolerance % or Value", "Tolerance % or Value"), "ToleranceValue")
            //.AddInputFilter(Html.Term("State", "State"), "State")
            //.AddInputFilter(Html.Term("City", "City"), "City")
            //.AddInputFilter(Html.Term("County", "County"), "County")            
            .AddSelectFilter(Html.Term("FineAndInterestRules"), "FineAndInterestRulesID", new Dictionary<string, string>() { { "", Html.Term("SelectaRule", "Select a Rule...") } }.AddRange(NetSteps.Data.Entities.Repositories.CTERepository.BrowseRules().ToDictionary(os => os.FineAndInterestRulesID.ToString(), os => os.Name)))
            //.AddSelectFilter(Html.Term("OrderStatus"), "OrderStatus", new Dictionary<string, string>() { { "", Html.Term("SelectaOrderStatus", "Select a Order Status...") } }.AddRange(SmallCollectionCache.Instance.OrderStatuses.ToDictionary(os => os.OrderStatusID.ToString(), os => os.GetTerm())))                                   
            //.AddSelectFilter(Html.Term("Account Type Restriction"), "AccountTypeRestrictionID", new Dictionary<string, string>() { { "", Html.Term("Select AccountType", "Select Account Type...") } }.AddRange(SmallCollectionCache.Instance.AccountTypes.ToDictionary(os => os.AccountTypeID.ToString(), os => os.GetTerm())))
            //.AddSelectFilter(Html.Term("Order Type Restriction"), "OrderTypeRestrictionID", new Dictionary<string, string>() { { "", Html.Term("SelectOrderType", "Select Order Type...") } }.AddRange(SmallCollectionCache.Instance.OrderTypes.ToDictionary(os => os.OrderTypeID.ToString(), os => os.GetTerm())))                        
            .SetDefaultSort("FineAndInterestRulesID", NetSteps.Common.Constants.SortDirection.Ascending)                          
            .HideClientSpecificColumns_()            
            .ClickEntireRow()
		    .Render(); 
    %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>

