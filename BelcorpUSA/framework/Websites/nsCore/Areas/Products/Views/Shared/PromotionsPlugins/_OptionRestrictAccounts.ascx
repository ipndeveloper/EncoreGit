<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.HasAccountTypes ? "#acctRestrictY" : "#acctRestrictN";  
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
			<%=Html.Term("PromotionOptions_RestrictAccountsOption", "Restrict to Accounts?")%></label>
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
		<div <%= Model.HasAccountTypes?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="AccountSelection">
			<span class="lawyer">
				<%=Html.Term("PromotionOptions_RestrictToAccountTip", "Only checked accounts will receive the promotion.")%>
			</span>
			<% 
				var service = NetSteps.Encore.Core.IoC.Create.New<NetSteps.Data.Common.Services.ITitleService>();
				var titles = service.GetTitles();
				var accountTypes = SmallCollectionCache.Instance.AccountTypes;
				int numRows = Math.Max(titles.Count(), accountTypes.Count);
			%>
			<table class="DataGrid" width="100%">
				<thead>
					<tr class="GridColHead Alt">
						<th>
							<%=Html.Term("PromotionOptions_PaidAsTitle", "Paid As Title") %>
						</th>
						<th>
							<%=Html.Term("PromotionOptions_RecognizedTitle", "Recognized Title") %>
						</th>
						<th>
							<%=Html.Term("PromotionOptions_AccountTypes", "Account Type") %>
						</th>
					</tr>
				</thead>
				<tbody>
					<% for (int i = 0; i < numRows; i++)
		{
			var title = titles.ElementAtOrDefault(i);
			var accountType = accountTypes.ElementAtOrDefault(i);
					%>
					<tr <%= i % 2 == 0 ? "class='Alt'" : "" %>>
						<%if (title != null)
		{ %>
						<td>
							<input type="checkbox" value="<%=title.TitleID %>" class="paidAs" <%= Model.PaidAsTitleIDs.Contains(title.TitleID) ? "checked='checked'" : "" %> />
							<%= Html.Term(title.TermName)%>
						</td>
						<td>
							<input type="checkbox" value="<%=title.TitleID %>" class="recognized" <%= Model.RecognizedTitleIDs.Contains(title.TitleID) ? "checked='checked'" : "" %> />
							<%= Html.Term(title.TermName)%>
						</td>
						<% }
		else
		{ %>
						<td />
						<td />
						<%}
		if (accountType != null)
		{%>
						<td>
							<input type="checkbox" value="<%= accountType.AccountTypeID %>" class="accountType"
								<%= Model.AccountTypeIDs.Contains(accountType.AccountTypeID) ? "checked='checked'" : "" %> /><%= accountType.GetTerm()%>
						</td>
						<%}
		else
		{ %>
						<td />
						<%} %>
					</tr>
					<%} %>
				</tbody>
			</table>
		</div>
	</div>
</div>
