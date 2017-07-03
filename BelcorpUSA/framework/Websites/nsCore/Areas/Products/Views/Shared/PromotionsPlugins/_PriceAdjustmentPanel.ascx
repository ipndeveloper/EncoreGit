<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Base.PriceAdjustmentPromotionModel>" %>
<script type="text/javascript">
    $(function () {
        $('.txtQuickAdd').jsonSuggest('<%= ResolveUrl("~/Products/Promotions/Search") %>', {
            minCharacters: 3, ajaxResults: true, onSelect: function (item) {
                $(this).text(item.text);
                $('#' + $(this).attr('hiddenid')).val(item.id);
            }
        });
    });
</script>
<div id="RewardsPanel">
    <% Html.RenderPartial("PromotionsPlugins/_CartPriceAdjustmentConditions"); %>
</div>
