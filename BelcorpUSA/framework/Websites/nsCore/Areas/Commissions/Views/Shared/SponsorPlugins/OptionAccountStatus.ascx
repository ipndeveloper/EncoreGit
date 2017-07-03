<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>
 <% List<SponsorDataAccountStatus> cantstatus = ViewData["spGetRulePerStatus"] as List<SponsorDataAccountStatus>;          
   string cantStatu = string.Empty;
   if (cantstatus != null){cantStatu = "1";}else{cantStatu = "0";}
    %>
<script type="text/javascript">
    $(function () { 
        if (<%=cantStatu %> > 0) {
            $('#acctRestrictY').attr('checked', true).trigger('change');
                     
        } else {
            $('#acctRestrictN').attr('checked', true).trigger('change');
        }
        $('#accounts a.checkAllOptions').toggle(function () {
            $(this).text('<%=Html.Term("PromotionOptions_UncheckAllLink", "uncheck all")%>');
            $(this).closest('div').find(':checkbox').attr('checked', 'checked');
        }, function () {
            $(this).text('<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%>');
            $(this).closest('div').find(':checkbox').removeAttr('checked');
        });
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
				<%=Html.Term("AccountStatusOptions_NoLabel", "No")%></label>
		</span><span>
			<input type="radio" value="yes" name="acctRestrict" id="acctRestrictY" />
			<label for="acctRestrictY">
				<%=Html.Term("AccountStatusOptions_YesLabel", "Yes")%></label>
		</span>
		
		<div class="UI-lightBg hiddenPanel pad10 overflow" id="AccountSelection">
			<span class="lawyer">
				<%=Html.Term("AccountStatusOptions_RestrictToAccountStatus", "Only checked accounts will receive the NEW BA.")%>
			</span>
				</span><a class="FR checkAllOptions" href="javascript:void();">
				<%=Html.Term("PromotionOptions_CheckAllLink", "check all")%></a> <span class="clr">
			</span>
			<ul class="flatList listNav">
            
				<% foreach (var items in ViewData["SponsorStatusAccounts"] as List<SponsorDataAccountStatus>)                   
                   {
                %>
				<li>
                <% var RulePerStatus = ViewData["spGetRulePerStatus"] as List<SponsorDataAccountStatus>;
                   var isTypeChecked = string.Empty;
                   if (RulePerStatus != null)
                   {
                       isTypeChecked = RulePerStatus.Any(x => x.AccountStatusID == items.AccountStatusID) ? "checked=\"checked\"" : string.Empty;
                   }
                   else {
                       isTypeChecked = string.Empty;
                   }
                %>
					<input type="checkbox" class="accountStatus" value="<%=items.AccountStatusID %>" name="checkacount[]" <%=isTypeChecked%> />
					<label><%=items.Name %></label>
				</li>
				<%                                       
                   }
                %>
			</ul>
		</div>
	</div>
</div>