<%@ Page Title="" Language="C#"MasterPageFile="~/Areas/CTE/Views/Shared/CTE.Master" %>


<%--//@01 20150817 BR-CC-002 G&S LIB: Se crea la pantalla--%>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        $(function () {
//            $.datepicker.regional['es'] = {
//                closeText: 'Cerrar',
//                prevText: '<Ant',
//                nextText: 'Sig>',
//                currentText: 'Hoy',
//                monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
//                monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
//                dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
//                dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
//                dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
//                weekHeader: 'Sm',
//                dateFormat: 'dd/mm/yy',
//                firstDay: 1,
//                isRTL: false,
//                showMonthAfterYear: false,
//                yearSuffix: ''
//            };

//            $.datepicker.setDefaults($.datepicker.regional['es']);

        });
    </script>

    <link href="../../../../Content/CSS/Validation.css" rel="stylesheet" type="text/css" />
    <script src="../../../../Scripts/jquery.number.min.js" type="text/javascript"></script>
    <script src="../../../../Scripts/jquery.numeric.js" type="text/javascript"></script>
    <script src="../../../../Scripts/Validaciones.js" type="text/javascript"></script>
    <script src="../../../../Scripts/jquery.filestyle.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {

            $('input[type=file]').filestyle({
                buttonText: '',
                buttonWidth: 32,
                buttonHeight: 32,
                inputHeight: 32,
                inputWidth: 350
            });


            $('#fileUpload').change(function () {
                var value = this.value;
                val = value.split("\\");
                var file = (val[val.length - 1]).split('.');
                var ext = file[file.length - 1];
                //
                if (ext == "ret" || ext == "txt") {
                    $("#idMsj").hide();
                    $('#sas').atribute("style:display: none;")
                }
                else {
                    $("#idMsj").show();
                    //showMessage("The selected file is no tvalid", true);
                    $(this).val("");
                    $("#sp0").hide();
                    $("#sp1").hide();
                    $("#sp2").hide();
                    $("#sp3").hide();
                }

            });

            $('#idload').hide();
            //-----------------------------------------------//
            $('#btnCargarInf').click(function () {

                $('#idload').show();

                if ('<%= ViewBag.MsgLoad  %>' == 0 || '<%= ViewBag.MsgLoad  %>' == 1 || '<%= ViewBag.MsgLoad  %>' == 2 || '<%= ViewBag.MsgLoad  %>' == 3) {
                    $('#idload').hide();
                }

            });

            //-----------------------------------------------//

        });

        $(document).ready(function () {
            var options = {
                beforeSend: function () {
                    //$("#progress").show();
                },
                success: function () {
                    //$("#progress").hide();

                },
                complete: function (response) {
                    //$("#message").html("<font color='green'>" + response.responseText + "</font>");
                    if (response.responseText == "uploaded") {
                        showMessage('Ceps Upload!', false);
                    }

                },
                error: function () {
                    //$("#message").html("<font color='red'> ERROR: unable to upload files</font>");
                }
            };

        });

        function loadBrowseBankPayments(bankID, bankConsolidateSec) {
         
            var data = { BankID: bankID, BankConsolidateSec: bankConsolidateSec };

            $.post('/CTE/BankConsolidateApplication/LoadBankID', data, function (response) {
                    
                });


            window.location = '<%= ResolveUrl("~/CTE/BankConsolidateApplication/BrowseBankPayments") %>';

        }
     
    </script>
    <style type="text/css">
        .file-fake
        {
            padding: 6px 2px 7px;
            border: 1px solid #ddd;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["Success"] != null) { %>
    <div id="successMessage">
        <%: ViewData["Success"] %>
    </div>
<% } %>

 <%       
        Dictionary<int, string> dcBank = (ViewBag.dcBank as Dictionary<int, string>) ?? new Dictionary<int, string>();       
     
    %>

   <div class="SectionHeader">
		<h2>
			<%=  Html.Term("BrowseBankConsolidateApplication", "Browse Bank Consolidate  Application")%>
        </h2>
        
        <%--<%= Html.Term("BrowsePaymentRulesConfiguration", "Browse Payment Rules Configuration") %> | <a href="<%= ResolveUrl("~/CTE/InterestRules/Index") %>"><%= Html.Term("GenerateRules", "Generate Rules")%></a>--%>
	</div>
 
   
   <div>
        <% using (Html.BeginForm("LoadInformation", "BankConsolidateApplication", FormMethod.Post, new { id = "frmLoadBulkCeps", enctype = "multipart/form-data" })) %>
        <% { %>
        <!-- Form content goes here -->

        <table id="newCeps" class="FormTable Section" width="100%">
            <tr>
                <td class="FLabel" style="width: 950px;">
                    <%= Html.Term("File")%>:
                </td><td></td>
            </tr>
            <tr>
                <td >
                <table>
                <tr> <td> 
                <input type="file" name="fileUpload" id="fileUpload" />
                </td>
                <td >
                 
                        <input id="btnCargarInf" type="submit" style="display: inline-block; cursor:pointer;" class="Button BigBlue" value="<%=  Html.Term("CargarInformacion", "Cargar Informacion") %>" />
                        <%--   <a href="javascript:void(0);" id="btnSave" style="display:inline-block;" class="Button BigBlue">
                                    <%= Html.Term("Update", "Update")%></a>--%>

                      

                 
                </td>
                <td  >
                  <img id="idload" src="../../../../Content/Images/Icons/loading-blue.gif"
                    class="FR" alt="asas"  />
                </td>

                </tr>
                </table>
                </td>
            </tr>
            <tr id="idMsj" style="display: none;">
                <td>
                    <p style="color:Red;"><small>The selected file is not valid</small></p>
                </td>
            </tr>
       
          
            
          
        </table>
        <% }  %>

          <% if(ViewBag.MsgLoad ==0) { %>
         <span id="sp0" class="ml10 FL" style='color:blue'>
           <%=   Html.Term("BankConsolidateOK", "El archivo finalizo correctamente") %> 
        </span>
        <% } %>

        <% if(ViewBag.MsgLoad ==1) { %>
        <span id="sp1" class="ml10 FL" style='color:red'>
           <%=  Html.Term("BankConsolidateErrorAbonoYaProcesado", "Abono ya procesado") %>
        </span>
        <% } %>

         <% if(ViewBag.MsgLoad ==2) { %>
        <span id="sp2" class="ml10 FL" style='color:red'>
           <%=  Html.Term("BankConsolidateError", "No se  encontro el Nombre del Archivo") %>
        </span>
        <% } %>

        
         <% if(ViewBag.MsgLoad ==3) { %>
        <span id="sp3" class="ml10 FL" style='color:red'>
           <%=  Html.Term("BankConsolidateDebeSelectArch", "Debe seleccionar Archivo") %>
        </span>
        <% } %>

    </div>
    <br />
     

    <% Html.PaginatedGrid<BankConsolidateApplicationSearchData>("~/CTE/BankConsolidateApplication/BrowseBankConsolidateApplication")
            .AutoGenerateColumns()
            .AddSelectFilter(Html.Term("Bank", "Bank"), "BankId", dcBank, startingValue: Convert.ToInt32(Request.QueryString["BankId"]))
            .AddInputFilter(Html.Term("BankConsolidateDatePro", "Date Pro"), "bankConsolidateDatePro", isDateTime: true)
                                                                                        
            //.HideClientSpecificColumns_()            
            .ClickEntireRow()
		    .Render(); 
    %>
</asp:Content>


