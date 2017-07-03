<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Support/Views/Shared/Support.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
    <ul class="SectionLinks">
        <li><a href="<%= ResolveUrl("~/Support/Admin") %>"><span>Admin Dashboard</span></a></li>
        <li><a class="selected" href="<%= ResolveUrl("~/Support/Admin/Prioritize") %>"><span>
            Set Priorities</span></a></li>
    </ul>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("TicketAdministration","Ticket Administration") %></h2>
    </div>
    <!-- Grid Filters -->
    <div class="UI-lightBg brdrAll GridFilters" id="paginatedGridFilters">
        <div class="FL FilterSet">
            <div class="FL">
                <label>
                    Status:</label>
                <select class="Filter" id="activeSelectFilter">
                    <option value="">Assigned</option>
                    <option value="true">Closed</option>
                    <option value="false">Inactive</option>
                </select>
            </div>
            <div class="FL">
                <label>
                    Priority:</label>
                <select class="Filter" id="Select1">
                    <option value="">Urgent</option>
                    <option value="true">Medium</option>
                    <option value="false">Low</option>
                </select>
            </div>
            <div class="FL">
                <label>
                    Date Range:</label>
                <input type="text" class="TextInput DatePicker fromDate" />
                -
                <input type="text" class="TextInput DatePicker toDate" />
            </div>
            <div class="FL">
                <label>
                    Ticket Number:</label>
                <input type="text" class="TextInput searchTicketNumber" />
            </div>
            <div class="FL">
                <label>
                    Account:</label>
                <input type="text" class="TextInput accountSearch" />
            </div>
            <div class="FL">
                <label>
                    Assigned to:</label>
                <input type="text" class="TextInput assignedTo disabled" value="Me (3433)" readonly="readonly" />
            </div>
        </div>
        <span class="clr"></span>
    </div>
    sortable list?
</asp:Content>
