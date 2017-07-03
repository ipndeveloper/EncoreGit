<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" 
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.HelperObjects.SearchData.TitleSearchData>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

     var isSaving=false;// validar que no se realize actualiaciones multiples
    function on_ChangeTitlePhases(){
     var isValidTitlePhase=validaciones.validarTitlePhase();
    }
     var validaciones =
     {
     validarTitlePhase:function()
         {
           var isValid =false;
          var cmbTitlePhases=$("#cmbTitlePhases");
              if(!cmbTitlePhases.val())
              {
                cmbTitlePhases.showError('<%= Html.JavascriptTerm("TitlePhasesIsRequired", "Title Phases Is Required") %>');
                isValid=false;
              } 
               else
              {
                  cmbTitlePhases.clearError();
                  isValid=true;
              }
              return isValid ;
         }
     };

     function  validarRequiredTitleCalculation(pTitlePlanID,pTitleCalculationTypeID)
     {

      var Exists=false;
      $('#gridReqTitle tbody tr').each(function(){

       var TitleCalculationTypeID=$(this).find("td:eq(1)").text();
       var TitlePlanID=$(this).find("td:eq(2)").text();
               if(TitleCalculationTypeID ==pTitleCalculationTypeID && pTitlePlanID==TitlePlanID )
               {
                     showMessage('<%=@Html.Term("ExistsCalculationPlan", "Already registered the type of calculation for the plan")%>', true);
                Exists=true;
               return false;
              
               }
            
      });
      return Exists;
     }
     
 
      function  validarRequiredRequirementLegs(pPlanID,PTitleRequiredID)
     {

      var Exists=false;
      $('#gridReqLeg tbody tr').each(function(){

       var PlanID=$(this).find("td:eq(1)").text();
       var TitleRequiredID=$(this).find("td:eq(2)").text();
       
               if(pPlanID ==PlanID && PTitleRequiredID==TitleRequiredID )
               {
                     showMessage('<%=@Html.Term("ExistsCalculationPlan", "Already registered the type of Plan for the Title Required")%>', true);
                    Exists=true;
                   return false;
              
               }
            
      });
      return Exists;
     }

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






            function HideSections() {
                $('#SectionTitle').css("display", "none");
                $('#SectionRequirementTitle').css("display", "none");
                $('#SectionRequirementLeg').css("display", "none");
            };

            $('#lnkDetails').click(function (event) {
                event.preventDefault();
                HideSections();
                $('#SectionTitle').css("display", "block");
            });
            $('#lnkReqTitles').click(function (event) {
                event.preventDefault();
                HideSections();

                $('#SectionRequirementTitle').css("display", "block");
            });

            $('#btnAddReqTitle').click(function () {
                HideSections();
                $('#SectionRequirementTitle').css("display", "block");
            });

            $('#lnkReqLegs').click(function (event) {
                event.preventDefault();
                HideSections();
                $('#SectionRequirementLeg').css("display", "block");
            });
            $('#btnAddReqLeg').click(function () {
                HideSections();
                $('#SectionRequirementLeg').css("display", "block");
            });

            HideSections();
            $('#SectionTitle').css("display", "block");

            $('#btnSaveRequirement').click(function () {

                $('#ReqTitleCalculationTypeID').clearError();
                if ($('#ReqTitleCalculationTypeID').val() == "" || eval($('#ReqTitleCalculationTypeID').val()) < 1) {
                    $('#ReqTitleCalculationTypeID').showError('<%= Html.JavascriptTerm("RequirementIsRequired", "Requirement Is Required") %>');
                    return false;
                }

                $('#ReqTitlePlanID').clearError();
                if ($('#ReqTitlePlanID').val() == "" || eval($('#ReqTitlePlanID').val()) < 1) {
                    $('#ReqTitlePlanID').showError('<%= Html.JavascriptTerm("PlanIsRequired", "Plan Is Required") %>');
                    return false;
                }

                 var resultado=validarRequiredTitleCalculation($('#ReqTitlePlanID').val() ,$('#ReqTitleCalculationTypeID').val());
                 if(resultado)
                 {
                    return false;
                 }
                $('#txtMinValue').clearError();
                if ($('#txtMinValue').val()=="" || eval($('#txtMinValue').val()) < 1) {
                    $('#txtMinValue').showError('<%= Html.JavascriptTerm("MinValueIsRequired", "Minimum value Is Required") %>');
                    return false;
                }

               /* $('#txtMaxValue').clearError();*/

               /*
                if ($('#txtMaxValue').val() == "" || eval($('#txtMaxValue').val()) < 1) {
                    $('#txtMaxValue').showError('<%= Html.JavascriptTerm("MaxValueIsRequired", "Maximum value Is Required") %>');
                    return false;
                }
                */


//                if (eval($('#txtMaxValue').val()) <= eval($('#txtMinValue').val())  &&  ($('#txtMaxValue').val()||0  )>0 ) {
//                    $('#txtMaxValue').showError('<%= Html.JavascriptTerm("MaxValueRangeValidation", "Maximum value can not be less than minimum value") %>');
//                    return false;
//                }



                var row = '<tr>' +
                            '<td><input type="checkbox" class="clsQuit"/></td>' +
	                        '<td>' + $('#ReqTitleCalculationTypeID').val() + '</td>' +
                            '<td>' + $('#ReqTitleCalculationTypeID option:selected').text() + '</td>' +
                            '<td>' + $('#ReqTitlePlanID').val() + '</td>' +
                            '<td>' + $('#txtMinValue').val() + '</td>' +
//                            '<td>' + $('#txtMaxValue').val() + '</td>' +
                            '<td></td>' +
	                      '</tr>';

                $('#gridReqTitle').find('tbody').append(row);

            });

            $('#btnAddNewRequirement').click(function () {

                $('#ReqTitleCalculationTypeID').val('');
                $('#ReqTitlePlanID').val('');
                $('#txtMinValue').val('');
                $('#txtMaxValue').val('');
            });

            $('#btnSaveRequirementLeg').click(function () {

                $('#ReqLegPlanID').clearError();
                if ($('#ReqLegPlanID').val() == "" || eval($('#ReqLegPlanID').val()) < 1) {
                    $('#ReqLegPlanID').showError('<%= Html.JavascriptTerm("PlanIsRequired", "Plan Is Required") %>');
                    return false;
                }

                $('#ReqLegTitleID').clearError();
                if ($('#ReqLegTitleID').val() == "" || eval($('#ReqLegTitleID').val()) < 1) {
                    $('#ReqLegTitleID').showError('<%= Html.JavascriptTerm("TitleRequiredIsRequired", "Title Is Required") %>');
                    return false;
                } 
                var resultado=validarRequiredRequirementLegs( $('#ReqLegPlanID').val(),$('#ReqLegTitleID').val());
                 if(resultado)
                 {
                    return false;
                 }
                 var Generation=$('#txtGeneration').val()||"0" ;

              
                  if (Generation=="") {// según nueva especificacion no es requerido este campo
                  //  if ($('#txtGeneration').val() == "" || eval($('#txtGeneration').val()) < 1) {
                    $('#txtGeneration').showError('<%= Html.JavascriptTerm("GenerationIsRequired", "Generation Is Required") %>');
                    return false;
                }else{
                  $('#txtGeneration').clearError();
                }

                 var Level=$('#txtLevel').val()||"0";


                
                //if ($('#txtLevel').val() == "" || eval($('#txtLevel').val()) < 1) {
                if (Level=="") {
                    $('#txtLevel').showError('<%= Html.JavascriptTerm("LevelIsRequired", "Level Is Required") %>');
                    return false;
                }else{
                  $('#txtLevel').clearError();
                }

                $('#txtQuantity').clearError();
                if ($('#txtQuantity').val() == "" || eval($('#txtQuantity').val()) < 1) {
                    $('#txtQuantity').showError('<%= Html.JavascriptTerm("QuantityIsRequired", "Quantity Is Required") %>');
                    return false;
                }


                var row = '<tr>' +
                            '<td><input type="checkbox" class="clsQuit"/></td>' +
	                        '<td>' + $('#ReqLegPlanID').val() + '</td>' +
                            '<td>' + $('#ReqLegTitleID').val() + '</td>' +
                            '<td>' + $('#ReqLegTitleID option:selected').text() + '</td>' +
                            '<td>' + ($('#txtGeneration').val() ) + '</td>' +
                            '<td>' + ($('#txtLevel').val()) + '</td>' +
                            '<td>' + $('#txtQuantity').val() + '</td>' +
	                      '</tr>';

                $('#gridReqLeg').find('tbody').append(row);

            });

            $('#btnAddNewRequirementLeg').click(function () {

                $('#ReqLegPlanID').val('');
                $('#ReqLegTitleID').val('');
                $('#txtGeneration').val('');
                $('#txtLevel').val('');
                $('#txtQuantity').val('');
            });

            //            $(document).on('click', '.clsQuit', function () {
            //                var row = $(this).parents().get(1);
            //                $(row).remove();
            //            });

            $(document).on('click', '.clsDelete', function () {
                var sel = false;
                var ch = $(this).closest('table').find('tbody input[type=checkbox]');

                ch.each(function () {
                    var $this = $(this);
                    if ($this.is(':checked')) {
                        sel = true;
                        $this.closest('tr').remove();
                    }
                });

            });

             
            $('#btnSave').click(function () {

                if(isSaving)
                {
                   return false; 
                }
             var isValidTitlePhase=validaciones.validarTitlePhase();
                        if(!isValidTitlePhase)
                        {
                            return isValidTitlePhase;
                        }

                if ($('#titleForm').checkRequiredFields()) {

                    var dataArray = [];
                    $('#gridReqTitle tbody').find('tr').each(function (i, el) {
                        var $tds = $(this).find('td');
                        var oReqTitle = {
                            titleId: $('#hddTitleId').val(),
                            CalculationtypeID: $tds.eq(1).text(),
                            PlanID: $tds.eq(3).text(),
                            MinValue: $tds.eq(4).text().replace('.', ','),
                            MaxValue: 0,
                            DateModified: $tds.eq(5).text()
                        };
                        dataArray.push(oReqTitle);
                    });


                    var dataLegs = [];
                    $('#gridReqLeg tbody').find('tr').each(function (i, el) {
                        var $tds = $(this).find('td');
                        var oReqLeg = {
                            TitleId: $('#hddTitleId').val(),
                            PlanID: $tds.eq(1).text(),
                            TitleRequired: $tds.eq(2).text(),
                            Generation: ($tds.eq(4).text() ||"0"),
                            Level: ($tds.eq(5).text()  ||"0"),
                            TitleQTY: $tds.eq(6).text()
                        };

                        dataLegs.push(oReqLeg);
                    });
                    
                    var data = {
                        titleId: $('#hddTitleId').val(),
                        titleCode: $('#txtTitleCode').val(),
                        name: $('#txtName').val(),
                        sortOrder: $('#txtSortOrder').val(),
                        clientCode: $('#txtClientCode').val(),
                        clientName: $('#txtClientName').val(),
                        TitlePhaseID:$("#cmbTitlePhases").val(),
                        RequirementTitleCalculations: dataArray,
                        RequirementLegs: dataLegs,
                    };
                    isSaving=true;
                    $.ajax({
                        url: '<%= ResolveUrl("~/Commissions/Configurations/SaveTitle") %>',
                        data: JSON.stringify(data),
                        type: 'POST',
                        contentType: 'application/json;',
                        dataType: 'json',
                        success: function (response) {

                        isSaving=false;
                            if (response.result) {
                                showMessage('<%=@Html.Term("titleSavedSuccessfully", "Title Saved Successfully")%>', false);
                                if (!$('#hddTitleId').val()) { // Create case
                                    $('#hddTitleId').val(response.Id);
                                    // Reload Edit Mode 
                                    //window.location.replace('<%= ResolveUrl("~/Commissions/Configurations/EditTitle") %>' + "/" + response.TitleId);
                                }
                            } else {
                                showMessage(response.message, true);
                                     isSaving=false;
                            }
                        }
                    });


                }
            });
        });
            </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LeftNav" runat="server">
        <div class="SectionNav">
			    <ul class="SectionLinks">
                    <li><a href="#" id="lnkDetails"><span><%=Html.Term("TitleDetails", "Title Details")%></span></a></li>
                    <li><a href="#" id="lnkReqTitles"><span><%=Html.Term("RequirementTitle", "Requirement Title")%></span></a></li>
                    <li><a href="#" id="lnkReqLegs"><span><%=Html.Term("RequirementLegs", "Requirement Legs")%></span></a></li>
			    </ul>
		</div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="MainContent" runat="server">
    
    <%
        var titleRequirements = (IEnumerable<System.Web.Mvc.SelectListItem>)TempData["Titles"];
        var calculationTypes = (IEnumerable<System.Web.Mvc.SelectListItem>)TempData["CalculationTypes"];        
    %>
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("Titles", "Titles")%>
        </h2>
        <a href="<%= ResolveUrl("~/Commissions/Configurations/Titles") %>"><%= Html.Term("BrowseTitles", "Browse Titles")%></a>
        | <a href="<%= ResolveUrl("~/Commissions/Configurations/EditTitle") %>"><%= Html.Term("CreateaNewTitle", "Create a New Title") %></a>
	</div>

    <div id="SectionTitle">
    <table id="titleForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("TTitleID", "Title ID")%>:
            </td>
            <td>
                <input id="Text1" maxlength="25" type="text" value="<%= Model.TitleID %>" style="width: 20.833em;" disabled="disabled" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("TTitleCode", "Title Code") %>:
            </td>
            <td>
                <input type="hidden" id="hddTitleId" value="<%= Model.TitleID == 0 ? "" : Model.TitleID.ToString() %>" />
                <input id="txtTitleCode" maxlength="25" type="text" value="<%= Model.TitleCode %>"
                    class="required" name="<%= Html.Term("titleCodeIsRequired", "Title code is required") %>" 
                    style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("TitleClientName", "Client Name")%>:
            </td>
            <td>
                <input id="txtClientName" maxlength="255" type="text" value="<%= Model.ClientName %>"
                    class="required" name="<%= Html.Term("titleExternalNameIsRequired", "External Name is required") %>" 
                    style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("TitleClientCode", "Client Code")%>:
            </td>
            <td>
 
                <input id="txtClientCode" maxlength="20" type="text" value="<%= Model.ClientCode %>"
                    class="required" name="<%= Html.Term("titleExternalCodeIsRequired", "External code is required") %>" 
                    style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Name", "Name") %>:
            </td>
            <td>
                <input id="txtName" type="text" value="<%= Model.Name %>"
                    class="required" name="<%= Html.Term("NameIsRequired", "Name is required") %>" 
                    style="width: 20.833em;" />
            </td>
        </tr>
          <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Phase", "Phase")%>:
            </td>
            <td>
                <%= Html.DropDownList("Titles", (TempData["TitlePhases"] as IEnumerable<SelectListItem>), Html.Term("SelectaTitlePhases", "Select a Title Phase..."), new { id = "cmbTitlePhases", onchange = "on_ChangeTitlePhases()", style = "width: 20.833em;" })%>
            </td>
        </tr>
       <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("SortOrder", "Sort Order")%>:
            </td>
            <td>
                <input id="txtSortOrder" type="number" value="<%= Model.SortOrder %>"
                    class="required" name="<%= Html.Term("SortOrderIsRequired", "Sort Order is required") %>" 
                    style="width: 10.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("SaveTitle", "Save title") %></a>
                    <a href="javascript:void(0);" id="btnAddReqTitle" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("AddRequirementTitle", "Add Requirement Title")%></a>
                    <a href="javascript:void(0);" id="btnAddReqLeg" style="display:inline-block;" class="Button BigBlue">
                        <%= Html.Term("AddRequirementLeg", "Add Requirement Leg")%></a>
                </p>
            </td>
        </tr>
    </table>
    </div>

    <div id="SectionRequirementTitle">
        <div class="Detail">
            <h2 class="FLabel">
			    <%= Html.Term("RequirementTitle", "Requirement Title")%>
            </h2>
            <br />
            <table id="tblReqTitle" class="FormTable" width="100%">
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("TitleRequirement", "Requirement")%>:
                    </td>
                    <td>
                        <%= Html.DropDownList("CalculationTypes", calculationTypes, Html.Term("SelectaRequirement", "Select a Requirement..."), new { id = "ReqTitleCalculationTypeID" })%>
                    </td>
                </tr>

                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("Plan", "Plan")%>:
                    </td>
                    <td>
                        <%= Html.DropDownList("ReqTitlePlan", (TempData["Plans"] as IEnumerable<SelectListItem>), Html.Term("SelectaPlan", "Select a Plan..."), new { id = "ReqTitlePlanID" })%>
                    </td>
                </tr>

                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("MinValue", "Minimum Value")%>:
                    </td>
                    <td>

                       <input id="txtMinValue" type="text" value="" monedaidioma='CultureIPN'
                            class="required" name="<%= Html.Term("titleMinValueIsRequired", "Minimum Value is required") %>" 
                            style="width: 20.833em;" />
                     <%--   <input id="txtMinValue" type="number" value="" monedaidioma='CultureIPN'
                            class="required" name="<%= Html.Term("titleMinValueIsRequired", "Minimum Value is required") %>" 
                            style="width: 20.833em;" />--%>
                    </td>
                </tr>

