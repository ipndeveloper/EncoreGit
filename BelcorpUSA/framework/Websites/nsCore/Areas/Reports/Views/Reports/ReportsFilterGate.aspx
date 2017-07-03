<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reports/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">


   <% Html.RenderPartial("ReportsFilter"); %>

</asp:Content>
