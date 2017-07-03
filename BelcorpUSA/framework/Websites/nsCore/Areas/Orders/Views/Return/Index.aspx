 <%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Order>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Orders") %>"><%= Html.Term("Orders", "Orders") %></a> >
	<a href="<%= ResolveUrl("~/Orders/Details/Index/") + Model.OrderNumber %>"><%= Html.Term("OrderDetail", "Order Detail") %></a> >
	<%= Html.Term("ReturnOrder", "Return Order") %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style> 
        [class^=childItem] 
        {
            display: none; 
            background-color: #DCDCDC;
        }
    </style>

    <%
        List<dynamic> OrderItems = new List<dynamic>();
         
        foreach (OrderCustomer customer in Model.OrderCustomers)
        {
            foreach (OrderItem orderItem in customer.ParentOrderItems)
            {

                //NetSteps.Data.Entities.Dto.ReturnOrderItemDto parent = new NetSteps.Data.Entities.Dto.ReturnOrderItemDto();
                NetSteps.Data.Entities.Dto.ReturnOrderItemDto parent = new NetSteps.Data.Entities.Dto.ReturnOrderItemDto();
                parent.OrderItemID = orderItem.OrderItemID;
                parent.ParentOrderItemID = orderItem.ParentOrderItemID;
                parent.ProductID = Convert.ToInt32(orderItem.ProductID);
                var parentQuantity = Convert.ToInt32(orderItem.Quantity);
                parent.SKU = orderItem.SKU;
                parent.AllHeader = true;
                parent.QuantityOrigen = orderItem.Quantity;
                OrderItems.Add(parent);   
                
                foreach (OrderItem childItem in orderItem.ChildOrderItems)
                {
                    NetSteps.Data.Entities.Dto.ReturnOrderItemDto child = new NetSteps.Data.Entities.Dto.ReturnOrderItemDto();
                    child.OrderItemID = childItem.OrderItemID;
                    child.ParentOrderItemID = childItem.ParentOrderItemID;
                    child.ProductID = Convert.ToInt32(childItem.ProductID);
                    child.SKU = childItem.SKU;
                    child.HasComponents = false;
                    child.AllHeader = false;
                    child.QuantityOrigen = childItem.Quantity;
                    child.ParentQuantity = parentQuantity;
                    OrderItems.Add(child);           
                 }
            }
        }
    %>
    

	<script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/numeric.js") %>"></script>
	<script type="text/javascript">

	    /*CS:30JUL2016.Inicio*/
	    var valorCantidadActual = 0;

	    //onfocus
	    function GetValueQuantity(Quantity, idControl) {
	        valorCantidadActual = Quantity;
	        //document.getElementById(idControl).style.backgroundColor = "#FFFFB2";
	        document.getElementById(idControl).style.backgroundColor = "#FFFFB2";
	    }

	    //onblur
	    function UpdateQuantity(Quantity, idControl) {
	        if (Quantity == valorCantidadActual) {
	            //document.getElementById(idControl).style.backgroundColor = "white";
	            document.getElementById(idControl).style.backgroundColor = "white";
	            return;
	        }
	        Actualizar();
	    }
	    /*CS:30JUL2016.Fin*/
	    var retornoTotal = false;
	    var ValorFleteOriginal = 0;
	    function Retornar() {
	        if (!$(this).hasClass('ButtonOff')) {
	            $('#btnUpdate').click();
	            var isTotal = false;

	            if ($('.returned').length == $('.returned:checked').length) {
	                isTotal = true;
	            }

	            var valorFlete = 0;
	            var orderStatusID = '<%= Model.OrderStatusID %>';
	            if (retornoTotal == true) {
	                valorFlete = $('#shippingRefunded').val();
	                isTotal = true;
	            }
	            else {
	                valorFlete = 0;
	                isTotal = false;
	            }

	            if (orderStatusID != 21) {/*[21] => Delivered*/
	                isTotal = true;
	            }
	            var data = {
	                originalOrderId: '<%= Model.OrderID %>',
	                refundOriginalPayments: false, //$('#originalPayment').prop('checked'),
	                returnType: $('#sReturnType').val(),
	                invoiceNotes: $('#notes').val(),
	                creditAmount: NoCurrency($('#amountCredit').text()), //$('#amountCredit').text().replace(noCurrency, ''),
	                creditType: $('#creditType').prop('checked'),
	                isTotal: isTotal
	            };

	            var OrderItemList = GetOrderItems();

	            $.each(OrderItemList, function (index, item) {
	                data['OrderItemList[' + index + '].ParentOrderItemID'] = item.ParentOrderItemID;
	                data['OrderItemList[' + index + '].OrderItemID'] = item.OrderItemID;
	                data['OrderItemList[' + index + '].ProductID'] = item.ProductID;
	                data['OrderItemList[' + index + '].SKU'] = item.SKU;
	                data['OrderItemList[' + index + '].ParentQuantity'] = item.ParentQuantity;
	                data['OrderItemList[' + index + '].Quantity'] = item.Quantity;
	                data['OrderItemList[' + index + '].QuantityOrigen'] = item.QuantityOrigen;
	                data['OrderItemList[' + index + '].ItemPrice'] = item.ItemPrice;
	                data['OrderItemList[' + index + '].HasComponents'] = item.HasComponents;
	                data['OrderItemList[' + index + '].AllHeader'] = item.AllHeader;
	                data['OrderItemList[' + index + '].IsChild'] = item.IsChild;

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

	            $.post('<%= ResolveUrl("~/Orders/Return/SubmitReturn") %>', data, function (response) {
	                if (response.result) {
	                    window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.returnOrderNumber;
	                } else {
	                    $('.WaitWin').jqmHide();
	                    showMessage(response.message, true);
	                }
	            });
	        }

	    }

	    function SubmitActualizar() {
	        var OrderItemList = GetOrderItems();

	        var Subtotal = 0;
	        var GrandTotal = 0;

	        $.each(OrderItemList, function (index, item) {
	            Subtotal += (item.Quantity * -1) * item.ItemPrice;
	            GrandTotal += (item.Quantity * -1) * item.ItemPrice;
	        });

	        var data = {
	            originalOrderId: '<%= Model.OrderID %>',
	            //overridenShipping: overridenShipping,
	            overridenShipping: true,
	            restockingFee: NoCurrency($('#restockingAmount').val()),
	            refundedShipping: NoCurrency($('#shippingRefunded').val()),
	            Subtotal: Subtotal,
	            GrandTotal: GrandTotal
	        }, i = 0;

	        $('#products tbody:first tr').each(function () {
	            var returnQuantity = $('.returnQuantity', this), quantity = $('.quantity', this).text();
	            if ($('.returned', this).prop('checked')) {
	                data['returnedProducts[' + i + '].productID'] = $('.productID', this).val();
	                data['returnedProducts[' + i + '].orderItemId'] = $('.orderItemId', this).val();
	                if ($(this).hasClass("childItem")) {
	                    data['returnedProducts[' + i + '].parentOrderItemId'] = $(this).prevAll('.parentItem:first').find('.orderItemId').val();
	                    data['returnedProducts[' + i + '].dynamicKitGroupId'] = $('.dynamicKitGroupId', this).val();
	                    if ($(this).prevAll('.parentItem:first').find('.returned').prop('checked')) {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = 0;
	                    } else {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = NoCurrency($('.returnPricePerItem', this).val()); //$('.returnPricePerItem', this).val().replace(noCurrency, '');
	                        data['returnedProducts[' + i + '].returnItemCV'] = NoCurrency($('.returnCVPerItem', this).val()); //$('.returnCVPerItem', this).val().replace(noCurrency, '');
	                    }
	                } else {
	                    data['returnedProducts[' + i + '].parentOrderItemId'] = null;
	                    data['returnedProducts[' + i + '].dynamicKitGroupId'] = null;
	                    var returnPricePerItemElement = $('.returnPricePerItem', this).val();
	                    if (returnPricePerItemElement != undefined && returnPricePerItemElement != null) {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = NoCurrency($('.returnPricePerItem', this).val()); //$('.returnPricePerItem', this).val().replace(noCurrency, '');
	                    } else {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = 0;
	                    }
	                    var returnItemCVElement = $('.returnCVPerItem', this).val();
	                    if (returnItemCVElement != undefined && returnItemCVElement != null) {
	                        data['returnedProducts[' + i + '].returnItemCV'] = NoCurrency(returnItemCVElement); //returnItemCVElement.replace(noCurrency, '');
	                    } else {
	                        data['returnedProducts[' + i + '].returnItemCV'] = 0;
	                    }
	                }
	                data['returnedProducts[' + i + '].itemPrice'] = NoCurrency($('.pricePerItem', this).text()); //$('.pricePerItem', this).text().replace(noCurrency, '');

	                data['returnedProducts[' + i + '].returnReasonID'] = $('.returnReasonId', this).val();
	                data['returnedProducts[' + i + '].quantityReturned'] = $.trim(returnQuantity.val());
	                data['returnedProducts[' + i + '].isRestockable'] = $('.restockable', this).prop('checked');
	                data['returnedProducts[' + i + '].hasBeenReceived'] = $('.hasBeenReceived', this).prop('checked');
	                data['returnedProducts[' + i + '].orderCustomerID'] = $('.orderCustomerId', this).val();
	                ++i;
	            }
	            calculateLineTotal(this);

	            returnQuantity.val($.trim(returnQuantity.val()));
	        });

	        $.ajax({
	            type: 'POST',
	            url: '<%= ResolveUrl("~/Orders/Return/UpdateProductReturns") %>',
	            data: JSON.stringify(data),
	            contentType: 'application/json; charset=utf-8',
	            dataType: 'json',
	            success: function (response) {
	                if (response.result) {
	                    $('.returnGrandTotal').text(response.totals.grandTotal);
	                    $('#amountCredit').text(response.totals.grandTotal);

	                    var newgrandTotal = parseFloat(NoCurrency(response.totals.grandTotal)) + parseFloat(NoCurrency($('#shippingRefunded').val()));
	                    response.totals.grandTotal = '$' + newgrandTotal;

	                    $('#products tbody:first tr').each(function (index, row) {
	                        if (response.returnedProducts) {
	                            for (var j = 0; j < response.returnedProducts.length; j++) {
	                                var returnedProduct = response.returnedProducts[j];

	                                if (returnedProduct.OrderItemID == $('.orderItemId', row).val()) {
	                                    $('.returnQuantity', row).val(returnedProduct.QuantityReturned);

	                                    calculateLineTotal(row);
	                                }
	                            }
	                        }
	                    });
	                    var SubTotal = 0;
	                    $(".lineTotal").each(function () {
	                        var sub = this.innerHTML.substr(1);
	                        SubTotal = SubTotal + parseFloat(sub);
	                    });

	                    $('.subtotal').text("$" + SubTotal.toFixed(2));
	                    var shippingTotal = $('#shippingRefunded').val();

	                    var total = parseFloat(shippingTotal) + SubTotal;
	                    $('.returnGrandTotal').text("$" + total.toFixed(2));
	                    //parseFloat(shippingTotal) + SubTotal)
	                    $('#amountCredit').html("$" + total.toFixed(2));
	                    if (response.totals.shippingTotal != null) {
	                        var shippingTotalFixed = parseFloat(response.totals.originalShippingTotal).toFixed(2);
	                        var originalShippingTotalFixed = parseFloat(response.totals.originalShippingTotal).toFixed(2);

	                        /*CS.27JUL2016.Inicio.Comentado*/
	                        var orderStatusID = '<%= Model.OrderStatusID %>';
	                        if (orderStatusID == 21) {/*[21] => Delivered*/

	                            var SubTotalOriginal = '<%= Model.Subtotal.ToDecimal() %>';

	                            if (Number(SubTotal).toFixed(2) == Number(SubTotalOriginal).toFixed(2) && $('#ckbReturnItemHead').prop('checked') == true) {
	                                $('#shippingRefunded').val(shippingTotalFixed);
	                                retornoTotal = true;
	                            }
	                            else {
	                                var flete = 0;
	                                $('#shippingRefunded').val(flete);
	                                retornoTotal: false;
	                            }
	                        }
	                        /*CS.27JUL2016.Fin.Comentado*/

	                        if (response.totals.originalShippingTotal != null) {
	                            $('#maxShippingRefund').val(originalShippingTotalFixed);
	                        }
	                        else {
	                            $('#maxShippingRefund').val(shippingTotalFixed);
	                        }
	                        //$('#shippingRefunded').get(0).disabled = false;
	                    }
	                    else {
	                        //$('#shippingRefunded').get(0).disabled = true;
	                    }

	                    checkBalance();
	                    Retornar();
	                } else {
	                    showMessage(response.message, true);
	                }
	            }
	        });

	    }

	    function Actualizar() {
	        var OrderItemList = GetOrderItems();

	        var Subtotal = 0;
	        var GrandTotal = 0;

	        $.each(OrderItemList, function (index, item) {
	            Subtotal += (item.Quantity * -1) * item.ItemPrice;
	            GrandTotal += (item.Quantity * -1) * item.ItemPrice;
	        });

	        /*CS.27JUL2016.Inicio.Comentado*/
	        var orderStatusID = '<%= Model.OrderStatusID %>';
	        
	        if (orderStatusID == 21) {/*[21] => Delivered*/

	            var SubTotalOriginal = '<%= Model.Subtotal.ToDecimal() %>';
	            if (Number(Subtotal).toFixed(2) == Number(SubTotalOriginal).toFixed(2) && $('#ckbReturnItemHead').prop('checked') == true) {
	                $('#shippingRefunded').val(ValorFleteOriginal);
	                retornoTotal = true;
	            }
	            else {
	                var flete = 0;
	                $('#shippingRefunded').val(flete);
	                retornoTotal = false;
	            }
	        }
//	        /*CS.27JUL2016.Fin.Comentado*/

	        var data = {
	            originalOrderId: '<%= Model.OrderID %>',
	            overridenShipping: overridenShipping,
	            restockingFee: NoCurrency($('#restockingAmount').val()),
	            refundedShipping: NoCurrency($('#shippingRefunded').val()),
	            Subtotal: Subtotal,
	            GrandTotal: GrandTotal
	        }, i = 0;

	        $('#products tbody:first tr').each(function () {
	            var returnQuantity = $('.returnQuantity', this), quantity = $('.quantity', this).text();
	            if ($('.returned', this).prop('checked')) {
	                data['returnedProducts[' + i + '].productID'] = $('.productID', this).val();
	                data['returnedProducts[' + i + '].orderItemId'] = $('.orderItemId', this).val();
	                if ($(this).hasClass("childItem")) {
	                    data['returnedProducts[' + i + '].parentOrderItemId'] = $(this).prevAll('.parentItem:first').find('.orderItemId').val();
	                    data['returnedProducts[' + i + '].dynamicKitGroupId'] = $('.dynamicKitGroupId', this).val();
	                    if ($(this).prevAll('.parentItem:first').find('.returned').prop('checked')) {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = 0;
	                    } else {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = NoCurrency($('.returnPricePerItem', this).val()); //$('.returnPricePerItem', this).val().replace(noCurrency, '');
	                        data['returnedProducts[' + i + '].returnItemCV'] = NoCurrency($('.returnCVPerItem', this).val()); //$('.returnCVPerItem', this).val().replace(noCurrency, '');
	                    }
	                } else {
	                    data['returnedProducts[' + i + '].parentOrderItemId'] = null;
	                    data['returnedProducts[' + i + '].dynamicKitGroupId'] = null;
	                    var returnPricePerItemElement = $('.returnPricePerItem', this).val();
	                    if (returnPricePerItemElement != undefined && returnPricePerItemElement != null) {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = NoCurrency($('.returnPricePerItem', this).val()); //$('.returnPricePerItem', this).val().replace(noCurrency, '');
	                    } else {
	                        data['returnedProducts[' + i + '].returnItemPrice'] = 0;
	                    }
	                    var returnItemCVElement = $('.returnCVPerItem', this).val();
	                    if (returnItemCVElement != undefined && returnItemCVElement != null) {
	                        data['returnedProducts[' + i + '].returnItemCV'] = NoCurrency(returnItemCVElement); //returnItemCVElement.replace(noCurrency, '');
	                    } else {
	                        data['returnedProducts[' + i + '].returnItemCV'] = 0;
	                    }
	                }
	                data['returnedProducts[' + i + '].itemPrice'] = NoCurrency($('.pricePerItem', this).text()); //$('.pricePerItem', this).text().replace(noCurrency, '');

	                data['returnedProducts[' + i + '].returnReasonID'] = $('.returnReasonId', this).val();
	                data['returnedProducts[' + i + '].quantityReturned'] = $.trim(returnQuantity.val());
	                data['returnedProducts[' + i + '].isRestockable'] = $('.restockable', this).prop('checked');
	                data['returnedProducts[' + i + '].hasBeenReceived'] = $('.hasBeenReceived', this).prop('checked');
	                data['returnedProducts[' + i + '].orderCustomerID'] = $('.orderCustomerId', this).val();
	                ++i;
	            }
	            calculateLineTotal(this);

	            returnQuantity.val($.trim(returnQuantity.val()));
	        });

	        $.ajax({
	            type: 'POST',
	            url: '<%= ResolveUrl("~/Orders/Return/UpdateProductReturns") %>',
	            data: JSON.stringify(data),
	            contentType: 'application/json; charset=utf-8',
	            dataType: 'json',
	            success: function (response) {
	                if (response.result) {
	                    ValorFleteOriginal = response.totals.originalShippingTotal;
	                    if (retornoTotal) {
	                        $('#shippingRefunded').val(ValorFleteOriginal);
	                    }
	                    $('.returnGrandTotal').text(response.totals.grandTotal);
	                    $('#amountCredit').text(response.totals.grandTotal);

	                    var newgrandTotal = parseFloat(NoCurrency(response.totals.grandTotal)) + parseFloat(NoCurrency($('#shippingRefunded').val()));
	                    response.totals.grandTotal = '$' + newgrandTotal;

	                    $('#products tbody:first tr').each(function (index, row) {
	                        if (response.returnedProducts) {
	                            for (var j = 0; j < response.returnedProducts.length; j++) {
	                                var returnedProduct = response.returnedProducts[j];

	                                if (returnedProduct.OrderItemID == $('.orderItemId', row).val()) {
	                                    $('.returnQuantity', row).val(returnedProduct.QuantityReturned);

	                                    calculateLineTotal(row);
	                                }
	                            }
	                        }
	                    });
	                    var SubTotal = 0;
	                    $(".lineTotal").each(function () {
	                        var sub = this.innerHTML.substr(1);
	                        SubTotal = SubTotal + parseFloat(sub);
	                    });

	                    $('.subtotal').text("$" + SubTotal.toFixed(2));
	                    var shippingTotal = $('#shippingRefunded').val();

	                    var total = parseFloat(shippingTotal) + SubTotal;
	                    $('.returnGrandTotal').text("$" + total.toFixed(2));
	                    //parseFloat(shippingTotal) + SubTotal)
	                    $('#amountCredit').html("$" + total.toFixed(2));
	                    if (response.totals.shippingTotal != null) {
	                        var shippingTotalFixed = parseFloat(response.totals.originalShippingTotal).toFixed(2);
	                        var originalShippingTotalFixed = parseFloat(response.totals.originalShippingTotal).toFixed(2);

	                        /*CS.27JUL2016.Inicio.Comentado*/
	                        //	                        	                        var orderStatusID = '<%= Model.OrderStatusID %>';
	                        //	                        	                        if (orderStatusID == 21) {/*[21] => Delivered*/

	                        //	                        	                            var SubTotalOriginal = '<%= Model.Subtotal.ToDecimal() %>';
	                        //	                        	                            if (Number(SubTotal).toFixed(2) == Number(SubTotalOriginal).toFixed(2) && $('#ckbReturnItemHead').prop('checked') == true) {
	                        //	                        	                                $('#shippingRefunded').val(shippingTotalFixed);
	                        //	                        	                                retornoTotal = true;
	                        //	                        	                            }
	                        //	                        	                            else {
	                        //	                        	                                var flete = 0;
	                        //	                        	                                $('#shippingRefunded').val(flete);
	                        //	                        	                                retornoTotal = false;
	                        //	                        	                            }
	                        //	                        	                        }
	                        /*CS.27JUL2016.Fin.Comentado*/

	                        if (response.totals.originalShippingTotal != null) {
	                            $('#maxShippingRefund').val(originalShippingTotalFixed);
	                        }
	                        else {
	                            $('#maxShippingRefund').val(shippingTotalFixed);
	                        }
	                        //$('#shippingRefunded').get(0).disabled = false;
	                    }
	                    else {
	                        //$('#shippingRefunded').get(0).disabled = true;
	                    }

	                    checkBalance();
	                } else {
	                    showMessage(response.message, true);
	                }
	            }
	        });

	    }

	    var noCurrency = /[^\d\.]/g, overridenShipping = false;
	    $(function () {

	        
	       

	        var OrderStatusID = '<%= Model.OrderStatusID %>';
	        //Desabilitar Flete
	        $("#shippingRefunded").attr("disabled", "disabled");
	        if (OrderStatusID.toString() != "21") {/*[21] => Delivered*/
	            //$("#shippingRefunded").attr("disabled", "disabled");
	            $('#shippingRefunded').val(NoCurrency('<%= Model.ShippingTotal.ToStringDecimalGlobalization(Model.CurrencyID) %>'));
	        }
	        else {
	            //$("#shippingRefunded").removeAttr("disabled");
	            $('#shippingRefunded').val("0.00");
	        }
	        var subTotal = 0;


	        $('.returned').click(function () {
	            var control = $(this);
	            validateKitItemsCheck($(this).closest('tr'), control);
                checkIfReturned.apply($(this).closest('tr').get(0));
	            calculateProductCredit();
	            //validateHeaderCheck();

	        });
	        $('.restockable').click(function () {
	            calculateRestockingAmount();
	        });

	        $('.hasBeenReceived').click(function () {
	            validateEnableSubmit();
	            //validateHeaderCheck();
	        });

	        $('.returnQuantity').blur(function () {

	            var control = $(this);

	            if ($(this).val().trim() != '') {
	                var maxQuantity = parseInt(control.closest('tr').find('.quantity').text().trim(), 10);
	                var currentQuantity = parseInt(control.val(), 10);

	                if (currentQuantity > maxQuantity) {
	                    showMessage("Input quantity exceeded maximum quantity allowed.", true);
	                    control.val('').focus();
	                }
	                else if (currentQuantity < 1 || isNaN(currentQuantity)) {
	                    showMessage("Input quantity not allowed.", true);
	                    control.val('').focus();
	                }
	                else {
	                    hideMessage();
	                }
	            }
	            else {
	                showMessage("Quantity to be returned can't be empty.", true);
	                control.focus();
	            }
	        });

	        $('#products tbody:first tr').each(checkIfReturned);

	        $('#shippingRefunded').numeric({ allowNegative: false });


	        $('#btnUpdate').click(function () {
	            Actualizar();
	            //	            $.post('<%= ResolveUrl("~/Orders/Return/UpdateProductReturns") %>', data, function (response) {
	            //	                if (response.result) {

	            //	                    $('#products tbody:first tr').each(function (index, row) {
	            //	                        if (response.returnedProducts) {
	            //	                            for (var j = 0; j < response.returnedProducts.length; j++) {
	            //	                                var returnedProduct = response.returnedProducts[j];

	            //	                                if (returnedProduct.OrderItemID == $('.orderItemId', row).val()) {
	            //	                                    $('.returnQuantity', row).val(returnedProduct.QuantityReturned);

	            //	                                    calculateLineTotal(row);
	            //	                                }
	            //	                            }
	            //	                        }
	            //	                    });

	            //	                    $('.subtotal').text(response.totals.subtotal);
	            //	                    $('.taxTotal').text(response.totals.taxTotal);
	            //	                    $('.returnGrandTotal').text(response.totals.grandTotal);
	            //	                    if (response.totals.shippingTotal != null) {
	            //	                        var shippingTotalFixed = parseFloat("24.00").toFixed(2)//parseFloat(response.totals.shippingTotal).toFixed(2);
	            //	                        var originalShippingTotalFixed = parseFloat(response.totals.originalShippingTotal).toFixed(2);

	            //	                        $('#shippingRefunded').val(shippingTotalFixed);
	            //	                        if (response.totals.originalShippingTotal != null) {
	            //	                            $('#maxShippingRefund').val(originalShippingTotalFixed);
	            //	                        }
	            //	                        else {
	            //	                            $('#maxShippingRefund').val(shippingTotalFixed);
	            //	                        }
	            //	                        $('#shippingRefunded').get(0).disabled = false;
	            //	                    }
	            //	                    else {
	            //	                        $('#shippingRefunded').get(0).disabled = true;
	            //	                    }

	            //	                    checkBalance();
	            //	                } else {
	            //	                    showMessage(response.message, true);
	            //	                }
	            //	            });
	        });

	        $('#restockingPercent').blur(function () {
	            calculateRestockingAmount();
	        });
	        $('#restockingAmount').blur(function () {
	            calculateRestockingPercentage();
	        });
	        $('#btnUpdateShipping').click(function () {
	            overridenShipping = true;
	            var maxShipping = parseFloat('<%= Model.ShippingTotal %>');
	            var $shippingAmount = $('#shippingRefunded');
	            if ($shippingAmount.val() > maxShipping)
	                $shippingAmount.val(maxShipping);
	            $.post('<%= ResolveUrl("~/Orders/Return/UpdateShippingRefunded") %>', { shippingAmount: $('#shippingRefunded').val() }, function (response) {
	                if (response.result) {

	                    var OrderItemList = GetOrderItems();
	                    var GrandTotal = response.grandTotal;

	                    $.each(OrderItemList, function (index, item) {
	                        GrandTotal += (item.Quantity * -1) * item.ItemPrice;
	                    });
	                    var SubTotal = 0;
	                    $(".lineTotal").each(function () {
	                        var sub = this.innerHTML.substr(1);
	                        SubTotal = SubTotal + parseFloat(sub);
	                    });
	                    $('.subtotal').text("$" + SubTotal.toFixed(2));
	                    var shippingTotal = $('#shippingRefunded').val();
	                    var total = parseFloat(shippingTotal) + SubTotal;
	                    $('.returnGrandTotal').text("$" + total.toFixed(2));
	                    //parseFloat(shippingTotal) + SubTotal)
	                    $('#amountCredit').html("$" + total.toFixed(2));


	                    //$('#amountCredit').text(response.grandTotal);
	                    //$('.returnGrandTotal').text(response.grandTotal);
	                    checkBalance();
	                } else {
	                    showMessage(response.message, true);
	                }
	            });
	        });

	        //	        $('#shippingRefunded').blur(function () {
	        //	            var maxAmount = parseFloat($(this).parent().find('#maxShippingRefund').val());
	        //	            var minAmount = 0;
	        //	            if ($(this).val() > maxAmount) {
	        //	                $(this).val(maxAmount.toFixed(2));
	        //	            }
	        //	            if ($(this).val() === undefined || $(this).val() == null || $(this).val() == '' || $(this).val() < minAmount) {
	        //	                $(this).val(minAmount.toFixed(2));
	        //	            }
	        //	            var grandTotal = NoCurrency($('.returnGrandTotal').text());
	        //	            var paymentTotal = 0;
	        //	            $('#originalPayments .paymentRefund').each(function () {
	        //	                paymentTotal += parseFloat($(this).val());
	        //	            });
	        //	            if (paymentTotal > grandTotal) {
	        //	                $(this).val((parseFloat($(this).val()) - (paymentTotal - grandTotal)).toFixed(2));
	        //	            }


	        //	        });

	        $('#originalPayments .paymentRefund').blur(function () {
	            var maxAmount = parseFloat($(this).parent().find('.paymentAmount').val());
	            if ($(this).val() > maxAmount) {
	                $(this).val(maxAmount.toFixed(2));
	            }
	            var grandTotal = NoCurrency($('.returnGrandTotal').text()); //$('.returnGrandTotal').text().replace(noCurrency, '');
	            var paymentTotal = 0;
	            $('#originalPayments .paymentRefund').each(function () {
	                paymentTotal += parseFloat($(this).val());
	            });
	            if ($('#originalPayments .paymentRefund').length == 1 && paymentTotal > grandTotal) {
	                paymentTotal = (parseFloat($(this).val()) - (paymentTotal - grandTotal)).toFixed(2);
	                $(this).val(paymentTotal);
	            }
	            else if (paymentTotal > grandTotal) {
	                paymentTotal = paymentTotal.toFixed(2);
	            }

	            //	            if (paymentTotal == grandTotal && $('.returned[type=checkbox]:checked').length > 0) {
	            //	                $('#btnSubmit').removeClass('ButtonOff');
	            //	                enablePedingConfirmButton(true); //@01 C09 - button Pending Confirm -->
	            //	            } else if (!$('#btnSubmit').hasClass('ButtonOff') || $('.returned[type=checkbox]:checked').length == 0) {
	            //	                $('#btnSubmit').addClass('ButtonOff');
	            //	                enablePedingConfirmButton(false); //@01 C10 -  button Pending Confirm -->
	            //	            }
	        });

	        $('input[name="paymentType"]').click(function () {
	            var grandTotal = NoCurrency($('.returnGrandTotal').text()); //$('.returnGrandTotal').text().replace(noCurrency, '');
	            var paymentTotal = 0;
	            $('#originalPayments .paymentRefund').each(function () {
	                paymentTotal += parseFloat($(this).val());
	            });

	            if ($(this).val() == 'check' && paymentTotal == grandTotal && $('.returned[type=checkbox]:checked').length > 0) {
	                $('#btnSubmit').removeClass('ButtonOff');
	                enablePedingConfirmButton(false); //@01 C11 -  button Pending Confirm -->
	            } else {
	                checkBalance();
	            }
	        });
	        $('.numeric').numeric();

	        $('#btnPendingConfirm').click(function () {

	            if (!$(this).hasClass('ButtonOff')) {
	                $('#btnUpdate').click();
	                var data = {
	                    originalOrderId: '<%= Model.OrderID %>',
	                    refundOriginalPayments: false, //$('#originalPayment').prop('checked'),
	                    returnType: $('#sReturnType').val(),
	                    invoiceNotes: $('#notes').val(),
	                    creditAmount: NoCurrency($('#amountCredit').text()),
	                    creditType: $('#creditType').prop('checked'),
	                    esOrdenTotal: retornoTotal
	                };

	                var OrderItemList = GetOrderItems();

	                $.each(OrderItemList, function (index, item) {
	                    data['OrderItemList[' + index + '].ParentOrderItemID'] = item.ParentOrderItemID;
	                    data['OrderItemList[' + index + '].OrderItemID'] = item.OrderItemID;
	                    data['OrderItemList[' + index + '].ProductID'] = item.ProductID;
	                    data['OrderItemList[' + index + '].SKU'] = item.SKU;
	                    data['OrderItemList[' + index + '].ParentQuantity'] = item.ParentQuantity;
	                    data['OrderItemList[' + index + '].Quantity'] = item.Quantity;
	                    data['OrderItemList[' + index + '].QuantityOrigen'] = item.QuantityOrigen;
	                    data['OrderItemList[' + index + '].ItemPrice'] = item.ItemPrice;
	                    data['OrderItemList[' + index + '].HasComponents'] = item.HasComponents;
	                    data['OrderItemList[' + index + '].AllHeader'] = item.AllHeader;
	                    data['OrderItemList[' + index + '].IsChild'] = item.IsChild;
	                });

	                var grandTotal = NoCurrency($('.returnGrandTotal').text());

	                $('.WaitWin').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' }).jqmShow();
	                $.post('<%= ResolveUrl("~/Orders/Return/PendingConfirm") %>', data, function (response) {
	                    if (response.result) {
	                        window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.returnOrderNumber;
	                    } else {
	                        $('.WaitWin').jqmHide();
	                        showMessage(response.message, true);
	                    }
	                });
	            }
	        });


	        $('#btnSubmit').click(function () {
	            if ($("#btnSubmit").hasClass("ButtonOff") == false)
	                SubmitActualizar();
	        });

	        $('#btnContinueLater').click(function () {
	            $('#btnUpdate').click();
	            $('#btnUpdateShipping').click();
	            var data = {
	                originalOrderId: '<%= Model.OrderID %>',
	                refundOriginalPayments: $('#originalPayment').prop('checked'),
	                returnType: $('#sReturnType').val(),
	                invoiceNotes: $('#notes').val(),
	                creditAmount: NoCurrency($('#amountCredit').text()), //$('#amountCredit').text().replace(noCurrency, ''),
	                creditType: $('#creditType').prop('checked')
	            };
	            if (data.refundOriginalPayments) {
	                $('#originalPayments .paymentRefund').each(function (i) {
	                    data['refundPayments[' + i + '].Key'] = $(this).attr('id').replace(/\D/g, '');
	                    data['refundPayments[' + i + '].Value'] = $(this).val();
	                });
	            }
	            var grandTotal = NoCurrency($('.returnGrandTotal').text()); //$('.returnGrandTotal').text().replace(noCurrency, '');
	            if (grandTotal == 0) {
	                showMessage('Please click the update button to save the changes to the order.', true);
	                return false;
	            }
	            $('.WaitWin').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' }).jqmShow();
	            $.post('<%= ResolveUrl("~/Orders/Return/UpdateReturn") %>', data, function (response) {
	                if (response.result) {
	                    //window.location = '<%= ResolveUrl("~/Accounts/Overview/") %>' + response.accountNumber;
	                    window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.returnOrderNumber;
	                } else {
	                    $('.WaitWin').jqmHide();
	                    showMessage(response.message, true);
	                }
	            });
	        });

	        //@01 C03 - checkboxs click event
	        $('#ckbReturnItemHead').click(function () {
	            selectReturnItemChecks();
	            calculateProductCredit();
	            Actualizar();
	        });

	        $('#ckbReceivedItemHead').click(function () {
	            selectReceivedItemsCheks();
	            //calculateRestockingAmount();
	        });
	        //@01 C03 - End checkboxs click event

	        //@01 C11 - Button Pending Confirm Click Event -->
	        $('#btnConfirmPending').click(function () {
	            var idsupportTicket = $('#HdfSupportTicketID').val().trim();
	            var idnationalMail = $('#IDNationalMail').val().trim();

	            if (idsupportTicket.length == 0) {
	                showMessage($('#IDSupportTicketValidation').val(), true);
	                return false;
	            }
	            else {
	                //logica de continue later
	                var promise = updateDeferred();
	                promise.done(updateShippingDeferred)
                           .done(function () {
                               var grandTotal = NoCurrency($('.returnGrandTotal').text()); //$('.returnGrandTotal').text().replace(noCurrency, '');
                               //                    if (grandTotal == 0) {
                               //                        showMessage('Please click the update button to save the changes to the order.', true);
                               //                        return false;
                               //                    }

                               var data = itemsSelected();
                               $('.WaitWin').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' }).jqmShow();

                               //@ at-007
                               $.post('<%= ResolveUrl("~/Orders/Return/SavePedingConfirm") %>', data, function (response) {
                                   if (response.result) {
                                       window.location = '<%= ResolveUrl("~/Orders/Details/Index/") %>' + response.returnOrderNumber;
                                   } else {
                                       $('.WaitWin').jqmHide();
                                       showMessage(response.message, true);
                                   }
                               });
                           });
	            }
	        });
	        //@01 C11 - End Button Pending Confirm Click Event -->
	    });

	    function itemsSelected() {
	        var data = {
	            originalOrderId: '<%= Model.OrderID %>',
	            refundOriginalPayments: $('#originalPayment').prop('checked'),
	            returnType: $('#sReturnType').val(),
	            invoiceNotes: $('#notes').val(),
	            //confirm
	            overridenShipping: overridenShipping,
	            restockingFee: $('#restockingAmount').val(),
	            refundedShipping: $('#shippingRefunded').val(),
	            idSupportTicket: $('#HdfSupportTicketID').val().trim(),
	            idnationalMail: $('#IDNationalMail').val().trim(),
	            productCreditAmount: NoCurrency($('#amountCredit').text()), //$('#amountCredit').text().replace(noCurrency, ''),
	            creditAmount: NoCurrency($('#amountCredit').text()), //$('#amountCredit').text().replace(noCurrency, ''),
	            creditType: $('#creditType').prop('checked')
	        },
                     i = 0;

	        if (data.refundOriginalPayments) {
	            $('#originalPayments .paymentRefund').each(function (j) {
	                data['refundPayments[' + j + '].Key'] = $(this).attr('id').replace(/\D/g, '');
	                data['refundPayments[' + j + '].Value'] = $(this).val();
	            });
	        }

	        $('#products tbody:first tr').each(function () {
	            var returnQuantity = $('.returnQuantity', this), quantity = $('.quantity', this).text();
	            if ($('.returned', this).prop('checked')) {
	                data['returnedProducts[' + i + '].productID'] = $('.productID', this).val();
	                data['returnedProducts[' + i + '].orderItemId'] = $('.orderItemId', this).val();

	                data['returnedProducts[' + i + '].itemPrice'] = NoCurrency($('.pricePerItem', this).text()); //$('.pricePerItem', this).text().replace(noCurrency, '');

	                data['returnedProducts[' + i + '].returnReasonID'] = $('.returnReasonId', this).val();
	                data['returnedProducts[' + i + '].quantityReturned'] = $.trim(returnQuantity.val());
	                data['returnedProducts[' + i + '].isRestockable'] = $('.restockable', this).prop('checked');
	                data['returnedProducts[' + i + '].hasBeenReceived'] = $('.hasBeenReceived', this).prop('checked');
	                data['returnedProducts[' + i + '].orderCustomerID'] = $('.orderCustomerId', this).val();
	                ++i;
	            }
	        });
	        return data;
	    }

	    function enablePedingConfirmButton(option) {
	        if ($("#isShipped"))
	            if (option) {
	                $('#btnConfirmPending').removeClass('ButtonOff'); //@01 C09 - button Pending Confirm -->
	                $('#btnConfirmPending').prop('disabled', false);
	            }
	            else {
	                $('#btnConfirmPending').addClass('ButtonOff'); //@01 C08 -  button Pending Confirm -->
	                $('#btnConfirmPending').prop('disabled', true);
	            }
	    }

	    function calculateRestockingAmount() {
	        var restockingSubtotal = 0, lineTotal;
	        $('#products tbody:first tr').each(function () {
	            //lineTotal = parseFloat($('.lineTotal', this).text().replace(noCurrency, ''));
	            var quantity = parseFloat($('.returnQuantity', this).val());
	            var itemPrice = parseFloat(NoCurrency($('.pricePerItem', this).text())); //parseFloat($('.pricePerItem', this).text().replace(noCurrency, ''));
	            if ($('.restockable', this).prop('checked')) {
	                restockingSubtotal += (itemPrice * quantity);
	            }
	        });
	        if (restockingSubtotal == 0) {
	            $('#restockingAmount').val($('#zeroDecimal').val());
	            $('#restockingPercent').val('0.00');
	        }
	        else {
	            restockingSubtotal *= ($('#restockingPercent').val() / 100);
	            $('#restockingAmount').val(restockingSubtotal.toFixed(2));
	            //$('#restockingFeeTotal').text(restockingSubtotal.toFixed(2));
	        }
	        var SubTotal = 0;
	        $(".lineTotal").each(function () {
	            var sub = this.innerHTML.substr(1);
	            SubTotal = SubTotal + parseFloat(sub);
	        });
	        $('.subtotal').text(SubTotal);
	        var shippingTotal = $('#shippingRefunded').val();
	        //$('.returnGrandTotal').text(parseFloat(shippingTotal) + SubTotal);
	        //$('.amountCredit').text(parseFloat(shippingTotal) + SubTotal); 


	    }

	    function calculateRestockingPercentage() {
	        var subtotal = 0;
	        $('#products tbody:first tr').each(function () {
	            //lineTotal = parseFloat($('.lineTotal', this).text().replace(noCurrency, ''));
	            var quantity = parseFloat($('.returnQuantity', this).val());
	            var itemPrice = parseFloat(NoCurrency($('.pricePerItem', this).text())); //parseFloat($('.pricePerItem', this).text().replace(noCurrency, ''));

	            if ($('.returned', this).prop('checked')) {
	                subtotal += (itemPrice * quantity);
	            }
	        });

	        var diff = $('#restockingAmount').val();
	        if (subtotal == 0) {
	            $('#restockingPercent').val('0.00');
	        }
	        else {
	            $('#restockingPercent').val(((diff / subtotal) * 100).toFixed(2));
	        }
	    }

	    function checkIfReturned() {
	        var checkBox = $('.returned', this);
	        if (checkBox.prop('checked')) {
	            if (!checkBox.closest('tr').hasClass("childItem")) {
	                //Hacer ckeck
	                var parentId = checkBox.length > 0 ? checkBox[0].getAttribute("data-parentid") : 0; // IE doesn't support: checkBox[0].dataset.parentid : 0;
	                var parentTotal = parseFloat($('#parentTotal' + parentId).val());
	                var quantity = $('.returnQuantity', this).val();
	                var pricePerItem = NoCurrency($('.pricePerItem', this).text()); //$('.pricePerItem', this).text().replace(noCurrency, '');
	                var itemTotal = (quantity * pricePerItem).toFixed(2);
	                var total = 0.0;

	                if (parentTotal > itemTotal)
	                    total = itemTotal;
	                else
	                    total = parentTotal;

	                total = total * 1;

	                $('.returnPricePerItem', this).val((total / quantity).toFixed(2));


	                $('.lineTotal', this).text('$' + total.toFixed(2));
	                var add = add + total;
	                $('.subtotal', this).text('$' + total.toFixed(2));


	            } else {
	                if (checkBox.closest('tr').prevAll('.parentItem:first').find('.returned').prop('checked')) {
	                    $('.lineTotal', this).text('$0.00');
	                } else {
	                    var parentId = checkBox.length > 0 ? checkBox[0].getAttribute("data-parentid") : 0; // IE doesn't support: checkBox[0].dataset.parentid : 0;
	                    var parentTotal = parseFloat($('#parentTotal' + parentId).val());
	                    var quantity = $('.returnQuantity', this).val();

	                    var pricePerItem = NoCurrency($('.pricePerItem', this).text()); //$('.pricePerItem', this).text().replace(noCurrency, '');
	                    var itemTotal = (quantity * pricePerItem).toFixed(2);
	                    var total = 0.0;

	                    total = itemTotal;
	                    //						if (parentTotal > itemTotal)
	                    //							total = itemTotal;
	                    //						else
	                    //							total = parentTotal;

	                    total = total * 1;

	                    $('.returnPricePerItem', this).val((total / quantity).toFixed(2));

	                    //$('#parentTotal' + parentId).val(parentTotal - total);

	                    $('.lineTotal', this).text('$' + total.toFixed(2));
	                }
	            }
	        } else {
	            var parentId = checkBox.length > 0 ? checkBox[0].getAttribute("data-parentid") : 0; // IE doesn't support: checkBox[0].dataset.parentid : 0;
	            var parentTotal = $('#parentTotal' + parentId).val();
	            var previousPrice = NoCurrency($('.lineTotal', this).text()); //$('.lineTotal', this).text().replace(noCurrency, '');

	            $('.returnPricePerItem', this).val(NoCurrency($('.pricePerItem', this).text())); //$('.pricePerItem', this).text().replace(noCurrency, ''));
	            $('#parentTotal' + parentId).val(parseFloat(parentTotal) + parseFloat(previousPrice));

	            $('.lineTotal', this).text('$0,00');
	        }
	        Actualizar();
	        calculateRestockingAmount();
	    }

	    function calculateLineTotal(row) {
	        var checkBox = $('.returned', row);
	        if (checkBox.prop('checked')) {
	            var itemPrice = $('.pricePerItem', row).text();
	            var itemPrice = NoCurrency($('.pricePerItem', row).text());
	            $('.lineTotal', row).text('$' + ($('.returnQuantity', row).val() * NoCurrency($('.pricePerItem', row).text())).toFixed(2));

	            //				if (!checkBox.closest('tr').hasClass("childItem")) {
	            //					$('.lineTotal', row).text('$' + ($('.returnQuantity', row).val() * NoCurrency($('.pricePerItem', row).text())).toFixed(2));//$('.pricePerItem', row).text().replace(noCurrency, '')).toFixed(2));
	            //	            } 
	            //                else {
	            //					if (checkBox.closest('tr').prevAll('.parentItem:first').find('.returned').prop('checked')) {
	            //						$('.lineTotal', row).text('$0.00');
	            //					} else {
	            //		                $('.lineTotal', row).text('$' + ($('.returnQuantity', row).val() * NoCurrency($('.returnPricePerItem', row).val())).toFixed(2)); //$('.returnPricePerItem', row).val().replace(noCurrency, '')).toFixed(2));
	            //					}
	            //				}
	        } else {
	            $('.lineTotal', row).text('$0.00'); //@1 this=>row
	        }
	    }

	    function checkBalance() {
	        var paymentTotal = 0;
	        var grandTotal = NoCurrency($('.returnGrandTotal').text()); //$('.returnGrandTotal').text().replace(noCurrency, '');
	        $('#originalPayments .paymentRefund').each(function () {
	            paymentTotal += parseFloat($(this).val());
	        });

	        //			if (!$('input[value="check"]').prop('checked')) {
	        //				if (grandTotal > 0 && paymentTotal == grandTotal && $('.returned[type=checkbox]:checked').length > 0) {
	        //				    $('#btnSubmit').removeClass('ButtonOff');				    
	        //				} else if (!$('#btnSubmit').hasClass('ButtonOff') || $('.returned[type=checkbox]:checked').length == 0) {
	        //				    $('#btnSubmit').addClass('ButtonOff');				    
	        //				}
	        //			}
	    }

	    //@01 C04 - checkboxs functions
	    function selectReturnItemChecks() {
	        var rows = 0;
	        var checked = $('#ckbReturnItemHead').prop('checked');

	        $("#ckbReceivedItemHead").prop("checked", "");
	        $("#ckbReceivedItemHead").prop("disabled", !checked);

	        $('#products tbody:first tr').each(function () {

	            var rowClass = $(this).attr('class') == undefined ? '' : $(this).attr('class');
	            var isChild = rowClass.indexOf('childItem') != -1 ? true : false;

	            if (!isChild) {
	                $('.returned', this).prop('checked', checked);
	                $('.hasBeenReceived', this).prop('checked', false).prop('disabled', !checked);

	                if (checked) {
	                    calculateLineTotal(this);
	                }
	                else {
	                    $('.lineTotal', this).text('$0.00');
	                }
	            }
	            else {
	                $('.returned', this).prop('checked', false);
	                $('.hasBeenReceived', this).prop('checked', false).prop('disabled', true);
	                
	            }
	        });

	        validateEnableSubmit();
	    }

	    //Valida que este marcado la cabecera check si todos los items estan marcados
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

	        $('#ckbReturnItemHead').prop('checked', returnIsChecked);
	        $('#ckbReceivedItemHead').prop('checked', receivedIsChecked);
	        selectReceivedItemsCheks();
	    }

	    function selectReceivedItemsCheks() {
	        $('#products tbody:first tr').each(function () {
	            var rowClass = $(this).attr('class') == undefined ? '' : $(this).attr('class');
	            var isChild = rowClass.indexOf('childItem') != -1 ? true : false;
	            //if (!isChild) {
	                if ($('.returned', this).prop('checked'))
	                    $('.hasBeenReceived', this).attr('checked', $('#ckbReceivedItemHead').prop('checked'));
	            //}
	        });
	        validateEnableSubmit();
	    }

	    function evaluateReturnItemCheck() {
	        //if all items checked = totalItems
	        var totalItemsCount = $('#totalOriginalItemsCount').val();
	        var n = 0;
	        $('#products tbody:first tr').each(function () {
	            if ($('.returned', this).prop('checked'))
	                ++n;
	        });

	        if (n == totalItemsCount) {
	            $('#ckbReturnItemHead').attr('checked', true);
	        }
	        else {
	            $('#ckbReturnItemHead').attr('checked', false);
	        }
	        return n;
	    }

	    function evaluateReceivedItemCheck() {
	        //if all items checked = totalItems
	        var totalItemsCount = $('#totalOriginalItemsCount').val();
	        var n = 0;
	        $('#products tbody:first tr').each(function () {
	            if ($('.hasBeenReceived', this).prop('checked'))
	                ++n;
	        });

	        if (n == totalItemsCount) {
	            $('#ckbReceivedItemHead').attr('checked', true);
	        }
	        else {
	            $('#ckbReceivedItemHead').attr('checked', false);
	        }

	        return n;
	    }
	    //@01 C04 - End checkboxs functions

	    //@01 C13 - isCancelPaidOrder function  -->
	    function CheckCancelPaidOrder(_isCancelPaidOrder) {
	        if (_isCancelPaidOrder) {
	            $('#products tbody:first tr').each(function () {
	                $('.hasBeenReceived', this).attr('checked', !_isCancelPaidOrder).click();
	                $('.returned', this).attr('checked', !_isCancelPaidOrder).click();

	                $('.hasBeenReceived', this).prop('disabled', _isCancelPaidOrder);
	                $('.returned', this).prop('disabled', _isCancelPaidOrder);
	                $('.returnReasonId', this).prop('disabled', _isCancelPaidOrder);
	                $('.restockable', this).prop('disabled', _isCancelPaidOrder);
	                $('.returnQuantity', this).prop('disabled', _isCancelPaidOrder);
	            });

	            //Disable/enable all checks and items		       
	            $('#ckbReturnItemHead').attr('checked', _isCancelPaidOrder);
	            $('#ckbReceivedItemHead').attr('checked', _isCancelPaidOrder);

	            $('#ckbReturnItemHead').prop('disabled', _isCancelPaidOrder);
	            $('#ckbReceivedItemHead').prop('disabled', _isCancelPaidOrder);
	            $('#restockingPercent').prop('disabled', _isCancelPaidOrder);
	            $('#restockingAmount').prop('disabled', _isCancelPaidOrder);
	        }
	    }
	    //@01 C13 - isCancelPaidOrder function -->

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

	    function checkRefoundPayment() {
	        $('#originalPayments .paymentRefund').each(function () {
	            var maxAmount = parseFloat($(this).parent().find('.paymentAmount').val());
	            $(this).val(maxAmount.toFixed(2));
	        });

	        $('#originalPayments .paymentRefund').each(function () {
	            var maxAmount = parseFloat($(this).parent().find('.paymentAmount').val());
	            if ($(this).val() > maxAmount) {
	                $(this).val(maxAmount.toFixed(2));
	            }

	            var grandTotal = NoCurrency($('.returnGrandTotal').text()); //$('.returnGrandTotal').text().replace(noCurrency, '');
	            var paymentTotal = 0;
	            $('#originalPayments .paymentRefund').each(function () {
	                paymentTotal += parseFloat($(this).val());
	            });

	            //		        if (paymentTotal == grandTotal && $('.returned[type=checkbox]:checked').length > 0) {
	            //		            $('#btnSubmit').removeClass('ButtonOff');
	            //		            enablePedingConfirmButton(true); //@01 C09 - button Pending Confirm -->
	            //		        } else if (!$('#btnSubmit').hasClass('ButtonOff') || $('.returned[type=checkbox]:checked').length == 0) {
	            //		            $('#btnSubmit').addClass('ButtonOff');
	            //		            enablePedingConfirmButton(false); //@01 C10 -  button Pending Confirm -->
	            //		        }
	        });
	    }

	    function updateDeferred() {
	        var deferred = $.Deferred();
	        deferred.done(function () {
	            $('#btnUpdate').click();
	        });

	        deferred.resolve();

	        return deferred.promise();
	    }

	    function updateShippingDeferred() {
	        var deferred = $.Deferred();
	        deferred.done(function () {
	            $('#btnUpdateShipping').click();
	        });

	        deferred.resolve();
	        return deferred.promise();
	    }

	    function resultDeferred() {
	        calculateProductCredit();
	        checkRefoundPayment();
	    }
    </script>

	<% Order returnOrder = ViewData["ReturnOrder"] == null ? null : ((Order)ViewData["ReturnOrder"]);
	   if (returnOrder != null)
	   { %>
	<script type="text/javascript">
		$(function () {			
            
            /*******************************************************************************************************************************/
            var OrderStatusID = '<%= Model.OrderStatusID %>';

            /*
            * 8	Printed
            * 9	Shipped
            * 14	Cancelled Paid
            * 20	Invoiced
            */

            if (OrderStatusID == 8 || OrderStatusID == 9 || OrderStatusID == 14 || OrderStatusID == 20 || OrderStatusID == 19 ||OrderStatusID == 22) {
                $('#ckbReturnItemHead').prop('checked', true).trigger('click').prop('checked', true);

                $('.restockable').prop('checked', false);

                $('#products tbody tr[class^=childItem]').find('.returned, .returnReasonId, .hasBeenReceived, .restockable').hide();
                
                $('#products tbody tr td').attr('class', 'Disabled');
                $('#products input, select').attr('disabled', true);

                var ItemRowsCount = $('#products tbody: tr').length - 3;
                $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).each(function () {
                    if ($(this).attr('class') == 'parentItem' || $(this).attr('class') == undefined){
                        $(this).find('.hasBeenReceived').prop('checked', true);
		                calculateLineTotal(this);
                        validateEnableSubmit();
                    }
                    else
                        $(this).find('.lineTotal').text('$0.00');
		        });
            }
            else if (OrderStatusID == 21){ //21	Deliveried
                $('#products tbody tr[class^=childItem]').find('.returnReasonId').hide();
            }
            else{
                $('#products tbody tr[class^=childItem]').find('.returned').prop('disabled', true);
                $('#products tbody tr[class^=childItem]').find('.returnReasonId, .hasBeenReceived, .restockable').hide();
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
                
                rows.each(function (){

                    var childRow = $(this);
                    var productIDChild = childRow.find('input.productID').val();

                    if (productID == productIDChild){
                        
                        if (show){
                            childRow.show();
                        }
                        else{
                            childRow.hide();
                        }
                    }
                });
            });

            /*******************************************************************************************************************************/

            var isCancelPaidOrder = <%= Json.Encode((bool)ViewData["isCancelPaidOrder"]) %>; 
            
            if(isCancelPaidOrder){
                //CheckCancelPaidOrder(isCancelPaidOrder);
                var promise = updateDeferred();
		        promise.done(resultDeferred);
            }
            else {
                $('#btnUpdate').click(); // updating will populate all of our labels with the correct totals
            }
		    
			<% var returnPayment = returnOrder.OrderPayments.FirstOrDefault();
				if(returnPayment != null && returnPayment.PaymentTypeID == (int)Constants.PaymentType.Check)
				{ %>
			$('input[value="check"]').click();
			<%  } %>
            
		});

        function GetOrderItems() {
	        var OrderItems = JSON.parse('<%= Newtonsoft.Json.JsonConvert.SerializeObject(OrderItems) %>');
	        var ItemRowsCount = $('#products tbody: tr').length - 3;
	        var OrderItemList = [];

            var OrderStatusID = '<%= Model.OrderStatusID %>';
            var contandoComponentes = 0;
            var hasComponents = false;
            /*
            * 8	Printed
            * 9	Shipped
            * 14	Cancelled Paid
            * 20	Invoiced
            */

            var CantidadActivos = 0;
            var CantidadInactivos = 0;
            $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).each(function () {
            var isChecked = $(this).find('.returned').prop('checked');
            if (isChecked) {
                CantidadActivos++;
                }
                else{
                    if(isChecked != undefined )
                    {
                        CantidadInactivos++;
                    }
                }
            });

            var mostrarAler=true;
            if (OrderStatusID != 8 || OrderStatusID != 9 || OrderStatusID != 14 || OrderStatusID != 20 || OrderStatusID != 19|| OrderStatusID != 22) {
	            $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).each(function () {

	                var isChecked = $(this).find('.returned').prop('checked');
                    //var isCheckedRestockable = $(this).find('.restockable').prop('checked');
                    
                      var isCheckedTodos = $('#ckbReturnItemHead').prop('checked');
//                    if(mostrarAler==true)
//                    {
//                        alert("CantidadActivos => "+CantidadActivos +" | CantidadInactivos => "+CantidadInactivos + " | isCheckedTodos => "+ isCheckedTodos);
//                    }
//                    mostrarAler=false;
                    
	                if (isChecked && (CantidadActivos > 1 || CantidadInactivos > 1) ) {
                        //alert("isChecked"+isChecked + "      | CantidadActivos => "+CantidadActivos);
	                    var rowClass = $(this).attr('class') == undefined ? '' : $(this).attr('class');
	                    var isChild = rowClass.indexOf('childItem') != -1 ? true : false;
	                    var OrderItemID = $(this).find('.orderItemId').val();
                        
                        //var Quantity = $('.quantity', this).text() * -1;
                        var  Quantity = document.getElementById(OrderItemID).value* -1;
                        
	                    if (isChild) {
	                        var ParentOrderItemID = rowClass.replace('childItem', '').trim();
	                        for (var i = 0; i < OrderItems.length; i++) {
                                var orderItem = OrderItems[i];
  
                                //var orderItemClone = { ParentOrderItemID : orderItem.ParentOrderItemID, OrderItemID : orderItem.OrderItemID, ProductID : orderItem.ProductID, ParentQuantity : 0, Quantity : 0, ItemPrice : 0, HasComponents: false, AllHeader : false };
                                

                                //alert("orderItem.OrderItemID ==> " + orderItem.OrderItemID + " | orderItem.ItemPrice => "+ orderItem.ItemPrice + " | orderItem.SKU => " + orderItem.SKU + " | orderItem.Quantity => " + orderItem.Quantity);

	                            if (orderItem.OrderItemID == ParentOrderItemID) {
	                                orderItem.ItemPrice = 0.00;
                                    orderItem.HasComponents = true;
                                    orderItem.AllHeader = false;
                                    orderItem.IsChild = false;
                                   //alert("Get Head al check Child OrderItemID: " + orderItem.OrderItemID + " | hasComponents: "+ orderItem.HasComponents + " | AllHeader: "+orderItem.AllHeader+ "   | ParentQuantity => "+orderItem.ParentQuantity
                                   // + "   | SKU => "+orderItem.SKU    + "   | Quantity => " + Quantity + " | ItemPrice: " + orderItem.ItemPrice);
	                            }
	                            else if (orderItem.OrderItemID == OrderItemID) {
	                                orderItem.ItemPrice = NoCurrency($('.pricePerItem', this).text());
                                    orderItem.HasComponents = false;
                                    orderItem.AllHeader = false;
                                    orderItem.IsChild = true;

                                    //alert("222 {   OrderItemID: " + orderItem.OrderItemID + " | hasComponents: "+ orderItem.HasComponents + " | AllHeader: "+orderItem.AllHeader+ "   | ParentQuantity => " + orderItem.ParentQuantity
                                    //+ "   | SKU => " + orderItem.SKU    + "   | Quantity => " + Quantity + " | ItemPrice: " + orderItem.ItemPrice);
                                }
	                            else {
	                                continue;
	                            }
                                orderItem.Quantity = Quantity;
                                OrderItemList.push(orderItem);
	                        }
	                    }//Fin isChild
	                    else {
                        //Not isChild

                        

	                        for (var i = 0; i < OrderItems.length; i++) {
	                            var orderItem = OrderItems[i];

                                contandoComponentes = 0;
                                for (var indexComponentes = 0; indexComponentes < OrderItems.length; indexComponentes++) {
                                    if (OrderItems[indexComponentes].ProductID == orderItem.ProductID) contandoComponentes++;
                                }
                                hasComponents = (contandoComponentes > 1);
                                //var orderItemClone = { ParentOrderItemID : orderItem.ParentOrderItemID, OrderItemID : orderItem.OrderItemID, ProductID : orderItem.ProductID, ParentQuantity : 0, Quantity : 0, ItemPrice : 0, HasComponents: false, AllHeader : allHeader };

                                
                                //alert("isChild false => "+isChild+"orderItem.OrderItemID => "+orderItem.OrderItemID + " | OrderItemID: " + OrderItemID + " | orderItem.ParentOrderItemID => " + orderItem.ParentOrderItemID);
	                            if (orderItem.OrderItemID == OrderItemID) {
	                                orderItem.ItemPrice = NoCurrency($('.pricePerItem', this).text());
                                    orderItem.HasComponents = hasComponents;
                                    orderItem.AllHeader = true;
                                    orderItem.IsChild = false;

                                    //alert("xxx { OrderItemID: " + orderItem.OrderItemID + " | hasComponents: " + hasComponents + " | AllHeader: "+orderItem.AllHeader+ " | ParentQuantity => "+orderItem.ParentQuantity
                                    //+ " | SKU => " + orderItem.SKU    + " | Quantity => " + Quantity + " | ItemPrice: " + orderItem.ItemPrice);
	                            }
	                            else if (orderItem.ParentOrderItemID == OrderItemID) {
                                
                                    orderItem.AllHeader = true;
                                    orderItem.HasComponents = true;
                                    orderItem.IsChild = true;
	                                orderItem.ItemPrice = 0.00;
                                    //alert("abcd {   OrderItemID: " + orderItem.OrderItemID + " | hasComponents: " + orderItem.HasComponents + " | AllHeader: " + orderItem.AllHeader+ "   | ParentQuantity => "+orderItem.ParentQuantity
                                    //+ "   | SKU => " + orderItem.SKU    + "   | Quantity => " + Quantity + " | ItemPrice: " + orderItem.ItemPrice);
                                    //orderItemClone.ItemPrice = 0.00;
	                            }
	                            else {
	                                continue;
                                    }
                                orderItem.Quantity = Quantity;
                                OrderItemList.push(orderItem);

	                        }//fin For
	                    }//Fin Else
	                }//Fin isChecked
                    else
                    {
                        
                        if(isChecked != undefined && CantidadActivos == 1 && CantidadInactivos == 0)
                        {
                            //alert("isChecked"+isChecked+"  |  CantidadActivos: "+CantidadActivos+ "  |  CantidadInactivos: "+CantidadInactivos );    
                            var rowClass = $(this).attr('class') == undefined ? '' : $(this).attr('class');
	                        var isChild = rowClass.indexOf('childItem') != -1 ? true : false;

	                        var OrderItemID = $(this).find('.orderItemId').val();
                            var ParentOrderItemID = rowClass.replace('childItem', '').trim();
                            //var Quantity = $('.quantity', this).text() * -1;
                            
                            //alert("QuantityParent"+document.getElementById(ParentOrderItemID).value* -1);
                            var  QuantityChild = 0;
                            var QuantityParent = 0;
                            //if(ParentOrderItemID == "parentItem"){
                                QuantityChild = document.getElementById(OrderItemID).value* -1;
                            //}
//                            else{
//                                QuantityParent = document.getElementById(ParentOrderItemID).value* -1;
//                            }
                        
//                            alert("ParentOrderItemID"+ParentOrderItemID+ " | QuantityChild: "+QuantityChild + " | QuantityParent =>"+QuantityParent); 
                         
//                             if(ParentOrderItemID.length > 0)
//                             {
                                 for (var i = 0; i < OrderItems.length; i++) {
                                    var orderItem = OrderItems[i];

                                    if (orderItem.OrderItemID == ParentOrderItemID) {
	                                    orderItem.ItemPrice = 0.00;
                                        orderItem.HasComponents = true;
                                        orderItem.AllHeader = false;
                                        orderItem.IsChild = false;
                                        //alert("Get Head Unico Child OrderItemID: " + orderItem.OrderItemID + " | hasComponents: "+ orderItem.HasComponents + " | AllHeader: "+orderItem.AllHeader+ "   | ParentQuantity => "+orderItem.ParentQuantity
                                        //+ "   | SKU => "+orderItem.SKU    + "   | Quantity => " + Quantity + " | ItemPrice: " + orderItem.ItemPrice);
                                        //orderItemClone.ItemPrice = 0.00;
	                                }
                                    else
                                        if(orderItem.OrderItemID == OrderItemID){
                                            
                                            orderItem.ItemPrice = NoCurrency($('.pricePerItem', this).text());
                                            orderItem.HasComponents = false;
                                            orderItem.AllHeader = false;
                                            orderItem.IsChild = true;

                                            //alert(" 7890 {   OrderItemID: " + orderItem.OrderItemID + " | hasComponents: "+hasComponents + " | AllHeader: "+orderItem.AllHeader + "   | ParentQuantity => "+orderItem.ParentQuantity
                                            //+ "   | SKU => "+orderItem.SKU    + "   | Quantity => " + orderItem.Quantity + " | ItemPrice: " + orderItem.ItemPrice);
                                    
                                            
                                        }
                                        else {
	                                        continue;
	                                    }
                                        orderItem.Quantity = QuantityChild;
                                        OrderItemList.push(orderItem);
                                 }//End Foreach
//                             }

                        }


                    }
	            });
            }

            return OrderItemList;
	    }

        function NoCurrency(value){
            var result = value;
            result = result.replace('S/.', '');
            result = result.replace('€', '');
            result = result.replace('$', '');
            result = result.replace(',', '.');
            result = result.replace('R$', '');
            result = result.replace('R', '');
            return result.trim();
        }

        function validateKitItemsCheck(row,control){
            var isReturned = row.find('.returned').prop('checked');

            if (row.attr('class') != undefined){
                var isChild = row.attr('class').indexOf('childItem') != -1 ? true : false;
                var productID = row.find('input.productID').val();
                var checkAll = true;
                
                var kitRows = $('#products tbody tr').filter(function(){
                                    if ($(this).find('input.productID').val() == productID)
                                        return $(this);
                                });

                if (isChild){
                    var quantityItemChild = 0;
                     kitRows.filter('[class^=childItem]').find('input.hasBeenReceived').each(function(){
                        quantityItemChild++;
                    });

                    kitRows.filter('[class^=childItem]').find('input.returned').each(function(){
                        checkAll = $(this).prop('checked');
                        if (!checkAll) 
                            return false;
                    });

                    
                    var rowParent = kitRows.filter('[class^=parentItem]');

                    rowParent.find('.returned').prop('checked', checkAll);
                    var quantityParentMax = rowParent.find('.quantity').text().trim();
                    var quantityChildMax = parseInt(control.closest('tr').find('.quantity').text().trim(), 10);

                    var valorCheck = rowParent.find('.returned').prop('checked');
//                    alert("Check Papa   : " + valorCheck + "  | quantityItemChild: " + quantityItemChild  + "    | checkAll: " + checkAll );

                    if(valorCheck == undefined)
                    {
                            
                            rowParent.find('.returned, .hasBeenReceived').prop('checked', false);
                            rowParent.find('.hasBeenReceived').prop('disabled', false);
                            rowParent.find('.lineTotal').text('$0.00'); 

                            var quantity = row.find('.returnQuantity').val();
                            
						    var pricePerItem = NoCurrency(row.find('.pricePerItem').text());
                            var itemTotal = (quantity * pricePerItem).toFixed(2);

                            if (isReturned){
                                row.find('.lineTotal').text('$' + itemTotal);
                            }
                            
//                            alert("if(valorCheck == undefined) |   quantity: " + quantity + "   | quantityChildMax: "+quantityChildMax + "  | quantityParent:"+quantityParentMax);
                            row.find('.hasBeenReceived').prop('checked', false).prop('disabled', false);
                    }
                    else
                    {
                        if(quantityItemChild == 1 && quantityParentMax == quantityChildMax)
                        {
                            rowParent.find('.returned').prop('checked', checkAll);
                            rowParent.find('.hasBeenReceived').prop('disabled', false);
                            calculateLineTotal(rowParent);
                        
                            kitRows.filter('[class^=childItem]').each(function(){
                                var currentRow = $(this);
                                currentRow.find('.returned, .hasBeenReceived, .restockable').prop('checked', false);
                                currentRow.find('.hasBeenReceived').prop('disabled', true);
                                currentRow.find('.lineTotal').text('$0.00');
                                currentRow.find('.returnQuantity').val(currentRow.find('.quantity').text().trim());
                            });
                            
                            row.find('.hasBeenReceived').prop('checked', false).prop('disabled', true);
//                            alert("Parent==Child");
                        }
                        else
                        if (checkAll && quantityItemChild > 1){
                            //if (checkAll & quantityChild > 1){
                            rowParent.find('.returned').prop('checked', checkAll);
                            rowParent.find('.hasBeenReceived').prop('disabled', false);
                            calculateLineTotal(rowParent);
                        
                            kitRows.filter('[class^=childItem]').each(function(){
                                var currentRow = $(this);
                                currentRow.find('.returned, .hasBeenReceived, .restockable').prop('checked', false);
                                currentRow.find('.hasBeenReceived').prop('disabled', true);
                                currentRow.find('.lineTotal').text('$0.00');
                                currentRow.find('.returnQuantity').val(currentRow.find('.quantity').text().trim());
                            });
                            
                            row.find('.hasBeenReceived').prop('checked', false).prop('disabled', true);
//                            alert("Child>1");
                        }
                        else{
                            
                            rowParent.find('.returned, .hasBeenReceived').prop('checked', false);
                            rowParent.find('.hasBeenReceived').prop('disabled', true);
                            rowParent.find('.lineTotal').text('$0.00'); 
                            
                            
                            var quantity = row.find('.returnQuantity').val();
						    var pricePerItem = NoCurrency(row.find('.pricePerItem').text());
                            var itemTotal = (quantity * pricePerItem).toFixed(2);

                            if (isReturned){
                                row.find('.lineTotal').text('$' + itemTotal);
                            }
                            row.find('.hasBeenReceived').prop('checked', false).prop('disabled', !isReturned);

//                            alert("Check Unico Hijo quantityChildActual=> " + quantity + " |   quantityChildMax=> "+ quantityChildMax+ "  | quantityParent:"+quantityParentMax);
                        }
                        
                    }
                }
                else{
                    row.find('.hasBeenReceived').prop('checked', false).prop('disabled', !isReturned);
                    row.find('.lineTotal').text('$0.00'); 

                    var quantityParent = row.find('.returnQuantity').val();
					var pricePerItemParent = NoCurrency(row.find('.pricePerItem').text());
                    var itemTotalParent = (quantityParent * pricePerItemParent).toFixed(2);

                    if (isReturned){
                        row.find('.lineTotal').text('$' + itemTotalParent);
                    }

                    kitRows.filter('[class^=childItem]').each(function(){
                        var currentRow = $(this);
                        currentRow.find('.returned, .hasBeenReceived, .restockable').prop('checked', false);
                        currentRow.find('.hasBeenReceived').prop('disabled', true);
                        currentRow.find('.lineTotal').text('$0.00');
                        currentRow.find('.returnQuantity').val(currentRow.find('.quantity').text().trim());
                    });
                }
            }
            else{
                row.find('.hasBeenReceived').prop('checked', false).prop('disabled', !isReturned);
            }

            var ItemRowsCount = $('#products tbody: tr').length - 3;

            var checkHeader = true;

            $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).each(function(){
               var rowClass = $(this).attr('class') == undefined ? '' : $(this).attr('class');
		       var isChild = rowClass.indexOf('childItem') != -1 ? true : false;

               if(!isChild){
                    checkHeader = $(this).find('.returned').prop('checked');

                    if (!checkHeader)
                        return false;
               }

            });
           // alert("Activar ckbReturnItemHead => "+checkHeader);
            $('#ckbReturnItemHead').prop('checked', checkHeader);

            var ReturnedCount = $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).find('.returned:checked').length;
            var activarReceivedItemHead = (ReturnedCount != 0);
            $("#ckbReceivedItemHead").prop("disabled", !activarReceivedItemHead);

            calculateProductCredit();
            validateEnableSubmit();  
        }

        function validateEnableSubmit(){
            //$('#btnUpdate').click();
	        $('#btnUpdateShipping').click();

            var ClassOff = 'Button BigBlue SubmitOrder ButtonOff';
            var ClassOn = 'Button BigBlue SubmitOrder ButtonOn';

            var btnSubmit = $('#btnSubmit');
            var btnPendingConfirm = $('#btnPendingConfirm');

            var ItemRowsCount = $('#products tbody: tr').length - 3;
            var ReturnedCount = $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).find('.returned:checked').length;
            var ReceivedCount = $('#products tbody: tr').not(':first').slice(0, ItemRowsCount).find('.hasBeenReceived:checked').length;

            if (ReturnedCount > 0 && ReturnedCount > ReceivedCount){
                btnSubmit.attr('class', ClassOff);
                btnPendingConfirm.attr('class', ClassOn);
            }
            else if (ReturnedCount > 0 && ReturnedCount == ReceivedCount){
                btnSubmit.attr('class', ClassOn);
                btnPendingConfirm.attr('class', ClassOff);
            }
            else{
                btnSubmit.attr('class', ClassOff);
                btnPendingConfirm.attr('class', ClassOff);
            }
        }

         $(document).ready(function () {
	        var controlReturnType = $("#sReturnType");
	        controlReturnType.removeAttr("disabled");
	    });
    </script>
	<% }%>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Return", "Return") %></h2>
	</div>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Products", "Products") %>
			</td>
			<td>
				<!-- Products In Order -->
				<table id="products" class="DataGrid" width="100%">
					<thead>
						<tr class="GridColHead">
							<th>
								<%= Html.Term("Return Item", "Return Item") %>
                                <br />
                                 <%--@01 C01 - ckbReturnItemHead --%>
                                <input type="checkbox" id="ckbReturnItemHead" onclick="javascript:void(0);" />
							</th>
							<th>
								<%= Html.Term("Return Reason", "Return Reason") %>
							</th>
							<th>
								<%= Html.Term("Received Item", "Received Item") %>
                                <br />
                                <%--@01 C02 - ckbReceivedItemHead --%>
                                <%--<input type="checkbox" id="ckbReceivedItemHead" onclick="javascript:void(0);" style="display: none;"/>--%>
                                <input type="checkbox" id="ckbReceivedItemHead" onclick="javascript:void(0);" disabled="disabled" />
							</th>
							<th>
								<%= Html.Term("Restockable", "Restockable") %>
							</th>
							<th>
								<%= Html.Term("SKU", "SKU") %>
							</th>
                            <th>
								<%= Html.Term("MaterialCode", "Material Code")%>
							</th>
							<th>
								<%= Html.Term("Product", "Product") %>
							</th>
							<th>
								<%= Html.Term("Quantity", "Quantity") %>
							</th>
							<th>
								<%= Html.Term("ReturnQuantity", "Return Quantity") %>
							</th>
							<th>
								<%= Html.Term("PricePerItem", "Price Per Item") %>
							</th>
							<th>
								<%= Html.Term("ReturnedPrice", "Returned Price") %>
							</th>
							<th>
								<%= Html.Term("Total", "Total") %>
							</th>
						</tr>
					</thead>
					<tbody>
						<%			
							var inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
                            
							List<OrderProductAmount> returnableProductsInfo = ViewData["ReturnableProducts"] as List<OrderProductAmount>;
							List<OrderItem> returnedItems = ViewData["ReturnedItems"] as List<OrderItem>;

                            List<NetSteps.Data.Entities.EntityModels.OrderReturnTable> listaOrderItem = ViewData["listaOrderItem"] as List<NetSteps.Data.Entities.EntityModels.OrderReturnTable>;
                            List<NetSteps.Data.Entities.EntityModels.OrderReturnChildrenTable> listaOrderItemChildren = ViewData["listaOrderItemChildren"] as List<NetSteps.Data.Entities.EntityModels.OrderReturnChildrenTable>;
                            
							foreach (OrderCustomer customer in Model.OrderCustomers)
							{%>
						<tr>
							<td colspan="12">
								<h3>
									<%=customer.FullName %></h3>
							</td>
						</tr>
						<%
                                
                                
                                
								foreach (OrderItem orderItem in customer.ParentOrderItems)
								{
									var returnableProductInfo = returnableProductsInfo.FirstOrDefault(p => p.OrderItemID == orderItem.OrderItemID);

									var orderItemTotalPaid = orderItem.GetAdjustedPrice(orderItem.ProductPriceTypeID ?? customer.ProductPriceTypeID) * orderItem.Quantity;
									var product = inventory.GetProduct(orderItem.ProductID.ToInt());

									// consultant has selected to return this item but has not submitted the return order yet.
									OrderItem returnItem = returnOrder == null ? null : 
                                        returnOrder.OrderCustomers.SelectMany(w => w.OrderItems).FirstOrDefault(ri => ri.OrderItemReturns != null 
                                            && ri.OrderItemReturns.Any() && ri.OrderItemReturns[0].OriginalOrderItemID == orderItem.OrderItemID);

									OrderItemReturn itemReturn = null;
									if (returnItem != null)
										itemReturn = OrderItemReturn.LoadByOrderItemID(returnItem.OrderItemID);

									// finding if this item has been returned before
                                      OrderItemReturn orderItemReturns =
                                        returnedItems.SelectMany(ri => ri.OrderItemReturns).FirstOrDefault(
                                            roi => roi.OriginalOrderItemID == orderItem.OrderItemID);
                                    
                                    var orderItemReturn = listaOrderItem.Where(donde => donde.ProductID == orderItem.ProductID).FirstOrDefault();
                                    //OrderItem previouslyReturnedItem = returnedItems.Count > 0 && orderItemReturn != null ? 
                                    //    returnedItems.FirstOrDefault(ri1 => ri1.OrderItemReturns != null && ri1.OrderItemReturns.Any() && 
                                    //        orderItemReturn.OrderItemID == ri1.OrderItemReturns[0].OrderItemID) : null;
                                    
                                    OrderItem previouslyReturnedItem = returnedItems.Count > 0 && orderItemReturn != null ?
                                        returnedItems.FirstOrDefault(ri1 => ri1.OrderItemReturns != null && ri1.OrderItemReturns.Any() &&
                                            orderItemReturn.ProductID == ri1.OrderItemReturns[0].OrderItemID) : null;
                                    
									bool isLineItemEditable = true;

									bool isReturningChildItemsIndividually = false;

									if (orderItem.HasChildOrderItems)
									{
										isReturningChildItemsIndividually = orderItem.ChildOrderItems.Any(childOrderItem => returnedItems.Any(ri => ri.OriginalOrderItemReturns.Count > 0 && ri.OrderItemReturns[0].OriginalOrderItemID == childOrderItem.OrderItemID));
									}
									// if qty is 0 then we have returned all of the products for this item.
                                    if (returnableProductInfo != null && returnableProductInfo.Quantity == 0)
                                    {
                                        isLineItemEditable = false;
                                    }
                                    else
                                        if (orderItemReturn != null)
                                        {
                                            if (orderItem.Quantity - orderItemReturn.QuantityReturn == 0)
                                                isLineItemEditable = false;
                                        }

									int lineItemCount = 1;
									if (isLineItemEditable && previouslyReturnedItem != null)
									{
										// we have some items returned and others not 
										// we need 2 line items.
										lineItemCount = 2;
									}
									
                                    int Index=0;
									for (int i = 0; i < lineItemCount; i++)
									{
                                        Index++;
                                        
						%>
						<tr <%=orderItem.HasChildOrderItems ? "class=\"parentItem\"" : "" %>>

                            <%--Check Return--%>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<input type="hidden" class="productID" value="<%=orderItem.ProductID%>" />
								<input type="hidden" class="orderItemId" value="<%=orderItem.OrderItemID%>" />
								<input type="hidden" class="orderCustomerId" value="<%=orderItem.OrderCustomerID%>" />
								<%
                                        if (isLineItemEditable && !isReturningChildItemsIndividually && !orderItemReturn.BlockHead)
                                        //if (isLineItemEditable && !isReturningChildItemsIndividually)
									{%>
								<input type="checkbox" class="returned" <%--onclick="$(this).closest('tr').find('.restockable').attr('checked', $(this).prop('checked'));"--%>
									<%=itemReturn != null ? "checked=\"checked\"" : ""%> data-parentid="<%= orderItem.OrderItemID %>" 
                                    <%--id = "chkReturn<%= orderItem.OrderItemID %>" --%>
                                    <%--<%=orderItemReturn.BlockHead ? "class=\"Disabled\"" : ""%>--%>
                                    />
								<%
									}%>
							</td>
                            
                            <%--Motivo Retorno--%>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<%
									if (isLineItemEditable && !isReturningChildItemsIndividually)
									{%>
								<select class="returnReasonId">
									<%
										foreach (var returnReason in SmallCollectionCache.Instance.ReturnReasons)
										{ %>
									<option value="<%=returnReason.ReturnReasonID%>" <%=itemReturn != null &&
															  itemReturn.ReturnReasonID == returnReason.ReturnReasonID
																  ? "selected=\"selected\""
																  : ""%>>
										<%= returnReason.GetTerm() %></option>
									<%
										}%>
								</select>
								<%
									}
									else if (itemReturn != null)
									{%><%=
											SmallCollectionCache.Instance.ReturnReasons.FirstOrDefault(
												rr => rr.ReturnReasonID == itemReturn.ReturnReasonID).
												GetTerm()%>
								<%
									}
									else if (previouslyReturnedItem != null)
									{%>
								<%=
											SmallCollectionCache.Instance.ReturnReasons.FirstOrDefault(
												rr =>
												rr.ReturnReasonID ==
												previouslyReturnedItem.OrderItemReturns[0].ReturnReasonID)
												.GetTerm()%>
								<%
									}

                                    if (orderItem.ChildOrderItems.Count > 0)
                                    {       
                                %>
                                    <span class="ClearAll"></span><a class="ViewKitContents TextLink Add" href="javascript:void(0);">
								    <%= Html.Term("ViewKitContents", "View Kit Contents")%></a>
                                <%
                                    }    
                                %>
							</td>

                            <%--Check Recibido--%>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<%
									if (isLineItemEditable && !isReturningChildItemsIndividually)
									{%>
								<input type="checkbox" class="hasBeenReceived" disabled="disabled" />
                                                        <%--<%=(itemReturn != null && itemReturn.HasBeenReceived) ||
														  (previouslyReturnedItem != null &&
														   !orderItem.HasChildOrderItems && !isLineItemEditable)
															  ? "checked=\"checked\""
															  : ""%> <%=previouslyReturnedItem != null && !isLineItemEditable ? "disabled=\"disabled\"" : ""%> />--%>
								<%
									}%>
							</td>

                            <%--Check Restockable--%>
							<td class="Disabled">
								<%
									if (isLineItemEditable && !isReturningChildItemsIndividually)
									{%>
								<input type="checkbox" class="restockable" disabled="disabled" <%=itemReturn != null && itemReturn.IsRestocked
															  ? "checked=\"checked\""
															  : ""%> />
								<%
									}%>
							</td>

                            <%--SKU--%>
							<td style="width:80px;" <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<%= orderItem.SKU%>
							</td>

                            <%--Codigo Material--%>
                            <td style="width:80px;" <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								&nbsp;
							</td>

                            <%--Nombre del Producto--%>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<%= product.Name%>
								<%
									if (product.IsDynamicKit())
									{%>
								<a class="btnAddToBundle" href="javascript:void(0);"><span class="UI-icon icon-bundle-full">
								</span></a>
								<%
									}%>
							</td>

                            <%--Cantidad Maxima Solo Lectura--%>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
                            <%if( !orderItemReturn.BlockHead)
                              {%>
								<span class="quantity">
									<%--<%=isLineItemEditable ? orderItem.Quantity : previouslyReturnedItem != null ? previouslyReturnedItem.Quantity : 0%>--%>
                                    <%=isLineItemEditable ? orderItem.Quantity - (orderItemReturn!=null? orderItemReturn.QuantityReturn :0): previouslyReturnedItem != null ? previouslyReturnedItem.Quantity : 0%>
								</span>
                                <%}%>
							</td>

                            <%--Cantidad TextBox--%>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<%
            
                                        if (isLineItemEditable && !orderItemReturn.BlockHead)
									{%>
								<%--<input type="text" value="<%=returnItem == null ? orderItem.Quantity : returnItem.Quantity%>"--%>
                                <input type="text" value="<%=returnItem == null ? orderItem.Quantity - (orderItemReturn!=null? orderItemReturn.QuantityReturn :0) : returnItem.Quantity%>"
									class="TextInput numeric returnQuantity" style="width: 25px;" onfocus="GetValueQuantity(value,<%=orderItem.OrderItemID%>);" 
                                    onblur="UpdateQuantity(value,<%=orderItem.OrderItemID%>);" 
                                    onkeypress="if (event.keyCode < 48 || event.keyCode > 57) event.returnValue = false;"
                                    id = "<%= orderItem.OrderItemID %>" 
                                    name="returnQuantity"/>
								<%
									}%>
							</td>

                            <%--Valor--%>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
                            <%if (Model.OrderStatusID == 19 && !orderItemReturn.BlockHead)
                              {%>
                                <span class="pricePerItem"><%=orderItem.ItemPrice.ToString(Model.CurrencyID) %></span>
                              <%}
                              else
                              {
                                  if (orderItem.OrderItemPrices.Count() > 0 && !orderItemReturn.BlockHead)
                                  {%>                                  
								<span class="pricePerItem"><%= orderItem.OrderItemPrices.Where(op => op.ProductPriceTypeID == (orderItem.ProductPriceTypeID ?? customer.ProductPriceTypeID)).FirstOrDefault().UnitPrice.ToString(Model.CurrencyID)%></span>
                              <%}
                              }
                                        if (orderItem.OrderItemPrices.Count() > 0 && !orderItemReturn.BlockHead)
                                        {
                               %>
								<input type="hidden" class="returnPricePerItem" value="<%= orderItem.OrderItemPrices.Where(op => op.ProductPriceTypeID == (orderItem.ProductPriceTypeID ?? customer.ProductPriceTypeID)).FirstOrDefault().UnitPrice.ToString(Model.CurrencyID) %>" />
								<% var orderItemPrice = orderItem.OrderItemPrices.Where(op => op.ProductPriceTypeID == customer.CommissionablePriceTypeID).FirstOrDefault();
                                   decimal unitPrice = 0m;
                                   if (orderItemPrice != null)
                                       unitPrice = orderItemPrice.UnitPrice;  %>
								<input type="hidden" class="returnCVPerItem" value="<%= unitPrice %>" />
                              <%}%>           
                                     </td>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<%=previouslyReturnedItem != null && !isLineItemEditable
																								  ? (previouslyReturnedItem.GetAdjustedPrice(orderItem.ProductPriceTypeID ?? customer.ProductPriceTypeID) * previouslyReturnedItem.Quantity).ToString("C")
														  : ""%>
							</td>
							<td <%=!isLineItemEditable ? "class=\"Disabled\"" : ""%>>
								<span class="lineTotal">$0.00</span>
							</td>
						</tr>
						<%
									if (lineItemCount > 1)
										isLineItemEditable = false;
								}
                                    
                          
								foreach (OrderItem childItem in orderItem.ChildOrderItems)
								{
                                    /*CS.05AG2016.Inicio.Obtener Componentes*/
                                    var orderItemReturnChildren = listaOrderItemChildren.Where(donde => donde.MaterialSKU == childItem.SKU).FirstOrDefault();
                                    /*CS.05AG2016.Fin.Obtener Componentes*/
                                    
									var childReturnableProductInfo =
										returnableProductsInfo.FirstOrDefault(
											p => p.OrderItemID == childItem.OrderItemID);
									var childProduct = inventory.GetProduct(childItem.ProductID.ToInt());
									OrderItem childReturnItem = returnOrder == null
																	? null
																	: returnOrder.OrderCustomers.SelectMany(
																		w => w.OrderItems).FirstOrDefault(
																			ri => ri.OrderItemReturns.Count > 0 && ri.OrderItemReturns[0].OriginalOrderItemID == childItem.OrderItemID);
									OrderItem previouslyReturnedChildItem = returnedItems.Count > 0
																				? returnedItems.FirstOrDefault(
																					ri => ri.OrderItemReturns.Count > 0 &&
																					ri.OrderItemReturns[0].
																						OriginalOrderItemID ==
																					childItem.OrderItemID)
																				: null;
									OrderItemReturn childItemReturn = null;
									if (childReturnItem != null)
										childItemReturn = OrderItemReturn.LoadByOrderItemID(childReturnItem.OrderItemID);

									bool isChildLineItemEditable = true;
									// if qty is 0 then we have returned all of the products for this item.
									if (childReturnableProductInfo.Quantity == 0)
									{
										isChildLineItemEditable = false;
									}
                                    else
                                        if (orderItemReturnChildren != null)
                                        {
                                            if (childItem.Quantity - orderItemReturnChildren.QuantityReturn == 0)
                                                isChildLineItemEditable = false;
                                        }
                                    
									if (previouslyReturnedChildItem != null)
									{
										orderItemTotalPaid -= previouslyReturnedChildItem.ItemPrice * previouslyReturnedChildItem.Quantity;
									}

									int childLineItemCount = 1;
									if (isChildLineItemEditable && previouslyReturnedChildItem != null)
									{
										// we have some items returned and others not 
										// we need 2 line items.
										childLineItemCount = 2;
									}
									for (int i = 0; i < childLineItemCount; i++)
									{%>
						<tr class="childItem <%=orderItem.OrderItemID%>">
							<td>
								<span class="UI-icon icon-bundle-arrow"></span>
								<input type="hidden" class="productID" value="<%=childItem.ProductID%>" />
								<input type="hidden" class="orderItemId" value="<%=childItem.OrderItemID%>" />
								<input type="hidden" class="dynamicKitGroupId" value="<%=childItem.DynamicKitGroupID%>" />
								<%
										if (isChildLineItemEditable)
										{%>
								<input type="checkbox" class="returned" <%--onclick="$(this).closest('tr').find('.restockable').attr('checked', $(this).prop('checked'));"--%>
									<%=childItemReturn != null ? "checked=\"checked\"" : ""%> data-parentid="<%= orderItem.OrderItemID %>" />
								<%
										}%>
							</td>
							<td <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<%
										if (isChildLineItemEditable)
										{%>
								<select class="returnReasonId">
									<%
											foreach (var returnReason in SmallCollectionCache.Instance.ReturnReasons)
											{ %>
									<option value="<%=returnReason.ReturnReasonID%>" <%=childItemReturn != null &&
																  childItemReturn.ReturnReasonID ==
																  returnReason.ReturnReasonID
																	  ? "selected=\"selected\""
																	  : ""%>>
										<%= returnReason.GetTerm() %></option>
									<%
											}%>
								</select>
								<%
										}
										else if (previouslyReturnedChildItem != null &&
												 previouslyReturnedChildItem.OrderItemReturns[0].OriginalOrderItemID ==
												 childItem.OrderItemID)
										{%>
								<%=
												SmallCollectionCache.Instance.ReturnReasons.FirstOrDefault(
													rr =>
													rr.ReturnReasonID ==
													previouslyReturnedChildItem.OrderItemReturns.FirstOrDefault(
														oir => oir.OriginalOrderItemID == childItem.OrderItemID).
														ReturnReasonID).GetTerm()%>
								<%
										}%>
							</td>
							<td>
								<input type="checkbox" class="hasBeenReceived" disabled="disabled" />
                                                        <%--<%=(childItemReturn != null && childItemReturn.HasBeenReceived) ||
														  !isChildLineItemEditable
															  ? "checked=\"checked\""
															  : ""%> <%=!isChildLineItemEditable
															  ? "disabled=\"disabled\""
															  : ""%> />--%>
							</td>
							<td class="Disabled">
								<%
										if (isChildLineItemEditable)
										{%>
								            <input type="checkbox" class="restockable" disabled="disabled" />
                                                            <%-- <%=childItemReturn != null && childItemReturn.IsRestocked
																  ? "checked=\"checked\""
																  : ""%> />--%>
								<%
										}%>
							</td>
							<td style="width:80px;" <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								&nbsp;
							</td>
                            <td style="width:80px;" <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<%= childItem.SKU%>
							</td>
							<td <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<%= childItem.ProductName %>
							</td>
							<td <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<span class="quantity">
                                    <%--<%= childReturnableProductInfo.Quantity %>--%>
									<%= childReturnableProductInfo.Quantity - (orderItemReturnChildren != null ? orderItemReturnChildren.QuantityReturn : 0)%>
                                </span>
							</td>
							<td <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<%
										if (isChildLineItemEditable)
										{%>

                                    <input type="text" value="<%=childReturnItem == null
																  ? childItem.Quantity - (orderItemReturnChildren != null ? orderItemReturnChildren.QuantityReturn : 0)
																  : childReturnItem.Quantity - (orderItemReturnChildren != null ? orderItemReturnChildren.QuantityReturn : 0)%>" class="TextInput numeric returnQuantity"  onfocus="GetValueQuantity(value,"<%=orderItem.OrderItemID%>");" 
                                                                  onblur="UpdateQuantity(value,<%=childItem.OrderItemID%>);" 
                                                                  onkeypress="if (event.keyCode < 48 || event.keyCode > 57) event.returnValue = false;"
                                                                  id = "<%= childItem.OrderItemID %>"
                                                                  name="returnQuantity"
									style="width: 25px;" />

								<%
										}
										else
										{%><%=0%><%
										}%>
							</td>
							<td <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<span class="pricePerItem">
									<%= childItem.ItemPrice.ToString("C")%></span>
								<input type="hidden" class="returnPricePerItem" value="<%=childItem.ItemPrice.ToString("C")%>" />
								 <% 
                                    var orderItemPrice = orderItem.OrderItemPrices.Where(op => op.ProductPriceTypeID == customer.CommissionablePriceTypeID).FirstOrDefault();
                                    decimal unitPrice = 0m;
                                    
                                    if (orderItemPrice != null)
                                       unitPrice = orderItemPrice.UnitPrice;  
                                %>
                                <input type="hidden" class="returnCVPerItem" value="<%= unitPrice %>" />
							</td>
							<td <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<%=
											previouslyReturnedChildItem == null || isChildLineItemEditable
												? ""
												: previouslyReturnedChildItem.ReturnedPrice.ToString(Model.CurrencyID)%>
							</td>
							<td <%=!isChildLineItemEditable
															  ? "class=\"Disabled\""
															  : ""%>>
								<span class="lineTotal">$0.00</span>
							</td>
						</tr>
						<%if (childLineItemCount > 1)
							  isChildLineItemEditable = false;
									}
								}%>
						<input type="hidden" id="parentTotal<%= orderItem.OrderItemID %>" value="<%= orderItemTotalPaid %>" />
						<% } %>
						<% } %>
					</tbody>
					<tbody>
						<tr class="GridTotalBar">
							<td colspan="9">
							</td>
							<td class="FLabel">
								<%= Html.Term("Restocking Fee", "Restocking Fee") %>
							</td>
							<td class="nowrap">
								%<input id="restockingPercent" type="text" value="0.00" class="TextInput numeric"
									style="width: 50px" />
							</td>
							<td class="nowrap">
								<% 
									OrderItem restockItem = null;
									string restockingFeeSku = string.Empty;
									Product restockingFeeProduct = Order.GetRestockingFeeProduct();
									if (restockingFeeProduct != null)
									{
										restockingFeeSku = restockingFeeProduct.SKU;
									}
									if (returnOrder != null) restockItem = returnOrder.OrderCustomers.SelectMany(w => w.OrderItems).FirstOrDefault(ri => ri.SKU == restockingFeeSku);						
								%>
								<% decimal zero = 0m; %>
								$<input id="restockingAmount" type="text" value="<%= restockItem == null ? zero.ToString() : (restockItem.ItemPrice * -1).ToString("N") %>"
									class="TextInput numeric" style="width: 50px" />
							</td>
						</tr>
						<tr class="GridTotalBar">
							<td colspan="10">
							</td>
							<td class="nowrap">
								<a id="btnUpdate" class="DTL Update" href="javascript:void(0);" style="visibility:hidden" >
									<%= Html.Term("Update", "Update") %></a>
							</td>
							<td class="nowrap">
                           <%if (Model.OrderStatusID == 21)
                             {%>                                 
								<b><span class="subtotal">0.00</span> (<%= Html.Term("Sub Total", "Sub Total")%>)</b>
                             <%}
                             else
                             { %>
								<b><span class="subtotal"><%=Model.Subtotal.ToString(Model.CurrencyID)%></span> (<%= Html.Term("Sub Total", "Sub Total")%>)</b>
                                <% }%>
							</td>
						</tr>
					</tbody>
				</table>
			</td>
		</tr>
	</table>
	<table class="FormTable Section" width="100%">
		<% Currency currency = SmallCollectionCache.Instance.Currencies.GetById(Model.CurrencyID); %>
		<tr>
			<td class="FLabel">
				<%= Html.Term("Refund", "Refund") %>
				<img class="Loading" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>"
					alt="loading..." style="float: right; position: relative; top: 25px;" />
			</td>
			<td>
				<table width="100%" class="DataGrid">
					<tr>
						<td>
							<table width="50%" class="DataGrid FL">
								<tr class="GridColHead">
									<th colspan="2">
										<%= Html.Term("Refunded Totals", "Refunded Totals") %>
									</th>
								</tr>
								<tr>
									<td style="text-align: right; width: 120px; vertical-align: top;">
										<%= Html.Term("Refunded Subtotal", "Refunded Subtotal") %>:<br />
										<%= Html.Term("Refunded Tax", "Refunded Tax") %>:<br />
										<%= Html.Term("RefundedS&amp;H", "Refunded S&amp;H") %>:
									</td>
									<td>
										<span class="subtotal">
											<%=Model.Subtotal.ToString(Model.CurrencyID)%></span><br />
										<span class="taxTotal">
											<%= 0M.ToString(Model.CurrencyID) %></span><br />
										<%= currency.CurrencySymbol %><input id="shippingRefunded" type="text" value="0.00"
											class="TextInput numeric" />
										<input type="hidden" id="maxShippingRefund" value="0.00" />
                                        <%--CS.27JUL2016.Inicio.Comentado--%>
										<%--<a id="btnUpdateShipping" class="DTL Update" href="javascript:void(0);">
											<%= Html.Term("Update", "Update") %></a>--%>
                                            <%--CS.27JUL2016.Fin.Comentado--%>
									</td>
								</tr>
							</table>
							<table width="50%" class="DataGrid FL">
								<tr class="GridColHead">
									<th colspan="2">
										<%= Html.Term("OriginalTotals", "Original Totals") %>
									</th>
								</tr>
								<tr>
									<td style="text-align: right; width: 120px; vertical-align: top;">
										<%= Html.Term("OriginalSubtotal", "Original Subtotal") %>:
										<br />
										<%= Html.Term("OriginalTax", "Original Tax") %>:
										<br />
										<%= Html.Term("OriginalS&amp;H", "Original S&amp;H") %>:
									</td>
									<td style="vertical-align: top;">
										<%= Model.Subtotal.ToDecimal().ToString(Model.CurrencyID) %><br />
										<%= (Model.TaxAmountTotalOverride != null) ? Model.TaxAmountTotalOverride.ToDecimal().ToString(currency) : Model.TaxAmountTotal.ToDecimal().ToString(currency)%><br />
										<% 
											decimal shipTotal = Model.ShippingTotalOverride ?? Model.ShippingTotal ?? 0;
											decimal handlingTotal = Model.HandlingTotal ?? 0;
										%>
										<%= (shipTotal + handlingTotal).ToString(currency) %><br />
									</td>
								</tr>
							</table>
						</td>
					</tr>
				</table>
				<div class="GridTotalBar ClearAll" style="padding: 5px;">
					<b>
						<%= Html.Term("TotalAmountToBeRefunded", "Total Amount To Be Refunded") %>:</b>
					<b style="color: #006600" class="returnGrandTotal"><%=(Model.ShippingTotal + Model.Subtotal).ToString(Model.CurrencyID)%></b>
				</div>
			</td>
		</tr>
	</table>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("ApplyRefundTo", "Apply Refund To") %>
			</td>
			<td style="text-align: left;">
         <%--       
				<div id="originalPayments" class="FL AddressProfile Default" style="min-width: 0px;">
					<input id="originalPayment" name="paymentType" class="Radio" type="radio" value="originalPayment"
						checked="checked" />
					<b>
						<%= Html.Term("OriginalPaymentMethod(s)", "Original Payment Method(s)") %>:
					</b>
					<br />
					<%
						var originalOrder = ViewData["OriginalOrder"] as Order;
						var previousPayments =
							ViewData["PreviousReturnPayments"] as List<nsCore.Areas.Orders.Models.PreviousReturnOrderPayment>;

						foreach (OrderPayment orderPayment in originalOrder.OrderPayments.Where(op => op.OrderPaymentStatusID == Constants.OrderPaymentStatus.Completed.ToShort()))
						{%>
					<%=orderPayment.OrderCustomer != null ? orderPayment.OrderCustomer.FullName:orderPayment.NameOnCard%>
					<br />
					<%= orderPayment.DecryptedAccountNumber.MaskString(4) %>
					<% var previousPayment = previousPayments.Any(pp => pp.AccountNumber == orderPayment.MaskedAccountNumber) ? previousPayments.FirstOrDefault(pp => pp.AccountNumber == orderPayment.MaskedAccountNumber) : null; %>
					<% if(previousPayment != null)
					   { %>
					   (<%= (Math.Max(orderPayment.Amount + previousPayment.Amount, 0)).ToString("C")%>)
					   <% previousPayment.Amount = Math.Min(orderPayment.Amount + previousPayment.Amount, 0); %>
					<% }
					   else
					   { %>
					   (<%= orderPayment.Amount.ToString("C")%>)
					<% } %>
					<br />
					<% decimal savedPaymentAmt = 0.00m;
					   if (returnOrder != null && returnOrder.AllOrderPayments.Any(op => op.AccountNumber == orderPayment.AccountNumber))
					   {
						   savedPaymentAmt = returnOrder.OrderPayments.First(op => op.AccountNumber == orderPayment.AccountNumber).Amount;
					   } %>
					<span style="margin-left: 7px;">$<input type="text" id="payment<%= orderPayment.OrderPaymentID %>"
						class="TextInput numeric paymentRefund" value="<%= savedPaymentAmt.ToString("F") %>" />
						<input type="hidden" class="paymentAmount" value="<%= orderPayment.Amount %>" /></span><br />
					<%} %>
				</div>
				<!-- Manual Check Option -->
				<div class="FL AddressProfile Default" style="margin-left: 40px; min-width: 0px;">
					<input name="paymentType" class="Radio" type="radio" value="check" />
					<%= Html.Term("Manual Check", "Manual Check") %>
				</div>
				<!-- End Manual Check Option -->
                                --%>
                <!-- @01 C05 - Product Credit Option TODO: Bind -->
				<div class="FL AddressProfile Default" style="margin-left: 40px; min-width: 0px;">
					<input id="creditType" name="paymentType" class="Radio" type="radio" value="creditType" checked="checked" />
                    <%= Html.Term("ProductCredit", "Product Credit")%>
                    <br />
                    <% string amountString = ViewData["ProductCreditAmountString"].ToString();
                       decimal creditamount = decimal.Parse(ViewData["ProductCreditAmount"].ToString());
                       %>
                    <div id="amountCredit"><%=(Model.ShippingTotal + Model.Subtotal).ToString(Model.CurrencyID)%>
                    </div> 
                    <input type="hidden" class="productCreditAmount" value="<%= creditamount %>" />
				</div>
				<!-- @01 C05 - End Product Credit Option TODO: Bind -->
				<br />
			</td>
		</tr>
	</table>
	<table class="FormTable Section" width="100%">
		<tr>
			<td class="FLabel">
				<%= Html.Term("Notes", "Notes") %>
			</td>
			<td>
				<p class="FL">
					<%= Html.Term("ReturnType", "Return Type") %>:<br />
					<%--<select id="sReturnType" class="TextInput">--%>
                    <select id="sReturnType">
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
					<textarea id="notes" class="TextInput" rows="5" cols="50"><%= returnOrder != null && returnOrder.InvoiceNotes != null ? returnOrder.InvoiceNotes.Trim() : "" %></textarea>
				</p>
				<span class="ClearAll"></span>
                 <% if (Model.OrderStatusID == (int)Constants.OrderStatus.Shipped)
                   {%>
                    <!-- @01 C06 - Return Order Pending -->

                      <%Func<string, string> _MostrarOcultar = (obj) =>
                            {
                                    string estilo = (obj == "0") ? "style='display:none'" : "";
                                    return estilo;
                            }; %>

                    <p <%=_MostrarOcultar( Convert.ToString(ViewData["SupportTicketNumber"]) ) %> class="FL">
					    <%= Html.Term("NroTicketSupport", "N° Ticket Support")%>:<br />
                        <% var supportTicket = ViewData["SupportTicketNumber"]; %>
                     
                        <input  id="HdfSupportTicketID" value="<%=ViewData["SupportTicketID"] %>"  type="hidden" />

                        <input type="text" name="IDSupportTicket" id="IDSupportTicket" value="<%= supportTicket %>" readonly/>
                        <input type="hidden" id = "IDSupportTicketValidation" name="IDSupportTicketValidation" value="<%= Html.Term("IdSupportTicketValidation", "Please enter a support ticket valid") %>" />
				    </p>
				    <span class="ClearAll"></span>
                    <p <%=_MostrarOcultar(Convert.ToString( ViewData["SupportTicketID"])) %>  class="FL">
                        <%= Html.Term("NroCorreoNacional", "N° Correo Nacional") %>:<br />
                        <input type="text" name="IDNationalMail" id="IDNationalMail" value=" " />
				    </p>
				    <span class="ClearAll"></span>
                    <!-- @01 C06 - End Return Order Pending -->
                <%} %>
			</td>
		</tr>
	</table>
	<table class="FormTable" width="100%">
		<tr>
			<td class="FLabel">
			</td>
			<td>
				<div>
                    <div>
					<!--/ End Entry Form -->
