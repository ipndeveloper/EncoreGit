<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript">
		$(function () {
			$('#btnEncrypt').click(function () {
				$.post('<%= ResolveUrl("~/Admin/Configuration/EncryptPaymentGateway") %>', function () {
					window.location.reload(true);
				});
			});
			$('#btnDecrypt').click(function () {
				$.post('<%= ResolveUrl("~/Admin/Configuration/DecryptPaymentGateway") %>', function () {
					window.location.reload(true);
				});
			});
		});
	</script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Admin") %>">
		<%= Html.Term("Admin") %></a> >
	<%= Html.Term("Configuration")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<h2>
		<%= Html.Term("Configuration")%></h2>
	<p>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnEncrypt">Encrypt</a>
		<a href="javascript:void(0);" class="Button BigBlue" id="btnDecrypt">Decrypt</a>
	</p>
</asp:Content>
