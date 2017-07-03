<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/OrdersPaypal.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadCrumbContent" runat="server">
<a href="<%= ResolveUrl("~/Orders") %>"><%= Html.Term("Orders") %></a> > 
	<%= Html.Term("AutoshipRunOverview", "Autoship Run Overview") %>

   <%-- <style type="text/css">
        
        #checkfrom {        
         width : 100px;
         background-color:Red;
         
        }
    </style> --%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       

<script src="../../../../Scripts/jquery.maskedinput.js" type="text/jscript" ></script>
<script type="text/javascript">



    $(function () {


        $('#txtName').bind('keypress', function (e) {
            return (e.which != 8 && e.which != 0 && e.which != 32 && (e.which < 65 || e.which > 122) && (e.which != 91 && e.which != 92 && e.which != 93
                     && e.which != 94 && e.which != 95 && e.which != 96)) ? false : true;
        });

        $('#txtTypeNumber').bind('keypress', function (e) {
            return (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57) && e.which != 46) ? false : true;
        });

     

        $("#sIdTypes").change(function () {
       
            $("#Aviso02").hide();
            var value = $(this).val();
            var TypeNumber = $('#txtTypeNumber').val();

            //  CPF -- ID  8;   11  carateres
            if (value == 8)
                $("#txtTypeNumber").mask("999.999.999-99");

            //RG  --ID 4 ;   9 carateres
            if (value == 4)
                $("#txtTypeNumber").mask("99.999.999-9");

            if (value == 9)//PIS
                $("#txtTypeNumber").mask("999.999.999-9");



            if (value == 0 &&  TypeNumber != '') {
                $("#msgName").show();
                $("#Error02").show();
                $("#Correcto02").hide();

                $(this).focus()

            }
            else {


                $('#txtTypeNumber').focus();

            }
        });

    });

    $(document).ready(function () {



        $('#btnContinuar').addClass("ButtonOff");

        // mascara de TypeNumber


        // NAME
        $('#txtName').on('blur', function () {
            $("#Aviso01").hide();

            var control = $(this);
            if (control.val() == '') {
                $("#msgName").show();
                $("#Error01").show();
                $("#Correcto01").hide();

                MostrarBotonContinuar();
            }
           
        }).keydown(function () {
            $("#Aviso01").hide();

            $("#msgName").hide();
            $("#Error01").hide();
            $("#Correcto01").show();

          
           
        });


        // TYPE NUMBER
        $('#txtTypeNumber').on('blur', function () {
            $("#Aviso02").hide();

            var stype = $("#sIdTypes").val();
            var control = $(this);

            if (control.val() == '' || stype == 0) {

                $("#msgTypeNumber").show();
                $("#Error02").show();
                $("#Correcto02").hide();
            }

            if (control.val() != '' && stype != '0') {
                var numsinMask = control.val().replace('.', '').replace('.', '').replace('-', '');

                //PIS  --ID  9 ; 10 carateres
                //  CPF -- ID  8;   11  carateres    
                if (stype == 8 && numsinMask.length == 11) {
                    DocumentValidation(stype, numsinMask);
                }
                //RG  --ID 4 ;   9 carateres  
                if (stype == 4 && numsinMask.length == 9) {
                    DocumentValidation(stype, numsinMask);
                }
                //PIS  --ID  9 ; 10 carateres
                if (stype == 9 && numsinMask.length == 10) {
                    DocumentValidation(stype, numsinMask);
                }
            }
            MostrarBotonContinuar();
        }).keydown(function () {

            $("#Aviso02").hide();

            if ($('#txtTypeNumber').val() == '' || $("#sIdTypes").val() == 0) {

                $("#msgTypeNumber").show();
                $("#Error02").show();
                $("#Correcto02").hide();

               
            }
            MostrarBotonContinuar();
        });



        //  CORREOS:
        $('#txtEmail').on('blur', function () {
            $("#Aviso03").hide();
            if ($.trim($(this).val()) == '') {
                $("#msgEmail").show();
                $("#Error03").show();
                $("#Correcto03").hide();
            }
            else {
                EmailValidation($(this).val());
                MostrarBotonContinuar();
            }

        }).keyup(function () {
            $("#Aviso03").hide();

            if ($('#txtEmail').val() == '') {
                $("#msgEmail").show();
                $("#Error03").show();
                $("#Correcto03").hide();               
            }
            MostrarBotonContinuar();
        });
    });

    function MostrarBotonContinuar() {
        if ($('#Correcto01').is(':visible') && $('#Correcto02').is(':visible') && $('#Correcto03').is(':visible')) {
            //            alert('esta oculto')
            $('#btnContinuar').removeClass("ButtonOff");
           
        } else {
            $('#btnContinuar').addClass("ButtonOff");
        }

    }

      function EmailValidation(Email) {
                   var strURL = '<%= ResolveUrl("~/EntryPayPal/EmailValidation") %>';
                   var Parameters = {
                       Email: Email
                   };
                   $.ajax({
                       type: 'POST',
                       url: strURL,
                       data: JSON.stringify(Parameters),
                       contentType: 'application/json; charset=utf-8',
                       dataType: 'json',
                       success: function (data) {
                           if (data.result) {
                               $("#msgEmail").hide();
                               $("#Error03").hide();
                               $("#Correcto03").show();
                           }
                           else {
                               $("#msgEmail").show();
                               $("#Error03").show();
                               $("#Correcto03").hide();
                           }
                       }
                   });
               }


               function DocumentValidation(DocumentType, DocumentValue) {



                   var strURL = '<%= ResolveUrl("~/EntryPayPal/DocumentValidation") %>';
                   var Parameters = {
                       DocumentType: DocumentType,
                       DocumentValue: DocumentValue
                   };

                   $.ajax({
                       type: 'POST',
                       url: strURL,
                       data: JSON.stringify(Parameters),
                       contentType: 'application/json; charset=utf-8',
                       dataType: 'json',
                       success: function (data) {
                           if (!data.result) {
                               $("#msgTypeNumber").show();
                               $("#Error02").show();
                               $("#Correcto02").hide();
                           } else {
                               $("#msgTypeNumber").hide();
                               $("#Error02").hide();
                               $("#Correcto02").show();
                           }

                       }
                   });
               }

 </script>
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("Pagamento", "Pagamento") %></h2>
	</div>
    
