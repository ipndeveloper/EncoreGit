<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Support/Views/Shared/Support.Master"
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Support.Models.Ticket.HistoryViewModel>" %>

<asp:Content ContentPlaceHolderID="YellowWidget" runat="server">
    <% Html.RenderPartial("YellowWidget"); %>
</asp:Content>

<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
    <ul class="SectionLinks">
        <li><%: Html.ActionLink(Html.Term("TicketDetails", "Ticket Details"), "Edit", new { id = Model.SupportTicketNumber })%></li>
        <li><%: Html.ActionLink(Html.Term("TicketHistory", "Ticket History"), "History", new { id = Model.SupportTicketNumber }, new { @class = "selected" })%></li>
    </ul>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <%: Html.ActionLink(Html.Term("BroweTickets", "Browse Tickets"), "Index", "Landing")%>
    &gt;
    <%: Html.ActionLink(Html.Term("EditTicketDetails", "Edit Ticket Details"), "Edit", new { id = Model.SupportTicketNumber })%>
    &gt;
    <%: Html.Term("TicketHistory", "Ticket History")%>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2><%: Html.Term("TicketHistory", "Ticket History")%></h2>
    </div>
    <% Html.RenderPartial("AuditHistoryGrid", Model.AuditHistoryGridModel);%> 
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="RightContent" runat="server">
    <td class="CoreRightColumn">
        <% Html.RenderPartial("Notes"); %>
    </td>
</asp:Content>