<%--					<span>
						<%= Html.Term("NewReturnOrderNumber", "New return order number") %>:
						<%= ((Order)ViewData["ReturnOrder"]).OrderNumber%>
					</span>--%>
					<p class="NextSection" style="display: none;">
						<a id="btnContinueLater" class="Button BigBlue" href="javascript:void(0);"><span>
							<%= Html.Term("ContinueOrderLater", "Continue Order Later")%></span></a>

                        <% bool isShipped = Model.OrderStatusID == (int)Constants.OrderStatus.Shipped; 
                           if (isShipped)
                           {%>
                         <!-- @01 C07 - Create button Pending Confirm -->
                           
                             <%Func<int, string> MostrarOcultar = (obj) =>
                            {
                                    string estilo = (obj == 0) ? "style='display:none'" : "";
                                    return estilo;
                            }; %>

                            <a <%=MostrarOcultar(Convert.ToInt32( ViewData["SupportTicketID"])) %> id="btnConfirmPending" class="Button BigBlue" href="javascript:void(0);">
                                 <span><%= Html.Term("PendingConfirm", "Pending Confirm")%></span>
                            </a>

                        <!-- @01 C07 - End Create button Pending Confirm -->
                            <%} %>
					</p>
                    </div>

					<span>
						<%= Html.Term("noteOnlyClicksubmitReturnIfYouHaveAlreadyVerifiedThattheProductwasReceived", "*Note: Only click 'Submit Return' if you have already verified that the product was received.")%>
					</span>
					<p class="NextSection">
						<a id="btnSubmit" class="Button BigBlue SubmitOrder ButtonOff" href="javascript:void(0);">
							<span>
								<%= Html.Term("SubmitReturn", "Submit Return") %>
								>></span></a>

                        <a id="btnPendingConfirm" class="Button BigBlue SubmitOrder ButtonOff" href="javascript:void(0);">
							<span>
								<%= Html.Term("PendingConfirm", "Pending Confirm") %>
								>></span></a>
					</p>
				</div>
				<div class="PModal WaitWin">
					<span>
						<%= Html.Term("OneMomentPlease", "One moment please...") %>
					</span>
					<br />
					<img src="<%= ResolveUrl("~/Content/Images/processing.gif") %>" alt="<%= Html.Term("Processing", "processing...") %>" />
				</div>
			</td>
		</tr>
	</table>
     <!--/ Defaults -->
    <div>
        <input type="hidden" name="zeroDecimal" id="zeroDecimal" value="<%= zero.ToString("F") %>" />
        <input type="hidden" name="isShipped" id="isShipped" value="<%= isShipped %>" />
    </div>
    <!--/ End Defaults -->

    
</asp:Content>