<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Shared/ProductManagement.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.ProductPropertyType>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/MediaLibrary.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ajaxupload.js") %>"></script>
    <%if (Model.Editable)
      { %>
    <script type="text/javascript">
        var uploads = {};
        var currentLi;

        $(function () {

        	$('#btnAddValue').click(function () {
        		var data = {
        			productPropertyTypeId: ($('#propertyTypeId').val() != "" ? $('#propertyTypeId').val() : -1),
        			valueId: -1
        		}

        		$.post('<%= ResolveUrl("~/Products/Properties/RenderPropertyValue") %>', data, function (response) {
        			if (response.result)
        				$('#values').append(response.valueHTML);
        			else
        				showMessage(response.message);
        		});
        		//$('#values').append('<li><input type="text" name="value0" class="value" /><a href="javascript:void(0);" class="delete" style="margin-left:.273em;"><img src="<%= ResolveUrl("~/") %>Content/Images/Icons/remove-trans.png" alt="Delete" /></a></li>');
        	});
        	$('#values .delete').live('click', function () {
        		var value = $(this).parent().find('input'), valueId = value.attr('name').replace(/\D/g, '');
        		if (valueId == 0 || confirm('Are you sure you want to delete this value?')) {
        			if (valueId > 0) {
        				$.post('<%= ResolveUrl("~/Products/Properties/DeleteValue") %>', { valueId: valueId });
        			}
        			value.parent().fadeOut('normal', function () {
        				$(this).remove();
        			});
        		}
        	});

        	$('#freeform').click(function () {
                $(this).prop('checked') && $('#valuesContainer').fadeOut('fast') && $('#displayTypeContainer').fadeOut('fast') && $('#showNameAndThumbnailContainer').fadeOut('fast') && $('#dataTypeContainer').fadeIn('fast') ||
                    $('#valuesContainer').fadeIn('fast') && $('#displayTypeContainer').fadeIn('fast') && $('#showNameAndThumbnailContainer').fadeIn('fast') && $('#dataTypeContainer').fadeOut('fast');
        	});

        	$('#btnSave').click(function () {
        		if ($('#propertyTypeProperties').checkRequiredFields()) {
        			var data = {
        				propertyTypeId: $('#propertyTypeId').val(),
        				name: $('#name').val(),
                        required: $('#required').prop('checked'),
                        isMaster: $('#isMaster').prop('checked'),
                        showNameAndThumbnail: $('#showNameAndThumbnail').prop('checked'),
                        isProductVariantProperty: $('#isProductVariantProperty').prop('checked'),
        				dataType: $('#dataType').val()
        			}, t = $(this);

        			if ($('#htmlInputTypeId').val() != 0) {
        				data['htmlInputTypeId'] = $('#htmlInputTypeId').val();
        			}

                    if (!$('#freeform').prop('checked')) {
        				$('#values .value').filter(function () { return !!$(this).val(); }).each(function (i) {
        					data['values[' + i + '].ProductPropertyValueID'] = $(this).attr('name').replace(/\D/g, '');
        					data['values[' + i + '].Value'] = $(this).val();
        					var thumbnail = $(this).closest('li').find('.imageThumbnail')
        					if (thumbnail.length != 0 && thumbnail.attr("src") != null) {
        						data['values[' + i + '].FilePath'] = thumbnail.attr("src");
        					}
        				});
        			}

        			var options = {
        				url: '<%= ResolveUrl("~/Products/Properties/Save") %>',
        				showLoading: t,
        				data: data,
        				success: function (response) {
        					showMessage(response.message || 'Property saved successfully!', !response.result);
        					if (response.result)
        						$('#propertyTypeId').val(response.propertyTypeId);
        				}
        			};
        			NS.post(options);
        		}
        	});

        	$('.btnDeleteImage').click(function () {
        		var li = $(this).closest("li");
        		li.find('.thumbnailPreview').hide();
        		li.find('.imageThumbnail').attr("src", '');
        		li.find('.btnBrowseServer').show();
        	});

        	$('.imageThumbnail').click(function () {
        		//alert("Here");
        		var li = $(this).closest("li");
        		//alert(li.length);
        		//alert(li.find('.btnBrowseServer').attr("id"));
        		//li.find('.btnBrowseServer').click();
        		li.find('.btnBrowseServer').trigger('click');
        	});
        });

    </script>
    <%} %>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Properties") %>">
            <%= Html.Term("Properties") %></a> >
    <%= Model.ProductPropertyTypeID > 0 ? Html.Term("EditProperty", "Edit Property") : Html.Term("NewProperty", "New Property") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Model.ProductPropertyTypeID > 0 ? Html.Term("EditProperty", "Edit Property") : Html.Term("NewProperty", "New Property") %></h2>
    </div>
    <input type="hidden" id="propertyTypeId" value="<%= Model.ProductPropertyTypeID == 0 ? "" : Model.ProductPropertyTypeID.ToString() %>" />
    <table id="propertyTypeProperties" width="100%" cellspacing="0" class="DataGrid">
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Name", "Name") %>:
            </td>
            <td>
                <%string valueTitle = Model.Name;
                  //if (!Model.TermName.IsNullOrEmpty())
                  //{
                  //    valueTitle = NetSteps.Common.Globalization.Translation.GetTerm(Model.TermName);
                  //}
                  valueTitle = Model.GetTerm();
                %>
                <input type="text" id="name" class="required" maxlength="25" name="<%= Html.Term("NameRequired", "Name is required.") %>"
                    value="<%= valueTitle%>" style="width: 25em;" <%= Model.Editable ? "" : "disabled=\"disabled\"" %> />
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Required", "Required") %>:
            </td>
            <td>
                <input type="checkbox" id="required" <%= Model.Required ? "checked=\"checked\"" : "" %>
                    <%= Model.Editable ? "" : "disabled=\"disabled\"" %> />
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Variant", "Variant")%>:
            </td>
            <td>
                <input type="checkbox" id="isProductVariantProperty" <%= Model.IsProductVariantProperty ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Master", "Master")%>:
            </td>
            <td>
                <input type="checkbox" id="isMaster" <%= Model.IsMaster ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td style="width: 13.636em;">
                <%= Html.Term("Freeform", "Freeform") %>:
            </td>
            <td>
                <input type="checkbox" id="freeform" <%= Model.ProductPropertyValues.Count == 0 ? "checked=\"checked\"" : "" %>
                    <%= Model.Editable ? "" : "disabled=\"disabled\"" %> />
            </td>
        </tr>
        <tr id="showNameAndThumbnailContainer" <%= Model.ProductPropertyValues.Count == 0 ? "style=\"display:none;\"" : "" %>>
            <td style="width: 13.636em;">
                <%= Html.Term("DisplayBothName&ThumbnailOnFront-end", "Display both name & thumbnail on front-end.")%>:
            </td>
            <td>
                <input type="checkbox" id="showNameAndThumbnail" <%= Model.ShowNameAndThumbnail ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr id="displayTypeContainer" <%= Model.ProductPropertyValues.Count == 0 ? "style=\"display:none;\"" : "" %>>
            <td style="width: 13.636em;">
                <%= Html.Term("DisplayType", "Display Type") %>:
            </td>
            <td>
                <select id="htmlInputTypeId">
                    <option value="0"><%= Html.Term("PleaseSelect", "Please Select") %></option>
                    <%foreach(HtmlInputType type in SmallCollectionCache.Instance.HtmlInputTypes) { %>
                        <option value="<%=type.HtmlInputTypeID %>" <%=type.HtmlInputTypeID.Equals(Model.HtmlInputTypeID) ? "selected='selected'" : "" %>><%=type.GetTerm() %></option>
                    <%}%>
                    <!--<option>Dropdown</option>
                        <option>Radio</option>
                        <option>Checkbox</option>
                        <option>Thumbnail</option>
                        <option>Text</option>-->
                </select>
            </td>
        </tr>
        <tr id="dataTypeContainer" <%= Model.ProductPropertyValues.Count == 0 ? "" : "style=\"display:none;\"" %>>
            <td style="width: 13.636em;">
                <%= Html.Term("Type", "Type") %>:
            </td>
            <td>
                <select id="dataType" <%= Model.Editable ? "" : "disabled=\"disabled\"" %>>
                    <option value="System.String" <%= Model.ProductPropertyValues.Count == 0 || Model.DataType == "System.String" ? "selected=\"selected\"" : "" %>>
                        <%= Html.Term("Text", "Text") %></option>
                    <option value="System.Int32" <%= Model.DataType == "System.Int32" ? "selected=\"selected\"" : "" %>>
                        <%= Html.Term("Number", "Number") %></option>
                    <option value="System.DateTime" <%= Model.DataType == "System.DateTime" ? "selected=\"selected\"" : "" %>>
                        <%= Html.Term("Date&Time", "Date & Time") %></option>
                </select>
            </td>
        </tr>
        <tr id="valuesContainer" <%= Model.ProductPropertyValues.Count == 0 ? "style=\"display:none;\"" : "" %>>
            <td>
                <%= Html.Term("Values", "Values") %>:
            </td>
            <td>
                <ul id="values" style="line-height: 2.545em;">
                    <%foreach (var value in Model.ProductPropertyValues)
                      {%>
                        <%Html.RenderPartial("PropertyValue", value);%>
                    <%} %>
                </ul>
                <%if (Model.Editable)
                  { %>
                <a id="btnAddValue" href="javascript:void(0);" class="DTL Add">
                    <%= Html.Term("AddaValue", "Add a value") %></a>
                <%} %>
            </td>
        </tr>
    </table>
    <p>
        <%if (Model.Editable)
          { %>
        <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
            <%= Html.Term("Save", "Save") %></a>
        <%} %>
        <a href="<%= ResolveUrl("~/Products/Properties") %>" class="Button"><span>
            <%= Html.Term("Cancel", "Cancel") %></span></a>
    </p>
</asp:Content>
