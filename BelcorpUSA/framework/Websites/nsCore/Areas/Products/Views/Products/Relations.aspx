<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Products.Models.ProductRelationModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/json2.js") %>"></script>
    <script type="text/javascript">

        var fillProductRelationsPerPeriod = function (data, indexParent) {
            var lstProductRelationsPerPeriod = $('#ProductRelationsPerPeriod');
            lstProductRelationsPerPeriod.html('');
            $.each(data.ProductRelationsPerPeriod, function (i, v) {
                lstProductRelationsPerPeriod.append($('<option>',
                                        {
                                            value: data.MaterialID + ',' + v.PeriodID,
                                            text: v.PeriodID + ' - ' + v.OfferType + ' - ' + v.ExternalCode
                                        }).attr("index", i)
                                          .attr("indexParent", indexParent)
                                        );
            });
        };

        function Validaciones() {
            this.ProductRelationsTypes =
        {
            RelatedItem: 1,
            Kit: 2
        };

            this.GetCurrentProductRelationsTypesSelected = function () {
                return $("#relationshipType").val();
            }
            return this;
        }


        $(function () {
            $('.numeric').numeric();

            var selectedProduct, selectedMaterial, selectedValue, porc, cont = 0, maxdiff = 0.01;
            var RelationModelMaterials = [];
            var RelationModelMaterialsEliminados = [];

            var dsRelationship;

            var productName = '';
            var replacementName = '';

            $("#txtProductRelationshipSearch").keyup(function () {
                if (productName != '' && $(this).val() != productName) {
                    selectedProduct = null;
                    productName == '';
                }
            });

            $("#txtMaterialsReplacementSearch").keyup(function () {
                if (replacementName != '' && $(this).val() != replacementName) {
                    selectedMaterial = null;
                    replacementName == '';
                }
            });

            //Load Total //Developed by WCS - CSTI
            $('#relations option').each(function () {
                var index = parseFloat($(this).text().split(' - ').length);
                porc = $(this).text().split(' - ')[index - 1].replace('%', '');
                cont += parseFloat(porc);
                $("#txtTotalParticipation").val(parseFloat(cont).toFixed(2));
            });

            //Developed by WCS - CSTI
            function ClsTextBoxes() {
                //                $("#txtParticipationPercent,#qtyOfProduct,#txtProductRelationshipSearch,#txtOfertType,#txtExternalCode").val('');
                $("#txtParticipationPercent,#qtyOfProduct,#txtProductRelationshipSearch").val('');
            }

            $('#txtProductRelationshipSearch').watermark('<%= Html.JavascriptTerm("EnterMaterialtNameOrSKU", "Enter material name or SKU") %>')
                .jsonSuggest('<%= ResolveUrl("~/Products/Materials/SearchFilter") %>', {
                    minCharacters: 3, ajaxResults: true, onSelect: function (item) {
                        selectedProduct = item.id;
                        productName = $('#txtProductRelationshipSearch').val();
                        $('#txtProductRelationshipSearch').clearError();
                    }
                });

            //Developed by ECH - BACK
            $('#txtMaterialsReplacementSearch').watermark('<%= Html.JavascriptTerm("EnterMaterialtNameOrSKU", "Enter material name or SKU") %>')
                .jsonSuggest('<%= ResolveUrl("~/Products/Materials/SearchFilter") %>', {
                    minCharacters: 3, ajaxResults: true, onSelect: function (item) {
                        selectedMaterial = item.id;
                        replacementName = $('#txtMaterialsReplacementSearch').val();
                        $('#txtMaterialsReplacementSearch').clearError();
                    }
                });
            $('.PricingTypeID').change(function () {
                var pricingTypeID = $('input:radio[name=PricingTypeID]:checked').val();

                NS.post({
                    url: '<%= ResolveUrl(string.Format("~/Products/Products/UpdateKitPricingType/{0}/{1}", Model.Product.ProductBaseID, Model.ProductID)) %>',
                    data: { pricingTypeID: pricingTypeID },
                    success: function (response) {
                        if (response.success) {

                        }

                        showMessage(response.message, !response.success);
                    }
                });
            });

            function ProductExistInList(productSKU) {

                var i;
                for (i = 0; i < RelationModelMaterials.length; i++) {
                    if (productSKU == RelationModelMaterials[i].SKU)
                        return true;
                }

                return false;

            }

            //Developed by KLC - CSTI
            function CalculatingTotalParticipations() {
                var total = 0.0;
                var i;
                for (i = 0; i < RelationModelMaterials.length; i++) {
                    total += eval(RelationModelMaterials[i].ParticipationPercentage);
                }

                $("#txtTotalParticipation").val(total.toFixed(2));

            }
            //Developed by WCS - CSTI
            function CalculatingTotalParticipation() {
                var tp = parseFloat($("#txtTotalParticipation").val());
                tp += parseFloat($("#txtParticipationPercent").val());
                $("#txtTotalParticipation").val(tp.toFixed(2));
            }

            //Developed by ECH - BACK
            function GetObjectsFromArray(ds, pkName, val) {
                var dsResult = [];
                if (ds != null)
                    $.each(ds, function (i, v) {
                        if (v != null && eval('v.' + pkName) == val) {
                            dsResult.push(v);
                        }
                    });
                return dsResult;
            }

            function CrearListaMateriales(lista) {// index="'+i+'"
                $('#relations').empty();
                var i = 0;
                for (i = 0; i < lista.length; i++) {
                    if (lista[i].ParticipationPercentage.toString().indexOf('.') == -1 && lista[i].ParticipationPercentage.toString().indexOf(',') == -1) {
                        lista[i].ParticipationPercentage = RelationModelMaterials[i].ParticipationPercentage + '.00';
                    }
                    else {
                        var inicial = lista[i].ParticipationPercentage.toString().indexOf('.') + 1;
                        var cad = lista[i].ParticipationPercentage.toString().substring(inicial, lista[i].ParticipationPercentage.length);
                        if (cad.length == 1) {
                            lista[i].ParticipationPercentage = RelationModelMaterials[i].ParticipationPercentage + '0';
                        }
                    }
                }

                $.each(lista, function (i, v) {
                    $('#relations').append($('<option>',
                            {
                                value: v.RelationsTypeID
                                + ','
                                + v.MaterialID
                    	        , text: getTextFromSelect(v.RelationsTypeID)
                                + ': '
                                + v.SKU
                                + ' - '
                                + v.Name
                                + ' - '
                                + v.ParticipationPercentage
                                + '%'
                            }
                        ).attr("index", i));
                });
            }

            //Developed by ECH - BACK
            function GetMaterials() {
                var data = {};
                $('#relations,#replacements').html('');
                $.post('<%= ResolveUrl(string.Format("~/Products/Products/GetMaterials/{0}/{1}", Model.Product.ProductBaseID, Model.ProductID)) %>', data, function (response) {
                    RelationModelMaterials = response.result.Relations;
                    CalculatingTotalParticipations(); // Developed By KLC - CSTI
                    CrearListaMateriales(RelationModelMaterials);
                    var relationType = RelationModelMaterials[RelationModelMaterials.length - 1].RelationsTypeID;
                    $('#relationshipType').val(relationType);
                    relationshipTypeChange();
                });
            }

            //Developed by ECH - BACK
            function GetObjectsFromArray(ds, pkName, val) {
                var dsResult = [];
                if (ds != null)
                    $.each(ds, function (i, v) {
                        if (v != null && eval('v.' + pkName) == val) {
                            dsResult.push(v);
                        }
                    });
                return dsResult;
            }
            function GetObjectFromArray(ds, pkName, val) {
                var item = null;
                if (ds != null)
                    $.each(ds, function (i, v) {
                        if (v != null && eval('v.' + pkName) == val) {
                            item = v;
                            return false;
                        }
                    });
                return item;
            }

            function removeItem(obj, prop, val) {
                var c, found = false;
                for (c in obj) {
                    if (obj[c][prop] == val) {
                        found = true;
                        break;
                    }
                }
                if (found) {
                    //delete obj[c];
                    obj.splice(c, 1);
                }
            }

            function getTextFromSelect(value) {
                return $('#relationshipType option').filter(function () { return $.trim($(this).val()) == value; }).text();
            }

            function cloneJSON(obj) {
                // basic type deep copy
                if (obj === null || obj === undefined || typeof obj !== 'object') {
                    return obj
                }
                // array deep copy
                if (obj instanceof Array) {
                    var cloneA = [];
                    for (var i = 0; i < obj.length; ++i) {
                        cloneA[i] = cloneJSON(obj[i]);
                    }
                    return cloneA;
                }
                // object deep copy
                var cloneO = {};
                for (var i in obj) {
                    cloneO[i] = cloneJSON(obj[i]);
                }
                return cloneO;
            }

            //Developed by WCS - CSTI
            $(".justNumbers").keydown(function (event) {
                if (event.shiftKey) event.preventDefault();
                if (event.keyCode == 46 || event.keyCode == 8) {
                }
                else {
                    if (event.keyCode < 95) {
                        if (event.keyCode < 48 || event.keyCode > 57) event.preventDefault();
                    }
                    else {
                        if (event.keyCode < 96 || event.keyCode > 105) event.preventDefault();
                    }
                }
            });

            function relationshipTypeChange() {
                var selValue = $('#relationshipType').val();
                if (selValue == "1") {
                    $("#qtyOfProduct").attr("disabled", "disabled").val('1');
                    $("#qtyOfProduct").val('');
                    $("#txtParticipationPercent").val('100');
                    $("#ParticipationItems").hide();
                }
                else {
                    $("#qtyOfProduct").removeAttr("disabled").val('').focus();
                    $("#txtParticipationPercent").val('');
                    $("#ParticipationItems").show();
                }
            };

            $('#relationshipType').change(function () {
                selectedValue = $(this).val();

                if (selectedValue == "1") {
                    $("#qtyOfProduct").attr("disabled", "disabled").val('1');
                    $("#qtyOfProduct").val('');
                    $("#txtParticipationPercent").val('100');
                    $("#ParticipationItems").hide();
                }
                else {
                    $("#qtyOfProduct").removeAttr("disabled").val('').focus();
                    $("#txtParticipationPercent").val('');
                    $("#ParticipationItems").show();
                }
            });

            //            function ValidateRestrictionsByValue() {
            //                
            //                var _productRelationTypeID = $("#relationshipType").val();

            //                if (_productRelationTypeID == 2) {
            //                    var valorPorcentajeEditado = $("#txtParticipationPercent").val();

            //                    var valorPorcentajeParametro = '<%= Model.ParticipacionPorcentaje %>';
            //                    if (valorPorcentajeEditado < valorPorcentajeParametro) {
            //                        var mensaje = ('<%= Html.Term("ParticipationRateIsLessThan", "Participation rate is less than") %>');
            //                        showMessage(mensaje + " " + valorPorcentajeParametro, true);
            //                        return;
            //                    }
            //                }
            //                AgregarRelation();
            //            }

            //Developed by WCS - CSTI
            $('#btnAddRelation').click(function () {
                //                ValidateRestrictionsByValue();
                AgregarRelation();
            });

            function AgregarRelation() {

                var _productRelationTypeID = $("#relationshipType").val();
                var ProductRelationTypeID = Validaciones().GetCurrentProductRelationsTypesSelected();
                var name = $("#txtProductRelationshipSearch").val();
                name = name.substring(name.indexOf("-") + 1, name.length)

                if (!!selectedProduct) {
                    var modelfinal, modelrepite;
                    var relationExists = false;
                    $('#relations option').each(function () {
                        if (this.value == $('#relationshipType').val() + ',' + selectedProduct) {
                            relationExists = true;
                            return false;
                        }
                    });
                    selectedValue = $('#relationshipType').val();
                    var selSKU = $("#txtProductRelationshipSearch").val().substr(0, $("#txtProductRelationshipSearch").val().indexOf("-"));
                    if (ProductExistInList(selSKU)) {
                        $("#txtProductRelationshipSearch").showError("This Product already was added.");
                        return false;
                    }
                    var option = '';
                    var quantity;

                    var iPerc;
                    var fOpt;
                    var isFinalOpt;
                    isFinalOpt = false;
                    if ($("#qtyOfProduct").val() == '') quantity = 1;
                    else quantity = parseFloat($("#qtyOfProduct").val());

                    iPerc = (parseFloat($("#txtParticipationPercent").val()) / quantity).toFixed(2);
                    if (iPerc * quantity != parseFloat($("#txtParticipationPercent").val())) {
                        isFinalOpt = true;
                    }
                    option = "<option value=\""
                              + $('#relationshipType').val()
                              + ","
                              + selectedProduct
                              + "\">"
                              + $('#relationshipType option:selected').text()
                              + ": "
                              + $("#txtProductRelationshipSearch").val()
                              + " - "
                              + (parseFloat($("#txtParticipationPercent").val()) / quantity).toFixed(2)
                              + "%"
                              + "</option>";

                    modelrepite = { 'RelationsTypeID': $('#relationshipType').val()
	                    , 'ChildSku': $("#txtProductRelationshipSearch").val().substr(0, $("#txtProductRelationshipSearch").val().indexOf("-"))
	                    , 'ParticipationPercentage': (parseFloat($("#txtParticipationPercent").val()) / quantity).toFixed(2)
	                    , 'OfertType': ''//$("#txtOfertType").val()
	                    , 'ExternalCode': ''//$("#txtExternalCode").val()
	                    , 'MaterialID': String(selectedProduct)
	                    , 'Replacements': []
                        , 'ProductRelationsPerPeriod': []
                        , 'index': 0
                        , 'Name': name
                        , 'SKU': $("#txtProductRelationshipSearch").val().substr(0, $("#txtProductRelationshipSearch").val().indexOf("-"))
                    };
                    if (isFinalOpt) {
                        fOpt = "<option value=\""
                               + $('#relationshipType').val()
                               + ","
                               + selectedProduct
                               + "\">"
                               + $('#relationshipType option:selected').text()
                               + ": "
                               + $("#txtProductRelationshipSearch").val()
                               + " - "
                               + (parseFloat($("#txtParticipationPercent").val()) - (parseFloat(iPerc) * (quantity - 1))).toFixed(2)
                               + "%"
                               + "</option>";

                        modelfinal = { 'RelationsTypeID': $("#relationshipType").val()
	                    , 'ChildSku': $("#txtProductRelationshipSearch").val().substr(0, $("#txtProductRelationshipSearch").val().indexOf("-"))

                        , 'SKU': $("#txtProductRelationshipSearch").val().substr(0, $("#txtProductRelationshipSearch").val().indexOf("-"))
                        , 'Name': name
	                    , 'ParticipationPercentage': (parseFloat($("#txtParticipationPercent").val()) - (parseFloat(iPerc) * (quantity - 1))).toFixed(2)
	                    , 'OfertType': ''//$("#txtOfertType").val()
	                    , 'ExternalCode': ''//$("#txtExternalCode").val()
	                    , 'MaterialID': String(selectedProduct)
	                    , 'Replacements': []
                        , 'ProductRelationsPerPeriod': []
                        , 'index': 0
                        };
                    }

                    if (eval($("#txtParticipationPercent").val()) <= 0) {
                        $("#txtParticipationPercent").showError("Please enter a valid % Participation"); return;
                    }

                    if (selectedValue != "1") {
                        var exists = false;
                        $('#relations option').each(function () {
                            var value = $(this).val().split(",")[0];
                            if (value == '1') {
                                showMessage('<%= Html.Term("SelectOneMaterialTypeRelatedItem", "you can only select a material type when [Related item].") %>', true)
                                exists = true;
                                return;
                            }
                        });
                        if (exists) return false;

                        if ($("#qtyOfProduct").val() != "") {
                            if ($("#txtParticipationPercent").val() != "") {
                                if (parseFloat($("#txtParticipationPercent").val()) > 100) {
                                    $("#txtParticipationPercent").showError("% Participation doesn't must be greater than 100%");
                                }
                                else {

                                    if (isFinalOpt) {

                                        for (var i = 0; i < quantity - 1; i++) {
                                            $('#relations').append(option);
                                            var cloneRepite = cloneJSON(modelrepite);
                                            cloneRepite.index = i + 1;
                                            RelationModelMaterials.push(cloneRepite);
                                        }
                                        $('#relations').append(fOpt);
                                        var cloneFinal = cloneJSON(modelfinal);
                                        cloneFinal.index = quantity + 1;
                                        RelationModelMaterials.push(cloneFinal);
                                    }
                                    else {
                                        for (var i = 0; i < quantity; i++) {
                                            $('#relations').append(option);
                                            var cloneRepite = cloneJSON(modelrepite);
                                            cloneRepite.index = i + 1;
                                            RelationModelMaterials.push(cloneRepite);
                                        }
                                    }
                                    CalculatingTotalParticipation();
                                    ClsTextBoxes();
                                }
                            }
                            else {
                                $("#txtParticipationPercent").showError("Please enter % Participation");
                            }
                        } else {
                            $("#qtyOfProduct").showError("Please enter number of Products");
                        }
                    }
                    else {
                        //validar {  solo permitir el ingreso de un metarial cuando la relacion es de tipo  "Ralated item"
                        var CantidadMaterialSeleccionado = $("#relations option").length;
                        if (CantidadMaterialSeleccionado > 0) {
                            showMessage('<%= Html.Term("SelectOneMaterialTypeRelatedItem", "you can only select a material type when [Related item].") %>', true)
                            return false;
                        }

                        if ($("#txtParticipationPercent").val() != "") {
                            if (parseFloat($("#txtParticipationPercent").val()) > 100) {
                                $("#txtParticipationPercent").showError("% Participation doesn't must be greater than 100%");
                            }
                            else {
                                if (!relationExists) {
                                    $('#relations').append(option);
                                    var cloneRepite = cloneJSON(modelrepite);
                                    cloneRepite.index = RelationModelMaterials.length + 1;
                                    RelationModelMaterials.push(cloneRepite);
                                    CalculatingTotalParticipation();
                                    ClsTextBoxes();
                                    $('#relationshipType').change();
                                } else {
                                    $('#relations option[value="'
                                    + $('#relationshipType').val()
                                    + ','
                                    + selectedProduct
                                    + '"]').attr('selected', 'selected');
                                }
                            }
                        }
                        else {
                            $("#txtParticipationPercent").showError("Please enter % Participation");
                        }
                    }
                    selectedProduct = '';
                }
                else {
                    $('#txtProductRelationshipSearch').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterial", "Please select a material") %>');
                }
                CrearListaMateriales(RelationModelMaterials);
            }

            $('#btnAddProductRelationsPerPeriod').click(function () {
                var ProductRelationTypeID = Validaciones().GetCurrentProductRelationsTypesSelected();
                var CantidadMaterialSeleccionado = $("#relations option:selected").length;
                var dataVal;
                if (ProductRelationTypeID == 1 & $('#relations option').length > 1) {
                    showMessage('<%= Html.Term("SelectOneMaterialTypeRelatedItem", "you can only select a material type when [Related item].") %>', true)
                    return false;
                }
                //                if (ProductRelationTypeID == 2 & $('#relations option:selected').length == 0) {
                if (ProductRelationTypeID == 2 & CantidadMaterialSeleccionado == 0) {
                    $('#txtExternalCode').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterialPrincipalOfList", "Please select a material principal of list") %>');
                    return false;
                }
                if ((ProductRelationTypeID == 2 || ProductRelationTypeID == 1) & $('#relations option:selected').length > 1) {
                    showMessage('<%= Html.Term("SelectedOnlyRelations", "Only must select a material, in current relationships on this product.") %>', true)
                    return false;
                }
                if (ProductRelationTypeID == 1 & CantidadMaterialSeleccionado == 0) {
                    $('#txtExternalCode').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterialPrincipalOfList", "Please select a material principal of list") %>');
                    return false;
                }
                if (!$("#txtOfertType").val()) {
                    $("#txtOfertType").showError("Please enter Ofert Type"); return;
                }
                if (!($("#txtOfertType").val().length == 3)) {
                    $("#txtOfertType").showError("Ofert Type must be 3 digits"); return;
                }
                if (!$("#txtExternalCode").val()) {
                    $("#txtExternalCode").showError("Please enter External Code"); return;
                }
                if (!($("#txtExternalCode").val().length == 5)) {
                    $("#txtExternalCode").showError("External Code must be 5 digits"); return;
                }

                var PeriodOfferTypeExists = false;
                $('#ProductRelationsPerPeriod option').each(function () {
                    if ($(this).val().split(",")[1] == $('#ddlPeriod').val()) {
                        $("#ddlPeriod").showError("There is an Offer Type and External Code for this period");
                        PeriodOfferTypeExists = true;
                        return;
                    }
                });

                if (PeriodOfferTypeExists) return;

                dataVal = ProductRelationTypeID == 1 ? $('#relations option:eq(0)') : $('#relations option:selected');

                var dsRelationship = RelationModelMaterials.filter(function (item) { return (item.MaterialID == dataVal.val().split(",")[1]); });
                var index = $(dataVal).attr("index");

                var MaterialID = dataVal.val().split(",")[1];
                $.each(RelationModelMaterials, function (i, item) {
                    if (item.MaterialID == MaterialID) {
                        item.ProductRelationsPerPeriod.push({ 'PeriodID': $('#ddlPeriod').val(),
                            'MaterialID': MaterialID,
                            'OfferType': $('#txtOfertType').val(),
                            'ExternalCode': $('#txtExternalCode').val()
                        });
                    }
                });
                if (ProductRelationTypeID == 1) {
                    $('#relations option:eq(0)').attr("selected", true);
                }

                $('#txtOfertType').val('');
                $('#txtExternalCode').val('');

                $('#relations').change();
            });

            //Developed by ECH - BACK
            $('#btnAddReplacement').click(function () {
                var ProductRelationTypeID = Validaciones().GetCurrentProductRelationsTypesSelected();
                var CantidadMaterialSeleccionado = $("#relations option").length;
                //}
                var dataVal;
                /*validacion : por Salcedo Vila G. G&S
                RelationshipType  
                1 Related item
                2 Kit 
                */
                if (ProductRelationTypeID == 1 & $('#relations option').length > 1) {
                    showMessage('<%= Html.Term("SelectOneMaterialTypeRelatedItem", "you can only select a material type when [Related item].") %>', true)
                    return false;
                }
                if (ProductRelationTypeID == 2 & $('#relations option:selected').length == 0) {
                    $('#txtMaterialsReplacementSearch').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterialPrincipalOfList", "Please select a material principal of list") %>');
                    return false;
                }
                if ((ProductRelationTypeID == 2 || ProductRelationTypeID == 1) & $('#relations option:selected').length > 1) {
                    showMessage('<%= Html.Term("SelectedOnlyRelations", "Only must select a material, in current relationships on this product.") %>', true)
                    return false;
                }

                if (ProductRelationTypeID == 1 & CantidadMaterialSeleccionado == 0) {
                    $('#txtMaterialsReplacementSearch').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterialPrincipalOfList", "Please select a material principal of list") %>');
                    return false;
                }
                //cuando es tipo relacion Kit se tiene que seleccionar el material principal                           
                //cuando es de tipo 1 solo se valida que tenga un elemento y se asocia al primero                                                         
                if ((!!selectedMaterial && !!$('#relations option:selected').val() && (ProductRelationTypeID == 2)) || (ProductRelationTypeID == 1 && CantidadMaterialSeleccionado > 0)) {
                    //si el tipo de relacion es 1 se toma el elemento principal de material[ya que si es de tipo Related Item solo se puede ingresar un material principal]
                    dataVal = ProductRelationTypeID == 1 ? $('#relations option:eq(0)') : $('#relations option:selected');

                    var dsRelationship = RelationModelMaterials.filter(function (item) { return (item.MaterialID == dataVal.val().split(",")[1]); });
                    var index = $(dataVal).attr("index");

                    //  $.each(dsRelationship, function (i, item) {	

                    if (selectedMaterial == undefined || selectedMaterial == null) {
                        $('#txtMaterialsReplacementSearch').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterial", "Please select a material") %>');
                        return;
                    }

                    var validateMaterial = true;
                    $("#replacements option").each(function () {
                        var materialID = $(this).val().split(',')[1];
                        if (materialID == selectedMaterial) {
                            validateMaterial = false;
                            return;
                        }
                    });
                    //----------------------------------------------------------------------------------------------------------------------
                    // validar que no exista en el Relations
                    //----------------------------------------------------------------------------------------------------------------------
                    var valorRelationshipSelect = dataVal.text();
                    var n = 0;
                    n = valorRelationshipSelect.indexOf(":");
                    if (n > 0)
                        n = n + 1;
                    var dat = valorRelationshipSelect.substring(n, valorRelationshipSelect.lenght);
                    var d = 0;
                    d = dat.indexOf("-", 20);
                    var RelationshipText = dat.substring(0, d);
                    RelationshipText = RelationshipText.replace('   ', '');
                    RelationshipText = RelationshipText.replace('-', '');
                    RelationshipText = RelationshipText.substring(1, 10);
                    var comparar = $('#txtMaterialsReplacementSearch').val().replace('-', '  ');
                    comparar = comparar.substring(0, 9);

                    if (RelationshipText == comparar) {
                        $('#txtMaterialsReplacementSearch').val('').showError('<%= Html.JavascriptTerm("MaterialSameRelations", "The material should not be the same as related.") %>');
                        return;
                    }
                    //-------------------------------------------------------------------------------------------------------------------
                    if (!validateMaterial) {
                        $('#txtMaterialsReplacementSearch').val('').showError('<%= Html.JavascriptTerm("MaterialAlreadyAdded", "The selected material was already added.") %>');
                        return;
                    }
                    else {
                        RelationModelMaterials[index].Replacements.push({ 'ParentMaterialID': dataVal.val().split(",")[1], 'MaterialID': selectedMaterial, 'Name': $('#txtMaterialsReplacementSearch').val() })
                    }

                    //RelationModelMaterials.filter(function (item) { return (item.MaterialID == dataVal[1]); })[0].Replacements.push({ 'ParentMaterialID': 66, 'Priority': 77 });
                    // });

                    $('#txtMaterialsReplacementSearch').val('');
                    selectedMaterial = null;
                }
                else {

                    if (!$('#relations option:selected').val())
                        $('#txtMaterialsReplacementSearch').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterialPrincipalOfList", "Please select a material principal of list") %>');
                    if (!selectedMaterial)
                        $('#txtMaterialsReplacementSearch').showError('<%= Html.JavascriptTerm("PleaseSelectAMaterial", "Please select a material") %>');
                }
                if (ProductRelationTypeID == 1) {
                    $('#relations option:eq(0)').attr("selected", true);
                }
                $('#relations').change();
            });

            $("#qtyOfProduct").focus(function () {
                $("#qtyOfProduct").clearError();
            });

            $("#txtExternalCode").focus(function () {
                $("#txtExternalCode").clearError();
            });

            $("#ddlPeriod").focus(function () {
                $("#ddlPeriod").clearError();
            });

            $("#txtOfertType").focus(function () {
                $("#txtOfertType").clearError();
            });

            //Developed by WCS - CSTI
            $("#txtParticipationPercent,#txtMaterialsReplacementSearch").focus(function () {
                $("#txtParticipationPercent,#txtMaterialsReplacementSearch").clearError();
            });

            //Developed by WCS - CSTI
            $('#btnRemoveRelation').click(function () {
                var childProducts = $('#relations').val(), data = {}, i, relationship;
                $('#replacements').html('');
                $('#ProductRelationsPerPeriod').html('');
                var CurrentItemSelected = $(this).val().split(',');
                // validaciones agregados por SvG G&S{
                var esEliminado = false;
                // RelationModelMaterials.splice(index,1);
                RelationModelMaterialsEliminados = [];
                for (var indice = 0; indice < RelationModelMaterials.length; indice++) {
                    $('#relations option').each(function () {
                        var index = $(this).attr("index");
                        if (indice == index && childProducts == $(this).val()) {
                            esEliminado = true;
                            return false;
                        }
                    });
                    if (!esEliminado) {
                        RelationModelMaterialsEliminados.push(RelationModelMaterials[indice]);
                    }
                    esEliminado = false;
                }
                RelationModelMaterials = [];
                RelationModelMaterials = RelationModelMaterialsEliminados;

                CrearListaMateriales(RelationModelMaterials);
                //}
                if (childProducts == null || childProducts.constructor != Array || childProducts.length <= 0) {
                    showMessage('<%= Html.Term("PleaseSelectARelationship", "Please select at least one relationship to remove.") %>', true);
                    return;
                }
                var dataText, percentage, index;
                $('#relations option:selected').each(function () {
                    index = parseFloat($(this).text().split(' - ').length);
                    dataText = $(this).text().split(' - ');
                    percentage = dataText[index - 1].replace('%', '');
                    var tp = parseFloat($("#txtTotalParticipation").val());
                    tp -= percentage;
                    if (tp < 0) tp = 0;
                    $("#txtTotalParticipation").val(parseFloat(tp).toFixed(2));
                    $(this).remove();
                });

                CalculatingTotalParticipations(); // Developed By KLC - CSTI
            });

            $('#btnRemoveProductRelationsPerPeriod').click(function () {
                var ProductRelationTypeID = Validaciones().GetCurrentProductRelationsTypesSelected();
                var childProducts = $('#ProductRelationsPerPeriod').val(), data = {}, i, relationship;
                var cantidadRelations = $("#relations option").length;

                if ($('#relations option').length == 0) return false;

                if ($('#ProductRelationsPerPeriod option').length == 0) return false;

                if (ProductRelationTypeID == 2) {
                    if ($('#relations option:selected').length == 0) {
                        return false;
                    }
                }
                if (ProductRelationTypeID == 1) {
                    if ($('#ProductRelationsPerPeriod option:selected').length == 0) {
                        return false;
                    }
                }
                if (ProductRelationTypeID == 1) {
                    if ($('#relations option').length > 1) {
                        showMessage('<%= Html.Term("SelectOneMaterialTypeRelatedItem", "you can only select a material type when [Related item].") %>', true)
                        return false;
                    }
                }
                //}
                var dataValRelation = ProductRelationTypeID == 2 ? $('#relations option:selected') : $('#relations option:eq(0)');
                var dataValReplace = $('#ProductRelationsPerPeriod option:selected').val().split(',');

                if (childProducts == null || childProducts.constructor != Array || childProducts.length <= 0) {
                    showMessage('<%= Html.Term("PleaseSelectAReplacement", "Please select at least one relationship to remove.") %>', true);
                    return;
                }
                var ProductRelationsPerPeriodTemp = [];
                $('#ProductRelationsPerPeriod option:selected').each(function () {
                    var MaterialID = $(this).val().split(',')[0];
                    var PeriodID = $(this).val().split(',')[1];

                    $.each(RelationModelMaterials, function (i, item) {
                        if (item.MaterialID == MaterialID) {
                            $.each(item.ProductRelationsPerPeriod, function (j, prpp) {
                                if (prpp.PeriodID != PeriodID) {
                                    //Eliminar
                                    ProductRelationsPerPeriodTemp.push(prpp);
                                }
                            });
                            item.ProductRelationsPerPeriod = ProductRelationsPerPeriodTemp
                            ProductRelationsPerPeriodTemp = [];
                        }
                    });
                });
                $("#relations").change();
            });

            //Developed by ECH - BACK
            $('#btnRemoveReplacement').click(function () {
                var ReplacementTemp = [];
                //GetMaterials();
                var ProductRelationTypeID = Validaciones().GetCurrentProductRelationsTypesSelected();
                /*validacion : por Salcedo Vila G. G&S
                RelationshipType  
                1 Related item
                2 Kit 
                */
                // validaciones agregados por SvG G&S{
                var childProducts = $('#replacements').val(), data = {}, i, relationship;
                var cantidadRelations = $("#relations option").length;

                if ($('#relations option').length == 0) {
                    return false;
                }
                if ($('#replacements option').length == 0) {
                    return false;
                }
                if (ProductRelationTypeID == 2) {
                    if ($('#relations option:selected').length == 0) {
                        return false;
                    }
                }
                if (ProductRelationTypeID == 1) {
                    if ($('#replacements option:selected').length == 0) {
                        return false;
                    }
                }
                if (ProductRelationTypeID == 1) {
                    if ($('#relations option').length > 1) {
                        showMessage('<%= Html.Term("SelectOneMaterialTypeRelatedItem", "you can only select a material type when [Related item].") %>', true)
                        return false;
                    }
                }
                //}
                var dataValRelation = ProductRelationTypeID == 2 ? $('#relations option:selected') : $('#relations option:eq(0)');
                var dataValReplace = $('#replacements option:selected').val().split(',');
                //var reemplazos = RelationModelMaterials.filter(function (item) { return (item.MaterialID == dataValRelation[1]); })[0].Replacements;
                // removeItem(RelationModelMaterials.filter(function (item) { return (item.MaterialID == dataValRelation[1]); })[0].Replacements, 'MaterialID', dataValReplace[1]);

                if (childProducts == null || childProducts.constructor != Array || childProducts.length <= 0) {
                    showMessage('<%= Html.Term("PleaseSelectAReplacement", "Please select at least one relationship to remove.") %>', true);
                    return;
                }
                //Cambios por SvG G&S{
                var indicePadre = $(dataValRelation).attr("index");
                ReplacementTemp = [];
                var esEliminado = false;
                for (var _index = 0; _index < RelationModelMaterials[indicePadre].Replacements.length; _index++) {
                    $('#replacements option:selected').each(function () {
                        var indexCurrentDelete = $(this).attr("index");

                        if (_index == indexCurrentDelete) {
                            esEliminado = true;
                            return false;
                        }
                    });
                    if (!esEliminado) {
                        ReplacementTemp.push(RelationModelMaterials[indicePadre].Replacements[_index])
                    }
                    esEliminado = false;
                }
                RelationModelMaterials[indicePadre].Replacements = [];
                RelationModelMaterials[indicePadre].Replacements = ReplacementTemp;
                $("#relations").change();
                //}
            });
            //Developed by WCS - CSTI
            $('#btnSaveKit').click(function () {
                var data = {}, rowVal, rowText, dataVal, index, dataText, counter = 0, i = 0, j = 0;
                var PeriodList = [];
                var ProductRelationTypeID = Validaciones().GetCurrentProductRelationsTypesSelected();

                if (ProductRelationTypeID == 1 & $('#relations option').length > 1) {
                    showMessage('<%= Html.Term("SelectOneMaterialTypeRelatedItem", "you can only select a material type when [Related item].") %>', true)
                    return false;
                }

                if ($('#relations option').length <= 0) {
                    showMessage("Please add at least one relationship.", true);
                    return;
                }

                $('#relations option').each(function () {
                    rowVal = $(this).val();
                    rowText = $(this).text();
                    dataVal = rowVal.split(',');
                    dataText = rowText.split(' - ');
                    index = parseFloat(dataText.length);
                    counter += parseFloat(dataText[index - 1].replace('%', ''));
                });
                var AreThereProductRelationsPerPeriodForAll = true;
                $.each(RelationModelMaterials, function (i, item) {
                    data['relationships[' + i + '].RelationsTypeID'] = item.RelationsTypeID;
                    data['relationships[' + i + '].ChildProductID'] = 0;
                    data['relationships[' + i + '].ChildSku'] = item.MaterialID; //item.ChildSku;
                    data['relationships[' + i + '].ParticipationPercentage'] = item.ParticipationPercentage.toString().replace(".", ","); // parseFloat(item.ParticipationPercentage);
                    data['relationships[' + i + '].OfertType'] = ''; // item.OfertType;
                    data['relationships[' + i + '].ExternalCode'] = ''; // item.ExternalCode;
                    data['relationships[' + i + '].MaterialID'] = item.MaterialID;
                    $.each(item.Replacements, function (j, replace) {
                        data['relationships[' + i + '].Replacements[' + j + '].ParentMaterialID'] = replace.ParentMaterialID;
                        data['relationships[' + i + '].Replacements[' + j + '].MaterialID'] = replace.MaterialID;
                        data['relationships[' + i + '].Replacements[' + j + '].Name'] = replace.Name;
                        data['relationships[' + i + '].Replacements[' + j + '].SKU'] = replace.SKU;
                        data['relationships[' + i + '].Replacements[' + j + '].Priority'] = j + 1;
                    });
                    var Periods = '';
                    $.each(item.ProductRelationsPerPeriod, function (k, prpp) {
                        Periods += prpp.PeriodID + ',';
                        //data['relationships[' + i + '].ProductRelationsPerPeriod[' + k + '].ParentProductID'] = prpp.ParentProductID;
                        data['relationships[' + i + '].ProductRelationsPerPeriod[' + k + '].MaterialID'] = prpp.MaterialID; // prpp.ParentProductID;
                        data['relationships[' + i + '].ProductRelationsPerPeriod[' + k + '].OfferType'] = prpp.OfferType;
                        data['relationships[' + i + '].ProductRelationsPerPeriod[' + k + '].ExternalCode'] = prpp.ExternalCode;
                        data['relationships[' + i + '].ProductRelationsPerPeriod[' + k + '].PeriodID'] = prpp.PeriodID;
                    });
                    PeriodList.push(Periods.substring(0, Periods.length - 1));
                });
                for (var i = 0; i < PeriodList.length; i++) {
                    if (PeriodList[i] == '') {
                        AreThereProductRelationsPerPeriodForAll = false;
                        break;
                    }
                }

                if (!AreThereProductRelationsPerPeriodForAll) {
                    showMessage("Please add at least one Offer Type and External Code.", true);
                    return;
                }
                var found = true;
                var prevFound = true;
                var NotFound = false;

                $('#ddlPeriod option').each(function () {
                    var PeriodID = $(this).val();
                    $.each(PeriodList, function (ind, period) {
                        found = period.contains(PeriodID);
                        if (ind > 0 && found != prevFound) { NotFound = true; return; }
                        prevFound = found;
                    });
                });

                if (NotFound) {
                    showMessage("Please add the same period for all relations (Offer Type and External Code)", true);
                    return;
                }

                var t = $(this);
                showLoading(t);

                var strURL = '<%= ResolveUrl(string.Format("~/Products/Products/SaveKit/{0}/{1}", Model.Product.ProductBaseID, Model.ProductID)) %>';
                $.ajax({
                    type: 'POST',
                    url: strURL,
                    data: JSON.stringify(data),
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    success: function (response) {
                        hideLoading(t);
                        if (response.result) {
                            showMessage('Kit saved!', false);
                        } else {
                            showMessage(response.message, true);
                        }
                    }
                });
            });

            //Dynamic Kits
            $('#dynamicKit input.ruleType').live('click', function () {
                var container = $('#' + $(this).attr('id') + 'Container');
                if (!container.is(':visible')) {
                    container.parent().find('div.' + container.attr('class')).fadeOut(function () {
                        container.fadeIn();
                    });
                }
            });

            $('#sProduct').change(function () {
                var pId = $(this).val();
                window.location = '<%= ResolveUrl(string.Format("~/Products/Products/Relations/{0}/", Model.Product.ProductBaseID)) %>' + pId;
            });

            $('#relations').change(function () {
                var data = {};
                var dataVal = $('#relations option:selected').val().split(',');
                var indexParent = $("#relations option:selected").attr("index");
                var totalRelationsSelected = $("#relations option:selected").length;

                $('#replacements').html('');
                if (totalRelationsSelected > 1) {
                    return false;
                }
                var index = $("#relations option:selected:eq(0)").attr("index");

                //dsRelationship = RelationModelMaterials.filter(function (item) { return (item.MaterialID == dataVal[1]); });
                $.each(RelationModelMaterials[index].Replacements, function (i, v) {
                    $('#replacements').append($('<option>',
                    {
                        value: RelationModelMaterials[index].MaterialID + ',' + v.MaterialID,
                        text: (v.SKU == undefined ? '' : (v.SKU + ' - ')) + v.Name
                    }).attr("index", i)
                      .attr("indexParent", indexParent)
                    );

                });

                fillProductRelationsPerPeriod(RelationModelMaterials[index], indexParent);
            });

            function validateReplacements(MaterialID, ParentMaterialID) {
                var data = {};
                var dataVal = $('#relations option:selected').val().split(',');
                var indexParent = $("#relations option:selected").attr("index");
                var totalRelationsSelected = $("#relations option:selected").length;
                var index = $("#relations option:selected:eq(0)").attr("index");
                var result = true;
                $.each(RelationModelMaterials[index].Replacements, function (i, v) {
                    if (parseInt(v.MaterialID) == parseInt(MaterialID) && parseInt(v.ParentMaterialID) == parseInt(ParentMaterialID)) {
                        result = false;
                        return false;
                    }
                });
                return result;
            }
            $("#btnSaveReplacements").hide();
            GetMaterials();
            $('#relationshipType').change();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
            <%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.Product.ProductBaseID, Model.ProductID)) %>">
                <%= Model.Product.Name %></a> >
    <%= Html.Term("Relations") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ProductRelations", "Product Relations") %></h2>
        <%
            if (Model.Product.IsVariantTemplate || Model.Product.IsVariant())
            {
        %>
        <%=Html.Term("Product") + ":"%>
        <%=Html.DropDownVariantProducts(htmlAttributes: new { id = "sProduct" }, selectedProductID: Model.ProductID, productBase: Model.Product.ProductBase, includeVariantTemplate: false) %>
        <%
            } 
        %>
    </div>
    <table width="100%" cellspacing="0" class="SectionTable">
        <tr>
            <td style="width: 30%;">
                <div class="pad5 brdrAll inner">
                    <h3>
                        <%= Html.Term("SelectAMaterial", "Select a Material")%>:</h3>
                    <p>
                        <input type="text" id="txtProductRelationshipSearch" style="width: 200px;" />
                        <input type="hidden" id="ParticipacionPorcentaje" data-value="@ViewBag.ParticipacionPorcentaje" />
                        <%= Html.Term("Quantity", "Qty") %>:</h3>
                        <input type="text" id="qtyOfProduct" style="width: 50px;" disabled="disabled" class="justNumbers" />
                    </p>
                    <p>
                        <table>
                            <tr>
                                <td>
                                    <%= Html.Term("RelationshipType", "Relationship type") %>:<br />
                                    <select id="relationshipType">
                                        <% 
                                            foreach (var relationType in Model.RelationTypes)
                                            { 
                                        %>
                                        <option value="<%= relationType.ProductRelationTypeID %>">
                                            <%= relationType.GetTerm()%></option>
                                        <% 
                                            }
                                        %>
                                    </select>
                                </td>
                                <%--<td style="width:10px">&nbsp</td>
                                <td>
                                    <%= Html.Term("OfertType", "Ofert Type")%>:<br />
                                    <input type="text" id="txtOfertType" style="width: 72px;" class="justNumbers" />
                                </td>
                                <td style="width:10px">&nbsp</td>
                                <td>
                                    <%= Html.Term("ExternalCode", "External Code")%>:<br />
                                    <input type="text" id="txtExternalCode" style="width: 72px;" class="justNumbers" />
                                </td>--%>
                            </tr>
                        </table>
                    </p>
                    <p>
                        <table id="ParticipationItems">
                            <tr>
                                <td>
                                    <%= Html.Term("Participation", "% Participation")%>:<br />
                                    <input type="text" id="txtParticipationPercent" style="width: 92px;" class="numeric" />
                                </td>
                                <td style="width: 10px">
                                    &nbsp
                                </td>
                                <td>
                                    <%= Html.Term("TotalParticipation", "Total %")%>:<br />
                                    <input type="text" id="txtTotalParticipation" style="width: 92px;" disabled="disabled"
                                        value="0.00" class="justNumbers" />
                                </td>
                            </tr>
                        </table>
                    </p>
                </div>
            </td>
            <td style="width: 24px; vertical-align: middle;">
                <p class="pad10">
                    <a id="btnAddRelation" title="<%= Html.Term("AddToRelationships", "Add to relationships") %>"
                        href="javascript:void(0);" class="moveArrow"><span class="UI-icon icon-ArrowNext">
                        </span></a>
                </p>
            </td>
            <td style="width: 70%;">
                <h3>
                    <%= Html.Term("CurrentRelationshipsForThisProduct", "Current relationships for this product") %>:</h3>
                <select id="relations" size="7" multiple="multiple" class="RelatedItems" style="width: 100%;">
                    <%
                        //Developed by WCS - CSTI
                        int count = 0;
                        double iPerc, iParticipation;
                        iParticipation = 0;
                        bool isFinalOpt;
                        isFinalOpt = false;

                        System.Data.DataTable kitItemValuations = Product.GetKitItemValuationsByParent(Model.Product.SKU); //Developed by WCS - CSTI
                        count = Model.Product.ChildProductRelations.Count;
                        for (int i = 0; i < count; i++)
                        {
                            var productRelation = Model.Product.ChildProductRelations[i];
                            ProductRelation productRelationNext = new ProductRelation();
                            if (i + 1 < count) productRelationNext = Model.Product.ChildProductRelations[i + 1];

                            //Developed by WCS - CSTI

                            if (productRelation.ChildProductID != null && productRelation.ChildProductID != 0)
                            {
                                var product = Product.Load(productRelation.ChildProductID);
                                var quantity = 0;
                                //Developed by WCS - CSTI
                                foreach (var item in Model.Product.ChildProductRelations)
                                {
                                    if (item.Product.SKU.Equals(product.SKU)) quantity++;
                                }
                                //Developed by WCS - CSTI
                                string filterExpression = string.Format("ChildSku = '{0}'", product.SKU);
                                //System.Data.DataRow[] result = kitItemValuations.Select(string.Format("ChildSku = {0}", product.SKU));
                                System.Data.DataRow[] result = kitItemValuations.Select(filterExpression);
                                //Developed by WCS - CSTI
                                //double participation = kitItemValuations.Rows.Count > 0 ? Convert.ToDouble(result[0][3]) : 1;
                                double participation = result.Count() > 0 ? Convert.ToDouble(result[0][3]) : 0;

                                iPerc = System.Math.Round((participation * 100) / quantity, 2);
                                if (iPerc * quantity != participation * 100) isFinalOpt = true;

                                //Developed by WCS - CSTI
                                if (productRelationNext.Product != null && isFinalOpt && productRelation.Product.SKU == productRelationNext.Product.SKU) iParticipation = System.Math.Round((participation / quantity) * 100, 2);
                                if ((productRelationNext.Product != null && isFinalOpt && productRelation.Product.SKU != productRelationNext.Product.SKU) || (productRelationNext.Product == null && isFinalOpt)) iParticipation = System.Math.Round((participation * 100 - ((iPerc) * (quantity - 1))), 2);
                                else iParticipation = System.Math.Round((participation / quantity) * 100, 2);     
                                                        
                    %>
                    <option value="<%= productRelation.ProductRelationsTypeID + "," + product.ProductID %>">
                        <%= SmallCollectionCache.Instance.ProductRelationsTypes.GetById(productRelation.ProductRelationsTypeID).GetTerm() + ": " + product.SKU + " - " + product.Translations.Name() + " - " + iParticipation + "%"%>
                    </option>
                    <% 
                            }
                        }
                    %>
                </select>
                <hr />
                <p>
                    <a id="btnRemoveRelation" class="textlink Remove UI-icon icon-remove Related" href="javascript:void(0);">
                        <%= Html.Term("RemoveSelectedFromThisList", "Remove selected from this list") %></a>
                    | <a id="btnSaveKit" href="javascript:void(0);" class="Button BigBlue">
                        <%= Html.Term("SaveKit", "Save Kit")%></a>
                </p>
            </td>
        </tr>
        <tr>
            <td style="width: 30%;">
                <div class="pad5 brdrAll inner">
                    <h3>
                        <%= Html.Term("SelectOfferTypeAndExternalCode", "Select Offer Type and External Code")%>:</h3>
                    <table>
                        <tr>
                            <td>
                                <span>
                                    <%=Html.Term("Period","Period")%>: </span>
                                <br />
                                <select id="ddlPeriod">
                                    <%
                                        Dictionary<int, bool> PeriodList = Periods.GetNextPeriodsByAccountType(6, 5, 0, true);
                                        foreach (KeyValuePair<int, bool> kvp in PeriodList)
                                        {
                                            if (kvp.Value)
                                            {
                                    %>
                                    <option value="<%= kvp.Key %>" selected="selected">
                                        <%= kvp.Key%></option>
                                    <%
                                            }
                                            else
                                            { 
                                    %>
                                    <option value="<%= kvp.Key %>">
                                        <%= kvp.Key%></option>
                                    <%
                                            }
                                        } 
                                    %>
                                </select>
                            </td>
                            <td style="width: 10px">
                                &nbsp
                            </td>
                            <td>
                                <%= Html.Term("OfertType", "Ofert Type")%>:<br />
                                <input type="text" id="txtOfertType" style="width: 72px;" class="justNumbers" />
                            </td>
                            <td style="width: 10px">
                                &nbsp
                            </td>
                            <td>
                                <%= Html.Term("ExternalCode", "External Code")%>:<br />
                                <input type="text" id="txtExternalCode" style="width: 72px;" class="justNumbers" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="width: 24px; vertical-align: middle;">
                <p class="pad10">
                    <a id="btnAddProductRelationsPerPeriod" title="<%= Html.Term("AddToRelationships", "Add to relationships") %>"
                        href="javascript:void(0);" class="moveArrow"><span class="UI-icon icon-ArrowNext">
                        </span></a>
                </p>
            </td>
            <td style="width: 70%;">
                <h3>
                    <%= Html.Term("CurrentOfferTypeExternalCodeForThisMaterial", "Current Offer Type and External Code for this material")%>:</h3>
                <select id="ProductRelationsPerPeriod" size="5" multiple="multiple" class="RelatedItems"
                    style="width: 100%;">
                </select>
                <hr />
                <p>
                    <a id="btnRemoveProductRelationsPerPeriod" class="textlink Remove UI-icon icon-remove Related"
                        href="javascript:void(0);">
                        <%= Html.Term("RemoveSelectedFromThisList", "Remove selected from this list") %></a>
                </p>
            </td>
        </tr>
        <tr>
            <td style="width: 30%;">
                <div class="pad5 brdrAll inner">
                    <h3>
                        <%= Html.Term("SelectAReplacement", "Select a Replacements")%>:</h3>
                    <p>
                        <input type="text" id="txtMaterialsReplacementSearch" style="width: 200px;" />
                    </p>
                </div>
            </td>
            <td style="width: 24px; vertical-align: middle;">
                <p class="pad10">
                    <a id="btnAddReplacement" title="<%= Html.Term("AddToRelationships", "Add to relationships") %>"
                        href="javascript:void(0);" class="moveArrow"><span class="UI-icon icon-ArrowNext">
                        </span></a>
                </p>
            </td>
            <td style="width: 70%;">
                <h3>
                    <%= Html.Term("CurrentReplacementsForThisMaterial", "Current replacements for this material")%>:</h3>
                <select id="replacements" size="3" multiple="multiple" class="RelatedItems" style="width: 100%;">
                </select>
                <hr />
                <p>
                    <a id="btnRemoveReplacement" class="textlink Remove UI-icon icon-remove Related"
                        href="javascript:void(0);">
                        <%= Html.Term("RemoveSelectedFromThisList", "Remove selected from this list") %></a>
                    <a id="btnSaveReplacements" href="javascript:void(0);">
                        <%= Html.Term("SaveReplacements", "Save Replacements")%></a>
                </p>
            </td>
        </tr>
        <tr>
            <% Html.RenderPartial("DynamicKitGroup", Model); %>
        </tr>
    </table>
</asp:Content>
