<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.AutoshipSchedule>>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Admin") %>"><%= Html.Term("Admin") %></a> > <%= Html.Term("AutoshipSchedules", "Autoship Schedules")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("AutoshipSchedules", "Autoship Schedules")%>
		</h2>
		<a href="<%= ResolveUrl("~/Admin/AutoshipSchedules/Edit") %>"><%= Html.Term("AddNewSchedule", "Add new schedule") %></a>
	</div>
	<%if (TempData["SavedSchedule"] != null)
   { %>
	<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0; border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
		<div style="color: #385E0F; display: block;">
			<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("ScheduleSaved", "Schedule saved successfully!") %></div>
	</div>
	<%} %>
	<table cellspacing="0" cellpadding="0" width="100%;" class="DataGrid">
		<thead>
			<tr class="GridColHead">
				<th>
					<%= Html.Term("Name") %></th>
				<th>
					<%= Html.Term("Interval") %></th>
				<th>
					<%= Html.Term("RunDays", "Run Day(s)") %></th>
				<th>
					<%= Html.Term("Status") %></th>
			</tr>
		</thead>
		<tbody>
			<%if (Model.Count == 0)
	 {
		 Response.Write("<tr><td colspan=\"9\">" + Html.Term("ThereAreNoAutoshipSchedules", "There are no autoship schedules.") + "</td></tr>");
	 }
	 else
	 {
		 int count = 0;
		 foreach (AutoshipSchedule schedule in Model)
		 { %>
			<tr class="<%= count % 2 == 0 ? "GridRow" : "GridRowAlt" %>">
				<td>
					<a href="<%= ResolveUrl("~/Admin/AutoshipSchedules/Edit/") + schedule.AutoshipScheduleID %>">
						<%= schedule.GetTerm() %>
					</a>
				</td>
				<td>
					<%= SmallCollectionCache.Instance.IntervalTypes.GetById(schedule.IntervalTypeID).GetTerm() %>
				</td>
				<td>
					<%= schedule.AutoshipScheduleDays.OrderBy(asd => asd.Day).Select(asd => asd.Day.ToString()).Join(", ") %>
				</td>
				<td>
					<%= schedule.Active ? Html.Term("Active") : Html.Term("Inactive") %>
				</td>
			</tr>
			<%++count;
		 }
	 } %>
		</tbody>
	</table>
</asp:Content>