<!-- Vista: Formulario preparar los datos para el manejo de operaciones de pasarela de pago con PayPal
    Proyecto: nscore
    Author: Juan Morales Olivares - CSTI * Año: 2016
-->

<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    <% Html.RenderPartial("PreparePaymentGate"); %>
</asp:Content>
