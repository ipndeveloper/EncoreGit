<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Disbursements.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.HelperObjects.SearchParameters.DisbursementReportSearchParameters>" %>
<%--/// <summary>
/// author           : mescobar
/// company         : CSTI - Peru
/// create        : 12/18/2015
/// reason          : page index of DisbursementRpoert where filtre data of Disbursement
/// modified    : 
/// reason          :
--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="SectionHeader">
		<h2>
			<%=  Html.Term("DisbursementReport", "Disbursement Report")%>
        </h2>
       
	</div>
    <% Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.DisbursementReportSearchData>("~/Commissions/DisbursementReport/Get")
        .AutoGenerateColumns()
        .HideClientSpecificColumns_()
             .AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"), "accountId")
             .AddSelectFilter(Html.Term("Period", "Period"), "periodID", new Dictionary<int, string>() { { 0, Html.Term("SelectPeriod", "Select Period...") } }.AddRange(Disbursement.ListPeriod()))
             .AddSelectFilter(Html.Term("DisbursementStatus", "Disbursement Status"), "disbursementstatusID", new Dictionary<int, string>() { { 0, Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(DisbursementReport.ListDisbursementStatuses()))
        .ClickEntireRow()
		.Render(); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {
        var AccountID = $('<input type="hidden" id="AccountID" class="Filter" />').val('');
        $('#accountIdInputFilter').change(function () {
            AccountID.val('');
        });
        $('#accountIdInputFilter').removeClass('Filter').after(AccountID).css('width', '275px')
				.val('')
				.watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up route by ID or name")  %>')
				.jsonSuggest('<%= ResolveUrl("~/Commissions/Disbursements/searchAccount") %>', { onSelect: function (item) {
				    AccountID.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
        });

        var exportTerm = '<%= Html.JavascriptTerm("Export", "Export")  %>';
        $('.Button').after('<a id="btnExport" class="Button" href="javascript:void(0);">' + exportTerm + '</a>')

        $('#btnExport').on('click', function (e) {
            e.preventDefault();
            Export();
        });
    });

    function Export() {

        var accountId = $('#AccountID').val() == '' ? 0 : $('#AccountID').val();
        var periodID = $('#periodIDSelectFilter').val() == '' ? 0 : $('#periodIDSelectFilter').val();
        var disbursementstatusID = $('#disbursementstatusIDSelectFilter').val() == '' ? 0 : $('#disbursementstatusIDSelectFilter').val();

        var url = '<%=ResolveUrl("~/Commissions/DisbursementReport/Export")%>' + '?';
        url += 'accountId=' + accountId;
        url += '&' + 'periodID=' + periodID;
        url += '&' + 'disbursementstatusID=' + disbursementstatusID;

        window.location = url;
    }
	</script>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">

<a href="<%= ResolveUrl("~/Commissions") %>">
        <%= Html.Term("MLM") %></a> >
    <%= Html.Term("DisbursementReport")%>

</asp:Content>
