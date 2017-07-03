<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<OrderRules.Service.DTO.RuleValidationsDTO>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.AccountTypeIDs.Count > 0 ? "#acctRestrictY" : "#acctRestrictN";  
	%>
	$(function () { 
		$('<%= radioID %>').attr('checked', 'checked').trigger('change'); 
	});
</script>
<div class="pad5 promotionOption restrictAccounts">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="acctRestrict">
			<%=Html.Term("RulesOptions_RestrictAccountTypesOption", "Restrict to Accounts?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="accounts">
		<span>
			<input type="radio" value="no" name="acctRestrict" id="acctRestrictN" />
			<label for="acctRestrictN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="acctRestrict" id="acctRestrictY" />
			<label for="acctRestrictY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<!-- account type selection panel -->
		<div <%= Model.AccountTypeIDs.Count > 0 ?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="AccountSelection">
			<span class="lawyer">
				<%=Html.Term("RulesOptions_RestrictToAccountTip", "Only checked accounts will contain the rule.")%>
			</span>
			<% 
				var accountTypes = SmallCollectionCache.Instance.AccountTypes;
                int numRows = accountTypes.Count;
			%>
			<table class="DataGrid" width="100%">
				<thead>
					<tr class="GridColHead Alt">
						<th>
							<%=Html.Term("PromotionOptions_AccountTypes", "Account Type") %>
						</th>
					</tr>
				</thead>
				<tbody>
					<% for (int i = 0; i < numRows; i++)
		{
			var accountType = accountTypes.ElementAtOrDefault(i);
					%>
					<tr <%= i % 2 == 0 ? "class='Alt'" : "" %>>						
                    <%
		if (accountType != null)
		{%>
						<td>
							<input type="checkbox" value="<%= accountType.AccountTypeID %>" class="accountType"
								<%= Model.AccountTypeIDs.Contains(accountType.AccountTypeID) ? "checked='checked'" : "" %> /><%= accountType.GetTerm()%>
						</td>
						<%}
         %>
					</tr>
					<%} %>
				</tbody>
			</table>
		</div>
	</div>
</div>
