<%@ Page Title="" Language="C#"  MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master" 
    Inherits="System.Web.Mvc.ViewPage<dynamic>"
  %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">

    <%		
        var Edit = ViewBag.Edit;
        var Description ="";
        var Site = "";
        var SkillfulCalendarFirst = "S";
        var SkillfulRemainingCalendar = "S";
        var ModifiesDates = "";
        var ModifiesValues = "";
        var Observation = "";
        var RenegotiationID = Edit.RenegotiationConfigurationID;
       
        var FineAndInterestRule=0;
        
         if (Edit.FineAndInterestRules != null)
	    {
            FineAndInterestRule = int.Parse(Edit.FineAndInterestRules);
        }
             
        if (Edit.DescriptionRenegotiation != null)
	    {
            Description = Edit.DescriptionRenegotiation;
        }

        if (Edit.Site != null)
        {
            Site = Edit.Site;
        }

        if (Edit.SkillfulCalendarFirst != null )
        {
            SkillfulCalendarFirst = Edit.SkillfulCalendarFirst;
        }


        if (Edit.SkillfulRemainingCalendar != null )
        {
            SkillfulRemainingCalendar = Edit.SkillfulRemainingCalendar;
        }


        if (Edit.ModifiesDates != null )
        {
            ModifiesDates = Edit.ModifiesDates;
        }

        if (Edit.ModifiesValues != null )
        {
            ModifiesValues = Edit.ModifiesValues;
        }

        if (Edit.Observation != null )
        {
            Observation = Edit.Observation;
        }

       
		
	%>

		<h2>
			<%=  Html.Term("RenegotiationMethods", "Renegotiation Methods")%>
        </h2>        
        
	</div>
 

    <h3>Renegotiation Method</h3>
    <table id="renegotiationMethodTable" class="DataGrid FormGrid" width="90%">
		<tbody>
			<tr>
				<td class="FLabel">
					<%= Html.Term("DescriptionRenegotiation", "Description Renegotiation")%>:
				</td>
				<td>
                <input id="Text1" type="hidden"                     
                     value="<%= RenegotiationID %>"                
                    />
					<input id="DescriptionRenegotiation" type="text" class="clear required"
                     name= "<%= Html.JavascriptTerm("DescriptionRenegotiationInvalid", "Description Renegotiation invalid.") %>" 
                     value="<%= Description %>" size="60px"  maxlength="50"                   
                    />
                    <input type="hidden"  id="hdnRenegotiationID" value="<%= Edit.RenegotiationConfigurationID%>" />
				</td>
			</tr>
            <tr>
				<td class="FLabel">
					<%= Html.Term("SharesNumber", "Shares Number")%>:
				</td>
				
				<td>
					<input id="SharesNumber" type="text" class="clear required justNumbers"
                     name= "<%= Html.JavascriptTerm("SharesNumberInvalid", "Shares Number invalid.") %>" 
                     value="<%= Edit.SharesNumber==0?"": Edit.SharesNumber %>"    />				
				</td>
			</tr>
             <tr>
				<td class="FLabel">
					<%= Html.Term("Site", "Site")%>:
				</td>			
				<td> 
                
                     <select id="sSite" name ="Site" class="clear required justNumbers" >
                        <option value ="">Select a Site</option>
						<option value="G"  <%= Site=="G" ? "selected=\"selected\"" : "" %>>GMP</option>  
                        <option value="D" <%= Site=="D" ? "selected=\"selected\"" : "" %>>DWS</option>
                        <option value="A" <%= Site=="A" ? "selected=\"selected\"" : "" %>>Both</option>                     
					</select> 			
				</td>			
			</tr>
          
           <tr>
                           
                <td class="FLabel">
                    <a  id="btnOpenRulesDet"  href="javascript:void(0);" >
					<%= Html.Term("Fine And Interest Rules", "Fine and Interest Rules")%>:</a>
				</td>
				<td>
                <input type="hidden" id="hdnDisableRule" value="<%= Edit.DisabledFineAndInterestRules%>" />
					<select id="fineAndInterestRulesID"  class="clear required justNumbers <%= Edit.DisabledFineAndInterestRules?"Desa":"Hab"%>">
                        <option value ="">Select a Rule</option>
						<% foreach (var rules in NetSteps.Data.Entities.Repositories.CTERepository.BrowseRules())
						   {
						%>
						<option value="<%= rules.FineAndInterestRulesID %>" <%=FineAndInterestRule== rules.FineAndInterestRulesID  ? "selected=\"selected\"" : "" %> >
							<%= rules.Name%></option>
						<%      
						   } 
						%>
					</select>
				</td>
                                		
			</tr>
			
			
		<%--	<tr>
				<td class="FLabel">
					<%= Html.Term("DayExpiration", "Day Expiration")%>:
				</td>
				<td>
					<input id="DayExpiration" type="text" 
                     name= "<%= Html.JavascriptTerm("DayExpirationInvalid", "Day Expiration invalid.") %>" 
                     value="<%= Edit.DayExpiration  %>"    />				
				</td>
                                 
			</tr>--%>
			<tr>
				<td class="FLabel">
					<%= Html.Term("DayValidate", "Day Validate")%>:
				</td>
				<td>
					<input id="DayValidate" type="text" class="clear required numeric"
                     name= "<%= Html.JavascriptTerm("DayValidateInvalid", "Day Validate invalid.") %>" 
                     value="<%= Edit.DayValidate %>"    />				
				</td>
             </tr>

        
           <tr>
				<td class="FLabel">
					<%= Html.Term("FirstSharesday", "First Sharesday")%>:
				</td>
				<td>
					<input id="FirstSharesday" type="text" class="clear required justNumbers"
                     name= "<%= Html.JavascriptTerm("FirstSharesdayInvalid", "First Sharesday invalid.") %>" 
                     value="<%= Edit.FirstSharesday==0?"":Edit.FirstSharesday %>"    />				
				</td>
                </tr>

           <tr>
				<td class="FLabel">
					<%= Html.Term("SkillfulCalendarFirst", "Skill Ful Calendar First")%>:
				</td>
				<td>                    		
                    <label> <input type="radio" name="rbSkillfulCalendarFirst" value="N"   <%= SkillfulCalendarFirst=="N"? "checked=\"checked\"" : "" %>   /> <%= Html.Term("ValidSatSundHol", "Valid Saturdays, Sundays and holidays")%> </label>
                    <label> <input type="radio" name="rbSkillfulCalendarFirst" value="S"  <%= SkillfulCalendarFirst=="S"? "checked=\"checked\"" : "" %> /> <%= Html.Term("CalendarDays", "Calendar Days")%></label> 
				</td>
           </tr>

               <tr>
				<td class="FLabel">
					<%= Html.Term("SharesInterval", "Shares Interval")%>:
				</td>
				<td>
					<input id="SharesInterval" type="text" class="clear required justNumbers"
                     name= "<%= Html.JavascriptTerm("SharesIntervalInvalid", "Shares Interval invalid.") %>" 
                     value="<%= Edit.SharesInterval==0?"":Edit.SharesInterval %>"    />				
				</td>
                </tr>

                <tr>
				<td class="FLabel">
					<%= Html.Term("SkillfulRemainingCalendar", "Skill ful Remaining Calendar")%>:
				</td>
				<td>
              
                    <label> <input type="radio" name="rbSkillfulRemainingCalendar" value="N"   <%= SkillfulRemainingCalendar=="N"? "checked=\"checked\"" : "" %>   /> <%= Html.Term("ValidSatSundHol", "Valid Saturdays, Sundays and holidays")%> </label>
                    <label> <input type="radio" name="rbSkillfulRemainingCalendar" value="S"  <%= SkillfulRemainingCalendar=="S"? "checked=\"checked\"" : "" %> /> <%= Html.Term("CalendarDays", "Calendar Days")%></label> 

				</td>
                </tr>
                

               <tr>
				<td class="FLabel">
					<%= Html.Term("ModifiesDates", "Modifies Dates")%>:
				</td>
				<td>
					<input id="ModifiesDates" type="checkbox"                     
                     <%= ModifiesDates=="S"? "checked=\"checked\"" : "" %>
                      />					
				</td>
                </tr>

                <tr>
				<td class="FLabel">
					<%= Html.Term("ModifiesValues", "Modifies Values")%>:
				</td>

				<td>  

					<input id="ModifiesValues" type="checkbox"                   
                      <%= (ModifiesValues=="S")? "checked=\"checked\"" : "" %>
                      />				
				</td>
                </tr>

                <tr>
				<td class="FLabel">
					<%= Html.Term("Observation", "Observation")%>:
				</td>
				<td>
					<input id="Observation" type="text" size="120px"  maxlength="200"
                     value="<%= Observation %>"    />				
				</td>
                </tr>
		</tbody>
	</table>

    <br />
  

   


    <table id="RegisterTable" class="DataGrid FormGrid" width="100%">
		<tr>			
			<td>
				<p >
					<a id="btnSaveRenegotiation" class="Button BigBlue" href="javascript:void(0);">
						<%= Html.Term("Save", "Save")%></a>
				</p>
			</td>
		</tr>
    </table>
    
     <script type="text/javascript">

         function Ocultar() {
             $("#noteModal").css("display", "none");
         };

         $(function () {
          $('#noteModal').jqm({
                modal: false
            });

           

          $('#btnOpenRulesDet').click(function () {   
               var url = '<%= ResolveUrl("~/CTE/RenegotiationMethods/GetRulesDetailsModal") %>';
               var odata = JSON.stringify({ FineAndInterestRulesID: $('#fineAndInterestRulesID').val() });

               $.ajax({
                   data: odata,
                   url: url,
                   dataType: "json",
                   type: "POST",
                   contentType: "application/json",
                   success: function (response) {
                       var html = response.Items;
                           $("#tblinformacion").html(html)
//                           $('#noteModal').jqmShow();
                           $("#noteModal").css("display", "block");
                   },
                   error: function (error) {
                   }
               });
           });

             //Guardar
             $('#btnSaveRenegotiation').click(function () {
                 var t = $(this);
               
                 if ($('#renegotiationMethodTable').checkRequiredFields()) {
                     var data = {};

                     data['Renegotiation.RenegotiationConfigurationID'] = $('#hdnRenegotiationID').val();
                     data['Renegotiation.DescriptionRenegotiation'] = $('#DescriptionRenegotiation').val();
                     data['Renegotiation.SharesNumber'] = $('#SharesNumber').val();
                     data['Renegotiation.Site'] = $('#sSite').val();
                     data['Renegotiation.FineAndInterestRules'] = $('#fineAndInterestRulesID').val();
//                     data['Renegotiation.DayExpiration'] = $('#DayExpiration').val();
                     data['Renegotiation.DayValidate'] = $('#DayValidate').val();
                     data['Renegotiation.FirstSharesday'] = $('#FirstSharesday').val();
                     data['Renegotiation.SkillfulCalendarFirst'] = $("input[name=rbSkillfulCalendarFirst]:checked").val(); // $('#sSkillfulCalendarFirst').val();
                     data['Renegotiation.SharesInterval'] = $('#SharesInterval').val();
                     data['Renegotiation.SkillfulRemainingCalendar'] = $("input[name=rbSkillfulRemainingCalendar]:checked").val(); // $('#sSkillfulRemainingCalendar').val();
                     data['Renegotiation.ModifiesDates'] = $('#ModifiesDates').prop('checked') ? 'S' : 'N';
                     data['Renegotiation.ModifiesValues'] = $('#ModifiesValues').prop('checked') ? 'S' : 'N';
                     data['Renegotiation.Observation'] = $('#Observation').val();

                     t = $(this);


                     showLoading(t);
                     $.post('/CTE/RenegotiationMethods/SaveRenegotiation', data, function (response) {
                         if (response.result) {
                             showMessage("Renogotiation Method was saved!", false);
                             hideLoading(t);
                         } else {
                             showMessage(response.message, true);
                         }
                     });
                 }
             });
         });
    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<div id="noteModal" class="jqmWindow LModal" style="left: 510px; z-index: 3000; top: 100px;">
        <div class="mContent">
            <h2>
              <%= Html.Term("RuleDetails","Rules Details")%>
            </h2>
            <table id="campaignAction" class="FormTable" width="1230px">
    <tr id="valuePanel">          
            <td id="taskAdd"> 

            <table class="DataGrid" width="100%">

               <thead>
                        <tr class="GridColHead">
                           
                            <th>
                                <%= Html.Term("Negotiation", "Negotiation")%>
                            </th>                                                   
                            <th>
                                <%= Html.Term("OpeningDay", "OpeningDay")%>
                            </th> 
                            
                            <th>
                                <%= Html.Term("FinalDay", "FinalDay")%>
                            </th>                           
                            <th>
                                <%= Html.Term("FinePercentage", "FinePercentage")%>
                            </th>   
                            
                            <th>
                                <%= Html.Term("AppliedValue", "AppliedValue")%>
                            </th>                           
                            <th>
                                <%= Html.Term("Minimum Debt", "Minimum Debt")%>
                            </th>   

                            <th>
                                <%= Html.Term("InterestPercentage", "InterestPercentage")%>
                            </th>                           
                            <th>
                                <%= Html.Term("Applied Value", "Applied Value")%>
                            </th>   
                            
                            <th>
                                <%= Html.Term("Discount", "Discount")%>
                            </th>                           
                            <th>
                                <%= Html.Term("Applied Value", "Applied Value")%>
                            </th>   
                            
                                                        
                        </tr>
                    </thead>
               <tbody id="tblinformacion">

                  
               </tbody>
            </table>
            </td></tr></table>
            <p>
                <a href="javascript:void(0);" class="Button LinkCancel jqmClose" 
                    id="btnCancelObservacion" onclick="Ocultar();">
                    <%= Html.Term("Close","Close")%></a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
