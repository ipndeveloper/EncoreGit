<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reports/Views/Shared/Reports.Master" 
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.HelperObjects.SearchParameters.DebtsPerAgeSearchParameters>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">

    <script type="text/javascript">
        $(function () {
            $('#exportToExcel').click(function () {
                //$(".Button.ModSearch.filterButton").click();
                var data = {
                    accountId: $("#accountIdFilter").val() != '' ? $("#accountIdFilter").val() : null,
                    startbirth: $('#startBirthInputFilter').val() != 'Start Date' ? $('#startBirthInputFilter').val() : null,
                    endbirth: $('#endBirthInputFilter').val() != 'End Date' ? $('#endBirthInputFilter').val() : null,
                    startDue: $('#startDueInputFilter').val() != 'Start Date' ? $('#startDueInputFilter').val() : null,
                    endDue: $('#endDueInputFilter').val() != 'End Date' ? $('#endDueInputFilter').val() : null,
                    startOverdue: $('#startOverdueInputFilter').val() != '' ? $('#startOverdueInputFilter').val() : null,
                    endOverdue: $('#endOverdueInputFilter').val() != '' ? $('#endOverdueInputFilter').val() : null,
                    orderNumber: $('#orderNumberInputFilter').val() != '' ? $('#orderNumberInputFilter').val() : '',
                    forfeit: $('#forfeitSelectFilter').val()
                };

                window.location = '<%= ResolveUrl("~/Reports/Reports/DebtsPerAgeExport") %>?' + $.param(data);
            });
        });
    </script>

    <script type="text/javascript">

        $(document).ready(function () {

            var accountSelected = false;
            var accountId = $('#accountIdFilter');
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


            if ('<%: Model.AccountText.Length %>' > 0) {
                accountId.val('<%: Model.AccountId %>');
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

    <%
        var grid = Html.PaginatedGrid("~/Reports/Reports/GetTableDebtsPerAge")

        .AddInputFilter(Html.Term("AccountNumber"), "accountId", Model.AccountText)
        .AddInputFilter(Html.Term("RangeDateOfBirth"), "startBirth", Model.StartBirthDate.HasValue ? Model.StartBirthDate.ToShortDateString() : Html.Term("StartDate"), true)
        .AddInputFilter(Html.Term("To"), "endBirth", Model.EndBirthDate.HasValue ? Model.EndBirthDate.ToShortDateString() : Html.Term("EndDate"), true, true)
        .AddInputFilter(Html.Term("OverdueDays"), "startOverdue", Model.DaysOverdueStart.HasValue ? Model.DaysOverdueStart.Value.ToString() : String.Empty)
        .AddInputFilter(Html.Term("to"), "endOverdue", Model.DaysOverdueEnd.HasValue ? Model.DaysOverdueEnd.Value.ToString() : String.Empty)
        .AddInputFilter(Html.Term("OrderNumber"), "orderNumber", Model.OrderNumber)
        .AddInputFilter(Html.Term("DueDate"), "startDue", Model.StartDueDate.HasValue ? Model.StartDueDate.ToShortDateString() : Html.Term("StartDate"), true)
        .AddInputFilter(Html.Term("To"), "endDue", Model.EndDueDate.HasValue ? Model.EndDueDate.ToShortDateString() : Html.Term("EndDate"), true, true)
        .AddSelectFilter(Html.Term("SelectForfeit"), "forfeit", new Dictionary<string, string>() { 
            {"", Html.Term("SelectaStatus")}, {"True", Html.Term("Yes")}, {"False", Html.Term("No")} },
            Model.Forfeit.HasValue ? Model.Forfeit.Value.ToString() : "")

        .AddColumn(Html.Term("AccountNumber", "Account No."), "AccountNumber")
        .AddColumn(Html.Term("FirstName", "First Name"), "FirstName")
        .AddColumn(Html.Term("LastName", "Last Name"), "LastName")
        .AddColumn(Html.Term("PaymentTicketNumber", "Payment Ticket No."), "PaymentTicketNumber")
        .AddColumn(Html.Term("OrderNumber", "Order No."), "OrderNumber")
        .AddColumn(Html.Term("NfeNumber", "No. Nfe"), "NfeNumber")
        .AddColumn(Html.Term("OrderDate", "Order Date"), "OrderDate")
        .AddColumn(Html.Term("ExpirationDate", "Expiration Date"), "ExpirationDate")
        .AddColumn(Html.Term("BalanceDate", "Balance Date"), "BalanceDate")
        .AddColumn(Html.Term("OriginalBalance", "Original Balance"), "OriginalBalance")
        .AddColumn(Html.Term("CurrentBalance", "Current Balance"), "CurrentBalance")
        .AddColumn(Html.Term("OverdueDays", "Overdue Days"), "OverdueDays")
        .AddColumn(Html.Term("Forfeit", "Forfeit"), "Forfeit")
        .AddColumn(Html.Term("Period", "Period"), "Period")
        .AddColumn(Html.Term("RangeDateOfBirth", "Date of Birth"), "DateOfBirth")

        .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"));
        //.ClickEntireRow();
        grid.Render(); 
        
    %>

    <script type="text/javascript">

        $("#startOverdueInputFilter").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#endOverdueInputFilter").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });
    
    </script>

</asp:Content>