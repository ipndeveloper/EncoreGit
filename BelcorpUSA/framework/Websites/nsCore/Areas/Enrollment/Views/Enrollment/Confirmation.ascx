<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
<% var enrollmentContext = ViewBag.EnrollmentContext as NetSteps.Web.Mvc.Controls.Models.Enrollment.EnrollmentContext; %>

<script type="text/javascript">
	$(function () {
		window.letUnload = true;
		window.enrollmentAccountNumber = '<%= enrollmentContext.EnrollingAccount.AccountNumber %>';
	});
</script>
<div class="StepGutter">
    <div class="BigSuccess"></div>
</div>
<div class="StepBody">
<h4><%= Html.Term("FinishedEnrolling", "Congratulations! You have just finished enrolling {0}.", ViewData["FullName"]) %></h4>

<%= Html.Term("NewAccountNumber", "{0}'s new account number is {1}.", ViewData["FullName"], ViewData["AccountNumber"]) %><br />
<br />
<%= Html.Term("YouMayGoToTheAccountOverview", "You may now go to the {0}account overview{1}.", string.Format("<a href=\"{0}{1}\">", ResolveUrl("~/Accounts/Overview/Index/"), ViewData["AccountNumber"]), "</a>") %>
</div>
<span class="ClearAll"></span>