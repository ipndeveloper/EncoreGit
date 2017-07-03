<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master"
                                Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Order>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Orders") %>">
		<%= Html.Term("Orders", "Orders") %></a> >
        <a href="<%= ResolveUrl("~/Orders/Details/Index/" + Model.OrderID.ToString()) %>">
	    <%= Html.Term("OrderDetail", "Order Detail") %></a> >
        <%= Html.Term("ClaimItems", "Claim Items") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
<style> 
    [class^=childItem] 
    {
        display: none; 
        background-color: #DCDCDC;
    }
 </style>
  

    <div class="SectionHeader">
        <h2><%= Html.Term("ClaimItems", "Claim Items")%></h2>
    </div>
    <br />

    <div id="formToClaim">
        
        <table>
        <%Func<int, string> MostrarOcultar = (obj) =>
          {
              string estilo = (obj == 0) ? "style='display:none'" : "";
              return estilo;
          }; %>
            <tr <%=MostrarOcultar(Convert.ToInt32( ViewBag.supportTicket)) %>>
                <td style="width: 300px"><%= Html.Term("NroTicketSupport", "N° Ticket Support") %></td>
                <td>
                    <input type="text" id="txtTicket" value='<%= ViewBag.supportTicket%>' readonly="readonly" />
                    <span id="message" style="color: Red;"></span>
                </td>
            </tr>
            <tr>
                <td colspan="2"><hr /></td>
            </tr>
            <tr>
                <td><%= Html.Term("Products") %></td>
                <td> 
                    <table width="100%" class="DataGrid" id="products">
							<thead>
								<tr class="GridColHead">
									<th>
										<%= Html.Term("ClaimItem", "Claim Item")%>
									</th>
							        <th>
								        <%= Html.Term("Return Reason", "Return Reason") %>
							        </th>                                    
							        <th>
								        <%= Html.Term("Received Item", "Received Item") %> 
							        </th>
									<th>
										<%= Html.Term("CUV", "CUV")%>
									</th>
									<th>
										<%= Html.Term("ProductName", "Product Name")%>
									</th>
									<th>
										<%= Html.Term("ProductType", "Product Type")%>
									</th>
									<th>
										<%= Html.Term("OrderQuantity", "Order Quantity")%>
									</th> 
								</tr>
							</thead>
							<tbody>
                                <tr>
                                    <td colspan="8">
                                        <span style="font-size: 15px; color: Gray;">
                                            <b><%= Model.ConsultantInfo.FullName %></b>
                                        </span>
                                    </td>
                                </tr>

                                 
                                <%foreach (OrderItem item in Model.OrderCustomers[0].ParentOrderItems)
                                  {%>
                                    <tr class="parentItem"> 
                                        <td>
                                        <input type="hidden" class="productID" value="<%=item.ProductID%>" />
								<input type="hidden" class="orderItemId" value="<%=item.OrderItemID%>" />
								<input type="hidden" class="orderCustomerId" value="<%=item.OrderCustomerID%>" />
								 
                                              <input type="checkbox" class="returned"  data-parentid="<%= item.OrderItemID %>" />
                                        </td>
                                     
                                           <td>
                                          <%=Html.DropDownList("sReturnReasons",(TempData["sReturnReasons"] as IEnumerable<SelectListItem>))%> 
                                            <%if (item.ChildOrderItems.Count>0)
                                              {%>
                                                  <span class="ClearAll"></span><a class="ViewKitContents TextLink Add" href="javascript:void(0);">
								    <%= Html.Term("ViewKitContents", "View Kit Contents")%></a>  
                                              <%} %>
                                           </td>
                                              <td> 
                                            <input type="checkbox" class="hasBeenReceived" disabled="disabled" />
                                        </td>
                                        <td>
                                            <%=item.SKU%>
                                        </td>         
                                        <td>
                                            <%=item.ProductName%>
                                        </td> 
                                        <td class="quantity">
                                            <%=item.Quantity%>
                                        </td>               
                                        <td>
                                            <input class="numeric" type="text"  value="<%=item.Quantity%>" />
                                        </td>                                
                                    </tr>  
                                    <%foreach (OrderItem oChild in item.ChildOrderItems)
                                      {
                                         // var orderItemPrice = item.OrderItemPrices.Where(op => op.ProductPriceTypeID == Model.OrderCustomers[0].CommissionablePriceTypeID).FirstOrDefault();
                                          
                                          %>
                                        <tr class="childItem<%=oChild.OrderItemID%> ">
                                         <td>

                                         
                                <input type="hidden" class="productID" value="<%=oChild.ProductID%>" />
								<input type="hidden" class="orderItemId" value="<%=oChild.OrderItemID%>" />
								<input type="hidden" class="orderCustomerId" value="<%=oChild.OrderCustomerID%>" />
								<input type="checkbox" class="returned" disabled="disabled" data-parentid="<%= item.OrderItemID %>" />
                                 
                                        </td>    
                                           <td>
                                          <%=Html.DropDownList("sReturnReasons",(TempData["sReturnReasons"] as IEnumerable<SelectListItem>))%>  
                                           </td>                                    
                                        <td>
                                            <input type="checkbox" class="hasBeenReceived" disabled="disabled" />
                                        </td>
                                        <td>
                                            <%=oChild.SKU%>
                                        </td>         
                                        <td>
                                            <%=oChild.ProductName%>
                                        </td> 
                                        <td class="quantity">
                                            <%=oChild.Quantity%>
                                        </td>               
                                        <td class="returnQuantity">
                                            <input class="numeric" type="text" disabled="disabled" value="<%=item.Quantity%>" />
                                        </td>   
                                        </tr>  
                                      <%} %>
                                  <%} %>
                                <tr>
                                    <td colspan="8"><div style="background: #C0C0C0; height: 4px;"></div></td>
                                </tr>
							</tbody>
						</table>
                </td>
            </tr>
            <tr>
                <td colspan="2"><hr /></td>
            </tr>
            <tr>
            </tr>
            <tr>
            <td class="FLabel">
				<%= Html.Term("Notes", "Notes") %>
			</td>
			<td>
				<p class="FL">
					<%= Html.Term("ReturnType", "Return Type") %>:<br />
					<select id="sReturnType" class="TextInput">
						<% foreach (var returnType in SmallCollectionCache.Instance.ReturnTypes)
						   { %>
						<option value="<%= returnType.ReturnTypeID %>" <%= Model.ReturnTypeID == returnType.ReturnTypeID ? "selected=\"selected\"" : "" %>>
							<%= returnType.GetTerm() %></option>
						<%} %>T
					</select>
				</p>
                <span class="ClearAll"></span>
				<p class="FL">
					<%= Html.Term("InternalNotes", "Internal Notes") %>:<br />
					<textarea id="notes" class="TextInput" rows="5" cols="50"></textarea>
				</p>
                </td>                
            </tr>
            <tr>
            <td></td>
             <td>
                    *<%= Html.Term("Note")%>: <%= Html.Term("ProductsWontBeCharged", "The claimed products won't represent payment by the account") %>.
                    <br />
                    <br />
                    
                   <p class="NextSection">
						<a id="btnSubmitClaim" class="Button BigBlue SubmitOrder ButtonOff" href="javascript:void(0);">
							<span>
								<%= Html.Term("SubmitClaim", "Submit Claim")%>
								>></span></a>

                        <a id="btnPendingConfirm" class="Button BigBlue SubmitOrder ButtonOff" href="javascript:void(0);">
							<span>
								<%= Html.Term("PendingConfirm", "Pending Confirm") %>
								>></span></a>
					</p>

                </td>
            </tr>
        </table>

    </div>

