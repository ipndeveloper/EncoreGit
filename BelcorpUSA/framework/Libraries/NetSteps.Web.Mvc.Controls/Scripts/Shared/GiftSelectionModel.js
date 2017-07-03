function GiftSelectionModel(data) {
    var self = this;
    
    self.addSelection = function (giftModel, errorMessage) {

        var cad = giftModel.Value().toString();
        cad = cad.substring(1);
        var num = parseFloat(cad);
        var IsEspecialPromo = self.IsEspecialPromo();
        var condition = (IsEspecialPromo != null && IsEspecialPromo) ? self.remainingQuantity() >= num : self.remainingQuantity() > 0;

        if (condition) {
            var clone = ko.mapping.fromJS(ko.toJS(giftModel));
            self.SelectedOptions.unshift(clone);
            $('#SelectedPromoProductsList .promoProduct:first').animate({
                'height': 'toggle',
                'opacity': 'show'
            }, 500);
            
           }
        else {
            alert(errorMessage);
        }


    }

    self.removeSelection = function (giftModel, event) {
        $(event.target).closest('div.promoProduct').animate({
            'height': 'toggle',
            'opacity': 'hide'
        }, 500, function () { self.SelectedOptions.remove(giftModel); });
    }

    self.saveGifts = function () {
        var data = {
            stepId: self.StepID(),
            productIds: self.SelectedOptions().map(function (o) { return o.ProductID(); })
        };
        var options = {
            url: self.SaveGiftSelectionUrl(),
            traditional: true,
            data: data,
            showLoading: $('#btnSave'),
            success: function (response) {
                if (response.result) {
                    $('#SelectPromotionProducts').jqmHide();
                    if (self.JavaScriptSaveCallbackFunctionName()) {
                        window[self.JavaScriptSaveCallbackFunctionName()](response);
                    }
                }
                else {
                    showMessage(response.message, true);
                }
            }
        };
        NS.post(options);
    }


    self.updateFromJS = function (data) {
        ko.mapping.fromJS(data, {}, self);
    };

    //init
    if (data) {
        self.updateFromJS(data)
    }

    self.Sum = ko.computed(function () {
        var sum = 0.0;
        var arr = self.SelectedOptions();
        var i = 0;

        for (; i < arr.length; i++) {
            sum += parseFloat(arr[i].Value().toString().substring(1));
        }
        return sum;
    });

    self.remainingPlus = ko.computed(function () {

        return ((self.IsEspecialPromo() != null && self.IsEspecialPromo()) ? self.Sum() : self.SelectedOptions().length);

    });
    self.remainingQuantity = ko.computed(function () {
        return ((self.IsEspecialPromo() != null && self.IsEspecialPromo()) ? parseFloat(self.MaxQuantity().toString()) - self.Sum() : self.MaxQuantity() - self.SelectedOptions().length);

    });
}