<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Products/Product.Master"
	Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Product>" %>

<%@ Import Namespace="nsCore.Areas.Products.Models" %>
<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<%--<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/3.5.2/adapters/jquery.js"></script>--%>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ajaxupload.js") %>"></script>
	<script type="text/javascript">
		var reorderImages;
		$(function () {
			reorderImages = function () {
				var images = $('.ImageThumbWrap'), length = images.length;
				images.each(function (i) {
					$(this).find('.moveUp')[i ? 'show' : 'hide']();
					$(this).find('.moveDown')[i == length - 1 ? 'hide' : 'show']();
				});

				if (length < 1) {
					$('#btnSaveImages').hide();
				} else {
					$('#btnSaveImages').show();
				}
			};
			$('#txtLongDescription').ckeditor();
			$('#sLanguage').change(function () {
				$.get('<%= ResolveUrl(string.Format("~/Products/Products/GetDescription/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>', { languageId: $(this).val() }, function (response) {
					if (response.result) {
						$('#txtName').val(response.name);
						$('#txtShortDescription').val(response.shortDescription);
						$('#txtLongDescription').val(response.longDescription);
					} else {
						showMessage(response.message, true);
					}
				});
			});

			$('#btnSaveDescriptions').live('click', function () {
				$('#txtLongDescription').html(CKEDITOR.instances['txtLongDescription'].getData());
				$.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveDescription/{0}/{1}", Model.ProductBaseID, Model.ProductID)) %>', { languageId: $('#sLanguage').val(), name: $('#txtName').val(), shortDescription: $('#txtShortDescription').val(), longDescription: $('#txtLongDescription').val(), productId: $('#hidProductId').val(), isVariant: $('#hidIsProductVariant').val() }, function (response) {
					showMessage(response.message || 'Content saved!', !response.result);
				});
			});

			new AjaxUpload('btnUpload', {
				action: '<%= ResolveUrl(string.Format("~/Products/Products/SaveImage/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>',
				data: { folder: 'Products', createProductFile: true, productId: $('#hidProductId').val() },
				responseType: 'json',
				onComplete: function (file, response) {
					if (response.result) {
						var imageCount = $('#imagesContainer img').length;
						$('#imagesContainer').append('<p id="productFile' + response.fileId + '" class="ImageThumbWrap"><span class="ImgTools"><a href="javascript:void(0);" class="moveUp"' + (imageCount > 0 ? '' : 'style="display:none;"') + '><span class="icon-ArrowUp"></span></a><br><a href="javascript:void(0);" class="moveDown" style="display:none;"><span class="icon-ArrowDown"></span></a><br></span><img width="100" src="' + response.imagePath + '" alt=""><input type="checkbox" class="checkbox" name=""></p>');
						$('#btnSaveImages').show();
						reorderImages();
					} else {
						showMessage(response.message, true);
					}
				}
			});

			$('.moveUp').live('click', function () {
				var image = $(this).parents('.ImageThumbWrap'), prev = image.prev('.ImageThumbWrap');
				image.detach();
				prev.before(image);
				reorderImages();
			});
			$('.moveDown').live('click', function () {
				var image = $(this).parents('.ImageThumbWrap'), next = image.next('.ImageThumbWrap');
				image.detach();
				next.after(image);
				reorderImages();
			});

			$('#btnSaveImages').live('click', function () {
				$('#btnSaveImages').attr('disabled', true).css('cursor', 'default').css('pointer-events', 'none');
				$('#btnDeleteImages').attr('disabled', true).css('cursor', 'default').css('pointer-events', 'none');
				var data = {
					productId: $('#hidProductId').val()
				};
				$('.ImageThumbWrap').each(function (i) {
					data['productFiles[' + i + ']'] = $(this).attr('id').replace(/\D/g, '');
				});
				$.post('<%= ResolveUrl(string.Format("~/Products/Products/SaveImagesSort/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>', data, function (response) {
					$('#btnSaveImages').removeAttr('disabled').css('cursor', '').css('pointer-events', 'auto');
					$('#btnDeleteImages').removeAttr('disabled').css('cursor', '').css('pointer-events', 'auto');
					showMessage(response.message || 'Images saved!', !response.result);
				});
			});

			$('#btnDeleteImages').live('click', function (e) {
				e.stopPropagation();

				var selected = $('.ImageThumbWrap input:checked');
				if (selected.length) {
					$('#btnSaveImages').attr('disabled', true).css('cursor', 'default').css('pointer-events', 'none');
					$('#btnDeleteImages').attr('disabled', true).css('cursor', 'default').css('pointer-events', 'none');
					var data = {
						productId: $('#hidProductId').val()
					};
					selected.each(function (i) {
						data['productFiles[' + i + ']'] = $(this).parent().attr('id').replace(/\D/g, '');
					});
					$.post('<%= ResolveUrl(string.Format("~/Products/Products/DeleteImages/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>', data, function (response) {
						$('#btnSaveImages').removeAttr('disabled').css('cursor', '').css('pointer-events', 'auto');
						$('#btnDeleteImages').removeAttr('disabled').css('cursor', '').css('pointer-events', 'auto');
						if (response.result) {
							selected.each(function (i) {
								$(this).parent().remove();
							});
							reorderImages();
						} else {
							showMessage(response.message, true);
						}
					});
				}
			});

			$("#sProduct option").each(function () {
				var productOption = $(this);
				var data = {
					productId: productOption.val()
				};
				$.post('<%= ResolveUrl(string.Format("~/Products/Products/GetProductToolTip/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>', data, function (response) {
					if (response.result) {
						productOption.attr({ 'title': response.toolTip });
					} else {
						showMessage(response.message);
					}
				});

			});

			$("#sProduct").change(function () {
				$("#CMS").html();
				var pId = $(this).val();
				var data = {
					productId: pId
				};
				$("#hidProductId").val(pId);
				var instance = CKEDITOR.instances['txtLongDescription'];
				if (instance) {
					CKEDITOR.remove(instance);
					instance.destroy();
				}
				$.post('<%= ResolveUrl(string.Format("~/Products/Products/RenderProductCMS/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>', data, function (response) {
					if (response.result) {
						$("#CMS").html(response.productCMSHTML);

						CKEDITOR.replace('txtLongDescription');
						//$('#txtLongDescription').ckeditorGet().readOnly(false);
						//                        if (pId=='<%=Model.ProductID %>') {
						//                            $('.childLock').click();
						//                        } else {
						//                            $('.childUnLock').click();
						//                        }
					} else {
						showMessage(response.message);
					}
				});
			});

			$('.childLock').live('click', function () {
				$(this).hide();
				$(this).closest('h3').find('.childUnLock').show();
				var td = $(this).closest('td');
				td.find('input,select,textarea').attr('disabled', false);
				// $('#txtLongDescription').ckeditorGet().readOnly(false);
			});

			$('.childUnLock').live('click', function () {
				$(this).hide();
				$(this).closest('h3').find('.childLock').show();
				var td = $(this).closest('td');
				td.find('input,select,textarea').attr('disabled', true);
				//$('#txtLongDescription').ckeditorGet().readOnly(true);
			});

			//            var cancelEvent = function (evt) {
			//                evt.cancel();
			//            };

			//            CKEDITOR.editor.prototype.readOnly = function (isReadOnly) {
			//                // Turn off contentEditable.
			//                this.document.$.body.disabled = isReadOnly;
			//                CKEDITOR.env.ie ? this.document.$.body.contentEditable = !isReadOnly
			//                  : this.document.$.designMode = isReadOnly ? "off" : "on";

			//                // Prevent key handling.
			//                this[isReadOnly ? 'on' : 'removeListener']('key', cancelEvent, null, null, 0);
			//                this[isReadOnly ? 'on' : 'removeListener']('selectionChange', cancelEvent, null, null, 0);

			//                // Disable all commands in wysiwyg mode.
			//                var command,
			//                    commands = this._.commands,
			//                    mode = this.mode;

			//                for (var name in commands) {
			//                    command = commands[name];
			//                    isReadOnly ? command.disable() : command[command.modes[mode] ? 'enable' : 'disable']();
			//                    this[isReadOnly ? 'on' : 'removeListener']('state', cancelEvent, null, null, 0);
			//                }
			//            }

			//$('#txtLongDescription').ckeditorGet().readOnly(false);
		});
	</script>
