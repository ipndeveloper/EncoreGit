<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.VariantProductModel>" %>
<%@ Import Namespace="System.Web.Helpers" %>
<%@ Import Namespace="NetSteps.Common.Extensions" %>
<%
	var currentProduct = Model.Product;
    var currentProductRelations = Model.ProductVariant;
	var productBase = (ProductBase)ViewData["productBase"];
	var productType = ProductType.LoadFull(productBase.ProductTypeID);
	var baseProperties = productBase.ProductBaseProperties.Select(pbp => pbp.ProductPropertyType).Where(ppt => ppt.IsProductVariantProperty);// && ppt.ProductPropertyValues.Count > 0);
	var typeProperties = productType.ProductPropertyTypes.Where(ppt => !baseProperties.Any(bp => bp.ProductPropertyTypeID == ppt.ProductPropertyTypeID) && ppt.IsProductVariantProperty);
	var applicableProperties = baseProperties.Union(typeProperties);
%>
<tr class="varSKUline">
	<td>
		<input type="checkbox" value="<%= currentProduct.ProductID %>" class="productId" />
	</td>
	<td>
		<input type="text" class="pad3 fullWidth variantSKU" value="<%= currentProduct.SKU %>"<%=currentProduct.ProductID == 0 ? "" : " readonly" %> id="variantSKU<%=Model.Index%>"/>
	</td>
	<td>
		<input type="text" class="pad3 fullWidth variantName" value="<%=currentProduct.ProductID > 0 ? currentProduct.Translations.Name() : productBase.Translations.Name() %>" id="variantName<%=Model.Index%>"/>
	</td>
	<td>
		<%=currentProduct.ProductID == 0 ? "" : currentProduct.Active ? Html.Term("Active") : Html.Term("Inactive")%>
	</td>
	  <td class="property">
     <select class="propertyValue">  
     <option value="0" rel=""></option>   
    <% foreach (ProductPropertyType propertyType in applicableProperties)
        { 
        foreach (var propertyValue in propertyType.ProductPropertyValues)
		{
            var property = currentProduct.Properties.FirstOrDefault(p => p.ProductPropertyTypeID == propertyType.ProductPropertyTypeID);
			
			var currentPropertyValue = property != null ? property.ProductPropertyValueID : 0;
            
            %>
			<option value="<%=propertyValue.ProductPropertyValueID %>" 
            
             <%=propertyValue.ProductPropertyValueID==currentPropertyValue ? "selected='selected'" : ""  %> 
            >
				<%=propertyValue.Value%></option>
		<%}
       }%>
    </select>
     </td>
     <td>
  <script type="text/javascript">
      $(function () {

          $('.variantOffertType').numeric();
          $('.variantExternalCode').numeric();

          var productSelected = false;
          var index = '<%=Model.Index%>';
          var productoId = $('#productoIdFilter' + index);
          $('#variantSapCode' + index).removeClass('Filter').css('width', '275px').watermark('<%=Html.JavascriptTerm("ProductSearch","Look up product by ID or name") %>').jsonSuggest('<%=ResolveUrl("~/Products/Materials/SearchFilter") %>',
                        { onSelect: function (item) {
                            productoId.val(item.id);
                            productSelected = false;
                        }, minCharacter: 3,
                            source: $('#variantSapCode' + index),
                            ajaxResults: true,
                            maxResult: 50,
                            showMore: true
                        }).blur(function () {
                            if (!$(this).val() || !$(this).val() == $(this).data('watermark')) {
                                productoId.val('');
                            }
                            productSelected = false;
                        }).after(productoId);
      });
  </script>
		<input type="text" class="pad3 fullWidth variantSapCode" id="variantSapCode<%=Model.Index%>" 
        value="<%=currentProductRelations.SapCode %>" />
        <input type="hidden" id="productoIdFilter<%=Model.Index%>"
         value="<%=currentProductRelations.MaterialID %>"
         class="Filter" />
        
	</td>
    <td>
		<input type="text" class="pad3 fullWidth variantOffertType" maxlength="3" 
        value="<%=currentProductRelations.OfferType %>" id="variantOffertType<%=Model.Index%>"
        />
	</td>
    <td>
		<input type="text" class="pad3 fullWidth variantExternalCode" maxlength="5"
        value="<%=currentProductRelations.ExternalCode  %>" id="variantExternalCode<%=Model.Index%>"
         />
	</td>

</tr>
