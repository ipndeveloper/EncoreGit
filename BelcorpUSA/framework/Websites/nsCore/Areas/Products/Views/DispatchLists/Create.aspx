<%@ Page Title="" Language="C#" MasterPageFile="~/Areas/Products/Views/Dispatch/Dispatch.Master" 
Inherits="System.Web.Mvc.ViewPage<dynamic>" %> 
<asp:Content ID="Content0" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="Stylesheet" type="text/css" href="<%= ResolveUrl("~/Resource/Content/CSS/timepickr.css") %>" />
    <script type="text/javascript" src="<%= ResolveUrl("~/Resource/Scripts/timepickr.js") %>"></script>
    <script type="text/javascript">

        $(function () {
            // --- Declaracion de variables
            var accountDispatchsLists = new Array();

            function onSuccess(result) {
                if (result.result) {
                    showMessage('<%= Html.Term("SaveRestriction", "Restriction Successfully Saved!") %>');
                    window.location = '<%= ResolveUrl("~/Products/DispatchLists/Save") %>/' + result.id;
                }
                else {
                    showMessage(result.message, true);
                }
            }

            $('#btnSave').click(function () {

                if ($("#nameDispatchList").val() == "") {
                    showMessage("Name List Dispatch is Empty", true);
                    return false;
                }

                // -------

                $("#conditionSingleGrid tbody tr").each(function (index) {
                    var campo1, campo2, campo3;
                    $(this).children("td").each(function (index2) {
                        switch (index2) {
                            case 0:
                                campo1 = 0;
                                break;
                            case 1:
                                campo2 = $(this).text();
                                break;
                            case 2:
                                campo3 = $(this).text();
                                break;
                        }
                    })
                    accountDispatchsLists.push({ AccountID: campo2, Name: campo3 });

                })
                // -------

                var data = {
                    "DispatchListID": 0
                            , "Editable": 1
                            , "Name": $("#nameDispatchList").val()
                            , "MarketID": 56
                            , "Accounts": accountDispatchsLists
                };

                var url = '<%= ResolveUrl("~/Products/DispatchLists/Save") %>';

                $.ajax({
                    url: url,
                    data: JSON.stringify(data),
                    type: 'POST',
                    contentType: 'application/json;',
                    dataType: 'json',
                    success: function (result) {
                        if (result.result == true) {
                            $('#btnSave').hide();
                            showMessage("Dispatch Lists Save Sucefully", false);
                            location.href = '<%= ResolveUrl("~/Products/DispatchLists") %>';
                        }
                        else {
                            showMessage("Dispatch List !! Error !! Save", true);
                        }
                    }
                });
            });

            // Look-up account ini
            $('#txtCustomerSuggest').jsonSuggest('<%= ResolveUrl("~/Accounts/Search") %>', { onSelect: function (item) {
                $('#accountId').val(item.id);
                $('#txtCustomerSuggest').clearError();
            }, minCharacters: 1, source: $('#txtCustomerSuggest'), ajaxResults: true, maxResults: 50, showMore: true
            });
            $('form').submit(function () {
                if (!$('#accountId').val())
                    return false;
            });

            $('#conditionSingleAccountAdd').click(function () {
                var accountId = $('#accountId').val();
                var strAccount = $('#txtCustomerSuggest').val();
                var londAccount = strAccount.indexOf("(");
                var nomAccount = strAccount.substr(0, londAccount - 1);
                if (accountId) {
                    $('#conditionSingleGrid tbody').prepend('<tr>'
						            + '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                                    + '<input type="hidden" id="accountId" class="accountId required" name="Account is required" value="' + accountId + '" /></td>'
						            + '<td>' + accountId + '</td>'
						            + '<td>' + nomAccount + '</td>'
						            + '</tr>');
                    $('#txtCustomerSuggest, #accountId').val('');
                    $('#conditionSingleGrid').show();
                }
            });


            $('#conditionSingleGrid .BtnDelete').live('click', function () {
                $(this).closest('tr').remove();
                $('#singleItemQuickAdd').show();
            });

            $('#btnCancel').click(function () {
                window.location.replace('<%= ResolveUrl("~/Products/DispatchLists") %>');
            });


        });
         
          
        // Fin Subida Imagen

        // Inicio Subida Matriz
        
    </script>

</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="BreadCrumbContent" runat="server">
    <a href="<%= ResolveUrl("~/Accounts") %>">
        <%= Html.Term("Accounts") %></a> > <a href="<%= ResolveUrl("~/Products/DispatchLists") %>">
            <%= Html.Term("BrowseDispatchLists", "Browse Dispatchs Lists")%></a> >
    <%= Html.Term("CreateaNewDispatchList", "Create a New Dispatch List")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
  <div class="SectionHeader">
    <h2><%= Html.Term("NewDispatchList", "New List Dispatch")%>
    </h2>
  </div>
   
