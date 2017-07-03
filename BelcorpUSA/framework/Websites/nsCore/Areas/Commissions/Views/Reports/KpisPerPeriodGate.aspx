<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" 
Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Commissions.Models.KpisPerPeriodModel>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Commissions") %>">
		<%= Html.Term("Commissions", "Commissions")%></a> > <%= Html.Term("KPIsPerPeriod", "KPIs per Period")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%=  Html.Term("KPIsPerPeriod", "KPIs per Period")%>
		</h2>
	</div>
    <% 
        var Periods = new Dictionary<string, string>();
        Periods.Add("", Html.Term("SelectPeriod", "Select a Period"));
        Periods.AddRange(Model.Periods);
        Html.PaginatedGrid("~/Commissions/Reports/KpisPerPeriod")
            .AddSelectFilter(Html.Term("CommissionReportPeriodsRange", "Periods Range To"), "PeriodStart", Periods, autoPostBack: false)
            .AddSelectFilter(null, "PeriodEnd", Periods, autoPostBack: false)
            .AddInputFilter(Html.Term("AccountID"), "AccountID", addBreak: true)
            .AddInputFilter(Html.Term("Sponsor"), "SponsorID")
            .AddColumn(Html.Term("CommissionReportPeriodID", "Period"), "PeriodID", true)
            .AddColumn(Html.Term("CommissionReportAccountID", "Account Number"), "AccountID", false)
            .AddColumn(Html.Term("CommissionReportAccountName", "Account Name"), "AccountName", false)
            .AddColumn(Html.Term("CommissionReportSponsorID", "Sponsor "), "SponsorID", false)
            .AddColumn(Html.Term("CommissionReportSponsorName", "Sponsor Name"), "SponsorName", false)
            .AddColumn(Html.Term("CommissionReportPaidAsCurrentMonth", "Paid-As Title"), "PaidAsCurrentMonth", false)
            .AddColumn(Html.Term("CommissionReportCareerTitle", "Career Title"), "CareerTitle", false)
            .AddColumn(Html.Term("CommissionReportPQV", "PQV"), "PQV", false)
            .AddColumn(Html.Term("CommissionReportPCV", "PCV"), "PCV", false)
            .AddColumn(Html.Term("CommissionReportDQV", "DQV"), "DQV", false)
            .AddColumn(Html.Term("CommissionReportCQL", "CQL"), "CQL", false)
            .AddColumn(Html.Term("CommissionReportTitle1Legs", "C0"), "Title1Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle2Legs", "C1"), "Title2Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle3Legs", "C2"), "Title3Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle4Legs", "C3"), "Title4Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle5Legs", "M1"), "Title5Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle6Legs", "M2"), "Title6Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle7Legs", "M3"), "Title7Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle8Legs", "L1"), "Title8Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle9Legs", "L2"), "Title9Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle10Legs", "L3"), "Title10Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle11Legs", "L4"), "Title11Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle12Legs", "L5"), "Title12Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle13Legs", "L6"), "Title13Legs", false)
            .AddColumn(Html.Term("CommissionReportTitle14Legs", "L7"), "Title14Legs", false)
            .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
            .ClickEntireRow()
            .Render(); 
    %>
    <div id="dialogo" title= "Encore" style="font-family: calibri; font-size: 10pt; font-weight: normal">
        <p></p>
    </div>
    <script type="text/javascript">
        $(function () {
            $('div.FilterSet > div').eq(0).append(periodStartFilter);
            $('div.FilterSet > div').eq(1).append(periodEndtFilter);
            $('div.FilterSet > div').eq(1).find('label').html('&nbsp;');
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(function () {
        var sponsorSelected = false;
        var accountSelected = false;
        var sponsorId = $('<input type="hidden" id="SponsorIDFilter" class="Filter" />');
        var accountId = $('<input type="hidden" id="AccountIDFilter" class="Filter" />');
        $('#SponsorIDInputFilter').removeClass('Filter').css('width', '275px').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
            sponsorId.val(item.id);
            sponsorSelected = true;
        }, minCharacters: 3, source: $('#SponsorIDFilter'), ajaxResults: true, maxResults: 50, showMore: true
        }).blur(function () {
            if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                sponsorId.val('');
            } else if (!sponsorSelected) {
                sponsorId.val('');
            }
            sponsorSelected = false;
        }).after(sponsorId).keyup(function (e) {
            var code = e.which;
            if (code == 13) {
                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    $('#SponsorIDFilter').val('');
                }
            }
        });
        $('#AccountIDInputFilter').removeClass('Filter').css('width', '275px').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
            accountId.val(item.id);
            accountSelected = true;
        }, minCharacters: 3, source: $('#AccountIDFilter'), ajaxResults: true, maxResults: 50, showMore: true
        }).blur(function () {
            if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                accountId.val('');
            } else if (!accountSelected) {
                accountId.val('');
            }
            accountSelected = false;
        }).after(accountId).keyup(function (e) {
            var code = e.which;
            if (code == 13) {
                if (!$(this).val() || $(this).val() == $(this).data('watermark') || $(this).val() == '') {
                    $('#AccountIDFilter').val('');
                }
            }
        });
        periodStartFilter = $('#PeriodStartSelectFilter').clone();
        periodEndtFilter = $('#PeriodEndSelectFilter').clone();
        $('#PeriodStartSelectFilter').remove();
        $('#PeriodEndSelectFilter').remove();
        exportE();
        IsValid();
        LoadInitialMessage();
    });
    function LoadInitialMessage() {
        var periodStart = $("#PeriodStartSelectFilter").find(":selected").index();
        var periodEnd = $("#PeriodEndSelectFilter").find(":selected").index();
        if (periodStart == 0 || periodEnd == 0) {
            ShowAlert('<%=Html.Term("GMP_InformationForSearching", "PLEASE ENTER INFORMATION FOR SEARCHING")%>');
            return false;
        }
    }

    function IsValid() {
        $('div.GridFilters a.filterButton').click(function () {
            var periodStart = $("#PeriodStartSelectFilter").find(":selected");
            var periodEnd = $("#PeriodEndSelectFilter").find(":selected");
            if (periodStart.index() == 0 || periodEnd.index() == 0) {
                ShowAlert('<%=Html.Term("ReportPeriodStartEndSearching", "START PERIOD AND END PERIOD MUST BE INCLUDE FOR SEARCHING")%>');
                return false;
            }
            if (periodStart.val() > periodEnd.val()) {
                ShowAlert('<%=Html.Term("ReportPeriodStartEndError", "START PERIOD MAY NOT EXCEED THE END PERIOD")%>');
                return false;
            }
        });
    }

    function exportE() {
        $('#exportToExcel').click(function () {
            var periodStart = $("#PeriodStartSelectFilter").find(":selected").index();
            var periodEnd = $("#PeriodEndSelectFilter").find(":selected").index();
            if (periodStart == 0 || periodEnd == 0) {
                ShowAlert('<%=Html.Term("GMP_InformationForSearching", "PLEASE ENTER INFORMATION FOR SEARCHING")%>');
                return false;
            }
            window.location = '<%=ResolveUrl("~/Commissions/Reports/KpisPerPeriodExport")%>';
        });
    }

    function ShowAlert(msj) {
        $("#dialogo").html(msj);
        $("#dialogo").dialog({
            create: function (event, ui) {
                $('.ui-dialog-titlebar').css({ 'background': '#00659e', 'border': 'none', 'font-family': 'Helvetica', 'font-size': '11px', 'color': 'white' });
            },
            modal: true,
            resizable: false,
            width: 350,
            height: 150,
            show: "fold",
            hide: "scale",
            buttons:
                         {
                             "1": { id: 'btnClose', text: 'Cerrar', click: function () { $(this).dialog("close"); }, "class": "Cerrar"
                             }
                         }
        });
    }
</script>
<style type="text/css">
    .ui-dialog .ui-dialog-buttonpane .ui-dialog-buttonset .Cerrar 
    {
        font-family:Helvetica;
        font-size:11px;
    }
</style>  
</asp:Content>