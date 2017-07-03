<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/CTE/Views/Shared/FInterestRulesManagement.Master" 
    Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Accounts.Models.Browse.AccountBrowseModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">
        function PopupSaldoAsignar(AccountID, CreditUtilizado, S) {

              
                $('#hdnCreditUtilizado').val(CreditUtilizado);
                $('#hdnSite').val(S);
                $('#txtAccountID').val(AccountID);
                $('#txtSaldoAsignar').val('');
                $("#trAccount").css("display", "none");

                $("#noteModal").css("display", "block");


            }

            function Ocultar() {
                $("#noteModal").css("display", "none");

                if ($('#hdnSite').val() == 1 ) {

                         window.location = '<%= ResolveUrl("~/CTE/CreditManagements/Index/") %>';
                 }
            };
            function OcultarLog() {
                $("#noteModalLog").css("display", "none");
            };

            function SetearAccountCredit() {
              
            var contador = 0;
            $('#paginatedGrid tbody:first tr').each(function (i) {
                if ($(this).find('td:first input[type=checkbox ]').is(':checked')) {

                    if (contador == 0) {

                        $('#txtAccountID').val($(this).find('td:first input[type=checkbox]').val());
                        $(this).find('td:first input[type=checkbox]').attr('checked', false); ;
                    }
                    contador = contador + 1;
                }
            });

            $('#hdnContador').val(contador);
        };

        $(function () {

            $('#txtSaldoAsignar').numeric();
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

            $('#AsignarCredito').click(function () {
                $("#txtAccountID").prop("disabled", "");
                $('#hdnSite').val('2');
                $('#txtAccountID').val('');
                $('#txtSaldoAsignar').val('');
                $("#trAccount").css("display", "block");
                $("#noteModal").css("display", "block");

            });


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


            var accountId = $('<input type="hidden" id="accountIdFilter" class="Filter" />');
            $('#accountNumberOrNameInputFilter').change(function () {
                accountId.val('');
            });

            $('#accountNumberOrNameInputFilter').removeClass('Filter').after(accountId).css('width', '275px')

				.watermark('<%=Html.JavascriptTerm("AccountSearch", "Look up account by ID or name")%>')
				.jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {

				    accountId.val(item.id);
				}, minCharacters: 3, ajaxResults: true, maxResults: 50, showMore: true
				});




            $('#exportToExcel').click(function () {


                var grid = $('#paginatedGrid');

                var _orderByDirection = 0;
                var _orderBy = '';

                $(grid.find("thead > tr > th")).each(function () {

                    var th = $(this);

                    if (th.hasClass("Ascending")) {


                        orderByDirection = 0;
                        _orderBy = th.attr("id");
                    }

                    if (th.hasClass("Descending")) {


                        orderByDirection = 1;
                        _orderBy = th.attr("id");
                    }


                });


                var _accountID = $('#accountIdFilter').val();

                var _status = $('#statusSelectFilter').val();
                var _type = $('#typeSelectFilter').val();
                var _state = $('#stateSelectFilter').val();
                var _city = $('#cityInputFilter').val();
                var _country = $('#countrySelectFilter').val();
                var _title = $('#titleSelectFilter').val();
                var _email = $('#emailInputFilter').val();
                var _sponsorId = $('#sponsorIdFilter').val();
                var _phone = $('#phoneInputFilter').val();
                var _siteUrl = $('#siteUrlInputFilter').val();
                var _postalCode = $('#postalCodeInputFilter').val();
                var _startDate = $('#startDateInputFilter').val();
                var _endDate = $('#endDateInputFilter').val();

                var d = new Date();

                var url = '<%= ResolveUrl("~/CTE/CreditManagements/ExportAccountCredit") %>';
                var parameters = {
                    page: 0,
                    pageSize: 20000, //parseInt(pagecount.toString()),
                    status: _status,
                    type: _type,
                    state: _state,
                    city: _city,
                    postalCode: _postalCode,
                    country: _country,
                    email: _email,
                    coApplicant: '',
                    sponsorId: _sponsorId,
                    title: _title,
                    startDate: _startDate,
                    endDate: _endDate,
                    accountID: _accountID,
                    siteUrl: _siteUrl,
                    phone: _phone,
                    orderBy: _orderBy,
                    orderByDirection: _orderByDirection

                };
                url = url + '?' + $.param(parameters).toString();
                $("#frmExportar").attr("src", url);
            });


            $('#BloquearCredit').click(function () {


                var data = {}, t = $(this);
                var j = 0;
                $('#paginatedGrid tbody:first tr').each(function (i) {
                    if ($(this).find('td:first input[type=checkbox ]').is(':checked')) {

                        data['items[' + j + ']'] = $(this).find('td:first input[type=checkbox]').val();
                        j++;
                    }
                });

                if (j > 0) {
                    $.post('/CTE/CreditManagements/BloquearCredit', data, function (response) {
                        if (response.result) {
                            showMessage(response.message, false);
                            window.location = '<%= ResolveUrl("~/CTE/CreditManagements/Index/") %>';
                        } else {
                            showMessage(response.message, true);

                        }
                    });


                } else {
                    showMessage('<%=Html.JavascriptTerm("SelecAccount", "Select the Account")%>', true);
                }

            });

            $('#DesBloquearCredit').click(function () {

                var data = {}, t = $(this);
                var j = 0;
                $('#paginatedGrid tbody:first tr').each(function (i) {
                    if ($(this).find('td:first input[type=checkbox ]').is(':checked')) {
                        data['items[' + j + ']'] = $(this).find('td:first input[type=checkbox]').val();
                        j++;
                    }
                });

                if (j > 0) {
                    $.post('/CTE/CreditManagements/DesBloquearCredit', data, function (response) {
                        if (response.result) {
                            showMessage(response.message, false);
                            window.location = '<%= ResolveUrl("~/CTE/CreditManagements/Index/") %>';
                        } else {
                            showMessage(response.message, true);

                        }
                    });

                } else {
                    showMessage('<%=Html.JavascriptTerm("SelecAccount", "Select the Account")%>', true);
                }

            });


            $('#SaldoCero').click(function () {
                var data = {}, t = $(this);
                var j = 0;
                $('#paginatedGrid tbody:first tr').each(function (i) {
                    if ($(this).find('td:first input[type=checkbox ]').is(':checked')) {
                        data['items[' + j + ']'] = $(this).find('td:first input[type=checkbox]').val();
                        j++;
                    }
                });



                if (j > 0) {
                    $.post('/CTE/CreditManagements/SaldoCero', data, function (response) {
                        if (response.result) {
                            showMessage(response.message, false);
                            window.location = '<%= ResolveUrl("~/CTE/CreditManagements/Index/") %>';
                        } else {
                            showMessage(response.message, true);

                        }
                    });
                } else {
                    showMessage('<%=Html.JavascriptTerm("SelecAccount", "Select the Account")%>', true);
                }

            });


            $('#AsignarNewSaldo').click(function () {


                $('#hdnSite').val('1');
                var data = {};
                //  $("#noteModal").css("display", "block");
                var data = {}, t = $(this);


                var contador = 0;
                $('#paginatedGrid tbody:first tr').each(function (i) {
                    if ($(this).find('td:first input[type=checkbox ]').is(':checked')) {
                        data['items[' + contador + ']'] = $(this).find('td:first input[type=checkbox]').val();
                        if (contador == 0) {

                            $('#txtAccountID').val($(this).find('td:first input[type=checkbox]').val());
                            $(this).find('td:first input[type=checkbox]').attr('checked', true);
                        }
                        contador = contador + 1;

                    }
                });

                if (contador == 0) {
                    showMessage('<%=Html.JavascriptTerm("SelecAccount", "Select the Account")%>', true);
                } else {
                    $.post('/CTE/CreditManagements/SetAccountSel', data, function (response) {
                        if (response.result) { }

                    });

                    $("#txtAccountID").prop("disabled", "disabled");
                    $("#noteModal").css("display", "block");
                    $('#hdnContador').val(contador);
                }
              


            });

            $('#btnSaldoAsignar').click(function () {



                var Accountid = $('#txtAccountID').val();
                // var CUtilizado = $('#hdnCreditUtilizado').val();
                var SAsignado = $('#txtSaldoAsignar').val();
                var S = $('#hdnSite').val();
                var data = {};              

                if (SAsignado <= 0)
                    showMessage('<%=Html.JavascriptTerm("ValidateMayorCero", "El Saldo debe ser mayor a cero")%>', true);
                else {

                    data = {
                        AccountID: $('#txtAccountID').val(),
                        NewSaldo: $('#txtSaldoAsignar').val(),
                        S: $('#hdnSite').val(),
                        contador: $('#hdnContador').val()
                    };



                    $.post('/CTE/CreditManagements/UpdateSaldoAsignar', data, function (response) {

                        if (response.result) {
                            showMessage(response.message, false);
                        }
                        else {
                            showMessage(response.message, true);
                        }

                        if (response.contador == 0) {
                            Ocultar();
                            window.location = '<%= ResolveUrl("~/CTE/CreditManagements/Index/") %>';
                        }
                        else {
                            $('#hdnContador').val(response.contador);
                            $('#txtAccountID').val(response.AccountID);
                            $('#txtSaldoAsignar').val('');
                        }

                    });
                }

            });


            $('#AsignarSaldoAnterior').click(function () {
                var data = {}, t = $(this);
                var j = 0;
                $('#paginatedGrid tbody:first tr').each(function (i) {
                    if ($(this).find('td:first input[type=checkbox ]').is(':checked')) {
                        data['items[' + j + ']'] = $(this).find('td:first input[type=checkbox]').val();
                        j++;
                    }
                });
                if (j > 0) {
                    $.post('/CTE/CreditManagements/AsignarSaldoAnterior', data, function (response) {
                        if (response.result) {
                            showMessage(response.message, false);
                            window.location = '<%= ResolveUrl("~/CTE/CreditManagements/Index/") %>';
                        } else {
                            showMessage(response.message, true);
                            window.location = '<%= ResolveUrl("~/CTE/CreditManagements/Index/") %>';
                        }
                    });

                } else {
                    showMessage('<%=Html.JavascriptTerm("SelecAccount", "Select the Account")%>', true);
                }
            });

            var sponsorSelected = false;
            var sponsorId = $('<input type="hidden" id="sponsorIdFilter" class="Filter" />');
            $('#sponsorInputFilter').removeClass('Filter').css('width', '275px')
            .watermark('<%= Html.JavascriptTerm("AccountSearch", "Look up account by ID or name") %>')
            .jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                sponsorId.val(item.id);
                sponsorSelected = true;
            }, minCharacters: 3, source: $('#sponsorFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {

                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    sponsorId.val('');
                } else if (!sponsorSelected) {
                    sponsorId.val('-1');
                }

                sponsorSelected = false;
            }).after(sponsorId);

            $('#emailInputFilter').css('width', '200px');

            if ('<%= Model.Sponsor.AccountID %>' > 0) {
                $('#sponsorInputFilter').val('<%: Model.Sponsor.FullName + " (#" + Model.Sponsor.AccountNumber + ")" %>');
                sponsorId.val('<%= Model.Sponsor.AccountID %>');
            }

            var cityValue = $('<input type="hidden" id="cityFilter" class="Filter" />');
            $('#CityInputFilter').removeClass('Filter').css('width', '120px').watermark('<%= Html.JavascriptTerm("CitySearch", "Look up city") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Browse/SearchCity") %>', { onSelect: function (item) {
                cityValue.val(item.text);
            }, minCharacters: 3, source: $('#cityFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {
                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    cityValue.val('');
                }
            }).after(cityValue);

            var postalCodeValue = $('<input type="hidden" id="postalCodeFilter" class="Filter" />');
            $('#PostalCodeInputFilter').removeClass('Filter').css('width', '120px').watermark('<%= Html.JavascriptTerm("PostalCodeSearch", "Look up postal code") %>').jsonSuggest('<%= ResolveUrl("~/Accounts/Browse/SearchPostalCode") %>', { onSelect: function (item) {
                postalCodeValue.val(item.text);
            }, minCharacters: 3, source: $('#postalCodeFilter'), ajaxResults: true, maxResults: 50, showMore: true
            }).blur(function () {
                if (!$(this).val() || $(this).val() == $(this).data('watermark')) {
                    postalCodeValue.val('');
                }
            }).after(postalCodeValue);

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
  <div id="noteModal" class="jqmWindow LModal" style="left: 700px; z-index: 800; top: 100px;" >

        <div class="mContent">       
         <table id="campaignAction" class="FormTable" width="430px">
            <tr id="valuePanel">          
            <td id="taskAdd"> 

            <table>
             <tr id="trAccount">
            	<td class="FLabel">
					<%= Html.Term("Account", "Account")%>:
				
			     <input id="txtAccountID" type="text" class="clear required justNumbers" size="60px"  maxlength="50"  />
			   </td>
            </tr>
            <tr>
            	<td class="FLabel">
					<%= Html.Term("SaldoAsignar", "Saldo Asignar")%>:
				
              
                <input type="hidden"  id="hdnCreditUtilizado"  />
                  <input type="hidden"  id="hdnContador"  value="0" />
             <input type="hidden"  id="hdnSite"  />
			     <input id="txtSaldoAsignar" type="text" class="clear required" size="60px"  maxlength="50"  />
			   </td>
            </tr>
             
            </table>
            </td>
            </tr>
            </table>
            <p>

              <a href="javascript:void(0);" class="Button LinkCancel jqmClose" 
                    id="A1" onclick="Ocultar()">
                    <%= Html.Term("Close","Close")%></a>

               
                <a  id="btnSaldoAsignar" href="javascript:void(0);" class="Button BigBlue">
                    <%= Html.Term("Aceptar", "Aceptar") %></a>
            
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>


    <div id="noteModalLog" class="jqmWindow LModal" style="left: 510px; z-index: 3000; top: 100px;" >

        <div class="mContent">
            <h2>
                <%= Html.Term("AuditoriaConsultora", "Auditoria por Consultora")%>
            </h2>
            <table id="Table1" class="FormTable" width="600px">
    <tr id="Tr1">          
            <td id="Td1"> 

            <table class="DataGrid" width="80%">

               <thead>
                        <tr class="GridColHead">
                           
                            <th >
                                <%= Html.Term("CreditLogUser", "User Name")%>
                            </th>                                                   
                            <th>
                                <%= Html.Term("CreditLogType", "Type")%>
                            </th> 
                            
                            <th>
                                <%= Html.Term("CreditLogAmount", "Amount")%>
                            </th>                           
                            <th>
                                <%= Html.Term("CreditLogDate", "Date")%>
                            </th>   
                            
                           
                            
                                                        
                        </tr>
                    </thead>
               <tbody id="tblinformacion">
               
                  
               </tbody>
            </table>
            </td></tr></table>
            <p>
                <a href="javascript:void(0);" class="Button LinkCancel jqmClose" 
                    id="btnCancelObservacion" onclick="OcultarLog()">
                    <%= Html.Term("Close","Close")%></a>
            </p>
            <span class="ClearAll"></span>
        </div>
    </div>
