<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Order>" %>

<%--Modifications:
    @01 20150715 BR-AT-005 G&S PGCT: Create imputs and functions needed for the requeriment--%>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <%--<a href="<%= ResolveUrl("~/Orders") %>">--%>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/numeric.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/json2.js") %>"></script>
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/jquery.limitkeypress.min.js") %>"></script>
    <script type="text/javascript">
    var JsonDc = <%= ViewData["JsonDc"]%>;
    var valorSeleccionado = 0;

    $(function () {
        $('.confirmCheck')
              .unbind('click') // takes care of jQuery-bound click events
              .attr('onclick', '') // clears `onclick` attributes in the HTML
              .each(function () { // reset `onclick` event handlers
                  this.onclick = null;
              });   

        $('.confirmCheck').click(marcarCheck);

        //Evento click del boton ReturnConfirm
        $("#btnReturnConfirm").click(function(){
            var tieneClassButtonOff= $("#btnReturnConfirm").hasClass("ButtonOff");
            if(tieneClassButtonOff)
            {
                return false;
            }
            
            $('.WaitWin').jqm({ modal: true, overlay: 90, overlayClass: 'HModalOverlay' }).jqmShow();
         
            $.when(saveDeferred()).then(function(response) {
                if (response.result) {
				    confirmOrderDeferred();
			    }
			    else {
				    showMessage(response.message, true);
			    }
            }); 
        });

        //Evento Click del boton Save
        $("#btnSave").click(function () {
            var lstOrderItemReturnConfirm = AgregarItemsSeleccionados();
            if(lstOrderItemReturnConfirm == null) {
                return false;
            }

            $.when(saveDeferred()).then(function(response) {
                if (response.result) {
				    location.reload(true);
			    }
			    else {
				    showMessage(response.message, true);
			    }
            }); 
        });

        $('#noteModal').jqm({
	                    modal: false 
	                });
    });
   
   //Llama al proceso SaveOrderReturn
    function saveDeferred(){
        var lstOrderItemReturnConfirm = AgregarItemsSeleccionados();
        if(lstOrderItemReturnConfirm == null) {
            return false;
        }

        var data = JSON.stringify({ lstOrderItemReturnConfirm: lstOrderItemReturnConfirm, 
            orderId:$("#orderId").val(),
            orderNote: $("#txtObservacionOrden").val() });
            
        var url = '<%= ResolveUrl("~/Orders/PendingConfirm/InsertarOrderItemReturnConfirm") %>';
                 
        var promise = $.ajax({
            data: data,
            url: url,
            dataType: "json",
            type: "POST",
            contentType: "application/json"            
        });

        return promise;
    }

    //Llama al proceso ConfimOrder
	function confirmOrderDeferred() {		   
        var data = { OrderId: $("#orderId").val() };
		 $.post('<%= ResolveUrl("~/Orders/PendingConfirm/ConfirmOrden") %>', data, function (response) {
		    if (response.result) {
		        window.location = '<%= ResolveUrl("~/Orders/PendingConfirm/") %>';
		    } else {
		        $('.WaitWin').jqmHide();
		        showMessage(response.message, true);
		    }
		});	
	}

    //Muestra PopUp con la observacion del item
    function verObservacion(ctr) {
        var OrderItemReturnID = $(ctr).attr("OrderItemReturnID");
        $("#txtObservacionItem").attr("OrderItemReturnID",OrderItemReturnID);

        var observacion = "";
        observacion = JsonDc[OrderItemReturnID]
        $("#txtObservacionItem").val(observacion);
        $('#noteModal').jqmShow({modal: true}); 
    }

    function BuscarIndex(lista,key,value) {
        var _index=-1;
        for(var index=0;index<lista.length;index++)
            {
                if(lista[index][key]==value)
                {
                    return index;
                }
            }

        return _index;
    }

    //GUARDAR OBSERVACION  ABRIR MODAL
    function guardarItemObservacion() {        
        var OrderItemReturnID = $("#txtObservacionItem").attr("OrderItemReturnID");
        JsonDc[OrderItemReturnID] = $("#txtObservacionItem").val();
        $('#noteModal').jqmHide();
    }

    //CANCELAR  OBSERVACION CERRAR MODAL
    function cancelar() {
        $('#noteModal').jqmHide();
    }

    function AgregarItemsSeleccionados() {
	    var data = new Array();
	    var oOrderItemReturnConfirm2 = null;
	   
	    $("#productsReturn tbody tr[data=true]").each(function () {
		    var row = this;
		    var _OrderItemReturnID=$(row).attr("OrderItemReturnID");
		    var cantidadDevolucion=  $(row).find("td:eq(3) input:text");

		    var checkbox = $(row).find("td:eq(5) input:checkbox");
		    if (checkbox.is(":checked")) {
			    var txtCantidadDiferencia= parseInt( $.trim( $(row).find("td:eq(4)").text()));
			    if(txtCantidadDiferencia!=0) {
				    var txtObservacionOrden=$.trim( $("#txtObservacionOrden").val());
				    if($.trim(JsonDc[_OrderItemReturnID])=="")//txtObservacionOrden=="")
				    {
					    alert('<%= Html.Term("NeedsAndObservation", "It needs an Observation") %>');
					    // cantidadDevolucion.css("background-color", "red");
					    cantidadDevolucion.focus();
				  
					    $(row).focus();
					
					    data=null;
					    return false;
				    }
				    else {
					    //cantidadDevolucion.css("background-color", "");
				    }                  
			    }

			    var OrderItemConfirmID = 0,
			    OrderItemReturnID = "",
			    Quantity = 0,
			    Note = "",
			    ModifiedByUserID=<%= CoreContext.CurrentUser.UserID %>;

			    OrderItemConfirmID = $(checkbox).attr("OrderItemConfirmID");
			    OrderItemReturnID = _OrderItemReturnID;
			    Quantity = $("input:text[id=" + OrderItemReturnID + "]").val();
			    Quantity = isNaN(Quantity) ? 0 : Quantity;

			    oOrderItemReturnConfirm2 = new Object();
			    oOrderItemReturnConfirm2.OrderItemConfirmID = OrderItemConfirmID;
			    oOrderItemReturnConfirm2.OrderItemReturnID = OrderItemReturnID;
			    oOrderItemReturnConfirm2.Quantity = Quantity;
			    oOrderItemReturnConfirm2.Note = JsonDc[OrderItemReturnID];
			    oOrderItemReturnConfirm2.ModifiedByUserID=ModifiedByUserID;
			    data.push(oOrderItemReturnConfirm2);
		    }
		    else {
			    cantidadDevolucion.css("background-color", "");
		    }
	    });

        return data;
    }

    //calcula la diferencia de cantidades para el item seleccionado
    function calcularDiferencia(ctr) {
        
        var cantidadRecepcion=$(ctr).val();
        
        cantidadRecepcion=$.trim(cantidadRecepcion);
        cantidadRecepcion=   isNaN(cantidadRecepcion)||cantidadRecepcion==""?0:cantidadRecepcion;

        var maxValue=$(ctr).attr("maxValue");
        
        cantidadRecepcion=cantidadRecepcion>maxValue?valorSeleccionado:cantidadRecepcion;

        var cantidadRetorno=$(ctr).parent("td").prev().text();
        cantidadRetorno=$.trim(cantidadRetorno);
        cantidadRetorno=isNaN( cantidadRetorno)||cantidadRetorno==""?0:cantidadRetorno

        var diferencia=cantidadRetorno- cantidadRecepcion ;
        return diferencia;
    }

    //valida diferencia para insertar diferencia y marcar checkbox
    function validarDiferencia(ctr) {
        var diferencia = calcularDiferencia(ctr)
        var checkbox=$(ctr).parent("td").next().next().find("input:checkbox");
        $(ctr).parent("td").next().text(diferencia);
           
        if(diferencia==0) {
            checkbox.attr("checked",true);
        }
        else {
            checkbox.attr("checked",false);
        }

        validarConfirmarRertorno();
    }

    //valida que en el item que tenga diferencia exista comentarios
    function validarNotas(ctr){
        var diferencia = calcularDiferencia(ctr)
           
        if(diferencia > 0){
            var OrderItemReturnID = $(ctr).attr("OrderItemReturnID");
            if(JsonDc[OrderItemReturnID].trim()==""){                
                showMessage('<%= Html.Term("YouMustEnterAnObservationInItemsWhichHasDifference", "You must enter an observation in items which has difference") %>', true);
                return false;
            }
        }
        
        return true;     
    }

    function validarConfirmarRertorno() {
        var tieneClassButtonOff= $("#btnReturnConfirm").hasClass("ButtonOff");
        var estanMarcadoTodos=true;
        var todasConObservaciones=true;
        $("#productsReturn tbody tr[data=true]").each(function () {
            var row = this;
            var checkbox = $(row).find("td:eq(5) input:checkbox");

            if (!checkbox.is(":checked")) {
                estanMarcadoTodos=false;
                return false;
            }
        });

        if(estanMarcadoTodos) {            
            if(tieneClassButtonOff) {
                $("#btnReturnConfirm").removeClass("ButtonOff");
            }

            $("#chkMarcarTodos").attr("checked",true);
                     
        }
        else {
            if(!tieneClassButtonOff) {
                $("#btnReturnConfirm").addClass("ButtonOff");
            }

            $("#chkMarcarTodos").attr("checked",false);
        }

        return estanMarcadoTodos;
    } 

    function marcarCheck(ctr, event) {           
        //var checkbox=$(ctr).parent("td").next().next().find("input:checkbox");
        //Event.preventDefault();
        var checkbox = $('.confirmCheck', ctr);
        //checkbox.attr("checked", false);

        //var thereIsDifference = validarNotas(ctr);
        checkbox.attr("checked", validarNotas(ctr));
        validarConfirmarRertorno();
    }

    function marcarTodos(ctr,evt) {
        var estado=$(ctr).is(":checked");
        var tieneClassButtonOff= $("#btnReturnConfirm").hasClass("ButtonOff");

        $("#productsReturn tbody tr[data=true]").each(function () {
            var row = this;
            var checkbox = $(row).find("td:eq(5) input:checkbox");
            $(checkbox).attr("checked",estado);
                    
            if(estado) {
            var txtCantidadRetorno= $(row).find("td:eq(2)");
            var txtCantidadRecepcion= $(row).find("td:eq(3) input:text"); 
            var txtCantidadDiferencia=  $(row).find("td:eq(4)");
            $(txtCantidadDiferencia).text("0");
            $(txtCantidadRecepcion).val($.trim($(txtCantidadRetorno).text()));
            }
        });
        
        if(estado) {
            if(tieneClassButtonOff) {
                $("#btnReturnConfirm").removeClass("ButtonOff");
            }                     
        }
        else {
            if(!tieneClassButtonOff) {
                $("#btnReturnConfirm").addClass("ButtonOff");
            }     
         }
    }

    function seleccionControl(ctr) {
        valorSeleccionado=$(ctr).val()==""||isNaN( $(ctr).val())?0:$.trim($(ctr).val());
    }

    function DeSeleccionControl(ctr) {
        var maxValue=parseInt( $(ctr).attr("maxValue"));
        var _valorSeleccionado=$(ctr).val()==""||isNaN( $(ctr).val())?0:$.trim($(ctr).val());
        if(_valorSeleccionado>maxValue) {
            $(ctr).val(valorSeleccionado);
        }
    }
        
    function isNumber(ctr,evt) {
        evt = (evt) ? evt : window.event;
        var charCode = (evt.which) ? evt.which : evt.keyCode;
        if (charCode > 31 && (charCode < 48 || charCode > 57)) {
            return false;
        }

        return true;
    }
 
    function verificarDevolucionTodo() {
        var esValido=true;
        $("#productsReturn tbody tr[data=true]").each(function () {
            var row = this;
            var checkbox = $(row).find("td:eq(5) input:checkbox");
            $(checkbox).attr("checked",estado);
                    
            if(estado) {
                var txtCantidadDiferencia =  $(row).find("td:eq(4)").val();
                if(txtCantidadDiferencia != 0) {
                    esValido=false;
                    return false;
                }
            }
        });
    }
 
    </script>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("Return", "Return") %></h2>
    </div>
    <table class="FormTable Section" width="100%">
        <tr>
            <td class="FLabel">
                <%= Html.Term("Products", "Products") %>
            </td>
            <td>
                <!-- Products In Order -->
                <table id="productsReturn" class="DataGrid" width="100%">
                    <thead>
                        <tr>
                            <th>
                            </th>
                            <th>
                            </th>
                            <th colspan="2">
                                <label for="chkMarcarTodos">
                                    Marcar toda la orden</label>
                                <input onchange="marcarTodos(this)" type="checkbox" id="chkMarcarTodos" />
                            </th>
                            <th>
                            </th>
                            <th>
                            </th>
                            <th>
                            </th>
                        </tr>
                        <tr class="GridColHead">
                            <%  %>
                            <th>
                                <%= Html.Term("CUV", "Codigo de producto") %>
                            </th>
                            <th>
                                <%= Html.Term("Nombre", "Nombre Producto") %>
                            </th>
                            <th>
                                <%= Html.Term("CantRetorno", "Cant. Retorno")%>
                            </th>
                            <th>
                                <%= Html.Term("CantRecepcion", "Cant. Recepcion")%>
                            </th>
                            <th>
                                <%= Html.Term("Diferencia", "Diferencia")%>
                            </th>
                            <th>
                                <%= Html.Term("Confirmar", "Confirmar")%><br />
                            </th>
                            <th>
                                <%= Html.Term("Observaciones", "Observaciones")%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                        <%			
                            List<NetSteps.Data.Entities.Business.OrderItemReturnConfirm> LstOrderItemReturnConfirm = ViewData["lstOrderItemReturnConfirm"] as List<NetSteps.Data.Entities.Business.OrderItemReturnConfirm>;

                            int total = LstOrderItemReturnConfirm.Count();
                            for (int index = 0; index < total; index++)
                            {%>
                        <tr data="true" orderitemreturnid="<%=LstOrderItemReturnConfirm[index].OrderItemReturnID %>">
                            <td>
                                <%=LstOrderItemReturnConfirm[index].CUV%>
                            </td>
                            <td>
                                <%=LstOrderItemReturnConfirm[index].ProductName%>
                            </td>
                            <td>
                                <%=LstOrderItemReturnConfirm[index].QuantityOrderItem%>
                            </td>
                            <td>
                                <input onblur="DeSeleccionControl(this)" onfocus="seleccionControl(this)" maxValue='<%=LstOrderItemReturnConfirm[index].QuantityOrderItem%>' onkeypress="return isNumber(this,event)" style="width: 45px" orderitemconfirmid="<%=LstOrderItemReturnConfirm[index].OrderItemConfirmID %>"                                   orderitemreturnid="<%=LstOrderItemReturnConfirm[index].OrderItemReturnID %>"                              quantyReceived="true"    onkeyup="validarDiferencia(this,event)" type="text" id="<%=LstOrderItemReturnConfirm[index].OrderItemReturnID %>" value=" <%=LstOrderItemReturnConfirm[index].QuantityOrderItemReturnConfirm%> " />
                            </td>
                            <td>
                                <%=LstOrderItemReturnConfirm[index].Diferencia%>
                            </td>
                            <td>
                                <% if (LstOrderItemReturnConfirm[index].Diferencia == 0)
                                   {%>
                                <input onchange="marcarCheck(this, event)" checked="checked" type="checkbox" class="confirmCheck" orderitemconfirmid="<%=LstOrderItemReturnConfirm[index].OrderItemConfirmID %>"
                                    orderitemreturnid="<%=LstOrderItemReturnConfirm[index].OrderItemReturnID %>" />
                                <%}
                                   else
                                   {%>
                                <input onchange="marcarCheck(this, event)" type="checkbox" class="confirmCheck" orderitemconfirmid="<%=LstOrderItemReturnConfirm[index].OrderItemConfirmID %>"
                                    orderitemreturnid="<%=LstOrderItemReturnConfirm[index].OrderItemReturnID %>" />
                                <%}%>
                            </td>
                            <td orderitemreturnid="<%= LstOrderItemReturnConfirm[index].OrderItemReturnID%>"
                                onclick="verObservacion(this)">
                                <a href="#"><%=  Html.Term("Observacion", "Observacion")%></a>
                            </td>
                        </tr>
                        <%} %>
                        <tr><td colspan= "7"></td></tr>
                        <tr>
                            <td colspan="7">
                                Observaciones:
                            </td>
                        </tr>
                        <tr>
                            <td colspan="7">
                                <textarea cols="10" style="width: 550px" rows="8" id="txtObservacionOrden"><%= ViewData["Note"]%></textarea>
                            </td>
                            <td colspan="2">
                            </td>
                        </tr>
                    </tbody>
                </table>
                <table class="FormTable" width="100%">
                    <tr>
                        <td class="FLabel">
                        </td>
                        <td>
                            <div>
                                <!--/ End Entry Form -->
                                <p class="NextSection">
                                    <a id="btnSave" class="Button BigBlue" href="javascript:void(0);"><span>
                                        <%= Html.Term("Save", "Save")%></span></a>
                                    <a id="btnReturnConfirm" class="Button BigBlue ButtonOff" href="javascript:void(0);">
                                        <span>
                                            <%= Html.Term("ReturnConfirm", "Return Confirm")%></span></a>
                                </p>
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <div>
        <input type="hidden" id="orderId" name="orderId" value="<%= Model.OrderID%>"/>
    </div>
    
    <div id="noteModal" class="jqmWindow LModal">
        <div class="mContent">
            <h2>
                Observacion
            </h2>
            <div id="noteErrors">
            </div>
            <div class="FormStyle2">
                <textarea id="txtObservacionItem" style="width: 20.833em; height: 8.333em;"></textarea>
            </div>
            <br />
            <input type="hidden" id="parentNoteID" style="width: 20.833em;" value="" /><br />
            <p>
                <a href="javascript:void(0);" id="btnSaveObservacion" onclick="guardarItemObservacion()" class="Button BigBlue">
                    <%= Html.Term("Save")%></a> <a href="javascript:void(0);" class="Button LinkCancel jqmClose"
                        onclick="cancelar()" id="btnCancelObservacion">
                        <%= Html.Term("Cancel")%></a>
            </p>
            <span class="ClearAll"></span>
        </div>
        <div class="PModal WaitWin">
			<span>
				<%= Html.Term("OneMomentPlease", "One moment please...") %>
			</span>
			<br />
			<img src="<%= ResolveUrl("~/Content/Images/processing.gif") %>" alt="<%= Html.Term("Processing", "processing...") %>" />
		</div>
    </div>
    <script>
        $(function () {
            $("input[quantyReceived=true]").limitkeypress({ rexp: /^[0-9]{1,3}$/ });
        });
    </script>
</asp:Content>
