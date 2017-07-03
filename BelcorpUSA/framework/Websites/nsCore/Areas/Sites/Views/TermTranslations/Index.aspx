<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Sites/Views/Shared/Sites.Master" Inherits="System.Web.Mvc.ViewPage<nsCore.Areas.Sites.Models.TermTranslationsIndexModel>" %>

<asp:Content ContentPlaceHolderID="HeadContent" runat="server">
	<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/ajaxupload.js") %>"></script>
	<script type="text/javascript" src="<%= ResolveUrl("~/Scripts/autoresize.min.js") %>"></script>
	<script type="text/javascript">
		var currentPage = 0, maxPage = Math.ceil(parseInt('<%= Model.EnglishTermCount %>') / 20) - 1;
		$(function () {
			new AjaxUpload('termsUpload', {
				action: '<%= ResolveUrl("~/Sites/TermTranslations/Import") %>',
				name: 'termFile',
				autoSubmit: true,
				onComplete: function (file, response) {
					response = eval('(' + response + ')');
					if (response.result) {
						if (response.anyOutOfDateTerms) {
							$('#outOfDateTerms').jqm({
								ajax: '<%= ResolveUrl("~/Sites/TermTranslations/Merge") %>',
								onShow: function (h) {
									h.w.css({
										top: (Math.floor($(window).height() / 2) - Math.floor((h.w.height() + 500) / 2)) + 'px',
										left: Math.floor($(window).width() / 2) + 'px'
									}).fadeIn('slow');
								},
								onHide: function (h) {
									h.w.fadeOut('slow');
									h.o.remove();
									showMessage('Terms imported successfully', false);
									getTerms();
								}
							}).jqmShow();
						} else {
							showMessage('Terms imported successfully', false);
							getTerms();
						}
					} else {
						showMessage('Term import failed.  Error: ' + response.message, true);
					}
				}
			});

			$('select').change(function () {
				currentPage = 0;
				getTerms();
				if (this.id == 'languageFilter') {
					$('#terms th:last').text($('option:selected', this).text());
				}
			});
			$('#txtSearchTerms').keyup(function (e) {
				if (e.keyCode == 13)
					$('#btnSearchTerms').click();
			});
			$('#btnSearchTerms').click(function () {
				currentPage = 0;
				getTerms();
			});
			$('#btnPreviousPage').click(function () {
				if (currentPage > 0) {
					--currentPage;
					getTerms();
				}
			});
			$('#btnNextPage').click(function () {
				if (currentPage < maxPage) {
					++currentPage;
					getTerms();
				}
			});
			$('.btnSave').click(function () {
				var data = { languageId: $('#languageFilter').val() };
				$('#terms tbody tr:has(.changed)').each(function (i) {
					data['terms[' + i + '].TermId'] = $(this).find('[id^="termid_"]:first').val();
					data['terms[' + i + '].TermName'] = $(this).find('[id^="termname_"]:first').val();
					data['terms[' + i + '].LocalTerm'] = $(this).find('.localTerm').val();
					data['terms[' + i + '].LanguageId'] = data.languageId;
				});
				$.post('<%= ResolveUrl("~/Sites/TermTranslations/Save") %>', data, function (results) {
					if (results.result) {
						showMessage('Terms saved successfully', false);
						$('#terms tbody tr:has(.changed)').each(function (i) {
							$('td:eq(0)', this).empty();
							$('.localTerm', this).removeClass('changed');
						});
					} else {
						showMessage(results.message, true);
						$('#errorCenter').focus();
					}
				});
			});
			$('.localTerm').live('keyup', function () {
				$(this).addClass('changed');
			});
			$('#languageFilter').val(1);
			$('#txtSearchTerms,#typeFilter').val('');
			getTerms();

			$('#btnExportTerms').click(function () {
				window.location = '<%= ResolveUrl("~/Sites/TermTranslations/Export") %>?languageId=' + $('#languageFilter').val() + '&type=' + $('#typeFilter').val() + '&term=' + $('#txtSearchTerms').val();
			});
			$('#termsGenerateDefaults').click(function () {
				var t = $(this);
				showLoading(t);
				$.getJSON('<%= ResolveUrl("~/Sites/TermTranslations/GenerateDefaultTerms") %>', {}, function (results) {
					hideLoading(t);
					if (results.result) {
						showMessage('<%= Html.Term("DefaultTermsCreatedSuccessfully", "Default terms created successfully!") %>', false);
					} else {
						showMessage(results.message, true);
					}
				});
			});
		});

		function getTerms() {
			if ($('#languageFilter').val() == null) {
				$('#languageFilter')[0].selectedIndex = 0 //ie9 has drop down default selection issue
			}
			$('#terms tbody').empty().append('<tr><td colspan="3"><img src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." /></td></tr>');
			$.getJSON('<%= ResolveUrl("~/Sites/TermTranslations/Get") %>', { page: $('#pageSize').val() ? currentPage : undefined, pageSize: $('#pageSize').val(), languageId: $('#languageFilter').val(), type: $('#typeFilter').val(), term: $('#txtSearchTerms').val() }, function (results) {
				maxPage = results.totalPages - 1;
				$('#terms tbody').html(results.terms);
				$('#btnNextPage,#btnPreviousPage').removeAttr('disabled').css({ color: '', cursor: '' });
				if (currentPage == maxPage)
					$('#btnNextPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
				if (currentPage == 0)
					$('#btnPreviousPage').attr('disabled', 'disabled').css({ color: 'black', cursor: 'default' });
				$('.pages').text(String.format('<%=Html.Term("XOfXPages", "{0} of {1}") %>', !results.totalPages ? 0 : currentPage + 1, results.totalPages || 0));
			});
		}
	</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="BreadCrumbContent" runat="server">
	<a href="<%= ResolveUrl("~/Sites") %>">
		<%= Html.Term("Sites", "Sites")%></a> >
	<%= Html.Term("TermTranslations", "Term Translations")%>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>
<asp:Content ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
	<div class="SectionHeader">
		<h2>
			<%= Html.Term("TermTranslations", "Term Translations")%></h2>
		<a id="btnExportTerms" href="javascript:void(0);" class="Excel textlink">
			<%= Html.Term("ExportTermsToExcel", "Export terms to Excel")%></a> | <a id="termsUpload" href="javascript:void(0);">
				<%= Html.Term("ImportTermsFromCSV", "Import terms from CSV")%></a> | <a id="termsGenerateDefaults" href="javascript:void(0);">
					<%= Html.Term("CreateMissingDefaultTermsForListItems", "Create missing default terms for list items")%></a>
	</div>
	<div class="UI-lightBg brdrAll GridFilters">
		<div class="FL FilterSet">
			<div class="FL">
				<label>
					<%= Html.Term("Language", "Language")%>:</label>
				<%= Html.DropDownLanguages(htmlAttributes: new { id = "languageFilter" }, selectedLanguageID: CoreContext.CurrentLanguageID)%>
				<%= Html.Term("Type", "Type")%>:
				<select id="typeFilter">
					<option value="">
						<%= Html.Term("AllTerms", "All Terms")%></option>
					<option value="Untranslated">
						<%= Html.Term("Untranslated", "Untranslated")%></option>
					<option value="OutOfDate">
						<%= Html.Term("OutOfDate", "Out of date")%></option>
				</select>
			</div>
			<div class="FL">
				<label>
					<%= Html.Term("SearchTerms", "Search Terms")%>:</label>
				<input id="txtSearchTerms" type="text" class="TextInput" style="width: 250px;" />
			</div>
			<div class="FL RunFilter">
				<a id="btnSearchTerms" class="ModSearch Button" href="javascript:void(0);"><%: Html.Term("ApplyFilter", "Apply Filter")%></a>
			</div>
			<span class="clr"></span>
		</div>
		<span class="clr"></span>
	</div>
	<div class="UI-lightBg mt10 mb10 pad5 brdrAll">
		<a href="javascript:void(0);" class="FL btnSave Button BigBlue">
			<%= Html.Term("SaveTermUpdates","Save Term Updates")%></a>
		<img class="FL Loading ml10" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." style="display: none;" height="28" />
		<span class="clr"></span>
	</div>
	<table id="terms" class="DataGrid" width="100%" cellspacing="0">
		<thead>
			<tr class="GridColHead">
				<th class="GridEdit">
				</th>
				<th style="width: 175px;">
					<%= Html.Term("TermName", "Term Name")%>
				</th>
				<th style="width: 175px;">
					<%= Html.Term("EnglishTerm", "English Term")%>
				</th>
				<th>
					<%= Html.Term("EnglishTerm", "English Term")%>
				</th>
			</tr>
		</thead>
		<tbody>
		</tbody>
	</table>
	<div class="UI-lightBg mt10 mb10 pad5 brdrAll">
		<a href="javascript:void(0);" class="FL btnSave Button BigBlue"><span>
			<%= Html.Term("SaveTermUpdates","Save Term Updates")%></span></a>
		<img class="FL Loading ml10" src="<%= ResolveUrl("~/Content/Images/Icons/loading-blue.gif") %>" alt="loading..." style="display: none;" height="28" />
		<span class="clr"></span>
	</div>
	<div class="UI-mainBg Pagination">
		<div class="PaginationContainer">
			<div class="Bar">
				<a href="javascript:void(0);" id="btnPreviousPage" class="previousPage mr10"><span>&lt;&lt;
					<%= Html.Term("Previous")%></span></a> <span class="pages"></span><a href="javascript:void(0);" id="btnNextPage" class="nextPage"><span>
						<%= Html.Term("Next")%>
						&gt;&gt;</span></a> <span class="clr"></span>
			</div>
			<div class="PageSize" class="PageSize">
				<%= Html.Term("PageSize", "Page Size")%>:
				<select id="pageSize">
					<option value="20" selected="selected">20</option>
					<option value="50">50</option>
					<option value="100">100</option>
					<option value="200">200</option>
					<option value="500">500</option>
				</select>
				<span class="clr"></span>
			</div>
			<span class="clr"></span>
		</div>
	</div>
	<div id="outOfDateTerms" class="LModal jqmWindow">
		<div class="mContent">
		</div>
	</div>
</asp:Content>
