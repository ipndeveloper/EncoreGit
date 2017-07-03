<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("AuditHistory", "Audit History")%></h2>
		<%= ViewData["Links"] %>
	</div>
    <% var auditHistoryGridModel = new AuditHistoryGridModel
       {
           EntityName = ViewData["EntityName"] as string,
           EntityId = Convert.ToInt32(ViewData["ID"]),
           LoadedEntitySessionVarKey = ViewData["LoadedEntitySessionVarKey"] as string
       };
       Html.RenderPartial("AuditHistoryGrid", auditHistoryGridModel); %>
</asp:Content>
