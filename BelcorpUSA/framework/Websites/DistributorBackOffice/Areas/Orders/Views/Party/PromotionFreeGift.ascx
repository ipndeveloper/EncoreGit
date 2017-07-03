
<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<GiftSelectionModel>" %>
<script type="text/javascript">

	$(function () {
		//Promotion Modal
		$('#SelectPromotionProducts').jqm({
			modal: true,
			trigger: '.selectGift',
			onShow: function (h) {
				h.o.fadeIn();
				h.w.css({
					top: 0 + 'em',
					left: 2 + 'em'
				}).fadeIn();
			},
			onHide: function (h) {
				h.w.fadeOut();
				if (h.o)
					h.o.fadeOut(function () { $(this).remove(); });
			}
		});

		$('a.selectGift').live('click', function () {
			var data = { stepId: $(this).attr('stepId') },
				options = {
					url: '<%= Model.GetStepInfoUrl %>',
					showLoading: $(this),
					data: data,
					success: function (response) {
						if (response.result) {
							var modal = $('#SelectPromotionProducts');
							modal.attr('stepId', data.stepId);
							updateModal(response.GiftSelectionModel.AvailableOptions, response.GiftSelectionModel.SelectedOptions, response.GiftSelectionModel.MaxQuantity);
							modal.jqmShow();
						}
						else {
							showMessage(response.message, true);
						}
					}
				};
			NS.post(options);
		});

		function updateModal(availableOptions, selections, maxQuantity) {
			var selectedPromoProductQty = selections.length,
                totalPromoProducts = maxQuantity,
                promoProductQty = totalPromoProducts - selectedPromoProductQty,
                remainingQty = $('span.promoRemaining .promoProductQty'),
                selectedQty = $('span.selectedPromo .promoProductQty');

			//Add qty text to modal display
			remainingQty.text(maxQuantity - selections.length);
			$('span.selectedPromo .totalPromoQty').text('/' + totalPromoProducts);
			selectedQty.text(selections.length);

			//Add options to left side
			var optionsHtml = '';
			$.each(availableOptions, function (i) { optionsHtml += createProductOptionRow(availableOptions[i]); });
			$('#PromoProductsList').empty().append(optionsHtml);

			var selectionsHtml = '';
			$.each(selections, function (i) {
				var productDetails = getPromoProductDetails($('#PromoProductsList .promoProduct').has('input[value="' + selections[i].ProductID + '"]'));
				selectionsHtml += createSelectedProductRow(productDetails);
			});
			$('#SelectedPromoProductsList').empty().prepend(selectionsHtml);

			//Display Selected Promotion Products in the right column
			$('#PromoProductsList .promoProduct').click(function () {
				if (selectedPromoProductQty < totalPromoProducts) {
					var t = $(this),
                        selectedPromoProdDetails = getPromoProductDetails(t),
                        selectedProduct = createSelectedProductRow(selectedPromoProdDetails, true);
					$('#SelectedPromoProductsList').prepend(selectedProduct).find(".promoProduct:first").animate({
						'height': 'toggle',
						'opacity': 'toggle'
					}, 500);
					remainingQty.text(promoProductQty -= 1);
					selectedQty.text(selectedPromoProductQty += 1);
				}
				else {
					alert('<%= Html.Term("CartPromotionModal_FreeGiftsAllBeenSelected", "Free Gifts have all been selected") %>');
				}
			});

			//Remove selected promotion products from selected list in right column
			$('.Remove').live('click', function () {
				$(this).closest('div.promoProduct').animate({
					'height': 'toggle',
					'opacity': 'toggle'
				}, 500, function () {
					$(this).remove();
				});
				remainingQty.text(promoProductQty += 1);
				selectedQty.text(selectedPromoProductQty -= 1);
			});
		}

		$('#btnSave').click(function () {
			var products = $('#SelectedPromoProductsList .promoProduct'),
				data = { stepId: $('#SelectPromotionProducts').attr('stepId') };
			if (products.length) {
				products.each(function (i) {
					data['products[' + i + '].productId'] = $(this).find('input.productId').val();
					data['products[' + i + '].quantity'] = 1;
				});
			}
			var options = {
				url: '<%= Model.SaveGiftSelectionUrl %>',
				data: data,
				showLoading: $(this),
				success: function (response) {
					if (response.result) {
						$('#SelectPromotionProducts').jqmHide();
						<%if (!string.IsNullOrEmpty(Model.JavaScriptSaveCallbackFunctionName)) { %>
						window['<%=Model.JavaScriptSaveCallbackFunctionName %>'](response)
						<%} %>
					}
					else {
						showMessage(response.message, true);
					}
				}
			};
			NS.post(options);

		});
	});

	function createSelectedProductRow(productDetails, hide) {
		var html = '<div class="m5 promoProduct"' + (hide ? 'style="display:none;"' : '') + '>' +
			'<input type="hidden" class="productId" value="' + productDetails.productId + '" />' +
            '<div class="FL m1 mr5 imagewrapper splitCol15">' + productDetails.thumb + '</div>' +
            '<div class="FL promoProductTitle splitCol80">' + productDetails.title + '</div>' +
            '<div class="FL promoProductValue splitCol80">' + productDetails.value + '</div>' +
            '<span class="clr"></span>' +
            '<div class="UI-lightBg mt5 mb10 pad2 brdrAll promoUtilities">' +
            '<a title="<%= Html.Term("CartPromotionModal_DeleteTitleTag", "Remove Gift") %>" class="FL UI-icon-container Remove" href="javascript:void(0);" id="">' +
            '<span class="icon-label"><%= Html.Term("CartPromotionModal_DeleteTitleTag") %></span></a>' +
            '<span class="clr"></span>' +
            '</div>' +
            '</div>';
		return html;
	}

	function createProductOptionRow(productInfo) {
		var html = '';
		if(productInfo.IsOutOfStock)
		{
			html = '<div class="brdr1 brdrAll pad5 m5 promoNotAvailable" title="<%= Html.Term("Out Of Stock", "Out of Stock") %>">'
				+ '<div class="FL m5 imagewrapper splitCol15"><img src="' + productInfo.Image + '" alt="" width="100%" /></div>'
				+ '<div class="FL ml5 pad5 ProductDescription splitCol80">'
				+ '<input type="hidden" class="productId" value="' + productInfo.ProductID + '" />'
				+ '<div class="FL splitCol70 bold promoProductTitle">' + productInfo.Name + '</div>'
				+ '<div class="FL freeItemSelection promoItemAvailability">'
				+ '<%= Html.Term("Out Of Stock", "Out of Stock") %></div>'
				+ '<span class="clr"></span>'
				+ '<div class="promoProductShortDescription"><p>' + productInfo.Description + '</p></div>'
				+ '<div class="promoProductValue"><small><%= Html.Term("CartPromotionModal_Value", "Value") %>: ' + productInfo.Value + '</small></div>'
				+ '</div><span class="clr"></span>'
				+ '</div>';
		}
		else
		{
			html = '<div class="brdr1 brdrAll pad5 m5 promoProduct" title="<%= Html.Term("CartPromotionModal_AddGiftTitleTag", "Add Gift") %>">'
				+ '<div class="FL m5 imagewrapper splitCol15"><img src="' + productInfo.Image + '" alt="" width="100%" /></div>'
				+ '<div class="FL ml5 pad5 ProductDescription splitCol80">'
				+ '<input type="hidden" class="productId" value="' + productInfo.ProductID + '" />'
				+ '<div class="FL splitCol70 bold promoProductTitle">' + productInfo.Name + '</div>'
				+ '<a href="javascript:void(0);" class="FR UI-icon-container addPromoItem"><span class="FL mr5 icon-label">'
				+ '<%= Html.Term("CartPromotionModal_AddGiftLinkText", "Add Gift") %></span> <span class="UI-icon icon-arrowNext">'
				+ '</span></a><span class="clr"></span>'
				+ '<div class="promoProductShortDescription"><p>' + productInfo.Description + '</p></div>'
				+ '<div class="promoProductValue"><small><%= Html.Term("CartPromotionModal_Value", "Value") %>: ' + productInfo.Value + '</small></div>'
				+ '</div><span class="clr"></span>'
				+ '</div>';
		}
		return html;
	}

	function getPromoProductDetails(t) {
		var img = '',
                name = '',
                val = '',
                icon = '',
				productId = '';
		if (t == null) {
			name = '<a href="javascript:void(0);" class="selectPromoProduct"><%= Html.Term("Cart_PromoProductSelectLinkText", "Select a Gift") %></a>',
                icon = '<span class="UI-icon icon-warning selectGiftNotification"></span>';
		}
		else {
			img = $('div.imagewrapper', t).html(),
                name = $('div.promoProductTitle', t).text(),
                val = $('div.promoProductValue', t).text(),
				productId = $('input.productId', t).val();
		}

		return {
			productId: productId,
			thumb: img,
			title: name,
			value: val,
			icon: icon
		};
	}
