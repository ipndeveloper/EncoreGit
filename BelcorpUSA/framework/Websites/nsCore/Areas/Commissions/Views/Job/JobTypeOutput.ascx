<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Commissions.Models.JobOutputModel>" %>

<% int counter = 0;
	foreach (var line in Model.JobStatuses)
	{
%>
		<p row="<%=counter %>"><%=line %></p>  
<%		counter++;
	} 
%>