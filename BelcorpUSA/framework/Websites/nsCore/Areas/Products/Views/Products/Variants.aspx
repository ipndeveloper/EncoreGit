<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<asp:Content ID="Content3" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/MediaLibrary.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ajaxupload.js") %>"></script>
    <script type="text/javascript">
    var uploads = {};
        $(function () {
            
           
            
            var currentLi;

            $(".variantGroupRadio").change(function() {
                if($(this).val()==0)
                {
                    $('#addToMaster').attr("disabled", "true");
                    $("#newVarGroupNameWrap").hide();
                    $("#masterProductPropertyTypeIdWrap").show();
                    $("#newVarGroupName").val('');
                    $("#addToMasterWrap").hide();
                }
                else
                {
                    $('#addToMaster').removeAttr("disabled");
                    $("#newVarGroupNameWrap").show();
                    $("#masterProductPropertyTypeIdWrap").hide();
                    $("#masterProductPropertyTypeId :selected").removeAttr("selected");
                    $("#addToMaster").removeAttr("checked");
                    $("#addToMasterWrap").show();
                }
            });

            //Create a new variant group
            $('#CreateVariantGroup').click(function () {
                var masterId = $("#masterProductPropertyTypeId").val();
                var groupName = (masterId==0 || masterId == null) ? $("#newVarGroupName").val() : $("#masterProductPropertyTypeId option:selected").text();
                var relValue = groupName.replace(/ /g,"");
                var variantContainer = '<div id="'+relValue+'" class="TabContent" style="display:none;">';
                if(masterId==0 || masterId == null) {
                    if(groupName=="") {
                        alert('<%= Html.Term("PleaseEnterTheGroupName", "Please enter the group name") %>');
                        $("#newVarGroupName").focus();
                        return true;
                    }
                        
                    var isMaster = $('#addToMaster').prop('checked');
                    variantContainer += '<div class="pad10 utilities"><div class="FL mr10">' +
                    '<input type="text" class="pad2 TextInput addVarLines" />' + 
                    '<span class="clr lawyer"><%=Html.Term("example") %>: <i><%=Html.Term("CommaSeperatedColors", "red, blue, green, brown")%></i></span></div>' +
                    '<div class="FL"><a href="javascript:void(0);" class="DTL Add"><span><%=Html.Term("Add") %></span></a></div>' +
                    '<div class="FR"><input type="hidden" class="isMaster" value="'+isMaster+'" /><input type="hidden" class="productPropertyTypeId" value="-1" /><%=Html.Term("Type") %>: <select class="htmlInputTypeId">' +
                        '<option value="0"><%= Html.Term("PleaseSelect", "Please Select") %></option>';
                        <%foreach(HtmlInputType type in SmallCollectionCache.Instance.HtmlInputTypes.Where(hit=>hit.Active)) { %>
                            variantContainer += '<option value="<%=type.HtmlInputTypeID %>"><%=type.GetTerm() %></option>';
                        <%}%>
                    variantContainer += '</select></div><span class="clr"></span></div><ul class="flatList variantAttributes">' +
                    '</ul><div class="brdrNNYY pad10 variantAttributeSaving"><p><input type="checkbox" id=-1" class="showNameAndThumbnail" /> ' +
                    '<label for="-1"><%=Html.Term("DisplayNameAndThumbnail", "Display both name &amp; thumbnail on front-end.")%></label></p><p>' +
                    '<a href="javascript:void(0);" class="Button BigBlue Save">' +
                    '<span><%=Html.Term("SaveGroup", "Save Group")%></span></a>' +
                    '<a href="javascript:void(0);" class="Button BtnDelete" title="Remove this group for just this product">' +
                    '<span><%=Html.Term("Products_RemoveVariantGroup", "Remove Group")%></span></a></p></div></div>';
                    addVariant(variantContainer, groupName);
                    
                } else {
                    
                    var itemExists = false;
                    $("#variantGroupsWrapper").find(".productPropertyTypeId").each(function(i) {
                        if($(this).val()==$("#masterProductPropertyTypeId").val()) {
                            itemExists = true;
                            return;
                        }
                    });

                    if(itemExists) {
                        alert('<%= Html.Term("VariantGroupAlreadyExistsOnProduct", "The variant group already exists on this product.") %>');
                        $("#masterProductPropertyTypeId").focus();
                        return true;
                    }
                    
                    var data = {productPropertyTypeId: masterId};
                    $.post('<%= ResolveUrl("~/Products/Products/RenderVariantGroup/" + Model.ProductID) %>', data, function (response) {
                        if (response.result) {
                            variantContainer += response.variantGroupHTML;
                            addVariant(variantContainer, groupName);
                        } else {
                            showMessage(response.message);
                        }
                    }); 
                }
            
            });

            $('#VariantsTabber li.current div.tabLabel').live('dblclick', function () {
                var labelVal = $(this).text();
                var labelWidth = $(this).parents('li').width();
                var title = '<%=Html.Term("SaveName", "Save name") %>';
                $(this).html('<div class="editGroupName"><input type="text" id="attrLabel-' + labelVal + '" name="" value="' + labelVal + '" class="attrLabel" style="width:' + labelWidth + 'px;"/><a href="javascript:void(0);" class="FL SaveName" title="'+title+'"><img src="/Content/Images/accept-trans.png" width="16" height="16" /></a><span class="clr"></span></div>');
                $(this).toggleClass('editTabLabel');
            });

            $('a.SaveName').live('click', function () {
                acceptNameChange();
            });

            $('.attrLabel').live('keypress', function(e) {
                if(e.which == 13) {
                    acceptNameChange();
                }
            });


            $('a.Add').live('click', function () {
                var t = $(this);
                showLoading(t);
                var tabContent = $(this).closest('.TabContent');
                var currentName = tabContent.attr("id");
                var editGroupNameDiv = $("li[rel="+currentName+"]").find(".editGroupName");
                var tabLabelText = $("li[rel="+currentName+"]").find(".tabLabel").text();
                var tempProductPropertyTypeId = tabContent.find('.productPropertyTypeId').val();
                var utilities = $(this).closest(".utilities");
                var htmlInputTypeId = utilities.find(".htmlInputTypeId").val();
            
                var productPropertyTypeId = utilities.find('.productPropertyTypeId').val();
                if(productPropertyTypeId<0) {
                    var data = {
                        propertyTypeId: null,
                        name: editGroupNameDiv.length!=0 ? editGroupNameDiv.find(".attrLabel").val() : tabLabelText,
                        required: true, 
                        isProductVariantProperty: true,
                        showNameAndThumbnail: tabContent.find('.showNameAndThumbnail').prop('checked'),
                        dataType: "System.String"
                    }
                    if(htmlInputTypeId!=0) {
                        data['htmlInputTypeId'] = htmlInputTypeId;
                    }
                    data['isMaster'] = utilities.find(".isMaster").val();
                    $.post('<%= ResolveUrl("~/Products/Properties/Save") %>', data, function (response) {
                        if (response.result) {
                            productPropertyTypeId = response.propertyTypeId;
                            utilities.find('.productPropertyTypeId').val(productPropertyTypeId);
                            addAttribute(utilities, productPropertyTypeId);
                        } else {
                            showMessage(response.message || '<%= Html.Term("VariantGroupsSavedSuccessfully", "Variant groups saved successfully") %>!', !response.result);
                        }
                        hideLoading(t);
                    });
                } else {
                    addAttribute(utilities, productPropertyTypeId);
                    hideLoading(t);
                }
            });

            $('.variantAttributes .Remove').live('click', function () {
                if (!confirm('<%= Html.Term("AreYouSureYouWishToDeleteThisAttribute", "Are you sure you wish to delete this attribute?") %>')) {
                    return;
                }
                var li = $(this).closest('li');
                var valueId = li.find('.valueId').val();
                if (valueId != null && valueId > 0) {
                    $.post('<%= ResolveUrl("~/Products/Properties/DeleteValue") %>', { valueId: valueId }, function(response) {
                        if(response.result) {
                            li.fadeOut('normal', function () {
                                li.remove();
                            });
                        }
                        else {
                            showMessage(response.message, true)
                        }
                    });
                } else {
                    li.fadeOut('normal', function () {
                        li.remove();
                    });
                }

            });

            $('.BtnDelete').live('click', function () {
                var t = $(this);
                var s = $('.Save');
                showLoading(t, { float: 'right' });
                showLoading(s);
                var div = $(this).closest('.TabContent');
                var currentName = div.attr("id");
                var relValue = currentName.replace(/ /g,"");
                var data = { productBaseId: '<%=Model.ProductBaseID %>', productPropertyTypeId: div.find('.productPropertyTypeId').val() };
                if (confirm('<%= Html.Term("AreYouSureYouWishToDeleteThisGroup", "Are you sure you wish to delete this group?") %>')) {
                    $.post('<%= ResolveUrl(string.Format("~/Products/Products/RemoveProductPropertyType/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
                        if(response.result) {
                            $("li[rel="+relValue+"]").remove();
                            div.remove();
                            showMessage(response.message || '<%= Html.Term("VariantGroupRemoveSuccessfully", "Variant group removed successfully") %>!', !response.result);
					        $("#masterProductPropertyTypes").html(response.masterProductProperTypesHTML);
                            hideLoading(t, { float: 'right' });
                            hideLoading(s);
                        } else {
                            showMessage(response.message, !response.result);
                            hideLoading(t, { float: 'right' });
                            hideLoading(s);
                        }
                    });
                } else {
                     hideLoading(t, { float: 'right' });
                     hideLoading(s);
                }
            });

            $('.Save').live('click', function () {
                var t = $(this);
                var d =  $('.BtnDelete');
                showLoading(t);
                showLoading(d, { float: 'right' });
                var div = $(this).closest('.TabContent');
                var currentName = div.attr("id");
                var editGroupNameDiv = $("li[rel="+currentName+"]").find(".editGroupName");
                var tabLabelText = $("li[rel="+currentName+"]").find(".tabLabel").text();
                var nameWasEdited = editGroupNameDiv.length!=0;
                var tempProductPropertyTypeId = div.find('.productPropertyTypeId').val();
                if(tempProductPropertyTypeId==-1)
                    tempProductPropertyTypeId = null;
                var data = {
                    propertyTypeId: tempProductPropertyTypeId,
                    name: editGroupNameDiv.length!=0 ? editGroupNameDiv.find(".attrLabel").val() : tabLabelText,
                    required: true, 
                    isProductVariantProperty: true,
                    showNameAndThumbnail: div.find('.showNameAndThumbnail').prop('checked'),
                    dataType: "System.String"
                }
                var utilitiesDiv = $(this).closest('div.TabContent').find('.utilities');
                var htmlInputTypeId = utilitiesDiv.find(".htmlInputTypeId").val();
                if(htmlInputTypeId!=0) {
                    data['htmlInputTypeId'] = htmlInputTypeId;
                }
                data['isMaster'] = utilitiesDiv.find(".isMaster").val();
                div.find('.variantAttributes .variantAttributeInput').filter(function () { return !!$(this).val(); }).each(function (i) {
                    data['values[' + i + '].ProductPropertyValueID'] = $(this).attr('name').replace(/\D/g, '');
                    data['values[' + i + '].Value'] = $(this).val();
                    var thumbnail = $(this).closest('li').find('.imageThumbnail')
                    if(thumbnail.length!=0 && thumbnail.attr("src")!=null) {
                        data['values[' + i + '].FilePath'] = thumbnail.attr("src");
                    }
                });

                $.post('<%= ResolveUrl("~/Products/Properties/Save") %>', data, function (response) {

                    if (response.result) {

                        div.find('.productPropertyTypeId').val(response.propertyTypeId);
                        var data2 = { productBaseId: '<%= Model.ProductBaseID%>', productPropertyTypeId: response.propertyTypeId };

					    $.post('<%= ResolveUrl(string.Format("~/Products/Products/AddProductPropertyType/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data2, function (response2) {
                            showMessage(response.message || '<%= Html.Term("VariantGroupsSavedSuccessfully", "Variant groups saved successfully") %>!', !response.result || !response2.result);
					        $("#masterProductPropertyTypes").html(response.masterProductProperTypesHTML);
                            hideLoading(t);
                            hideLoading(d, { float: 'right' });
                        });

                        if(nameWasEdited) {
                            acceptNameChange();
                        }

                    } else {
                        showMessage(response.message || '<%= Html.Term("VariantGroupsSavedSuccessfully", "Variant groups saved successfully") %>!', !response.result);
                        hideLoading(t);
                        hideLoading(d, { float: 'right' });
                    }
                    
                });
            });

            $('.DataGrid tr:even').addClass("Alt");

	
	        // Input watermarking 
		    $('.optional').watermark('Optional');
		    $('.FindUser').watermark('Find an nsCORE User');
		    $(".QuickProductSearch").watermark("Quick Product Search (SKU or Name)");
		    $('.Username').watermark('username');
		    $('.Password').watermark('password');
		    $('.DateFrom').watermark('From');
		    $('.DateTo').watermark('To');
		    $('.SearchNameDesc').watermark('search by name or description');
		    $('input.newVarGroupName').watermark('group name');
		    $('input.variantAttributeInput').watermark('attribute name');
		    $('input.addVarLines').watermark('add more attribute lines (comma separated)');
		
		
	        $('.SubTab').hover(
		        function() { $(this).addClass('Selected'),  $('div.DropDown', this).slideDown('fast'); },
		        function() { $('div.DropDown', this).fadeOut('fast'),  $(this).removeClass('Selected'); }
		    );
	
	        ﻿$('.SearchBox .TextInput, .TabSearch .TextInput, .IntegratedSearch .TextInput').focus(
                function () {$(this).parent().addClass('SearchFocus');
	        });
	
	        $('.SearchBox .TextInput, .TabSearch .TextInput, .IntegratedSearch .TextInput').blur(
                function () {$(this).parent().removeClass('SearchFocus');
	        });
	
	        $('.fade').fadeIn(600).delay(2000).slideUp(1000);
	
	        $('input.comInput').keyup(
		        function() { $(this).addClass('Selected'),  $('a.comLink').css('display', 'block'); }
		    );

	        $('input.bizInput').keyup(
		        function() { $(this).addClass('Selected'),  $('a.bizLink').css('display', 'block'); }
		    );
	
	        $('.ProxyWin').jqm({modal: false, trigger: 'a.LoadProxy'});
	
	        // Tabber
		    $(".Tabber li").each(function () {
			    $(this).click(function(){
				    tabClick($(this));
			    });

	        });

            $(function() {
		        $(".DatePicker, .DatePick").datepicker();
	        });

            $('.btnDeleteImage').live('click', function () {
                var li = $(this).closest("li");
                li.find('.thumbnailPreview').hide();
                li.find('.imageThumbnail').attr("src", '');
                li.find('.btnBrowseServer').show();
            });

            $('.imageThumbnail').click(function () {
                var li = $(this).closest("li");
                var id = li.find('.btnBrowseServer').attr("id").replace("btnBrowseServer","");
                //alert(id);
                var name = 'userfile' + id;
                //alert(name);
                //alert($("[name='"+name+"']").length);
                //$("[name='"+name+"']").click();
            });

            $('#ProductVariants li .RemoveRow .Remove').hover(
                function() {
                    $(this).parent().parent().addClass('indicator');
                },
                function() {
                    $(this).parent().parent().removeClass('indicator');
                }
            );
	
        });

        function tabClick(tab) {
            var defaultState = tab.html();
		    // switch all tabs off
		    $(".Tabber .current").removeClass("current");
				
		    // switch this tab on
		    tab.addClass("current");
				
		    // slide all content up
		    $(".TabContent").hide();
				
		    // slide this content up
		    var content_show = tab.attr("rel");
		    $("#"+content_show).show();
		    return false;
        }

        function addVariant(variantContainer, groupName) {
            $('#variantGroupsWrapper').append(variantContainer);
            var relValue = groupName.replace(/ /g,"");
            var newTab = '<li rel="'+relValue+'"><div class="tabLabel">'+groupName+'</div></li>';
            $('#VariantsTabber').append(newTab);
            var htmlInputTypeId = $("#htmlInputTypeId").val();
            
            $("#newVarGroupName").val('');
            $("#masterProductPropertyTypeId :selected").removeAttr("selected");
            $("#addToMaster").removeAttr("checked");
            $("li[rel="+relValue+"]").click(function(){
			    tabClick($(this));
		    });
            $("li[rel="+relValue+"]").click();
            $('input.addVarLines').watermark('add more attribute lines (comma separated)');
            $("#"+groupName).find(".htmlInputTypeId").val(htmlInputTypeId);
            $("#htmlInputTypeId :selected").removeAttr("selected");
            $('#masterProductPropertyTypeId option[value=' +  $("#"+groupName).find(".productPropertyTypeId").val() + ']').remove();
        }

        function addAttribute(utilities, productPropertyTypeId) {
            var tabContentDiv = utilities.closest('.TabContent');
            var attributeNameBox = utilities.find(".addVarLines");
            var commaStr = attributeNameBox.val();
            var nameArr = commaStr.split(",");
            for(var i=0; i<nameArr.length; i++) {
                var name = nameArr[i];
                //var attributeLine = "<li><p class=\"FL mr10\"><label>Attribute Name:</label><input type=\"text\" class=\"pad2 variantAttributeInput\" value=\""+name+"\" /></p><p class=\"FL mr10\"><label>Thumbnail:</label><a href=\"#\" class=\"Button\"><span>Upload</span></a></p><p class=\"FL\"><a href=\"#\" class=\"Remove\" title=\"Delete attribute\"><span></span></a></p></li>";
                var data = {productPropertyTypeId: productPropertyTypeId, name: name, value: name};
                $.post('<%= ResolveUrl(string.Format("~/Products/Products/RenderVariantGroupAttribute/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', data, function (response) {
                    if (response.result) {
                        tabContentDiv.find('.variantAttributes').append(response.variantGroupAttributeHTML);
                        tabContentDiv.find('.Remove:last').hover(
                            function() {
                                $(this).parent().parent().addClass('indicator');
                            },
                            function() {
                                $(this).parent().parent().removeClass('indicator');
                            }
                        );
                    } else {
                        showMessage(response.message);
                    }
                }); 
            }
            attributeNameBox.val('');
        }

        function createUploadButton(theButton, productPropertyTypeID, productPropertyValueID) {
            new AjaxUpload(theButton, {
                action: '<%= ResolveUrl("~/Products/Properties/UploadImage") %>',
                responseType: 'json',
                autoSubmit: false,
                onChange: function (file) {
                    currentLi = $("#btnBrowseServer"+productPropertyValueID).closest('li');
                    this.submit();
                },
                onSubmit: function (file, extension) {
                    this.setData({
                        productPropertyTypeId: productPropertyTypeID,
                        valueId: productPropertyValueID
                    });
                },
                onComplete: function (file, response) {
                    if (response.result) {
                        currentLi.find('.thumbnailPreview').show();
                        currentLi.find('.imageThumbnail').attr("src", response.imagePath);
                        currentLi.find('.btnBrowseServer').hide();
                    } else {
                        showMessage(response.message, true);
                    }
                }   
            });
        }

        function acceptNameChange() {
            var li = $('li.current');
            if(li.length!=0) {
                var tabLabel = li.find(".tabLabel");
                tabLabel.removeClass("editTabLabel");
                var editGroupNameDiv = tabLabel.find(".editGroupName");
                tabLabel.html(editGroupNameDiv.find(".attrLabel").val());
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Products") %>">
        <%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
            <%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
                <%= Model.Translations.Name() %></a> >
    <%= Html.Term("Variants") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Section Header -->
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("ProductVariants", "Product Variants") %></h2>
        <%= Html.Term("SetUpNewVariants", "Set-Up New Variants") %>
        | <a href="<%= ResolveUrl(string.Format("~/Products/Products/VariantSKUS/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>">
            <%= Html.Term("ManageVariantSKUS", "Manage Variant SKUS") %></a>
    </div>
    <!-- adding new variant groups -->
    <div class="UI-lightBg brdrAll pad10 mb10 FilterSet variantGroupTools">
        <h4>
            <%= Html.Term("CreateaNewVariantGroup", "Create a New Variant Group")%></h4>
        <!-- 2 different ways you can create a new variant group -->
        <div class="pad5 mb10 variantSetupChoices">
            <div class="FL">
                <input id="existingGroup" type="radio" name="variantGroup" class="FL variantGroupRadio" value="0" checked="checked" />
                <label class="FL" for="existingGroup">
                    <%= Html.Term("ChooseFromMaster","Choose from master")%></label>
            </div>
            <div class="FL ml10">
                <input id="newGroup" type="radio" name="variantGroup" class="FL variantGroupRadio" value="1" />
                <label class="FL" for="newGroup">
                    <%= Html.Term("CreateNewGroup","Create new group")%></label>
            </div>
            <span class="clr"></span>
        </div>
        <div class="newVarGroupOptions">
            <div class="FL mr10" id="masterProductPropertyTypeIdWrap">
                <div id="masterProductPropertyTypes">
                    <label>
                        <%=Html.Term("VariantMasterList-FormLabel","Master List") %>:</label>
                    
                        <%  var properties = SmallCollectionCache.Instance.ProductPropertyTypes.Where(ppt => ppt.IsProductVariantProperty && ppt.IsMaster && !Model.ProductBase.ProductBaseProperties.Any(pbp => pbp.ProductPropertyTypeID == ppt.ProductPropertyTypeID)); %>
                        <select id="masterProductPropertyTypeId" style='<%= properties.Count() > 0 ? "" : "width: 70px" %>'>
                        <option value="0"></option>
                        <%
                            foreach (var type in properties)
                          { %>
                        <option value="<%=type.ProductPropertyTypeID %>"><%=type.GetTerm() %></option>
                        <%}%>
                    </select>
                </div>
            </div>
            <div class="FL  newName" id="newVarGroupNameWrap" style="display: none;">
                <label>
                    <%=Html.Term("VariantGroupName-FormLabel","Group Name") %>:</label>
                <input type="text" class="TextInput pad2 newVarGroupName" id="newVarGroupName" />
            </div>
            <div class="FL ml10 addToMaster" id="addToMasterWrap" style="display: none;">
                <label for="addToMaster">
                    <%= Html.Term("AddToMasterList", "Add To Master List")%>:</label>
                <input type="checkbox" id="addToMaster" disabled='disabled' />
                
            </div>
            <div class="FL ">
                <label>
                    <%= Html.Term("Type")%>:</label>
                <select id="htmlInputTypeId">
                    <%foreach (var type in SmallCollectionCache.Instance.HtmlInputTypes.Where(hit=>hit.Active))
                      { %>
                    <option value="<%=type.HtmlInputTypeID %>">
                        <%=type.GetTerm() %></option>
                    <%}%>
                </select>
            </div>
            <div class="FL  RunFilter">
                <a href="javascript:void(0);" class="Button" id="CreateVariantGroup"><span>
                    <%= Html.Term("AddVariantGroup", "Add Variant Group")%></span></a>
            </div>
            <span class="clr"></span>
        </div>
    </div>
    <!--/ end adding utility -->
    <%var variantProperties = Model.ProductBase.ProductBaseProperties.Select(pbp => pbp.ProductPropertyType).Where(ppt => ppt.IsProductVariantProperty); %>
    <!-- display the variant groups -->
    <div id="ProductVariants">
        <!-- tab switcher for multiple variants -->
        <div class="Tabber">
            <ul class="inlineNav" id="VariantsTabber">
                <%
                    int index = 0;
                    foreach (var propertyType in variantProperties)
                    {%>
                <li <%=index==0 ? "class='current'" : "" %> rel="<%=propertyType.Name.Replace(" ","") %>">
                    <div class="tabLabel"><%=propertyType.GetTerm() %></div>
                </li>
                <%index++;
                    }
                    index = 0;
                %>
                <!--<li class="current" rel="Color"><div class="tabLabel">Color</div></li>
                <li rel="Size"><div class="tabLabel">Size</div></li>-->
            </ul>
        </div>
        <div class="brdrNNYY variantGroup" id="variantGroupsWrapper">
            <%
                foreach (var propertyType in variantProperties)
                {%>
            <div id="<%=propertyType.Name.Replace(" ","") %>" class="TabContent" style="display: <%=index==0 ? "block" : "none" %>;">
                <%Html.RenderPartial("VariantGroup", propertyType);%>
            </div>
            <%index++;
                }
            %>
        </div>
    </div>
    <!--/ end variant groups -->
</asp:Content>
