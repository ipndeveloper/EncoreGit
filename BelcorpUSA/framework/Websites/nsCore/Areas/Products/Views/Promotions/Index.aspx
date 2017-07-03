<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Promotions/Promotions.Master"
	Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> >
			<%= Html.Term("Promotions") %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2><%= Html.Term("Promotions") %></h2>
	</div>
	<div>
		<%if (TempData["SavedPromotion"] != null)
	{ %>
		<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous;
			-moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0;
			border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold;
			margin-bottom: 10px; padding: 7px;">
			<div style="color: #385E0F; display: block;">
				<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("PromotionSaved", "Promotion saved successfully!") %></div>
		</div>
		<%} %>
		<% Html.PaginatedGrid("~/Products/Promotions/Get")
            .AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") }, { "true", Html.Term("Active") }, { "false", Html.Term("Inactive") } })
            .AddColumn(Html.Term("PromotionID", "Promotion ID"), "PromotionID", true)
			.AddColumn(Html.Term("Name"), "Description", true, true, NetSteps.Common.Constants.SortDirection.Ascending)
            .AddColumn(Html.Term("StartDate", "Start Date"), "StartDate", true)
			.AddColumn(Html.Term("EndDate", "End Date"), "EndDate", true)
			.AddColumn(Html.Term("Status"), "PromotionStatusTypeID", true)
            .AddColumn(Html.Term("PromotionType", "Type"), "PromotionKind", true)
			.CanDelete("~/Products/Promotions/DeletePromotions")
			.CanChangeStatus(true, true, "~/Products/Promotions/ChangePromotionStatus")
            .ClickEntireRow()
			.Render(); %>
	</div>
</asp:Content>
