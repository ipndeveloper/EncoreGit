<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
    <%IEnumerable<Category> categoryTrees = ViewData["Categories"] as IEnumerable<Category>; %>
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">
		$(function () {
			function changeProperties() {
				$.get('<%= ResolveUrl("~/Products/Products/GetProperties") %>', { productTypeId: $('#sProductType').val() }, function (response) {
					if (response.result === undefined || response.result) {
						$('#propertyList').html(response)
							.find('.DatePicker').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100' }).end()
							.find('.numeric').numeric();
					} else {
						showMessage(response.message, true);
					}
				});
			}

			changeProperties();

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
//                    return value.replace(/\D/g, "")
//                                 .replace(/([0-9])([0-9]{2})$/, '$1.$2')
//                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, ",");
                });

//            }

        });



			$('#sProductType').change(changeProperties);

			$('#sCurrency').change(function () {
				var cur = this;
				var symbol = cur.options[cur.selectedIndex].text.split(' ')[0];
				$('.currencySymbol').text(symbol);                
			});

			$('#catalogs .catalog').live('click', function () {
				var catalogId = $(this).val();
                var catalogIds = this.name;

				if ($(this).prop('checked')) {
					var timepicker = $('#catalog' + catalogId).fadeIn('fast').find('.TimePicker');
					if (!timepicker.data('timepickr')) {
						timepicker.timepickr({ convention: 12 });
					}
                    //KLC-CSTI APPEND CATALGO  
                    $('#sCatalog option[value=""]').remove();                     
                     $('#sCatalog').append('<option value="' + catalogId +  '">' + catalogIds + '</option>');                                        
                         
                        var div =  $("#divclone").clone();                        
                        div.attr({    
                            id: 'div'+catalogId,
                            'class': 'ocultar'
                            });
                        div.css({
                            display:'none'
                            });                       
                        $("#fiel").append(div);
                        $('#c').hide();                      

                        var selVal = $( "#sCatalog" ).val();
                        $('#div'+selVal).show();  
				} else {
					$('#catalog' + catalogId).fadeOut('fast')
                    //KLC-CSTI Remove Catalog
                     $("#sCatalog").find("option[value=" + catalogId + "]").remove();
                     //Delete Div Prices
                     div=document.getElementById('div'+catalogId);
                     $(div).remove();

                     if ($('#sCatalog option').length === 0) {
                        $('#sCatalog').append('<option value="">Select Catalog</option>');
                        $('#c input[type="text"]').val('');
                        $('#c').show();                          
                     }
                     else{
                         var selVal = $( "#sCatalog" ).val();                                    
                         $('#div'+selVal).show();  
                     }
				}
			});

            //KLC - CSTI
            $('#sCatalog').on('change', function() {               
                var selVal = $(this).val(); 
                if(selVal == 'Seleccionar')
                {
                    $('.ocultar').each(function(){
                        if($(this).css('display') == 'block') $(this).css('display','none');
                    });                    
                }                
                var elElemento=document.getElementById('div'+selVal);
                    elElemento.classList.remove('ocultar');
                    
                if(elElemento.style.display == 'none') 
                {
                    elElemento.style.display = 'block';
                    var elements = document.getElementsByClassName("ocultar");
                    for(var i=0; i<elements.length; i++) elements[i].style.display = 'none';
                            elElemento.classList.add('ocultar');                                            
                } else {
                    elElemento.style.display = 'none';                      
                }
            });                
            //

			$('#categoriesSelector').jqm({ modal: false, trigger: '#btnOpenCategoriesSelector', overlay: 0 });

			$('#txtWeight').numeric();

			<% if(categoryTrees != null && categoryTrees.Any())
			   { %>
			$('#sCategoryTree').val('<%= categoryTrees.First().CategoryID %>').change(function () {
				$.get('<%= ResolveUrl("~/Products/Products/GetCategoryTree") %>', { categoryTreeId: $('#sCategoryTree').val() }, function (response) {
					$('#categoryTreeSelector').html(response);
				});
			});
			<% } %>

			$('.category').live('click', function () {
				var categories = $('#categoryTreeSelector').data('categories');
				if (!categories)
					categories = [];
				if ($(this).prop('checked')) {
					categories[$(this).val()] = $('#category' + $(this).val()).text();
				} else {
					delete categories[$(this).val()];
				}

				$('#categoriesSelected').text(getValues(categories).join(', '));

				$('#categoryTreeSelector').data('categories', categories);
			});

			$('#btnSave').click(function () {         
                if ($('#newProduct').checkRequiredFields()) {                                       
                    if (($('#sCatalog option').length === 1) && ($("#sCatalog" ).val() === "") ){
                       showMessage('<%= Html.JavascriptTerm("msgCatalog", "Please, but you must select a catalog.") %>', true);
                       return false;                   
                    }                 

                    var elem="";
                    $('.ocultar').find(':input[type="text"][class="numeric price required"]').each(function() {
                      elem= this.value; 
                    });
                    if(elem == ""){
                         showMessage('<%= Html.Term("PriceRequireds", "Prices for catalogs can not be null.") %>', true);
                        return false;
                    }

					var t = $(this);
					showLoading(t);                   
                    
					var data = {
						productTypeId: $('#sProductType').val(),
						taxCategoryId: $('#sTaxCategory').val(),
						sku: $('#txtSKU').val(),
						name: $('#txtName').val(),
						backOrderBehaviorId: $('#backOrderBehavior').val(),
						weight: $('#txtWeight').val(),
						chargeShipping: $('#chkChargeShipping').prop('checked'),
						chargeTax: $('#chkChargeTax').prop('checked'),
						chargeTaxOnShipping: $('#chkChargeTaxOnShipping').prop('checked'),
						isShippable: $('#chkIsShippable').prop('checked'),
						currencyId: $('#sCurrency').val()
						//					shortDescription: $('#txtShortDescription').val(),
						//					longDescription: $('#txtLongDescription').val()
					};

					var categories = $('#categoryTreeSelector').data('categories'), i = 0, categoryId;
					if (categories) {
						for (categoryId in categories) {
							if (typeof (categories[categoryId]) != 'function') {
								data['categories[' + i + ']'] = categoryId;
								++i;
							}
						}
					}

					var dateRegex = /\d+\/\d+\/\d+/i, timeRegex = /\d+\:\d+\s(am|pm)/i;
					$('#catalogs .catalog:checked').each(function (i) {
						var catalogId = $(this).val(),
						catalogContainer = $('#catalog' + catalogId),
						startDate = catalogContainer.find('.StartDate').val(),
						startTime = catalogContainer.find('.StartTime').val(),
						endDate = catalogContainer.find('.EndDate').val(),
						endTime = catalogContainer.find('.EndTime').val();
						data['catalogs[' + i + '].CatalogID'] = catalogId;
						data['catalogs[' + i + '].StartDate'] = (dateRegex.test(startDate) ? startDate : '') + (timeRegex.test(startTime) ? ' ' + startTime : '');
						data['catalogs[' + i + '].EndDate'] = (dateRegex.test(endDate) ? endDate : '') + (timeRegex.test(endTime) ? ' ' + endTime : '');
					});

//					$('#warehouses .warehouse:checked').each(function (i) {
//						var warehouseId = $(this).val();
//						data['warehouses[' + i + '].WarehouseID'] = warehouseId;
//						data['warehouses[' + i + '].QuantityOnHand'] = $('#warehouseQuantity' + warehouseId).val();
//						data['warehouses[' + i + '].QuantityBuffer'] = $('#warehouseBuffer' + warehouseId).val();
//					});

                    /* var array=[];
		             var CatalogID="";
		             var ProductPriceTypeID="";
		             var Price="0";
                     var CurrencyID="1";
                     var prices=[];
                     var ProductID="1";*/
                     

		                for (var i = 0; i<$('.ocultar').length; i++) {
                     
                            var pric= $('.ocultar')[0].children[0].children[0].children.length -1;                            		                
				             for (j=1;j <$('.ocultar')[0].children[0].children[0].children.length;j++){
			                    /*CatalogID=$('.ocultar')[i].id;
                                ProductID=ProductID;
				                ProductPriceTypeID=$('.ocultar')[i].children[0].children[0].children[j].cells[1].children[1].id;
			                    Price=$('.ocultar')[i].children[0].children[0].children[j].cells[1].children[1].value;
			                    array={"productId":ProductID,"currencyId":1,"catalogID":CatalogID,"ProductPriceTypeID":ProductPriceTypeID,"Price":Price};
				                prices.push(array);   */
                                 data['prices[' + (j - 1 + i*pric) + '].productId']= parseInt("1");
                                 data['prices[' + (j - 1 + i*pric) + '].currencyId']= parseInt($('#sCurrency').val());  
                                 data['prices[' + (j - 1 + i*pric) + '].catalogID']= parseInt($('.ocultar')[i].id.replace(/\D/g, '')); 
                                 data['prices[' + (j - 1 + i*pric) + '].ProductPriceTypeID']= parseInt($('.ocultar')[i].children[0].children[0].children[j].cells[1].children[1].id);
                                 data['prices[' + (j - 1 + i*pric) + '].Price']= $('.ocultar')[i].children[0].children[0].children[j].cells[1].children[1].value;                                    
                             }
		                };                    
                   
					$('#propertyList .propertyValue').filter(function () { return !!$(this).val(); }).each(function (i) {
						data['properties[' + i + '].ProductPropertyTypeID'] = $(this).attr('id').replace(/\D/g, '');
						if ($(this).is('select')) {
							data['properties[' + i + '].ProductPropertyValueID'] = $(this).val();
						} else {
							data['properties[' + i + '].PropertyValue'] = $(this).val();
						}
					});

					$.post('<%= ResolveUrl("~/Products/Products/Save") %>',data, function (response) {
						if (response.result) {                            
							window.location = '<%= ResolveUrl("~/Products/Products/Overview/") %>' + response.productId;
						}
						else {
							showMessage(response.message, true);
						}
					})
					.always(function() {
						hideLoading(t);                    
					});
				}
			});
		});
    </script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> >
    <%= Html.Term("NewProduct", "New Product") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <%var categoryTrees = ViewData["Categories"] as IEnumerable<Category>; %>
    <!-- Section Header -->
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("AddaNewProduct", "Add a New Product") %></h2>
    </div>
    <table id="newProduct" class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("SKU") %>:
            </td>
            <td>
                <input id="txtSKU" type="text" class="required" name="<%: Html.Term("SKUisrequired","SKU is required.") %>" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Name") %>:
            </td>
            <td>
                <input id="txtName" type="text" class="required" name="<%: Html.Term("ProductNameIsRequired","Product Name is required.") %>"
                    style="width: 400px;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 200px;">
                <%= Html.Term("ProductType", "Product Type") %>:
            </td>
            <td>
                <select id="sProductType">
                    <%foreach (ProductType productType in SmallCollectionCache.Instance.ProductTypes.Where(pt => pt.Active))
                      { %>
                    <option value="<%= productType.ProductTypeID %>">
                        <%= productType.GetTerm() %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 200px;">
                <%= Html.Term("TaxCategory", "Tax Category") %>:
            </td>
            <td>
                <select id="sTaxCategory">
                    <option value="">
                        <%= Html.Term("None") %></option>
                    <%foreach (TaxCategory taxCategory in SmallCollectionCache.Instance.TaxCategories)
                      { %>
                    <option value="<%= taxCategory.TaxCategoryID %>">
                        <%= taxCategory.GetTerm() %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <%--<tr>
			<td class="FLabel">
				Short Description:
			</td>
			<td>
				<textarea id="txtShortDescription" style="width: 500px; height: 50px;"></textarea>
			</td>
		</tr>
		<tr>
			<td class="FLabel">
				Long Description:
			</td>
			<td>
				<textarea id="txtLongDescription" style="width: 500px; height: 300px;"></textarea>
			</td>
		</tr>--%>
        <tr>
            <td class="FLabel">
                <%= Html.Term("BackOrderBehavior", "Back Order Behavior") %>:
            </td>
            <td>
                <select id="backOrderBehavior">
                    <%foreach (var behavior in SmallCollectionCache.Instance.ProductBackOrderBehaviors)
                      { %>
                    <option value="<%= behavior.ProductBackOrderBehaviorID %>" <%= behavior.IsDefault ? "selected=\"selected\"" : "" %>>
                        <%= behavior.GetTerm() %></option>
                    <%} %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Weight") %>:
            </td>
            <td>
                <input id="txtWeight" type="text" class="numeric" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ChargeShipping", "Charge Shipping") %>:
            </td>
            <td>
                <input id="chkChargeShipping" type="checkbox" checked="checked" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ChargeTax", "Charge Tax") %>:
            </td>
            <td>
                <input id="chkChargeTax" type="checkbox" checked="checked" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ChargeTaxOnShipping", "Charge Tax On Shipping") %>:
            </td>
            <td>
                <input id="chkChargeTaxOnShipping" type="checkbox" checked="checked" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("IsShippable", "Is Shippable") %>:
            </td>
            <td>
                <input id="chkIsShippable" type="checkbox" checked="checked" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("ProductCategories", "Product Categories") %>:
            </td>
            <td>
                <div id="categoriesSelected">
                </div>
                <a id="btnOpenCategoriesSelector" href="javascript:void(0);">
                    <%= Html.Term("OpenCategoriesSelector", "Open Categories Selector") %></a>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Catalogs") %>:
            </td>
            <td>
                <p id="catalogs">
                    <%foreach (var catalog in Catalog.LoadAllFullByMarkets(SmallCollectionCache.Instance.Markets.Select(m => m.MarketID)))
                      { %>
                    <input type="checkbox" class="catalog" value="<%= catalog.CatalogID %>" name="<%= catalog.Translations.Name() %>" />
                    <b>
                        <%= catalog.Translations.Name() %></b> <span class="Schedule" id="catalog<%= catalog.CatalogID %>"
                            style="display: none;">
                            <input type="text" class="TextInput DatePicker StartDate" value="Start Date" />
                            <input type="text" class="TimePicker StartTime" value="Start Time" />
                            to
                            <input type="text" class="TextInput DatePicker EndDate" value="End Date" />
                            <input type="text" class="TimePicker EndTime" value="End Time" />
                        </span>
                    <br />
                    <%} %>
                </p>
            </td>
        </tr>
        <%--<tr>
            <td class="FLabel">
                <%= Html.Term("Warehouses") %>:
            </td>
            <td id="warehouses">
                <%foreach (var warehouse in SmallCollectionCache.Instance.Warehouses)
                  { %>
                <input type="checkbox" value="<%= warehouse.WarehouseID %>" class="warehouse" onclick="$(this).prop('checked') && $('#warehouse<%= warehouse.WarehouseID %>').fadeIn('fast') ||  $('#warehouse<%= warehouse.WarehouseID %>').fadeOut('fast');" />
                <b>
                    <%= warehouse.GetTerm() %></b><span class="warehouseProduct" id="warehouse<%= warehouse.WarehouseID %>"
                        style="display: none;">
                        <%= Html.Term("QuantityOnHand", "Quantity on Hand") %>:
                        <input type="text" id="warehouseQuantity<%= warehouse.WarehouseID %>" value="0" class="numeric" />
                        <%= Html.Term("QuantityBuffer", "Quantity Buffer") %>:
                        <input type="text" id="warehouseBuffer<%= warehouse.WarehouseID %>" value="0" class="numeric" />
                    </span>
                <br />
                <%} %>
            </td>
        </tr>--%>
        <tr>
            <td class="FLabel" style="vertical-align: top;">
                <%= Html.Term("Properties") %>:
            </td>
            <td>
                <ul id="propertyList">
                </ul>
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="vertical-align: top;">
                <%= Html.Term("ProductPricing", "Product Pricing") %>:
            </td>
            <td>
                <%= Html.Term("Currency", "Currency") %>:
                <select id="sCurrency">
                    <%  
                        var activeCurrencies = (from country in SmallCollectionCache.Instance.Countries
                                                join currency in SmallCollectionCache.Instance.Currencies on country.CurrencyID equals currency.CurrencyID
                                                where country.Active
                                                select currency).Distinct().ToList();
                        Currency firstCurrency = activeCurrencies.FirstOrDefault();
                        foreach (Currency currency in activeCurrencies)
                        { %>
                    <option value="<%= currency.CurrencyID %>" <%= firstCurrency != null && firstCurrency.CurrencyID == currency.CurrencyID ? "selected=\"selected\"" : "" %>>
                        <%= currency.CurrencySymbol %>
                        <%= currency.Name %>
                    </option>
                    <%} %>
                </select>
                <!--KLC-->
                <%= Html.Term("Catalogs", "Catalogs")%>:
                <select id="sCatalog">
                    <option value="">Seleccionar</option>
                </select>
                <!--FIN -->
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="vertical-align: top;">
                &nbsp;
            </td>
            <td>
                <fieldset id="fiel">
                </fieldset>
                <div id="c">
                    <table id="prices1" class="DataGrid">
                        <tr class="GridColHead">
                            <th style="width: 11.364em; min-width: 11.364em;">
                                <%= Html.Term("PriceType", "Price Type") %>
                            </th>
                            <th style="min-width: 11.364em;">
                                <%= Html.Term("Price", "Price") %>
                            </th>
                        </tr>
                        <%
                            foreach (ProductPriceType priceTypes in SmallCollectionCache.Instance.ProductPriceTypes)
                            {
                                /*bool required = (priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.Retail
                                    || priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.QV
                                    || priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.CV
                                    || priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.Wholesale);  */                          
                                   //bool required=Convert.ToBoolean((priceType.Mandatory));
                                bool required = NetSteps.Data.Entities.Business.Logic.ProductPriceTypeExtensionBusinessLogic.Instance.GetMandatory(priceTypes.ProductPriceTypeID);                   
                        %>
                        <tr>
                            <td>
                                <%= priceTypes.Name%>
                            </td>
                            <td>
                                <span class="currencySymbol">
                                    <%= firstCurrency == null ? "" : firstCurrency.CurrencySymbol %></span>
                                <input type="text" id="<%=priceTypes.ProductPriceTypeID %>" class="numeric price<%: required == true ? " required" : "" %>"
                                   monedaidioma='CultureIPN' name="<%= required ? priceTypes.Name + " " + Html.Term("isrequired","is required") : "" %>" style="width: 6.25em;"
                                    value="" />
                            </td>
                        </tr>
                        <%
                        } 
                        %>
                    </table>
                </div>
                <div id="divclone" style="display:none;">
                    <table id="prices" class="DataGrid">
                        <tr class="GridColHead">
                            <th style="width: 11.364em; min-width: 11.364em;">
                                <%= Html.Term("PriceType", "Price Type") %>
                            </th>
                            <th style="min-width: 11.364em;">
                                <%= Html.Term("Price", "Price") %>
                            </th>
                        </tr>
                        <%
                            foreach (ProductPriceType priceType in SmallCollectionCache.Instance.ProductPriceTypes)
                            {
                                /*bool required = (priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.Retail
                                    || priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.QV
                                    || priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.CV
                                    || priceType.ProductPriceTypeID == (int)NetSteps.Data.Entities.Generated.ConstantsGenerated.ProductPriceType.Wholesale);  */
                                //bool required=Convert.ToBoolean((priceType.Mandatory));
                                bool required = NetSteps.Data.Entities.Business.Logic.ProductPriceTypeExtensionBusinessLogic.Instance.GetMandatory(priceType.ProductPriceTypeID);                   
                        %>
                        <tr>
                            <td>
                                <%= priceType.Name %>
                            </td>
                            <td>
                                <span class="currencySymbol">
                                    <%= firstCurrency == null ? "" : firstCurrency.CurrencySymbol %></span>
                                <input type="text"  id="<%=priceType.ProductPriceTypeID %>" class="numeric price<%= required == true ? " required":"" %>"
                                    name="<%= required ? priceType.Name + "is required." : "" %>" style="width: 6.25em;"
                                    value=""  monedaidioma='CultureIPN'/>
                            </td>
                        </tr>
                        <%
                        } 
                        %>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <table class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 200px;">
                &nbsp;
            </td>
            <td>
                <p>
                    <a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><span>
                        <%= Html.Term("SaveProduct", "Save Product") %></span></a></p>
            </td>
        </tr>
    </table>
    <div id="categoriesSelector" class="jqmWindow LModal" style="min-width: 350px;">
        <div class="mContent">
            <h2>
                <%= Html.Term("CategoriesSelector", "Categories Selector") %></h2>
            <select id="sCategoryTree">
                <% 
                    foreach (var categoryTree in categoryTrees)
                    { %>
                <option value="<%= categoryTree.CategoryID %>">
                    <%= categoryTree.Translations.Name()%></option>
                <% 
                    }
                %>
            </select>
            <div id="categoryTreeSelector" style="height: 400px; overflow: auto; margin-bottom: 1.818em;">
                <%= ViewData["CategoryTree"] %>
            </div>
            <p>
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Close", "Close") %></a>
            </p>
        </div>
    </div>
</asp:Content>
