<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Support/Views/Shared/Support.Master"
 Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Support.Models.SupportLevel.EditLevelTreeModel>" 
 %>
 <asp:Content ID="Content4" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
 <asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
 <div class="SectionNav">
	<% 
		NetSteps.Data.Entities.SupportTicket ticketToEdit = CoreContext.CurrentSupportTicket ?? new NetSteps.Data.Entities.SupportTicket();		        
	%>
	<ul class="SectionLinks"> 
      <%= Html.SelectedLink("~/Support/Level/EditTree/", Html.Term("SupportLevels", "Support Levels"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
      <%= Html.SelectedLink("~/Support/Motive/Index/", Html.Term("BrowseSupportMotives", "Browse Support Motives"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>
      <%= Html.SelectedLink("~/Support/Motive/Edit/", Html.Term("AddNewSupportMotive", "Add New Support Motive"), LinkSelectionType.ActualPageWithPossibleID, CoreContext.CurrentUser, "")%>           		
	</ul>
    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
	<%--<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/adapters/jquery.js"></script>--%>
    <script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/") %>Scripts/ajaxupload.js"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/") %>Scripts/tree.min.js"></script>
	<script type="text/javascript">

	    var firstPass = true, levelStatus = {}, lastLeafId = "";

	    $(function () {

	        createTree();

	        $("#category<%=Model.levelId %>").find("a").focus();
	        $("#category<%=Model.levelId %>").find("a").click();



	        $('#levelDetalleCenter').messageCenter({ color: '#458416', background: '#e6fad9', iconPath: 'UI-icon icon-check' }).messageCenter('addMessage', 'You are adding a root Level.');

	        $('#addRoot').click(function () {
	            //	            alert('sas');
	            $('#levelDetalleCenter').messageCenter('clearAllMessages').messageCenter('addMessage', '<%= Html.Term("YouAreAddingaRootLevel.", "You are adding a root Level.")%>');
	            //	            $('#levelDetalleCenter').messageCenter('addMessage', 'You are adding a root Level.');
	            resetLevelDetails('<%= Model.LevelTree.SupportLevelID %>');
	        });

	        $('#btnDeleteLevel').click(function () {
	            if ($('#levelId').val() && confirm('Are you sure you want to delete this level and all child levels?')) {
	                $.post('<%= ResolveUrl(Model.DeleteURL) %>', { levelId: $('#levelId').val() }, function (response) {
	                    if (response.result) {
	                        $('#level' + $('#levelId').val()).remove();
	                        $('#addRoot').click();

	                        $("#levelTree").replaceWith("<div id='levelTree' class='LevelTree'></div>");
	                        $('#levelTree').html(response.levels);
	                        createTree(); // rebinds events and formats the tree by adding css classes
	                        $('#levelId').val(response.levelId);

	                        $('#levelDetalleCenter').messageCenter('clearAllMessages').messageCenter('addMessage', 'You are editing ' + $('#Level' + response.levelId + ' > a.Level').text() + '.');

	                        showMessage('Level deleted!', false);

	                        resetLevelDetails(0);
	                        //	                        if (response.Eliminado) {
	                        //	                           

	                        //	                        } else {
	                        //	                            showMessage(response.message, true);
	                        //	                        }


	                    } else {
	                        showMessage(response.message, true);
	                    }
	                });
	            }
	        });

	        $('#btnSave').click(function () {


	            if (!$('#levelDetails').checkRequiredFields()) {
	                return false;
	            }
	            var t = $(this);
	            showLoading(t);

	            $.post('<%= ResolveUrl(Model.SaveURL) %>',
	        	{
	        	    levelId: $('#levelId').val(),
	        	    name: $('#txtName').val(),
	        	    descripction: $('#txtDescription').val(),
	        	    active: $('#chkActive').prop('checked'),
	        	    visible: $('#chkVisible').prop('checked'),
	        	    parentId: $('#parentId').val(),
	        	    sortIndex: $('#sortIndex').val(),
	        	    languageId: 1
	        	},

	        	function (response) {

	        	    hideLoading(t);
	        	    if (response.result) {
	        	        $('#levelTree li').each(function () {
	        	            if (!($(this).hasClass("last") && $(this).hasClass("leaf"))) {
	        	                levelStatus[$(this).attr('id')] = $(this).attr('class');
	        	            }
	        	        });
	        	        $("#levelTree").replaceWith("<div id='levelTree' class='LevelTree'></div>");
	        	        $('#levelTree').html(response.levels);
	        	        createTree(); // rebinds events and formats the tree by adding css classes
	        	        $('#levelId').val(response.levelId);
	        	        $('#levelDetalleCenter').messageCenter('clearAllMessages').messageCenter('addMessage', 'You are editing ' + $('#Level' + response.levelId + ' > a.Level').text() + '.');
	        	       
	        	        resetLevelDetails(0);
	        	        showMessage('Level saved!', false);


	        	    } else {
	        	        showMessage(response.message, true);
	        	    }
	        	});
	        });



	        //fin 
	    });

	 function resetLevelDetails(parentId) {
	     $('#levelId,#txtName,#txtDescription').val('');
	     //	            $('#imgPreview').attr('src', '');
	     $('#chkActive').prop('checked', false);
	     $('#chkVisible').prop('checked', false);
	     $('#parentId').val(parentId || '<%= Model.LevelTree.SupportLevelID %>');
	     $('#sortIndex').val('0');
	 }



	 function createTree() {

	     if ($('#levelTree ul').length) {
	         $('#levelTree').addClass('ltr').tree({
	             rules: {
	                 multitree: true,
	                 draggable: 'all',
	                 createat: 'bottom',
	                 //Entity framework can only handle 8 levels
	                 max_depth: 7
	             },
	             //					ui: {
	             //						dots: false
	             //					},
	             callback: {
	                 onmove: function (node, refNode, type) {
	                     var parent = $(node).parents('li').length > 0 ? $(node).parents('li')[0] : null,
							data = { parentId: parent ? parent.id.replace(/\D/g, '') : '<%= Model.LevelTree.SupportLevelID %>' },
							nodes = $(node).parent().children('li');
	                     for (var i = 0; i < nodes.length; i++) {
	                         data['levelIds[' + i + ']'] = nodes[i].id.replace(/\D/g, '');
	                     }
	                     ///  aca se quito 888
	                     $.post('<%= ResolveUrl(Model.MoveURL) %>', data, function (response) {
	                         if (!response.result)
	                             showMessage(response.message, true);
	                     });
	                 },
	                 onselect: function (node, tree) {
	                     var levelId;
	                     if ($(node).parent().hasClass('AddCat')) {
	                         levelId = $(node).parent().parent().attr('id').replace(/\D/g, '');
	                         $('#levelDetalleCenter').messageCenter('clearAllMessages').messageCenter('addMessage', String.format('<%= Html.Term("AddingChildLevel", "You are adding a child to {0}.") %>', $('#Level' + levelId + ' > a.Level').text()));
	                         resetLevelDetails(levelId);
	                     } else {
	                         levelId = $(node).parent().attr('id').replace(/\D/g, '');
	                         $('#levelId').val(levelId);
	                         getLevel();
	                     }

	                     if ($("#Level" + levelId).hasClass("last") || $("#Level" + levelId).hasClass("leaf")) {
	                         lastLeafId = "#Level" + levelId;
	                     }
	                 }
	             }
	         });

	         if (firstPass) {
	             $('#levelTree').find('ul.ltr').removeClass('ltr').find('li.closed').removeClass('closed').addClass('open'); // expand every folder element in the tree
	             firstPass = false;
	         } else {
	             $('#levelTree').find('ul.ltr').removeClass('ltr');
	             $('#levelTree li').each(function () {
	                 $(this).attr('class', levelStatus[$(this).attr('id')]);
	             });

	             if ($(lastLeafId).children("ul").length > 0) {
	                 $(lastLeafId).removeClass("closed").addClass("open");
	                 $(lastLeafId).removeClass("leaf").addClass("open");
	             }
	             var count = $('#levelTree ul ul li').length;
	             if (count == 1) {
	                 window.location.reload();
	             }
	         }
	     }
	 }

	 function getLevel() {
	     //	            alert($('#levelId').val());
	     if ($('#levelId').val()) {

	         var data = { levelId: $('#levelId').val() };
	         var parent = $('#Level' + data.levelId).closest('li:not(#Level' + data.levelId + ')'); //.parents('li:first');

	         $.getJSON('<%= ResolveUrl(Model.GetURL) %>', data, function (response) {
	             if (response.result) {
	                 $('#levelDetalleCenter').messageCenter('clearAllMessages').messageCenter('addMessage', 'You are editing ' + response.name);
	                 //	                        $('#btnDelete').show();
	                 // populate the controls
	                 $('#levelId').val(response.levelId);
	                 $('#txtName').val(response.name);
	                 $('#txtDescription').val(response.description);
	                 $('#chkActive').prop('checked', response.active);
	                 $('#chkVisible').prop('checked', response.visible);
	                 $('#parentId').val(response.parentSupportLevelID);

	                 $('#sortIndex').val(response.sortIndex);
	             } else
	                 showMessage(response.message, true);
	         });
	     }
	 }


	</script>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Support") %>">
		<%= Html.Term("Support")%></a> >
	    <%= Html.Term("TreeManagement", "Tree Management")%>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
 <div class="SectionHeader">
		<h2>
			<%= Html.Term("SupportLevels", "Support Levels")%>
        </h2>
       
	</div>
	<table width="100%" class="SectionTable">
		<tr>
			<td width="40%">
				<h3>
					<a href="javascript:void(0);" id="rootLevelLink">
						<%= Html.Term("Root", "Root")%></a>
				</h3>
				<div class="UI-lightBg brdrAll pad10 mb10">
					<span><a id="addRoot" href="javascript:void(0);">
						<%= Html.Term("AddaRootLevel", "Add a Root Level")%></a><span class="pipe" style="float: none;">&nbsp;</span>

                         <a id="btnExpandAll" href="javascript:void(0);" onclick="$('#levelTree').find('li.closed').removeClass('closed').addClass('open');">
							<%= Html.Term("ExpandAll", "Expand All")%></a>
                            
                            <span class="pipe" style="float: none;">&nbsp;</span>

                         <a id="btnCollapseAll" href="javascript:void(0);" onclick="$('#levelTree').find('li.open').removeClass('open').addClass('closed');">
								<%= Html.Term("CollapseAll", "Collapse All")%></a>

						<%--| <a href="javascript:void(0);">Edit Tree Name</a>--%></span>
				</div>
				<div id="levelTree" class="LevelTree">
					<%= Model.levels %> <%--es un string ejempl <lu li--%>
				</div>
			</td>
			<td width="60%">
				<h3>
					<%= Html.Term("LevelDetails", "Level Details")%>
					<span><a id="btnDeleteLevel" href="javascript:void(0);">
						<%= Html.Term("RemoveLevel", "Remove Level")%></a></span></h3>
				<table id="levelDetails" class="DataGrid" width="100%">
					<tr>
						<td colspan="2" style="padding: 0;">
							<div id="levelDetalleCenter" class="UI-lightBg brdrAll pad5 mb10">
							</div>
						</td>
					</tr>			
					<tr>
						<td class="FLabel">
							<%= Html.Term("Name", "Name")%>:
						</td>
						<td>
							<input id="txtName" type="text" class="required fullWidth pad5" name="Name is required." maxlength="100" />
						</td>
					</tr>

                    
                    <tr>
						<td class="FLabel">
							Description:
						</td>
						<td>
							<textarea id="txtDescription" style="width: 500px; height: 50px;"></textarea>
                            <input type="hidden" id="rootlevelId" value="<%=ViewData["RootCategoryId"] %>" />
						    <input type="hidden" id="levelId"  />
                            <input type="hidden" id="parentId" value="0" />
                            <input type="hidden" id="sortIndex" value="0" />
						</td>
					</tr>

					<tr>
						<td class="FLabel">
							Visible in WorStation:
						</td>
						<td>
							<input id="chkVisible" type="checkbox"/>
						</td>
					</tr>
					<tr>
						<td class="FLabel">
							Active:
						</td>
						<td>
							<input id="chkActive" type="checkbox"  />
						</td>
					</tr>

					<tr>
						<td>
						</td>
						<td>
							<br />
							<p>
								<a id="btnSave" href="javascript:void(0);" class="Button BigBlue">
									<%= Html.Term("Save Level", "Save Level")%></a>
							</p>
						</td>
					</tr>
				</table>
			</td>
		</tr>
	</table>
</asp:Content>
