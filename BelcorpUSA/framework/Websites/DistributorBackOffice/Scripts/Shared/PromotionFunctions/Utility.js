var show = (function () {
    var makeCallback = function (url) {
        return function () {
            var buttons = $('#btnContinue,#btnNext');
            $.post(url, function (response) {
                if (response) {
                    buttons.show();
                    $('#partyOrderBelowMinimumAmountMessage').hide();
                } else {
                    buttons.hide();
                    $('#partyOrderBelowMinimumAmountMessage').show();
                }
            });
        };
    };
    return { makeCallback: makeCallback };
} ());


//checks for us and canada
function verifyPostalCode(value) {
    return /(^\d{5}$)|(^[ABCEGHJKLMNPRSTVXY]\d[A-Z] *\d[A-Z]\d$)/.test(value.toUpperCase());
}                  
                  
