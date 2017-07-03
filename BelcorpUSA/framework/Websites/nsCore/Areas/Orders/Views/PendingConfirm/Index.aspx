<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Orders/Views/Shared/Orders.Master" Inherits="System.Web.Mvc.ViewPage<NetSteps.Data.Entities.Order>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="YellowWidget" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="BreadCrumbContent" runat="server">

  <div class="SectionHeader">
        <h2>
            <%= Html.Term("OrdersPendingReturn", "Orders Pending Return") %>
        </h2>
    </div>
    <div id="rowCountLabel">
        <%= Html.Term("ThereAre", "There are ") %> <span id="rowCountGrid"></span><%= Html.Term("Total", " Total") %>
    </div>
    <%
        if (TempData["Error"] != null)
        { %>
    <div style="-moz-background-clip: border; -moz-background-inline-policy: continuous; -moz-background-origin: padding; background: #FEE9E9 none repeat scroll 0 0; border: 1px solid #FF0000; display: block; font-size: 14px; font-weight: bold; margin-bottom: 10px; padding: 7px;">
        <div style="color: #FF0000; display: block;" class="ui-icon icon-exclamation">
            <%= TempData["Error"].ToString() %></div>
    </div>
    <% } %>

<% Html.PaginatedGrid<NetSteps.Data.Entities.Business.HelperObjects.SearchData.OrderPendingSearchData>("~/Orders/PendingConfirm/GetOrdersPending")
.AutoGenerateColumns()
.AddInputFilter(Html.Term("OrderNumber", "Order Return Number "),"OrderNumber") 
.AddInputFilter(Html.Term("AccountNumberOrName", "Account Number or Name"),"AccountNumber")
.AddInputFilter(Html.Term("NumberSupportTicket", "Number support Ticket"), "numberSupportTicket") 
.AddInputFilter(Html.Term("NumberMail", "Number Mail"), "numberMail") 
            
.AddInputFilter(Html.Term("DateRange", "Date Range"), "startDate",Html.Term("StartDate", "Start Date")/*new DateTime(1900, 1, 1).ToShortDateString()*/, true)
.AddInputFilter(Html.Term("To", "To"), "endDate", Html.Term("EndDate", "End Date")/*DateTime.Now.ToShortDateString()*/, true, true)
        
.ClickEntireRow()
 
