<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.HasActivityStatusIDs ? "#restrictActivityStatusY" : "#restrictActivityStatusN";  
	%>
	$(function () { 
		$('<%= radioID %>').attr('checked', 'checked').trigger('change'); 
	});
</script>
<div class="pad5 promotionOption restrictActivityStatuses">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="ordersRestrict">
			<%=Html.Term("PromotionOptions_RestrictActivityStatusesOption", "Restrict to Activity?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="activityStatuses">
		<span>
			<input type="radio" value="no" name="activityStatusesRestrict" id="restrictActivityStatusN" />
			<label for="restrictActivityStatusN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="activityStatusesRestrict" id="restrictActivityStatusY" />
			<label for="restrictActivityStatusY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<div <%= Model.HasActivityStatusIDs?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="ActivityStatusSelection">
			<span class="lawyer">
				<%=Html.Term("PromotionOptions_RestrictActivityStatusesTip", "Only accounts with the activity status will receive the promotion.")%>
			</span>
            <table class="DataGrid" width="100%">
				<thead>
					<tr class="GridColHead Alt">
						<th>
							<%=Html.Term("PromotionOptions_ActivityStatus", "Activity Status") %>
						</th>
					</tr>
				</thead>
				<tbody>
                <% int contador = 0;
                foreach (ActivityStatus activityStatus in SmallCollectionCache.Instance.ActivityStatuses)
                { %>
                <tr <%= contador % 2 == 0 ? "class='Alt'" : "" %>>
                    <td>
                        <input type="checkbox" class="activityStatus" value="<%= activityStatus.ActivityStatusId %>" <%= Model.ActivityStatusIDs.Contains(activityStatus.ActivityStatusId) ? "checked='checked'" : "" %> />
					    <label><%= activityStatus.GetTerm()%></label>
                    </td>
                </tr>
				<%
                    contador++;
                } %>
                </tbody>
            </table>
		</div>
	</div>
</div>
