<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Support.Models.Ticket.SupportTicketDetaillModel>" %>
<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/timepickr.js") %>"></script>
<hr />
<style>
    .button
    {
        background-image: url ( '~/Content/Images/Icons/16x16/search-trans.png' ) no-repeat;
        cursor: pointer;
</style>

<table id="tblDetaill">
    <tr tipo='separador' filadinamica="FilaDinamica" tipocontroles="Separador">
        <td colspan="2">
            <hr />
        </td>
    </tr>
    <% 
        NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();
        Func<bool, bool, string> FncHabilitarDeshabilitar = (estado, pisDws) =>
        {
            if (ticketToEdit.SupportTicketID > 0)
            {
                estado = true;
                pisDws = true;
            }
            return (!estado || pisDws) ? "disabled='disabled'" : "";
        };

        Func<bool, string> FncHabilitarDeshabilitarClass = (estado) =>
        {
            return !estado ? "class='ModoLectura'" : "class='myButton'";
        };
        var objSupportTicketDetaillModel = Model as nsCore.Areas.Support.Models.Ticket.SupportTicketDetaillModel;
        bool ModoEdicion = objSupportTicketDetaillModel.ModoEdicion;
        bool isDws = objSupportTicketDetaillModel.IsSiteDWS;

        List<SupportMotivePropertyTypes> LstNoSolucion = objSupportTicketDetaillModel.LstSupportMotivePropertyTypes.FindAll((o) => !o.FieldSolution);
        List<SupportMotivePropertyTypes> LstSolucion = objSupportTicketDetaillModel.LstSupportMotivePropertyTypes.FindAll((o) => o.FieldSolution);

        //objSupportTicketDetaillModel.LstSupportMotivePropertyTypes
        for (int index = 0; index < LstNoSolucion.Count; index++)
        { %>
    <tr filadinamica="FilaDinamica" solucion="no">
        <td class="FLabel" colspan="2">
            <%=LstNoSolucion[index].Name%>:
        </td>
    </tr>
    <tr filadinamica="FilaDinamica" solucion="no" tipocontroles="Propiedades">
        <td>
        </td>
        <td>
            <%if (LstNoSolucion[index].DataType == "List")
              { %>
            <select <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> onchange="VerificarValidacionControl(this)"
                datatype="<%=LstNoSolucion[index].DataType%>" required="<%=LstNoSolucion[index].Required?"1":"0"%>"
                supportticketspropertyid="<%= LstNoSolucion[index].SupportTicketsPropertyID%>"
                supportmotivepropertytypeid="<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"
                id="Cmb_<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>">
                <option value="0">-----<%= Html.Term("SelectedValue", "Selected Value")%>-----</option>
                <%
                  List<SupportMotivePropertyValues> LstSupportMotivePropertyValues =
                      objSupportTicketDetaillModel.LstSupportMotivePropertyValues.FindAll((obj) => obj.SupportMotivePropertyTypeID == LstNoSolucion[index].SupportMotivePropertyTypeID) ?? new List<SupportMotivePropertyValues>();

                  for (int indiceSupprtValues = 0; indiceSupprtValues < LstSupportMotivePropertyValues.Count; indiceSupprtValues++)
                  { %>
                <% if (LstSupportMotivePropertyValues[indiceSupprtValues].SupportMotivePropertyValueID == LstNoSolucion[index].SupportTicketsPropertyValueID)
                   {%>
                <option selected="selected" value="<%= LstSupportMotivePropertyValues[indiceSupprtValues].SupportMotivePropertyValueID%>">
                    <%= LstSupportMotivePropertyValues[indiceSupprtValues].Value%></option>
                <% }
                   else
                   {%>
                <option value="<%= LstSupportMotivePropertyValues[indiceSupprtValues].SupportMotivePropertyValueID%>">
                    <%= LstSupportMotivePropertyValues[indiceSupprtValues].Value%></option>
                <%}%>
                <%}%>
            </select>
            <%}
              else
              {%>
            <%if (LstNoSolucion[index].DataType == "Text")
              { %>
            <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> onchange="VerificarValidacionControl(this)"
                datatype="<%=LstNoSolucion[index].DataType%>" required="<%=LstNoSolucion[index].Required?"1":"0"%>"
                supportmotivepropertytypeid="<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"
                type="text" supportticketspropertyid="<%= LstNoSolucion[index].SupportTicketsPropertyID%>"
                id="Txt_<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>" value="<%= LstNoSolucion[index].PropertyValue%>" />
            <%} %>
            <%if (LstNoSolucion[index].DataType == "MultiLine")
              { %>
            <textarea style="height: 1cm; overflow-y: scroll" class="fullWidth" <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %>
                onchange="VerificarValidacionControl(this)" datatype="<%=LstNoSolucion[index].DataType%>"
                required="<%=LstNoSolucion[index].Required?"1":"0"%>" supportmotivepropertytypeid="<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstNoSolucion[index].SupportTicketsPropertyID%>"
                id="TxtArea_<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"><%= LstNoSolucion[index].PropertyValue%></textarea>
            <%} %>
            <%if (LstNoSolucion[index].DataType == "Numeric")
              { %>
            <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> onchange="VerificarValidacionControl(this)"
                datatype="<%=LstNoSolucion[index].DataType%>" required="<%=LstNoSolucion[index].Required?"1":"0"%>"
                supportmotivepropertytypeid="<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstNoSolucion[index].SupportTicketsPropertyID%>"
                id="Txt_<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>" onkeypress="return isNumberKey(event)"
                value="<%= LstNoSolucion[index].PropertyValue%>" />
            <%}%>
            <%if (LstNoSolucion[index].DataType == "Busqueda")
              {
            %>
            <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> id="Txt_<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"
                readonly="readonly" onchange="VerificarValidacionControl(this)" datatype="<%=LstNoSolucion[index].DataType%>"
                required="<%=LstNoSolucion[index].Required?"1":"0"%>" supportmotivepropertytypeid="<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstNoSolucion[index].SupportTicketsPropertyID%>"
                value="<%= LstNoSolucion[index].PropertyValue%>" style="color: Gray" />
            <input type="button" name="button" value="<%= Html.Term("Search", "Search")%>" onclick="OpenWindowSearch(this)"
                class="button" />
            <input class="BusquedaType" <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> style="visibility:hidden;" readonly="readonly" value="<%= LstNoSolucion[index].DinamicName%>" />
            <%}%>        
            <%if (LstNoSolucion[index].DataType == "Date")
              { %>
            <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> datatype="<%=LstNoSolucion[index].DataType%>"
                required="<%=LstNoSolucion[index].Required?"1":"0"%>" supportmotivepropertytypeid="<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstNoSolucion[index].SupportTicketsPropertyID%>"
                id="Txt_<%= LstNoSolucion[index].SupportMotivePropertyTypeID%>" value="<%= LstNoSolucion[index].PropertyValue%>"
                type="Date" />
            <%}%>
            <%} %>
        </td>
    </tr>
    <%  }%>
    <%List<SupportMotiveTask> lstSupportMotiveTask =
                          objSupportTicketDetaillModel.LstSupportMotiveTask ?? new List<SupportMotiveTask>();
                          
                     

    %>
    <% if (lstSupportMotiveTask.Count > 0)
       {  %>
    <tr filadinamica="FilaDinamica" tipocontroles="Task">
        <td>
        </td>
        <td>
            <%for (int IndexTask = 0; IndexTask < lstSupportMotiveTask.Count; IndexTask++)
              { %><%--link--%>
            <input propiedad="<%=lstSupportMotiveTask[IndexTask].SupportMotivePropertyTypeID %>"
                <%=FncHabilitarDeshabilitarClass(ModoEdicion) %> id="Task_<%=lstSupportMotiveTask[IndexTask].SupportMotiveTaskID %>"
                onclick="GuardarFormulario(this)" url="<%= lstSupportMotiveTask[IndexTask].Link%>"
                type="button" value="<%= lstSupportMotiveTask[IndexTask].Name%>" />
            <% }%>
        </td>
    </tr>
    <% }
    %>
    <%
    
        for (int index = 0; index < LstSolucion.Count; index++)
        { %>
    <tr filadinamica="FilaDinamicaSolucion" solucion="si" tipocontroles="Propiedades">
        <td class="FLabel">
            <%=LstSolucion[index].Name%>:
        </td>
        <td>
            <%if (LstSolucion[index].DataType == "List")
              { %>
            <select <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> onchange="VerificarValidacionControl(this)"
                datatype="<%=LstSolucion[index].DataType%>" required="<%=LstSolucion[index].Required?"1":"0"%>"
                supportticketspropertyid="<%= LstSolucion[index].SupportTicketsPropertyID%>"
                supportmotivepropertytypeid="<%= LstSolucion[index].SupportMotivePropertyTypeID%>"
                id="Select1">
                <option value="0">-----<%= Html.Term("SelectedValue", "Selected Value")%>-----</option>
                <%
                  List<SupportMotivePropertyValues> LstSupportMotivePropertyValues =
                      objSupportTicketDetaillModel.LstSupportMotivePropertyValues.FindAll((obj) => obj.SupportMotivePropertyTypeID == LstSolucion[index].SupportMotivePropertyTypeID) ?? new List<SupportMotivePropertyValues>();

                  for (int indiceSupprtValues = 0; indiceSupprtValues < LstSupportMotivePropertyValues.Count; indiceSupprtValues++)
                  { %>
                <% if (LstSupportMotivePropertyValues[indiceSupprtValues].SupportMotivePropertyValueID == LstSolucion[index].SupportTicketsPropertyValueID)
                   {%>
                <option selected="selected" value="<%= LstSupportMotivePropertyValues[indiceSupprtValues].SupportMotivePropertyValueID%>">
                    <%= LstSupportMotivePropertyValues[indiceSupprtValues].Value%></option>
                <% }
                   else
                   {%>
                <option value="<%= LstSupportMotivePropertyValues[indiceSupprtValues].SupportMotivePropertyValueID%>">
                    <%= LstSupportMotivePropertyValues[indiceSupprtValues].Value%></option>
                <%}%>
                <%}%>
            </select>
            <%}
              else
              {%>
            <%if (LstSolucion[index].DataType == "Text")
              { %>
            <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> onchange="VerificarValidacionControl(this)"
                datatype="<%=LstSolucion[index].DataType%>" required="<%=LstSolucion[index].Required?"1":"0"%>"
                supportmotivepropertytypeid="<%= LstSolucion[index].SupportMotivePropertyTypeID%>"
                type="text" supportticketspropertyid="<%= LstSolucion[index].SupportTicketsPropertyID%>"
                id="Txt_<%= LstSolucion[index].SupportMotivePropertyTypeID%>" value="<%= LstSolucion[index].PropertyValue%>" />
            <%} %>
            <%if (LstSolucion[index].DataType == "MultiLine")
              { %>
            <textarea style="height: 1cm; overflow-y: scroll" class="fullWidth" <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %>
                onchange="VerificarValidacionControl(this)" datatype="<%=LstSolucion[index].DataType%>"
                required="<%=LstSolucion[index].Required?"1":"0"%>" supportmotivepropertytypeid="<%= LstSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstSolucion[index].SupportTicketsPropertyID%>"
                id="TxtArea_<%= LstSolucion[index].SupportMotivePropertyTypeID%>"> <%= LstSolucion[index].PropertyValue%></textarea>
            <%} %>
            <%if (LstSolucion[index].DataType == "Numeric")
              { %>
            <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> onchange="VerificarValidacionControl(this)"
                datatype="<%=LstSolucion[index].DataType%>" required="<%=LstSolucion[index].Required?"1":"0"%>"
                supportmotivepropertytypeid="<%= LstSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstSolucion[index].SupportTicketsPropertyID%>"
                id="Txt_<%= LstSolucion[index].SupportMotivePropertyTypeID%>" onkeypress="return isNumberKey(event)"
                value="<%= LstSolucion[index].PropertyValue%>" />
            <%}%>
            <%if (LstSolucion[index].DataType == "Busqueda")
              {
            %>
            <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> id="Txt_<%= LstSolucion[index].SupportMotivePropertyTypeID%>"
                readonly="readonly" onchange="VerificarValidacionControl(this)" datatype="<%=LstSolucion[index].DataType%>"
                required="<%=LstSolucion[index].Required?"1":"0"%>" supportmotivepropertytypeid="<%= LstSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstSolucion[index].SupportTicketsPropertyID%>"
                value="<%= LstSolucion[index].PropertyValue%>" style="color: Gray" />
            <input type="button" name="button" value="<%= Html.Term("Search", "Search")%>" onclick="OpenWindowSearch(this)"
                class="button" />
            <input class="BusquedaType" <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> style="visibility:hidden;" readonly="readonly" value="<%= LstSolucion[index].DinamicName%>" />
            <%}%>
            <%if (LstSolucion[index].DataType == "Date")
              { %>
                <input <%=FncHabilitarDeshabilitar(ModoEdicion,isDws) %> datatype="<%=LstSolucion[index].DataType%>"
                required="<%=LstSolucion[index].Required?"1":"0"%>" supportmotivepropertytypeid="<%= LstSolucion[index].SupportMotivePropertyTypeID%>"
                supportticketspropertyid="<%= LstSolucion[index].SupportTicketsPropertyID%>"
                id="Txt_<%= LstSolucion[index].SupportMotivePropertyTypeID%>" value="<%= LstSolucion[index].PropertyValue%>"
                type="Date" />
            <%}%>
            <%} %>
        </td>
    </tr>
    <%  }%>
</table>
<hr />
