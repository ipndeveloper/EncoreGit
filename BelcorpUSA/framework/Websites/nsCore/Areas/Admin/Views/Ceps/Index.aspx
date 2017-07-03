<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master" Inherits="System.Web.Mvc.ViewPage<System.Collections.Generic.List<NetSteps.Data.Entities.CorporateUser>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

    <link href="../../../../Content/CSS/Validation.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>

     <script type="text/javascript">
         var cambio = 0;
         $(document).ready(function () {

             $("#TxtCep").fn_util_validarNumeros();
             $("#TxtCep1").fn_util_validarNumeros();
             $("#aborDdl").hide();
             $("#aDdl").hide();

             $("#btnSave").on("click", function () {
                var p = $(this).parent();
                 showLoading(p);
                 Register($(this));
                 hideLoading(p);

             });

             $("#TxtCity").focus(function () {
                 $("#TxtNeigborhood").clearError();
             });


             $("#TxtNeigborhood").focus(function () {
                 $("#TxtNeigborhood").clearError();
             });

             $("#TxStreet").focus(function () {
                 $("#TxStreet").clearError();
             });


             $("#TxtCep").focus(function () {
                 $("#TxtCep").clearError();
             });

             $("#TxtCep1").focus(function () {
                 $("#TxtCep1").clearError();
             });



             $("#ddlCountry").on("change", function () {

                 if ($(this).val() != "") {
                     $("#ddlCountry").clearError();
                 }

             });

             $("#ddlState").on("change", function () {

                 fn_cargaCombos("#ddlCity", true);
                 $("#ddlNeigborhood").html('');
                 $("#ddlNeigborhood").append('<option >[SELECCIONE]</option>');

                 if ($(this).val() != "") {
                     $("#ddlState").clearError();
                 }
             });

             $("#ddlCity").on("change", function () {

                 if ($(this).val() != "") {
                     $("#ddlCity").clearError();
                     fn_cargaCombos("#ddlNeigborhood", false);

                 } else {
                     $("#ddlNeigborhood").html('');
                     $("#ddlNeigborhood").append('<option >[SELECCIONE]</option>');
                 }

             });
             $("#ddlNeigborhood").on("change", function () {
                 if ($(this).val() != "") {
                     $("#ddlNeigborhood").clearError();
                 }
             });


             fn_cargaCombosState("#ddlState");
             fn_cargaCombos("#ddlCity", true);
             fn_cargaCombos("#ddlNeigborhood", false);



         });

         function fn_agregaCargandoAjax(pIdObjeto) {
             var sIdImage = pIdObjeto.replace('#', '');
             $(pIdObjeto).parent().append('<img id="img_ajax_cargando_' + sIdImage + '" src="<%= ResolveUrl("~/Content/Images/loading.gif") %>" />');
         }

         function fn_removerCargandoAjax(pIdObjeto) {
             var sIdImage = pIdObjeto.replace('#', '');
             $('#img_ajax_cargando_' + sIdImage).remove();
         }
         function fn_cargaCombos(Combo, tipo, valor) { 
         
             var State = $('#ddlState').val();
             var TaxCacheSearchData = new Object();
             TaxCacheSearchData.CountyDefault = tipo;
             TaxCacheSearchData.CountryID = 73;
             TaxCacheSearchData.State = State;
             var objEn = { objTaxt: TaxCacheSearchData };
             var sParams = JSON.stringify(objEn);
             fn_agregaCargandoAjax(Combo);
             $.ajax({
                 type: "POST",
                 //url: '/Ceps/ListCombosTax',
                 url: '<%= ResolveUrl("~/Ceps/ListCombosTax") %>',
                 data: sParams,
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: "false",
                 success: function (response) {
                     if (response.result==true) {

                         $(Combo).html(response.listado);
                     }

                     fn_removerCargandoAjax(Combo);
                 },
                 error: function (resultado) {

                     hideLoading(valor);

                     var error = eval("(" + resultado.responseText + ")");
                 

                 }
             });
         }
          function fn_cargaCombosState(Combo) {
             var TaxCacheSearchData = new Object();
             TaxCacheSearchData.CountryID = 73;
             var objEn = { objTaxt : TaxCacheSearchData };
             var sParams = JSON.stringify(objEn);
             fn_agregaCargandoAjax(Combo);
             $.ajax({
                 type: "POST",
//                 url: '/Ceps/ListCombosState',
                 url: '<%= ResolveUrl("~/Ceps/ListCombosState") %>',
                 data: sParams,
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: "false",
                 success: function (response) {
                    
                     if (response.result == true) {

                         $(Combo).html(response.listado);
                     }
                     fn_removerCargandoAjax(Combo);

                 },
                 error: function (resultado) {
                     

                     var error = eval("(" + resultado.responseText + ")");
                 }
             });


         }
         function Register() {


             $("#TxtCity").clearError();
             $("#TxtNeigborhood").clearError();
             $("#TxStreet").clearError();

             if ($("#ddlCountry").val() == "") {                          
                 $("#ddlCountry").showError("Select a Country");
                 return false;
             }
     
             else if ($("#ddlState").val()=="") {
                 $("#ddlState").showError("Select a State");
                 return false;
             }

             else if ($.trim($("#TxtCity").val()) == "" && $("#TxtCity").is(":visible") == true) {
          
                 $("#TxtCity").showError("Enter a City");
                 return false;
             }

             else if ($.trim($("#TxtCep").val()) == "") {

                 $("#TxtCep").showError("Enter a TxtCep");
                 return false;
             }
             else if ($.trim($("#TxtCep1").val()) == "") {

                 $("#TxtCep1").showError("Enter a TxtCep1");
                 return false;
             }
             else if ($.trim($("#TxtCep").val()).length < 5) {

                 $("#TxtCep").showError("Enter 5 digits");
                 return false;
             }
             else if ($.trim($("#TxtCep1").val()).length < 3) {

                 $("#TxtCep1").showError("Enter 3 digits");
                 return false;
             }


//             else if ($("#ddlCity").val() == "" && $("#ddlCity").is(":visible") == true) {

//                 $("#ddlCity").showError("Select a City");
//                 return false;
//             }
//             else if ($("#ddlNeigborhood").val() == "" && $("#ddlNeigborhood").is(":visible") == true) {
//                 $("#ddlNeigborhood").showError("Select a County");
//                 return false;
//             }
//             else if ($("#TxtNeigborhood").val() == "" && $("#TxtNeigborhood").is(":visible") == true) {
//                 $("#TxtNeigborhood").showError("Enter a County");
//                 return false;
//             }
             else if ($("#TxStreet").val() == "") {
                 $("#TxStreet").showError("enter a Street");
                 return false;
             }


                var oTaxCache = new Object();

                oTaxCache.CountryID= $("#ddlCountry").val();
                oTaxCache.Active = $("#chkActive").is(':checked');
                oTaxCache.StateAbbreviation = $("#ddlState").val();
                oTaxCache.State = $("#ddlState option:selected").text();
                oTaxCache.PostalCode = $("#TxtCep").val() + $("#TxtCep1").val();
                oTaxCache.City = $("#TxtCity").is(":visible") == true ? $("#TxtCity").val() : ($("#ddlCity option:selected").text() == "[SELECCIONE]" ? '' : $("#ddlCity option:selected").text());
                oTaxCache.County = $("#TxtNeigborhood").is(":visible") == true ? $("#TxtNeigborhood").val() : ($("#ddlNeigborhood option:selected").text() == "[SELECCIONE]" ? '' : $("#ddlNeigborhood option:selected").text());
                oTaxCache.Street = $("#TxStreet").val();
                oTaxCache.CountyFIPS = "dsd";
                oTaxCache.Latitude=90
                oTaxCache.Longitude = 90


                oTaxCache.DateCreatedUTC ="1900-01-01";
                oTaxCache.DateCachedUTC ="1900-01-01";
                oTaxCache.EffectiveDateUTC ="1900-01-01";
                oTaxCache.ExpirationDateUTC = "1900-01-01";
                oTaxCache.DataVersion = "1900-01-01";


              var objEn = { objTaxt : oTaxCache };
             var sParams = JSON.stringify(objEn);
             $.ajax({
                 type: "POST",
//                 url: '/Ceps/Register',
                 url: '<%= ResolveUrl("~/Ceps/Register") %>',
                 data: sParams,
                 contentType: "application/json; charset=utf-8",
                 dataType: "json",
                 async: "false",
                 success: function (response) {
                     if (response.result) {
                         ClearControl();
                         showMessage('Ceps saved!', false);
                     } else {
                         showMessage(response.message, true);
                     }
                 },
                 error: function (resultado) {

                 }
             });

          }

              function AddClassRequierd() {
                    
                    var t=   $("#TxtCity").val();
                    if ($('#TxtCity').is(':visible')) 
                    {
                        $("#ddlCity").val("1");
                    }

                    if (!$('#ddlCity').is(':visible')) 
                    {
                        $("#ddlCity option[value='1']").remove();
                        $("#ddlCity").append('<option value=1>My option</option>');
                        $("#ddlCity").val("1");
 
                    }
 

                 }


          function fn_ChangeNeigborhood(tipo) {
              if (tipo == 1) {
                  $("#TxtNeigborhood").hide();
                  $("#ddlNeigborhood").show();

                  $("#aborTxt").show();
                  $("#aborDdl").hide();
              }
              else {


                  $("#TxtNeigborhood").show();
                  $("#ddlNeigborhood").hide();
                  $("#aborTxt").hide();
                  $("#aborDdl").show();


              }

          }

          function fn_ChangeCity(tipo) {
              if (tipo == 1) {
                  $("#TxtCity").hide();
                  $("#ddlCity").show()
                  $("#aTxt").show();
                  $("#aDdl").hide();
                  
              }
              else {
                  $("#ddlCity").trigger("change");
                  $("#TxtCity").show();
                  $("#ddlCity").hide();
                  $("#aTxt").hide();
                  $("#aDdl").show();

              }
          }


          function ClearControl() {
              $(":text").each(function (index) {
                  $(this).val('');
              });
         
              $("#ddlState").val("");
              $("#ddlCity").html('');
              $("#ddlCity").append('<option >[SELECCIONE]</option>');

              $("#ddlNeigborhood").html('');
              $("#ddlNeigborhood").append('<option >[SELECCIONE]</option>');
          };
     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
		<%= Html.Term("Admin", "Admin") %></a> > <%= Html.Term("Users") %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
		<h2>
			<%= Html.Term("Add New Ceps")%>
		</h2>
		<a href="<%= ResolveUrl("~/Admin/Ceps/LoadBulk") %>"><%= Html.Term("CepsUdpate", "Ceps Udpate")%></a>|<%= Html.Term("Add New Ceps")%>
	</div>

    

    <div style="margin:20px;">
	   <table id="newCeps" class="FormTable Section" width="100%">
       <tr >
            <td class="FLabel" style="width:950px;">
                    <%= Html.Term("Country")%>:
            </td>
   
       </tr>
		<tr >
			
			<td  style="width:950px;">



                <select  id="ddlCountry"class="required" name="Country is required." >
                <option selected="selected" value="">[SELECCIONE]</option> 
                 <option selected="selected" value="73">Brazil</option> 
                </select>
				&nbsp;&nbsp; &nbsp;<%= Html.Term("Active")%>: <input  type="checkbox" id="chkActive" />
			</td>
		</tr>
        <tr >
         	<td class="FLabel" style="width:950px;">
				<%= Html.Term("State")%>:
			</td>
        </tr>
       
	    <tr >
        <td>
          <table>
                <tr>
                    <td>
                        <select  id="ddlState"class="required" name="Code Material is required." >
                        <option selected="selected" value="">[SELECCIONE]</option> 
            
                        </select>
			
                    </td>
 
                    <td>
                    <%= Html.Term("CEP")%>: <input  type="text" id="TxtCep"  name="Code Material is required." style="width:90px;" maxlength ="5" />&nbsp; - <input  type="text" id="TxtCep1" style="width:40px;"class="required" name="Code Material is required." maxlength ="3"/>
                    </td>
                </tr>
            
            </table>
        
        </td>
          




		</tr>
        <tr >
        	<td class="FLabel" style="width:950px;">
				<%= Html.Term("City")%>:
			</td>
        
        </tr>
        <tr >
		
			<td  style="width:800px;">

               <table>
                   <tr>
                        <td>
                            <select  id="ddlCity" name="Select city is required." >
                            <option selected="selected" value="">[SELECCIONE]</option> 
                           
                            </select>
                            <input  type="text" id="TxtCity"  name="Code Material is required."   style="display:none;"/>
                        </td>
                   
                        <td>
                         <a href="javascript:void(0);" id="aTxt" onclick="fn_ChangeCity(0);"><%= Html.Term("AddNewCity", "Add New City")%></a><a href="javascript:void(0);"  onclick="fn_ChangeCity(1);" id="aDdl"><%= Html.Term("Cancel", "Cancel")%></a>
                        </td>
                   </tr>

               
               </table>
        
                
                
                </td>
		</tr>

        <tr >
        	<td class="FLabel" style="width:950px;">
				<%= Html.Term("Neigborhood")%>:
			</td>
        
        
        </tr>
        <tr style="width:800px;">
          <td>
          
          <table>
              <tr>
                    <td>
                        <select  id="ddlNeigborhood" name="Code Material is required1233."  >
                        <option selected="selected" value="">[SELECCIONE]</option> 
                       
                        </select>
                        <input  type="text" id="TxtNeigborhood"  name="Code Material is required."  style="display:none;"/>
                    </td>
                    <td>
                  <a href="javascript:void(0);" onclick="fn_ChangeNeigborhood(0);" id="aborTxt"><%= Html.Term("AddNewNeigborhood", "Add New Neigborhood")%></a><a href="javascript:void(0);"  onclick="fn_ChangeNeigborhood(1);" id="aborDdl"><%= Html.Term("Cancel", "Cancel")%></a>
                    </td>
              </tr>
           
           </table>
          
          
          </td>
           
		
    
        </tr>


        <tr style="width:950px;">
                <td class="FLabel">
                <%= Html.Term("Street")%>:
                </td>
        </tr>
           <tr style="width:950px;">
		
			<td>
                <input  type="text" id="TxStreet" class="required" name="Code Material is required." /> </td>
		</tr>

                <tr >
   
                    <td>
                        <p>
                            <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                            <%= Html.Term("SaveCeps", "Save Ceps")%></a>
                        </p>
                    </td>
                </tr>
	</table>
      </div>
</asp:Content>