<script type="text/javascript">

    $(document).ready(function () {
       

        $('.Hab').attr("enabled", true);
        $('.Desa').attr("disabled", true);

        $("#OpeningDay").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#FinalDay").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#SharesNumber").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#DayValidate").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });

        $("#DayExpiration").keypress(function () {
            if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                event.preventDefault();
            }
        });


    });


    function ValidateDays() {
        var canAdd = true;
        var openingInput = $("#OpeningDay").val();
        var finalInput = $("#FinalDay").val();


        if (Number(openingInput) >= Number(finalInput)) {
            showMessage("Nope", true);
            return false;
        }

        $("#paginatedGrid > tbody > tr").each(function () {

            if ($(this).children("td").length > 1) {
                var openingDay = $(this).find("td").get(1).textContent;
                var finalDay = $(this).find("td").get(2).textContent;

                if (Number(openingInput) >= Number(openingDay) && Number(openingInput) <= Number(finalDay)
                        
                        ) {
                    canAdd = false;
                }
                else {
                    if (Number(finalInput) >= Number(openingDay) && Number(finalInput) <= Number(finalDay)
                            
                            ) {
                        canAdd = false;
                    }
                }
            }
        });

        return canAdd;
    }

    function ValidateDiscount() {
        var canAdd = true;
        var discount = $("#Discount").val();



        if (Number(discount) < 0 || Number(discount) > 100) {
            showMessage("Nope", true);
            return false;
        }

        return canAdd;
    }

    function deletefirsRow() {

        $('#paginatedGrid tbody:first tr').each(function (i) {

            var dat = $(this).find("td").eq(0).html();
            if (dat == 'There are no details') {
                $(this).remove();
            }
        });
    }
    </script>




</asp:Content>
