//Dev. By salcedo vila G. G&S
(function ($) {

    //Parametro  , Valor 
    jQuery.SetParamSearchGridView = function (Parametro, Value)  {

        if (window.Data == undefined || window.Data == null) {
            window.Data = {};
        }
        window.Data[Parametro] = Value;
    }
})(jQuery);