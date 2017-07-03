<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Products.Models.Promotions.Interfaces.IStandardQualificationPromotionModel>" %>
<script type="text/javascript">
	<% 
	string radioID = Model.HasAccountIDs ? "#acctIDRestrictY" : "#acctIDRestrictN";  
	%>
		
    $(function () {
		$('.WaitWin').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' });
		$('<%= radioID %>').attr('checked', 'checked').trigger('change'); 
	    $('#paginatedGridAccountID .deleteButton').click(function () {
			$('#AccountIdGrid .selectRow:checked').each(function () {
				$(this).closest('tr').remove();
			});
			$('#AccountIdGrid').refreshTable();
		});
       
     	$('#accountIDTextSearch').watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
			$('.WaitWin:first').jqmShow();
	  	  }, defaultToFirst: false, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true, width: $('#txtSearch').outerWidth(true) + $('#btnGo').outerWidth() - (accountLanding ? 2 : 4)
		});


	});
</script>
<div class="pad5 promotionOption restrictAccounts">
	<div class="FL optionHelpIcons">
    </div>
	<div class="FLabel">
		<label for="acctIDRestrict">
			<%=Html.Term("PromotionOptions_RestrictAccountsOptionIDs", "Restrict to AccountsIDs?")%></label>
	</div>
	<div rel="isYes" class="hasPanel" id="accountIDs">
		<span>
			<input type="radio" value="no" name="acctRestrict" id="acctIDRestrictN" />
			<label for="acctIDRestrictN">
				<%=Html.Term("PromotionOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="acctRestrict" id="acctIDRestrictY" />
			<label for="acctIDRestrictY">
				<%=Html.Term("PromotionOptions_YesLabel", "Yes")%></label>
		</span>
		<!-- account type selection panel -->
		<div <%= Model.HasAccountIDs?"":"style=\"display: none;\"" %> class="UI-lightBg hiddenPanel pad10 overflow" id="AccountIDSelection">
			<span class="lawyer">
				<%=Html.Term("PromotionOptions_RestrictToAccountTip", "Only checked accounts will receive the promotion.")%>
			</span>
			<% 
                int numRows = Model.AccountIDs.Count();
            %>
            <div class="UI-secBg pad10 brdrYYNN">
								<div class="FL mr10">
									<%= Html.Term("Promotions_QuickProductAdd", "Quick Account Add")%>:<br />
									<input id="accountIDTextSearch" type="text" class="TextInput distributorSearch" 
										hiddenid="accountIDAdd" />
									<input type="hidden" value="" id="accountIDAdd" />
			</div>
           
           <div class="UI-mainBg icon-24 brdrAll brdr1 GridSelectOptions GridUtility" id="paginatedGridAccountID">
								<a class="deleteButton UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected">
								</span><span>
									<%=Html.Term("DeleteSelected", "Delete Selected") %></span></a>
			</div>
			<table width="100%" class="DataGrid" id="AccountIdGrid">
								<thead>
									<tr class="GridColHead">
										<th class="GridCheckBox">
											<input type="checkbox" id="rewardsCheckAll" />
										</th>
										<th>
											<%=Html.Term("ID")%>
										</th>
										<th>
											<%=Html.Term("AccountName")%>
										</th>
									</tr>
				                </thead>
				<tbody>
				<% for (int i = 0; i < numRows; i++) {

                        var account = Account.Load(Model.AccountIDs.ElementAtOrDefault(i));
                        
                        %>
                        <tr>
							<td>
								<input type="checkbox" class="selectRow" /><input type="hidden" class="accountID" value="<%= account.AccountID %>" />
							</td>
							<td>
								<%= account.AccountID%>
							</td>
							<td>
								<%= account.FullName%>
							</td>
						</tr>
                        <%
                        
                   }
                 %>
				</tbody>
			</table>
		</div>
	</div>
</div>


