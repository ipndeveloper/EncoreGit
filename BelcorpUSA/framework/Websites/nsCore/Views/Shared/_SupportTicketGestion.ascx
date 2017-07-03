<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<System.Collections.Generic.List<NetSteps.Data.Entities.Business.SupportTicketGestionBE>>" %> 

<%
    List<SupportTicketGestionBE> lstSupportTicketGestionBE = Model as List<SupportTicketGestionBE>;
 %>

    <div class="demo">
    <span class="ClearAll"></span>
    <div class="ModTitle" id="uxNotesDiv">
       <%= Html.Term("DetailGestion", "DETAIL GESTION")%>   <span class="ClearAll"></span>
    </div>
    <%for (int indice = 0; indice < lstSupportTicketGestionBE.Count(); indice++)
      {  %>
    <div id="notesSuportGestion">
       <div class="AcctNote">
            <span class="FL NoteTitle">
               <%-- <b>hghghgtiytulo (#8267)</b> (0 Follow-up(s))--%>
            </span>
            <span class="ClearAll">
            </span>
            <span class="NoteAuthor">
                <%= Html.Term("PostedOn") %>: 
                  <%= lstSupportTicketGestionBE[indice].DateCreatedUTC.ToShortDateStringDisplay(CoreContext.CurrentCultureInfo)%>
                <br>   <%= Html.Term("PostedBy")%>:  <%= lstSupportTicketGestionBE[indice].Username%><br>  <%= Html.Term("PublishNoteToOwner")%>:<% =lstSupportTicketGestionBE[indice].isInternal?"Yes":"no"%>
             
             <br>   <%= Html.Term("StatusSupportGestion","Status")%>:  <%= lstSupportTicketGestionBE[indice].NameStatus%>

            </span>
       <strong >
        <%= lstSupportTicketGestionBE[indice].Descripction%>
       </strong>   

        </div>
    </div>
    <%} %>
    </div>