<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.AccountPropertyType>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<%if (Model.Editable)
	  { %>
	<script type="text/javascript">
		$(function () {
			$('#btnAddValue').click(function () {
				$('#values').append('<li><input type="text" name="value0" class="value" /><a href="javascript:void(0);" class="delete" style="margin-left:.273em;"><img src="<%= ResolveUrl("~/") %>Content/Images/Icons/remove-trans.png" alt="Delete" /></a></li>');
			});
			$('#values .delete').live('click', function () {
				var value = $(this).parent().find('input'), valueId = value.attr('name').replace(/\D/g, '');
				if (valueId == 0 || confirm('Are you sure you want to delete this value?')) {
					if (valueId > 0) {
						$.post('<%= ResolveUrl("~/Accounts/Properties/DeleteValue") %>', { valueId: valueId });
					}
					value.parent().fadeOut('normal', function () {
						$(this).remove();
					});
				}
			});

			$('#freeform').click(function () {
				$(this).prop('checked') && $('#valuesContainer').fadeOut('fast') && $('#dataTypeContainer').fadeIn('fast') || $('#valuesContainer').fadeIn('fast') && $('#dataTypeContainer').fadeOut('fast');
			});

			$('#btnSave').click(function () {
				if ($('#propertyTypeProperties').checkRequiredFields()) {
					var data = {
						propertyTypeId: $('#propertyTypeId').val(),
						name: $('#name').val(),
						required: $('#required').prop('checked'),
						active: $('#active').prop('checked'),
						dataType: $('#dataType').val(),
                        minLength: $('#MinimumLength').val(),
                        maxLength: $('#MaximumLength').val()
					}, t = $(this);

					showLoading(t);

					if (!$('#freeform').prop('checked')) {
						$('#values .value').filter(function () { return !!$(this).val(); }).each(function (i) {
							data['values[' + i + '].AccountPropertyValueID'] = $(this).attr('name').replace(/\D/g, '');
							data['values[' + i + '].Value'] = $(this).val();
						});
					}

					$.post('<%= ResolveUrl("~/Accounts/Properties/Save") %>', data, function (response) {
						hideLoading(t);
						showMessage(response.message || 'Property saved successfully!', !response.result);
						if (response.result)
							$('#propertyTypeId').val(response.propertyTypeId);
					});
				}
			});
		});
	</script>
	<%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>">
		<%= Html.Term("Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Properties") %>">
			<%= Html.Term("Properties") %></a> >
	<%= Model.AccountPropertyTypeID > 0 ? Html.Term("EditProperty", "Edit Property") : Html.Term("NewProperty", "New Property") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Model.AccountPropertyTypeID > 0 ? Html.Term("EditProperty", "Edit Property") : Html.Term("NewProperty", "New Property") %></h2>
	</div>
	<input type="hidden" id="propertyTypeId" value="<%= Model.AccountPropertyTypeID == 0 ? "" : Model.AccountPropertyTypeID.ToString() %>" />
	<table id="propertyTypeProperties" width="100%" cellspacing="0" class="DataGrid">
		<tr>
			<td style="width: 13.636em;">
				<%= Html.Term("Name", "Name") %>:
			</td>
			<td>
				<%string valueTitle = Model.Name;
				  if (!Model.TermName.IsNullOrEmpty())
				  {
					  valueTitle = NetSteps.Common.Globalization.Translation.GetTerm(Model.TermName);
				  }
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
				<%= Html.Term("Active", "Active")%>:
			</td>
			<td>
				<input type="checkbox" id="active" <%= Model.Active ? "checked=\"checked\"" : "" %>
					<%= Model.Editable ? "" : "disabled=\"disabled\"" %> />
			</td>
		</tr>
		<tr>
			<td style="width: 13.636em;">
				<%= Html.Term("Freeform", "Freeform") %>:
			</td>
			<td>
				<input type="checkbox" id="freeform" <%= Model.AccountPropertyValues.Count == 0 ? "checked=\"checked\"" : "" %>
					<%= Model.Editable ? "" : "disabled=\"disabled\"" %> />
			</td>
		</tr>
		<tr id="dataTypeContainer" <%= Model.AccountPropertyValues.Count == 0 ? "" : "style=\"display:none;\"" %>>
			<td style="width: 13.636em;">
				<%= Html.Term("Type", "Type") %>:
			</td>
			<td>
				<select id="dataType" <%= Model.Editable ? "" : "disabled=\"disabled\"" %>>
					<option value="System.String" <%= Model.AccountPropertyValues.Count == 0 || Model.DataType == "System.String" ? "selected=\"selected\"" : "" %>>
						<%= Html.Term("Text", "Text") %></option>
					<option value="System.Int32" <%= Model.DataType == "System.Int32" ? "selected=\"selected\"" : "" %>>
						<%= Html.Term("Number", "Number") %></option>
					<option value="System.DateTime" <%= Model.DataType == "System.DateTime" ? "selected=\"selected\"" : "" %>>
						<%= Html.Term("Date&Time", "Date & Time") %></option>
				</select>
			</td>
		</tr>
		<tr id="valuesContainer" <%= Model.AccountPropertyValues.Count == 0 ? "style=\"display:none;\"" : "" %>>
			<td>
				<%= Html.Term("Values", "Values") %>:
			</td>
			<td>
				<ul id="values" style="line-height: 2.545em;">
					<%foreach (var value in Model.AccountPropertyValues)
					  { %>
					<li>
						<%if (Model.Editable)
						  { %>
						<input type="text" name="value<%= value.AccountPropertyValueID %>" value="<%= value.Value %>"
							class="value" /><a href="javascript:void(0);" class="delete" style="margin-left: .273em;"><img
								src="<%= ResolveUrl("~/") %>Content/Images/Icons/remove-trans.png" alt="Delete" /></a>
						<%}
						  else
						  { %>
						<img src="<%= ResolveUrl("~/Content/Images/Icons/16x16/lock_disabled-trans.png") %>"
							alt="locked" /><input type="text" value="<%= value.Value %>" disabled="disabled" />
						<%} %></li>
					<%} %>
				</ul>
				<%if (Model.Editable)
				  { %>
				<a id="btnAddValue" href="javascript:void(0);" class="DTL Add">
					<%= Html.Term("AddaValue", "Add a value") %></a>
				<%} %>
			</td>
		</tr>
        <tr>
            <td>
                <%=Html.Term("MinimumLength","Minimum Length") %>
            </td>
            <td>
                <%=Html.TextBoxFor(model=> Model.MinimumLength) %>
            </td>
        </tr>
        <tr>
            <td>
                <%=Html.Term("MaximumLength","Maximum Length") %>
            </td>
            <td>
                <%=Html.TextBoxFor(model=> Model.MaximumLength) %>
            </td>
        </tr>
	</table>
	<p>
		<%if (Model.Editable)
		  { %>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
			<%= Html.Term("Save", "Save") %></a>
		<%} %>
		<a href="<%= ResolveUrl("~/Accounts/Properties") %>" class="Button"><span>
			<%= Html.Term("Cancel", "Cancel") %></span></a>
	</p>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>