<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Commissions/Views/Shared/Commissions.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="SectionHeader">
        <h2>
            <%= Html.Term("SponsorRules", "Create Sponsorship Rules") %></h2>
    </div>
    <div id="PromotionForm" class="splitCol mb10">
        <h3 class="UI-lightBg pad10">
            <%= Html.Term("Sponsorship_OptionsHeading", "General Rules for Sponsorship:")%>
            <span class="clr"></span>
        </h3>
        <div id="PromoOptions">        
            <% Html.RenderPartial("SponsorPlugins/OptionAccountStatus"); %>
            <% Html.RenderPartial("SponsorPlugins/OptionValidDocuments"); %>
        </div>
        <% Html.RenderPartial("SponsorPlugins/OptionRestrictPerTitles"); %>
    </div>
    <div class="mt10" id="SaveReawards">
        <a class="Button BigBlue" id="btnSave" href="javascript:void(0);">
            <%= Html.Term("Save", "Save")%></a>
    </div>
    <% Html.RenderPartial("MessageCenter"); %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    
    <% Html.RenderPartial("_EditScript"); %>
    <script type="text/javascript">
        function getProductInfo(productId, showLoading, success) {
            var options = {
                url: '<%= ResolveUrl("~/Products/ProductPromotions/QuickAddProduct") %>',
                showLoading: showLoading,
                data: { productId: productId },
                success: success
            };
            NS.post(options);
        }
        
        jQuery.fn.refreshTable = function () {
            $(this).find('tr:odd').addClass('Alt');
            $(this).find('tr:even').removeClass('Alt');
            return $(this);
        }

        function wireupOptionsPanel() {
            $('div.hasPanel').each(function () {
                var trig = $(this).attr('rel'),
							yPan = $('div.hiddenPanel', this).attr('id');
                $('input[type=radio]', this).change(function () {
                    var YN = $($(this)).val(),
								rWrap = $(this).parents('span'),
								radioClass = $(this).attr('class');

                    if (trig == 'isYes' && (!radioClass || radioClass.indexOf('disablePanel') == -1)) {

                        // "yes" triggers the panel
                        if (YN == 'yes') {
                            $('#' + yPan).slideDown('fast');
                            $(rWrap).addClass('UI-lightBg');
                        } else if (YN == 'no') {
                            $('#' + yPan).slideUp('fast');
                            $('div.hasPanel span').removeClass('UI-lightBg');
                        }
                    } else if (trig == 'isNo' && (!radioClass || radioClass.indexOf('disablePanel') == -1)) {

                        // "no" triggers the panel
                        if (YN == 'no') {
                            $('#' + yPan).slideDown('fast');
                            $(rWrap).addClass('UI-lightBg');
                        } else {
                            $('#' + yPan).slideUp('fast');
                            $('div.hasPanel span').removeClass('UI-lightBg');
                        }
                    }
                });
            });
        }

        $(document).ready(function () {
            wireupOptionsPanel();

            $('.qty, .numeric').numeric();

            //Added ability to click help icon and view information about the Adjustment types.
            $('#AdjustemntDescHelp').click(function () {
                $('#sType option:selected').each(function () {
                    var desc = $(this).attr('rel');
                    if (desc !== undefined) {
                        $('div#' + desc).fadeIn();
                    } else {
                        $('#adjAll').fadeIn();
                    }
                });
            });

            $('.hideDesc').click(function () {
                $(this).closest('div.desc').fadeOut('fast');
            });
        });
    </script>
    <script type="text/javascript">
        $(function () {
            
            $('#OrderTypes a.checkAllOptions').toggle(function () {
                $(this).text('<%=Html.Term("SponsorOptions_UncheckAllLink", "uncheck all")%>');
                $(this).closest('div').find(':checkbox').attr('checked', 'checked');
            }, function () {
                $(this).text('<%=Html.Term("SponsorOptions_CheckAllLink", "check all")%>');
                $(this).closest('div').find(':checkbox').removeAttr('checked');
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="LeftNav" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="BreadCrumbContent" runat="server">
</asp:Content>
