<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/FInterestRulesManagement.Master" 
Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.CTERulesNegotiationData>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("GenerateRules", "Generate Rules")%></h2>
            <a href="<%= ResolveUrl("~/CTE/InterestRules/BrowseRules") %>"><%= Html.Term("BrowseRules", "Browse Rules")%></a>

	</div>
    <table class="FormTable Section" id="newRule" width="100%">   
        <tr>
            <td class="FLabel"><%= Html.Term("Name", "Name") %>:
            </td>
            <td><input type="hidden" id="ruleID" value="<%= Model.FineAndInterestRulesID == 0 ? "0" : Model.FineAndInterestRulesID.ToString() %>" />
                <input type="text" id="rName" value="<%=Model.Name%>" class="required" name="<%= Html.JavascriptTerm("Rulenamecannotbenull.", "Rule name can not be null.") %>" style="width: 200px;"/></td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("Negotiation", "Negotiation")%>:
            </td>
            <td>
                <select id="sNegotiation" class="required" name="<%= Html.JavascriptTerm("ValNegotiation", "Not selected negotiation.") %>">
                    <option value=""><%= Html.Term("SelectTypeNegotiation", "Select Type Negotiation")%></option>
                        <% foreach (var items in ViewData["Negotiations"] as List<CTENegotiationSearchData>)
                           {
                            %>
                     <option value="<%=items.NegotiationLevelID %>"><%=items.Name%></option>
                            <%                                       
                            }                    
                        %>                        
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("InitialDay", "Initial Day") %>:
            </td>
            <td><input type="text" id="initialDay"  class="clear required" name="<%= Html.JavascriptTerm("InitialInvalidDay", "Initial invalid day.") %>"/></td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("FinalDay", "Final Day") %>:
            </td>
            <td><input type="text" id="finalDay" class="clear required" name="<%= Html.JavascriptTerm("Invalidfinalday", "Invalid final day.") %>" /> </td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("%Fine", "% Fine") %>:
            </td>
            <td><input type="text" id="fine"  monedaidioma='CultureIPN'  class="clear required" name="<%= Html.JavascriptTerm("Percentageoffineinvalid", "Percentage of fine invalid.") %>" /></td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("AppliedValue", "Applied Value")%>:
            </td>
            <td>
                <select id="sAppliedValue" class="required" name="<%= Html.JavascriptTerm("Valuetoapplyfinenotselected", "Value to apply fine not selected.") %>" >
                    <option value=""><%= Html.Term("SelectValor", "Select Valor")%></option>
                    <% foreach (var items in ViewData["BaseAmounts"] as List<CTEFineBaseAmountsData>)
                           {
                            %>
                     <option value="<%=items.FineBaseAmountID %>"><%=items.Name%></option>
                            <%                                       
                            }                    
                        %>
                </select>
            </td>
        </tr>
        <tr>
            <td class="FLabel" ><%= Html.Term("MinimumDebt", "Minimum Dbet")%>:
            </td>
            <td><input type="text" id="minimumDebut"   monedaidioma='CultureIPN'  class="clear required" name="<%= Html.JavascriptTerm("InvalidBiddebt", "Invalid Bid debt.") %>"/></td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("%Interest", "% Interest")%>:
            </td>
            <td><input type="text" id="interest"  monedaidioma='CultureIPN'  class="clear required" name="<%= Html.JavascriptTerm("PercentageofInterestinvalid", "Percentage of Interest invalid.") %>"/></td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("AppliedValue", "Applied Value")%>:
            </td>
            <td>
                <select id="sInterestBaseAmount" class="required" name="<%= Html.JavascriptTerm("ValuetoapplyInterest", "Value to apply Interest not selected.") %>" >
                    <option value=""><%= Html.Term("SelectValor", "Select Valor")%></option>
                    <% foreach (var items in ViewData["BaseAmounts"] as List<CTEFineBaseAmountsData>)
                           {
                            %>
                     <option value="<%=items.FineBaseAmountID %>"><%=items.Name%></option>
                            <%                                       
                            }                    
                        %>
                </select>
            </td>
        </tr>
        
        
         <tr>
            <td class="FLabel"><%= Html.Term("%Discount", "% Discount")%>:
            </td>
            <td><input type="text" id="Discount"  monedaidioma='CultureIPN' class="clear required" name="<%= Html.JavascriptTerm("Discountinvalid", "Discount invalid.") %>"/></td>
        </tr>
        <tr>
            <td class="FLabel"><%= Html.Term("Applied Value", "Applied Value")%>:
            </td>
            <td>
                <select id="sDiscountBaseAmounts" class="required" name="<%= Html.JavascriptTerm("ValuetoapplyDiscount", "Value to apply Discount not selected.") %>" >
                    <option value=""><%= Html.Term("SelectValor", "Select Valor")%></option>
                    <% foreach (var items in ViewData["BaseAmounts"] as List<CTEFineBaseAmountsData>)
                           {
                            %>
                     <option value="<%=items.FineBaseAmountID %>"><%=items.Name%></option>
                            <%                                       
                            }                    
                        %>
                </select>
            </td>
        </tr>           
    </table>

    <table class="FormTable" width="100%">
		<tr>			
			<td>
				<p>
					<a id="btnAdd" href="javascript:void(0);" class="Button BigBlue"><span>
						<%= Html.Term("Add", "Add") %></span></a></p>
			</td>
		</tr>
	</table>   
    
    <% 
      string id = Model.FineAndInterestRulesID.ToString();           
    %>

    <div style="overflow-y: auto; height:300px; ">


    <% Html.PaginatedGrid<CTERulesNegotiationData>("~/CTE/InterestRules/GetRulesNegotiation/")
            .AutoGenerateColumns()
            .AddData("id", Model.FineAndInterestRulesID.ToString())
            .HideClientSpecificColumns_()
            .CanDelete("")             
            .ClickEntireRow()
		    .Render(); 
    %>

                    
    </div>
    
