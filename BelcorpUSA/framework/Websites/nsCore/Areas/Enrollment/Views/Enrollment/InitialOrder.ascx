<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%@ Import Namespace="NetSteps.Web.Mvc.Controls.Controllers.Enrollment" %>
<% var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext; %>
<%--CSTI(mescobar)-18/01/2016-Inicio--%>
<style type="text/css">
        .fadeInbox
        {
            padding: 20px;
            margin: 16px;
            border: 3px solid #39399C;
            width: 500px;
            height: auto;
            background-color: #D9D1DE;
            color: Black;
            position: relative;
            z-index: 999;
            left: 400px;
            top: 700px;
            font-size: 1.2em;
            border-top-left-radius: 3em;
            border-top-right-radius: 3em;
            border-bottom-right-radius: 3em;
            border-bottom-left-radius: 3em;
        }
        .fadeInbox a
        {
            color: Blue;
            text-decoration: underline;
            font-style: italic;
        }
    </style>
<%--CSTI(mescobar)-18/01/2016-Fin--%>
<script type="text/javascript">
    $(function () {
        // CSTI(mescobar)-18/01/2016-Inicio
        $('.fadeInbox').jqm({
            modal: true,
            trigger: true,
            onShow: function (h) {
                h.w.css({
                    position: 'relative',
                    top: '-350px',
                    left: '300px'
                }).fadeIn();
            },
            overlay: 50,
            overlayClass: 'HModalOverlay'
        });

        $('.closeFadeInbox').on("click", function () {
            $('.fadeInbox').jqmHide();
        });
        // CSTI(mescobar)-16/01/2016-Fin

		//Add the following 4 lines to the top of any addon - DES
		window.enrollmentAccountNumber = '<%= enrollmentContext.EnrollingAccount.AccountNumber %>';
		if (parseBool('<%= ViewData["IsSkippable"] %>')) {
			$('#btnSkip').show();
		}


		$('#btnQuickAdd').click(function () {

			var productId = $('#hQuickAddProductId').val(), quantity = parseInt($('#txtQuickAddQuantity').val())
			if (productId && quantity) {
				var t = $(this);
				showLoading(t);
				$('#ProductLoad').show();

				$.post('<%= ResolveUrl("~/Enrollment/InitialOrder/AddToCart") %>', { productId: $('#hQuickAddProductId').val(), quantity: parseInt($('#txtQuickAddQuantity').val()) }, function (results) {
					if (results.result) {
						$('#hQuickAddProductId,#txtQuickAddSearch,#txtQuickAddQuantity').val('');
						updateCartAndTotals(results);
						hideLoading(t);
						$('.initialOrderRemoveItem').on('click', function () {
							var guid = $(this).attr('guid');
							remove(guid);
						});
					}
					else {
						hideLoading(t);
						showMessage('<%= Html.Term("TheProductCouldNotBeAdded", "The product could not be added") %>: ' + results.message, true);
					}
					$('#ProductLoad').hide();
				});
			}
		});

		$('#txtQuickAddSearch').jsonSuggest('<%= ResolveUrl("~/Enrollment/InitialOrder/SearchProducts") %>', { minCharacters: 3, source: $('#txtQuickAddSearch'), ajaxResults: true, width: 250, onSelect: function (item) {
			$('#hQuickAddProductId').val(item.id);
			$('#txtQuickAddQuantity').val('1');
		}
		});

		// Don't use live() for this or it will affect other partial views.
		$('#shippingMethods').delegate('input:radio[name=shippingMethod]', 'click', function () {
			$.post('<%= ResolveUrl("~/Enrollment/InitialOrder/SetShippingMethod") %>', { shippingMethodId: $(this).val() }, function (results) {
				if (results.result) {
					updateTotals(results);
				}
				else {
					showMessage('<%= Html.Term("TheShippingMethodCouldNotBeChanged", "The shipping method could not be changed") %>: ' + results.message, true);
				}
			});
		});

        $('#btnNext').click(function () {
            /*CSTI(mescobar)-18/01/2016-Inicio*/
            $('#ProductLoad').show();
            $.post('<%= ResolveUrl("~/Enrollment/InitialOrder/ValidateOrderRules") %>', {}, function (response) {
                $('#ProductLoad').hide();
                if (response.result) {
                    var strReplaceAll = response.message;
                    var intIndexOfMatch = strReplaceAll.indexOf("|n");
                    while (intIndexOfMatch != -1) {
                        strReplaceAll = strReplaceAll.replace("|n", String.fromCharCode(10))
                        intIndexOfMatch = strReplaceAll.indexOf("|n");
                    }
                    $("#PopUpGenericPreViewMessage").html(strReplaceAll)
                    $('.fadeInbox').jqmShow();
                }
                else {
                    var data = getData();
                    if (data === false)
                        return false;

                    window.letUnload = false;
                    enrollmentMaster.postStepAction({
                        step: "InitialOrder",
                        stepAction: "SubmitStep",
                        data: data,
                        showLoadingElement: $('#btnNext').parent(),
                        load: true
                    });
                }
            });
            /*CSTI(mescobar)-18/01/2016-Fin*/
        });

		$('#btnSkip').click(function () {
			window.letUnload = false;
			enrollmentMaster.postStepAction({
				step: "InitialOrder",
				stepAction: "SkipStep",
				load: true
			});
		});
	});

	function remove(guid) {
		$('#ProductLoad').show();
		var t = $(this);
		$.post('<%= ResolveUrl("~/Enrollment/InitialOrder/RemoveFromCart") %>', { guid: guid }, function (results) {
			if (results.result) {
				$('#oi' + guid).remove();
				var childsItemGUID = results.childsItemGUID;
				if (childsItemGUID.length > 0) {
					for (var i = 0; i < childsItemGUID.length; i++) {
						$('#oi' + childsItemGUID[i]).remove();
					}
				}
				updateCartAndTotals(results);
				$('#ProductLoad').hide();
//				
				if (results.message !== undefined && results.message.length > 0) {
					showMessage(results.message, true);
				}
			} else {
				showMessage('<%= Html.Term("ErrorRemovingProduct", "The product could not be removed")%>: ' + results.message, true);
				$('#ProductLoad').hide();
			}
		});
	}


	function updateCartAndTotals(results) {
		if (results.orderItems) {
			var i;
			var productsBody = $('#products tbody:first');
			productsBody.html('');

			for (i = 0; i < results.orderItems.length; i++) {
				productsBody.append(results.orderItems[i].orderItem);
			}
		}
		// refresh the totals
		updateTotals(results);
		$('#shippingMethods').html(results.shippingMethods);
	}

	function updateTotals(results) {
		$.each(['subtotal', 'commissionableTotal', 'taxTotal', 'shippingTotal', 'grandTotal'], function (i, item) {
			$('.' + item).text(results.totals[item]);
		});
	}

	function getData() {
		return {};
	}