</script>
<div id="SelectPromotionProducts" class="jqmWindow LModal" style="z-index: 3000;
	width: 69.091em; margin-left: 0;position:fixed;">
	<div class="mContent">
		<h2>
			<%= Html.Term("CartPromotionModal_H2", "Your order qualifies for FREE gifts!") %></h2>
		<div class="FL splitCol65 promoProductChoices">
			<h3>
				<%= Html.Term("CartPromotionModal_AvailableProductsHeading", "Available Gifts") %>
				<span class="FR promoRemaining qtyIndicator"><span class="FL bold promoProductQty"></span>
					&nbsp;
					<%= Html.Term("CartPromotionModal_QuantityRemainingNotification", "selections remaining") %></span>
				<span class="clr"></span>
			</h3>
			<div id="PromoProductsList" class="brdr1 pad2 promoProductsWrap">
				
			</div>
		</div>
		<div class="FR splitCol35 PromoProductsAdded">
			<h3>
				<%= Html.Term("CartPromotionModal_CurrentSelectionsHeading", "Your Current Selection(s)") %>
				<span class="FR selectedPromo qtyIndicator"><span class="FL bold promoProductQty">0</span><span
					class="FL totalPromoQty"></span> </span><span class="clr"></span>
			</h3>
			<div id="SelectedPromoProductsList" class="brdr1 brdrAll pad2 promoProductsWrap">
			</div>
		</div>
		<span class="clr"></span>
		<p class="mt10">
			<a class="jqmClose FL" href="javascript:void(0);">
				<%= Html.Term("Close", "Close") %></a> <a class="Button FR" href="javascript:void(0);"
					id="btnSave"><span>
						<%= Html.Term("CartPromotionModal_AddPromotionSelectionsLinkText", "Save Gifts to Order")%></span></a>
		</p>
		<span class="clr"></span>
	</div>
</div>
