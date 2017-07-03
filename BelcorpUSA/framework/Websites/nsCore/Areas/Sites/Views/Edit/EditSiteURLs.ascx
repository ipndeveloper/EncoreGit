<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<List<EditModel.EditSiteUrlModel>>" %>
<%@ Import Namespace="nsCore.Areas.Sites.Models" %>
<%--NOTE: Don't use html helpers to render the html in this control. They bind to the post data regardless of changes made to the model in an action method--%>

<%= Html.ValidationMessage("SiteUrls") == null ? string.Empty : Html.ValidationMessage("SiteUrls") + "<br />" %>
<% for (int i = 0; i < Model.Count; i++)
   {
       string namePrefix = string.Format("SiteUrls[{0}].", i);
       var urlModel = Model[i];
       if (Model.Count > 1)
       { %>
<img onclick="RemoveURL(<%= i %>);" style="cursor: pointer" src="<%= Url.Content("~/Content/Images/Icons/remove-12-trans.png") %>" title="<%= Html.Term("remove") %>" alt="<%= Html.Term("remove") %>" />
    <% } %>
http://
<input type="text" name="<%= namePrefix + "Subdomain" %>" value="<%= urlModel.Subdomain %>" style="width: 100px;" />
.
<select name="<%= namePrefix + "Domain" %>">
    <% foreach (var d in urlModel.Domains)
       { %>
    <option value="<%= d.Value %>" <%= d.Selected ? "selected=\"selected\"" : string.Empty %>><%= d.Text %></option>
    <% } %>
</select>
<input type="checkbox" name="<%= namePrefix + "IsPrimaryUrl" %>" value="True" <%= urlModel.IsPrimaryUrl ? "checked=\"checked\"" : string.Empty %> />&nbsp;<%= Html.Term("Primary URL") %>
<%= Html.ValidationMessage(namePrefix + "FullURL")%>
<br />
<% } %>

<a href="javascript:void(0);" onclick="AddAnotherURL();" class="DTL Add"><%= Html.Term("AddAnotherUrl", "Add another url")%></a>