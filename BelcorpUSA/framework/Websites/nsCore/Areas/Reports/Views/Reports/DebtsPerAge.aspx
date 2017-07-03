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
            <h3>
                <%= Html.Term("AccountLookUp", " Quick Account Look-Up") %></h3>
            <span class="LawyerText">
                <%= Html.Term("3CharacterMin", "(3 characters min)") %></span>
        </div>
        <div class="Body">
            <div class="StartSearch">
                
                <div class="SearchBox">
                    <input type="hidden" id="accountId" class="Filter" />
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
                        <label for="txtStartBirthDate">
                            <%= Html.Term("RangeDateOfBirth", "Date of Birth") %>:</label><br />
                        <input type="text" id="txtStartBirthDate" class="DatePicker TextInput" value="<%= Html.Term("StartDate", "Start Date") %>" />
                        to
                        <input type="text" id="txtEndBirthDate" class="DatePicker TextInput" value="<%= Html.Term("EndDate", "End Date") %>" />
                    </div>
                    <div class="mb10">
                        <label for="txtStartDueDate">
                            <%= Html.Term("DueDate", "Due Date") %>:</label><br />
                        <input type="text" id="txtStartDueDate" class="DatePicker TextInput" value="<%= Html.Term("StartDate", "Start Date") %>" />
                        to
                        <input type="text" id="txtEndDueDate" class="DatePicker TextInput" value="<%= Html.Term("EndDate", "End Date") %>" />
                    </div>
                    <div class="mb10">
                        <label for="txtStartOverdue">
                            <%= Html.Term("OverdueDays", "Overdue Days") %>:</label><br />
                        <input type="text" id="txtStartOverdue" class="TextInput" />
                        <input type="text" id="txtEndOverdue" class="TextInput" />
                    </div>
                    <div class="mb10">
                        <label for="txtOrderNumber">
                            <%= Html.Term("OrderNumber")%>:</label><br />
                        <input type="text" id="txtOrderNumber" class="TextInput" style="width: 15em;" />
                    </div>
                    <div class="mb10">
                        <label for="cboForfeit">
                            <%= Html.Term("SelectForfeit", "Forfeit?")%>:</label><br />
                        <select id="cboForfeit" style="width: 15em;">
                            <option value=""><%= Html.Term("SelectaStatus")%></option>
                            <option value="true"><%= Html.Term("Yes")%></option>
                            <option value="false"><%= Html.Term("No")%></option>
                        </select>
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

        $(document).ready(function(){
            $("#txtStartOverdue").keypress(function () {
                if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                    event.preventDefault();
                }
            });

            $("#txtEndOverdue").keypress(function () {
                if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                    event.preventDefault();
                }
            });
        });

        $(function () {

            $('#btnGo').click(function () {
                var txtAccountNumber = $('#txtAccountNumber');
                if (txtAccountNumber.val() == txtAccountNumber.attr('title'))
                    txtAccountNumber.val('');

                var data = {
                    accountId: $("#accountId").val(),
                    accountText: txtAccountNumber.val(),
                    startbirth: null,
                    endbirth: null,
                    startDue: null,
                    endDue: null,
                    startOverdue: null,
                    endOverdue: null,
                    orderNumber: '',
                    forfeit: ''
                };
                window.location = '<%= ResolveUrl("~/Reports/Reports/BrowseDebtsPerAge") %>?' + $.param(data);
            });
            var accountLanding = window.location.pathname == '/' || window.location.pathname == '/Accounts';
            $('#txtAccountNumber').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                $("#accountId").val(item.id);
                $('#txtAccountNumber').val(item.text/*.replace(/.+\(\#([^\)]*)\)/, '$1')*/);
            }, defaultToFirst: false, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: $('#txtAccountNumber').outerWidth(true) + $('#btnGo').outerWidth() - (accountLanding ? 2 : 4)
            }).keyup(function (e) {
                if (e.keyCode == 13) {
                    $('#btnGo').click();
                }
            });


            $('#btnAdvancedGo').click(function () {
                
                var data = {
                    accountId: null,
                    accountText: '',
                    startbirth: $('#txtStartBirthDate').val() != 'Start Date' ? $('#txtStartBirthDate').val() : null,
                    endbirth: $('#txtEndBirthDate').val() != 'End Date' ? $('#txtEndBirthDate').val() : null,
                    startDue: $('#txtStartDueDate').val() != 'Start Date' ? $('#txtStartDueDate').val() : null,
                    endDue: $('#txtEndDueDate').val() != 'End Date' ? $('#txtEndDueDate').val() : null,
                    startOverdue: $('#txtStartOverdue').val() != '' ? $('#txtEndOverdue').val() : null,
                    endOverdue: $('#txtEndOverdue').val() != '' ? $('#txtEndOverdue').val() : null,
                    orderNumber: $('#txtOrderNumber').val() != '' ? $('#txtOrderNumber').val() : '',
                    forfeit: $('#cboForfeit').val()
                };
                window.location = '<%= ResolveUrl("~/Reports/Reports/BrowseDebtsPerAge") %>?' + $.param(data);
            });

        });

    </script>

</asp:Content>
