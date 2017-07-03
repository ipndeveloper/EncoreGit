<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reports/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">

    <div class="LandingTools">
        <div class="Title">
            <h3> <%= Html.Term("TicketLookUp", "Payment Ticket No. Look-Up")%></h3>
            <span class="LawyerText"> <%= Html.Term("3CharacterMin", "(3 characters min)") %> </span>
            
            <br />
            <br />

            <h3> <%= Html.Term("AccountLookUp", " Quick Account Look-Up") %></h3>
            <span class="LawyerText"> <%= Html.Term("3CharacterMin", "(3 characters min)") %> </span>
        </div>
        <div class="Body">
            <div class="StartSearch">
                
                <div class="SearchBox">
                    <input type="hidden" id="ticketNumberFilter" class="Filter" />
	                <input id="txtTicketNumber" type="text" class="TextInput distributorSearch" />
	                <a href="javascript:void(0);" id="A1" class="SearchIcon"><span>Go</span></a>
                </div>
                
                <br />
                <span class="ClearAll"></span>
                <br />

                <div class="SearchBox">
                    <input type="hidden" id="accountIdFilter" class="Filter" />
	                <input id="txtAccountNumber" type="text" class="TextInput distributorSearch" />
	                <a href="javascript:void(0);" id="btnGo" class="SearchIcon"><span>Go</span></a>
                </div>

            </div>
        </div>
        <span class="ClearAll"></span>
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
                        <label for="txtStartIssueDate">
                            <%= Html.Term("IssueDate", "Issue Date")%>:</label><br />
                        <input type="text" id="txtStartIssueDate" class="DatePicker TextInput" value="<%= Html.Term("StartDate", "Start Date") %>" />
                        to
                        <input type="text" id="txtEndIssueDate" class="DatePicker TextInput" value="<%= Html.Term("EndDate", "End Date") %>" />
                    </div>
                    <div class="mb10">
                        <label for="txtStartDueDate">
                            <%= Html.Term("DueDate", "Due Date") %>:</label><br />
                        <input type="text" id="txtStartDueDate" class="DatePicker TextInput" value="<%= Html.Term("StartDate", "Start Date") %>" />
                        to
                        <input type="text" id="txtEndDueDate" class="DatePicker TextInput" value="<%= Html.Term("EndDate", "End Date") %>" />
                    </div>
                    <div class="mb10">
                        <label for="txtOrderNumber">
                            <%= Html.Term("OrderNumber")%>:</label><br />
                        <input type="text" id="txtOrderNumber" class="TextInput" style="width: 15em;" />
                    </div>
                    <div class="mb10">
                        <label for="cboStatus">
                            <%= Html.Term("PaymentStatus")%>:</label><br />
                        <select id="cboStatus" style="width: 15em;">
                            <option value="0"><%= Html.Term("SelectaStatus")%></option>
                            <% foreach (var status in ViewBag.statuses as Dictionary<int, string>)
                               { %>
                                   <option value='<%: status.Key %>'><%: status.Value %></option>
                            <% } %>
                        </select>
                    </div>
                    <div class="mb10">
                        <label for="txtMonth">
                            * <%= Html.Term("Month")%>:</label><br />
                        <input type="text" id="txtMonth" class="TextInput" style="width: 10em;" />
                        <span id="monthMessage" style="color: Red;"></span>
                    </div>
                    <br />
                    <p>
                        <a href="javascript:void(0);" id="btnAdvancedGo" class="Button BigBlue">
                            <%= Html.Term("Search", "Search") %></a>
                    </p>
                </div>
            </div>
        </div>
        <span class="ClearAll"></span>
    </div>




<script type="text/javascript">

    $(function () {

        var accountId = $('#accountIdFilter');
        var accountLanding = window.location.pathname == '/' || window.location.pathname == '/Accounts';
        $('#txtAccountNumber').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').
        jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
            $('#txtAccountNumber').val(item.text/*.replace(/.+\(\#([^\)]*)\)/, '$1')*/);
            accountId.val(item.id);
        }, defaultToFirst: false, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: $('#txtAccountNumber').outerWidth(true) + $('#btnGo').outerWidth() - (accountLanding ? 2 : 4)
        });

        var ticketId = $('#ticketNumberFilter');
        $('#txtTicketNumber').watermark('<%= Html.JavascriptTerm("TicketSearch", "Look up order by Ticket Number") %>').
        jsonSuggest('<%= ResolveUrl("~/Reports/Reports/SearchTicketNumber") %>', { onSelect: function (item) {
            $('#txtTicketNumber').val(item.text);
            ticketId.val(item.id);
        }, defaultToFirst: false, minCharacters: 1, ajaxResults: true, maxResults: 50, showMore: true, width: $('#txtTicketNumber').outerWidth(true) + $('#btnGo').outerWidth() - (accountLanding ? 2 : 4)
        });


        $('#btnAdvancedGo').click(function () {
            var month = $("#monthMessage");
            if ($("#txtMonth").val().length != 6) {
                month.text('<%= Html.JavascriptTerm("YearMonthRequired", "Month is Required: YYYYMM") %>');
                return false;
            }

            month.text('');

            
            var data = {
                ticketNumber: $('#ticketNumberFilter').val() != '' ? $('#ticketNumberFilter').val() : null,
                ticketText: $('#txtTicketNumber').val(),
                accountId: $('#accountIdFilter').val() != '' ? $('#accountIdFilter').val() : null,
                accountText: $('#txtAccountNumber').val(),


                startIssue: isDate($('#txtStartIssueDate').val()) == true ? $('#txtStartIssueDate').val() : null,
                endIssue: isDate($('#txtEndIssueDate').val()) == true ? $('#txtEndIssueDate').val() : null,
                startDue: isDate($('#txtStartDueDate').val())==true ? $('#txtStartDueDate').val() : null,
                endDue: isDate($('#txtEndDueDate').val())==true ? $('#txtEndDueDate').val() : null,







//                startIssue: $('#txtStartIssueDate').val() != 'Start Date' ? $('#txtStartIssueDate').val() : null,
//                endIssue: $('#txtEndIssueDate').val() != 'End Date' ? $('#txtEndIssueDate').val() : null,
//                startDue: $('#txtStartDueDate').val() != 'Start Date' ? $('#txtStartDueDate').val() : null,
//                endDue: $('#txtEndDueDate').val() != 'End Date' ? $('#txtEndDueDate').val() : null,
                orderNumber: $('#txtOrderNumber').val(),
                statusId: $('#cboStatus').val() != '0' ? $('#cboStatus').val() : null,
                month: $("#txtMonth").val()
            };
            window.location = '<%= ResolveUrl("~/Reports/Reports/BrowseTicketPaymentsPerMonth") %>?' + $.param(data);
        });

    });
    function isDate(val) {
        var d = new Date(val);
        return !isNaN(d.valueOf());
    }
</script>


<script type="text/javascript">

    $("#txtMonth").keypress(function () {
        if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });

    $("#txtTicketNumber").keypress(function () {
        if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
            event.preventDefault();
        }
    });
    
</script>


</asp:Content>