</asp:Content>
<%--<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>--%>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("BrowseAccountsCredit", "Browse Accounts Credit") %>
        </h2>
    </div>
    <% 
        Html.PaginatedGrid<AccountCreditSearchData>("~/CTE/CreditManagements/GetAccounts")
			.AutoGenerateColumns()
            .HideClientSpecificColumns()
             .AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"), "accountNumberOrName")
			.AddData("accountNumberOrName", ViewData["q"])
            .AddSelectFilter(Html.Term("Status"), "status", new Dictionary<string, string>() { { "", Html.Term("SelectaStatus", "Select a Status...") } }.AddRange(Model.StatusList), startingValue: Model.SearchParameters.AccountStatusID.ToString())
            .AddSelectFilter(Html.Term("Type"), "type", new Dictionary<string, string>() { { "", Html.Term("SelectaType", "Select a Type...") } }.AddRange(Model.AccoutTypeList), startingValue: Model.SearchParameters.AccountTypes != null && Model.SearchParameters.AccountTypes.Count() > 0 ? Model.SearchParameters.AccountTypes.First().ToString() : null)
            .AddSelectFilter(Html.Term("State"), "state", new Dictionary<string, string>() { { "", Html.Term("SelectaState", "Select a State...") } }.AddRange(Model.StateList), startingValue: Model.SearchParameters.StateProvinceID.ToString())
            .AddSelectFilter(Html.Term("Country"), "country", new Dictionary<string, string>() { { "", Html.Term("SelectCountry", "Select Country...") } }.AddRange(Model.CountryList), startingValue: Model.SearchParameters.CountryID.ToString())
            .AddSelectFilter(Html.Term("Title"), "title", new Dictionary<string, string>() { { "", Html.Term("SelectTitle", "Select Title...") } }.AddRange(Model.TitleList), startingValue: Model.SearchParameters.TitleID.ToString())
            .AddInputFilter(Html.Term("Email"), "email", Model.SearchParameters.Email, addBreak: true)
			.AddInputFilter(Html.Term("Sponsor"), "sponsor")
            .AddInputFilter(Html.Term("City"), "city", startingValue: Model.SearchParameters.City)
            .AddInputFilter(Html.Term("Phone"), "phone", startingValue: Model.SearchParameters.PhoneNumber)
            .AddInputFilter(Html.Term("SiteUrl", "Site Url"), "siteUrl", startingValue: Model.SearchParameters.SiteUrl)
            .AddInputFilter(Html.Term("PostalCode", "Postal Code"), "postalCode", startingValue: Model.SearchParameters.PostalCode)
            .AddInputFilter(Html.Term("EnrollmentDateRange", "Enrollment Date Range"), "startDate", startingValue: Model.SearchParameters.StartDate, isDateTime: true)
            .AddInputFilter("To", "endDate", startingValue: Model.SearchParameters.EndDate, isDateTime: true)
			//.AddInputFilter(Html.Term("AccountNumberorName", "Account Number or Name"), "accountNumberOrName", ViewData["q"])
            //.CanDeactivate(true,true,"~/CTE/CreditManagements/GetAccounts")
             .AddOption("BloquearCredit", Html.Term("BloquearCredit", "Bloquear Crédito"))
             .AddOption("DesBloquearCredit", Html.Term("DesBloquearCredit", "Desbloquear Crédito"))
             .AddOption("SaldoCero", Html.Term("SaldoCero", "Saldo a Cero"))
             .AddOption("AsignarSaldoAnterior", Html.Term("AsignarSaldoAnterior", "AsignarSaldo Anterior"))

               .AddOption("AsignarNewSaldo", Html.Term("AsignarNewSaldo", "Asignar New Saldo"))
             .AddOption("AsignarCredito", Html.Term("AsignarCrédito", "Asignar Crédito"))
            .AddOption("exportToExcel", Html.Term("ExportToExcel", "Export to Excel"))
            
            .ClickEntireRow()
			.Render();
            
            
    %>

    <iframe   name ="frmExportar" id="frmExportar" style="display:none" src=""></iframe>

    <script type="text/javascript">

