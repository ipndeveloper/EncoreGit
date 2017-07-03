<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%if (TempData["Error"] != null)
   { %>
<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
	-moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
	border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
	margin-bottom: 10px; padding: 7px;">
	<div style="color: #FF0000; display: block;">
		<img alt="" src="<%= ResolveUrl("~/Content/Images/exclamation.png") %>" />&nbsp;<%= TempData["Error"] %></div>
</div>
<%} %>