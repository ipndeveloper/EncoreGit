<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<nsCore.Areas.Support.Models.Ticket.JerarquiaSupportLevelModel>" %>


<tr>
    <%--inicio fial --%>
    <td>
    Cateogry:
    </td>
    <td colspan="10">

    <table>
    <tbody>
    <tr>

<%
nsCore.Areas.Support.Models.Ticket.JerarquiaSupportLevelModel objJerarquiaSupportLevelModel = Model as nsCore.Areas.Support.Models.Ticket.JerarquiaSupportLevelModel;
int totalHijosJerarquiaDescenedenteSupportLevel=objJerarquiaSupportLevelModel.lstHijosJerarquiaDescenedenteSupportLevel.Count;

List<SupportLevelSearchData> lstHijosJerarquiaDescenedenteSupportLevel = objJerarquiaSupportLevelModel.lstHijosJerarquiaDescenedenteSupportLevel;
        
List<SupportLevelSearchData> lstHijosJerarquiaAscendenteSupportLevel =objJerarquiaSupportLevelModel.lstHijosJerarquiaAscendenteSupportLevel ;
List<SupportLevelSearchData> ListaPrimerNivel=lstHijosJerarquiaAscendenteSupportLevel .FindAll((obj)=>obj.ParentSupportLevelID==0);
  
  if(totalHijosJerarquiaDescenedenteSupportLevel>0){
   %>
  
     <td>
     <select>
        <%for (int indice = 0; indice < ListaPrimerNivel.Count; indice++)
        { 
        %>
            <%if (ListaPrimerNivel[indice].SupportLevelID == lstHijosJerarquiaDescenedenteSupportLevel[lstHijosJerarquiaDescenedenteSupportLevel.Count - 1].SupportLevelID)
              { %>
                     <option id="<% %>" selected="selected"  value ="<%=ListaPrimerNivel[indice].SupportLevelID %>"><%=ListaPrimerNivel[indice].Name%></option>
            <%}
              else
              {%>
                      <option id="Option1" value ="<%=ListaPrimerNivel[indice].SupportLevelID %>"><%=ListaPrimerNivel[indice].Name%></option>
            <%} %>
        <%
        }
            lstHijosJerarquiaDescenedenteSupportLevel = lstHijosJerarquiaDescenedenteSupportLevel.OrderBy((ob)=>ob.SupportLevelID).ToList().FindAll((obj) => obj.ParentSupportLevelID != 0);
        %>
    </select>
    </td>
            <%  for (int indice = 0; indice < lstHijosJerarquiaDescenedenteSupportLevel.Count; indice++)
                {
                    List<SupportLevelSearchData> ListaHijos = lstHijosJerarquiaAscendenteSupportLevel.FindAll((obj) => obj.ParentSupportLevelID == lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID);
            %>
            <td id="celda<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>-<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>">
            <select  onchange="cargarHijo(this)"  id="<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>-<%=lstHijosJerarquiaDescenedenteSupportLevel[indice].ParentSupportLevelID %>">
            <%for (int indiceHijos = 0; indiceHijos < ListaHijos.Count; indiceHijos++)
              {%>
                <option value="<%=ListaHijos[indiceHijos].SupportLevelID %>"><%=ListaHijos[indiceHijos].Name%></option>
            <%} %>
            </select>
            </td>
            <%
                }
            %>
            <%
      List<System.Tuple<int, string, int>> LstSupportLevelMotive = SupportTicket.GetLevelSupportLevelMotive(lstHijosJerarquiaDescenedenteSupportLevel[lstHijosJerarquiaDescenedenteSupportLevel.Count-1].SupportLevelID) ?? new List<System.Tuple<int, string, int>>();
   
    if(LstSupportLevelMotive.Count>0){
    %>
    <select  onchange="SeleccionarMotivo(this)"  id="cmbMotivos">
    <%for(int indice =0;indice <LstSupportLevelMotive.Count;indice++){ %>
    <option SupportLevelID="<%=lstHijosJerarquiaDescenedenteSupportLevel[0].SupportLevelID %>" SupportMotiveID="<%=LstSupportLevelMotive[indice].Item3%>" value ="<%=LstSupportLevelMotive[indice].Item3%>">
    <%= LstSupportLevelMotive[indice].Item2%>
    </option>
    <%} %>
    </select>
    <%} }%>

    <%--fin fila--%>
      </tr>
    </tbody>
      </table>
     </td>
 </tr>