<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/MaterialManagement.master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.MaterialsSearchParameters>" %>
<%--Inherits="System.Web.Mvc.ViewPage<dynamic>" --%>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">


    <%--<%var account = Model.ConsultantOrCustomerAccountID.HasValue ? NetSteps.Data.Entities.Account.LoadSlim(Model.ConsultantOrCustomerAccountID.Value) : new AccountSlimSearchData(); %>--%>
  <script type="text/javascript">
      $(function () {
          var accountNumberOrName = $('<input type="hidden" id="accountNumberOrName" class="Filter" />').val('');
          $('#accountNumberOrNameInputFilter').change(function () {



              accountNumberOrName.val('');
          });
          $('#accountNumberOrNameInputFilter').removeClass('Filter').after(accountNumberOrName).css('width', '275px')
          	.val('')

				.watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
				.jsonSuggest('<%= ResolveUrl("~/Materials/SearchFilter") %>', { onSelect: function (item) {



				    accountNumberOrName.val(item.id);
				    $('#accountNumberOrName').val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});
          //$(".orderSearch").watermark("Look up by order ID");

          $(document).ajaxSuccess(function (event, xhr, settings) {
              if (settings.url == "/Products/Materials/ActiveDeactive") {
                  $('.filterButton').click();
              }
          });
      });
	</script>
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > 
			<%= Html.Term("BrowseMaterial", "Browse Material")%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 <%     
     Dictionary<int, string> dcBrands = (ViewBag.Brands as Dictionary<int, string>) ?? new Dictionary<int, string>();
    %>
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Materials") %></h2>
		<%= Html.Term("BrowseMaterial", "Browse Material")%> | <a href="<%= ResolveUrl("~/Products/Materials/NewMaterial") %>"><%= Html.Term("CreateNewMaterial", "Create New Material")%></a>
       
	</div>
	<% Html.PaginatedGrid<MaterialsSearchData>("~/Products/Materials/ListMaterials")
		.AutoGenerateColumns()
        .HideClientSpecificColumns_()
        .AddSelectFilter(Html.Term("Status"), "Active", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
        //.AddSelectFilter(Html.Term("Status"), "Active", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") }, { "true", Html.Term("Active", "Active") }, { "false", Html.Term("Inactive", "Inactive") } })
        //.AddInputFilter(Html.Term("Brand"), "Brand")
        .AddSelectFilter(Html.Term("Brand", "Brand"), "BrandId", dcBrands)
        .AddInputFilter(Html.Term("BPCSCode"), "BPCSCode")
        .AddInputFilter(Html.Term("MaterialNumberorName", "Material Number or  Name"), "accountNumberOrName")
        //.CanDelete("~/Products/Categories/DeleteTrees")
        //.AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(SmallCollectionCache.Instance.OrderStatuses.ToDictionary(os => os.OrderStatusID.ToString(), os => os.GetTerm())), startingValue: Model.OrderStatusID.ToString())
        //.AddInputFilter(Html.Term("Last4DigitsOfCreditCard", "Last 4 Digits of Credit Card"), "lastFour", Model.CreditCardLastFourDigits)
        .CanChangeStatus(true, true, "~/Products/Materials/ActiveDeactive")
        .ClickEntireRow()
		.Render(); %>
</asp:Content>
