<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script>
        $(function () {
            $("#txtExpirationDateDesde").datepicker({
                changeMonth: true, changeYear: true
            });
            $("#txtExpirationDateHasta").datepicker({
                changeMonth: true, changeYear: true
            });

            $("#txtOrderDateInicio").datepicker({
                changeMonth: true, changeYear: true
            });

            $("#txtOrderDateFin").datepicker({
                changeMonth: true, changeYear: true
            });
            $("#txtLiquidationDateInicio").datepicker({
                changeMonth: true, changeYear: true
            });
            $("#txtLiquidationDateFin").datepicker({
                changeMonth: true, changeYear: true
            });

//            var txtName = $('#txtName');
//            $('#AuAccount')
//                .removeClass('Filter')
//                .watermark('<%= Html.JavascriptTerm("CitySearch", "Look up city") %>')
//                .jsonSuggest('<%= ResolveUrl("~/CTE/PaymentTicketsReport/AccountSearch") %>',
//                    {
//                        onSelect: function (item) {
//                            $('#AuAccount').val(item.id);
//                            $('#txtName').val(item.text);

//                        },
//                        minCharacters: 3,
//                        source: $('#txtName'),
//                        ajaxResults: true,
//                        maxResults: 50,
//                        showMore: true
//                    }).blur(function () {
//                        if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
//                            txtName.val('');
//                        }
//                    });



            $("#btnSearchTicket").click(function () {

                var AccountId = $("#AuAccount").val();
                AccountId = (isNaN(AccountId) || $.trim(AccountId) == "") ? 0 : AccountId
                var OrderId = $("#txtOrder").val();
                OrderId = (isNaN(OrderId) || $.trim(OrderId) == "") ? 0 : OrderId
                var CountryId = $("#cmbCountry").val();
                CountryId = (isNaN(CountryId) || $.trim(CountryId) == "") ? 0 : CountryId
                var FiscalNote = $("#txtFiscalNote").val();

                var AccountName = $("#txtName").val();
                
                var BankId = $("#cmbBank").val();
                BankId = (isNaN(BankId) || $.trim(BankId) == "") ? 0 : BankId
                var OrderPaymentStatusId = $("#cmbOrderPaymentStatus").val();
                OrderPaymentStatusId = (isNaN(OrderPaymentStatusId) || $.trim(OrderPaymentStatusId) == "") ? 0 : OrderPaymentStatusId
                var NegotiationLevelsId = $("#cmbNegotiationLevels").val();
                NegotiationLevelsId = (isNaN(NegotiationLevelsId) || $.trim(NegotiationLevelsId) == "") ? 0 : NegotiationLevelsId

                var ExpirationStatusesId = $("#cmbExpirationStatuses").val();
                ExpirationStatusesId = (isNaN(ExpirationStatusesId) || $.trim(ExpirationStatusesId) == "") ? 0 : ExpirationStatusesId

                var ExpirationDateDesde = $("#txtExpirationDateDesde").val();
               // ExpirationDateDesde = (isNaN(ExpirationDateDesde) || $.trim(ExpirationDateDesde) == "") ? "" : ExpirationDateDesde

                var ExpirationDateHasta = $("#txtExpirationDateHasta").val();
               // ExpirationDateHasta = (isNaN(ExpirationDateHasta) || $.trim(ExpirationDateHasta) == "") ? "" : ExpirationDateHasta

                var OrderDateInicio = $("#txtOrderDateInicio").val();
                var OrderDateFin = $("#txtOrderDateFin").val();

                var LiquidationDateInicio = $("#txtLiquidationDateInicio").val();
                var LiquidationDateFin = $("#txtLiquidationDateFin").val();

                var query = "?AccountId=" + AccountId
                query += "&OrderId=" + OrderId
                query += "&CountryId=" + CountryId
                query += "&FiscalNote=" + FiscalNote
                query += "&BankId=" + BankId
                query += "&OrderPaymentStatusId=" + OrderPaymentStatusId
                query += "&NegotiationLevelsId=" + NegotiationLevelsId
                query += "&ExpirationStatusesId=" + ExpirationStatusesId

                query += "&ExpirationDateDesde=" + ExpirationDateDesde
                query += "&ExpirationDateHasta=" + ExpirationDateHasta

                query += "&OrderDateInicio=" + OrderDateInicio
                query += "&OrderDateFin=" + OrderDateFin

                query += "&LiquidationDateInicio=" + LiquidationDateInicio
                query += "&LiquidationDateFin=" + LiquidationDateFin

                query += "&AccountName=" + AccountName
                

                window.location = '<%= ResolveUrl("~/CTE/PaymentTicketsReport/BrowseTickets/") %>' + query
            });


        });

          
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <table class="FormTable" width="40%">
        <tr>
            <td>
                Account:
            </td>
            <td>
                <input id="AuAccount" class="Filter TextInput" type="text" value=""/>
            </td>
            <td>
                Name:
            </td>
            <td>
                <input id="txtName" disabled="disabled" type="text" value=""/>
            </td>
        </tr>
        <tr>
            <td>
                Order:
            </td>
            <td>
                <input id="txtOrder" class="Filter TextInput" type="text" value=""/>
            </td>
            <td>
                Contry:
            </td>
            <td>
                <select id="cmbCountry">
                  <option value="0"><%= Html.Term("SelectCountry", "Select a Country...")%></option>
                    <%     Dictionary<int, string> dcCountry = ViewBag.dcCountry as Dictionary<int, string>;
                           foreach (var kvP in dcCountry)
                           {%>
                    <option value="<%= kvP.Key.ToString()%>">
                        <%= kvP.Value%></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                Fiscal Note:
            </td>
            <td>
                <input id="txtFiscalNote" class="Filter TextInput" type="text" value=""/>
            </td>
            <td>
                Bank:
            </td>
            <td>
                <select id="cmbBank">
                  <option value="0"><%= Html.Term("SelectBank", "Select a Bank...") %></option>
                    <%     Dictionary<int, string> dcBank = ViewBag.dcBank as Dictionary<int, string>;
                           foreach (var kvP in dcBank)
                           {%>
                    <option value="<%= kvP.Key.ToString()%>">
                        <%= kvP.Value%></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                Status Payment:
            </td>
            <td>
                <select id="cmbOrderPaymentStatus">
                  <option value="0"><%=Html.Term("SelectStatus", "Select a Status...")%> </option>
                    <%     Dictionary<int, string> dcOrderPaymentStatuses = ViewBag.dcOrderPaymentStatuses as Dictionary<int, string>;
                           foreach (var kvP in dcOrderPaymentStatuses)
                           {%>
                    <option value="<%= kvP.Key.ToString()%>">
                        <%= kvP.Value%></option>
                    <%} %>
                </select>
            </td>
            <td>
                Negotation Level:
            </td>
            <td>
                <select id="cmbNegotiationLevels">
                <option value="0"><%=Html.Term("SelectLevel", "Select a Level...")%> </option>
                    <%     Dictionary<int, string> dcNegotiationLevels = ViewBag.dcNegotiationLevels as Dictionary<int, string>;
                           foreach (var kvP in dcNegotiationLevels)
                           {%>
                    <option value="<%= kvP.Key.ToString()%>">
                        <%= kvP.Value%></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                Expiration Date
            </td>
            <td>
                <input id="txtExpirationDateDesde" class="DatePicker" type="text" value="Start Date" />
            </td>
            <td>
                to
            </td>
            <td>
                <input id="txtExpirationDateHasta" class="DatePicker" type="text" value="End Date" />
            </td>
            <td>
                Expiration Situation
            </td>
            <td>
                <select id="cmbExpirationStatuses">
                  <option value="0"><%=Html.Term("SelectExpirationSituation", "Select a Expiration Situation...")%> </option>
                 
                    <%     Dictionary<int, string> dcExpirationStatuses = ViewBag.dcExpirationStatuses as Dictionary<int, string>;
                           foreach (var kvP in dcExpirationStatuses)
                           {%>
                    <option value="<%= kvP.Key.ToString()%>">
                        <%= kvP.Value%></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td>
                Order Date
            </td>
            <td>
                <input id="txtOrderDateInicio" class="DatePicker" type="text" value="Start Date" />
            </td>
            <td>
                to
            </td>
            <td>
                <input id="txtOrderDateFin" class="DatePicker" type="text" value="End Date" />
            </td>
        </tr>
        <tr>
            <td>
                Liquidation Date
            </td>
            <td>
                <input id="txtLiquidationDateInicio" class="DatePicker" type="text" value="Start Date" />
            </td>
            <td>
                to
            </td>
            <td>
                <input id="txtLiquidationDateFin" class="DatePicker" type="text" value="End Date" />
            </td>
        </tr>
    </table>
    <table class="FormTable" width="100%">
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <div>
                    <!--/ End Entry Form -->
                    <p class="NextSection">
                        <a id="btnSearchTicket" class="Button BigBlue" href="javascript:void(0);"><span>
                            <%= Html.Term("SearchTicket", "Search Ticket")%></span></a>
                    </p>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
