<%@ Page EnableSessionState="false" Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="DistributorBackOffice.WebForms.ReportsDisplay" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%--<asp:Label runat="server" Text="" id="lblReportName"></asp:Label>--%>
    </title>
    <link href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"></script>
    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var imgToHide = $('img[src^="/Reserved.ReportViewerWebControl.axd?"]');
            imgToHide.hide();
        });
    </script>
    <style type="text/css">
        .reportPanelStyle
        {
            height: 95%;
            width: 100%;
            overflow: hidden;
        }
    </style>
</head>
<body>
    <style type="text/css">
        html, body, form
        {
            margin: 0;
            padding: 0;
            height: 99%;
            overflow: hidden;
            font-family: Verdana, Tahoma, Arial;
            font-size: small;
        }
    </style>
    <form id="form1" runat="server" style="width: 100%; height: 100%;">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Panel ID="Panel1" runat="server" Style="overflow: hidden;" ScrollBars="Both"
        CssClass="reportPanelStyle">
        <rsweb:ReportViewer ID="rptViewer" AsyncRendering="true" InteractiveDeviceInfos="(Collection)"
            WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" ShowPrintButton="true"
            Style="display: table !important; margin: 0px; overflow: auto !important;" Visible="true"
            ProcessingMode="Remote" Height="100%" runat="server" Width="100%">
        </rsweb:ReportViewer>
    </asp:Panel>
    </form>
</body>
</html>
