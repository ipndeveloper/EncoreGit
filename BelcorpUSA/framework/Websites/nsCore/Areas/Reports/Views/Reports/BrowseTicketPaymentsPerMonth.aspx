<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reports/Views/Shared/Reports.Master" 
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.HelperObjects.SearchParameters.TicketPaymentPerMonthSearchParameters>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
    
    <script type="text/javascript">
        $(function () {
            $('#exportToExcel').click(function () {

                if ($("#monthInputFilter").val().length != 6)
                    $("#monthInputFilter").val('<%: DateTime.Now.ToString("yyyyMM") %>');

                var data = {
                    ticketNumber: $("#ticketNumberFilter").val() != '' ? $("#ticketNumberFilter").val() : null,
                    accountId: $("#accountIdFilter").val() != '' ? $("#accountIdFilter").val() : null,
                    startIssue: $('#startIssueInputFilter').val() != 'Start Date' ? $('#startIssueInputFilter').val() : null,
                    endIssue: $('#endIssueInputFilter').val() != 'End Date' ? $('#endIssueInputFilter').val() : null,
                    startDue: $('#startDueInputFilter').val() != 'Start Date' ? $('#startDueInputFilter').val() : null,
                    endDue: $('#endDueInputFilter').val() != 'End Date' ? $('#endDueInputFilter').val() : null,
                    orderNumber: $('#orderNumberInputFilter').val(),
                    statusId: $('#statusIdSelectFilter').val() != '0' ? $('#statusIdSelectFilter').val() : null,
                    month: $("#monthInputFilter").val()
                };
                window.location = '<%= ResolveUrl("~/Reports/Reports/TicketPaymentsPerMonthExport") %>?' + $.param(data);
            });
        });
    </script>

    <script type="text/javascript">

        $(document).ready(function () {

            var accountId = $('#accountIdFilter');
            var accountSelected = accountId.val() != '';
            $('#accountIdInputFilter').removeClass('Filter').css('width', '275px').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                accountId.val(item.id);
                accountSelected = true;
            }, minCharacters: 3, source: $('#accountIdFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {

                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    accountId.val('');
                }

                accountSelected = false;
            }).after(accountId);


            var ticketId = $('#ticketNumberFilter');
            var ticketSelected = ticketId.val() != '';
            $('#ticketNumberInputFilter').removeClass('Filter').css('width', '275px').watermark('<%= Html.JavascriptTerm("TicketSearch", "Look up order by Ticket Number") %>').jsonSuggest('<%= ResolveUrl("~/Reports/Reports/SearchTicketNumber") %>', { onSelect: function (item) {
                ticketId.val(item.id);
                ticketSelected = true;
            }, minCharacters: 1, source: $('#ticketNumberFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {

                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    ticketId.val('');
                }

                ticketSelected = false;
            }).after(ticketId);


            if ('<%: Model.AccountText.Length %>' > 0) {
                accountId.val('<%: Model.AccountId %>');
            }

            if ('<%: Model.TicketText.Length %>' > 0) {
                ticketId.val('<%: Model.TicketNumber %>');
            }

        });
    
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    
    <input type="hidden" id="accountIdFilter" class="Filter" />
    <input type="hidden" id="ticketNumberFilter" class="Filter" />

    <%
        var grid = Html.PaginatedGrid("~/Reports/Reports/GetTableTicketPaymentsPerMonth")

        .AddInputFilter(Html.Term("PaymentTicketNumber"), "ticketNumber", Model.TicketText)
        .AddInputFilter(Html.Term("AccountNumber"), "accountId", Model.AccountText)
        .AddInputFilter(Html.Term("IssueDate"), "startIssue", Model.StartIssueDate.HasValue ? Model.StartIssueDate.Value.ToString("d",CoreContext.CurrentCultureInfo) : Html.Term("StartDate"), true)
        .AddInputFilter(Html.Term("To"), "endIssue", Model.EndIssueDate.HasValue ? Model.EndIssueDate.Value.ToString("d",CoreContext.CurrentCultureInfo) : Html.Term("EndDate"), true, true)
        .AddInputFilter(Html.Term("OrderNumber"), "orderNumber", Model.OrderNumber)
        .AddInputFilter(Html.Term("DueDate"), "startDue", Model.StartDueDate.HasValue ? Model.StartDueDate.Value.ToString(CoreContext.CurrentCultureInfo): Html.Term("StartDate"), true)
        .AddInputFilter(Html.Term("To"), "endDue", Model.EndDueDate.HasValue ? Model.EndDueDate.ToShortDateString() : Html.Term("EndDate"), true, true)
        .AddSelectFilter(Html.Term("PaymentStatus"), "statusId",
                new Dictionary<int, string>() { { 0, Html.Term("SelectaStatus") } }.AddRange(ViewBag.statuses as Dictionary<int, string>),
                Model.StatusId.HasValue ? Model.StatusId.Value : 0)
        .AddInputFilter("* " + Html.Term("Month"), "month", Model.Month.ToString("yyyyMM"))
        
        .AddColumn(Html.Term("PaymentTicketNumber", "Payment Ticket No."), "PaymentTicketNumber")
        .AddColumn(Html.Term("OrderNumber", "Order No."), "OrderNumber")
        .AddColumn(Html.Term("NfeNumber", "No. Nfe"), "NfeNumber")
        .AddColumn(Html.Term("OrderDate", "Order Date"), "OrderDate")
        .AddColumn(Html.Term("ExpirationDate", "Expiration Date"), "ExpirationDate")
        .AddColumn(Html.Term("BalanceDate", "Balance Date"), "BalanceDate")
        .AddColumn(Html.Term("OriginalBalance", "Original Balance"), "OriginalBalance")
        .AddColumn(Html.Term("CurrentBalance", "Current Balance"), "CurrentBalance")
        .AddColumn(Html.Term("Status", "Status"), "Status")
        .AddColumn(Html.Term("OriginalExpirationDate", "Original Expiration Date"), "OriginalExpirationDate")
        .AddColumn(Html.Term("AccountNumber", "Account No."), "AccountNumber")
        .AddColumn(Html.Term("FirstName", "First Name"), "FirstName")
        .AddColumn(Html.Term("LastName", "Last Name"), "LastName")
        .AddColumn(Html.Term("PhoneNumber", "Phone Number"), "PhoneNumber")

        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"));
        //.ClickEntireRow();
        grid.Render(); 
        
    %>


    <script type="text/javascript">

        $("#monthInputFilter").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#ticketNumberInputFilter").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
    
    </script>

</asp:Content>