//        $(document).ready(function () {
//            var ApplyFilter = "<a id='ApplyFilter'  class='Button ModSearch filterButton' href='javascript:void(0);'>Apply Filter</a>";

//            var botonProcesarPago = "                                  <a href='javascript:void(0);' id='ProcesarPago' class='Button BigBlue'>Procesar Pago</a>";
//            $('.RunFilter').append(botonProcesarPago);


//            $(".RunFilter span").each(function (index) {
//                $(this).removeClass();
//                $(this).remove();
//            })
//        });
        $(function () {
            $('#noteModal').jqm({
                modal: false
            });

            $('#noteModalLog').jqm({
                modal: false
            });
        });

                    function PopupAccountCreditLog(accountID) {



                        var url = '<%= ResolveUrl("~/CTE/CreditManagements/GetAccountCreditModal") %>';


                        var odata = JSON.stringify({ AccountID: accountID });

                        $.ajax({
                            data: odata,
                            url: url,
                            dataType: "json",
                            type: "POST",
                            contentType: "application/json",
                            success: function (response) {
                                var html = response.Items;

                                $("#tblinformacion").html(html)
                                //                        $('#noteModal').jqmShow();
                                $("#noteModalLog").css("display", "block");

                            },
                            error: function (error) {
                            }
                        });
                    }

     

               
     

      
    </script>
</asp:Content>

