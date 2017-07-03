<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<NetSteps.Web.Mvc.Controls.Models.OrderEntryModel>" %>
<%@ Import Namespace="NetSteps.Web.Extensions" %>
<% string baseUrl = ResolveUrl("~/") + ViewContext.RouteData.DataTokens["area"].ToString() + "/" + ViewContext.RouteData.Values["controller"].ToString() + "/"; %>
<% string currentController = ViewContext.Controller.ValueProvider.GetValue("controller").RawValue.ToString(); %>
<script type="text/javascript">


	function OnAddressSavedSuccessfully(response) {    
		ChangeShippingAddress($('#sShippingAddress').val());
	}

	$(function () {
		$('#ProductLoad').hide();
		$('.Loading').hide();
		//        if (!/^true$/i.test('Inventory.Instance.InventoryLoaded %>')) {
		//            $('#productWait').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' }).jqmShow();
		//            $.get(' ResolveUrl("~/Orders/OrderEntry/LoadProducts") %>', {}, function () {
		//                $('#productWait').jqmHide();
		//            });
		//        }

		//Not used?
		$('#shippingAddressModal').jqm({ modal: true, trigger: '#btnAddShippingAddress' });
		$('#paymentMethodModal').jqm({ modal: true, trigger: '#btnAddPaymentMethod' });

		//Not used?
		$('#GiftCardCode').change(lookupGiftCardBalance);

		//Not used?

		$('#btnLookupGC').click(lookupGiftCardBalance);
	});




	function updateCartAndTotals(results) {
		if (results.orderItems) {
			var itemsHtml = '';
			var orderItem, i;
			for (i = 0; i < results.orderItems.length; i++) {
				itemsHtml = itemsHtml + results.orderItems[i].orderItem;
			}

			$('#CartItems').html(itemsHtml).find('.quantity').numeric({ allowNegative: false, allowDecimal: false });

			if (results.orderItems.length && $('#btnPerformOverrides').length) {
				$('#btnPerformOverrides').removeClass('ButtonOff');
			}
		}
		
		// remove payments (the payments were already removed from the object, now we need to update the html)
		$('#payments .paymentItem').remove();
		if (!$('#payments .paymentItem').length)
			$('#noPaymentsRow').show();

		if (results.outOfStockItems && results.outOfStockItems.length > 0 && results.showOutOfStockMessage) {
			$('#outOfStockProducts li').remove();
			$('#outOfStockMessage').show();
			for (var i = 0; i < results.outOfStockItems.length; i++) {
				$('#outOfStockProducts').append('<li>' + results.outOfStockItems[i] + '</li>');
			}
		}
		else {
			$('#outOfStockMessage').hide();
		}

		//update promotion information
		$('div.promoNotifications').hide();
		if (results.promotions && results.promotions.length)
		{
			var promoHtml = '';
			$.each(results.promotions, function () {
			    var row = '<div class="promoNotification">' + this.Description;
			    if (this.StepID && this.isAvailable) {
			        row += '<a class="bold selectGift" href="javascript:void(0);" stepId="' + this.StepID + '"> <%= Html.Term("Promotions_SelectFreeGiftLink", "Select Free Gift >")%></a>';
			    }
			    else if (this.StepID && !this.isAvailable){
			        row += '<span title="<%=Html.Term("The free item(s) for this promotion are currently Out of Stock")%>" class="bold promoItemAvailability"> <%=Html.Term("Unavailable", "Unavailable")%></span>';
			    }
			    else if (!this.isAvailable) {
			        row += '<span title="<%=Html.Term("The free item(s) for this promotion are currently Out of Stock")%>" class="bold promoItemAvailability"> <%=Html.Term("Unavailable", "Unavailable")%></span>';
			    }
			    row += '</div>';
			    promoHtml += row;
			});
			$('#PromotionList').empty().html(promoHtml);
			$('div.promoNotifications').show();
		}

		// refresh the totals
		updateTotals(results);

		if (!results.shippingMethods || results.shippingMethods == "") {
			var noShippingMethodsMessage = '<%= Html.Term("NoShippingRatesAvailable", "No shipping rates available. Please enter a shipping address if one is not yet added to order or adjust existing address or order contents.") %>';
			$('#shippingMethods').html(noShippingMethodsMessage);
			if (!$('.shippingMethodsSection').is(':visible')) {
				$('.shippingMethodsSection').effect("blind", { mode: 'show' }, 500);
			}
        } else {
            if (results.shippingMethods != null && results.shippingMethods != undefined && results.shippingMethods != '')
                results.shippingMethods = results.shippingMethods.toUpperCase().replace('DAY(S)', '<%= Html.Term("DayPlural", "day(s)").ToUpper() %>');

			$('#shippingMethods').html(results.shippingMethods);
			if (!$('.shippingMethodsSection').is(':visible')) {
				$('.shippingMethodsSection').effect("blind", { mode: 'show' }, 500);
			}
		} 
	}

	$('.quantity').numeric();

    function updateTotals(results)
    {
        $.each(['subtotal', 'commissionableTotal', 'qualificationTotal', 'taxTotal', 'shippingTotal', 'handlingTotal', 'grandTotal', 'balanceDue', 'paymentTotal'], function (i, item) {
            $('.' + item).text(results.totals[item]);
        });

        /*CGI(CMR)-07/04/2015-Inicio*/
//        if (results.totals['adjustedSubtotal'] != results.totals['subtotal'])
//        {
//            $('.subtotal').html(results.totals['adjustedSubtotal']);
//            $('.customerSubtotal').html('<span class="strikethrough">' + results.totals['subtotal'] + '</span> ' + results.totals['adjustedSubtotal']);
//        }
//        else
//        {
//            $('.customerSubtotal').text(results.totals['subtotal']);
//        }
        //AdjustedItemPrice, descuento
        $('#SubTotalOriginalPrice').hide();
        if (results.totals['sumAdjustedItemPrice'] != results.totals['sumPreadjustedItemPrice']) {
            //$('#SubTotalOriginalPrice').text(results.totals['subTotalPreadjustedItemPrice'] + results.totals['subTotalPreadjustedItemPrice_Text']);
            $('#SubTotalDiscountPrice').text(results.totals['adjustedSubtotal']);
            
            //$('#SubTotalOriginalPrice').show();
            $('#SubTotalDiscountPrice').show();
            $('#SubTotalOnlyOriginalPrice').hide();
        }
        else { 
            $('#SubTotalOnlyOriginalPrice').text(results.totals['adjustedSubtotal']);
            $('#SubTotalOriginalPrice').hide();
            $('#SubTotalDiscountPrice').hide();
            $('#SubTotalOnlyOriginalPrice').show();
        }

        /*CGI(CMR)-07/04/2015-Fin*/

        var submitButton = $('.Submit');
        if (!submitButton.hasClass('ButtonOff'))
            submitButton.addClass('ButtonOff');

//        if (results.totals['balanceAmount'] >= 0 ) {
//            $('.balanceDue').css('color', 'green');            
//        }else{
//        $('.balanceDue').css('color', 'red');
//        }

//        

        if (results.totals['balanceAmount'] >= 0 && results.totals['numberOfItems'] > 0)
        {
            $('.balanceDue').css('color', 'green');
            if ($('#sShippingAddress').val())
                submitButton.removeClass('ButtonOff');
        }
        else
        {
            $('.balanceDue').css('color', 'red');
        }

        if (results.totals['balanceAmount'] < 0)
            $('#txtPaymentAmount').val(results.totals['balanceAmount'].toFixed(2) * (-1));
        else
            $('#txtPaymentAmount').val('');

		if (results.orderItems) {
			var itemsHtml = '';
			var orderItem, i;
			for (i = 0; i < results.orderItems.length; i++) {
				itemsHtml = itemsHtml + results.orderItems[i].orderItem;
			}
			$('#CartItems').html(itemsHtml).find('.quantity').numeric({ allowNegative: false, allowDecimal: false });

            if (results.orderItems.length && $('#btnPerformOverrides').length)
            {
				$('#btnPerformOverrides').removeClass('ButtonOff');
			}
		}

        /*CGI(CMR)-31/03/2015-Inicio*/
        var SumItemOriginalUnitPrice = 0; var SumItemUnitPrice = 0; var SumItemOnlyOriginalUnitPrice = 0;
        $("#products").find('tr').each(function (i) {
            var tdsOUP = $(this).find('#ItemOriginalUnitPrice');
            if (tdsOUP.eq(0).text().length > 0) {
                SumItemOriginalUnitPrice += (tdsOUP.eq(0).text().indexOf("$") == -1 ? 0 : parseFloat(tdsOUP.eq(0).text().replace("$", "")));
            }

            var tdsUP = $(this).find('#ItemUnitPrice');
            if (tdsUP.eq(0).text().length > 0) {
                SumItemUnitPrice += (tdsUP.eq(0).text().indexOf("$") == -1 ? 0 : parseFloat(tdsUP.eq(0).text().replace("$", "")));
            }

            var tdsOOUP = $(this).find('#ItemOnlyOriginalUnitPrice');
            if (tdsOOUP.eq(0).text().length > 0) {
                //                SumItemOnlyOriginalUnitPrice += (tdsOOUP.eq(0).text().indexOf("$") == -1 ? 0 : parseFloat(tdsOOUP.eq(0).text().replace("$", ""))); //EL QV NO DEBE TENER SIGNO $
                SumItemOnlyOriginalUnitPrice += (parseFloat(tdsOOUP.eq(0).text().replace("$", "")));
            }
        });

        $('#SubTotalCV').text(results.totals['commissionableTotal']);
        $('#SubTotalQty').text(results.totals['numberOfItems']);
//        $('#SubTotalQV').text('$' + (SumItemOnlyOriginalUnitPrice + SumItemOriginalUnitPrice - SumItemUnitPrice).toFixed(2)); //EL QV NO DEBE TENER SIGNO $
        $('#SubTotalQV').text(SumItemOnlyOriginalUnitPrice + SumItemOriginalUnitPrice - SumItemUnitPrice);

        //Calculando el subTotales
//        var SumItemOriginalPrice = 0; var SumItemDiscountPrice = 0; var SumItemOnlyOriginalPrice = 0;
//        $("#products").find('tr').each(function (i) {
//            var tdsOP = $(this).find('#ItemOriginalPrice');
//            if (tdsOP.eq(0).text().length > 0) {
//                SumItemOriginalPrice += (tdsOP.eq(0).text().indexOf("$") == -1 ? 0 : parseFloat(tdsOP.eq(0).text().replace("$", "")));
//            }

//            var tdsDP = $(this).find('#ItemDiscountPrice');
//            if (tdsDP.eq(0).text().length > 0) {
//                SumItemDiscountPrice += (tdsDP.eq(0).text().indexOf("$") == -1 ? 0 : parseFloat(tdsDP.eq(0).text().replace("$", "")));
//            }

//            var tdsOOP = $(this).find('#ItemOnlyOriginalPrice');
//            if (tdsOOP.eq(0).text().length > 0) {
//                SumItemOnlyOriginalPrice += (tdsOOP.eq(0).text().indexOf("$") == -1 ? 0 : parseFloat(tdsOOP.eq(0).text().replace("$", "")));
//            }
//        });


//        if (SumItemOnlyOriginalPrice > 0) $("#SubTotalOnlyOriginalPrice").text('$' + SumItemOnlyOriginalPrice);
//        if (SumItemOriginalPrice > 0 || SumItemDiscountPrice > 0) {
//            $("#SubTotalOriginalPrice").text('$' + SumItemOriginalPrice);
//            $("#SubTotalDiscountPrice").text('$' + SumItemDiscountPrice);
//        }

        /*CGI(CMR)-31/03/2015-Fin*/
    }

	//Not used?
	function lookupGiftCardBalance() {
		$('#gcLoader').show();
		$('#btnLookupGC').hide();
		$.post('<%= ResolveUrl("~/Orders/OrderEntry/LookupGiftCardBalance") %>', { giftCardCode: $('#GiftCardCode').val() }, function (results) {
			hideMessage();
			$('#gcLoader').hide();
			$('#btnLookupGC').show();
			if (results.result && results.balance) {
				$('#GiftCardBalance').html(results.balance);
				$('#txtPaymentAmount').val(results.amountToApply);
			}
			else {
				$('#GiftCardBalance').html('-');
				if (results.message) {
					showMessage(results.message, true);
				}
			}
		});
	}

