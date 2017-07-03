<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Support/Views/Shared/Support.Master"
    Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ContentPlaceHolderID="YellowWidget" runat="server">
    <% Html.RenderPartial("YellowWidget"); %>
</asp:Content>
<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
    <style>
        .ModoLectura
        {
            -moz-box-shadow: inset 0px 1px 0px 0px #ffffff;
            -webkit-box-shadow: inset 0px 1px 0px 0px #ffffff;
            box-shadow: inset 0px 1px 0px 0px #ffffff;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #ededed), color-stop(1, #dfdfdf));
            background: -moz-linear-gradient(top, #ededed 5%, #dfdfdf 100%);
            background: -webkit-linear-gradient(top, #ededed 5%, #dfdfdf 100%);
            background: -o-linear-gradient(top, #ededed 5%, #dfdfdf 100%);
            background: -ms-linear-gradient(top, #ededed 5%, #dfdfdf 100%);
            background: linear-gradient(to bottom, #ededed 5%, #dfdfdf 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#ededed', endColorstr='#dfdfdf',GradientType=0);
            background-color: #ededed;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #dcdcdc;
            display: inline-block;
            cursor: pointer;
            color: #777777;
            font-family: Arial;
            font-size: 12px;
            font-weight: bold;
            padding: 2px 6px;
            text-decoration: none;
            text-shadow: 0px 1px 0px #ffffff;
        }
        .ModoLectura:active
        {
            position: relative;
            top: 1px;
        }
        .myButton
        {
            -moz-box-shadow: 0px 1px 0px 0px #f0f7fa;
            -webkit-box-shadow: 0px 1px 0px 0px #f0f7fa;
            box-shadow: 0px 1px 0px 0px #f0f7fa;
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #33bdef), color-stop(1, #019ad2));
            background: -moz-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: -webkit-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: -o-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: -ms-linear-gradient(top, #33bdef 5%, #019ad2 100%);
            background: linear-gradient(to bottom, #33bdef 5%, #019ad2 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#33bdef', endColorstr='#019ad2',GradientType=0);
            background-color: #33bdef;
            -moz-border-radius: 6px;
            -webkit-border-radius: 6px;
            border-radius: 6px;
            border: 1px solid #057fd0;
            display: inline-block;
            cursor: pointer;
            color: #ffffff;
            font-family: Arial;
            font-size: 11px;
            font-weight: bold;
            padding: 2px 6px;
            text-decoration: none;
            text-shadow: 0px -1px 0px #5b6178;
        }
        .myButton:hover
        {
            background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #019ad2), color-stop(1, #33bdef));
            background: -moz-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: -webkit-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: -o-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: -ms-linear-gradient(top, #019ad2 5%, #33bdef 100%);
            background: linear-gradient(to bottom, #019ad2 5%, #33bdef 100%);
            filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#019ad2', endColorstr='#33bdef',GradientType=0);
            background-color: #019ad2;
        }
        .myButton:active
        {
            position: relative;
            top: 1px;
        }
    </style>
    <% 
        NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();		        
    %>
    <ul class="SectionLinks">
        <li><a class="selected" href="<%= ResolveUrl("~/Support/Ticket/Edit/" + ticketToEdit.SupportTicketNumber) %>">
            <span><%= Html.Term("TicketDetails", "Ticket Details") %></span></a></li>
        <%
            if (ticketToEdit.SupportTicketID > 0)
            {
        %>
        <li>
            <%: Html.ActionLink(Html.Term("TicketHistory", "Ticket History"), "History", new { id = ticketToEdit.SupportTicketNumber }) %></li>
        <%
            }
        %>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <%
        NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();
    %>
    <a href="<%= ResolveUrl("~/Support/Consult") %>">
        <%= Html.Term("BroweTickets", "Browse Tickets") %></a> &gt;
    <% if (ticketToEdit.SupportTicketID > 0)
       {
    %>
    <%= Html.Term("EditTicketDetails", "Edit Ticket Details")%>
    <%
       }
       else
       {
    %>
    <%= Html.Term("NewTicketDetails", "Create New Ticket")%>
    <%
       }
    %>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/json2.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.ns-autogrow.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.form.min.js") %>"></script>
    <%--<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jqfloat.min.js") %>"></script>--%>
    <% 
        NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();
        var status = Constants.SupportTicketStatus.NeedsInput.ToInt();
    %>
    <script type="text/javascript">
        var returnPopupControl;
        var ControlesError ={};
 	    var isSelected=false;
        var totalError =0;
		
        $(function () {
		    $('#noteModalNeedsInput').jqm({
			    modal: false,
				onHide: function(hash)
                {
				    hash.w.fadeOut('fast', function(){
				        hash.o.remove();
						$('#newNoteSubjectNeedsInput,#newNoteTextNeedsInput').val('').clearError();
					});
			    }});
		   
			// When Ticket status changed to 'Needs Input' display the Notes dialog
			$('#supportTicketStatusID').change(function(){
				var val = $('#supportTicketStatusID').val();
				if((parseInt(val) == <%= status %>)){
					$('#noteModalNeedsInput').jqmShow();
				}
			});
			
			$("#btnSaveNoteNeedsInput").click(function () {      
			    if ($('#noteModalNeedsInput').checkRequiredFields()) {
				    var data = {
						parentEntityID: $('#noteParentIdentity').val(),
						parentId: $('#parentNoteIDNeedsInput').val(),
						subject: $('#newNoteSubjectNeedsInput').val(),
						noteText: $('#newNoteTextNeedsInput').val(),
						isNotInternal: $('#isNotInternal').prop('checked')
					};

					var t = $(this);
					showLoading(t, {float: 'right'});
		 
					$.post('<%= ResolveUrl("~/") + ViewContext.RouteData.DataTokens["area"] %>/Notes/Add', data, function (response) {
					    if (response.result) {
						    $('#noteModalNeedsInput').jqmHide();
						    hideLoading(t);
						    $('#parentNoteIDNeedsInput,#newNoteSubjectNeedsInput,#newNoteTextNeedsInput').val('');
						    getNotes();
					    }
					    else {
						    showMessage(response.message, true);
					    }
					});
				}
			});
			
			$('#btnCancelNote').click(function () {
			    $('#addNoteTitle,#addNoteText').val('').clearError();
			});
			
			$('#oldTicketStatusID').val($('#supportTicketStatusID').val());

			<% if (ticketToEdit.AccountID != 0 && ticketToEdit.Account != null)
			{ 
			%>
				$('#txtSearch').val("<%= string.Format("{0} {1} (#{2})", ticketToEdit.Account.FirstName, ticketToEdit.Account.LastName, ticketToEdit.AccountID.ToString()) %>");
			<% 
			}  
			%>

			$('#txtSearch').watermark('<%= Html.JavascriptTerm("ConsultantSearch", "Look up consultant by ID or name") %>').keyup(function (e) 
			{ }).jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
				$('#accountID').val( item.id);

                CargarInformacionCuenta(item.id);
			}
			, minCharacters: 3
			, ajaxResults: true
			, maxResults: 50
			, showMore: true
			, width: $('#txtSearch').outerWidth(true)
			});

			<% if (ticketToEdit.AssignedUserID.HasValue) { %>
					isSelected = true;
			<%}%>
			$('#txtUserSearch').watermark('<%= Html.JavascriptTerm("UserSearch", "Look up user by ID or name") %>').keyup(function (e) 
			{ }).jsonSuggest('<%= ResolveUrl("~/Admin/Users/Search") %>', { onSelect: function (item) {
				$('#assignedUserID').val( item.id);
			   isSelected=true;
			}
			, minCharacters: 1
			, ajaxResults: true
			, maxResults: 50
			, showMore: true
			, width: $('#txtUserSearch').outerWidth(true)
			});

			$('#txtUserSearch').change(function (e) {

				if ($('#txtUserSearch').val() == '')
				{
					$('#assignedUserID').val('');
				}

			});

            $('#btnSave').click(function (){
                GuardarFormulario();
            });           
        });
        
        var Url="";
        function OpenWindowSearch(button)
        {   
            var selectedTD = $(button).closest('td');
            returnPopupControl = selectedTD.find('input[id^=Txt_]');
            var valToSearch = selectedTD.find('input[class=BusquedaType]').val();//$('#Txt_Busqueda1').val();
            //  alert (valToSearch).val();
            //  var title = $('#Txt_Busqueda' ).attr('title');
            //  var cmd = $('#Txt_Busqueda').attr('name');
            //  alert (cmd);
                     
            var _accountID=$('#accountID').val();
            var w = (window.outerWidth *60)/100;
            var h = (window.outerHeight*60)/100;

            var left = (window.outerWidth /2)-(w/2);
            var top = (window.outerHeight/2)-(h/2);
                          
            var  url = '<%= ResolveUrl("~/Support/Ticket/SearchTh") %>';
        
            var parameters = {
                accountID:_accountID,
                textx: valToSearch 
            };
                               
            url = url + '?' + $.param(parameters).toString();
            var  childWindow=  window.open(url, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=yes, addressbar=no, titlebar=no, width='+w+', height='+h+', top='+top+', left='+left);
            
            if (childWindow.opener == null) 
            {
                childWindow.opener = self;
                // alert(childWindow);
            }
            
            if (childWindow != null) 
            {
                //  alert(childWindow);
            }
        }
        
        function HandlePopupResult(result) {
            returnPopupControl.val(result);
            //document.getElementById("Txt_Busqueda").value = result; 
            //alert(result);
        }
        
        function GuardarFormulario(ctr)
        {                 
            if(ctr !=undefined)
            {
                Url=ctr.getAttribute("url");
                var index=Url.indexOf("?");
                    
                var Propiedad=ctr.getAttribute("Propiedad");
                
                if(Propiedad!=0)
                {
                    var TxtPropiedad=$("#Txt_"+Propiedad+"");
                    var propiedadUrl=TxtPropiedad.val();
                    
                    if($.trim(propiedadUrl)=="")
                    {
                        var ErrorMessaje = '<%= Html.JavascriptTerm("FieldIsrequired", "Field is required") %>';
                        TxtPropiedad.showError(ErrorMessaje);
                        return false ;
                    }else{
                        switch(index){
                            case -1:
                                Url=Url+propiedadUrl;
                                break;
                            default:
                                var index1=Url.indexOf("=");
                                if(index1==-1)
                                {
                                    Url=Url+"="+propiedadUrl;
                                }else{
                                    Url=Url+propiedadUrl;
                                }
                                break;
                        }
                    }
                }

                var w = (window.outerWidth *80)/100;
                var h = (window.outerHeight*80)/100;

                var left = (window.outerWidth /2)-(w/2);
                var top = (window.outerHeight/2)-(h/2);
                window.open(Url, '', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=yes, addressbar=no, titlebar=no, width='+w+', height='+h+', top='+top+', left='+left);

                //window.open(Url,'mywindow',w,h);
            }
            $( "#GuardarArchivo" ).submit(); 

        }
        
        function validarControError()
        {             
            for(var key in ControlesError )
            {
                return true ;
            }
            return false ;
        }

        function Guardar( )
        {      
            var isValid =true ;
			var LstSupportTicketsProperty = ObtenerSupportTicketsProperty();
              
            if(SeleccionadoSupportMotiveID==0)
            {
                showMessage('<%= Html.Term("NotSelectedSupportMotive", "Not selected a support motive ")%>', true);
                isValid=false;
            }
            
            if(validarControError())
            {
                isValid=false;
            }
			
            var userSearchVal= $('#txtUserSearch').val();
			if((isSelected==false)&&(userSearchVal!=''))
			{
                  // ControlesError["isSelecteduserSearchVal"]="isSelecteduserSearchVal";
				showMessage('<%= Html.Term("AssignedNotValid", "Please enter a valid assigned to username or leave it blank to unsassign it") %>', true);
				isValid=false;
			}
			
            //delete ControlesError["isSelecteduserSearchVal"];
			if (!$('#ticketTable').checkRequiredFields()) 
            {
                //ControlesError["checkRequiredFields"]="checkRequiredFields";
				isValid=false;
			}

            //delete ControlesError["checkRequiredFields"];
			// Ensure that the Consultant exists
			if($('#accountID').val() == 0){
				showMessage('<%= Html.Term("ConsultantNotFoundPleaseSelectAnotherOne", "Consultant not found. Please select another consultant") %>', true);
				isValid=false;
			}
            
            var url= '<%= ResolveUrl("~/Support/Ticket/Save") %>';

			var t = $(this);
			showLoading(t);
            var txtSolucion =$("#txtSolucion");
            var ErrorMessaje = '<%= Html.JavascriptTerm("SolutionSupportTicketGestion", "Field solution  is required") %>';
             
            if($.trim(txtSolucion.val())=="")
            {
                txtSolucion.showError(ErrorMessaje);
                isValid=false ;
            }else 
            {
                txtSolucion.clearError();
            }
            
            if(!isValid)
            {
                return false ;
            }
                
            var objSupportTicketGestionBE=
            {
                Descripction:$("#txtSolucion").val()
            };
                  
            var odata = JSON.stringify( 
            { 
                accountID: $('#accountID').val(),
                title: $('#title').val(),
                description: $('#description').val(),
                supportTicketCategoryID: 0,
                supportTicketPriorityID: $('#supportTicketPriorityID').val(),
                supportTicketStatusID: $('#supportTicketStatusID').val(),
                assignedUserID: $('#assignedUserID').val(),
                isVisibleToOwner: $('#isVisibleToOwner').prop('checked'),
                oldTicketStatusID: $('#oldTicketStatusID').val(),
                lstSupportTicketsPropertyBE:LstSupportTicketsProperty,
                lstSupportTicketsFilesBE:[],
                SupportLevelID:SeleccionadoSupportLevelID,
                SupportMotiveID:SeleccionadoSupportMotiveID,
                ListaEliminarSupportTicketsFiles:listaEliminar,
                objSupportTicketGestionBE:objSupportTicketGestionBE,
                notifyMails: $('#txtNotifyMails').val()
            });

            $.ajax({
            data: odata,
            url: url ,
            dataType: "json",
            type: "POST",
            contentType: "application/json",
            success: function (response) {
				hideLoading(t);
					if(response)
                    {
                    var index=Url.indexOf("?");
                     
                                if(Url!="")
                                {
                                
                                    switch(index)
                                    {
                                        case -1:
                                        // window.location =Url+"?SupportTicketID="+ response.SupportTicketID;
                                                break;
                                        default:
                                    //   window.location =Url+"&SupportTicketID="+ response.SupportTicketID;
                                                break;
                                    }
                                    
                                }
                            else
						    {
							    window.location = '<%= ResolveUrl("~/Support/Ticket/Edit/") %>' + response.supportTicketNumber;
						    }

                    }               
						
					                 
				showMessage(response.message || '<%= Html.Term("SavedSuccessfully", "Saved successfully!") %>', !response.result);
			    },
                    error: function (error) {
                }
            });
        }
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            $("#description").autogrow({vertical: true, horizontal: false});
            $("#description").keyup();

            $("#txtSolucion").autogrow({vertical: true, horizontal: false});
            $("#txtSolucion").keyup();

            $("#cmbParentLevel").change(function () {
                cargarHijo(this);
            });
        
            <%if (ticketToEdit.SupportTicketID == 0)
            {   %>
                $("#cmbParentLevel").change();
            <%}%>

            <%if(Convert.ToInt32(  ViewData["SupportMotiveID"])>0)
            {%>
                $("#cmbMotivos").change();
            <%}%>                                         
        });

        var SeleccionadoSupportMotiveID=<%=  Convert.ToInt32( ViewData["SupportMotiveID"])%>;
        var SeleccionadoSupportLevelID=<%= Convert.ToInt32(ViewData["SupportLevelID"]) %>;
        
        function cargarHijo(ctr) {
            var value = $(ctr).val();
           
            if (value == 0) {
                clearNext($(ctr).parent("td"));
                return false;
            }

            $("#tblDetaill tbody tr").remove();
            var odata = JSON.stringify({ input: value });
            clearNext($(ctr).parent("td"));
            var url = '<%= ResolveUrl("~/Support/Ticket/ListarSupportLevel") %>';
            $.ajax({
                data: odata,
                url: url ,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {
                    var respuesta = response;

                    if (respuesta.isLast) 
                    {
                        SeleccionadoSupportLevelID=value;
                        crearDetalle(ctr,respuesta.items)
                    } else 
                    {
                        crearHijo(ctr, value, respuesta.items);
                    }
                },
                error: function (error) {
                }
            });
        }

        function crearDetalle(ctr, lista) {
            var FilaActual = $(ctr).parent("td").parent("tr")

            var MensajeDefecto = '<%= Html.JavascriptTerm("SelectedSupportLevelMotive", "-------Selected Support Level Motive-------") %>';
            var comboMotivos = $("<select  onchange='SeleccionarMotivo(this)' id='cmbMotivos'>");
            $(comboMotivos).append("<option value='0'>"+MensajeDefecto+"</option>");
          
            for (var index = 0; index < lista.length; index++) { 
              
              if (lista[index].Item4>0 )//agrega los items activos cuando ticket == 0, crea un nuevo ticket
                $(comboMotivos).append("<option SupportLevelID='" + lista[index].Item1 + "'SupportMotiveID='" + lista[index].Item3 + "' value ='" + lista[index].Item3 + "'>" + lista[index].Item2 + "</option>");
            }
            var celda=$("<td id='pnlMotivo'></td>")
            celda.append(comboMotivos);
            $(FilaActual).append(celda);

            $("#description").keyup();
            $("#txtSolucion").keyup();
        }

        function crearHijo(parent, id, lista) {
            var filaPadre = $(parent).parent("td").parent("tr");
            var celdaPadre = $(parent).parent("td");
            var idPadre = $(parent).val();

            var celda = $("<td id='celda" + idPadre + "-" + id + "'></td>");

            var MensajeDefecto = '<%= Html.JavascriptTerm("SelectedSupportLevel", "-------Selected Support Level-------") %>';

            var comboHijo = $("<select onchange='cargarHijo(this)'  id='" + idPadre + "-" + id + "'></select>");
            $(comboHijo).append("<option value='0'>"+MensajeDefecto +"</option>");
            for (var index = 0; index < lista.length; index++) 
            {
              if (lista[index].Item4>0 )//agrega los items activos cuando ticket == 0, crea un nuevo ticket
                $(comboHijo).append("<option value='" + lista[index].Item1 + "'>" + lista[index].Item2 + "</option>");
            }
            $(celda).append($(comboHijo));
            $(filaPadre).append($(celda));

            $("#description").keyup();
            $("#txtSolucion").keyup();
        }


        function clearNext(ctr) {
            SeleccionadoSupportLevelID=0;
            SeleccionadoSupportMotiveID=0;

            var eliminar = [];
            var next = $(ctr);
            while (next != null && next.html() != null) 
            {
                next = $(next).next("td");
                eliminar.push($(next).attr("id"));
            }
           
            for (var index = 0; index < eliminar.length; index++)
            {
                $("#" + eliminar[index]).remove();
            }
            $("#ticketTable tr[FilaDinamica='FilaDinamica']").remove();            
            $("#pnlMotivo").remove();
        }

        function SeleccionarMotivo( ctr )
        {
            var idMotive = $(ctr).val();
            SeleccionadoSupportMotiveID=idMotive;
            CargarDetalleTicket(idMotive);
        }

        function CargarDetalleTicket(idMotivo) {
            var url = '<%=Url.Content("~/Support/Ticket/GetDetaillSupporMotiveLevel") %>';
            $.get(url, { SupportMotiveID: idMotivo,SupportTicketID:<%=ticketToEdit.SupportTicketID %>,ModoEdicion:<%=ViewData["Edicion"] %> ,IsSiteDWS:<%=ViewData["IsSiteDWS"] %> }, function (data) {
                $("#ticketTable tr[FilaDinamica='FilaDinamica']").remove();
                $("#ticketTable tr[FilaDinamica='FilaDinamicaSolucion']").remove();
                $("#ticketTable tr[tipo='separador']").remove();
                
                var listaFilaDinamica=   $(data).find("tr[FilaDinamica='FilaDinamica']").toArray();
                var listaFilaDinamicaSolucion=   $(data).find("tr[FilaDinamica='FilaDinamicaSolucion']").toArray();
                  
                /*
                    $('.example1 textarea').autogrow();
                    $('.example2 textarea').autogrow({vertical: true, horizontal: false});
                    $('.example3 textarea').autogrow({vertical: false, horizontal: true});
                    $('.example4 textarea').autogrow({flickering: false});
                */

                for(var index=listaFilaDinamica.length;index>0;index--)
                {
                    $(listaFilaDinamica[index-1]).insertAfter($("#Categoria"));

                    var textarea=$(listaFilaDinamica[index-1]).find("textarea")
                    if(textarea)
                    {
                        textarea.autogrow({vertical: true, horizontal: false});
                    }
                }
                
                var filaSeprador =$("<tr tipo='separador'> <td colspan='2'><hr></td></tr>");
                if(listaFilaDinamica.length!=0)
                {
                    filaSeprador.insertAfter(listaFilaDinamica[listaFilaDinamica.length-1])                       
                }

//                 var SolutionDescriptionSuportTicket = '<%= Html.JavascriptTerm("SolutionDescriptionSuportTicket", "Solution") %>';
//                    var solucionFila ="";
//                            solucionFila+=      "<tr id='txtSolucion'>";
//                            solucionFila+=              "<td class='FLabel'>";
//                            solucionFila+=              "</td>";
//                            solucionFila+=              "<td>";
//                            solucionFila+=                  "<textarea id='txtSolucion' style='height: 16.667em;' class='fullWidth required' rows='0'></textarea>";
//                            solucionFila+=              "</td>";
//                            solucionFila+=      "</tr>";
                                  
                var trSolucion= $("#trSolucion");
                for(var index=listaFilaDinamicaSolucion.length;index>0;index--)
                {                     
                    if(listaFilaDinamica.length==0)
                    {
                        $(listaFilaDinamicaSolucion[index-1]).insertAfter(trSolucion);                                    
                    }else
                    {
                        $(listaFilaDinamicaSolucion[index-1]).insertAfter(trSolucion);
                    }

                    var textarea=$(listaFilaDinamicaSolucion[index-1]).find("textarea")
                    if(textarea)
                    {
                        textarea.autogrow({vertical: true, horizontal: false});
                    }     
                }

                if(listaFilaDinamicaSolucion.length!=0)
                {
                    filaSeprador.clone(true).insertAfter(listaFilaDinamicaSolucion[listaFilaDinamicaSolucion.length-1])
                }
            });
        }
        
        function CargarInformacionCuenta(AccountID) {
            var url = '<%=Url.Content("~/Support/Ticket/InformacionCuenta") %>';
            $.get(url, { AccountID: AccountID }, function (data) {               
                $("#informacionConsultora").remove();
                $(data).insertBefore($(".SectionNav"))                
            });
        }

        function isNumberKey(evt)
        {
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode != 46 && charCode > 31 
                && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }
  
        function ObtenerSupportTicketsProperty()
        {       
            var LstSupportTicketsProperty= new Array();
            var objSupportTicketsProperty=null;
          
            $("#ticketTable tbody tr[TipoControles='Propiedades']").each(function()
            {
                objSupportTicketsProperty=new Object()
           
                var ErrorMessaje = '<%= Html.JavascriptTerm("FieldIsrequired", "Field is required") %>';
                var Control = [];

                //if($(this).find("select").length==0){
                if(Control.length==0){
                    Control=$(this).find("input:text")
                }
                
                //if($(this).find("input:text").length==0){
                                       
                if(Control.length==0){
                    Control=$(this).find("select");
                }

                //if( $(this).find("input:Date").length!=0){              
                if(Control.length==0)
                {
                    Control=$(this).find("textarea");
                }

                if(Control.length==0){
                    Control=$(this).find("input");
                }

                var Required= Control[0].getAttribute("required");
                var DataType=Control.attr("DataType");
                var SupportMotivePropertyTypeID=Control.attr("SupportMotivePropertyTypeID")
                var SupportTicketsPropertyID=Control.attr("SupportTicketsPropertyID")
                var value=null;
                    value=Control.val();

                var SupportMotivePropertyValueID =0;
                var PropertyValue="";
                var SupportTicketsPropertyValueID=0;

                objSupportTicketsProperty.SupportMotivePropertyTypeID=SupportMotivePropertyTypeID;
                objSupportTicketsProperty.SupportTicketsPropertyID=SupportTicketsPropertyID;

                objSupportTicketsProperty.SupportTicketID=<%= ticketToEdit.SupportTicketID %>;

                if(Required==1)
                {
                    if(value==0 ||$.trim(value)=="")
                    {
                          Control.showError(ErrorMessaje);
                          ControlesError[Control.attr("id")]=Control.attr("id");
                    }else
                    {
                         Control.clearError();
                         delete ControlesError[Control.attr("id")]
                    }
                }else
                {
                    Control.clearError();
                    delete ControlesError[Control.attr("id")]
                }
                switch(DataType)
                {
                    case "List":
                        objSupportTicketsProperty.SupportTicketsPropertyValueID=value;
                        objSupportTicketsProperty.PropertyValue="";
                        break;
                    default:
                         objSupportTicketsProperty.PropertyValue=value;
                         objSupportTicketsProperty.SupportTicketsPropertyValueID=0;
                        break;
                }
                LstSupportTicketsProperty.push(objSupportTicketsProperty);              
            });
            return LstSupportTicketsProperty;        
        }
        
        function ObtenerSupportTicketsPropertySolucion()
        {
             var LstSupportTicketsProperty= new Array();
             var objSupportTicketsProperty=null;
          
            $("#ticketTable tbody tr[TipoControles='Propiedades'][solucion='si']").each(function()
            {
                objSupportTicketsProperty=new Object()
           
                var ErrorMessaje = '<%= Html.JavascriptTerm("FieldIsrequired", "Field is required") %>';
                var Control = null;

                if($(this).find("select").length==0){
                    Control=$(this).find("input:text")
                }
                
                if($(this).find("input:text").length==0){
                    Control=$(this).find("select");
                }

                var Required= Control[0].getAttribute("required");
                var DataType=Control.attr("DataType");
                var SupportMotivePropertyTypeID=Control.attr("SupportMotivePropertyTypeID")
                var SupportTicketsPropertyID=Control.attr("SupportTicketsPropertyID")
                var value=null;
                    value=Control.val();

                var SupportMotivePropertyValueID =0;
                var PropertyValue="";
                var SupportTicketsPropertyValueID=0;

                objSupportTicketsProperty.SupportMotivePropertyTypeID=SupportMotivePropertyTypeID;
                objSupportTicketsProperty.SupportTicketsPropertyID=SupportTicketsPropertyID;

                objSupportTicketsProperty.SupportTicketID=<%= ticketToEdit.SupportTicketID %>;

                if(Required==1)
                {
                    if(value==0 ||$.trim(value)=="")
                    {
                        Control.showError(ErrorMessaje);
                        ControlesError[Control.attr("id")]=Control.attr("id");
                    }else
                    {
                        Control.clearError();
                        delete ControlesError[Control.attr("id")]
                    }
                }else
                {
                    Control.clearError();
                    delete ControlesError[Control.attr("id")]
                }

                switch(DataType)
                {
                case "List":
                    objSupportTicketsProperty.SupportTicketsPropertyValueID=value;
                    objSupportTicketsProperty.PropertyValue="";
                    break;
                default:
                     objSupportTicketsProperty.PropertyValue=value;
                     objSupportTicketsProperty.SupportTicketsPropertyValueID=0;
                    break;
                }
                                
                LstSupportTicketsProperty.push(objSupportTicketsProperty);
              
            });
            return LstSupportTicketsProperty;        
        }
    </script>
    <script>    
        $(document).ready(function() {    
            $('#GuardarArchivo').ajaxForm({
                beforeSend: function() {
                },
                uploadProgress: function(event, position, total, percentComplete) {
                },
                success: function() {
                },
	            complete: function(xhr) {
                         Guardar();
	            }
            }); 
        });

        var listaArchivos=<%= ViewData["StrlstSupportTicketsFilesBE"]%>

        function agregarArchivo()
        {
            listaArchivos.push
            (
                {
                    SupportTicketFileID:0,
                    SupportTicketID:0,
                    FilePath:"",
                    UserID:0
                }
            );

            var file =$("<input name='"+(listaArchivos.length-1)+"' index='"+(listaArchivos.length-1)+"' onchange='cargarArchivo(this)' id='"+(listaArchivos.length-1)+"' type='file' />")
            $("#GuardarArchivo").append(file);
            file.click();
       
            var checkBox=$("<input SupportTicketFileID='0' value='"+(listaArchivos.length-1)+"' type='checkbox'/>");
            var celdacheckBox =$("<td></td>");
            celdacheckBox.append(checkBox)
            var celdaNombre =$("<td></td>");

            var className="''";
            if(((listaArchivos.length-1))%2==0)
            {
                className="'Alt'";
            }
            
            var UploadFile  = '<%= Html.JavascriptTerm("UploadFile", "Upload  a file ") %>';
            var celdacheckSubir =$("<td>   <img style='cursor:pointer' title="+UploadFile+" onclick='SubirArchivo("+(listaArchivos.length-1)+")' src='/Content/Images/Upload.png' width='20' height='20'/> </td>");
            var fila =$("<tr class="+className+" id='"+(listaArchivos.length-1)+"'></tr>");
            fila.append(celdacheckBox);
            fila.append(celdaNombre );
            fila.append(celdacheckSubir);

            $("#Files tbody").append(fila);
        }

        function cargarArchivo(ctr)
        {
            var index =$(ctr).attr("index");
            var celda = $("#Files tbody tr[id='"+index+"'] td:eq(1)")
            celda.text($(ctr).val());
        } 

        function colorearFilas()
        {
            var index=0;
            $("#Files tbody tr:not(:hidden)").each(function()
            {
                index++;
                if(index % 2==0)
                {
                    $(this).attr("class","Alt")
                }else
                {
                    $(this).attr("class","")
                }
            });
        }

        function SubirArchivo(index)
        {
            var file =$("#GuardarArchivo input:file[id='"+index+"']");
            if(!file)
            {
                file =$("<input name ='"+index+"' index='"+index+"' onchange='cargarArchivo(this)' id='"+index+"' type='file' />")
                $("#GuardarArchivo").append(file);
            }
            file.click();
        }
        var listaEliminar=[];

        function EliminarArchivos()
        {
            var ErrorMessaje = '<%= Html.JavascriptTerm("EliminarArchivo", "this really want to delete") %>';

            if(!confirm(ErrorMessaje))
            {
                return false;
            }
            var listaSeleccionados=[];
            var check=null;
            $("#Files tbody tr").each(function()
            {
                var index=$(this).attr("id");
                var check=$(this).find("input:checkbox");
                var SupportTicketFileID=(check).attr("SupportTicketFileID");

                if(check.is(":checked"))
                {
                    if(SupportTicketFileID!=0)
                    {
                        listaEliminar.push(SupportTicketFileID);                                
                        $(this).hide();                                            
                    }
                    else
                    {
                            $(this).hide();
                            $("#GuardarArchivo input:file[id='"+index+"']").remove();
                    }
                }
            });

            colorearFilas();
        }

        function marcarTodos(ctr)
        {
            $("#Files tbody tr").each(function()
            {
                var $this=this;
                var check=$($this).find("td:eq(0) input:checkbox");
                check.attr("checked",ctr.checked);
            });
        }

        function EliminarFilesOcultos()
        {
            $("#Files tbody tr").each(function(){
                var index=$(this).attr("id");
                if($(this).is(":hidden"))
                {
                    $("#GuardarArchivo input:file[id='"+index+"']").remove();
                }
            });
        }
        
        $(document).ready(function (){
            $( "#GuardarArchivo" ).submit(function( event ) {                
            });                          
        });

        function VerificarValidacionControl(ctr)
        {
            if(ctr.getAttribute("required")==1)
            {
                var value =ctr.value;

                if(value)
                {
                    var NextElement = ctr.nextElementSibling;
                    if(NextElement)
                    {
                        var parent =ctr.parentNode  ;
                       parent.removeChild(NextElement);
                       ctr.style.border="";
                    }
                }
            }               
        }       
    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <% 
        NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();		
    %>
    <div class="SectionHeader">
        <h2>
            <% if (ticketToEdit.SupportTicketID > 0)
               {
            %>
            <%= Html.Term("EditTicketDetails", "Edit Ticket Details")%>
            <%
               }
               else
               {
            %>
            <%= Html.Term("NewTicketDetails", "Create New Ticket")%>
            <%
               }
            %>
        </h2>
    </div>
    <input id="accountID" type="hidden" value="<%= ticketToEdit.AccountID %>" class="fullWidth" />
    <table id="ticketTable" class="DataGrid FormGrid" width="90%">
        <tbody>
            <% if (ticketToEdit.SupportTicketID == 0)
               {
            %>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Consultant", "Consultant")%>:
                </td>
                <td>
                    <input id="txtSearch" type="text" class="fullWidth required" />
                </td>
            </tr>
            <%
               }
            %>
            <%if (ticketToEdit.SupportTicketID == 0 || (ticketToEdit.SupportTicketID != 0 && Convert.ToInt32(ViewData["SupportLevelID"]) == 0))
              {   %>
            <tr id="Categoria">
                <td class="FLabel">
                    <%= Html.Term("Category", "Category")%>:                    
                </td>
                <td colspan="10">
                    <%

                  List<System.Tuple<int, string, int, int>> lstSupportLevelParent = ViewBag.lstSupportLevelParent as List<System.Tuple<int, string, int, int>>;
               
                    %>
                    <table>
                        <tr>
                            <td>
                                <select id="cmbParentLevel">
                                    <option value="0">
                                        <%= Html.Term("SelectedSupportLevel", "-------Selected Support Level-------")%></option>
                                    <%for (int index = 0; index < lstSupportLevelParent.Count; index++)
                                      {
                                          if (lstSupportLevelParent[index].Item4 > 0)%>
                                    <option value="<%=lstSupportLevelParent[index].Item1%>">
                                        <%=lstSupportLevelParent[index].Item2%>
                                    </option>
                                    <%} %>
                                </select>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%}

              if (Convert.ToInt32(ViewData["SupportLevelID"]) > 0)
              {  %>
            <%
                  int _SupportTicketID = Convert.ToInt32(ViewData["SupportTicketID"]);
                  int _SupportMotiveID = Convert.ToInt32(ViewData["SupportMotiveID"]);
                  int _SupportLevelID = Convert.ToInt32(ViewData["SupportLevelID"]);

                  nsCore.Areas.Support.Models.Ticket.JerarquiaSupportLevelModel _objJerarquiaSupportLevelModel = null;
                  _objJerarquiaSupportLevelModel = new nsCore.Areas.Support.Models.Ticket.JerarquiaSupportLevelModel(_SupportLevelID, _SupportMotiveID);
            %>
            <%--inicio control--%>
            <tr id="Categoria">
                <%--inicio fial --%>
                <td class="FLabel">
                     <%= Html.Term("Category", "Category")%>: 
                </td>
                <td colspan="10">
                    <table>
                        <tbody>
                            <tr>
                                <%
                  //nsCore.Areas.Support.Models.Ticket.JerarquiaSupportLevelModel objJerarquiaSupportLevelModel = Model as nsCore.Areas.Support.Models.Ticket.JerarquiaSupportLevelModel;
                  int totalHijosJerarquiaDescenedenteSupportLevel = _objJerarquiaSupportLevelModel.lstHijosJerarquiaDescenedenteSupportLevel.Count;

                  List<SupportLevelSearchData> lstHijosJerarquiaDescenedenteSupportLevel = _objJerarquiaSupportLevelModel.lstHijosJerarquiaDescenedenteSupportLevel;

                  List<SupportLevelSearchData> lstHijosJerarquiaAscendenteSupportLevel = _objJerarquiaSupportLevelModel.lstHijosJerarquiaAscendenteSupportLevel;
                  List<SupportLevelSearchData> ListaPrimerNivel = lstHijosJerarquiaAscendenteSupportLevel.FindAll((obj) => obj.ParentSupportLevelID == 0);

                  Boolean ModoEdicion = Convert.ToInt32(ViewData["Edicion"]) == 1;

                  Func<bool, string> FncHabilitarDeshabilitar = (estado) =>
                  {
                      return (!estado) ? "disabled='disabled'" : "";
                  };


                  if (totalHijosJerarquiaDescenedenteSupportLevel > 0)
                  {
                                %>
                                <td>
                                    <select disabled="disabled" <%=FncHabilitarDeshabilitar(ModoEdicion) %> id="cmbParentLevel">
                                        <option value="0">
                                            <%= Html.Term("SelectedSupportLevel", "-------Selected Support Level-------")%></option>
                                        <%for (int indice = 0; indice < ListaPrimerNivel.Count; indice++)
                                          { 
                                        %>
                                        <%if (ListaPrimerNivel[indice].SupportLevelID == lstHijosJerarquiaDescenedenteSupportLevel[lstHijosJerarquiaDescenedenteSupportLevel.Count - 1].SupportLevelID)
                                          { %>
                                        <option id="<% %>" selected="selected" value="<%=ListaPrimerNivel[indice].SupportLevelID %>">
                                            <%=ListaPrimerNivel[indice].Name%></option>
                                        <%}
                                          else
                                          {%>
                                        <option id="Option1" value="<%=ListaPrimerNivel[indice].SupportLevelID %>">
                                            <%=ListaPrimerNivel[indice].Name%></option>
                                        <%} %>
                                        <%
                                          }

                                          var lst = lstHijosJerarquiaDescenedenteSupportLevel.OrderBy((ob) =>
                                              ob.SupportLevelID).ToList().
                                              FindAll((obj) => obj.ParentSupportLevelID != 0);
                                          if (lst.Count > 0)
                                          {
                                              lstHijosJerarquiaDescenedenteSupportLevel = lstHijosJerarquiaDescenedenteSupportLevel.OrderBy((ob) =>
                                                  ob.SupportLevelID).ToList().
                                                  FindAll((obj) => obj.ParentSupportLevelID != 0);
                                          }
                                        %>
                                    </select>
                                </td>
                                <%  for (int indice = 0; indice < lstHijosJerarquiaDescenedenteSupportLevel.Count; indice++)
                                    {
                                        List<SupportLevelSearchData> ListaHijos = lstHijosJerarquiaAscendenteSupportLevel.FindAll((obj) => obj.ParentSupportLevelID == lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID);
                                %>
                                <td id="celda<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>-<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>">
                                    <select disabled="disabled" <%=FncHabilitarDeshabilitar(ModoEdicion) %> onchange="cargarHijo(this)"
                                        id="<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>-<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>">
                                        <option value="0">
                                            <%= Html.Term("SelectedSupportLevel", "-------Selected Support Level-------")%></option>
                                        <%for (int indiceHijos = 0; indiceHijos < ListaHijos.Count; indiceHijos++)
                                          {%>
                                        <%if (lstHijosJerarquiaDescenedenteSupportLevel[indice].SupportLevelID == ListaHijos[indiceHijos].SupportLevelID)
                                          {%>
                                        <option selected="selected" value="<%=ListaHijos[indiceHijos].SupportLevelID %>">
                                            <%=ListaHijos[indiceHijos].Name%></option>
                                        <%}
                                          else
                                          {%>
                                        <option value="<%=ListaHijos[indiceHijos].SupportLevelID %>">
                                            <%=ListaHijos[indiceHijos].Name%></option>
                                        <% } %>
                                        <%} %>
                                    </select>
                                </td>
                                <%
                                    }
                                %>
                                <%
                                    List<System.Tuple<int, string, int, int>> LstSupportLevelMotive = SupportTicket.GetLevelSupportLevelMotiveIsActive(lstHijosJerarquiaDescenedenteSupportLevel[lstHijosJerarquiaDescenedenteSupportLevel.Count - 1].SupportLevelID) ?? new List<System.Tuple<int, string, int, int>>();

                                    if (LstSupportLevelMotive.Count > 0)
                                    {
                                %>
                                <td id="pnlMotivo">
                                    <select disabled="disabled" <%=FncHabilitarDeshabilitar(ModoEdicion) %> onchange="SeleccionarMotivo(this)"
                                        id="cmbMotivos">
                                        <option value="0">
                                            <%= Html.Term("SelectedSupportLevelMotive", "-------Selected Support Level Motive-------")%></option>
                                        <%for (int indice = 0; indice < LstSupportLevelMotive.Count; indice++)
                                          { %>
                                        <% if (_SupportMotiveID == LstSupportLevelMotive[indice].Item3)
                                           {%>
                                        <option selected="selected" supportlevelid="<%=lstHijosJerarquiaDescenedenteSupportLevel[0].SupportLevelID %>"
                                            supportmotiveid="<%=LstSupportLevelMotive[indice].Item3%>" value="<%=LstSupportLevelMotive[indice].Item3%>">
                                            <%= LstSupportLevelMotive[indice].Item2%>
                                        </option>
                                        <%}
                                           else
                                           {%>
                                        <option supportlevelid="<%=lstHijosJerarquiaDescenedenteSupportLevel[0].SupportLevelID %>"
                                            supportmotiveid="<%=LstSupportLevelMotive[indice].Item3%>" value="<%=LstSupportLevelMotive[indice].Item3%>">
                                            <%= LstSupportLevelMotive[indice].Item2%>
                                        </option>
                                        <%}%>
                                        <%} %>
                                    </select>
                                </td>
                                <%}
                  }%>
                                <%--fin fila--%>
                            </tr>
                        </tbody>
                    </table>
                </td>
            </tr>
            <%--fin del control--%>
            <%} %>
            <%--   </td>
            <div>
            </div>
            </td>
            </tr>--%>
            <%
                var showTit = " style='display:block'";
                var resul = OrderExtensions.GeneralParameterVal(Convert.ToInt32(CoreContext.CurrentMarketId), "ETT");
                if (string.IsNullOrEmpty(resul))
                {
                    resul = "N";
                }

                if (resul == "N")
                {
                    showTit = " style='display:none' ";
                }
            %>
            <tr <%= showTit %>>
                <td class="FLabel">
                    <%= Html.Term("Title", "Title")%>:
                </td>
                <td>
                    <%
                    
                        Boolean _ModoEdicion = Convert.ToInt32(ViewData["Edicion"]) == 1;

                        Boolean IsSiteDWS = Convert.ToInt32(ViewData["IsSiteDWS"]) == 1;

                        Func<bool, string> _FncHabilitarDeshabilitar = (estado) =>
                        {
                            return (!estado) ? "disabled='disabled'" : "";
                        };
                        Func<bool, bool, string> _FncHabilitarDeshabilitarTexto = (estado, pIsSiteDWS) =>
                        {
                            if (ticketToEdit.SupportTicketID > 0)
                            {
                                estado = true;
                                pIsSiteDWS = true;
                            }
                            return (!estado || pIsSiteDWS) ? "disabled='disabled'" : "";
                        };
                    %>
                    <input <%=_FncHabilitarDeshabilitarTexto(_ModoEdicion,IsSiteDWS) %> id="title" type="text"
                        value="<%= ticketToEdit.Title %>" class="fullWidth required" />
                </td>
            </tr>
            <tr id="Descripcion">
                <td class="FLabel">
                    <%= Html.Term("Description", "Description")%>:
                </td>
                <td>
                    <textarea <%=_FncHabilitarDeshabilitarTexto(_ModoEdicion,IsSiteDWS) %> id="description"
                        style="height: 1cm;" class="fullWidth required"><%= ticketToEdit.Description%></textarea>
                </td>
            </tr>
            <tr id="trSolucion">
                <td class="FLabel">
                    <%= Html.Term("SolutionDescriptionSuportTicket", "Solution")%>:
                </td>
                <td>
                    <textarea <%=_FncHabilitarDeshabilitar(_ModoEdicion) %> id="txtSolucion" style="height: 1cm;"
                        class="fullWidth required"></textarea>
                </td>
            </tr>
            <%--	<tr>
				<td class="FLabel">
					<%= Html.Term("Category", "Category") %>:
				</td>
				<td>
					<select id="supportTicketCategoryID">
						<% foreach (var supportTicketCategory in SmallCollectionCache.Instance.SupportTicketCategories.OrderBy(s => s.SortIndex).Where(a => a.Active))
						   {
						%>
						<option value="<%= supportTicketCategory.SupportTicketCategoryID %>" <%= ticketToEdit.SupportTicketCategoryID == supportTicketCategory.SupportTicketCategoryID ? "selected=\"selected\"" : "" %>>
							<%= supportTicketCategory.GetTerm() %></option>
						<%      
						   } 
						%>
					</select>
				</td>
			</tr>--%>
            <tr>
                <%

                    Boolean __ModoEdicion = Convert.ToInt32(ViewData["Edicion"]) == 1;

                    Func<bool, string> __FncHabilitarDeshabilitar = (estado) =>
                    {
                        return !estado ? "disabled='disabled'" : "";
                    };

                %>
                <td class="FLabel">
                    <%= Html.Term("Priority")%>:
                </td>
                <td>
                    <select <%=__FncHabilitarDeshabilitar(__ModoEdicion) %> id="supportTicketPriorityID">
                        <% foreach (var supportTicketPriority in SmallCollectionCache.Instance.SupportTicketPriorities.OrderBy(p => p.SortIndex).Where(a => a.Active))
                           {
                        %>
                        <option value="<%= supportTicketPriority.SupportTicketPriorityID %>" <%= ticketToEdit.SupportTicketPriorityID == supportTicketPriority.SupportTicketPriorityID ? "selected=\"selected\"" : "" %>>
                            <%= supportTicketPriority.GetTerm()%></option>
                        <%      
                           } 
                        %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Status")%>:
                </td>
                <td>
                    <select <%=__FncHabilitarDeshabilitar(__ModoEdicion) %> id="supportTicketStatusID">
                        <% if (ticketToEdit.SupportTicketID > 0)
                           {  %>
                        <% foreach (var supportTicketStatus in SmallCollectionCache.Instance.SupportTicketStatuses.OrderBy(p => p.SortIndex).Where(a => a.Active))
                           {
                        %>
                        <option value="<%= supportTicketStatus.SupportTicketStatusID %>" <%= ticketToEdit.SupportTicketStatusID == supportTicketStatus.SupportTicketStatusID ? "selected=\"selected\"" : "" %>>
                            <%= supportTicketStatus.GetTerm()%></option>
                        <%      
                           }                           
                        %>
                        <% }
                           else
                           { %>
                        <% foreach (var supportTicketStatus in SmallCollectionCache.Instance.SupportTicketStatuses.OrderBy(p => p.SortIndex).Where(a => a.Active && a.SupportTicketStatusID != (short)Constants.SupportTicketStatus.NeedsInput))
                           {
                        %>
                        <option value="<%= supportTicketStatus.SupportTicketStatusID %>" <%= ticketToEdit.SupportTicketStatusID == supportTicketStatus.SupportTicketStatusID ? "selected=\"selected\"" : "" %>>
                            <%= supportTicketStatus.GetTerm()%></option>
                        <%      
                           }                           
                        %>
                        <% } %>
                    </select>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("AssignedTo", "Assigned to")%>:
                </td>
                <td>
                    <input <%=__FncHabilitarDeshabilitar(__ModoEdicion) %> id="txtUserSearch" type="text"
                        value="<%= ticketToEdit.AssignedUserID.HasValue ? CachedData.GetUser(ticketToEdit.AssignedUserID.ToInt()).Username : string.Empty %>" />
                    <input id="assignedUserID" type="hidden" value="<%= ticketToEdit.AssignedUserID %>"
                        class="fullWidth" />
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("NotifytoMails", "Notify to Mails")%>:
                </td>
                <td>
                    <input <%=__FncHabilitarDeshabilitar(__ModoEdicion) %> id="txtNotifyMails" type="text" style="width: 70%;"
                        value="" /><br />                        
                    <span style="color: #858585;"><%= Html.Term("NotifytoMailsLabel", "Mails must be separated by (;)")%></span>             
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                    <%= Html.Term("Options", "Options")%>:
                </td>
                <td>
                    <p>
                        <input id="isVisibleToOwner" type="checkbox" <%= ticketToEdit.IsVisibleToOwner ? "checked=\"Checked\"" : "" %> />
                        <%= Html.Term("TicketVisibleToOwner", "Ticket visible to owner") %>
                    </p>
                </td>
            </tr>
            <tr id="productPanel">
                <td class="FLabel" style="vertical-align: top;">
                    <%= Html.Term("Atachements", "Atachements")%>:
                    <div id="productPanelOverlay" style="background-color: #999999; height: 0px; width: 0px;
                        position: absolute; left: 0px; top: 0px; z-index: 1; opacity: 0.6; filter: alpha(opacity=60);">
                    </div>
                </td>
                <td id="propertyNoAdd" style="display: none;">
                    The motive must be saved before Property can be added.
                </td>
                <td id="propertyAdd">
                    <div>
                        <p class="FL">
                        </p>
                        <p class="FL">
                        </p>
                        <p class="FL">
                        </p>
                        <p class="FL">
                        </p>
                        <p class="FL">
                        </p>
                        <p class="FR">
                            <%
                                Boolean AgregarArchivo = Convert.ToInt32(ViewData["Edicion"]) == 1;

                                if (AgregarArchivo)
                                { %>
                            <a onclick="agregarArchivo()" id="btnApplySchedule" href="javascript:void(0);" class="">
                                <%= Html.Term("AddFile", "Add File")%></a> | <a onclick="EliminarArchivos()" id="btnRemoveProperty" href="javascript:void(0);"
                                    class=""><%= Html.Term("RemoveFile", "Remove File")%></a>
                            <%} %>
                        </p>
                    </div>
                    <span class="ClearAll"></span>
                    <!-- Products In Order -->
                    <table id="Files" width="100%" class="DataGrid">
                        <thead>
                            <tr class="GridColHead">
                                <th class="GridCheckBox">
                                    <input id="selectAllPropertyItems" onchange="marcarTodos(this)" type="checkbox" />
                                </th>
                                <th>
                                    <%= Html.Term("FileName", "File Name")%>                                                                        
                                </th>
                                <th>
                                    <%= Html.Term("DownloadFileHeader", "Download")%>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <%
                   
                                List<SupportTicketsFilesBE> lstSupportTicketsFilesBE = ViewData["lstSupportTicketsFilesBE"] as List<SupportTicketsFilesBE>;
                                for (int IndiceFilas = 0; IndiceFilas < (lstSupportTicketsFilesBE ?? new List<SupportTicketsFilesBE>()).Count; IndiceFilas++)
                                {
                            %>
                            <%if (IndiceFilas % 2 == 0)
                              {%>
                            <tr class="Alt">
                                <td>
                                    <input <%=__FncHabilitarDeshabilitar(__ModoEdicion) %> supportticketfileid='<%= lstSupportTicketsFilesBE[IndiceFilas].SupportTicketFileID%>'
                                        value="<%=IndiceFilas%>" type="checkbox" />
                                </td>
                                <td>
                                    <%=lstSupportTicketsFilesBE[IndiceFilas].FilePath %>
                                </td>
                                <td>
                                    <a title="<%= Html.Term("DownloadFile", "Download")%>" href="/Ticket/Download/<%=lstSupportTicketsFilesBE[IndiceFilas].SupportTicketFileID%>">
                                        <img src="/Content/Images/DownloadFile.png" width="20" height="20" /></a>
                                </td>
                            </tr>
                            <%}
                              else
                              {%>
                            <tr>
                                <td>
                                    <input <%=__FncHabilitarDeshabilitar(__ModoEdicion) %> supportticketfileid='<%= lstSupportTicketsFilesBE[IndiceFilas].SupportTicketFileID%>'
                                        value="<%=IndiceFilas%>" type="checkbox" />
                                </td>
                                <td>
                                    <%=lstSupportTicketsFilesBE[IndiceFilas].FilePath %>
                                </td>
                                <td>
                                    <a title="<%= Html.Term("DownloadFile", "Download")%>" href="/Ticket/Download/<%=lstSupportTicketsFilesBE[IndiceFilas].SupportTicketFileID%>">
                                        <img src="/Content/Images/DownloadFile.png" width="20" height="20" /></a>
                                </td>
                            </tr>
                            <%}%>
                            <%
                                }
                            %>
                        </tbody>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="FLabel">
                </td>
                <td>
                    <p class="FormButtons">
                        <%
                       
                            if (AgregarArchivo)
                            { %>
                        <a id="btnSave" class="Button BigBlue" href="javascript:void(0);">
                            <%= Html.Term("SaveTicket", "Save Ticket")%></a>
                        <%} %>
                        <a id="btnCancel" class="Button jqmClose" href="<%= ResolveUrl("~/Support/Consult") %>">
                            <%= Html.Term("Cancel") %></a>
                    </p>
                </td>
            </tr>
        </tbody>
    </table>
    <input type="hidden" id="oldTicketStatusID" value="" />
    <div class=" ClearAll NoteFilters" style="padding: 5px;">
        <div id="noteModalNeedsInput" class="jqmWindow LModal">
            <div class="mContent">
                <h2>
                    <%= Html.Term("AddaNote", "Add a Note")%></h2>
                <div id="noteErrorsNeedsInput">
                </div>
                <div class="FormStyle2">
                    <%= Html.Term("Title")%>:
                    <br />
                    <input id="newNoteSubjectNeedsInput" type="text" style="width: 250px;" maxlength="100"
                        value="" class="required" /><br />
                    <%= Html.Term("Note")%>:
                    <br />
                    <textarea id="newNoteTextNeedsInput" style="width: 250px; height: 100px;"></textarea>
                    <br />
                    <input id="isNotInternal" type="checkbox" checked="checked" style="display: none" />
                    <%= Html.Term("GMP_Support_Ticket_NeedsInput_PublishText", "This note will be published to the consultant")%>
                </div>
                <br />
                <input type="hidden" id="parentNoteIDNeedsInput" style="width: 250px;" value="" /><br />
                <p>
                    <a href="javascript:void(0);" id="btnSaveNoteNeedsInput" class="Button BigBlue">
                        <%= Html.Term("Save")%></a> <a href="javascript:void(0);" class="Button LinkCancel jqmClose"
                            id="btnCancelNoteNeedsInput">
                            <%= Html.Term("Cancel")%></a>
                </p>
                <span class="ClearAll"></span>
            </div>
        </div>
    </div>
    <%if (ViewData["BlockUserName"] != null)
      {  %>
    <div style="display: none" id="userBlock">
        <%=Convert.ToString(ViewData["BlockUserName"])%>
    </div>
    <%} %>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="RightContent" runat="server">
    <% 
        NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();		        
    %>
    <td class="CoreRightColumn">
        <% if (ticketToEdit.SupportTicketID > 0)
           {
               Html.RenderPartial("Notes");

           } 
        %>
    </td>
    <form id="GuardarArchivo" target="frmGuardarArchivo" action="/Ticket/GuardarArchivos/"
    method="post" enctype="multipart/form-data">
    </form>
    <iframe name="frmGuardarArchivo" style="display: none"></iframe>
</asp:Content>