<table class="FormTable" width="100%">
    <tr> 
       <td>
         <%= Html.Term("NameDispatchList", "Name List Dispatch")%>:
       </td>
       <td>
        <input type="text" id="nameDispatchList" style="width: 300px;" />
       </td>
       <td>
        <a class="Button BigBlue" id="btnDownloadTemplate" href="javascript:void(0);">
                <%= Html.Term("DownloadTemplate", "Download Template")%>
            </a>
       </td>
    </tr>
    <tr>
       <td>   
       <%= Html.Term("AccountSearch", "Account Search")%>:<span class="LawyerText"/>
       <br/>
       (<%= Html.Term("EnterAtLeast3Characters", "enter at least 3 characters")%>) 
       </td>
       <td>
            <input type="text" id="txtCustomerSuggest" style="width: 300px;" />
            <input type="hidden" name="accountId" id="accountId" />
            <a class="DTL Add" href="javascript:void(0);" id="conditionSingleAccountAdd"><%= Html.Term("Promotions_QuickAdd", "Add")%></a> 
       </td>
       <td>
        <form enctype="multipart/form-data" action="" id="formLoad" method="post"> 
                    <a class="Button BigBlue" id="btnBrowse" href="javascript:void(0);">
                        <%= Html.Term("LoadCampaingMatrix", "LOAD FROM EXCEL")%>
                    </a>
                    <label id="label">
                    </label>
                    <input type="file" id="inputLoadMatrix" name="ninputLoadMatrix" accept="xlsx|xls"
                        style="display: none" />
                    <input type="submit" id="submitHidden" style="display: none" />
        </form> 
       </td>
    </tr>
    <tr>
    <td>
    </td>
    <td>
        <div>
            <table width="100%" class="DataGrid" id="conditionSingleGrid">
                    <thead>
                        <tr class="GridColHead">
                            <th class="GridCheckBox">
                            </th>
                            <th>
                                <%=Html.Term("AccountNumber")%>
                            </th>
                            <th>
                                <%=Html.Term("Name")%>
                            </th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
        </div>
    </td>
    </tr>
    <tr>        
    <td>
    </td>    
            <td>
                <p>
                    <a href="javascript:void(0);" id="btnSave" style="display: inline-block;" class="Button BigBlue"">
                        <%= Html.Term("Save", "Save") %></a> 
                    <a href="javascript:void(0);" id="btnCancel" style="display: inline-block;" class="Button BigWhite">
                        <%= Html.Term("Cancel", "Cancel") %></a>
                </p>
            </td>
        </tr>
</table> 
                 
  
<div id="formPartial">
</div>
<script type="text/javascript">

    $(document).ready(function () {
        $('#inputLoadMatrix').change(function (e) {
            $('#submitHidden').trigger('click');
        });

        $('#btnBrowse').click(function () {
            $('#inputLoadMatrix').trigger('click');
        });

        $("#btnDownloadTemplate").click(function () {
            window.location = '/Products/File/DownloadTemplate';
            return false;
        });
        document.getElementById('formLoad').onsubmit = function () {
            var formdata = new FormData();
            var fileInput = document.getElementById('inputLoadMatrix');
            for (i = 0; i < fileInput.files.length; i++) {
                formdata.append(fileInput.files[i].name, fileInput.files[i]);
            }
            var xhr = new XMLHttpRequest();
            xhr.open('POST', '/Products/File/FileAccounts');
            xhr.send(formdata);
            $('#btnBrowse').showLoading();
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4 && xhr.status == 200) {
                    var response = JSON.parse(xhr.responseText);
                    if (response.result) {
                        for (var i = 0; i < response.accounts.length; i++) {
                            $('#conditionSingleGrid tbody').prepend('<tr>'
                            + '<td><a class="BtnDelete" href="javascript:void(0);"><span class="Delete icon-x"></span></a>'
                            + '<input type="hidden" id="AccountNumber" class="productId required" name="Product is required" value="' + response.accounts[i].AccountNumber + '" /></td>'
						    + '<td>' + response.accounts[i].AccountNumber + '</td>'
						    + '<td>' + response.accounts[i].Name + '</td>'
						    + '</tr>');
                        }


                    }
                }
                $('#btnBrowse').hideLoading();
            }
            return false;
        }
    });
</script>
</asp:Content>
