<%@ Page Language="C#" Inherits="System.Web.Mvc.ViewPage" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#txtAccount').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                $('#accountId').val(item.id);
            }, minCharacters: 1, source: $('#txtSponsor'), ajaxResults: true, maxResults: 50, showMore: true
            });

            $('.TabSearch input[type=text]').keyup(function (e) {
                if (e.keyCode == 13)
                    $('#btnAdvancedGo').click();
            });

            $('#btnAdvancedGo').click(function () {
                
                var data = {
                    lastFour: $('#txtCCNLast4').val(),
                    status: $('#sStatus').val(),
                    type: $('#sOrderType').val(),
                    accountId: $('#accountId').val(),
                    startDate:  $('#startDate').val(),
                    endDate: $('#endDate').val()
//                    startDate: !/Invalid|NaN/.test(new Date($('#startDate').val())) ? $('#startDate').val() : '',
//                    endDate: !/Invalid|NaN/.test(new Date($('#endDate').val())) ? $('#endDate').val() : ''
                };
                window.location = '<%= ResolveUrl("~/Orders/Browse/Index") %>?' + $.param(data);

            });
        });

    </script>

</asp:Content>
<asp:Content ID="Content4" runat="server" ContentPlaceHolderID="BreadCrumbContent">
    <%= Html.Term("Orders") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="MainContent">
    <div class="LandingTools StartSearch">
        <div class="Title">
            <h3>
                <%= Html.Term("OrderLook-up", "Order Look-up") %></h3>
        </div>
        <div class="Body">
            <% Html.RenderPartial("Find", ViewData); %>
        </div>
        <span class="Clear"></span>
    </div>
    <div class="LandingTools">
        <div class="Title">
            <h3>
                <%= Html.Term("AdvancedSearch", "Advanced Search") %></h3>
        </div>
        <div class="Body">
            <div class="StartSearch">
                <div class="TabSearch">
                    <div class="mb10">
                        <%= Html.Term("Last4DigitsOfCreditCard", "Last 4 Digits of Credit Card") %>:<br />
                        <input type="text" id="txtCCNLast4" />
                    </div>
                    <div class="mb10">
                        <%= Html.Term("Status") %>:<br />
                        <select id="sStatus">
                            <option value="">
                                <%= Html.Term("SelectStatus", "Select Status...") %></option>
                            <%foreach (OrderStatus orderStatus in SmallCollectionCache.Instance.OrderStatuses)
                              { %>
                            <option value="<%= orderStatus.OrderStatusID %>">
                                <%= orderStatus.GetTerm() %></option>
                            <%} %>
                        </select>
                    </div>
                    <div class="mb10">
                        <%= Html.Term("OrderType", "Order Type") %>:<br />
                        <select id="sOrderType">
                            <option value="">
                                <%= Html.Term("SelectOrderType", "Select Order Type...") %></option>
                            <%foreach (OrderType orderType in SmallCollectionCache.Instance.OrderTypes)
                              { %>
                            <option value="<%= orderType.OrderTypeID %>">
                                <%= orderType.GetTerm()%></option>
                            <%} %>
                        </select>
                    </div>
                    <div class="mb10">
                        <%= Html.Term("DateRange", "Date Range") %>:<br />
                        <input type="text" id="startDate" class="DatePicker TextInput" value="<%= Html.Term("StartDate", "Start Date") %>" />
                        to
                        <input type="text" id="endDate" class="DatePicker TextInput" value="<%= Html.Term("EndDate", "End Date") %>" />
                    </div>
                    <div class="mb10">
                        <%= Html.Term("Account") %>:<br />
                        <input type="text" id="txtAccount" style="width: 22em;" />
                        <input type="hidden" id="accountId" />
                    </div>
                    <p>
                        <a href="javascript:void(0);" id="btnAdvancedGo" class="Button BigBlue">
                            <%= Html.Term("Search")%></a>
                    </p>
                </div>
            </div>
        </div>
        <span class="ClearAll"></span>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
