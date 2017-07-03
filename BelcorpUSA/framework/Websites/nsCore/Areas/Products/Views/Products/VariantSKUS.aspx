<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>
    <%@ Import Namespace="nsCore.Areas.Products.Models" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
            GetGridData();
            $('.propertyValue').live('change', function () {
                var selectBox = $(this);
                var selectedOption = selectBox.find('option:selected');
                var imageSrc = selectedOption.attr("rel");
                selectBox.closest(".property").find(".varAttrThumb>img").attr("src", imageSrc);
            });

            $('.Add').live('click', function () {
                var t = $(this);
                var data = {
                    productBaseId: '<%=Model.ProductBaseID %>'
                };

                showLoading(t);

                $.post('<%= ResolveUrl(string.Format("~/Products/Products/RenderVariantSKU/{0}/{1}", Model.ProductBaseID, + Model.ProductID)) %>', data, function (response) {
                    hideLoading(t);
                    if (response.result)
                        $('.ProductGrid').append(response.variantSKUHTML);
                    else
                        showMessage(response.message);
                });
            });

            $('.deleteButton').click(function () {
                if (confirm('<%=Html.Term("AreYouSureYouWantToDeleteTheSelectedItems", "Are you sure you want to delete the selected items?") %>')) {
                    var button = $(".BigBlue");
                    if (button.length > 0)
                        showLoading(button);
                    $.post('<%= ResolveUrl(string.Format("~/Products/Products/DeleteVariantProduct/{0}/{1}", Model.ProductBaseID, Model.ProductID))%>', buildSelectedItems(), handleResponse);
                }
            });

            $('#.ProductGrid input.checkAll').click(function () {
                $('.ProductGrid input[type="checkbox"]').not(':disabled').attr('checked', $(this).is(':checked'));
            });

            $('.deactivateButton').click(function ()
            { changeActiveStatus(false); });
            $('.activateButton').click(function () { changeActiveStatus(true); });

            buildSelectedItems = function () {
                var data = {};
                $('.ProductGrid input[type="checkbox"]:checked:not(.checkAll)').each(function (i) {
                    data['items[' + i + ']'] = $(this).val();
                });
                return data;
            },

            handleResponse = function (response) {
                if (response.result) {
                    GetGridData();
                    var button = $(".BigBlue");
                    if (button.length > 0)
                        hideLoading(button);
                } else {
                    showMessage(response.message, true);
                }
            },

		    changeActiveStatus = function (active) {
		        var data = buildSelectedItems();
		        data.active = active;
		        $.post('<%= ResolveUrl(string.Format("~/Products/Products/ChangeVariantProductStatus/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, handleResponse);
		    };

            $('.btnSaveSKUs').live('click', function () {
                
                var skuIsSet = false;
                var propertyIsSet = false;
                var hasDuplicate = false;               

                var t = $(this);
                var btnInventory = $("#btnInventory");
                var data = {
                    productBaseId: '<%=Model.ProductBaseID %>'
                }

                showLoading(t);
                showLoading(btnInventory);

                $('.variantName').clearError();
                $('.propertyValue').clearError();
                $('.variantSapCode').clearError();
                $('.variantOffertType').clearError();
                $('.variantExternalCode').clearError();

                $('.varSKUline').filter(function () { return $(this).find('.variantSKU').is(':disabled') == false && $(this).find('.variantSKU').val() != ""; }).each(function (i) {
                    

                    if ($('.variantName', this).val() == "") {
                        $('.variantName', this).showError("");
                        propertyIsSet =true;
                    }
                    if ($('.propertyValue', this).val() == 0) {
                        $('.propertyValue', this).showError("");
                        propertyIsSet = true;
                    }                    
                    if ($('.variantSapCode', this).val() == "") {
                        $('.variantSapCode', this).showError("");
                        propertyIsSet = true;
                    }
                    if ($('.variantOffertType', this).val().length != 3) {
                        $('.variantOffertType', this).showError("");
                        propertyIsSet = true;
                    }
                    if ($('.variantExternalCode', this).val().length != 5) {
                        $('.variantExternalCode', this).showError("");
                        propertyIsSet = true;
                    }

                 });


                if (propertyIsSet) {
                
                    hideLoading(t);
                    hideLoading(btnInventory);
                    showMessage('<%=Html.Term("PleaseEnterTheSKUAndSetAtLeastOneProperty", "Please enter the sku and set at least one property")%>', true);
                    return false;
                }

                $('.varSKUline').filter(function () { return $(this).find('.variantSKU').is(':disabled') == false && $(this).find('.variantSKU').val() != ""; }).each(function (i) {
                    skuIsSet = true;
                    data['variantProductModels[' + i + '].SKU'] = $('.variantSKU', this).val();
                    data['variantProductModels[' + i + '].CodigoSap'] = $('.Filter', this).val();
                    data['variantProductModels[' + i + '].OffertType'] = $('.variantOffertType', this).val();
                    data['variantProductModels[' + i + '].ExternalCode'] = $('.variantExternalCode', this).val();
                    data['variantProductModels[' + i + '].PropertyValueID'] = $('.propertyValue', this).val();

                    data['variantProductModels[' + i + '].Name'] = $('.variantName', this).val();

                    var currentRow = $(this);
                    var rowIndex = currentRow.prevAll().length;
                    data['variantProductModels[' + i + '].ProductId'] = currentRow.find('.productId').val();
                    var productId = currentRow.find('.productId').val();
                    var typeArr = new Array();
                    var valueArr = new Array();


                    currentRow.find('.property').each(function (j) {
                        var currentCell = $(this);
                        var productPropertyType = currentCell.find('.productPropertyTypeId');
                        var propertyValue = currentCell.find('.propertyValue');
                        var type = productPropertyType.val();
                        var value = propertyValue.val();
                        if (value != 0) {
                            propertyIsSet = true;
                        }

                        data['variantProductModels[' + i + '].Properties[' + j + '].ProductPropertyID'] = currentCell.find('.productPropertyId').val();
                        data['variantProductModels[' + i + '].Properties[' + j + '].ProductPropertyTypeID'] = type;
                        if (propertyValue.is('input')) {
                            data['variantProductModels[' + i + '].Properties[' + j + '].PropertyValue'] = value;
                            propertyIsSet = (value && !(value == ''));
                        }
                        else {
                            data['variantProductModels[' + i + '].Properties[' + j + '].ProductPropertyValueID'] = value;
                        }
                        typeArr[j] = type;
                        valueArr[j] = value;

                    });

                    if (!isUniqueRow(typeArr, valueArr, currentRow)) {
                        hasDuplicate = true;
                        return false;
                    }
                });

                if (hasDuplicate) {
                    hideLoading(t);
                    hideLoading(btnInventory);
                    showMessage('<%=Html.Term("PropertyCombinationsMustBeUnique", "Property combinations must be unique") %>', true);
                    $('.variantSKU', this).focus();
                    return false;
                }

                $.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveVariantProducts/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
                    if (response.result) {
                        showMessage(response.message || '<%=Html.Term("VariantProductsSavedSuccessfully", "Variant products saved successfully") %>!', !response.result);
                        window.location.reload(true);
                    }
                    else {

                        if (response.lisTValidacionRetorno.length != 0) {
                            showMessage('<%=Html.Term("SkuCombinationsMustBeUnique", "Sku combinations must be unique") %>', true);
                        }
                    }
                })
                .always(function () {
                    hideLoading(t);
                    hideLoading(btnInventory);
                });

            });

        });

        function isUniqueRow(searchTypeArr, searchValueArr, searchRow) {
          
            var isUnique = true;
            var arrLength = searchTypeArr.length;

            searchRow.nextAll().filter(function () { return $(this).find('.variantSKU').is(':disabled') == false && $(this).find('.variantSKU').val() != ""; }).each(function (i) {
                var currentRow = $(this);
                var typeArr = new Array();
                var valueArr = new Array();
                currentRow.find('.property').each(function (j) {
                    var currentCell = $(this);
                    var productPropertyType = currentCell.find('.productPropertyTypeId')
                    var propertyValue = currentCell.find('.propertyValue');
                    var type = productPropertyType.val();
                    var value = propertyValue.val();
                    typeArr[j] = type;
                    valueArr[j] = value;
                });
                var matchCount = 0;
                for (var x = 0; x < arrLength; x++) {
                    if (searchValueArr[x] == valueArr[x]) {
                        matchCount++;
                    } else {
                        break;
                    }
                }
                if (matchCount == arrLength) {
                    isUnique = false;
                }

            });
            return isUnique;
        }

        function GetGridData() {
            $.get('<%= ResolveUrl(string.Format("~/Products/Products/GetVariantSKUs/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', function (response) {
                if (response) {
                    $('.varSKUlines').html(response.page);
                } else {
                    showMessage(response.message);
                }
            });
            
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
			<%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/VariantSKUS/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
				<%= Model.Translations.Name() %></a> >
	<%= Html.Term("Variants") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    <div class="SectionHeader">
	    <h2><%= Html.Term("ProductVariants", "Product Variants") %></h2>
	    <a href="<%= ResolveUrl(string.Format("~/Products/Products/Variants/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>"><%= Html.Term("SetUpNewVariants", "Set-Up New Variants") %></a>
	    | <%= Html.Term("ManageVariantSKUS", "Manage Variant SKUS") %>
    </div>

<%
var productBase = Model.ProductBase;
var productType = ProductType.LoadFull(productBase.ProductTypeID);
var baseProperties = productBase.ProductBaseProperties.Select(pbp => pbp.ProductPropertyType).Where(ppt => ppt.IsProductVariantProperty);// && ppt.ProductPropertyValues.Count > 0);
var typeProperties = productType.ProductPropertyTypes.Where(ppt => !baseProperties.Any(bp => bp.ProductPropertyTypeID == ppt.ProductPropertyTypeID) && ppt.IsProductVariantProperty);
var applicableProperties = baseProperties.Union(typeProperties);
%>

    <!-- utitlity for adding more lines to the SKU generator matrix -->
    <div class="UI-lightBg brdrAll pad10 mb10 FilterSet variantGroupTools">
        <h4><%= Html.Term("GenerateAvailableSKUsForProductVariations", "Generate Available SKUs for Product Variations")%></h4>
        	
      <%--  <div>
            <a href="javascript:void(0);" class="DTL Add" id=""><span><%= Html.Term("AddMoreLines", "Add More Lines")%></span></a>
        </div>--%>
            
            
    </div>   
    <!--/ end utility -->
     <!-- Variant Product Options -->
    <div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility">
            <a class="deleteButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected">
            </span><span><%=Html.Term("DeleteSelected", "Delete Selected") %></span></a>
            <a class="deactivateButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-x icon-deactive">
            </span><span><%=Html.Term("DeactivateSelected", "Deactivate Selected") %></span></a>
            <a class="activateButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-plus icon-activate">
            </span><span><%=Html.Term("ActivateSelected", "Activate Selected") %></span></a>
            <span class="ClearAll"></span>
    </div>
    <!-- Variant SKU Matrix -->
    <table class="DataGrid ProductGrid SkuMatrix" width="100%"> 
    
        <thead>
            <tr class="GridColHead">
                <th><input type="checkbox" class="checkAll" title="<%=Html.Term("SelectAll", "Select All...") %>" /></th>
                <th class="varSKU"><%=Html.Term("SKU")%></th>
                <th class="varName"><%=Html.Term("Name")%></th>
                <th class="varStatus"><%=Html.Term("Status")%></th>
                 <th class="varStatus"><%=Html.Term("SkinType","Skin Type")%></th>   
                 <th class="varStatus"><%=Html.Term("SapCode","Sap Code")%></th>     
                 <th class="varStatus"><%=Html.Term("OffertType", "Offert Type")%></th> 
                 <th class="varStatus"><%=Html.Term("ExternalCode", "External Code")%></th> 
            </tr>
        </thead>
        <tbody class="varSKUlines">
        </tbody>
    </table>
    <!--/ end matrix -->     
    <hr />
    <p class="mt10">
        <a href="javascript:void(0);" class="Button BigBlue btnSaveSKUs"><span><%=Html.Term("SaveSKUS", "Save SKUs")%></span></a>
        <%--<%=Html.Term("InventoryNotification", "Inventory is copied from the master product to each variant sku, please double check and adjust the inventory level for each new SKU") %> <a href="<%= ResolveUrl("~/Products/Products/Inventory/" + Model.ProductBaseID + "/" + Model.ProductID ) %>" id="btnInventory"><span><%=Html.Term("JumptoInventory", "Jump to inventory")%></span></a>--%>
        <%=Html.Term("InventoryNotification", "Inventory is copied from the master product to each variant sku, please double check and adjust the inventory level for each new SKU") %> <a href="<%= ResolveUrl("~/Products/Warehouses") %>" id="btnInventory"><span><%=Html.Term("JumptoInventory", "Jump to inventory")%></span></a>
    </p>
</asp:Content>