</asp:Content>
<asp:Content ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Products") %>">
		<%= Html.Term("Products") %></a> > <a href="<%= ResolveUrl("~/Products/Products") %>">
			<%= Html.Term("BrowseProducts", "Browse Products") %></a> > <a href="<%= ResolveUrl(string.Format("~/Products/Products/Overview/{0}/{1}", Model.ProductBaseID, Model.ProductID )) %>">
				<%= Model.Translations.Name() %></a> >
	<%= Html.Term("CMS") %>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<!-- Section Header -->
	<div class="SectionHeader">
		<h2>
		<% 
			var translations = Model.Translations;
			var translation = translations.Any() ?  translations.FirstOrDefault(t => t.LanguageID == CoreContext.CurrentLanguageID) ?? translations.First() : null;
			var selectedLanguageId = translation != null ? translation.LanguageID : CoreContext.CurrentLanguageID;
		%>
			<%= Model.Translations.Name() %>
			<%= Html.Term("CMS", "CMS") %></h2>
		<%= Html.Term("Localization", "Localization") %>:
		<%= Html.DropDownLanguages(htmlAttributes: new { id = "sLanguage" }, selectedLanguageID: selectedLanguageId)%>
		<%if (Model.IsVariantTemplate)
		  {
			  Response.Write(Html.Term("Product") + ":");
			  Response.Write(Html.DropDownVariantProducts(htmlAttributes: new { id = "sProduct" }, selectedProductID: (Model.ProductID), productBase: (Model.ProductBase), includeVariantTemplate: (true)));

		  } %>
		  <input type="hidden" id="hidProductId" value="<%=Model.ProductID%>" />
	</div>
	<!-- Product CMS Stuff -->
	
	<div id="CMS">
		<%--<%ViewDataDictionary viewDataDictionary = new ViewDataDictionary();
		  viewDataDictionary.Add("ProductBase", Model.ProductBase);
		  viewDataDictionary.Add("IsVariant", false);
		  Html.RenderPartial("ProductCMS", Model, viewDataDictionary); %>--%>
		<%ProductCMSModel productCMSModel = new ProductCMSModel(Model, false);
		  Html.RenderPartial("ProductCMS", productCMSModel); %>
	</div>
</asp:Content>
