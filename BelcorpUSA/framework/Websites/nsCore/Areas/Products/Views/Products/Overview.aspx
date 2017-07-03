<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Products.Models.ProductOverviewModel>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
	<script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
	<script type="text/javascript">
	    $(function () {
	        $('.CatalogItemSchedule.DatePicker').datepicker('destroy').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100', onSelect: changeCatalogItemSchedule });
	        $('.CatalogItemSchedule.TimePicker').timepickr({ convention: 12, select: changeCatalogItemSchedule });

	        /*CATALGOS*/
	        $('#sCatalogPricing').change(function () {
	            $('.prc').val('N/A');
	            var t = $(this);
	            showLoading(t);
	            $.get('<%= ResolveUrl(string.Format("~/Products/Products/GetPricesCatalogsOverview/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>',
                { productId: $("#hidProductId").val(), catalogID: $(this).val() }, function (response) {
                    if (response.result) {
                        hideLoading(t);
                        var CurrencySymbol = response.CurrencySymbol;
                        if (response.prices) {
                            for (var i = 0; i < response.prices.length; i++) {
                                $('#priceType' + response.prices[i].priceTypeId).val(CurrencySymbol + ' ' + response.prices[i].price.toFixed(2));
                            }
                        } 
                    }
                });
	        });
	        /**/
	    });

     
		function changeCatalogItemSchedule() {
			var catalogItemId = $(this).attr('id').replace(/\D/g, '');
			$.post('<%= ResolveUrl(string.Format("~/Products/Products/ChangeCatalogItemSchedule/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', { catalogItemId: catalogItemId, startDate: $('#startDate' + catalogItemId).val(), startTime: $('#startTime' + catalogItemId).val(), endDate: $('#endDate' + catalogItemId).val(), endTime: $('#endTime' + catalogItemId).val() }, function (response) {
				if (!response.result)
					showMessage(response.message, true);
           });
		}
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
			<%= Html.Term("BrowseProducts", "Browse Products") %></a> >
	<%= Model.Product.Translations.Name() %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<input type="hidden" id="hidProductId" value="<%=Model.Product.ProductID %>" />
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("ProductOverview", "{0} Overview", Model.Product.Translations.Name()) %></h2>
		<%= Html.Term("Overview")%>
		| <a id="auditHistory" href="<%= ResolveUrl(string.Format("~/Products/Products/AuditHistory/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>">
			<%= Html.Term("AuditHistory", "Audit History") %></a>
	</div>
	<!-- Product CMS Stuff -->
	<table width="100%" cellspacing="0" class="">
		<tr>
			<td style="width: 40%; border-right: 1px solid #c0c0c0; padding: .455em;">
				<h3>
					<%= Html.Term("AvailableIn", "Available In") %>
					<span><a href="<%= ResolveUrl(string.Format("~/Products/Products/Availability/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>">
						<%= Html.Term("Edit", "Edit") %></a></span></h3>
				<table class="DataGrid" width="100%">
					<tr class="GridColHead">
						<th>
							<%= Html.Term("StoreFront", "Store Front") %>
						</th>
						<th>
							<%= Html.Term("Catalog", "Catalog") %>
						</th>
						<th>
							<%= Html.Term("Schedule", "Schedule") %>
						</th>
					</tr>
					<% 
						int count = 0;
						foreach (CatalogItem item in Model.Product.CatalogItems)
						{ 
					%>
					<tr <%= count % 2 == 1 ? "class=\"Alt\"" : "" %>>
						<td>
							<%= string.Join(", ", item.Catalog.StoreFronts.Select(sf => SmallCollectionCache.Instance.StoreFronts.GetById(sf.StoreFrontID).GetTerm())) %>
						</td>
						<td>
							<%= item.Catalog.Translations.Name() %>
						</td>
						<td>
							<p class="FL">
								<input id="startDate<%= item.CatalogItemID %>" type="text" class="TextInput DatePicker CatalogItemSchedule" value="<%= item.StartDate.HasValue ? item.StartDate.Value.ToShortDateString() : "Start Date" %>" /><br />
								<input id="startTime<%= item.CatalogItemID %>" type="text" class="TextInput TimePicker CatalogItemSchedule" value="<%= item.StartDate.HasValue ? item.StartDate.Value.ToShortTimeString() : "Start Time" %>" />
							</p>
							<p class="FL LawyerText" style="margin: 0px .455em; font-style: italic; line-height: 5em;">
								to:</p>
							<p class="FL">
								<input id="endDate<%= item.CatalogItemID %>" type="text" class="TextInput DatePicker CatalogItemSchedule" value="<%= item.EndDate.HasValue ? item.EndDate.Value.ToShortDateString() : "End Date" %>" /><br />
								<input id="endTime<%= item.CatalogItemID %>" type="text" class="TextInput TimePicker CatalogItemSchedule" value="<%= item.EndDate.HasValue ? item.EndDate.Value.ToShortTimeString() : "End Time" %>" />
							</p>
							<span class="Clear"></span>
						</td>
					</tr>
					<% 
							++count;
						} 
					%>
				</table>
				<br />
			<%--   EB-229	<h3>
					<%= Html.Term("WarehouseAvailability", "Warehouse Availability") %>
					<span><a href="<%= ResolveUrl(string.Format("~/Products/Products/Inventory/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>">
						<%= Html.Term("Edit", "Edit") %></a></span></h3>
				<table width="100%" class="DataGrid">
					<tr class="GridColHead">
						<th>
							<%= Html.Term("Warehouse", "Warehouse") %>
						</th>
						<%
							if (Model.Product.IsVariantTemplate)
							{ 
						%>
						<th>
							<%= Html.Term("SKU", "SKU") %>
						</th>
						<%
							}
						%>
						<th>
							<%= Html.Term("QuantityOnHand", "Quantity on Hand") %>
						</th>
						<th>
							<%= Html.Term("QuantityBuffer", "Quantity Buffer") %>
						</th>
						<th>
							<%= Html.Term("QuantityAllocated", "Quantity Allocated")%>
						</th>
						<th>
							<%= Html.Term("Available", "Available") %>
						</th>
						<th>
							<%= Html.Term("Allocated")%>
						</th>
					</tr>
					<% 
						count = 0;
						var products = Model.Product.ProductBase.Products.Where(p => !p.IsVariantTemplate).Select(p => new { ProductId = p.ProductID, Sku = p.SKU });
						foreach (Warehouse warehouse in Warehouse.Repository.LoadAllFull().Where(w => w.Active))
						{
							foreach (var warehouseProduct in warehouse.WarehouseProducts.Where(wp => products.Any(p => p.ProductId == wp.ProductID)))
							{
					%>
					<tr <%= count % 2 == 1 ? "class=\"Alt\"" : "" %>>
						<td>
							<%= warehouse.GetTerm()%>
						</td>
						<%
								if (Model.Product.IsVariantTemplate)
								{
									var sibling = products.FirstOrDefault(p => p.ProductId == warehouseProduct.ProductID);
						%>
						<td>
							<%= sibling == null ? "N/A" : sibling.Sku%>
						</td>
						<%
								}
						%>
						<td>
							<%= warehouseProduct == null ? "N/A" : warehouseProduct.QuantityOnHand.ToString()%>
						</td>
						<td>
							<%= warehouseProduct == null ? "N/A" : warehouseProduct.QuantityBuffer.ToString()%>
						</td>
						<td>
							<%= warehouseProduct == null ? "N/A" : warehouseProduct.QuantityAllocated.ToString()%>
						</td>
						<td>
							<%= warehouseProduct == null ? "N/A" : (warehouseProduct.IsAvailable ? Html.Term("Yes") : Html.Term("No"))%>
						</td>
						<td>
							<%= warehouseProduct == null ? "N/A" : warehouseProduct.QuantityAllocated.ToString()%>
						</td>
					</tr>
					<%
								++count;
							}
						} 
					%>
				</table>
				<br />--%>
              <div class="SectionHeader">
                <!-- KLC -CSTI -->
                <%= Html.Term("Catalog", "Catalog")%>:
                <select id="sCatalogPricing">
                    <% foreach (var items in ViewData["CatalogsID"] as List<PricesPerCatalogsData>)
                            {
                        %>
                            <option value="<%=items.CatalogID %>"><%=items.Name%></option>
                        <%                                       
                            }                   
                    %>
                </select>
               <!--end -->
                </div>
				<% 
					var defaultCurrencyId = CoreContext.CurrentMarket.GetDefaultCurrencyID();
					//Default to US if we can't find a currency
					if (defaultCurrencyId == 0)
						defaultCurrencyId = SmallCollectionCache.Instance.Currencies.First(c => c.CurrencyCode.Equals("USD", StringComparison.InvariantCultureIgnoreCase)).CurrencyID;
					var defaultCurrency = SmallCollectionCache.Instance.Currencies.GetById(defaultCurrencyId); 
				%>
				<h3>
					<%= Html.Term("Available", "Available") %>
					<%= Html.Term("PricingIn", "Pricing in") %>
					<%= defaultCurrency.CurrencyCode %><span><a href="<%= ResolveUrl(string.Format("~/Products/Products/Pricing/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>"><%= Html.Term("Edit", "Edit") %></a></span></h3>
				<%--<table width="100%" class="DataGrid">
					<tr class="GridColHead">
						<th>
							<%= Html.Term("PriceType", "Price Type") %>
						</th>
						<th>
							<%= Html.Term("Price", "Price") %>
						</th>
					</tr>
					<% 
						count = 0;
						foreach (var priceType in SmallCollectionCache.Instance.ProductPriceTypes)
						{ 
					%>
					<tr <%= count % 2 == 1 ? "class=\"Alt\"" : "" %>>
						<td>
							<%= priceType.GetTerm() %>
						</td>
						<td>
							<% 
							var price = Model.Product.Prices.FirstOrDefault(pp => pp.ProductPriceTypeID == priceType.ProductPriceTypeID && pp.CurrencyID == defaultCurrencyId);//usCurrency.CurrencyID);
							if (price == null || price.ProductPriceID == 0)
								Response.Write("N/A");
							else
								Response.Write(price.Price.ToString(defaultCurrency));
							%>
						</td>
					</tr>
					<%
							++count;
						} 
					%>
				</table>--%>

                <table id="prices" width="100%" class="DataGrid">
                    <tr class="GridColHead">
                        <th >
                            <%= Html.Term("PriceType", "Price Type") %>
                        </th>
                        <th align="center" >
                            <%= Html.Term("Price", "Price") %>
                        </th>
                    </tr>
                     <%  var activeCurrencies = (from country in SmallCollectionCache.Instance.Countries
                                        join currency in SmallCollectionCache.Instance.Currencies on country.CurrencyID equals currency.CurrencyID
                                        where country.Active
                                        select currency).Distinct().ToList();
                        Currency firstCurrency = activeCurrencies.FirstOrDefault();
                        foreach (ProductPriceType priceType in SmallCollectionCache.Instance.ProductPriceTypes)
                      {
                         
                    %>
                    <tr>
                        <td>
                            <%= priceType.Name %>
                        </td>
                        <td>  
                        <% 
                          List<ProductPrice> Prices = ViewData["ProductPrices"] as List<ProductPrice>; 
                          ProductPrice price = Prices.FirstOrDefault(pp => pp.ProductPriceTypeID == priceType.ProductPriceTypeID);
                          //INICIO 06042017 COMENTADO POR IPN 
                             
                            //string simbolo = firstCurrency == null ? "" : firstCurrency.CurrencySymbol;
                          //FIN 06042017
                           %>
                            
                         <%--  INICIO 06042017 COMENTADO POR IPN  --%>
                         <%-- <input class="prc"  readonly="true" type="text" id="priceType<%= priceType.ProductPriceTypeID %>"  style="width: 6.25em; border-width:0; text-align:center;" value="<%= price == null    ? "N/A" : precioFormat  %>" />--%>
                         <%--  FIN 06042017 --%>
                        <%-- INICIO 06042017 AGREGADO POR IPN PARA GENERALISAR EL FORMATO DE MONEDA POR TIPO DE LENGUAJE--%>
                          <input class="prc"  readonly="true" type="text" id="Text1"  style="width: 6.25em; border-width:0; text-align:center;" value="<%= price == null    ? "N/A" : price.Price.ToString("C", CoreContext.CurrentCultureInfo)  %>" />
                          <%--  FIN 06042017 --%>








                        </td>
                    </tr>
                    <%} %>
                </table>

				<br />
				<% 
					var applicableProperties = Model.Product.ProductBase.ProductBaseProperties.Select(pbp => pbp.ProductPropertyType).Where(ppt => ppt.IsProductVariantProperty && ppt.ProductPropertyValues.Count > 0);
					var applicableProducts = Model.Product.ProductBase.Products.Where(p => !p.IsVariantTemplate);
					var totalChildProductCount = Model.Product.ProductBase.Products.Count();
					if (totalChildProductCount > 1 && applicableProducts.Count() > 0)
					{
				%>
				<h3>
					<%= Html.Term("AvailableVariantsOfThisProduct", "Available Variants of this Product")%>
					<span><a href="<%= ResolveUrl(string.Format("~/Products/Products/VariantSKUS/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>">
						<%= Html.Term("Edit", "Edit")%></a></span></h3>
				<table width="100%" class="DataGrid">
					<tr class="GridColHead">
						<th>
							<%= Html.Term("SKU")%>
						</th>
						<th>
							<%= Html.Term("Name")%>
						</th>
						<%
						foreach (var type in applicableProperties)
						{
						%>
						<th>
							<%=type.GetTerm()%>
						</th>
						<%
						}
						%>
					</tr>
					<%
						count = 0;
						foreach (var p in applicableProducts)
						{
					%>
					<tr <%= count % 2 == 1 ? "class=\"Alt\"" : "" %>>
						<td>
							<%= p.SKU%>
						</td>
						<td>
							<% 
							var translations = p.Translations;
							//Check to see if a translation was found
							//If a translation is found, use that translation's Name() call.
							//If no translation is found, this might be a variant product that was loaded into the model's translation section
							if (translations.Count > 0)
							{
							%>
							<%= p.Translations.Name()%>
							<%
							}
							else if (Model.VariantProductTranslations.Any(x => x.Key == p.ProductID))
							{
							%>
							<%=Model.VariantProductTranslations.First(x => x.Key == p.ProductID).Value.Name()%>
							<%
							}
							%>
						</td>
						<%
							foreach (var type in applicableProperties)
							{
								var currentProperty = p.Properties.FirstOrDefault(prop => prop.ProductPropertyTypeID == type.ProductPropertyTypeID);
								var currentValue = currentProperty == null ? null : currentProperty.ProductPropertyValue;
								//If currentValue is null, check the Model.VariantProductPropertyValues for an entry for the given product
								//Key is productProperty value is the ProductPropertyValue
								if (currentValue == null)
								{
									//Search the model dictionary for the current product's properties
									if (Model.VariantProductPropertyValues.Any(x => x.Key == p.ProductID))
									{
										//property is found, so grab the propertyValue.
										currentValue = Model.VariantProductPropertyValues.FirstOrDefault(x => x.Key == p.ProductID).Value;
									}
								}
						%>
						<th>
							<%=currentValue == null ? "" : currentValue.Value%>
						</th>
						<%
							}
						%>
					</tr>
					<%
							++count;
						} 
					%>
				</table>
				<%
					}
				%>
			</td>
			<td style="width: 60%; padding: .455em;">
				<h3>
					<%= Html.Term("ProductDetails", "Product Details") %><span><a href="<%= ResolveUrl(string.Format("~/Products/Products/Details/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>"><%= Html.Term("Edit", "Edit") %></a></span></h3>
				<table class="DataGrid" width="100%">
					<tr>
						<td class="FLabel">
							<%= Html.Term("SKU", "SKU") %>:
						</td>
						<td>
							<%= Model.Product.SKU %>
						</td>
					</tr>
					<tr>
						<td class="FLabel" style="width: 18.182em;">
							<%= Html.Term("ProductType", "Product Type") %>:
						</td>
						<td>
							<% 
								var productTypeName = string.Empty;
								if (SmallCollectionCache.Instance.ProductTypes.Count(pt => pt.ProductTypeID == Model.Product.ProductBase.ProductTypeID) > 0)
								{
									var productType = SmallCollectionCache.Instance.ProductTypes.GetById(Model.Product.ProductBase.ProductTypeID);
									productTypeName = productType.GetTerm();
									if (!productType.Active)
									{
										productTypeName = string.Format("{0} - <i>({1})</i>", productTypeName, Html.Term("Inactive"));
									}
								}
								else
								{
									productTypeName = Html.Term("None");
								}
							%>
							<%= productTypeName%>
						</td>
					</tr>
					<tr>
						<td class="FLabel" style="width: 18.182em;">
							<%= Html.Term("TaxCategory", "Tax Category") %>:
						</td>
						<td>
							<% 
								string taxCategoryName = string.Empty;
								if (Model.Product.ProductBase.TaxCategoryID.HasValue)
								{
									var taxCategory = SmallCollectionCache.Instance.TaxCategories.GetById(Model.Product.ProductBase.TaxCategoryID.Value);
									taxCategoryName = taxCategory.GetTerm();
									if (!taxCategory.Active)
									{
										taxCategoryName = string.Format("{0} - <i>({1})</i>", taxCategoryName, Html.Term("Inactive"));
									}
								}
								else
								{
									taxCategoryName = Html.Term("None");
								}
							%>
							<%= taxCategoryName%>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("BackOrderBehavior", "Back Order Behavior") %>:
						</td>
						<td>
							<%= SmallCollectionCache.Instance.ProductBackOrderBehaviors.GetById(Model.Product.ProductBackOrderBehaviorID).GetTerm() %>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("Weight", "Weight") %>:
						</td>
						<td>
							<%= Model.Product.Weight %>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("ChargeShipping", "Charge Shipping") %>:
						</td>
						<td>
							<input id="chkChargeShipping" type="checkbox" disabled="disabled" <%= Model.Product.ProductBase.ChargeShipping ? "checked=\"checked\"" : "" %> />
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("ChargeTax", "Charge Tax") %>:
						</td>
						<td>
							<input id="chkChargeTax" type="checkbox" disabled="disabled" <%= Model.Product.ProductBase.ChargeTax ? "checked=\"checked\"" : "" %> />
						</td>
					</tr>
					<%
						if (ViewBag.DisplayChargeTaxOnShipping == null || ViewBag.DisplayChargeTaxOnShipping)
						{
					%>
					<tr>
						<td class="FLabel">
							<%= Html.Term("ChargeTaxOnShipping", "Charge Tax On Shipping") %>:
						</td>
						<td>
							<input id="chkChargeTaxOnShipping" type="checkbox" disabled="disabled" <%= Model.Product.ProductBase.ChargeTaxOnShipping ? "checked=\"checked\"" : "" %> />
						</td>
					</tr>
					<%
						}
					%>
					<tr>
						<td class="FLabel">
							<%= Html.Term("IsShippable", "Is Shippable") %>:
						</td>
						<td>
							<input id="chkIsShippable" type="checkbox" disabled="disabled" <%= Model.Product.ProductBase.IsShippable ? "checked=\"checked\"" : "" %> />
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("UpdateInventoryOnBase", "Update Inventory On Base") %>:
						</td>
						<td>
							<input id="chkUpdateInventoryOnBase" type="checkbox" disabled="disabled" <%= Model.Product.ProductBase.UpdateInventoryOnBase ? "checked=\"checked\"" : "" %> />
						</td>
					</tr>
					<%
						if (Model.Product.ChildProductRelations.Count(r => r.ProductRelationsTypeID == (int)Constants.ProductRelationsType.Kit) > 0)
						{
					%>
					<tr>
						<td class="FLabel">
							<%= Html.Term("ShowKitContents", "Show Kit Contents")%>:
						</td>
						<td>
							<input id="chkShowKitContents" type="checkbox" disabled="disabled" <%= Model.Product.ShowKitContents ? "checked=\"checked\"" : "" %> />
						</td>
					</tr>
					<%
						}
					%>
				</table>
				<h3>
					<%= Html.Term("ProductCMS", "Product CMS") %>
					<span><a href="<%= ResolveUrl(string.Format("~/Products/Products/CMS/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>">
						<%= Html.Term("Edit", "Edit") %></a></span></h3>
				<table class="DataGrid" width="100%">
					<tr>
						<td class="FLabel">
							<%= Html.Term("DisplayName", "Display Name") %>:
						</td>
						<td>
							<%= Model.Product.Translations.Name() %>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("ShortDescription", "Short Description") %>:
						</td>
						<td>
							<%= Model.Product.Translations.ShortDescription() %>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("LongDescription", "Long Description") %>:
						</td>
						<td>
							<%= Model.Product.Translations.LongDescription() %>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							<%= Html.Term("Images", "Images") %>:
						</td>
						<td>
							<p>
								<% 
									if (Model.Product.Files.ContainsProductFileTypeID(Constants.ProductFileType.Image.ToInt()))
									{
										foreach (ProductFile image in Model.Product.Files.GetByProductFileTypeID(Constants.ProductFileType.Image.ToInt()).OrderBy(pf => pf.SortIndex))
										{ 
								%>
								<img src="<%= image.FilePath.ReplaceFileUploadPathToken() %>" alt="" class="productImageThumb" />
								<%
										}
									} 
								%>
							</p>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
