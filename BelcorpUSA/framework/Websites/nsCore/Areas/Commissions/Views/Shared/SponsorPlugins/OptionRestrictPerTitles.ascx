<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<%
    var CantGetTitlesPaids = ViewData["spGetTitlesPaids"] as List<SponsorDataTitleType>;
    var CantGetTitlesRecognizeds = ViewData["spGetTitlesRecognizeds"] as List<SponsorDataTitleType>;
    string canttitles = "";
    if ((CantGetTitlesPaids.Count != 0) ||(CantGetTitlesRecognizeds.Count != 0))
    {
        canttitles = "1";
    }
    else {
        canttitles = "0";
    }
%>
<script type="text/javascript">
    $(function () {
        

        if (<%=canttitles %> > 0) {
            $('#titleRestrictY').attr('checked', 'checked').trigger('change');
        } else {
            $('#titleRestrictN').attr('checked', 'checked').trigger('change');
        }

    });
</script>

<h3 class="UI-lightBg mt10 pad10">
	<%= Html.Term("Promotionds_DefineRewadrdsHeading", "Rules for Sponsorship Automatically") %>:</h3>
<div class="pad5 promotionOption couponCode">
	<div class="FL optionHelpIcons">
    </div>
    <div id="DefineRewards">
        <div class="FLabel">
		    <label for="titleRestrict">
			    <%=Html.Term("RestrisctPerTitlesOptions_RestrisctPerTitlesOption", "Restrisct Per Titles?")%></label>
	    </div>
	    <div rel="isYes" class="hasPanel" id="accounts">
        
		    <span>
			<input type="radio" value="no" name="titleRestrict" id="titleRestrictN" />
			<label for="titleRestrictN">
				<%=Html.Term("RestrisctPerTitlesOptions_NoLabel", "No")%></label>
		    </span>
            <span>
			    <input type="radio" value="yes" name="titleRestrict" id="titleRestrictY" />
			    <label for="titleRestrictY">
				    <%=Html.Term("RestrisctPerTitlesOptions_YesLabel", "Yes")%></label>
		    </span>

            <div  class="UI-lightBg hiddenPanel pad10 overflow" id="titleSelection">
           <span class="lawyer">
				<%=Html.Term("RulesOptions_RestrictToPerTitles", "Only checked accounts will receive the new BA.")%>
			</span>
			<% 
				var service = NetSteps.Encore.Core.IoC.Create.New<NetSteps.Data.Common.Services.ITitleService>();
				var titles = service.GetTitles();

                Dictionary<string, string> dcTitulos = TitleExtensions.ListTitles();
                
				var accountTypes = SmallCollectionCache.Instance.AccountTypes;
				int numRows = Math.Max(titles.Count(), accountTypes.Count);
			%>
			<table class="DataGrid" width="100%" >
				<thead>
					<tr class="GridColHead Alt">                      
						<th>						
                             <label><%=Html.Term("RestrisctPerTitlesOptions_PaidTitle", "Paid As Title")%></label> 
						</th>   
						<th>
							<%=Html.Term("RestrisctOptions_RecognizedTitle", "Recognized Title")%>
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
                            { 
                        %>
						<td>
                        <% 
                                var isTypeCheckedTitlesPaids = CantGetTitlesPaids.Any(x => x.TitleID == title.TitleID) ? "checked=\"checked\"" : string.Empty;              
                        %> 
							<input type="checkbox" value="<%=title.TitleID %>" class="titles" <%=isTypeCheckedTitlesPaids%> id="1"/>
							<%= Html.Term(title.TermName, dcTitulos[Convert.ToString(title.TitleID)])%>
						</td>
						<td>
                        <% 
                                var isTypeCheckedTitlesRecognizeds = CantGetTitlesRecognizeds.Any(x => x.TitleID == title.TitleID) ? "checked=\"checked\"" : string.Empty;              
                        %>
							<input type="checkbox" value="<%=title.TitleID %>" class="titles" <%=isTypeCheckedTitlesRecognizeds%>  id="2"/>
							<%= Html.Term(title.TermName, dcTitulos[Convert.ToString(title.TitleID)])%>
						</td>
						<% } %>
                   </tr>
                  <% } %>                    
				</tbody>
			</table>
		</div>
	    </div>
    </div>
    </div>