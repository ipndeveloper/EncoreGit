<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RuleValidationsDTO>" %>
<script type="text/javascript">
   <%   ProductPriceType productPrice = SmallCollectionCache.Instance.ProductPriceTypes.Where(x => x.ProductPriceTypeID == (int)Constants.ProductPriceType.Retail).FirstOrDefault();

        var customerRetailTotal = NetSteps.Encore.Core.IoC.Create.New<OrderRules.Service.DTO.CustomerPriceTotalDTO>();
        var CustomerPriceTotal = Model.CustomerPriceTotalDTO.Where(x => x.ProductPriceTypeID == productPrice.ProductPriceTypeID).ToList();
        if (CustomerPriceTotal.Count > 0)
        {
            customerRetailTotal = CustomerPriceTotal.FirstOrDefault();
        }
        else if (Model.CustomerPriceSubTotalDTO.Count > 0)
        {
            var customerSubtotal = Model.CustomerPriceSubTotalDTO.FirstOrDefault();
            customerRetailTotal.MinimumAmount = customerSubtotal.MinimumAmount;
            customerRetailTotal.MaximumAmount = customerSubtotal.MaximumAmount;
            customerRetailTotal.ProductPriceTypeID = 0;
            customerRetailTotal.CurrencyID = customerSubtotal.CurrencyID;
            customerRetailTotal.RuleValidationID = customerSubtotal.RuleValidationID;
            customerRetailTotal.CustomerPriceTotalID = customerSubtotal.CustomerPriceSubTotalID;
        }
        else
        {
            if (CustomerPriceTotal.Count == 0 && Model.CustomerPriceSubTotalDTO.Count == 0 )
            {
                customerRetailTotal.MinimumAmount = 0;
                customerRetailTotal.MaximumAmount = 0;
                customerRetailTotal.ProductPriceTypeID = productPrice.ProductPriceTypeID;
                customerRetailTotal.CurrencyID = Market.Load(CoreContext.CurrentMarketId).GetDefaultCurrencyID();
                customerRetailTotal.RuleValidationID = 0;
            }else{
            
                var customerSubtotal_ = Model.CustomerPriceSubTotalDTO.FirstOrDefault();
                customerRetailTotal.MinimumAmount = customerSubtotal_.MinimumAmount;
                customerRetailTotal.MaximumAmount = customerSubtotal_.MaximumAmount;
                customerRetailTotal.ProductPriceTypeID = 0;
                customerRetailTotal.CurrencyID = customerSubtotal_.CurrencyID;
                customerRetailTotal.RuleValidationID = customerSubtotal_.RuleValidationID;
                customerRetailTotal.CustomerPriceTotalID = customerSubtotal_.CustomerPriceSubTotalID;
            }
        }
    %>
	$(function () {

    
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
        var rsltdo = <%= (Model.CustomerPriceSubTotalDTO.Count > 0 ? "true" : CustomerPriceTotal.Count > 0 ? "true" : "false") %>;
        $('#rbtnSubtotal').attr('checked', <%= (Model.CustomerPriceSubTotalDTO.Count > 0 ? "true" : "false") %>).trigger('change');
        $('#rbtnRetail').attr('checked', <%= (Model.CustomerPriceSubTotalDTO.Count > 0 ? "false" : "true") %>).trigger('change');
        $('#txtMinimumAmount').numeric({ allowNegative: false, allowDecimal: true });
        $('#txtMinimumAmount').addClass(rsltdo ? 'required' : '');
         
        $('#chkMinimumAmount').attr('checked', rsltdo).change(function() {
            if(this.checked) {
                $('#txtMinimumAmount').addClass('required');
            }else
            {
                $('#txtMinimumAmount').removeClass('required');
            }
        });
	});
</script>
<div class="pad5 promotionOption couponCode percentOff">
	<div class="FL optionHelpIcons">
    </div>
    <div class="FLabel" style="width:170px;">
			<%=Html.Term("Rules_MinimumAmountRequired", "Minimum Amount Required?")%>
	</div>
	<div rel="isYes" class="hasPanel" id="RequirePrice">
        <span>
            <input type="checkbox" id="chkMinimumAmount" />
        </span>
        <span>
           <%-- <input type="text" size="10" name="Minimum Amount cannot be empty" class="pad2" value="<%=decimal.Round((decimal)(customerRetailTotal.MinimumAmount ?? 0), 2) %>" id="txtMinimumAmount" />--%>
            <input type="text" size="10" name="Minimum Amount cannot be empty" class="pad2" value="<%=decimal.Round((decimal)(customerRetailTotal.MinimumAmount ?? 0), 2) %>" id="txtMinimumAmount" monedaidioma='CultureIPN' />
        </span>
        <span>
			<input type="radio" name="rbtnSubtotalRequired" value="Retail" id="rbtnRetail" />
			<label for="rbtnRetail">
				<%=Html.Term("Rules_Retail", "Retail")%></label>
		</span>
		<span>
			<input type="radio" value="Subtotal" name="rbtnSubtotalRequired" id="rbtnSubtotal" />
			<label for="rbtnSubtotal">
				<%=Html.Term("Rules_Subtotal", "Subtotal")%></label>
		</span>
        <input type="hidden" id="CustomerPriceTotalID" value="<%=customerRetailTotal.CustomerPriceTotalID %>" />
        <input type="hidden" id="ProductPriceTypeID" value="<%=productPrice.ProductPriceTypeID %>" />
        <input type="hidden" id="CurrencyID" value="<%=customerRetailTotal.CurrencyID %>" />
        <input type="hidden" id="RuleValidationID" value="<%=customerRetailTotal.RuleValidationID %>" />
	</div>
</div>
