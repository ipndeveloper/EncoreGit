<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Content/CSS/fullcalendar.css") %>" />
	<style type="text/css">
		#calendar { width: 100%; }
	</style>

	<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/fullcalendar.min.js") %>"></script>

	<script type="text/javascript">
	    $(function () {
	        var monthSelect = $('<select id="fc-month" style="font-size:18px;color:#000000;" />'),
				yearSelect = $('<select id="fc-year" style="font-size:18px;color:#000000;" />'),
				today = new Date(),

			setYears = function (year) {
			    yearSelect.empty();
			    for (var i = year - 10; i <= year + 10; i++) {
			        yearSelect.append('<option value="' + i + '">' + i + '</option>');
			    }
			    yearSelect.val(year);
			},

			setAddEventHref = function (startDate) {
			    $('#calendar .fc-content tbody .fc-state-default').each(function () {
			        var url = '<%= ResolveUrl("~/Sites/CalendarEvents/Edit") %>?startDate=' + (startDate.getMonth() + 1) + '-' + startDate.getDate() + '-' + startDate.getFullYear();
			        if ($(this).find('.addEvent').length) {
			            $(this).find('.addEvent').attr('href', url);
			        } else {
			            $(this).prepend('<a class="addEvent" href="' + url + '"><img src="<%= ResolveUrl("~/Content/Images/add.gif") %>" alt="+" /> <%= Html.Term("AddaNewEvent", "Add a new event") %></a>');
			        }
			        startDate.setDate(startDate.getDate() + 1);
			    });
			};

	        monthSelect.empty();

	        rebuildMonths = function () {
	            $.each(['<%= CoreContext.GetMonthName(1) %>', '<%= CoreContext.GetMonthName(2) %>', '<%= CoreContext.GetMonthName(3) %>', '<%= CoreContext.GetMonthName(4) %>', '<%= CoreContext.GetMonthName(5) %>', '<%= CoreContext.GetMonthName(6) %>', '<%= CoreContext.GetMonthName(7) %>', '<%= CoreContext.GetMonthName(8) %>', '<%= CoreContext.GetMonthName(9) %>', '<%= CoreContext.GetMonthName(10) %>', '<%= CoreContext.GetMonthName(11) %>', '<%= CoreContext.GetMonthName(12) %>'], function (i, month) {
	                monthSelect.append('<option value="' + i + '">' + month + '</option>');
	            });
	        };
	        monthSelect.val(today.getMonth());
	        setYears(today.getFullYear());

	        $('#calendar').fullCalendar({
	            draggable: false,
	            editable: false,
	            events: '<%= ResolveUrl("~/Sites/CalendarEvents/Get") %>',
	            viewDisplay: function (view) {
	                if (!$('#fc-month').length) {
	                    if (!monthSelect.val()) {
	                        rebuildMonths();
                        }
	                    $('.fc-header-title', this).empty().append(monthSelect.val(view.start.getMonth()).change(function () {
	                        $('#calendar').fullCalendar('gotoDate', new Date(parseInt(yearSelect.val()), parseInt(monthSelect.val()), 1));
	                    })).append(yearSelect.change(function () {
	                        $('#calendar').fullCalendar('gotoDate', new Date(parseInt(yearSelect.val()), parseInt(monthSelect.val()), 1));
	                    }));

	                    setYears(view.start.getFullYear());

	                    //setAddEventHref(view.visStart);
	                }
	            }
	        });
	    });

	</script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>">
		<%= Html.Term("Sites") %></a> >
	<%= Html.Term("CalendarEvents", "Calendar Events") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("CalendarEvents", "Calendar Events") %></h2>
		<a href="<%= ResolveUrl("~/Sites/CalendarEvents/Edit") %>">
			<%= Html.Term("AddNewEvent", "Add a New Event") %></a>
	</div>
	<%if (TempData["SavedEvent"] != null)
   { %>
	<div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #CAFF70 none repeat scroll 0 0; border: 1px solid #385E0F; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
		<div style="color: #385E0F; display: block;">
			<img alt="" src="<%= ResolveUrl("~/Content/Images/accept-trans.png") %>" />&nbsp;<%= Html.Term("EventSaved", "Event saved successfully!") %></div>
	</div>
	<%} %>
	<img class="Loading FR" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." style="margin-right: 125px;" />
	<div id="calendar">
	</div>
</asp:Content>
