$(document).ready(function () {

    $("#txtMaterialCode").fn_util_validarNumeros();
    $("#txtEanCode").fn_util_validarNumeros();
    $("#txtWeigt").fn_util_validaDecimal(2);
    $("#txtVolume").fn_util_validaDecimal(2);
});
///***********************************************************************
//* ***  Registrar Material
//***********************************************************************/

function RegisterMaterial() {

    if ($('#newMaterial').checkRequiredFields()) {

        var enMaterial = new Object();

        var txtMaterialCode = $("#txtMaterialCode").val();
        var txtName = $("#txtName").val();
        var TxtMeasurement = $("#TxtMeasurement").val();
        var TxtBPCS = $("#TxtBPCS").val();
        var txtVolume = $("#txtVolume").val();
        var txtBrand = $("#txtBrand").val();
        var txtGroup = $("#txtGroup").val();
        var txtNCM = $("#txtNCM").val();
        var txtOrigin = $("#txtOrigin").val();
        var txtMarket = $("#txtMarket").val();
        var txtEanCode = $("#txtEanCode").val();
        var txtEanCode = $("#txtEanCode").val();
        var txtWeigt = $("#txtWeigt").val();  
        
         
            enMaterial.Name=txtName;
            enMaterial.SKU = txtMaterialCode;
            enMaterial.Active =true;
            enMaterial.EANCode=txtEanCode;
            enMaterial.BPCSCode =TxtBPCS;
            enMaterial.UnityType =TxtMeasurement;
            enMaterial.Weight = txtWeigt;
            enMaterial.Volume = txtVolume;
            enMaterial.NCM =TxtMeasurement;
            enMaterial.Origin = txtOrigin;
            enMaterial.OriginCountry =0;
            enMaterial.Brand = txtBrand;
            enMaterial.Group = txtGroup;
            enMaterial.MarketID = txtMarket;

            var objEnPuntoCobranza = { oenMaterial: enMaterial };
            var sParams = JSON.stringify(objEnPuntoCobranza);

            $.ajax({
                type: "POST",
                url: $.domain(''),
                data: sParams,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                async: "false",
                success: function (result) {


                },
                error: function (resultado) {



                }
            });
    }

    }

    function validarEAN() {
        var enMaterial = new Object();
        var objEnPuntoCobranza = { oenMaterial: enMaterial };
        var sParams = JSON.stringify(objEnPuntoCobranza);

        $.ajax({
            type: "POST",
            url: $.domain(''),
            data: sParams,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: "false",
            success: function (result) {

                return resul.existe;
            },
            error: function (resultado) {



            }
        });

    }