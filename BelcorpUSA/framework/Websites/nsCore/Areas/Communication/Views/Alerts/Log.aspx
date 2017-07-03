<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>Alerts Log</h2>
    </div>

    <% Html.PaginatedGrid("~/Communication/GetAlertEntries")
            .AddColumn("Date Created", "Created", true, true, Constants.SortDirection.Ascending)
            .AddColumn("Date Processed", "Processed", true)
            .AddColumn("Name", "Name", true)
            .AddColumn("Alert Trigger Name", "AlertTriggerName", true)
            .AddInputFilter("Start Date","StartDate",DateTime.Now.AddMonths(-1).ToShortDateString(), true)
            .AddInputFilter("End Date", "EndDate", DateTime.Now.ToShortDateString(), true)
            .Render(); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
