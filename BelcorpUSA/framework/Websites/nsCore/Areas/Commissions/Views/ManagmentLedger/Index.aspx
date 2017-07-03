<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Disbursements.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.HelperObjects.SearchParameters.ManagmentLedgerSearchParameters>" %>
<%--/// <summary>
/// author           : mescobar
/// company         : CSTI - Peru
/// create        : 12/18/2015
/// reason          : page index of ManagamentLedger where filtre data of accountledger
/// modified    : --------------
/// reason          :--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <div class="SectionHeader">
	        <h2>
		        <%=  Html.Term("ManagmentLedger", "Managment Ledger")%>
            </h2>


        </div>   

        <%   
            var Periods = Disbursement.ListPeriod();

            Periods = Periods == null ? new Dictionary<int, string>() : Periods.OrderByDescending(x => x.Key).ToDictionary(p => p.Key, p => p.Value);
            
            Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.ManagmentLedgerSearchData>("~/Commissions/ManagmentLedger/Get")
             .AutoGenerateColumns()
             .AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"), "accountId")
             .AddSelectFilter(Html.Term("Period", "Period"), "periodId", new Dictionary<int, string>() { { 0, Html.Term("SelectPeriod", "Select Period...") } }.AddRange(Periods))
             .AddSelectFilter(Html.Term("BonusType", "Bonus Type"), "bonustypeId", new Dictionary<int, string>() { { 0, Html.Term("SelectBonusType", "Select a Bonus Type...") } }.AddRange(ManagmentLedger.ListBonusTypesML()))
             .AddInputFilter(Html.Term("EffectiveDateRange", "Effective Date Range"), "startDate", Html.Term("StartDate", "Start Date"), true)
             .AddInputFilter(Html.Term("To", "To"), "endDate", Html.Term("EndDate", "End Date"), true)
             .AddInputFilter(Html.Term("EntryAmount", "Entry Amount"), "entryamount")
             .AddSelectFilter(Html.Term("EntryReason", "Entry Reason"), "entryreasonId", new Dictionary<int, string>() { { 0, Html.Term("SelectEntryReason", "Select a Entry Reason...") } }.AddRange(ManagmentLedger.ListLedgerEntryReasonsML()))
             .AddSelectFilter(Html.Term("EntryOrigin", "Entry Origin"), "entryoriginId", new Dictionary<int, string>() { { 0, Html.Term("SelectEntryOrigin", "Select a Entry Origin...") } }.AddRange(ManagmentLedger.ListLedgerEntryOriginsML()))
             .AddSelectFilter(Html.Term("EntryType", "Entry Type"), "entrytypeId", new Dictionary<int, string>() { { 0, Html.Term("SelectEntryType", "Select a EntryType...") } }.AddRange(ManagmentLedger.ListLedgerEntryTypesML()))
             .ClickEntireRow()
             .Render(); 
         %>   
	
   
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

<script type="text/javascript">
    $(function () {
        var AccountID = $('<input type="hidden" id="AccountID" class="Filter" />').val('');
        $('#accountIdInputFilter').change(function () {
            AccountID.val('');
        });
        $('#entryamountInputFilter').keyup(function (event) {

            var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
            // var value = parseFloat($(this).val());


            var formatDecimal = '$1.$2'; // valores por defaul 
            var formatMiles = ",";  // valores por defaul

            if (cultureInfo === 'en-US') {
                 formatDecimal = '$1.$2';
                 formatMiles = ",";
            }
            else if (cultureInfo === 'es-US') {
                 formatDecimal = '$1,$2';
                 formatMiles = ".";
            }
            else if (cultureInfo === 'pt-BR') {
                formatDecimal = '$1,$2';
                formatMiles = ".";
            }


            //            if (!isNaN(value)) {
            if (event.which >= 37 && event.which <= 40) {
                event.preventDefault();
            }

            $(this).val(function (index, value) {


                return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
            });

            //            }

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
        var periodId = $('#periodIdSelectFilter').val() == '' ? 0 : $('#periodIdSelectFilter').val();
        var bonustypeId = $('#bonustypeIdSelectFilter').val() == '' ? 0 : $('#bonustypeIdSelectFilter').val();
        var startDate = $('#startDateInputFilter').val().length != 10 ? null : $('#startDateInputFilter').val();
        var endDate = $('#endDateInputFilter').val().length != 10 ? null : $('#endDateInputFilter').val();
        var entryamount = $('#entryamountInputFilter').val() == '' ? 0 : $('#entryamountInputFilter').val();
        var entryreasonId = $('#entryreasonIdSelectFilter').val() == '' ? 0 : $('#entryreasonIdSelectFilter').val();
        var entryoriginId = $('#entryoriginIdSelectFilter').val() == '' ? 0 : $('#entryoriginIdSelectFilter').val();
        var entrytypeId = $('#entrytypeIdSelectFilter').val() == '' ? 0 : $('#entrytypeIdSelectFilter').val();

        var url = '<%=ResolveUrl("~/Commissions/ManagmentLedger/Export")%>' + '?';
        url += 'accountId=' + accountId;
        url += '&' + 'periodId=' + periodId;
        url += '&' + 'bonustypeId=' + bonustypeId;
        url += '&' + 'startDate=' + startDate;
        url += '&' + 'endDate=' + endDate;
        url += '&' + 'entryamount=' + entryamount;
        url += '&' + 'entryreasonId=' + entryreasonId;
        url += '&' + 'entryoriginId=' + entryoriginId;
        url += '&' + 'entrytypeId=' + entrytypeId;

        window.location = url;
    }
</script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Commissions") %>">
        <%= Html.Term("MLM") %></a> >
    <%= Html.Term("ManagmentLedger")%>
</asp:Content>
