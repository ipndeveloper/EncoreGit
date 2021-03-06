﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" 
Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Commissions.Models.EarningsPerPeriodModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<div class="SectionHeader">
		<h2>
			<%=  Html.Term("EarningsPerPeriod", "Earnings per Period")%>
		</h2>
	</div>
    <% 
        var Periods = new Dictionary<string, string>();
        var BonusTypes = new Dictionary<string, string>();
        Periods.Add("", Html.Term("SelectPeriod", "Select a Period"));
        BonusTypes.Add("", Html.Term("SelectBonus", "Bonus..."));
        Periods.AddRange(Model.Periods);
        BonusTypes.AddRange(Model.BonusTypes);
        Html.PaginatedGrid("~/Commissions/Reports/EarningsPerPeriod")
            .AddSelectFilter(Html.Term("CommissionReportPeriodsRange", "Periods Range To"), "PeriodStart", Periods, autoPostBack: false)
            .AddSelectFilter(null, "PeriodEnd", Periods, autoPostBack: false, addBreak: true)
            .AddSelectFilter(Html.Term("SelectaBonus", "Select a Bonus"), "BonusTypeID", BonusTypes, autoPostBack: false)
            .AddInputFilter(Html.Term("CommissionReportAccountID", "Account Number"), "AccountID")
            .AddColumn(Html.Term("CommissionReportPeriodID", "Period"), "PeriodID", true)
            .AddColumn(Html.Term("CommissionReportBonusName", "Bonus Name"), "BonusName", true)
            .AddColumn(Html.Term("CommissionReportAccountID", "Account Number"), "AccountNumber", true)
            .AddColumn(Html.Term("CommissionReportAccountName", "Account Name"), "AccountName", true)
            .AddColumn(Html.Term("CommissionReportAmount", "Amount"), "Amount", true)
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
            $('div.FilterSet > div').eq(1).append(periodEndFilter);
            $('div.FilterSet > div').eq(1).find('label').html('&nbsp;');
            $('div.FilterSet > div').eq(2).append(bonusTypeIDFilter);
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
<script type="text/javascript">
    $(function () {
        periodStartFilter = $('#PeriodStartSelectFilter').clone();
        periodEndFilter = $('#PeriodEndSelectFilter').clone();
        bonusTypeIDFilter = $('#BonusTypeIDSelectFilter').clone();
        $('#PeriodStartSelectFilter').remove();
        $('#PeriodEndSelectFilter').remove();
        $('#BonusTypeIDSelectFilter').remove();
        var accountId = $('<input type="hidden" id="AccountIDFilter" class="Filter" />');
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
            window.location = '<%=ResolveUrl("~/Commissions/Reports/EarningsPerPeriodExport")%>';
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

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Commissions") %>">
		<%= Html.Term("Commissions", "Commissions")%></a> > <%= Html.Term("EarningsPerPeriod", "Earnings per Period")%>
</asp:Content>
