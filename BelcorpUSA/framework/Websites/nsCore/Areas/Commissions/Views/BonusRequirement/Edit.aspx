<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master"%>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">    
    <div class="SectionNav">
        <ul class="SectionLinks">
            <%= Html.SelectedLink("~/Commissions/BonusRequirement/", Html.Term("Browse", "Browse Commission Rules"), LinkSelectionType.ActualPage, CoreContext.CurrentUser, "")%>                
        </ul>
    </div>    
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
  <a href="<%= ResolveUrl("~/Commissions/BonusRequirement") %>">
        <%= Html.Term("Commissions", "Commissions")%></a> >
    <%= Html.Term("EditBonusRequirement", "Edit Bonus Requirement")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

 <% if (TempData["Error"] != null && !string.IsNullOrEmpty(TempData["Error"].ToString()))
       { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
        -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0;
        border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold;
        margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="UI-icon icon-exclamation">
            <%= TempData["Error"].ToString() %></div>
    </div>
    <% } %>
    
    <%Html.RenderPartial("~/Areas/Commissions/Views/BonusRequirement/Detail.cshtml", Model); %>
</asp:Content>
