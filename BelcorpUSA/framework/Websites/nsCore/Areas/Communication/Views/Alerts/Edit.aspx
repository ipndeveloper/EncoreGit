<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Communication/Views/Shared/Communication.Master"
	Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Communication.Models.AlertTemplateViewModel>" %>

<%@ Import Namespace="nsCore.Areas.Communication.Helpers" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%=ResolveUrl("~/Scripts/jquery.insertatcaret.js") %>"></script>
	<script type="text/javascript">
		$(function () {
			$('#btnAddToken').live('click', function () {
				var tokenList = $('#tokenList').val();
				if (tokenList != null && tokenList.length > 0) {
					$('#message').insertAtCaret(tokenList);
				}
			});

			/** Editing Alert Template Translation **/
			$('.btnEditTemplate').live('click', function () {
				var alertTemplateTranslationID = $(this).closest('div#IdHolder').attr('data-id');

				$.get('<%= ResolveUrl("~/Communication/Alerts/GetAlertTemplateTranslation") %>',
                        {
                        	alertTemplateTranslationID: alertTemplateTranslationID,
                        	alertTemplateID: $('#alertTemplateID').val()
                        },
                        function (result) {
                        	// display partial view
                        	var alertTemplateForm = $('div#templateTranslationForm');
                        	alertTemplateForm.html(result);
                        	alertTemplateForm.show();
                        });
			});

			/** Deleting Alert Template Translation **/
			$('.btnDelete').live('click', function () {
				var alertTemplateTranslationID = $(this).closest('div#IdHolder').attr('data-id');

				if (confirm('<%: Html.Term("AreYouSureYouWishToDeleteThisItem", "Are you sure you wish to delete this item?") %>')) {

					var t = $(this);
					showLoading(t);

					$.post('<%= ResolveUrl("~/Communication/Alerts/DeleteAlertTemplateTranslation") %>',
                    {
                    	alertTemplateTranslationID: alertTemplateTranslationID,
                    	alertTemplateID: $('#alertTemplateID').val()
                    },
                    function (response) {
                    	hideLoading(t);

                    	if (response.result) {
                    		$('#alertTemplateID').val(response.alertTemplateID);
                    		window.location = "/Communication/Alerts/Edit/" + response.alertTemplateID;
                    	}
                    	//showMessage(response.message || '<%= Html.Term("DeleteSuccessfully", "Deleted successfully!") %>', !response.result);
                    });
				}
			});

			$('#btnAdd').click(function () {
				var alertTemplateID = $('#alertTemplateID').val();
				var langCount = $('#languageCount').val();
				if (langCount > 0) {
					if (alertTemplateID != 0) {

						$.get('<%: ResolveUrl("~/Communication/Alerts/GetAlertTemplateTranslation")%>',
                            { alertTemplateID: alertTemplateID },
                            function (result) {
                            	var alertTemplateForm = $('div#templateTranslationForm');
                            	alertTemplateForm.html(result);
                            	alertTemplateForm.show();
                            });

						$('div#alertTemplateForm').show();

					} else {
						alert('<%= Html.Term("SaveAlertTemplateFirst", "You must save a new alert template first.") %>');
					}
				} else {
					/* No more languages */
					alert('<%= Html.Term("AllLanguagesAreTaken", "All of the available languages have been selected.") %>');
				}

			});

			$('#btnSaveTemplate').click(function () {
				if (!$('#templateForm').checkRequiredFields()) {
					return false;
				}

				var t = $(this);
				showLoading(t);

				$.post('<%: ResolveUrl("~/Communication/Alerts/SaveAlertTemplate") %>',
                    {
                    	AlertTemplateID: $('#alertTemplateID').val(),
                    	Name: $('#name').val(),
                    	Active: $('#active').prop('checked'),
                    	AlertPriorityID: $('#alertPriority').val()
                    },
                    function (response) {
                    	hideLoading(t);
                    	showMessage(response.message || '<%= Html.Term("SavedSuccessfully", "Saved successfully!") %>', !response.result);
                    	if (response.result) {
                    		$('#alertTemplateID').val(response.alertTemplateID);
                    	}
                    });
			});

			$('#btnCancelTemplate').click(function () {
				window.location = '<%: ResolveUrl("~/Communication/Alerts") %>';
			});


		});


	</script>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%: Model.AlertTemplate.AlertTemplateID == 0 ? Html.Term("NewAlertTemplate", "New Alert Template")
                                       : Html.Term("EditAlertTemplate", "Edit Alert Template") %>
		</h2>
	</div>
	<%: Html.Hidden("languageCount", Model.LanguageCount)%>
	<%: Html.Hidden("alertTemplateID", Model.AlertTemplate.AlertTemplateID) %>
	<!-- AlertTemplate: Name, Priority, Active -->
	<div id="templateForm" class="splitCol formStyle1">
		<div class="FRow mb10">
			<label class="bold">
				<%: Html.Term("Name") %>:
			</label>
			<div class="FInput">
				<input id="name" type="text" class="required fullWidth pad5" name="Name is required."
					value="<%: Model.AlertTemplate.Name %>" />
			</div>
		</div>
		<div class="FRow mb10">
			<label class="bold">
				<%: Html.Term("Priority") %>:
				<!-- Dropdownlist for alert priorities -->
			</label>
			<div class="FInput Priority">
				<%: Html.DropDownList("alertPriority", new SelectList(SmallCollectionCache.Instance.AlertPriorities as System.Collections.IEnumerable,
                                               "AlertPriorityID", "TermName", 
                                               Model.AlertTemplate != null ? Model.AlertTemplate.AlertPriorityID : default(int)))%>
			</div>
		</div>
		<div class="FRow mb10">
			<label for="active" class="bold">
				<%: Html.Term("Active") %>
			</label>
			<div class="FInput">
				<input id="active" type="checkbox" <%: (Model.AlertTemplate.Active != null && Model.AlertTemplate.Active == true)? "checked=\"checked\"" : "" %> />
			</div>
		</div>
		<p>
			<a id="btnSaveTemplate" href="javascript:void(0);" class="Button BigBlue"><span>
				<%: Html.Term("Save") %></span></a>
            <a id="btnCancelTemplate" href="javascript:void(0);" class="Button"><span>
				<%: Html.Term("Cancel") %></span></a>
		</p>
	</div>
	<hr />
	<div class="FLcolWrapper mt10">
		<div class="FL splitCol30">
			<h3>
				<%: Html.Term("AlertTemplateTranslations", "Alert Template Translations") %>
				<a style="cursor: pointer" id="btnAdd" class="FR Add"><span>
					<%: Html.Term("Add", "Add") %></span> </a>
			</h3>
			<div class="emailTranslationsWrapper">
				<% foreach (var alertTemplate in Model.AlertTemplate.AlertTemplateTranslations)
	   {  %>
				<div class="UI-lightBg brdrAll mb10 emailTranslation" id="IdHolder" data-id="<%: alertTemplate.AlertTemplateTranslationID %>">
					<div class="inner pad5">
						<div class="brdrAll pad5 UI-secBg">
							<a class="btnDelete DTL Remove FL" href="javascript:void(0);"><span>
								<%: Html.Term("Delete") %></span></a> <a title="<%: Html.Term("Edit") %> <%: Language.Load(alertTemplate.LanguageID).TermName %>"
									class="btnEditTemplate FR DTL EditSite" id="title<%:alertTemplate.AlertTemplateTranslationID %>"
									href="javascript:void(0);"><span>
										<%: Html.Term("Edit") %></span></a> <span class="clr"></span>
						</div>
						<ul class="infoList">
							<li><span class="label">
								<%: Html.Term("Language") %>:</span> <span class="data">
									<%: Language.Load(alertTemplate.LanguageID).TermName %></span> <span class="clr">
								</span></li>
							<li>
								<label id="message<%: alertTemplate.AlertTemplateTranslationID %>">
									<%: String.IsNullOrEmpty(alertTemplate.Message) ? Html.Term("Nomessage"): Html.Truncate(alertTemplate.Message, 55) %>
								</label>
							</li>
						</ul>
					</div>
				</div>
				<% } %>
			</div>
		</div>
		<div class="FR splitCol70">
			<div id="templateTranslationForm" style="display: none;">
				<% Html.RenderPartial("_AlertTemplateTranslation", Model); %>
			</div>
		</div>
	</div>
	
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<% if (Model.AlertTemplate.AlertTemplateID != 0)
	{%>
	<a href="<%: ResolveUrl("~/Communication/Alerts") %>">
		<%: Html.Term("AlertTemplates", "Alert Templates")%>
	</a>>
	<%: Html.Term("EditAlertTemplate", "Edit Alert Template")%>
	<% }
	else
	{ %>
	<a href="<%: ResolveUrl("~/Communication/Alerts") %>">
		<%: Html.Term("AlertTemplates", "Alert Templates")%>
	</a>>
	<%: Html.Term("NewAlertTemplate", "New Alert Template")%>
	<% } %>
</asp:Content>
