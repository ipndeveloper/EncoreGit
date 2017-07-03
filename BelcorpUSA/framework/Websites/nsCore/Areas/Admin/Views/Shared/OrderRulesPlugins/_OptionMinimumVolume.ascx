<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RuleValidationsDTO>" %>
<script type="text/javascript">
    <% ProductPriceType productPriceQV = SmallCollectionCache.Instance.ProductPriceTypes.Where(x => x.ProductPriceTypeID == (int)Constants.ProductPriceType.QV).FirstOrDefault();
            
           var customerQVTotal = NetSteps.Encore.Core.IoC.Create.New<OrderRules.Service.DTO.CustomerPriceTotalDTO>();
            if (Model.CustomerPriceTotalDTO.Where(x => x.ProductPriceTypeID == productPriceQV.ProductPriceTypeID).Count() > 0)
            {
                customerQVTotal = Model.CustomerPriceTotalDTO.Where(x => x.ProductPriceTypeID == productPriceQV.ProductPriceTypeID).FirstOrDefault();
            }else
            {
                customerQVTotal.MinimumAmount = 0;
                customerQVTotal.MaximumAmount = 0;
                customerQVTotal.ProductPriceTypeID = productPriceQV.ProductPriceTypeID;
                customerQVTotal.CurrencyID = Market.Load(CoreContext.CurrentMarketId).GetDefaultCurrencyID();
                customerQVTotal.RuleValidationID = 0;
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
        var rsltdo = <%= (Model.CustomerPriceTotalDTO.Where(x => x.ProductPriceTypeID == productPriceQV.ProductPriceTypeID).Count() > 0 ? "true" : "false") %>;
        $('#chkMinimumVolume').attr('checked', rsltdo).trigger('change');
        $('#txtMinimumVolumeQV').numeric({ allowNegative: false, allowDecimal: true });
        $('#txtMinimumVolumeQV').addClass(rsltdo ? 'required' : '');
         
        $('#chkMinimumVolume').attr('checked', rsltdo).change(function() {
            if(this.checked) {
                $('#txtMinimumVolumeQV').addClass('required');
            }else
            {
                $('#txtMinimumVolumeQV').removeClass('required');
            }
        });
	});
</script>
<div class="pad5 oneTimeUse percentOff">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel" style="width:170px;">
			<%=Html.Term("Rules_MinimumVolumeRequired", "Minimum Volume Required?")%>
	</div>
	<div rel="isYes" class="hasPanel" style="height:29px;">
		<span>
            <input type="checkbox" id="chkMinimumVolume" />
        </span>
        <span>
            <input type="text" size="10" name="Minimum Volume cannot be empty" class="pad2 required" value="<%=decimal.Round((decimal)(customerQVTotal.MinimumAmount ?? 0), 2) %>" id="txtMinimumVolumeQV"  monedaidioma='CultureIPN'/>
            <input type="hidden" id="CustomerPriceTotalIDQV" value="<%=customerQVTotal.CustomerPriceTotalID %>" />
            <input type="hidden" id="ProductPriceTypeIDQV" value="<%=productPriceQV.ProductPriceTypeID %>" />
            <input type="hidden" id="CurrencyIDQV" value="<%=customerQVTotal.CurrencyID %>" />
            <input type="hidden" id="RuleValidationIDQV" value="<%=customerQVTotal.RuleValidationID %>" />
        </span>
	</div>
</div>
