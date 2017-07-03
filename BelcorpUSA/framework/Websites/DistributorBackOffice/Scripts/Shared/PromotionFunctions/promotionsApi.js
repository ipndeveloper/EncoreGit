var apiPromoFunctions = (function () {
    var _selectedPromotionId,
        defaultPromoFunctions = {
            hideElementSetPromotionId: function ($element) {
                var $parent = $element.parents('div.promotionCoupon').parent();
                $parent.hide();
                this.setPromotionId($parent.attr('id'));
            },
            getCouponCode: function ($element) {
                return $element.find('span.promotionCode').text();
            },
            findCouponCodeBySelected: function () {
                return $('#' + _selectedPromotionId).find('span.promotionCode').text();
            },
            setPromotionId: function (id) { _selectedPromotionId = id; },
            getPromotionId: function () { return _selectedPromotionId; }
        };

    function wireUpClickHandlers(options) {
        var mainSelector = options["mainSelector"],
            subSelector = options["subSelector"],
            actionName = options["actionName"],
            promoFunctions = options["promoFunctions"],
            allFunctions = $.extend({}, defaultPromoFunctions, promoFunctions);

        if (!mainSelector || !subSelector || !actionName) return;

        $(mainSelector).delegate(subSelector, "click", function () {
            var $element = $(this),
                actionNameToUse = typeof actionName === "function" ? actionName($element) : actionName,
                action = allFunctions[actionNameToUse];

            if (action && typeof action === "function") {
                action.apply(defaultPromoFunctions, [$element]);
            }
            else {
                console.log('missing action: ' + actionNameToUse);
            }
        });

    }
    return {
        wireUpClickHandlers: wireUpClickHandlers
    };
} ());

//The following is examples of how you could set up the promotion functions.
//apiPromoFunctions.wireUpClickHandlers({
//    mainSelector: "body",
//    subSelector: "a.promotionCodeBtn",
//    actionName: "hideAndInject"
//});
//apiPromoFunctions.wireUpClickHandlers({
//    mainSelector: "body",
//    subSelector: "a.promotionCodeBtn",
//    actionName: function ($element) { return $element.attr('title'); }
//});
//apiPromoFunctions.wireUpClickHandlers({
//    mainSelector: "body",
//    subSelector: "a.promotionCodeBtn",
//    actionName: "myTestAction",
//    promoFunctions: {
//        myTestAction: function ($element) {
//            console.log('i provided my own function!');
//        }
//    }
//});
//apiPromoFunctions.setPromotionId(1);
//console.log(apiPromoFunctions.getPromotionId())