</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="RightContent" runat="server">
	
     <%
        List<dynamic> OrderItems = new List<dynamic>();
         
        foreach (OrderCustomer customer in Model.OrderCustomers)
        {
            foreach (OrderItem orderItem in customer.ParentOrderItems)
            {

                NetSteps.Data.Entities.Dto.ReturnOrderItemDto parent = new NetSteps.Data.Entities.Dto.ReturnOrderItemDto();
                parent.OrderItemID = orderItem.OrderItemID;
                parent.ParentOrderItemID = orderItem.ParentOrderItemID;
                OrderItems.Add(parent);   
                
                foreach (OrderItem childItem in orderItem.ChildOrderItems)
                {
                    NetSteps.Data.Entities.Dto.ReturnOrderItemDto child = new NetSteps.Data.Entities.Dto.ReturnOrderItemDto();
                    child.OrderItemID = childItem.OrderItemID;
                    child.ParentOrderItemID = childItem.ParentOrderItemID;
                    OrderItems.Add(child);           
                 }
            }
        }
        string asd = "arr";
    %>
    

    <input type="hidden" id="hdfCultureInfo" value="" />

    <script type="text/javascript">

        $(document).ready(function () {
            validateEnableSubmit();

            $('.returned').click(function () {
                checkIfReturned.apply($(this).closest('tr').get(0));
                //calculateProductCredit();
                validateHeaderCheck();
                validateKitItemsCheck($(this).closest('tr'));
            });

            function validateHeaderCheck() {
                var returnIsChecked = true;
                var receivedIsChecked = true;

                $('.returned').each(function () {
                    if (!$(this).prop('checked')) {
                        returnIsChecked = false;
                    }
                });
                $('.hasBeenReceived').each(function () {
                    if (!$(this).prop('checked')) {
                        receivedIsChecked = false;
                    }
                });

                //$('#ckbReturnItemHead').prop('checked', returnIsChecked);
                //$('#ckbReceivedItemHead').prop('checked', receivedIsChecked);
            }
            function validateKitItemsCheck(row) {
                var isReturned = row.find('.returned').prop('checked');

                if (row.attr('class') != undefined) {
                    var isChild = row.attr('class').indexOf('childItem') != -1 ? true : false;
                    var productID = row.find('input.productID').val();
                    //alert()
                    var checkAll = true;

                    var kitRows = $('#products tbody tr').filter(function () {
                        if ($(this).find('input.productID').val() == productID)
                            return $(this);
                    });

                    if (isChild) {
                        kitRows.filter('[class^=childItem]').find('input.returned').each(function () {
                            checkAll = $(this).prop('checked');
                            if (!checkAll)
                                return false;
                        });

                        var rowParent = kitRows.filter('[class^=parentItem]');

                        if (checkAll) {
                            rowParent.find('.returned').prop('checked', checkAll);
                            rowParent.find('.hasBeenReceived').prop('disabled', false);
                            calculateLineTotal(rowParent);

                            kitRows.filter('[class^=childItem]').each(function () {
                                var currentRow = $(this);
                                currentRow.find('.returned, .hasBeenReceived, .restockable').prop('checked', false);
                                currentRow.find('.hasBeenReceived').prop('disabled', true);
                                currentRow.find('.lineTotal').text('$0.00');
                                currentRow.find('.returnQuantity').val(currentRow.find('.quantity').text().trim());
                            });
                        }
                        else {

                            rowParent.find('.returned, .hasBeenReceived').prop('checked', false);
                            rowParent.find('.hasBeenReceived').prop('disabled', true);
                            rowParent.find('.lineTotal').text('$0.00');

                            var quantity = row.find('.returnQuantity').val();
                            var pricePerItem = NoCurrency(row.find('.pricePerItem').text());
                            var itemTotal = (quantity * pricePerItem);

                            if (isReturned) {
                                row.find('.lineTotal').text('$' + itemTotal);
                            }

                            row.find('.hasBeenReceived').prop('checked', false).prop('disabled', !isReturned);
                        }
                    }
                    else {
                        row.find('.hasBeenReceived').prop('checked', false).prop('disabled', !isReturned);
                        row.find('.lineTotal').text('$0.00');

                        //var quantity = $('.quantity', this).text();
                        var quantityParent = row.find('.quantity').text();
                        var pricePerItemParent = NoCurrency(row.find('.pricePerItem').text());
                        var itemTotalParent = (quantityParent * pricePerItemParent);

                        if (isReturned) {
                            row.find('.lineTotal').text('$' + itemTotalParent);
                            //alert(itemTotalParent);
                        }

                        kitRows.filter('[class^=childItem]').each(function () {
                            var currentRow = $(this);
                            currentRow.find('.returned, .hasBeenReceived, .restockable').prop('checked', false);
                            currentRow.find('.hasBeenReceived').prop('disabled', true);
                            currentRow.find('.lineTotal').text('$0.00');
                            currentRow.find('.returnQuantity').val(currentRow.find('.quantity').text().trim());
                        });
                    }
                }
                else {
                    row.find('.hasBeenReceived').prop('checked', false).prop('disabled', !isReturned);
                }

                var ItemRowsCount = $('#products tbody: tr').length - 2;

                var checkHeader = true;

                $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).each(function () {
                    var rowClass = $(this).attr('class') == undefined ? '' : $(this).attr('class');
                    var isChild = rowClass.indexOf('childItem') != -1 ? true : false;

                    if (!isChild) {
                        checkHeader = $(this).find('.returned').prop('checked');

                        if (!checkHeader)
                            return false;
                    }

                });

                $('#ckbReturnItemHead').prop('checked', checkHeader);

                //calculateProductCredit();
                validateEnableSubmit();
            }

            function NoCurrency(value) {
                var result = value;
                result = result.replace('S/.', '');
                result = result.replace('€', '');
                result = result.replace('$', '');
                result = result.replace(',', '.');
                return result.trim();
            }

            function checkIfReturned() {
                var checkBox = $('.returned', this);
                if (checkBox.prop('checked')) {
                    if (!checkBox.closest('tr').hasClass("childItem")) {
                        var quantity = $('.quantity', this).text();
                        var pricePerItem = NoCurrency($('.pricePerItem', this).text()); //$('.pricePerItem', this).text().replace(noCurrency, '');
                        //alert('quantity-->' +  quantity + 'pricePerItem-->' + pricePerItem);
                        var total = (parseInt(quantity) * parseFloat(pricePerItem)).toFixed(2);
                        $('.lineTotal', this).text('$' + total);
                    } else {

                        // $('.lineTotal', this).text('$' + total.toFixed(2));
                    }
                } else {

                    $('.lineTotal', this).text('$0.00');
                }
            }

            function calculateProductCredit() {
                var restockingSubtotal = 0;
                $('#products tbody:first tr').each(function () {
                    if ($('.returned', this).prop('checked')) {
                        var quantity = parseFloat($('.returnQuantity', this).val());
                        var itemPrice = parseFloat(NoCurrency($('.pricePerItem', this).text())); //parseFloat($('.pricePerItem', this).text().replace(noCurrency, ''));

                        restockingSubtotal += (itemPrice * quantity);
                    }
                });

                //$('#amountCredit').html('$' + restockingSubtotal.toFixed(2));
            }

            LoadItems();

            $("#txtTicket").keypress(function () {
                if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                    event.preventDefault();
                }
            });

            //            $("#btnSubmitClaim").click(function () {
            //                SubmitItems();
            //            });


            function GetOrderItems() {

                var OrderItems = JSON.parse('<%= Newtonsoft.Json.JsonConvert.SerializeObject(OrderItems) %>');
                var ItemRowsCount = $('#products tbody: tr').length - 2;
                var OrderItemList = [];
                //alert(ItemRowsCount);
                var OrderStatusID = '<%= Model.OrderStatusID %>';

                /*
                * 8 Printed
                * 9 Shipped
                * 14    Cancelled Paid
                * 20    Invoiced
                */

                if (OrderStatusID != 8 || OrderStatusID != 9 || OrderStatusID != 14 || OrderStatusID != 20 || OrderStatusID != 19) {
                    $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).each(function () {

                        var isChecked = $(this).find('.returned').prop('checked');

                        if (isChecked) {
                            var rowClass = $(this).attr('class') == undefined ? '' : $(this).attr('class');
                            var isChild = rowClass.indexOf('childItem') != -1 ? true : false;

                            var OrderItemID = $(this).find('.orderItemId').val();
                            var Quantity = $('.quantity', this).text();

                            if (isChild) {
                                var ParentOrderItemID = rowClass.replace('childItem', '').trim();

                                for (var i = 0; i < OrderItems.length; i++) {
                                    var orderItem = OrderItems[i];

                                    if (orderItem.OrderItemID == ParentOrderItemID) {
                                        orderItem.ItemPrice = 0.00;
                                    }
                                    else if (orderItem.OrderItemID == OrderItemID) {
                                        orderItem.ItemPrice = NoCurrency($('.pricePerItem', this).text());
                                    }
                                    else {
                                        continue;
                                    }
                                    orderItem.Quantity = Quantity;
                                    OrderItemList.push(orderItem);
                                }
                            }
                            else {
                                for (var i = 0; i < OrderItems.length; i++) {
                                    var orderItem = OrderItems[i];

                                    if (orderItem.OrderItemID == OrderItemID) {
                                        orderItem.ItemPrice = NoCurrency($('.pricePerItem', this).text());
                                    }
                                    else if (orderItem.ParentOrderItemID == OrderItemID) {
                                        orderItem.ItemPrice = 0.00;
                                    }
                                    else {
                                        continue;
                                    }
                                    orderItem.Quantity = Quantity;
                                    OrderItemList.push(orderItem);
                                }
                            }

                        }

                    });
                }

                return OrderItemList;
            }
            $('#btnPendingConfirm').click(function () {
                if (!$(this).hasClass('ButtonOff')) {
                    //$('#btnUpdate').click();

                    var data = {
                        originalOrderId: '<%= Model.OrderID %>',
                        refundOriginalPayments: false, //$('#originalPayment').prop('checked'),
                        returnType: $('#sReturnType').val(),
                        invoiceNotes: $('#notes').val(),
                        creditAmount: NoCurrency($('#amountCredit').text()), //$('#amountCredit').text().replace(noCurrency, ''),
                        creditType: true
                    };

                    var OrderItemList = GetOrderItems();

                    $.each(OrderItemList, function (index, item) {
                        data['OrderItemList[' + index + '].ParentOrderItemID'] = item.ParentOrderItemID;
                        data['OrderItemList[' + index + '].OrderItemID'] = item.OrderItemID;
                        data['OrderItemList[' + index + '].Quantity'] = item.Quantity;
                        data['OrderItemList[' + index + '].ItemPrice'] = item.ItemPrice;
                        //alert('ParentOrderItemID --> ' + item.ParentOrderItemID + 'OrderItemID --> ' + item.OrderItemID + 'Quantity --> ' + item.Quantity + 'ItemPrice --> ' + item.ItemPrice)
                    });


                    if (data.refundOriginalPayments) {
                        var i = 0;
                        $('#originalPayments .paymentRefund').each(function () {
                            data['refundPayments[' + i + '].Key'] = $(this).attr('id').replace(/\D/g, '');
                            data['refundPayments[' + i + '].Value'] = $(this).val();
                            ++i;
                        });
                    }
                    var grandTotal = NoCurrency($('.returnGrandTotal').text()); //$('.returnGrandTotal').text().replace(noCurrency, '');
                    //					if (grandTotal == 0) {
                    //						showMessage('Please click the update button to save the changes to the order.', true);
                    //						return false;
                    //					}
                    $('.WaitWin').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' }).jqmShow();

                    $.post('<%= ResolveUrl("~/Orders/Details/ClaimItemsByOrderNumber") %>', data, function (response) {
                        if (response.result) { 
                            if (response.result) { 
                                window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.orderNumber;
                            }
                            // window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.returnOrderNumber;
                        } else {
                            $('.WaitWin').jqmHide();
                            showMessage(response.message, true);
                        }
                    });
                }
            });

        });


        function LoadItems() {

            $.ajax({
                url: '<%= Url.Action("GetItemsToClaim", "Details")%>',
                type: 'GET',
                contentType: 'application/json; charset=utf-8',
                data: { orderNumber: '<%= Model.OrderNumber %>' },
                success: function (result) {
                    $("#hdfCultureInfo").val(result.cultureInfo);
                    //$("#itemsToClaimGrid > tbody").append(result.rows);
                    AssignNumericEvent();
                },
                error: function (jqXHR, textStatus) {
                    console.log(jqXHR);
                    console.log(textStatus);
                }
            });
        }

        $('a.ViewKitContents').live('click', function () {
            var link = $(this);
            var show = false;

            if (link.hasClass('Minimize')) {
                link.text('<%= Html.Term("ViewKitContents", "View Kit Contents") %>').removeClass('Minimize').next().slideUp('fast');
            } else {
                link.text('<%= Html.Term("Close") %>').addClass('Minimize').next().slideDown('fast');
                show = true;
            }

            var productID = link.closest('tr').find('input.productID').val();

            var rows = $('#products tbody tr[class^=childItem]');

            rows.each(function () {

                var childRow = $(this);
                var productIDChild = childRow.find('input.productID').val();

                if (productID == productIDChild) {

                    if (show) {
                        childRow.show();
                    }
                    else {
                        childRow.hide();
                    }
                }
            });
        });


        function AssignNumericEvent() {

            $("input[type=checkbox]").each(function () {

                $(this).click(function () {
                    var input = $(this).closest("tr").find(".numeric");

                    if ($(this).is(':checked')) {
                        $(input).removeProp("disabled");
                    } else {
                        $(input).val("0");
                        $(input).prop("disabled", "disabled");
                    }
                });
            });

            $('.hasBeenReceived').click(function () {
                validateEnableSubmit();
                //validateHeaderCheck();
            });

            $("input.numeric").each(function () {

                $(this).keypress(function () {
                    if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                        event.preventDefault();
                    }
                });

                $(this).keyup(function () {
                    var value = $(this).val();
                    if (value != "") {
                        if (!isNaN(value)) {
                            var limit = $(this).closest("td").prev("td").text();
                            if (Number(value) > Number(limit)) {
                                $(this).val(limit);
                                value = $(this).val();
                            }
                        }
                    }

                    var currencyCode = $("#hdfCultureInfo").val().split("|")[0];
                    var cultureInfo = $("#hdfCultureInfo").val().split("|")[1];

                    var nextCell = $(this).closest("td").next("td");
                    var qtyCell = $(nextCell).text();
                    var quantity = Number(qtyCell.substring(qtyCell.indexOf(qtyCell.match(/[0-9]/))));
                    $(nextCell).next("td").text((Number(value) * quantity).toLocaleString(cultureInfo,
                                                                        { style: "currency", currency: currencyCode, minimumFractionDigits: 2 }));

                    //$(this).val(Number(value));
                });
            });
        }

        function validateEnableSubmit() {
            //$('#btnUpdate').click();
            $('#btnUpdateShipping').click();

            var ClassOff = 'Button BigBlue SubmitOrder ButtonOff';
            var ClassOn = 'Button BigBlue SubmitOrder ButtonOn';

            var btnSubmit = $('#btnSubmitClaim');
            var btnPendingConfirm = $('#btnPendingConfirm');

            var ItemRowsCount = $('#products tbody: tr').length - 2;
            var ReturnedCount = $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).find('.returned:checked').length;
            var ReceivedCount = $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).find('.hasBeenReceived:checked').length;

            if (ReturnedCount > 0 && ReturnedCount > ReceivedCount) {
                btnSubmit.attr('class', ClassOff);
                btnPendingConfirm.attr('class', ClassOn);
            }
            else if (ReturnedCount > 0 && ReturnedCount == ReceivedCount) {
                btnSubmit.attr('class', ClassOn);
                btnPendingConfirm.attr('class', ClassOff);
            }
            else {
                btnSubmit.attr('class', ClassOff);
                btnPendingConfirm.attr('class', ClassOff);
            }
        }

        function SubmitItems() {
            $("#message").text("");

            var ticketNumber = $("#txtTicket").val() == "" ? 0 : $("#txtTicket").val();
            var list = [];

            $("input[type=checkbox]:checked").each(function () {

                var itemId = $(this).prop("id").split("_")[1];
                var quantityToClaim = $(this).closest("tr").find(".numeric").val();

                var obj = {
                    Key: Number(itemId),
                    Value: Number(quantityToClaim == "" ? 0 : quantityToClaim)
                }

                list.push(obj);
            });

            $.ajax({
                url: '<%= Url.Action("ClaimItemsByOrderNumber", "Details")%>',
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ listToClaim: list, orderNumber: '<%= Model.OrderNumber %>', ticketNumber: ticketNumber }),
                success: function (result) {

                    if (result.success == false) {
                        if (result.fatal == false)
                            $("#message").text(result.message);
                        else
                            showMessage(result.message, true);
                    }
                    else {
                        showMessage(result.message, false);
                    }

                },
                error: function (jqXHR, textStatus) {
                    console.log(jqXHR);
                    console.log(textStatus);
                }
            });

        }

        

    </script>

</asp:Content>