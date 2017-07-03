<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.HasBAStatusIDs ? "#restrictNewBAY" : "#restrictNewBAN";  
	%>
	$(function () { 
		$('<%= radioID %>').attr('checked', 'checked').trigger('change'); 
	});
</script>
<div class="pad5 promotionOption restrictOrderTypes">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="ordersRestrict">
			<%=Html.Term("PromotionOptions_RestrictNewBAsOption", "Restrict to New BA?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="newBAs">
		<span>
			<input type="radio" value="no" name="newBAsRestrict" id="restrictNewBAN" />
			<label for="restrictNewBAN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="newBAsRestrict" id="restrictNewBAY" />
			<label for="restrictNewBAY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<div <%= Model.HasBAStatusIDs?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="NewBASelection">
			<span class="lawyer">
				<%=Html.Term("PromotionOptions_RestrictNewBAsTip", "Only checked accounts with new status will receive the promotion.")%>
			</span>
            <table class="DataGrid" width="100%">
				<thead>
					<tr class="GridColHead Alt">
						<th>
							<%=Html.Term("PromotionOptions_NewBAStatus", "New BA Status") %>
						</th>
					</tr>
				</thead>
				<tbody>
                <% int contador = 0;
                foreach (AccountConsistencyStatus accountConsistencyStatus in SmallCollectionCache.Instance.AccountConsistencyStatuses)
	            { %>
                <tr <%= contador % 2 == 0 ? "class='Alt'" : "" %>>
                    <td>
                        <input type="checkbox" class="newBA" value="<%= accountConsistencyStatus.AccountConsistencyStatusID %>" <%= Model.NewBAStatusIDs.Contains(accountConsistencyStatus.AccountConsistencyStatusID) ? "checked='checked'" : "" %> />
					    <label><%= accountConsistencyStatus.GetTerm()%></label>
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