<table class="FormTable" width="100%">
		<tr>
			
			<td>
				<p>
					<a id="SaveRule" href="javascript:void(0);" class="Button BigBlue"><span>
						<%= Html.Term("Save", "Save") %></span></a></p>
			</td>
		</tr>
	</table>
    
    <script type="text/javascript">
        $(function () {

            $('input[monedaidioma=CultureIPN]').keyup(function (event) {

                var cultureInfo = '<%= CoreContext.CurrentCultureInfo.Name%>';
                // var value = parseFloat($(this).val());


                var formatDecimal = '$1.$2'; // valores por defaul 
                var formatMiles = ",";  // valores por defaul

                if (cultureInfo === 'en-US') {
                     formatDecimal = '$1.$2';
                     formatMiles = ",";
                }
                else if (cultureInfo === 'es-US') {
                     formatDecimal = '$1,$2';
                     formatMiles = ".";
                }
                else if (cultureInfo === 'pt-BR') {
                    formatDecimal = '$1,$2';
                    formatMiles = ".";
                }


                //            if (!isNaN(value)) {
                if (event.which >= 37 && event.which <= 40) {
                    event.preventDefault();
                }

                $(this).val(function (index, value) {


                    return value.replace(/\D/g, "")
                                 .replace(/([0-9])([0-9]{2})$/, formatDecimal)
                                 .replace(/\B(?=(\d{3})+(?!\d)\.?)/g, formatMiles);
                });

                //            }

            });


            $('.deleteButton').unbind("click");
            $('.deleteButton').click(function (e) {
                $(".checkAll:first").removeProp("checked");

                $("input[type=checkbox]:checked").parents("#paginatedGrid > tbody > tr").remove();
            });

            $('#btnAdd').click(function (e) {
                if ($('#newRule').checkRequiredFields()) {


                    if (!$('#sNegotiation').val() && $('#sNegotiation').is(':visible')) {
                        $('#sNegotiation').showError('<%= Html.JavascriptTerm("ValNegotiation", "Not selected negotiation.") %>');
                        errorCount++;
                    }

                    if (!$('#sAppliedValue').val() && $('#sAppliedValue').is(':visible')) {
                        $('#sAppliedValue').showError('<%= Html.JavascriptTerm("ValAppliedValue", "Value to apply fine not selected.") %>');
                        errorCount++;
                    }
                    if (!$('#sInterestBaseAmount').val() && $('#sInterestBaseAmount').is(':visible')) {
                        $('#sInterestBaseAmount').showError('<%= Html.JavascriptTerm("ValuetoapplyInterest", "Value to apply Interest not selected.") %>');
                        errorCount++;
                    }

                    if (!ValidateDays()) {
                        showMessage('<%= Html.JavascriptTerm("OverlappingNotAllwed", "Sorry, but overlapping is not allowed.") %>', true);
                        return;
                    }

                    // var state = $("#sState option:selected").html();
                    //var city = $("#sCity option:selected").html();
                    //var col3 = $('#checkExcept').is(":checked");
                    var negotiation = $("#sNegotiation option:selected").html();
                    var sNegotiation = $("#sNegotiation").val();
                    var initialDay = $("#initialDay").val();
                    var finalDay = $("#finalDay").val();
                    var fine = $("#fine").val();
                    var AppliedValue = $("#sAppliedValue option:selected").html();
                    var sAppliedValue = $("#sAppliedValue").val();
                    var minimumDebut = $("#minimumDebut").val();
                    var interest = $("#interest").val();
                    var InterestBaseAmounts = $("#sInterestBaseAmount option:selected").html();
                    var InterestBaseAmount = $("#sInterestBaseAmount").val();

                    var sDiscountBaseAmounts = $("#sDiscountBaseAmounts option:selected").html();
                    var Discount = $("#Discount").val();

                    //                row = $('<tr class="Alt hover"></tr>');
                    //                col3 = $('<td>' + sNegotiation + '</td>');
                    //                col4 = $('<td>' + initialDay + '</td>');
                    //                col5 = $('<td>' + finalDay + '</td>');
                    //                col6 = $('<td>' + fine + '</td>');
                    //                col7 = $('<td>' + sAppliedValue + '</td>');
                    //                col8 = $('<td>' + minimumDebut + '</td>');
                    //                col9 = $('<td>' + interest + '</td>');
                    //                col10 = $('<td>' + InterestBaseAmount + '</td>');
                    //                row.append(col3, col4, col5, col6, col7, col8, col9, col10).prependTo("#paginatedGrid2 tbody");

                    rules = $('<tr class="Alt hover"></tr>');
                    check = $('<td>' + '<input id="0" type="checkbox" value="0"/>' + '</td>');
                    rul1 = $('<td>' + negotiation + '</td>');
                    rul2 = $('<td>' + initialDay + '</td>');
                    rul3 = $('<td>' + finalDay + '</td>');
                    rul4 = $('<td>' + fine + '</td>');
                    rul5 = $('<td>' + AppliedValue + '</td>');
                    rul6 = $('<td>' + minimumDebut + '</td>');
                    rul7 = $('<td>' + interest + '</td>');
                    rul8 = $('<td>' + InterestBaseAmounts + '</td>');

                    rul9 = $('<td>' + Discount + '</td>');

                    rul10 = $('<td>' + sDiscountBaseAmounts + '</td>');

                    rules.append(check, rul1, rul2, rul3, rul4, rul5, rul6, rul7, rul8, rul9, rul10).prependTo("#paginatedGrid tbody");
                    deletefirsRow();
                    $(".clear").val("");
                }
            });


            //Guardar
            $('#SaveRule').click(function () {
                var data = { Name: $('#rName').val(),
                    FineAndInterestRulesID: $('#ruleID').val()
                }, t = $(this);
                $('#paginatedGrid tbody:first tr').each(function (i) {

                    var ID = $(this).find('td:first input[type=checkbox]').val();


                    //                if (isEnabled == "0") {


                    data['rules[' + i + '].FineAndInterestRulesPerNegotiationLevelID'] = ID;
                    data['rules[' + i + '].NegotiationLevel'] = $(this).find("td").eq(1).html();
                    data['rules[' + i + '].StartDay'] = $(this).find("td").eq(2).html();
                    data['rules[' + i + '].EndDay'] = $(this).find("td").eq(3).html();
                    data['rules[' + i + '].FinePercentage'] = $(this).find("td").eq(4).html();
                    data['rules[' + i + '].FineBaseAmount'] = $(this).find("td").eq(5).html();
                    data['rules[' + i + '].MinimumAmountForFine'] = $(this).find("td").eq(6).html();
                    data['rules[' + i + '].InterestPercentage'] = $(this).find("td").eq(7).html();
                    data['rules[' + i + '].InterestBaseAmount'] = $(this).find("td").eq(8).html();
                    data['rules[' + i + '].Discount'] = $(this).find("td").eq(9).html();
                    data['rules[' + i + '].FineBaseAmountIDReg'] = $(this).find("td").eq(10).html();

                    //                }
                });

                $.post('/CTE/InterestRules/SaveRule', data, function (response) {
                    if (response.result) {
                        showMessage("Rule was saved!", false);
                        window.location = '<%= ResolveUrl("~/CTE/InterestRules/BrowseRules/") %>';
                    } else {
                        showMessage(response.message, true);
                        window.location = '<%= ResolveUrl("~/CTE/InterestRules/BrowseRules/") %>';
                    }
                });
            });
        });
    </script>