</script>
<div id="productWait" class="PModal WaitWin">
	<%= Html.Term("PleaseWaitWhileWeLoadTheProducts", "Please wait while we load the products...")%>
	<br />
	<img src="<%= ResolveUrl("~/Content/Images/processing.gif") %>" alt="<%= Html.Term("loading", "loading...")%>" />
</div>
<%
   Html.RenderPartial("OrderEntryPlugins/PartialShippingAddress", Model); %>
<% if ((CoreContext.CurrentAccount.AccountTypeID == (int)Constants.AccountType.Distributor
			|| CoreContext.CurrentAccount.AccountTypeID == (int)Constants.AccountType.PreferredCustomer) && !Model.Order.IsTemplate && CoreContext.CurrentAccount.AccountTypeID != (int)Constants.AccountType.RetailCustomer && Model.MarkAsAutoship)
   { %>
<% Html.RenderPartial("OrderEntryPlugins/PartialMarkAsAutoship", Model); %>
<% } %>
<% if (Model.BulkAddModal)
   {
	   Html.RenderPartial("OrderEntryPlugins/PartialAddProduct", Model);
   }
   if (Model.ReplacementTables)
   {
	   Html.RenderPartial("OrderEntryPlugins/PartialReplacementTables", Model);
   }
   //Html.RenderPartial("OrderEntryPlugins/PartialOutOfStock", Model);
   Html.RenderPartial("OrderEntryPlugins/PartialShippingMethod", Model);
   
   Html.RenderPartial("OrderEntryPlugins/PartialCoupon", Model);
   Html.RenderPartial("OrderEntryPlugins/PartialPayment", Model);
   Html.RenderPartial("OrderEntryPlugins/PartialInvoiceNotes", Model);
    
   //<!-- Modals -->


   Html.RenderPartial("OrderEntryPlugins/PartialBundlePackModal", Model);

   if (Model.BulkAddModal)
   {
       Html.RenderPartial("OrderEntryPlugins/PartialBulkAddModal", Model);
   }

   Html.RenderPartial("OrderEntryPlugins/PartialPaymentInfoModal", Model);

   Html.RenderPartial("BillingShippingModal"); %>
