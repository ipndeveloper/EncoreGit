<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master"
	Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Commissions.Models.CommissionPlanProfileModel>" %>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Commissions") %>">
		<%= Html.Term("CommissionRun_Commissions", "Commissions")%></a> > <%= Html.Term("CommissionRun_"+Model.Plan.PlanCode, Model.Plan.PlanCode) %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<script type="text/javascript">
		
		//we need to always update every planType
		//maybe later we will put in logic to stop refreshing at some point.
		//Maybe put up a loader when we are actually making the ajax call for the refresh.
		setInterval("refreshAllTypes()", 5000);

		function toggleRefresh()
		{
			var refreshBool = $('#RefreshPlans');
			var refreshButton = $('#ToggleRefreshButton');
			//Currently Enabled, clicking means we want to disable
			if(refreshBool.val() == 'true')
			{
				refreshBool.val(false);
				refreshButton.text('<%=Html.Term("CommissionRun_EnableRefresh", "Enable Refresh") %>')
			}
			else
			{
				refreshBool.val(true);
				refreshButton.text('<%=Html.Term("CommissionRun_DisableRefresh", "Disable Refresh") %>')
			}
			
		}

		function refreshAllTypes()
		{
			if($('#RefreshPlans').val() == 'true'){
				<%foreach(var job in Model.Jobs) {%>
					tryRefreshOutput('<%=job.JobName %>', <%=job.CommissionRunTypeID %>);
				<%}%>
			}
		}

		function startJob(jobName, commissionRunTypeID) {
			var data = { jobName: jobName };
			NS.post({
			url: '<%= ResolveUrl("~/Commissions/Job/StartJob") %>', 
			data: data, 
			success: function (response) {
				if(response == "True"){
					hideJobErrorMessage(commissionRunTypeID);
					//clear out the output span
					var outputSpan = $('span#RunTypeOutput[runTypeID=\''+commissionRunTypeID+'\']');
					outputSpan.html('')
					//give the output span the data that was returned from the job output
					tryRefreshOutput(jobName, commissionRunTypeID);
				}
				else{
					$('#JobErrorMessage_'+commissionRunTypeID).show();
					$('#JobErrorMessage_'+commissionRunTypeID).html("Job already running.")
				}
			}});
		}

		function tryRefreshOutput(jobName, commissionRunTypeID) {
			//call isJobRunning for all tyoes on page.
			//IfJobIsRunning call refresshOutput for the given type.
			var data = { jobName: jobName };
			NS.post({
				url: '<%= ResolveUrl("~/Commissions/Job/IsJobRunning") %>', 
				data: data, 
				showLoading: $('div#LoadingIcon_'+commissionRunTypeID),
				success: function (response) {
				if (response == "True") {
					disableRunJob(commissionRunTypeID);
				}
				else{
					//make the job runnable
					enableRunJob(commissionRunTypeID);
				}
				//Make sure the code knows that we need to keep refreshing this section.
				//Still display the output?
				refreshOutput(jobName, commissionRunTypeID);
			}});
		}

		function refreshOutput(jobName, commissionRunTypeID) {
			var data = { jobName: jobName };
			
			var outputSpan = $('span#RunTypeOutput[runTypeID=\''+commissionRunTypeID+'\']');

			NS.post({
			url: '<%= ResolveUrl("~/Commissions/Job/GetJobOutput") %>', 
			data: data, 
			showLoading: $('div#LoadingIcon_'+commissionRunTypeID),
			success: function (response) {
				//This should return a built partial view to replace the output section with
				var outputSpan = $('span#RunTypeOutput[runtypeid=\''+commissionRunTypeID+'\']');

				$(outputSpan).html(response);
				
				var toggleButton = $('#ToggleOutputButton_'+commissionRunTypeID);

				var show = toggleButton.text() == '<%=Html.Term("CommissionRun_Less", "LESS") %>';
				showOutput(show, commissionRunTypeID);
				hideJobErrorMessage(commissionRunTypeID);
			}});
		}

		function enableRunJob(commissionRunTypeID) {
			//Find the button
			$('#RunTypeButton_'+commissionRunTypeID).first().removeAttr("disabled");
		}

		function disableRunJob(commissionRunTypeID) {
			$('#RunTypeButton_'+commissionRunTypeID).first().attr("disabled", "disabled");
		}

		function hideJobErrorMessage(commissionRunTypeID)
		{
			$('#JobErrorMessage_'+commissionRunTypeID).hide();
			$('#JobErrorMessage_'+commissionRunTypeID).html("");
		}

		function toggleOutput(commissionRunTypeID)
		{
			
			
			var toggleButton = $('#ToggleOutputButton_'+commissionRunTypeID);

			var show = toggleButton.text() == '<%=Html.Term("CommissionRun_More", "MORE") %>';

			showOutput(show,commissionRunTypeID);

			if(show)
			{
				toggleButton.text('<%=Html.Term("CommissionRun_Less", "LESS") %>');
			}
			else
			{
				toggleButton.text('<%=Html.Term("CommissionRun_More", "MORE") %>');
			}
		}

		function showOutput(show, commissionRunTypeID)
		{
		var pChildren = $('span#RunTypeOutput[runtypeid=\''+commissionRunTypeID+'\'] p');
		//Loop through the children and every child that has a row num greater than 0 or 1 after commissions fixes the blank P row gets a hide
			for (var i = 0; i < pChildren.length; i++) {
				var child = $(pChildren[i]);
				if(parseInt(child.attr("row")) > 4)
				{
					if(show)
					{
						child.show();
					}
					else
					{
						child.hide();
					}
				}
			}
		}

	</script>
	<div id="PlanTypeContent" class="Content">
		<input type="hidden" id="RefreshPlans" value="true" />
		<div class="SectionHeader"><h2><%=Html.Term("CommissionRun_PlanName",  "Plan Name") %>: <%=Model.Plan.Name %></h2></div>
		
		<div class="UI-lightBg mt10 mb10 pad10 brdrAll"><a id="ToggleRefreshButton" class="Button BigBlue" onclick="toggleRefresh();"><%=Html.Term("CommissionRun_DisableRefresh", "Disable Refresh") %></a> 
		
		<%=Html.Term("CommissionRun_UpdateMessage", "Page updates every 5 seconds.") %>
		</div>
		<%foreach (var job in Model.Jobs)
		{%>
			<%=Html.Partial("JobTypeDetails", job) %>
		<%} %>
	</div>
</asp:Content>
