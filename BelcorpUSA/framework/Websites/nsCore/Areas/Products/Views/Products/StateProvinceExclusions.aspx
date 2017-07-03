<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Products.Models.StateProvinceExclusionsModel>" %>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
			<%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>">
				<%= Model.Product.Translations.Name() %></a> >
	<%= Html.Term("Details") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		$(function () {
			$('#btnSave').click(function () {
				var stateProvinceIds = {};
				$('#stateProvinceContainer .stateProvince:checked').each(function (i) {
					stateProvinceIds['stateProvinceIds[' + i + ']'] = $(this).context.name;
				});
				showLoading($('#btnSave').parent());
				$.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveStateProvinceExclusions/{0}/{1}", Model.Product.ProductBaseID, Model.Product.ProductID)) %>', stateProvinceIds, function (response) {
					hideLoading($('#btnSave').parent());
					showMessage(response.message || '<%= Html.Term("StateProvinceExclusionsSaved", "State/Province Exclusions saved!") %>', !response.result);
				});
			});
		});
	</script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("StateProvinceExclusions", "State/Province Exclusions")%></h2>
	</div>
	<table width="100%" cellspacing="0" class="">
		<tr>
			<td style="width: 50%; border-right: 1px solid #c0c0c0; padding: 5px;">
				<div id="stateProvinceContainer" class="CategoriesManagement">
					<%
						foreach (var cntry in Model.AvailableStateProvinces)
						{
					%>
					<h3>
						<%=Html.Term(cntry.Key.TermName)%></h3>
					<table width="100%" cellspacing="0">
						<tbody>
							<%
							int i = 0;
							bool newrow = true;
							foreach (var sp in cntry.Value)
							{
								if (newrow)
								{ 
							%>
							<tr>
								<%
								}
								%>
								<td width="33%">
									<%= Html.CheckBox(sp.StateProvinceID.ToString(), Model.ExistingExcludedStateProvinceIDs.Contains(sp.StateProvinceID), new { @class = "stateProvince" })%>
									<span>:
										<%=String.Format("{0} - {1}", sp.StateAbbreviation, sp.Name) %></span>
								</td>
								<%
								newrow = ++i % 3 == 0;
								if (newrow)
								{ 
								%>
							</tr>
							<%
					}
							}
							if (!newrow)
							{ 
							%>
				</tr>
				<%
						}
				%>
				</tbody>
			</table>
			<%
								}
			%>
			</div> 
		</td>
		<td style="width: 50%; padding: 5px;">
			<p id="buttonContainer">
				<a id="btnSave" href="javascript:void(0);" class="Button BigBlue"><span>
					<%= Html.Term("SaveStateProvinceExclusions", "Save State/Province Exclusions") %></span></a>
			</p>
		</td>
	</tr> 
</table>
	<%Html.RenderPartial("MessageCenter"); %>
</asp:Content>
