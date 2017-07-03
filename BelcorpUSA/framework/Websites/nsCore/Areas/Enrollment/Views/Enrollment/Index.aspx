<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Enrollment/Views/Shared/Enrollment.Master"
    Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <form>
    <% Html.RenderPartial(ViewBag.PartialViewName as string, ViewBag.PartialViewModel as object); %>
    </form>
</asp:Content>
