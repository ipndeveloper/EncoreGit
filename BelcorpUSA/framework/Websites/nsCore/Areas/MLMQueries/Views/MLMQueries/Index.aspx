<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/MLMQueries/Views/Shared/MLMQueries.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h2>MLM Indicators per BA</h2>
<script type="text/javascript">
    function lookupID() {
        var accountSelected = false;
        var accountId = $('<input type="hidden" id="accountIDFilter" class="Filter" />');
        $('#accountNameInputFilter').removeClass('Filter').css('width', '180px').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
            accountId.val(item.id);
            accountSelected = true;
        }, minCharacters: 3, source: $('#accountFilter'), ajaxResults: true, maxResults: 50, showMore: true
        }).blur(function () {

            if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                accountId.val('');
            } else if (!accountSelected) {
                accountId.val('-1');
            }

            accountSelected = false;
        }).after(accountId);
    }
</script>
<% Html.RenderPartial("MLMQueriesReportView"); %>
</asp:Content>