.Render(); %>

    <script type="text/javascript">
        $(function () {
            $('#paginatedGridFilters a.filterButton')
              .unbind('click') // takes care of jQuery-bound click events
              .attr('onclick', '') // clears `onclick` attributes in the HTML
              .each(function () { // reset `onclick` event handlers
                  this.onclick = null;
              });            
        });    
    </script>

     <script type="text/javascript">
         $(function () {
             $('#additionalFilterButton').click(function () {
                 var plusText = $("#additionalFilterPlus").text();
                 $("#additionalFilters").slideToggle('fast'),
            $("#additionalFilterPlus").text(plusText == "+" ? "-" : "+");
                 return false;
             });

             var currentPage = 0;
             var maxPage;
             var sortableColumns = $('#paginatedGrid th.sort'),

		buildSelectedItems = function () {
		    var data = {};
		    $('#paginatedGrid input[type="checkbox"]:checked:not(.checkAll)').each(function (i) {
		        data['items[' + i + ']'] = $(this).val();
		    });
		    return data;
		},

		resetPage = function () {
		    currentPage = 0;
		    showPage();
		},

		showPage = function () {
		    $('#paginatedGrid .checkAll').removeAttr('checked');
		    $('#paginatedGrid tbody').html('<tr class="noHover"><td colspan="5"><img src="../../Resource/Content/Images/Icons/loading-blue.gif" alt="Loading" /></td></tr>');

		    var data = { page: currentPage,
		        pageSize: $('#paginatedGridPagination .pageSize').val()
		    };



		    if ($('#paginatedGridExtraData').length) {
		        $('#paginatedGridExtraData input.Data').each(function () {
		            var t = $(this);
		            data[t.attr('id').replace(/Data$/, '')] = t.val();
		        });
		    }

		    $('#paginatedGridFilters .Filter').each(function () {
		        var t = $(this), val = t.val(), key = t.attr('id').replace(/(Select|Input)?Filter$/, '');
		        if (t.attr('multiple')) {
		            val = t.multiselect('getChecked').map(function () {
		                return this.value;
		            }).get();
		            var i = 0;
		            for (i; i < val.length; i++) {
		                data[key + '[' + i + ']'] = val[i];
		            }
		        } else {
		            data[key] = val;
		        }
		    });

		    if (sortableColumns.length) {
		        var currentSort = sortableColumns.filter('.currentSort');
		        data.orderBy = currentSort.attr('id');
		        data.orderByDirection = currentSort.attr('class').split(' ')[2];
		    }

		    $.get('/Orders/PendingConfirm/GetOrdersPending', data, function (response) {
		        if (response.result === undefined || response.result) {
		            $('#paginatedGrid tbody').html(response.page || '<tr><td colspan="7">There are no records that match that criteria.  Please try again.</td></tr>').find('tr:even').addClass('Alt');
		            $('#rowCountGrid').text(' ' + response.totalCount + ' ');
		            maxPage = !response.totalPages ? 0 : response.totalPages - 1;
		            $('#paginatedGridPagination a').removeClass('disabled');
		            if (currentPage == maxPage)
		                $('#paginatedGridPagination a.nextPage').addClass('disabled');
		            if (currentPage == 0)
		                $('#paginatedGridPagination a.previousPage').addClass('disabled');

		            $('#paginatedGridPagination span.pages').text(String.format('{0} of {1}', !response.totalPages ? 0 : currentPage + 1, response.totalPages || 0));		            
		        } else {
		            showMessage(response.message, true);
		        }
		    });
		},

		handleResponse = function (response) {
		    if (response.result) {
		        showPage();
		        var button = $(".BigBlue");
		        if (button.length > 0)
		            hideLoading(button);
		    } else {
		        showMessage(response.message, true);
		    }
		},

		changeActiveStatus = function (active) {
		    var data = buildSelectedItems();
		    data.active = active;
		    $.post('', data, handleResponse);
		};

             $('#paginatedGridFilters select[multiple]').multiselect({
                 selectedText: '# selected',
                 noneSelectedText: 'Select options',
                 checkAllText: 'Check all',
                 uncheckAllText: 'Uncheck all'
             });
             $('#paginatedGridFilters .AutoPost').bind('multiselectclick', resetPage).bind('multiselectcheckall', resetPage).bind('multiselectuncheckall', resetPage);
             $('#paginatedGridFilters select.Filter').change(resetPage);

             $('#paginatedGridFilters input.Filter.DatePicker').datepicker({ changeMonth: true, changeYear: true, minDate: new Date(1900, 0, 1), yearRange: '-100:+100', onSelect: resetPage });

             $('#paginatedGridFilters input.TextInput').keyup(function (e) {
                 if (e.keyCode == 13)
                     $('#paginatedGridFilters a.filterButton').click();
             });

             $('#paginatedGridFilters a.filterButton').click(resetPage);

             $('#paginatedGridOptions a.clearFiltersButton').click(function () { window.location.reload(); });

             $('#paginatedGridOptions a.deleteButton').click(function () {
                 if (confirm('Are you sure you want to delete the selected items?')) {
                     var button = $(".BigBlue");
                     if (button.length > 0)
                         showLoading(button);
                     $.post('', buildSelectedItems(), handleResponse);
                 }
             });
             $('#paginatedGridOptions a.deactivateButton').click(function () { changeActiveStatus(false); });
             $('#paginatedGridOptions a.activateButton').click(function () { changeActiveStatus(true); });


             $('#paginatedGridOptions a#btnShip').click(function () {
                 if ($('#paginatedGrid input[type="checkbox"]:checked:not(.checkAll)').length > 0) {
                     var data = buildSelectedItems();
                     window.location = '?' + $.param(data);
                 }
             });

             if (sortableColumns.length) {
                 if (!sortableColumns.filter('.currentSort').length) {
                     $(sortableColumns.get(0)).addClass('currentSort Ascending');
                 }
                 sortableColumns.click(function () {
                     var t = $(this), s = t.find('span.IconLink');
                     if (t.hasClass('currentSort') > 0) {
                         t.toggleClass('Ascending').toggleClass('Descending');
                         s.toggleClass('SortDescend');
                     } else {
                         $('#paginatedGrid th.currentSort').attr('class', 'sort');
                         t.addClass('currentSort Ascending');
                     }
                     showPage();
                 });
             }

             $('#paginatedGrid input.checkAll').click(function () {
                 $('#paginatedGrid input[type="checkbox"]').not(':disabled').attr('checked', $(this).is(':checked'));
             });

             $('#paginatedGrid').delegate('tr:not(.noHover)', 'hover', function () {
                 $(this).toggleClass('hover');
             });

             if (/^\s*true\s*$/i.test('True')) {
                 var index = parseInt('0');
                 $('#paginatedGrid').delegate('tr', 'click', function (e) {
                     //We normally use a tags with spans in them, so I have to check for both, otherwise this will fire on decorated buttons, as well as inputs for checkboxes - DES
                     if (e.target.nodeName.toLowerCase() != 'a' && e.target.nodeName.toLowerCase() != 'span' && e.target.nodeName.toLowerCase() != 'input') {
                         var a = $('a', this);
                         if (a.length >= index + 1) {
                             var tag = $(a.get(index)), href = tag.attr('href');
                             if (href && href == '#' || href == 'javascript:void(0)' || href == 'javascript:void(0);')
                                 tag.triggerHandler('click');
                             else {
                                 if ((tag.attr('target') && tag.attr('target') == '_blank') || (tag.attr('rel') && tag.attr('rel') == 'external')) {
                                     window.open(href);
                                 } else {
                                     window.location = href;
                                 }
                             }
                         }
                     }
                 });
             }

             $('#paginatedGridPagination a.nextPage').click(function () {
                 if (currentPage < maxPage) {
                     ++currentPage;
                     showPage();
                 }
             });
             $('#paginatedGridPagination a.previousPage').click(function () {
                 if (currentPage > 0) {
                     --currentPage;
                     showPage();
                 }
             });

             if (!$('#paginatedGridPagination select.pageSize option:selected').length) {
                 $('#paginatedGridPagination select.pageSize option:first').attr('selected', 'selected');
             }

             $('#paginatedGridPagination select.pageSize').change(resetPage);

             $('#paginatedGridRefresh').click(showPage);

             showPage();

             $('#paginatedGridOptions a:gt(0)').before('<span class="pipe">&nbsp;</span>');
         });
	//]]>
</script>
</asp:Content>

<asp:Content ID="Content6" ContentPlaceHolderID="RightContent" runat="server">
</asp:Content>
