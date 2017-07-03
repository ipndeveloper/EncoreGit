<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<%--TODO: Find out the list of tokens we want to support--%>
<div class="TokenList">
    <select id="tokenList">
        <% foreach (string tokenName in EmailTemplateToken.GetTokenNames()) %>
        <% { %>
        <option value="{{<%: tokenName %>}}">
            <%: Html.Term(tokenName, tokenName.PascalToSpaced()) %></option>
        <% } %>
    </select>
    <a id="btnAddToken" href="javascript:void(0);" class="Button BigBlue"><span>
        <%= Html.Term("AddToken", "Add Token")%></span></a>
</div>
