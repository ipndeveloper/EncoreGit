<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Admin/Views/Shared/Admin.Master"
    Inherits="System.Web.Mvc.ViewPage<IList<NetSteps.Common.Interfaces.IListValue>>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">

    <script type="text/javascript">
    	$(function () {
    		$('#btnAdd').bind('click', addListValue);

    		$('.delete').die().live('click', function () {
    			if (confirm('Would you like to delete this list value?')) {
    				var listValue = $(this).parent().find('input'), listValueId = listValue.attr('name').replace(/\D/g, '');
    				if (listValueId > 0) {
    					var data = { type: '<%= ViewData["ListType"] %>', listValueId: listValueId };
    					$.post('<%= ResolveUrl("~/Admin/ListTypes/DeleteValue") %>', data, function (response) {
    						if (response.result) {
    							listValue.parent().fadeOut('normal', function () {
    								$(this).remove();
    								$('#btnAdd').die('click').bind('click', addListValue);
    							});
    						}
    						else {
    							showMessage(response.message, true);
    							return false;
    						}
    					});
    				}
    				else if (listValue) {
    					listValue.parent().fadeOut('normal', function () {
    						$(this).remove();
    						$('#btnAdd').die('click').bind('click', addListValue);
    					});
    				}
    			}
    		});

    		$('.sortDown').click(function () {
    			var data = { type: '<%= ViewData["ListType"] %>' }, t = $(this);

    			data['sortIndex'] = this.name;
    			data['direction'] = 'Descending';

    			showLoading(t);
    			$.post('<%= ResolveUrl("~/Admin/ListTypes/Move") %>', data, function (response) {
    				hideLoading(t);
    				if (response.result) {
    					window.location.reload();
    				}
    				else {
    					showMessage(response.message, true);
    					return false;
    				}
    			});
    		});

    		$('.sortUp').click(function () {
    			var data = { type: '<%= ViewData["ListType"] %>' }, t = $(this);

    			data['sortIndex'] = this.name;
    			data['direction'] = 'Ascending';

    			showLoading(t);
    			$.post('<%= ResolveUrl("~/Admin/ListTypes/Move") %>', data, function (response) {
    				hideLoading(t);
    				if (response.result) {
    					window.location.reload();
    				}
    				else {
    					showMessage(response.message, true);
    					return false;
    				}
    			});
    		});

    		$('#btnSave').click(function () {

    			// Ensure all inputs are filled
    			if (checkForEmptyListTypes()) {
    				var data = { type: '<%= ViewData["ListType"] %>' }, t = $(this);

    				showLoading(t);

    				$('#listValues .listValue').each(function (i) {
    					data['listValues[' + i + '].Key'] = $(this).attr('name').replace(/\D/g, '');
    					data['listValues[' + i + '].Value'] = $(this).val();
    				});

    				$('#listValues .sortIndexValue').each(function (i) {
    					data['sortIndexValues[' + i + '].Key'] = $(this).attr('name').replace(/\D/g, '');
    					data['sortIndexValues[' + i + '].Value'] = $(this).val();
    				});

    				$.post('<%= ResolveUrl("~/Admin/ListTypes/SaveValues") %>', data, function (response) {
    					hideLoading(t);
    					if (response.result) {
    						// window.location = '<%= ResolveUrl("~/Admin/ListTypes") %>';
    						window.location = '<%= ResolveUrl("~/Admin/ListTypes/Values/" + ViewData["ListType"].ToString()) %>';
    						showMessage(response.message);
    					}
    					else {
    						showMessage(response.message, true);
    						return false;
    					}
    				});
    				//checkListTypes();
    			}
    		});
    	});

    	function addListValue() {
    		$('#listValues').append('<li><input type="text" name="value0" class="listValue" /><a href="javascript:void(0);" class="delete"><span class="UI-icon icon-x" title="<%= Html.Term("Delete", "Delete")%>"></span></a></li>');
    		$('#btnAdd').unbind('click');
    	}

    	function checkForEmptyListTypes() {
    		var listTypeInputs = $('ul#listValues > li > input.listValue');
    		var result = true;

    		if (!onlyLockedListTypes()) {
    			listTypeInputs.each(function (i) {
    				if ($(this).val().length === 0) {
    					result = false;
    				}

    			});
    			return result;
    		}
    		return false;
    	}

    	function onlyLockedListTypes() {
    		var listTypeInputs = $('ul#listValues > li > input');
    		var result = true;

    		listTypeInputs.each(function (i) {
    			if ($(this).hasClass('listValue')) {
    				result = false;
    			}
    		});

    		return result;
    	}

    </script>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Admin") %>">
        <%= Html.Term("Admin", "Admin") %></a> > <a href="<%= ResolveUrl("~/Admin/ListTypes") %>">
            <%= Html.Term("ListTypes", "List Types")%></a> >
    <%= Html.Term("ListValues", "List Values")%>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ListValues", "List Values")%>
            -
            <%  String mensaje;

                mensaje = ViewData["ListType"].ToString().Trim();
                
                switch (ViewData["ListType"].ToString().Trim())
               {
                   case "AccountStatusChangeReason": mensaje = Html.Term("AccountStatusChangeReason", "Account Status Change Reason");
                       break;
                   case "ArchiveType": mensaje = Html.Term("ArchiveType", "Archive Type");
                       break;
                   case "CommunicationPreference": mensaje = Html.Term("CommunicationPreference", "Communication Preference");
                       break;
                   case "ContactCategory": mensaje = Html.Term("ContactCategory", "Contact Category");
                       break;
                   case "ContactMethod": mensaje = Html.Term("ContactMethod", "Contac tMethod");
                       break;
                   case "NewsType": mensaje = Html.Term("NewsType", "News Type");
                       break;
                   case "ReturnReasons": mensaje = Html.Term("ReturnReasons", "ReturnReasons");
                       break;
                   case "ReturnTypes": mensaje = Html.Term("ReturnTypes", "ReturnTypes");
                       break;
                   case "SiteStatusChangeReason": mensaje = Html.Term("SiteStatusChangeReason", "Site Status Change Reason");
                       break;
                   case "SupportTicketCategory": mensaje = Html.Term("SupportTicketCategory", "Support Ticket Category");
                       break;
                   case "SupportTicketPriority": mensaje = Html.Term("SupportTicketPriority", "Support Ticket Priority");
                       break;
                   case "ReplacementReason": mensaje = Html.Term("ReplacementReason", "Replacement Reason");
                       break;
                   case "ContactType": mensaje = Html.Term("ContactType", "Contact Type");
                       break;
                   case "ContactStatus": mensaje = Html.Term("ContactStatus", "Contact Status");
                       break;
                    default:
                       mensaje = "No hay nada";
                        break;
               }
               %>

                <%= mensaje %>

        </h2>
        <a href="javascript:void(0);" id="btnAdd">
            <%= Html.Term("AddNewlistValue", "Add new list value") %></a>
    </div>
    <% if (Model.Any() && Model[0] is NewsType)
       { %>
    <div class="UI-lightBg brdrAll GridFilters">
        <div class="FL FilterSet">
            <div class="FL">
                Current Language: <%= CoreContext.CurrentLanguage.GetTerm() %>
                <br />
                <%= Html.Term("ListValues_SelectDifferentLangauge", "(Use the language selection in the upper right to change the translation and sort order by a different language)") %>
            </div>
        </div>
         <span class="clr"></span>
    </div>
    <br />
    <% } %>
    <ul id="listValues" class="listValues">
        <% 
                        
          foreach (var value in Model)
          {
              bool showDown = Model.IndexOf(value) == Model.Count - 1 ? false : true;
              bool showUp = Model.IndexOf(value) == 0 ? false : true;
              
              %>
        <li>
            <%if (!(value is IEditable) || (value as IEditable).Editable)
              {
                  var hasSortIndex = false;
                  var sortIndex = 0;

                  if (value is NetSteps.Common.Interfaces.ISortIndex)
                  {
                      hasSortIndex = true;
                      sortIndex = ((NetSteps.Common.Interfaces.ISortIndex)value).SortIndex;
                  }
                  else if (value is NewsType)
                  {
                      hasSortIndex = true;
                      sortIndex = ((NewsType)value).GetSortIndexByLanguage(CoreContext.CurrentLanguageID);
                  }

                  string valueTitle = value.Title;
                  if (value is ITermName)
                  {
                      valueTitle = (value as ITermName).GetTerm();
                  }
            %>
            <input type="text" name="value<%= value.ID %>" value="<%= valueTitle %>" class="listValue" />
            <% 
                
                  if (hasSortIndex)
                  {

            %>
            <input type="hidden" name="sortIndex<%= sortIndex %>" value="<%= sortIndex %>" class="sortIndexValue" />
            <a href="javascript:void(0);" name="<%= sortIndex %>" class="sortUp moveArrow" <%= showUp ? string.Empty : "style=\"Visibility: Hidden;\"" %>><span class="UI-icon icon-ArrowUp" title="<%= Html.Term("MoveUp", "Move Up") %>"></span></a> <a href="javascript:void(0);" name="<%= sortIndex %>" class="sortDown moveArrow" <%= showDown ? string.Empty : "style=\"Visibility: Hidden;\"" %>><span class="UI-icon icon-ArrowDown" title="<%= Html.Term("MoveDown", "Move Down") %>"></span></a>
            <% 
                
                  }
                  
            %>
            <a href="javascript:void(0);" class="delete">
                <span title="<%= Html.Term("Delete", "Delete") %>" class="UI-icon icon-x"></span></a>
            <% 
                
              }
              else
              { 
            %>
            <span class="UI-icon icon-lock" title="locked"></span><input type="text" value="<%= value.Title %>" disabled="disabled" />
            <%} %>
        </li>
        <%} %>
    </ul>
    <span class="ClearAll"></span>
    <p>
        <a href="javascript:void(0);" class="Button BigBlue" id="btnSave">
            <%= Html.Term("Save", "Save") %></a>
        <a href="<%= ResolveUrl("~/Admin/ListTypes") %>" class="Button">
            <%= Html.Term("Cancel", "Cancel") %></a>
    </p>
</asp:Content>
