<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
	<script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
	<script type="text/javascript">
		$(function () {
			$('.numeric').numeric();
			$('#btnSave').click(function () {
				if ($('#properties').checkRequiredFields()) {
					var data = {};

					$('#properties .property').filter(function () { return !!$('.propertyValue', this).val(); }).each(function (i) {
						var value = $('.propertyValue', this);
						data['properties[' + i + '].ProductPropertyID'] = $('.productPropertyId', this).val();
						data['properties[' + i + '].ProductPropertyTypeID'] = $('.productPropertyTypeId', this).val();
						if (value.is('select')) {
							data['properties[' + i + '].ProductPropertyValueID'] = value.val();
						} else {
							data['properties[' + i + '].PropertyValue'] = value.val();
						}
					});

					$.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveProperties/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
						showMessage(response.message || '<%= Html.Term("PropertiesSavedSuccessfully", "Properties saved successfully!") %>', !response.result);
					});
				}
			});

			$('.TimePicker').timepickr({ convention: 12 });
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
			<%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
				<%= Model.Translations.Name() %></a> >
	<%= Html.Term("Properties") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Properties", "Properties") %></h2>
	</div>
	<table id="properties" class="FormTable" width="100%">
		<% 
			var productType = ProductType.LoadFull(Model.ProductBase.ProductTypeID);
			var productBase = Model.ProductBase;
			var propertiesExist = false;
			if (productType.ProductPropertyTypes.Count > 0 || productBase.ProductBaseProperties.Select(pbp => pbp.ProductPropertyType).Any())
			{
				propertiesExist = true;
				foreach (var propertyType in productType.ProductPropertyTypes)
				{
					var property = Model.Properties.FirstOrDefault(p => p.ProductPropertyTypeID == propertyType.ProductPropertyTypeID); 
		%>
		<tr class="property">
			<td class="FLabel">
				<%=propertyType.GetTerm()%>:
				<input type="hidden" class="productPropertyId" value="<%= property == null ? "" : property.ProductPropertyID.ToString() %>" />
				<input type="hidden" class="productPropertyTypeId" value="<%= propertyType.ProductPropertyTypeID %>" />
			</td>
			<td>
				<%
					if (!propertyType.ProductPropertyValues.Any())
					{ 
				%>
				<input type="text" class="propertyValue<%= propertyType.DataType == "System.DateTime" ? " DatePicker" : "" %><%= propertyType.DataType == "System.Int32" ? " numeric" : "" %><%= propertyType.Required ? " required\" name=\"" + propertyType.Name + " is required.\"" : "" %>" value="<%= property != null ? property.PropertyValue : "" %>" />
				<%
					}
					else
					{ 
				%>
				<select class="propertyValue">
					<%
						if (!propertyType.Required)
						{ 
					%>
					<option value="" <%= property == null || !property.ProductPropertyValueID.HasValue ? "selected=\"selected\"" : "" %>>
						<%= Html.Term("NoValue", "No Value") %></option>
					<%
						}
						foreach (var value in propertyType.ProductPropertyValues)
						{ 
					%>
					<option value="<%= value.ProductPropertyValueID %>" <%= property != null && property.ProductPropertyValueID == value.ProductPropertyValueID ? "selected=\"selected\"" : ""  %>>
						<%= value.Name%></option>
					<%
						} 
					%>
				</select>
				<%
					} 
				%>
			</td>
		</tr>
		<%
				}
				foreach (var propertyType in productBase.ProductBaseProperties.Select(pbp => pbp.ProductPropertyType))
				{
					var property = Model.Properties.FirstOrDefault(p => p.ProductPropertyTypeID == propertyType.ProductPropertyTypeID); 
		%>
		<tr class="property">
			<td class="FLabel">
				<%=propertyType.GetTerm()%>:
				<input type="hidden" class="productPropertyId" value="<%= property == null ? "" : property.ProductPropertyID.ToString() %>" />
				<input type="hidden" class="productPropertyTypeId" value="<%= propertyType.ProductPropertyTypeID %>" />
			</td>
			<td>
				<%
					if (propertyType.ProductPropertyValues.Count == 0)
					{ 
				%>
				<input type="text" class="propertyValue<%= propertyType.DataType == "System.DateTime" ? " DatePicker" : "" %><%= propertyType.DataType == "System.Int32" ? " numeric" : "" %><%= propertyType.Required ? " required\" name=\"" + propertyType.Name + " is required.\"" : "" %>" value="<%= property != null ? property.PropertyValue : "" %>" />
				<%
					}
					else
					{ 
				%>
				<select class="propertyValue">
					<%
						if (!propertyType.Required)
						{ 
					%>
					<option value="" <%= property == null || !property.ProductPropertyValueID.HasValue ? "selected=\"selected\"" : "" %>>
						<%= Html.Term("NoValue", "No Value") %></option>
					<%
						}
						foreach (var value in propertyType.ProductPropertyValues)
						{ 
					%>
					<option value="<%= value.ProductPropertyValueID %>" <%= property != null && property.ProductPropertyValueID == value.ProductPropertyValueID ? "selected=\"selected\"" : ""  %>>
						<%= value.Value%></option>
					<%
						}
					%>
				</select>
				<%
					} 
				%>
			</td>
		</tr>
		<%
				}
			}
			else
			{ 
		%>
		<tr>
			<td>
				<%= Html.Term("NoPropertiesForThisProductTypeAndProductBase", "No properties for this product type and product base") %>
			</td>
		</tr>
		<%
			}
		%>
	</table>
	<p>
		<%
			if (propertiesExist)
			{ 
		%>
		<a href="javascript:void(0);" id="btnSave" class="Button BigBlue">
			<%= Html.Term("Save", "Save") %></a>
		<%
			} 
		%>
		<a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>" class="Button">
			<%= Html.Term("Cancel", "Cancel") %></a>
	</p>
</asp:Content>
