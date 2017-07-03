<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/GeneralLedger/Views/Shared/TicketPaymentManagement.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

<h2>===> Implementar Pantalla BrowseTickets <br />
    ===> BR - CC - 010 Generación de boletos de pago 
</h2>
<br />
<a href="<%= ResolveUrl("~/GeneralLedger/PaymentTickets/TicketDetails/31891") %>">@TicketNumber 31891</a>
===> BR-CC-013 – Gestión de cuenta corriente manual
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>



<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
