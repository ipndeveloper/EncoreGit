<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Support/Views/Shared/Support.Master"
    Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Business.SupportMotiveSearchData>" %>

<asp:Content ID="Content6" ContentPlaceHolderID="YellowWidget" runat="server">
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/json2.js") %>"></script>
   
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
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <%IEnumerable<Category> categoryTrees = ViewData["Categories"] as IEnumerable<Category>; %>
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">
        var estaGuardando=false;// mediante esta variable se valida que no se registre multiples veces
	    var currentPage = 0;
        var SupportMotiveSelected =<% =ViewBag.lstSupporMotiveLevelIds%>;
	    

        function SeleccionarDataType()
        {
          var posicion=document.getElementById('sPropertyItemDataType').options.selectedIndex; //posicion
          var Datat =document.getElementById('sPropertyItemDataType').options[posicion].text;
      //    alert(document.getElementById('sPropertyItemDataType').options[posicion].text); //valor
          
            if (Datat=='Busqueda')
          
                document.getElementById('sPropertyItemDataTypeDinamic').setAttribute('style','visibility:visible');
            else
                document.getElementById('sPropertyItemDataTypeDinamic').setAttribute('style','visibility:hidden');

          
        }

         function CloseModal (hash) {

                ObtenerSupportMotiveConcatenados();



                hash.w.hide() && hash.o && hash.o.remove();
                          
        };

      
        
        $(function () {
	        $('#levelsSelector').jqm({ modal: false, trigger: '#btnOpenLevelsSelector', overlay: 0,onHide:CloseModal });



	        if ('<%= Model.SupportMotiveID %>' > 0) {
	            enableProductPanel();
	            getPropertyItems();
	            getTaskItems();
	        } else {
	            disableProductPanel();
	        }


	        $('#txtMotiveSLA').numeric();

	        $('#btnSave').click(function () {
	            var NameIsValid = true;
	            var MotiveSLAIsValid = true;
	            var SupportLevelIDsIsvalid = true;

	            $('#txtName').clearError();
	            if ($('#txtName').val() == '') {
	                $('#txtName').showError("");
	                $('#txtName').showError('<%= Html.JavascriptTerm("NameIsRequired", "Name is required") %>');

	                NameIsValid = false;
	            } else {
	                NameIsValid = true;
	                $('#txtName').clearError();
	            }
	            $('#txtMotiveSLA').clearError();
	            if ($('#txtMotiveSLA').val() == '') {
	                $('#txtMotiveSLA').showError("");
	                $('#txtMotiveSLA').showError('<%= Html.JavascriptTerm("MotiveSLAIsRequired", "Motive SLA is required") %>');
	                MotiveSLAIsValid = false;
	            } else {
	                MotiveSLAIsValid = true;
	                $('#txtMotiveSLA').clearError();
	            }

	           if ($('input[name="storeFronts02"]:checked').length == 0) {
	                $('#btnOpenLevelsSelector').showError('<%= Html.JavascriptTerm("LeveRequired", "Level is required") %>');
	               
                    SupportLevelIDsIsvalid = false;
	            } else {
                    $('#btnOpenLevelsSelector').clearError();
                    SupportLevelIDsIsvalid = true;
                } 

                if (!(NameIsValid && MotiveSLAIsValid && SupportLevelIDsIsvalid))
                {
                return false ;
                }


	            if ($('#catalogForm').checkRequiredFields()) {
	                var data = {
	                    supportMotiveId: $('#supportMotiveID').val(),
	                    name: $('#txtName').val(),
	                    description: $('#txtDescription').val(),
	                    motiveSLA: $('#txtMotiveSLA').val(),
	                    active: $('#chkActive').prop('checked'),
	                    isVisibleToWorkStation: $('#chkIsVisibleToWorkStation').prop('checked'),
	                    hasConfirmation: $('#chkHasConfirmation').prop('checked'),
                        Edit:<%= Model.Edit?1:0 %> 
	                };

                    /*
	                $('input[name="storeFronts"]:checked').each(function (i) {
	                    data['MarketIDs[' + i + ']'] = $(this).val();
	                });
                    */

	                $('input[name="storeFronts02"]:checked').each(function (i) {
	                    data['SupportLevelIDs[' + i + ']'] = $(this).val();
	                });


                   

	              
               
                   if(estaGuardando)
                   {
                        return false;
                   }
                   estaGuardando=true;

	                $.post('<%= ResolveUrl("~/Support/Motive/Save") %>', data, function (response) {
	                   var _response=response;
                        if (_response.result) {
	                      //  if (!$('#supportMotiveID').val()) {
	                            $('#supportMotiveID').val(_response.supportMotiveID);
	                           // enableProductPanel();
                                showMessage('-Motive saved.');
                                window.location = '<%= ResolveUrl("~/Support/Motive/Edit/") %>' + _response.supportMotiveID;
	                        //}
                              estaGuardando=false;
                             
	                     
	                    } else {
	                        showMessage(_response.message, true);
                            estaGuardando=false;
	                    }


	                });
	            }
	        });


	        //Developed by BAL - CSTI - AFIN

	        function showProductContainer() {
	            $('#propertyNoAdd').hide();
	            $('#propertyAdd').show();

	            $('#taskNoAdd').hide();
	            $('#taskAdd').show();
	        }

	        function hideProductContainer() {
	            $('#propertyNoAdd').show();
	            $('#propertyAdd').hide();

	            $('#taskNoAdd').show();
	            $('#taskAdd').hide();
	        }

	        function enableProductPanel() {
	            showProductContainer();
	        }

	        function disableProductPanel() {
	            hideProductContainer();
	        }


	        $('#btnApplySchedule').click(function () {
              
              if($(this).attr("Edit")=="False")
                 {
                     return false;
                 }
	            var IsValidPropertyItemName = false;
	            var isValidPropertyItemDataType = false;

	            var txtPropertyItemName = $("#txtPropertyItemName");
	            var sPropertyItemDataType = $("#sPropertyItemDataType");

	            if ($.trim(txtPropertyItemName.val()) == '') {
	                txtPropertyItemName.showError('<%= Html.JavascriptTerm("PropertyItemNameRequired", "Property Item Name is required") %>');
	                IsValidPropertyItemName = false;
	            } else {
	                txtPropertyItemName.clearError();
	                IsValidPropertyItemName = true;
	            }

	            if (sPropertyItemDataType.val() == "0") {
	                sPropertyItemDataType.showError('<%= Html.JavascriptTerm("PropertyItemDataTypeRequerido", "Property Item Data Type is required") %>');
	                isValidPropertyItemDataType = false;
	            }
	            else {
	                isValidPropertyItemDataType = true;
	                sPropertyItemDataType.clearError();
	            }

                $('.errorMessageBubble').click(function(){
                    $(this).hide();
                });

	            if (!(isValidPropertyItemDataType && IsValidPropertyItemName)) {
	                return false;
	            }

                 var data = {
	                supportMotiveID: $('#supportMotiveID').val(),
	                name: $('#txtPropertyItemName').val(),
	                dataType: $('#sPropertyItemDataType').val(),
	                required: $('#chkPropertyItemRequired').prop('checked'),
	                editable: $('#chkPropertyItemsortIndex').prop('checked'),
	                visible: $('#chkPropertyItemVisible').prop('checked'),
                    SupportMotivePropertyTypeID:0,
                    FieldSolution: $('#chkFieldSolution').prop('checked'),
                     SupportMotivePropertyDinamicID: $('#sPropertyItemDataTypeDinamic').val()
	            };

	             GuardarPropiedad(data);
	        });

            function GuardarPropiedad(data)
            {
           
	            //            , selected = $('#catalogItems > tbody input[type="checkbox"]:checked');
	            //	        selected.each(function (i) {
	            //	            data['catalogItems[' + i + ']'] = $(this).val();
	            //	        });
	            $.post('<%= ResolveUrl("~/Support/Motive/AddSupportMotiveProperty") %>', data, function (response) {
	                getPropertyItems();
	            })
            }
	        function getPropertyItems() {
	            //	            var sas = ViewData["PropertiesSelect"].toString();
	            //	            alert(sas);
	            //	            alert('#fdfdfdf');
	            $('#selectAllPropertyItems').attr('checked', false);
	            var t = $('#propertyItems tbody');
	            var t2 = $('#selectItems');
	            t.html('<tr><td colspan="5"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
	            $.get('<%= ResolveUrl("~/Support/Motive/GetPropertyItems") %>', { Disabled: <%= Model.Disabled %>,Enabled:  <%= Model.Edit?1:0 %>, page: currentPage, pageSize: $('#pageSize').val(), supportMotiveID: $('#supportMotiveID').val() }, function (response) {
	                if (response.result === undefined || response.result) {
	                    t.html(response.propertyItems);
	                    t2.html(response.select);
	                    //	                    maxPage = response.resultCount == 0 ? 0 : Math.ceil(response.resultCount / $('#pageSize').val()) - 1;
	                    //	                    $('#btnNextPage,#btnPreviousPage').removeAttr('disabled').css({ color: '', cursor: '' });
	                    //	                    if (currentPage == maxPage)
	                    //	                        $('#btnNextPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                    //	                    if (currentPage == 0)
	                    //	                        $('#btnPreviousPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                } else {
	                    showMessage(response.message, true);
	                }
	            })
            .fail(function () {
                t.html('<tr><td colspan="5"></td></tr>');
                showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
            });

	        }


	        $('#btnRemoveProperty').click(function () {
            
                if($(this).attr("Edit")=="False")
                {
                    return false;
                }

	            var data = {};
	            $('#propertyItems > tbody input[type="checkbox"]:checked').each(function (i) {
	                data['PropertyIDs[' + i + ']'] = $(this).val();
	            });

	            $.post('<%= ResolveUrl("~/Support/Motive/RemovePropertyItems") %>', data, function (repsonse) {
	                getPropertyItems();
	            });
	        });

	        $('#btnRemoveAction').click(function () {
            
               if($(this).attr("Edit")=="False")
                 {
                     return false;
                 }

	            var data = {};
	            $('#taskItems > tbody input[type="checkbox"]:checked').each(function (i) {
	                data['MarketIDs[' + i + ']'] = $(this).val();
	            });

	            $.post('<%= ResolveUrl("~/Support/Motive/RemoveTaskItems") %>', data, function (repsonse) {
	                getTaskItems();
	            });
	        });
	        ///////// AddAction =TASK  //////////
	        $('#btnAddAction').click(function () {

                 if($(this).attr("Edit")=="False")
                 {
                     return false;
                 }

	            var IsValidTaskItemName = true;
	            var IsValidTaskItemURL = true;


	            if ($.trim($('#txtTaskItemName').val()) == '') {
	                $('#txtTaskItemName').showError('<%= Html.JavascriptTerm("TaskItemNameRequired", "Task Item is required") %>');
	                IsValidTaskItemName = false;
	            } else {
	                $('#txtTaskItemName').clearError();
	                IsValidTaskItemName = true;
	            }

	            if ($.trim($('#txtTaskItemURL').val()) == '') {
	                $('#txtTaskItemURL').showError('<%= Html.JavascriptTerm("TaskItemUrlRequired", "Task Item  URl is required") %>');
	                IsValidTaskItemURL = false;
	            } else {
	                $('#txtTaskItemURL').clearError();
	                IsValidTaskItemURL = true;
	            }

                $('.errorMessageBubble').click(function(){
                    $(this).hide();
                });

	            if (!(IsValidTaskItemName && IsValidTaskItemURL)) {
	                return false;
	            }

	            var data = {
	                supportMotiveId: $('#supportMotiveID').val(),
	                name: $('#txtTaskItemName').val(),
	                url: $('#txtTaskItemURL').val(),
	                propertyTypeID: $('#sTaskItemProperty').val(),
                    SupportMotiveTaskID:0
	            };
	            //            , selected = $('#catalogItems > tbody input[type="checkbox"]:checked');
	            //	        selected.each(function (i) {
	            //	            data['catalogItems[' + i + ']'] = $(this).val();
	            //	        });
	            $.post('<%= ResolveUrl("~/Support/Motive/AddSupportMotiveTask") %>', data, function (response) {
	                getTaskItems();
	            });

	        });
	        function getTaskItems() {
	            $('#selectAllTaskItems').attr('checked', false);
	            var t = $('#taskItems tbody');
	            t.html('<tr><td colspan="5"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
	            $.get('<%= ResolveUrl("~/Support/Motive/GetTaskItems") %>', { Enabled: <%= Model.Edit?1:0 %> ,page: currentPage, pageSize: $('#pageSize').val(), supportMotiveID: $('#supportMotiveID').val() }, function (response) {
	                if (response.result === undefined || response.result) {
	                    t.html(response.taskItems);
	                    //	                    maxPage = response.resultCount == 0 ? 0 : Math.ceil(response.resultCount / $('#pageSize').val()) - 1;
	                    //	                    $('#btnNextPage,#btnPreviousPage').removeAttr('disabled').css({ color: '', cursor: '' });
	                    //	                    if (currentPage == maxPage)
	                    //	                        $('#btnNextPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                    //	                    if (currentPage == 0)
	                    //	                        $('#btnPreviousPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                } else {
	                    showMessage(response.message, true);
	                }
	            })
            .fail(function () {
                t.html('<tr><td colspan="5"></td></tr>');
                showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
            });
	        }



	        $('.btnEditCampaignAction').live('click', function () {

	            var obj = $(this).closest('tr'), supportMotivePropertyTypeID = obj.attr('data-id');


	            $.get('<%= ResolveUrl("~/Support/Motive/EditPropertyValuesModal")%>', {
	                SupportMotivePropertyTypeID: supportMotivePropertyTypeID
	            },
				function (result) {

				    $('#newsletterModalSub').html(result);
				    $('#newsletterModal').jqmShow();

				})
	        });

	        $('#newsletterModal').jqm({
	            modal: false
	        });



	        // end function general¿
	    });

	      
	    
	  
    </script>
    <script>
        $(document).ready(function () {
            for (var index = 0; index < <% =ViewBag.lstSupporMotiveLevelIds%>.length; index++) {
                $("#categoryContainer0 input:radio[value=" + <% =ViewBag.lstSupporMotiveLevelIds%>[index] + "]").attr("checked", true)
            }
        });


        function getLevel(levelId) {
            var data = { levelId: levelId };
            $.getJSON('<%= ResolveUrl("~/Support/Motive/Get") %>', data, function (response) {
                if (response.NivelesConcatenados) {
                    alert(response.NivelesConcatenados)
                } else
                    showMessage(response.message, true);
            });

        }

        function obtenerSeleccionado() {
            var seleccionados = [];

            $('input[name="storeFronts02"]:checked').each(function () {
               seleccionados.push($(this).val())

            });
            return seleccionados;
        }

      

        function ObtenerSupportMotiveConcatenados(Lista) {
            var url = '<%= ResolveUrl("~/Support/Motive/ListarSupporLevelConcatenados") %>';

            var listaSeleccionadosSupportLevel =Lista|| obtenerSeleccionado();
            var data = JSON.stringify({
                lstSupportLevelSeleccionados: listaSeleccionadosSupportLevel 
                
            });

            $.ajax({
                data: data,
                url: url,
                dataType: "json",
                type: "POST",
                contentType: "application/json",
                success: function (response) {
                    $("#SupportSelected").html("");
                    var view = '<%= Html.JavascriptTerm("ViewSupportMotive", "View") %>';
                    var deleteSupportLevel = '<%= Html.JavascriptTerm("DeleteSupportMotive", "Delete") %>';
                    if (response.Keys) {

                        var Values = response.Values;
                        var Keys = response.Keys;
                        $("#totalSupportLeveSelected").text(Keys.length);


                        for (var index = 0; index < Keys.length; index++) {
                        <%
                         if (Model.Edit) {  %>
                            $("#SupportSelected").append("<li> <span title=" + deleteSupportLevel + " style='cursor:pointer; color:Red' onclick='Eliminar(this," + Keys[index] + ")'>(X)</span>" + Values[index] + "<span title=" + view + " style='cursor:pointer; color:blue' onclick='Ver(" + Keys[index] + ")'>(" + view + ")</span></li>");
                       <%}else { %>
                            $("#SupportSelected").append("<li> " + Values[index] + "<span title=" + view + " style='cursor:pointer; color:blue' onclick='Ver(" + Keys[index] + ")'>(" + view + ")</span></li>");
                       <%} %>

                        }
                    }

                    if ($('input[name="storeFronts02"]:checked').length == 0) {
                        $('#btnOpenLevelsSelector').showError('<%= Html.JavascriptTerm("LeveRequired", "Level is required") %>');
                    } else {
                        $('#btnOpenLevelsSelector').clearError();
                    }

                },
                error: function (error) {

                }
            })

        }
        function Eliminar(ctr,SupportLevelID) {
            $("#levelTreeSelector input:radio[value=" + SupportLevelID + "]").attr("checked", false);
            $(ctr).parent("li").remove();

            $("#totalSupportLeveSelected").text($("#SupportSelected li").length);
        } 

        function Ver(SupportLevelID) {
            $("#btnOpenLevelsSelector").click();
            $("#levelTreeSelector input:radio[value=" + SupportLevelID + "]").focus();

            $("#levelTreeSelector input:radio[value=" + SupportLevelID + "]").attr("checked", true);

            $("#levelTreeSelector input:radio[value=" + SupportLevelID + "]").next("span").css("background-color", "yellow");
              setTimeout(function () {
                  $("#levelTreeSelector input:radio[value=" + SupportLevelID + "]").next("span").css("background-color", "");
              }, 3000);

        }

        function Cancel(ctr)
        {
            dataEdicionActual=null;
            ObtenerPropertyItems();
        }
        
        var dataEdicionActual =null;
        function Edit (ctr)
        {
          var imageButton = ctr.src;
           ctr.src="/content/Images/accept-trans.png";
          
           if(imageButton.indexOf("pencil-12-trans.png")!=-1)
           {
                 $(ctr).removeClass("Read");
                 $(ctr).addClass("Edicion");
                 $("#propertyItems tr img.Read").hide();
                 dataEdicionActual=  Editar(ctr);
           }else{
                ConfirmarEdicion(dataEdicionActual);
           }
          var imgCancel=$(ctr).next("img");
              $(imgCancel).show();
           
          

        }
        function ConfirmarEdicion(Pdata)
        {
            var IsValidPropertyItemName = false;
            var isValidPropertyItemDataType = false;
              var data = 
            {
                supportMotiveID: $('#supportMotiveID').val(),
                name: Pdata.txtPropertyItemName.val(),
                dataType: Pdata.sPropertyItemDataType.val(),
                required: Pdata.chkPropertyItemRequired.prop('checked'),
                editable: Pdata.chkPropertyItemsortIndex.prop('checked'),
                visible:Pdata.chkPropertyItemVisible.prop('checked'),
                SupportMotivePropertyTypeID:Pdata.SupportMotivePropertyTypeID,
                FieldSolution: Pdata.chkFieldSolution.prop('checked'),
                SupportMotivePropertyDinamicID: $('#sPropertyItemDataTypeDinamic').val()
            };

              if ($.trim(Pdata.txtPropertyItemName.val()) == '') {
	                $(Pdata.txtPropertyItemName).showError('<%= Html.JavascriptTerm("PropertyItemNameRequired", "Property Item Name is required") %>');
	                IsValidPropertyItemName = false;
	            } else {
	                $(Pdata.txtPropertyItemName).clearError();
	                IsValidPropertyItemName = true;
	            }

	            if (Pdata.sPropertyItemDataType.val() == "0") {
	                $(Pdata.sPropertyItemDataType).showError('<%= Html.JavascriptTerm("PropertyItemDataTypeRequerido", "Property Item Data Type is required") %>');
	                isValidPropertyItemDataType = false;
	            }
	            else {
	                isValidPropertyItemDataType = true;
	               $(Pdata.sPropertyItemDataType).clearError();
	            }

	            if (!(isValidPropertyItemDataType && IsValidPropertyItemName)) {
	                return false;
	            }


                $.post('<%= ResolveUrl("~/Support/Motive/AddSupportMotiveProperty") %>', data, function (response) {
	                ObtenerPropertyItems();
	            })

        }
        function ObtenerPropertyItems() {
	            //	            var sas = ViewData["PropertiesSelect"].toString();
	            //	            alert(sas);
	            //	            alert('#fdfdfdf');
	            $('#selectAllPropertyItems').attr('checked', false);
	            var t = $('#propertyItems tbody');
	            var t2 = $('#selectItems');
	            t.html('<tr><td colspan="5"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
	            $.get('<%= ResolveUrl("~/Support/Motive/GetPropertyItems") %>', { Disabled: <%= Model.Disabled %>,Enabled:  <%= Model.Edit?1:0 %>, page: currentPage, pageSize: $('#pageSize').val(), supportMotiveID: $('#supportMotiveID').val() }, function (response) {
	                if (response.result === undefined || response.result) {
	                    t.html(response.propertyItems);
	                    t2.html(response.select);
	                    //	                    maxPage = response.resultCount == 0 ? 0 : Math.ceil(response.resultCount / $('#pageSize').val()) - 1;
	                    //	                    $('#btnNextPage,#btnPreviousPage').removeAttr('disabled').css({ color: '', cursor: '' });
	                    //	                    if (currentPage == maxPage)
	                    //	                        $('#btnNextPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                    //	                    if (currentPage == 0)
	                    //	                        $('#btnPreviousPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                } else {
	                    showMessage(response.message, true);
	                }
	            })
            .fail(function () {
                t.html('<tr><td colspan="5"></td></tr>');
                showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
            });

	        }


        function Editar(ctr)
        {
          var index=ctr.getAttribute("index");
        

          var fila =$("#propertyItems tbody tr:eq("+index+")");
          var SupportMotivePropertyTypeID=fila.attr("data-id");
          var sPropertyItemDataType=$("#sPropertyItemDataType").clone(true);
          
          var txtPropertyItemName=$("<input style='width :100%' />")
          var PropertyItemName=$("#propertyItems tbody tr:eq("+index+") td:eq(1) span").text();
          var tdPropertyItem=$("#propertyItems tbody tr:eq("+index+") td:eq(1)");
              txtPropertyItemName.val(PropertyItemName);
              tdPropertyItem.append(txtPropertyItemName);
          
          var dataType=$("#propertyItems tbody tr:eq("+index+") td:eq(2) span").text();
              seleccionarComboPorTexto(sPropertyItemDataType,dataType);
          var tddataType=$("#propertyItems tbody tr:eq("+index+") td:eq(2)");
              tddataType.append(sPropertyItemDataType);

          var chkPropertyItemRequired=$("<input type='checkbox'>");
          var PropertyItemRequired =$("#propertyItems tbody tr:eq("+index+") td:eq(3) span").text();
          var tdPropertyItemRequired=$("#propertyItems tbody tr:eq("+index+") td:eq(3)");
              MarcarCheckbox(chkPropertyItemRequired,PropertyItemRequired);
              tdPropertyItemRequired.append(chkPropertyItemRequired);
              

         var chkPropertyItemsortIndex=$("<input type='checkbox'>");
         var PropertyItemsortIndex =$("#propertyItems tbody tr:eq("+index+") td:eq(4) span").text();
         var tdPropertyItemsortIndex=$("#propertyItems tbody tr:eq("+index+") td:eq(4)");
             MarcarCheckbox(chkPropertyItemsortIndex,PropertyItemRequired);
             tdPropertyItemsortIndex.append(chkPropertyItemsortIndex);
              
              



          var chkPropertyItemVisible=$("<input type='checkbox'>");
          var PropertyItemVisible =$("#propertyItems tbody tr:eq("+index+") td:eq(6) span").text();
          var tdPropertyItemVisible=$("#propertyItems tbody tr:eq("+index+") td:eq(6)");
              MarcarCheckbox(chkPropertyItemVisible,PropertyItemVisible);
              tdPropertyItemVisible.append(chkPropertyItemVisible);

               
          var chkFieldSolution=$("<input type='checkbox'>");
              FieldSolution=$("#propertyItems tbody tr:eq("+index+") td:eq(7) span").text();
          var tdFieldSolution=$("#propertyItems tbody tr:eq("+index+") td:eq(7)");
              MarcarCheckbox(chkFieldSolution,FieldSolution);
              tdFieldSolution.append(chkFieldSolution);

              fila.find("span").css("display","none");

              
      var Controles=
            {
                txtPropertyItemName:txtPropertyItemName,
                sPropertyItemDataType:sPropertyItemDataType,
                chkPropertyItemRequired:chkPropertyItemRequired,
                chkPropertyItemVisible:chkPropertyItemVisible,
                chkFieldSolution:chkFieldSolution,
                SupportMotivePropertyTypeID:SupportMotivePropertyTypeID,
                chkPropertyItemsortIndex:chkPropertyItemsortIndex
            }
            return Controles;
        }
        function seleccionarComboPorTexto(combo,texto)
        {

        texto=((texto=="MultiLine")?"Multi Line":texto);

        $(combo).find("option")
             .filter(
                     function(index){
                                 return ($(this).text()==texto ); 
                          }
                  )
             .attr("selected",true);
        }
        function  MarcarCheckbox( control, texto )
        {
            $(control).attr("checked",texto=="Yes");
        }

/********************************************************************/
 var dataEdicionActualTask=null;
 function CancelTask(ctr)
        {
            dataEdicionActualTask=null;
            ObtenerTaskItems();
        }
        
function EditTask (ctr)
        {
          var imageButton = ctr.src;
           ctr.src="/content/Images/accept-trans.png";
          
           if(imageButton.indexOf("pencil-12-trans.png")!=-1)
           {
                 $(ctr).removeClass("Read");
                 $(ctr).addClass("Edicion");
                 $("#taskItems tr img.Read").hide();
                 dataEdicionActualTask=  EditarTask(ctr);
           }else{
                ConfirmarEdicionTask(dataEdicionActualTask);
           }
          var imgCancel=$(ctr).next("img");
              $(imgCancel).show();
           
        }

           function ConfirmarEdicionTask(Pdata)
        {

                var IsValidTaskItemName = true;
	            var IsValidTaskItemURL = true;


	            if ($.trim(Pdata.txtTaskItemName.val()) == '') {
	                $(Pdata.txtTaskItemName).showError('<%= Html.JavascriptTerm("TaskItemNameRequired", "Task Item is required") %>');
	                IsValidTaskItemName = false;
	            } else {
	                 $(Pdata.txtTaskItemName).clearError();
	                IsValidTaskItemName = true;
	            }

	            if ($.trim(Pdata.txtTaskItemURL.val()) == '') {
	                $(Pdata.txtTaskItemURL).showError('<%= Html.JavascriptTerm("TaskItemUrlRequired", "Task Item  URl is required") %>');
	                IsValidTaskItemURL = false;
	            } else {
	                $(Pdata.txtTaskItemURL).clearError();
	                IsValidTaskItemURL = true;
	            }

	            if (!(IsValidTaskItemName && IsValidTaskItemURL)) {
	                return false;
	            }



              var data = 
            {
                supportMotiveID: $('#supportMotiveID').val(),
                name: Pdata.txtTaskItemName.val(),
                url: Pdata.txtTaskItemURL.val(),
                propertyTypeID: Pdata.sTaskItemProperty.val(),
                SupportMotiveTaskID:Pdata.SupportMotiveTaskID
            };

                $.post('<%= ResolveUrl("~/Support/Motive/AddSupportMotiveTask") %>', data, function (response) {
	                ObtenerTaskItems();
	            })

        }


function EditarTask(ctr)
        {
          var index=ctr.getAttribute("index");
        
       
          var fila =$("#taskItems tbody tr:eq("+index+")");
          var SupportMotiveTaskID=fila.attr("data-id");
          var sTaskItemProperty=$("#sTaskItemProperty").clone(true);
          
          var txtTaskItemName=$("<input style='width :100%' />")
          var TaskItemName=$("#taskItems tbody tr:eq("+index+") td:eq(1) span").text();
          var tdTaskItemName=$("#taskItems tbody tr:eq("+index+") td:eq(1)");
              txtTaskItemName.val(TaskItemName);
              tdTaskItemName.append(txtTaskItemName);

                
          var txtTaskItemURL=$("<input style='width :100%' />")
          var TaskItemURL=$("#taskItems tbody tr:eq("+index+") td:eq(2) span").text();
          var tdTaskItemURL=$("#taskItems tbody tr:eq("+index+") td:eq(2)");
              txtTaskItemURL.val(TaskItemURL);
              tdTaskItemURL.append(txtTaskItemURL);

          
          var  TaskItemProperty=$("#taskItems tbody tr:eq("+index+") td:eq(3) span").text();
              seleccionarComboPorTexto(sTaskItemProperty,TaskItemProperty);
          var tdTaskItemProperty=$("#taskItems tbody tr:eq("+index+") td:eq(3)");
              tdTaskItemProperty.append(sTaskItemProperty);

         

              fila.find("span").css("display","none");

              
      var Controles=
            {
                txtTaskItemName:txtTaskItemName,
                txtTaskItemURL:txtTaskItemURL,
                sTaskItemProperty:sTaskItemProperty,
                SupportMotiveTaskID:SupportMotiveTaskID 
            }
            return Controles;
        }

         function ObtenerTaskItems() {
	            $('#selectAllTaskItems').attr('checked', false);
	            var t = $('#taskItems tbody');
	            t.html('<tr><td colspan="5"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
	            $.get('<%= ResolveUrl("~/Support/Motive/GetTaskItems") %>', { Enabled: <%= Model.Edit?1:0 %> ,page: currentPage, pageSize: $('#pageSize').val(), supportMotiveID: $('#supportMotiveID').val() }, function (response) {
	                if (response.result === undefined || response.result) {
	                    t.html(response.taskItems);
	                    //	                    maxPage = response.resultCount == 0 ? 0 : Math.ceil(response.resultCount / $('#pageSize').val()) - 1;
	                    //	                    $('#btnNextPage,#btnPreviousPage').removeAttr('disabled').css({ color: '', cursor: '' });
	                    //	                    if (currentPage == maxPage)
	                    //	                        $('#btnNextPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                    //	                    if (currentPage == 0)
	                    //	                        $('#btnPreviousPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' }); ;
	                } else {
	                    showMessage(response.message, true);
	                }
	            })
            .fail(function () {
                t.html('<tr><td colspan="5"></td></tr>');
                showMessage('@Html.Term("ErrorProcessingRequest", "There was a fatal error while processing your request.  If this persists, please contact support.")', true);
            });
	        }


        /*
        <tr>
            <td class="FLabel">
                <%= Html.Term("Market", "Market")%>:
            </td>
            <td>
                <% 
                    foreach (var storeFront in Model.listaMarket)
                    { %>
                <input <%= Model.Disabled %> type="checkbox" name="storeFronts" id="storeFronts<%= storeFront.MarketID %>"
                    value="<%= storeFront.MarketID %>" <%= storeFront.Relacionado==1? "checked=\"checked\"" : "" %> />
                <label for="storeFronts<%= storeFront.MarketID %>">
                    <%= storeFront.Name %></label><br />
                <%} %>
            </td>
        </tr>
        */
    </script>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Support") %>">
        <%= Html.Term("Support")%></a> > <a href="<%= ResolveUrl("~/Support/Edit") %>">
            <%= Html.Term("NewMotive", "New Motive")%></a>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="MainContent" runat="server">
    <%var levelTrees = ViewData["Levels"] as IEnumerable<SupportLevels>; %>
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("AddNewMotive", "Add a New Motive")%>
        </h2>
    </div>
    <%--    <div class="SectionHeader">
		<h2>
			<%= Html.Term("Support", "Support")%>
        </h2>
        <a href="<%= ResolveUrl("~/Support/Motive") %>"><%= Html.Term("Support", "New Motive")%></a>
        | <a href="<%= ResolveUrl("~/Support/Motive/Edit") %>"><%= Html.Term("CreateaNewMotive", "Create a New Motive")%></a>
	</div>--%>
    <table id="periodForm" class="FormTable" width="100%">
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Name", "Name")%>:
                <input type="hidden" id="supportMotiveID" value="<%= Model.SupportMotiveID == 0 ? "" : Model.SupportMotiveID.ToString() %>" />
            </td>
            <td>
                <input id="txtName" type="text" value="<%= Model.Name %>" class="required" name="<%= Html.Term("NameIsRequired", "Name is required") %>"
                    style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("Description", "Description")%>:
            </td>
            <td>
                <input id="txtDescription" type="text" value="<%= Model.Description %>" style="width: 20.833em;" />
            </td>
        </tr>
        <tr>
            <td class="FLabel" style="width: 18.182em;">
                <%: Html.Term("SLA", "SLA")%>:
            </td>
            <td>
                <input id="txtMotiveSLA" type="number" value="<%= Model.MotiveSLA %>" class="required"
                    name="<%= Html.Term("MotiveSLAIsRequired", "Motive SLA is required") %>" style="width: 20.833em;" /><%: Html.Term("hour", "hour")%>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("Active", "Active") %>:
            </td>
            <td>
                <input id="chkActive" type="checkbox" <%= Model.Active ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <td class="FLabel">
                <%= Html.Term("IsVisibleToWorkStation", "Visible in ToWorkStation")%>:
            </td>
            <td>
                <input id="chkIsVisibleToWorkStation" type="checkbox" <%= Model.IsVisibleToWorkStation ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr style="display:none">
            <td class="FLabel">
                <%= Html.Term("HasConfirmation", "Has Confirmation")%>:
            </td>
            <td>
                <input id="chkHasConfirmation" type="checkbox" <%= Model.HasConfirmation ? "checked=\"checked\"" : "" %> />
            </td>
        </tr>
        <tr>
            <% 
                List<string> values = ViewBag.values as List<string>;
                List<int> Claves = ViewBag.Claves as List<int>;%>
            <td class="FLabel">
                <%= Html.Term("MotiveLevels", "Motive Levels") %>:
            </td>
            <td>
                <div id="levelsSelected">
                </div>
                <a id="btnOpenLevelsSelector" href="javascript:void(0);">
                    <%= Html.Term("OpenLevelSelector", "Open Level Selector") %></a>
                <%= Html.Term("TotalMotiveLevels", "Total")%>: <span id="totalSupportLeveSelected">
                    <%=Claves.Count%></span>
            </td>
        </tr>
        <tr>
            <td>
                <%= Html.Term("SuportLevelSelected", "Suport Level selected ")%>:
            </td>
            <td>
                <div style="width: 400px; height: 25px;">
                    <ol id="SupportSelected">
                        <% for (int index = 0; index < values.Count; index++) %>
                        <%{
                              if (Model.Edit)
                              {  %>
                        <li><span title="<%= Html.Term("DeleteSupportMotive","Delete")%>" style="cursor: pointer;
                            color: Red" onclick="Eliminar(this,<%=Claves[index]%>)">X</span>
                            <%=values[index]%>
                            <span title="<%= Html.Term("ViewSupportMotive","View")%>" style="cursor: pointer;
                                color: blue" onclick="Ver(<%=Claves[index] %>)">(
                                <%= Html.Term("ViewSupportMotive","View")%>) </span></li>
                        <%}
                 else
                 {%>
                        <li>
                            <%=values[index]%>
                            <span title="<%= Html.Term("ViewSupportMotive","View")%>" style="cursor: pointer;
                                color: blue" onclick="Ver(<%=Claves[index] %>)">(
                                <%= Html.Term("ViewSupportMotive","View")%>) </span></li>
                        <%}
                          } %>
                    </ol>
                </div>
            </td>
        </tr>
        <%--  <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("Save Motive", "Save Motive") %></a>
                </p>
            </td>
        </tr>--%>
        <tr id="productPanel">
            <td class="FLabel" style="vertical-align: top;">
                <%= Html.Term("Properties", "Properties")%>:
                <div id="productPanelOverlay" style="background-color: #999999; height: 0px; width: 0px;
                    position: absolute; left: 0px; top: 0px; z-index: 1; opacity: 0.6; filter: alpha(opacity=60);">
                    <%--<span style="opacity: 1; filter: alpha(opacity=1); color: #FF0000; margin: auto;">Please
						save your catalog first</span>--%>
                </div>
            </td>
            <td id="propertyNoAdd" style="display: none;">
                <%= Html.Term("PropertyNotSaved", "The motive must be saved before Property can be added.") %>
            </td>
            <td id="propertyAdd">
                <div>
                    <p class="FL">
                        <%= Html.Term("PropertyItemName", "Name")%>:
                        <input <%= Model.Disabled %> id="txtPropertyItemName" type="text" class="" value="" />
                    </p>
                    <p class="FL">
                        <%= Html.Term("PropertyItemDataType", "Data Type")%>:
                        <select <%= Model.Disabled %> id="sPropertyItemDataType" onchange="SeleccionarDataType()" >
                            <option value="0" selected="selected">Select Data Type</option>
                             <option value="Date">Date</option>
                            <option value="Text">Text</option>
                            <option value="MultiLine">Multi Line</option>
                            <option value="Numeric">Numeric</option>
                            <option value="List">List</option>
                            <option value="Busqueda">Busqueda</option>
                        </select>
                       
                        <%    List<NetSteps.Data.Entities.EntityModels.SupportMotivePropertyDinamic> MotivePropertyDinamic = SupportMotives.GetSupportMotivePropertyDinamic();%>
                        <%  if (MotivePropertyDinamic != null)
                            {                                 
                        %>
                        <select id="sPropertyItemDataTypeDinamic" style="visibility:hidden">
                            <option value="0">
                                <%= Html.Term("SelectedType", "-------Selected Type-------")%></option>
                            <%for (int index = 0; index < MotivePropertyDinamic.Count; index++)
                              {%>
                            <option value="<%=MotivePropertyDinamic[index].SupportMotivePropertyDinamicID%>">
                                <%=MotivePropertyDinamic[index].Name%>
                            </option>
                            <%} %>
                        </select>
                        <%} %>
                    </p>
                    <p class="FL">
                        <%= Html.Term("PropertyItemRequired", "Required")%>:
                        <input <%= Model.Disabled %> id="chkPropertyItemRequired" type="checkbox" />
                    </p>
                    <p class="FL">
                        <%= Html.Term("Editable", "Is Editable")%>:
                        <input <%= Model.Disabled %> id="chkPropertyItemsortIndex" type="checkbox" />
                    </p>
                    <p class="FL">
                        <%= Html.Term("PropertyItemVisibleinWS", "Visible in WS")%>:
                        <input <%= Model.Disabled %> id="chkPropertyItemVisible" type="checkbox" />
                    </p>
                    <p class="FL">
                        <%= Html.Term("IsSolution", "Solution")%>:
                        <input <%= Model.Disabled %> id="chkFieldSolution" type="checkbox" />
                    </p>
                    <p class="FR">
                        <% %>
                        <a edit="<%= Model.Edit %>" style="color: <%=Model.ColorFont%>" id="btnApplySchedule"
                            href="javascript:void(0);" class="">
                            <%= Html.Term("AddSchedule", "Add Property") %></a> | <a edit="<%= Model.Edit %>"
                                style="color: <%=Model.ColorFont%>" id="btnRemoveProperty" href="javascript:void(0);"
                                class="">
                                <%= Html.Term("RemoveFromMotive", "Remove from Motive") %></a>
                    </p>
                </div>
                <span class="ClearAll"></span>
                <!-- Products In Order -->
            
                <table id="propertyItems" width="100%" class="DataGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                                <input <%= Model.Disabled %> id="selectAllPropertyItems" type="checkbox" />
                            </th>
                            <th>
                                <%= Html.Term("Name", "Name") %>
                            </th>
                            <th>
                                <%= Html.Term("DateType", "Date Type")%>
                            </th>
                            <th>
                                <%= Html.Term("Required", "Required")%>
                            </th>
                            <th>
                                <%= Html.Term("Editable", "Editable")%>
                            </th>
                            <th>
                                <%= Html.Term("SortIndex", "Sort Index")%>
                            </th>
                            <th>
                                <%= Html.Term("VisibleinWS", "Visible in WS")%>
                            </th>
                            <th>
                                <%= Html.Term("FieldSolution", "Field Solution")%>
                            </th>
                            <th>
                                <%= Html.Term("Values", "Values")%>
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                <tbody ></tbody>
                    
                </table>
                
                <div class="Pagination" style="visibility: hidden">
                    <a href="javascript:void(0);" id="btnPreviousPage">&lt;&lt;
                        <%= Html.Term("Previous")%></a><a href="javascript:void(0);" id="btnNextPage" style="margin-left: .909em;"><%= Html.Term("Next", "Next") %>
                            &gt;&gt;</a> <span style="margin-left: .909em;">
                                <%= Html.Term("PageSize", "Page Size") %>:<select id="pageSize">
                                    <option selected="selected" value="20">20</option>
                                    <option value="50">50</option>
                                    <option value="100">100</option>
                                </select>
                            </span>
                </div>
            </td>
        </tr>
        <tr id="taskPanel">
            <td class="FLabel" style="vertical-align: top;">
                <%= Html.Term("Actions", "Actions")%>:
                <div id="Div1" style="background-color: #999999; height: 0px; width: 0px; position: absolute;
                    left: 0px; top: 0px; z-index: 1; opacity: 0.6; filter: alpha(opacity=60);">
                    <%--<span style="opacity: 1; filter: alpha(opacity=1); color: #FF0000; margin: auto;">Please
						save your catalog first</span>--%>
                </div>
            </td>
            <td id="taskNoAdd" style="display: none;">
                <%= Html.Term("taskNotSaved", "The motive must be saved before Action can be added.") %>
            </td>
            <td id="taskAdd">
                <div>
                    <p class="FL">
                        <%= Html.Term("TaskItemName", "Name")%>:
                        <input <%= Model.Disabled %> id="txtTaskItemName" type="text" class="" value="" />
                    </p>
                    <p class="FL">
                        <%= Html.Term("TaskItemURL", "URL")%>:
                        <input <%= Model.Disabled %> id="txtTaskItemURL" type="text" class="" value="" />
                    </p>
                    <p class="FL">
                        <%= Html.Term("TaskItemProperty", "Property")%>:
                    </p>
                    <div id="selectItems">
                    </div>
                    <p class="FR">
                        <a edit="<%= Model.Edit %>" id="btnAddAction" style="color: <%=Model.ColorFont%>"
                            href="javascript:void(0);" class="">
                            <%= Html.Term("AddAction", "Add Action") %></a> | <a style="color: <%=Model.ColorFont%>"
                                edit="<%= Model.Edit %>" id="btnRemoveAction" href="javascript:void(0);" class="">
                                <%= Html.Term("RemoveAction", "Remove Action") %></a>
                    </p>
                </div>
                <span class="ClearAll"></span>
                <!-- Products In Order -->
                <table id="taskItems" width="100%" class="DataGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                                <input <%= Model.Disabled %> id="selectAllActionItems" type="checkbox" />
                            </th>
                            <th>
                                <%= Html.Term("Name", "Name") %>
                            </th>
                            <th>
                                <%= Html.Term("URL", "URL")%>
                            </th>
                            <th>
                                <%= Html.Term("Property", "Property")%>
                            </th>
                            <th>
                                <%= Html.Term("SortIndex", "Sort Index")%>
                            </th>
                            <th>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
                <div class="Pagination" style="visibility: hidden">
                    <a href="javascript:void(0);" id="A3">&lt;&lt;
                        <%= Html.Term("Previous")%></a><a href="javascript:void(0);" id="A4" style="margin-left: .909em;"><%= Html.Term("Next", "Next") %>
                            &gt;&gt;</a> <span style="margin-left: .909em;">
                                <%= Html.Term("PageSize", "Page Size") %>:<select id="Select2">
                                    <option selected="selected" value="20">20</option>
                                    <option value="50">50</option>
                                    <option value="100">100</option>
                                </select>
                            </span>
                </div>
            </td>
        </tr>
        <tr>
            <td class="FLabel">
            </td>
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue">
                        <%= Html.Term("Save Motive", "Save Motive") %></a>
                </p>
            </td>
        </tr>
    </table>
    <div id="levelsSelector" class="jqmWindow LModal" style="min-width: 350px;">
        <div class="mContent">
            <h2>
                <%= Html.Term("LevelsSelector", "Levels Selector") %></h2>
            <div id="levelTreeSelector" style="height: 400px; overflow: auto; margin-bottom: 1.818em;">
                <%= ViewData["LevelTree"] %>
            </div>
            <p>
                <a href="javascript:void(0);" class="Button jqmClose">
                    <%= Html.Term("Close", "Close") %></a>
            </p>
        </div>
    </div>
    <div id="newsletterModal" class="jqmWindow LModal">
        <div id="newsletterModalSub">
        </div>
        <p>
            <a href="javascript:void(0);" class="Button jqmClose">
                <%= Html.Term("Close", "Close") %></a>
        </p>
    </div>
</asp:Content>