</script>
<div class="StepGutter">
	<h3>
		<b>
			<%= Html.Term("EnrollmentStep", "Step {0}", ViewData["StepCounter"])%></b>
		<%= Html.Term("MakeAnInitialOrder", "Make an initial order") %></h3>
</div>
<div class="StepBody">
	<div>
		<% bool variableOrder = true; // (InitialOrderStep.EnrollmentOrderType)ViewData["OrderType"] != InitialOrderStep.EnrollmentOrderType.Fixed;
	 if (variableOrder)
	 { %>
		<div class="QuickAdd">
			<div class="FL">
				<%= Html.Term("SKUorName", "SKU or Name") %>:
				<input id="txtQuickAddSearch" type="text" style="width: 20.833em;" />
				<input id="hQuickAddProductId" type="hidden" />
			</div>
			<div class="FL">
				<%= Html.Term("Quantity") %>:
				<input id="txtQuickAddQuantity" type="text" class="Short quantity" style="width: 4.167em;" />
				<a id="btnQuickAdd" href="javascript:void(0);" class="DTL Add">
					<%= Html.Term("AddToOrder", "Add to Order") %></a>
			</div>
			<span class="ClearAll"></span>
		</div>
		<%} %>
		<table id="products" width="100%" class="DataGrid">
			<thead>
				<tr class="GridColHead">
					<%if (variableOrder)
	   { %>
					<th class="GridCheckBox">
					</th>
					<%} %>
					<th>
						<%= Html.Term("SKU") %>
					</th>
					<th>
						<%= Html.Term("Product") %>
					</th>
					<th>
						<%= Html.Term("PricePerItem", "Price Per Item") %>
					</th>
					<th style="width: 9.091em;">
						<%= Html.Term("Quantity") %>
					</th>
					<th>
						<%= Html.Term("Price") %>
					</th>
				</tr>
			</thead>
			<tbody>
				<% int count = 0;
	   var inventory = NetSteps.Encore.Core.IoC.Create.New<InventoryBaseRepository>();
	   foreach (var fixedProduct in ViewData["Products"] as Dictionary<string, int>)
	   {
		   Product product = inventory.GetProduct(fixedProduct.Key);
		   decimal price = product.GetPrice(enrollmentContext.EnrollingAccount.AccountTypeID, ConstantsGenerated.PriceRelationshipType.Products, enrollmentContext.CurrencyID); %>
				<tr class="GridRow<%= count % 2 == 1 ? " Alt" : "" %>">
					<%if (variableOrder)
	   { %>
					<td>
					</td>
					<%} %>
					<td>
						<input type="hidden" class="productId" value="<%= product.ProductID %>" />
						<%= product.SKU%>
					</td>
					<td>
						<%= product.Name%>
					</td>
					<td>
						<%= price.ToString(enrollmentContext.CurrencyID) %>
					</td>
					<td>
						<%= fixedProduct.Value %>
					</td>
					<td>
						<%= (fixedProduct.Value * price).ToString(enrollmentContext.CurrencyID)%>
					</td>
				</tr>
				<%++count;
				   } %>
			</tbody>
			<tbody>
				<tr id="productTotalBar" class="GridTotalBar">
					<%if (variableOrder)
	   { %>
					<td>
					</td>
					<%} %>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td>
					</td>
					<td>
						<b><span class="subtotal">
							<%= ViewData["Subtotal"] %></span> (<%= Html.Term("SubTotal", "Sub Total") %>)</b>
					</td>
				</tr>
			</tbody>
		</table>
	</div>
	<table class="FormTable Section" width="100%">
		<tr class="shippingRow">
			<td class="FLabel">
				<%= Html.Term("ShippingMethod", "Shipping Method")%>
			</td>
			<td id="shippingMethods">
				<% bool requriesShipping = (bool)ViewData["RequiresShippingMethod"];
	   if (requriesShipping)
	   {
		   IEnumerable<ShippingMethodWithRate> shippingMethods = (ViewData["ShippingMethods"] as IEnumerable<ShippingMethodWithRate>).OrderBy(sm => sm.ShippingAmount);
		   int selectedShippingMethodId = shippingMethods.FirstOrDefault() == default(ShippingMethodWithRate) ? 0 : shippingMethods.FirstOrDefault().ShippingMethodID;
		   foreach (ShippingMethodWithRate shippingMethod in shippingMethods)
		   { %>
				<div class="FL AddressProfile">
					<input id="shippingMethod<%= shippingMethod.ShippingMethodID %>" type="radio" name="shippingMethod"
						class="Radio" value="<%= shippingMethod.ShippingMethodID %>" <%= shippingMethod.ShippingMethodID == selectedShippingMethodId ? "checked=\"checked\"" : "" %> />
					<b>
						<%= Html.Term(shippingMethod.Name)%></b><br />
					<%= shippingMethod.ShippingAmount.ToString(enrollmentContext.CurrencyID)%>
				</div>
				<% } %>
				<% } %>
				<% else %>
				<% { %>
				<b>
					<%= Html.Term("NotApplicable", "N/A") %>
				</b>
				<% } %>
			</td>
		</tr>
	</table>
	<table width="100%" class="DataGrid">
		<tr id="totalBar" class="GridTotalBar">
			<td colspan="2" style="text-align: right;">
				<div class="FL Loading" id="TotalsLoad">
				</div>
				<%= Html.Term("Subtotal") %>:<br />


				<%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%string valorSCV = OrderExtensions.GeneralParameterVal(CoreContext.CurrentMarketId, "SCV");
                                if (valorSCV == "S")
                                { %>
                                <%= Html.Term("CommissionableTotal", "Commissionable Total") %>:<br />
                                 <%}%>
                            <%--CS.03MAY2016.Fin.Muestra CV--%>


				<%= Html.Term("Tax") %>:<br />
				<%= Html.Raw(Html.Term("S&H", "S&amp;H"))%>:<br />
				<%= Html.Term("OrderTotal", "Order Total") %>:
			</td>
			<td>
				<span class="subtotal">
					<%= ViewData["Subtotal"] %></span>
				<br />

                <%--CS.03MAY2016.Inicio.Muestra CV--%>
                            <%if (valorSCV == "S"){%>
				<span class="commissionableTotal">
					<%= ViewData["Commissionable"] %></span>
				<br />
                <%}%>
                <%--CS.03MAY2016.Fin.Muestra CV--%>


				<span class="taxTotal">
					<%= ViewData["Tax"]%></span>
				<br />
				<span class="shippingTotal">
					<%= ViewData["Shipping"]%></span>
				<br />
				<b><span class="grandTotal">
					<%= ViewData["GrandTotal"]%></span></b>
			</td>
		</tr>
	</table>
     <%--CGI(CMR)-06/05/2015-Inicio--%>
    <div id="PopUpGenericPreView" class="fadeInbox" style="display: none">
        <div style="text-align:left">
        <span id="PopUpGenericPreViewMessage"></span>
        <a class="closeFadeInbox" href="javascript:void(0)">
        <%= Html.Term("GoBackAndAddProducts", "Click here to go back and add more products")  %>
        </a>
        </div>
    </div>
    <%--CGI(CMR)-06/05/2015-Fin--%>
</div>
<span class="ClearAll"></span>
<p class="Enrollment SubmitPage">
	<a id="btnNext" href="javascript:void(0);" class="Button BigBlue">
		<%= Html.Term("Next") %>&gt;&gt;</a> <a id="btnSkip" href="javascript:void(0);" class="Button"
			style="display: none;">
			<%= Html.Term("Skip") %>&gt;&gt;</a>
</p>
<%--CGI(CMR)-06/05/2015-Inicio--%>
<script type="text/javascript">
    $(document).ready(function () {
        $('#btnNext').click(function (event) {
            event.stopPropagation();
        });
    });
</script>
<%--CGI(CMR)-06/05/2015-Fin--%>