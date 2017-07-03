<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Accounts/Views/Shared/Accounts.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Site>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

	<script type="text/javascript">
		$(function () {
			$('#btnSaveChanges').click(function () {
				if (!$('#siteDetails').checkRequiredFields()) {
					return false;
				}
				var data = {
					baseSiteId: $('#baseSiteId').val(),
					siteName: $('#siteName').val(),
					siteDescription: $('#siteDescription').val(),
					siteStatusId: $('#siteStatusId').val(),
					siteDefaultLanguageId: $('#siteDefaultLanguageId').val()
				};
				$('#siteUrls .urlContainer').each(function (i) {
					data['urls[' + i + '].SiteUrlID'] = $('.siteUrlId', this).length ? $('.siteUrlId', this).val() : 0;
					data['urls[' + i + '].Url'] = 'http://' + ($('.toggleCustomUrl', this).prop('checked') ? $('.url', this).val() : $('.subdomain', this).val() + '.' + $('.domain', this).val());
				});
				$.post('<%= ResolveUrl("~/Accounts/SiteSubscriptions/Save") %>', data, function (response) {
					showMessage(repsonse.message || 'Site saved successfully!', !response.result);
				}, 'json');
			});
		});
	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Accounts") %>"><%= Html.Term("Accounts", "Accounts") %></a> > <a href="<%= ResolveUrl("~/Accounts/Overview") %>">
		<%= CoreContext.CurrentAccount.FullName %></a> > <%= Html.Term("SiteSubscriptions", "Site Subscriptions") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("SiteSubscriptions", "Site Subscriptions") %></h2>
	</div>
	<input type="hidden" id="baseSiteId" value="<%= ViewData["BaseSiteId"] %>" />
	<% Html.RenderPartial("SiteSubscriptions", Model); %>
	<br />
	<table width="100%">
		<tr>
			<td class="FLabel">
				&nbsp;
			</td>
			<td>
				<p>
					<% if (Model != null)
		{ %>
					<a id="btnSaveChanges" href="javascript:void(0);" class="Button BigBlue"><span><%= Html.Term("SaveChanges", "Save Changes") %></span></a>
					<%  } %>
				</p>
			</td>
		</tr>
	</table>
</asp:Content>