<div id="paypalID">
	
    
	<table style="height: 200px;">
		
		<tr>
			<td>
			 <table>
                  <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>                
                <%= Html.Term("PayPalName", "Nome impreso no Cartao")%>:
            </td>
            <td> 
                <input type="text" maxlength="100" id="txtName"  class="required"style="width: 250px;" tabindex="101" />
                <br />
                <span class="ml10 FL" style='color:red; display:none' id="msgName">
                  <%=  Html.Term("DigiteInformacionSolicitada", "Digite la informacion solicitada") %>
                </span>
                <%--<span title="Primary URL" id="checkfrom"  class="UI-icon icon-check"   style="border-top-color:Red;"  ></span>
                 <span title="Primary URL" id="SpanErrorFrom"  class="UI-icon icon-exclamation"></span>--%>
              
            </td>
              <td>
                 <img  id="Aviso01"  src="../../../../FileUploads/Paypal/AvisoLlenar.png" 
                  alt="Por favor, informe o nome completo como aparece no cartao"/>
                 <img  id="Error01" style="display:none" src="../../../../FileUploads/Paypal/AdvertenciaError.png" alt="Revise por favor" />                 
                 <img  id="Correcto01" style="display:none" src="../../../../FileUploads/Paypal/CheckVerde.png"  alt="" />
              </td>
        
        </tr>

            <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>                
                <%= Html.Term("PayPalType", "CPF ou CNPJ do titular do Cartao")%>:
            </td>
            <td>

            <select id="sIdTypes" tabindex="102">
            <option value="0" >
				
			</option>
			<%
                foreach (var item in ViewBag.listIDTypes)
					{                                 
			%>
			<option value="<%= item.IDTypeID %>" >
				<%= item.Descriptions%>
			</option>
			<%
					} 
			%>
		</select>


                <input type="text" maxlength="100" id="txtTypeNumber"  class="required"style="width: 250px;" tabindex="103" />
                <br />
                <span class="ml10 FL" style='color:red; display:none' id="msgTypeNumber">
                  <%=  Html.Term("DigiteInformacionSolicitada", "Digite la informacion solicitada") %>
                </span>
            </td>
            <td>
                 <img  id="Aviso02"  src="../../../../FileUploads/Paypal/AvisoLlenar.png" 
                        alt="Por favor, informe o documento do propietario do cartao" />
                 <img  id="Error02" style="display:none" src="../../../../FileUploads/Paypal/AdvertenciaError.png" alt="Revise por favor" />                 
                 <img  id="Correcto02" style="display:none"  src="../../../../FileUploads/Paypal/CheckVerde.png"  alt="" />
              </td>
        </tr>
         <tr>
            <td class="FLabel">
                <span class="requiredMarker">*</span>
                 <%= Html.Term("PayPalEmail", "Email do titular do Cartao")%>:
            </td>
            <td>
                <input type="text" maxlength="100" id="txtEmail" style="width: 250px;" tabindex="104" />
                   <br />
                <span class="ml10 FL" style='color:red; display:none' id="msgEmail">
                  <%=  Html.Term("DigiteInformacionSolicitada", "Digite la informacion solicitada") %>
                </span>
            </td>
            <td>
                 <img  id="Aviso03"  src="../../../../FileUploads/Paypal/AvisoLlenar.png" 
                 alt="Por favor, informe su e-mail" />
                 <img  id="Error03" style="display:none" src="../../../../FileUploads/Paypal/AdvertenciaError.png" alt="Revise por favor" />                 
                 <img  id="Correcto03" style="display:none" src="../../../../FileUploads/Paypal/CheckVerde.png"  alt="" />
             </td>
        </tr>

         <tr>
            <td class="FLabel">
                
            </td>
            <td>
                 <a href="javascript:void(0);" id="btnContinuar" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("Continuar", "Continuar")%></a>

                        <a href="javascript:void(0);" id="btnCancelar" style="display: inline-block;" class="Button BigBlue  ">
                        <%= Html.Term("Cancelar", "Cancelar")%></a>
            </td>
            <td>
               
             </td>
        </tr>
        </table>
			</td>
		</tr>
		
	</table>

  
</div>
</asp:Content>