<%--                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("MaxValue", "Maximum Value")%>:
                    </td>
                    <td>
                        <input id="txtMaxValue" type="number" value=""
                            class="required" name="<%= Html.Term("titleMaxValueIsRequired", "Maximum value is required") %>" 
                            style="width: 20.833em;" />
                        <input type="hidden" id="hddDateEdit" value="" />
                    </td>
                </tr>--%>

                <tr>
                    <td class="FLabel">
                    </td>
                    <td>
                        <p>
                            <a href="javascript:void(0);" id="btnSaveRequirement" style="display:inline-block;" class="Button BigBlue">
                                <%= Html.Term("SaveRequirement", "Save Requirement")%></a>
                        </p>
                    </td>
                </tr>
            </table>



        </div>
        <div class="List">
            <div class="UI-mainBg GridUtility">
                <a href="javascript:void(0);" id="btnDeleteRequirement" style="display:inline-block;" class="clsDelete">
                    <span class="UI-icon icon-x icon-deactive"></span>
                    <%= Html.Term("DeleteSelected", "Delete Selected")%>
                </a>
                <a href="javascript:void(0);" id="btnAddNewRequirement" style="display:inline-block;" >
                    <span class="UI-icon icon-plus icon-activate">
                    </span>
                    <%= Html.Term("AddNewRequirement", "Add New Requirement")%>
                </a>
            </div>
            <table id="gridReqTitle" width="100%" class="DataGrid">
                <thead>
                    <tr class="GridColHead">
                        <th></th>
                        <th><%: Html.Term("TitleRequirement", "Requirement")%></th>
                        <th><%: Html.Term("TitleRequirementName", "Requirement Name")%></th>
                        <th><%: Html.Term("Plan", "Plan")%></th>
                        <th><%: Html.Term("MinValue", "Minimum Value")%></th>
