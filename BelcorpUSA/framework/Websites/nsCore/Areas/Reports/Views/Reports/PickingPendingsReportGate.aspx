﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reports/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<h2>PickingPendings Report</h2>
<% Html.RenderPartial("PickingPendingsReport"); %>
</asp:Content>

