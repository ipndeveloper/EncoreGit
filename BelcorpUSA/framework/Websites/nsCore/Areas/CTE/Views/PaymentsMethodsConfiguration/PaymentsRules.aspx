<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/PaymentMethodsConfigurationsManagement.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<%--//@01 20150717 BR-CC-003 G&S LIB: Se crea la pantalla--%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">

    <%		
        var Edit = ViewBag.Edit;
        var Description ="";
        var NumberCuotas = 0;
        var NumberDayVal = 0;
        
        if (Edit.Description != null )
	    {
		 Description=Edit.Description;
        }
        
        if (Edit.NumberCuotas != null || Edit.NumberCuotas == 0)
	    {
		 NumberCuotas= Edit.NumberCuotas ;
        }
        if (Edit.NumberDayVal != null || Edit.NumberDayVal == 0)
        {
            NumberDayVal = Edit.NumberDayVal;
        }

        if (Edit.NumberDayVal != null || Edit.NumberDayVal == 0)
        {
            NumberDayVal = Edit.NumberDayVal;
        }
        
        List<PaymentConfigurationPerAccountSearchData> EditAccountTypes = ViewBag.EditAccountTypes;
        List<PaymentConfigurationPerOrderTypesSearchData> EditOrderTypes = ViewBag.EditOrderTypes;
        var EditAccountTypesCount = ViewBag.EditAccountTypesCount;        
        var EditOrderTypesCount = ViewBag.EditOrderTypesCount;
        var EditCreditDisp = Edit.PaymentCredit;  
		
	%>

		<h2>
			<%=  Html.Term("PaymentRulesandConfiguration", "Payment Rules and Configurations")%>
        </h2>
        
        <%--<%= Html.Term("BrowsePaymentRulesConfiguration", "Browse Payment Rules Configuration") %> | <a href="<%= ResolveUrl("~/CTE/InterestRules/Index") %>"><%= Html.Term("GenerateRules", "Generate Rules")%></a>--%>
         <a href="<%= ResolveUrl("~/CTE/PaymentsMethodsConfiguration/BrowsePayments") %>"><%=  Html.Term("BrowsePaymentRulesConfiguration", "Browse Payment Rules Configuration")%></a>
	</div>
   
    <h3>General Configuration</h3>
    <table id="generalConfigurationTable" class="DataGrid FormGrid" width="90%">
		<tbody>
			<tr>
				<td class="FLabel">
					<%= Html.Term("Collection Entity", "Collection Entity")%>:
				</td>
				<td>
					<select id="collectionEntityID" class = "required">
                        <option value ="">Select Collection Entity</option>
						<% foreach (var collectionEntity in NetSteps.Data.Entities.Repositories.PaymentMethodsRepository.BrowseCollectionEntities())
						   {
						%>
						<option value="<%= collectionEntity.CollectionEntityID %>"  <%= Edit.CollectionEntityID== collectionEntity.CollectionEntityID ? "selected=\"selected\"" : "" %> >
							<%= collectionEntity.CollectionEntityName%></option>
						<%      
						   } 
						%>
					</select>
				</td>
			</tr>
            <tr>
				<td class="FLabel">
					<%= Html.Term("Order Status", "Order Status")%>:
				</td>
				<td>
					<select id="orderStatusID" class = "required">
                        <option value ="">Select an Order Status</option>
						<% foreach (var orderStatus in SmallCollectionCache.Instance.OrderStatuses.Where(a => a.Active))
						   {
						%>
						<option value="<%= orderStatus.OrderStatusID %>"  <%= Edit.OrderStatusID== orderStatus.OrderStatusID  ? "selected=\"selected\"" : "" %> >
							<%= orderStatus.Name%></option>
						<%      
						   } 
						%>
					</select>
				</td>
			</tr>
            <tr>
				<td class="FLabel">
					<%= Html.Term("Fine And Interest Rules", "Fine and Interest Rules")%>:
				</td>
				<td>
					<select id="fineAndInterestRulesID" class = "required">
                        <option value ="">Select a Rule</option>
						<% foreach (var rules in NetSteps.Data.Entities.Repositories.CTERepository.BrowseRules())
						   {
						%>
						<option value="<%= rules.FineAndInterestRulesID %>" <%= Edit.FineAndInterestRulesID== rules.FineAndInterestRulesID  ? "selected=\"selected\"" : "" %> >
							<%= rules.Name%></option>
						<%      
						   } 
						%>
					</select>
				</td>
			</tr>
			<tr>
				<td class="FLabel">
					<%= Html.Term("Days For Payment", "Days For Payment")%>:
				</td>
				<td>
					<input id="daysForPayment" type="text" class="clear required justNumbers"
                     name= "<%= Html.JavascriptTerm("DaysForPaymentInvalid", "Days for Payment invalid.") %>" 
                     value="<%= Edit.DaysForPayment==0?"":Edit.DaysForPayment%>"                  
                    />
                    <input id="hdnPaymentConfigurationID"  value= "<%= Edit.PaymentConfigurationID%>"  type="hidden"/>
				</td>
			</tr>
			
			<tr>
				<td class="FLabel">
					<%= Html.Term("Tolerance Percentage", "Tolerance Percentage")%>:
				</td>
				<td>
					<input id="tolerancePercentage" type="text"   
                    <%=  Edit.TolerancePercentage==null?"disabled=\"disabled\"":"" %> 
                     name= "<%= Html.JavascriptTerm("TolerancePercentageInvalid", "Tolerance Percentage invalid.") %>"
                     value="<%= Edit.TolerancePercentage==null?"":Edit.TolerancePercentage%>"                      
                    monedaidioma='CultureIPN' />
                    <input id ="radioTolerancePercentage" value="value" type="radio" 
                       <%=  Edit.TolerancePercentage==null?"": "checked=\"checked\"" %> 
                      />
				</td>
                                 
			</tr>
			<tr>
				<td class="FLabel">
					<%= Html.Term("Tolerance Value", "Tolerance Value")%>:
				</td>
				<td>
					<input id="toleranceValue" type="text"   
                    <%=  Edit.ToleranceValue==null?"disabled=\"disabled\"":"" %> 
                    name= "<%= Html.JavascriptTerm("ToleranceValueInvalid", "Tolerance Value invalid.") %>" 
                     value= "<%= Edit.ToleranceValue==null?"":Edit.ToleranceValue%>"
                     class="justNumbers"
                    />
                    <input id ="radioToleranceValue" type="radio" value= "value"  
                     <%=  Edit.ToleranceValue==null?"": "checked=\"checked\"" %> 
                    />
				</td> 
			</tr> 
                <tr>
                    <td class="FLabel">
					    <%= Html.Term("Description", "Description")%>:
                    </td>
                      <td colspan = "2">
                      <input id="txtDescription" type="text" value ="<%= Description %>"  />
                    </td>
                </tr>
                <tr class="trNumCuotas">
                    <td class="FLabel">
					    <%= Html.Term("Number Cuotas", "Number Cuotas")%>:
                    </td>
                      <td colspan = "2">
                      <input id="txtNumberCuotas" type="text" class="justNumbers" value ="<%= NumberCuotas %>" />
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
					    <%= Html.Term("DaysVal", "Días de Validez")%>:
                    </td>
                      <td colspan = "2">
                      <input id="txtDaysVal" type="text" value ="<%= NumberDayVal %>"  maxlength="3" class="clear required justNumbers" />
                    </td>
                </tr> 

               <tr class="trCreditDisp" >
               
                <td>
                <%= Html.Term("UtilizaCreditoDisponible", "Utiliza Crédito Disponible")%>:
                </td>
                <td>
                    <label> <input type="radio" name="rbCreditDisp" value="N"   <%= EditCreditDisp == "N"? "checked=\"checked\"" : "" %>   /> <%= Html.Term("No", "No")%> </label>
                    <label> <input type="radio" name="rbCreditDisp" value="S"  <%=  EditCreditDisp == "S"? "checked=\"checked\"" : "" %>  /> <%= Html.Term("Yes", "Yes")%> </label> 
                </td>
             </tr>

		</tbody>
	</table>

    <h3>Aditional Configuration</h3>
    <table id="restrictionsTable" class="DataGrid FormGrid" >
		<tbody>
            <tr>
                <td>
                    Specific Accounts? 
                </td>
                <td>
                    <label> <input type="radio" name="rbRestrictAcc" value="0"   <%= EditAccountTypesCount == null? "checked=\"checked\""  :  EditAccountTypesCount == 0? "checked=\"checked\"" : "" %>   /> <%= Html.Term("No", "No")%> </label>
                    <label> <input type="radio" name="rbRestrictAcc" value="1"  <%=  EditAccountTypesCount == null? "" : EditAccountTypesCount > 0? "checked=\"checked\"" : "" %>  /> <%= Html.Term("Yes", "Yes")%> </label> 
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <table id="tRestrictAcc"  class="DataGrid" >
                        <thead>
                            <tr class="GridColHead">
                                <th>
                                    <input id="chkAllRestrictAcc" type="checkbox" class="chkAll"/> 
                                </th>
                                <th style="width:300px">
                                    <%= Html.Term("CheckAll", "Check All")%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                        
                        <% 
                            int cRestrictAcc = 0;
                            foreach (var accountType in SmallCollectionCache.Instance.AccountTypes.Where(a => a.Active))
						   {
                               var exist = 0;
                                if(EditAccountTypes!=null)
                                    exist= EditAccountTypes.Where( x => x.AccountTypeID == accountType.AccountTypeID).Count();
                                
						%>
                            
                            <tr <%= cRestrictAcc % 2 == 0 ? "class='Alt'" : string.Empty %>>
                                <td>


                                    <input type="checkbox" value="<%= accountType.AccountTypeID %>"  
                                      name="chkAcc"
                                      <%= exist>0? "checked=\"checked\"" : "" %>
                                    />
                                </td>
                                <td>
                                    <%= accountType.GetTerm()%>
                                </td>
                            </tr>
						<%   
                               cRestrictAcc++;   
						   } 
						%>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    Specific to Order Types? 
                </td>
                <td>
                    <label> <input type="radio" name="rbRestrictOrd" value="0"  <%= EditOrderTypesCount == null? "checked=\"checked\""  :  EditOrderTypesCount == 0? "checked=\"checked\"" : "" %>   /> <%= Html.Term("No", "No")%> </label>
                    <label> <input type="radio" name="rbRestrictOrd" value="1"  <%=  EditOrderTypesCount == null? "" : EditOrderTypesCount > 0? "checked=\"checked\"" : "" %>  /> <%= Html.Term("Yes", "Yes")%> </label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <table id="tRestrictOrd"  class="DataGrid" >
                        <thead>
                            <tr class="GridColHead">
                                <th>
                                    <input id="chkAllRestrictOrd" type="checkbox" class="chkAll"/>
                                </th>
                                <th style="width:300px">
                                    <%= Html.Term("CheckAll", "Check All")%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                    
                            <% 
                                int cRestrictOrd = 0;
                                foreach (var orderTypes in SmallCollectionCache.Instance.OrderTypes.Where(a => a.Active))
						        {
                                 var existO = 0;
                                 if (EditOrderTypes != null)
                                     existO = EditOrderTypes.Where(x => x.OrderTypeID == orderTypes.OrderTypeID).Count();
						    %>
                            <tr <%= cRestrictOrd % 2 == 0 ? "class='Alt'" : string.Empty %>>
                                <td>
                                    <input type="checkbox" value="<%= orderTypes.OrderTypeID %>"
                                    <%= existO>0? "checked=\"checked\"" : "" %>
                                    />
                                </td>
                                <td>
                                    <%= orderTypes.GetTerm()%>
                                </td>
                            </tr>
						    <%     
                                    cRestrictOrd++;    
						        } 
						    %>
                        </tbody>
                    </table>
                </td>
            </tr> 
		</tbody>
	</table>

    <h3>Location Configuration</h3>
    <table id="LocationRestrictionsTable" class="DataGrid FormGrid" width="90%">
		<tbody>
			<tr>
				<td class="FLabel">
					<%= Html.Term("State", "State")%>:
				</td>
				<td>
					<select id="state" name ="state" >
                        <option value ="">Select a State</option>
						<%                            
                            foreach (var state in ViewData["getStatesTaxCache"] as List<StateProvincesData>)
						   {
						%>
						<option value="<%= state.Name %>"><%= state.Name%></option><%}%>                        
					</select>                    
                   
				</td>
            </tr>
			<tr> 
				<td class="FLabel">
					<%= Html.Term("City", "City")%>:
				</td>
                <td>
					<select id="city">
                        <option value ="">Select a City</option>						
					</select>
                    
				</td> 
             </tr>               
             <tr>  				
                <td class="FLabel">
					<%= Html.Term("County", "County")%>:
				</td>
                <td>
					<select id="county">
                        <option value ="">Select a County</option>						
					</select>
				</td>
                <td class="Flabel"><%=Html.Term("Except", "Except")%> : 
                <input type="checkbox" id ="checkExcept"  /></td>
			</tr>
            <tr></tr>
            <tr>
                <td>
                    <p class="FormButtons">
                        <a id="SaveZone" href="javascript:void(0);" class="Button BigBlue">
                               <%= Html.Term("Add", "Add")%>
                        </a>
                    </p>
                </td>
            </tr>
		</tbody>
	</table>
    <span class="ClearAll"></span>  
    <% 
        string id = Edit.PaymentConfigurationID.ToString();           
    %>  
        <% Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.PaymentsZonesData>("~/CTE/PaymentsMethodsConfiguration/GetZones/?ID=" +id)
            .AutoGenerateColumns()
            .HideClientSpecificColumns_()
            .CanChangeStatus(true,true,"")
            .ClickEntireRow()
		    .Render(); 
        %>


    <table id="RegisterTable" class="DataGrid FormGrid" width="90%">
		<tr>			
			<td>
				<p class="FormButtons">
					<a id="btnSave" class="Button BigBlue" href="javascript:void(0);">
						<%= Html.Term("SavePayment", "Save Payment")%></a>
				</p>
			</td>
		</tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
   
	
	<%= Html.Term("GeneralConfiguration", "General Configuration")%>
	
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
               

	
         //Developed by WCS - CSTI
	        $(".justNumbers").keydown(function (event) {
	            if (event.shiftKey) event.preventDefault();
	            if (event.keyCode == 46 || event.keyCode == 8) {
	            }
	            else {
	                if (event.keyCode < 95) {
	                    if (event.keyCode < 48 || event.keyCode > 57) event.preventDefault();
	                }
	                else {
	                    if (event.keyCode < 96 || event.keyCode > 105) event.preventDefault();
	                }
	            }
	        });

        <%if (ViewBag.IsTargetCredit == true ) { %> 
        $(".trNumCuotas").show(); 
         <%
        }else { 
	                    %> $(".trNumCuotas").hide(); <%
        }       %>
                 
     <%if (ViewBag.IsCreditDisp == true ) { %> 
        $(".trCreditDisp").show();         
         <% }else {  %>
         $(".trCreditDisp").hide(); 
          <% }%>
	        

	        var r = $('<a class="Delete UI-icon-container" href="javascript:void(0);"><span class="UI-icon icon-deleteSelected"></span><span>Remove Selected</span></a><span class="pipe"></span>');
	        var space = $('<span class="ClearAll"></span>')

	        $("#paginatedGridOptions").append(r);
	        $("#paginatedGridOptions").append(space);
	        $(".deactivateButton").css("display", "none");
	        $(".activateButton").css("display", "none");



	        document.getElementById('radioTolerancePercentage').onchange = disablefieldValue;
	        document.getElementById('radioToleranceValue').onchange = disablefieldPercentage;


	        function disablefieldValue() {

	            if (document.getElementById('radioTolerancePercentage').checked == true) {

	                document.getElementById('radioToleranceValue').checked = false;
	                document.getElementById('tolerancePercentage').disabled = false;
	                document.getElementById('toleranceValue').value = '';
	                document.getElementById('toleranceValue').disabled = true;
	                //alert(document.getElementById('toleranceValue').val());
	            }

	        }

	        function disablefieldPercentage() {

	            if (document.getElementById('radioToleranceValue').checked == true) {

	                document.getElementById('radioTolerancePercentage').checked = false;
	                document.getElementById('toleranceValue').disabled = false;
	                document.getElementById('tolerancePercentage').value = '';
	                document.getElementById('tolerancePercentage').disabled = true;
	                alert(document.getElementById('tolerancePercentage').val());
	            }
	        }



	        $('#SaveZone').click(function (e) {

	            //                if (!$('#state').val() && $('#state').is(':visible')) {
	            //                    $('#state').showError('<%= Html.JavascriptTerm("valStateProvince", "Value to States Provinces not selected.") %>');
	            //                    errorCount++;
	            //                }
	            //                if (!$('#city').val() && $('#city').is(':visible')) {
	            //                    $('#city').showError('<%= Html.JavascriptTerm("valStateCity", "Value to City not selected.") %>');
	            //                    errorCount++;
	            //                }
	            var state = $("#state option:selected").html();
	            var city = $("#city option:selected").html();
	            var county = $("#county option:selected").html();
	            var col3 = $('#checkExcept').is(":checked");
	            var scopeLevelID;


	            if ($("#state option:selected").val() != "" && $("#city option:selected").val() != "" && $("#county option:selected").val() != "") {
	                scopeLevelID = 3;
	            }
	            if ($("#state option:selected").val() != "" && $("#city option:selected").val() != "" && $("#county option:selected").val() == "") {
	                scopeLevelID = 2;
	            }
	            if ($("#state option:selected").val() != "" && $("#city option:selected").val() == "" && $("#county option:selected").val() == "") {
	                scopeLevelID = 1;
	            }
	            if (scopeLevelID == 1 || scopeLevelID == 2 || scopeLevelID == 3) {
	                $.get('<%= ResolveUrl(string.Format("~/CTE/PaymentsMethodsConfiguration/GetScopeLevels/")) %>', { scopeLevelID: scopeLevelID }, function (response) {
	                    if (response.result) {
	                        if (response.scopeLevels) {

	                            row = $('<tr class="Alt hover"></tr>');
	                            check = $('<td>' + '<input type="checkbox" >' + '</td>');
	                            colState = $('<td>' + response.scopeLevels[0].Name + '</td>');
	                            if (scopeLevelID == 1) colCity = $('<td>' + state + '</td>');
	                            if (scopeLevelID == 2) colCity = $('<td>' + city + '</td>');
	                            if (scopeLevelID == 3) colCity = $('<td>' + county + '</td>');
	                            colExcept = $('<td>' + col3 + '</td>');

	                            row.append(check, colState, colCity, colExcept).prependTo("#paginatedGrid tbody");
	                        }
	                    } else {
	                        showMessage(response.message, true);
	                    }
	                });
	            }



	        });


	        $('.Delete').click(function (e) {
	            try {
	                var table = document.getElementById('paginatedGrid');
	                var rowCount = table.rows.length;

	                for (var i = 0; i < rowCount; i++) {
	                    var row = table.rows[i];
	                    var chkbox = row.cells[0].childNodes[0];
	                    if (null != chkbox && true == chkbox.checked) {
	                        table.deleteRow(i);
	                        rowCount--;
	                        i--;
	                    }
	                }
	            } catch (e) {
	                alert(e);
	            }
	        });


	        $('#state').change(function () {
	            var select = document.getElementById('city');
	            var length = select.options.length;
	            for (i = 1; i < length; i++) {
	                select.options[i] = null;
	            }
	            $('#city').prop('selectedIndex', 0);
	            var t = $(this);
	            var state = $("#state option:selected").val();

	            showLoading(t);
	            $.get('<%= ResolveUrl(string.Format("~/CTE/PaymentsMethodsConfiguration/GetCities/")) %>', { state: state }, function (response) {
	                if (response.result) {
	                    hideLoading(t);
	                    $('#city').children('option:not(:first)').remove();
	                    $('#county').children('option:not(:first)').remove();
	                    if (response.Cities) {
	                        for (var i = 0; i < response.Cities.length; i++) {
	                            $('#city').append('<option value="' + response.Cities[i] + '">' + response.Cities[i] + '</option>');
	                        }
	                    }
	                } else {
	                    showMessage(response.message, true);
	                }
	            });
	        });



	        $('#city').change(function () {
	            $('#county').prop('selectedIndex', 0);
	            var t = $(this);
	            //var State = $("#state option:selected").html();
	            //var city = $("#city option:selected").html();
	            var data = { state: $("#state option:selected").val(), city: $("#city option:selected").val() }, t = $(this);

	            showLoading(t);
	            $.get('<%= ResolveUrl(string.Format("~/CTE/PaymentsMethodsConfiguration/GetCounty/")) %>', data, function (response) {
	                if (response.result) {
	                    hideLoading(t);
	                    $('#county').children('option:not(:first)').remove();
	                    if (response.County) {
	                        for (var i = 0; i < response.County.length; i++) {
	                            $('#county').append('<option value="' + response.County[i] + '">' + response.County[i] + '</option>');
	                        }
	                    }
	                } else {
	                    showMessage(response.message, true);
	                }
	            });
	        });

	        $("#collectionEntityID").change(function () {
           
	            $.ajax({
	                type: 'POST',
	                url: '<%=ResolveUrl("~/CTE/PaymentsMethodsConfiguration/IsCreditCard/")%>',
	                data: (
                        {
                            CollectionEntityID: $("#collectionEntityID").val()
                        }),
	                asyn: false,
	                success: function (data) {
	                    if (data.result == true) {
	                        $(".trNumCuotas").show();

	                    } else {
	                        $(".trNumCuotas").hide();
	                    }

                         if (data.IsCreditDisp == true) {
	                        $(".trCreditDisp").show();

	                    } else {
	                        $(".trCreditDisp").hide();
                            $("input[name='rbCreditDisp' value='N']").prop('checked', false);
//                             $('#chkAllRestrictOrd, #tRestrictOrd input:radio').not(this).prop('checked', false);
	                    }
	                }
	            });
	        });

	        $('#btnSave').click(function () {


	            var numbersOnly = /^\d+$/;
	            var decimalOnly = /^\s*-?[1-9]\d*(\.\d{1,2})?\s*$/;
	            var errCount = 0;
	            if ($('#generalConfigurationTable').checkRequiredFields()) {
	                if ($('#restrictionsTable').checkRequiredFields()) {
	                    if ($('#LocationRestrictionsTable').checkRequiredFields()) {


	                        if (!numbersOnly.test($('#daysForPayment').val())) {
	                            $('#daysForPayment').showError('<%= Html.JavascriptTerm("DaysForPaymentInvalid", "Days For Payment Invalid.") %>');
	                            errCount++;
	                            return false;
	                        }

	                        if ($('#tolerancePercentage').val() && !decimalOnly.test($('#tolerancePercentage').val())) {
	                            $('#tolerancePercentage').showError('<%= Html.JavascriptTerm("TolerancePercentageInvalid", "Tolerance Percentage Invalid.") %>');
	                            errCount++;
	                            return false;
	                        }

	                        if ($('#toleranceValue').val() && !numbersOnly.test($('#toleranceValue').val())) {
	                            $('#toleranceValue').showError('<%= Html.JavascriptTerm("ToleranceValueInvalid", "Tolerance Value Invalid.") %>');
	                            errCount++;
	                            return false;
	                        }

	                        var RestrictAccList = GetRestrictAccList();

	                        if (RestrictAccList.length == 0) {
	                            showMessage('<%= Html.Term("AccountTypeRestrict", "You must select at least 1 Account Type to Restrict.") %>', true);
	                            return;
	                        }

	                        var RestrictOrdList = GetRestrictOrdList();

	                        if (RestrictOrdList.length == 0) {
	                            showMessage('<%= Html.Term("OrderTypeRestrict", "You must select at least 1 Order Type to Restrict.") %>', true);
	                            return;
	                        }

	                        if (errCount == 0) {
	                            var data = { paymentID: $('#collectionEntityID').val(),
	                                rulesID: $('#fineAndInterestRulesID').val(),
	                                orderStatus: $('#orderStatusID').val(),
	                                daysForPayment: $('#daysForPayment').val(),
	                                tolerancePercentage: $('#tolerancePercentage').val(),
	                                toleranceValue: $('#toleranceValue').val(),
	                                paymentConfigurationID: $('#hdnPaymentConfigurationID').val(),
	                                Description: $('#txtDescription').val(),
	                                NumberCuotas: $('#txtNumberCuotas').val(),
                                    DaysVal: $('#txtDaysVal').val(),
	                                RestrictAccList: RestrictAccList,
	                                RestrictOrdList: RestrictOrdList,
                                    PaymentCredit : $("input[name=rbCreditDisp]:checked").val()

	                            }, t = $(this);

	                            $('#paginatedGrid tbody:first tr').each(function (i) {
	                                data['zones[' + i + '].Name'] = $(this).find("td").eq(1).html();
	                                data['zones[' + i + '].Value'] = $(this).find("td").eq(2).html();
	                                data['zones[' + i + '].Except'] = $(this).find("td").eq(3).html();
	                            });

	                            var strURL = '<%= ResolveUrl("~/CTE/PaymentsMethodsConfiguration/SavePaymentsRules") %>';

	                            $.ajax({
	                                type: 'POST',
	                                url: strURL,
	                                data: JSON.stringify(data),
	                                contentType: 'application/json; charset=utf-8',
	                                dataType: 'json',
	                                success: function (response) {
	                                    if (response.result) {
	                                        showMessage("Payment was saved!", false);


	                                    } else {
	                                        showMessage(response.message, true);
	                                    }
	                                }
	                            });

	                            //	                            $.post('/CTE/PaymentsMethodsConfiguration/SavePaymentsRules', data, function (response) {
	                            //	                                if (response.result) {
	                            //	                                    showMessage("Payment was saved!", false);
	                            //	                                } else {
	                            //	                                    showMessage(response.message, true);
	                            //	                                }
	                            //	                            });
	                        }
	                    }
	                }
	            }
	        });


	        if ($("input[name=rbRestrictAcc]:checked").val() == 1) {
	            $("#tRestrictAcc").show();
	        }
	        else {
	            $("#tRestrictAcc").hide();
	        }


	        $("input[name=rbRestrictAcc]:radio").change(function () {
	            if ($("input[name=rbRestrictAcc]:checked").val() == 1) {
	                $('#chkAllRestrictAcc, #tRestrictAcc input:checkbox').not(this).prop('checked', false);
	                $("#tRestrictAcc").show();
	            }
	            else {
	                $("#tRestrictAcc").hide();
	            }
	        });


	        if ($("input[name=rbRestrictOrd]:checked").val() == 1) {
	            $("#tRestrictOrd").show();
	        }
	        else {
	            $("#tRestrictOrd").hide();
	        }

	        $('#chkAllRestrictAcc').click(function () {
	            $('#tRestrictAcc input:checkbox').not(this).prop('checked', this.checked);
	        });

	        $("input[name=rbRestrictOrd]:radio").change(function () {

	            if ($("input[name=rbRestrictOrd]:checked").val() == 1) {
	                $('#chkAllRestrictOrd, #tRestrictOrd input:checkbox').not(this).prop('checked', false);
	                $("#tRestrictOrd").show();
	            }
	            else {
	                $("#tRestrictOrd").hide();
	            }
	        });

	        $('#chkAllRestrictOrd').click(function () {
	            $('#tRestrictOrd input:checkbox').not(this).prop('checked', this.checked);
	        });

	    });

	    function GetRestrictAccList() {

	        var RestrictAccList = [];

	        var CheckBoxes = $('#tRestrictAcc input:checkbox').not('.chkAll');

	        if ($("input[name=rbRestrictAcc]:checked").val() == 1) {
	            CheckBoxes.each(function () {
	                if ($(this).prop('checked')) {
	                    RestrictAccList.push($(this).val());
	                }
	            });
	        }
	        else {
	            CheckBoxes.each(function () {
	                RestrictAccList.push($(this).val());
	            });
	        }

	        return RestrictAccList;
	    }

	    function GetRestrictOrdList() {

	        var RestrictOrdList = [];

	        var CheckBoxes = $('#tRestrictOrd input:checkbox').not('.chkAll');

	        if ($("input[name=rbRestrictOrd]:checked").val() == 1) {
	            CheckBoxes.each(function () {
	                if ($(this).prop('checked')) {
	                    RestrictOrdList.push($(this).val());
	                }
	            });
	        }
	        else {
	            CheckBoxes.each(function () {
	                RestrictOrdList.push($(this).val());
	            });
	        }

	        return RestrictOrdList;
        }
    </script>
	
</asp:Content>


<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
