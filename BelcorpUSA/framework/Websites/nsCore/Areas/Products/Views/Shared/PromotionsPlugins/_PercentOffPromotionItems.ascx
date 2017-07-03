<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<nsCore.Areas.Products.Models.Promotions.PromotionProductModel>>" %>
<script type="text/javascript">
	$(function () {
		$('#percentForm').show();
		$('#priceAdjLabel, #cvAdjLabel, #qvAdjLabel').text('<%=Html.Term("DefinePromotion_PercentageOff", "Percentage Off") %>');
		$('#percentOffApplyToSelected').click(function () { setPercentages($('#paginatedGrid .selectRow:checked').closest('tr')); });
		$('span.helperSymbol').each(function () {
			$(this).clone().text('%').appendTo($(this).closest('td'));
		});
	});

	function setPercentages(rows) {
    
		var retail = $('#pctRetailPrice').val(),
			qv = $('#pctQVPrice').val(),
			cv = $('#pctCVPrice').val();
		$.each(rows, function () {
		    
			var t = $(this);
			t.find('input.retailPrice').val(retail).change(); //trigger change to update price info
			t.find('input.qvPrice').val(qv);
			t.find('input.cvPrice').val(cv);
		});
	}
</script>
<% Html.RenderPartial("PromotionsPlugins/_PromotionItems"); %>