<%--                        <th><%: Html.Term("MaxValue", "Maximum Value")%></th>--%>
                        <th><%: Html.Term("DateCreated", "Date Created")%></th>
                        
                    </tr>
                </thead>
                <tbody>
                        <%
                        if (Model.RequirementTitleCalculations!=null)

						foreach (var item in Model.RequirementTitleCalculations)
								{                                 
						%>
                        <tr>
                            <td><input type="checkbox" class="clsQuit"/></td>
                            <td><%= item.CalculationTypeID %></td>
                            <td><%= calculationTypes.Where(x => x.Value == item.CalculationTypeID.ToString()).First().Text%></td>
                            <td><%= item.PlanID%></td>
                            <td><%= item.MinValue.ToString("N",CoreContext.CurrentCultureInfo)%></td>
<%--                            <td><%= item.MaxValue%></td>--%>
                            <td><%= item.DateModified%></td>
                            
                        </tr>

						<%
								} 
						%>
                </tbody>

            </table>

        </div>
    </div>
    
    <div id="SectionRequirementLeg">
        <div class="Detail">
            <h2>
			    <%= Html.Term("RequirementsLeg", "Requirements Leg")%>
            </h2>
            <br />
            <table id="tblReqLeg" class="FormTable" width="100%">
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("Plan", "Plan")%>:
                    </td>
                    <td>
                        <%= Html.DropDownList("ReqLegPlan", (TempData["Plans"] as IEnumerable<SelectListItem>), Html.Term("SelectaPlan", "Select a Plan..."), new { id = "ReqLegPlanID" })%>
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("TitleRequired", "Title Required")%>:
                    </td>
                    <td>
                        <%= Html.DropDownList("Titles", titleRequirements, Html.Term("SelectaTitle", "Select a Title..."), new { id = "ReqLegTitleID" })%>
                    </td>
                </tr>
                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("TitleGeneration", "Generation")%>:
                    </td>
                    <td>
                        <input id="txtGeneration" type="number" value=""
                            class="required" name="<%= Html.Term("titleGenerationIsRequired", "Generation is required") %>" 
                            style="width: 20.833em;" />
                    </td>
                </tr>

                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("TitleLevel", "Level")%>:
                    </td>
                    <td>
                        <input id="txtLevel" type="number" value=""
                            class="required" name="<%= Html.Term("titleLevelIsRequired", "Level is required") %>" 
                            style="width: 20.833em;" />
                       
                    </td>
                </tr>

                <tr>
                    <td class="FLabel" style="width: 18.182em;">
                        <%: Html.Term("TitleQuantity", "Quantity")%>:
                    </td>
                    <td>
                        <input id="txtQuantity" type="number" value=""
                            class="required" name="<%= Html.Term("titleQuantityIsRequired", "Quantity is required") %>" 
                            style="width: 20.833em;" />
                       
                    </td>
                </tr>
                <tr>
                    <td class="FLabel">
                    </td>
                    <td>
                        <p>
                            <a href="javascript:void(0);" id="btnSaveRequirementLeg" style="display:inline-block;" class="Button BigBlue">
                                <%= Html.Term("SaveRequirementLeg", "Save Requirement Leg")%></a>
                        </p>
                    </td>
                </tr>
            </table>


        </div>
        <div class="List">

            <div class="UI-mainBg GridUtility">
                <a href="javascript:void(0);" id="btnDeleteRequirementLeg" style="display:inline-block;" class="clsDelete">
                    <span class="UI-icon icon-x icon-deactive"></span>
                    <%= Html.Term("DeleteSelected", "Delete Selected")%>
                </a>
                <a href="javascript:void(0);" id="btnAddNewRequirementLeg" style="display:inline-block;" >
                    <span class="UI-icon icon-plus icon-activate">
                    </span>
                    <%= Html.Term("AddNewRequirementLeg", "Add New Requirement Leg")%>
                </a>
            </div>
             <table id="gridReqLeg" width="100%" class="DataGrid">
                <thead>
                    <tr class="GridColHead">
                        <th></th>
                        <th><%: Html.Term("Plan", "Plan")%></th>
                        <th><%: Html.Term("TitleRequired", "Title Required")%></th>
                        <th><%: Html.Term("TitleRequiredName", "Title Required Name")%></th>
                        <th><%: Html.Term("TitleGeneration", "Generation")%></th>
                        <th><%: Html.Term("TitleLevel", "Level")%></th>
                        <th><%: Html.Term("TitleQuantity", "Quantity")%></th>
  
                    </tr>
                </thead>
                <tbody>

                        <%
                        if (Model.RequirementLegs!=null)
                            
                            
                            foreach (var item in Model.RequirementLegs)
								{                                 
						%>
                        <tr>
                            <td><input type="checkbox" class="clsQuit"/></td>
                            <td><%= item.PlanID %></td>
                            <td><%= item.TitleRequired%></td>
                            <td><%= titleRequirements.Where(x => x.Value == item.TitleRequired.ToString()).First().Text%></td>
                            <td><%= item.Generation%></td>
                            <td><%= item.Level%></td>
                            <td><%= item.TitleQTY%></td>
                            
                        </tr>

						<%
								} 
						%>

                </tbody>

            </table>

        </div>
    </div>

</asp:Content>
