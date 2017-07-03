<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/OrderRules/OrderRules.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .style1
        {
            width: 60%;
        }
    </style>
    
    <script type="text/javascript">

        $(document).ready(function() {
            
      $('input[monedaidioma=CultureIPN]').keyup(function (event) {

		        var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
		        // var value = parseFloat($(this).val());


		        var formatDecimal = '$1.$2'; // valores por defaul 
		        var formatMiles = ",";  // valores por defaul

		        if (cultureInfo === 'en-US') {
		             formatDecimal = '$1.$2';
		             formatMiles = ",";
		        }
		        else if (cultureInfo === 'es-US') {
		             formatDecimal = '$1,$2';
		             formatMiles = ".";
		        }
		        else if (cultureInfo === 'pt-BR') {
                     formatDecimal = '$1,$2';
		             formatMiles = ".";
		        }


		        //            if (!isNaN(value)) {
		        if (event.which >= 37 && event.which <= 40) {
		            event.preventDefault();
		        }

		        $(this).val(function (index, value) {


		            return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
		        });

		        //            }

		    });

            $("#MaxOrderPerDay").focus();

            if (<%= ViewBag.HasOrders.ToString().ToLower() %> == true) {
                
                    $.post('<%= @Url.Action("GetOrderRulesConfiguration", "OrderRules") %>',
                        null,
                        function (json) {
                            for (var i = 0; i < json.length; i++) {
                                var obj = json[i];

                                console.log(obj.TermName);
                                
                                // variables for iteration
                                var list = null;
                                var qty = 0;
                                    
                                switch (obj.TermName.toString()) {
                                  case "MaxOrderPerDay":
                                        for (var x = 0; x < obj.List.length; x++) {
                                            list = obj.List[x];

                                            $.each(list, function (k, v) {
                                                    
                                                if (v.Key == 'OrderStatusID') {
                                                    var value = v.Value;
                                                    $('input[name=orderStatusList][value =' + value + ']').prop("checked", "checked");
                                                } else if (v.Key == 'QuantityMax') {
                                                    qty = v.Value;
                                                }
                                            });
                                        }

                                      $("#MaxOrderPerDay").val(qty);
                                      break;
                                    case "MaxOrderWithoutPayment":
                                        for (var y = 0; y < obj.List.length; y++) {
                                            list = obj.List[y];

                                            $.each(list, function (k, v) {
                                                if (v.Key == 'OrderStatusID') {
                                                    var value = v.Value;
                                                    $('input[name=orderStatusPaymentList][value =' + value + ']').prop("checked", "checked");
                                                } else if (v.Key == 'QuantityMax') {
                                                    qty = v.Value;
                                                }
                                            });
                                        }

                                        $("#MaxOrderWithoutPayment").val(qty);
                                        break;
                                    case "MaxOfTicketsPayment":
                                        for (var z = 0; z < obj.List.length; z++) {
                                            list = obj.List[z];
                                            
                                            $.each(list, function (k, v) {
                                                if (v.Key == 'ExpirationStatusID') {
                                                    var value = v.Value;
                                                    $('input[name=ticketPaymentStatusList][value =' + value + ']').prop("checked", "checked");
                                                } else if (v.Key == 'QuantityMax') {
                                                    qty = v.Value;
                                                }
                                            });
                                        }

                                        $("#MaxOfTicketsPayment").val(qty);
                                        break;
                                    case "MaxOfTicketsPaymentNegotied":
                                        for (var w = 0; w < obj.List.length; w++) {
                                            list = obj.List[w];

                                            $.each(list, function (k, v) {
                                                if (v.Key == 'NegotiationLevelID') {
                                                    var value = v.Value;
                                                    $('input[name=negotiationLevelList][value =' + value + ']').prop("checked", "checked");
                                                } else if (v.Key == 'QuantityMax') {
                                                    qty = v.Value;
                                                }
                                            });
                                        }

                                        $("#MaxOfTicketsPaymentNegotied").val(qty);
                                        break;
                                    default:
                                }
                            }
                        })
                    .fail(function () {
                        showMessage('@Html.Term("ErrorSavingUser", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
                    })
                    .always(function () {
                       
                    });
            }

            $("#btnSave").click(function() {

                var orderStatusList = [];
                var orderPaymentStatusList = [];
                var ticketPaymentStatusList = [];
                var negotiationLevelList = [];

                $.each($("input[name='orderStatusList']:checked"), function() {
                    orderStatusList.push($(this).val());
                });

                $.each($("input[name='orderStatusPaymentList']:checked"), function() {
                    orderPaymentStatusList.push($(this).val());
                });

                $.each($("input[name='ticketPaymentStatusList']:checked"), function() {
                    ticketPaymentStatusList.push($(this).val());
                });

                $.each($("input[name='negotiationLevelList']:checked"), function() {
                    negotiationLevelList.push($(this).val());
                });


                var orderPerDay =
                {
                    Name: "MaxOrderPerDay",
                    quantity: $("#MaxOrderPerDay").val(),
                    List: orderStatusList
                }

                var orderWithoutPayment =
                {
                    Name: "MaxOrderWithoutPayment",
                    quantity: $("#MaxOrderWithoutPayment").val(),
                    List: orderPaymentStatusList
                }

                var ticketPayment =
                {
                    Name: "MaxOfTicketsPayment",
                    quantity: $("#MaxOfTicketsPayment").val(),
                    List: ticketPaymentStatusList
                }

                var ticketNegotiation =
                {
                    Name: "MaxOfTicketsPaymentNegotied",
                    quantity: $("#MaxOfTicketsPaymentNegotied").val(),
                    List: negotiationLevelList
                }

                var orderRules =
                {
                    orderPerDay: orderPerDay,
                    orderWithoutPayment: orderWithoutPayment,
                    ticketPayment: ticketPayment,
                    ticketNegotiation: ticketNegotiation
                }

                var json = {
                    data: JSON.stringify(orderRules)
                };

                var t = $(this);
                showLoading(t);

                $.post('<%= @Url.Action("Save", "OrderRules") %>',
                        json,
                        function (response) {
                            if (response == 'Success') {
                                showMessage('Order rules was saved successfully!', null);
                            } else {
                                console.log(response);
                                showMessage('@Html.Term("ErrorSavingUser", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
                            }
                        })
                    .fail(function() {
                        showMessage('@Html.Term("ErrorSavingUser", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
                    })
                    .always(function() {
                        hideLoading(t);
                    });
            });
        })

    </script>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("OrderPreconditions", "Order-PreCondition")%>
		</h2>
	</div>

    <table class="style1">
        <tr>
            <td>
                <%= Html.Term("MaxOrderPerDay", "Máx order per day")%>
            </td>
            <td>
                <input type="text" name="MaxOrderPerDay" value="" id="MaxOrderPerDay" />
            </td>
            <td>
                <%= Html.Term("OrderStatuses", "Order Statuses")%>
            </td>
            <td>
                <%foreach (var item in ViewBag.orderStatusListPerDay)
                  {
                      string termName = item.TermName;
                      string name = item.Name;      
                %>
                        <input type="checkbox" name="orderStatusList" value="<%= item.OrderStatusID %>" id="<%= item.TermName %>" />
                        <label for="<%= item.TermName %>">
                            <%= Html.Term(termName, name)%>
                        </label>
                        <br/>
                <%} %>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("MaxOrderWithoutPayment", "Máx. of orders without payment")%></td>
            <td>
                <input type="text" name="MaxOrderWithoutPayment" value="" id="MaxOrderWithoutPayment" /></td>
            <td>
                <%= Html.Term("OrderStatuses", "Order Statuses")%>
            </td>
            <td>
                <%foreach (var item in ViewBag.orderStatusWithoutPayment)
                  {
                      string termName = item.TermName;
                      string name = item.Name;      
                %>
                        <input type="checkbox" name="orderStatusPaymentList" value="<%= item.OrderStatusID %>" id="<%= item.TermName %>" />
                        <label for="<%= item.TermName %>">
                            <%= Html.Term(termName, name)%>
                        </label>
                        <br/>
                <%} %>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("MaxOfTicketsPayment", "Máx. of tickets payment")%></td>
            <td>
                <input type="text" name="MaxOfTicketsPayment" id="MaxOfTicketsPayment" value=" "  monedaidioma='CultureIPN'/></td>
            <td> 
                <%= Html.Term("TicketPaymentStatus", "Ticket Payment Status")%> </td>
            <td>
                <%foreach (var item in ViewBag.expirationStatuses)
                  {
                      string termName = item.TermName;
                      string name = item.Name;      
                %>
                        <input type="checkbox" name="ticketPaymentStatusList" value="<%= item.ExpirationStatusID %>" id="<%= item.TermName %>" />
                        <label for="<%= item.TermName %>">
                            <%= Html.Term(termName, name)%>
                        </label>
                        <br/>
                <%} %>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("MaxOfTicketsPaymentNegotied", "Máx. of tickets payment negotied")%>
            </td>
            <td>
                <input type="text" name="MaxOfTicketsPaymentNegotied" id="MaxOfTicketsPaymentNegotied" value=" "  monedaidioma='CultureIPN' /></td>
            <td>
                <%= Html.Term("NegotiationLevel", "Negotiation Level")%>
            </td>
            <td>
                <%foreach (var item in ViewBag.negotiationLevels)
                  {
                      string termName = item.TermName;
                      string name = item.Name;      
                %>
                        <input type="checkbox" name="negotiationLevelList" value="<%= item.NegotiationLevelID %>" id="<%= item.TermName %>" />
                        <label for="<%= item.TermName %>">
                            <%= Html.Term(termName, name)%>
                        </label>
                        <br/>
                <%} %>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
            <td>
                &nbsp;</td>
        </tr>
        <tr>
            <td colspan="4" style="text-align:center">
                <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
			        <%= Html.Term("Save") %></a> <a href="<%= ResolveUrl("~/Admin/") %>" class="Button">
                    <span><%= Html.Term("Cancel") %></span>
                </a>
            </td>
        </tr>
    </table>
    <p>&nbsp;</p>

</asp:Content>