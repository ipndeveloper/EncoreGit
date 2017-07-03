<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Reports/Views/Shared/Reports.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.ReportCategory>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h3><%= Model.CategoryName %></h3>
    <% foreach (Report report in Model.ListReports()) %>
    <% { %>
            <div id="ReportContainer">
                <a href="#" onclick='window.open ("<%= report.Url %>",
"Report","menubar=0,location=0,toolbar=0,status=0,resizable=1,width=1400,height=1000");'><h3><%= report.Name %></h3>
                
                <span class="UI-icon icon-reporting"></span></a>
            	<p><%= report.Description %></p>
            </div>
    <% } %>
</asp:Content>

    