</asp:Content>


  



<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">

    <script type="text/javascript">

        $(document).ready(function () {
            $("#paginatedGridPagination").css("display", "none");

            $("#initialDay").keypress(function () {
                if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                    event.preventDefault();
                }
            });

            $("#finalDay").keypress(function () {
                if (event.which != 8 && isNaN(String.fromCharCode(event.which))) {
                    event.preventDefault();
                }
            });

        });


        function ValidateDays() {
            var canAdd = true;
            var openingInput = $("#initialDay").val();
            var finalInput = $("#finalDay").val();
            var sNegotiationInput = $('#sNegotiation option:selected').text();

            if (Number(openingInput) >= Number(finalInput)){
                showMessage("Nope", true);
                return false;
            }

            $("#paginatedGrid > tbody > tr").each(function () {

                if ($(this).children("td").length > 1) {
                    var sNegotiation = $(this).find("td").get(1).textContent;
                    var openingDay = $(this).find("td").get(2).textContent;
                    var finalDay = $(this).find("td").get(3).textContent;

                    if (Number(openingInput) >= Number(openingDay) && Number(openingInput) <= Number(finalDay)
                        && sNegotiation == sNegotiationInput
                        ) {
                        canAdd = false;
                    }
                    else {
                        if (Number(finalInput) >= Number(openingDay) && Number(finalInput) <= Number(finalDay)
                            && sNegotiation == sNegotiationInput 
                            ) {
                            canAdd = false;
                        }
                    }
                }
            });

            return canAdd;
        }

        function deletefirsRow() {
            
            $('#paginatedGrid tbody:first tr').each(function (i) {

                var dat = $(this).find("td").eq(0).html();
                if (dat == 'There are no rules') {
                    $(this).remove();
                }
            });
        }
    </script>

</asp:Content>


