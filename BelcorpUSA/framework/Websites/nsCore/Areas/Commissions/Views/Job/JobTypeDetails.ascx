<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Commissions.Models.JobDetailsModel>" %>
<script type="text/javascript">
	//This partial is only responsible for a single section refresh.
</script>
<div typeID="<%=Model.CommissionRunTypeID %>" jobName="<%=Model.JobName %>" class="commissionsRunnerMenu FL">
	<h3 class="TypeName"><%= Model.CommissionRunType.Name%></h3>

	<div class="UI-lightBg mt10 mb10 pad10 brdrAll">
		<a id="<%=String.Format("RunTypeButton_{0}", Model.CommissionRunTypeID) %>" class="Button BigBlue Enabled" name="<%=Model.JobName %>" onclick="startJob('<%=Model.JobName %>', <%=Model.CommissionRunTypeID %>);" ><%=Html.Term("CommissionRun_StartJob", "Start Job") %></a>
	    <div class="FR"><a id="ToggleOutputButton_<%= Model.CommissionRunTypeID %>" class="Button" onclick="toggleOutput(<%= Model.CommissionRunTypeID%>)"><%=Html.Term("CommissionRun_Less", "LESS") %></a></div> 
    </div>	
	<br />
	<span id="<%=String.Format("JobErrorMessage_{0}", Model.CommissionRunTypeID) %>"></span>
	
	<div id="LoadingIcon_<%= Model.CommissionRunTypeID%>" style="width:25px;height:25px;display:none;"></div>
	<span id="RunTypeOutput" runtypeid="<%=Model.CommissionRunTypeID %>">
	<%if (Model.JobOutput != null)
	 { %>
		
		<%Html.RenderPartial("JobTypeOutput", Model.JobOutput); %>
	<%} %>
	</span>
</div>
