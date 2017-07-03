<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master" 
Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Communication.Models.GeneralTemplateModel>" %>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">

    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/fileuploader.css") %>" />

	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/ckeditor.js"></script>
	<script type="text/javascript" src="/fileuploads/resources/ckeditor/4.4.4/adapters/jquery.js"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/fileuploader.js") %>"></script>

    <script type="text/javascript">
        $(function () {

            $('.btnEditTemplate').click(function () {
                
                var id = $(this).attr("id");
                
                $.getJSON('<%= ResolveUrl("~/Communication/NewPrintOrder/GetGeneralTemplateTranslation") %>',
						{ id: $(this).attr("id") },
						function (data) {
						    $('#body').val(data.body);
						    $('#activeGTT').prop("checked", data.active);
						});

                $('#generalTemplateTranslationID').val(id);
                
                $.getJSON('<%= ResolveUrl("~/Communication/NewPrintOrder/AvailableLanguages") %>',
					{ id: $('#generalTemplateId').val(), generalTemplateTranslationID: $(this).attr("id") },
					function (data) {
					    $('#LanguageID >option').remove();
					    $(data.languages).each(function () {
					        $("<option>").val(this.ID)
										 .text(this.Name)
										 .appendTo('#LanguageID');

					        $('#LanguageID').val(data.selected);

					    });
					    $('#generalTemplateForm').show();
					});
            });

            $('#btnCancel').click(function () {
                $('#generalTemplateForm').hide();
            });

            $('#btnAdd').click(function () {

                var id = $('#generalTemplateId').val();
                var langCount = $('#languageCount').val();
                if (langCount > 0) {
                    if (parseInt(id)) {
                        $('#body').val('');
                        $('#generalTemplateTranslationID').val('');

                        $.getJSON('<%= ResolveUrl("~/Communication/NewPrintOrder/AllLanguages") %>',
							{ id: $('#generalTemplateId').val() },
							function (data) {

							    $('#LanguageID >option').remove();
							    $(data).each(function () {
							        $("<option>").val(this.ID)
													.text(this.Name)
													.appendTo('#LanguageID');

							    });
							    $('#generalTemplateForm').show();
							});
                    } else {
                        alert('<%= Html.Term("SaveEmailTemplateFirst", "You must save a new email template first.") %>');
                    }
                } else {
                    /* No more languages */
                    alert('<%= Html.Term("AllLanguagesAreTaken", "All of the available languages have been selected.") %>');
                }

            });

            $('#btnSave').click(function () {
                if (!$('#generalTemplateForm').checkRequiredFields()) {
                    return false;
                }

                var isValid = true;

                if (isValid) {
                    var t = $(this);
                    showLoading(t);
                    $.post('<%= ResolveUrl("~/Communication/NewPrintOrder/SaveGeneralTemplateTranslation") %>', {
                        GeneralTemplateTranslationID: $('#generalTemplateTranslationID').val(),
                        GeneralTemplateID: $('#generalTemplateId').val(),
                        LanguageID: $('#LanguageID').val(),
                        Body: $('#bodyHtml').is(':visible') ? $('#bodyHtml').val() : $('#body').val(),
                        Active: $('#activeGTT').prop('checked')

                    }, function (response) {
                       
                        hideLoading(t);
                        showMessage(response.message, !response.result);
                        if (response.result) {

                            $('#generalTemplateId').val(response.generalTemplateId);
                            $('#generalTemplateForm').hide();
                            //							if (response.templateOnly) {
                            window.location = "/Communication/NewPrintOrder/Edit/" + response.generalTemplateID;
                            //							} 
                        }
                    });
                }
            });

            $('#order').numeric();

            $('#btnCancelTemplate').click(function () {

                window.location = "/Communication/NewPrintOrder";
            });

            $('#btnSaveTemplate').click(function () {
                var isComplete = true;

                if (!$('#templateForm').checkRequiredFields()) {
                    isComplete = false;
                }

                if (isComplete) {

                    var t = $(this);
                    showLoading(t);

                    var data = {
                        GeneralTemplateID: $('#generalTemplateId').val(),
                        Name: $('#name').val(),
                        GeneralTemplateSectionID: $('#section').val(),
                        Order: $('#order').val(),
                        DateStar: $('#txtDateStar').val(),
                        DateEnd: $('#txtDateEnd').val(),
                        PeriodID: $('#period').val(),
                        Active: $('#active').prop('checked')
                    };
                    $.post('<%= ResolveUrl("~/Communication/NewPrintOrder/Save") %>', data, function (response) {
                        hideLoading(t);
                        showMessage(response.message || '<% = Html.Term("SavedSuccessfully", "Saved successfully!") %>', !response.result);
                        if (response.result) {
                            $('#generalTemplateId').val(response.GeneralTemplateID);

                            //                            if (response.redirectUrl)
                            //                                window.location = response.redirectUrl;
                        }
                    });
                }
                else {
                    return isComplete;
                }
            });
        });

    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">

	<a href="<%= ResolveUrl("~/Communication/NewPrintOrder") %>">
		<%= Html.Term("Templates", "New Print Order") %></a> >
	<%= Model.NewPrintOrderSearchData.GeneralTemplateID == 0 ? Html.Term("NewTemplate", "New Template") : Html.Term("EditTemplate", "Edit Template") %>
	
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <div class="SectionHeader">
		<h2>
			<%= Model.NewPrintOrderSearchData.GeneralTemplateID == 0 ? Html.Term("NewTemplate", "New Template") : Html.Term("EditTemplate", "Edit Template") %>
        </h2>
	</div>
    <div id="templateForm" class="splitCol formStyle1"> 
        <div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("Name")%>:</label>
			<div class="FInput">
				<input type="hidden" id="generalTemplateId" value="<%= Model.NewPrintOrderSearchData.GeneralTemplateID %>" />
				<input id="name" type="text" class="required fullWidth pad5" name="template name is required." value="<%= Model.NewPrintOrderSearchData.Name %>" />
			</div>
		</div>
        <div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("Section")%>:</label>
			<div class="FInput">
				<select id="section">
					<% 
						foreach (var generalTemplateSection in NewPrintOrder.ListGeneralSection())
						{ %>
					<option value="<%= generalTemplateSection.Key %>" <%= generalTemplateSection.Key == Model.NewPrintOrderSearchData._GeneralTemplateSectionID ? "selected=\"selected\"" : "" %>>
						<%= generalTemplateSection.Value%></option>
					<%} %>
				</select>
			</div>  
		</div>
       
        <div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("Order")%>:</label>
			<div class="FInput">
				<input id="order" type="text" class="required fullWidth pad5" name="order is required." value="<%= Model.NewPrintOrderSearchData.Order == 0? "": Model.NewPrintOrderSearchData.Order.ToString()%>" />
                
			</div>
		</div>
        <div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("Star Date")%>:</label>
			<div class="FInput">
                    <input id="txtDateStar" type="text" class="DatePicker" value="<%= Model.NewPrintOrderSearchData.DateStar.HasValue ? Model.NewPrintOrderSearchData.DateStar.Value.ToShortDateString() : "" %>"
                    style="width: 9.091em;" /><br />
			</div>
		</div>
         <div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("End Date")%>:</label>
			<div class="FInput">
				<input id="txtDateEnd" type="text" class="DatePicker" value="<%= Model.NewPrintOrderSearchData.DateEnd.HasValue ? Model.NewPrintOrderSearchData.DateEnd.Value.ToShortDateString() : "" %>"
                    style="width: 9.091em;" /><br />
			</div>
		</div>
        <div class="FRow mb10">
			<label class="bold">
				<%= Html.Term("Period")%>:</label>
			<div class="FInput">
				<select id="period">
					<% 
						foreach (var generalTemplatePeriod in NewPrintOrder.ListPeriod())
						{ %>
					<option value="<%= generalTemplatePeriod.Key %>" <%= generalTemplatePeriod.Key == Model.NewPrintOrderSearchData.Period ? "selected=\"selected\"" : "" %>>
						<%= generalTemplatePeriod.Value%></option>
					<%} %>
				</select>
			</div>  
		</div>
        <div class="FRow mb10">
			<label class="bold" for="active">
				<%= Html.Term("Active")%></label>
			<input id="active" type="checkbox" <%= (Model.NewPrintOrderSearchData.Statu != null && Model.NewPrintOrderSearchData.Statu) ? "checked=\"checked\"" : "" %> />
		</div>


        <div class="FRow">
			<p>
				<a id="btnSaveTemplate" href="javascript:void(0);" class="Button BigBlue"><span>
					<%= Html.Term("Save")%></span></a>
                <a id="btnCancelTemplate" href="javascript:void(0);" class="Button"><span>
					<%= Html.Term("Cancel")%></span></a>
			</p>
		</div>
    </div>

    <!-- Displays the  Templates Translations if any -->
	<div class="FLcolWrapper mt10">
		<div class="FL splitCol30">
			<h3 class="mb10">
				<%= Html.Term("TemplateTranslations", "Template Translations")%>
				<a style="cursor: pointer" id="btnAdd" class="FR DTL Add">
					<%= Html.Term("Add", "Add")%></a>
             </h3>
            <div class="emailTranslationsWrapper">
                <% 
                    if (Model.NewPrintOrderSearchData.GeneralTemplateTranslations != null)
                    {
                        foreach (var GeneralTemplateTranslations in Model.NewPrintOrderSearchData.GeneralTemplateTranslations)
					    { 
				%>
                <div class="UI-lightBg brdrAll mb10 emailTranslation">
					<div class="inner pad5">
						<div class="brdrAll pad5 UI-secBg">
						   <%: Html.ActionLink("Delete", "DeleteTemplateTranslation", new { id = GeneralTemplateTranslations.GeneralTemplateTranslationID, generalTemplateID = Model.NewPrintOrderSearchData.GeneralTemplateID }, new { @class = "DTL FL Remove" })%>
							<a title="<%= Html.Term("Edit") %> " class="FR DTL EditSite mr10 btnEditTemplate" id="<%: GeneralTemplateTranslations.GeneralTemplateTranslationID %>" href="javascript:void(0);">Edit</a>							
							<span class="clr"></span>
						</div>
						<ul class="infoList">
                            <li>
							    <span class="label"><%= Html.Term("Language") %>: </span>
							    <span class="data"><%:Language.Load(GeneralTemplateTranslations.LanguageID).TermName%></span>
							    <span class="clr"></span>
							</li>
							
						</ul>
					</div>
				</div>
                <%
                        }
					}      
				%>
				
			</div>
		</div>
		<div class="FR splitCol70">
			<% Html.RenderPartial("_GeneralTemplateTranslation", Model); %>
		</div>
		<span class="clr"></span>
	</div>
</asp:Content>
