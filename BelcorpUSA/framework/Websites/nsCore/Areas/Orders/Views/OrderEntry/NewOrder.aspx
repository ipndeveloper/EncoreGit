<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Orders") %>">
        <%= Html.Term("Orders", "Orders")%></a> >
    <%= Html.Term("NewOrder", "New Order")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#txtCustomerSuggest').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                $('#accountId').val(item.id);
                $('#txtCustomerSuggest').clearError();
            }, minCharacters: 1, source: $('#txtCustomerSuggest'), ajaxResults: true, maxResults: 50, showMore: true
            });
            $('form').submit(function () {
                if (!$('#accountId').val())
                    return false;
            });
        });
    </script>
    <% if (TempData["Error"] != null && !string.IsNullOrEmpty(TempData["Error"].ToString()))
       { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
        -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
        border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
        margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation">
            <%= TempData["Error"].ToString() %></div>
    </div>
    <% } %>
    <form action="<%= ResolveUrl("~/Orders/OrderEntry") %>" method="get">
    <p>
        <%= Html.Term("AccountSearch", "Account Search")%>: <span class="LawyerText">
            (<%= Html.Term("EnterAtLeast3Characters", "enter at least 3 characters")%>)</p>
    <input type="text" id="txtCustomerSuggest" style="width: 250px;" />
    <input type="hidden" name="accountId" id="accountId" />
    <a href="javascript:void(0);" onclick="if($('#accountId').val()){$(this).closest('form').submit();}else{$('#txtCustomerSuggest').showError('<%= Html.JavascriptTerm("PleaseSelectAnAccount", "Please select an account.")%>');}"
        class="Button BigBlue"><%= Html.Term("StartOrder", "Start Order")%></a>
    </form>
</asp:Content>