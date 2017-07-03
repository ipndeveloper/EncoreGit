<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Order>" %>

<%@ Import Namespace="NetSteps.Data.Entities.Business.HelperObjects.OrderPackages" %>
<%@ Import Namespace="nsCore.Areas.Orders.Models.Details" %>
<%--Modifications:--%>
<%--@01 20150630 BR-AT-006 CSTI BAL : Add link to Tracking--%>
<%--@02 20150817 BR-AT-008 GYS EFP : Add link to Claim Items--%>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Orders") %>">
        <%= Html.Term("Orders", "Orders") %></a> >
    <%= Html.Term("OrderDetail", "Order Detail") %>

     
</asp:Content>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            if (window.location.search.length) {
                var qsVariables = window.location.search.substring(1).split('&'), i;
                for (i = 0; i < qsVariables.length; i++) {
                    var qsVariable = qsVariables[i].split('=');
                    if (qsVariable[0] == 'message') {
                        //var messageCenter = $('#messageCenter').messageCenter('#385E0F', '#CAFF70', '/Content/Images/accept-trans.png', '1000', null, '3000', '1000', null);
                        showMessage(qsVariable[1].replace(/_/g, ' '), false);
                        //messageCenter.addMessage(qsVariable[1].replace(/_/g, ' '));
                        break;
                    }
                }
            }

         
         if ('<%=Session["Edit"] %>' != null){
          if ('<%=Session["Edit"] %>' != "") {
           if ('<%=Session["Edit"] %>' != "3") {

           $.post('/Orders/Details/ValidateSendEmailBoleto', { orderNumber: '<%= Model.OrderNumber %>' }, function (results) {
                        
                        if (results.result) {
                      
                            var url = '<%= ResolveUrl("~/Orders/OrderEntry/ExportarBoleta") %>?';
                            url = url + "OrderPaymentID=" + results.OrderPaymentID + "&BankCode=" + results.BankCode;
                            $("#frmExportar").attr("src", url);
                        }                        
                    });
             }
           }
         }

            $('#noteParentEntityID').val('<%= Model.OrderNumber %>');

            if ($('#returnOrder').length) {
                $.getJSON('<%= ResolveUrl("~/Orders/Details/CheckIfOrderFullyReturned") %>', { orderNumber: $('#orderNumber').val() }, function (results) {
                    if (!results.fullyReturned) {
                        //26/11/2015 - Ini - Comentado a solicitud de Leonardo Caceres - Encore Colombia
                        //$('#returnOrder').attr('href', '<%= ResolveUrl("~/Orders/Return/OrderParams?orderNumber=") + Model.OrderNumber + "&idSupport=" + Model.IDSupportTicket %>').css({ cursor: '', color: '' });
                        //26/11/2015 - Fin - Comentado a solicitud de Leonardo Caceres - Encore Colombia

                        <%if(Request.QueryString["SupportTicketID"]!=null){ %>
                        $('#returnOrder').attr('href', '<%= ResolveUrl("~/Orders/Return/Index/") + Model.OrderID %>?SupportTicketID=<%=ViewData["SupportTicketID"]%>').css({ cursor: '', color: '' });
                   <%}else { %>
                     $('#returnOrder').attr('href', '<%= ResolveUrl("~/Orders/Return/Index/") + Model.OrderID %>').css({ cursor: '', color: '' });
                   <%} %>


                    }
                });
            }
            //26/11/2015 - Ini - Comentado a solicitud de Leonardo Caceres - Encore Colombia
            //            $('#returnOrder').click(function () {
            //                if (('<%= Model.IDSupportTicket%>').trim() == "") {
            //                    alert('<%= Html.Term("SupportTicketNotFound", "Support Ticket Not Found")%>');
            //                    event.preventDefault();
            //                    return false;
            //                }
            //            });
            //26/11/2015 - Fin - Comentado a solicitud de Leonardo Caceres - Encore Colombia

            $('#cancelOrder').click(function () {
                if (confirm('Are you sure?')) {
                    $.post('/Orders/Details/Cancel', { orderNumber: '<%= Model.OrderNumber %>' }, function (results) {
                        if (results.result) {
                            location.reload(true);
                        }
                        else {
                            showMessage(results.message, true);
                        }
                    });
                }
            });

            $('#cancelPaidOrder').click(function () {
            <% if(Request.QueryString["SupportTicketID"]!=null) {%>
                if (('<%= Request.QueryString["SupportTicketID"]%>').trim() == "") {
                    alert('<%= Html.Term("SupportTicketNotFound", "Support Ticket Not Found")%>');
                    return false;
                }
                <%} %>

                if (confirm('Are you sure?')) {
                    $.post('/Orders/Details/Cancel', { orderNumber: '<%= Model.OrderNumber %>' }, function (results) {
                        if (results.result) {
                            //location = '/Orders/Return/OrderParams?orderNumber=' + '<%= Model.OrderNumber%>' + '&idSupport=' + '<%= ViewData["SupportTicketID"]%>&SupportTicketID=<%=ViewData["SupportTicketID"]%>';
                            location = '/Orders/Return/OrderParams?accountID=' + '<%= Model.OrderCustomers.FirstOrDefault().AccountID%>' + '&orderNumber=' + '<%= Model.OrderNumber%>' + '&idSupport=' + '<%= ViewData["SupportTicketID"]%>&SupportTicketID=<%=ViewData["SupportTicketID"]%>';
                        }
                        else {
                            showMessage(results.message, true);
                        }
                    });
                }
            });

            if ($('#customers .OrderCustomer').length > 1) {
                $('#customers').accordion();
            }

            $('#changeCommissionConsultantModal').jqm({ trigger: '#btnShowChange',
                onShow: function (h) {
                    h.w.css({
                        top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                        left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                    }).fadeIn();
                },
                onHide: function (h) {
                    $('#txtConsultantSearch').val('<%: Model.ConsultantInfo.FullName + " (#" + Model.ConsultantInfo.AccountNumber + ")"%>').parent().find('.jsonSuggestResults').empty().hide();

                    h.w.css({
                        top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                        left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                    }).fadeOut(function () { h.o.remove(); });
                }
            });

            $('#changePartyOrderModal').jqm({ trigger: '#btnChangePartyId, #btnAttachToParty',
                onShow: function (h) {
                    h.w.css({
                        top: Math.floor(parseInt($(window).height() / 2)) - Math.floor(parseInt(h.w.height() / 2)) + 'px',
                        left: Math.floor(parseInt($(window).width() / 2)) + 'px'
                    }).fadeIn();
                }
            });

            $('a.ViewKitContents').live('click', function () {
                var t = $(this);
                if (t.hasClass('Minimize')) {
                    t.text('<%= Html.Term("ViewKitContents", "View Kit Contents") %>').removeClass('Minimize').next().slideUp('fast');
                } else {
                    t.text('<%= Html.Term("Close") %>').addClass('Minimize').next().slideDown('fast');
                }
            });

            $.ajax({
                type: 'POST',
                url: '/Orders/Details/ListWarehouseMaterialLacks',
                asyn: false,
                success: function (data) {
                    if (data.result == true) {
                        for (var i = 0; i < data.listWarehouseMaterialLacks.length; i++) {
                            $("#productLacks > tbody:first").append('<tr><td>' + data.listWarehouseMaterialLacks[i].ProductId + '</td>' +
                                                                    '<td>' + data.listWarehouseMaterialLacks[i].NameProduct + '</td>' +
                                                                    '<td>' + data.listWarehouseMaterialLacks[i].Quantity + '</td>' +
                                                                    '<td>' + data.listWarehouseMaterialLacks[i].Motive + '</td>' + '</tr>');
                        }
                    }
                }
            });
        });

        function showPaymentModal(id) {
            var options = {
                url: '<%= ResolveUrl("~/Orders/Details/GetPaymentInfo") %>',
                data: { paymentId: id },
                success: function (response) {
                    if (response.result) {
                        $('#paymentInfo').html(response.paymentHTML);
                        $('#paymentInfoModal').jqm({ modal: false }).jqmShow();
                    }
                    else {
                        showMessage(response.message, true);
                    }
                }
            };
            NS.get(options);
        }
    </script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%
                bool isReturnOrder = Model.OrderTypeID == Constants.OrderType.ReturnOrder.ToShort();
                bool isReplacementOrder = Model.OrderTypeID == Constants.OrderType.ReplacementOrder.ToShort();
                bool isPartyOrder = (Model.OrderTypeID == Constants.OrderType.PartyOrder.ToShort())
                    || (isReturnOrder && Model.OrderCustomers.Count > 1);
            %>
            <%= Html.Term("OrderDetails", "Order Details") %></h2>
        <%= Html.Term("Details", "Details") %>
        |
        <%  //Cancel
            if (NetSteps.Data.Entities.Business.Logic.OrderBusinessLogic.IsCancelPossible(Model.OrderTypeID, Model.OrderStatusID))
            {
        %>
        <a id="cancelOrder" href="javascript:void(0);">
            <%= Html.Term("CancelOrder", "Cancel Order") %></a> |
        <%
            }//Cancel Paid Order
            if (NetSteps.Data.Entities.Business.Logic.OrderBusinessLogic.IsCancelPaidOrderPossible(Model.OrderTypeID, Model.OrderStatusID))
            {
        %>
        <a id="cancelPaidOrder" href="javascript:void(0);">
            <%= Html.Term("CancelPaidOrder", "Cancel Paid Order") %></a> |
        <%
            }
            if (Model.IsEditable())
            {
                if (Model.OrderTypeID != (short)Constants.OrderType.PartyOrder)
                {
                    string editOrderUrl = "";
                    if (isReturnOrder)
                    {
                        editOrderUrl = ResolveUrl("~/Orders/Return/Edit") + "?orderNumber=" + Model.OrderNumber;
                    }
                    else if (isReplacementOrder)
                    {
                        editOrderUrl = ResolveUrl("~/Orders/Replacement/Edit") + "?orderNumber=" + Model.OrderNumber;
                    }
                    else
                    {
                        editOrderUrl = ResolveUrl("~/Orders/OrderEntry/Edit") + "?orderNumber=" + Model.OrderNumber;
                    } 
        %>
        <a id="editOrder" href="<%= editOrderUrl %>">
            <%= Html.Term("EditOrder", "Edit Order") %></a> |
        <%
                }
        %>
        <a id="recalculateOrder" href="<%= ResolveUrl("~/Orders/OrderEntry/RecalculateOrder") + "?orderNumber=" + Model.OrderNumber %>">
            <%= Html.Term("RecalculateOrder", "Recalculate Order")%></a> |
        <%
            }
            else if (Model.IsReturnable())
            {
        %><a id="returnOrder" href="javascript:void(0);" style="cursor: default; color: #696969;">
            <%= Html.Term("ReturnOrder", "Return Order") %></a> |
        <%
            }
            if (Model.OrderStatusID == (short)Constants.OrderStatus.Shipped)
            {
        %>
        <a id="replacementOrder" href="<%= ResolveUrl("~/Orders/Replacement") + "?baseOrderID=" + Model.OrderID + "&accountID=" + Model.OrderCustomers.FirstOrDefault().AccountID %>">
            <%= Html.Term("ReplacementOrder", "Replacement Order") %></a> |
        <%
            }
        %>
        <a id="auditHistory" href="<%= ResolveUrl("~/Orders/Details/AuditHistory") + "?orderNumber=" + Model.OrderNumber %>">
            <%= Html.Term("AuditHistory", "Audit History") %></a>
        <%
            if (!(Model.OrderStatusID == (short)Constants.OrderStatus.Pending || Model.OrderStatusID == (short)Constants.OrderStatus.PendingError || Model.OrderStatusID == (short)Constants.OrderStatus.Cancelled))
            {
        %>
        <%--modificacion realizada por salcedo vila G. GYS --%>
        <%-- @02 {--%>
        |<a id="printOrder" href="<%= ResolveUrl("~/Orders/Details/PrintInvoicePDF") + "?orderNumber=" + Model.OrderNumber %>">
            <%--}--%>
            <%= Html.Term("OrderSummary", "Order Summary")%></a>
        <%
            }
        %>
        <%--@01 A01--%>
        |<a id="tracking" href="<%= ResolveUrl("~/Orders/Details/Tracking") + "?orderNumber=" + Model.OrderNumber %>"><%= Html.Term("Tracking", "Tracking")%></a>
        <%--@02 A01--%>
        <%--@01 A01--%>
        |<a id="invoices" href="<%= ResolveUrl("~/Orders/Details/Invoices") + "?orderNumber=" + Model.OrderNumber %>"><%= Html.Term("Invoices", "Invoices")%></a>
        <%--@02 A01--%>
        <%
            if (Model.OrderStatusID == (short)Constants.OrderStatus.Delivered)
            {
        %>
            |<a id="claimItems" href="<%= ResolveUrl("~/Orders/Details/ClaimItems") + "?orderNumber=" + Model.OrderNumber %>"><%= Html.Term("ClaimItems", "Claim Items")%></a>
        <% } 
        %>        
    </div>
    <% 
        if (TempData["Error"] != null && !string.IsNullOrEmpty(TempData["Error"].ToString()))
        {
    %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
        -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
        border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
        margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="icon-exclamation">
            <%= TempData["Error"].ToString() %></div>
    </div>
    <%
        }

        if (TempData["PopupMessage"] != null && !string.IsNullOrEmpty(TempData["PopupMessage"].ToString()))
        {
    %>
    <script type="text/javascript">
        $(function () {
            alert('<%= TempData["PopupMessage"].ToString() %>');
        });
    </script>
    <%
        } 
    %>
    <input type="hidden" id="orderNumber" value="<%= Model.OrderNumber %>" />
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("Order", "Order") %>
            </td>
            <td>
                <div>
                    <% Html.RenderPartial("PartialOrderInformation", new PartialOrderInformationModel(Model, false)); %>
                </div>
            </td>
        </tr>
    </table>
    <div id="customers" style="min-width: 1200px;">
        <% foreach (nsCore.Areas.Orders.Models.Details.PartialOrderCustomerDetailsModel customerPartialDetailsModel in ViewBag.OrderCustomersPartialDetails)
           {
               //Use ViewBag to create and store PartialOrderCustomerDetailsModel and pass this to the partial instead of the OC
               Html.RenderPartial("PartialOrderCustomerDetails", customerPartialDetailsModel);
           }
        %>
    </div>
    <%
        if (isPartyOrder)
        {
            var partyOrderOrderShipments = Model.OrderShipments.Where(op => !op.OrderCustomerID.HasValue).ToList();
            if (partyOrderOrderShipments != null && partyOrderOrderShipments.Any())
            { 
    %>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("PartyAddress", "Party Address")%>
            </td>
            <td>
                <% 

                foreach (var shipment in partyOrderOrderShipments)
                {
                    if (shipment != null)
                    { 
                %>
                <%= shipment.ToDisplay()%>
                <%
                    }
                }
                %>
            </td>
        </tr>
    </table>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("ShipMethod", "Ship Method")%>
                <br />
                <br />
            </td>
            <td>
                <%
                if (partyOrderOrderShipments != null && partyOrderOrderShipments.Count > 0)
                {
                    foreach (var shipment in partyOrderOrderShipments)
                    {
                        if ((shipment.OrderShipmentPackages == null || shipment.OrderShipmentPackages.Count == 0) && shipment.ShippingMethodID.HasValue)
                        {
                %>
                <%= SmallCollectionCache.Instance.ShippingMethods.GetById(shipment.ShippingMethodID.Value).Name%><br />
                <%
                        }
                        else
                        {
                            foreach (var package in shipment.OrderShipmentPackages)
                            {
                %>
                <%= package.ShippingMethodName%><br />
                <span>
                    <%=Html.Term("ShippedOn", "Shipped On")%>:
                    <%= package.DateShipped.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)%></span>
                <%
                                if (!string.IsNullOrEmpty(package.TrackingNumber))
                                {
                                    var baseUrl = SmallCollectionCache.Instance.ShippingMethods.GetById(package.ShippingMethodID ?? shipment.ShippingMethodID ?? 0).TrackingNumberBaseUrl;
                                    if (!string.IsNullOrEmpty(baseUrl))
                                    {
                %>
                <br />
                <span>
                    <%=Html.Term("TrackingNumber", "Tracking#")%>: <a href="<%: string.Format(baseUrl, package.TrackingNumber)%>"
                        target="_blank" rel="external">
                        <%= package.TrackingNumber%></a></span>
                <%
                                    }
                                    else
                                    {
                %>
                <br />
                <span>
                    <%=Html.Term("TrackingNumber", "Tracking#")%>:
                    <%= package.TrackingNumber%></span>
                <%
                                    }
                                }
                            }
                        }
                    }
                } 
                %>
            </td>
        </tr>
    </table>
    <%
            }
            var partyOrderOrderPayments = Model.OrderPayments.Where(op => !op.OrderCustomerID.HasValue).ToList();
            if (partyOrderOrderPayments != null && partyOrderOrderPayments.Count > 0)
            {
    %>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("Payment", "Payment")%>
            </td>
            <td style="text-align: left;">
                <table class="DataGrid" width="100%">
                    <tr class="GridColHead">
                        <th>
                            <%= Html.Term("Payment", "Payment")%>
                        </th>
                        <th>
                            <%= Html.Term("Amount", "Amount")%>
                        </th>
                    </tr>
                    <tbody>
                        <% 
                if (partyOrderOrderPayments.Count == 0)
                {
                        %>
                        <tr>
                            <td colspan="3">
                                <%= Html.Term("NoPaymentsApplied", "No payments applied")%>
                            </td>
                        </tr>
                        <%
                }
                else
                {
                    foreach (var orderPayment in partyOrderOrderPayments)
                    {
                        string paymentColor = "black";
                        if (orderPayment.OrderPaymentStatusID == Constants.OrderPaymentStatus.Declined.ToInt())
                        {
                            paymentColor = "red";
                        }
                        else if (orderPayment.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToInt())
                        {
                            paymentColor = "green";
                        }
                        var descrip = PaymentMethods.GetTDescPaymentConfiguationByOrderPayment(orderPayment.OrderPaymentID);
                        descrip = (descrip == "PREVIOS BALANCE") ? "Product Credit" : descrip;
                        %>
                        <tr>
                            <td>
                                <a href="javascript:void(0);" title="View payment details" onclick="showPaymentModal(<%= orderPayment.OrderPaymentID %>);">
                               
                                     <%=descrip%></a>&nbsp;<span style="color: <%= paymentColor %>"><i>(<%=SmallCollectionCache.Instance.OrderPaymentStatuses.GetById(orderPayment.OrderPaymentStatusID).GetTerm()%>)</i></span>
                            </td>
                            <td>
                                <%= orderPayment.Amount.ToMoneyString()%>
                            </td>
                        </tr>
                        <%
                    }
                } 
                        %>
                    </tbody>
                </table>
            </td>
        </tr>
    </table>
    <% 
            }
    %>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("PartyTotals", "Party Totals")%>
            </td>
            <td style="text-align: left;">
                <table class="DataGrid" width="100%">
                    <tr class="GridTotalBar">
                        <td colspan="1" style="text-align: right;">
                            <%= Html.Term("Subtotal", "Subtotal")%>:<br />

                            <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%string valorSCV = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
                                if (valorSCV == "S")
                                { %>
                            <%= Html.Term("CommissionableTotal", "Commissionable Total")%>:<br />
                             <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>


                            <%= Html.Term("Tax", "Tax")%>:<br />
                            <%= Html.Term("S&H Total", "S&amp;H Total")%>:<br />
                            <%= Html.Term("OrderTotal", "Order Total")%>:<br />
                        </td>
                        <td>
                            <%= Model.Subtotal.ToDecimal().ToString(Model.CurrencyID)%><br />

                             <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%if (valorSCV == "S"){%>
                            <%= Model.CommissionableTotal.ToDecimal().ToString(Model.CurrencyID)%><br />
                            			 <%}%>
                <%--CS.03MAY2016.Fin.Muestra CV--%>
                  

                            <%= Model.TaxAmountTotal.ToDecimal().ToString(Model.CurrencyID)%><br />
                            <%= Model.ShippingTotal.ToDecimal().ToString(Model.CurrencyID)%><br />
                            <b>
                                <%= Model.GrandTotal.ToDecimal().ToString(Model.CurrencyID)%></b><br />
                        </td>
                    </tr>
                    <tr style="font-size: 1.2em;">
                        <td colspan="1" style="text-align: right">
                            <%= Html.Term("PaymentsApplied", "Payments Applied")%>:<br />
                            <%= Html.Term("BalanceDue", "Balance Due")%>:
                        </td>
                        <td>
                            <b>
                            <%
                             
            var paymentsMade = Model.OrderPayments.Where(y => y.Amount > 0).Sum(x => x.Amount);
                               
                            %>
                                <%= paymentsMade.ToString(Model.CurrencyID)%></b><br />
                            <b><span style="color: <%= (Model.Balance ?? 0) > 0 ? "Red" : "Green" %>;"> 
                                <%= Model.Balance.ToString(Model.CurrencyID)%></span></b>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%
        }
    %>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("InvoiceNotes", "Invoice Notes")%>
            </td>
            <td>
                <%= Model.InvoiceNotes ?? "" %>
            </td>
        </tr>
    </table>
    <% 
        if (isReturnOrder)
        {
    %>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("ReasonCodes", "Reason Codes")%>
            </td>
            <td>
                <%= Html.Term("ReturnType", "Return Type")%>:
                <% 
            if (Model.ReturnTypeID.ToInt() > 0)
            {
                %>
                <%= SmallCollectionCache.Instance.ReturnTypes.GetById(Model.ReturnTypeID.ToInt()).GetTerm() %><br />
                <% 
            }
                %>
            </td>
        </tr>
    </table>
    <% 
        }
    %>
    <div id="paymentInfoModal" class="LModal jqmWindow">
        <div class="mContent">
            <h2>
                <%= Html.Term("BillingInformation", "Billing Information")%></h2>
            <table id="paymentInfo" class="DataGrid" width="100%">
            </table>
            <p>
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Close", "Close")%></a></p>
        </div>
    </div>
       <iframe   name ="frmExportar" id="frmExportar" style="display:none" src="">
        </iframe>
</asp:Content>
<asp:Content ContentPlaceHolderID="RightContent" runat="server">
    <td class="CoreRightColumn">SS
        <% 
            ViewData["ParentIdentity"] = Model.OrderNumber;
            Html.RenderPartial("Notes");
        %>
    </td>
</asp:Content>
